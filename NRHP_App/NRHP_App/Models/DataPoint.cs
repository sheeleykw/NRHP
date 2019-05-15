using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NRHP_App.Models
{
    public class DataPoint : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string _refnum;
        [JsonProperty("refnum")]
        public string Refnum
        {
            get => _refnum;
            set
            {
                if (_refnum == value)
                    return;
                _refnum = value;

                HandlePropertyChanged();
            }
        }

        string _name;
        [JsonProperty("name")]
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value)
                    return;
                _name = value;

                HandlePropertyChanged();
            }
        }

        string _address;
        [JsonProperty("address")]
        public string Address
        {
            get => _address;
            set
            {
                if (_address == value)
                    return;
                _address = value;

                HandlePropertyChanged();
            }
        }

        double _latitude;
        [JsonProperty("latitude")]
        public double Latitude
        {
            get => _latitude;
            set
            {
                if (_latitude == value)
                    return;
                _latitude = value;

                HandlePropertyChanged();
            }
        }

        double _longitude;
        [JsonProperty("longitude")]
        public double Longitude
        {
            get => _longitude;
            set
            {
                if (_longitude == value)
                    return;
                _longitude = value;

                HandlePropertyChanged();
            }
        }

        string _category;
        [JsonProperty("category")]
        public string Category
        {
            get => _category;
            set
            {
                if (_category == value)
                    return;
                _category = value;

                HandlePropertyChanged();
            }
        }

        void HandlePropertyChanged([CallerMemberName]string propertyName = "")
        {
            var eventArgs = new PropertyChangedEventArgs(propertyName);

            PropertyChanged?.Invoke(this, eventArgs);
        }
    }
}
