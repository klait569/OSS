using OSSAssessment.DataLayer;
using System;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace OSSAssessment.Services
{
    public class WebApiProxy
    {
        public delegate void ShowReceivedMessage(string m);

        public delegate void ShowStatus(string txt);
        
        private HttpSelfHostServer _server;
                
        public bool InitServer(string myport)
        {
            GlobalDataModel.Instance.Port = myport;
            return StartWebApiServer();
        }

        public bool StartWebApiServer()
        {
            try
            {
                string url = "http://localhost:" + GlobalDataModel.Instance.Port + "/";
                HttpSelfHostConfiguration config = new HttpSelfHostConfiguration(url);

                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );

                _server = new HttpSelfHostServer(config);
                _server.OpenAsync().Wait();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public void StopWebApiServer()
        {
            _server.CloseAsync().Wait();
        }
    }
}