using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.Core;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.PatientList.PatientFile.Prescriptions.ReadyPrescription.ReadyPrescriptionEditor;
using Unity;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.Prescriptions.ReadyPrescription
{
    public partial class ReadyPrescriptionViewModel
    {

    }

    public partial class ReadyPrescriptionViewModel : BaseViewModel<ReadyPrescriptionView>
    {
        private readonly IUnityContainer _container;
        private readonly IReadyPrescriptionRepository _readyPrescriptionRepository;

        public ReadyPrescriptionViewModel(IUnityContainer container,
                    IReadyPrescriptionRepository readyPrescriptionRepository) {
            _container = container;
            _readyPrescriptionRepository = readyPrescriptionRepository;

            AddReadyPrescriptionCommand = new RelayCommand(AddReadyPrescription);
            EditReadyPrescriptionCommand = new RelayCommand(EditReadyPrescription);
            DeleteReadyPrescriptionCommand = new RelayCommand(DeleteReadyPrescription);
        }

        public override async Task Initialize() {
            ReadyPrescriptions = new ObservableCollection<Model.ReadyPrescriptionDto>(
                await _readyPrescriptionRepository.GetAll());
        }

        public ObservableCollection<Model.ReadyPrescriptionDto> ReadyPrescriptions { get; set; }
        public Model.ReadyPrescriptionDto SelectedPrescription { get; set; }


        public ICommand AddReadyPrescriptionCommand { get; set; }
        public ICommand EditReadyPrescriptionCommand { get; set; }
        public ICommand DeleteReadyPrescriptionCommand { get; set; }


        private void AddReadyPrescription() {
            BusyExecute(async () => {
                var editor = _container.Resolve<ReadyPrescriptionEditorViewModel>();
                await editor.Initialize();

                _container.Resolve<IDialogService>().ShowEditorDialog(editor.GetView() as ReadyPrescriptionEditorView,
                    async () => {
                        if (editor.ReadyPrescriptionMedicines.IsNullOrEmpty())
                            return false;

                        var readyPrescription = editor.BuildFromProperties();
                        await _readyPrescriptionRepository.Add(readyPrescription);
                        await Initialize();
                        return true;
                    });
            });
        }

        private void EditReadyPrescription() {
            if (SelectedPrescription == null)
                return;

            BusyExecute(async () => {
                var editor = _container.Resolve<ReadyPrescriptionEditorViewModel>();
                await editor.Initialize();
                editor.Fill(SelectedPrescription);

                _container.Resolve<IDialogService>().ShowEditorDialog(editor.GetView() as ReadyPrescriptionEditorView,
                    async () => {
                        if (editor.ReadyPrescriptionMedicines.IsNullOrEmpty())
                            return false;

                        var readyPrescription = editor.BuildFromProperties();
                        await _readyPrescriptionRepository.Update(readyPrescription);
                        await Initialize();
                        return true;
                    });
            });
        }

        private void DeleteReadyPrescription() {
            if (SelectedPrescription == null)
                return;

            BusyExecute(async () => {
                await _readyPrescriptionRepository.Delete(e => e.Id == SelectedPrescription.Id);
                ReadyPrescriptions.Remove(SelectedPrescription);
            });
        }
    }
}
