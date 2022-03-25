using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EquipmentRent3ISP9_7.EF;

namespace EquipmentRent3ISP9_7.HelperClass
{
    public class HelperCl
    {
        public static Entities Context { get; } = new Entities();
    }
}
