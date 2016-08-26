using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SugarCRMRestClient
{
    public class SugarOAuthRequest
    {

        public SugarOAuthRequest(IDictionary<string,string> apikeys)
        {
            client_id = apikeys.FirstOrDefault(x => x.Key == "SugarClientId").Value;
            client_secret = apikeys.FirstOrDefault(x => x.Key == "SugarClientSecret").Value;
            grant_type = "password";
            username = apikeys.FirstOrDefault(x => x.Key == "SugarUsername").Value;
            password = apikeys.FirstOrDefault(x => x.Key == "SugarPassword").Value;
            platform = "CRM_Xero_Connector";
        }

        public string grant_type { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string platform { get; set; }
        public string refresh_token { get; set; }

        public List<KeyValuePair<string, string>> GetAuthParams()
        {
            var returnList = new List<KeyValuePair<string, string>>();
            var properties = this.GetType().GetProperties();
            foreach (PropertyInfo propInfo in properties)
            {
                if (propInfo.Name == "refresh_token")
                    continue;
                returnList.Add(new KeyValuePair<string, string>(propInfo.Name, propInfo.GetValue(this).ToString()));
            }

            return returnList;
        }

        public List<KeyValuePair<string, string>> GetRefreshAuthParams()
        {
            var returnList = new List<KeyValuePair<string, string>>();
            var properties = this.GetType().GetProperties();
            foreach (PropertyInfo propInfo in properties)
            {
                if (propInfo.Name == "username" || propInfo.Name == "password")
                    continue;
                returnList.Add(new KeyValuePair<string, string>(propInfo.Name, propInfo.GetValue(this).ToString()));
            }

            return returnList;
        }
    }
}