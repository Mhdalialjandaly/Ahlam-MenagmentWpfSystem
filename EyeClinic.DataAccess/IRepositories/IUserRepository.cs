using System.Collections.Generic;
using System.Threading.Tasks;
using EyeClinic.Core.Interface;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.Model;

namespace EyeClinic.DataAccess.IRepositories
{
    public interface IUserRepository : IBaseRepository<Model.UserDto, User>, IInjectable
    {
        Task<List<Model.RoleDto>> GetRoles();
        Task Add();
        //Task AddWrongPassWordRecord(string  user);
    }
}
