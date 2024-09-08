using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.Base;
using Microsoft.EntityFrameworkCore;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.LabTests.LabTestHistory
{
    public partial class LabTestHistoryViewModel
    {
        public LabTestHistoryViewModel() { }
    }

    public partial class LabTestHistoryViewModel : BaseViewModel<LabTestHistoryView>
    {
        private readonly IPatientVisitLabTestRepository _patientVisitLabTestRepository;
        private readonly IMapper _mapper;

        public LabTestHistoryViewModel(IPatientVisitLabTestRepository patientVisitLabTestRepository,
            IMapper mapper) {
            _patientVisitLabTestRepository = patientVisitLabTestRepository;
            _mapper = mapper;
        }

        public async Task Initialize(int patientId) {
            PatientId = patientId;

            var requiredTests = _mapper.Map<List<PatientVisitLabTestDto>>(
                await _patientVisitLabTestRepository.Get()
                .AsNoTracking()
                .Include(e => e.PatientVisit)
                .Include(e => e.LabTest)
                .Where(e => e.PatientVisit.PatientId == patientId)
                .OrderByDescending(e => e.PatientVisit.VisitDate)
                .ToListAsync());

            if (requiredTests == null)
                return;

            PatientVisitLabTests = new ObservableCollection<LabTestHistoryModel>(
                requiredTests
                    .GroupBy(e => e.PatientVisit.VisitDate.Date,
                        (key, value) => new LabTestHistoryModel {
                            VisitDate = key,
                            PatientVisitLabTests = value.ToList()
                        })
                    .OrderByDescending(e => e.VisitDate)
                    .ToList());
        }

        public async Task InitializeByVisitId(int visitId) {
            //PatientId = patientId;

            var requiredTests = _mapper.Map<List<PatientVisitLabTestDto>>(
                await _patientVisitLabTestRepository.Get()
                    .AsNoTracking()
                    .Include(e => e.PatientVisit)
                    .Include(e => e.LabTest)
                    .Where(e => e.PatientVisit.Id == visitId)
                    .OrderByDescending(e => e.PatientVisit.VisitDate)
                    .ToListAsync());

            if (requiredTests == null)
                return;

            PatientVisitLabTests = new ObservableCollection<LabTestHistoryModel>(
                requiredTests
                    .GroupBy(e => e.PatientVisit.VisitDate.Date,
                        (key, value) => new LabTestHistoryModel {
                            VisitDate = key,
                            PatientVisitLabTests = value.ToList()
                        })
                    .OrderByDescending(e => e.VisitDate)
                    .ToList());
        }

        public ObservableCollection<LabTestHistoryModel> PatientVisitLabTests { get; set; }
        public int PatientId { get; set; }
    }
}
