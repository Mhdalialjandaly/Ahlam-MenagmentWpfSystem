using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;

namespace EyeClinic.DataAccess.Repositories
{
    public class ReservationRepository : BaseRepository<Model.ReservationDto, Reservation>, IReservationRepository
    {
        public ReservationRepository(IMapper mapper, EyeClinicContext context)
            : base(mapper, context) {
        }
    }
}
