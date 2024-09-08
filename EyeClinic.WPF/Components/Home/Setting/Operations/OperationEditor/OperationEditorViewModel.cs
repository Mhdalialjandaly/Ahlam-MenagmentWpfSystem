using System;
using System.Threading.Tasks;
using EyeClinic.Core.Enums;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.WPF.Base;
using operation = EyeClinic.Core.Enums.Operation;
namespace EyeClinic.WPF.Components.Home.Setting.Operations.OperationEditor
{
    public partial class OperationEditorViewModel
    {
        public OperationEditorViewModel() { }
    }

    public partial class OperationEditorViewModel : BaseViewModel<OperationEditorView>
    {
        private readonly IOperationRepository _operationRepository;

        public OperationEditorViewModel(IOperationRepository operationRepository) {
            _operationRepository = operationRepository;
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string EnName { get; set; }
        public string ArName { get; set; }
        public bool IsSurgical { get; set; }
        public DateTime CreatedDate { get; set; }

        public async Task<Model.OperationDto> Save() {
            if (ValidForSave()) {
                var item = BuildFromProperties();
                if (Operation == operation.Add) {
                    item.Code = item.EnName;
                    item.ArName = item.EnName;
                    var addedItem = await _operationRepository.Add(item);
                    return addedItem;
                }

                if (Operation == operation.Edit && Id > 0) {
                    item.Id = Id;
                    item.CreatedDate = CreatedDate;
                    item.LastModifiedDate = DateTime.Now;
                    await _operationRepository.Update(item);
                    return item;
                }
            }

            return null;
        }

        private bool ValidForSave() {
            return !string.IsNullOrWhiteSpace(EnName);
        }

        private Model.OperationDto BuildFromProperties() {
            return new() {
                Id = Id,
                Code = Code,
                ArName = ArName,
                EnName = EnName,
                IsSergical = IsSurgical,
                CreatedDate = CreatedDate
            };
        }

        public void BuildFromModel(Model.OperationDto disease) {
            Id = disease.Id;
            Code = disease.Code;
            ArName = disease.ArName;
            EnName = disease.EnName;
            IsSurgical = disease.IsSergical;
            CreatedDate = disease.CreatedDate;
        }
    }
}
