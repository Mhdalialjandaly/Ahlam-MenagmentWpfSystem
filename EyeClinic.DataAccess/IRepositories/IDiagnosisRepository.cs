using EyeClinic.Core.Interface;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;

namespace EyeClinic.DataAccess.IRepositories
{
    public interface IDiagnosisRepository : IBaseRepository<Model.DiagnosisDto, Diagnosis>, IInjectable
    {

    }
}
