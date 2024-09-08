using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;

namespace EyeClinic.DataAccess.Repositories
{
    public class OldPatientEyeImageRepository : BaseRepository<OldPatientEyeImageDto, OldPatientEyeImage>, IOldPatientEyeImageRepository
    {
        public OldPatientEyeImageRepository(IMapper mapper, EyeClinicContext context) 
            : base(mapper, context) {
        }
    }
}
