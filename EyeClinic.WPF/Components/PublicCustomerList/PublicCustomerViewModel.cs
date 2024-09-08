using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.Localization;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Dialogs.PasswordInput;
using Syncfusion.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EyeClinic.WPF.Components.PublicCustomerList
{
    public partial class PublicCustomerViewModel
    {
        public PublicCustomerViewModel() { }
    }
    public partial class PublicCustomerViewModel : BaseViewModel<PublicCustomerListView>
    {
        private readonly INotificationService _notificationService;
        public PublicCustomerViewModel(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public ObservableCollection<PatientDto> PatientList { get; set; }

        private RecordEntry _selectedPatient;
        public RecordEntry SelectedPatient
        {
            get => _selectedPatient;
            set
            {
                _selectedPatient = value;
                if (value is { Data: PatientDto selectedPatient })
                {
                    OnSelectPatient?.Invoke(this, selectedPatient);
                }
            }
        }

        public bool ShowDeleteButton { set; get; }
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageNumber { get; set; }


        public bool EnableSearchByImageNumber { get; set; }

        public event EventHandler<PatientDto> OnSelectPatient;
        public event EventHandler<PatientDto> OnAddPatientToQueue;


        public ICommand SearchCommand { get; set; }
        public ICommand AddToQueueCommand { get; set; }
        public ICommand AddPatientCommand { get; set; }
        public ICommand EditPatientCommand { get; set; }
        public ICommand DeletePatientCommand { get; set; }

        public ICommand BackHomeCommand { get; set; }
        public ICommand GoPatientFileCommand { get; set; }


        public void Search()
        {
            //Task.Run(() =>
            //{
            //    BusyExecute(async () =>
            //    {
            //        if (string.IsNullOrWhiteSpace(ImageNumber))
            //        {
            //            PatientList = new ObservableCollection<PatientDto>(
            //                await _patientRepository.Search(Code, FirstName, LastName, EnableSearchByImageNumber)
            //                    .ConfigureAwait(false));
            //        }
            //        else
            //        {
            //            PatientList = new ObservableCollection<PatientDto>(
            //                await _patientRepository.SearchByImageNumber(ImageNumber)
            //                    .ConfigureAwait(false));
            //        }

            //        SelectedPatient = null;
            //        OnSelectPatient?.Invoke(this, null);
            //    });
            //});
        }

        public void AddToQueue(RecordEntry entry)
        {
            //if (entry.Data is PatientDto selectedPatient)
            //{

            //    BusyExecute(async () =>
            //    {
            //        var exist = await _container.Resolve<IPatientVisitRepository>().GetByKey(
            //            e => e.VisitDate.Date == DateTime.Now.Date &&
            //                 e.PatientId == selectedPatient.Id);
            //        if (!exist.IsNullOrEmpty())
            //        {
            //            _container.Resolve<IDialogService>()
            //                .ShowInformationDialog("This patient already treated");
            //            return;
            //        }

            //        OnAddPatientToQueue?.Invoke(this, selectedPatient);
            //    });
            //}


        }

        public void AddPatient()
        {
            //BusyExecute(async () =>
            //{
            //    var editor = _container.Resolve<PatientEditorViewModel>();
            //    await editor.Initialize();
            //    editor.Operation = Operation.Add;
            //    _dialogService.ShowEditorDialog(editor.GetView() as PatientEditorView,
            //        async () =>
            //        {
            //            var patient = await editor.SaveAsync();
            //            if (patient == null)
            //                return false;

            //            PatientList = new ObservableCollection<PatientDto> { patient };
            //            return true;
            //        });
            //});
        }

        public void EditPatient()
        {
            //if (SelectedPatient == null)
            //    return;
            //if (SelectedPatient.Data is PatientDto selectedPatient)
            //{
            //    if (selectedPatient.Id == 0)
            //        return;

            //    BusyExecute(async () =>
            //    {
            //        var editor = _container.Resolve<PatientEditorViewModel>();
            //        await editor.Initialize();
            //        editor.Operation = Operation.Edit;
            //        editor.Id = selectedPatient.Id;
            //        editor.BuildFromModel(selectedPatient);
            //        _dialogService.ShowEditorDialog(editor.GetView() as PatientEditorView,
            //            async () =>
            //            {
            //                var patient = await editor.SaveAsync();
            //                if (patient == null)
            //                    return false;

            //                PatientList ??= new ObservableCollection<PatientDto>();
            //                PatientList.UpdateItem(selectedPatient, patient);
            //                return true;
            //            });
            //    });
            //}
        }

        private void DeletePatient()
        {

            //if (!_container.CheckUserRoleSilent(UserRoles.Admin))
            //{
            //    _container.Resolve<INotificationService>()
            //    .Warning("You have no permission");
            //    return;
            //}
            //if (SelectedPatient == null)
            //    return;

            //if (SelectedPatient.Data is PatientDto patient)
            //{
            //    if (patient.Id == 0)
            //        return;

            //    _dialogService.ShowConfirmationMessage("Are you Sure ?", async () =>
            //    {
            //        try
            //        {
            //            await _patientRepository.Delete(patient.Id);
            //            if (SelectedPatient.Data is PatientDto selectedPatient)
            //            {
            //                PatientList.Remove(selectedPatient);
            //            }

            //            return true;
            //        }
            //        catch (Exception ex)
            //        {
            //            _container.Resolve<INotificationService>()
            //                .Error(ex.GetBaseException().Message);
            //            return false;
            //        }
            //    });
            //}
        }

        private void BackHome()
        {
            //    if (_dialogService.IsPopupActivated())
            //    {
            //        _dialogService.DisposeLastDialog();
            //        return;
            //    }

            //    BusyExecute(async () =>
            //    {
            //        await _container.BackHome();
            //    });
        }

        public void GoPatientFile(RecordEntry record)
        {
            //if (record.Data is PatientDto patient)
            //{
            //    if (patient.Id == 0)
            //        _container.Resolve<INotificationService>()
            //            .Warning(_container.Resolve<ILocalizationService>().Localize("PatientFromExcelFile"));

            //    BusyExecute(async () =>
            //    {
            //        var patientFile = _container.Resolve<PatientFileViewModel>();
            //        var lastVisit = await _container.Resolve<IPatientVisitRepository>()
            //            .GetLastVisitByPatientId(patient.Id);

            //        if (lastVisit == null)
            //        {
            //            _container.Resolve<INotificationService>()
            //                .Error(_container.Resolve<ILocalizationService>().Localize("PatientFileHasNoData"));
            //            return;
            //        }

            //        await patientFile.Initialize(patient.Id, lastVisit.Id, true);
            //        _dialogService.ShowFullScreenPopupContent(patientFile.GetView() as PatientFileView);
            //        //_container.Navigate(patientFile.GetView() as PatientFileView);
            //    });
            //}
        }
    }
}
