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
using EyeClinic.WPF.Components.Home.Setting.MedicineUsage.MedicineUsageEditor;
using Unity;
using operation = EyeClinic.Core.Enums.Operation;

namespace EyeClinic.WPF.Components.Home.Setting.MedicineUsage
{
    public partial class MedicineUsageViewModel
    {
        public MedicineUsageViewModel() { }
    }

    public partial class MedicineUsageViewModel : BaseViewModel<MedicineUsageView>
    {
        private readonly IUnityContainer _container;
        private readonly IMedicineUsageRepository _medicineUsageRepository;
        private readonly IMedicineTypeRepository _medicineTypeRepository;

        public MedicineUsageViewModel(IUnityContainer container, IMedicineUsageRepository medicineUsageRepository,
             IMedicineTypeRepository medicineTypeRepository) {
            _container = container;
            _medicineUsageRepository = medicineUsageRepository;
            _medicineTypeRepository = medicineTypeRepository;

            AddDiseaseCommand = new RelayCommand(AddDisease);
            EditDiseaseCommand = new RelayCommand(EditDisease);
            DeleteDiseaseCommand = new RelayCommand<Model.MedicineUsageDto>(DeleteDisease);
            SearchCommand = new RelayCommand(Search);
        }

        public override async Task Initialize() {
            DiseasesList = new ObservableCollection<Model.MedicineUsageDto>(
                await _medicineUsageRepository.GetAll());

            MedicineTypes = await _medicineTypeRepository.GetAll();
        }


        public ObservableCollection<Model.MedicineUsageDto> DiseasesList { get; set; }
        public Model.MedicineUsageDto SelectedDisease { get; set; }

        public List<MedicineTypeDto> MedicineTypes { get; set; }

        public string SearchText { get; set; }

        public ICommand SearchCommand { get; set; }
        public ICommand AddDiseaseCommand { get; set; }
        public ICommand EditDiseaseCommand { get; set; }
        public ICommand DeleteDiseaseCommand { get; set; }


        private void AddDisease() {
            BusyExecute(async () => {
                var editor = _container.Resolve<MedicineUsageEditorViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Add;

                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as MedicineUsageEditorView, async () => {
                        var item = await editor.Save();
                        if (item == null)
                            return false;

                        item.UsageMedicineType = MedicineTypes.FirstOrDefault(e => e.Id == item.UsageMedicineTypeId);
                        DiseasesList.Add(item);
                        return true;
                    });
            });
        }

        private void EditDisease() {
            if (SelectedDisease == null)
                return;

            BusyExecute(async () => {
                var editor = _container.Resolve<MedicineUsageEditorViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Edit;
                editor.BuildFromModel(SelectedDisease);

                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as MedicineUsageEditorView, async () => {
                        var item = await editor.Save();
                        if (item == null)
                            return false;

                        item.UsageMedicineType = MedicineTypes.FirstOrDefault(e => e.Id == item.UsageMedicineTypeId);
                        DiseasesList.UpdateItem(SelectedDisease, item);
                        return true;
                    });
            });
        }

        private void DeleteDisease(Model.MedicineUsageDto disease) {
           
            BusyExecute(async () => {
                await _medicineUsageRepository.Delete(e => e.Id == disease.Id);
                DiseasesList.Remove(disease);
            });
        }

        private void Search() {
            BusyExecute(async () => {
                SearchText ??= "";

                DiseasesList = new ObservableCollection<Model.MedicineUsageDto>(
                    await _medicineUsageRepository
                        .GetByKey(e =>
                            e.UsageName.ToLower().Contains(SearchText.ToLower())));
            });
        }
    }
}
