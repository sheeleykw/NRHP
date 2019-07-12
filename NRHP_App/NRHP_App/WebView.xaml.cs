using Xamarin.Forms;

namespace NRHP_App
{
    public partial class WebView : ContentPage
    {
        public WebView(string link)
        {
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();
            webView.Source = link;
        }
    }
}
