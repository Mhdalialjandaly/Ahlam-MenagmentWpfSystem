using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EyeClinic.DataAccess.Repositories
{
    public class PatientVisitPrescriptionRepository :
        BaseRepository<Model.PatientVisitPrescriptionDto, PatientVisitPrescription>, IPatientVisitPrescriptionRepository
    {
        private readonly IMapper _mapper;
        private readonly EyeClinicContext _context;

        public PatientVisitPrescriptionRepository(IMapper mapper, EyeClinicContext context)
            : base(mapper, context) {
            _mapper = mapper;
            _context = context;
        }

        public async Task AddOrUpdate(List<Model.PatientVisitPrescriptionDto> patientVisitPrescription) {
            var transaction = await _context.Database.BeginTransactionAsync();
            var oldPrescription = _context.PatientVisitPrescriptions
                .Where(e => e.PatientVisitId
                                     == patientVisitPrescription.FirstOrDefault().PatientVisitId);
            _context.PatientVisitPrescriptions.RemoveRange(oldPrescription);

            await AddRange(patientVisitPrescription);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        public override async Task<List<Model.PatientVisitPrescriptionDto>> GetByKey(Expression<Func<PatientVisitPrescription, bool>> predicate) {
            var result = await _context.PatientVisitPrescriptions
                .AsNoTracking()
                .Include(e => e.PatientVisit)
                .Include(e => e.Medicine)
                .Include(e => e.MedicineUsage)
                .Include(e => e.Medicine)
                .ThenInclude(e => e.MedicineType)
                .Where(e => e.DeletedDate == null)
                .Where(predicate)
                .ToListAsync();

            return _mapper.Map<List<Model.PatientVisitPrescriptionDto>>(result);
        }
    }
}
