using Newtonsoft.Json;
using OSSAssessment.DataLayer;
using OSSAssessment.WebAPI;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace OSSAssessment.Services
{
    internal class RestService
    {
        private DeviceService deviceService;

        public DeviceService DeviceService
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

        public async Task<bool> SendDataAsync(string address)
        {
            try
            {
                using (HttpClient client = new HttpClient() { BaseAddress = new Uri(address) })
                {
                    MediaTypeFormatter jsonFormatter = new JsonMediaTypeFormatter();
                    HttpContent content = new ObjectContent<DataModel>(GlobalDataModel.Instance.Model, jsonFormatter);
                    var myContent = JsonConvert.SerializeObject(GlobalDataModel.Instance.Model);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var adr = client.BaseAddress.ToString();
                    string adress = adr + "api/structure";
                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(adress),
                        Method = HttpMethod.Post,
                        Content = byteContent
                    };

                    request.Headers.Add(Headers.REST_HEADER_NAME, System.Environment.MachineName);
                    var response = await client.SendAsync(request);
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        DeviceModel device = DeviceService.GetDeviceByAddress(address);
                        if (device != null)
                        {
                            device.Connected = false;
                        }
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                DeviceModel device = DeviceService.GetDeviceByAddress(address);
                if (device != null)
                {
                    device.Connected = false;
                }
                return false;
            }
        }

        public async Task<string> GetDataAsync(string address)
        {
            try
            {
                using (HttpClient client = new HttpClient() { BaseAddress = new Uri(address) })
                {
                    var adr = client.BaseAddress.ToString();
                    string adress = adr + "api/structure";
                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(adress),
                        Method = HttpMethod.Get,
                    };

                    request.Headers.Add(Headers.REST_HEADER_NAME, System.Environment.MachineName);
                    var response = client.SendAsync(request).Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        return content;
                    }
                    else
                    {
                        DeviceModel device = DeviceService.GetDeviceByAddress(address);
                        if (device != null)
                        {
                            device.Connected = false;
                        }
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                DeviceModel device = DeviceService.GetDeviceByAddress(address);
                if (device != null)
                {
                    device.Connected = false;
                }
                return null;
            }
        }

        public async Task<string> PingAsync(string address)
        {
            try
            {
                using (HttpClient client = new HttpClient() { BaseAddress = new Uri(address) })
                {
                    var adr = client.BaseAddress.ToString();
                    string adress = adr + "api/connection?address=" + DeviceService.GetLocalDeviceIPAddress() + "&port=" + GlobalDataModel.Instance.Port;

                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(adress),
                        Method = HttpMethod.Get,
                    };

                    request.Headers.Add(Headers.REST_HEADER_NAME, System.Environment.MachineName);
                    var response = await client.SendAsync(request);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var deviceName = response.Headers.GetValues(Headers.REST_HEADER_NAME).First();
                        return deviceName;
                    }
                    else
                    {
                        DeviceModel device = DeviceService.GetDeviceByAddress(address);
                        if (device != null)
                        {
                            device.Connected = false;
                        }
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                DeviceModel device = DeviceService.GetDeviceByAddress(address);
                if (device != null)
                {
                    device.Connected = false;
                }
                return null;
            }
        }
    }
}