using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentRent3ISP9_7.EF
{
    partial class Passport
    {
        public string FullPassport { get => $"{PassportSeries}{PassportNumber}"; }
    }
}
