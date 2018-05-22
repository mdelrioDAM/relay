using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//------------------------------
using System.ComponentModel;
using System.Windows.Input;

namespace Catalogo2018
{
    class CatalogoVM : INotifyPropertyChanged
    {
        #region Campos
        IDAO _dao;
        bool _tipoConexion = false; //MySQl: true, SQlite: false
        string _mensaje = "<Sin datos>";
        List<Dvd> _listado;
        Dvd _dvdSeleccionado;

        #endregion

        #region Propiedades

        public Dvd DvdSeleccionado
        {
            get { return _dvdSeleccionado; }
            set {
                if (_dvdSeleccionado != value)
                {
                    _dvdSeleccionado = value;
                    if (_dao.Conectado() && _dvdSeleccionado != null)
                        Mensaje = _dao.SeleccionarPais(_dvdSeleccionado.Pais).Nombre;
                    else
                        Mensaje = "<desconocido>";

                    NotificarCambiodePropiedad("DVDSeleccionadoNoNulo");
                }
            }
        }

        public List<Dvd> Listado
        {
            get { return _listado; }
            set
            {
                _listado = value;
                NotificarCambiodePropiedad("Listado");
            }
        }

        public bool DVDSeleccionadoNoNulo
        {
            get { return DvdSeleccionado != null; }
        }

        public bool NoConectado
        {
            get
            {
                return !Conectado;
            }
        }

        public bool TipoConexion
        {
            get { return _tipoConexion; }
            set
            {
                if (_tipoConexion != value)
                    _tipoConexion = value;
                NotificarCambiodePropiedad("TipoConexion");
            }
        }

        public string Mensaje
        {
            get { return _mensaje; }
            set
            {
                if (_mensaje != value)
                {
                    _mensaje = value;
                    NotificarCambiodePropiedad("Mensaje");
                }
            }
        }

        public bool Conectado
        {
            get
            {
                if (_dao == null)
                    return false;
                else
                    return _dao.Conectado();
            }
        }

        public string ColorConectar
        {
            get
            {
                if (Conectado)
                    return "green";
                else
                    return "red";
            }
            set
            {
                NotificarCambiodePropiedad("ColorConectar");
            }
        }

        #endregion

        #region Comandos

        public ICommand ListarTodosDVD_Click
        {
            get
            {
                return new RelayCommand(o => ListarTodosDVD(), o => true);
            }
        }

        public ICommand ConectarBD_Click
        {
            get
            {
                return new RelayCommand(o => ConectarBD(), o => true);
            }
        }

        public ICommand DesconectarBD_Click
        {
            get
            {
                return new RelayCommand(o => DesconectarBD(), o => true);
            }
        }

        public ICommand BorrarDVD_Click
        {
            get
            {
                return new RelayCommand(o => BorrarDVD(), o => true);
            }
        }

        private void ListarTodosDVD()
        {
            if (TipoConexion)
            {
                int nFilas = 0;
                Listado = _dao.SeleccionarPA(null, out nFilas);
                Mensaje = string.Format("Filas encontradas: {0}", nFilas);
            }
            else
            {
                Listado = _dao.Seleccionar(null);
                Mensaje = "Datos cargados correctamente";
            }
        }

        private void ConectarBD() // Todo comando es void, y NO recibe parámetros
        {
            try
            {
                _dao = null;
                if (_tipoConexion)
                { //MySQL
                    _dao = new DAOMySQL();
                    _dao.Conectar("localhost", "3306", "catalogo", "usr_catalogo", "123");
                    Mensaje = "Conectado con éxito a la BD a través de MySql";
                }
                else //SQlite
                {
                    _dao = new DAOSQlite();
                    _dao.Conectar(null, null, "catalogo.db", null, null);
                    Mensaje = "Conectado con éxito a la BD a través de SQLite";
                }
            }
            catch (Exception e)
            {
                Mensaje = e.Message;
            }
            NotificarCambiodePropiedad("ColorConectar");
            NotificarCambiodePropiedad("Conectado");
            NotificarCambiodePropiedad("NoConectado");
        }

        private void DesconectarBD()
        {
            _dao.Desconectar();
            Mensaje = "Desconectado de la BD";
            Listado = null;
            NotificarCambiodePropiedad("ColorConectar");
            NotificarCambiodePropiedad("Conectado");
            NotificarCambiodePropiedad("NoConectado");
        }

        private void BorrarDVD()
        {
            if (DVDSeleccionadoNoNulo)
            {
                if (_dao.Borrar(DvdSeleccionado.Codigo.ToString()) == 1)
                    Mensaje = "Registro eliminado";
                else
                    Mensaje = "Error al eliminar el registro";
                ListarTodosDVD();
            }
        }

        #endregion

        #region Eventos
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotificarCambiodePropiedad(string propiedad)
        {
            PropertyChangedEventHandler manejador = PropertyChanged;
            if (manejador != null)
                manejador(this, new PropertyChangedEventArgs(propiedad));
        }
        #endregion
    }
}
