using System;

namespace EyeClinic.Model
{
    public class MedicineDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string MedicineName { get; set; }
        public double MedicinCount { get; set; }
        public double AvailibleMedicinCount { get; set; }
        public int MedicineTypeId { get; set; }
        public double TotalEntry { get; set; }
        public double TotalExtry { get; set; }
        public double TotalWaste { get; set; }
        public double TotalResult { get; set; }
        public double FirstTermValue { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual MedicineTypeDto MedicineType { get; set; }


        public override string ToString() {
            return $"{Code} - {MedicineName}";
        }
    }
}
