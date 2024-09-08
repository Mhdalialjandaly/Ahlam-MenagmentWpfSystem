using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Base.Extends;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.Tests.TestPicker
{
    public class TestPickerViewModel : BaseViewModel<TestPickerView>
    {
        private readonly ITestsRepository _testRepository;

        public TestPickerViewModel(ITestsRepository testRepository) {
            _testRepository = testRepository;
            
            SearchCommand = new RelayCommand(SearchAsync);
        }

        

        public override async Task Initialize() {
            Tests = (await _testRepository.GetByIsProduct(false))
                .Select(e => new Checkable<TestDto>() {
                    Item = e
                }).OrderBy(e=>e.Item.Code).ToList();            
        }

        public List<Checkable<TestDto>> Tests { get; set; }
        public string SearchText { get; set; }
        public ICommand SearchCommand { get; set; }
        private  void SearchAsync()
        {
            BusyExecute(async () =>
            {
                SearchText ??= "";
                if (SearchText == "")
                {
                    Tests = (await _testRepository.GetByIsProduct(false))
                   .Select(e => new Checkable<TestDto>()
                   {
                       Item = e
                   }).OrderByDescending(e => e.Item.LastModifiedDate).ToList();
                }
                else
                {
                    Tests = (await _testRepository.GetByIsProduct(false))
                   .Where(e => e.ArName.ToLower().StartsWith(SearchText.ToLower()))
                   .Select(e => new Checkable<TestDto>()
                   {
                       Item = e
                   }).OrderByDescending(e => e.Item.LastModifiedDate).ToList();
                }
               
            });
        }

    }
}
