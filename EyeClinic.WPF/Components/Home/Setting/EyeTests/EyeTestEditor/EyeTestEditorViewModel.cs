using System;
using System.Threading.Tasks;
using EyeClinic.Core.Enums;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.WPF.Base;
using operation = EyeClinic.Core.Enums.Operation;
namespace EyeClinic.WPF.Components.Home.Setting.EyeTests.EyeTestEditor
{
    public partial class EyeTestEditorViewModel
    {
        public EyeTestEditorViewModel() { }
    }

    public partial class EyeTestEditorViewModel : BaseViewModel<EyeTestEditorView>
    {
        private readonly IEyeTestRepository _eyeTestRepository;

        public EyeTestEditorViewModel(IEyeTestRepository eyeTestRepository) {
            _eyeTestRepository = eyeTestRepository;
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string EnName { get; set; }
        public string ArName { get; set; }
        public DateTime CreatedDate { get; set; }

        public async Task<Model.EyeTestDto> Save() {
            if (ValidForSave()) {
                var item = BuildFromProperties();
                if (Operation == operation.Add) {
                    var addedItem = await _eyeTestRepository.Add(item);
                    return addedItem;
                }

                if (Operation == operation.Edit && Id > 0) {
                    item.Id = Id;
                    item.CreatedDate = CreatedDate;
                    item.LastModifiedDate = DateTime.Now;
                    await _eyeTestRepository.Update(item);
                    return item;
                }
            }

            return null;
        }

        private bool ValidForSave() {
            return !string.IsNullOrWhiteSpace(EnName) || !string.IsNullOrWhiteSpace(ArName);
        }

        private Model.EyeTestDto BuildFromProperties() {
            return new() {
                Id = Id,
                Code = Code,
                ArName = ArName,
                EnName = EnName,
                CreatedDate = CreatedDate
            };
        }

        public void BuildFromModel(Model.EyeTestDto disease) {
            Id = disease.Id;
            Code = disease.Code;
            ArName = disease.ArName;
            EnName = disease.EnName;
            CreatedDate = disease.CreatedDate;
        }
    }
}
