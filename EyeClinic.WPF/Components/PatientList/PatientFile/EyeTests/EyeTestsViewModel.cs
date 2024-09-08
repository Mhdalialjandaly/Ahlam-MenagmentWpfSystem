using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.Core;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.DataAccess.Repositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Base.Extends;
using EyeClinic.WPF.Components.PatientList.PatientFile.EyeTests.EyeTestHistory;
using Microsoft.EntityFrameworkCore;
using Unity;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.EyeTests
{
    public class EyeTestsViewModel : BaseViewModel<EyeTestsView>
    {
        private readonly IEyeTestRepository _eyeTestRepository;
        private readonly IUnityContainer _container;
        private readonly IPatientVisitEyeTestRepository _patientVisitEyeTestRepository;
        private readonly IPatientVisitRepository _patientVisitRepository;
        private readonly IDialogService _dialogService;

        public EyeTestsViewModel(IEyeTestRepository eyeTestRepository, IUnityContainer container,
            IPatientVisitEyeTestRepository patientVisitEyeTestRepository,
            IPatientVisitRepository patientVisitRepository,
            IDialogService dialogService) {
            _eyeTestRepository = eyeTestRepository;
            _container = container;
            _patientVisitEyeTestRepository = patientVisitEyeTestRepository;
            _patientVisitRepository = patientVisitRepository;
            _dialogService = dialogService;

            AddEyeTestCommand = new RelayCommand(AddEyeTest);
            EyeTestHistoryCommand = new RelayCommand(EyeTestHistory);
            SaveAllCommand = new RelayCommand(SaveAll);
        }

        public override Task Initialize() {
            throw new Exception("Use Initialize with param");
        }


        public PatientFileViewModel PatientFileViewModel { get; set; }

        public async Task Initialize(int patientVisitId) {
            PatientVisitId = patientVisitId;

            EyeTests = await _eyeTestRepository.GetAll();

            VisitDate = await _patientVisitRepository
              .GetVisitDateById(PatientVisitId);

            var item = await _patientVisitRepository.Get().AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == patientVisitId);

            if (item != null)
                PatientId = item.PatientId;

            var requiredEyeTests = await _patientVisitEyeTestRepository
                .GetByKey(e => e.PatientVisitId == patientVisitId);
            if (requiredEyeTests.IsNullOrEmpty()) {
                PatientVisitEyeTests = new ObservableCollection<PatientVisitEyeTestDto>(
                    EyeTests.Select(e => new PatientVisitEyeTestDto() {
                        EyeTest = e,
                        EyeTestId = e.Id,
                        CreatedDate = DateTime.Now,
                        PatientVisitId = PatientVisitId
                    }).ToList());

                return;
            }

            PatientVisitEyeTests = new ObservableCollection<PatientVisitEyeTestDto>(requiredEyeTests);
        }

        internal Task Initialize(object id) {
            throw new NotImplementedException();
        }
        public DateTime? VisitDate { get; set; }
        public int PatientVisitId { get; set; }
        public int PatientId { get; set; }

        public List<EyeTestDto> EyeTests { get; set; }
        public EyeTestDto SelectedEyeTest { get; set; }

        public ObservableCollection<PatientVisitEyeTestDto> PatientVisitEyeTests { get; set; }


        public ICommand AddEyeTestCommand { get; set; }
        public ICommand EyeTestHistoryCommand { get; set; }
        public ICommand SaveAllCommand { get; set; }
        public ICommand CancelCommand => new RelayCommand(() => {
            ContainerHandler.Container.Resolve<IDialogService>().DisposeLastDialog();
        });

        public async void AddEyeTest() {
            var visitEyeTest = BuildFromProperties();
            if (visitEyeTest == null)
                return;

            PatientVisitEyeTests ??= new ObservableCollection<PatientVisitEyeTestDto>();

            if (PatientVisitEyeTests.Any(e => e.EyeTest.Code == SelectedEyeTest.Code))
                return;

            await _patientVisitEyeTestRepository.Add(visitEyeTest);

            visitEyeTest.EyeTest = SelectedEyeTest;
            PatientVisitEyeTests.Add(visitEyeTest);
        }

        private PatientVisitEyeTestDto BuildFromProperties() {
            if (SelectedEyeTest == null)
                return null;

            return new PatientVisitEyeTestDto {
                EyeTestId = SelectedEyeTest.Id,
                PatientVisitId = PatientVisitId
            };
        }

        private void EyeTestHistory() {
            BusyExecute(async () => {
                var history = _container.Resolve<EyeTestHistoryViewModel>();
                await history.Initialize(PatientId);

                _dialogService.ShowInformationDialog(history.GetView() as EyeTestHistoryView);
            });
        }


        public Action OnSave;

        private void SaveAll() {
            var list = new List<PatientVisitEyeTestDto>();
            if (PatientVisitEyeTests.Count > 2) {
                foreach (var patientVisitEyeTestDto in PatientVisitEyeTests) {
                    if (!string.IsNullOrWhiteSpace(patientVisitEyeTestDto.LeftEyeResult) ||
                        !string.IsNullOrWhiteSpace(patientVisitEyeTestDto.RightEyeResult)) {
                        list.Add(patientVisitEyeTestDto);
                    }
                }

                if (list.All(e => e.EyeTestId != 2))
                    list.Add(new PatientVisitEyeTestDto() {
                        CreatedDate = DateTime.Now,
                        EyeTestId = 2,
                        PatientVisitId = PatientVisitId,
                    });
                if (list.All(e => e.EyeTestId != 3))
                    list.Add(new PatientVisitEyeTestDto() {
                        CreatedDate = DateTime.Now,
                        EyeTestId = 3,
                        PatientVisitId = PatientVisitId,
                    });
            } else {
                list = PatientVisitEyeTests.ToList();
            }

            BusyExecute(async () => {
                await _patientVisitEyeTestRepository
                    .Delete(e => e.PatientVisitId == PatientVisitId);
                await _patientVisitEyeTestRepository.AddRange(list.Select(
                    e => new PatientVisitEyeTestDto {
                        PatientVisitId = e.PatientVisitId,
                        CreatedDate = DateTime.Now,
                        EyeTestId = e.EyeTestId,
                        LeftEyeResult = e.LeftEyeResult,
                        RightEyeResult = e.RightEyeResult
                    }).ToList());

                OnSave?.Invoke();
            });
        }



    }
}
