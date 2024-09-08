using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.Core;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Home.Setting.Diseases.DiseaseEditor;
using Unity;
using operation = EyeClinic.Core.Enums.Operation;

namespace EyeClinic.WPF.Components.Home.Setting.Diseases
{
    public partial class DiseaseViewModel
    {
        public DiseaseViewModel() { }
    }

    public partial class DiseaseViewModel : BaseViewModel<DiseaseView>
    {
        private readonly IUnityContainer _container;
        private readonly IDiseasesRepository _diseasesRepository;

        public DiseaseViewModel(IUnityContainer container, IDiseasesRepository diseasesRepository) {
            _container = container;
            _diseasesRepository = diseasesRepository;

            AddDiseaseCommand = new RelayCommand(AddDisease);
            EditDiseaseCommand = new RelayCommand(EditDisease);
            DeleteDiseaseCommand = new RelayCommand<DiseaseDto>(DeleteDisease);
            SearchCommand = new RelayCommand(Search);
        }

        public override async Task Initialize() {
            DiseasesList = new ObservableCollection<DiseaseDto>(
                await _diseasesRepository.GetAll());
        }


        public ObservableCollection<DiseaseDto> DiseasesList { get; set; }
        public DiseaseDto SelectedDisease { get; set; }

        public string SearchText { get; set; }

        public ICommand SearchCommand { get; set; }
        public ICommand AddDiseaseCommand { get; set; }
        public ICommand EditDiseaseCommand { get; set; }
        public ICommand DeleteDiseaseCommand { get; set; }


        private void AddDisease() {
            BusyExecute(async () => {
                var editor = _container.Resolve<DiseaseEditorViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Add;

                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as DiseaseEditorView, async () => {
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
                var editor = _container.Resolve<DiseaseEditorViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Edit;
                editor.BuildFromModel(SelectedDisease);

                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as DiseaseEditorView, async () => {
                        var item = await editor.Save();
                        if (item == null)
                            return false;

                        DiseasesList.UpdateItem(SelectedDisease, item);
                        return true;
                    });
            });
        }

        private void DeleteDisease(DiseaseDto disease) {
        
            BusyExecute(async () => {
                await _diseasesRepository.Delete(e => e.Id == disease.Id);
                DiseasesList.Remove(disease);
            });
        }

        private void Search() {
            BusyExecute(async () => {
                SearchText ??= "";

                DiseasesList = new ObservableCollection<DiseaseDto>(
                    await _diseasesRepository
                        .GetByKey(e =>
                            e.EnName.ToLower().Contains(SearchText.ToLower()) ||
                            e.ArName.ToLower().Contains(SearchText.ToLower())
                            ));
            });
        }
    }
}
