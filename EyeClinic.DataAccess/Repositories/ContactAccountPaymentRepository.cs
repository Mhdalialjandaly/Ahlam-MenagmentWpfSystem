using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;

namespace EyeClinic.DataAccess.Repositories
{
    public class ContactAccountPaymentRepository : BaseRepository<Model.ContactAccountPaymentDto, ContactAccountPayment>, IContactAccountPaymentRepository
    {
        public ContactAccountPaymentRepository(IMapper mapper, EyeClinicContext context) : base(mapper, context) {
        }
    }
}
