using NRHP_App.iOS;
using NRHP_App;
using Xamarin.Forms;
using Xamarin.Forms.Maps.iOS;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using MapKit;
using System;
using System.Threading.Tasks;
using CoreGraphics;

[assembly: ExportRenderer(typeof(NRHPMap), typeof(IOSMapRenderer))]
namespace NRHP_App.iOS
{
    public class IOSMapRenderer : MapRenderer
    {
        MKMapView nativeMap;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                nativeMap = Control as MKMapView;
                if (nativeMap != null)
                {
                    //nativeMap.RemoveAnnotations(nativeMap.Annotations);
                    //nativeMap.GetViewForAnnotation = null;
                    //nativeMap.CalloutAccessoryControlTapped -= OnCalloutAccessoryControlTapped;
                    //nativeMap.DidSelectAnnotationView -= OnDidSelectAnnotationView;
                    //nativeMap.DidDeselectAnnotationView -= OnDidDeselectAnnotationView;
                }
            }

            if (e.NewElement != null)
            {
                var formsMap = (NRHPMap)e.NewElement;
                nativeMap = Control as MKMapView;

                nativeMap.DidSelectAnnotationView += SelectPoint;
                nativeMap.DidDeselectAnnotationView += DeselectPoint;
                App.mainPage.SearchCompleted += LoadAnnotationElement;

                //nativeMap.SelectedAnnotation;
                //nativeMap.GetViewForAnnotation = GetViewForAnnotation;
            }
        }

        private async void LoadAnnotationElement(object sender, MapPoint e)
        {
            var markerFound = false;

            if (e != null)
            {
                while (!markerFound)
                {
                    foreach (IMKAnnotation annotation in nativeMap.Annotations)
                    {
                        if (annotation.GetTitle().Equals(e.Name))
                        {
                            nativeMap.SelectAnnotation(annotation, true);
                            markerFound = true;

                            App.currentPinRefNum = (await App.mapDatabase.GetRefNumAsync(annotation.GetTitle(), annotation.Coordinate.Latitude, annotation.Coordinate.Longitude)).RefNum;
                            App.mainPage.SwitchDetailPageButton();
                        }
                    }
                    await Task.Delay(150);
                }
            }
        }

        private async void SelectPoint(object sender, MKAnnotationViewEventArgs e)
        {
            var annotation = e.View.Annotation;
            if (annotation is MKPointAnnotation)
            {
                var moveCamera = new MKMapCamera
                {
                    CenterCoordinate = annotation.Coordinate,
                    Altitude = nativeMap.Camera.Altitude,
                    Heading = nativeMap.Camera.Heading
                };
                nativeMap.SetCamera(moveCamera, true);
                App.currentPinRefNum = (await App.mapDatabase.GetRefNumAsync(annotation.GetTitle(), annotation.Coordinate.Latitude, annotation.Coordinate.Longitude)).RefNum;
                App.mainPage.SwitchDetailPageButton();
            }
        }

        private void DeselectPoint(object sender, MKAnnotationViewEventArgs e)
        {
            App.currentPinRefNum = null;
            App.mainPage.SwitchDetailPageButton();
        }

        protected override MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            var annotationView = base.GetViewForAnnotation(mapView, annotation);
            if (annotationView != null)
            {
                //annotationView.Bounds = new CGRect(0, 0, 200, 200);
                //annotationView.DetailCalloutAccessoryView.Select(this);
            }
            return annotationView;
        }
    }
}