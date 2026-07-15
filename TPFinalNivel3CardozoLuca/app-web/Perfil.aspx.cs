using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace app_web
{
    public partial class Perfil : System.Web.UI.Page
    {
        public const string UrlAvatarPlaceholder = "https://st3.depositphotos.com/6672868/13701/v/450/depositphotos_137014128-stock-illustration-user-profile-icon.jpg";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Seguridad.sesionActiva(Session["usuario"]))
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                cargarDatosUsuario();
            }
        }

        private void cargarDatosUsuario()
        {
            Usuario usuario =
                Seguridad.usuarioActual(Session);

            txtEmail.Text = usuario.Email;

            if (usuario.Nombre != null)
                txtNombre.Text = usuario.Nombre;
            else
                txtNombre.Text = "";

            if (usuario.Apellido != null)
                txtApellido.Text = usuario.Apellido;
            else
                txtApellido.Text = "";

            txtPassword.Text = "";
            txtConfirmarPassword.Text = "";

            if (!string.IsNullOrWhiteSpace(usuario.Imagen))
            {
                hfImagenActual.Value = usuario.Imagen;
                imgPreviewPerfil.ImageUrl = usuario.Imagen;
            }
            else
            {
                hfImagenActual.Value = "";
                imgPreviewPerfil.ImageUrl = UrlAvatarPlaceholder;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Page.Validate("Perfil");

            if (!Page.IsValid)
                return;

            string password = txtPassword.Text;
            string confirmarPassword = txtConfirmarPassword.Text;

            bool ingresoPassword = !string.IsNullOrWhiteSpace(password);

            bool ingresoConfirmacion = !string.IsNullOrWhiteSpace(confirmarPassword);

            bool cambioPassword = ingresoPassword;

            if (ingresoPassword || ingresoConfirmacion)
            {
                if (!ingresoPassword)
                {
                    mostrarMensaje("Debe ingresar la nueva contraseña.", "danger");
                    return;
                }

                if (!ingresoConfirmacion)
                {
                    mostrarMensaje("Debe confirmar la nueva contraseña.", "danger");
                    return;
                }

                if (password != confirmarPassword)
                {
                    mostrarMensaje("Las contraseñas no coinciden.", "danger");
                    return;
                }
            }

            Usuario usuarioSesion = Seguridad.usuarioActual(Session);

            UsuarioNegocio negocio = new UsuarioNegocio();

            string imagenAnterior = hfImagenActual.Value;

            string nuevaImagenGuardada = null;

            try
            {
                Usuario usuarioModificado = new Usuario();

                usuarioModificado.Id = usuarioSesion.Id;

                usuarioModificado.Email = usuarioSesion.Email;

                if (string.IsNullOrWhiteSpace(password))
                {
                    usuarioModificado.Password = usuarioSesion.Password;
                }
                else
                {
                    usuarioModificado.Password = password;
                }

                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    usuarioModificado.Nombre = null;
                }
                else
                {
                    usuarioModificado.Nombre = txtNombre.Text.Trim();
                }

                if (string.IsNullOrWhiteSpace(txtApellido.Text))
                {
                    usuarioModificado.Apellido = null;
                }
                else
                {
                    usuarioModificado.Apellido = txtApellido.Text.Trim();
                }

                nuevaImagenGuardada = guardarImagenPerfil();

                if (!string.IsNullOrWhiteSpace(nuevaImagenGuardada))
                {
                    usuarioModificado.Imagen = nuevaImagenGuardada;
                }
                else if (!string.IsNullOrWhiteSpace(imagenAnterior))
                {
                    usuarioModificado.Imagen = imagenAnterior;
                }
                else
                {
                    usuarioModificado.Imagen = null;
                }

                usuarioModificado.Admin = usuarioSesion.Admin;

                usuarioModificado.Favoritos = usuarioSesion.Favoritos;

                negocio.modificarPerfil(usuarioModificado);

                if (cambioPassword)
                {
                    try
                    {
                        EmailService emailService = new EmailService();

                        string cuerpo =
                            "<h2>Cambio de contraseña</h2>" +
                            "<p>Hola " +
                            (string.IsNullOrWhiteSpace(usuarioModificado.Nombre)
                                ? usuarioModificado.Email
                                : usuarioModificado.Nombre) +
                            ",</p>" +
                            "<p>La contraseña asociada a tu cuenta fue modificada correctamente.</p>" +
                            "<p><strong>Fecha y hora:</strong> " +
                            DateTime.Now.ToString("dd/MM/yyyy HH:mm") +
                            "</p>" +
                            "<p>Si realizaste este cambio, no es necesario que hagas nada.</p>" +
                            "<p>Si no reconocés esta acción, cambiá tu contraseña lo antes posible.</p>";

                        emailService.armarCorreo(usuarioModificado.Email, "Aviso de cambio de contraseña", cuerpo);

                        emailService.enviarEmail();
                    }
                    catch (Exception exEmail)
                    {
                        System.Diagnostics.Debug.WriteLine("No se pudo enviar el correo: " + exEmail.Message);
                    }
                }

                if (imagenAnterior != usuarioModificado.Imagen)
                {
                    eliminarImagenPerfil(imagenAnterior);
                }

                // Actualizar el usuario guardado en sesión
                Session["usuario"] = usuarioModificado;

                hfImagenActual.Value = usuarioModificado.Imagen ?? "";

                if (!string.IsNullOrWhiteSpace(usuarioModificado.Imagen))
                {
                    imgPreviewPerfil.ImageUrl = usuarioModificado.Imagen;
                }
                else
                {
                    imgPreviewPerfil.ImageUrl = UrlAvatarPlaceholder;
                }

                txtPassword.Text = "";
                txtConfirmarPassword.Text = "";

                mostrarMensaje("El perfil fue modificado correctamente. Será redirigido al inicio en 3 segundos...", "success");
                redirigirAlHome();

            }
            catch (Exception ex)
            {
                // Se elimina la imagen si el UPDATE falla
                if (!string.IsNullOrWhiteSpace(nuevaImagenGuardada))
                {
                    eliminarImagenPerfil(nuevaImagenGuardada);
                }
                mostrarMensaje("No se pudo modificar el perfil: " + ex.Message, "danger");
            }
        }

        private string guardarImagenPerfil()
        {
            if (!fuImagenPerfil.HasFile)
                return null;

            string extension = Path.GetExtension(fuImagenPerfil.FileName).ToLower();

            bool extensionValida = extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".webp";

            if (!extensionValida)
            {
                throw new Exception("La imagen debe tener formato JPG, JPEG, PNG o WEBP.");
            }

            int tamanioMaximo = 5 * 1024 * 1024;

            if (fuImagenPerfil.PostedFile.ContentLength > tamanioMaximo)
            {
                throw new Exception("La imagen no puede superar los 5 MB.");
            }

            string nombreArchivo = Guid.NewGuid().ToString() + extension;

            string carpetaFisica = Server.MapPath("~/Images/Perfiles/");

            if (!Directory.Exists(carpetaFisica))
            {
                Directory.CreateDirectory(carpetaFisica);
            }

            string rutaFisica = Path.Combine(carpetaFisica, nombreArchivo);

            fuImagenPerfil.SaveAs(rutaFisica);

            return "~/Images/Perfiles/" + nombreArchivo;
        }

        private void eliminarImagenPerfil(string rutaImagen)
        {
            if (string.IsNullOrWhiteSpace(rutaImagen))
            {
                return;
            }

            if (!rutaImagen.StartsWith("~/Images/Perfiles/"))
            {
                return;
            }

            string rutaFisica = Server.MapPath(rutaImagen);

            if (File.Exists(rutaFisica))
            {
                File.Delete(rutaFisica);
            }
        }

        private void mostrarMensaje(string mensaje, string tipo)
        {
            pnlMensaje.Visible = true;

            pnlMensaje.CssClass = "alert alert-" + tipo + " text-center shadow-sm";

            lblMensaje.Text = mensaje;
        }

        private void redirigirAlHome()
        {
            ClientScript.RegisterStartupScript(this.GetType(), "redirigirHome", "setTimeout(function(){ window.location='Home.aspx'; }, 3000);", true);
        }

    }
}