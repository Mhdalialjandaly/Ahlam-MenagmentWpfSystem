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
    public class PatientVisitEyeTestRepository :
        BaseRepository<Model.PatientVisitEyeTestDto, PatientVisitEyeTest>, IPatientVisitEyeTestRepository
    {
        private readonly IMapper _mapper;
        private readonly EyeClinicContext _context;

        public PatientVisitEyeTestRepository(IMapper mapper, EyeClinicContext context)
            : base(mapper, context) {
            _mapper = mapper;
            _context = context;
        }

        public override async Task<List<Model.PatientVisitEyeTestDto>> GetByKey(Expression<Func<PatientVisitEyeTest, bool>> predicate) {
            var result = await _context.PatientVisitEyeTests
                .Include(e => e.EyeTest)
                .AsNoTracking()
                .Where(e => e.DeletedDate == null)
                .Where(predicate)
                .ToListAsync();

            return _mapper.Map<List<Model.PatientVisitEyeTestDto>>(result);
        }
    }
}
