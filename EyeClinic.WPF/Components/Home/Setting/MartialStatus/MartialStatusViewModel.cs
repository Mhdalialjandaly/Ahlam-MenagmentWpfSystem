using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.Core;
using EyeClinic.Core.Enums;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Home.Setting.MartialStatus.MartialStatusEditor;
using Unity;
using operation = EyeClinic.Core.Enums.Operation;
namespace EyeClinic.WPF.Components.Home.Setting.MartialStatus
{
    public partial class MartialStatusViewModel
    {
        public MartialStatusViewModel() { }
    }

    public partial class MartialStatusViewModel : BaseViewModel<MartialStatusView>
    {
        private readonly IUnityContainer _container;
        private readonly IMartialStatusRepository _martialStatusRepository;

        public MartialStatusViewModel(IUnityContainer container, IMartialStatusRepository martialStatusRepository) {
            _container = container;
            _martialStatusRepository = martialStatusRepository;

            AddDiseaseCommand = new RelayCommand(AddDisease);
            EditDiseaseCommand = new RelayCommand(EditDisease);
            DeleteDiseaseCommand = new RelayCommand<Model.MartialStatusDto>(DeleteDisease);
            SearchCommand = new RelayCommand(Search);
        }

        public override async Task Initialize() {
            DiseasesList = new ObservableCollection<Model.MartialStatusDto>(
                await _martialStatusRepository.GetAll());
        }


        public ObservableCollection<Model.MartialStatusDto> DiseasesList { get; set; }
        public Model.MartialStatusDto SelectedDisease { get; set; }

        public string SearchText { get; set; }

        public ICommand SearchCommand { get; set; }
        public ICommand AddDiseaseCommand { get; set; }
        public ICommand EditDiseaseCommand { get; set; }
        public ICommand DeleteDiseaseCommand { get; set; }


        private void AddDisease() {
            BusyExecute(async () => {
                var editor = _container.Resolve<MartialStatusEditorViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Add;

                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as MartialStatusEditorView, async () => {
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
                var editor = _container.Resolve<MartialStatusEditorViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Edit;
                editor.BuildFromModel(SelectedDisease);

                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as MartialStatusEditorView, async () => {
                        var item = await editor.Save();
                        if (item == null)
                            return false;

                        DiseasesList.UpdateItem(SelectedDisease, item);
                        return true;
                    });
            });
        }

        private void DeleteDisease(Model.MartialStatusDto disease) {
            
            BusyExecute(async () => {
                await _martialStatusRepository.Delete(e => e.Id == disease.Id);
                DiseasesList.Remove(disease);
            });
        }

        private void Search() {
            BusyExecute(async () => {
                SearchText ??= "";

                DiseasesList = new ObservableCollection<Model.MartialStatusDto>(
                    await _martialStatusRepository
                        .GetByKey(e =>
                            e.EnName.ToLower().Contains(SearchText.ToLower()) ||
                            e.ArName.ToLower().Contains(SearchText.ToLower())
                            ));
            });
        }
    }
}
