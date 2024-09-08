using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Home.Reception.Queue.IndexSelector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeClinic.WPF.Components.Home.Operation.OrderQueue.OrderIndexSelecter
{
    
        public class OrderIndexSelecterViewModel : BaseViewModel<OrderIndexSelecterView>
        {
            public int Index { get; set; }
        }
    
}
