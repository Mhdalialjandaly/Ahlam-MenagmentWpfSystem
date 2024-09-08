using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.Base;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.LabTests.LabTestResult
{
    public class LabTestResultViewModel : BaseViewModel<LabTestResultView>
    {
        private readonly IPatientVisitLabTestRepository _patientVisitLabTestRepository;
        private readonly IPatientVisitRepository _patientVisitRepository;

        public LabTestResultViewModel(IPatientVisitLabTestRepository patientVisitLabTestRepository,
                IPatientVisitRepository patientVisitRepository) {
            _patientVisitLabTestRepository = patientVisitLabTestRepository;
            _patientVisitRepository = patientVisitRepository;
        }


        public async Task Initialize(int patientVisitId) {
            PatientVisitId = patientVisitId;
            var selectedVisitDate = await _patientVisitRepository.GetById(patientVisitId);

            PatientVisitLabTests = await _patientVisitLabTestRepository
                .GetByKey(e => !e.Done &&
                               e.PatientVisit.PatientId == selectedVisitDate.PatientId &&
                                    e.PatientVisit.VisitDate.Date <= selectedVisitDate.VisitDate.Date);
        }

        public List<PatientVisitLabTest> PatientVisitLabTests { get; set; }

        public int PatientVisitId { get; set; }

    }
}
