using System;
using System.Threading.Tasks;
using EyeClinic.Core.Enums;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.WPF.Base;
using operation = EyeClinic.Core.Enums.Operation;
namespace EyeClinic.WPF.Components.Home.Setting.MedicalCenters.MedicalCenterEditor
{
    public partial class MedicalCenterEditorViewModel
    {
        public MedicalCenterEditorViewModel() { }
    }

    public partial class MedicalCenterEditorViewModel : BaseViewModel<MedicalCenterEditorView>
    {
        private readonly IMedicalCenterRepository _medicalCenterRepository;

        public MedicalCenterEditorViewModel(IMedicalCenterRepository medicalCenterRepository)
        {
            _medicalCenterRepository = medicalCenterRepository;
        }

        public int Id { get; set; }
        public string EnName { get; set; }
        public string ArName { get; set; }
        public DateTime CreatedDate { get; set; }

        public async Task<Model.MedicalCenterDto> Save() {
            if (ValidForSave()) {
                var item = BuildFromProperties();
                if (Operation == operation.Add) {
                    var addedItem = await _medicalCenterRepository.Add(item);
                    return addedItem;
                }

                if (Operation == operation.Edit && Id > 0) {
                    item.Id = Id;
                    item.CreatedDate = CreatedDate;
                    item.LastModifiedDate = DateTime.Now;
                    await _medicalCenterRepository.Update(item);
                    return item;
                }
            }

            return null;
        }

        private bool ValidForSave() {
            return !string.IsNullOrWhiteSpace(EnName) || !string.IsNullOrWhiteSpace(ArName);
        }

        private Model.MedicalCenterDto BuildFromProperties() {
            return new() {
                Id = Id,
                ArName = ArName,
                EnName = EnName,
                CreatedDate = CreatedDate
            };
        }

        public void BuildFromModel(Model.MedicalCenterDto disease) {
            Id = disease.Id;
            ArName = disease.ArName;
            EnName = disease.EnName;
            CreatedDate = disease.CreatedDate;
        }
    }
}
