using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EyeClinic.DataAccess.Repositories
{
    public class PaymentTypeRepository : BaseRepository<Model.PaymentTypeDto, PaymentType>, IPaymentTypeRepository
    {
        private readonly IMapper _mapper;
        private readonly EyeClinicContext _context;

        public PaymentTypeRepository(IMapper mapper, EyeClinicContext context)
            : base(mapper, context) {
            _mapper = mapper;
            _context = context;
        }

        public override async Task<List<Model.PaymentTypeDto>> GetByKey(Expression<Func<PaymentType, bool>> predicate) {
            var items = await _context.PaymentTypes
                .AsNoTracking()
                .Include(e => e.Payments)
                .Where(e => e.DeletedDate == null)
                .Where(predicate)
                .ToListAsync();

            return _mapper.Map<List<Model.PaymentTypeDto>>(items);
        }

        public async Task<int> GetRemaining(int id) {
            return (await _context.PaymentTypes.FirstOrDefaultAsync(e => e.Id == id))
                ?.Remaining ?? 0;
        }
    }
}
