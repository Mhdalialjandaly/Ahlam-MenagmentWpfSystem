using System;
using System.Threading.Tasks;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using operation = EyeClinic.Core.Enums.Operation;

namespace EyeClinic.WPF.Components.Home.Setting.Diseases.DiseaseEditor
{
    public partial class DiseaseEditorViewModel
    {
        public DiseaseEditorViewModel() { }
    }

    public partial class DiseaseEditorViewModel : BaseViewModel<DiseaseEditorView>
    {
        private readonly IDiseasesRepository _diseasesRepository;
        private readonly INotificationService _notificationService;
        public DiseaseEditorViewModel(IDiseasesRepository diseasesRepository,INotificationService notificationService) {
            _diseasesRepository = diseasesRepository;
            _notificationService = notificationService;
        }

        public int Id { get; set; }
        public string EnName { get; set; }
        public string ArName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ThePNumber { set; get; }
        public string LastEnrtyValue { set; get; }
        public string MinimumExtry { set; get; }
        public double FirstValue { set; get; }
        public string Note { set; get; }
        


        public async Task<DiseaseDto> Save() {
            if (ValidForSave()) {
                var item = BuildFromProperties();
                if (Operation == operation.Add) {
                    var addedItem = await _diseasesRepository.Add(item);
                    return addedItem;
                }

                if (Operation == operation.Edit && Id > 0) {
                    item.Id = Id;
                    item.CreatedDate = CreatedDate;
                    item.LastModifiedDate = DateTime.Now;
                    await _diseasesRepository.Update(item);
                    return item;
                }
            }

            return null;
        }

        private bool ValidForSave() {
            if (FirstValue != 0 && !int.TryParse(MinimumExtry, out _))
            {
                _notificationService.Warning("الرجاء ادخال رقم في خانة رصيد اول المدة ..");
                return false;
            }
            return !string.IsNullOrWhiteSpace(ArName)  || FirstValue != 0 || ThePNumber != 0 || !int.TryParse(MinimumExtry, out _);
        }

        private DiseaseDto BuildFromProperties() {
            return new() {
                Id = Id,
                ArName = ArName,
                EnName = EnName,
                CreatedDate = DateTime.Now,
                ThePNumber=ThePNumber,
                Extry=MinimumExtry,
                FirstValue=FirstValue,
                Note=Note,
                Enrty= LastEnrtyValue
            };
        }

        public void BuildFromModel(DiseaseDto disease) {
            Id = disease.Id;
            ArName = disease.ArName;
            EnName = disease.EnName;
            CreatedDate = disease.CreatedDate;
            ThePNumber = disease.ThePNumber;
            FirstValue = disease.FirstValue;
            MinimumExtry = disease.Extry;
            Note = disease.Note;            
        }
    }
}
