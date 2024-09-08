using System.Collections.Generic;
using EyeClinic.Core.Enums;

namespace EyeClinic.WPF.Base.Interfaces
{
    public interface IPatientWizardStep : IBaseViewModel
    {
        int Index { get; set; }
        void AppendData<T>(ref T model);
        void Validate(ref List<string> errors);
    }
}
