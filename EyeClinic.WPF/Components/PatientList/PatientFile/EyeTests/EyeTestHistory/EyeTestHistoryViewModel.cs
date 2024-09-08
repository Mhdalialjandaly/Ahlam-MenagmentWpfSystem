using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.Base;
using Microsoft.EntityFrameworkCore;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.EyeTests.EyeTestHistory
{
    public partial class EyeTestHistoryViewModel
    {
        public EyeTestHistoryViewModel() { }
    }

    public partial class EyeTestHistoryViewModel : BaseViewModel<EyeTestHistoryView>
    {
        private readonly IPatientVisitEyeTestRepository _patientVisitEyeTestRepository;
        private readonly IMapper _mapper;

        public EyeTestHistoryViewModel(IPatientVisitEyeTestRepository patientVisitEyeTestRepository,
            IMapper mapper) {
            _patientVisitEyeTestRepository = patientVisitEyeTestRepository;
            _mapper = mapper;
        }

        public async Task Initialize(int patientId) {
            PatientId = patientId;

            var requiredTests = _mapper.Map<List<PatientVisitEyeTestDto>>(
                await _patientVisitEyeTestRepository.Get()
                .AsNoTracking()
                .Include(e => e.PatientVisit)
                .Include(e => e.EyeTest)
                .Where(e => e.PatientVisit.PatientId == patientId &&
                            e.DeletedDate == null)
                .OrderByDescending(e => e.PatientVisit.VisitDate)
                .ToListAsync());

            PatientVisitEyeTests = requiredTests
                .GroupBy(e => e.PatientVisit.VisitDate,
                    (key, value) => new TestHistoryModel {
                        VisitDate = key,
                        PatientVisitEyeTests = value.ToList()
                    })
                .OrderByDescending(e => e.VisitDate)
                .ToList();
        }
        public async Task InitializeByVisitId(int visitId) {
            //PatientId = patientId;

            var requiredTests = _mapper.Map<List<PatientVisitEyeTestDto>>(
                await _patientVisitEyeTestRepository.Get()
                    .AsNoTracking()
                    .Include(e => e.PatientVisit)
                    .Include(e => e.EyeTest)
                    .Where(e => e.PatientVisit.Id == visitId &&
                                e.DeletedDate == null)
                    .OrderByDescending(e => e.PatientVisit.VisitDate)
                    .ToListAsync());

            PatientVisitEyeTests = requiredTests
                .GroupBy(e => e.PatientVisit.VisitDate,
                    (key, value) => new TestHistoryModel {
                        VisitDate = key,
                        PatientVisitEyeTests = value.ToList()
                    })
                .OrderByDescending(e => e.VisitDate)
                .ToList();
        }

        public List<TestHistoryModel> PatientVisitEyeTests { get; set; }
        public int PatientId { get; set; }
    }
}
