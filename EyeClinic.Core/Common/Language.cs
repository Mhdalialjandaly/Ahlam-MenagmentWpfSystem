using System.Collections.Generic;
using EyeClinic.Model.Custom;

namespace EyeClinic.Core.Common
{
    public class Language
    {
        public string Name { get; set; }
        public string Code { get; set; }

        public List<Language> GetLanguages() {
            return new()
            {
                new Language { Code = CurrentLanguageCodeHandler.ArLanguageCode, Name = "Arabic" },
                new Language { Code = CurrentLanguageCodeHandler.EnLanguageCode, Name = "English" }
            };
        }
    }
}
