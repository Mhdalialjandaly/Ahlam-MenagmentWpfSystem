using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.Base;
using Microsoft.EntityFrameworkCore;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.EyeDiagnosis.EyeDiagnosisHistory
{
    public partial class EyeDiagnosisHistoryViewModel
    {
        public EyeDiagnosisHistoryViewModel() { }
    }

    public partial class EyeDiagnosisHistoryViewModel : BaseViewModel<EyeDiagnosisHistoryView>
    {
        private readonly IPatientVisitDiagnosisRepository _patientVisitDiagnosisRepository;
        private readonly IMapper _mapper;

        public EyeDiagnosisHistoryViewModel(IPatientVisitDiagnosisRepository patientVisitDiagnosisRepository,
            IMapper mapper) {
            _patientVisitDiagnosisRepository = patientVisitDiagnosisRepository;
            _mapper = mapper;
        }

        public async Task Initialize(int patientId) {
            PatientId = patientId;

            var requiredTests = _mapper.Map<List<PatientVisitDiagnosis>>(
                await _patientVisitDiagnosisRepository.Get()
                .AsNoTracking()
                .Include(e => e.PatientVisit)
                .Where(e=>e.DeletedBy==null)
                .Include(e => e.Diagnosis)
                .Where(e => e.DeletedDate == null)
                .Where(e => e.PatientVisit.PatientId == patientId && e.PatientVisit.DeletedDate == null)
                .OrderByDescending(e => e.PatientVisit.VisitDate)
                .ToListAsync());

            if (requiredTests == null)
                return;

            PatientVisitDiagnoses = requiredTests
                .GroupBy(e => e.PatientVisit.VisitDate,
                    (key, value) => {
                        var patientVisitDiagnoses = value.ToList();
                        return new DiagnosisHistoryModel() {
                            VisitDate = key,
                            LeftEyeDiagnoses = patientVisitDiagnoses
                                .Where(e => e.LeftEye)
                                .ToList(),
                            RightEyeDiagnoses = patientVisitDiagnoses
                                .Where(e => e.LeftEye == false)
                                .ToList()
                        };
                    })
                .OrderByDescending(e => e.VisitDate)
                .ToList();
        }
        public async Task InitializeByVisitId(int visitId) {
            //PatientId = patientId;

            var requiredTests = _mapper.Map<List<PatientVisitDiagnosis>>(
                await _patientVisitDiagnosisRepository.Get()
                    .AsNoTracking()
                    .Include(e => e.PatientVisit)
                    .Where(e => e.DeletedBy == null)
                    .Include(e => e.Diagnosis)
                    .Where(e => e.DeletedDate == null)
                    .Where(e => e.PatientVisit.Id == visitId)
                    .OrderByDescending(e => e.PatientVisit.VisitDate)
                    .ToListAsync());

            if (requiredTests == null)
                return;

            PatientVisitDiagnoses = requiredTests
                .GroupBy(e => e.PatientVisit.VisitDate,
                    (key, value) => {
                        var patientVisitDiagnoses = value.ToList();
                        return new DiagnosisHistoryModel() {
                            VisitDate = key,
                            LeftEyeDiagnoses = patientVisitDiagnoses
                                .Where(e => e.LeftEye)
                                .ToList(),
                            RightEyeDiagnoses = patientVisitDiagnoses
                                .Where(e => e.LeftEye == false)
                                .ToList()
                        };
                    })
                .OrderByDescending(e => e.VisitDate)
                .ToList();
        }

        public List<DiagnosisHistoryModel> PatientVisitDiagnoses { get; set; }
        public int PatientId { get; set; }
    }
}
