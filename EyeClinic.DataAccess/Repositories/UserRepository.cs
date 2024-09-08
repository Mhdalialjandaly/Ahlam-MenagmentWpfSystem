using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using Microsoft.EntityFrameworkCore;
using User = EyeClinic.DataAccess.Entities.User;

namespace EyeClinic.DataAccess.Repositories
{
    public class UserRepository : BaseRepository<Model.UserDto, User>, IUserRepository
    {
        private readonly IMapper _mapper;
        private readonly EyeClinicContext _context;

        public UserRepository(IMapper mapper, EyeClinicContext context)
            : base(mapper, context)
        {
            _mapper = mapper;
            _context = context;
        }

        public override async Task<List<Model.UserDto>> GetByKey(Expression<Func<User, bool>> predicate)
        {
            var result = await _context.Users.AsNoTracking()
                .Include(e => e.Role)
                .Where(e => e.DeletedDate == null)
                .Where(predicate).ToListAsync();
            return _mapper.Map<List<Model.UserDto>>(result);
        }

        public async Task<List<RoleDto>> GetRoles()
        {
            var roles = await _context.Roles
                .Where(e => e.DeletedDate == null)
                .ToListAsync();
            return _mapper.Map<List<RoleDto>>(roles);
        }

      

        public async Task Add()
        {

            var UserHasWrongPass = await _context
                .Roles
                .Where(e => e.EnName == "Admin")
                .FirstOrDefaultAsync();
            UserHasWrongPass.PassWrong = DateTime.Today;
         
         
        }
    }
}

