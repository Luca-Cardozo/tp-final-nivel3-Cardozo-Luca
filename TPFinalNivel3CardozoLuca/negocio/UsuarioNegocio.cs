using Acceso_Datos;
using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class UsuarioNegocio
    {
        public Usuario login(string email, string password)
        {
            Usuario usuario = null;
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT Id, email, pass, nombre, apellido, urlImagenPerfil, admin " +
                "FROM USERS WHERE email = @Email AND pass = @Password");

                datos.setearParametro("@Email", email);
                datos.setearParametro("@Password", password);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    usuario = new Usuario();
                    usuario.Id = (int)datos.Lector["Id"];
                    usuario.Email = datos.Lector["email"].ToString();
                    usuario.Password = datos.Lector["pass"].ToString();
                    usuario.Nombre = datos.Lector["nombre"] is DBNull ? null : datos.Lector["nombre"].ToString();
                    usuario.Apellido = datos.Lector["apellido"] is DBNull ? null : datos.Lector["apellido"].ToString();
                    usuario.Imagen = datos.Lector["urlImagenPerfil"] is DBNull ? null : datos.Lector["urlImagenPerfil"].ToString();
                    usuario.Admin = (bool)datos.Lector["admin"];
                }
                return usuario;
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

        public void altaUsuario(Usuario nuevo)
        {
            if (existeUsuario(nuevo.Email))
                throw new Exception("Ya existe un usuario con ese email.");

            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("INSERT INTO USERS (email, pass, nombre, apellido, " +
                "urlImagenPerfil, admin) VALUES (@Email, @Password, @Nombre, @Apellido, @Imagen, 0");

                datos.setearParametro("@Email", nuevo.Email);
                datos.setearParametro("@Password", nuevo.Password);
                if (string.IsNullOrWhiteSpace(nuevo.Nombre))
                    datos.setearParametro("@Nombre", DBNull.Value);
                else
                    datos.setearParametro("@Nombre", nuevo.Nombre);
                if (string.IsNullOrWhiteSpace(nuevo.Apellido))
                    datos.setearParametro("@Apellido", DBNull.Value);
                else
                    datos.setearParametro("@Apellido", nuevo.Apellido);
                if (string.IsNullOrWhiteSpace(nuevo.Imagen))
                    datos.setearParametro("@Imagen", DBNull.Value);
                else
                    datos.setearParametro("@Imagen", nuevo.Imagen);
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

        public void modificarPerfil(Usuario usuario)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("UPDATE USERS SET nombre = @Nombre, apellido = @Apellido, " +
                    "urlImagenPerfil = @Imagen, pass = @Password WHERE Id = @Id");

                if (string.IsNullOrWhiteSpace(usuario.Nombre))
                    datos.setearParametro("@Nombre", DBNull.Value);
                else
                    datos.setearParametro("@Nombre", usuario.Nombre);
                if (string.IsNullOrWhiteSpace(usuario.Apellido))
                    datos.setearParametro("@Apellido", DBNull.Value);
                else
                    datos.setearParametro("@Apellido", usuario.Apellido);
                if (string.IsNullOrWhiteSpace(usuario.Imagen))
                    datos.setearParametro("@Imagen", DBNull.Value);
                else
                    datos.setearParametro("@Imagen", usuario.Imagen);
                datos.setearParametro("@Password", usuario.Password);
                datos.setearParametro("@Id", usuario.Id);

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

        public bool existeUsuario(string email)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT 1 FROM USERS WHERE email = @Email");
                datos.setearParametro("@Email", email);
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

    }
}
