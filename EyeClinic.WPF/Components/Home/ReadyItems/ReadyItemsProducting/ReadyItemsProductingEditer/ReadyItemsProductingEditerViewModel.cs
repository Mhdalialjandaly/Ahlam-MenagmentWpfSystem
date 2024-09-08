using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Base.Interfaces;
using EyeClinic.WPF.Components.Home.CartoonForm.ItemsEditer.ItemsEditerDialog;
using LFSO102Lib;
using Syncfusion.Windows.Shared;
using Syncfusion.XlsIO.Implementation.PivotAnalysis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media.Media3D;
using operation = EyeClinic.Core.Enums.Operation;

namespace EyeClinic.WPF.Components.Home.ReadyItems.ReadyItemsProducting.ReadyItemsProductingEditer
{
    public partial class ReadyItemsProductingEditerViewModel
    {
        public ReadyItemsProductingEditerViewModel() { }
    }

    public partial class ReadyItemsProductingEditerViewModel : BaseViewModel<ReadyItemsProductingEditerView>
    {
        private readonly IReadyProductRepository _cartoonItemRepository;
        private readonly INotificationService _notificationService;
        

        public ReadyItemsProductingEditerViewModel(IReadyProductRepository cartoonItemRepository,INotificationService notificationService)
        {
            _cartoonItemRepository = cartoonItemRepository;
            _notificationService = notificationService;
           

            CreatedValueUnit = new List<string> { "غ", "كغ", "مل", "ل" , "قطعة" };
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
        public double  BoolValue { get; set; }        
        public DateTime DateOf { get; set; } = DateTime.Now;



        public async Task<ReadyProductDbo> Save(TestDto SelectedCartoonName,double rest)
        {

            res = rest;
            ConvertUnits();
            GetResult(SelectedCartoonName);
            if (ValidForSave(SelectedCartoonName))
            {                
                var item = BuildFromProperties(SelectedCartoonName);               
                if (Operation == operation.Add)
                {
                    var addedItem = await _cartoonItemRepository.Add(item);                   
                    _notificationService.Success("تم");
                    return addedItem;
                }

                if (Operation == operation.Edit && Id > 0)
                {
                    item.Id = Id;
                    item.CreatedDate = CreatedDate.Date;
                    item.LastModifiedDate = DateTime.Now;
                    await _cartoonItemRepository.Update(item);
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
            if (testDto.Unit == "ل" && SelectedCreatedValueUnit == "كغ" || testDto.Unit == "ل" && SelectedWasteValueUnit == "كغ")
            {
                _notificationService.Warning("الرجاء اخراج مادة من نفس قيمة وحدة المنتج");
                return false;
            }
            if (BoolValue < TotalValue + WasteValue + ExportedValue)
            {
                _notificationService.Warning("لا يوجد مواد خام كافية");
                return false;
            }
            return CreatedValue != 0 &&
                   ProductedValue != 0 &&
                   BoolResult >= 0;
               
        }
        public   void GetResult(TestDto testDto)
        {
            double result = res ;     
            BoolValue =  result;
           
                if (testDto.Unit == SelectedCreatedValueUnit && testDto.Unit == SelectedWasteValueUnit)
                {
                    TotalValue = CreatedValue * ProductedValue;
                SelectedWasteValueUnit = testDto.Unit;
                SelectedProductedValueUnit = testDto.Unit;
            }
                else if (testDto.Unit == SelectedCreatedValueUnit && testDto.Unit != SelectedWasteValueUnit)
                {
                    TotalValue = CreatedValue * ProductedValue;
                    WasteValue = WasteValue * 0.9;
                    SelectedWasteValueUnit = testDto.Unit;
                    SelectedProductedValueUnit = testDto.Unit;
                }
                else if (testDto.Unit != SelectedCreatedValueUnit && testDto.Unit == SelectedWasteValueUnit)
                {
                    TotalValue = CreatedValue * ProductedValue * 0.9;
                SelectedWasteValueUnit = testDto.Unit;
                SelectedProductedValueUnit = testDto.Unit;
            }
                else
            {
                    TotalValue = CreatedValue * ProductedValue * 0.9;
                    WasteValue = WasteValue * 0.9;
                    SelectedWasteValueUnit = testDto.Unit;
                    SelectedProductedValueUnit = testDto.Unit;

            }




            if (BoolValue >= TotalValue)
                BoolResult = BoolValue - TotalValue - ExportedValue - WasteValue ;
            else
                BoolResult = 0;
                

            if (WasteValue < 0 || WasteValue > BoolValue)
                _notificationService.Warning("قيمة الهدر غير مسموحة ");

            if (ExportedValue < 0 || ExportedValue > BoolValue)
                _notificationService.Warning("قيمة الاخراج غير مسموحة ");
        }
        public void ConvertUnits()
        {
            switch (SelectedCreatedValueUnit)
            {
                case "غ":
                    CreatedValue = CreatedValue / 1000;
                    SelectedCreatedValueUnit = "كغ";                   
                    break;
                case "مل":
                    CreatedValue = CreatedValue / 1000;
                    SelectedCreatedValueUnit = "ل";
                    break;
            }
           
            switch (SelectedWasteValueUnit)
            {
                case "غ" :
                    WasteValue = WasteValue / 1000;
                    SelectedWasteValueUnit = "كغ";
                    break;
                case "مل":
                    WasteValue = WasteValue / 1000;
                    SelectedWasteValueUnit = "ل";
                    break;
            }

        }

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
                TotalResult = BoolResult,
                IsIncreaseDogma = false
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
        // return  await _cartoonItemRepository.GetSumValueProducts(selecteditem);
        //}

    }
}


