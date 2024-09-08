using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using Microsoft.EntityFrameworkCore;

namespace EyeClinic.DataAccess.Repositories
{
    public class OldMedicineRepository : BaseRepository<Model.OldMedicineViewTableDto, OldMedicineViewTable>, IOldMedicineRepository
    {
        private readonly IMapper _mapper;
        private readonly EyeClinicContext _context;

        public OldMedicineRepository(IMapper mapper, EyeClinicContext context)
            : base(mapper, context) {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<Model.OldMedicineViewTableDto>> GetByPatientVisitId(int patientVisitId) {
            var items = await _context.OldMedicineViewTables
                .AsNoTracking()
                .Include(e => e.PatientVisit)
                .Where(e => e.DeletedDate == null)
                .Where(e => e.PatientVisit.Id == patientVisitId)
                .OrderBy(e => e.Index)
                .ToListAsync();

            return _mapper.Map<List<Model.OldMedicineViewTableDto>>(items);
        }

        public async Task<List<Model.OldMedicineViewTableDto>> GetByPatientId(int patientId) {
            var items = await _context.OldMedicineViewTables
                .AsNoTracking()
                .Include(e => e.PatientVisit)
                .Where(e => e.DeletedDate == null)
                .Where(e => e.PatientVisit.PatientId == patientId)
                .OrderByDescending(e => e.PatientVisit.VisitDate)
                .ThenBy(e => e.Index)
                .ToListAsync();

            return _mapper.Map<List<Model.OldMedicineViewTableDto>>(items);
        }

        public async Task<List<OldMedicineViewTableDto>> GetByDateAndMedicine(DateTime fromDate, DateTime toDate, string medicineName) {
            var items = await _context.OldMedicineViewTables
                .AsNoTracking()
                .Include(e => e.PatientVisit)
                .ThenInclude(e => e.Patient)
                .Where(e => e.DeletedDate == null)
                .Where(e => e.MedicineName == medicineName)
                .OrderByDescending(e => e.PatientVisit.VisitDate)
                .ThenBy(e => e.Index)
                .ToListAsync();

            return _mapper.Map<List<Model.OldMedicineViewTableDto>>(items);
        }
    }
}
