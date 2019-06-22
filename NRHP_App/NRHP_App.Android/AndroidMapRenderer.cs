﻿using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Android.Content;
using Xamarin.Forms.Maps.Android;
using NRHP_App;
using NRHP_App.Droid;
using Android.Gms.Maps.Model;
using Android.Gms.Maps;
using static Android.Gms.Maps.GoogleMap;
using System;
using System.Threading.Tasks;

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
            App.mainPage.SearchCompleted += LoadInfoWindow;
        }

        private async void LoadInfoWindow(object sender, MapPoint e)
        {
            //await Task.Delay(800);
            var markerFound = false;

            if (e != null)
            {
                Marker marker = null;
                while (!markerFound)
                {
                    try
                    {
                        marker = GetMarkerForPin(App.currentPins.Find(pin => pin.Label.Equals(e.Name)));
                        markerFound = true;
                    }
                    catch (Exception excp)
                    {
                        Console.WriteLine(excp.Message);
                        await Task.Delay(150);
                    }
                }
                if (!marker.IsInfoWindowShown)
                {
                    marker.ShowInfoWindow();
                    App.currentPinRefNum = (await App.mapDatabase.GetRefNumAsync(marker.Title, marker.Position.Latitude, marker.Position.Longitude)).RefNum;
                    App.mainPage.SwitchDetailPageButton();
                }
            }
        }

        //Changes the currentPoints refnum and moves the camera to the selected point
        //Possibles changes might be made to the animation of the camera during selection
        private async void SelectPoint(object sender, MarkerClickEventArgs e)
        {
            var marker = e.Marker;
            if (!marker.IsInfoWindowShown)
            {
                var moveCamera = CameraUpdateFactory.NewLatLng(marker.Position);
                NativeMap.AnimateCamera(moveCamera);
                marker.ShowInfoWindow();
                App.currentPinRefNum = (await App.mapDatabase.GetRefNumAsync(marker.Title, marker.Position.Latitude, marker.Position.Longitude)).RefNum;
                App.mainPage.SwitchDetailPageButton();
            }
        }

        private void DeselectPoint(object sender, InfoWindowCloseEventArgs e)
        {
            App.currentPinRefNum = null;
            App.mainPage.SwitchDetailPageButton();
        }

        protected override MarkerOptions CreateMarker(Pin pin)
        {
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            marker.SetTitle(pin.Label);
            marker.SetSnippet(pin.Address);
            marker.SetIcon(BitmapDescriptorFactory.DefaultMarker());
            return marker;
        }
    }
}