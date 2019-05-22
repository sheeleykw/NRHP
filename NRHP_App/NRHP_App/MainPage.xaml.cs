using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Position = Xamarin.Forms.Maps.Position;

namespace NRHP_App
{
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        private Plugin.Geolocator.Abstractions.Position currentUserPosition;
        private IGeolocator locator = CrossGeolocator.Current;
        private double LatitudeDegrees = 0.0095328892176525;
        private double LongitudeDegrees = 0.00882815569639206;
        private double TopLatitude;
        private double BottomLatitude;
        private double RightLongitude;
        private double LeftLongitude;

        //Creates the page and starts up the userPosition listening eventHandler
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            MapSetup();
        }

        //First intialization of the userPosition and viewChanging eventHandler
        //Possibles changes might be need to the userPosition listener/eventHandler
        private async void MapSetup()
        {
            currentUserPosition = await locator.GetPositionAsync(TimeSpan.FromSeconds(100));
            locator.PositionChanged += PositionChanged;

            map.IsShowingUser = true;
            map.PropertyChanged += ChangedView;
            map.MoveToRegion(new MapSpan(new Position(currentUserPosition.Latitude, currentUserPosition.Longitude), LatitudeDegrees, LongitudeDegrees));
        }

        //Gets called when the user position changes
        private void PositionChanged(object sender, PositionEventArgs e)
        {
            currentUserPosition = e.Position;
        }

        //Updates the view of the camera to allow the database to know where to search
        void ChangedView(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("VisibleRegion"))
            {
                TopLatitude = map.VisibleRegion.Center.Latitude + (map.VisibleRegion.LatitudeDegrees / 2);
                BottomLatitude = map.VisibleRegion.Center.Latitude - (map.VisibleRegion.LatitudeDegrees / 2);
                RightLongitude = map.VisibleRegion.Center.Longitude + (map.VisibleRegion.LongitudeDegrees / 2);
                LeftLongitude = map.VisibleRegion.Center.Longitude - (map.VisibleRegion.LongitudeDegrees / 2);

                UpdateMap();
            }
            else if (e.PropertyName.Equals("Height"))
            {
                TopLatitude = currentUserPosition.Latitude + LatitudeDegrees;
                BottomLatitude = currentUserPosition.Latitude - LatitudeDegrees;
                RightLongitude = currentUserPosition.Longitude + LongitudeDegrees;
                LeftLongitude = currentUserPosition.Longitude - LongitudeDegrees;

                UpdateMap();
            }
        }

        //Gets called when the view is changed either when first loading the map or when the camera changes the view
        //Calls the database to retrieve the points that are currently rendered within the cameras view
        private async void UpdateMap()
        {
            var dataPoints = await App.database.GetPointsAsync(TopLatitude, BottomLatitude, RightLongitude, LeftLongitude);
            foreach (DataPoint dataPoint in dataPoints)
            {
                var point = new Point
                {
                    RefNum = dataPoint.RefNum,
                    Label = dataPoint.Name,
                    Address = "NULL",
                    Position = new Position(dataPoint.Latitude, dataPoint.Longitude),
                    Category = dataPoint.Category
                };
                if (!map.Pins.Contains(point))
                    map.Pins.Add(point);
            }
        }

        //Responds to the detailPageButton
        //Needs to open another page which displays the details of the page
        private void OpenDetailPage(object sender, EventArgs e)
        {
            var imageUri = new Uri("https://npgallery.nps.gov/pdfhost/docs/NRHP/Photos/" + App.currentPinRefNum + ".pdf");
            Device.OpenUri(imageUri);
        }

        private async void OpenFavoritesPage(object sender, EventArgs e)
        {
            var favorites = await App.database.GetFavoritePointsAsync();
            Console.WriteLine(favorites.Count);
            await Navigation.PushAsync(new FavoritesPage(favorites));
        }

        //Is called when a pin is selected or deselected
        //Changes the state of the detailPageButton to reflect the user's ability to open the detail page for a selected point
        public void SwitchDetailPageButton()
        {
            if (!(App.currentPinRefNum == null))
            {
                detailPageButton.IsEnabled = true;
            }
            else
            {
                detailPageButton.IsEnabled = false;
            }
        }
    }
}
