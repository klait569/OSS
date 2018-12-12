using OSSAssessment.DataLayer;
using OSSAssessment.Services;
using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Windows;
using System.Windows.Threading;

namespace OSSAssessment.WebAPI
{
    public class ConnectionController : ApiController
    {
        private DeviceService deviceService;

        private DeviceService DeviceService
        {
            get
            {
                if (deviceService == null)
                {
                    deviceService = new DeviceService();
                }
                return deviceService;
            }
        }

        public HttpResponseMessage Get(string address, string port)
        {
            string deviceName = string.Empty;
            if (Request.Headers.Contains(Headers.REST_HEADER_NAME))
            {
                deviceName = Request.Headers.GetValues(Headers.REST_HEADER_NAME).First();
            }

            if (address == DeviceService.GetLocalDeviceIPAddress().ToString())
            {
                address = "http://localhost:";

                if (port == GlobalDataModel.Instance.Port)
                {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => MessageBox.Show("Can't add yourself!", "Not allowed", MessageBoxButton.OK, MessageBoxImage.Stop)));
                    return Request.CreateResponse(System.Net.HttpStatusCode.Forbidden);
                }
            }

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                new Action(() => DeviceService.AddDevice(address + port, deviceName)));

            var response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
            AddResponseHeaders(response);
            return response;
        }

        private static void AddResponseHeaders(HttpResponseMessage response)
        {
            response.Headers.Add(Headers.REST_HEADER_NAME, System.Environment.MachineName);
        }

        public delegate void EventHandler(object sender, MessageEventArgs args);

        public static event EventHandler ThrowMessageArrivedEvent = delegate { };

        public void MessageArrived(string m)
        {
            ThrowMessageArrivedEvent(this, new MessageEventArgs(m));
        }
    }

    public class MessageEventArgs : EventArgs
    {
        public MessageEventArgs(string m)
        {
            this.Message = m;
        }

        public string Message;
    }
}