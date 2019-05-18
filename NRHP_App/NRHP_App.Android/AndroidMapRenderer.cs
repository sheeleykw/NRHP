using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Android.Content;
using Xamarin.Forms.Maps.Android;
using NRHP_App;
using NRHP_App.Droid;
using Android.Gms.Maps.Model;
using Android.Gms.Maps;
using static Android.Gms.Maps.GoogleMap;
using System;

[assembly: ExportRenderer(typeof(NRHPMap), typeof(AndroidMapRenderer))]
namespace NRHP_App.Droid
{
    public class AndroidMapRenderer : MapRenderer
    {
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
                Control.GetMapAsync(this);
            }
        }

        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);
            //NativeMap.SetOnCameraMoveListener(new MovingMapListener());
        }

        //internal class MovingMapListener : Java.Lang.Object, IOnCameraMoveListener
        //{

        //    async public void OnCameraMove()
        //    {
        //    }
        //}

        //protected async override MarkerOptions CreateMarker(Pin pin)
        //{
        //    var marker = new MarkerOptions();
        //    marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
        //    marker.SetTitle();
        //    marker.SetSnippet(pin.);
        //    marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.mr_button_connected_dark));
        //    return marker;
        //}
    }
}