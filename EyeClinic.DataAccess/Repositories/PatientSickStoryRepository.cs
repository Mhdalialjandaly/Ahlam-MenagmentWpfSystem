using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;

namespace EyeClinic.DataAccess.Repositories
{
    public class PatientSickStoryRepository : BaseRepository<Model.PatientSickStoryDto, PatientSickStory>, IPatientSickStory
    {
        public PatientSickStoryRepository(IMapper mapper, EyeClinicContext context)
            : base(mapper, context) {
        }
    }
}
