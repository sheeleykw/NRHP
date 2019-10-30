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

            if (e.NewElement != null)
            {
                Control.GetMapAsync(this);
            }
        }

        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);

            NativeMap.SetMinZoomPreference(11);
            NativeMap.SetMaxZoomPreference(18);

            NativeMap.MarkerClick += SelectPoint;
            NativeMap.InfoWindowClose += DeselectPoint;

            App.mainPage.SearchCompleted += LoadInfoWindow;
        }

        //Changes the currentPoints refnum and moves the camera to the selected point
        //Possible changes might be made to the animation of the camera during selection
        private async void SelectPoint(object sender, MarkerClickEventArgs e)
        {
            Marker marker = e.Marker;
            if (!marker.IsInfoWindowShown)
            {
                var moveCamera = CameraUpdateFactory.NewLatLng(marker.Position);
                NativeMap.AnimateCamera(moveCamera);
                marker.ShowInfoWindow();

                Pin pin = GetPinFromMarker(marker);

                DataPoint currentPoint = null;
                if (pin != null)
                {
                    currentPoint = await App.itemDatabase.GetPointAsync(pin.StyleId);
                }

                App.mainPage.ShowDetail(currentPoint);
            }
        }

        //Called when a point on the map is deselected.
        private void DeselectPoint(object sender, InfoWindowCloseEventArgs e)
        {
            App.mainPage.HideDetail();
        }

        protected override MarkerOptions CreateMarker(Pin pin)
        {
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            marker.SetTitle(pin.Label);
            marker.SetSnippet(pin.Address);
            marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.s));
            return marker;
        }

        private Pin GetPinFromMarker(Marker marker)
        {
            var position = new Position(marker.Position.Latitude, marker.Position.Longitude);
            foreach (var pin in App.mainPage.map.Pins)
            {
                if (pin.Position == position)
                {
                    return pin;
                }
            }
            return null;
        }

        private async void LoadInfoWindow(object sender, Pin pin)
        {
            Marker marker = null;
            while (marker == null)
            {
                marker = GetMarkerForPin(App.mainPage.map.Pins[App.mainPage.map.Pins.IndexOf(pin)]);
            }

            if (!marker.IsInfoWindowShown)
            {
                marker.ShowInfoWindow();

                DataPoint currentPoint = await App.itemDatabase.GetPointAsync(pin.StyleId);
                App.mainPage.ShowDetail(currentPoint);
            }
        }
    }
}