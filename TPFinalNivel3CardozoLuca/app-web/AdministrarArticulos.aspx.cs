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
    public partial class AdministrarArticulos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            validarAdministrador();

            if (!IsPostBack)
            {
                cargarFiltros();
                cargarArticulos();
            }
        }

        private void validarAdministrador()
        {
            Usuario usuario = Seguridad.usuarioActual(Session);

            if (!Seguridad.sesionActiva(Session["usuario"]))
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!Seguridad.esAdmin(Session))
            {
                Response.Redirect("AccesoDenegado.aspx");
                return;
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
                mostrarError("No se pudieron cargar los filtros: " + ex.Message);
            }
        }

        private void cargarArticulos()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                List<Articulo> lista = negocio.listar();

                dgvArticulos.DataSource = lista;
                dgvArticulos.DataBind();

                pnlSinArticulos.Visible = lista.Count == 0;
                pnlError.Visible = false;
            }
            catch (Exception ex)
            {
                mostrarError("No se pudieron cargar los artículos: " + ex.Message);
            }
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

            dgvArticulos.PageIndex = 0;

            try
            {
                cargarArticulosFiltrados();
                pnlError.Visible = false;
            }
            catch (Exception ex)
            {
                mostrarError("No se pudo realizar la búsqueda: " + ex.Message);
            }
        }

        protected void btnRecargar_Click(object sender, EventArgs e)
        {
            txtCodigo.Text = "";
            txtNombre.Text = "";
            txtPrecioMinimo.Text = "";
            txtPrecioMaximo.Text = "";

            ddlMarca.SelectedIndex = 0;
            ddlCategoria.SelectedIndex = 0;
            ddlOrden.SelectedIndex = 0;

            lblErrorFiltros.Text = "";
            pnlError.Visible = false;

            dgvArticulos.PageIndex = 0;
            cargarArticulos();
        }

        protected void btnAgregarArticulo_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormularioArticulo.aspx");
            return;
        }

        protected void dgvArticulos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "VerDetalle")
            {
                int idArticulo = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("FormularioArticulo.aspx?id=" + idArticulo);
                return;
            }
        }

        public string mostrarPrecio(object precio)
        {
            if (precio == null || precio == DBNull.Value)
            {
                return "Sin precio";
            }
            decimal valor = (decimal)precio;
            return valor.ToString("C2");
        }

        private void mostrarError(string mensaje)
        {
            pnlError.Visible = true;
            lblError.Text = mensaje;
        }

        private void cargarArticulosFiltrados()
        {
            int idMarca = int.Parse(ddlMarca.SelectedValue);
            int idCategoria = int.Parse(ddlCategoria.SelectedValue);

            decimal? precioMinimo = null;
            decimal? precioMaximo = null;

            decimal valor;

            if (!string.IsNullOrWhiteSpace(txtPrecioMinimo.Text) && decimal.TryParse(txtPrecioMinimo.Text, out valor))
            {
                precioMinimo = valor;
            }

            if (!string.IsNullOrWhiteSpace(txtPrecioMaximo.Text) && decimal.TryParse(txtPrecioMaximo.Text, out valor))
            {
                precioMaximo = valor;
            }

            ArticuloNegocio negocio = new ArticuloNegocio();

            List<Articulo> lista = negocio.filtrar(txtCodigo.Text.Trim(), txtNombre.Text.Trim(), idMarca, idCategoria, precioMinimo, precioMaximo, ddlOrden.SelectedValue);

            dgvArticulos.DataSource = lista;
            dgvArticulos.DataBind();

            pnlSinArticulos.Visible = lista.Count == 0;
        }

        protected void dgvArticulos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvArticulos.PageIndex = e.NewPageIndex;
            try
            {
                cargarArticulosFiltrados();
            }
            catch (Exception ex)
            {
                mostrarError("No se pudo cambiar de página: " + ex.Message);
            }
        }

    }
}