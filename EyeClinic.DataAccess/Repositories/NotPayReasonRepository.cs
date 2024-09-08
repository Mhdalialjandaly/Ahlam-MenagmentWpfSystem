using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;

namespace EyeClinic.DataAccess.Repositories
{
    public class NotPayReasonRepository : BaseRepository<Model.NotPayReasonDto, NotPayReason>, INotPayReasonRepository
    {
        public NotPayReasonRepository(IMapper mapper, EyeClinicContext context) 
            : base(mapper, context) {
        }
    }
}
