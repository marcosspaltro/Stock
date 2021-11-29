using Stock.Clases.Herramientas;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net;
using Xamarin.Forms;

namespace Stock.Clases
{
    public class Stock_DatosPagina : Notificable
    {
        private int tipo;

        readonly c_Base cbase = new c_Base();

        public Stock datos = new Stock();
        private string Titulo;
        private ObservableCollection<Stock> stocks;
        private Stock registro_Seleccionado;

        public Stock_DatosPagina()
        {
        }

        public void cargar()
        {
            Stock = new ObservableCollection<Stock>();
            //Se setea la fecha al domingo de la semana anterior. 
            double d = Convert.ToDouble(DateTime.Today.DayOfWeek);

            //Chequear que no sea mayor a martes.
            if (d > 2)
            {

            }
            datos.Fecha = DateTime.Today.AddDays(d * -1);

            var IpAddress = Dns.GetHostAddresses(Dns.GetHostName()).FirstOrDefault();

            //Chequear que haya encontrado la ip
            var ns = cbase.Dato_Generico($"SELECT Id FROM Sucursales WHERE Ip='{IpAddress}'");
            datos.Sucursal = Convert.ToInt32(ns);

            ns = cbase.Dato_Generico("SELECT Nombre FROM Sucursales WHERE Id=" + datos.Sucursal);
            Titulo = $"{datos.Sucursal}. {ns}  -  {datos.Fecha.AddDays(1):dd/MM}";

            DataTable dt = Stock_tipo(tipo);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var dato = new Stock();

                dato.Producto = Convert.ToInt32(dt.Rows[i]["Prod"]);
                dato.Descripcion = dt.Rows[i]["Nombre"].ToString();
                dato.Kilos = Convert.ToSingle(dt.Rows[i]["Kilos"]);

                stocks.Add(dato);
            }


            // Aca guardamos el dato.
            MessagingCenter.Subscribe<Vistas.vw_Stock, Stock>(this, Literals.DatoModificado, async(sender, args) =>
            {
                if (DateTime.Today <= datos.Fecha.AddDays(-2))
                {
                    datos.Producto = args.Producto;
                    datos.Descripcion = args.Descripcion;
                    datos.Kilos = args.Kilos;

                    Actualizar_Stock();
                }
                else
                {
                    if (args.Tipo == tipo)
                    { 
                    await Application.Current.MainPage.DisplayAlert("Carga Invalida", "Ya no es posible Editar el Stock", "OK");
                        stocks.Clear();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            var dato = new Stock();
                            dato.Tipo = tipo;
                            dato.Producto = Convert.ToInt32(dt.Rows[i]["Prod"]);
                            dato.Descripcion = dt.Rows[i]["Nombre"].ToString();
                            dato.Kilos = Convert.ToSingle(dt.Rows[i]["Kilos"]);

                            stocks.Add(dato);
                        }
                    }
                }
            });

            // Ponemos el dato en cero.
            MessagingCenter.Subscribe<Vistas.vw_Stock, Stock>(this, Literals.BotonBorrar, (sender, args) =>
            {
                datos.Producto = args.Producto;
                datos.Descripcion = args.Descripcion;
                datos.Kilos = 0;

                Actualizar_Stock();
            });
        }

        private void Actualizar_Stock()
        {
            string cm = $"DELETE FROM Stock WHERE Fecha='{datos.Fecha:MM/dd/yyyy}' AND ID_Sucursales={datos.Sucursal} AND ID_Productos={datos.Producto}";
            cbase.Ejecutar_Comando(cm);

            cm = $"INSERT INTO Stock (Fecha, ID_Sucursales, ID_Productos, Descripcion, Kilos) VALUES(" +
                $"'{datos.Fecha:MM/dd/yyyy}', {datos.Sucursal}, {datos.Producto}, '{datos.Descripcion}', {datos.Kilos.ToString().Replace(",", ".")}" +
                $")";
            cbase.Ejecutar_Comando(cm);
        }

        private void PropiedadCambiada(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Kilos")
            {
                registro_Seleccionado.Kilos = 0;
            }
        }

        public ObservableCollection<Stock> Stock
        {
            get => stocks; set
            {
                if (value == stocks) { return; }
                stocks = value;

            }
        }
        public Stock Registro_Seleccionado
        {
            get => registro_Seleccionado; set
            {
                if (value == registro_Seleccionado) { return; }
                registro_Seleccionado = value;

            }
        }

        public string Nombre_Sucursal { get => Titulo; set => Titulo = value; }
        public int Tipo{ get => tipo; set {  tipo = value;  cargar(); } }


        private DataTable Stock_tipo(int tipo = 0)
        {
            string s = $"SELECT P.Id Prod, P.Nombre" +
                $", ISNULL((SELECT S.Kilos FROM Stock S WHERE S.Fecha = '{datos.Fecha:MM/dd/yy}' AND S.Id_Sucursales = {datos.Sucursal} AND S.Id_Productos = P.Id), 0) Kilos" +
                $" FROM Productos P " +
                $" WHERE (P.ID_Tipo = {tipo}) AND (P.Ver = 1)";
            DataTable dt = cbase.Datos_Genericos(s);
            return dt;

        }

    }


}
