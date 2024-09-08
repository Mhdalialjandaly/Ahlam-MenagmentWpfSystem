using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeClinic.Model
{
    public class CartoonLabelsDto
    {
        public int Id { get; set; }
        public int CartoonNameId { get; set; }
        public DateTime CreatedDate { get; set; } 
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int ThePaperCode { set; get; }
        public int Entry { set; get; }
        public int Extry { set; get; }
        public double DEntry { set; get; }
        public double DExtry { set; get; }
        public int FirstDayValue { set; get; }
        public int MinimumValue { set; get; }
        public double CurrentValue { get; set; }
        public string Note { set; get; }
        public double DetailedCurrentValue => CurrentValue + DEntry - DExtry; 
    }
}
