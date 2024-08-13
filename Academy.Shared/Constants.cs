using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Shared
{
    public class Constants
    {
        public class ValidationRegex
        {
            public const string Name = "^[a-zA-Z ]*$/";
            public const string Pincode = "/\\d{6,8}/";
        }
    }
}
