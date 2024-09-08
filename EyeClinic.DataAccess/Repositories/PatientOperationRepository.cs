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

namespace EyeClinic.DataAccess.Repositories
{
    public class PatientOperationRepository : BaseRepository<Model.PatientOperationDto, PatientOperation>, IPatientOperationRepository
    {
        private readonly IMapper _mapper;
        private readonly EyeClinicContext _context;

        public PatientOperationRepository(IMapper mapper, EyeClinicContext context)
            : base(mapper, context) {
            _mapper = mapper;
            _context = context;
        }

        public override async Task<Model.PatientOperationDto> Add(Model.PatientOperationDto modelToAdd) {
            var dbItem = _mapper.Map<PatientOperation>(modelToAdd);
            var item = await _context.PatientOperations.AddAsync(dbItem);
            await _context.SaveChangesAsync();
            await item.ReloadAsync();
            return _mapper.Map<Model.PatientOperationDto>(item.Entity);
        }

        public override async Task<List<Model.PatientOperationDto>> GetByKey(Expression<Func<PatientOperation, bool>> predicate) {
            return _mapper.Map<List<Model.PatientOperationDto>>(
                await _context.PatientOperations
                    .AsNoTracking()
                    .Include(e => e.MedicalCenter)
                    .Include(e => e.LeftEyeOperation)
                    .Include(e => e.RightEyeOperation)
                    .Where(e => e.DeletedDate == null)
                    .Where(predicate)
                    .OrderByDescending(e => e.OperationDate)
                    .ToListAsync()
            );
        }

        public async Task UpdateOperationStatus(int patientOperationId, bool finish) {
            var existItem = await _context.PatientOperations
                .FirstOrDefaultAsync(e => e.Id == patientOperationId);

            existItem.IsFinish = finish;
            existItem.LastModifiedDate = DateTime.Now;
            await SaveChangesAsync();
        }

        public async Task<List<Model.PatientOperationDto>> GetOperationSchedule(DateTime operationDate) {
            var items = await _context.PatientOperations
                .AsNoTracking()
                .Include(e => e.Patient)
                .Include(e => e.MedicalCenter)
                .Include(e => e.LeftEyeOperation)
                .Include(e => e.RightEyeOperation)
                .Where(e => e.DeletedDate == null)
                .Where(e => e.OperationDate.Date == operationDate.Date)
                .OrderByDescending(e=>e.RightEyeOperationId)
                .OrderByDescending(e=>e.LeftEyeOperationId)

                .ToListAsync();

            return _mapper.Map<List<Model.PatientOperationDto>>(items);
        }

        public async Task<List<PatientOperationSessionDto>> GetSessionSchedule(DateTime targetDateDate) {
            var items = await _context.PatientOperationSessions
                .AsNoTracking()
                .Include(e => e.PatientOperation)
                .ThenInclude(e => e.Patient)
                .Include(e => e.PatientOperation)
                .ThenInclude(e => e.MedicalCenter)
                .Include(e => e.PatientOperation)
                .ThenInclude(e => e.LeftEyeOperation)
                .Include(e => e.PatientOperation)
                .ThenInclude(e => e.RightEyeOperation)
                .Where(e => e.DeletedDate == null)
                .Where(e => e.SessionDate.Date == targetDateDate.Date)
                .ToListAsync();

            return _mapper.Map<List<PatientOperationSessionDto>>(items);
        }

        public async Task<int> GetRemainingByPatientId(int patientId) {
            var remaining = await _context.PatientOperations
                .AsNoTracking()
                .Where(e => e.PatientId == patientId)
                .SumAsync(e => e.Remaining);

            return remaining;
        }

        public async Task AddPayment(int patientId, int payment) {
            var operations = await _context
                .PatientOperations
                .Where(e => e.PatientId == patientId)
                .Where(e => e.Remaining > 0)
                .Where(e => e.DeletedDate == null)
                .OrderBy(e => e.OperationDate)
                .ToListAsync();

            foreach (var operation in operations) {
                if (payment == 0)
                    break;

                if (payment >= operation.Remaining) {
                    operation.DownPayment += operation.Remaining;
                    payment -= operation.Remaining;
                } else {
                    operation.DownPayment += payment;
                    payment = 0;
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
