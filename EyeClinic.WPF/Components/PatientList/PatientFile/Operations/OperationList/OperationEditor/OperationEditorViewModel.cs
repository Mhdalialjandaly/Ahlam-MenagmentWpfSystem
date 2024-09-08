using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Home.Setting.MedicalCenters.MedicalCenterEditor;
using EyeClinic.WPF.Components.PatientList.PatientFile.Operations.OperationList.OperationEditor.OperationSelector;
using Syncfusion.UI.Xaml.Grid;
using Unity;
using Operation = EyeClinic.Core.Enums.Operation;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.Operations.OperationList.OperationEditor
{
    public class OperationEditorViewModel : BaseViewModel<OperationEditorView>
    {
        private readonly IPatientOperationRepository _patientOperationRepository;
        private readonly IOperationRepository _operationRepository;
        private readonly IMedicalCenterRepository _medicalCenterRepository;
        private readonly IUnityContainer _container;
        private readonly IDialogService _dialogService;

        public OperationEditorViewModel(IPatientOperationRepository patientOperationRepository,
            IOperationRepository operationRepository, IMedicalCenterRepository medicalCenterRepository,
            IUnityContainer container, IDialogService dialogService) {
            _patientOperationRepository = patientOperationRepository;
            _operationRepository = operationRepository;
            _medicalCenterRepository = medicalCenterRepository;
            _container = container;
            _dialogService = dialogService;

            ReceptionWindowCommand = new RelayCommand(ReceptionWindow);
            AddMedicalCenterCommand = new RelayCommand(AddMedicalCenter);
            OperationDate = DateTime.Now.Date;

            AddLeftEyeOperationCommand = new RelayCommand(AddLeftEyeOperation);
            AddRightEyeOperationCommand = new RelayCommand(AddRightEyeOperation);
            //AddNewOperationNameCommand = new RelayCommand(AddNewOperationName);
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += (_, _) => {
               
                Revenue = DownPayment - CenterCost - ClinicCost;

                if (LeftEyeOperation != null) {
                    IsSurgicalOperation = LeftEyeOperation.IsSergical;
                }
                if (RightEyeOperation != null) {
                    IsSurgicalOperation = RightEyeOperation.IsSergical;
                }

                if (LeftEyeOperation == null && RightEyeOperation == null) {
                    IsSurgicalOperation = false;
                }
            };
            timer.Start();
        }

        //private void AddNewOperationName()
        //{         
        //        BusyExecute(async () => {
        //            var editor = _container.Resolve<OperationEditorViewModel>();
        //            await editor.Initialize();
        //            editor.Operation = Operation.Add;

        //            _container.Resolve<IDialogService>()
        //                .ShowEditorDialog(editor.GetView() as OperationEditorView, async () => {
        //                    var item = await editor.Save();
        //                    if (item == null)
        //                        return false;

        //                    OperationNameList.Add(item);
        //                    return true;
        //                });
        //        });            
        //}
        


        public bool IsSurgicalOperation { get; set; }

        private void AddRightEyeOperation() {
            BusyExecute(async () => {
                var modal = _container.Resolve<OperationSelectorViewModel>();
                await modal.Initialize();
                _dialogService.ShowEditorDialog(modal.GetView() as OperationSelectorView,
                    () => {
                        if (modal.RightEyeOperation == null)
                            return Task.FromResult(false);

                        RightEyeOperation = Operations.FirstOrDefault(e => e.Id == modal.RightEyeOperation.Id);
                        RightEyeLens = modal.RightEyeLens;
                        RightEyeLensType = modal.RightEyeLensType;
                        RightEyeNote = modal.RightEyeNote;

                        return Task.FromResult(true);
                    });
            });
        }

        private void AddLeftEyeOperation() {
            BusyExecute(async () => {
                var modal = _container.Resolve<OperationSelectorViewModel>();
                await modal.Initialize();
                _dialogService.ShowEditorDialog(modal.GetView() as OperationSelectorView,
                    () => {
                        if (modal.RightEyeOperation == null)
                            return Task.FromResult(false);

                        LeftEyeOperation = Operations.FirstOrDefault(e => e.Id == modal.RightEyeOperation.Id);
                        LeftEyeLens = modal.RightEyeLens;
                        LeftEyeLensType = modal.RightEyeLensType;
                        LeftEyeNote = modal.RightEyeNote;

                        return Task.FromResult(true);
                    });
            });
        }

        public async Task Initialize(int patientId) {
            PatientId = patientId;

            Operations = await _operationRepository.GetAll();
            MedicalCenters = new ObservableCollection<MedicalCenterDto>(
                await _medicalCenterRepository.GetAll());

           

            PhotoSources = new List<string>()
            {
                "In The Clinic",
                "With The patient",
                ""
            };
            PaymentLocations = new List<string>()
            {
                "In The Clinic",
                "In The Medical Center"
            };

            GetView();
            IsSurgicalOperation = (LeftEyeOperation?.IsSergical ?? false) && (RightEyeOperation?.IsSergical ?? false);

            await base.Initialize();
        }

        public List<Model.OperationDto> Operations { get; set; }
        public Model.OperationDto LeftEyeOperation { get; set; }
        public Model.OperationDto RightEyeOperation { get; set; }

        public ObservableCollection<Model.OperationDto> OperationNameList { get; set; }
        public ObservableCollection<MedicalCenterDto> MedicalCenters { get; set; }
        public MedicalCenterDto SelectedMedicalCenter { get; set; }


        public List<string> PhotoSources { get; set; }
        public string SelectedPhotoSource { get; set; }

        public List<string> PaymentLocations { get; set; }
        public string SelectedPaymentLocation { get; set; }


        public int Id { get; set; }
        public int PatientId { get; set; }
        public DateTime OperationDate { get; set; }
        public int? LeftEyeOperationId { get; set; }
        public int? RightEyeOperationId { get; set; }
        public int MedicalCenterId { get; set; }
        public bool MedicalCenterReserved { get; set; }
        public int TotalSessions { get; set; }
        public string PhotoSource { get; set; }
        public string PaymentLocation { get; set; }
        public int TotalCost { get; set; }
        public int CenterCost { get; set; }
        public int DownPayment { get; set; }
        public int Revenue { get; set; }
        public int ClinicCost { get; set; }
        public string LeftEyeNote { get; set; }
        public string RightEyeNote { get; set; }
        public string LeftEyeLens { get; set; }
        public string LeftEyeLensType { get; set; }
        public string RightEyeLens { get; set; }
        public string RightEyeLensType { get; set; }
        public string Report { get; set; }
        public bool IsFinish { get; set; }
        public DateTime CreatedDate { get; set; }

        public ICommand AddMedicalCenterCommand { get; set; }
        public ICommand ReceptionWindowCommand { set; get; }
        public ICommand AddLeftEyeOperationCommand { get; set; }
        public ICommand AddRightEyeOperationCommand { get; set; }

        public ICommand AddNewOperationNameCommand { set; get; }
        private bool ValidForSave() {
            if (RightEyeOperation == null && LeftEyeOperation == null) {
                _container.Resolve<INotificationService>()
                    .Error("You have to choose the operation type");
                return false;
            }

            if (SelectedMedicalCenter == null) {
                _container.Resolve<INotificationService>()
                    .Error("You have to choose the medical center");
                return false;
            }

            return true;
        }

        public async Task<PatientOperationDto> SaveAsync() {
            if (!ValidForSave())
                return null;

            var patientOperation = BuildFromProperties();

            if (Operation == Operation.Add) {
                var operationItem = await _patientOperationRepository
                    .Add(patientOperation);

                operationItem.MedicalCenter = SelectedMedicalCenter;
                operationItem.LeftEyeOperation = LeftEyeOperation;
                operationItem.RightEyeOperation = RightEyeOperation;
                return operationItem;
            }

            if (Operation == Operation.Edit && Id > 0) {
                patientOperation.LastModifiedDate = DateTime.Now;
                await _patientOperationRepository.Update(patientOperation);

                patientOperation.MedicalCenter = SelectedMedicalCenter;
                patientOperation.LeftEyeOperation = LeftEyeOperation;
                patientOperation.RightEyeOperation = RightEyeOperation;
                return patientOperation;
            }

            return null;
        }

        private PatientOperationDto BuildFromProperties() =>
            new() {
                Id = Id,
                PatientId = PatientId,
                OperationDate = OperationDate,
                LeftEyeOperationId = LeftEyeOperation?.Id,
                RightEyeOperationId = RightEyeOperation?.Id,
                MedicalCenterId = SelectedMedicalCenter.Id,
                MedicalCenterReserved = MedicalCenterReserved,
                TotalSessions = TotalSessions,
                PhotoSource = SelectedPhotoSource,
                PaymentLocation = SelectedPaymentLocation,
                TotalCost = TotalCost,
                ClinicCost = ClinicCost,
                CenterCost = CenterCost,
                DownPayment = DownPayment,
                LeftEyeNote = LeftEyeNote,
                RightEyeNote = RightEyeNote,
                LeftEyeLens = LeftEyeLens,
                LeftEyeLensType = LeftEyeLensType,
                RightEyeLens = RightEyeLens,
                RightEyeLensType = RightEyeLensType,
                Revenue = Revenue,
                Report = Report,
                IsFinish = IsFinish,
                CreatedDate = CreatedDate,
            };

        public void BuildFromModel(PatientOperationDto patientOperation) {
            Id = patientOperation.Id;
            PatientId = patientOperation.PatientId;
            OperationDate = patientOperation.OperationDate;
            LeftEyeOperationId = patientOperation.LeftEyeOperationId;
            RightEyeOperationId = patientOperation.RightEyeOperationId;
            MedicalCenterId = patientOperation.MedicalCenterId;
            MedicalCenterReserved = patientOperation.MedicalCenterReserved;
            TotalSessions = patientOperation.TotalSessions;

            SelectedPhotoSource = PhotoSources.FirstOrDefault(e => e == patientOperation.PhotoSource);
            PhotoSource = SelectedPhotoSource;

            SelectedPaymentLocation = PaymentLocations.FirstOrDefault(e => e == patientOperation.PaymentLocation);
            PaymentLocation = SelectedPaymentLocation;

            TotalCost = patientOperation.TotalCost;
            ClinicCost = patientOperation.ClinicCost;
            DownPayment = patientOperation.DownPayment ?? 0;
            Revenue = patientOperation.Revenue;
            CenterCost = patientOperation.CenterCost;
            LeftEyeNote = patientOperation.LeftEyeNote;
            RightEyeNote = patientOperation.RightEyeNote;
            LeftEyeLens = patientOperation.LeftEyeLens;
            LeftEyeLensType = patientOperation.LeftEyeLensType;
            RightEyeLens = patientOperation.RightEyeLens;
            RightEyeLensType = patientOperation.RightEyeLensType;
            Report = patientOperation.Report;
            IsFinish = patientOperation.IsFinish;
            CreatedDate = patientOperation.CreatedDate;

            LeftEyeOperation = Operations.FirstOrDefault(e => e.Id == LeftEyeOperationId);
            RightEyeOperation = Operations.FirstOrDefault(e => e.Id == RightEyeOperationId);
            SelectedMedicalCenter = MedicalCenters.FirstOrDefault(e => e.Id == MedicalCenterId);
        }

        private void AddMedicalCenter() {
            BusyExecute(async () => {
                var locationEditor = _container.Resolve<MedicalCenterEditorViewModel>();
                await locationEditor.Initialize();

                _dialogService.ShowEditorDialog(locationEditor.GetView() as MedicalCenterEditorView, async () => {
                    var item = await locationEditor.Save();
                    if (item == null)
                        return false;

                    MedicalCenters ??= new ObservableCollection<MedicalCenterDto>();
                    MedicalCenters.Add(item);
                    return true;
                });
            });
        }

        private void ReceptionWindow() {

        }

    }
}
