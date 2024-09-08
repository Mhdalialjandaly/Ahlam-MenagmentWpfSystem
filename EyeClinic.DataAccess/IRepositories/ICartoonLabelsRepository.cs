using EyeClinic.Core.Interface;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EyeClinic.DataAccess.IRepositories
{
    public interface ICartoonLabelsRepository : IBaseRepository<Model.CartoonLabelsDto, CartoonLabels>, IInjectable
    {
        public  Task<List<Model.CartoonLabelsDto>> GetCartoonItemAsyncById(int Id);
        public Task<double> GetSumCartoonLabels(int id);
        public Task<double> GetLastValue(int id);
    }
}
