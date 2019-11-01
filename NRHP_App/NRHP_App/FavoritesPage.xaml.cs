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

        public FavoritesPage()
        {
            InitializeComponent();
            favoritesListView.RefreshControlColor = Color.Black;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            wheelie.IsRunning = true;
            wheelie.IsVisible = true;
            SetupCurrentFavorites();
        }

        public async void SetupCurrentFavorites()
        {
            allFavorites = await App.itemDatabase.GetFavoritedPointsAsync();
            currentFavorites = allFavorites;
            favoritesListView.ItemsSource = currentFavorites;
            if (currentFavorites.Count == 0)
            {
                favoritesListView.IsVisible = false;
                noFavorites.IsVisible = true;
            }
            else 
            {
                favoritesListView.IsVisible = true;
                noFavorites.IsVisible = false;
            }
            wheelie.IsRunning = false;
            wheelie.IsVisible = false;
        }

        private void Search(object sender, EventArgs e)
        {
            if (allFavorites.Count > 0)
            {
                var list = new List<DataPoint>();
                foreach (DataPoint dataPoint in allFavorites)
                {
                    if (dataPoint.Name.ToLower().Contains(searchBar.Text.ToLower()))
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
        }

        private void DeleteFavorite(object sender, EventArgs e)
        {
            Console.WriteLine(sender.ToString());
        }

        private async void MainPageButton(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(false);
        }

        private async void ListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PopModalAsync(false);
            var mapPoint = await App.mapDatabase.GetPointAsync(currentFavorites[e.SelectedItemIndex].RefNum);
            App.mainPage.MoveToPoint(mapPoint);
        }
    }
}