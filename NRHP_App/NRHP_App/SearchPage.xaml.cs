using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NRHP_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        public SearchBar searchBar;
        private List<DataPoint> currentSearchItems = new List<DataPoint>();
        private List<MapPoint> currentSearchPositions;

        public SearchPage(string searchText, List<MapPoint> mapPoints)
        {
            InitializeComponent();

            searchBar = new SearchBar
            {
                Placeholder = "Enter search term",
                Text = searchText,
                SearchCommand = new Command(Search)
            };
            NavigationPage.SetTitleView(this, searchBar);

            currentSearchPositions = mapPoints;
            SetupCurrentSearchItems();
        }

        public async void SetupCurrentSearchItems()
        {
            foreach(MapPoint mapPoint in currentSearchPositions)
            {
                currentSearchItems.Add(await App.itemDatabase.GetPointAsync(mapPoint.RefNum));
            }
            searchListView.ItemsSource = currentSearchItems;
            if (currentSearchItems.Count == 0)
            {
                searchListView.IsVisible = false;
                noItemsFound.IsVisible = true;
            }
        }

        private async void Search()
        {
            List<MapPoint> nameSearch = new List<MapPoint>();

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

            if (splitSearch.Length == 1)
            {
                nameSearch = await App.mapDatabase.SearchNameAsync(splitSearch[0]);
            }
            else if (splitSearch.Length == 2)
            {
                nameSearch = await App.mapDatabase.SearchNameAsync(splitSearch[0], splitSearch[1]);
            }
            else if (splitSearch.Length == 3)
            {
                nameSearch = await App.mapDatabase.SearchNameAsync(splitSearch[0], splitSearch[1], splitSearch[2]);
            }
            else if (splitSearch.Length == 4)
            {
                nameSearch = await App.mapDatabase.SearchNameAsync(splitSearch[0], splitSearch[1], splitSearch[2], splitSearch[3]);
            }

            currentSearchPositions = nameSearch;
            SetupCurrentSearchItems();
        }

        private async void MainPageButton(object sender, EventArgs e)
        {
            await App.navPage.PopAsync();
        }

        private async void FavoritePageButton(object sender, EventArgs e)
        {
            await App.navPage.PopAsync();
            await App.navPage.PushAsync(new FavoritesPage());
        }

        private async void ListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await App.navPage.PopAsync();
            App.mainPage.MoveToPoint(currentSearchPositions[e.SelectedItemIndex]);
        }
    }
}