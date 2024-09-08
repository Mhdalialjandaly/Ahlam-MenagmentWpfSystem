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
    public class ReadyProductRepository : BaseRepository<Model.ReadyProductDbo, ReadyProduct>, IReadyProductRepository
    {
        private readonly IMapper _mapper;
        private readonly EyeClinicContext _context;

        public ReadyProductRepository(IMapper mapper, EyeClinicContext context) : base(mapper, context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<List<ReadyProductDbo>> GetReadyProductAsyncById(TestDto testDto)
        {
            var cartoonLabels = _mapper.Map<List<ReadyProductDbo>>(await _context.ReadyProducts
               .Where(e => e.DeletedDate == null && e.ProductId == testDto.Id )
               .OrderBy(e=>e.Id)
               .ToListAsync());


            return cartoonLabels;
        }
        public async Task<List<ReadyProduct>> GetReadyProductAsyncById2(TestDto testDto)
        {
            var cartoonLabels = _mapper.Map<List<ReadyProduct>>(await _context.ReadyProducts
               .Where(e => e.DeletedDate == null && e.ProductId == testDto.Id)
               .ToListAsync());


            return cartoonLabels;
        }
        public async Task<List<Model.ReadyProductDbo>> GetReadyProductAsyncByName(TestDto testDto,string searchTest)
        {
            var cartoonLabels = _mapper.Map<List<Model.ReadyProductDbo>>(await _context.ReadyProducts
               .Where(e => e.DeletedDate == null && e.ProductId == testDto.Id && e.ArName.ToLower().StartsWith(searchTest.ToLower()))
               .ToListAsync());


            return cartoonLabels;
        }
        public async Task<List<Model.ReadyProductDbo>> GetReadyProductWareHouse()
        {
            var cartoonLabels = _mapper.Map<List<Model.ReadyProductDbo>>(await _context.ReadyProducts
               .Where(e => e.DeletedDate == null && e.IsIncreaseDogma == false)
               .ToListAsync());


            return cartoonLabels;
        }
        public async Task<List<Model.ReadyProductDbo>> GetReadyProductWareHouseByName(string Name)
        {
            var cartoonLabels = _mapper.Map<List<Model.ReadyProductDbo>>(await _context.ReadyProducts
               .Where(e => e.DeletedDate == null && e.IsIncreaseDogma == false && e.ArName.ToLower().StartsWith(Name))
               .ToListAsync());


            return cartoonLabels;
        }

        public async Task<double> GetSumReadyProducts(int id)
        {
            var MaxEntry = await _context.ReadyProducts
                .AsNoTracking()
                .Where(e => e.DeletedDate == null && e.ProductId == id)
                .ToListAsync();
            var SumEntry = MaxEntry.Sum(e => e.TotalValue);

            var Result = await _context.Tests
                .AsNoTracking()
                .Where(e => e.DeletedDate == null && e.Id == id)
                .ToListAsync();
            var Res = Result.Sum(e => e.UnitValue);

            return Res - SumEntry  ;
        }
        public async Task<double> GetSumWasteValueProducts(int id)
        {
            var MaxEntry = await _context.ReadyProducts
                .AsNoTracking()
                .Where(e => e.DeletedDate == null && e.ProductId == id)
                .ToListAsync();
            var SumEntry = MaxEntry.Sum(e => e.WasteValue);

            return  SumEntry ;
        }
        public async Task<double> GetSumWightValueProducts(int id)
        {
            var MaxEntry = await _context.ReadyProducts
                .AsNoTracking()
                .Where(e => e.DeletedDate == null && e.ProductId == id)
                .ToListAsync();
            var SumEntry = MaxEntry.Sum(e => e.ExportedValue);


            return SumEntry;
        }
        //public async Task<double> GetSumValueProducts(Model.TestDto test)
        //{         
        //    var MaxEtry = await _context.ReadyProducts
        //      .AsNoTracking()
        //      .Where(e => e.DeletedDate == null && e.ProductId == test.Id )
        //      .ToListAsync();

           
        //    var SumtotalWaste = MaxEtry.Sum(e=>e.WasteValue);
        //    var SumtotalEntry =MaxEtry.Sum(e=>e.IsIncreaseDogmaValue);
        //    var SumtotalExported = MaxEtry.Sum(e=>e.ExportedValue);
        //    var SumtotalWightValue = MaxEtry.Sum(e => e.TotalValue);
            
        //    var SumtotalResult = (test.TotalValue + SumtotalEntry);
        //    //double x =  (double)Math.Round(SumEntry, MidpointRounding.ToPositiveInfinity);
        //    return SumtotalResult ;
        //}

        public async Task<double> GetCountValueProducts(int id)
        {
            var MaxEtry = await _context.ReadyProducts
              .AsNoTracking()
              .Where(e => e.DeletedDate == null && e.ProductId == id)
              .ToListAsync();

            var SumEtry = MaxEtry.Count();

            return SumEtry;
        }
    }
}
