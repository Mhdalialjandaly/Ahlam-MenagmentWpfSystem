using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using Microsoft.EntityFrameworkCore;

namespace EyeClinic.DataAccess.Repositories
{
    public class PatientOperationSessionRepository : BaseRepository<Model.PatientOperationSessionDto, PatientOperationSession>, IPatientOperationSessionRepository
    {
        private readonly IMapper _mapper;
        private readonly EyeClinicContext _context;

        public PatientOperationSessionRepository(IMapper mapper, EyeClinicContext context)
            : base(mapper, context) {
            _mapper = mapper;
            _context = context;
        }

        public override async Task<PatientOperationSessionDto> Add(PatientOperationSessionDto modelToAdd) {
            var entityToAdd = _mapper.Map<PatientOperationSession>(modelToAdd);

            var totalSessions = await _context.PatientOperationSessions
                .Where(e => e.PatientOperationId == modelToAdd.PatientOperationId)
                .CountAsync();
            entityToAdd.SessionNumber = totalSessions + 2;

            var result = await _context.AddAsync(entityToAdd);

            var changedEntriesCopy = _context.ChangeTracker.Entries()
                .Where(e => e.State is EntityState.Added or
                    EntityState.Modified or EntityState.Deleted)
                .ToList();

            await SaveChangesAsync();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;

            return _mapper.Map<PatientOperationSessionDto>(result.Entity);
        }
    }
}
