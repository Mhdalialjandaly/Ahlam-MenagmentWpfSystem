using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.Core;
using EyeClinic.Core.Enums;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Home.Setting.Diagnosis.DiagnosisEditor;
using EyeClinic.WPF.Components.Home.Setting.Operations.OperationEditor;
using Unity;
using operation = EyeClinic.Core.Enums.Operation;
namespace EyeClinic.WPF.Components.Home.Setting.Operations
{
    public partial class OperationViewModel
    {
        public OperationViewModel() { }
    }

    public partial class OperationViewModel : BaseViewModel<OperationView>
    {
        private readonly IUnityContainer _container;
        private readonly IOperationRepository _operationRepository;

        public OperationViewModel(IUnityContainer container, IOperationRepository operationRepository) {
            _container = container;
            _operationRepository = operationRepository;

            AddOperationCommand = new RelayCommand(AddOperation);
            EditDiseaseCommand = new RelayCommand(EditDisease);
            DeleteDiseaseCommand = new RelayCommand<Model.OperationDto>(DeleteDisease);
            SearchCommand = new RelayCommand(Search);
        }

        public override async Task Initialize() {
            OperationNameList = new ObservableCollection<Model.OperationDto>(
                await _operationRepository.GetAll());
        }


        public ObservableCollection<Model.OperationDto> OperationNameList { get; set; }
        public Model.OperationDto SelectedDisease { get; set; }

        public string SearchText { get; set; }

        public ICommand SearchCommand { get; set; }
        public ICommand AddOperationCommand { get; set; }
        public ICommand EditDiseaseCommand { get; set; }
        public ICommand DeleteDiseaseCommand { get; set; }


        private void AddOperation() {
            BusyExecute(async () => {
                var editor = _container.Resolve<OperationEditorViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Add;

                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as OperationEditorView, async () => {
                        var item = await editor.Save();
                        if (item == null)
                            return false;

                        OperationNameList.Add(item);
                        return true;
                    });
            });
        }

        private void EditDisease() {
            if (SelectedDisease == null)
                return;

            BusyExecute(async () => {
                var editor = _container.Resolve<OperationEditorViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Edit;
                editor.BuildFromModel(SelectedDisease);

                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as OperationEditorView, async () => {
                        var item = await editor.Save();
                        if (item == null)
                            return false;

                        OperationNameList.UpdateItem(SelectedDisease, item);
                        return true;
                    });
            });
        }

        private void DeleteDisease(Model.OperationDto disease) {
            
            BusyExecute(async () => {
                await _operationRepository.Delete(e => e.Id == disease.Id);
                OperationNameList.Remove(disease);
            });
        }

        private void Search() {
            BusyExecute(async () => {
                SearchText ??= "";

                OperationNameList = new ObservableCollection<Model.OperationDto>(
                    await _operationRepository
                        .GetByKey(e =>
                            e.EnName.ToLower().Contains(SearchText.ToLower()) ||
                            e.ArName.ToLower().Contains(SearchText.ToLower())
                            ));
            });
        }
    }
}
