using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.Core;
using EyeClinic.Core.Enums;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Home.Setting.Diagnosis.DiagnosisEditor;
using Unity;
using operation = EyeClinic.Core.Enums.Operation;
namespace EyeClinic.WPF.Components.Home.Setting.Diagnosis
{
    public partial class DiagnosisViewModel
    {
        public DiagnosisViewModel() { }
    }

    public partial class DiagnosisViewModel : BaseViewModel<DiagnosisView>
    {
        private readonly IUnityContainer _container;
        private readonly IDiagnosisRepository _diagnosisRepository;

        public DiagnosisViewModel(IUnityContainer container, IDiagnosisRepository diagnosisRepository) {
            _container = container;
            _diagnosisRepository = diagnosisRepository;

            AddDiseaseCommand = new RelayCommand(AddDisease);
            EditDiseaseCommand = new RelayCommand(EditDisease);
            DeleteDiseaseCommand = new RelayCommand<Model.DiagnosisDto>(DeleteDisease);
            SearchCommand = new RelayCommand(Search);
        }

        public override async Task Initialize() {
            DiseasesList = new ObservableCollection<Model.DiagnosisDto>(
                await _diagnosisRepository.GetAll());
        }


        public ObservableCollection<Model.DiagnosisDto> DiseasesList { get; set; }
        public Model.DiagnosisDto SelectedDisease { get; set; }

        public string SearchText { get; set; }

        public ICommand SearchCommand { get; set; }
        public ICommand AddDiseaseCommand { get; set; }
        public ICommand EditDiseaseCommand { get; set; }
        public ICommand DeleteDiseaseCommand { get; set; }


        private void AddDisease() {
            BusyExecute(async () => {
                var editor = _container.Resolve<DiagnosisEditorViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Add;

                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as DiagnosisEditorView, async () => {
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
                var editor = _container.Resolve<DiagnosisEditorViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Edit;
                editor.BuildFromModel(SelectedDisease);

                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as DiagnosisEditorView, async () => {
                        var item = await editor.Save();
                        if (item == null)
                            return false;

                        DiseasesList.UpdateItem(SelectedDisease, item);
                        return true;
                    });
            });
        }

        private void DeleteDisease(Model.DiagnosisDto disease) {
           
            BusyExecute(async () => {
                await _diagnosisRepository.Delete(e => e.Id == disease.Id);
                DiseasesList.Remove(disease);
            });
        }

        private void Search() {
            BusyExecute(async () => {
                SearchText ??= "";

                DiseasesList = new ObservableCollection<Model.DiagnosisDto>(
                    await _diagnosisRepository
                        .GetByKey(e =>
                            e.EnName.ToLower().Contains(SearchText.ToLower()) ||
                            e.ArName.ToLower().Contains(SearchText.ToLower())
                            ));
            });
        }
    }
}
