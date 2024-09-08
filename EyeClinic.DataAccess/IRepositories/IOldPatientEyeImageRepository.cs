using EyeClinic.Core.Interface;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.Model;

namespace EyeClinic.DataAccess.IRepositories
{
    public interface IOldPatientEyeImageRepository : IBaseRepository<OldPatientEyeImageDto, OldPatientEyeImage>, IInjectable
    {

    }
}
