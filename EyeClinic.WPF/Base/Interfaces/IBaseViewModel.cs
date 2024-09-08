using System.Threading.Tasks;
using EyeClinic.Core.Enums;

namespace EyeClinic.WPF.Base.Interfaces
{
    public interface IViewModel : IPresentable
    {
        bool Initialized { get; set; }
        Operation Operation { get; set; }
    }

    public interface IBaseViewModel : IViewModel
    {
        Task Initialize();
    }
}
