using System;
using Xamarin.Forms;

namespace NRHP_App
{
    public partial class WebView : ContentPage
    {
        public WebView(string link)
        {
            InitializeComponent();
            webView.Source = link;
        }
    }
}
