using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;

namespace EyeClinic.DataAccess.Repositories
{
    public class PayoutRepository : BaseRepository<Model.PayoutDto, Payout>, IPayoutRepository
    {
        public PayoutRepository(IMapper mapper, EyeClinicContext context)
            : base(mapper, context) {
        }
    }
}
