using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.WPF.AppServices.Print;
using EyeClinic.WPF.Base;

namespace EyeClinic.WPF.Components.Management
{
    public class ManagementViewModel : BaseViewModel<ManagementView>
    {
        private readonly IPrintService _printService;

        public ManagementViewModel(IPrintService printService) {
            _printService = printService;

            PrintTestPageCommand = new RelayCommand(PrintTestPage);
        }

        private void PrintTestPage() {
        }

        public override Task Initialize() {
            Printers = _printService.GetAllPrinters();
            return base.Initialize();
        }

        public List<Printer> Printers { get; set; }
        public Printer SelectedPrinter { get; set; }

        public ICommand PrintTestPageCommand { get; set; }
    }

    public class Printer
    {
        public string Name { get; set; }

        public override string ToString() { return Name; }
    }
}
