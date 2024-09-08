using System;

namespace EyeClinic.Model
{
    public class MedicineUsageDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string UsageName { get; set; }
        public int UsageMedicineTypeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual MedicineTypeDto UsageMedicineType { get; set; }


        public override string ToString() {
            return $"{Code} - {UsageName}";
        }
    }
}
