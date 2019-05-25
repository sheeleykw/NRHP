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
    public partial class DetailPage : ContentPage
    {
        private DataPoint currentPoint;

        public DetailPage()
        {
            InitializeComponent();
            SetupLabels();
        }

        private async void SetupLabels()
        {
            Console.WriteLine(App.currentPinRefNum);
            currentPoint = await App.itemDatabase.GetPointAsync(App.currentPinRefNum);
            name.Text = currentPoint.Name;
            category.Text = "Category: " + currentPoint.Category;
            sourceDate.Text = "Date added to register: " + currentPoint.SourceDate;
            address.Text = "Reported Street Address: " + currentPoint.Address;
            cityState.Text = "Location: " + currentPoint.City + ", " + currentPoint.State;
            county.Text = "County: " + currentPoint.County;
            people.Text = "Architects/Builders: " + currentPoint.Architects;
        }

        private void FavoriteItem(object sender, EventArgs e)
        {
            currentPoint.IsFavorited = !currentPoint.IsFavorited;
            App.itemDatabase.UpdatePoint(currentPoint);
        }

        private void BackButton(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void PhotoButton(object sender, EventArgs e)
        {
            var pdfUri = new Uri("https://npgallery.nps.gov/pdfhost/docs/NRHP/Photos/" + App.currentPinRefNum + ".pdf");
            Device.OpenUri(pdfUri);
        }

        private void DocButton(object sender, EventArgs e)
        {
            var docUri = new Uri("https://npgallery.nps.gov/pdfhost/docs/NRHP/Text/" + App.currentPinRefNum + ".pdf");
            Device.OpenUri(docUri);
        }
    }
}