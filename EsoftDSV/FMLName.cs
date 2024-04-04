using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsoftDSV
{
    internal class FMLName
    {
        public static string GetFullName(User name)
        {
            return name.MiddleName + " " + name.FirstName + " " + name.LastName;
        }
    }
}
