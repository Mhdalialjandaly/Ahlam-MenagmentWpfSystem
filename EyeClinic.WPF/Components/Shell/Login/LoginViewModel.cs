using System.Windows.Input;
using EyeClinic.WPF.Base;

namespace EyeClinic.WPF.Components.Shell.Login
{
    public partial class LoginViewModel
    {
        public LoginViewModel() {
            if (IsDesignMode) { }
        }
    }

    public partial class LoginViewModel : BaseViewModel<LoginView>
    {


        public ICommand SignInCommand { get; set; }

        private void SignIn() {
            GetView();

            var password = View.PasswordControl.Password;
        }
    }
}
