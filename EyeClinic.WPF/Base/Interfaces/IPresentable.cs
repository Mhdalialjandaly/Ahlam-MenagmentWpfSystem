using EyeClinic.Model;
using System.Threading.Tasks;
using System.Windows;

namespace EyeClinic.WPF.Base.Interfaces
{
    public interface IPresentable
    {
        UIElement GetView();
      
    }
}
