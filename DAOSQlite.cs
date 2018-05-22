using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//-----------------------------
using System.Data.SQLite;

namespace Catalogo2018
{
    class DAOSQlite : IDAO
    {
        public SQLiteConnection conexion;


        public bool Conectar(string srv, string port, string db, string user, string pwd)
        {
            string cadenaConexion =
                //string.Format("server={0};port={1};database={2};uid={3};pwd={4};", srv, port, db, user, pwd);
                string.Format("Data Source={0};Version=3;FailIfMissing=True", db);
            //instanciamos un objeto SQLiteConnection

            try
            {
                conexion = new SQLiteConnection(cadenaConexion);
                conexion.Open(); // Abrir la sesion SQLite.
                return true;
            }
            catch (SQLiteException e)
            {
                throw new Exception("Error de conexión" + e.Data);
            }
        }

        public void Desconectar()
        {
            try
            {
                conexion.Close();
            }
            catch (SQLiteException)
            {
                throw;
            }
        }

        public bool Conectado()
        {
            if (conexion != null)
            {
                return conexion.State == ConnectionState.Open;
            }
            else
                return false;
        }


        public List<Dvd> Seleccionar(string codigo)
        {
            // "select codigo, titulo, artista, pais, compania, precio, anio from dvd where codigo = '1030'";
            List<Dvd> resultado = new List<Dvd>(); // recoge el resultado del select
            string orden;
            if (codigo == null)
                orden = "select codigo, titulo, artista, pais, compania, precio, anio from dvd";
            else
                orden = String.Format("select codigo, titulo, artista, pais, compania, precio, anio from dvd where codigo = {0}", codigo);

            SQLiteCommand cmd = new SQLiteCommand(orden, conexion);
            SQLiteDataReader lector = null;
            try
            {
                lector = cmd.ExecuteReader();
                while (lector.Read())
                {
                    Dvd undvd = new Dvd();
                    undvd.Codigo = short.Parse(lector["codigo"].ToString());
                    undvd.Titulo = lector["titulo"].ToString();
                    undvd.Artista = lector["artista"].ToString();
                    undvd.Pais = lector["pais"].ToString();
                    undvd.Compania = lector["compania"].ToString();
                    undvd.Precio = double.Parse(lector["precio"].ToString());
                    undvd.Anio = lector["anio"].ToString();

                    resultado.Add(undvd);
                }
                return resultado;
            }
            catch (SQLiteException)
            {
                throw new Exception("No tiene permisos para ejecutar esta orden");
            }
            finally
            {
                if (lector != null)
                    lector.Close();
            }
        }

        public DataTable SeleccionarTB(string codigo)
        {
            DataTable dt = new DataTable();
            string orden;
            if (codigo == null)
                orden = "select codigo, titulo, artista, pais, compania precio, anio from dvd";
            else
                orden = String.Format("select codigo, titulo, artista, pais, comapnia, precio, anio from dvd where codigo = {0}", codigo);
            SQLiteCommand cmd = new SQLiteCommand(orden, conexion);
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            da.Fill(dt);

            return dt;
        }


        public List<Dvd> SeleccionarPA(string codigo, out int resul)
        {
            throw new Exception("No implementado");
        }

        public int Borrar(string codigo)
        {
            string orden;

            if (codigo != null)
            {
                orden = string.Format("delete from dvd where codigo = '{0}'", codigo);
                SQLiteCommand cmd = new SQLiteCommand(orden, conexion);
                return cmd.ExecuteNonQuery();
            }
            else
                return -1;

        }

        public Pais SeleccionarPais(string iso2)
        {
            Pais resultado = new Pais();
            string orden;
            orden = string.Format("select nombre from pais where iso2 = '{0}'", iso2);
            SQLiteCommand cmd = new SQLiteCommand(orden, conexion);

            object salida = cmd.ExecuteScalar();
            if (salida != null)
            {
                resultado.Iso2 = iso2;
                resultado.Nombre = salida.ToString();
            }
            return resultado;
        }

        public int Actualizar(Dvd unDVD)
        {
            string orden;
            if (unDVD != null)
            {
                orden = string.Format("update dvd set titulo= '{0}', artista='{1}', pais='{2}', compania='{3}', precio='{4}', anio='{5}' where codigo = '{6}'",
                                                       unDVD.Titulo, unDVD.Artista, unDVD.Pais, unDVD.Compania, unDVD.Precio, unDVD.Anio, unDVD.Codigo);
                SQLiteCommand cmd = new SQLiteCommand(orden, conexion);
                return cmd.ExecuteNonQuery();
            }
            else
                return -1;
        }

        public int Insertar(Dvd unDVD)
        {
            string orden;

            if (unDVD != null)
            {
                orden = String.Format("insert into dvd (codigo,titulo,artista,pais,compania,precio,anio) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')",
                    unDVD.Codigo, unDVD.Titulo, unDVD.Artista, unDVD.Pais, unDVD.Compania, unDVD.Precio, unDVD.Anio);

                SQLiteCommand cmd = new SQLiteCommand(orden, conexion);
                return cmd.ExecuteNonQuery();
            }
            else
                return -1;
        }
    }
}
