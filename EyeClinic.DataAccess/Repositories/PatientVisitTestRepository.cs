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
    public class PatientVisitTestRepository : BaseRepository<Model.PatientVisitsTestDto, PatientVisitsTest>, IPatientVisitTestRepository
    {
        private readonly IMapper _mapper;
        private readonly EyeClinicContext _context;

        public PatientVisitTestRepository(IMapper mapper, EyeClinicContext context)
            : base(mapper, context) {
            _mapper = mapper;
            _context = context;
        }

        public override async Task<List<Model.PatientVisitsTestDto>> GetByKey(Expression<Func<PatientVisitsTest, bool>> predicate) {
            var result = await _context.PatientVisitsTests
                .Include(e => e.Test)
                .Include(e => e.MedicalCenter)
                .AsNoTracking()
                .Where(e => e.DeletedDate == null)
                .Where(predicate)
                .ToListAsync();

            return _mapper.Map<List<Model.PatientVisitsTestDto>>(result);
        }

        public async Task<int> GetLastImageNumber() {
            var list = await _context.PatientVisitsTests
                .Include(e => e.Test)
                .Where(e => e.DeletedDate == null && !string.IsNullOrWhiteSpace(e.ImageNumber))
                //.Where(e => e.Test.Code == "17")
                .Where(e => e.Test.Code == "17" || e.Test.Code == "1" || e.Test.Code == "10")
                .ToListAsync();

            var max = list.Max(GetMaxImageNumber);
            return max;
        }
        public async Task<DateTime?> GetVisitDateById(int patientVisitId) {
            var item = (await _context
                    .PatientVisits
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Id == patientVisitId))
                ?.VisitDate;

            return item;
        }
        
         public async Task<int> GetByVisitIdImagesHadScanned(int id)
        {
            var eyeMuscles = await _context.PatientVisitsTests
                .AsNoTracking()
                .Where(e => e.DeletedDate == null &&
                            e.PatientVisitId ==id)
                .ToListAsync();
            var max = eyeMuscles.Max(GetMaxImageNumber);
            return max;
        }

        private int GetMaxImageNumber(PatientVisitsTest arg) {
            if (int.TryParse(arg.ImageNumber, out int imageNumber)) {
                return imageNumber;
            }
            return 0;
        }

        public async Task<int> GetLastScanNumber() {
            var list = await _context.PatientVisitsTests
                .Include(e => e.Test)
                .Where(e => e.DeletedDate == null && !string.IsNullOrWhiteSpace(e.ImageNumber))
                .Where(e => e.Test.EnName.ToLower().Contains("scan"))
                .ToListAsync();

            var max = list.Max(GetMaxImageNumber);
            return max;
        }

        public async Task<int> GetTotalEyeMuscles() {
            var eyeMuscles = await _context.PatientVisitsTests
                .AsNoTracking()
                .Where(e => e.DeletedDate == null &&
                            e.TestId == 27)
                .ToListAsync();
            var max = eyeMuscles.Max(GetMaxImageNumber);
            return max;

        }

        public async Task ClearImageNumber(int id) {
            var item = await _context.PatientVisitsTests.FirstOrDefaultAsync(e => e.Id == id);
            item.ImageNumber = "";
            await _context.SaveChangesAsync();
        }

        public async Task<List<string>> GetImagesByPatientId(int patientId) {
            var images = await _context
                .PatientVisitsTests.AsNoTracking()
                .Include(e => e.PatientVisit)
                .Where(e => e.DeletedDate == null &&
                            e.PatientVisit.PatientId == patientId &&
                           !string.IsNullOrEmpty(e.ImageNumber))
                .Select(e => e.ImageNumber)
                .ToListAsync();

            return images;
        }

        public async Task<List<PatientVisitsTestDto>> GetByDateAndTest(DateTime fromDate, DateTime toDate, int testId) {
            var items = await _context
                .PatientVisitsTests
                .AsNoTracking()
                .Include(e => e.PatientVisit)
                .ThenInclude(e => e.Patient)
                .Where(e=> e.PatientVisit.DeletedDate == null)                
                .Include(e => e.Test)
                .Where(e => e.DeletedDate == null)
                .Where(e => e.TestId == testId)                
                .Where(e =>!string.IsNullOrWhiteSpace(e.LeftEyeNote) && !string.IsNullOrWhiteSpace(e.RightEyeNote))
                .Where(e => e.PatientVisit.VisitDate.Date >= fromDate.Date &&
                            e.PatientVisit.VisitDate.Date <= toDate.Date)
                .ToListAsync();

            return _mapper.Map<List<PatientVisitsTestDto>>(items);            
        }
        public async Task<List<PatientVisitsTestDto>> GetByDateAndTestD(DateTime fromDate, DateTime toDate, int testId)
        {
            var items = await _context
                .PatientVisitsTests
                .AsNoTracking()
                .Include(e => e.PatientVisit)
                .ThenInclude(e => e.Patient)
                .Where(e => e.PatientVisit.DeletedDate == null)
                .Include(e => e.Test)
                .Where(e => e.DeletedDate == null)
                .Where(e => e.TestId == testId)
                .Where(e => string.IsNullOrWhiteSpace(e.LeftEyeNote) && string.IsNullOrWhiteSpace(e.RightEyeNote))
                .Where(e => e.PatientVisit.VisitDate.Date >= fromDate.Date &&
                            e.PatientVisit.VisitDate.Date <= toDate.Date)
                .ToListAsync();

            return _mapper.Map<List<PatientVisitsTestDto>>(items);
        }



        public async Task<List<PatientVisitsTestDto>> GetByDateAndTestRightEye(DateTime fromDate, DateTime toDate, int testId) 
        {
            var items = await _context
                   .PatientVisitsTests
                   .AsNoTracking()              
                   .Include(e => e.PatientVisit)
                   .ThenInclude(e => e.Patient)
                   .Where(e => e.PatientVisit.DeletedDate == null  )
                   .Include(e => e.Test)
                   .Where(e => e.DeletedDate == null)
                   .Where(e => e.TestId == testId)
                   .Where(e => !string.IsNullOrWhiteSpace(e.RightEyeNote) && string.IsNullOrWhiteSpace(e.LeftEyeNote))
                   .Where(e => e.PatientVisit.VisitDate.Date >= fromDate.Date &&
                               e.PatientVisit.VisitDate.Date <= toDate.Date)
                   .ToListAsync();

            return _mapper.Map<List<PatientVisitsTestDto>>(items);
        }

        public async Task<List<PatientVisitsTestDto>> GetByDateAndTestRightEyeD(DateTime fromDate, DateTime toDate, int testId)
        {
            var items = await _context
                   .PatientVisitsTests
                   .AsNoTracking()
                   .Include(e => e.PatientVisit)
                   .ThenInclude(e => e.Patient)
                   .Where(e => e.PatientVisit.DeletedDate == null)
                   .Include(e => e.Test)
                   .Where(e => e.DeletedDate == null)
                   .Where(e => e.TestId == testId)
                   .Where(e => e.RightEyeNote=="" && e.LeftEyeNote!="")
                   .Where(e => e.PatientVisit.VisitDate.Date >= fromDate.Date &&
                               e.PatientVisit.VisitDate.Date <= toDate.Date)
                   .ToListAsync();

            return _mapper.Map<List<PatientVisitsTestDto>>(items);
        }


        public async Task<List<PatientVisitsTestDto>> GetByDateAndTestLeftEye(DateTime fromDate, DateTime toDate, int testId)
        {

            var items = await _context
                   .PatientVisitsTests
                   .AsNoTracking()
                   .Include(e => e.PatientVisit)
                   .ThenInclude(e => e.Patient)
                   .Where(e => e.PatientVisit.DeletedDate == null)
                   .Include(e => e.Test)
                   .Where(e => e.DeletedDate == null)
                   .Where(e => e.TestId == testId)
                   .Where(e => string.IsNullOrWhiteSpace(e.RightEyeNote) && !string.IsNullOrWhiteSpace(e.LeftEyeNote))
                   .Where(e => e.PatientVisit.VisitDate.Date >= fromDate.Date &&
                               e.PatientVisit.VisitDate.Date <= toDate.Date)
                   .ToListAsync();


            return _mapper.Map<List<PatientVisitsTestDto>>(items);
        }
        public async Task<List<PatientVisitsTestDto>> GetByDateAndTestLeftEyeD(DateTime fromDate, DateTime toDate, int testId)
        {
            var items = await _context
                   .PatientVisitsTests
                   .AsNoTracking()
                   .Include(e => e.PatientVisit)
                   .ThenInclude(e => e.Patient)
                   .Where(e => e.PatientVisit.DeletedDate == null)
                   .Include(e => e.Test)
                   .Where(e => e.DeletedDate == null)
                   .Where(e => e.TestId == testId)
                   .Where(e => e.RightEyeNote != "" && e.LeftEyeNote == "")
                   .Where(e => e.PatientVisit.VisitDate.Date >= fromDate.Date &&
                               e.PatientVisit.VisitDate.Date <= toDate.Date)
                   .ToListAsync();

            return _mapper.Map<List<PatientVisitsTestDto>>(items);
        }

    }
}
