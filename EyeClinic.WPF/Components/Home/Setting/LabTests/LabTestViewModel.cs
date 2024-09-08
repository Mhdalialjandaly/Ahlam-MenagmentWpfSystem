using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.Core;
using EyeClinic.Core.Enums;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Home.Setting.LabTests.LabTestEditor;
using Unity;
using operation = EyeClinic.Core.Enums.Operation;
namespace EyeClinic.WPF.Components.Home.Setting.LabTests
{
    public partial class LabTestViewModel
    {
        public LabTestViewModel() { }
    }

    public partial class LabTestViewModel : BaseViewModel<LabTestView>
    {
        private readonly IUnityContainer _container;
        private readonly ILabTestRepository _labTestRepository;

        public LabTestViewModel(IUnityContainer container, ILabTestRepository labTestRepository) {
            _container = container;
            _labTestRepository = labTestRepository;

            AddDiseaseCommand = new RelayCommand(AddDisease);
            EditDiseaseCommand = new RelayCommand(EditDisease);
            DeleteDiseaseCommand = new RelayCommand<Model.LabTestDto>(DeleteDisease);
            SearchCommand = new RelayCommand(Search);
        }

        public override async Task Initialize() {
            DiseasesList = new ObservableCollection<Model.LabTestDto>(
                await _labTestRepository.GetAll());
        }


        public ObservableCollection<Model.LabTestDto> DiseasesList { get; set; }
        public Model.LabTestDto SelectedDisease { get; set; }

        public string SearchText { get; set; }

        public ICommand SearchCommand { get; set; }
        public ICommand AddDiseaseCommand { get; set; }
        public ICommand EditDiseaseCommand { get; set; }
        public ICommand DeleteDiseaseCommand { get; set; }


        private void AddDisease() {
            BusyExecute(async () => {
                var editor = _container.Resolve<LabTestEditorViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Add;

                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as LabTestEditorView, async () => {
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
                var editor = _container.Resolve<LabTestEditorViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Edit;
                editor.BuildFromModel(SelectedDisease);

                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as LabTestEditorView, async () => {
                        var item = await editor.Save();
                        if (item == null)
                            return false;

                        DiseasesList.UpdateItem(SelectedDisease, item);
                        return true;
                    });
            });
        }

        private void DeleteDisease(Model.LabTestDto disease) {
         
            BusyExecute(async () => {
                await _labTestRepository.Delete(e => e.Id == disease.Id);
                DiseasesList.Remove(disease);
            });
        }

        private void Search() {
            BusyExecute(async () => {
                SearchText ??= "";

                DiseasesList = new ObservableCollection<Model.LabTestDto>(
                    await _labTestRepository
                        .GetByKey(e =>
                            e.LabTestName.ToLower().Contains(SearchText.ToLower())));
            });
        }
    }
}
