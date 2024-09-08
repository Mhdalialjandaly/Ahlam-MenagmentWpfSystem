using EyeClinic.DataAccess.IRepositories;
using EyeClinic.DataAccess.Migrations;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using operation = EyeClinic.Core.Enums.Operation;
namespace EyeClinic.WPF.Components.Home.ReadyItems.ReadyItemsProducting.AddDogmaEditer
{
    public partial class AddDogmaEditerViewModel {
        public AddDogmaEditerViewModel() { }
    }
    public partial class AddDogmaEditerViewModel : BaseViewModel<AddDogmaEditerView>
    {

        private readonly IReadyProductRepository _readyProductRepository;
        private readonly INotificationService _notificationService;

        public AddDogmaEditerViewModel( IReadyProductRepository readyProductRepository,INotificationService notificationService) 
        {
            _readyProductRepository = readyProductRepository;
            _notificationService = notificationService;


            CreatedValueUnit = new List<string> { "غ", "كغ", "مل", "ل" ,"قطعة"};
        }
        public int Id { get; set; }
        public string ArName { get; set; }
        public int ProductId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public double CreatedValue { set; get; }
        public List<string> CreatedValueUnit { get; set; }
        public string SelectedCreatedValueUnit { get; set; }
        public double ProductedValue { set; get; }
        public string SelectedProductedValueUnit { set; get; }
        public double ExportedValue { set; get; }
        public string SelectedExportedValueUnit { set; get; }
        public double WasteValue { set; get; }
        public string SelectedWasteValueUnit { set; get; }
        public double TotalWight { get; set; }
        public double TotalWaste { get; set; }
        public double TotalValue { get; set; }
        public double res { get; set; }
        public string Note { set; get; }
        public double BoolResult { get; set; }
        public double BoolValue { get; set; }
        public double AddValue { get; set; }
        public DateTime DateOf { get; set; } = DateTime.Now;



        public async Task<ReadyProductDbo> Save(TestDto SelectedCartoonName, double rest)
        {

            res = rest;
            //ConvertUnits();
            //GetResult(SelectedCartoonName);
            if (ValidForSave(SelectedCartoonName))
            {
                var item = BuildFromProperties(SelectedCartoonName);
                if (Operation == operation.Add)
                {
                    item.IsIncreaseDogma = true;
                    var addedItem = await _readyProductRepository.Add(item);
                    _notificationService.Success("تم");
                    return addedItem;
                }

                if (Operation == operation.Edit && Id > 0)
                {
                    item.IsIncreaseDogma = true;
                    item.Id = Id;
                    item.CreatedDate = CreatedDate.Date;
                    item.LastModifiedDate = DateTime.Now;
                    await _readyProductRepository.Update(item);
                    _notificationService.Success("تم");
                    return item;
                }
            }
            else
            {
                _notificationService.Warning("الرجاء التاكد من الادخالات");
            }

            return null;
        }

        private bool ValidForSave(TestDto testDto)
        {
            return AddValue >= 0 &&
                   !string.IsNullOrWhiteSpace(ArName);
        }
        //public void GetResult(TestDto testDto)
        //{
        //    double result = testDto.Quintity;
        //    BoolValue = result;

        //    if (testDto.Unit == SelectedCreatedValueUnit && testDto.Unit == SelectedWasteValueUnit)
        //    {
        //        TotalValue = CreatedValue * ProductedValue;
        //    }
        //    else if (testDto.Unit == SelectedCreatedValueUnit && testDto.Unit != SelectedWasteValueUnit)
        //    {
        //        TotalValue = CreatedValue * ProductedValue;
        //        WasteValue = WasteValue * 0.9;
        //        SelectedWasteValueUnit = testDto.Unit;
        //    }
        //    else if (testDto.Unit != SelectedCreatedValueUnit && testDto.Unit == SelectedWasteValueUnit)
        //    {
        //        TotalValue = CreatedValue * ProductedValue * 0.9;
        //    }
        //    else
        //    {
        //        TotalValue = CreatedValue * ProductedValue * 0.9;
        //        WasteValue = WasteValue * 0.9;
        //        SelectedWasteValueUnit = testDto.Unit;
        //    }


        //    if (BoolValue >= TotalValue)
        //        BoolResult = BoolValue - TotalValue - ExportedValue - WasteValue ;
        //    else
        //        BoolResult = 0;


        //    if (WasteValue < 0 || WasteValue > BoolValue)
        //        _notificationService.Warning("قيمة الهدر غير مسموحة ");

        //    if (ExportedValue < 0 || ExportedValue > BoolValue)
        //        _notificationService.Warning("قيمة الاخراج غير مسموحة ");
        //}
        //public void ConvertUnits()
        //{
        //    switch (SelectedCreatedValueUnit)
        //    {
        //        case "غ":
        //            CreatedValue = CreatedValue / 1000;
        //            SelectedCreatedValueUnit = "كغ";
        //            break;
        //        case "مل":
        //            CreatedValue = CreatedValue / 1000;
        //            SelectedCreatedValueUnit = "ل";
        //            break;
        //    }

        //    switch (SelectedWasteValueUnit)
        //    {
        //        case "غ":
        //            WasteValue = WasteValue / 1000;
        //            SelectedWasteValueUnit = "كغ";
        //            break;
        //        case "مل":
        //            WasteValue = WasteValue / 1000;
        //            SelectedWasteValueUnit = "ل";
        //            break;
        //    }

        //}

        private ReadyProductDbo BuildFromProperties(TestDto SelectedCartoonName)
        {

            return new()
            {
                Id = Id,
                ArName = ArName,
                ProductId = SelectedCartoonName.Id,
                CreatedValue = CreatedValue,
                CreatedDate = DateOf.Date,
                Note = Note,
                ProductedValue = ProductedValue,
                ExportedValue = ExportedValue,
                WasteValue = WasteValue,
                ProductedValueUnit = SelectedProductedValueUnit,
                ExportedValueUnit = SelectedExportedValueUnit,
                WasteValueUnit = SelectedWasteValueUnit,
                CreatedValueUnit = SelectedCreatedValueUnit,
                TotalValue = TotalValue,
                IsIncreaseDogmaValue = AddValue,
                TotalResult = res + AddValue,
                IsIncreaseDogma = true
                
            };
        }


        public void BuildFromModel(ReadyProductDbo disease)
        {
            Id = disease.Id;
            ArName = disease.ArName;
            ProductId = disease.ProductId;
            CreatedDate = disease.CreatedDate.Date;
            Note = disease.Note;
            DateOf = disease.CreatedDate;
            CreatedValue = disease.CreatedValue;
            ProductedValue = disease.ProductedValue;
            ExportedValue = disease.ExportedValue;
            WasteValue = disease.WasteValue;
            SelectedCreatedValueUnit = disease.CreatedValueUnit;
            SelectedExportedValueUnit = disease.ExportedValueUnit;
            SelectedProductedValueUnit = disease.ProductedValueUnit;
            SelectedWasteValueUnit = disease.WasteValueUnit;
        }
        //public async Task<double> GetRest(TestDto selecteditem)
        //{
        //    return await _readyProductRepository.GetSumValueProducts(selecteditem);
        //}

    }
}
