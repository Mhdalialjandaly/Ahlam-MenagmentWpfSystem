using EyeClinic.Core;
using System.Collections.Generic;

namespace EyeClinic.WPF.DataModel
{
    public class Navigation
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public int Pending { get; set; }

        public bool IsPending => Pending > 0;

        public static List<Navigation> Create() {
            if (Global.DeviceName == "Admin")
            {
                return new()
            {
                    new() { Name = "واجهة العملاء", Image = "/Images/Navigation/WaitingRoom.png", Pending = 1 },
                    new() { Name = "واجهة العمل الجماعي", Image = "/Images/Navigation/InsideClinic.png", Pending = 0 },
                    new() { Name = "الكرتون", Image = "/Images/Navigation/1.png", Pending = 3 },
                    new() { Name = "واجهة التصنيع", Image = "/Images/Navigation/Payments.png", Pending = 3 },
                    new() { Name = "الماركات", Image = "/Images/Navigation/Markiting.png", Pending = 3 },
                    new() { Name = "تقارير", Image = "/Images/Navigation/Reports.png", Pending = 3 },
                    new() { Name = "واجهة المراسلات", Image = "/Images/Navigation/AddressBook.png", Pending = 3 }
            };
            }
            else if (Global.DeviceName == "VS")
            {
                return new()
                {
                    new() { Name = "واجهة العملاء", Image = "/Images/Navigation/WaitingRoom.png", Pending = 1 },
                    new() { Name = "واجهة العمل الجماعي", Image = "/Images/Navigation/InsideClinic.png", Pending = 0 },
                    new() { Name = "الكرتون", Image = "/Images/Navigation/1.png", Pending = 3 },
                    new() { Name = "واجهة التصنيع", Image = "/Images/Navigation/Payments.png", Pending = 3 },
                    new() { Name = "الماركات", Image = "/Images/Navigation/Markiting.png", Pending = 3 },
                    new() { Name = "تقارير", Image = "/Images/Navigation/Reports.png", Pending = 3 },
                    new() { Name = "واجهة المراسلات", Image = "/Images/Navigation/AddressBook.png", Pending = 3 },
                    new() { Name = "العمليات", Image = "/Images/Navigation/Operation.png", Pending = 1 }

                };
            }
            else if (Global.DeviceName == "Operation")
            {
                return new()          
                {
                    new() { Name = "الكرتون", Image = "/Images/Navigation/1.png", Pending = 3 },
                    new() { Name = "واجهة التصنيع", Image = "/Images/Navigation/Payments.png", Pending = 3 },
                    new() { Name = "الماركات", Image = "/Images/Navigation/Markiting.png", Pending = 3 },
                    new() { Name = "واجهة المراسلات", Image = "/Images/Navigation/AddressBook.png", Pending = 3 },
                    new() { Name = "العمليات", Image = "/Images/Navigation/Operation.png", Pending = 1 }
           
                };
            }
            else if (Global.DeviceName != "Public")           
            {
                return new()          
                {
                    new() { Name = "واجهة العملاء", Image = "/Images/Navigation/WaitingRoom.png", Pending = 1 },             
                    new() { Name = "واجهة العمل الجماعي", Image = "/Images/Navigation/InsideClinic.png", Pending = 0 },             
                    new() { Name = "الكرتون", Image = "/Images/Navigation/1.png", Pending = 3 },            
                    new() { Name = "واجهة التصنيع", Image = "/Images/Navigation/Payments.png", Pending = 3 },              
                    new() { Name = "الماركات", Image = "/Images/Navigation/Markiting.png", Pending = 3 },             
                    new() { Name = "واجهة المراسلات", Image = "/Images/Navigation/AddressBook.png", Pending = 3 }
                };
            }          

            else
            {
                return null;
            }
            
        }
    }
}
