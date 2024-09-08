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
    public class PatientVisitDiagnosisRepository : BaseRepository<Model.PatientVisitDiagnosis, PatientVisitDiagnosis>, IPatientVisitDiagnosisRepository
    {
        private readonly IMapper _mapper;
        private readonly EyeClinicContext _context;

        public PatientVisitDiagnosisRepository(IMapper mapper, EyeClinicContext context)
            : base(mapper, context) {
            _mapper = mapper;
            _context = context;
        }

        public override async Task<List<Model.PatientVisitDiagnosis>> GetByKey(Expression<Func<PatientVisitDiagnosis, bool>> predicate) {
            return _mapper.Map<List<Model.PatientVisitDiagnosis>>(
                await _context.PatientVisitDiagnoses
                    .Include(e => e.Diagnosis)
                    .Where(e => e.DeletedDate == null)
                    .Where(predicate)
                    .ToListAsync());
        }
    }
}
