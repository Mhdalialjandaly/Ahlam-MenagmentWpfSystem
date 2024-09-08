using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;

namespace EyeClinic.DataAccess.Repositories
{
    public class LocationRepository : BaseRepository<Model.LocationDto, Location>, ILocationRepository
    {
        public LocationRepository(IMapper mapper, EyeClinicContext context)
            : base(mapper, context) {
        }
    }
}
