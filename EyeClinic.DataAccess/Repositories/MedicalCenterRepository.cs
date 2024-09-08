using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;

namespace EyeClinic.DataAccess.Repositories
{
    public class MedicalCenterRepository : BaseRepository<Model.MedicalCenterDto, MedicalCenter>, IMedicalCenterRepository
    {
        public MedicalCenterRepository(IMapper mapper, EyeClinicContext context)
            : base(mapper, context) {
        }
    }
}
