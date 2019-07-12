using System;
using NRHP_App;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PdfViewer), typeof(WebViewRenderer))]
namespace NRHP_App.iOS
{
    public class PdfViewerRenderer : WebViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if(e.NewElement != null)
            {
                var webView = (UIWebView)NativeView;
                webView.ScrollView.ScrollEnabled = true;
                webView.ScalesPageToFit = true;
                //webView.ScrollView.zoom
            }
            
        }
    }
}
