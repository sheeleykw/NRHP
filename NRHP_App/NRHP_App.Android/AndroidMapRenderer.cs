using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Android.Content;
using Xamarin.Forms.Maps.Android;
using NRHP_App;
using NRHP_App.Droid;
using Android.Gms.Maps.Model;

[assembly: ExportRenderer(typeof(NRHPMap), typeof(AndroidMapRenderer))]
namespace NRHP_App.Droid
{
    public class AndroidMapRenderer : MapRenderer
    {
        IList<Pin> pins;
        public AndroidMapRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
            }

            if (e.NewElement != null)
            {
                var formsMap = (NRHPMap)e.NewElement;
                pins = formsMap.Pins;
                Control.GetMapAsync(this);
            }
        }

        protected override void OnMapReady(Android.Gms.Maps.GoogleMap map)
        {
            base.OnMapReady(map);
        }

        //protected override MarkerOptions CreateMarker(Pin pin)
        //{
        //    var marker = new MarkerOptions();
        //    marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
        //    marker.SetTitle(pin.Label);
        //    //marker.SetSnippet(dataPoint.Category);
        //    marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.mr_button_connected_dark));
        //    return marker;
        //}
    }
}