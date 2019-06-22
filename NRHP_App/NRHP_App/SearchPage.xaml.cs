using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Text = searchText
            };
            NavigationPage.SetTitleView(this, searchBar);

            currentSearchPositions = mapPoints;
            SetupCurrentSearchItems();
        }


        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //}

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

        //private void Search(object sender, EventArgs e)
        //{
        //    if (allFavorites.Count > 0)
        //    {
        //        var list = new List<DataPoint>();
        //        foreach (DataPoint dataPoint in allFavorites)
        //        {
        //            if (dataPoint.Name.ToLower().Contains(searchBar.Text.ToLower()))
        //            {
        //                list.Add(dataPoint);
        //            }
        //        }

        //        currentFavorites = list;
        //        favoritesListView.ItemsSource = currentFavorites;

        //        if (currentFavorites.Count == 0)
        //        {
        //            favoritesListView.IsVisible = false;
        //            noFavoritesFound.IsVisible = true;
        //        }
        //        else
        //        {
        //            favoritesListView.IsVisible = true;
        //            noFavoritesFound.IsVisible = false;
        //        }
        //    }
        //}

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