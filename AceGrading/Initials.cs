using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AceGrading
{
    public class Initials
    {
        public Initials() { }

        public string MakeInitials(string Name)
        {
            Name = Name.Trim();
            if (Name.Length == 0)
                return "";

            string[] names = Name.Split(' ');

            if (names.Length > 1)
                return names[0].Substring(0, 1).ToUpper() + names[names.Length - 1].Substring(0, 1).ToUpper();
            else if (Name.Length == 1)
                return Name.ToUpper();
            else if (names.Length == 1)
                return names[0].Substring(0, 2).ToUpper();
            else
                return "";
        }
    }
}
