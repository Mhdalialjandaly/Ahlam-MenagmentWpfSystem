using System;
using System.Threading.Tasks;
using EyeClinic.Core.Enums;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.WPF.Base;
using operation = EyeClinic.Core.Enums.Operation;
namespace EyeClinic.WPF.Components.Home.Setting.LabTests.LabTestEditor
{
    public partial class LabTestEditorViewModel
    {
        public LabTestEditorViewModel() { }
    }

    public partial class LabTestEditorViewModel : BaseViewModel<LabTestEditorView>
    {
        private readonly ILabTestRepository _labTestRepository;

        public LabTestEditorViewModel(ILabTestRepository labTestRepository) {
            _labTestRepository = labTestRepository;
        }

        public int Id { get; set; }
        public string LabTestName { get; set; }
        public DateTime CreatedDate { get; set; }

        public async Task<Model.LabTestDto> Save() {
            if (ValidForSave()) {
                var item = BuildFromProperties();
                if (Operation == operation.Add) {
                    var addedItem = await _labTestRepository.Add(item);
                    return addedItem;
                }

                if (Operation == operation.Edit && Id > 0) {
                    item.Id = Id;
                    item.CreatedDate = CreatedDate;
                    item.LastModifiedDate = DateTime.Now;
                    await _labTestRepository.Update(item);
                    return item;
                }
            }

            return null;
        }

        private bool ValidForSave() {
            return !string.IsNullOrWhiteSpace(LabTestName);
        }

        private Model.LabTestDto BuildFromProperties() {
            return new() {
                Id = Id,
                LabTestName = LabTestName,
                CreatedDate = CreatedDate
            };
        }

        public void BuildFromModel(Model.LabTestDto disease) {
            Id = disease.Id;
            LabTestName = disease.LabTestName;
            CreatedDate = disease.CreatedDate;
        }
    }
}
