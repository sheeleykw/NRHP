﻿#pragma warning disable CS4014
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Position = Xamarin.Forms.Maps.Position;

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

        private DataPoint currentPoint = null;
        private bool imageAccess;
        public NRHPMap map;
        private int mapMoving = 0;
        public EventHandler<Pin> SearchCompleted;
        private Pin searchPin;
        private bool searchGoing;


        //Creates the page and starts up the userPosition listening eventHandler
        public MainPage()
        {
            AdMobView adView = null;
            //testID
            //AdUnitId = "ca-app-pub-3940256099942544/6300978111";
            if (Device.RuntimePlatform == Device.iOS)
            {
                adView = new AdMobView { AdUnitId = "ca-app-pub-3281339494640251/8346216507" };
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                adView = new AdMobView { AdUnitId = "ca-app-pub-3940256099942544/6300978111" };
                //realID
                //AdUnitId = "ca-app-pub-3281339494640251/9986601233";
            }

            //adView.HorizontalOptions = LayoutOptions.FillAndExpand;
            //adView.VerticalOptions = LayoutOptions.CenterAndExpand;

            InitializeComponent();
            //adSpace.Children.Add(adView);
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

            mapStack.Children.Add(map);

            TopLatitude = App.userPosition.Latitude + LatitudeDegrees;
            BottomLatitude = App.userPosition.Latitude - LatitudeDegrees;
            RightLongitude = App.userPosition.Longitude + LongitudeDegrees;
            LeftLongitude = App.userPosition.Longitude - LongitudeDegrees;

            await Task.Delay(150);
            await UpdateMap();

            map.PropertyChanged += ChangedView;
        }

        //Updates the view of the camera to allow the database to know where to search
         async void ChangedView(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("VisibleRegion"))
            {
                mapMoving++;
                await Task.Delay(250);
                mapMoving--;

                if (mapMoving == 0)
                {
                    TopLatitude = map.VisibleRegion.Center.Latitude + (map.VisibleRegion.LatitudeDegrees / 2);
                    BottomLatitude = map.VisibleRegion.Center.Latitude - (map.VisibleRegion.LatitudeDegrees / 2);
                    RightLongitude = map.VisibleRegion.Center.Longitude + (map.VisibleRegion.LongitudeDegrees / 2);
                    LeftLongitude = map.VisibleRegion.Center.Longitude - (map.VisibleRegion.LongitudeDegrees / 2);

                    await UpdateMap();
                }
            }
        }

        public void ChangeText(string searchText)
        {
            searchBar.Text = searchText;
        }

        //Gets called when the view is changed either when first loading the map or when the camera changes the view
        //Calls the database to retrieve the points that are currently rendered within the cameras view
        private async Task UpdateMap()
        {
            var mapPoints = await App.mapDatabase.GetPointsAsync(TopLatitude, BottomLatitude, RightLongitude, LeftLongitude);
            foreach (MapPoint mapPoint in mapPoints)
            {
                Pin pin = new Pin
                {
                    Label = mapPoint.Name,
                    Address = mapPoint.Category,
                    Position = new Position(mapPoint.Latitude, mapPoint.Longitude),
                    StyleId = mapPoint.RefNum
                };

                if (!map.Pins.Contains(pin))
                {
                    map.Pins.Add(pin);
                }
                else if (pin.Equals(searchPin) && searchGoing)
                {
                    EventHandler<Pin> handler = SearchCompleted;
                    handler?.Invoke(this, pin);
                    searchGoing = false;
                }
            }
        }

        //Search method for when the users presses the search button.
        private async void Search(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SearchPage(searchBar.Text), false);
        }

        //Moves the map to the latitude and longitude coordinates accessed from the given mapPoint.
        public void MoveToPoint(MapPoint mapPoint)
        {
            if (!mapPoint.Category.Equals("City"))
            {
                map.MoveToRegion(new MapSpan(new Position(mapPoint.Latitude, mapPoint.Longitude), LatitudeDegrees, LongitudeDegrees));

                searchPin = new Pin
                {
                    Label = mapPoint.Name,
                    Address = mapPoint.Category,
                    Position = new Position(mapPoint.Latitude, mapPoint.Longitude),
                    StyleId = mapPoint.RefNum
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

        public async void OpenFavoritesPage(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(App.favPage, false);
        }

        //Called when a pin is selected or deselected
        //Changes the state of the detailPageButton to reflect the user's ability to open the detail page for a selected point
        public void ShowDetail(DataPoint currentPoint)
        {
            this.currentPoint = currentPoint;

            searchBar.IsVisible = false;
            detailStack.TranslateTo(0, 0, 300, Easing.CubicInOut);
            name.TranslateTo(0, 0, 300, Easing.CubicInOut);

            name.Text = currentPoint.Name;
            category.Text = "Category: " + currentPoint.Category;
            refNum.Text = "Reference Number: " + "#" + currentPoint.RefNum;
            sourceDate.Text = "Date added to register: " + currentPoint.SourceDate;
            address.Text = "Reported Street Address: " + currentPoint.Address;
            cityState.Text = "Location: " + currentPoint.City + ", " + currentPoint.State;
            county.Text = "County: " + currentPoint.County;
            people.Text = "Architects/Builders: " + currentPoint.Architects;
            
            if(currentPoint.IsFavorited)
            {
                favoriteButton.Source = "bluehearticon.png";
            }
            else
            {
                favoriteButton.Source = "bluehearticonhollow.png";
            }

            try
            {
                imageAccess = App.stateList.Find(stateBind => stateBind.objectName.Equals(currentPoint.State)).objectState;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void HideDetail()
        {
            detailStack.TranslateTo(0, 500, 300, Easing.CubicInOut);
            name.TranslateTo(0, -500, 300, Easing.CubicInOut);
            searchBar.IsVisible = true;
        }

        private void FavoriteItemToggle(object sender, EventArgs e)
        {
            App.updatedFavorites = true;
            if (currentPoint.IsFavorited)
            {
                favoriteButton.Source = "bluehearticonhollow.png";
                currentPoint.IsFavorited = false;
            }
            else
            {
                favoriteButton.Source = "bluehearticon.png";
                currentPoint.IsFavorited = true;
            }
            App.itemDatabase.UpdatePoint(currentPoint);
        }

        private async void OpenPhotos(object sender, EventArgs e)
        {
            if (imageAccess)
            {
                if (Device.RuntimePlatform.Equals(Device.iOS))
                {
                    await Navigation.PushModalAsync(new WebView("https://npgallery.nps.gov/pdfhost/docs/NRHP/Photos/" + currentPoint.RefNum + ".pdf"), false);
                }
                else if (Device.RuntimePlatform.Equals(Device.Android))
                {
                    Device.OpenUri(new Uri("https://npgallery.nps.gov/pdfhost/docs/NRHP/Photos/" + currentPoint.RefNum + ".pdf"));
                }
            }
            else
            {
                await DisplayAlert("The photos are unavailable.", "Unfortunately, we have not yet obtained the copyright access to display the images in our app.", "Okay");
            }
        }

        private async void OpenDocs(object sender, EventArgs e)
        {
            if (imageAccess)
            {
                if (Device.RuntimePlatform.Equals(Device.iOS))
                {
                    await Navigation.PushModalAsync(new WebView("https://npgallery.nps.gov/pdfhost/docs/NRHP/Text/" + currentPoint.RefNum + ".pdf"), false);
                }
                else if (Device.RuntimePlatform.Equals(Device.Android))
                {
                    Device.OpenUri(new Uri("https://npgallery.nps.gov/pdfhost/docs/NRHP/Text/" + currentPoint.RefNum + ".pdf"));
                }
            }
            else
            {
                await DisplayAlert("The documents are unavailable.", "Unfortunately, we have not yet obtained the copyright access to display the images in our app.", "Okay");
            }
        }
    }
}
