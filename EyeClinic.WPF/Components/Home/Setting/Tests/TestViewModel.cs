using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.Core;
using EyeClinic.Core.Enums;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Dialogs.PasswordInput;
using EyeClinic.WPF.Components.Home.Setting.Tests.TestEditor;
using EyeClinic.WPF.Setup;
using Microsoft.VisualBasic.ApplicationServices;
using Org.BouncyCastle.Utilities;
using Unity;
using operation = EyeClinic.Core.Enums.Operation;
namespace EyeClinic.WPF.Components.Home.Setting.Tests
{
    public partial class TestViewModel
    {
        public TestViewModel() { }
    }

    public partial class TestViewModel : BaseViewModel<TestView>
    {
        private readonly IUnityContainer _container;
        private readonly ITestsRepository _testsRepository;

        public TestViewModel(IUnityContainer container, ITestsRepository testsRepository) {
            _container = container;
            _testsRepository = testsRepository;

            AddDiseaseCommand = new RelayCommand(AddDisease);
            EditDiseaseCommand = new RelayCommand(EditDisease);
            DeleteDiseaseCommand = new RelayCommand<Model.TestDto>(DeleteDisease);
            SearchCommand = new RelayCommand(Search);
        }

        public override async Task Initialize() {
            DiseasesList = new ObservableCollection<Model.TestDto>(
                await _testsRepository.GetAll());
        }


        public ObservableCollection<Model.TestDto> DiseasesList { get; set; }
        public Model.TestDto SelectedDisease { get; set; }

        public string SearchText { get; set; }

        public ICommand SearchCommand { get; set; }
        public ICommand AddDiseaseCommand { get; set; }
        public ICommand EditDiseaseCommand { get; set; }
        public ICommand DeleteDiseaseCommand { get; set; }


        private void AddDisease() {
            BusyExecute(async () => {
                var editor = _container.Resolve<TestEditorViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Add;

                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as TestEditorView, async () => {
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
                var editor = _container.Resolve<TestEditorViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Edit;
                editor.BuildFromModel(SelectedDisease);                
                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as TestEditorView, async () => {
                        var item = await editor.Save();
                        if (item == null)
                            return false;

                        DiseasesList.UpdateItem(SelectedDisease, item);
                        return true;
                    });
            });
        }

        private void DeleteDisease(Model.TestDto disease) {
            if (_container.CheckUserRole(UserRoles.Admin))
            {
                BusyExecute(async () => {
                    await _testsRepository.Delete(e => e.Id == disease.Id);
                    DiseasesList.Remove(disease);
                });
            }            
        }

        private void Search() {
            BusyExecute(async () => {
                SearchText ??= "";

                DiseasesList = new ObservableCollection<Model.TestDto>(
                    await _testsRepository
                        .GetByKey(e =>
                            e.EnName.ToLower().Contains(SearchText.ToLower()) ||
                            e.ArName.ToLower().Contains(SearchText.ToLower())
                        ));
            });
        }
    }
}
