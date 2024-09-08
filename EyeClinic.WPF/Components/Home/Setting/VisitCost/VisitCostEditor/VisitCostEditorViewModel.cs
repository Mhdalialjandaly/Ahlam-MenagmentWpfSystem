using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EyeClinic.Core.Enums;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.WPF.Base;
using operation = EyeClinic.Core.Enums.Operation;
namespace EyeClinic.WPF.Components.Home.Setting.VisitCost.VisitCostEditor
{
    public partial class VisitCostEditorViewModel
    {
        public VisitCostEditorViewModel() { }
    }

    public partial class VisitCostEditorViewModel : BaseViewModel<VisitCostEditorView>
    {
        private readonly IVisitTypeRepository _notPayReasonRepository;

        public VisitCostEditorViewModel(IVisitTypeRepository notPayReasonRepository) {
            _notPayReasonRepository = notPayReasonRepository;
        }

        public int Id { get; set; }
        public string EnName { get; set; }
        public string ArName { get; set; }
        public int Cost { get; set; }
        public DateTime CreatedDate { get; set; }

        public async Task<Model.VisitTypeDto> Save() {
            if (ValidForSave()) {
                var item = BuildFromProperties();
                item.ArName = item.EnName;

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
            return !string.IsNullOrWhiteSpace(EnName);
        }

        private Model.VisitTypeDto BuildFromProperties() {
            return new() {
                Id = Id,
                ArName = EnName,
                EnName = EnName,
                Cost = Cost,
                CreatedDate = CreatedDate
            };
        }

        public void BuildFromModel(Model.VisitTypeDto disease) {
            Id = disease.Id;
            ArName = disease.ArName;
            EnName = disease.EnName;
            Cost = disease.Cost;
            CreatedDate = disease.CreatedDate;
        }
    }
}
