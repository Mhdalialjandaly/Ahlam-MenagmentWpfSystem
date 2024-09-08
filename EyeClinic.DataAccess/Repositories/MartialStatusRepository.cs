using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;

namespace EyeClinic.DataAccess.Repositories
{
    public class MartialStatusRepository : BaseRepository<Model.MartialStatusDto, MartialStatus>, IMartialStatusRepository
    {
        public MartialStatusRepository(IMapper mapper, EyeClinicContext context) 
            : base(mapper, context) {
        }
    }
}
