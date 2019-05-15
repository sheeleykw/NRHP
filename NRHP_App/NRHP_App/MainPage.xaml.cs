using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using NRHP_App.Models;
using Position = Xamarin.Forms.Maps.Position;
using System.Reflection;
using System.IO;
using NRHP_App.Data;

namespace NRHP_App
{
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        private Plugin.Geolocator.Abstractions.Position currentUserPosition;
        private double LatitudeDegrees = 0.5;
        private double LongitudeDegrees = 0.5;
        private double TopLatitude;
        private double BottomLatitude;
        private double RightLongitude;
        private double LeftLongitude;
        private IGeolocator locator = CrossGeolocator.Current;

        public MainPage()
        {
            InitializeComponent();
            MapSetup();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //App.Database.GetPath();
        }

        async void MapSetup()
        {
            currentUserPosition = await locator.GetPositionAsync(TimeSpan.FromSeconds(100));
            await StartLocationListening();

            map.MoveToRegion(new MapSpan(new Position(currentUserPosition.Latitude, currentUserPosition.Longitude), LatitudeDegrees, LongitudeDegrees));
            map.PropertyChanged += ChangedView;
        }

        void ChangedView(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("VisibleRegion"))
            {
                TopLatitude = map.VisibleRegion.Center.Latitude + (map.VisibleRegion.LatitudeDegrees/2);
                BottomLatitude = map.VisibleRegion.Center.Latitude - (map.VisibleRegion.LatitudeDegrees/2);
                RightLongitude = map.VisibleRegion.Center.Longitude + (map.VisibleRegion.LongitudeDegrees/2);
                LeftLongitude = map.VisibleRegion.Center.Longitude - (map.VisibleRegion.LongitudeDegrees/2);

                AddPoints();
            }
        }

        async void AddPoints()
        {
            var points = await CosmoDBAccessPoint.GetPoints(TopLatitude, BottomLatitude, RightLongitude, LeftLongitude);

            foreach (DataPoint point in points)
            {
                map.Pins.Add(new Point
                {
                    RefNum = point.Refnum,
                    Label = point.Name,
                    Address = point.Address,
                    Position = new Position(point.Latitude, point.Longitude),
                    Category = point.Category
                });
            }

        }

        void PositionChanged(object sender, PositionEventArgs e)
        {
            currentUserPosition = e.Position;
        }

        async Task StartLocationListening()
        {
            await locator.StartListeningAsync(TimeSpan.FromSeconds(5), 10, true, new ListenerSettings
            {
                ActivityType = ActivityType.AutomotiveNavigation,
                AllowBackgroundUpdates = true,
                DeferLocationUpdates = true,
                DeferralDistanceMeters = 1,
                DeferralTime = TimeSpan.FromSeconds(1),
                ListenForSignificantChanges = true,
                PauseLocationUpdatesAutomatically = false
            });
            locator.PositionChanged += PositionChanged;
        }
    }
}
