using System;

namespace EyeClinic.Model
{
    public class ReminderDto
    {
        public int Id { get; set; }
        public string ReminderText { get; set; }
        public DateTime ReminderDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
