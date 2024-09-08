using System;
using System.Threading.Tasks;
using EyeClinic.Core.Enums;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.WPF.Base;
using operation = EyeClinic.Core.Enums.Operation;
namespace EyeClinic.WPF.Components.Home.Setting.MartialStatus.MartialStatusEditor
{
    public partial class MartialStatusEditorViewModel
    {
        public MartialStatusEditorViewModel() { }
    }

    public partial class MartialStatusEditorViewModel : BaseViewModel<MartialStatusEditorView>
    {
        private readonly IMartialStatusRepository _martialStatusRepository;

        public MartialStatusEditorViewModel(IMartialStatusRepository martialStatusRepository) {
            _martialStatusRepository = martialStatusRepository;
        }

        public int Id { get; set; }
        public string EnName { get; set; }
        public string ArName { get; set; }
        public DateTime CreatedDate { get; set; }

        public async Task<Model.MartialStatusDto> Save() {
            if (ValidForSave()) {
                var item = BuildFromProperties();
                if (Operation == operation.Add) {
                    var addedItem = await _martialStatusRepository.Add(item);
                    return addedItem;
                }

                if (Operation == operation.Edit && Id > 0) {
                    item.Id = Id;
                    item.CreatedDate = CreatedDate;
                    item.LastModifiedDate = DateTime.Now;
                    await _martialStatusRepository.Update(item);
                    return item;
                }
            }

            return null;
        }

        private bool ValidForSave() {
            return !string.IsNullOrWhiteSpace(EnName) || !string.IsNullOrWhiteSpace(ArName);
        }

        private Model.MartialStatusDto BuildFromProperties() {
            return new() {
                Id = Id,
                ArName = ArName,
                EnName = EnName,
                CreatedDate = CreatedDate
            };
        }

        public void BuildFromModel(Model.MartialStatusDto disease) {
            Id = disease.Id;
            ArName = disease.ArName;
            EnName = disease.EnName;
            CreatedDate = disease.CreatedDate;
        }
    }
}
