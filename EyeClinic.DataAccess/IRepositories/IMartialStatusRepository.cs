using EyeClinic.Core.Interface;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;

namespace EyeClinic.DataAccess.IRepositories
{
    public interface IMartialStatusRepository : IBaseRepository<Model.MartialStatusDto, MartialStatus>, IInjectable
    {

    }
}
