using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using operation = EyeClinic.Core.Enums.Operation;

namespace EyeClinic.WPF.Components.Home.ReadyItems.WoreHouse.WareHouseEditer
{
    public partial class WareHouseEditerViewModel 
    {       
        public WareHouseEditerViewModel() { }
    }
    public partial class WareHouseEditerViewModel:BaseViewModel<WareHouseEditerView>
    {
        private readonly INotificationService _notificationService;
        private readonly IWareHouseReadyMaterialRepository _materialRepository;
        public  WareHouseEditerViewModel(INotificationService notificationService)
        {
            _notificationService = notificationService;

            UnitItem = new List<string> { "غ", "كغ", "ل", "مل" };
            DateOf = DateTime.Now;
        }

        public int Id { get; set; }
        public DateTime DateOf { get; set; }
        public string Code { get; set; }
        public string EnName { get; set; }
        public string ArName { get; set; }
        public string Unit { get; set; }
        public double UnitValue { get; set; }
        public double IncreasUnitValue { get; set; }
        public string SelectedUnit { set; get; }
        public List<string> UnitItem { get; set; }
        public double Quintity { get; set; }
        public double FirstTermBalanceWareHouseValue { get; set; }
        public double TotalWasteWareHouseValue { get; set; }
        public double TotalWareHouseValue { get; set; }
        public double TotalEntryWareHouseValue { get; set; }
        public string FirstTermBalance { get; set; }
        public DateTime CreatedDate { get; set; }  
        public double TotalWaste { get; set; }
        public double TotalWight { get; set; }
        public double TotalValue { get; set; }
        public ICommand AddImageCommand1 { set; get; }
        public ICommand AddImageCommand2 { set; get; }
        public ICommand AddImageCommand3 { set; get; }
        public ICommand AddPdfFileCommand { set; get; }
        public async Task<Model.WareHouseReadyMaterialDto> Save()
        {
            if (ValidForSave())
            {
                ConvertUnits();
                var item = BuildFromProperties();
                //var Item2 = BuildReadyItemFromProperties(item);
                if (Operation == operation.Add)
                {
                    var addedItem = await _materialRepository.Add(item);
                    _notificationService.Success("تم الحفظ .");
                    return addedItem;
                }

                if (Operation == operation.Edit && Id > 0)
                {
                    item.Id = Id;
                    item.CreatedDate = CreatedDate;
                    item.LastModifiedDate = DateTime.Now;
                    await _materialRepository.Update(item);
                    //BusyExecute(async () => { await _materialRepository.Add(Item2); });
                    _notificationService.Success("تم التعديل .");

                    return item;
                }
            }

            return null;

        }

        private bool ValidForSave()
        {


            return !string.IsNullOrWhiteSpace(EnName) && !string.IsNullOrWhiteSpace(ArName)
               && !string.IsNullOrWhiteSpace(Code);


        }
        public void ConvertUnits()
        {
            switch (SelectedUnit)
            {
                case "غ":
                    UnitValue = UnitValue / 1000;
                    SelectedUnit = "كغ";
                    break;
                case "مل":
                    UnitValue = UnitValue / 1000;
                    SelectedUnit = "ل";
                    break;
            }
        }

        private Model.WareHouseReadyMaterialDto BuildFromProperties()
        {
            return new()
            {
                Id = Id,
                Code = Code,
                ArName = ArName,
                EnName = EnName,
                CreatedDate = DateOf,              
                Unit = SelectedUnit,
                Quintity = Quintity,
                FirstTermBalance = FirstTermBalance,
                UnitValue = UnitValue + IncreasUnitValue,
                IsProduct = true
            };
        }
        //private ReadyProductDbo BuildReadyItemFromProperties(TestDto SelectedCartoonName)
        //{

        //    return new()
        //    {
        //        ArName = "اضافة",
        //        ProductId = SelectedCartoonName.Id,
        //        CreatedValue = IncreasUnitValue,
        //        CreatedDate = DateTime.Now,
        //        Note = "Note",
        //        ProductedValue = 0,
        //        ExportedValue = 0,
        //        WasteValue = 0,
        //        ProductedValueUnit = "-",
        //        ExportedValueUnit = "-",
        //        WasteValueUnit = "-",
        //        CreatedValueUnit = "-",
        //        TotalValue = 0,
        //        TotalResult = 0,
        //        IsIncreaseDogma = true
        //    };
        //}
        public void BuildFromModel(Model.TestDto disease)
        {
            Id = disease.Id;
            Code = disease.Code;
            ArName = disease.ArName;
            EnName = disease.EnName;
            DateOf = disease.CreatedDate;           
            Quintity = disease.Quintity;
            FirstTermBalance = disease.FirstTermBalance;
            SelectedUnit = disease.Unit;
            FirstTermBalanceWareHouseValue=disease.FirstTermBalanceWareHouseValue;
            TotalWareHouseValue = disease.TotalWareHouseValue; 
            TotalWasteWareHouseValue=disease.TotalWasteWareHouseValue;
            TotalEntryWareHouseValue=disease.TotalEntryWareHouseValue;
        }
        public void BuildFromTestModel(Model.TestDto disease)
        {
            Id = disease.Id;
            Code = disease.Code;
            ArName = disease.ArName;
            EnName = disease.EnName;
            DateOf = disease.CreatedDate;
            Quintity = disease.Quintity;
            FirstTermBalance = disease.FirstTermBalance;
            SelectedUnit = disease.Unit;
        }
    }
}
