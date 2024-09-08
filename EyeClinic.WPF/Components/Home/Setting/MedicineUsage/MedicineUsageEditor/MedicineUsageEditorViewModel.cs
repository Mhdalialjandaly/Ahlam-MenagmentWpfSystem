using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.Base;
using operation = EyeClinic.Core.Enums.Operation;

namespace EyeClinic.WPF.Components.Home.Setting.MedicineUsage.MedicineUsageEditor
{
    public partial class MedicineUsageEditorViewModel
    {
        public MedicineUsageEditorViewModel() { }
    }

    public partial class MedicineUsageEditorViewModel : BaseViewModel<MedicineUsageEditorView>
    {
        private readonly IMedicineUsageRepository _medicineUsageRepository;
        private readonly IMedicineTypeRepository _medicineTypeRepository;

        public MedicineUsageEditorViewModel(IMedicineUsageRepository medicineUsageRepository,
             IMedicineTypeRepository medicineTypeRepository) {
            _medicineUsageRepository = medicineUsageRepository;
            _medicineTypeRepository = medicineTypeRepository;
        }

        public override async Task Initialize() {
            MedicineTypes = await _medicineTypeRepository.GetAll();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string UsageName { get; set; }
        public int UsageMedicineTypeId { get; set; }
        public DateTime CreatedDate { get; set; }


        public List<MedicineTypeDto> MedicineTypes { get; set; }
        public MedicineTypeDto SelectedMedicineType { get; set; }

        public async Task<Model.MedicineUsageDto> Save() {
            if (ValidForSave()) {
                var item = BuildFromProperties();
                if (Operation == operation.Add) {
                    var addedItem = await _medicineUsageRepository.Add(item);
                    return addedItem;
                }

                if (Operation == operation.Edit && Id > 0) {
                    item.Id = Id;
                    item.CreatedDate = CreatedDate;
                    item.LastModifiedDate = DateTime.Now;
                    await _medicineUsageRepository.Update(item);
                    return item;
                }
            }

            return null;
        }

        private bool ValidForSave() {
            return !string.IsNullOrWhiteSpace(UsageName) && !string.IsNullOrWhiteSpace(Code)
                                                         && SelectedMedicineType != null;
        }

        private Model.MedicineUsageDto BuildFromProperties() {
            return new() {
                Id = Id,
                Code = Code,
                UsageName = UsageName,
                UsageMedicineTypeId = SelectedMedicineType.Id,
                CreatedDate = CreatedDate
            };
        }

        public void BuildFromModel(Model.MedicineUsageDto disease) {
            Id = disease.Id;
            Code = disease.Code;
            UsageName = disease.UsageName;
            UsageMedicineTypeId = disease.UsageMedicineTypeId;
            CreatedDate = disease.CreatedDate;

            SelectedMedicineType = MedicineTypes.FirstOrDefault(e => e.Id == disease.UsageMedicineTypeId);
        }
    }
}
