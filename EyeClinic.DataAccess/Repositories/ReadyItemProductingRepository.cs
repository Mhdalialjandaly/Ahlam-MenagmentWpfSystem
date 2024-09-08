using AutoMapper;
using EyeClinic.Core.Interface;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EyeClinic.DataAccess.Repositories
{
     public class ReadyItemProductingRepository : BaseRepository<Model.ReadyItemProductingDto, ReadyItemProducting>, IReadyItemProductingRepository
    {
        private readonly IMapper _mapper;
        private readonly EyeClinicContext _context;

        public ReadyItemProductingRepository(IMapper mapper, EyeClinicContext context) : base(mapper, context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<List<Model.ReadyItemProductingDto>> GetProductingItemAsyncById(int Id)
        {
            var cartoonLabels = _mapper.Map<List<Model.ReadyItemProductingDto>>(await _context.Producting
               .Where(e => e.DeletedDate == null)
               .ToListAsync());


            return cartoonLabels;
        }
        public async Task<double> GetSumProductingItems(int id)
        {
            var MaxEntry = await _context.CartoonLabels
                .AsNoTracking()
                .Where(e => e.DeletedDate == null && e.CartoonNameId == id)
                .ToListAsync();
            var SumEntry = MaxEntry.Sum(e => e.DEntry);

            var MaxExtry = await _context.CartoonLabels
               .AsNoTracking()
               .Where(e => e.DeletedDate == null)
               .ToListAsync();
            var SumExtry = MaxEntry.Sum(e => e.DExtry);

            return SumEntry - SumExtry;

        }
    }
}
