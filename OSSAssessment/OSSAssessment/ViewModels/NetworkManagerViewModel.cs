using Newtonsoft.Json;
using OSSAssessment.DataLayer;
using OSSAssessment.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Threading;

namespace OSSAssessment.ViewModels
{
    internal class NetworkManagerViewModel : BindableBase
    {
        public ObservableCollection<DeviceModel> Devices
        {
            get { return GlobalDataModel.Instance.Devices; }
            set
            {
                SetProperty(ref GlobalDataModel.Instance.Devices, value);
            }
        }

        private WebApiProxy proxy;

        public WebApiProxy Proxy
        {
            get
            {
                if (proxy == null)
                {
                    proxy = new WebApiProxy();
                }
                return proxy;
            }
        }

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

        private RestService restService;

        public RestService RestService
        {
            get
            {
                if (restService == null)
                {
                    restService = new RestService();
                }
                return restService;
            }
        }

        private string port = "800";

        public string Port
        {
            get { return port; }
            set
            {
                SetProperty(ref port, value);
            }
        }

        private string partnerAddress = "http://localhost:800";

        public string PartnerAddress
        {
            get { return partnerAddress; }
            set
            {
                SetProperty(ref partnerAddress, value);
            }
        }

        private bool serverRunning = false;

        public bool ServerRunning
        {
            get { return serverRunning; }
            set
            {
                if (value)
                {
                    if(!Proxy.InitServer(port))
                    {
                        MessageBox.Show("Can't initialize data sharing. Try another port.", "Sharing dissabled", MessageBoxButton.OK, MessageBoxImage.Warning);
                        value = false;
                    }
                }
                else
                {
                    Proxy.StopWebApiServer();
                    Devices.Clear();
                }
                SetProperty(ref serverRunning, value);
            }
        }

        public IPAddress LocalIpAddress
        {
            get
            {
                return DeviceService.GetLocalDeviceIPAddress();
            }
        }

        public ActionCommand SendDataCommand
        {
            get
            {
                return new ActionCommand(SendCommandAction);
            }
        }

        public ActionCommand AddDeviceCommand
        {
            get
            {
                return new ActionCommand(AddDeviceActionAsync);
            }
        }

        public ActionCommand RemoveCommand
        {
            get
            {
                return new ActionCommand(RemoveAction);
            }
        }

        public ActionCommand GetDataCommand
        {
            get
            {
                return new ActionCommand(GetDataAction);
            }
        }

        private async void GetDataAction(object deviceId)
        {
            var device = GlobalDataModel.Instance.Devices.Select(x => x).Where(x => x.Id == (int)deviceId).FirstOrDefault();
            var newData = await RestService.GetDataAsync(device.Address);

            if (newData != null)
            {
                DataModel model = JsonConvert.DeserializeObject<DataModel>(newData);
                SharingService ss = new SharingService();
                ss.ReceiveStructureData(model, device.Name);
            }
            else
            {
                await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => MessageBox.Show("Getting data from device " + device.Name + " failed.", "Connection failed", MessageBoxButton.OK, MessageBoxImage.Stop)));
            }
        }

        private void RemoveAction(object deviceId)
        {
            var device = GlobalDataModel.Instance.Devices.Select(x => x).Where(x => x.Id == (int)deviceId).FirstOrDefault();
            // test connection
            GlobalDataModel.Instance.Devices.Remove(device);
        }

        private async void AddDeviceActionAsync(object data)
        {
            var name = await RestService.PingAsync(PartnerAddress);

            if (!string.IsNullOrEmpty(name))
            {
                DeviceService.AddDevice(PartnerAddress, name);
                PartnerAddress = "http://";
            }
            else
            {
                if (!(PartnerAddress.Contains(Port) && PartnerAddress.Contains("localhost")))
                {
                    await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => MessageBox.Show("Connection to address " + PartnerAddress + " failed.", "Connection failed", MessageBoxButton.OK, MessageBoxImage.Stop)));
                }
            }
        }

        private async void SendCommandAction(object deviceId)
        {
            var device = Devices.Select(x => x).Where(x => x.Id == (int)deviceId).FirstOrDefault();
            bool result = await RestService.SendDataAsync(device.Address);
            if(!result)
            {
                await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => MessageBox.Show("Sending data to device " + device.Name + " failed.", "Connection failed", MessageBoxButton.OK, MessageBoxImage.Stop)));
            }
        }
    }
}