using OSSAssessment.DataLayer;
using OSSAssessment.Services;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace OSSAssessment.WebAPI
{
    public class StructureController : ApiController
    {
        public HttpResponseMessage Post(DataModel model)
        {
            //Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
            //    new Action(() => GlobalDataModel.Instance.LoadModel(model)));

            string deviceName = string.Empty;
            if (Request.Headers.Contains(Headers.REST_HEADER_NAME))
            {
                deviceName = Request.Headers.GetValues(Headers.REST_HEADER_NAME).First();
            }

            SharingService ss = new SharingService();
            ss.ReceiveStructureData(model, deviceName);

            var response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
            AddResponseHeaders(response);
            return response;
        }

        public HttpResponseMessage Get()
        {
            var response = Request.CreateResponse(System.Net.HttpStatusCode.OK, GlobalDataModel.Instance.Model);
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
}