using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace app_web
{
    public partial class Favoritos : System.Web.UI.Page
    {
        public const string UrlPlaceholder = "https://media.istockphoto.com/id/1980276924/es/vector/sin-elemento-gr%C3%A1fico-en-miniatura-de-la-foto-no-se-ha-encontrado-ninguna-imagen-o-est%C3%A1.jpg?s=612x612&w=0&k=20&c=artWlQoi5R1edWQBv9LfzeLXupOcH_alZnMgvXdYkF4=";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Seguridad.sesionActiva(Session["usuario"]))
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                cargarFiltros();
                cargarFavoritos();
            }
        }

        private void cargarFavoritos()
        {
            Usuario usuario = Seguridad.usuarioActual(Session);
            FavoritoNegocio negocio = new FavoritoNegocio();

            try
            {
                List<Articulo> lista = negocio.listarFavoritos(usuario.Id);

                repFavoritos.DataSource = lista;
                repFavoritos.DataBind();

                pnlSinFavoritos.Visible = lista.Count == 0;
            }
            catch (Exception ex)
            {
                mostrarMensaje("No se pudieron cargar los favoritos: " + ex.Message, "danger");
            }
        }

        protected void repFavoritos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "EliminarFavorito")
                return;

            int idArticulo;

            if (!int.TryParse(e.CommandArgument.ToString(), out idArticulo))
            {
                mostrarMensaje("El identificador del artículo no es válido.", "danger");
                return;
            }

            Usuario usuario = Seguridad.usuarioActual(Session);
            FavoritoNegocio negocio = new FavoritoNegocio();

            try
            {
                negocio.eliminarFavorito(usuario.Id, idArticulo);
                cargarFavoritos();
                mostrarMensaje("El artículo fue eliminado de favoritos.", "success");
            }
            catch (Exception ex)
            {
                mostrarMensaje("No se pudo quitar el artículo de favoritos: " + ex.Message, "danger");
            }
        }

        public string obtenerImagen(object imagen)
        {
            if (imagen == null || imagen == DBNull.Value)
            {
                return UrlPlaceholder;
            }

            string ruta = imagen.ToString();

            if (string.IsNullOrWhiteSpace(ruta))
                return UrlPlaceholder;

            if (ruta.StartsWith("~/"))
                return ResolveUrl(ruta);

            return ruta;
        }

        public string mostrarPrecio(object precio)
        {
            if (precio == null || precio == DBNull.Value)
            {
                return "Precio no disponible";
            }

            decimal valor = (decimal)precio;

            return valor.ToString("C2");
        }

        private void mostrarMensaje(string mensaje, string tipo)
        {
            pnlMensaje.Visible = true;

            pnlMensaje.CssClass = "alert alert-" + tipo + " text-center shadow-sm";

            lblMensaje.Text = mensaje;
        }

        private void cargarFiltros()
        {
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();

            try
            {
                ddlMarca.DataSource = marcaNegocio.listar();
                ddlMarca.DataValueField = "Id";
                ddlMarca.DataTextField = "Descripcion";
                ddlMarca.DataBind();
                ddlMarca.Items.Insert(0, new ListItem("Todas las marcas", "0"));

                ddlCategoria.DataSource = categoriaNegocio.listar();
                ddlCategoria.DataValueField = "Id";
                ddlCategoria.DataTextField = "Descripcion";
                ddlCategoria.DataBind();
                ddlCategoria.Items.Insert(0, new ListItem("Todas las categorías", "0"));
            }
            catch (Exception ex)
            {
                mostrarMensaje("No se pudieron cargar los filtros: " + ex.Message, "danger");
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            Usuario usuario = Seguridad.usuarioActual(Session);

            FavoritoNegocio negocio = new FavoritoNegocio();

            try
            {
                List<Articulo> lista = negocio.filtrarFavoritos(usuario.Id, txtNombre.Text.Trim(), int.Parse(ddlMarca.SelectedValue), int.Parse(ddlCategoria.SelectedValue), ddlOrden.SelectedValue);

                repFavoritos.DataSource = lista;
                repFavoritos.DataBind();

                pnlSinFavoritos.Visible = lista.Count == 0;

                pnlMensaje.Visible = false;
            }
            catch (Exception ex)
            {
                mostrarMensaje("No se pudo realizar la búsqueda: " + ex.Message, "danger");
            }
        }

        protected void btnRecargar_Click(object sender, EventArgs e)
        {
            txtNombre.Text = "";

            ddlMarca.SelectedIndex = 0;
            ddlCategoria.SelectedIndex = 0;
            ddlOrden.SelectedIndex = 0;

            pnlMensaje.Visible = false;

            cargarFavoritos();
        }

    }
}