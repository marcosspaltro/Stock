namespace Stock.Clases
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    

    public class Precios_Sucursales : c_Base
    {
        public Precios_Sucursales()
        {
            Tabla = "Precios_Sucursales";
            Vista = "vw_PreciosSucursales";
        }


        
        public DateTime Fecha { get; set; }
        public Producto Producto { get; set; } = new Producto();
        public Sucursales Sucursal { get; set; } = new Sucursales();
        public float Precio { get; set; }


        #region " Devolver Datos "
        public float Buscar()
        {
            if (Producto.ID == 399)
            {
                Precio = 1;
            }
            else
            {
                var conexionSql = new SqlConnection(Sistema.dbDatosConnectionString);
                object d = null;

                //Si no hay una sucursal seleccionada se devuelve el primer precio encontrado
                string suc = $"(SELECT TOP 1 Id_Sucursales FROM Precios_Sucursales WHERE Fecha<='{Fecha.ToString("MM/dd/yyy")}'" +
                        $" AND Id_Productos={Producto.ID} ORDER BY Fecha DESC)";
                if (Sucursal.ID > 0) { suc = Sucursal.ID.ToString(); }
                try
                {

                    SqlCommand comandoSql = new SqlCommand($"SELECT TOP 1 Precio FROM Precios_Sucursales  WHERE Fecha<='{Fecha.ToString("MM/dd/yyy")}'" +
                        $" AND Id_Productos={Producto.ID} AND ID_Sucursales={suc} ORDER BY Fecha DESC", conexionSql);

                    conexionSql.Open();

                    comandoSql.CommandType = CommandType.Text;
                    d = comandoSql.ExecuteScalar();

                    conexionSql.Close();
                }
                catch (Exception)
                {
                    d = 0;
                }
                Precio = Convert.ToSingle(d);
            }
            return Precio;
        }
             

        public DataTable Tabla_Precios(int tipo)
        {
            var dt = new DataTable("Datos");
            var conexionSql = new SqlConnection(Sistema.dbDatosConnectionString);

            try
            {
                SqlCommand comandoSql = new SqlCommand($"SELECT Id, Nombre, 0.0 Precio FROM Productos WHERE Id_Tipo={tipo} AND Ver=1 ORDER BY Id", conexionSql);
                comandoSql.CommandType = CommandType.Text;

                SqlDataAdapter SqlDat = new SqlDataAdapter(comandoSql);
                SqlDat.Fill(dt);

            }
            catch (Exception)
            {
                dt = null;
            }

            return dt;
        }
        public DataTable Tabla_Precios(string filtro)
        {
            var dt = new DataTable("Datos");
            var conexionSql = new SqlConnection(Sistema.dbDatosConnectionString);

            try
            {
                SqlCommand comandoSql = new SqlCommand($"SELECT Id, Nombre, 0.0 Precio FROM Productos WHERE {filtro} AND Ver=1 ORDER BY Id", conexionSql);
                comandoSql.CommandType = CommandType.Text;

                SqlDataAdapter SqlDat = new SqlDataAdapter(comandoSql);
                SqlDat.Fill(dt);

            }
            catch (Exception)
            {
                dt = null;
            }

            return dt;
        }
        public DataTable Precios(byte tipo)
        {
            var dt = new DataTable("Datos");
            var conexionSql = new SqlConnection(Sistema.dbDatosConnectionString);

            try
            {
                string fecha = Dato_Generico("vw_PreciosSucursales", "Id_Tipo=" + tipo, "MAX(Fecha)").ToString();

                if (Fecha != null) { fecha = $"'{Fecha:MM/dd/yy}'"; }
                if(Sucursal.ID != 0)
                {
                    SqlCommand comandoSql = new SqlCommand($"SELECT Id, Nombre, dbo.f_Precio({fecha}, {Sucursal.ID}, Id) Precio FROM Productos WHERE Id_Tipo={tipo} AND Ver=1 ORDER BY Id", conexionSql);
                    comandoSql.CommandType = CommandType.Text;

                    SqlDataAdapter SqlDat = new SqlDataAdapter(comandoSql);
                    SqlDat.Fill(dt);
                }

            }
            catch (Exception)
            {
                dt = null;
            }

            return dt;
        }
        public DataTable Precios(string filtro)
        {
            var dt = new DataTable("Datos");
            var conexionSql = new SqlConnection(Sistema.dbDatosConnectionString);

            try
            {
                string fecha = Dato_Generico("vw_PreciosSucursales", filtro, "MAX(Fecha)").ToString();
                
                if (Fecha != null) { fecha = $"'{Fecha:MM/dd/yy}'"; }
                if (Sucursal.ID != 0)
                {
                    SqlCommand comandoSql = new SqlCommand($"SELECT Id, Nombre, dbo.f_Precio({fecha}, {Sucursal.ID}, Id) Precio FROM Productos WHERE {filtro} AND Ver=1 ORDER BY Id", conexionSql);
                    comandoSql.CommandType = CommandType.Text;

                    SqlDataAdapter SqlDat = new SqlDataAdapter(comandoSql);
                    SqlDat.Fill(dt);
                }

            }
            catch (Exception)
            {
                dt = null;
            }

            return dt;
        }

        public DataTable Fechas(int tipo, int prod = 0, string filtroSuc = "", int top = 50)
        {
            var dt = new DataTable("Datos");
            var conexionSql = new SqlConnection(Sistema.dbDatosConnectionString);

            string filtroProd = "";
            if (prod != 0) { filtroProd = " AND ID_Productos=" + prod; }

            if (filtroSuc != "") { filtroSuc = $" ID_Sucursales IN({filtroSuc}) AND "; }

            try
            {
                string mostrarPrecio = "";
                if (prod == 1 | prod == 300) { mostrarPrecio = ", Precio"; }

                SqlCommand comandoSql = new SqlCommand($"SELECT TOP {top} Fecha{mostrarPrecio} FROM vw_PreciosSucursales WHERE {filtroSuc}Id_Tipo={tipo} {filtroProd} GROUP BY Fecha{mostrarPrecio} ORDER BY Fecha DESC", conexionSql);
                comandoSql.CommandType = CommandType.Text;

                SqlDataAdapter SqlDat = new SqlDataAdapter(comandoSql);
                SqlDat.Fill(dt);

            }
            catch (Exception)
            {
                dt = null;
            }

            return dt;
        }
        public DataTable Fechas_Granja(int top = 50)
        {
            var dt = new DataTable("Datos");
            var conexionSql = new SqlConnection(Sistema.dbDatosConnectionString);

            try
            {
                SqlCommand comandoSql = new SqlCommand($"SELECT TOP {top} Fecha, Precio FROM Precios_Sucursales " +
                    $"WHERE ID_Productos=300 " +
                    $"GROUP BY Fecha, Precio ORDER BY Fecha DESC", conexionSql);
                comandoSql.CommandType = CommandType.Text;

                SqlDataAdapter SqlDat = new SqlDataAdapter(comandoSql);
                SqlDat.Fill(dt);

            }
            catch (Exception)
            {
                dt = null;
            }

            return dt;
        }

        public DataTable Precios_CarneListaKilos()
        {
            var dt = new DataTable("Datos");
            var conexionSql = new SqlConnection(Sistema.dbDatosConnectionString);

            try
            {
                SqlCommand comandoSql = new SqlCommand("SELECT * FROM vw_Precios_CarneListaKilos ORDER BY ID_Productos", conexionSql);
                comandoSql.CommandType = CommandType.Text;

                SqlDataAdapter SqlDat = new SqlDataAdapter(comandoSql);
                SqlDat.Fill(dt);

            }
            catch (Exception)
            {
                dt = null;
            }

            return dt;
        }

        public DataTable Precios_Formulas()
        {
            var dt = new DataTable("Datos");
            var conexionSql = new SqlConnection(Sistema.dbDatosConnectionString);

            try
            {
                SqlCommand comandoSql = new SqlCommand("SELECT * FROM vw_Precios_Formulas ORDER BY ID_Productos", conexionSql);
                comandoSql.CommandType = CommandType.Text;

                SqlDataAdapter SqlDat = new SqlDataAdapter(comandoSql);
                SqlDat.Fill(dt);

            }
            catch (Exception)
            {
                dt = null;
            }

            return dt;
        }

        public DataTable Integraciones_Sucursales(DateTime fecha)
        {
            var dt = new DataTable("Datos");
            var conexionSql = new SqlConnection(Sistema.dbDatosConnectionString);

            try
            {
                SqlCommand comandoSql = new SqlCommand($"dbo.sp_IntegracionesSucursales '{fecha:MM/dd/yy}'", conexionSql);
                comandoSql.CommandType = CommandType.Text;

                SqlDataAdapter SqlDat = new SqlDataAdapter(comandoSql);
                SqlDat.Fill(dt);

            }
            catch (Exception)
            {
                dt = null;
            }

            return dt;
        }

        public DataTable Integraciones_Sucursales(bool Agrupado)
        {
            var dt = new DataTable("Datos");
            var conexionSql = new SqlConnection(Sistema.dbDatosConnectionString);

            try
            {
                SqlCommand comandoSql = new SqlCommand($"SELECT TOP 50 Fecha, Precio AS Integracion FROM Precios_Sucursales WHERE Id_Productos=1 GROUP BY Fecha, Precio ORDER BY Fecha DESC, Precio ", conexionSql);
                comandoSql.CommandType = CommandType.Text;

                SqlDataAdapter SqlDat = new SqlDataAdapter(comandoSql);
                SqlDat.Fill(dt);

            }
            catch (Exception)
            {
                dt = null;
            }

            return dt;
        }
        #endregion

        #region " Editar Datos "
        public new void  Actualizar()
        {
            var sql = new SqlConnection(Sistema.dbDatosConnectionString);

            try
            {
                SqlCommand command =
                    new SqlCommand($"UPDATE Precios_Sucursales " +
                    $"SET Fecha='{Fecha.ToString("MM/dd/yyy")}', Id_Sucursales={Sucursal.ID}, Id_Productos={Producto.ID}, Precio={Precio.ToString().Replace(",", ".")} " +
                    $"WHERE Id={ID}", sql);
                command.CommandType = CommandType.Text;
                command.Connection = sql;
                sql.Open();

                var d = command.ExecuteNonQuery();

                sql.Close();
            }
            catch (Exception)
            {
                //MessageBox.Show(e.Message, "Error");
            }
        }

        public new void Agregar()
        {
            var sql = new SqlConnection(Sistema.dbDatosConnectionString);

            try
            {
                SqlCommand command =
                    new SqlCommand($"DELETE FROM Precios_Sucursales WHERE Fecha='{Fecha:MM/dd/yyy}' AND Id_Sucursales={Sucursal.ID} AND Id_Productos={Producto.ID}", sql);
                command.CommandType = CommandType.Text;
                command.Connection = sql;
                sql.Open();

                var d = command.ExecuteNonQuery();

                command.CommandText = $"INSERT INTO Precios_Sucursales (Fecha, Id_Sucursales, Id_Productos, Precio) " +
                    $"VALUES('{Fecha.ToString("MM/dd/yyy")}', {Sucursal.ID}, {Producto.ID}, {Precio.ToString().Replace(",", ".")} )";

                d = command.ExecuteNonQuery();

                sql.Close();
            }
            catch (Exception)
            {
                //MessageBox.Show(e.Message, "Error");
            }
        }

        public new void Borrar()
        {
            var sql = new SqlConnection(Sistema.dbDatosConnectionString);

            try
            {
                SqlCommand command = new SqlCommand("DELETE FROM Precios_Sucursales WHERE Id=" + ID, sql);
                command.CommandType = CommandType.Text;
                command.Connection = sql;
                sql.Open();

                var d = command.ExecuteNonQuery();

                ID = 0;

                sql.Close();
            }
            catch (Exception)
            {
                //MessageBox.Show(e.Message, "Error");
            }
        }

        public void Borrar_Lista(int Tipo)
        {
            var sql = new SqlConnection(Sistema.dbDatosConnectionString);

            try
            {
                //Se pide el tipo para no borrar listas de distinto tipo. Ej: carne/men/emb
                SqlCommand command = new SqlCommand($"DELETE FROM Precios_Sucursales " +
                    $"WHERE ID IN(SELECT ID FROM vw_PreciosSucursales " +
                    $"WHERE ID_Sucursales={Sucursal.ID} AND Fecha='{Fecha:MM/dd/yyyy}' AND Id_Tipo={Tipo})", sql);
                command.CommandType = CommandType.Text;
                command.Connection = sql;
                sql.Open();

                var d = command.ExecuteNonQuery();

                ID = 0;

                sql.Close();
            }
            catch (Exception)
            {
                //MessageBox.Show(e.Message, "Error");
            }
        }
        #endregion


    }
}
