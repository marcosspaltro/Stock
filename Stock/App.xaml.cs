using Stock.Vistas;
using System.Globalization;
using System.Threading;
using Xamarin.Forms;

namespace Stock
{
    public partial class App : Application
    {
        public App()
        {
            Xamarin.Forms.DataGrid.DataGridComponent.Init();

            InitializeComponent();

            MainPage = new Stock_CarneView();


        }

        protected override void OnStart()
        {
            var userSelectedCulture = new CultureInfo("es-AR");
            Thread.CurrentThread.CurrentCulture = userSelectedCulture;
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
