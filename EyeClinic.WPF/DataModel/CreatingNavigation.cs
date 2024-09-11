using System.Collections.Generic;


namespace EyeClinic.WPF.DataModel
{
  public  class CreatingNavigation
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public int Pending { get; set; }

        public bool IsPending => Pending > 0;

        public static List<CreatingNavigation> Create()
        {
            return new()
                {
                    new() { Name = "قسم التحضير 1", Image = "/Images/Navigation/edit1.png", Pending = 1 },
                    new() { Name = "قسم التحضير 2", Image = "/Images/Navigation/WaitingRoom1.png", Pending = 0 },
                    new() { Name = "قسم الحلاوة", Image = "/Images/Navigation/WaitingRoom.png", Pending = 3 },
                    new() { Name = "قسم الخل والزيت", Image = "/Images/Navigation/WaitingRoom.png", Pending = 3 },
                    new() { Name = "قسم الطبخ", Image = "/Images/Navigation/WaitingRoom.png", Pending = 3 },
                    new() { Name = "قسم اللبنة", Image = "/Images/Navigation/WaitingRoom.png", Pending = 3 },
                    new() { Name = "قسم المربيات", Image = "/Images/Navigation/WaitingRoom.png", Pending = 3 },
                    new() { Name = "قسم الطحينة", Image = "/Images/Navigation/WaitingRoom.png", Pending = 3 },
                    new() { Name = "قسم المعلبات", Image = "/Images/Navigation/WaitingRoom.png", Pending = 3},
                    new() { Name = "قسم الزيتون", Image = "/Images/Navigation/WaitingRoom.png", Pending = 3},
                    new() { Name = "الرجوع", Image = "/Images/Navigation/WaitingRoom.png", Pending = 3}

                };

        }
    }
}
