using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.Core.Enums;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Home.AddressBook.ContactAccountCategoryList;
using EyeClinic.WPF.Components.Home.AddressBook.ContactAccountList;
using Unity;
using operation = EyeClinic.Core.Enums.Operation;
namespace EyeClinic.WPF.Components.Home.AddressBook.ContactAccountPaymentList.ContactAccountPaymentEditor
{
    public partial class ContactAccountPaymentEditorViewModel
    {
        public ContactAccountPaymentEditorViewModel() { }
    }

    public partial class ContactAccountPaymentEditorViewModel : BaseViewModel<ContactAccountPaymentEditorView>
    {
        private readonly IContactAccountPaymentRepository _contactAccountPaymentRepository;
        private readonly IContactAccountRepository _contactAccountRepository;
        private readonly INotificationService _notificationService;
        private readonly IUnityContainer _container;

        public ContactAccountPaymentEditorViewModel(IContactAccountPaymentRepository contactAccountPaymentRepository,
            IContactAccountRepository contactAccountRepository, INotificationService notificationService,
            IUnityContainer container) {
            _contactAccountPaymentRepository = contactAccountPaymentRepository;
            _contactAccountRepository = contactAccountRepository;
            _notificationService = notificationService;
            _container = container;

            ManageCategoryCommand = new RelayCommand(ManageCategory);
            PayTypes = new() { "Pay", "Receive" };

            PaymentDate = DateTime.Now.Date;
        }

        public async Task Initialize(int contactId) {
            ContactId = contactId;

            var accounts = await _contactAccountRepository
                .GetByKey(e => e.ContactId == contactId);
            ContactAccounts = new ObservableCollection<ContactAccountDto>(accounts);

            var categories = await _container.Resolve<IAccountPaymentCategoryRepository>()
                .GetAll();
            AccountCategories = new ObservableCollection<AccountPaymentCategoryDto>(categories);
        }

        public ObservableCollection<ContactAccountDto> ContactAccounts { get; set; }
        public ContactAccountDto SelectedContactAccount { get; set; }

        public ObservableCollection<AccountPaymentCategoryDto> AccountCategories { get; set; }
        public AccountPaymentCategoryDto SelectedAccountCategory { get; set; }

        public List<string> PayTypes { get; set; }
        public string SelectedPayType { get; set; }

        public int Id { get; set; }
        public int? ContactAccountId { get; set; }
        public int ContactId { get; set; }
        public DateTime PaymentDate { get; set; }
        public int PaymentValue { get; set; }
        public bool PayoutTransaction { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }


        public ICommand ManageCategoryCommand { get; set; }

        

        private void ManageCategory() {
            BusyExecute(async () => {
                var editor = _container.Resolve<ContactAccountCategoryListViewModel>();
                await editor.Initialize();
                _container.Resolve<IDialogService>()
                    .ShowInformationDialog(editor.GetView() as ContactAccountCategoryListView,
                        ReloadAccounts);
            });
        }

        private void ReloadAccounts() {
            BusyExecute(async () => await Initialize(ContactId));
        }

        public async Task<ContactAccountPaymentDto> SaveAsync() {
            if (!ValidForSave())
                return null;

            var contact = BuildFromProperties();
            contact.ContactAccountId = SelectedContactAccount?.Id;

            if (Operation == operation.Add) {
                var paymentItem = await _contactAccountPaymentRepository.Add(contact);
                return paymentItem;
            }

            if (Operation == operation.Edit && Id > 0) {
                contact.Id = Id;
                contact.CreatedDate = CreatedDate;
                contact.LastModifiedDate = DateTime.Now;
                await _contactAccountPaymentRepository.Update(contact);
                return contact;
            }

            return null;
        }

        private ContactAccountPaymentDto BuildFromProperties() {
            return new() {
                Id = Id,
                ContactAccountId = ContactAccountId,
                CategoryId = 1,
                ContactId = ContactId,
                PaymentDate = PaymentDate,
                PaymentValue = PaymentValue,
                PayoutTransaction = SelectedPayType == "Pay",
                Notes = Notes,
                CreatedDate = CreatedDate
            };
        }

        private bool ValidForSave() {
            if (PaymentValue <= 0) {
                _notificationService.Warning("Payment value should be more than zero");
                return false;
            }
            if (PaymentDate == DateTime.MinValue) {
                _notificationService.Warning("Payment Date should be correct");
                return false;
            }

            if (SelectedPayType == null)
                return false;

            return true;
        }

        public void BuildFromModel(ContactAccountPaymentDto selectedContact) {
            Id = selectedContact.Id;
            ContactAccountId = selectedContact.ContactAccountId;
            ContactId = selectedContact.ContactId;
            PaymentDate = selectedContact.PaymentDate;
            PaymentValue = selectedContact.PaymentValue;
            PayoutTransaction = selectedContact.PayoutTransaction;
            Notes = selectedContact.Notes;
            CreatedDate = selectedContact.CreatedDate;

            SelectedPayType = selectedContact.PayoutTransaction ? PayTypes.FirstOrDefault() : PayTypes.LastOrDefault();
            SelectedContactAccount = ContactAccounts.FirstOrDefault(e => e.Id == selectedContact.ContactAccountId);
            SelectedAccountCategory = AccountCategories.FirstOrDefault(e => e.Id == selectedContact.CategoryId);
        }
    }
}
