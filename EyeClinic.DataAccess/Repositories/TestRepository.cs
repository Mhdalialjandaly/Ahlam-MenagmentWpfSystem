using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EyeClinic.DataAccess.Repositories
{
    public class TestRepository : BaseRepository<Model.TestDto, Test>, ITestsRepository
    {
        private readonly IMapper _mapper;
        private readonly EyeClinicContext _context;

        public TestRepository(IMapper mapper, EyeClinicContext context)
            : base(mapper, context) {
            _mapper = mapper;
            _context = context;
        }

        public async Task DropAll(List<Model.PatientVisitsTestDto> patientVisitTests) {
            var ids = patientVisitTests.Select(e => e.Id).ToList();

            var entities = await _context.PatientVisitsTests
                .Where(e => ids.Contains(e.Id))
                .ToListAsync();

            foreach (var entity in entities) {
                entity.Dropped = true;
            }

            await _context.SaveChangesAsync();
        }
        public async Task<List<Model.TestDto>> GetByCode(string code)
        {
            var items = await _context.Tests
                   .Where(e => e.Code==code)
                   .AsNoTracking()
                   .OrderBy(e => e.CreatedDate)
                   .ToListAsync();

            return _mapper.Map<List<Model.TestDto>>(items);
        }

        public async Task<List<Model.TestDto>> GetByIsProduct(bool isproduct)
        {
            var items = await _context.Tests
                   .Where(e => e.IsProduct == isproduct && e.DeletedDate == null)
                   .AsNoTracking()
                   .OrderBy(e => e.Code)
                   .ToListAsync();

            return _mapper.Map<List<Model.TestDto>>(items);
        }
        public async Task<List<Model.TestDto>> GetByIsProductWithName(bool isproduct, string arname)
        {
            var items = await _context.Tests
                   .Where(e => e.IsProduct == isproduct && 
                   e.DeletedDate == null &&                    
                   e.ArName.ToLower().Contains(arname.ToLower()))
                   .AsNoTracking()
                   .OrderBy(e => e.Code)
                   .ToListAsync();

            return _mapper.Map<List<Model.TestDto>>(items);
        }
    }
}
