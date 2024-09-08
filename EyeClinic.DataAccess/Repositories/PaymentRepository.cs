using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using EyeClinic.Core;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EyeClinic.DataAccess.Repositories
{
    public class PaymentRepository : BaseRepository<Model.PaymentDto, Payment>, IPaymentRepository
    {
        private readonly IMapper _mapper;
        private readonly EyeClinicContext _context;

        public PaymentRepository(IMapper mapper, EyeClinicContext context)
            : base(mapper, context) {
            _mapper = mapper;
            _context = context;
        }

        public override async Task<List<Model.PaymentDto>> GetByKey(Expression<Func<Payment, bool>> predicate) {
            var dbItems = await _context.Payments
                .Include(e => e.PaymentType)
                .Where(e => e.DeletedDate == null)
                .Where(predicate)
                .ToListAsync();

            if (!dbItems.IsNullOrEmpty()) {
                foreach (var payment in dbItems) {
                    await _context.Entry(payment).ReloadAsync();
                }
            }

            return _mapper.Map<List<Model.PaymentDto>>(dbItems);
        }
    }
}
