using System;

namespace EyeClinic.Model
{
    public class DeniedUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime DeniedDate { get; set; }
        public string DeviceName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedBy { get; set; }
    }

}
