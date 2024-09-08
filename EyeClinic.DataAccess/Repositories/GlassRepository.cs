using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;

namespace EyeClinic.DataAccess.Repositories
{
    public class GlassRepository : BaseRepository<Model.GlassDto, Glass>, IGlassRepository
    {
        public GlassRepository(IMapper mapper, EyeClinicContext context)
            : base(mapper, context) {
        }
    }
}
