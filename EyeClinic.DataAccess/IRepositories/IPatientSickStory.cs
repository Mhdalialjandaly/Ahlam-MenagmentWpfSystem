using EyeClinic.Core.Interface;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;

namespace EyeClinic.DataAccess.IRepositories
{
    public interface IPatientSickStory : IBaseRepository<Model.PatientSickStoryDto, PatientSickStory>, IInjectable
    {

    }
}
