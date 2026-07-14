using Acceso_Datos;
using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class CategoriaNegocio
    {
        public List<Categoria> listar()
        {
            List<Categoria> lista = new List<Categoria>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("Select Id, Descripcion From CATEGORIAS");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Categoria aux = new Categoria();
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

        public void agregar(string categoriaNueva)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                if (existeDescripcion(categoriaNueva))
                    throw new Exception("Ya existe una categoría con esa descripción.");

                datos.setearConsulta("INSERT INTO CATEGORIAS (Descripcion) VALUES (@Descripcion)");
                datos.setearParametro("@Descripcion", categoriaNueva);
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

        public void modificar(Categoria categoriaModificada)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                if (existeDescripcion(categoriaModificada.Descripcion, categoriaModificada.Id))
                    throw new Exception("Ya existe otra categoría con esa descripción.");

                datos.setearConsulta("UPDATE CATEGORIAS SET Descripcion = @Descripcion WHERE Id = @Id");
                datos.setearParametro("@Id", categoriaModificada.Id);
                datos.setearParametro("@Descripcion", categoriaModificada.Descripcion);
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

        public void eliminar(int idCategoria)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                if (tieneArticulosAsociados(idCategoria))
                    throw new Exception("No se puede eliminar la categoría porque tiene artículos asociados.");

                datos.setearConsulta("DELETE FROM CATEGORIAS WHERE Id = @Id");
                datos.setearParametro("@Id", idCategoria);
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

        public bool tieneArticulosAsociados(int idCategoria)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT Id FROM Articulos WHERE IdCategoria = @Id");
                datos.setearParametro("@Id", idCategoria);
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
                datos.setearConsulta("SELECT Id FROM CATEGORIAS WHERE Descripcion = @Descripcion AND Id <> @IdExcluir");
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

        public List<Categoria> filtrar(string descripcion, string orden)
        {
            List<Categoria> lista = new List<Categoria>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = "SELECT Id, Descripcion FROM CATEGORIAS WHERE 1 = 1 ";

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
                    Categoria aux = new Categoria();
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
