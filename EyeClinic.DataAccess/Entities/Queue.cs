using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeClinic.DataAccess.Entities
{
    public class Queue
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public DateTime VisitDate { get; set; }
        public DateTime? ReviewDate { get; set; }
        public int Cost { get; set; }
        public int Payment { get; set; }
        public int? NotPaymentReasonId { get; set; }
        public int Remaining { get; set; }
        public byte VisitStatus { get; set; }
        public int VisitIndex { get; set; }
        public string Notes { get; set; }
        public string MedicalReport { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public Patient Patient { get; set; }
    }
}
