using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EyeClinic.Core.Enums;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.Base;
using operation = EyeClinic.Core.Enums.Operation;
namespace EyeClinic.WPF.Components.Home.AddressBook.ContactAccountList.ContactAccountEditor
{
    public partial class ContactAccountEditorViewModel
    {
        public ContactAccountEditorViewModel() { }
    }

    public partial class ContactAccountEditorViewModel : BaseViewModel<ContactAccountEditorView>, IEditorViewModel<ContactAccountDto>
    {
        private readonly IContactAccountRepository _contactAccountRepository;

        public ContactAccountEditorViewModel(IContactAccountRepository contactAccountRepository) {
            _contactAccountRepository = contactAccountRepository;
        }

        public void Initialize(int contactId) {
            ContactId = contactId;
            PayTypes = new() { "Pay", "Recieve" };
        }

        public int Id { get; set; }
        public int ContactId { get; set; }
        public string AccountName { get; set; }
        public int TotalCost { get; set; }
        public bool PayOutAccount { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }


        public List<string> PayTypes { get; set; }
        public string SelectedPayType { get; set; }


        public async Task<ContactAccountDto> Save() {
            if (!ValidForSave())
                return null;

            var contact = BuildFromProperties();

            if (Operation == operation.Add) {
                var account = await _contactAccountRepository.Add(contact);
                return account;
            }

            if (Operation == operation.Edit && Id > 0) {
                contact.Id = Id;
                contact.CreatedDate = CreatedDate;
                contact.LastModifiedDate = DateTime.Now;
                await _contactAccountRepository.Update(contact);
                return contact;
            }

            return null;
        }

        public ContactAccountDto BuildFromProperties() {
            return new() {
                ContactId = ContactId,
                AccountName = AccountName,
                TotalCost = TotalCost,
                PayOutAccount = SelectedPayType == "Pay",
                Notes = Notes,
                CreatedDate = CreatedDate,
                LastModifiedDate = LastModifiedDate
            };
        }

        public bool ValidForSave() {
            if (string.IsNullOrWhiteSpace(AccountName) ||
                TotalCost <= 0)
                return false;

            return true;
        }

        public void BuildFromModel(ContactAccountDto selectedContact) {
            Id = selectedContact.Id;
            ContactId = selectedContact.ContactId;
            AccountName = selectedContact.AccountName;
            TotalCost = selectedContact.TotalCost;
            Notes = selectedContact.Notes;
            PayOutAccount = selectedContact.PayOutAccount;
            CreatedDate = selectedContact.CreatedDate;
            LastModifiedDate = selectedContact.LastModifiedDate;
            SelectedPayType = selectedContact.PayOutAccount ? PayTypes.FirstOrDefault() : PayTypes.LastOrDefault();
        }
    }
}
