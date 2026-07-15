using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace app_web
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Seguridad.sesionActiva(Session["usuario"]))
            {
                Response.Redirect("Home.aspx");
                return;
            }
        }

        protected void btnVolverHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx", false);
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Page.Validate("Login");

            if (!Page.IsValid)
                return;

            pnlError.Visible = false;
            lblError.Text = "";

            string email = txtEmail.Text.Trim();

            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(email))
            {
                mostrarError("Debe ingresar un email.");
                return;
            }

            if (!emailValido(email))
            {
                mostrarError("El formato del email no es válido.");
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                mostrarError("Debe ingresar una contraseña.");
                return;
            }

            UsuarioNegocio negocio = new UsuarioNegocio();

            try
            {
                Usuario usuario = negocio.login(email, password);

                if (usuario == null)
                {
                    mostrarError("Email o contraseña incorrectos.");
                    return;
                }

                Session["usuario"] = usuario;

                pnlError.Visible = false;

                Response.Redirect("Home.aspx");
            }
            catch (Exception ex)
            {
                mostrarError("Ocurrió un error al intentar iniciar sesión. " + ex.ToString());
            }
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

        private void mostrarError(string mensaje)
        {
            pnlError.Visible = true;
            lblError.Text = mensaje;
        }

    }
}