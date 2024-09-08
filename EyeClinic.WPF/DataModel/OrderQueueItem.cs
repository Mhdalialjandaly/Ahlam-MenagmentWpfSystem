using EyeClinic.Model;
using EyeClinic.WPF.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeClinic.WPF.DataModel
{
   
        public class OrderQueueItem : Bindable
        {
            public PublicCustomerOrderDto PublicCustomerOrder { get; set; }
            public int OrderQueueIndex { get; set; }
            public string LastPublicCustomerName { get; set; }
            public string WaitingTimeh { get; set; }
            public string WaitingTimem { get; set; }
            public string WaitingTimed { get; set; }
        }
    
}
