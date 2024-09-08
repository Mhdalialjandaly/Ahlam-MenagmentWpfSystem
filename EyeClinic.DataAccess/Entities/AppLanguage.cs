using System;
using System.Collections.Generic;

#nullable disable

namespace EyeClinic.DataAccess.Entities
{
    public partial class AppLanguage : IEntityModel
    {
        public int Id { get; set; }
        public string LanguageCode { get; set; }
        public string DeviceName { get; set; }
        public string PrinterName { get; set; }
        public bool WaitingNext { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
