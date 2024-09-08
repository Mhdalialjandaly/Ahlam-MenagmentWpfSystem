using EyeClinic.Model;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Dialogs.PasswordInput;
using EyeClinic.WPF.Components.Home.Reception.Review;
using EyeClinic.WPF.Components.Home.Reception.TodayPayout;
using EyeClinic.WPF.Components.Home.Reception.TodayVisit;
using EyeClinic.WPF.Components.Shell.QueueWindow;

using System.Threading.Tasks;
using System.Windows.Input;
using Unity;
using System.Windows.Controls;
using System.Windows;
using operation = EyeClinic.Core.Enums.Operation;
using EyeClinic.WPF.Setup;
using EyeClinic.WPF.Components.Home.Operation.PublicCustomer;
using EyeClinic.WPF.Components.Home.Operation.OrderQueue;
using EyeClinic.WPF.Components.Home.Operation.OrderQueueWindow;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.Components.Home.Operation.PublicCustomer.PublicCustomerEditer;
using WIA;
using System.Windows.Media.Imaging;
using System.IO;



namespace EyeClinic.WPF.Components.Home.Operation
{
    
        public partial class OperationViewModel
        {
            public OperationViewModel()
            {
                if (IsDesignMode) { }
            }
        }

        public partial class OperationViewModel : BaseViewModel<OperationView>
        {

            private readonly IUnityContainer _container;
            public static IUnityContainer container2;
            public readonly IDialogService _dialogService;
            public OperationViewModel(IUnityContainer container ,IDialogService dialogService)
            {
                _container = container;
            _dialogService =dialogService;
                NavigateCommand = new RelayCommand<string>(Navigate);
                OperationWindowCommand = new RelayCommand(OperationWindowShow);
            }


            private void OperationWindowShow()
            {
                BusyExecute(async () =>
                {
                    var queueWindow = _container.Resolve<OrderQueueWindowViewModel>();
                    await queueWindow.Initialize();
                    _dialogService.ShowFullScreenPopupContent(queueWindow.GetView() as OrderQueueWindowView);
                });
            }

            public override async Task Initialize()
            {
                var publicCustomer = _container.Resolve<PublicCustomerViewModel>();
                await publicCustomer.Initialize();
                publicCustomer.OnAddPublicCustomerToQueue += PatientListOnAddToQueueAction;
                publicCustomer.OnSelectPublicCustomer += OnPublicCustomerSelected;
                PublicCustomerList = publicCustomer.GetView() as PublicCustomerView;

                PublicCustomerEditerViewModel = _container.Resolve<PublicCustomerEditerViewModel>();               

                OrderQueueModule = _container.Resolve<OrderQueueViewModel>();
                OrderQueueModule.IsReceptionMode = true;
                await OrderQueueModule.Initialize();

                Navigate("OrderQueue");
            }

            private void OnPublicCustomerSelected(object sender, PublicCustomerDto publicCustomerDto)
            {
            SelectedPublicCustomer = publicCustomerDto;
                Navigate(ActiveNavigation);
            }
            public bool IsReceptionMode { get; set; }

            public string ActiveNavigation { get; set; }
            public PublicCustomerDto SelectedPublicCustomer { get; set; }

            public PublicCustomerEditerViewModel PublicCustomerEditerViewModel { get; set; }
            public OrderQueueViewModel OrderQueueModule { get; set; }

            public UserControl PublicCustomerList { get; set; }
            public UserControl Content { get; set; }

            public ICommand NavigateCommand { get; set; }
            public ICommand OperationWindowCommand { get; set; }

            private void PatientListOnAddToQueueAction(object sender, PublicCustomerDto publicCustomerDto)
        
        {
            BusyExecute(async () => await OrderQueueModule.AskForOrder(publicCustomerDto));
        }

            private void Navigate(string target)
            {
                if (ActiveNavigation != "PublicCustomer Info" && ActiveNavigation == target)
                    return;

                ActiveNavigation = target;
                BusyExecute(async () => {
                    if (target == "OrderQueue")
                    {
                        await OrderQueueModule.Initialize();
                        Content = OrderQueueModule.GetView() as OrderQueueView;
                        return;
                    }

                    if (target == "OrderQueue Info")
                    {   
                        if (SelectedPublicCustomer == null)
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
                                            //Source = im.,
                                            VerticalAlignment = VerticalAlignment.Center,
                                            HorizontalAlignment = HorizontalAlignment.Center
                                        }
                                    }
                                    }
                                };
                            });
                            return;
                        }

                        await PublicCustomerEditerViewModel.Initialize();
                        //PatientEditorModule.BuildFromModel(SelectedPatient);
                        PublicCustomerEditerViewModel.Operation = operation.View;

                        Content = PublicCustomerEditerViewModel.GetView() as PublicCustomerEditerView;
                        return;
                    }

                    if (target == "UpComingDate")
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

