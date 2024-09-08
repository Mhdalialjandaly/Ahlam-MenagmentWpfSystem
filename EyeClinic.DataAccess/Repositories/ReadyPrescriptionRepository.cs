using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EyeClinic.DataAccess.Repositories
{
    public class ReadyPrescriptionRepository : BaseRepository<Model.ReadyPrescriptionDto, ReadyPrescription>, IReadyPrescriptionRepository
    {
        private readonly IMapper _mapper;
        private readonly EyeClinicContext _context;

        public ReadyPrescriptionRepository(IMapper mapper, EyeClinicContext context)
            : base(mapper, context) {
            _mapper = mapper;
            _context = context;
        }

        public override async Task<List<Model.ReadyPrescriptionDto>> GetAll() {
            return _mapper.Map<List<Model.ReadyPrescriptionDto>>(
                await _context.ReadyPrescriptions.AsNoTracking()
                    .Include(e => e.ReadyPrescriptionMedicines)
                    .ThenInclude(e => e.Medicine)
                    .ThenInclude(e => e.MedicineType)
                    .Include(e => e.ReadyPrescriptionMedicines)
                    .ThenInclude(e => e.MedicineUsage)
                    .Where(e => e.DeletedDate == null)
                    .ToListAsync());
        }
    }
}
