using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NRHP_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : MasterDetailPage
    {
        public MasterPage(ContentPage mainPage)
        {
            InitializeComponent();
            Detail = mainPage;
        }
    }
}