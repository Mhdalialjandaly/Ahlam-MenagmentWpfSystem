using EyeClinic.Core;
using EyeClinic.Core.Enums;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Dialogs.AskForCost;
using EyeClinic.WPF.Components.PatientList.PatientFile;
using EyeClinic.WPF.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Components.Dialogs.PasswordInput;
using EyeClinic.WPF.Components.Home.EyeImages;
using EyeClinic.WPF.Components.Home.Reception.Queue.IndexSelector;
using EyeClinic.WPF.Components.Shell.QueueWindow;
using EyeClinic.WPF.Setup;
using Unity;

using Syncfusion.Data.Extensions;



namespace EyeClinic.WPF.Components.Home.Reception.Queue
{
    public partial class QueueViewModel
    {
        public QueueViewModel() {
            if (IsDesignMode) { }
        }
    }

    public partial class QueueViewModel : BaseViewModel<QueueView>
    {
        private readonly IUnityContainer _container;
        private readonly IDialogService _dialogService;
        private readonly IPatientVisitRepository _patientVisitRepository;


        public QueueViewModel(IUnityContainer container, IDialogService dialogService,
            IPatientVisitRepository patientVisitRepository) {
            _container = container;
            _dialogService = dialogService;
            _patientVisitRepository = patientVisitRepository;

            RemoveSelectedQueueCommand = new RelayCommand<QueueItem>(RemoveSelectedQueue);

       

            AddToQueueWindowCommand = new RelayCommand<QueueItem>(AddToQueueWindow);
            StartServiceCommand = new RelayCommand<QueueItem>(StartService);
            GoPatientFileCommand = new RelayCommand<QueueItem>(GoPatientFile);
            BackToQueueCommand = new RelayCommand<QueueItem>(BackToQueue);
            BackHomeCommand = new RelayCommand(BackHome);
            RefreshCommand = new RelayCommand(Refresh);
            GoReceptionCommand = new RelayCommand(GoReception);
            GoSearchForTestCommand = new RelayCommand(GoSearchForTest);
            RequestNextPatientCommand = new RelayCommand(RequestNextPatient,
                _ => IsBusy == false);

            var timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(5) };
            timer.Tick += (_, _) => {
                if (QueueList == null)
                    return;

                foreach (var queue in QueueList) {
                    TimeSpan span = DateTime.Now - queue.PatientVisit.VisitDate;
                    queue.WaitingTimeh = $"ساعات : {span.Hours} ";
                    queue.WaitingTimem = $"دقائق : {span.Minutes} ";
                    queue.WaitingTimed = $"ايام : {span.Days} ";
                }
            };
            timer.Start();

            ShowManageVisitButtons = Global.DeviceName.ToLower() != "clinic";
        }

        private async void RequestNextPatient() {
            //_container.Resolve<INotificationService>().Warning(" You can't request As annoying <(; ");
            await _container.Resolve<IAppLanguageRepository>()
                .RequestNextPatient(Global.DeviceName);

            Refresh();
        }
         
        public int LastQueueIndex { get; set; }
        public string LastPatientName { get; set; }
        public bool ShowManageVisitButtons { get; set; }
        public string VisitTypes { get; set; }
        public override async Task Initialize() {           
            
            
            SelectedDate ??= DateTime.Now.Date;
            var visitsToday =
                await _patientVisitRepository.GetVisits(!IsReceptionMode);

            TotalPatients = visitsToday.Count;
            StartedPatients = visitsToday.Count(e => e.VisitStatus == (int)PatientVisitStatus.Started);
            PendingPatients = visitsToday.Count(e => e.VisitStatus == (int)PatientVisitStatus.Created);
            DonePatients = visitsToday.Count(e => e.VisitStatus == (int)PatientVisitStatus.Finished);



            BaseQueueList = visitsToday.Select(e => new QueueItem {
                QueueIndex = QueueList?.Count + 1 ?? 1,
                PatientVisit = e
            }).OrderBy(e=>e.PatientVisit.VisitDate).ToList();
            QueueList = new ObservableCollection<QueueItem>(BaseQueueList);
            QueueListHasData = !QueueList.IsNullOrEmpty();

            var queueWindow = _container.Resolve<QueueWindowViewModel>();
            await queueWindow.Initialize();

            var lastQueue = queueWindow.QueueList
                .OrderByDescending(e => e.QueueIndex).FirstOrDefault();
            if(lastQueue != null)
            {
                LastQueueIndex = lastQueue.QueueIndex;
                LastPatientName = lastQueue.PatientVisit?.Patient?.FullName;
            }

            // Force Filter queue
            if (ForceFilter) {
                var selected = SelectedFilter;
                FilterTypes = QueueFilterType.GetAll();
                SelectedFilter = FilterTypes.FirstOrDefault(e => e.VisitStatus == selected.VisitStatus);
                ForceFilter = false;
            }

            // If queue already filtered, return
            if (!FilterTypes.IsNullOrEmpty())
                return;

            // Filter by default
            FilterTypes = QueueFilterType.GetAll();
            SelectedFilter = FilterTypes.FirstOrDefault();


        }


        public async Task GetLastPatient()
        {
            var queueWindow = _container.Resolve<QueueWindowViewModel>();
            await queueWindow.Initialize();

            var lastQueue = queueWindow.QueueList
                .OrderByDescending(e => e.QueueIndex).FirstOrDefault();
            if (lastQueue != null)
            {
                LastQueueIndex = lastQueue.QueueIndex;
                LastPatientName = lastQueue.PatientVisit?.Patient?.FullName;
            }

            await Initialize();
        }

        public DateTime? SelectedDate { get; set; }

        public bool IsReceptionMode { get; set; }
        public int TotalPatients { get; set; }
        public int StartedPatients { get; set; }
        public int PendingPatients { get; set; }
        public int DonePatients { get; set; }

        public bool IsClinicDevice => Global.DeviceName.ToLower() == "clinic";

        public bool ForceFilter { get; set; }

        public bool QueueListHasData { get; set; }

        public List<QueueItem> BaseQueueList { get; set; }
        public ObservableCollection<QueueItem> QueueList { get; set; }

        public List<QueueFilterType> FilterTypes { get; set; }

        private QueueFilterType _selectedFilter;
       

        public QueueFilterType SelectedFilter {
            get => _selectedFilter;
            set {
                _selectedFilter = value;
                if (value?.VisitStatus == null)
                    QueueList = new ObservableCollection<QueueItem>(BaseQueueList);
                else
                    QueueList = new ObservableCollection<QueueItem>(
                        BaseQueueList.Where(e => e.PatientVisit.VisitStatus == (int)value.VisitStatus.Value));
                QueueListHasData = !QueueList.IsNullOrEmpty();
            }
        }



        public ICommand RemoveSelectedQueueCommand { get; set; }
        public ICommand AddToQueueWindowCommand { get; set; }
        public ICommand StartServiceCommand { get; set; }
        public ICommand GoPatientFileCommand { get; set; }
        public ICommand BackToQueueCommand { get; set; }
        public ICommand BackHomeCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand GoReceptionCommand { get; set; }
        public ICommand GoSearchForTestCommand { get; set; }
        public ICommand RequestNextPatientCommand { get; set; }


        public async Task AskForCost(PatientDto patient) {
            // Ask for total cost of service and how much patient payed
            var askForCost = _container.Resolve<AskForCostViewModel>();
            await askForCost.Initialize(patient);
            _dialogService.ShowEditorDialog(askForCost.GetView() as AskForCostView, async () => {
                var item = await askForCost.Save(patient.Id);
                if (item == null)
                    return false;

                item.Patient = patient;
                item.Remaining = askForCost.SelectedNotPayReason != null
                            ? 0 : askForCost.Cost - askForCost.Payment;


              
                if (SelectedDate == DateTime.Now.Date) {
                    BaseQueueList.Add(new QueueItem { PatientVisit = item });
                    App.Current.Dispatcher.Invoke((Action)delegate 
                    {
                        QueueList.Add(new QueueItem { PatientVisit = item });
                    });
                    QueueListHasData = !QueueList.IsNullOrEmpty();
                }
                return true;
            });
        }

        private async Task QueueItemStarted(PatientVisitDto patientVisit) {
            await _patientVisitRepository.ChangeVisitStatus(patientVisit.Id);
            QueueList.Remove(QueueList.FirstOrDefault(e => e.PatientVisit.Id == patientVisit.Id));
            BaseQueueList.Remove(QueueList.FirstOrDefault(e => e.PatientVisit.Id == patientVisit.Id));
            QueueListHasData = !QueueList.IsNullOrEmpty();
        }



        public void AddToQueueWindow(QueueItem queue) {
            var indexSelector = _container.Resolve<IndexSelectorViewModel>();

            _dialogService.ShowEditorDialog(indexSelector.GetView() as IndexSelector.IndexSelector,
                async () => {
                    await _container.Resolve<QueueWindowViewModel>()
                        .Add(new QueueItem { PatientVisit = queue.PatientVisit },
                            indexSelector.Index);

                    await GetLastPatient();
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

        public void RemoveSelectedQueue(QueueItem queue) {
            if (!_container.CheckUserRoleSilent(UserRoles.Admin) )
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
                    await _patientVisitRepository.RemoveFromQueue(queue.PatientVisit.Id);
                    QueueList.Remove(queue);
                    BaseQueueList.Remove(queue);
                    QueueListHasData = !QueueList.IsNullOrEmpty();
                    await _container.Resolve<QueueWindowViewModel>().Remove(queue);
                });             
            };
            _container.Resolve<IDialogService>()
                   .ShowPopupContent(pwd.GetView() as PasswordInputView);
        }

        public void StartService(QueueItem queue) {
            BusyExecute(async () => {
                await QueueItemStarted(queue.PatientVisit);
            });
        }

        public void GoPatientFile(QueueItem queue) {
            BusyExecute(async () => {
                var patientFile = _container.Resolve<PatientFileViewModel>();
                await patientFile.Initialize(queue.PatientVisit.PatientId, queue.PatientVisit.Id, IsReceptionMode);

                if (IsReceptionMode)
                    _dialogService.ShowFullScreenPopupContent(patientFile.GetView() as PatientFileView);
                else
                    _container.Navigate(patientFile.GetView() as PatientFileView);
            });
        }

        public void BackToQueue(QueueItem queue) {
            //if (_container.CheckUserRoleSilent(UserRoles.Reception))
            //    return;
            if (!_container.CheckUserRole(UserRoles.Admin,UserRoles.Seller))
                return;
            if (queue.PatientVisit.VisitStatus == (int)PatientVisitStatus.Created) {
                _container.Resolve<INotificationService>()
                    .Warning("This patient already in the Queue");
                return;
            }


            BusyExecute(async () => {
                await _patientVisitRepository.ChangeVisitStatus(queue.PatientVisit.Id,
                    PatientVisitStatus.Created);
                ForceFilter = true;
                await Initialize();

                if (SelectedFilter.VisitStatus != PatientVisitStatus.Created) {
                    QueueList.Remove(queue);
                    BaseQueueList.Remove(queue);
                    QueueListHasData = !QueueList.IsNullOrEmpty();
                }
            });
        }

        private void BackHome() {
            BusyExecute(async () => await _container.BackHome());
        }

        private void Refresh() {
            ForceFilter = true;
            var total = StartedPatients;

            BusyExecute(async () => {
                if (Global.DeviceName.ToLower() != "clinic") {
                    await GetAllVisits();
                    return;
                }

                for (int i = 0; i < 10; i++) {
                    var startedVisits = (await _patientVisitRepository.GetVisitsByDate(SelectedDate ?? DateTime.Now.Date,
                        !IsReceptionMode))
                        .Where(e => e.VisitStatus == (int)PatientVisitStatus.Started)
                        .ToList();

                    if (startedVisits.Count > total)
                        break;
                    await Task.Delay(2000);
                }

                await GetAllVisits();
                OnPropertyChanged(nameof(SelectedFilter));
            });
        }
        public async Task GetAllVisits()
        {

            SelectedDate ??= DateTime.Now.Date;
            var visitsToday =
                await _patientVisitRepository.GetVisitsByDate(SelectedDate.Value, !IsReceptionMode);

            TotalPatients = visitsToday.Count;
            StartedPatients = visitsToday.Count(e => e.VisitStatus == (int)PatientVisitStatus.Started);
            PendingPatients = visitsToday.Count(e => e.VisitStatus == (int)PatientVisitStatus.Created);
            DonePatients = visitsToday.Count(e => e.VisitStatus == (int)PatientVisitStatus.Finished);
            BaseQueueList = visitsToday.Select(e => new QueueItem
            {
                QueueIndex = QueueList?.Count + 1 ?? 1,
                PatientVisit = e
            }).ToList();
            QueueList = new ObservableCollection<QueueItem>(BaseQueueList);
            QueueListHasData = !QueueList.IsNullOrEmpty();

        }

        private void GoReception() {
            BusyExecute(async () => {
                var reception = _container.Resolve<ReceptionViewModel>();
                await reception.Initialize();
                _dialogService.ShowFullScreenPopupContent(reception.GetView() as ReceptionView);
            });
        }

        private void GoSearchForTest() {
            BusyExecute(async () => {
                var eyeImages = _container.Resolve<EyeImagesViewModel>();
                await eyeImages.Initialize();
                _dialogService.ShowFullScreenPopupContent(eyeImages.GetView() as EyeImagesView);
            });
        }
    }
}
