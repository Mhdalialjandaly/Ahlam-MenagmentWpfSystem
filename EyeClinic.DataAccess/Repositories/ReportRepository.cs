using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using Microsoft.EntityFrameworkCore;

namespace EyeClinic.DataAccess.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly EyeClinicContext _context;
        private readonly IMapper _mapper;

        public ReportRepository(EyeClinicContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<PatientDto>> GetPatientNamesByDisease(DiseaseDto disease, DateTime fromDate, DateTime toDate)
        {
            var report = await _context.PatientDiseases
                .AsNoTracking()
                 .Include(e => e.Patient)
                 .ThenInclude(e => e.PatientVisits)
                 .Where(e => e.Patient.PatientVisits.Any(c => c.VisitDate >= fromDate.Date && c.VisitDate <= toDate.Date))
                 .Where(e => e.DiseaseId == disease.Id &&
                             e.DeletedDate == null &&
                             e.Patient.DeletedDate == null)
                .Select(e => e.Patient)
                .ToListAsync();

            return _mapper.Map<List<PatientDto>>(report);
        }

        public async Task<List<PatientDto>> GetPatientNamesByOperationId(int selectedOperationId,DateTime fromDate,DateTime toDate)
        {
            var report = await _context
                .PatientOperations
                .AsNoTracking()
                .Include(e => e.Patient.PatientOperations)                
                 .Where(e => e.OperationDate>= fromDate.Date && e.OperationDate<=toDate.Date )
                .Where(e => e.LeftEyeOperationId == selectedOperationId ||
                            e.RightEyeOperationId == selectedOperationId)
                .Where(e => e.DeletedDate == null &&
                            e.Patient.DeletedDate == null)
                .Where(e=>e.DownPayment!=0)
                .Where(e=>e.LeftEyeOperation==null || e.RightEyeOperation==null)
                .Select(e => e.Patient)
                .ToListAsync(); 

            return _mapper.Map<List<PatientDto>>(report);
        }

        public async Task<List<PatientVisitDto>> GetTotalVisits(DateTime from, DateTime to)
        {
            var report = await _context.PatientVisits
                .AsNoTracking()
                .Include(e => e.Patient)
                .Where(e => e.VisitDate.Date >= from.Date &&
                            e.VisitDate.Date <= to.Date)
                .Where(e => e.DeletedDate == null &&
                            e.Patient.DeletedDate == null)
                .ToListAsync();

            return _mapper.Map<List<PatientVisitDto>>(report);
        }
    }
}
