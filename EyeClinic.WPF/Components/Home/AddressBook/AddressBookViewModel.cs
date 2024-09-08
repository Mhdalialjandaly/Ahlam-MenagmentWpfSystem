using System.Collections.ObjectModel;
using System.Threading.Tasks;
using EyeClinic.Model;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Home.AddressBook.ContactAccountPaymentList;
using EyeClinic.WPF.Components.Home.AddressBook.ContactList;
using Unity;

namespace EyeClinic.WPF.Components.Home.AddressBook
{
    public partial class AddressBookViewModel
    {
        public AddressBookViewModel() { }
    }

    public partial class AddressBookViewModel : BaseViewModel<AddressBookView>
    {
        private readonly IUnityContainer _container;

        public AddressBookViewModel(IUnityContainer container) {
            _container = container;
        }

        public override async Task Initialize() {
            var contactList = _container.Resolve<ContactListViewModel>();
            await contactList.Initialize();

            var contactAccountPayments = _container.Resolve<ContactAccountPaymentListViewModel>();

            contactList.OnSelectedContactChanged += async (_, contact) => {
                if (contact == null) {
                    contactAccountPayments.ContactPayments =
                        new ObservableCollection<ContactAccountPaymentDto>();
                    contactAccountPayments.SelectedContactPayment = null;
                    return;
                }

                await contactAccountPayments.Initialize(contact.Id, contact.ContactName);
            };

            ContactList = contactList.GetView() as ContactListView;
            ContactAccountPayments = contactAccountPayments.GetView() as ContactAccountPaymentListView;
        }

        public ContactListView ContactList { get; set; }
        public ContactAccountPaymentListView ContactAccountPayments { get; set; }
    }
}
