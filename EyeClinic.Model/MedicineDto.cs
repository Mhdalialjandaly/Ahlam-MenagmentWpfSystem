using System;

namespace EyeClinic.Model
{
    public class MedicineDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string MedicineName { get; set; }
        public int MedicineTypeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual MedicineTypeDto MedicineType { get; set; }


        public override string ToString() {
            return $"{Code} - {MedicineName}";
        }
    }
}
