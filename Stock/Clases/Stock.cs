namespace Stock.Clases
{
    using System;

    public class Stock
    {
        private float kilos;

        public DateTime Fecha { get; set; }
        public int Producto { get; set; }
        public int Sucursal { get; set; }
        public string Descripcion { get; set; }
        public int Tipo { get; set; }
        public void Asignar_Tipo(string stockt = "")
        {
            c_Base cbase = new c_Base();
            if (stockt == "")
            {
                Tipo = Convert.ToInt32(cbase.Dato_Generico($"SELECT Id_Tipo FROM Productos WHERE Id= {Producto}"));
            }
            else
            { 
            Tipo = Convert.ToInt32(cbase.Dato_Generico($"SELECT Id FROM TipoProductos WHERE Nombre= '{stockt}'"));
            }
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
