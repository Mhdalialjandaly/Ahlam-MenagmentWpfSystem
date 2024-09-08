using EyeClinic.Core.Enums;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Dialogs.PasswordInput;
using EyeClinic.WPF.Components.PatientList.PatientFile.FinishVisit;
using EyeClinic.WPF.Components.PatientList.PatientFile.MedicalReport;
using EyeClinic.WPF.Components.PatientList.PatientFile.Operations.OperationList;
using EyeClinic.WPF.Components.PatientList.PatientFile.PDFMerger;
using EyeClinic.WPF.Components.PatientList.PatientFile.SpecialNote;
using EyeClinic.WPF.Components.PatientList.PatientFile;
using EyeClinic.WPF.Components.PatientList.PatientVisitList.VisitNote.VisitNoteList;
using EyeClinic.WPF.Components.PatientList.PatientVisitList.VisitNote;
using EyeClinic.WPF.Components.PatientList.PatientVisitList.Why;
using EyeClinic.WPF.Components.PatientList.PatientVisitList;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;
using EyeClinic.WPF.Setup;
using System.Windows.Controls;
using EyeClinic.WPF.Components.Home.Operation.PublicCustomer;
using EyeClinic.WPF.Components.Home.Operation.PublicCustomer.PublicCustomerOrderList;

namespace EyeClinic.WPF.Components.Home.Operation.PublicCustomerFile
{
    
        public partial class PublicCustomerFileViewModel
        {
            public PublicCustomerFileViewModel()
            {
                if (IsDesignMode) { }
            }
        }

        public partial class PublicCustomerFileViewModel : BaseViewModel<PublicCustomerFileView>
        {
            private readonly IUnityContainer _container;
            private readonly IPatientRepository _patientRepository;
            private readonly IPatientVisitRepository _patientVisitRepository;
            private readonly IDialogService _dialogService;

            public PublicCustomerFileViewModel(IUnityContainer container, IPatientRepository patientRepository,
                IPatientVisitRepository patientVisitRepository, IDialogService dialogService)
            {
                _container = container;
                _patientRepository = patientRepository;
                _patientVisitRepository = patientVisitRepository;
                _dialogService = dialogService;
                NavigateCommand = new RelayCommand<string>(Navigate);
                FinishVisitCommand = new RelayCommand(FinishVisit);
                CancelCommand = new RelayCommand(Cancel);
                GoSearchForTestCommand = new RelayCommand(GoSearchForTest);
            }

        private void Navigate(string obj)
        {
            throw new NotImplementedException();
        }

        public async Task Initialize(int publicCustomerId, int publicCustomerOrderId, bool isPopup = false)
           {
            //PublicCustomerOrderId = publicCustomerOrderId;

            //var publicCustomerOrderViewModel = _container.Resolve<PublicCustomerOrderListViewModel>();
            //await publicCustomerOrderViewModel.Initialize(publicCustomerId);
            //publicCustomerOrderViewModel.PatientVisitChanged += OnPatientVisitChanged;
            //PatientVisits = patientVisitViewModel.GetView() as PatientVisitListView;

            //SelectedPatient = await _patientRepository.GetById(patientId);
            //await HighlightNavigation(patientId, patientVisitViewModel.SelectedPatientVisit.Id);
            //_container.SetCurrentPatient(SelectedPatient, isPopup);
            //IsPopup = isPopup;
            ////Navigate(nameof(PatientService.PatientFile));
            //var GetSpecialNote = _container.Resolve<IPatientRepository>();
            //await GetSpecialNote.GetById(patientId);




            //TotalRemaining = patientVisitViewModel.PatientVisitList.Where(e => e.DeletedDate == null).Sum(e => e.Remaining);
            //var operation = _container.Resolve<OperationListViewModel>();
            //await operation.Initialize(patientId);
            //TotalRemaining += operation.Operations.Where(e => e.MedicalCenterReserved == true || e.IsFinish == true).Sum(e => e.Remaining);

            //AddPaymentCommand = new RelayCommand(() =>
            //{
            //    var payment = _container.Resolve<PaymentForRemainingViewModel>();
            //    _dialogService.ShowEditorDialog(payment.GetView() as PaymentForRemainingView,
            //        async () =>
            //        {
            //            if (payment.Payment > 0)
            //            {
            //                await _patientVisitRepository.AddPayment(patientId, payment.Payment);
            //                await _patientVisitRepository.AddPaymentForToday(patientId, payment.Payment);
            //                return true;
            //            }
            //            return false;
            //        });
            //});
            //var visits = await _patientVisitRepository
            //    .GetByKey(e => e.PatientId == patientId);
            //PatientVisitList = new ObservableCollection<PatientVisitDto>(visits
            //    .OrderByDescending(e => e.VisitDate).ToList());

            //var visitWhy = _container.Resolve<IPatientVisitRepository>();
            //await visitWhy.GetById(patientId);

         }

            public int TotalRemaining { get; set; }
            public ICommand AddPaymentCommand { get; set; }



            private void OnPatientVisitChanged(object sender, PatientVisitDto patientVisit)
            {
                SelectedVisit = patientVisit;
                OnPatientVisitChanged(patientVisit.PatientId, patientVisit.Id, true);
            }

            public void OnPatientVisitChanged(int patientId, int patientVisitId, bool navigateCurrent = false)
            {
                //BusyExecute(async () => {
                //    await HighlightNavigation(patientId, patientVisitId);
                //    IsBusy = false;
                //    if (navigateCurrent)
                //        Navigate(CurrentNavigation.ToString());
                //});
            }

            public int PublicCustomerOrderId { get; set; }
            public bool IsPopup { get; set; }

            public List<PatientService> HighLightServices { get; set; }

            private async Task HighlightNavigation(int patientId, int patientVisitId)
            {
                HighLightServices = await _patientVisitRepository
                    .GetNavigationIncludedData(patientId, patientVisitId);
            }


            public ICommand NavigateCommand { get; set; }
            public ICommand FinishVisitCommand { get; set; }
            public ICommand CancelCommand { get; set; }
            public ICommand GoSearchForTestCommand { get; set; }

            public UserControl Content { get; set; }
            public UserControl PatientVisits { get; set; }
            public PatientService? CurrentNavigation { get; set; }
            public PatientDto SelectedPatient { get; set; }
            public PatientVisitDto SelectedVisit { get; set; }
            public ObservableCollection<PatientVisitDto> PatientVisitList { get; set; }
            public bool ShowPatientVisitList { get; set; }

            //public void Navigate(string target)
            //{
            //    SelectedVisit =
            //        (PatientVisits.DataContext as PatientVisitListViewModel)?.SelectedPatientVisit;

            //    //if (/*target =="Operations" ||*/ target== "Remaining") {
            //    //    if (!_container.CheckUserRoleSilent(UserRoles.Admin) && !_container.CheckUserRoleSilent(UserRoles.Doctor)) {
            //    //        _container.Resolve<INotificationService>()
            //    //        .Warning("You have no permission");
            //    //        return;
            //    //    }
            //    //}

            //    ShowPatientVisitList = true;

            //    switch (target)
            //    {
            //        case nameof(PatientService.PatientFile):
            //            //if (CurrentNavigation == PatientService.PatientFile)
            //            //    break;
            //            BusyExecute(async () => {
            //                var patientSickStory = _container.Resolve<PatientSickStoryViewModel>();
            //                if (SelectedVisit != null)
            //                    await patientSickStory.Initialize(SelectedVisit.Id);
            //                patientSickStory.PatientFileViewModel = this;
            //                patientSickStory.OnSave += (_, specialNote) => {
            //                    SelectedPatient.Notes = specialNote;
            //                    OnPatientVisitChanged(SelectedPatient.Id, SelectedVisit.Id);

            //                    OnPropertyChanged(nameof(HighLightServices));
            //                    _container.Resolve<INotificationService>().Success("Data saved");
            //                };

            //                _dialogService.ShowFullScreenPopupContent(patientSickStory.GetView() as PatientSickStoryView);
            //                _dialogService.OnDisposeDialog += () => {
            //                    if (patientSickStory.BasePatientSpecialNote != patientSickStory.PatientSpecialNote)
            //                    {
            //                        _container.Resolve<INotificationService>().Warning("Save the special note before exit");
            //                        return false;
            //                    }
            //                    return true;
            //                };
            //            });
            //            //CurrentNavigation = PatientService.PatientFile;
            //            break;

            //        case nameof(PatientService.Operations):
            //            //if (CurrentNavigation == PatientService.Operations)
            //            //    break;

            //            BusyExecute(async () => {
            //                var operations = _container.Resolve<OperationsViewModel>();
            //                operations.PatientFileViewModel = this;
            //                if (SelectedVisit != null)
            //                    await operations.Initialize(SelectedVisit.PatientId);

            //                operations.OnSave += () => {
            //                    OnPatientVisitChanged(SelectedPatient.Id, SelectedVisit.Id);

            //                    OnPropertyChanged(nameof(HighLightServices));
            //                    _container.Resolve<INotificationService>().Success("Data saved");
            //                };
            //                ShowPatientVisitList = false;
            //                //Content = operations.GetView() as OperationsView;
            //                _dialogService.ShowFullScreenPopupContent(operations.GetView() as OperationsView);
            //            });
            //            //CurrentNavigation = PatientService.Operations;
            //            break;
            //        case nameof(PatientService.Tests):
            //            BusyExecute(async () => {
            //                var tests = _container.Resolve<TestsViewModel>();
            //                if (SelectedVisit != null)
            //                    await tests.Initialize(SelectedVisit.Id);

            //                tests.PatientFileViewModel = this;
            //                tests.OnSave += () => {
            //                    OnPatientVisitChanged(SelectedPatient.Id, SelectedVisit.Id);

            //                    OnPropertyChanged(nameof(HighLightServices));
            //                    _container.Resolve<INotificationService>().Success("Data saved");
            //                };

            //                //Content = tests.GetView() as TestsView;
            //                _dialogService.ShowFullScreenPopupContent(tests.GetView() as TestsView);
            //            });
            //            //CurrentNavigation = PatientService.Tests;
            //            break;
            //        case nameof(PatientService.LabTests):
            //            BusyExecute(async () => {
            //                var labTests = _container.Resolve<LabTestsViewModel>();
            //                if (SelectedVisit != null)
            //                    await labTests.Initialize(SelectedVisit.Id);

            //                labTests.PatientFileViewModel = this;
            //                labTests.OnSave += () => {
            //                    OnPatientVisitChanged(SelectedPatient.Id, SelectedVisit.Id);

            //                    OnPropertyChanged(nameof(HighLightServices));
            //                    _container.Resolve<INotificationService>().Success("Data saved");
            //                };
            //                //Content = labTests.GetView() as LabTestsView;
            //                _dialogService.ShowFullScreenPopupContent(labTests.GetView() as LabTestsView);
            //            });
            //            //CurrentNavigation = PatientService.LabTests;
            //            break;
            //        case nameof(PatientService.NewGlass):
            //            //BusyExecute(async () => {
            //            //    var newGlass = _container.Resolve<NewGlassViewModel>();
            //            //    if (SelectedVisit != null)
            //            //        await newGlass.Initialize(SelectedVisit.Id);

            //            //    newGlass.PatientFileViewModel = this;
            //            //    newGlass.OnSave += () => {
            //            //        OnPatientVisitChanged(SelectedPatient.Id, SelectedVisit.Id);

            //            //        OnPropertyChanged(nameof(HighLightServices));
            //            //        _container.Resolve<INotificationService>().Success("Data saved");
            //            //    };
            //            //    //Content = newGlass.GetView() as NewGlassView;
            //            //    _dialogService.ShowFullScreenPopupContent(newGlass.GetView() as NewGlassView);
            //            //});
            //            //CurrentNavigation = PatientService.NewGlass;
            //            PDFMergerWindow pm = new PDFMergerWindow();
            //            pm.ShowDialog();
            //            break;
            //        case nameof(PatientService.Prescriptions):
            //            BusyExecute(async () => {
            //                var prescriptions = _container.Resolve<PrescriptionsViewModel>();
            //                if (SelectedVisit != null)
            //                    await prescriptions.Initialize(SelectedVisit.Id);

            //                prescriptions.PatientFileViewModel = this;
            //                prescriptions.OnSave += () => {
            //                    OnPatientVisitChanged(SelectedPatient.Id, SelectedVisit.Id);

            //                    OnPropertyChanged(nameof(HighLightServices));
            //                    _container.Resolve<INotificationService>().Success("Data saved");
            //                };
            //                //Content = prescriptions.GetView() as PrescriptionsView;
            //                _dialogService.ShowFullScreenPopupContent(prescriptions.GetView() as PrescriptionsView);
            //            });
            //            //CurrentNavigation = PatientService.Prescriptions;
            //            break;
            //        case nameof(PatientService.EyeTests):
            //            BusyExecute(async () => {
            //                var eyeTests = _container.Resolve<EyeTestsViewModel>();
            //                if (SelectedVisit != null)
            //                    await eyeTests.Initialize(SelectedVisit.Id);

            //                eyeTests.PatientFileViewModel = this;
            //                eyeTests.OnSave += () => {
            //                    OnPatientVisitChanged(SelectedPatient.Id, SelectedVisit.Id);

            //                    OnPropertyChanged(nameof(HighLightServices));
            //                    _container.Resolve<INotificationService>().Success("Data saved");
            //                };
            //                //Content = eyeTests.GetView() as EyeTestsView;
            //                _dialogService.ShowFullScreenPopupContent(eyeTests.GetView() as EyeTestsView);
            //            });
            //            //CurrentNavigation = PatientService.EyeTests;
            //            break;
            //        case nameof(PatientService.Diagnosis):
            //            BusyExecute(async () => {
            //                var diagnosis = _container.Resolve<EyeDiagnosisViewModel>();
            //                if (SelectedVisit != null)
            //                    await diagnosis.Initialize(SelectedVisit.Id);

            //                diagnosis.PatientFileViewModel = this;
            //                diagnosis.OnSave += () => {
            //                    OnPatientVisitChanged(SelectedPatient.Id, SelectedVisit.Id);

            //                    OnPropertyChanged(nameof(HighLightServices));
            //                    _container.Resolve<INotificationService>().Success("Data saved");
            //                };
            //                //Content = diagnosis.GetView() as EyeDiagnosisView;
            //                _dialogService.ShowFullScreenPopupContent(diagnosis.GetView() as EyeDiagnosisView);
            //            });
            //            //CurrentNavigation = PatientService.Diagnosis;
            //            break;
            //        case nameof(PatientService.MedicalReports):
            //            var medicalReport = _container.Resolve<MedicalReportViewModel>();
            //            medicalReport.Initialize(SelectedVisit, SelectedPatient);
            //            medicalReport.OnSave += () => {
            //                OnPatientVisitChanged(SelectedPatient.Id, SelectedVisit.Id);

            //                OnPropertyChanged(nameof(HighLightServices));
            //                _container.Resolve<INotificationService>().Success("Data saved");
            //            };
            //            //Content = medicalReport.GetView() as MedicalReportView;
            //            _dialogService.ShowFullScreenPopupContent(medicalReport.GetView() as MedicalReportView);

            //            //CurrentNavigation = PatientService.MedicalReports;
            //            break;
            //        case nameof(PatientService.SpecialNote):
            //            var specialNote = _container.Resolve<SpecialNoteViewModel>();
            //            specialNote.Initialize(SelectedPatient.Id, SelectedPatient.Notes);
            //            specialNote.OnSave += (_, note) => {
            //                SelectedPatient.Notes = note;
            //                OnPatientVisitChanged(SelectedPatient.Id, SelectedVisit.Id);

            //                OnPropertyChanged(nameof(HighLightServices));
            //                _container.Resolve<INotificationService>().Success("Data saved");
            //            };

            //            _dialogService.ShowFullScreenPopupContent(specialNote.GetView() as SpecialNoteView);
            //            break;

            //        case nameof(PatientService.Remaining):
            //            BusyExecute(async () => {
            //                var Remaining = _container.Resolve<RemainingViewModel>();
            //                var operation = _container.Resolve<OperationListViewModel>();

            //                await operation.Initialize(SelectedPatient.Id);
            //                await Remaining.InitializeAsync(
            //                    SelectedPatient.Id,
            //                    SelectedPatient.Remaining,
            //                    operation.Operations.Where(e => e.MedicalCenterReserved == true || e.IsFinish == true).Sum(e => e.Remaining)
            //                    , SelectedVisit.Id);

            //                Remaining.OnAddPayment += async (_, remaining) => {
            //                    if (PatientVisits.DataContext is PatientVisitListViewModel patientvl)
            //                    {
            //                        SelectedPatient = await _patientRepository.GetById(SelectedPatient.Id);
            //                        patientvl.PatientVisitList.Clear();
            //                        await patientvl.Initialize(SelectedPatient.Id);
            //                    }
            //                };

            //                _dialogService.ShowFullScreenPopupContent(Remaining.GetView() as RemainingView);
            //            });
            //            break;

            //        case nameof(PatientService.VisitNote):
            //            var VisitNote = _container.Resolve<VisitNoteViewModel>();
            //            VisitNote.Initialize(SelectedVisit.Id, SelectedPatient.Notes, SelectedVisit.PdfPath);
            //            VisitNote.OnSave += (_, note, pdfPath) =>
            //            {
            //                SelectedPatient.Notes = (string)note;
            //                OnPatientVisitChanged(SelectedPatient.Id, SelectedVisit.Id);
            //                OnPropertyChanged(nameof(HighLightServices));
            //            };
            //            var VisitNote1 = _container.Resolve<VisitNoteViewModel>();
            //            VisitNote1.Initialize(SelectedVisit.Id, SelectedVisit.Notes, SelectedVisit.PdfPath);

            //            //_dialogService.ShowFullScreenPopupContent(CouseOfVisit.GetView() as VisitWhyView);
            //            _dialogService.ShowEditorDialog(VisitNote1.GetView() as VisitNoteView,
            //              async () => {
            //                  if (await VisitNote1.Save())
            //                  {
            //                      _container.Resolve<INotificationService>().Success("Data saved .......!");
            //                      SelectedVisit.Notes = VisitNote1.Note;
            //                      await Initialize(SelectedPatient.Id, SelectedVisit.Id, false);
            //                      return true;
            //                  }
            //                  return false;
            //              });
            //            break;

            //        case nameof(PatientService.ShowAllVisitsNotes):
            //            var visitList = _container.Resolve<VisitNoteListViewModel>();
            //            visitList.Initialize(PatientVisitList
            //                .Where(e => !string.IsNullOrWhiteSpace(e.Notes))
            //                .ToList());

            //            _dialogService.ShowFullScreenPopupContent(visitList.GetView() as VisitNoteListView);
            //            break;

            //        case nameof(PatientService.CouseOfVisit):
            //            var CouseOfVisit = _container.Resolve<VisitWhyViewModel>();
            //            CouseOfVisit.Initialize(SelectedPatient.Id, SelectedPatient.Why);
            //            CouseOfVisit.OnSave += (_, why) =>
            //            {
            //                SelectedPatient.Why = (string)why;
            //                OnPatientVisitChanged(SelectedPatient.Id, SelectedVisit.Id);
            //                OnPropertyChanged(nameof(HighLightServices));
            //            };
            //            var visitWhy = _container.Resolve<VisitWhyViewModel>();
            //            visitWhy.Initialize(SelectedVisit.Id, SelectedVisit.Why);

            //            //_dialogService.ShowFullScreenPopupContent(CouseOfVisit.GetView() as VisitWhyView);
            //            _dialogService.ShowEditorDialog(visitWhy.GetView() as VisitWhyView,
            //              async () => {
            //                  if (await visitWhy.Save())
            //                  {
            //                      _container.Resolve<INotificationService>().Success("Data saved .......!");
            //                      SelectedVisit.Notes = visitWhy.Why;
            //                      await Initialize(SelectedPatient.Id, SelectedVisit.Id, false);
            //                      return true;
            //                  }
            //                  return false;
            //              });
            //            break;


            //    }
            //}

            public void FinishVisit()
            {
                //if (!_container.CheckUserRoleSilent(UserRoles.Seller, UserRoles.Admin, UserRoles.AdministrativeAssistant))
                //    return;

                //if (SelectedVisit == null)
                //    SelectedVisit = (PatientVisits?.DataContext as PatientVisitListViewModel)?
                //        .PatientVisitList.OrderByDescending(e => e.VisitDate)
                //        .FirstOrDefault();

                //BusyExecute(async () => {

                //    SelectedVisit =
                //    (PatientVisits.DataContext as PatientVisitListViewModel)?.SelectedPatientVisit;
                //    var eyeTests = _container.Resolve<EyeTestsViewModel>();
                //    if (SelectedVisit != null)
                //        await eyeTests.Initialize(SelectedVisit.Id);

                //    var newGlass = _container.Resolve<NewGlassViewModel>();
                //    if (SelectedVisit != null)
                //        await newGlass.Initialize(SelectedVisit.Id);

                //    var visitWhy = _container.Resolve<VisitWhyViewModel>();
                //    visitWhy.Initialize(SelectedVisit.Id, SelectedVisit.Why);

                //    //if (string.IsNullOrEmpty(visitWhy.Why))
                //    //{
                //    //    //_dialogService.ShowFullScreenPopupContent(CouseOfVisit.GetView() as VisitWhyView);
                //    //    _dialogService.ShowEditorDialog(visitWhy.GetView() as VisitWhyView,
                //    //      async () => {
                //    //          if (await visitWhy.Save())
                //    //          {
                //    //              SelectedVisit.Notes = visitWhy.Why;
                //    //              await Initialize(SelectedPatient.Id, SelectedVisit.Id, false);
                //    //              return true;
                //    //          }
                //    //          return false;
                //    //      });
                //    //    return;
                //    //}

                //    var cva = eyeTests.PatientVisitEyeTests
                //        .FirstOrDefault(e => e.EyeTest.EnName.ToLower().Contains("cva"));

                //    //if (cva != null) {
                //    //    if (string.IsNullOrWhiteSpace(cva.LeftEyeResult) &&
                //    //        string.IsNullOrWhiteSpace(cva.RightEyeResult) &&
                //    //        string.IsNullOrWhiteSpace(newGlass.RVa) && string.IsNullOrWhiteSpace(newGlass.RVa2) &&
                //    //        string.IsNullOrWhiteSpace(newGlass.LVa) && string.IsNullOrWhiteSpace(newGlass.LVa2)) {
                //    //        _dialogService.ShowConfirmationMessage(
                //    //            "No CVA or U-CVA inserted for the patient, Do you want to continue ?",
                //    //            async () => {
                //    //                await BackToClinic();
                //    //                _container.SetCurrentPatient(null);
                //    //                return true;
                //    //            });
                //    //        return;
                //    //    }
                //    //}               

                //    await BackToClinic();
                //    _container.SetCurrentPatient(null);


                //});
            }

            private async Task BackToClinic()
            {
                var home = _container.Resolve<HomeViewModel>();
                var finishVisitControl = _container.Resolve<FinishVisitViewModel>();
                finishVisitControl.Initialize(SelectedVisit.Id);
                await finishVisitControl.Save();

                await home.Initialize();

                home.NavigateCommand?.Execute("Clinic");

                if (_dialogService.IsPopupActivated())
                    _dialogService.DisposeLastDialog();
            }

            private void GoSearchForTest()
            {
                //BusyExecute(async () => {
                //    var eyeImages = _container.Resolve<EyeImagesViewModel>();
                //    await eyeImages.Initialize();
                //    _dialogService.ShowFullScreenPopupContent(eyeImages.GetView() as EyeImagesView);
                //});
            }
            private void Cancel()
            {
                _container.SetCurrentPatient(null);
                if (_dialogService.IsPopupActivated())
                {
                    _dialogService.DisposeLastDialog();
                    return;
                }

                BusyExecute(async () => {
                    var clinic = _container.Resolve<PublicCustomerViewModel>();
                    await clinic.Initialize();
                    _container.Navigate(clinic.GetView() as PublicCustomerView);
                });
            }
        }
    
}
