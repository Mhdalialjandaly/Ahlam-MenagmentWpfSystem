using EyeClinic.Core;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.Localization;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Dialogs.PasswordInput;
using EyeClinic.WPF.Components.Home.Operation.PublicCustomer.PublicCustomerEditer;
using EyeClinic.WPF.Components.Home.Operation.PublicCustomerFile;
using EyeClinic.WPF.Components.PatientList;
using EyeClinic.WPF.Setup;
using Syncfusion.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;
using operation = EyeClinic.Core.Enums.Operation;

namespace EyeClinic.WPF.Components.Home.Operation.PublicCustomer
{
   
        public partial class PublicCustomerViewModel
        {
            public PublicCustomerViewModel()
            {
                if (IsDesignMode) { }
            }
        }

        public partial class PublicCustomerViewModel : BaseViewModel<PublicCustomerView>
        {
           
            private readonly IUnityContainer _container;
            private readonly IDialogService _dialogService;
            private readonly IPublicCustomerRepository _publicCustomerRepository;
  
            public PublicCustomerViewModel( IUnityContainer container,
                IDialogService dialogService,IPublicCustomerRepository publicCustomerRepository)
            {
                _container = container;
                _dialogService = dialogService;
            _publicCustomerRepository = publicCustomerRepository;

                SearchCommand = new RelayCommand(Search);
                AddToQueueCommand = new RelayCommand<RecordEntry>(AddToQueue);
                AddPublicCustomerCommand = new RelayCommand(AddPublicCustomer);
            EditPublicCustomerCommand = new RelayCommand(EditPublicCustomer);
                DeletePublicCustomerCommand = new RelayCommand(DeletePublicCustomer);
                BackHomeCommand = new RelayCommand(BackHome);
                GoPatientFileCommand = new RelayCommand<RecordEntry>(GoPublicCustomerFile);

                ShowDeleteButton = Global.DeviceName.ToLower() == "clinic";
            }


            public ObservableCollection<PublicCustomerDto> PublicCustomers { get; set; }

            private RecordEntry _selectedPublicCustomer;
            public RecordEntry SelectedPublicCustomer
        {
                get => _selectedPublicCustomer;
                set
                {
                _selectedPublicCustomer = value;
                    if (value is { Data: PublicCustomerDto SelectedPublicCustomer })
                    {
                        OnSelectPublicCustomer?.Invoke(this, SelectedPublicCustomer);
                    }
                }
            }

            public bool ShowDeleteButton { set; get; }
            public string Code { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
         


            public bool EnableSearchByImageNumber { get; set; }

            public event EventHandler<PublicCustomerDto> OnSelectPublicCustomer;
            public event EventHandler<PublicCustomerDto> OnAddPublicCustomerToQueue;


            public ICommand SearchCommand { get; set; }
            public ICommand AddToQueueCommand { get; set; }
            public ICommand AddPublicCustomerCommand { get; set; }
            public ICommand EditPublicCustomerCommand { get; set; }
            public ICommand DeletePublicCustomerCommand { get; set; }

            public ICommand BackHomeCommand { get; set; }
            public ICommand GoPatientFileCommand { get; set; }


        
          public void Search()
        
          { 
            
                BusyExecute(async () => {

                    PublicCustomers = new ObservableCollection<PublicCustomerDto>(
                        await _publicCustomerRepository.Search(Code,FirstName,LastName).ConfigureAwait(true));


                    //SelectedPublicCustomer = null;
                    //OnAddPublicCustomerToQueue?.Invoke(this, null);
                });
           
          
          }

            public void AddToQueue(RecordEntry entry)
            {
                if (entry.Data is PublicCustomerDto selectedPublicCustomer)
                {

                    BusyExecute(async () => {
                        var exist = await _container.Resolve<IPublicCustomerOrderRepository>().GetByKey(
                            e => e.OrderDate.Date == DateTime.Now.Date &&
                                 e.PublicCustomerId == selectedPublicCustomer.Id);
                        if (!exist.IsNullOrEmpty())
                        {
                            _container.Resolve<IDialogService>()
                                .ShowInformationDialog("This Customer already treated");
                            return;
                        }

                        OnAddPublicCustomerToQueue?.Invoke(this, selectedPublicCustomer);
                    });
                }


            }

            public void AddPublicCustomer()
            {
            BusyExecute(async () =>
            {
                var editor = _container.Resolve<PublicCustomerEditerViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Add;
                _dialogService.ShowEditorDialog(editor.GetView() as PublicCustomerEditerView,
                    async () =>
                    {
                        var patient = await editor.SaveAsync();
                        if (patient == null)
                            return false;

                        PublicCustomers = new ObservableCollection<PublicCustomerDto> { patient };
                        return true;
                    });
            });
        }

            public void EditPublicCustomer()
            {
            if (SelectedPublicCustomer == null)
                return;
            if (SelectedPublicCustomer.Data is PublicCustomerDto SelectedpublicCustomer)
            {
                if (SelectedpublicCustomer.Id == 0)
                    return;

                BusyExecute(async () =>
                {
                    var editor = _container.Resolve<PublicCustomerEditerViewModel>();
                    await editor.Initialize();
                    editor.Operation = operation.Edit;
                    editor.Id = SelectedpublicCustomer.Id;
                    editor.BuildFromModel(SelectedpublicCustomer);
                    _dialogService.ShowEditorDialog(editor.GetView() as PublicCustomerEditerView,
                        async () =>
                        {
                            var publiccustomer = await editor.SaveAsync();
                            if (publiccustomer == null)
                                return false;

                            PublicCustomers ??= new ObservableCollection<PublicCustomerDto>();
                            PublicCustomers.UpdateItem(SelectedpublicCustomer, publiccustomer);
                            return true;
                        });
                });
            }
        }

            private void DeletePublicCustomer()
            {

            if (!_container.CheckUserRoleSilent(UserRoles.Admin))
            {
                _container.Resolve<INotificationService>()
                .Warning("You have no permission");
                return;
            }
            if (SelectedPublicCustomer == null)
                return;

            if (SelectedPublicCustomer.Data is PublicCustomerDto Publiccustomer)
            {
                if (Publiccustomer.Id == 0)
                    return;

                _dialogService.ShowConfirmationMessage("Are you Sure ?", async () =>
                {
                    try
                    {
                        await _publicCustomerRepository.Delete(Publiccustomer.Id);
                        if (SelectedPublicCustomer.Data is PublicCustomerDto selectedPubliccustomer)
                        {
                            PublicCustomers.Remove(selectedPubliccustomer);
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        _container.Resolve<INotificationService>()
                            .Error(ex.GetBaseException().Message);
                        return false;
                    }
                });
            }
        }

            private void BackHome()
            {
                if (_dialogService.IsPopupActivated())
                {
                    _dialogService.DisposeLastDialog();
                    return;
                }

                BusyExecute(async () => {
                    await _container.BackHome();
                });
            }

            public void GoPublicCustomerFile(RecordEntry record)
        {
            if (record.Data is PublicCustomerDto publicCustomer)
            {
                if (publicCustomer.Id == 0)
                    _container.Resolve<INotificationService>()
                        .Warning(_container.Resolve<ILocalizationService>().Localize("Customer"));

                BusyExecute(async () =>
                {
                    var publicCustomerFile = _container.Resolve<PublicCustomerFileViewModel>();
                    var lastOrder = await _container.Resolve<IPublicCustomerOrderRepository>()
                        .GetLastOrderByPublicCustomerId(publicCustomer.Id);

                    if (lastOrder == null)
                    {
                        _container.Resolve<INotificationService>()
                            .Error(_container.Resolve<ILocalizationService>().Localize("PatientFileHasNoData"));
                        return;
                    }

                    await publicCustomerFile.Initialize(publicCustomer.Id, lastOrder.Id, true);
                    _dialogService.ShowFullScreenPopupContent(publicCustomerFile.GetView() as PublicCustomerFileView);
                    //_container.Navigate(patientFile.GetView() as PatientFileView);
                });
            }
        }
        }
    
}
