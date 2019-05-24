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

        private void BackButton(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}