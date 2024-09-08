using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeClinic.DataAccess.Repositories
{
    public class marketsExtryRepository : BaseRepository<Model.MarketsExtryDto, MarketsExtry>, IMarketsExtryRepository
    {
        private readonly IMapper _mapper;
        private readonly EyeClinicContext _context;
        public marketsExtryRepository(IMapper mapper, EyeClinicContext context) : base(mapper, context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<MarketsExtryDto>> GetMarketsExtriesById(MarketsProductsDto marketsProducts)
        {
            var MarketExtry = _mapper.Map<List<MarketsExtryDto>>(await _context.MarketsExtries
             .Where(e => e.DeletedDate == null)
             .Where(e => e.MarketsProductsId == marketsProducts.Id)
             .ToListAsync());


            return MarketExtry;
        }
    }
}
