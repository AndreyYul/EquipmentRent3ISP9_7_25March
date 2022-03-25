using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentRent3ISP9_7.EF
{
    public partial class Client
    {
        public string FIO { get => $"{LastName} {FirstName} {MiddleName}"; }
        public string CMBProperty { get => $"{Passport.FullPassport}:  {LastName} {FirstName} {MiddleName}"; }
    }
}
