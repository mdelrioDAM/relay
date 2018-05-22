using System.Collections.ObjectModel;
using System.Data;
using System.Collections.Generic;

namespace Catalogo2018
{
    interface IDAO
    {
        bool Conectar(string srv, string port, string db, string user, string pwd);
        void Desconectar();
        bool Conectado();
        List<Dvd> SeleccionarPA(string codigo, out int resul); //Llamada al procedimiento
        DataTable SeleccionarTB(string codigo); //Selecciona la tabla
        List<Dvd> Seleccionar(string codigo);   // Devuelve una lista de dvd
        int Borrar(string codigo);
        Pais SeleccionarPais(string iso2);  // Selecciona Pais
        int Actualizar(Dvd unDVD);
        int Insertar(Dvd unDVD);
    }
}
