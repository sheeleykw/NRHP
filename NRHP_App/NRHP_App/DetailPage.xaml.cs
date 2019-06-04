using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NRHP_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailPage : ContentPage
    {
        private string currentRefNum = App.currentPinRefNum;
        private DataPoint currentPoint;
        private Button favoriteButton = new Button
        {
            BackgroundColor = Color.Snow,
            HorizontalOptions = LayoutOptions.EndAndExpand,
            VerticalOptions = LayoutOptions.CenterAndExpand
        };

        //Constructor for mainPage
        public DetailPage()
        {
            InitializeComponent();
            favoriteButton.Clicked += FavoriteItemToggle;
            NavigationPage.SetTitleView(this, favoriteButton);
            SetupLabels();
        }

        public DetailPage(string RefNum)
        {
            InitializeComponent();
            favoriteButton.Clicked += FavoriteItemToggle;
            NavigationPage.SetTitleView(this, favoriteButton);
            currentRefNum = RefNum;
            SetupLabels();
        }

        private async void SetupLabels()
        {
            currentPoint = await App.itemDatabase.GetPointAsync(currentRefNum);
            name.Text = currentPoint.Name;
            category.Text = "Category: " + currentPoint.Category;
            sourceDate.Text = "Date added to register: " + currentPoint.SourceDate;
            address.Text = "Reported Street Address: " + currentPoint.Address;
            cityState.Text = "Location: " + currentPoint.City + ", " + currentPoint.State;
            county.Text = "County: " + currentPoint.County;
            people.Text = "Architects/Builders: " + currentPoint.Architects;
        }

        private async void MainPageButton(object sender, EventArgs e)
        {
            await App.navPage.PopToRootAsync();
        }

        private void FavoriteItemToggle(object sender, EventArgs e)
        {
            currentPoint.IsFavorited = !currentPoint.IsFavorited;
            App.itemDatabase.UpdatePoint(currentPoint);
        }

        //private async void BackButton(object sender, EventArgs e)
        //{
        //    if (Navigation.ModalStack.Count > 1 && Navigation.ModalStack.ElementAt(Navigation.ModalStack.Count - 2).GetType().Equals((new FavoritesPage()).GetType()))
        //    {
        //        var favoritesPage = (FavoritesPage)Navigation.ModalStack.ElementAt(Navigation.ModalStack.Count - 2);
        //        favoritesPage.SetupFavorites();
        //    }
        //    await App.navPage.PopAsync();
        //}

        private async void PhotoButton(object sender, EventArgs e)
        {
            if (Device.RuntimePlatform.Equals(Device.iOS))
            {
                await App.navPage.PushAsync(new WebView("https://npgallery.nps.gov/pdfhost/docs/NRHP/Photos/" + App.currentPinRefNum + ".pdf"));
            }
            else if (Device.RuntimePlatform.Equals(Device.Android))
            {
                Device.OpenUri(new Uri("https://npgallery.nps.gov/pdfhost/docs/NRHP/Photos/" + App.currentPinRefNum + ".pdf"));
            }
        }

        private async void DocButton(object sender, EventArgs e)
        {
            if (Device.RuntimePlatform.Equals(Device.iOS))
            {
                await App.navPage.PushAsync(new WebView("https://npgallery.nps.gov/pdfhost/docs/NRHP/Text/" + App.currentPinRefNum + ".pdf"));
            }
            else if (Device.RuntimePlatform.Equals(Device.Android))
            {
                Device.OpenUri(new Uri("https://npgallery.nps.gov/pdfhost/docs/NRHP/Text/" + App.currentPinRefNum + ".pdf"));
            }
        }
    }
}