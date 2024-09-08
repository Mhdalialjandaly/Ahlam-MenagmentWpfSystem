using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using operation = EyeClinic.Core.Enums.Operation;

namespace EyeClinic.WPF.Components.Home.CartoonForm.Customers.CustomerEditer
{
   public partial class CustomerEditerViewModel
    {
    }
    public partial class CustomerEditerViewModel :BaseViewModel<CustomerEditerView>
    {
        public readonly  INotificationService _notification;
        public readonly ICustomerRepository _customerRepository;
        public CustomerEditerViewModel(INotificationService notificationService,ICustomerRepository customerRepository) 
        {
            _notification = notificationService;
            _customerRepository = customerRepository;

            DateOf = DateTime.Now.Date;
            SaveCommand = new RelayCommand(SaveMethode);
            EditeCommand = new RelayCommand(SaveMethode);

        }

       

        public int Id { get; set; }
        public int ContactId { get; set; }
        public string AccountName { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime DateOf { get; set; } 
        public bool PayOutAccount { get; set; }
        public int Remaining { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand EditeCommand { get; set; }
        public async Task<CustomerDto> Save()
        {
            var cus = await _customerRepository.GetAll();
            if (ValidForSave(cus))
            {
                var item = BuildFromProperties();
                if (Operation == operation.Add)
                {
                    var addedItem = await _customerRepository.Add(item);
                    return addedItem;
                }

                if (Operation == operation.Edit && Id > 0)
                {
                    item.Id = Id;
                    item.CreatedDate = CreatedDate.Date;
                    item.LastModifiedDate = DateTime.Now;
                    await _customerRepository.Update(item);
                    return item;
                }
            }

            return null;
        }

        private bool ValidForSave(List<CustomerDto> customerDtos)
        {
            foreach (var item in customerDtos)
            {
                if (item.AccountName == AccountName)
                {
                    _notification.Error("..هذا الاسم موجود");
                    return false;                    
                } 
            }
            return string.IsNullOrWhiteSpace(AccountName)  || ContactId != 0 ;
        }

        private CustomerDto BuildFromProperties()
        {
            return new()
            {
                Id = Id,
                AccountName = AccountName,
                ContactId = ContactId,
                Notes = Notes,
                CreatedDate = DateOf.Date.Date               
            };
        }

        public void BuildFromModel(CustomerDto disease)
        {
            Id = disease.Id;
            ContactId = disease.ContactId;
            AccountName = disease.AccountName;
            Notes = disease.Notes;
            CreatedDate = disease.CreatedDate.Date.Date;           
        }
        private void SaveMethode()
        {
            BusyExecute(async () => {
            var cus = await _customerRepository.GetAll();
                if (ValidForSave(cus))
                {
                    var item = BuildFromProperties();
                    if (Operation == operation.Add)
                    {
                        var addedItem = await _customerRepository.Add(item);
                        _notification.Success("تم الاضافة ");
                    }
                  
                }                   
                
            });
           
        }
        public void EditeMethode()
        {
            BusyExecute(async () => {
                var cus = await _customerRepository.GetAll();
                if (ValidForSave(cus))
                {
                    var item = BuildFromProperties();
                   
                        item.Id = Id;
                        item.CreatedDate = CreatedDate.Date;
                        item.LastModifiedDate = DateTime.Now;
                        await _customerRepository.Update(item);

                    
                }
            });
        }
    }
}
