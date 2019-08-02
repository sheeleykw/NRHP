﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NRHP_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailPage : ContentPage
    {
        private string currentRefNum = App.currentPinRefNum;
        private Page previousPage;
        private DataPoint currentPoint;
        private ImageButton favoriteButton = new ImageButton
        {
            Source = "bluehearticonhollow.png",
            HorizontalOptions = LayoutOptions.EndAndExpand,
            VerticalOptions = LayoutOptions.CenterAndExpand,
            HeightRequest = 35,
            Margin = new Thickness
            {
                Right = 5
            }
        };
        private Label titleLabel = new Label { Text = "PROPERTY DETAIL" };
        private AbsoluteLayout titleLayout = new AbsoluteLayout();
        private bool imageAccess = false;

        //Constructor for mainPage
        public DetailPage(Page previousPage)
        {
            NavigationPage.SetBackButtonTitle(this, "");
            this.previousPage = previousPage;

            InitializeComponent();
            SetupPage();
        }

        public DetailPage(Page previousPage, string RefNum)
        {
            NavigationPage.SetBackButtonTitle(this, "");
            this.previousPage = previousPage;
            currentRefNum = RefNum;

            InitializeComponent();
            SetupPage();
        }

        private async void SetupPage()
        {
            //Absolute layout of title goes here. JUST DO IT
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            currentPoint = await App.itemDatabase.GetPointAsync(currentRefNum);

            if (currentPoint.IsFavorited)
            {
                favoriteButton.Source = "bluehearticon.png";
            }
            NavigationPage.SetTitleView(this, titleLayout);
            favoriteButton.Clicked += FavoriteItemToggle;

            name.Text = currentPoint.Name;
            category.Text = "Category: " + currentPoint.Category;
            refNum.Text = "Reference Number: " + "#" + currentPoint.RefNum; 
            sourceDate.Text = "Date added to register: " + currentPoint.SourceDate;
            address.Text = "Reported Street Address: " + currentPoint.Address;
            cityState.Text = "Location: " + currentPoint.City + ", " + currentPoint.State;
            county.Text = "County: " + currentPoint.County;
            people.Text = "Architects/Builders: " + currentPoint.Architects;

            try
            {
                imageAccess = App.stateList.Find(stateBind => stateBind.objectName.Equals(currentPoint.State)).objectState;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to match state of current point to list of stateBindings");
            }
        }

        private async void MainPageButton(object sender, EventArgs e)
        {
            await App.navPage.PopToRootAsync();
        }

        private async void FavoritePageButton(object sender, EventArgs e)
        {
            await App.navPage.PopAsync();

            var page = new FavoritesPage();
            if (!previousPage.GetType().Equals(page.GetType()))
            {
                await App.navPage.PushAsync(new FavoritesPage());
            }
        }

        private void FavoriteItemToggle(object sender, EventArgs e)
        {
            currentPoint.IsFavorited = !currentPoint.IsFavorited;
            if (currentPoint.IsFavorited)
            {
                favoriteButton.Source = "bluehearticon.png";
            }
            else
            {
                favoriteButton.Source = "bluehearticonhollow.png";
            }
            App.itemDatabase.UpdatePoint(currentPoint);
        }

        private async void PhotoButton(object sender, EventArgs e)
        {
            if (imageAccess)
            {
                if (Device.RuntimePlatform.Equals(Device.iOS))
                {
                    await App.navPage.PushAsync(new WebView("https://npgallery.nps.gov/pdfhost/docs/NRHP/Photos/" + currentRefNum + ".pdf"));
                }
                else if (Device.RuntimePlatform.Equals(Device.Android))
                {
                    Device.OpenUri(new Uri("https://npgallery.nps.gov/pdfhost/docs/NRHP/Photos/" + currentRefNum + ".pdf"));
                }
            }
            else
            {
                await DisplayAlert("The photos are unavailable.", "Unfortunately, we have not yet obtained the copyright access to display the images in our app.", "Okay");
            }
        }

        private async void DocButton(object sender, EventArgs e)
        {
            if (imageAccess)
            {
                if (Device.RuntimePlatform.Equals(Device.iOS))
                {
                    await App.navPage.PushAsync(new WebView("https://npgallery.nps.gov/pdfhost/docs/NRHP/Text/" + currentRefNum + ".pdf"));
                }
                else if (Device.RuntimePlatform.Equals(Device.Android))
                {
                    Device.OpenUri(new Uri("https://npgallery.nps.gov/pdfhost/docs/NRHP/Text/" + currentRefNum + ".pdf"));
                }
            }
            else
            {
                await DisplayAlert("The documents are unavailable.", "Unfortunately, we have not yet obtained the copyright access to display the images in our app.", "Okay");
            }
        }
    }
}