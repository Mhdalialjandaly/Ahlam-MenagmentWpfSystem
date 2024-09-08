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
    public class PatientVisitGlassRepository : BaseRepository<Model.PatientVisitGlassDto, PatientVisitGlass>, IPatientVisitGlassRepository
    {
        private readonly IMapper _mapper;
        private readonly EyeClinicContext _context;

        public PatientVisitGlassRepository(IMapper mapper, EyeClinicContext context)
            : base(mapper, context) {
            _mapper = mapper;
            _context = context;
        }


        public async Task<Model.PatientVisitGlassDto> AddOrUpdate(Model.PatientVisitGlassDto patientVisitGlass) {
            var exist = _context.PatientVisitGlasses
                .AsNoTracking()
                .FirstOrDefault(e => e.PatientVisitId == patientVisitGlass.PatientVisitId);

            if (exist == null) {
                return await Add(patientVisitGlass);
            }

            patientVisitGlass.Id = exist.Id;
            patientVisitGlass.CreatedDate = exist.CreatedDate;
            patientVisitGlass.LastModifiedDate = DateTime.Now;
            await Update(patientVisitGlass);
            return patientVisitGlass;
        }

        public async Task<Model.PatientVisitGlassDto> GetLastGlassInfo(int patientId) {
            var dbItem = await _context.PatientVisitGlasses.AsNoTracking()
                .Include(e => e.PatientVisit)
                .Where(e => e.PatientVisit.PatientId == patientId)
                .OrderByDescending(e => e.PatientVisit.VisitDate)
                .FirstOrDefaultAsync();

            return _mapper.Map<Model.PatientVisitGlassDto>(dbItem);
        }

        public async Task<DateTime?> GetVisitDateById(int patientVisitId) {
            var item = (await _context
                    .PatientVisits
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Id == patientVisitId))
                ?.VisitDate;

            return item;
        }

        public async Task<List<PatientVisitGlassDto>> GetByDate(DateTime fromDateDate, DateTime toDateDate) {
            var items = await _context
                .PatientVisitGlasses
                .AsNoTracking()
                .Include(e => e.PatientVisit)
                .Where(e => e.DeletedDate == null)
                .Include(e => e.PatientVisit.Patient)
                .Where(e => e.PatientVisit.VisitDate.Date >= fromDateDate.Date &&
                            e.PatientVisit.VisitDate.Date <= toDateDate.Date)
                .ToListAsync();

            return _mapper.Map<List<PatientVisitGlassDto>>(items);
        }
    }
}
