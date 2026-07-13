using negocio;
using System;
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
                repArticulos.DataSource = negocio.listar();
                repArticulos.DataBind();
                pnlSinArticulos.Visible = repArticulos.Items.Count == 0;
                pnlError.Visible = false;
            }
            catch (Exception ex)
            {
                pnlError.Visible = true;
                lblError.Text = "No se pudieron cargar los artículos: " + ex.Message;
            }
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

            int idMarca = int.Parse(ddlMarca.SelectedValue);
            int idCategoria = int.Parse(ddlCategoria.SelectedValue);
            string orden = ddlOrden.SelectedValue;

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
                    lblErrorFiltros.Text ="El precio máximo ingresado no es válido.";
                    return;
                }
                precioMaximo = valor;
            }

            if (precioMinimo.HasValue && precioMaximo.HasValue && precioMinimo.Value > precioMaximo.Value)
            {
                lblErrorFiltros.Text = "El precio mínimo no puede ser mayor que el precio máximo.";
                return;
            }

            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                repArticulos.DataSource = negocio.filtrar(txtNombre.Text.Trim(), idMarca, idCategoria, precioMinimo, precioMaximo, orden);
                repArticulos.DataBind();
                pnlSinArticulos.Visible = repArticulos.Items.Count == 0;
                pnlError.Visible = false;
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

            cargarArticulos();
        }

    }
}