using System;

namespace EyeClinic.WPF.Components.Home.Reception.Review
{
    public class ReviewDataModel
    {
        public int? PatientVisitId { get; set; }
        public int? ReservationId { get; set; }
        public string PatientName { get; set; }
        public DateTime DateTime { get; set; }
        public string PatinetReviewTime { set; get; }
        public string PhoneNumber { get; set; }
        public string Type { get; set; }
        public string Time { get; set; }
        public string PatientVisitType { get; internal set; }
        //public string PatientVisitType { get;  set; }
    }
}
