using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.AppServices.Print;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Home.Setting.Medicines.MedicineEditor;
using EyeClinic.WPF.Components.Home.Setting.MedicineUsage.MedicineUsageEditor;
using EyeClinic.WPF.Components.PatientList.PatientFile.Prescriptions.PrescriptionHistory;
using EyeClinic.WPF.Components.PatientList.PatientFile.Prescriptions.ReadyPrescription;
using Microsoft.EntityFrameworkCore;
using Unity;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.Prescriptions
{
    public partial class TempPrescriptionsViewModel
    {
        public TempPrescriptionsViewModel() {
            if (IsDesignMode) { }
        }
    }

    public partial class TempPrescriptionsViewModel : BaseViewModel<PrescriptionsView>
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

        public TempPrescriptionsViewModel(IUnityContainer container, IOldMedicineRepository oldMedicineRepository,
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
            AddMedicineCommand = new RelayCommand(AddMedicine);
            DeletePrescriptionItemCommand = new RelayCommand<PatientVisitPrescription>(DeletePrescriptionItem);
            PrescriptionHistoryCommand = new RelayCommand(PrescriptionHistory);

            PickReadyPrescriptionCommand = new RelayCommand(PickReadyPrescription);
            AddNewMedicineCommand = new RelayCommand(AddNewMedicine);
            AddMedicineUsageCommand = new RelayCommand(AddMedicineUsage);
            ChangeMedicineTypeCommand = new RelayCommand<MedicineType>(type => SelectedMedicineType = type);

            OnReadyPrescriptionSelected += ReadyPrescriptionSelected;
        }

        private void ReadyPrescriptionSelected(object sender, ReadyPrescriptionViewModel readyPrescriptions) {
            BusyExecute(async () => {
                var items = readyPrescriptions.SelectedPrescription.ReadyPrescriptionMedicines
                    .Select((e, i) => new PatientVisitPrescription {
                        PatientVisitId = PatientVisitId,
                        MedicineId = e.MedicineId,
                        Medicine = e.Medicine,
                        MedicineUsage = e.MedicineUsage,
                        MedicineUsageId = e.MedicineUsageId,
                        RowIndex = i
                    }).ToList();
                await _patientVisitPrescriptionRepository.AddOrUpdate(items);
                PrescriptionItems = new ObservableCollection<PatientVisitPrescription>(items);

                _container.Resolve<IDialogService>().ShowConfirmationMessage(
                    "Do you want to edit ?", () => {
                        _container.Resolve<IDialogService>().DisposeLastDialog();
                        return Task.FromResult(true);
                    }, () => {
                        PrintPrescriptionCommand?.Execute(null);
                        _container.Resolve<IDialogService>().DisposeLastDialog();
                        OnSave?.Invoke();
                    });
            });
        }

        public override Task Initialize() {
            throw new Exception("Use Initialize with param");
        }

        public async Task Initialize(int patientVisitId) {
            PatientVisitId = patientVisitId;

            BaseMedicines = (await _medicineRepository.GetAll()).OrderBy(e => e.MedicineName).ToList();
            BaseUsages = await _medicineUsageRepository.GetAll();
            MedicineTypes = await _medicineTypeRepository.GetAll();

            SelectedMedicineType = MedicineTypes[0];

            PrescriptionItems = new ObservableCollection<PatientVisitPrescription>();

            var oldPrescriptions = await _oldMedicineRepository.Get()
                .AsNoTracking()
                .Include(e => e.PatientVisit)
                .Where(e => e.PatientVisit.Id == patientVisitId)
                .OrderByDescending(e => e.VisitDate)
                .ToListAsync();

            var presc = new PatientVisitPrescription();
            for (int i = 0; i < oldPrescriptions.Count; i++) {
                if (i % 2 == 0) {
                    // Medicine Name
                    presc = new PatientVisitPrescription {
                        Id = oldPrescriptions[i].Id,
                        Medicine = new() {
                            MedicineName = oldPrescriptions[i].MedicineName,
                            MedicineType = new() { EnName = oldPrescriptions[i].MedicineType }
                        },
                        PatientVisit = new() {
                            Id = oldPrescriptions[i].PatientVisit.Id,
                            VisitDate = oldPrescriptions[i].VisitDate,
                        },
                        PatientVisitId = oldPrescriptions[i].PatientVisitId
                    };
                } else {
                    // Usage
                    presc.MedicineUsage = new() { Id = oldPrescriptions[i].Id, UsageName = oldPrescriptions[i].MedicineName };
                    presc.IsOldMedicine = true;
                    presc.RowIndex = PrescriptionItems.Count;
                    PrescriptionItems.Add(presc);
                }
            }

            var prescriptionList = (await _patientVisitPrescriptionRepository
                    .GetByKey(e => e.PatientVisitId == patientVisitId))
                .OrderBy(e => e.RowIndex);
            foreach (var item in prescriptionList) {
                PrescriptionItems.Add(item);
            }

            var selectedVisit = await _patientVisitRepository.Get().AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == patientVisitId);

            if (selectedVisit != null)
                PatientId = selectedVisit.PatientId;

            MedicineSearchText = "";
            MedicineCode = "";
            UsageSearchText = "";
        }


        public int PatientVisitId { get; set; }
        public int PatientId { get; set; }

        public List<MedicineType> MedicineTypes { get; set; }

        private MedicineType _selectedMedicineType;
        public MedicineType SelectedMedicineType {
            get => _selectedMedicineType;
            set {
                _selectedMedicineType = value;
                if (value != null)
                    FilterMedicinesByType(value);
            }
        }

        private string _medicineCode;
        public string MedicineCode {
            get => _medicineCode;
            set {
                _medicineCode = value;
                //SearchMedicineByCode(value);
            }
        }

        private string _medicineSearchText;
        public string MedicineSearchText {
            get => _medicineSearchText;
            set {
                _medicineSearchText = value;
                //SearchMedicineByName(value);
            }
        }


        public List<Medicine> BaseMedicines { get; set; }
        public ObservableCollection<Medicine> AvailableMedicines { get; set; }
        public Medicine SelectedMedicine { get; set; }


        private string _usageSearchText;
        public string UsageSearchText {
            get => _usageSearchText;
            set {
                _usageSearchText = value;
                //SearchUsage(value);
            }
        }


        public List<MedicineUsage> BaseUsages { get; set; }
        public ObservableCollection<MedicineUsage> AvailableUsages { get; set; }
        public MedicineUsage SelectedUsage { get; set; }

        public ObservableCollection<PatientVisitPrescription> PrescriptionItems { get; set; }


        public ICommand PrintPrescriptionCommand { get; set; }
        public ICommand DeletePrescriptionCommand { get; set; }
        public ICommand DeletePrescriptionItemCommand { get; set; }
        public ICommand AddMedicineCommand { get; set; }
        public ICommand PickReadyPrescriptionCommand { get; set; }
        public ICommand PrescriptionHistoryCommand { get; set; }

        public ICommand ChangeMedicineTypeCommand { get; set; }

        public ICommand AddNewMedicineCommand { get; set; }
        public ICommand AddMedicineUsageCommand { get; set; }


        private PatientVisitPrescription BuildFromProperties() {
            return new() {
                PatientVisitId = PatientVisitId,
                MedicineId = SelectedMedicine.Id,
                MedicineUsageId = SelectedUsage.Id
            };
        }

        private void FilterMedicinesByType(MedicineType medicineType) {
            AvailableMedicines = new ObservableCollection<Medicine>(
                BaseMedicines
                    .Where(e => e.MedicineTypeId == medicineType.Id)
                    .OrderBy(e => e.MedicineName)
                    .ToList());

            AvailableUsages = new ObservableCollection<MedicineUsage>(
                BaseUsages
                    .Where(e => e.UsageMedicineTypeId == medicineType.Id)
                    .ToList());
        }


        private void DeletePrescriptionItem(PatientVisitPrescription obj) {
            BusyExecute(async () => {
                if (obj.IsOldMedicine) {
                    await _oldMedicineRepository.Delete(obj.Id);
                    await _oldMedicineRepository.Delete(obj.MedicineUsage.Id);
                } else
                    await _patientVisitPrescriptionRepository.Delete(obj.Id);

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
                await _patientVisitPrescriptionRepository.Delete(e => e.PatientVisitId == PatientVisitId);

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

        private void AddMedicine() {
            if (!IsValidForSave())
                return;

            BusyExecute(async () => {
                PrescriptionItems ??= new ObservableCollection<PatientVisitPrescription>();
                var item = BuildFromProperties();
                item.RowIndex = PrescriptionItems.Count;
                var addedItem = await _patientVisitPrescriptionRepository.Add(item);

                addedItem.Medicine = SelectedMedicine;
                addedItem.MedicineUsage = SelectedUsage;
                PrescriptionItems.Add(addedItem);
                MedicineSearchText = "";
                MedicineCode = "";
                UsageSearchText = "";

                OnSave?.Invoke();
            });
        }

        private bool IsValidForSave() {
            if (SelectedMedicine == null) {
                _notificationService.Warning("You have to pick medicine first");
                return false;
            }

            if (SelectedUsage == null) {
                _notificationService.Warning("You have to pick medicine usage first");
                return false;
            }

            return true;
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

                    BaseMedicines ??= new List<Medicine>();
                    AvailableMedicines ??= new ObservableCollection<Medicine>();
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


                    AvailableUsages ??= new ObservableCollection<MedicineUsage>();
                    BaseUsages ??= new List<MedicineUsage>();
                    AvailableUsages.Add(item);
                    BaseUsages.Add(item);
                    return true;
                });
            });
        }
    }
}
