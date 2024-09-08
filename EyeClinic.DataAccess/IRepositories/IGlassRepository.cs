using EyeClinic.Core.Interface;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;

namespace EyeClinic.DataAccess.IRepositories
{
    public interface IGlassRepository : IBaseRepository<Model.GlassDto, Glass>, IInjectable
    {

    }
}
