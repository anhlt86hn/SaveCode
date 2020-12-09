using System;
using System.Collections.Generic;
using System.Text;

namespace RicoCore.Utilities.Constants
{
    public class CommonConstants
    {
        public static string DefaultFooterId = "default";
        //public const string DefaultFooterId = "default";
        //public const string DefaultContactId = "default";
        public const string CartSession = "CartSession";
        public const string ProductTag = "Product";
        public const string PostTag = "Post";        
        public const string DefaultGuid = "00000000-0000-0000-0000-000000000000";
        public class AppRole
        {
            public const string Admin = "Admin";
            public const string Rico = "Rico";
        }

        public class UserClaims
        {
            public const string Roles = "Roles";
        }
    }
}
