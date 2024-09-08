using AutoMapper;
using EyeClinic.DataAccess.Entities;
using EyeClinic.Model;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Shell.QueueWindow;
using EyeClinic.WPF.DataModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace EyeClinic.WPF.Components.Home.Operation.OrderQueueWindow
{
    
        public class OrderQueueWindowViewModel : BaseViewModel<OrderQueueWindowView>
        {
            private readonly EyeClinicContext _context;
            private readonly IMapper _mapper;


            public OrderQueueWindowViewModel(EyeClinicContext context,
                IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
                OrderQueueList = new ObservableCollection<OrderQueueItem>();
                OrderQueueList1 = new ObservableCollection<OrderQueueItem>();
            }


            public override async Task Initialize()
            {
                var queues = await _context.OrderQueues
                    .AsNoTracking()
                    .Include(e => e.publicCustomer)
                    .Where(e => e.OrderDate.Date == DateTime.Now.Date)
                    .ToListAsync();

                OrderQueueList = new ObservableCollection<OrderQueueItem>(
                    queues.Select(e => new OrderQueueItem
                    {
                        OrderQueueIndex = e.OrderIndex,
                        LastPublicCustomerName = e.publicCustomer.FirstName,
                        PublicCustomerOrder = _mapper.Map<PublicCustomerOrderDto>(e)
                    }).ToList());

                RemoveFromOrderQueueCommand = new RelayCommand<OrderQueueItem>(RemoveFromOrderQueue);


                OrderQueueList1 = new ObservableCollection<OrderQueueItem>(
                    queues.Select(e => new OrderQueueItem
                    {
                        OrderQueueIndex = e.OrderIndex,
                        LastPublicCustomerName = e.publicCustomer.FirstName,
                        PublicCustomerOrder = _mapper.Map<PublicCustomerOrderDto>(e)
                    }).ToList());

                RemoveFromOrderQueueCommand = new RelayCommand<OrderQueueItem>(RemoveFromOrderQueue);

                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
                timer.Tick += TimerOnTick;
                timer.Start();

            }

            public ObservableCollection<OrderQueueItem> OrderQueueList { get; set; }
            public ObservableCollection<OrderQueueItem> OrderQueueList1 { get; set; }

            public string CurrentTime { get; set; }

            public string CurrentText { get; set; }

            public ICommand CloseCommand { get; set; }

            public ICommand SmallMinimizeCommand { set; get; }
            public ICommand MinimizeCommand { get; set; }
            public ICommand MaximizeCommand { get; set; }
        public ICommand RemoveFromOrderQueueCommand { get; set; }
        private void TimerOnTick(object sender, EventArgs e)
            {
                CurrentTime = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
                if (DateTime.Now.TimeOfDay.Minutes / 2 % 2 == 0)
                {
                    CurrentText = "القسم الاداري";
                }
                else
                {
                    CurrentText = "نظام ادارة الطلبات";
                }
            }


            //public async Task Add(OrderQueueItem queueItem, int index)
            //{
            //    queueItem.OrderQueueIndex = index;
            //    var queue = _mapper.Map<OrderQueue>(queueItem.PublicCustomerOrder);
            //    var visit = await _context.PublicCustomerOrders
            //        .FirstOrDefaultAsync(e => e.Id == queue.Id);
            //    if (visit != null)
            //    {
            //        visit.OrderQueueIndex = queueItem.OrderQueueIndex;
            //    }

            //    queue.OrderIndex = index;

            //    queue.Id = 0;
            //    queue.PublicCustomer = null;

            //    await _context.OrderQueues.AddAsync(queue);
            //    await _context.SaveChangesAsync();
            //    OrderQueueList.Add(queueItem);

            //    OrderQueueList1.Add(queueItem);
            //}
           


            public async Task Remove(OrderQueueItem queueItem)
            {
                var existQueue = OrderQueueList
                    .FirstOrDefault(e => e.PublicCustomerOrder.PublicCustomerId == queueItem.PublicCustomerOrder.PublicCustomerId);
                if (existQueue != null)
                {
                    var existDbItem = await _context.OrderQueues.FirstOrDefaultAsync(
                        e => e.PublicCustomerId == queueItem.PublicCustomerOrder.PublicCustomerId);
                    if (existDbItem != null)
                    {
                        _context.OrderQueues.Remove(existDbItem);
                        await _context.SaveChangesAsync();
                    }

                    OrderQueueList.Remove(existQueue);
                    OrderQueueList1.Remove(existQueue);
                }
            }

            public void RemoveFromOrderQueue(OrderQueueItem queue)
            {
                BusyExecute(async () =>
                {
                    var item = await _context.OrderQueues
                        .FirstOrDefaultAsync(e => e.Id == queue.PublicCustomerOrder.Id);
                    if (item != null)
                    {
                        _context.OrderQueues.Remove(item);
                        await _context.SaveChangesAsync();

                        OrderQueueList.Remove(queue);
                        OrderQueueList1.Remove(queue);

                    }
                });
            }

        public async Task Add(OrderQueueItem orderQueueItem, int index)
        {
            //orderQueueItem.OrderQueueIndex = index;
            //var queue = _mapper.Map<OrderQueue>(orderQueueItem.PublicCustomerOrder);
            //var visit = await _context.PublicCustomerOrders
            //    .FirstOrDefaultAsync(e => e.Id == queue.Id);
            //if (visit != null)
            //{
            //    visit.QueueIndex = orderQueueItem.OrderQueueIndex;
            //}

            //queue.OrderIndex = index;

            //queue.Id = 0;
            //queue.PublicCustomer = null;

            //await _context.OrderQueues.AddAsync(queue);
            //await _context.SaveChangesAsync();
            //OrderQueueList.Add(orderQueueItem);

            //OrderQueueList1.Add(orderQueueItem);
        }
    }
}
