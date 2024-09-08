using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.AppServices.Print;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Base.Extends;
using EyeClinic.WPF.Components.Home.Setting.Medicines.MedicineEditor;
using EyeClinic.WPF.Components.Home.Setting.MedicineUsage.MedicineUsageEditor;
using EyeClinic.WPF.Components.PatientList.PatientFile.Prescriptions.PrescriptionHistory;
using EyeClinic.WPF.Components.PatientList.PatientFile.Prescriptions.ReadyPrescription;
using Microsoft.EntityFrameworkCore;
using Unity;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.Prescriptions
{
    public partial class PrescriptionsViewModel { public PrescriptionsViewModel() { } }

    public partial class PrescriptionsViewModel : BaseViewModel<PrescriptionsView>
    {
        private readonly IUnityContainer _container;
        private readonly IOldMedicineRepository _oldMedicineRepository;
        private readonly IPatientVisitRepository _patientVisitRepository;
        private readonly IDialogService _dialogService;
        private readonly INotificationService _notificationService;
        private readonly IMedicineRepository _medicineRepository;
        private readonly IMedicineUsageRepository _medicineUsageRepository;
        private readonly IPatientVisitPrescriptionRepository _patientVisitPrescriptionRepository;
        private readonly IPrintService _printService;
        private readonly IMedicineTypeRepository _medicineTypeRepository;

        public PrescriptionsViewModel(IUnityContainer container, IOldMedicineRepository oldMedicineRepository,
            IPatientVisitRepository patientVisitRepository,
            IDialogService dialogService,
            INotificationService notificationService,
            IMedicineRepository medicineRepository, IMedicineUsageRepository medicineUsageRepository,
            IPatientVisitPrescriptionRepository patientVisitPrescriptionRepository, IPrintService printService,
            IMedicineTypeRepository medicineTypeRepository) {
            _container = container;
            _oldMedicineRepository = oldMedicineRepository;
            _patientVisitRepository = patientVisitRepository;
            _dialogService = dialogService;
            _notificationService = notificationService;
            _medicineRepository = medicineRepository;
            _medicineUsageRepository = medicineUsageRepository;
            _patientVisitPrescriptionRepository = patientVisitPrescriptionRepository;
            _printService = printService;
            _medicineTypeRepository = medicineTypeRepository;

            PrintPrescriptionCommand = new RelayCommand(PrintPrescription);
            DeletePrescriptionCommand = new RelayCommand(DeletePrescription);
            DeletePrescriptionItemCommand = new RelayCommand<OldMedicineViewTableDto>(DeletePrescriptionItem);
            PrescriptionHistoryCommand = new RelayCommand(PrescriptionHistory);

            PickReadyPrescriptionCommand = new RelayCommand(PickReadyPrescription);
            AddNewMedicineCommand = new RelayCommand(AddNewMedicine);
            AddMedicineUsageCommand = new RelayCommand(AddMedicineUsage);
            ChangeMedicineTypeCommand = new RelayCommand<MedicineTypeDto>(type => SelectedMedicineType = type);

            OnReadyPrescriptionSelected += ReadyPrescriptionSelected;

            SearchCommand = new RelayCommand(FilterMedicinesByName);
            SearchUsageCommand = new RelayCommand(FilterUsageByName);
        }


        public override Task Initialize() {
            throw new Exception("Use Initialize with param");
        }

        private int rowIndex = 0;
        public async Task Initialize(int patientVisitId) {
            PatientVisitId = patientVisitId;
            SelectedVisit = await _patientVisitRepository.GetById(patientVisitId);
            PatientId = SelectedVisit.PatientId;


            BaseMedicines = (await _medicineRepository.GetAll()).OrderBy(e => e.MedicineName).ToList();
            BaseUsages = await _medicineUsageRepository.GetAll();
            MedicineTypes = await _medicineTypeRepository.GetAll();

            SelectedMedicineType = MedicineTypes[0];

            PrescriptionItems = new ObservableCollection<OldMedicineViewTableDto>(
                await _oldMedicineRepository
                    .GetByPatientVisitId(patientVisitId));
             rowIndex = 0;

            foreach (var prescriptionItem in PrescriptionItems) {
                if (!string.IsNullOrWhiteSpace(prescriptionItem.MedicineType)) {
                    rowIndex++;
                }
                prescriptionItem.RowIndex = rowIndex;
            }

            var prescriptionList = (await _patientVisitPrescriptionRepository
                    .GetByKey(e => e.PatientVisitId == patientVisitId))
                .OrderBy(e => e.RowIndex);

            foreach (var item in prescriptionList) {
                rowIndex++;
                
                PrescriptionItems.Add(new OldMedicineViewTableDto {
                    MedicineName = item.Medicine.MedicineName,
                    MedicineType = item.Medicine.MedicineType.EnName,
                    PatientVisitId = PatientVisitId,
                    VisitDate = SelectedVisit.VisitDate,
                    Index = PrescriptionItems.Count,
                    RowIndex = rowIndex,
                });
                PrescriptionItems.Add(new OldMedicineViewTableDto {
                    MedicineName = item.MedicineUsage.UsageName,
                    MedicineType = string.Empty,
                    PatientVisitId = PatientVisitId,
                    VisitDate = SelectedVisit.VisitDate,
                    Index = PrescriptionItems.Count
                });
            }

            CurrentIndex = rowIndex;
        }

        public string SearchText { get; set; }
        public string SearchUsageText { get; set; }

        public int PatientVisitId { get; set; }
        public PatientVisitDto SelectedVisit { get; set; }
        public int PatientId { get; set; }

        public List<MedicineTypeDto> MedicineTypes { get; set; }

        private MedicineTypeDto _selectedMedicineType;
        public MedicineTypeDto SelectedMedicineType {
            get => _selectedMedicineType;
            set {
                _selectedMedicineType = value;
                if (value != null)
                    FilterMedicinesByType(value);
            }
        }


        public List<MedicineDto> BaseMedicines { get; set; }
        public ObservableCollection<MedicineDto> AvailableMedicines { get; set; }
        public MedicineDto SelectedMedicine { get; set; }


        public List<MedicineUsageDto> BaseUsages { get; set; }
        public ObservableCollection<MedicineUsageDto> AvailableUsages { get; set; }
        public MedicineUsageDto SelectedUsage { get; set; }

        public ObservableCollection<OldMedicineViewTableDto> PrescriptionItems { get; set; }


        public ICommand PrintPrescriptionCommand { get; set; }
        public ICommand DeletePrescriptionCommand { get; set; }
        public ICommand DeletePrescriptionItemCommand { get; set; }

        public ICommand PickReadyPrescriptionCommand { get; set; }
        public ICommand PrescriptionHistoryCommand { get; set; }
        public ICommand SearchCommand { set; get; }
        public ICommand SearchUsageCommand { set; get; }

        public ICommand ChangeMedicineTypeCommand { get; set; }

        public ICommand AddNewMedicineCommand { get; set; }
        public ICommand AddMedicineUsageCommand { get; set; }

        public ICommand CancelCommand => new RelayCommand(() => {
            ContainerHandler.Container.Resolve<IDialogService>().DisposeLastDialog();
        });
        public PatientFileViewModel PatientFileViewModel { get; set; }


        private void FilterMedicinesByType(MedicineTypeDto medicineType) {
            AvailableMedicines = new ObservableCollection<MedicineDto>(
                BaseMedicines
                    .Where(e => e.MedicineTypeId == medicineType.Id)
                    .OrderBy(e => e.MedicineName)
                    .ToList());

            AvailableUsages = new ObservableCollection<MedicineUsageDto>(
                BaseUsages
                    .Where(e => e.UsageMedicineTypeId == medicineType.Id)
                    .ToList());
        }
        private void FilterMedicinesByName()
        {
            SearchText ??= "";
            AvailableMedicines = new ObservableCollection<MedicineDto>(
                BaseMedicines
                    .Where(e => e.MedicineName.ToLower().StartsWith(SearchText.ToLower()))
                    .OrderBy(e => e.MedicineName)
                    .ToList());
         
        }
        private void FilterUsageByName()
        {
            SearchUsageText ??= "";
            AvailableUsages = new ObservableCollection<MedicineUsageDto>(
             BaseUsages
                 .Where(e => e.UsageName.ToLower().StartsWith(SearchUsageText.ToLower()))
                 .ToList());

        }

        private void DeletePrescriptionItem(OldMedicineViewTableDto obj) {
            BusyExecute(async () => {
                await _oldMedicineRepository.Delete(obj.Id);
                CurrentIndex= PrescriptionItems.Count - 1;
                PrescriptionItems.Remove(obj);
                OnSave?.Invoke();
            });
        }

        private void PrintPrescription() {
            BusyExecute(async () => {
                var patient = await _container.Resolve<IPatientRepository>()
                    .GetByPatientVisitId(PatientVisitId);
                _printService.PrintPrescription(patient, PrescriptionItems.ToList());
            });
        }

        private void DeletePrescription() {
            BusyExecute(async () => {
                await _oldMedicineRepository.Delete(e => e.PatientVisitId == PatientVisitId);

                await Initialize(PatientVisitId);
                OnSave?.Invoke();
            });
        }

        private void PickReadyPrescription() {
            BusyExecute(async () => {
                var readyPrescriptions = _container.Resolve<ReadyPrescriptionViewModel>();
                await readyPrescriptions.Initialize();
                _container.Resolve<IDialogService>().ShowEditorDialog(readyPrescriptions.GetView() as ReadyPrescriptionView,
                    () => {
                        if (readyPrescriptions.SelectedPrescription == null)
                            return Task.FromResult(false);
                        OnReadyPrescriptionSelected?.Invoke(this, readyPrescriptions);
                        return Task.FromResult(false);
                    });
            });
        }

        public Action OnSave;
        public event EventHandler<ReadyPrescriptionViewModel> OnReadyPrescriptionSelected;


        public int CurrentIndex { get; set; }
        public void AddMedicine() {
            if (SelectedMedicine == null)
                return;

            BusyExecute(async () => {
                PrescriptionItems ??= new ObservableCollection<OldMedicineViewTableDto>();
                CurrentIndex++;
                var item = new OldMedicineViewTableDto {
                    PatientVisitId = PatientVisitId,
                    VisitDate = SelectedVisit.VisitDate,
                    MedicineName = SelectedMedicine.MedicineName,
                    MedicineType = SelectedMedicine.MedicineType.EnName,
                    Index = PrescriptionItems.Count,
                    RowIndex = CurrentIndex
                };

                var added = await _oldMedicineRepository.Add(item);
                item.Id = added.Id;

                PrescriptionItems.Add(item);
                OnSave?.Invoke();
            });
        }

        public void AddUsage() {
            if (SelectedUsage == null)
                return;

            BusyExecute(async () => {
                PrescriptionItems ??= new ObservableCollection<OldMedicineViewTableDto>();

                var item = new OldMedicineViewTableDto {
                    PatientVisitId = PatientVisitId,
                    VisitDate = SelectedVisit.VisitDate,
                    MedicineName = SelectedUsage.UsageName,
                    MedicineType = string.Empty,
                    Index = PrescriptionItems.Count,
                    RowIndex = CurrentIndex
                };

                var added = await _oldMedicineRepository.Add(item);
                item.Id = added.Id;

                PrescriptionItems.Add(item);
                OnSave?.Invoke();
            });
        }

        private void PrescriptionHistory() {
            BusyExecute(async () => {
                var history = _container.Resolve<PrescriptionHistoryViewModel>();
                await history.Initialize(PatientId);

                _dialogService.ShowInformationDialog(history.GetView() as PrescriptionHistoryView);
            });
        }

        private void AddNewMedicine() {
            BusyExecute(async () => {
                var locationEditor = _container.Resolve<MedicineEditorViewModel>();
                await locationEditor.Initialize();

                _dialogService.ShowEditorDialog(locationEditor.GetView() as MedicineEditorView, async () => {
                    var item = await locationEditor.Save();
                    if (item == null)
                        return false;

                    BaseMedicines ??= new List<MedicineDto>();
                    AvailableMedicines ??= new ObservableCollection<MedicineDto>();
                    AvailableMedicines.Add(item);
                    BaseMedicines.Add(item);

                    return true;
                });
            });
        }

        private void AddMedicineUsage() {
            BusyExecute(async () => {
                var locationEditor = _container.Resolve<MedicineUsageEditorViewModel>();
                await locationEditor.Initialize();

                _dialogService.ShowEditorDialog(locationEditor.GetView() as MedicineUsageEditorView, async () => {
                    var item = await locationEditor.Save();
                    if (item == null)
                        return false;


                    AvailableUsages ??= new ObservableCollection<MedicineUsageDto>();
                    BaseUsages ??= new List<MedicineUsageDto>();
                    AvailableUsages.Add(item);
                    BaseUsages.Add(item);
                    return true;
                });
            });
        }

        private void ReadyPrescriptionSelected(object sender, ReadyPrescriptionViewModel readyPrescriptions) {
            BusyExecute(() =>
            {
                // TODO: Add Medicine from ready prescription

                _container.Resolve<IDialogService>().ShowConfirmationMessage(
                    "Do you want to edit ?", () =>
                    {
                        _container.Resolve<IDialogService>().DisposeLastDialog();
                        return Task.FromResult(true);
                    }, () =>
                    {
                        PrintPrescriptionCommand?.Execute(null);
                        _container.Resolve<IDialogService>().DisposeLastDialog();
                        OnSave?.Invoke();
                    });
                return Task.CompletedTask;
            });
        }
    }
}
