using System;
using System.Threading.Tasks;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.Base;
using operation = EyeClinic.Core.Enums.Operation;

namespace EyeClinic.WPF.Components.Home.AddressBook.ContactList.ContactEditor
{
    public class ContactEditorViewModel : BaseViewModel<ContactEditorView>
    {
        private readonly IContactRepository _contactRepository;

        public ContactEditorViewModel(IContactRepository contactRepository) {
            _contactRepository = contactRepository;
        }


        public int Id { get; set; }
        public string ContactName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string ContactPhones { get; set; }
        public DateTime CreatedDate { get; set; }
        public string WebSite { get; set; }
        public string CompanyType { get; set; }
        public string EmailAdress { get; set; }
        public string Note { get; set; }


        public async Task<ContactDto> Save() {
            if (!ValidForSave())
                return null;

            var contact = BuildFromProperties();

            if (Operation == operation.Add) {
                var paymentItem = await _contactRepository.Add(contact);
                return paymentItem;
            }

            if (Operation == operation.Edit && Id > 0) {
                contact.Id = Id;
                contact.CreatedDate = CreatedDate;
                contact.LastModifiedDate = DateTime.Now;
                await _contactRepository.Update(contact);
                return contact;
            }

            return null;
        }

        private ContactDto BuildFromProperties() {
            return new() {
                ContactName = ContactName,
                ContactPhones = ContactPhones,
                Address = Address,
                Phone = Phone,
                EmailAdress = EmailAdress,
                CompanyType = CompanyType,
                WebSite = WebSite,
                note = Note
            };
        }

        private bool ValidForSave() {
            if (string.IsNullOrWhiteSpace(ContactName) ||
                string.IsNullOrWhiteSpace(Phone))
                return false;

            return true;
        }

        public void BuildFromModel(ContactDto selectedContact) {
            Id = selectedContact.Id;
            ContactName = selectedContact.ContactName;
            ContactPhones = selectedContact.ContactPhones;
            Address = selectedContact.Address;
            Phone = selectedContact.Phone;
            CreatedDate = selectedContact.CreatedDate;
            CompanyType=selectedContact.CompanyType;
            EmailAdress = selectedContact.EmailAdress;
            Note = selectedContact.note;
            WebSite = selectedContact.WebSite;
        }
    }
}
