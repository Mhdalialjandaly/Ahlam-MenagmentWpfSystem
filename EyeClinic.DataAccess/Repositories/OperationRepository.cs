using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EyeClinic.DataAccess.Repositories
{
    public class OperationRepository : BaseRepository<Model.OperationDto, Operation>, IOperationRepository
    {
        private readonly IMapper _mapper;
        private readonly EyeClinicContext _context;

        public OperationRepository(IMapper mapper, EyeClinicContext context)
            : base(mapper, context) {
            _mapper = mapper;
            _context = context;
        }

        public async Task CalculateRevenue(int operationId) {
            var operation = await _context.PatientOperations
                .Include(e => e.PatientOperationSessions)
                .FirstOrDefaultAsync(e => e.Id == operationId);

            var totalSessionPayments = operation.PatientOperationSessions
                .Sum(e => e.PaymentValue);

            var revenue = ((operation.DownPayment ??
                            0) + totalSessionPayments) - operation.CenterCost - operation.ClinicCost;
            operation.Revenue = revenue;
            await _context.SaveChangesAsync();
        }
    }
}
