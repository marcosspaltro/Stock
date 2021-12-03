using Stock.Vistas;
using System.Globalization;
using System.Threading;
using Xamarin.Forms;
using Stock.Clases;
using System.Data;
using System;

namespace Stock
{
    public partial class App : Application
    {
        readonly c_Base cbase = new c_Base();

        public Clases.Stock datos = new Clases.Stock();

        public App()
        {
            Xamarin.Forms.DataGrid.DataGridComponent.Init();

            InitializeComponent();


        var userSelectedCulture = new CultureInfo("es-AR");
            Thread.CurrentThread.CurrentCulture = userSelectedCulture;
            //dejar como estaba antes y poner un msj para ver el error
            CarouselPage carouselPage = new CarouselPage();
            DataTable dtt = cbase.Datos_Genericos("SELECT Id, Nombre FROM TipoProductos");
            Random rd = new Random();
            for (int j = 0; j < dtt.Rows.Count; j++)
            {
                Resources.Remove("Carne");
                Resources.Add("Carne", Color.FromRgb(rd.Next(256), rd.Next(256), rd.Next(256)));
                Resources.Remove("stockicon");
                Resources.Add("stockicon", $"it{Convert.ToInt32(dtt.Rows[j]["Id"])}.jpg");
                Resources.Remove("Eltipo");
                Resources.Add("Eltipo", Convert.ToInt32(dtt.Rows[j]["Id"]));
                Resources.Remove("Nomtipo");
                Resources.Add("Nomtipo", "Stock " + dtt.Rows[j]["Nombre"].ToString());
                carouselPage.Children.Add(new vw_Stock());
            }
            MainPage = new NavigationPage(carouselPage);
            //var userSelectedCulture = new CultureInfo("es-AR");
            //Thread.CurrentThread.CurrentCulture = userSelectedCulture;
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
