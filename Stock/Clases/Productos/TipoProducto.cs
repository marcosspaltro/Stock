
using Stock.Clases;

namespace Stock.Clases

{
    public class TipoProducto : c_Base
    {
        public TipoProducto()
        {
            Tabla = "TipoProductos";
            Vista = "TipoProductos";
        }

        public TipoProducto(int id, string nombre)
        {
            ID = id;
            Nombre = nombre;
        }

        public bool ResumirPorVenta { get; set; }
        public bool Tiene_Estadistica { get; set; }


    }
}
