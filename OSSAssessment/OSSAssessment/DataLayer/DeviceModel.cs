using System;

namespace OSSAssessment.DataLayer
{
    internal class DeviceModel : BindableBase
    {
        private int id;

        public int Id
        {
            get { return id; }
            set
            {
                SetProperty(ref id, value);
            }
        }

        private bool connected;

        public bool Connected
        {
            get { return connected; }
            set
            {
                SetProperty(ref connected, value);
            }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                SetProperty(ref name, value);
            }
        }

        private string address;

        public string Address
        {
            get { return address; }
            set
            {
                SetProperty(ref address, value);
            }
        }

        private Guid secret;

        public Guid SecretFopPartner
        {
            get { return secret; }
            set
            {
                SetProperty(ref secret, value);
            }
        }

        private Guid secret2;

        public Guid PartnerSecret
        {
            get { return secret2; }
            set
            {
                SetProperty(ref secret2, value);
            }
        }
    }
}