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
                NativeMap.MarkerClick -= SelectPoint;
                NativeMap.InfoWindowClose -= DeselectPoint;
            }

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
        }

        //Changes the currentPoints refnum and moves the camera to the selected point
        //Possibles changes might be made to the animation of the camera during selection
        private async void SelectPoint(object sender, GoogleMap.MarkerClickEventArgs e)
        {
            var marker = e.Marker;
            if (!marker.IsInfoWindowShown)
            {
                var moveCamera = CameraUpdateFactory.NewLatLng(marker.Position);
                NativeMap.AnimateCamera(moveCamera);
                marker.ShowInfoWindow();
                App.currentPinRefNum = (await App.database.GetPointNameAsync(marker.Title)).RefNum;
                App.mainPage.SwitchDetailPageButton();
            }
        }

        private void DeselectPoint(object sender, GoogleMap.InfoWindowCloseEventArgs e)
        {
            App.currentPinRefNum = null;
            App.mainPage.SwitchDetailPageButton();
        }

        protected override MarkerOptions CreateMarker(Pin pin)
        {
            var point = (Point)pin;
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(point.Position.Latitude, point.Position.Longitude));
            marker.SetTitle(point.Label);
            marker.SetSnippet(point.Category);
            marker.SetIcon(BitmapDescriptorFactory.DefaultMarker());
            return marker;
        }
    }
}