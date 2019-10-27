using Xamarin.Forms.Maps;

namespace NRHP_App
{
    public class NRHPMap : Map
    {
        public NRHPMap(MapSpan mapSpan) : base(mapSpan)
        {
        }

        public void MoveToRegion(MapPoint mapPoint)
        {
            if (!mapPoint.Category.Equals("City"))
            {
                MoveToRegion(new MapSpan(new Position(mapPoint.Latitude, mapPoint.Longitude), VisibleRegion.LatitudeDegrees, VisibleRegion.LongitudeDegrees));
            }
            else
            {
                MoveToRegion(new MapSpan(new Position(mapPoint.Latitude, mapPoint.Longitude), 1.0, 1.0));
            }
        }
    }
}
