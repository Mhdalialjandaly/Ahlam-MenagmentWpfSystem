using EyeClinic.Core.Interface;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeClinic.DataAccess.IRepositories
{
    public interface IReadyProductRepository : IBaseRepository<Model.ReadyProductDbo, ReadyProduct>, IInjectable
    {
        public Task<List<ReadyProductDbo>> GetReadyProductAsyncById(TestDto testDto);
        public Task<List<ReadyProduct>> GetReadyProductAsyncById2(TestDto testDto);
        public Task<List<Model.ReadyProductDbo>> GetReadyProductAsyncByName(TestDto testDto,string searchText);
        public Task<List<Model.ReadyProductDbo>> GetReadyProductWareHouse();
        public Task<List<Model.ReadyProductDbo>> GetReadyProductWareHouseByName(string Name);
        public Task<double> GetSumReadyProducts(int id);
        public  Task<double> GetSumWasteValueProducts(int id);
        public  Task<double> GetSumWightValueProducts(int id);
        //public  Task<double> GetSumValueProducts(Model.TestDto test);
        public  Task<double> GetCountValueProducts(int id);
    }
}
