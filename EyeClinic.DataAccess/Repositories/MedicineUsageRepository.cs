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
    public class MedicineUsageRepository : BaseRepository<Model.MedicineUsageDto, MedicineUsage>, IMedicineUsageRepository
    {
        private readonly IMapper _mapper;
        private readonly EyeClinicContext _context;

        public MedicineUsageRepository(IMapper mapper, EyeClinicContext context)
            : base(mapper, context) {
            _mapper = mapper;
            _context = context;
        }

        public override async Task<List<Model.MedicineUsageDto>> GetAll() {
            var items = await _context.MedicineUsages.AsNoTracking()
                .Include(e => e.UsageMedicineType)
                .Where(e => e.DeletedDate == null)
                .ToListAsync();

            return _mapper.Map<List<Model.MedicineUsageDto>>(items);
        }

        public override async Task<List<Model.MedicineUsageDto>> GetByKey(Expression<Func<MedicineUsage, bool>> predicate) {
            var items = await _context.MedicineUsages.AsNoTracking()
                .Include(e => e.UsageMedicineType)
                .Where(e => e.DeletedDate == null)
                .Where(predicate)
                .ToListAsync();

            return _mapper.Map<List<Model.MedicineUsageDto>>(items);
        }
    }
}
