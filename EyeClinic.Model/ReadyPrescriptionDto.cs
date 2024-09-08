using System;
using System.Collections.Generic;
using EyeClinic.Model.Custom;

namespace EyeClinic.Model
{
    public class ReadyPrescriptionDto
    {
        public int Id { get; set; }
        public string EnName { get; set; }
        public string ArName { get; set; }
        public bool Disabled { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public List<ReadyPrescriptionMedicineDto> ReadyPrescriptionMedicines { get; set; }

        public override string ToString() {
            var code = CurrentLanguageCodeHandler.ArLanguageCode;

            return CurrentLanguageCodeHandler.CurrentLanguageCode == code
                ? $"{ArName}"
                : $"{EnName}";
        }
    }
}
