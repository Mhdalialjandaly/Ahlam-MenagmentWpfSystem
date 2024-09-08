using System.Threading.Tasks;
using EyeClinic.Core.Interface;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;

namespace EyeClinic.DataAccess.IRepositories
{
    public interface IPaymentTypeRepository : IBaseRepository<Model.PaymentTypeDto, PaymentType>, IInjectable
    {
        Task<int> GetRemaining(int id);
    }
}
