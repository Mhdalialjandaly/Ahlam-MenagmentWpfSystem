using System;

namespace EyeClinic.Model
{
    public class PatientVisitGlassDto
    {
        public int Id { get; set; }
        public int PatientVisitId { get; set; }
        public string RSph { get; set; }
        public string LSph { get; set; }
        public string RSph2 { get; set; }
        public string LSph2 { get; set; }
        public string RCyl { get; set; }
        public string LCyl { get; set; }
        public string RCyl2 { get; set; }
        public string LCyl2 { get; set; }
        public string RAxis { get; set; }
        public string LAxis { get; set; }
        public string RAxis2 { get; set; }
        public string LAxis2 { get; set; }
        public string RPrism { get; set; }
        public string LPrism { get; set; }
        public string RPrism2 { get; set; }
        public string LPrism2 { get; set; }
        public string RBase { get; set; }
        public string LBase { get; set; }
        public string RBase2 { get; set; }
        public string LBase2 { get; set; }
        public string RIpd { get; set; }
        public string LIpd { get; set; }
        public string RIpd2 { get; set; }
        public string LIpd2 { get; set; }
        public string RVa { get; set; }
        public string LVa { get; set; }
        public string RVa2 { get; set; }
        public string LVa2 { get; set; }
        public bool AddVision { get; set; }
        public bool ContactLenses { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public PatientVisitDto PatientVisit { get; set; }

    }
}
