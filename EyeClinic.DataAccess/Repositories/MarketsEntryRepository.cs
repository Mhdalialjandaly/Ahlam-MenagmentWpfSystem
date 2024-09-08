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
    internal class MarketsEntryRepository : BaseRepository<Model.MarketsEntryDto, MarketsEntry>, IMarketsEntryRepository
    {
        private readonly IMapper _mapper;
        private readonly EyeClinicContext _context;
        public MarketsEntryRepository(IMapper mapper, EyeClinicContext context ,IMapper mapper1,EyeClinicContext eyeClinicContext) : base(mapper, context)
        {
            _mapper = mapper;
            _context = eyeClinicContext;
        }

        public async Task<List<MarketsEntryDto>> GetMarketsEntriesById(MarketsProductsDto marketsProducts)
        {
            var MarketEntry = _mapper.Map<List<MarketsEntryDto>>(await _context.MarketsEntries
             .Where(e => e.DeletedDate == null)
             .Where(e => e.MarketsProductsId == marketsProducts.Id)
             .ToListAsync());

            return MarketEntry;
        }
    }
}
