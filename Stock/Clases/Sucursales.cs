
namespace Stock.Clases
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    
    public class Sucursales : c_Base
    {
        public Sucursales()
        {
            Tabla = "Sucursales";
            Vista = "vw_Sucursales";
        }

        public enum Filtrar_SucsClientes { Todas = 0, Sucursales, Clientes };

        public TipoSucursales Tipo { get; set; } = new TipoSucursales();

        public bool Ver { get; set; }

        public bool Propio { get; set; }

        public string Titular { get; set; }

        public string CUIT { get; set; }

        public string Direccion { get; set; }

        public string Alias { get; set; }

        
        public string Balanza { get; set; }


        public Filtrar_SucsClientes Filtro_SucCliente { get; set; }

        public bool Mostrar_Ocultos { get; set; } = false;
        public bool Ordern_XId { get; set; } = true;

        public new DataTable Datos(string filtro = "")
        {
            Herramientas.Herramientas h = new Herramientas.Herramientas();

            if (Mostrar_Ocultos == false)
            {
                filtro = h.Unir(filtro, " (Ver=1) ");
            }
            switch (Filtro_SucCliente)
            {
                case Filtrar_SucsClientes.Clientes:
                    filtro = h.Unir(filtro, " (Propio=0) ");
                    break;
                case Filtrar_SucsClientes.Sucursales:
                    filtro = h.Unir(filtro, " (Propio=1) ");
                    break;
            }
            
            return Datos_Vista(filtro, "*", Ordern_XId ? "ID" : "Nombre");

        }

        public void Siguiente()
        {

            var dt = new DataTable("Datos");
            var conexionSql = new SqlConnection(Sistema.dbDatosConnectionString);


            try
            {
                SqlCommand comandoSql = new SqlCommand($"SELECT TOP 1 * FROM Sucursales WHERE Id>{ID} AND Ver = 1 AND Propio = 1 ORDER BY Id", conexionSql);
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
                    comandoSql.CommandText = ($"SELECT TOP 1 * FROM Sucursales ORDER BY Id");
                    comandoSql.CommandType = CommandType.Text;

                    SqlDat.Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        ID = 0;
                    }
                    else
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

        private void Asignar(DataRow dr)
        {
            ID = Convert.ToInt32(dr["Id"]);
            Nombre = dr["Nombre"].ToString();
            Tipo.ID = Convert.ToInt32(dr["Tipo"]);
            Ver = Convert.ToBoolean(dr["Ver"]);
            Titular = dr["Titular"].ToString();
            Direccion = dr["Direccion"].ToString();
            Alias = dr["Alias"].ToString();
            CUIT = dr["CUIT"].ToString();
            Balanza = dr["Balanza"].ToString();
            
        }

        public new void Actualizar()
        {
            var sql = new SqlConnection(Sistema.dbDatosConnectionString);

            try
            {
                SqlCommand command =
                    new SqlCommand($"UPDATE Sucursales SET Nombre='{Nombre}', Tipo={Tipo.ID}, Ver={(Ver ? "1" : "0")}, Propio={(Propio ? "1" : "0")}, Titular='{Titular}', CUIT='{CUIT}', Direccion='{Direccion}'" +
                    $",Alias='{Alias}', Balanza='{Balanza}' WHERE Id={ID}", sql);
                command.CommandType = CommandType.Text;
                command.Connection = sql;
                sql.Open();

                var d = command.ExecuteNonQuery();

                sql.Close();
            }
            catch (Exception e)
            {
                
            }
        }

        public new void Agregar()
        {
            var sql = new SqlConnection(Sistema.dbDatosConnectionString);

            try
            {
                string vver = Ver ? "1" : "0";
                string vpropio = Propio ? "1" : "0";

                SqlCommand command =
                    new SqlCommand($"INSERT INTO Sucursales (Id, Nombre, Tipo, Ver, Propio, Titular, CUIT, Direccion, Alias, Balanza) VALUES({ID}, '{Nombre}', {Tipo.ID}, {vver}, " +
                    $"{vpropio}, '{Titular}', '{CUIT}', '{Direccion}', '{Alias}', '{Balanza}')", sql);
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
