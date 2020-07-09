using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LauncherCraftZone.Models;

namespace LauncherCraftZone.Modules
{ 
    class RequestMethods
    {              
        private Dictionary<string, string> queryParams;

        private ClientInfoAdd clientInfoAdd;

        public RequestMethods(ClientInfoAdd clientInfoAdd)
        {
            this.clientInfoAdd = clientInfoAdd;
        }

        public void setQueryParams(Dictionary<string, string> queryParams)
        {
            this.queryParams = queryParams;
        }

        public object info()
        {   
            ClientInfo c = new ClientInfo {
                Login = queryParams["login"],
                IpAddress = queryParams["remote_host"],
                VersionClient = queryParams["version"],
                VersionLauncher = queryParams["version_launcher"],
                Memory = Convert.ToInt32(queryParams["memory"]),
                IsForge = Convert.ToBoolean(queryParams["is_forge"])
            };


            if (clientInfoAdd != null)
                clientInfoAdd(c);

            return "OK!";
        }

        public void auth()
        {
            //динамически создаём страницу
            //string responseString = @"<!DOCTYPE HTML>
            //<html><head></head><body>
            //<form method=""post"" action=""say"">
            //<p><b>Name: </b><br>
            //<input type=""text"" name=""myname"" size=""40""></p>
            //<p><input type=""submit"" value=""send""></p>
            //</form></body></html>";
        }
    }
}
