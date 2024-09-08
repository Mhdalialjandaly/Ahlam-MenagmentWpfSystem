using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.Core.Enums;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.Base;
using Unity;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.Operations.OperationList.OperationEditor.OperationSelector
{
    public class OperationSelectorViewModel : BaseViewModel<OperationSelectorView>
    {
        private readonly IUnityContainer _container;
        private readonly IOperationRepository _operationRepository;

        public OperationSelectorViewModel(IUnityContainer container,
            IOperationRepository operationRepository) {
            _container = container;
            _operationRepository = operationRepository;
            AddOperationCommand = new RelayCommand(AddOperation);
        }

        public override async Task Initialize() {
            Operations = await _operationRepository.GetAll();
        }

        public List<Model.OperationDto> Operations { get; set; }
        public OperationDto RightEyeOperation { get; set; }
        public OperationDto LeftEyeOperation { get; set; }

        public string RightEyeLensType { get; set; }
        public string RightEyeLens { get; set; }
        public string RightEyeNote { get; set; }
        public ICommand AddOperationCommand { get; set; }


        private void AddOperation() {
            BusyExecute(async () => {
                var editor = _container.Resolve<Home.Setting.Operations.OperationEditor.OperationEditorViewModel>();
                await editor.Initialize();
                editor.Operation = Operation.Add;

                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as Home.Setting.Operations.OperationEditor.OperationEditorView, async () => {
                        var item = await editor.Save();
                        if (item == null)
                            return false;

                        Operations.Add(item);
                        return true;
                    });
            });
        }

    }
}
