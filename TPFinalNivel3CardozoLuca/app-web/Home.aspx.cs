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
                cargarArticulos();
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

    }
}