using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NRHP_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoritesPage : ContentPage
    {

        public FavoritesPage()
        {
            InitializeComponent();
            SetupFavorites();
        }

        private async void SetupFavorites()
        {
            var favorites = await App.itemDatabase.GetFavoritedPointsAsync();
            favoritesListView.ItemsSource = favorites;
            if (favorites.Count == 0)
            {
                favoritesListView.IsVisible = false;
                noFavorites.IsVisible = true;
            }
        }

        private void BackButton(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}