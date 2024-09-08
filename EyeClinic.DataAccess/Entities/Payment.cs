using System;
using System.Collections.Generic;

#nullable disable

namespace EyeClinic.DataAccess.Entities
{
    public partial class Payment : IEntityModel
    {
        public int Id { get; set; }
        public int PaymentTypeId { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime? ReminderDate { get; set; }
        public int PaymentValue { get; set; }
        public string Notes { get; set; }
        public bool? Paid { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public virtual PaymentType PaymentType { get; set; }
    }
}
