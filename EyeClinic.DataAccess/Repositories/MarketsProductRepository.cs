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
    public class MarketsProductRepository : BaseRepository<Model.MarketsProductsDto, MarketsProducts>, IMarketsProductsRepository
    {
        private readonly IMapper _mapper;
        private readonly EyeClinicContext _context;

        public MarketsProductRepository(IMapper mapper, EyeClinicContext context): base(mapper, context)
        {
          _context = context;
          _mapper = mapper;
        }

        public async Task UpdateMarketProductByItem(Model.MarketsProductsDto marketsProducts)
        {
           var resEntry =  _context.MarketsEntries.Where(e=>e.MarketsProductsId==marketsProducts.Id && e.DeletedDate == null).Sum(e => e.Quinttity);
           var resExtry =  _context.MarketsExtries.Where(e=>e.MarketsProductsId==marketsProducts.Id && e.DeletedDate == null).Sum(e => e.Quinttity);
            var resSum = _mapper.Map<MarketsProducts>(marketsProducts);
            resSum.Entry=resEntry;
            resSum.Extry=resExtry;
            resSum.FirstTermValue=marketsProducts.FirstTermValue;
            resSum.RealValue = resEntry + marketsProducts.FirstTermValue - resExtry ;
            resSum.Status = marketsProducts.Status;            
            resSum.LastModifiedDate = DateTime.Now;
            if (resSum.TheMinimumValue > resSum.RealValue)
            {
                resSum.Status = "Visible";
            }
            else
            {
                resSum.Status = "Hidden";
            }
             _context.Update(resSum);           
            
            await SaveChangesAsync();            
        }
    }
}
