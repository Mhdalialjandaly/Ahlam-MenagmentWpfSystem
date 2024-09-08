using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Base.Interfaces;
using MimeKit;
using MailKit.Net.Smtp;
using Notifications.Wpf.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EyeClinic.Model;
using System.Collections.ObjectModel;
using EyeClinic.Core.Enums;
using operation = EyeClinic.Core.Enums.Operation;
namespace EyeClinic.WPF.Components.Home.AddressBook.ContactList.MessageForm
{
    public partial class MessageViewModel
    {
        public MessageViewModel() { }
    }
    public partial class MessageViewModel : BaseViewModel<MessageView>
    {
        INotificationService _notificationservice;
        public  MessageViewModel(INotificationService notificationService)
        {
            _notificationservice = notificationService;
        }       
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string PersonName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string MessageBody { get; set; }
        public string MessageTitle { get; set; }
        public DateTime SentDate { get; set; }
        public ContactDto Contact { get; set; }

        //public async Task<Model.EyeTestDto> Save()
        //{
        //    if (ValidForSave())
        //    {
        //        var item = BuildFromProperties();
        //        if (Operation == Operation.Add)
        //        {
        //            var addedItem = await _eyeTestRepository.Add(item);
        //            return addedItem;
        //        }

        //        if (Operation == Operation.Edit && Id > 0)
        //        {
        //            item.Id = Id;
        //            item.CreatedDate = CreatedDate;
        //            item.LastModifiedDate = DateTime.Now;
        //            await _eyeTestRepository.Update(item);
        //            return item;
        //        }
        //    }

        //    return null;
        //}

        //private bool ValidForSave()
        //{
        //    return !string.IsNullOrWhiteSpace(EnName) || !string.IsNullOrWhiteSpace(ArName);
        //}

        //private Model.EyeTestDto BuildFromProperties()
        //{
        //    return new()
        //    {
        //        Id = Id,
        //        Code = Code,
        //        ArName = ArName,
        //        EnName = EnName,
        //        CreatedDate = CreatedDate
        //    };
        //}

        //public void BuildFromModel(Model.EyeTestDto disease)
        //{
        //    Id = disease.Id;
        //    Code = disease.Code;
        //    ArName = disease.ArName;
        //    EnName = disease.EnName;
        //    CreatedDate = disease.CreatedDate;
        //}
        public string SendMessage() 
        {
            

            // Create an email message
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Your Name", "your_email@example.com"));
            email.To.Add(new MailboxAddress("Recipient Name", "recipient@example.com"));
            email.Subject = "Your Subject Here";
            email.Body = new TextPart("plain")
            {
                Text = "Your email body content here."
            };

            // Configure the SMTP client (for Windows 10 Mail, use your email provider's SMTP settings)
            var smtpClient = new SmtpClient();
            smtpClient.Connect("smtp.example.com", 587, false); // Replace with your SMTP server and port
            smtpClient.Authenticate("your_email@example.com", "your_email_password"); // Replace with your credentials

            // Send the email
            smtpClient.Send(email);            
            smtpClient.Disconnect(true);
            // Dispose the client
            smtpClient.Dispose();
            return "Done";
        }
    }
}
