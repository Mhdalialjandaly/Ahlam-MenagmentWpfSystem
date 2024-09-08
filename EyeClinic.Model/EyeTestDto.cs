using System;
using EyeClinic.Model.Custom;

namespace EyeClinic.Model
{
    public class EyeTestDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string EnName { get; set; }
        public string ArName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public override string ToString() {
            var code = CurrentLanguageCodeHandler.ArLanguageCode;

            return CurrentLanguageCodeHandler.CurrentLanguageCode == code
                ? $"{Code} - {ArName}"
                : $"{Code} - {EnName}";
        }
    }
}
