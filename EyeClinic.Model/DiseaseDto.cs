using System;
using System.Security;
using EyeClinic.Model.Custom;

namespace EyeClinic.Model
{
    public class DiseaseDto
    {
        public int Id { get; set; }
        public string EnName { get; set; }
        public string ArName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int ThePNumber { set; get; }
        public string Enrty { set; get; }
        public string Extry { set; get; }
        public string FirstDayValue { set; get; }
        public double FirstValue { set; get; }
        public string Note { set; get; }
        public string? IsDecreaseOfMinimumValue  =>  GetResult();
        public double CurrentValue { get; set; }        


        public override string ToString() {
            var code = CurrentLanguageCodeHandler.ArLanguageCode;

            return CurrentLanguageCodeHandler.CurrentLanguageCode == code
                ? $"{ArName}"
                : $"{EnName}";
        }
        public string GetResult() 
        {
            try
            {
                int.TryParse(Extry ,out int v);
                if (v == 0 || CurrentValue == 0)
                    return "جيد";
                if (CurrentValue < v)
                    return "بحاجةطلب";

                return "جيد";
            }
            catch (Exception ex) 
            {
                 
            }

            return "جيد";


        }
       
    }
}
