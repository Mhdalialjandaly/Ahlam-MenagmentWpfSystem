using System;
using System.Collections.Generic;

namespace EyeClinic.Model
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string ReservationTime { set; get; }
        public string PatientVisitType { set; get; }

    }
}
