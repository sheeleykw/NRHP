using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NRHP_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoritesPage : ContentPage
    {
        private List<DataPoint> allFavorites;
        private List<DataPoint> currentFavorites;
        private SearchBar searchBar;

        public FavoritesPage()
        {
            InitializeComponent();
            searchBar = new SearchBar
            {
                Placeholder = "Enter search term",
            };
            NavigationPage.SetTitleView(this, searchBar);
        }

        protected override void OnAppearing()
        {
            SetupcurrentFavorites();
            base.OnAppearing();
        }

        public async void SetupcurrentFavorites()
        {
            allFavorites = await App.itemDatabase.GetFavoritedPointsAsync();
            currentFavorites = allFavorites;
            favoritesListView.ItemsSource = currentFavorites;
            if (currentFavorites.Count == 0)
            {
                favoritesListView.IsVisible = false;
                noFavorites.IsVisible = true;
            }
            searchBar.TextChanged += Search;
        }

        private void Search(object sender, EventArgs e)
        {
            var list = new List<DataPoint>();
            foreach(DataPoint dataPoint in allFavorites)
            {
                if(dataPoint.Name.ToLower().Contains(searchBar.Text.ToLower()))
                {
                    list.Add(dataPoint);
                }
            }

            currentFavorites = list;
            favoritesListView.ItemsSource = currentFavorites;

            if (currentFavorites.Count == 0)
            {
                favoritesListView.IsVisible = false;
                noFavoritesFound.IsVisible = true;
            }
            else
            {
                favoritesListView.IsVisible = true;
                noFavoritesFound.IsVisible = false;
            }
        }

        private async void MainPageButton(object sender, EventArgs e)
        {
            await App.navPage.PopToRootAsync();
        }

        private async void ListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await App.navPage.PushAsync(new DetailPage(App.navPage.CurrentPage, currentFavorites[e.SelectedItemIndex].RefNum));
        }
    }
}