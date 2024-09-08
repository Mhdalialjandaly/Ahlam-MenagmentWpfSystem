using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;

namespace EyeClinic.DataAccess.Repositories
{
    public class EyeTestRepository : BaseRepository<Model.EyeTestDto, EyeTest>, IEyeTestRepository
    {
        public EyeTestRepository(IMapper mapper, EyeClinicContext context)
            : base(mapper, context) {
        }
    }
}
