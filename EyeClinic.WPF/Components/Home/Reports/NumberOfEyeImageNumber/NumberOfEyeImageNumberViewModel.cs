using EyeClinic.DataAccess.IRepositories;
using EyeClinic.WPF.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeClinic.WPF.Components.Home.Reports.NumberOfEyeImageNumber
{
    internal class NumberOfEyeImageNumberViewModel : BaseViewModel<NumberOfEyeImageNumberView>
    {

        private readonly IReportRepository _reportRepository;
        private readonly IOperationRepository _operationRepository;

        public NumberOfEyeImageNumberViewModel(IReportRepository reportRepository,
            IOperationRepository operationRepository)
        {
            _reportRepository = reportRepository;
            _operationRepository = operationRepository;


        }
    }
}
