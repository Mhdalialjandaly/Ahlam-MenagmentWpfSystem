using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;

namespace EyeClinic.DataAccess.Repositories
{
    public class DeniedUserRepository : BaseRepository<Model.DeniedUser, DeniedUser>, IDeniedUserRepository
    {
        public DeniedUserRepository(IMapper mapper, EyeClinicContext context) : base(mapper, context)
        {
        }
    }
}
