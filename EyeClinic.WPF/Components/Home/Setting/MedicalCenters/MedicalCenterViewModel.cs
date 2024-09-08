using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.Core;
using EyeClinic.Core.Enums;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Home.Setting.MedicalCenters.MedicalCenterEditor;
using Unity;
using operation = EyeClinic.Core.Enums.Operation;
namespace EyeClinic.WPF.Components.Home.Setting.MedicalCenters
{
    public partial class MedicalCenterViewModel
    {
        public MedicalCenterViewModel() { }
    }

    public partial class MedicalCenterViewModel : BaseViewModel<MedicalCenterView>
    {
        private readonly IUnityContainer _container;
        private readonly IMedicalCenterRepository _medicalCenterRepository;

        public MedicalCenterViewModel(IUnityContainer container, IMedicalCenterRepository medicalCenterRepository) {
            _container = container;
            _medicalCenterRepository = medicalCenterRepository;

            AddDiseaseCommand = new RelayCommand(AddDisease);
            EditDiseaseCommand = new RelayCommand(EditDisease);
            DeleteDiseaseCommand = new RelayCommand<Model.MedicalCenterDto>(DeleteDisease);
            SearchCommand = new RelayCommand(Search);
        }

        public override async Task Initialize() {
            DiseasesList = new ObservableCollection<Model.MedicalCenterDto>(
                await _medicalCenterRepository.GetAll());
        }


        public ObservableCollection<Model.MedicalCenterDto> DiseasesList { get; set; }
        public Model.MedicalCenterDto SelectedDisease { get; set; }

        public string SearchText { get; set; }

        public ICommand SearchCommand { get; set; }
        public ICommand AddDiseaseCommand { get; set; }
        public ICommand EditDiseaseCommand { get; set; }
        public ICommand DeleteDiseaseCommand { get; set; }


        private void AddDisease() {
            BusyExecute(async () => {
                var editor = _container.Resolve<MedicalCenterEditorViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Add;

                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as MedicalCenterEditorView, async () => {
                        var item = await editor.Save();
                        if (item == null)
                            return false;

                        DiseasesList.Add(item);
                        return true;
                    });
            });
        }

        private void EditDisease() {
            if (SelectedDisease == null)
                return;

            BusyExecute(async () => {
                var editor = _container.Resolve<MedicalCenterEditorViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Edit;
                editor.BuildFromModel(SelectedDisease);

                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as MedicalCenterEditorView, async () => {
                        var item = await editor.Save();
                        if (item == null)
                            return false;

                        DiseasesList.UpdateItem(SelectedDisease, item);
                        return true;
                    });
            });
        }

        private void DeleteDisease(Model.MedicalCenterDto disease) {
          
            BusyExecute(async () => {
                await _medicalCenterRepository.Delete(e => e.Id == disease.Id);
                DiseasesList.Remove(disease);
            });
        }

        private void Search() {
            BusyExecute(async () => {
                SearchText ??= "";

                DiseasesList = new ObservableCollection<Model.MedicalCenterDto>(
                    await _medicalCenterRepository
                        .GetByKey(e =>
                            e.EnName.ToLower().Contains(SearchText.ToLower()) ||
                            e.ArName.ToLower().Contains(SearchText.ToLower())
                            ));
            });
        }
    }
}
