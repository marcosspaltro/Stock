namespace Stock.Clases
{
    using System;

    public class Stock
    {

        public DateTime Fecha { get; set; }
        public int Sucursal { get; set; }
        public int Producto { get; set; }
        public string Descripcion { get; set; }

        public float Kilos { get; set; }

        public override string ToString()
        {
            return Descripcion;
        }
    }
}
