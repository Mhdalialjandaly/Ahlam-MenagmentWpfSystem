using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.Core;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Home.Setting.Medicines.MedicineEditor;
using Unity;
using operation = EyeClinic.Core.Enums.Operation;

namespace EyeClinic.WPF.Components.Home.Setting.Medicines
{
    public partial class MedicineViewModel
    {
        public MedicineViewModel() { }
    }

    public partial class MedicineViewModel : BaseViewModel<MedicineView>
    {
        private readonly IUnityContainer _container;
        private readonly IMedicineRepository _medicineRepository;
        private readonly IMedicineTypeRepository _medicineTypeRepository;

        public MedicineViewModel(IUnityContainer container, IMedicineRepository medicineRepository,
            IMedicineTypeRepository medicineTypeRepository) {
            _container = container;
            _medicineRepository = medicineRepository;
            _medicineTypeRepository = medicineTypeRepository;

            AddDiseaseCommand = new RelayCommand(AddDisease);
            EditDiseaseCommand = new RelayCommand(EditDisease);
            DeleteDiseaseCommand = new RelayCommand<Model.MedicineDto>(DeleteDisease);
            SearchCommand = new RelayCommand(Search);
        }

        public override async Task Initialize() {
            DiseasesList = new ObservableCollection<Model.MedicineDto>(
                await _medicineRepository.GetAll());

            MedicineTypes = await _medicineTypeRepository.GetAll();
        }


        public ObservableCollection<Model.MedicineDto> DiseasesList { get; set; }
        public Model.MedicineDto SelectedDisease { get; set; }
        public List<MedicineTypeDto> MedicineTypes { get; set; }

        public string SearchText { get; set; }

        public ICommand SearchCommand { get; set; }
        public ICommand AddDiseaseCommand { get; set; }
        public ICommand EditDiseaseCommand { get; set; }
        public ICommand DeleteDiseaseCommand { get; set; }


        private void AddDisease() {
            BusyExecute(async () => {
                var editor = _container.Resolve<MedicineEditorViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Add;

                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as MedicineEditorView, async () => {
                        var item = await editor.Save();
                        if (item == null)
                            return false;

                        item.MedicineType = MedicineTypes.FirstOrDefault(e => e.Id == item.MedicineTypeId);
                        DiseasesList.Add(item);
                        return true;
                    });
            });
        }

        private void EditDisease() {
            if (SelectedDisease == null)
                return;

            BusyExecute(async () => {
                var editor = _container.Resolve<MedicineEditorViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Edit;
                editor.BuildFromModel(SelectedDisease);

                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as MedicineEditorView, async () => {
                        var item = await editor.Save();
                        if (item == null)
                            return false;

                        item.MedicineType = MedicineTypes.FirstOrDefault(e => e.Id == item.MedicineTypeId);
                        DiseasesList.UpdateItem(SelectedDisease, item);
                        return true;
                    });
            });
        }

        private void DeleteDisease(Model.MedicineDto disease) {
          
            BusyExecute(async () => {
                await _medicineRepository.Delete(e => e.Id == disease.Id);
                DiseasesList.Remove(disease);
            });
        }

        private void Search() {
            BusyExecute(async () => {
                SearchText ??= "";

                DiseasesList = new ObservableCollection<MedicineDto>(
                    await _medicineRepository
                        .GetByKey(e =>
                                       e.MedicineName.ToLower().StartsWith(SearchText.ToLower())));
            });
        }
    }
}
