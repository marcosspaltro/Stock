namespace Stock.Clases
{
    using System;

    public class Stock
    {
        private float kilos;

        public DateTime Fecha { get; set; }
        public int Producto { get; set; }
        public int Sucursal { get; set; }
        public int Tipo { get; set; }
        public string Descripcion { get; set; }

        public void Asignar_Tipo()
        {
            c_Base cbase = new c_Base();
            Tipo = 1;
        }

        public float Kilos
        {
            get => kilos; set
            {
                if (value == kilos) { return; }
                kilos = value;
            }
        }

        public override string ToString()
        {
            return $"{Producto}. {Descripcion}";
        }
    }
}
