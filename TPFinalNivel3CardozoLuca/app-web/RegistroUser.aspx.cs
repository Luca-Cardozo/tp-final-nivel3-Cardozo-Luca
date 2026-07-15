using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace app_web
{
    public partial class RegistroUser : System.Web.UI.Page
    {
        public const string UrlAvatarPlaceholder = "https://st3.depositphotos.com/6672868/13701/v/450/depositphotos_137014128-stock-illustration-user-profile-icon.jpg";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Seguridad.sesionActiva(Session["usuario"]))
            {
                Response.Redirect("Home.aspx");
                return;
            }

            if (!IsPostBack)
            {
                imgPreviewPerfil.ImageUrl = UrlAvatarPlaceholder;
            }
        }

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            Page.Validate("Registro");

            if (!Page.IsValid)
                return;

            pnlMensaje.Visible = false;

            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;
            string confirmarPassword = txtConfirmarPassword.Text;

            if (string.IsNullOrWhiteSpace(email))
            {
                mostrarMensaje("El email es obligatorio.", "danger");
                return;
            }

            if (!emailValido(email))
            {
                mostrarMensaje("El formato del email no es válido.", "danger");
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                mostrarMensaje("La contraseña es obligatoria.", "danger");
                return;
            }

            if (string.IsNullOrWhiteSpace(confirmarPassword))
            {
                mostrarMensaje("Debe confirmar la contraseña.", "danger");
                return;
            }

            if (password != confirmarPassword)
            {
                mostrarMensaje("Las contraseñas no coinciden.", "danger");
                return;
            }

            UsuarioNegocio negocio = new UsuarioNegocio();

            string rutaImagenGuardada = null;

            try
            {
                Usuario usuario = new Usuario();

                usuario.Email = email;

                usuario.Password = password;

                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    usuario.Nombre = null;
                }
                else
                {
                    usuario.Nombre = txtNombre.Text.Trim();
                }

                if (string.IsNullOrWhiteSpace(txtApellido.Text))
                {
                    usuario.Apellido = null;
                }
                else
                {
                    usuario.Apellido = txtApellido.Text.Trim();
                }

                rutaImagenGuardada = guardarImagenPerfil();

                usuario.Imagen = rutaImagenGuardada;

                usuario.Admin = false;

                negocio.altaUsuario(usuario);

                try
                {
                    EmailService emailService = new EmailService();

                    string nombreMostrar = string.IsNullOrWhiteSpace(usuario.Nombre) ? usuario.Email : usuario.Nombre;

                    string cuerpo =
                        "<h2>¡Bienvenido a Catálogo Tecnológico!</h2>" +
                        "<p>Hola <b>" + nombreMostrar + "</b>,</p>" +
                        "<p>Tu cuenta fue creada correctamente.</p>" +
                        "<p>Ya podés iniciar sesión, guardar favoritos y administrar tu perfil.</p>" +
                        "<br/>" +
                        "<p>Gracias por registrarte.</p>";

                    emailService.armarCorreo(usuario.Email, "Bienvenido a Catálogo Tecnológico", cuerpo);

                    emailService.enviarEmail();
                }
                catch (Exception exEmail)
                {
                    System.Diagnostics.Debug.WriteLine("Error al enviar email de bienvenida: " + exEmail.Message);
                }

                mostrarMensaje("La cuenta fue creada correctamente. Será redirigido al inicio de sesión en 3 segundos...", "success");
                limpiarFormulario();
                redirigirAlLogin();

            }
            catch (Exception ex)
            {
                // Si el archivo se guardó, pero el INSERT falló, se elimina para no dejar archivos huérfanos.
                if (!string.IsNullOrWhiteSpace(rutaImagenGuardada))
                {
                    eliminarImagenPerfil(rutaImagenGuardada);
                }
                mostrarMensaje(ex.Message, "danger");
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

            string carpetaFisica = Server.MapPath("~/Images/Usuarios/");

            if (!Directory.Exists(carpetaFisica))
            {
                Directory.CreateDirectory(carpetaFisica);
            }

            string rutaFisica = Path.Combine(carpetaFisica, nombreArchivo);

            fuImagenPerfil.SaveAs(rutaFisica);

            return "~/Images/Usuarios/" + nombreArchivo;
        }

        private void eliminarImagenPerfil(string rutaImagen)
        {
            if (string.IsNullOrWhiteSpace(rutaImagen))
                return;

            if (!rutaImagen.StartsWith("~/Images/Usuarios/"))
            {
                return;
            }

            string rutaFisica = Server.MapPath(rutaImagen);

            if (File.Exists(rutaFisica))
            {
                File.Delete(rutaFisica);
            }
        }

        private void limpiarFormulario()
        {
            txtEmail.Text = "";
            txtPassword.Text = "";
            txtConfirmarPassword.Text = "";
            txtNombre.Text = "";
            txtApellido.Text = "";

            imgPreviewPerfil.ImageUrl = UrlAvatarPlaceholder;
        }

        private void mostrarMensaje(string mensaje, string tipo)
        {
            pnlMensaje.Visible = true;

            pnlMensaje.CssClass = "alert alert-" + tipo + " text-center shadow-sm";

            lblMensaje.Text = mensaje;
        }

        private void redirigirAlLogin()
        {
            ClientScript.RegisterStartupScript(this.GetType(), "redirigirLogin", "setTimeout(function(){ window.location='Login.aspx'; }, 3000);", true);
        }

        private bool emailValido(string email)
        {
            try
            {
                MailAddress direccion = new MailAddress(email);
                return direccion.Address == email;
            }
            catch
            {
                return false;
            }
        }

    }
}