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
    public interface IReadyItemProductingRepository : IBaseRepository<Model.ReadyItemProductingDto, ReadyItemProducting>, IInjectable
    {
        public Task<List<Model.ReadyItemProductingDto>> GetProductingItemAsyncById(int Id);
        public Task<double> GetSumProductingItems(int id);
    }
}
