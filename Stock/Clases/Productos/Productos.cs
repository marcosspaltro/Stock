using System;
using System.Collections.Generic;
using System.Data;

namespace Stock.Clases
{
    public class Productos
    {
        public static List<Producto> GetProductos()
        {
            var data = new List<Producto>();

            Producto prods = new Producto();
            DataTable dt = prods.Datos_Vista("Ver=1 AND Id_Tipo=1", "Id, Nombre, 0.0 Kilos");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var producto = new Producto();
                producto.ID = Convert.ToInt32(dt.Rows[i]["Id"]);
                producto.Nombre = dt.Rows[i]["Nombre"].ToString();
                Random r = new Random();
                //producto.Kilos = r.Next(300);
                data.Add(producto);
            }

            return data;
        }
    }
}
