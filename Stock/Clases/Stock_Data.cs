using Stock.Clases.Herramientas;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using Xamarin.Forms;

namespace Stock.Clases
{
    public class Stock_Data : Notificable
    {
        readonly c_Base cbase = new c_Base();

        public Stock datos = new Stock();
        private string nombre_Sucursal;
        private ObservableCollection<Stock> stocks;
        private Stock registro_Seleccionado;

        public Stock_Data()
        {
            Stocks = new ObservableCollection<Stock>();


            datos.Fecha = new DateTime(2021, 10, 18);

            //Falta implementar la asignacion de suc en una pantalla de login
            datos.Sucursal = 1;

            var ns = cbase.Dato_Generico("SELECT Nombre FROM Sucursales WHERE Id=" + datos.Sucursal);
            nombre_Sucursal = $"{datos.Sucursal}. {ns}";

            DataTable dt = Stock_Carne();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var dato = new Stock();

                dato.Producto = Convert.ToInt32(dt.Rows[i]["Prod"]);
                dato.Descripcion = dt.Rows[i]["Nombre"].ToString();
                dato.Kilos = Convert.ToSingle(dt.Rows[i]["Kilos"]);


                stocks.Add(dato);
            }


            // Aca guardamos el dato.
            MessagingCenter.Subscribe<Vistas.Stock_CarneView, Stock>(this, Literals.DatoModificado, (sender, args) =>
            {
                datos.Producto = args.Producto;
                datos.Descripcion = args.Descripcion;
                datos.Kilos = args.Kilos;
                
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

        public ObservableCollection<Stock> Stocks
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

        public string Nombre_Sucursal { get => nombre_Sucursal; set => nombre_Sucursal = value; }

        private DataTable Stock_Carne()
        {
            string s = $"SELECT P.Id Prod, P.Nombre" +
                $", ISNULL((SELECT S.Kilos FROM Stock S WHERE S.Fecha = '{datos.Fecha:MM/dd/yy}' AND S.Id_Sucursales = {datos.Sucursal} AND S.Id_Productos = P.Id), 0) Kilos" +
                $" FROM Productos P " +
                $" WHERE (P.ID_Tipo = 1) AND (P.Ver = 1)";
            DataTable dt = cbase.Datos_Genericos(s);
            return dt;

        }
    }


}
