using System;
using NRHP_App;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PdfViewer), typeof(WebViewRenderer))]
namespace NRHP_App.iOS
{
    public class PdfViewerRenderer : ViewRenderer<PdfViewer, UIWebView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<PdfViewer> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Control.BackgroundColor = UIColor.Black;
            }
        }
    }
}
