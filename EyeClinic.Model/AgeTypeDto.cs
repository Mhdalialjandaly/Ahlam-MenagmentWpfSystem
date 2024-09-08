using System;
using System.Collections.Generic;
using EyeClinic.Model.Custom;

namespace EyeClinic.Model
{
    public class AgeTypeDto
    {
        public int Id { get; set; }
        public string EnName { get; set; }
        public string ArName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public static List<AgeTypeDto> Create() {
            return new()
            {
                new() { Id = 0, EnName = "يوم", ArName = "يوم" },
                new() { Id = 1, EnName = "شهر", ArName = "شهر" },
                new() { Id = 2, EnName = "سنة", ArName = "سنة" },
            };
        }

        public override string ToString() {
            var code = CurrentLanguageCodeHandler.ArLanguageCode;

            return CurrentLanguageCodeHandler.CurrentLanguageCode == code
                ? ArName
                : EnName;
        }
    }
}
