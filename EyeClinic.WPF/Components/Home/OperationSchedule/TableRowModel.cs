using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeClinic.WPF.Components.Home.OperationSchedule
{
    public class TableRowModel
    {
        public string Date { get; set; }
        public string Name { get; set; }
        public string SurgeryType { get; set; }
        public string Eye { get; set; }
        public string Ling { get; set; }
        public int? Session { get; set; }
        public string Image { get; set; }
        public string MedicalCenter { get; set; }
        public int Cost { get; set; }
        public int Paid { get; set; }
        public int Remaining { get; set; }
        public string Note { get; set; }
    }
}
