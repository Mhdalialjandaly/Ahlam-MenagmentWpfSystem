using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.DataAccess.Repositories;
using EyeClinic.Model.Custom;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Base.Extends;
using EyeClinic.WPF.Components.Home.Clinic;
using EyeClinic.WPF.Components.PatientList.PatientFile.EyeDiagnosis.EyeDiagnosisHistory;
using EyeClinic.WPF.Components.PatientList.PatientFile.EyeTests.EyeTestHistory;
using EyeClinic.WPF.Components.PatientList.PatientFile.LabTests.LabTestHistory;
using EyeClinic.WPF.Components.PatientList.PatientFile.NewGlass;
using EyeClinic.WPF.Components.PatientList.PatientFile.NewGlass.NewGlassHistory;
using EyeClinic.WPF.Components.PatientList.PatientFile.Prescriptions.PrescriptionHistory;
using EyeClinic.WPF.Components.PatientList.PatientFile.Tests.TestHistory;
using EyeClinic.WPF.Setup;
using Unity;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.PatientSickStory
{
    public class PatientSickStoryViewModel : BaseViewModel<PatientSickStoryView>
    {
        private readonly IUnityContainer _container;
        private readonly IPatientSickStory _patientSickStory;
        private readonly IPatientVisitRepository _patientVisitRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDialogService _dialogService;
      

        public PatientSickStoryViewModel(IUnityContainer container, IPatientSickStory patientSickStory,
            IPatientVisitRepository patientVisitRepository, IPatientRepository patientRepository,
            IDialogService dialogService) {
            _container = container;
            _patientSickStory = patientSickStory;
            _patientVisitRepository = patientVisitRepository;
            _patientRepository = patientRepository;
            _dialogService = dialogService;

            SaveSickStoryCommand = new RelayCommand(SaveSickStory);
            EyeTestHistoryCommand = new RelayCommand(EyeTestHistory);
            DiagnosisHistoryCommand = new RelayCommand(DiagnosisHistory);
            PrescriptionHistoryCommand = new RelayCommand(PrescriptionHistory);
            TestHistoryCommand = new RelayCommand(TestHistory);
            PatientVisitGlassHistoryCommand = new RelayCommand(PatientVisitGlassHistory);
            LabTestHistoryCommand = new RelayCommand(LabTestHistory);
          
        }

        public PatientFileViewModel PatientFileViewModel { get; set; }
      
        private void SaveSickStory() {
            BusyExecute(async () => {
                if (PatientSickStory.CreatedDate == DateTime.MinValue)
                    PatientSickStory = await _patientSickStory.Add(PatientSickStory);
                else
                    await _patientSickStory.Update(PatientSickStory);

                await _patientRepository.UpdateNote(PatientSickStory.PatientId, PatientSpecialNote);
                BasePatientSpecialNote = PatientSpecialNote;
                OnSave?.Invoke(this, PatientSpecialNote);
            });
        }

        public override Task Initialize() {
            throw new Exception("Use Initialize with param");

        }

        public async Task Initialize(int patientVisitId) {
            var visit = await _patientVisitRepository.GetById(patientVisitId);
            PatientSickStory = (await _patientSickStory
                .GetByKey(e => e.PatientId == visit.PatientId))
                .FirstOrDefault() ?? new Model.PatientSickStoryDto { PatientId = visit.PatientId };

            PatientSpecialNote = (await _patientRepository.GetById(visit.PatientId)).Notes;
            BasePatientSpecialNote = PatientSpecialNote;


            PatientVisitId = patientVisitId;
            VisitDate = await _patientVisitRepository
              .GetVisitDateById(PatientVisitId);
        }

        public event EventHandler<string> OnSave;
        public DateTime? VisitDate { get; set; }

        public Model.PatientSickStoryDto PatientSickStory { get; set; }
        public string PatientSpecialNote { get; set; }
        public string BasePatientSpecialNote { get; set; }

        public PatientQuickInfo PatientQuickInfo { get; set; }
        public int PatientVisitId { get; set; }
        public string LastGlassInfoDate { get; set; }
        public NewGlassView LastGlassInfo { get; set; }


        public ICommand SaveSickStoryCommand { get; set; }
        public ICommand EyeTestHistoryCommand { get; set; }
        public ICommand DiagnosisHistoryCommand { get; set; }
        public ICommand PrescriptionHistoryCommand { get; set; }
        public ICommand TestHistoryCommand { get; set; }
        public ICommand PatientVisitGlassHistoryCommand { get; set; }
        public ICommand LabTestHistoryCommand { get; set; }
        public ICommand CancelCommand => new RelayCommand(() => {
            ContainerHandler.Container.Resolve<IDialogService>().DisposeLastDialog();
        });

        private void EyeTestHistory() {
            BusyExecute(async () => {
                var history = _container.Resolve<EyeTestHistoryViewModel>();
                await history.Initialize(PatientSickStory.PatientId);
                _dialogService.ShowInformationDialog(history.GetView() as EyeTestHistoryView);
            });
        }

        private void DiagnosisHistory() {
            BusyExecute(async () => {
                var history = _container.Resolve<EyeDiagnosisHistoryViewModel>();
                await history.Initialize(PatientSickStory.PatientId);

                _dialogService.ShowInformationDialog(history.GetView() as EyeDiagnosisHistoryView);
            });
        }

        private void PrescriptionHistory() {
            BusyExecute(async () => {
                var history = _container.Resolve<PrescriptionHistoryViewModel>();
                await history.Initialize(PatientSickStory.PatientId);

                _dialogService.ShowInformationDialog(history.GetView() as PrescriptionHistoryView);
            });
        }

        private void TestHistory() {
            BusyExecute(async () => {
                var history = _container.Resolve<TestHistoryViewModel>();
                await history.Initialize(PatientSickStory.PatientId);
                
                _dialogService.ShowInformationDialog(history.GetView() as TestHistoryView);
            });
        }

        private void PatientVisitGlassHistory() {
            BusyExecute(async () => {
                var history = _container.Resolve<NewGlassHistoryViewModel>();
                await history.Initialize(PatientSickStory.PatientId);

                _dialogService.ShowInformationDialog(history.GetView() as NewGlassHistoryView);
            });
        }

      
        private void LabTestHistory() {
            BusyExecute(async () => {
                var history = _container.Resolve<LabTestHistoryViewModel>();
                await history.Initialize(PatientSickStory.PatientId);

                _dialogService.ShowInformationDialog(history.GetView() as LabTestHistoryView);
            });
        }

        
    }
}
