using dominio;
using negocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace app_web
{
    public partial class Home : System.Web.UI.Page
    {

        public const string UrlPlaceholder = "https://media.istockphoto.com/id/1980276924/es/vector/sin-elemento-gr%C3%A1fico-en-miniatura-de-la-foto-no-se-ha-encontrado-ninguna-imagen-o-est%C3%A1.jpg?s=612x612&w=0&k=20&c=artWlQoi5R1edWQBv9LfzeLXupOcH_alZnMgvXdYkF4=";

        // Para la paginación
        private int PaginaActual
        {
            get
            {
                if (ViewState["PaginaActual"] == null)
                    return 0;

                return (int)ViewState["PaginaActual"];
            }

            set
            {
                ViewState["PaginaActual"] = value;
            }
        }

        private bool FiltrosActivos
        {
            get
            {
                if (ViewState["FiltrosActivos"] == null)
                    return false;

                return (bool)ViewState["FiltrosActivos"];
            }

            set
            {
                ViewState["FiltrosActivos"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargarFiltros();
                cargarArticulos();
            }
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
                pnlError.Visible = true;
                lblError.Text = "No se pudieron cargar los filtros: " + ex.Message;
            }
        }

        private void cargarArticulos()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                List<Articulo> lista = negocio.listar();
                mostrarArticulos(lista);
            }
            catch (Exception ex)
            {
                pnlError.Visible = true;
                lblError.Text = "No se pudieron cargar los artículos: " + ex.Message;
            }
        }

        private void cargarArticulosFiltrados()
        {
            int idMarca = int.Parse(ddlMarca.SelectedValue);
            int idCategoria = int.Parse(ddlCategoria.SelectedValue);

            decimal? precioMinimo = null;
            decimal? precioMaximo = null;

            decimal valor;

            if (!string.IsNullOrWhiteSpace(txtPrecioMinimo.Text))
            {
                if (decimal.TryParse(txtPrecioMinimo.Text, out valor))
                {
                    precioMinimo = valor;
                }
            }

            if (!string.IsNullOrWhiteSpace(txtPrecioMaximo.Text))
            {
                if (decimal.TryParse(txtPrecioMaximo.Text, out valor))
                {
                    precioMaximo = valor;
                }
            }

            ArticuloNegocio negocio = new ArticuloNegocio();

            List<Articulo> lista = negocio.filtrar(null, txtNombre.Text.Trim(), idMarca, idCategoria, precioMinimo, precioMaximo, ddlOrden.SelectedValue);
            mostrarArticulos(lista);
        }

        public string obtenerImagen(object imagen)
        {
            if (imagen == null || imagen == DBNull.Value)
                return UrlPlaceholder;

            string url = imagen.ToString();

            if (string.IsNullOrWhiteSpace(url))
                return UrlPlaceholder;

            return url;
        }

        public string mostrarPrecio(object precio)
        {
            if (precio == null || precio == DBNull.Value)
                return "Precio no disponible";

            decimal valor = (decimal)precio;
            return valor.ToString("C2");
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            lblErrorFiltros.Text = "";

            decimal? precioMinimo = null;
            decimal? precioMaximo = null;
            decimal valor;

            if (!string.IsNullOrWhiteSpace(txtPrecioMinimo.Text))
            {
                if (!decimal.TryParse(txtPrecioMinimo.Text, out valor))
                {
                    lblErrorFiltros.Text = "El precio mínimo ingresado no es válido.";
                    return;
                }
                precioMinimo = valor;
            }

            if (!string.IsNullOrWhiteSpace(txtPrecioMaximo.Text))
            {
                if (!decimal.TryParse(txtPrecioMaximo.Text, out valor))
                {
                    lblErrorFiltros.Text = "El precio máximo ingresado no es válido.";
                    return;
                }
                precioMaximo = valor;
            }

            if (precioMinimo.HasValue && precioMaximo.HasValue && precioMinimo.Value > precioMaximo.Value)
            {
                lblErrorFiltros.Text = "El precio mínimo no puede ser mayor que el precio máximo.";
                return;
            }

            PaginaActual = 0;
            FiltrosActivos = true;

            try
            {
                cargarArticulosFiltrados();
            }
            catch (Exception ex)
            {
                pnlError.Visible = true;
                lblError.Text = "No se pudo realizar la búsqueda: " + ex.Message;
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtNombre.Text = "";
            txtPrecioMinimo.Text = "";
            txtPrecioMaximo.Text = "";

            ddlMarca.SelectedIndex = 0;
            ddlCategoria.SelectedIndex = 0;
            ddlOrden.SelectedIndex = 0;

            lblErrorFiltros.Text = "";
            pnlError.Visible = false;

            PaginaActual = 0;
            FiltrosActivos = false;

            pnlMensaje.Visible = false;

            cargarArticulos();
        }

        protected void btnAnterior_Click(object sender, EventArgs e)
        {
            if (PaginaActual > 0)
                PaginaActual--;

            recargarListadoActual();
        }

        protected void btnSiguiente_Click(object sender, EventArgs e)
        {
            PaginaActual++;

            recargarListadoActual();
        }

        private void mostrarArticulos(List<Articulo> lista)
        {
            PagedDataSource paginado = new PagedDataSource();

            paginado.DataSource = lista;
            paginado.AllowPaging = true;
            paginado.PageSize = 16;

            if (PaginaActual >= paginado.PageCount && paginado.PageCount > 0)
            {
                PaginaActual = paginado.PageCount - 1;
            }

            paginado.CurrentPageIndex = PaginaActual;

            repArticulos.DataSource = paginado;
            repArticulos.DataBind();

            btnAnterior.Enabled = !paginado.IsFirstPage;
            btnSiguiente.Enabled = !paginado.IsLastPage;

            if (lista.Count > 0)
            {
                lblPagina.Text = "Página " + (PaginaActual + 1) + " de " + paginado.PageCount;
                pnlPaginacion.Visible = paginado.PageCount > 1;
            }
            else
            {
                lblPagina.Text = "";
                pnlPaginacion.Visible = false;
            }

            pnlSinArticulos.Visible = lista.Count == 0;
            pnlError.Visible = false;
        }

        private void recargarListadoActual()
        {
            try
            {
                if (FiltrosActivos)
                    cargarArticulosFiltrados();
                else
                    cargarArticulos();
            }
            catch (Exception ex)
            {
                pnlError.Visible = true;
                lblError.Text = "No se pudo cambiar de página: " + ex.Message;
            }
        }

        protected void repArticulos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "AgregarFavorito")
                return;

            Usuario usuario = Seguridad.usuarioActual(Session);

            if (usuario == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            int idArticulo;

            if (!int.TryParse(e.CommandArgument.ToString(), out idArticulo))
            {
                pnlError.Visible = true;
                lblError.Text = "El identificador del artículo no es válido.";
                return;
            }

            FavoritoNegocio negocio = new FavoritoNegocio();

            try
            {
                negocio.agregarFavorito(usuario.Id, idArticulo);

                pnlError.Visible = false;

                mostrarMensaje("El artículo fue agregado a favoritos.", "success");
            }
            catch (Exception ex)
            {
                mostrarMensaje(ex.Message, "danger");
            }
        }

        private void mostrarMensaje(string mensaje, string tipo)
        {
            pnlMensaje.Visible = true;
            pnlMensaje.CssClass = "alert alert-" + tipo + " text-center shadow-sm";
            lblMensaje.Text = mensaje;
        }

    }
}