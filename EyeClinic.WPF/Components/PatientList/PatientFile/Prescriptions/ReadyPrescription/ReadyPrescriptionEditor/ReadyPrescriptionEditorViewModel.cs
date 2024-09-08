using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.Base;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.Prescriptions.ReadyPrescription.ReadyPrescriptionEditor
{
    public partial class ReadyPrescriptionEditorViewModel
    {
        public ReadyPrescriptionEditorViewModel() {
            if (IsDesignMode) { }
        }
    }

    public partial class ReadyPrescriptionEditorViewModel : BaseViewModel<ReadyPrescriptionEditorView>
    {
        private readonly IMedicineRepository _medicineRepository;
        private readonly IMedicineUsageRepository _medicineUsageRepository;
        private readonly IMedicineTypeRepository _medicineTypeRepository;

        public ReadyPrescriptionEditorViewModel(IMedicineRepository medicineRepository,
            IMedicineUsageRepository medicineUsageRepository, IMedicineTypeRepository medicineTypeRepository) {
            _medicineRepository = medicineRepository;
            _medicineUsageRepository = medicineUsageRepository;
            _medicineTypeRepository = medicineTypeRepository;

            AddMedicineCommand = new RelayCommand(AddMedicine);
            DeletePrescriptionItemCommand = new RelayCommand<ReadyPrescriptionMedicineDto>(DeletePrescriptionItem);
        }

        public override async Task Initialize() {
            BaseMedicines = await _medicineRepository.GetAll();
            BaseUsages = await _medicineUsageRepository.GetAll();
            MedicineTypes = await _medicineTypeRepository.GetAll();
            MedicineTypes.Insert(0, new MedicineTypeDto {
                Id = -1,
                ArName = "الكل",
                EnName = "All"
            });

            SelectedMedicineType = MedicineTypes[0];
        }


        public int Id { get; set; }
        public string EnName { get; set; }
        public string ArName { get; set; }
        public DateTime CreatedDate { get; set; }

        public ObservableCollection<ReadyPrescriptionMedicineDto> ReadyPrescriptionMedicines { get; set; }


        public bool PrintAfterSave { get; set; }

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

        private string _medicineCode;
        public string MedicineCode {
            get => _medicineCode;
            set {
                _medicineCode = value;
                SearchMedicineByCode(value);
            }
        }

        private string _medicineSearchText;
        public string MedicineSearchText {
            get => _medicineSearchText;
            set {
                _medicineSearchText = value;
                SearchMedicineByName(value);
            }
        }


        public List<MedicineDto> BaseMedicines { get; set; }
        public ObservableCollection<MedicineDto> AvailableMedicines { get; set; }
        public MedicineDto SelectedMedicine { get; set; }


        private string _usageSearchText;
        public string UsageSearchText {
            get => _usageSearchText;
            set {
                _usageSearchText = value;
                SearchUsage(value);
            }
        }


        public List<MedicineUsageDto> BaseUsages { get; set; }
        public ObservableCollection<MedicineUsageDto> AvailableUsages { get; set; }
        public MedicineUsageDto SelectedUsage { get; set; }


        public ICommand DeletePrescriptionItemCommand { get; set; }
        public ICommand AddMedicineCommand { get; set; }


        public Model.ReadyPrescriptionDto BuildFromProperties() {
            if (IsPropertiesInValid())
                return null;

            return new() {
                Id = Id,
                EnName = EnName,
                ArName = ArName,
                CreatedDate = CreatedDate,
                ReadyPrescriptionMedicines = ReadyPrescriptionMedicines
                    .Select(e => new ReadyPrescriptionMedicineDto() {
                        MedicineUsageId = e.MedicineUsageId,
                        MedicineId = e.MedicineId,
                        Id = e.Id,
                        CreatedDate = e.CreatedDate,
                        ReadyPrescriptionId = e.ReadyPrescriptionId,
                    })
                    .ToList()
            };
        }

        public void Fill(Model.ReadyPrescriptionDto readyPrescription) {
            Id = readyPrescription.Id;
            EnName = readyPrescription.EnName;
            ArName = readyPrescription.ArName;
            CreatedDate = readyPrescription.CreatedDate;
            ReadyPrescriptionMedicines = new ObservableCollection<ReadyPrescriptionMedicineDto>(
                                    readyPrescription.ReadyPrescriptionMedicines);
        }

        private void SearchMedicineByCode(string code) {
            AvailableMedicines = new ObservableCollection<MedicineDto>(
                BaseMedicines.Where(e => e.Code == code)
                    .ToList());
        }

        private void SearchMedicineByName(string medicineName) {
            AvailableMedicines = new ObservableCollection<MedicineDto>(
                BaseMedicines
                    .Where(e => e.MedicineName.ToLower().Contains(medicineName.ToLower())
                    && (SelectedMedicineType.Id == -1 || e.MedicineTypeId == SelectedMedicineType.Id))
                    .ToList());
        }

        private void SearchUsage(string usageName) {
            AvailableUsages = new ObservableCollection<MedicineUsageDto>(
                BaseUsages.Where(e => e.UsageName.ToLower().Contains(usageName.ToLower())
                                      && (SelectedMedicineType.Id == -1 || e.UsageMedicineTypeId == SelectedMedicineType.Id))
                    .ToList());
        }

        private void FilterMedicinesByType(MedicineTypeDto medicineType) {
            if (medicineType.Id == -1) {
                AvailableMedicines = new ObservableCollection<MedicineDto>(BaseMedicines);
                AvailableUsages = new ObservableCollection<MedicineUsageDto>(BaseUsages);
                return;
            }

            AvailableMedicines = new ObservableCollection<MedicineDto>(
                BaseMedicines
                    .Where(e => e.MedicineTypeId == medicineType.Id)
                    .ToList());

            AvailableUsages = new ObservableCollection<MedicineUsageDto>(
                BaseUsages
                    .Where(e => e.UsageMedicineTypeId == medicineType.Id)
                    .ToList());
        }


        private void DeletePrescriptionItem(ReadyPrescriptionMedicineDto item) {
            ReadyPrescriptionMedicines.Remove(item);
        }

        private void AddMedicine() {
            if (IsPropertiesInValid()) {
                // Show invalid
            }

            ReadyPrescriptionMedicines ??= new ObservableCollection<ReadyPrescriptionMedicineDto>();
            ReadyPrescriptionMedicines.Add(new ReadyPrescriptionMedicineDto {
                Medicine = SelectedMedicine,
                MedicineId = SelectedMedicine.Id,
                MedicineUsage = SelectedUsage,
                MedicineUsageId = SelectedUsage.Id
            });
        }

        private bool IsPropertiesInValid() {
            return SelectedMedicine == null || SelectedUsage == null;
        }
    }
}
