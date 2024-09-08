using System;
using System.Threading.Tasks;
using EyeClinic.Core.Enums;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.WPF.Base;
using operation = EyeClinic.Core.Enums.Operation;
namespace EyeClinic.WPF.Components.Home.Setting.Diagnosis.DiagnosisEditor
{
    public partial class DiagnosisEditorViewModel
    {
        public DiagnosisEditorViewModel() { }
    }

    public partial class DiagnosisEditorViewModel : BaseViewModel<DiagnosisEditorView>
    {
        private readonly IDiagnosisRepository _diagnosisRepository;

        public DiagnosisEditorViewModel(IDiagnosisRepository diagnosisRepository) {
            _diagnosisRepository = diagnosisRepository;
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string EnName { get; set; }
        public string ArName { get; set; }
        public DateTime CreatedDate { get; set; }

        public async Task<Model.DiagnosisDto> Save() {
            if (ValidForSave()) {
                var item = BuildFromProperties();
                if (Operation == operation.Add) {
                    var addedItem = await _diagnosisRepository.Add(item);
                    return addedItem;
                }

                if (Operation == operation.Edit && Id > 0) {
                    item.Id = Id;
                    item.CreatedDate = CreatedDate;
                    item.LastModifiedDate = DateTime.Now;
                    await _diagnosisRepository.Update(item);
                    return item;
                }
            }

            return null;
        }

        private bool ValidForSave() {
            return !string.IsNullOrWhiteSpace(EnName) && !string.IsNullOrWhiteSpace(ArName)
                && !string.IsNullOrWhiteSpace(Code);
        }

        private Model.DiagnosisDto BuildFromProperties() {
            return new() {
                Id = Id,
                Code = Code,
                ArName = ArName,
                EnName = EnName,
                CreatedDate = CreatedDate
            };
        }

        public void BuildFromModel(Model.DiagnosisDto disease) {
            Id = disease.Id;
            Code = disease.Code;
            ArName = disease.ArName;
            EnName = disease.EnName;
            CreatedDate = disease.CreatedDate;
        }
    }
}
