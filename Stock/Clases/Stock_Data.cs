using System;
using System.Collections.ObjectModel;
using System.Data;

namespace Stock.Clases
{
    public class Stock_Data : Notificable
    {
        readonly c_Base cbase = new c_Base();
        public Stock datos = new Stock();

        private ObservableCollection<Stock> stocks;
        private Stock registro_Seleccionado;

        public Stock_Data()
        {
            Stocks = new ObservableCollection<Stock>();

            datos.Fecha = new DateTime(2021, 9, 26);
            datos.Sucursal = 1;

            DataTable dt = Stock_Carne();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var dato = new Stock();

                dato.Producto = Convert.ToInt32(dt.Rows[i]["Prod"]);
                dato.Descripcion = dt.Rows[i]["Nombre"].ToString();
                dato.Kilos = Convert.ToSingle(dt.Rows[i]["Kilos"]);


                stocks.Add(dato);
            }

            //Nos subscribimos al "Centro de Mensajes" para poder estar al tanto de las encuestas nuevas que se vayan agregando.
            //MessagingCenter.Subscribe<ContentPage, Encuesta>(this, Mensajes.NuevaEncuestaCompleta, (sender, args) => { encuestas.Add(args); });
        }

        public ObservableCollection<Stock> Stocks
        {
            get => stocks; set
            {
                if (value == stocks) { return; }
                stocks = value;
                OnPropertyChanged();
            }
        }

        public Stock Registro_Seleccionado
        {
            get => registro_Seleccionado; set
            {
                if(value == registro_Seleccionado) { return; }
                registro_Seleccionado = value;
                OnPropertyChanged();
            }
        }
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
