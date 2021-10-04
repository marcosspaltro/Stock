namespace Stock.Clases
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    
    public class Producto : c_Base
    {
        public Producto()
        {
            Tabla = "Productos";
            Vista = "vw_Productos";
        }
                
        public TipoProducto Tipo { get; set; } = new TipoProducto();

        public bool Ver { get; set; }

        public bool Imprimir { get; set; }

        public bool Pesable { get; set; }

        public int Multiplicador { get; set; }


        public bool Mostrar_Ocultos { get; set; } = false;

        public bool Filtrar_Ver { get; set; } = true;

        public bool Ordern_XId { get; set; } = true;
               

        public new DataTable Datos(string filtro = "")
        {
            return Datos_Vista(filtro, " Id, Id_Tipo, Descripcion, Nombre, Ver, Imprimir, Pesable, Multiplicador ");
        }

        public void Siguiente(string Filtro = "")
        {
            var dt = new DataTable("Datos");
            var conexionSql = new SqlConnection(Sistema.dbDatosConnectionString);

            if (Filtro != "")
            {
                Filtro = " AND " + Filtro;
            }
            string s = $"SELECT TOP 1 * FROM vw_Productos WHERE Id>{ID} {Filtro} ORDER BY Id";
           
            try
            {
                SqlCommand comandoSql = new SqlCommand(s, conexionSql);
                comandoSql.CommandType = CommandType.Text;

                SqlDataAdapter SqlDat = new SqlDataAdapter(comandoSql);
                SqlDat.Fill(dt);
                DataRow dr;

                if (dt.Rows.Count > 0)
                {
                    dr = dt.Rows[0];
                    Asignar(dr);
                }
                else
                {
                    if (Filtro != "")
                    {
                        Filtro = Filtro.Replace(" AND ", " WHERE ");
                    }
                    comandoSql.CommandText = ($"SELECT TOP 1 * FROM vw_Productos {Filtro} ORDER BY Id");
                    comandoSql.CommandType = CommandType.Text;

                    SqlDat.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        dr = dt.Rows[0];
                        Asignar(dr);
                    }
                }
            }
            catch (Exception)
            {
                ID = 0;
            }
        }

        public void Buscar()
        {
            DataTable dr = Datos("ID=" + ID);
            Asignar(dr.Rows[0]);
        }

        private void Asignar(DataRow dr)
        {
            ID = Convert.ToInt32(dr["Id"]);
            Nombre = dr["Nombre"].ToString();
            Tipo.ID = Convert.ToInt32(dr["Id_Tipo"]);
            Ver = Convert.ToBoolean(dr["Ver"]);
            Imprimir = Convert.ToBoolean(dr["Imprimir"]);
            Pesable = Convert.ToBoolean(dr["Pesable"]);
            Multiplicador = Convert.ToInt32(dr["Multiplicador"]);
        }

        public new void Actualizar()
        {
            var sql = new SqlConnection(Sistema.dbDatosConnectionString);

            try
            {
                string vver = Ver ? "1" : "0";
                string vimp = Imprimir ? "1" : "0";
                string vpesa = Pesable ? "1" : "0";

                SqlCommand command =
                    new SqlCommand($"UPDATE Productos SET Nombre='{Nombre}', Id_Tipo={Tipo.ID}, Ver={vver}, Imprimir={vimp}, Pesable={vpesa}, Multiplicador={Multiplicador} WHERE Id={ID}", sql);
                command.CommandType = CommandType.Text;
                command.Connection = sql;
                sql.Open();

                var d = command.ExecuteNonQuery();

                sql.Close();
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Error");
            }
        }

        public new void Agregar()
        {
            var sql = new SqlConnection(Sistema.dbDatosConnectionString);

            try
            {
                string vver = Ver ? "1" : "0";
                string vimp = Imprimir ? "1" : "0";
                string vpesa = Pesable ? "1" : "0";

                SqlCommand command =
                    new SqlCommand($"INSERT INTO Productos (Id, Nombre, Id_Tipo, Ver, Imprimir, Pesable, Multiplicador) VALUES({ID}, '{Nombre}', {Tipo.ID}, {vver}, " +
                    $"{vimp}, {vpesa}, {Multiplicador})", sql);
                command.CommandType = CommandType.Text;
                command.Connection = sql;
                sql.Open();

                var d = command.ExecuteNonQuery();

                sql.Close();
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Error");
            }
        }
            
    }
}
