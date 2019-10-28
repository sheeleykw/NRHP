using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NRHP_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        public SearchBar searchBar;
        //List that holds the data that is displayed, DataPoints.
        private List<DataPoint> currentSearchItems = new List<DataPoint>();
        //List that holds the data that is sorted, MapPoints.
        private List<MapPoint> currentSearchPositions;

        public SearchPage(List<MapPoint> mapPoints)
        {
            InitializeComponent();

            searchBar = new SearchBar
            {
                Placeholder = "Enter search term",
                //Text = App.mainPage.searchBar.Text,
                SearchCommand = new Command(() => Search())
            };
            //searchBar.TextChanged += TextChanged;
            NavigationPage.SetTitleView(this, searchBar);
            NavigationPage.SetBackButtonTitle(this, "");

            currentSearchPositions = mapPoints;
            SetupCurrentSearchItems();
        }

        public async void SetupCurrentSearchItems()
        {
            if (currentSearchPositions.Count == 0)
            {
                //searchListView.IsVisible = false;
                searchListView.IsRefreshing = true;
                //noItemsFound.IsVisible = true;
            }
            else
            {
                //Insert sort method on nameSearch
                Sort();

                //Intialize components for the visualization of the search list.
                searchListView.IsVisible = true;
                noItemsFound.IsVisible = false;
                var list = new List<DataPoint>();
                foreach (MapPoint mapPoint in currentSearchPositions)
                {
                    if (mapPoint.Category.Equals("City"))
                    {
                        var count = await App.itemDatabase.SearchCityAsync(mapPoint.Name.Split(',')[0], mapPoint.Name.Split(',')[1].Trim());
                        var cityPoint = new DataPoint
                        {
                            RefNum = "",
                            Name = mapPoint.Name,
                            City = "Number of sites found in city",
                            State = count.Count.ToString()
                        };
                        list.Add(cityPoint);
                    }
                    else
                    {
                        list.Add(await App.itemDatabase.GetPointAsync(mapPoint.RefNum));
                    }
                }
                currentSearchItems = list;
                searchListView.ItemsSource = currentSearchItems;
            }
        }

        private async void Search()
        {
            List<MapPoint> nameSearch = await SearchClass.NameSearch(searchBar.Text);

            currentSearchPositions = nameSearch;
            SetupCurrentSearchItems();
        }

        private void Sort()
        {
            var currentPosition = App.userPosition;

            for (int firstLoop = currentSearchPositions.Count; firstLoop > 1; firstLoop--)
            {
                for (int secondLoop = 0; secondLoop < firstLoop - 1; secondLoop++)
                {
                    var lengthToFirstPoint = Math.Abs(currentSearchPositions[secondLoop].Latitude - currentPosition.Latitude) + Math.Abs(currentSearchPositions[secondLoop].Longitude - currentPosition.Longitude);
                    var lengthToSecondPoint = Math.Abs(currentSearchPositions[secondLoop + 1].Latitude - currentPosition.Latitude) + Math.Abs(currentSearchPositions[secondLoop + 1].Longitude - currentPosition.Longitude);
                    if (lengthToFirstPoint > lengthToSecondPoint)
                    {
                        var firstPoint = currentSearchPositions[secondLoop];
                        var secondPoint = currentSearchPositions[secondLoop + 1];
                        currentSearchPositions[secondLoop] = secondPoint;
                        currentSearchPositions[secondLoop + 1] = firstPoint;
                    }
                }
            }
        }

        //private void TextChanged(object sender, EventArgs e)
        //{
        //    App.mainPage.searchBar.Text = searchBar.Text;
        //}

        private async void MainPageButton(object sender, EventArgs e)
        {
            await Navigation.PopAsync(false);
        }

        private async void FavoritePageButton(object sender, EventArgs e)
        {
            await Navigation.PopAsync(false);
            await Navigation.PushAsync(new FavoritesPage(), false);
        }

        private async void ListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PopAsync(false);
            App.mainPage.MoveToPoint(currentSearchPositions[e.SelectedItemIndex]);
        }
    }
}