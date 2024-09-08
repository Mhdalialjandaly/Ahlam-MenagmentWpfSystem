using EyeClinic.Core.Interface;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;

namespace EyeClinic.DataAccess.IRepositories
{
    public interface IContactAccountRepository : IBaseRepository<Model.ContactAccountDto, ContactAccount>, IInjectable
    {

    }
}
