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

        public SearchPage(List<MapPoint> mapPoints)
        {
            InitializeComponent();

            searchBar = new SearchBar
            {
                Placeholder = "Enter search term",
                Text = App.currentMapSearchTerm,
                SearchCommand = new Command(() => Search())
            };
            searchBar.TextChanged += TextChanged;
            NavigationPage.SetTitleView(this, searchBar);

            currentSearchPositions = mapPoints;
            SetupCurrentSearchItems();
        }

        public async void SetupCurrentSearchItems()
        {
            if (currentSearchPositions.Count == 0)
            {
                searchListView.IsVisible = false;
                noItemsFound.IsVisible = true;
            }
            else
            {
                var list = new List<DataPoint>();
                foreach (MapPoint mapPoint in currentSearchPositions)
                {
                    list.Add(await App.itemDatabase.GetPointAsync(mapPoint.RefNum));
                }
                currentSearchItems = list;
                searchListView.ItemsSource = currentSearchItems;
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

        private void TextChanged(object sender, EventArgs e)
        {
            App.currentMapSearchTerm = searchBar.Text;
            App.mainPage.UpdateText();
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