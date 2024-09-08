using System.Threading.Tasks;
using EyeClinic.Core.Interface;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;

namespace EyeClinic.DataAccess.IRepositories
{
    public interface IPatientGlassRepository : IBaseRepository<Model.PatientGlassDto, PatientGlass>, IInjectable
    {
        Task<Model.PatientGlassDto> AddOrUpdate(Model.PatientGlassDto buildFromProperties);
    }
}
