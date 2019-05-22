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
    public partial class FavoritesPage : ContentPage
    {
        private List<FavoritePoint> favoritePoints;
        private View title = new Label
        {
            Text = "Favorites",
            TranslationX = 100
        };
        public FavoritesPage(List<FavoritePoint> favorites)
        {
            InitializeComponent();
            
            NavigationPage.SetTitleView(this, title);

            favoritePoints = favorites;
            favoritesListView.ItemsSource = favorites;
        }
    }
}