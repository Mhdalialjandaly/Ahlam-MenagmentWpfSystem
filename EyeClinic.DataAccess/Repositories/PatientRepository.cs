using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EyeClinic.Core;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.Model.Custom;
using Microsoft.EntityFrameworkCore;
using PatientDisease = EyeClinic.DataAccess.Entities.PatientDisease;

namespace EyeClinic.DataAccess.Repositories
{
    public class PatientRepository : BaseRepository<Model.PatientDto, Patient>, IPatientRepository
    {
        private readonly IMapper _mapper;
        private readonly EyeClinicContext _context;

        public PatientRepository(IMapper mapper, EyeClinicContext context) : base(mapper, context) {
            _mapper = mapper;
            _context = context;
        }

        public override async Task<Model.PatientDto> GetById(object id) {
            var patient = await _context.Patients.AsNoTracking()
                .Include(e => e.PatientDiseases)
                .ThenInclude(e => e.Disease)
                .Include(e => e.PatientVisits.Where(c=>c.DeletedDate==null))
                .FirstOrDefaultAsync(e => e.Id == (int)id);

            return _mapper.Map<Model.PatientDto>(patient);
        }


        public override async Task Update(Model.PatientDto modelToUpdate) {
            var entityToUpdate = _mapper.Map<Patient>(modelToUpdate);
            entityToUpdate.LastModifiedDate = DateTime.Now;
            _context.Update(entityToUpdate);

            var changedEntriesCopy = _context.ChangeTracker.Entries()
                .Where(e => e.State is EntityState.Added or
                    EntityState.Modified or EntityState.Deleted)
                .ToList();

            await SaveChangesAsync();

            var items = await _context.PatientDiseases
                .Where(e => e.PatientId == modelToUpdate.Id)
                .ToListAsync();

            _context.PatientDiseases.RemoveRange(items);

            await _context.AddRangeAsync(modelToUpdate.PatientDiseases
                .Select(e => new PatientDisease {
                    PatientId = entityToUpdate.Id,
                    AgeOfInjury = e.AgeOfInjury,
                    CreatedDate = e.CreatedDate,
                    DiseaseId = e.DiseaseId,
                    LastModifiedDate = DateTime.Now,
                    LastCheckMeasure = e.LastCheckMeasure,
                    MaxMeasure = e.MaxMeasure
                }));

            await _context.SaveChangesAsync();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }

        public async Task<Model.PatientDto> GetByPatientVisitId(int patientVisitId) {
            var visit = await _context.PatientVisits
                .Include(e => e.Patient)
                .FirstOrDefaultAsync(e => e.Id == patientVisitId);

            return visit == null ? null : _mapper.Map<Model.PatientDto>(visit.Patient);
        }

        public async Task<List<Model.PatientDto>> Search(string code, string firstName, string lastName, bool includeExcelFile) {
            var patients = _mapper.Map<List<Model.PatientDto>>(await _context.Patients
                .Include(e => e.PatientVisits)
                .Include(e => e.PatientGlass)
                .Include(e => e.PatientDiseases)
                .ThenInclude(e => e.Disease)
                .Include(e => e.PatientOperations)
                .Where(e => e.DeletedDate == null)
                .Where(e => string.IsNullOrWhiteSpace(code) || e.Number.ToString() == code)
                .Where(e => string.IsNullOrWhiteSpace(firstName) || e.FirstName.StartsWith(firstName))
                .Where(e => string.IsNullOrWhiteSpace(lastName) || e.LastName.StartsWith(lastName))
                .ToListAsync());


            if (includeExcelFile) {
                var excelPatients = await _context.OldPatientEyeImages
                    .AsNoTracking()
                    .Where(e => string.IsNullOrWhiteSpace(firstName) || e.FirstName.StartsWith(firstName))
                    .Where(e => string.IsNullOrWhiteSpace(lastName) || e.LastName.StartsWith(lastName))
                    .ToListAsync();

                foreach (var patient in excelPatients) {
                    patients.Add(new PatientDto() {
                        FirstName = patient.FirstName,
                        LastName = patient.LastName
                    });
                }
            }

            return patients;
        }

        public async Task UpdateNote(int patientId, string specialNote) {
            var patient = await _context.Patients.FirstOrDefaultAsync(
                e => e.Id == patientId);
            patient.Notes = specialNote;
            patient.LastModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Model.PatientDto>> SearchByImageNumber(string imageNumber) {
            var patientIds = await _context.PatientVisitsTests
                .Include(e => e.PatientVisit)
                .Where(e => e.DeletedDate == null)
                .Where(e => e.ImageNumber == imageNumber)
                .Select(e => e.PatientVisit.PatientId)
                .ToListAsync();


            var dbItems = await _context.Patients
                .Include(e => e.PatientGlass)
                .Include(e => e.PatientDiseases)
                .ThenInclude(e => e.Disease)
                .Include(e => e.PatientOperations)
                .Where(e => patientIds.Contains(e.Id))
                .ToListAsync();

            var patientsFromExcel = await _context.OldPatientEyeImages
                .AsNoTracking()
                .Where(e => e.ImageNumber == imageNumber)
                .ToListAsync();

            foreach (var oldPatientEyeImage in patientsFromExcel) {
                dbItems.Add(new Patient {
                    FirstName = oldPatientEyeImage.FirstName,
                    LastName = oldPatientEyeImage.LastName,
                });
            }

            return _mapper.Map<List<Model.PatientDto>>(dbItems);
        }
    }
}
