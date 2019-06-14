using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Position = Xamarin.Forms.Maps.Position;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;

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

        private SearchBar searchBar;
        public EventHandler<MapPoint> SearchCompleted;


        //Creates the page and starts up the userPosition listening eventHandler
        public MainPage()
        {
            InitializeComponent();
            searchBar = new SearchBar
            {
                Placeholder = "Enter search term",
                SearchCommand = new Command(() => Search())
            };
            NavigationPage.SetTitleView(this, searchBar);
            MapSetup();
        }

        //First intialization of the userPosition and viewChanging eventHandler
        //Possibles changes might be need to the userPosition listener/eventHandler
        private async void MapSetup()
        {
            currentUserPosition = await locator.GetPositionAsync(TimeSpan.FromSeconds(5));
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
                {
                    map.Pins.Add(pin);
                    App.currentPins.Add(pin);
                }

            }
        }

        private async void Search()
        {
            var result = await App.itemDatabase.GetDataPoints();
            var oldestPoint = result[0];
            foreach (DataPoint dataPoint in result)
            {
                var datasourceDate = dataPoint.SourceDate.Split('/');
                var dataday = Convert.ToInt32(datasourceDate[1]);
                var datamonth = Convert.ToInt32(datasourceDate[0]);
                var datayear = Convert.ToInt32(datasourceDate[2]);

                var oldsourceDate = oldestPoint.SourceDate.Split('/');
                var oldday = Convert.ToInt32(oldsourceDate[1]);
                var oldmonth = Convert.ToInt32(oldsourceDate[0]);
                var oldyear = Convert.ToInt32(oldsourceDate[2]);

                if (datayear <= oldyear)
                {
                    if (datamonth <= oldmonth)
                    {
                        if (dataday < oldday)
                        {
                            oldestPoint = dataPoint;
                        }
                    }
                }
            }

            Console.WriteLine(oldestPoint.Name + ": " + oldestPoint.City + ", " + oldestPoint.State + ", " + oldestPoint.Category + ".." + oldestPoint.SourceDate);

            EventHandler<MapPoint> handler = SearchCompleted;
            string searchBarText = searchBar.Text.ToLower();

            List<MapPoint> nameSearch = await App.mapDatabase.SearchPointsNameAsync(searchBarText);
            MapPoint refNumSearch = await App.mapDatabase.SearchPointsRefNumAsync(searchBarText);

            if (refNumSearch != null)
            {
                map.MoveToRegion(new MapSpan(new Position(refNumSearch.Latitude, refNumSearch.Longitude), map.VisibleRegion.LatitudeDegrees, map.VisibleRegion.LongitudeDegrees));
                var searchPoint = refNumSearch;
                handler?.Invoke(this, searchPoint);
            }
            else if (nameSearch.Count == 1)
            {
                map.MoveToRegion(new MapSpan(new Position(nameSearch[0].Latitude, nameSearch[0].Longitude), map.VisibleRegion.LatitudeDegrees, map.VisibleRegion.LongitudeDegrees));
                var searchPoint = nameSearch[0];
                handler?.Invoke(this, searchPoint);
            }
            else
            {
                string searchText = "";

                foreach(char spot in searchBarText)
                {
                    if (!char.IsPunctuation(spot))
                    {
                        searchText = searchText.Insert(searchText.Length, spot.ToString());
                    }
                }

                string[] searchWords = searchText.Split(' ');
                List<List<MapPoint>> nameSearches = new List<List<MapPoint>>();
                List<List<DataPoint>> citySearches = new List<List<DataPoint>>();
                List<MapPoint> mainNameSearch;
                List<DataPoint> mainCitySearch;

                var i = 0;
                var biggestNameSearch = 0;
                var biggestCitySearch = 0;

                foreach (string word in searchWords)
                {
                    nameSearches.Add(await App.mapDatabase.SearchPointsNameAsync(word));
                    citySearches.Add(await App.itemDatabase.SearchPointsCityAsync(word));
                    if (nameSearches[i].Count > biggestNameSearch)
                        biggestNameSearch = i;
                    if (citySearches[i].Count > biggestCitySearch)
                        biggestCitySearch = i;
                    i++;
                }
                if (searchWords.Length > 1)
                {
                    nameSearches.Add(await App.mapDatabase.SearchPointsNameAsync(searchText));
                    citySearches.Add(await App.itemDatabase.SearchPointsCityAsync(searchText));
                    i++;
                }

                mainNameSearch = nameSearches[biggestNameSearch];
                mainCitySearch = citySearches[biggestCitySearch];
                nameSearches.RemoveAt(biggestNameSearch);
                citySearches.RemoveAt(biggestCitySearch);

                //Begin sorting part of method
                List<List<MapPoint>> nameRelevanceLevels = new List<List<MapPoint>>();
                List<List<DataPoint>> cityRelevanceLevels = new List<List<DataPoint>>();

                for (int j = i - 1; j > 0; j--)
                {
                    nameRelevanceLevels.Add(new List<MapPoint>());
                    cityRelevanceLevels.Add(new List<DataPoint>());
                }

                foreach(MapPoint nameItem in mainNameSearch)
                {
                    var itemOccurrence = 0;

                    if (nameSearches.Count > 0)
                    {
                        foreach (List<MapPoint> nameSearchList in nameSearches)
                        {
                            if (nameSearchList.Find(item => item.RefNum.Equals(nameItem.RefNum) && item.Latitude == nameItem.Latitude && item.Longitude == nameItem.Longitude) != null)
                                itemOccurrence++;
                        }
                    }

                    nameRelevanceLevels[itemOccurrence].Add(nameItem);
                }

                foreach(List<MapPoint> relevanceLevel in nameRelevanceLevels)
                {
                    if (relevanceLevel.Count > 0)
                        Console.WriteLine(relevanceLevel[0].Name);
                }
            }
        }

        //Responds to the detailPageButton
        //Needs to open another page which displays the details of the page
        private async void OpenDetailPage(object sender, EventArgs e)
        {
            await App.navPage.PushAsync(new DetailPage(App.navPage.CurrentPage));
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
