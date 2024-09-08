using System;
using System.Threading.Tasks;
using EyeClinic.Core.Enums;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.WPF.Base;
using operation = EyeClinic.Core.Enums.Operation;
namespace EyeClinic.WPF.Components.Home.Setting.NotPayReason.NotPayReasonEditor
{
    public partial class NotPayReasonEditorViewModel
    {
        public NotPayReasonEditorViewModel() { }
    }

    public partial class NotPayReasonEditorViewModel : BaseViewModel<NotPayReasonEditorView>
    {
        private readonly INotPayReasonRepository _notPayReasonRepository;

        public NotPayReasonEditorViewModel(INotPayReasonRepository notPayReasonRepository) {
            _notPayReasonRepository = notPayReasonRepository;
        }

        public int Id { get; set; }
        public string EnName { get; set; }
        public string ArName { get; set; }
        public DateTime CreatedDate { get; set; }

        public async Task<Model.NotPayReasonDto> Save() {
            if (ValidForSave()) {
                var item = BuildFromProperties();
                if (Operation == operation.Add) {
                    var addedItem = await _notPayReasonRepository.Add(item);
                    return addedItem;
                }

                if (Operation == operation.Edit && Id > 0) {
                    item.Id = Id;
                    item.CreatedDate = CreatedDate;
                    item.LastModifiedDate = DateTime.Now;
                    await _notPayReasonRepository.Update(item);
                    return item;
                }
            }

            return null;
        }

        private bool ValidForSave() {
            return !string.IsNullOrWhiteSpace(EnName) || !string.IsNullOrWhiteSpace(ArName);
        }

        private Model.NotPayReasonDto BuildFromProperties() {
            return new() {
                Id = Id,
                ArName = ArName,
                EnName = EnName,
                CreatedDate = CreatedDate
            };
        }

        public void BuildFromModel(Model.NotPayReasonDto disease) {
            Id = disease.Id;
            ArName = disease.ArName;
            EnName = disease.EnName;
            CreatedDate = disease.CreatedDate;
        }
    }
}
