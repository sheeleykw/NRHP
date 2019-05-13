using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;
using SQLite;

namespace NRHP_App.Models
{
    public class DataPoint
    {
        [PrimaryKey]
        public string RefNum { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Category { get; set; }
    }
}
