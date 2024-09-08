using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.Core.Enums;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Home.AddressBook.ContactAccountCategoryList.ContactAccountCategoryEditor;
using Unity;
using operation = EyeClinic.Core.Enums.Operation;
namespace EyeClinic.WPF.Components.Home.AddressBook.ContactAccountCategoryList
{
    public partial class ContactAccountCategoryListViewModel
    {
        public ContactAccountCategoryListViewModel() { }
    }

    public partial class ContactAccountCategoryListViewModel : BaseViewModel<ContactAccountCategoryListView>
    {
        private readonly IUnityContainer _container;
        private readonly IDialogService _dialogService;
        private readonly IAccountPaymentCategoryRepository _accountPaymentCategoryRepository;

        public ContactAccountCategoryListViewModel(IUnityContainer container, IDialogService dialogService,
                    IAccountPaymentCategoryRepository contactAccountRepository) {
            _container = container;
            _dialogService = dialogService;
            _accountPaymentCategoryRepository = contactAccountRepository;

            AddCategoryCommand = new RelayCommand(AddCategory);
            EditCategoryCommand = new RelayCommand(EditCategory);
            DeleteCategoryCommand = new RelayCommand<AccountPaymentCategoryDto>(DeleteCategory);
        }

        public override async Task Initialize() {
            var categories = await _accountPaymentCategoryRepository
                .GetAll();

            Categories = new ObservableCollection<AccountPaymentCategoryDto>(categories);
        }

        public ObservableCollection<AccountPaymentCategoryDto> Categories { get; set; }
        public AccountPaymentCategoryDto SelectedCategory { get; set; }


        public ICommand AddCategoryCommand { get; set; }
        public ICommand EditCategoryCommand { get; set; }
        public ICommand DeleteCategoryCommand { get; set; }


        private void AddCategory() {
            var editor = _container.Resolve<ContactAccountCategoryEditorViewModel>();
            editor.Operation = operation.Add;
            _dialogService.ShowEditorDialog(editor.GetView() as ContactAccountCategoryEditorView, async () => {
                var account = await editor.Save();
                if (account == null)
                    return false;

                Categories.Insert(0, account);
                return true;
            });
        }

        private void EditCategory() {
            if (SelectedCategory == null) {
                _container.Resolve<INotificationService>().Warning("Please, Select Category to edit");
                return;
            }

            var editor = _container.Resolve<ContactAccountCategoryEditorViewModel>();
            editor.BuildFromModel(SelectedCategory);
            editor.Operation = operation.Edit;
            _dialogService.ShowEditorDialog(editor.GetView() as ContactAccountCategoryEditorView, async () => {
                var account = await editor.Save();
                if (account == null)
                    return false;

                await Initialize();
                return true;
            });
        }

        private void DeleteCategory(AccountPaymentCategoryDto account) {
            _dialogService.ShowConfirmationMessage("Are you sure ?", async () => {
                await _accountPaymentCategoryRepository.Delete(e => e.Id == account.Id);
                Categories.Remove(account);
                return true;
            });
        }
    }
}
