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
    public partial class FavoritesPage : ContentPage
    {
        private List<DataPoint> favorites;

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

        public void BackButton(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}