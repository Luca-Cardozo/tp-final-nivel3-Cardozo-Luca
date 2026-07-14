using Acceso_Datos;
using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class MarcaNegocio
    {
        public List<Marca> listar()
        {
            List<Marca> lista = new List<Marca>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("Select Id, Descripcion From MARCAS");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Marca aux = new Marca();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void agregar(string marcaNueva)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                if (existeDescripcion(marcaNueva))
                    throw new Exception("Ya existe una marca con esa descripción.");

                datos.setearConsulta("INSERT INTO MARCAS (Descripcion) VALUES (@Descripcion)");
                datos.setearParametro("@Descripcion", marcaNueva);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void modificar(Marca marcaModificada)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                if (existeDescripcion(marcaModificada.Descripcion, marcaModificada.Id))
                    throw new Exception("Ya existe otra marca con esa descripción.");

                datos.setearConsulta("UPDATE MARCAS SET Descripcion = @Descripcion WHERE Id = @Id");
                datos.setearParametro("@Id", marcaModificada.Id);
                datos.setearParametro("@Descripcion", marcaModificada.Descripcion);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void eliminar(int idMarca)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                if (tieneArticulosAsociados(idMarca))
                    throw new Exception("No se puede eliminar la marca porque tiene artículos asociados.");

                datos.setearConsulta("DELETE FROM MARCAS WHERE Id = @Id");
                datos.setearParametro("@Id", idMarca);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public bool tieneArticulosAsociados(int idMarca)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT Id FROM Articulos WHERE IdMarca = @Id");
                datos.setearParametro("@Id", idMarca);
                datos.ejecutarLectura();
                if (datos.Lector.Read())
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public bool existeDescripcion(string descripcion, int idExcluir = 0)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT Id FROM MARCAS WHERE Descripcion = @Descripcion AND Id <> @IdExcluir");
                datos.setearParametro("@Descripcion", descripcion);
                datos.setearParametro("@IdExcluir", idExcluir);
                datos.ejecutarLectura();
                return datos.Lector.Read();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public List<Marca> filtrar(string descripcion, string orden)
        {
            List<Marca> lista = new List<Marca>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = "SELECT Id, Descripcion FROM MARCAS WHERE 1 = 1 ";

                if (!string.IsNullOrWhiteSpace(descripcion))
                    consulta += "AND Descripcion LIKE @Descripcion ";

                if (orden == "Asc")
                    consulta += "ORDER BY Descripcion ASC";
                else if (orden == "Desc")
                    consulta += "ORDER BY Descripcion DESC";

                datos.setearConsulta(consulta);

                if (!string.IsNullOrWhiteSpace(descripcion))
                {
                    datos.setearParametro("@Descripcion", "%" + descripcion + "%");
                }

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Marca aux = new Marca();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Descripcion = datos.Lector["Descripcion"].ToString();
                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

    }
}
