using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using AutoMapper;
using EyeClinic.DataAccess.Entities;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.Localization;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.DataModel;
using Microsoft.EntityFrameworkCore;

namespace EyeClinic.WPF.Components.Shell.QueueWindow
{
    public class QueueWindowViewModel : BaseViewModel<QueueWindowView>
    {
        private readonly EyeClinicContext _context;
        private readonly IMapper _mapper;


        public QueueWindowViewModel(EyeClinicContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            QueueList = new ObservableCollection<QueueItem>();
            QueueList1 = new ObservableCollection<QueueItem>();


        }


        public override async Task Initialize()
        {
            var queues = await _context.Queues
                .AsNoTracking()
                .Include(e => e.Patient)
                .Where(e => e.VisitDate.Date == DateTime.Now.Date)
                .ToListAsync();

            QueueList = new ObservableCollection<QueueItem>(
                queues.Select(e => new QueueItem
                {
                    QueueIndex = e.VisitIndex,
                    LastPatientName = e.Patient.FirstName,
                    PatientVisit = _mapper.Map<PatientVisitDto>(e)
                }).ToList());

            RemoveFromQueueCommand = new RelayCommand<QueueItem>(RemoveFromQueue);


            QueueList1 = new ObservableCollection<QueueItem>(
                queues.Select(e => new QueueItem
                {
                    QueueIndex = e.VisitIndex,
                    LastPatientName = e.Patient.FirstName,
                    PatientVisit = _mapper.Map<PatientVisitDto>(e)
                }).ToList());

            RemoveFromQueueCommand = new RelayCommand<QueueItem>(RemoveFromQueue);

            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += TimerOnTick;
            timer.Start();

        }

        public ObservableCollection<QueueItem> QueueList { get; set; }
        public ObservableCollection<QueueItem> QueueList1 { get; set; }

        public string CurrentTime { get; set; }

        public string CurrentText { get; set; }

        public ICommand CloseCommand { get; set; }

        public ICommand SmallMinimizeCommand { set; get; }
        public ICommand MinimizeCommand { get; set; }
        public ICommand MaximizeCommand { get; set; }
        private void TimerOnTick(object sender, EventArgs e)
        {
            CurrentTime = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
            if (DateTime.Now.TimeOfDay.Minutes/2 %2==0)
            {
                CurrentText = "القسم الاداري";              
            }
            else
            {
                CurrentText = "نظام ادارة الطلبات";
            }
        }


        public async Task Add(QueueItem queueItem, int index)
        {
            queueItem.QueueIndex = index;
            var queue = _mapper.Map<Queue>(queueItem.PatientVisit);
            var visit = await _context.PatientVisits
                .FirstOrDefaultAsync(e => e.Id == queue.Id);
            if (visit != null)
            {
                visit.QueueIndex = queueItem.QueueIndex;
            }

            queue.VisitIndex = index;

            queue.Id = 0;
            queue.Patient = null;

            await _context.Queues.AddAsync(queue);
            await _context.SaveChangesAsync();
            QueueList.Add(queueItem);

            QueueList1.Add(queueItem);
        }
        public ICommand RemoveFromQueueCommand { get; set; }


        public async Task Remove(QueueItem queueItem)
        {
            var existQueue = QueueList
                .FirstOrDefault(e => e.PatientVisit.PatientId == queueItem.PatientVisit.PatientId);
            if (existQueue != null)
            {
                var existDbItem = await _context.Queues.FirstOrDefaultAsync(
                    e => e.PatientId == queueItem.PatientVisit.PatientId);
                if (existDbItem != null)
                {
                    _context.Queues.Remove(existDbItem);
                    await _context.SaveChangesAsync();
                }

                QueueList.Remove(existQueue);
                QueueList1.Remove(existQueue);
            }
        }

        public void RemoveFromQueue(QueueItem queue)
        {
            BusyExecute(async () =>
            {
                var item = await _context.Queues
                    .FirstOrDefaultAsync(e => e.Id == queue.PatientVisit.Id);
                if (item != null)
                {
                    _context.Queues.Remove(item);
                    await _context.SaveChangesAsync();

                    QueueList.Remove(queue);
                    QueueList1.Remove(queue);

                }
            });
        }

    }
}
