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
        private Page previousPage;
        private DataPoint currentPoint;
        private ImageButton favoriteButton = new ImageButton
        {
            Source = "bluehearticonhollow.png",
            HorizontalOptions = LayoutOptions.EndAndExpand,
            VerticalOptions = LayoutOptions.CenterAndExpand,
            HeightRequest = 35
        };

        //Constructor for mainPage
        public DetailPage(Page previousPage)
        {
            InitializeComponent();
            this.previousPage = previousPage;
            favoriteButton.Clicked += FavoriteItemToggle;
            NavigationPage.SetTitleView(this, favoriteButton);
            SetupLabels();
        }

        public DetailPage(Page previousPage, string RefNum)
        {
            InitializeComponent();
            this.previousPage = previousPage;
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
            refNum.Text = "Reference #: " + currentPoint.RefNum; 
            sourceDate.Text = "Date added to register: " + currentPoint.SourceDate;
            address.Text = "Reported Street Address: " + currentPoint.Address;
            cityState.Text = "Location: " + currentPoint.City + ", " + currentPoint.State;
            county.Text = "County: " + currentPoint.County;
            people.Text = "Architects/Builders: " + currentPoint.Architects;
            if (currentPoint.IsFavorited)
            {
                favoriteButton.Source = "bluehearticon.png";
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