using EyeClinic.Model;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Dialogs.PasswordInput;
using EyeClinic.WPF.Components.Home.Reception.Queue;
using EyeClinic.WPF.Components.Home.Reception.Review;
using EyeClinic.WPF.Components.Home.Reception.TodayPayout;
using EyeClinic.WPF.Components.Home.Reception.TodayVisit;
using EyeClinic.WPF.Components.Home.Reception;
using EyeClinic.WPF.Components.PatientList.PatientEditor;
using EyeClinic.WPF.Components.PatientList;
using EyeClinic.WPF.Components.Shell.QueueWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;
using System.Windows.Controls;
using System.Windows;
using EyeClinic.Core.Enums;
using operation = EyeClinic.Core.Enums.Operation;
using EyeClinic.WPF.Setup;
using EyeClinic.WPF.Components.PublicCustomerList;

namespace EyeClinic.WPF.Components.Home.Operations
{
  
    public partial class OperationViewModel
    {
        public OperationViewModel()
        {
            if (IsDesignMode) { }
        }
    }

    public partial class OperationViewModel : BaseViewModel<OperationsView>
    {

        private readonly IUnityContainer _container;
        public static IUnityContainer container2;
        public OperationViewModel(IUnityContainer container)
        {
            _container = container;

            NavigateCommand = new RelayCommand<string>(Navigate);
            ReceptionWindowCommand = new RelayCommand(ReceptionWindowShow);
        }


        private void ReceptionWindowShow()
        {
            BusyExecute(async () =>
            {
                var queueWindow = _container.Resolve<QueueWindowViewModel>();
                await queueWindow.Initialize();
                var view = queueWindow.GetView();
                if (view is QueueWindowView _v)
                {
                    _v = new QueueWindowView { DataContext = queueWindow };
                    _v?.Show();
                }
            });
        }

        public override async Task Initialize()
        {
            var patientList = _container.Resolve<PublicCustomerViewModel>();
            await patientList.Initialize();
            patientList.OnAddPatientToQueue += PatientListOnAddToQueueAction;
            patientList.OnSelectPatient += OnPatientSelected;
            PublicCustomersList = patientList.GetView() as PublicCustomerListView;

            PatientEditorModule = _container.Resolve<PatientEditorViewModel>();
            PatientEditorModule.OnPatientGlassChanged += (sender, glass) => {
                SelectedPatient.PatientGlass = glass;
            };

            QueueModule = _container.Resolve<QueueViewModel>();
            QueueModule.IsReceptionMode = true;
            await QueueModule.Initialize();

            Navigate("Queue");
        }

        private void OnPatientSelected(object sender, PatientDto patient)
        {
            SelectedPatient = patient;
            Navigate(ActiveNavigation);
        }
        public bool IsReceptionMode { get; set; }

        public string ActiveNavigation { get; set; }
        public PatientDto SelectedPatient { get; set; }

        public PatientEditorViewModel PatientEditorModule { get; set; }
        public QueueViewModel QueueModule { get; set; }

        public UserControl PublicCustomersList { get; set; }
        public UserControl Content { get; set; }

        public ICommand NavigateCommand { get; set; }
        public ICommand ReceptionWindowCommand { get; set; }

        private void PatientListOnAddToQueueAction(object sender, PatientDto patient)
        {
            BusyExecute(async () => await QueueModule.AskForCost(patient));
        }

        private void Navigate(string target)
        {
            if (ActiveNavigation != "Patient Info" && ActiveNavigation == target)
                return;

            ActiveNavigation = target;
            BusyExecute(async () => {
                if (target == "Queue")
                {
                    await QueueModule.Initialize();
                    Content = QueueModule.GetView() as QueueView;
                    return;
                }

                if (target == "Patient Info")
                {
                    if (SelectedPatient == null)
                    {
                        Application.Current.Dispatcher.Invoke(() => {
                            Content = new UserControl
                            {
                                Content = new DockPanel
                                {
                                    Children =
                                    {
                                        new Image
                                        {
                                            //Source ="D:\\SourceCode\EyeClinic.WPF\Images\eyelogo.png",                                          
                                            VerticalAlignment = VerticalAlignment.Center,
                                            HorizontalAlignment = HorizontalAlignment.Center
                                        }
                                    }
                                }
                            };
                        });
                        return;
                    }

                    await PatientEditorModule.Initialize();
                    PatientEditorModule.BuildFromModel(SelectedPatient);
                    PatientEditorModule.Operation = operation.View;

                    Content = PatientEditorModule.GetView() as PatientEditorView;
                    return;
                }

                if (target == "Reviews")
                {
                    var review = _container.Resolve<ReviewViewModel>();
                    await review.Initialize();
                    Content = review.GetView() as ReviewView;
                    return;
                }

                if (target == "Today Visits")
                {
                    var todayVisits = _container.Resolve<TodayVisitViewModel>();
                    await todayVisits.Initialize();
                    Content = todayVisits.GetView() as TodayVisitView;
                    return;
                }

                if (target == "Today Payouts")
                {
                    if (_container.CheckUserRoleSilent(UserRoles.Admin))
                    {
                        var todayPayout = _container.Resolve<TodayPayoutViewModel>();
                        await todayPayout.Initialize();
                        Content = todayPayout.GetView() as TodayPayoutView;
                        return;
                    }
                    else
                    {
                        _container.Resolve<INotificationService>()
                          .Warning("You have no permission");
                        return;
                    }
                }

                if (target == "Index Window")
                {
                    BusyExecute(async () =>
                    {
                        var queueWindow = _container.Resolve<QueueWindowViewModel>();
                        await queueWindow.Initialize();
                        _container.RegisterInstance(queueWindow);

                        (queueWindow.GetView() as QueueWindowView)?.Show();
                    });
                }
            });
        }
    }
}
