using System;

namespace FibreLib
{
    public class Home
    {
        private string id;
        private string address;
        private string owner;
        private string fibreProvider;
        private bool isCovered;
        private int speed;
        private string isp;

        public string ID { get => id; set => id = value; }
        public string Address { get => address; set => address = value; }
        public string Owner { get => owner; set => owner = value; }
        public string FibreProvider { get => fibreProvider; set => fibreProvider = value; }
        public bool IsCovered { get => isCovered; set => isCovered = value; }
        public int Speed { get => speed; set => speed = value; }
        public string Isp { get => isp; set => isp = value; }

        public Home()
        {
            ID = "";
            Address = "";
            Owner = "";
            FibreProvider = "";
            IsCovered = false;
            Speed = 0;
            Isp = "";
        }

        public Home(string id, string address, string owner, string fibreProvider, bool isCovered, int speed, string isp)
        {
            ID = id;
            Address = address;
            Owner = owner;
            FibreProvider = fibreProvider;
            IsCovered = isCovered;
            Speed = speed;
            Isp = isp;
        }

        public string HomeToString()
        {
            return "ID: " + ID +
                "\nAddress: " + Address +
                "\nOwner Name: " + Owner +
                "\nFibre Provider: " + FibreProvider +
                "\nIs Covered: " + IsCovered +
                "\nLine Speed: " + Speed +
                "\nISP: " + Isp;
        }

        public string csvString()
        {
            return $"{ID},{Address},{Owner},{FibreProvider},{IsCovered},{Speed},{Isp}";
        }
    }
}
