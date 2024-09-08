using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EyeClinic.DataAccess.Repositories
{
    public class PatientGlassRepository : BaseRepository<Model.PatientGlassDto, PatientGlass>, IPatientGlassRepository
    {
        private readonly EyeClinicContext _context;

        public PatientGlassRepository(IMapper mapper, EyeClinicContext context)
            : base(mapper, context) {
            _context = context;
        }

        public async Task<Model.PatientGlassDto> AddOrUpdate(Model.PatientGlassDto patientGlass) {
            var exist = _context.PatientGlasses
                .AsNoTracking()
                .FirstOrDefault(e => e.PatientId == patientGlass.PatientId);

            if (exist == null) {
                return await Add(patientGlass);
            }

            patientGlass.Id = exist.Id;
            patientGlass.CreatedDate = exist.CreatedDate;
            patientGlass.LastModifiedDate = DateTime.Now;
            await Update(patientGlass);
            return patientGlass;
        }
    }
}
