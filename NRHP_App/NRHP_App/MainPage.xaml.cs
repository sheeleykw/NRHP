#pragma warning disable CS4014
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Position = Xamarin.Forms.Maps.Position;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace NRHP_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private double LatitudeDegrees = 0.055328892176525;
        private double LongitudeDegrees = 0.0482815569639206;
        private double TopLatitude;
        private double BottomLatitude;
        private double RightLongitude;
        private double LeftLongitude;

        public NRHPMap map;
        public SearchBar searchBar;
        public EventHandler<Pin> SearchCompleted;
        private Pin searchPin;
        private bool searchGoing;


        //Creates the page and starts up the userPosition listening eventHandler
        public MainPage()
        {
            searchBar = new SearchBar
            {
                Placeholder = "Enter search term",
                Text = "",
                SearchCommand = new Command(() => Search())
            };

            NavigationPage.SetTitleView(this, searchBar);
            NavigationPage.SetBackButtonTitle(this, "");

            InitializeComponent();
            MapSetup();
        }

        //First intialization of the userPosition and viewChanging eventHandler
        private async void MapSetup()
        {
            if (PermissionStatus.Granted == await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location))
            {
                var foundLocation = await Geolocation.GetLocationAsync();
                App.userPosition = new Position(foundLocation.Latitude, foundLocation.Longitude);
                map = new NRHPMap(new MapSpan(App.userPosition, LatitudeDegrees, LongitudeDegrees))
                {
                    MapType = MapType.Street
                };
                map.IsShowingUser = true;
            }
            else
            {
                map = new NRHPMap(new MapSpan(App.userPosition, LatitudeDegrees, LongitudeDegrees))
                {
                    MapType = MapType.Street
                };
            }

            stack.Children.Insert(stack.Children.IndexOf(border) + 1, map);

            TopLatitude = App.userPosition.Latitude + LatitudeDegrees;
            BottomLatitude = App.userPosition.Latitude - LatitudeDegrees;
            RightLongitude = App.userPosition.Longitude + LongitudeDegrees;
            LeftLongitude = App.userPosition.Longitude - LongitudeDegrees;

            await Task.Delay(150);
            await UpdateMap();

            map.PropertyChanged += ChangedView;
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
        }

        //Gets called when the view is changed either when first loading the map or when the camera changes the view
        //Calls the database to retrieve the points that are currently rendered within the cameras view
        private async Task UpdateMap()
        {
            var mapPoints = await App.mapDatabase.GetPointsAsync(TopLatitude, BottomLatitude, RightLongitude, LeftLongitude);
            foreach (MapPoint mapPoint in mapPoints)
            {
                var isEnabled = App.filterList.Find(objectBind => objectBind.objectName.Equals(mapPoint.Category)).objectState;
                if (isEnabled)
                {
                    var pin = new Pin
                    {
                        Label = mapPoint.Name,
                        Address = mapPoint.Category,
                        Position = new Position(mapPoint.Latitude, mapPoint.Longitude)
                    };

                    if (!map.Pins.Contains(pin))
                    {
                        map.Pins.Add(pin);
                    }

                    if (pin.Equals(searchPin) && searchGoing)
                    {
                        EventHandler<Pin> handler = SearchCompleted;
                        handler?.Invoke(this, pin);
                        searchGoing = false;
                    }
                }
            }
        }

        public void FilterChange()
        {
            map.Pins.Clear();
            UpdateMap();
        }

        //Search method for when the users presses the search button.
        private async void Search()
        {
            string searchBarText = searchBar.Text.ToLower().Trim();
            string searchText = "";
            foreach (char spot in searchBarText)
            {
                if (!char.IsPunctuation(spot))
                {
                    searchText = searchText.Insert(searchText.Length, spot.ToString());
                }
            }
            var splitSearch = searchText.Split(' ');

            List<MapPoint> nameSearch = await SearchClass.NameSearch(splitSearch);

            await App.navPage.PushAsync(new SearchPage(nameSearch));
        }

        //Moves the map to the latitude and longitude coordinates accessed from the given mapPoint.
        public void MoveToPoint(MapPoint mapPoint)
        {
            if (!mapPoint.Category.Equals("City"))
            {
                map.MoveToRegion(new MapSpan(new Position(mapPoint.Latitude, mapPoint.Longitude), map.VisibleRegion.LatitudeDegrees, map.VisibleRegion.LongitudeDegrees));

                searchPin = new Pin
                {
                    Label = mapPoint.Name,
                    Address = mapPoint.Category,
                    Position = new Position(mapPoint.Latitude, mapPoint.Longitude)
                };
                searchGoing = true;
            }
            else
            {
                map.MoveToRegion(new MapSpan(new Position(mapPoint.Latitude, mapPoint.Longitude), 1.0, 1.0));
            }
        }

        public void centerOnUser()
        {
            map.MoveToRegion(new MapSpan(App.userPosition, LatitudeDegrees, LongitudeDegrees));
            map.IsShowingUser = true;
        }

        private void OpenDetailPage(object sender, EventArgs e)
        {
            OpenDetailPage();
        }

        public async void OpenDetailPage()
        {
            DataPoint currentPoint = await App.itemDatabase.GetPointAsync(App.currentPinRefNum);
            Console.WriteLine(currentPoint.Name);
            await App.navPage.PushAsync(new DetailPage(App.navPage.CurrentPage, currentPoint));
        }

        private async void OpenFavoritesPage(object sender, EventArgs e)
        {
            await App.navPage.PushAsync(new FavoritesPage());
        }

        //Called when a pin is selected or deselected
        //Changes the state of the detailPageButton to reflect the user's ability to open the detail page for a selected point
        public void SwitchDetailPageButton()
        {
            if (!(App.currentPinRefNum == null))
            {
                detailPageButton.IsEnabled = true;
                detailPageButton.Opacity = 1;
            }
            else
            {
                detailPageButton.IsEnabled = false;
                detailPageButton.Opacity = 0.2;
            }
        }
    }
}
