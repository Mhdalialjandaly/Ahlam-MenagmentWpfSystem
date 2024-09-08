using EyeClinic.Core;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Dialogs.PasswordInput;
using EyeClinic.WPF.Components.Home.CartoonForm.Customers.CustomerEditer;
using EyeClinic.WPF.Setup;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;
using operation = EyeClinic.Core.Enums.Operation;
namespace EyeClinic.WPF.Components.Home.CartoonForm.Customers
{
    public partial class CustomerViewModel 
    {
        public CustomerViewModel()
        {
            if (IsDesignMode) { }
        }
    }
    public partial class CustomerViewModel : BaseViewModel<CustomerView>
    {
        private IUnityContainer _container;
        public readonly  INotificationService _notificationService;
        public readonly ICustomerRepository _customerrepository;
        private readonly IDialogService _dialogService;
        public CustomerViewModel(INotificationService notificationService, 
            ICustomerRepository customerRepository,
            IUnityContainer container,IDialogService dialogService)
        {
            _dialogService = dialogService;
            _notificationService = notificationService;
            _customerrepository = customerRepository;
            _container = container;

            AddDiseaseCommand = new RelayCommand(AddDisease);
            EditDiseaseCommand = new RelayCommand(EditDisease);
            DeleteCustomerCommand = new RelayCommand(DeleteDisease);
            SearchCommand = new RelayCommand(Search);
            BackCommand = new RelayCommand(GoBack);
        }

        

        public override async Task Initialize()
        {
            DiseasesList1 = new ObservableCollection<CustomerDto>(
                  await _customerrepository.GetAll());             
        }

        public event EventHandler<CustomerDto> OnSelectedContactChanged;
        private CustomerDto _selectedContact;

        public CustomerDto SelectedContact
        {
            get => _selectedContact;
            set
            {
                _selectedContact = value;
                OnSelectedContactChanged?.Invoke(this, value);
            }
        }

        public ObservableCollection<CustomerDto> DiseasesList1 { get; set; }
        public CustomerDto SelectedDisease { get; set; }

        public string SearchText { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand AddDiseaseCommand { get; set; }
        public ICommand EditDiseaseCommand { get; set; }
        public ICommand DeleteCustomerCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand ShowItemsCommand { set; get; }

        private void AddDisease()
        {
            BusyExecute(async () =>
            {
                var editor = _container.Resolve<CustomerEditerViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Add;

                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as CustomerEditerView, async () =>
                    {
                        var item = await editor.Save();
                        if (item == null)
                            return false;

                        DiseasesList1.Add(item);
                        return true;
                    });
            });

        }

        private void EditDisease()
        {
            if (SelectedDisease == null)
                return;

            BusyExecute(async () =>
            {
                var editor = _container.Resolve<CustomerEditerViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Edit;
                editor.BuildFromModel(SelectedDisease);

                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as CustomerEditerView, async () =>
                    {
                        var item = await editor.Save();
                        if (item == null)
                            return false;

                        DiseasesList1.UpdateItem(SelectedDisease, item);
                        return true;
                    });
            });
        }

        private void DeleteDisease()
        {
            var pwd = _container.Resolve<PasswordInputViewModel>();
            pwd.DisposeOnLogin = false;
            pwd.CustomPassword = "54425";
            pwd.OnSuccessLogin += () =>
            {
                if (SelectedDisease == null)
                    return;                
                    _dialogService.DisposeLastDialog();
                    BusyExecute(async () =>
                    {
                        await _customerrepository.Delete(e => e.Id == SelectedDisease.Id);
                        DiseasesList1.Remove(SelectedDisease);
                    });                
            };
            _container.Resolve<IDialogService>()
                   .ShowPopupContent(pwd.GetView() as PasswordInputView);

          
        }

        private void Search()
        {
            BusyExecute(async () =>
            {
                SearchText ??= "";

                DiseasesList1 = new ObservableCollection<CustomerDto>(
                    await _customerrepository
                        .GetByKey(e =>
                            e.AccountName.ToLower().Contains(SearchText.ToLower())));
            });
        }
     
            public void GoBack()
        {
            
            BusyExecute(async () =>
            {
                var cartoonView = _container.Resolve<CartoonViewModel>();
                await cartoonView.Initialize();
                _container.Navigate(cartoonView.GetView() as CartoonView);
            });
        }
        

    }

}
