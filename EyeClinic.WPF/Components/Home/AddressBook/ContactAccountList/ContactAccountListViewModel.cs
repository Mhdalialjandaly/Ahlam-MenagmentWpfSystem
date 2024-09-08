using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.Core.Enums;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Home.AddressBook.ContactAccountList.ContactAccountEditor;
using Unity;
using operation = EyeClinic.Core.Enums.Operation;
namespace EyeClinic.WPF.Components.Home.AddressBook.ContactAccountList
{
    public partial class ContactAccountListViewModel
    {
        public ContactAccountListViewModel() { }
    }

    public partial class ContactAccountListViewModel : BaseViewModel<ContactAccountListView>
    {
        private readonly IUnityContainer _container;
        private readonly IDialogService _dialogService;
        private readonly IContactAccountRepository _contactAccountRepository;

        public ContactAccountListViewModel(IUnityContainer container, IDialogService dialogService,
                    IContactAccountRepository contactAccountRepository) {
            _container = container;
            _dialogService = dialogService;
            _contactAccountRepository = contactAccountRepository;

            AddAccountCommand = new RelayCommand(AddAccount);
            EditAccountCommand = new RelayCommand(EditAccount);
            DeleteAccountCommand = new RelayCommand<ContactAccountDto>(DeleteAccount);
        }

        public async Task Initialize(int contactId) {
            ContactId = contactId;

            var accounts = await _contactAccountRepository
                .GetByKey(e => e.ContactId == contactId);
            ContactAccounts = new ObservableCollection<ContactAccountDto>(accounts);
        }

        public int ContactId { get; set; }


        public ObservableCollection<ContactAccountDto> ContactAccounts { get; set; }
        public ContactAccountDto SelectedContactAccount { get; set; }


        public ICommand AddAccountCommand { get; set; }
        public ICommand EditAccountCommand { get; set; }
        public ICommand DeleteAccountCommand { get; set; }


        private void AddAccount() {
            var editor = _container.Resolve<ContactAccountEditorViewModel>();
            editor.Initialize(ContactId);
            editor.Operation = operation.Add;
            _dialogService.ShowEditorDialog(editor.GetView() as ContactAccountEditorView, async () => {
                var account = await editor.Save();
                if (account == null)
                    return false;

                ContactAccounts.Insert(0, account);
                return true;
            });
        }

        private void EditAccount() {
            if (SelectedContactAccount == null) {
                _container.Resolve<INotificationService>().Warning("Please, Select Account to edit");
                return;
            }

            var editor = _container.Resolve<ContactAccountEditorViewModel>();
            editor.Initialize(ContactId);
            editor.BuildFromModel(SelectedContactAccount);
            editor.Operation = operation.Edit;
            _dialogService.ShowEditorDialog(editor.GetView() as ContactAccountEditorView, async () => {
                var account = await editor.Save();
                if (account == null)
                    return false;

                await Initialize(ContactId);
                return true;
            });
        }

        private void DeleteAccount(ContactAccountDto account) {
            _dialogService.ShowConfirmationMessage("Are you sure ?", async () => {
                await _contactAccountRepository.Delete(e => e.Id == account.Id);
                ContactAccounts.Remove(account);
                return true;
            });
        }
    }
}
