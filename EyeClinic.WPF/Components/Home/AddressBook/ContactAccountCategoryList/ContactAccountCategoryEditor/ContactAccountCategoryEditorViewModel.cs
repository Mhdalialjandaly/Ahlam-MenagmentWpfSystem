using System;
using System.Threading.Tasks;
using EyeClinic.Core.Enums;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.Base;
using operation = EyeClinic.Core.Enums.Operation;
namespace EyeClinic.WPF.Components.Home.AddressBook.ContactAccountCategoryList.ContactAccountCategoryEditor
{
    public partial class ContactAccountCategoryEditorViewModel
    {
        public ContactAccountCategoryEditorViewModel() { }
    }

    public partial class ContactAccountCategoryEditorViewModel : BaseViewModel<ContactAccountCategoryEditorView>, IEditorViewModel<AccountPaymentCategoryDto>
    {
        private readonly IAccountPaymentCategoryRepository _accountPaymentCategoryRepository;

        public ContactAccountCategoryEditorViewModel(IAccountPaymentCategoryRepository accountPaymentCategoryRepository) {
            _accountPaymentCategoryRepository = accountPaymentCategoryRepository;
        }

        public int Id { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }


        public async Task<AccountPaymentCategoryDto> Save() {
            if (!ValidForSave())
                return null;

            var contact = BuildFromProperties();

            if (Operation == operation.Add) {
                var account = await _accountPaymentCategoryRepository.Add(contact);
                return account;
            }

            if (Operation == operation.Edit && Id > 0) {
                contact.Id = Id;
                contact.CreatedDate = CreatedDate;
                contact.LastModifiedDate = DateTime.Now;
                await _accountPaymentCategoryRepository.Update(contact);
                return contact;
            }

            return null;
        }

        public AccountPaymentCategoryDto BuildFromProperties() {
            return new() {
                Id = Id,
                CategoryName = CategoryName,
                CreatedDate = CreatedDate,
                LastModifiedDate = LastModifiedDate
            };
        }

        public bool ValidForSave() {
            if (string.IsNullOrWhiteSpace(CategoryName))
                return false;

            return true;
        }

        public void BuildFromModel(AccountPaymentCategoryDto selectedContact) {
            Id = selectedContact.Id;
            CategoryName = selectedContact.CategoryName;
            CreatedDate = selectedContact.CreatedDate;
            LastModifiedDate = selectedContact.LastModifiedDate;
        }
    }
}
