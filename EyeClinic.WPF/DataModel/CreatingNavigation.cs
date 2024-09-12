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
                    new() { Name = "التحضير1", Image = "/Images/Navigation/3.png", Pending = 1 },
                    new() { Name = "التحضير2", Image = "/Images/Navigation/3.png", Pending = 0 },
                    new() { Name = "الحلاوة", Image = "/Images/Navigation/4.png", Pending = 3 },
                    new() { Name = "زيت", Image = "/Images/Navigation/5.png", Pending = 3 },
                    new() { Name = "طبخ", Image = "/Images/Navigation/6.png", Pending = 3 },
                    new() { Name = "لبنة", Image = "/Images/Navigation/7.png", Pending = 3 },
                    new() { Name = "مربى", Image = "/Images/Navigation/8.png", Pending = 3 },
                    new() { Name = "طحينة", Image = "/Images/Navigation/9.png", Pending = 3 },
                    new() { Name = "معلبات", Image = "/Images/Navigation/10.png", Pending = 3},
                    new() { Name = "الزيتون", Image = "/Images/Navigation/11.png", Pending = 3},
                    new() { Name = "خروج", Image = "/Images/Navigation/Logo1.png", Pending = 3}

                };

        }
    }
}
