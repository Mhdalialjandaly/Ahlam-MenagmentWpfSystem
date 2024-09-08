using System;
using System.Collections.Generic;
using EyeClinic.WPF.Base.Interfaces;

namespace EyeClinic.WPF.AppServices.DialogService.Wizard
{
    public interface IWizardViewModel : IBaseViewModel
    {
        event Action OnSave;
        event Action OnCancel;
        List<IPatientWizardStep> Steps { get; set; }
        void AppendData<T>(ref T model);
    }
}
