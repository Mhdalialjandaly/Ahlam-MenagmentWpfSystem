using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Controls;
using AutoMapper;
using EyeClinic.Core;
using EyeClinic.DataAccess;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Repositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.Localization;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.AppServices.Print;
using EyeClinic.WPF.Components.Dialogs.PasswordInput;
using EyeClinic.WPF.Components.Home;
using EyeClinic.WPF.Components.Shell;
using EyeClinic.WPF.Components.Shell.Header;
using log4net;
using Unity;

namespace EyeClinic.WPF.Setup
{
    public static class UnityContainerExtension
    {
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        public static void RegisterServices(this IUnityContainer container) {
            var configuration = new MapperConfiguration(e =>
                e.AddProfiles(new List<Profile> {
                    new EyeClinicMappingProfile(),
                    new CustomMappingProfile()
                }));

            var mapper = configuration.CreateMapper();
            container.RegisterInstance(mapper);

            container.RegisterInstance(Log);

            container.RegisterInstance<INotificationService>(new NotificationService());
            container.RegisterSingleton<IDialogService, DialogServiceViewModel>();
            container.RegisterSingleton<ILocalizationService, LocalizationService>();
            container.RegisterSingleton<IPrintService, PrintService>();

            container.AddIInjectableDependencies(typeof(PatientRepository));
            container.RegisterType(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));
        }


        public static async Task BackHome(this IUnityContainer container) {
            var shell = container.Resolve<ShellViewModel>();

            var homeViewModel = container.Resolve<HomeViewModel>();
            await homeViewModel.Initialize();
            shell.Content = homeViewModel.GetView() as HomeView;
        }

        public static void Navigate(this IUnityContainer container, UserControl view) {
            var shell = container.Resolve<ShellViewModel>();
            shell.Content = view;
        }

        public static void SetCurrentPatient(this IUnityContainer container, PatientDto patient,
            bool isViewOnly = false) {
            var header = container.Resolve<HeaderViewModel>();
            header.SetCurrentPatient(patient, isViewOnly);
        }

        public static bool CheckUserRole(this IUnityContainer container, params UserRoles[] roles) {
            var user = Global.GetValue(GlobalKeys.LoggedUser);
           
            if (user is not UserDto loggedUser)
                return false;
           
            if (roles.Any(e => (int)e == loggedUser.RoleId))
                return true;

            container.Resolve<INotificationService>()
                .Error("Access Denied");
            return false;
        }

        public static bool CheckUserRoleSilent(this IUnityContainer container, params UserRoles[] roles) {
            var user = Global.GetValue(GlobalKeys.LoggedUser);
            if (user is not UserDto loggedUser)
                return false;
           
            return roles.Any(e => (int)e == loggedUser.RoleId);
        }
    }
}
