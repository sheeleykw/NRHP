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
        private SearchBar searchBar = new SearchBar
        {
            Placeholder = "Enter building name"
        };

        public FavoritesPage()
        {
            InitializeComponent();
            Console.WriteLine(Navigation.ModalStack.Count);
            Console.WriteLine(Navigation.NavigationStack.Count);
            NavigationPage.SetTitleView(this, searchBar);
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

        private async void MainPageButton(object sender, EventArgs e)
        {
            await App.navPage.PopToRootAsync();
        }

        private async void ListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await App.navPage.PushAsync(new DetailPage(favorites[e.SelectedItemIndex].RefNum));
        }

        //private void BackButton(object sender, EventArgs e)
        //{
        //    Navigation.PopModalAsync();
        //}
    }
}