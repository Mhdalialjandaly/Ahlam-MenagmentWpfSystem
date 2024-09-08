using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Home.AddressBook.ContactList.ContactEditor;
using EyeClinic.WPF.Setup;
using Unity;
using operation = EyeClinic.Core.Enums.Operation;
using System.Windows;


namespace EyeClinic.WPF.Components.Home.AddressBook.ContactList
{
    public partial class ContactListViewModel
    {
        public ContactListViewModel() { }
    }
    public partial class ContactListViewModel : BaseViewModel<ContactListView>
    {
        private readonly IContactRepository _contactRepository;
        private readonly IDialogService _dialogService;
        private readonly IUnityContainer _container;
        private readonly INotificationService _notification;

        public ContactListViewModel(IContactRepository contactRepository, IDialogService dialogService,
            IUnityContainer container,INotificationService notification) {
            _contactRepository = contactRepository;
            _dialogService = dialogService;
            _container = container;
            _notification = notification;

            AddContactCommand = new RelayCommand(AddContact);
            EditContactCommand = new RelayCommand(EditContact);
            SendContactCommand = new RelayCommand(SendMessage);
            DeleteContactCommand = new RelayCommand<ContactDto>(DeleteContact);
            SearchCommand = new RelayCommand(Search);
            BackCommand = new RelayCommand(Back);
        }

      

        public override async Task Initialize() {
            var contacts = await _contactRepository.GetAll();
            Contacts = new ObservableCollection<ContactDto>(
                contacts.OrderByDescending(e => e.CreatedDate).ToList());
        }


        public event EventHandler<ContactDto> OnSelectedContactChanged;

        public ObservableCollection<ContactDto> Contacts { get; set; }

        private ContactDto _selectedContact;
        public ContactDto SelectedContact {
            get => _selectedContact;
            set {
                _selectedContact = value;
                OnSelectedContactChanged?.Invoke(this, value);
            }
        }

        public string SearchText { get; set; }
        public string MessageBody { get; set; }
        public string MessageTitle { get; set; }


        public ICommand AddContactCommand { get; set; }
        public ICommand EditContactCommand { get; set; }
        public ICommand DeleteContactCommand { get; set; }
        public ICommand SendContactCommand { set; get; }
        public ICommand SearchCommand { get; set; }
        public ICommand BackCommand { get; set; }

        private void AddContact() {
            BusyExecute(async () => {
                var editor = _container.Resolve<ContactEditorViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Add;
                _dialogService.ShowEditorDialog(editor.GetView() as ContactEditorView,
                    async () => {
                        var contact = await editor.Save();
                        if (contact == null)
                            return false;

                        Contacts.Insert(0, contact);
                        return true;
                    });
            });
        }

        private void EditContact() {
            if (SelectedContact == null)
                return;

            BusyExecute(async () => {
                var editor = _container.Resolve<ContactEditorViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Edit;
                editor.Id = SelectedContact.Id;
                editor.BuildFromModel(SelectedContact);
                _dialogService.ShowEditorDialog(editor.GetView() as ContactEditorView,
                    async () => {
                        var contact = await editor.Save();
                        if (contact != null) {
                            await Initialize();
                            return true;
                        }

                        return false;
                    });
            });
        }

        private void DeleteContact(ContactDto contact) {
            BusyExecute(async () => {
                await _contactRepository.Delete(contact.Id);
                await Initialize();
                _container.Resolve<INotificationService>()
                    .Success("Deleted");
            });
        }
        private void SendMessage()
        {
            if (SelectedContact.EmailAdress == null)
            {
                _notification.Warning("الرجال اختيار الشخص ");
                return; }
            try
            {
                Microsoft.Office.Interop.Outlook.Application oApp = new Microsoft.Office.Interop.Outlook.Application();
                Microsoft.Office.Interop.Outlook.MailItem oMsg = (Microsoft.Office.Interop.Outlook.MailItem)oApp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
                oMsg.Subject = "شركة الاحلام";                
                oMsg.To = SelectedContact.EmailAdress.ToString();
                oMsg.BodyFormat = Microsoft.Office.Interop.Outlook.OlBodyFormat.olFormatHTML;
                oMsg.HTMLBody = "منتجات شركة الاحلام";
                //oMsg.Attachments.Add("c:/temp/test.txt", Microsoft.Office.Interop.Outlook.OlAttachmentType.olByValue, Type.Missing, Type.Missing);
                oMsg.Display(true);
                oMsg.Send();
                _notification.Success("تم المراسلة");
            }
            catch (Exception ex)
            {
                _notification.Warning(ex.Message);
            }
        
            //BusyExecute(async () => {
            //    var editor = _container.Resolve<ContactEditorViewModel>();
            //    await editor.Initialize();
            //    editor.Operation = Operation.Add;
            //    _dialogService.ShowEditorDialog(editor.GetView() as ContactEditorView,
            //        async () => {
            //            var contact = await editor.Save();
            //            if (contact == null)
            //                return false;

                //            Contacts.Insert(0, contact);
                //            return true;
                //        });
                //});
                //if (!string.IsNullOrEmpty(MessageBody)||
                //    !string.IsNullOrEmpty(MessageTitle))
                //{
                //    SendMail(SelectedContact.EmailAdress, MessageTitle, MessageBody);
                //}
            //else
            //{
                
            //}

        }

        private void Search() {
            BusyExecute(async () => {
                if (string.IsNullOrWhiteSpace(SearchText)) {
                    Contacts = new ObservableCollection<ContactDto>(
                        (await _contactRepository.GetAll()).OrderByDescending(e => e.CreatedDate).ToList());
                    return;
                }

                Contacts = new ObservableCollection<ContactDto>(
                    (await _contactRepository.GetByKey(
                        e => e.ContactName.ToLower().StartsWith(SearchText.ToLower()) ||
                             e.ContactPhones.ToLower().StartsWith(SearchText.ToLower())
                        )).OrderByDescending(e => e.CreatedDate).ToList());
            });
        }

        private void Back() {
            BusyExecute(async () => {
                await _container.BackHome();
            });
        }

        public void SendMail(string recipient, string subject, string body)
        {
            try
            {
                string smtpServer = "smtp.office365.com"; // Use the appropriate SMTP server
                int smtpPort = 587; // Port for TLS
                string senderEmail = "AlahlamCoManager@outlook.com";
                string senderPassword = "Ad123zxc";

                using (SmtpClient client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.Credentials = new NetworkCredential(senderEmail, senderPassword);
                    client.EnableSsl = true;

                    MailMessage email = new MailMessage(senderEmail, recipient, subject, body);
                    client.Send(email);
                    _notification.Success("Message Sent SuccessFuly");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending email: {ex.Message}");
            }
        }
       
    }
}
