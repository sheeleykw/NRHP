using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Position = Xamarin.Forms.Maps.Position;
using System.Collections.Generic;

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

        private async void MapSetup()
        {
            currentUserPosition = await locator.GetPositionAsync(TimeSpan.FromMilliseconds(100));
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

                UpdateMap();
            }
            else if(e.PropertyName.Equals("Height"))
            {
                TopLatitude = currentUserPosition.Latitude + LatitudeDegrees;
                BottomLatitude = currentUserPosition.Latitude - LatitudeDegrees;
                RightLongitude = currentUserPosition.Longitude + LongitudeDegrees;
                LeftLongitude = currentUserPosition.Longitude - LongitudeDegrees;

                UpdateMap();
            }
        }

        private async void UpdateMap()
        {
            var dataPoints = await App.database.GetPointsAsync(TopLatitude, BottomLatitude, RightLongitude, LeftLongitude);
            App.currentDataPoints = dataPoints;
            foreach(DataPoint dataPoint in dataPoints)
            {
                var point = new Point
                {
                    RefNum = dataPoint.RefNum,
                    Label = dataPoint.Name,
                    Address = dataPoint.Category,
                    Position = new Position(dataPoint.Latitude, dataPoint.Longitude),
                    Category = dataPoint.Category
                };
                if (!map.Pins.Contains(point))
                    map.Pins.Add(point);
            }
            //var copyList = new List<Pin>(map.Pins);
            //foreach (Point point in copyList)
            //{
            //    var find = App.currentDataPoints.Find(dataPoint => dataPoint.RefNum == point.RefNum);
            //    if (find == null)
            //    {
            //        map.Pins.Remove(point);
            //    }
            //}
        }

        private void PositionChanged(object sender, PositionEventArgs e)
        {
            currentUserPosition = e.Position;
        }

        private async Task StartLocationListening()
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
