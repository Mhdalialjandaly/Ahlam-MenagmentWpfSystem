using System;
using System.Collections.Generic;

#nullable disable

namespace EyeClinic.DataAccess.Entities
{
    public partial class Reservation : IEntityModel
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string ReservationTime { set; get; }
        public string PatientVisitType { set; get; }

    }
}
