using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.Base;
using operation = EyeClinic.Core.Enums.Operation;

namespace EyeClinic.WPF.Components.Home.Setting.Medicines.MedicineEditor
{
    public partial class MedicineEditorViewModel
    {
        public MedicineEditorViewModel() { }
    }

    public partial class MedicineEditorViewModel : BaseViewModel<MedicineEditorView>
    {
        private readonly IMedicineRepository _medicineRepository;
        private readonly IMedicineTypeRepository _medicineTypeRepository;

        public MedicineEditorViewModel(IMedicineRepository medicineRepository,
            IMedicineTypeRepository medicineTypeRepository) {
            _medicineRepository = medicineRepository;
            _medicineTypeRepository = medicineTypeRepository;
        }

        public override async Task Initialize() {
            MedicineTypes = await _medicineTypeRepository.GetAll();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string MedicineName { get; set; }
        public int MedicineTypeId { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<MedicineTypeDto> MedicineTypes { get; set; }
        public MedicineTypeDto SelectedMedicineType { get; set; }

        public async Task<MedicineDto> Save() {
            if (ValidForSave()) {
                var item = BuildFromProperties();
                if (Operation == operation.Add) {
                    var addedItem = await _medicineRepository.Add(item);

                    var medicine = await _medicineRepository.GetById(addedItem.Id);
                    return medicine;
                }

                if (Operation == operation.Edit && Id > 0) {
                    item.Id = Id;
                    item.CreatedDate = CreatedDate;
                    item.LastModifiedDate = DateTime.Now;
                    await _medicineRepository.Update(item);
                    return item;
                }
            }

            return null;
        }

        private bool ValidForSave() {
            return !string.IsNullOrWhiteSpace(MedicineName) && SelectedMedicineType != null;
        }

        private MedicineDto BuildFromProperties() {
            return new() {
                Id = Id,
                Code = MedicineName,
                MedicineName = MedicineName,
                MedicineTypeId = SelectedMedicineType.Id,
                CreatedDate = CreatedDate
            };
        }

        public void BuildFromModel(MedicineDto disease) {
            Id = disease.Id;
            Code = disease.Code;
            MedicineName = disease.MedicineName;
            MedicineTypeId = disease.MedicineTypeId;
            CreatedDate = disease.CreatedDate;

            SelectedMedicineType = MedicineTypes.FirstOrDefault(e => e.Id == disease.MedicineTypeId);
        }
    }
}
