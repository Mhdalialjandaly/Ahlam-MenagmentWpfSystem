using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;

namespace EyeClinic.DataAccess.Repositories
{
    public class CustomerRepository : BaseRepository<Model.CustomerDto, Customer>, ICustomerRepository
    {
        public CustomerRepository(IMapper mapper, EyeClinicContext context) : base(mapper, context) 
        {
        }
    }
}
