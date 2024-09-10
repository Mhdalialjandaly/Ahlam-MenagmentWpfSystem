using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EyeClinic.DataAccess.Repositories
{
    public class WareHouseReadyRepository : BaseRepository<Model.WareHouseReadyMaterialDto, WareHouseReadyMaterial>, IWareHouseReadyMaterialRepository
    {
        private readonly IMapper _mapper;
        private readonly EyeClinicContext _context;

        public WareHouseReadyRepository(IMapper mapper, EyeClinicContext context)
            : base(mapper, context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<Model.WareHouseReadyMaterialDto> GetByTestId(TestDto testDto)
        {
            var material = await _context.WareHouseReadyMaterials.AsNoTracking()
               .Include(e => e.Test)               
               .FirstOrDefaultAsync(e => e.TestId == (int)testDto.Id);

            return _mapper.Map<Model.WareHouseReadyMaterialDto>(material);
        }
    }
}
