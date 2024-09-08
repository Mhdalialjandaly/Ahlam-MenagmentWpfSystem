using EyeClinic.Core.Enums;
using EyeClinic.Core;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Dialogs.PasswordInput;
using EyeClinic.WPF.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Unity;
using EyeClinic.WPF.Setup;
using EyeClinic.WPF.Components.Dialogs.AskForOrderType;
using EyeClinic.WPF.Components.Home.Operation.OrderQueueWindow;
using EyeClinic.WPF.Components.Home.Operation.OrderQueue.OrderIndexSelecter;
using EyeClinic.WPF.Components.Home.Operation.PublicCustomerFile;

namespace EyeClinic.WPF.Components.Home.Operation.OrderQueue
{
   
        public partial class OrderQueueViewModel
        {
            public OrderQueueViewModel()
            {
                if (IsDesignMode) { }
            }
        }

        public partial class OrderQueueViewModel : BaseViewModel<OrderQueueView>
        {
            private readonly IUnityContainer _container;
            private readonly IDialogService _dialogService;
            private readonly IPublicCustomerOrderRepository _publicCusomerOrderRepository;


            public OrderQueueViewModel(IUnityContainer container, IDialogService dialogService,
                IPublicCustomerOrderRepository publicCustomerOrderRepository)
            {
                _container = container;
                _dialogService = dialogService;
                _publicCusomerOrderRepository = publicCustomerOrderRepository;

                RemoveSelectedOrderQueueCommand = new RelayCommand<OrderQueueItem>(RemoveSelectedOrderQueue);



                AddToOrderQueueWindowCommand = new RelayCommand<OrderQueueItem>(AddToOrderQueueWindow);
                StartServiceCommand = new RelayCommand<OrderQueueItem>(StartService);
                GoPublicCustomerFileCommand = new RelayCommand<OrderQueueItem>(GoPublicCustomerFile);
                BackToOrderQueueCommand = new RelayCommand<OrderQueueItem>(BackToOrderQueue);
                BackHomeCommand = new RelayCommand(BackHome);
                RefreshCommand = new RelayCommand(Refresh);
                GoOperationCommand = new RelayCommand(GoOperation);
                //GoSearchForTestCommand = new RelayCommand(GoSearchForTest);
                RequestNextPublicCustomerCommand = new RelayCommand(RequestNextPublicCustomer,
                    _ => IsBusy == false);

                var timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(5) };
                timer.Tick += (_, _) => {
                    if (OrderQueueList == null)
                        return;

                    foreach (var queue in OrderQueueList)
                    {
                        TimeSpan span = DateTime.Now - queue.PublicCustomerOrder.OrderDate;
                        queue.WaitingTimeh = $"ساعات : {span.Hours} ";
                        queue.WaitingTimem = $"دقائق : {span.Minutes} ";
                        queue.WaitingTimed = $"ايام : {span.Days} ";
                    }
                };
                timer.Start();

                ShowManageVisitButtons = Global.DeviceName.ToLower() != "clinic";
            }

            private async void RequestNextPublicCustomer()
            {
                //_container.Resolve<INotificationService>().Warning(" You can't request As annoying <(; ");
                await _container.Resolve<IAppLanguageRepository>()
                    .RequestNextPatient(Global.DeviceName);

                Refresh();
            }

            public int LastOrderQueueIndex { get; set; }
            public string LastPublicCustomerName { get; set; }
            public bool ShowManageVisitButtons { get; set; }
            public string OrderTypes { get; set; }
            public override async Task Initialize()
            {


                
            SelectedDate ??= DateTime.Now.Date;
                var orderDate =
                    await _publicCusomerOrderRepository.GetOrders(!IsReceptionMode);

                TotalPublicCustomers = orderDate.Count;
                StartedPublicCustomers = orderDate.Count(e => e.OrderStatus == (int)PublicCustomerOrderStatus.Started);
                PendingPublicCustomers = orderDate.Count(e => e.OrderStatus == (int)PublicCustomerOrderStatus.Created);
                DonePublicCustomers = orderDate.Count(e => e.OrderStatus == (int)PublicCustomerOrderStatus.Finished);



                BaseOrderQueueList = orderDate.Select(e => new OrderQueueItem
                {
                    OrderQueueIndex = OrderQueueList?.Count + 1 ?? 1,
                    PublicCustomerOrder = e
                }).OrderBy(e => e.PublicCustomerOrder.OrderDate).ToList();
                OrderQueueList = new ObservableCollection<OrderQueueItem>(BaseOrderQueueList);
                OrderQueueListHasData = !OrderQueueList.IsNullOrEmpty();

                var queueWindow = _container.Resolve<OrderQueueWindowViewModel>();
                await queueWindow.Initialize();

                var lastQueue = queueWindow.OrderQueueList
                    .OrderByDescending(e => e.OrderQueueIndex).FirstOrDefault();
                if (lastQueue != null)
                {
                    LastOrderQueueIndex = lastQueue.OrderQueueIndex;
                    LastPublicCustomerName = lastQueue.PublicCustomerOrder?.PublicCustomer?.FullName;
                }

                // Force Filter queue
                if (ForceFilter)
                {
                    var selected = SelectedFilter;
                    FilterTypes = OrderQueueFilterType.GetAll();
                    SelectedFilter = FilterTypes.FirstOrDefault(e => e.OrderStatus == selected.OrderStatus);
                    ForceFilter = false;
                }

                // If queue already filtered, return
                if (!FilterTypes.IsNullOrEmpty())
                    return;

                // Filter by default
                FilterTypes = OrderQueueFilterType.GetAll();
                SelectedFilter = FilterTypes.FirstOrDefault();


            }


            public async Task GetLastPublicCustomer()
            {
                var queueWindow = _container.Resolve<OrderQueueWindowViewModel>();
                await queueWindow.Initialize();

                var lastQueue = queueWindow.OrderQueueList
                    .OrderByDescending(e => e.OrderQueueIndex).FirstOrDefault();
                if (lastQueue != null)
                {
                    LastOrderQueueIndex = lastQueue.OrderQueueIndex;
                    LastPublicCustomerName = lastQueue.PublicCustomerOrder?.PublicCustomer?.FullName;
                }

                await Initialize();
            }

            public DateTime? SelectedDate { get; set; }

            public bool IsReceptionMode { get; set; }
            public int TotalPublicCustomers { get; set; }
            public int StartedPublicCustomers { get; set; }
            public int PendingPublicCustomers { get; set; }
            public int DonePublicCustomers { get; set; }

            public bool IsClinicDevice => Global.DeviceName.ToLower() == "clinic";

            public bool ForceFilter { get; set; }

            public bool OrderQueueListHasData { get; set; }

            public List<OrderQueueItem> BaseOrderQueueList { get; set; }
            public ObservableCollection<OrderQueueItem> OrderQueueList { get; set; }

            public List<OrderQueueFilterType> FilterTypes { get; set; }

            private OrderQueueFilterType _selectedFilter;


            public OrderQueueFilterType SelectedFilter
            {
                get => _selectedFilter;
                set
                {
                    _selectedFilter = value;
                    if (value?.OrderStatus == null)
                        OrderQueueList = new ObservableCollection<OrderQueueItem>(BaseOrderQueueList);
                    else
                        OrderQueueList = new ObservableCollection<OrderQueueItem>(
                            BaseOrderQueueList.Where(e => e.PublicCustomerOrder.OrderStatus == (int)value.OrderStatus.Value));
                    OrderQueueListHasData = !OrderQueueList.IsNullOrEmpty();
                }
            }



            public ICommand RemoveSelectedOrderQueueCommand { get; set; }
            public ICommand AddToOrderQueueWindowCommand { get; set; }
            public ICommand StartServiceCommand { get; set; }
            public ICommand GoPublicCustomerFileCommand { get; set; }
            public ICommand BackToOrderQueueCommand { get; set; }
            public ICommand BackHomeCommand { get; set; }
            public ICommand RefreshCommand { get; set; }
            public ICommand GoOperationCommand { get; set; }
            public ICommand GoSearchForTestCommand { get; set; }
            public ICommand RequestNextPublicCustomerCommand { get; set; }


            public async Task AskForOrder(PublicCustomerDto publicCustomerDto)
            {
                // Ask for total cost of service and how much patient payed
                var askfororder = _container.Resolve<AskForOrderTypeViewModel>();
                await askfororder.Initialize(publicCustomerDto);
                _dialogService.ShowEditorDialog(askfororder.GetView() as AskForOrderTypeView, async () => {
                    var item = await askfororder.Save(publicCustomerDto.Id);
                    if (item == null)
                        return false;

                    item.PublicCustomer = publicCustomerDto;
                    //item.Remaining = askForCost.SelectedNotPayReason != null
                    //            ? 0 : askForCost.Cost - askForCost.Payment;



                    if (SelectedDate == DateTime.Now.Date)
                    {
                        BaseOrderQueueList.Add(new OrderQueueItem { PublicCustomerOrder = item });
                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            OrderQueueList.Add(new OrderQueueItem { PublicCustomerOrder = item });
                        });
                        OrderQueueListHasData = !OrderQueueList.IsNullOrEmpty();
                    }
                    return true;
                });
            }

            private async Task OrderQueueItemStarted(PublicCustomerOrderDto publicCustomerOrderDto)
            {
                await _publicCusomerOrderRepository.ChangeOrderStatus(publicCustomerOrderDto.Id);
                OrderQueueList.Remove(OrderQueueList.FirstOrDefault(e => e.PublicCustomerOrder.Id == publicCustomerOrderDto.Id));
                BaseOrderQueueList.Remove(OrderQueueList.FirstOrDefault(e => e.PublicCustomerOrder.Id == publicCustomerOrderDto.Id));
                OrderQueueListHasData = !OrderQueueList.IsNullOrEmpty();
            }



            public void AddToOrderQueueWindow(OrderQueueItem queue)
            {
            var indexSelector = _container.Resolve<OrderIndexSelecterViewModel>();

            _dialogService.ShowEditorDialog(indexSelector.GetView() as OrderIndexSelecter.OrderIndexSelecterView,
                async () =>
                {
                    await _container.Resolve<OrderQueueWindowViewModel>()
                        .Add(new OrderQueueItem { PublicCustomerOrder = queue.PublicCustomerOrder },
                            indexSelector.Index);

                    await GetLastPublicCustomer();
                    return true;
                });
            //sort queue items
            //ICollectionView dataview = CollectionViewSource.GetDefaultView(queue);
            //using (dataview.DeferRefresh())
            //{
            //    dataview.SortDescriptions.Clear();
            //    SortDescription sd = new SortDescription("SortByIndexe", ListSortDirection.Ascending);
            //    dataview.SortDescriptions.Add(sd);
            //}

        }

            public void RemoveSelectedOrderQueue(OrderQueueItem queue)
            {
                if (!_container.CheckUserRoleSilent(UserRoles.Admin))
                {
                    _container.Resolve<INotificationService>()
                    .Warning("You have no permission");
                    return;
                }

                var pwd = _container.Resolve<PasswordInputViewModel>();
                pwd.DisposeOnLogin = false;
                pwd.CustomPassword = "54425";
                pwd.OnSuccessLogin += () =>
                {
                    _dialogService.DisposeLastDialog();
                    BusyExecute(async () => {
                        await _publicCusomerOrderRepository.RemoveFromOrderQueue(queue.PublicCustomerOrder.Id);
                        OrderQueueList.Remove(queue);
                        BaseOrderQueueList.Remove(queue);
                        OrderQueueListHasData = !OrderQueueList.IsNullOrEmpty();
                        await _container.Resolve<OrderQueueWindowViewModel>().Remove(queue);
                    });
                };
                _container.Resolve<IDialogService>()
                       .ShowPopupContent(pwd.GetView() as PasswordInputView);
            }

            public void StartService(OrderQueueItem queue)
            {
                BusyExecute(async () => {
                    await OrderQueueItemStarted(queue.PublicCustomerOrder);
                });
            }

            public void GoPublicCustomerFile(OrderQueueItem queue)
            {
                BusyExecute(async () => {
                    var patientFile = _container.Resolve<PublicCustomerFileViewModel>();
                    await patientFile.Initialize(queue.PublicCustomerOrder.PublicCustomerId, queue.PublicCustomerOrder.Id, IsReceptionMode);

                    if (IsReceptionMode)
                        _dialogService.ShowFullScreenPopupContent(patientFile.GetView() as PublicCustomerFileView);
                    else
                        _container.Navigate(patientFile.GetView() as PublicCustomerFileView);
                });
            }

            public void BackToOrderQueue(OrderQueueItem queue)
            {
                //if (_container.CheckUserRoleSilent(UserRoles.Reception))
                //    return;
                if (!_container.CheckUserRole(UserRoles.Admin, UserRoles.Seller))
                    return;
                if (queue.PublicCustomerOrder.OrderStatus == (int)PublicCustomerOrderStatus.Created)
                {
                    _container.Resolve<INotificationService>()
                        .Warning("This patient already in the Queue");
                    return;
                }


                BusyExecute(async () => {
                    await _publicCusomerOrderRepository.ChangeOrderStatus(queue.PublicCustomerOrder.Id,
                        PublicCustomerOrderStatus.Created);
                    ForceFilter = true;
                    await Initialize();

                    if (SelectedFilter.OrderStatus != PublicCustomerOrderStatus.Created)
                    {
                        OrderQueueList.Remove(queue);
                        BaseOrderQueueList.Remove(queue);
                        OrderQueueListHasData = !OrderQueueList.IsNullOrEmpty();
                    }
                });
            }

            private void BackHome()
            {
                BusyExecute(async () => await _container.BackHome());
            }

            private void Refresh()
            {
                ForceFilter = true;
                var total = StartedPublicCustomers;

                BusyExecute(async () => {
                    if (Global.DeviceName.ToLower() != "clinic")
                    {
                        await GetAllOrders();
                        return;
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        var startedVisits = (await _publicCusomerOrderRepository.GetOrdersByDate(SelectedDate ?? DateTime.Now.Date,
                            !IsReceptionMode))
                            .Where(e => e.OrderStatus == (int)PublicCustomerOrderStatus.Started)
                            .ToList();

                        if (startedVisits.Count > total)
                            break;
                        await Task.Delay(2000);
                    }

                    await GetAllOrders();
                    OnPropertyChanged(nameof(SelectedFilter));
                });
            }
            public async Task GetAllOrders()
            {

                SelectedDate ??= DateTime.Now.Date;
                var OrderssToday =
                    await _publicCusomerOrderRepository.GetOrdersByDate(SelectedDate.Value, !IsReceptionMode);

                TotalPublicCustomers = OrderssToday.Count;
                StartedPublicCustomers = OrderssToday.Count(e => e.OrderStatus == (int)PublicCustomerOrderStatus.Started);
                PendingPublicCustomers = OrderssToday.Count(e => e.OrderStatus == (int)PublicCustomerOrderStatus.Created);
                DonePublicCustomers = OrderssToday.Count(e => e.OrderStatus == (int)PublicCustomerOrderStatus.Finished);
                BaseOrderQueueList = OrderssToday.Select(e => new OrderQueueItem
                {
                    OrderQueueIndex = OrderQueueList?.Count + 1 ?? 1,
                    PublicCustomerOrder = e
                }).ToList();
                OrderQueueList = new ObservableCollection<OrderQueueItem>(BaseOrderQueueList);
                OrderQueueListHasData = !OrderQueueList.IsNullOrEmpty();

            }

            private void GoOperation()
            {
                BusyExecute(async () => {
                    var reception = _container.Resolve<OperationViewModel>();
                    await reception.Initialize();
                    _dialogService.ShowFullScreenPopupContent(reception.GetView() as OperationView);
                });
            }

            //private void GoSearchForTest()
            //{
            //    BusyExecute(async () => {
            //        var eyeImages = _container.Resolve<EyeImagesViewModel>();
            //        await eyeImages.Initialize();
            //        _dialogService.ShowFullScreenPopupContent(eyeImages.GetView() as EyeImagesView);
            //    });
            //}
        }
    
}
