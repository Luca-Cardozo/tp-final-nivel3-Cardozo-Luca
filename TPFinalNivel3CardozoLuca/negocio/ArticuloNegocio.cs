using Acceso_Datos;
using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// Se agrega para la validación que usa expresiones regulares
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace negocio
{
    public class ArticuloNegocio
    {
        public List<Articulo> listar()
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT A.Id, A.Codigo, A.Nombre, A.Descripcion, " +
                    "A.IdMarca, A.IdCategoria, A.ImagenUrl, " +
                    "M.Descripcion AS Marca, C.Descripcion AS Categoria, " +
                    "A.Precio FROM ARTICULOS A " +
                    "INNER JOIN MARCAS M ON A.IdMarca = M.Id " +
                    "INNER JOIN CATEGORIAS C ON A.IdCategoria = C.Id");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    if (datos.Lector["Descripcion"] is DBNull)
                        aux.Descripcion = null;
                    else
                        aux.Descripcion = datos.Lector["Descripcion"].ToString();
                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];
                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];
                    if (datos.Lector["Precio"] is DBNull)
                        aux.Precio = null;
                    else
                        aux.Precio = (decimal)datos.Lector["Precio"];
                    if (datos.Lector["ImagenUrl"] is DBNull)
                        aux.Imagen = null;
                    else
                        aux.Imagen = datos.Lector["ImagenUrl"].ToString();
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

        public void agregar(Articulo articuloNuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                articuloNuevo.Codigo = articuloNuevo.Codigo.ToUpper();

                if (!validarCodigo(articuloNuevo.Codigo))
                    throw new Exception("El código debe tener una letra seguida de dos números. Ejemplo: A12.");

                if (validarCodigoDuplicado(articuloNuevo.Codigo))
                    throw new Exception("Ya existe un artículo con ese código.");

                if (validarNombreDuplicado(articuloNuevo.Nombre))
                    throw new Exception("Ya existe un artículo con ese nombre.");

                datos.setearConsulta("INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio) " +
                     "VALUES (@Codigo, @Nombre, @Descripcion, @IdMarca, @IdCategoria, @Imagen, @Precio)");

                datos.setearParametro("@Codigo", articuloNuevo.Codigo);
                datos.setearParametro("@Nombre", articuloNuevo.Nombre);
                datos.setearParametro("@Descripcion", articuloNuevo.Descripcion);
                datos.setearParametro("@IdMarca", articuloNuevo.Marca.Id);
                datos.setearParametro("@IdCategoria", articuloNuevo.Categoria.Id);
                datos.setearParametro("@Imagen", articuloNuevo.Imagen);
                datos.setearParametro("@Precio", articuloNuevo.Precio);

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

        public void modificar(Articulo articuloModificado)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                articuloModificado.Codigo = articuloModificado.Codigo.ToUpper();

                if (!validarCodigo(articuloModificado.Codigo))
                    throw new Exception("El código debe tener una letra seguida de dos números. Ejemplo: A12.");

                if (validarCodigoDuplicado(articuloModificado.Codigo, articuloModificado.Id))
                    throw new Exception("Ya existe otro artículo con ese código.");

                if (validarNombreDuplicado(articuloModificado.Nombre, articuloModificado.Id))
                    throw new Exception("Ya existe otro artículo con ese nombre.");

                datos.setearConsulta("UPDATE ARTICULOS SET Codigo = @Codigo, Nombre = @Nombre, Descripcion = @Descripcion, IdMarca = @IdMarca, IdCategoria = @IdCategoria, ImagenUrl = @Imagen, Precio = @Precio WHERE Id = @Id");

                datos.setearParametro("@Codigo", articuloModificado.Codigo);
                datos.setearParametro("@Nombre", articuloModificado.Nombre);
                datos.setearParametro("@Descripcion", articuloModificado.Descripcion);
                datos.setearParametro("@IdMarca", articuloModificado.Marca.Id);
                datos.setearParametro("@IdCategoria", articuloModificado.Categoria.Id);
                datos.setearParametro("@Imagen", articuloModificado.Imagen);
                datos.setearParametro("@Precio", articuloModificado.Precio);
                datos.setearParametro("@Id", articuloModificado.Id);

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

        public void eliminar(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("DELETE FROM ARTICULOS WHERE Id = @Id");
                datos.setearParametro("@Id", id);
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

        public bool validarCodigoDuplicado(string nuevoCodigo, int idExcluir = 0)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT Id FROM ARTICULOS WHERE Codigo = @Codigo AND Id <> @IdExcluir");
                datos.setearParametro("@Codigo", nuevoCodigo);
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

        public bool validarNombreDuplicado(string nuevoNombre, int idExcluir = 0)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT Id FROM ARTICULOS WHERE Nombre = @Nombre AND Id <> @IdExcluir");
                datos.setearParametro("@Nombre", nuevoNombre);
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

        // Uso una expresión regular para validar que el código sea de una letra seguida de 2 números
        public bool validarCodigo(string codigo)
        {
            return Regex.IsMatch(codigo, @"^[A-Za-z][0-9]{2}$");
        }

        public Articulo buscarPorId(int idArticulo)
        {
            Articulo articulo = null;
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(
                    "SELECT A.Id, A.Codigo, A.Nombre, A.Descripcion, A.IdMarca, A.IdCategoria, " +
                    "A.ImagenUrl, M.Descripcion AS Marca, C.Descripcion AS Categoria, A.Precio " +
                    "FROM ARTICULOS A " +
                    "INNER JOIN MARCAS M ON A.IdMarca = M.Id " +
                    "INNER JOIN CATEGORIAS C ON A.IdCategoria = C.Id " +
                    "WHERE A.Id = @Id"
                );

                datos.setearParametro("@Id", idArticulo);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    articulo = new Articulo();

                    articulo.Id = (int)datos.Lector["Id"];
                    articulo.Codigo = datos.Lector["Codigo"].ToString();
                    articulo.Nombre = datos.Lector["Nombre"].ToString();

                    if (datos.Lector["Descripcion"] is DBNull)
                        articulo.Descripcion = null;
                    else
                        articulo.Descripcion = datos.Lector["Descripcion"].ToString();

                    articulo.Marca = new Marca();
                    articulo.Marca.Id = (int)datos.Lector["IdMarca"];
                    articulo.Marca.Descripcion = datos.Lector["Marca"].ToString();

                    articulo.Categoria = new Categoria();
                    articulo.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    articulo.Categoria.Descripcion =
                        datos.Lector["Categoria"].ToString();

                    if (datos.Lector["Precio"] is DBNull)
                        articulo.Precio = null;
                    else
                        articulo.Precio = (decimal)datos.Lector["Precio"];

                    if (datos.Lector["ImagenUrl"] is DBNull)
                        articulo.Imagen = null;
                    else
                        articulo.Imagen = datos.Lector["ImagenUrl"].ToString();
                }

                return articulo;
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
