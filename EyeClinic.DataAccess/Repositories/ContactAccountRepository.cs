using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;

namespace EyeClinic.DataAccess.Repositories
{
    public class ContactAccountRepository : BaseRepository<Model.ContactAccountDto, ContactAccount>, IContactAccountRepository
    {
        public ContactAccountRepository(IMapper mapper, EyeClinicContext context) : base(mapper, context) {
        }
    }
}
