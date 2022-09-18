using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ep.Mobile.Reference
{
    public static class Constant
    {
        public const string ApiBaseUrl = "https://andysignalrapi.azurewebsites.net";
        public const string CreateCustomerEndpoint = "api/customer/create";
        public const string CreateShopEndpoint = "api/shop/create";
        public const string CustomerRegUrl = "https://epdeploytest.azurewebsites.net/";
        public const string GoogleApiBaseUrl = "https://maps.googleapis.com/maps/api/place/autocomplete/json?";
        public const string StorageEmailKey = "Email";
        public const string StoragePasswordKey = "Password";
        public const string StorageSaltKey = "Salt";
        public const string SymKey = "symKey";
        public const string UpdateShopEndpoint = "api/shop/update";
        public const string InvalidLoginMessage = "Invalid email or password";
        public const string CurrentPasswordInvalidMessage = "Current Password is invalid";
    }
}
