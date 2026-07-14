using Acceso_Datos;
using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class FavoritoNegocio
    {
        public List<Articulo> listarFavoritos(int idUsuario)
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT A.Id, A.Codigo, A.Nombre, A.Descripcion, " +
                    "A.IdMarca, A.IdCategoria, A.ImagenUrl, A.Precio, " +
                    "M.Descripcion AS Marca, C.Descripcion AS Categoria " +
                    "FROM FAVORITOS F " +
                    "INNER JOIN ARTICULOS A ON A.Id = F.IdArticulo " +
                    "INNER JOIN MARCAS M ON A.IdMarca = M.Id " +
                    "INNER JOIN CATEGORIAS C ON A.IdCategoria = C.Id " +
                    "WHERE F.IdUser = @IdUser");

                datos.setearParametro("@IdUser", idUsuario);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();

                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];

                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];

                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];

                    if (datos.Lector["Descripcion"] is DBNull)
                        aux.Descripcion = null;
                    else
                        aux.Descripcion = datos.Lector["Descripcion"].ToString();

                    if (datos.Lector["ImagenUrl"] is DBNull)
                        aux.Imagen = null;
                    else
                        aux.Imagen = datos.Lector["ImagenUrl"].ToString();

                    if (datos.Lector["Precio"] is DBNull)
                        aux.Precio = null;
                    else
                        aux.Precio = (decimal)datos.Lector["Precio"];

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

        public void agregarFavorito(int idUsuario, int idArticulo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                if (existeFavorito(idUsuario, idArticulo))
                    throw new Exception("El artículo ya está en favoritos.");

                datos.setearConsulta("INSERT INTO FAVORITOS (IdUser, IdArticulo) VALUES (@IdUser, @IdArticulo)");
                datos.setearParametro("@IdUser", idUsuario);
                datos.setearParametro("@IdArticulo", idArticulo);
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

        public void eliminarFavorito(int idUsuario, int idArticulo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("DELETE FROM FAVORITOS WHERE IdUser = @IdUser AND IdArticulo = @IdArticulo");
                datos.setearParametro("@IdUser", idUsuario);
                datos.setearParametro("@IdArticulo", idArticulo);
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

        public bool existeFavorito(int idUsuario, int idArticulo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT Id FROM FAVORITOS WHERE IdUser = @IdUser AND IdArticulo = @IdArticulo");
                datos.setearParametro("@IdUser", idUsuario);
                datos.setearParametro("@IdArticulo", idArticulo);
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

        public List<Articulo> filtrarFavoritos(int idUsuario, string nombre, int idMarca, int idCategoria, string orden)
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta =
                    "SELECT A.Id, A.Codigo, A.Nombre, A.Descripcion, A.IdMarca, A.IdCategoria, " +
                    "A.ImagenUrl, A.Precio, M.Descripcion AS Marca, C.Descripcion AS Categoria " +
                    "FROM FAVORITOS F " +
                    "INNER JOIN ARTICULOS A ON A.Id = F.IdArticulo " +
                    "INNER JOIN MARCAS M ON A.IdMarca = M.Id " +
                    "INNER JOIN CATEGORIAS C ON A.IdCategoria = C.Id " +
                    "WHERE F.IdUser = @IdUser ";

                if (!string.IsNullOrWhiteSpace(nombre))
                    consulta += "AND A.Nombre LIKE @Nombre ";

                if (idMarca > 0)
                    consulta += "AND A.IdMarca = @IdMarca ";

                if (idCategoria > 0)
                    consulta += "AND A.IdCategoria = @IdCategoria ";

                switch (orden)
                {
                    case "NombreAsc":
                        consulta += "ORDER BY A.Nombre ASC";
                        break;

                    case "NombreDesc":
                        consulta += "ORDER BY A.Nombre DESC";
                        break;

                    case "PrecioAsc":
                        consulta += "ORDER BY A.Precio ASC";
                        break;

                    case "PrecioDesc":
                        consulta += "ORDER BY A.Precio DESC";
                        break;
                }

                datos.setearConsulta(consulta);

                datos.setearParametro("@IdUser", idUsuario);

                if (!string.IsNullOrWhiteSpace(nombre))
                    datos.setearParametro("@Nombre", "%" + nombre + "%");

                if (idMarca > 0)
                    datos.setearParametro("@IdMarca", idMarca);

                if (idCategoria > 0)
                    datos.setearParametro("@IdCategoria", idCategoria);

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();

                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = datos.Lector["Codigo"].ToString();
                    aux.Nombre = datos.Lector["Nombre"].ToString();

                    if (datos.Lector["Descripcion"] is DBNull)
                        aux.Descripcion = null;
                    else
                        aux.Descripcion = datos.Lector["Descripcion"].ToString();

                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = datos.Lector["Marca"].ToString();

                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = datos.Lector["Categoria"].ToString();

                    if (datos.Lector["ImagenUrl"] is DBNull)
                        aux.Imagen = null;
                    else
                        aux.Imagen = datos.Lector["ImagenUrl"].ToString();

                    if (datos.Lector["Precio"] is DBNull)
                        aux.Precio = null;
                    else
                        aux.Precio = (decimal)datos.Lector["Precio"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch
            {
                throw;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

    }
}
