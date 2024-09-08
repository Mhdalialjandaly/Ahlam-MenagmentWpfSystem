using EyeClinic.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeClinic.WPF.Components.Home.Operation.OrderQueue
{
   
        public class OrderQueueFilterType
        {
            public PublicCustomerOrderStatus? OrderStatus { get; set; }
            public string DisplayName { get; set; }

            public static List<OrderQueueFilterType> GetAll()
            {
                return new List<OrderQueueFilterType>()
            {
                new() {DisplayName = "Started", OrderStatus = PublicCustomerOrderStatus.Started},
                new() {DisplayName = "Pending", OrderStatus = PublicCustomerOrderStatus.Created},
                new() {DisplayName = "Done", OrderStatus = PublicCustomerOrderStatus.Finished},
                new() {DisplayName = "All", OrderStatus = null},
            };
            }

            public override string ToString() => DisplayName;
        }
    
}
