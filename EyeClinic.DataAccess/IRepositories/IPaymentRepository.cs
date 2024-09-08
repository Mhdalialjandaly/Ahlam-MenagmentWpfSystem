using EyeClinic.Core.Interface;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;

namespace EyeClinic.DataAccess.IRepositories
{
    public interface IPaymentRepository : IBaseRepository<Model.PaymentDto, Payment>, IInjectable
    {

    }
}
