using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Home.AddressBook.ContactAccountList;
using EyeClinic.WPF.Components.Home.AddressBook.ContactAccountPaymentList.ContactAccountPaymentEditor;
using Microsoft.EntityFrameworkCore;
using Unity;

namespace EyeClinic.WPF.Components.Home.AddressBook.ContactAccountPaymentList
{
    public partial class ContactAccountPaymentListViewModel
    {
        public ContactAccountPaymentListViewModel() { }
    }

    public partial class ContactAccountPaymentListViewModel : BaseViewModel<ContactAccountPaymentListView>
    {
        private readonly IContactAccountPaymentRepository _contactAccountPaymentRepository;
        private readonly IDialogService _dialogService;
        private readonly IContactAccountRepository _contactAccountRepository;
        private readonly IUnityContainer _container;

        public ContactAccountPaymentListViewModel(IContactAccountPaymentRepository contactAccountPaymentRepository,
            IDialogService dialogService,
            IContactAccountRepository contactAccountRepository,
            IUnityContainer container) {
            _contactAccountPaymentRepository = contactAccountPaymentRepository;
            _dialogService = dialogService;
            _contactAccountRepository = contactAccountRepository;
            _container = container;

            AddContactCommand = new RelayCommand(AddContact);
            EditContactCommand = new RelayCommand(EditContact);
            DeleteContactCommand = new RelayCommand<ContactAccountPaymentDto>(DeleteContact);
        }

        public async Task Initialize(int contactId, string contactName) {
            ContactId = contactId;
            ContactName = contactName;

            ManageContactAccountCommand = new RelayCommand(ManageContactAccount);

            var accounts = await _contactAccountRepository
                .GetByKey(e => e.ContactId == contactId);
            Accounts = new ObservableCollection<ContactAccountDto>(accounts);
            Accounts.Insert(0, new ContactAccountDto() {
                Id = -1,
                AccountName = "No Account"
            });
        }


        public ObservableCollection<ContactAccountPaymentDto> ContactPayments { get; set; }
        public ContactAccountPaymentDto SelectedContactPayment { get; set; }


        public ObservableCollection<ContactAccountDto> Accounts { get; set; }

        private ContactAccountDto _selectedAccount;

        public ContactAccountDto SelectedAccount {
            get { return _selectedAccount; }
            set {
                _selectedAccount = value;
                GetPaymentsByAccount(value);
            }
        }

        private void GetPaymentsByAccount(ContactAccountDto value) {
            BusyExecute(async () => {
                if (value == null) {
                    ContactPayments = new ObservableCollection<ContactAccountPaymentDto>();
                } else {
                    if (value.Id == -1) {
                        // Get payments without account
                        var contactPayments = _container.Resolve<IMapper>()
                            .Map<List<ContactAccountPaymentDto>>(
                                await _contactAccountPaymentRepository
                                    .Get().AsNoTracking()
                                    .Include(e => e.ContactAccount)
                                    .Include(e => e.Category)
                                    .Where(e => e.DeletedDate == null)
                                    .Where(e => e.ContactAccount.DeletedDate == null)
                                    .Where(e => e.ContactAccountId == null &&
                                                e.ContactId == ContactId)
                                    .ToListAsync());
                        ContactPayments = new ObservableCollection<ContactAccountPaymentDto>(
                            contactPayments.OrderByDescending(e => e.PaymentDate).ToList());
                    } else {
                        var contactPayments = _container.Resolve<IMapper>()
                            .Map<List<ContactAccountPaymentDto>>(
                                await _contactAccountPaymentRepository
                                    .Get().AsNoTracking()
                                    .Include(e => e.ContactAccount)
                                    .Include(e => e.Category)
                                    .Where(e => e.DeletedDate == null)
                                    .Where(e => e.ContactAccount.DeletedDate == null)
                                    .Where(e => e.ContactAccountId == value.Id)
                                    .ToListAsync());
                        ContactPayments = new ObservableCollection<ContactAccountPaymentDto>(
                            contactPayments.OrderByDescending(e => e.PaymentDate).ToList());
                    }
                }
            });
        }


        public int ContactId { get; set; }
        public string ContactName { get; set; }

        public ICommand AddContactCommand { get; set; }
        public ICommand EditContactCommand { get; set; }
        public ICommand DeleteContactCommand { get; set; }
        public ICommand ManageContactAccountCommand { get; set; }


        private void ManageContactAccount() {
            BusyExecute(async () => {
                var editor = _container.Resolve<ContactAccountListViewModel>();
                await editor.Initialize(ContactId);
                _container.Resolve<IDialogService>()
                    .ShowInformationDialog(editor.GetView() as ContactAccountListView);
            });
        }

        private void AddContact() {
            BusyExecute(async () => {
                var editor = _container.Resolve<ContactAccountPaymentEditorViewModel>();
                await editor.Initialize(ContactId);
                editor.Operation = Core.Enums.Operation.Add;
                if (SelectedAccount != null) {
                    editor.SelectedContactAccount = editor.ContactAccounts
                        .FirstOrDefault(e => e.Id == SelectedAccount.Id);
                    editor.SelectedPayType = SelectedAccount.PayOutAccount
                        ? editor.PayTypes.FirstOrDefault()
                        : editor.PayTypes.LastOrDefault();
                }

                _dialogService.ShowEditorDialog(editor.GetView() as ContactAccountPaymentEditorView,
                    async () => {
                        var contact = await editor.SaveAsync();
                        if (contact == null)
                            return false;

                        await Initialize(ContactId, ContactName);
                        return true;
                    });
            });
        }

        private void EditContact() {
            if (SelectedContactPayment == null)
                return;

            BusyExecute(async () => {
                var editor = _container.Resolve<ContactAccountPaymentEditorViewModel>();
                await editor.Initialize(ContactId);
                editor.Operation = Core.Enums.Operation.Edit;
                editor.Id = SelectedContactPayment.Id;
                editor.BuildFromModel(SelectedContactPayment);
                _dialogService.ShowEditorDialog(editor.GetView() as ContactAccountPaymentEditorView,
                    async () => {
                        var contact = await editor.SaveAsync();
                        if (contact == null)
                            return false;

                        await Initialize(ContactId, ContactName);
                        return true;
                    });
            });
        }

        private void DeleteContact(ContactAccountPaymentDto contact) {
            _dialogService.ShowConfirmationMessage("Are you sure ?", async () => {
                await _contactAccountPaymentRepository.Delete(contact.Id);
                ContactPayments.Remove(contact);

                _container.Resolve<INotificationService>()
                    .Success("Deleted");
                return true;
            });
        }

    }
}
