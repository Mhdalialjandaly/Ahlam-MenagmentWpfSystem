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
    public class MedicineRepository : BaseRepository<Model.MedicineDto, Medicine>, IMedicineRepository
    {
        private readonly IMapper _mapper;
        private readonly EyeClinicContext _context;

        public MedicineRepository(IMapper mapper, EyeClinicContext context)
            : base(mapper, context) {
            _mapper = mapper;
            _context = context;
        }

        public override async Task<List<Model.MedicineDto>> GetAll() {
            return _mapper.Map<List<Model.MedicineDto>>(
                await _context.Medicines
                    .Include(e => e.MedicineType)
                    .Where(e => e.DeletedDate == null)
                    .ToListAsync());
        }

        public override async Task<List<Model.MedicineDto>> GetByKey(Expression<Func<Medicine, bool>> predicate) {
            var result = await _context.Medicines.AsNoTracking()
                .Include(e => e.MedicineType)
                .Where(e => e.DeletedDate == null)
                .Where(predicate).ToListAsync();

            return _mapper.Map<List<Model.MedicineDto>>(result);
        }

        public override async Task<MedicineDto> GetById(object id) {
            var result = await _context.Medicines.AsNoTracking()
                .Include(e => e.MedicineType)
                .FirstOrDefaultAsync(e => e.Id == (int)id);

            return _mapper.Map<MedicineDto>(result);
        }
    }
}
