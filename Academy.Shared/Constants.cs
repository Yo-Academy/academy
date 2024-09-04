using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Shared
{
    public class Constants
    {
        public static string DefaultPassword = "123456";
        public class ValidationRegex
        {
            public const string Name = "^[a-zA-Z ]*$";
            public const string Pincode = @"\d{6,8}";
        }

        public class S3Directory
        {
            public const string Logo = nameof(Logo);
            public const string QR = nameof(QR);
        }

        public class UserRole
        {
            public const string User = "User";
            public const string Admin = "Admin";
            public const string SAdmin = "SAdmin";
        }

        public static string DefaultDomain = "@yosports.in";
    }
}
