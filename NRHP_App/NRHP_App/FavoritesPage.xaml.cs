using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NRHP_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoritesPage : ContentPage
    {
        private List<DataPoint> favorites;

        public FavoritesPage()
        {
            InitializeComponent();
            NavigationPage.SetTitleView(this, new SearchBar
            {
                Placeholder = "Enter building"
            });
        }

        public async void SetupFavorites()
        {
            favorites = await App.itemDatabase.GetFavoritedPointsAsync();
            favoritesListView.ItemsSource = favorites;
            if (favorites.Count == 0)
            {
                favoritesListView.IsVisible = false;
                noFavorites.IsVisible = true;
            }
        }

        protected override void OnAppearing()
        {
            SetupFavorites();
            base.OnAppearing();
        }

        private async void MainPageButton(object sender, EventArgs e)
        {
            await App.navPage.PopToRootAsync();
        }

        private async void ListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await App.navPage.PushAsync(new DetailPage(favorites[e.SelectedItemIndex].RefNum));
        }
    }
}