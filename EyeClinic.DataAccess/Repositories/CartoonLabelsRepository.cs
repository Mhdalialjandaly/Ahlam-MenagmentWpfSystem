using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EyeClinic.DataAccess.Repositories
{
    public class CartoonLabelsRepository : BaseRepository<Model.CartoonLabelsDto, CartoonLabels>, ICartoonLabelsRepository
    {
        private readonly IMapper _mapper;
        private readonly EyeClinicContext _context;

        public CartoonLabelsRepository(IMapper mapper, EyeClinicContext context) : base(mapper, context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<List<Model.CartoonLabelsDto>> GetCartoonItemAsyncById(int Id)
        {
            var cartoonLabels = _mapper.Map<List<Model.CartoonLabelsDto>>(await _context.CartoonLabels
               .Where(e => e.DeletedDate == null)
               .Where(e => e.CartoonNameId == Id)
               .ToListAsync());


            return cartoonLabels;
        }
        public async Task<double> GetSumCartoonLabels(int id)
        {
            var MaxEntry = await _context.CartoonLabels
                .AsNoTracking()
                .Where(e => e.DeletedDate == null && e.CartoonNameId == id)
                .ToListAsync();
            var SumEntry = MaxEntry.Sum(e=>e.DEntry);

            var MaxExtry = await _context.CartoonLabels
               .AsNoTracking()
               .Where(e => e.DeletedDate == null && e.CartoonNameId == id)
               .ToListAsync();
            var SumExtry = MaxExtry.Sum(e => e.DExtry);

            var Result = await _context.Diseases
                .AsNoTracking()
                .Where(e => e.DeletedDate == null && e.Id == id)
                .ToListAsync();
            var Res = Result.Sum(e=>e.FirstValue);

                return Res + SumEntry - SumExtry;           
           
        }
        public async  Task<double> GetLastValue(int id)
        {

            var list = await _context.CartoonLabels
               .Where(e => e.DeletedDate == null && e.CartoonNameId == id)
               .ToListAsync();

            
            if (list.Count == 0)
            {
                return 0;
            }
            else
            {
                var max = list.Max(e => e.CurrentValue);
                return max;
            }           
           
        }
        



    }
}
