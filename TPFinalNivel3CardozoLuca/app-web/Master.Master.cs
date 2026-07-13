using dominio;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace app_web
{
    public partial class Master : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            imgAvatar.ImageUrl = "https://st3.depositphotos.com/6672868/13701/v/450/depositphotos_137014128-stock-illustration-user-profile-icon.jpg";

            bool paginaPublica = Page is Home || Page is Login || Page is Logout || Page is RegistroUser || Page is DetalleArticulo;

            Usuario usuario = Seguridad.usuarioActual(Session);

            if (!paginaPublica && usuario == null)
            {
                Response.Redirect("Login.aspx", false);
                return;
            }

            configurarNavbar(usuario);
        }

        private void configurarNavbar(Usuario usuario)
        {
            bool sesionActiva = usuario != null;
            bool esAdministrador = sesionActiva && usuario.Admin;

            liRegistro.Visible = !sesionActiva;
            liLogin.Visible = !sesionActiva;

            liLogout.Visible = sesionActiva;
            liPerfil.Visible = sesionActiva;
            liFavoritos.Visible = sesionActiva;

            liUsuarioLogueado.Visible = sesionActiva;
            liAvatar.Visible = sesionActiva;

            liAdministrarArticulos.Visible = esAdministrador;

            if (sesionActiva)
            {
                cargarDatosUsuario(usuario);
            }
        }

        private void cargarDatosUsuario(Usuario usuario)
        {
            if (!string.IsNullOrEmpty(usuario.Nombre))
            {
                lblUsuarioLogueado.Text = usuario.Nombre;
            }
            else
            {
                lblUsuarioLogueado.Text = usuario.Email;
            }

            if (!string.IsNullOrEmpty(usuario.Imagen))
            {
                imgAvatar.ImageUrl = usuario.Imagen;
            }
            else
            {
                imgAvatar.ImageUrl = "https://st3.depositphotos.com/6672868/13701/v/450/depositphotos_137014128-stock-illustration-user-profile-icon.jpg";
            }
        }

    }
}