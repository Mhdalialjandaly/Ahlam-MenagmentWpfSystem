using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;

namespace EyeClinic.DataAccess.Repositories
{
    public class ContactRepository : BaseRepository<Model.ContactDto, Contact>, IContactRepository
    {
        public ContactRepository(IMapper mapper, EyeClinicContext context) : base(mapper, context) {
        }
    }
}
