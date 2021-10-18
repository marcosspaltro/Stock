namespace Stock.Clases
{
    using global::Stock.Clases.Herramientas;
    using System;

    public class Stock
    {
        private float kilos;

        public DateTime Fecha { get; set; }
        public int Sucursal { get; set; }
        public int Producto { get; set; }
        public string Descripcion { get; set; }

        public float Kilos
        {
            get => kilos; set
            {
                if(value == kilos) { return; }
                kilos = value;
            }
        }

        public override string ToString()
        {
            return $"{Producto}. {Descripcion}";
        }
    }
}
