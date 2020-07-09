using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.IO;
using System.Reflection;
using LauncherCraftZone.Models;

namespace LauncherCraftZone.Modules
{
    delegate void ClientInfoAdd(ClientInfo client);

    class WebServer
    {
        protected List<ClientInfo> listClient = new List<ClientInfo>();

        const string SERVER_URL = "localhost";

        const string SERVER_PORT = "4598";

        HttpListener httpListner;

        private string action;

        private string controller;

        private Dictionary<string, string> queryParams;

        RequestMethods methods;

        public WebServer(ClientInfoAdd clientInfoAdd)
        {
            httpListner = new HttpListener();
            httpListner.Prefixes.Add(String.Format("http://{0}:{1}/main/", SERVER_URL, SERVER_PORT));

            methods = new RequestMethods(clientInfoAdd);
        }

        public void run()
        {
            httpListner.Start();

            Thread th = new Thread(listening);
            th.IsBackground = true;
            th.Start();
        }

        private void listening()
        {
            while (true)
            {
                HttpListenerContext client = httpListner.GetContext();
                
                HttpListenerRequest request = client.Request;

                parseRequestData(client);
               
                methods.setQueryParams(queryParams);
                MethodInfo info = methods.GetType().GetMethod(action);
                object result = info.Invoke(methods, null);
                sendResponseData(client, result.ToString());
            }
        }

        private void parseRequestData(HttpListenerContext client)
        {
            queryParams = new Dictionary<string, string>();

            HttpListenerRequest request = client.Request;

            queryParams.Add("remote_host", request.UserHostName);

            this.parseRequestURL(request.Url);

            // Если тело пустое
            if (!request.HasEntityBody)
                return;

            using (Stream body = request.InputStream)
            {
                using (StreamReader reader = new StreamReader(body, Encoding.UTF8))
                {
                    string text = reader.ReadToEnd();
                    string[] query = text.Split('&');
                    for (int i = 0; i < query.Length; i++)
                    {
                        string[] data = query[i].Split('=');
                        queryParams.Add(data[0], data[1]);
                    }
                }
            }
        }

        private void parseRequestURL(Uri url)
        { 
            string[] urlArray = url.LocalPath.Split('/');

            urlArray = urlArray.Where(x => x != "").ToArray();

            if (urlArray.Length < 2)
                return;

            this.controller = urlArray[0];
            this.action = urlArray[1];

            if (String.IsNullOrEmpty(url.Query))
                return;

            string[] query = url.Query.Replace('?', ' ').Trim().Split('&');
            for (int i = 0; i < query.Length; i++)
            {
                string[] data = query[i].Split('=');
                queryParams.Add(data[0], data[1]);
            }
        }

        public void sendResponseData(HttpListenerContext client, string responseString)
        {
            HttpListenerResponse response = client.Response;
            response.ContentType = "text/html; charset=UTF-8";
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
        }
    }
}
