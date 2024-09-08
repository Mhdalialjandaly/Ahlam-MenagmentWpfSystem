using System.Collections.Generic;

namespace EyeClinic.Core.Common
{
    public class GenderType
    {
        public bool Type { get; set; }
        public string Name { get; set; }

        public static List<GenderType> Create() {
            return new()
            {
                new() { Type = false, Name = "انثى" },
                new() { Type = true, Name = "ذكر" },
            };
        }

        public override string ToString() {
            return Name;
        }
    }
}
