using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Threading;

namespace LauncherCraftZone.Modules.http
{  
    delegate void ResponseServer(string response);

    abstract class WebClient
    {                 
        const string SERVER_URL = "localhost";

        const string SERVER_PORT = "4598";

        const string CONTROLLER = "main";

        private string action;
                                                
        protected WebRequest request;

        private ResponseServer responseServer;

        private string MessageError { get; set; }

        protected void setResponse(ResponseServer responseServer)
        {
            this.responseServer = responseServer;
        }

        protected void setAction(string action, string method)
        {
            if (!String.IsNullOrEmpty(action) && !String.IsNullOrEmpty(method))
            {
                this.action = action;

                request = WebRequest.Create(String.Format("http://{0}:{1}//{2}/{3}", SERVER_URL, SERVER_PORT, CONTROLLER, action));
                request.Method = method;

                if(method.Equals("POST"))
                    request.ContentType = "application/x-www-form-urlencoded";
            }
        }

        protected void sendAsync(string data)
        {
            if (String.IsNullOrEmpty(action))
                return;

            Thread th = new Thread(() => this.sendData(data));
            th.IsBackground = true;
            th.Start();
        }

        private void sendData(string data)
        { 
            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(data);

                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();

                WebResponse response = request.GetResponse();
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.  
                string responseFromServer = reader.ReadToEnd();

                if (responseServer != null)
                    responseServer(responseFromServer);
            }
            catch (Exception ex) { MessageError = ex.Message; }
        }

        public void send(string data)
        {
            this.sendAsync(data);
        }
    }
}
