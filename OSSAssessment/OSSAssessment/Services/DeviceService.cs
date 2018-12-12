using OSSAssessment.DataLayer;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace OSSAssessment.Services
{
    internal class DeviceService
    {
        public void AddDevice(string address, string name)
        {
            var existing = GlobalDataModel.Instance.Devices.Select(x => x).Where(x => x.Address == address && x.Name == name).FirstOrDefault();

            if (existing == null)
            {
                var newDevice = new DeviceModel()
                {
                    Id = GetNewDeviceId(),
                    Address = address,
                    Name = name,
                    Connected = true
                };
                // test connection
                //change guid
                GlobalDataModel.Instance.Devices.Add(newDevice);
            }
            else
            {
                existing.Connected = true;
            }
        }

        public DeviceModel GetDeviceById(int deviceId)
        {
            return GlobalDataModel.Instance.Devices.Select(x => x).Where(x => x.Id == deviceId).FirstOrDefault();
        }

        public DeviceModel GetDeviceByAddress(string deviceAddress)
        {
            return GlobalDataModel.Instance.Devices.Select(x => x).Where(x => x.Address == deviceAddress).FirstOrDefault();
        }

        private int GetNewDeviceId()
        {
            int maxId;
            if (GlobalDataModel.Instance.Devices.Count > 0)
            {
                maxId = GlobalDataModel.Instance.Devices.Select(x => x.Id).Max();
                maxId++;
            }
            else
            {
                maxId = 1;
            }
            return maxId;
        }

        public IPAddress GetLocalDeviceIPAddress()
        {
            IPAddress[] hostAddresses = Dns.GetHostAddresses("");

            foreach (IPAddress hostAddress in hostAddresses)
            {
                if (hostAddress.AddressFamily == AddressFamily.InterNetwork &&
                    !IPAddress.IsLoopback(hostAddress))
                    return hostAddress;
            }
            return null; // or IPAddress.None if you prefer
        }
    }
}