using System;
using EyeClinic.Model.Custom;

namespace EyeClinic.Model
{
    public class TestDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string EnName { get; set; }
        public string ArName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string ImagePath { get; set; }
        public string ImagePath2 { get; set; }
        public string ImagePath3 { get; set; }
        public byte Imagex { get; set; }
        public byte[] Imagex2 { get; set; }
        public byte[] Imagex3 { get; set; }
        public byte[] Imagex4 { get; set; }
        public double TotalWight { get; set; }
        public double TotalWaste { get; set; }
        public double TotalValue { get; set; }
        public double TotalResult { get; set; }
        public double Quintity { get; set; }
        public string Unit { get; set; }
        public double ReayProductsValue { get; set; }
        public string FirstTermBalance { get; set; }
        public double FirstTermBalanceWareHouseValue { get; set; }
        public double TotalWasteWareHouseValue { get; set; }
        public double TotalWareHouseValue { get; set; }
        public double TotalEntryWareHouseValue { get; set; }
        public double WasteValue { set; get; }
        public double UnitValue { get; set; }
        public bool IsProduct { get; set; }
        public double AddDogmaValue { get; set; }
        public int IndexN { get; set; }
        public override string ToString() {
            var code = CurrentLanguageCodeHandler.ArLanguageCode;

            return CurrentLanguageCodeHandler.CurrentLanguageCode == code
                ? $"{Code} - {ArName}"
                : $"{Code} - {EnName}";
        }
    }
}
