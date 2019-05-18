using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;
using Position = Xamarin.Forms.Maps.Position;

namespace NRHP_App
{
    public class NRHPMap : Map
    {
        public List<DataPoint> DataPoints { get; set; }

        public NRHPMap()
        {
            DataPoints = new List<DataPoint>();
        }

        public void AddDataPoint(DataPoint dataPoint)
        {
            DataPoints.Add(dataPoint);
            Pins.Add(new Pin
            {
                Label = dataPoint.Name,
                Address = dataPoint.Category,
                Position = new Position(dataPoint.Latitude, dataPoint.Longitude),
                Type = PinType.Place,
            });
        }

        public void RemoveExcessDataPoints(double TopLatitude, double BottomLatitude, double RightLongitude, double LeftLongitude)
        {
            foreach (DataPoint dataPoint in DataPoints)
            {
                if (dataPoint.Latitude > TopLatitude || dataPoint.Latitude < BottomLatitude || dataPoint.Longitude > RightLongitude || dataPoint.Longitude < LeftLongitude)
                {

                }
            }
        }
    }
}
