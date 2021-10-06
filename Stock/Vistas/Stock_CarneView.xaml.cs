using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Stock.Clases;

namespace Stock.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Stock_CarneView : ContentPage
    {

        public Stock_CarneView()
        {
            InitializeComponent();

        }

        private void lstItemsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            
        }
    }
}