using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeClinic.DataAccess.Repositories
{
    public class PublicCustomerRepository : BaseRepository<Model.PublicCustomerDto, PublicCustomer>, IPublicCustomerRepository
    {
        private readonly IMapper _mapper;
        private readonly EyeClinicContext _context;

        public PublicCustomerRepository(IMapper mapper, EyeClinicContext context) : base(mapper, context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<List<PublicCustomerDto>> Search(string code, string firstName, string lastName)
        {
            var patients = _mapper.Map<List<Model.PublicCustomerDto>>(await _context.PublicCustomers
                .Include(e => e.CustomerOrders)
                
                .Where(e => e.DeletedDate == null)
                .Where(e => string.IsNullOrWhiteSpace(code) || e.Number.ToString() == code)
                .Where(e => string.IsNullOrWhiteSpace(firstName) || e.FirstName.StartsWith(firstName))
                .Where(e => string.IsNullOrWhiteSpace(lastName) || e.LastName.StartsWith(lastName))
                .ToListAsync());


          

            return patients;
        }
    }
}
