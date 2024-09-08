using System.Threading.Tasks;
using EyeClinic.Core.Interface;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;

namespace EyeClinic.DataAccess.IRepositories
{
    public interface IOperationRepository : IBaseRepository<Model.OperationDto, Operation>, IInjectable
    {
        Task CalculateRevenue(int operationId);
    }
}
