using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeClinic.Model
{
    public class PublicCustomerDto
    {

        public int Id { get; set; }
        public int Number { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Type { get; set; }

        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string Nationality { get; set; }
        public int LocationId { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool Referral { get; set; }
        public int? TempId { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string FullName => $"{FirstName} - {LastName} - Company : {CompanyName} Of : {Type}";
        public string LastModifiedDateDisplayName
        {
            get
            {
                var lastDate = LastModifiedDate ?? CreatedDate;
                if ((DateTime.Now.Date - lastDate.Date).TotalDays > 1)
                    return $"Modified : {lastDate:dd/MM/yyyy}";

                return null;
            }
        }

    }
}
