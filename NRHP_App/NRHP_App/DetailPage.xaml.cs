using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NRHP_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailPage : ContentPage
    {
        //private string currentRefNum = App.currentPinRefNum;
        private Page previousPage;
        private DataPoint currentPoint;
        private bool imageAccess = false;
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

        public DetailPage(Page previousPage, DataPoint currentPoint)
        {
            NavigationPage.SetBackButtonTitle(this, "");
            this.previousPage = previousPage;
            this.currentPoint = currentPoint;

            InitializeComponent();

            if (currentPoint.IsFavorited)
            {
                favoriteButton.Source = "bluehearticon.png";
            }
            NavigationPage.SetTitleView(this, favoriteButton);
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
                Console.WriteLine(e.Message);
            }
        }

        private async void MainPageButton(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(false);
        }

        private async void FavoritePageButton(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(false);

            var page = new FavoritesPage();
            if (!previousPage.GetType().Equals(page.GetType()))
            {
                await Navigation.PushModalAsync(new FavoritesPage(), false);
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


    }
}