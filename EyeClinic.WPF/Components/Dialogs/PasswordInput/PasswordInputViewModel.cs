using System;
using System.Linq;
using System.Windows.Input;
using EyeClinic.Core;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Shell.Footer;
using Unity;

namespace EyeClinic.WPF.Components.Dialogs.PasswordInput
{
    public partial class PasswordInputViewModel
    {
        public PasswordInputViewModel() { if (IsDesignMode) { } }
    }

    public partial class PasswordInputViewModel : BaseViewModel<PasswordInputView>
    {
        private readonly IDialogService _dialogService;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;
        private readonly IUnityContainer _container;

        private readonly IDeniedUserRepository _deniedUserRepository;



        public PasswordInputViewModel(IDialogService dialogService, IUserRepository userRepository,
                INotificationService notificationService, IUnityContainer container, IDeniedUserRepository deniedUser)
        {
            _dialogService = dialogService;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _container = container;
            _deniedUserRepository = deniedUser;

            InputCommand = new RelayCommand<string>(Input);
            CancelCommand = new RelayCommand(dialogService.DisposeLastDialog);
            OkCommand = new RelayCommand(Ok);
            RemoveLastCharacterCommand = new RelayCommand(RemoveLastCharacter);
        }

        public Action OnSuccessLogin;
        public int CounterPass { get; set; }
        public string CustomPassword { get; set; }
        public bool DisposeOnLogin { get; set; } = true;

        private void Input(string obj)
        {
            if (GetView() is PasswordInputView view)
            {
                view.PasswordBoxControl.Password += obj;
            }
        }


        public ICommand InputCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand OkCommand { get; set; }
        public ICommand RemoveLastCharacterCommand { get; set; }

        private async void Ok()
        {

            if (GetView() is PasswordInputView view)
            {
                if (string.IsNullOrWhiteSpace(CustomPassword))
                {
                    BusyExecute(async () =>
                    {
                        var user = (await _userRepository
                                .GetByKey(e => e.Password == view.PasswordBoxControl.Password))
                            .FirstOrDefault();

                        if (user == null)
                        {
                            _notificationService.Error("Invalid Password");
                            view.PasswordBoxControl.Password = "";
                            return;
                        }

                        Global.AddValue(GlobalKeys.LoggedUser, user);
                        _container.Resolve<FooterViewModel>().LoggedInUser = user.UserName;

                        OnSuccessLogin?.Invoke();
                        if (DisposeOnLogin)
                            _dialogService.DisposeLastDialog();
                    });
                }
                else
                { // Custom Password is not empty
                    if (view.PasswordBoxControl.Password == CustomPassword)
                    {
                        OnSuccessLogin?.Invoke();
                        if (DisposeOnLogin)
                            _dialogService.DisposeLastDialog();
                    }
                    else
                    {
                        _notificationService.Error("Invalid Password");
                        view.PasswordBoxControl.Password = "";
                        CounterPass = CounterPass + 1;
                    }
                    if (CounterPass == 3)
                    {
                        _notificationService.Warning("Your Tryed three time");
                        var user = Global.GetValue(GlobalKeys.LoggedUser);
                        if (user is Model.UserDto usermodel)
                        {
                            await _deniedUserRepository.Add(new Model.DeniedUser
                            {
                                Id = usermodel.Id,
                                DeviceName = Global.DeviceName,
                                UserName = usermodel.UserName,
                                DeniedDate = DateTime.Today,
                                CreatedDate = DateTime.Today,                                
                            });
                            CounterPass = 0;
                         _dialogService.DisposeLastDialog();
                        }
                    }

                }
            }
        }
     

        private void RemoveLastCharacter()
        {
            if (GetView() is PasswordInputView view)
            {
                if (string.IsNullOrWhiteSpace(view.PasswordBoxControl.Password))
                    return;

                view.PasswordBoxControl.Password =
                    view.PasswordBoxControl.Password.Remove(
                        view.PasswordBoxControl.Password.Length - 1, 1);
            }
        }
    }

    public enum UserRoles
    {
        Admin = 1,
        Selleing = 2,
        MakeingReady1 = 3,
        MakeingReady2 = 4,
        HalvaDepartment = 5,
        Reporter= 6,
        Quality = 7 ,
        OilAndVingerDepartment = 8,
        CookingDepartment = 9,
        MilksDepartment =10,
        JamsDepartment = 11,
        CannedDepartment= 12,
        GlassesWareHouse = 13,


    }
}
