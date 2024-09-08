using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeClinic.DataAccess.Entities 
{
    public class CustomerNoteStory : IEntityModel
    {
        [Key]
        public int PublicCustomerId { get; set; }
        public string NoteStory { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public virtual PublicCustomer PublicCustomer { get; set; }
    }
}
