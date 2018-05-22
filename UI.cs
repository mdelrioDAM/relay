using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogo2018
{
    class UI
    {
        static DAOMySQL dao;
        static string host = "localhost";
        static string bd = "catalogo";
        static string usr = "usr_catalogo";
        static string pwd = "123";
        static string port = "3306";

        public UI()
        {
            dao = new DAOMySQL();
            PedirOpcion();
        }
        static void Menu()
        {
            Console.WriteLine("CATALOGO de DVDs - Opciones");
            Console.WriteLine("===========================");
            Console.WriteLine("[0] Conectar a la BD");
            Console.WriteLine("[1] Desconectar a la BD");
            Console.WriteLine("[2] ¿Hay conexión?");
            Console.WriteLine("[3] Seleccionar toda la tabla");
            Console.WriteLine("[4] Seleccionar un código de DVD");
            Console.WriteLine("[5] Seleccionar toda la tabla con PA");
            Console.WriteLine("[6] Borrar un DVD de la tabla");
            Console.WriteLine("[7] Insertar un DVD en la tabla");
            Console.WriteLine("[S] Fin del pograma");
            Console.Write("¿Opción? ");

        }
        static void PedirOpcion()
        {
            ConsoleKeyInfo opcion;
            do
            {
                Menu();
                opcion = Console.ReadKey();
                Console.WriteLine();
                try
                {
                    switch ((char)opcion.Key)
                    {
                        case '0': // conexion a la BD
                            if (!dao.Conectado())
                            {
                                if (dao.Conectar(host, port, bd, usr, pwd))
                                    Console.WriteLine("Conexión con éxtito a la bd {0}", bd);
                            }
                            else
                                Console.WriteLine("Ya hay una conexión establecida");
                            break;

                        case '1': // desconexion
                            if (dao.Conectado())
                            {
                                dao.Desconectar();
                                Console.WriteLine("Desconexión con éxito de la bd {0}", bd);
                            }
                            else
                                Console.WriteLine("No hay conexión activa a la bd");
                            break;

                        case '2': // Comprobar si hay conexion
                            if (dao.Conectado())
                                Console.WriteLine("Conexión válida");
                            else
                                Console.WriteLine("No hay conexión");
                            break;

                        case '3': // Seleccionar generico toda la tabla
                            List<Dvd> listado = new List<Dvd>();
                            listado = dao.Seleccionar(null);
                            MostrarListado(listado);
                            break;

                        case '4': // Seleccionar generico un codigo de DVD
                            List<Dvd> listado2 = new List<Dvd>();
                            listado2 = dao.Seleccionar("2000");
                            MostrarListado(listado2);
                            break;

                        case '5': // Seleccionar toda la tabla a través de un Procedimiento Almacenado
                            List<Dvd> listado3 = new List<Dvd>();
                            int resultado;
                            listado3 = dao.SeleccionarPA(null, out resultado);
                            Console.WriteLine("Número de filas retornadas: {0}", resultado);
                            if (resultado != 0)
                            {
                                MostrarListado(listado3);
                            }
                            else
                            {
                                Console.WriteLine("No hay resultados");
                            }
                            break;

                        case '6':
                            Console.WriteLine("¿Código del DVD a eliminar?");
                            string codigo = Console.ReadLine();
                            Console.WriteLine(dao.Borrar(codigo) + " fila/s borrada/s");
                            break;

                        case '7':
                            Console.WriteLine("¿Código del DVD a insertar?");
                            Dvd elDVD = new Dvd();
                            elDVD.Codigo = short.Parse(Console.ReadLine());
                            elDVD.Titulo = "Titulo1";
                            elDVD.Compania = "Compañia1";
                            elDVD.Anio = "2010";
                            elDVD.Precio = 10.00;
                            elDVD.Pais = "US";
                            elDVD.Artista = "Artista1";
                            Console.WriteLine(dao.Insertar(elDVD) + " fila/s insertada/s");
                            break;

                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ha ocurrido un error: " + e.Message);
                }
            } while (opcion.Key != ConsoleKey.S);
        }

        private static void MostrarListado(List<Dvd> listado)
        {
            if (dao.Conectado())
            {
                foreach (Dvd item in listado)
                {
                    Console.WriteLine(item.ToString());
                }
            }
            else
                Console.WriteLine("No hay conexión válida");
        }
    }
}
