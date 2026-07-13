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
    public partial class DetalleArticulo : System.Web.UI.Page
    {
        private const string UrlPlaceholder = "https://media.istockphoto.com/id/1980276924/es/vector/sin-elemento-gr%C3%A1fico-en-miniatura-de-la-foto-no-se-ha-encontrado-ninguna-imagen-o-est%C3%A1.jpg?s=612x612&w=0&k=20&c=artWlQoi5R1edWQBv9LfzeLXupOcH_alZnMgvXdYkF4=";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargarArticulo();
            }
        }

        private void cargarArticulo()
        {
            int idArticulo;

            if (!int.TryParse(Request.QueryString["id"], out idArticulo))
            {
                mostrarError("El identificador del artículo no es válido.");
                return;
            }

            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                Articulo articulo = negocio.buscarPorId(idArticulo);

                if (articulo == null)
                {
                    mostrarError("No se encontró el artículo solicitado.");
                    return;
                }

                lblNombre.Text = articulo.Nombre;
                lblCodigo.Text = articulo.Codigo;
                lblMarca.Text = articulo.Marca.Descripcion;
                lblCategoria.Text = articulo.Categoria.Descripcion;
                if (articulo.Precio.HasValue)
                    lblPrecio.Text = articulo.Precio.Value.ToString("C2");
                else
                    lblPrecio.Text = "Precio no disponible";

                if (string.IsNullOrWhiteSpace(articulo.Descripcion))
                    lblDescripcion.Text = "Sin descripción disponible.";
                else
                    lblDescripcion.Text = articulo.Descripcion;

                if (string.IsNullOrWhiteSpace(articulo.Imagen))
                    imgArticulo.ImageUrl = UrlPlaceholder;
                else
                    imgArticulo.ImageUrl = articulo.Imagen;

                imgArticulo.Attributes["onerror"] = "this.onerror=null; this.src='" + UrlPlaceholder + "';";

                pnlDetalle.Visible = true;
            }
            catch (Exception ex)
            {
                mostrarError("No se pudo cargar el artículo: " + ex.Message);
            }
        }

        private void mostrarError(string mensaje)
        {
            pnlDetalle.Visible = false;
            pnlError.Visible = true;
            lblError.Text = mensaje;
        }

    }
}