using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Position = Xamarin.Forms.Maps.Position;
using Xamarin.Forms.Xaml;

namespace NRHP_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
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
        private SearchBar searchBar = new SearchBar
        {
            Placeholder = "Enter city/state"
        };

        //Creates the page and starts up the userPosition listening eventHandler
        public MainPage()
        {
            InitializeComponent();
            Console.WriteLine(Navigation.ModalStack.Count);
            Console.WriteLine(Navigation.NavigationStack.Count);
            NavigationPage.SetTitleView(this, searchBar);
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
            var mapPoints = await App.mapDatabase.GetPointsAsync(TopLatitude, BottomLatitude, RightLongitude, LeftLongitude);
            foreach (MapPoint mapPoint in mapPoints)
            {
                var pin = new Pin
                {
                    Label = mapPoint.Name,
                    Address = mapPoint.Category,
                    Position = new Position(mapPoint.Latitude, mapPoint.Longitude)
                };
                if (!map.Pins.Contains(pin))
                    map.Pins.Add(pin);
            }
        }

        //Responds to the detailPageButton
        //Needs to open another page which displays the details of the page
        private async void OpenDetailPage(object sender, EventArgs e)
        {
            await App.navPage.PushAsync(new DetailPage());
            //var imageUri = new Uri("https://npgallery.nps.gov/pdfhost/docs/NRHP/Photos/" + App.currentPinRefNum + ".pdf");
            //Device.OpenUri(imageUri);
        }

        private async void OpenFavoritesPage(object sender, EventArgs e)
        {
            await App.navPage.PushAsync(new FavoritesPage());
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
