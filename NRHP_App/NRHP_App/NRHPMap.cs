using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;
using NRHP_App.Models;

namespace NRHP_App
{
    public class NRHPMap : Map
    {
        public IEnumerable<DataPoint> DataPoints { get; set; }
        //public List<Polygon> Polygons { get; set; }
        public NRHPMap()
        {
            //DataPoints = new List<DataPoint>();
            //Polygons = new List<Polygon>();
        }
    }
}
