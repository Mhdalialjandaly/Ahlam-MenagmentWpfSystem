using System;
using System.Collections.Generic;

#nullable disable

namespace EyeClinic.DataAccess.Entities
{
    public partial class Payout : IEntityModel
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int Amount { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
