using System;

namespace EyeClinic.Model
{
    public class ReadyPrescriptionMedicineDto
    {
        public int Id { get; set; }
        public int ReadyPrescriptionId { get; set; }
        public int MedicineId { get; set; }
        public int MedicineUsageId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public MedicineDto Medicine { get; set; }
        public MedicineUsageDto MedicineUsage { get; set; }
    }
}
