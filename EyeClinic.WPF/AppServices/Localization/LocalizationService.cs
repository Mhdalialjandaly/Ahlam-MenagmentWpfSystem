using System.Reflection;
using WPFLocalizeExtension.Extensions;

namespace EyeClinic.WPF.AppServices.Localization
{
    public class LocalizationService : ILocalizationService
    {
        public string Localize(string key) {
            return LocExtension.GetLocalizedValue<string>(key);
        }
    }
}
