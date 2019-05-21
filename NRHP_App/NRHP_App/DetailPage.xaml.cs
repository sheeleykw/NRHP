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
        public string imageUri;

		public DetailPage (string imageUri)
		{
			InitializeComponent();
            this.imageUri = imageUri;
            var image = new WebView { Source = "https://npgallery.nps.gov/pdfhost/docs/NRHP/Photos/80003972.pdf" };
            Content = image;
		}
	}
}