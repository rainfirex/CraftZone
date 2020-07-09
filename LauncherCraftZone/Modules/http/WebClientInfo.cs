using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace LauncherCraftZone.Modules.http
{
    class WebClientInfo : WebClient
    {
        const string ACTION = "info";

        const string METHOD = "POST";

        public WebClientInfo(ResponseServer responseServer)
        {
            setAction(ACTION, METHOD);
            setResponse(responseServer);
        }
    }
}
