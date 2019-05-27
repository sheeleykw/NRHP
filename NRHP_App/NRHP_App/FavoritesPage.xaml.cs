using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NRHP_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoritesPage : ContentPage
    {
        List<DataPoint> favorites;

        public FavoritesPage()
        {
            InitializeComponent();
            SetupFavorites();
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

        private void ListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Navigation.PushModalAsync(new DetailPage(favorites[e.SelectedItemIndex].RefNum));
        }

        private void BackButton(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}