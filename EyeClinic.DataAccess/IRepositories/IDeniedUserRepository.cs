using System.Threading.Tasks;
using EyeClinic.Core.Interface;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;

namespace EyeClinic.DataAccess.IRepositories
{
    public interface IDeniedUserRepository : IBaseRepository<Model.DeniedUser, DeniedUser>, IInjectable
    {
     
    }
}
