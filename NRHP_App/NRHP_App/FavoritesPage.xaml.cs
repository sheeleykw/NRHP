﻿using System;
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
            searchBar = new SearchBar
            {
                Placeholder = "Enter search term",
            };

            NavigationPage.SetTitleView(this, searchBar);
            NavigationPage.SetBackButtonTitle(this, "");

            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            SetupCurrentFavorites();
            base.OnAppearing();
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
            searchBar.TextChanged += Search;
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

        private async void MainPageButton(object sender, EventArgs e)
        {
            await App.navPage.PopToRootAsync();
        }

        private async void ListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            DataPoint currentPoint = await App.itemDatabase.GetPointAsync(currentFavorites[e.SelectedItemIndex].RefNum);
            await App.navPage.PushAsync(new DetailPage(App.navPage.CurrentPage, currentPoint));
        }
    }
}