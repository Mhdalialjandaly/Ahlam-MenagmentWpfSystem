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
    public class PatientVisitLabTestRepository : BaseRepository<Model.PatientVisitLabTestDto, PatientVisitLabTest>, IPatientVisitLabTestRepository
    {
        private readonly IMapper _mapper;
        private readonly EyeClinicContext _context;

        public PatientVisitLabTestRepository(IMapper mapper, EyeClinicContext context)
            : base(mapper, context) {
            _mapper = mapper;
            _context = context;
        }

        public override async Task<List<Model.PatientVisitLabTestDto>> GetByKey(Expression<Func<PatientVisitLabTest, bool>> predicate) {
            var result = await _context.PatientVisitLabTests
                .Include(e => e.PatientVisit)
                .Include(e => e.LabTest)
                .AsNoTracking()
                .Where(e => e.DeletedDate == null && e.PatientVisit.DeletedDate == null)
                .Where(predicate)
                .ToListAsync();

            return _mapper.Map<List<Model.PatientVisitLabTestDto>>(result);
        }
    }
}
