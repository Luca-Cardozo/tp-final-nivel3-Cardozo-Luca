using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace app_web
{
    public partial class FormularioArticulo : System.Web.UI.Page
    {
        public const string UrlPlaceholder = "https://media.istockphoto.com/id/1980276924/es/vector/sin-elemento-gr%C3%A1fico-en-miniatura-de-la-foto-no-se-ha-encontrado-ninguna-imagen-o-est%C3%A1.jpg?s=612x612&w=0&k=20&c=artWlQoi5R1edWQBv9LfzeLXupOcH_alZnMgvXdYkF4=";

        private int IdArticulo
        {
            get
            {
                int id;

                if (int.TryParse(Request.QueryString["id"], out id))
                    return id;

                return 0;
            }
        }

        private bool ModoEdicion
        {
            get
            {
                return IdArticulo > 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            validarAdministrador();

            if (!IsPostBack)
            {
                imgPreview.ImageUrl = UrlPlaceholder;
                cargarDropDownLists();
                configurarFormulario();
            }
        }

        private void validarAdministrador()
        {
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

        private void configurarFormulario()
        {
            if (ModoEdicion)
            {
                lblTitulo.Text = "Modificar artículo";

                lblSubtitulo.Text = "Modifique los datos o elimine el artículo seleccionado";

                btnGuardar.Text = "💾 Guardar cambios";
                btnEliminar.Visible = true;

                cargarArticulo();
            }
            else
            {
                lblTitulo.Text = "Agregar artículo";

                lblSubtitulo.Text = "Cargue los datos del nuevo artículo.";

                btnGuardar.Text = "💾 Agregar artículo";
                btnEliminar.Visible = false;
            }
        }

        private void cargarDropDownLists()
        {
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();

            try
            {
                ddlMarca.DataSource = marcaNegocio.listar();
                ddlMarca.DataValueField = "Id";
                ddlMarca.DataTextField = "Descripcion";
                ddlMarca.DataBind();

                ddlMarca.Items.Insert(0, new ListItem("Seleccione una marca", "0"));

                ddlCategoria.DataSource = categoriaNegocio.listar();
                ddlCategoria.DataValueField = "Id";
                ddlCategoria.DataTextField = "Descripcion";
                ddlCategoria.DataBind();

                ddlCategoria.Items.Insert(0, new ListItem("Seleccione una categoría", "0"));
            }
            catch (Exception ex)
            {
                mostrarMensaje("No se pudieron cargar las marcas y categorías: " + ex.Message, "danger");
                pnlFormulario.Visible = false;
            }
        }

        private void cargarArticulo()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                Articulo articulo = negocio.buscarPorId(IdArticulo);

                if (articulo == null)
                {
                    mostrarMensaje("No se encontró el artículo solicitado.", "danger");
                    pnlFormulario.Visible = false;
                    return;
                }

                txtCodigo.Text = articulo.Codigo;
                txtNombre.Text = articulo.Nombre;

                if (articulo.Descripcion != null)
                    txtDescripcion.Text = articulo.Descripcion;
                else
                    txtDescripcion.Text = "";

                ddlMarca.SelectedValue = articulo.Marca.Id.ToString();

                ddlCategoria.SelectedValue = articulo.Categoria.Id.ToString();

                if (articulo.Imagen != null)
                {
                    txtImagen.Text = articulo.Imagen;
                    imgPreview.ImageUrl = articulo.Imagen;
                }
                else
                {
                    txtImagen.Text = "";
                    imgPreview.ImageUrl = UrlPlaceholder;
                }

                if (articulo.Precio.HasValue)
                {
                    txtPrecio.Text = articulo.Precio.ToString();
                }
                else
                {
                    txtPrecio.Text = "";
                }
            }
            catch (Exception ex)
            {
                mostrarMensaje("No se pudo cargar el artículo: " + ex.Message, "danger");
                pnlFormulario.Visible = false;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Page.Validate("Articulo");

            if (!Page.IsValid)
                return;

            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                Articulo articulo = construirArticulo();

                if (ModoEdicion)
                {
                    articulo.Id = IdArticulo;
                    negocio.modificar(articulo);
                    mostrarMensaje("El artículo fue modificado correctamente. Será redirigido al listado en 3 segundos...", "success");
                    redirigirAlListado();
                }
                else
                {
                    negocio.agregar(articulo);
                    mostrarMensaje("El artículo fue agregado correctamente. Será redirigido al listado en 3 segundos...", "success");
                    limpiarFormulario();
                    redirigirAlListado();
                }
            }
            catch (Exception ex)
            {
                mostrarMensaje(ex.Message, "danger");
            }
        }

        private Articulo construirArticulo()
        {
            Articulo articulo = new Articulo();

            articulo.Codigo = txtCodigo.Text.Trim().ToUpper();
            articulo.Nombre = txtNombre.Text.Trim();

            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                articulo.Descripcion = null;
            }
            else
            {
                articulo.Descripcion = txtDescripcion.Text.Trim();
            }

            articulo.Marca = new Marca();
            articulo.Marca.Id = int.Parse(ddlMarca.SelectedValue);

            articulo.Categoria = new Categoria();
            articulo.Categoria.Id = int.Parse(ddlCategoria.SelectedValue);

            if (string.IsNullOrWhiteSpace(txtImagen.Text))
            {
                articulo.Imagen = null;
            }
            else
            {
                articulo.Imagen = txtImagen.Text.Trim();
            }


            if (string.IsNullOrWhiteSpace(txtPrecio.Text))
            {
                articulo.Precio = null;
            }
            else
            {
                articulo.Precio = decimal.Parse(txtPrecio.Text);
            }
            return articulo;
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            if (!ModoEdicion)
            {
                mostrarMensaje("No se indicó un artículo para eliminar.", "danger");
                return;
            }

            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                negocio.eliminar(IdArticulo);
                mostrarMensaje("El artículo fue eliminado permanentemente. Será redirigido al listado en 3 segundos...", "success");
                pnlFormulario.Visible = false;
                redirigirAlListado();
            }
            catch (Exception ex)
            {
                mostrarMensaje("No se pudo eliminar el artículo: " + ex.Message, "danger");
            }
        }

        private void limpiarFormulario()
        {
            txtCodigo.Text = "";
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            txtImagen.Text = "";
            txtPrecio.Text = "";

            ddlMarca.SelectedIndex = 0;
            ddlCategoria.SelectedIndex = 0;

            imgPreview.ImageUrl = UrlPlaceholder;
        }

        private void mostrarMensaje(string mensaje, string tipo)
        {
            pnlMensaje.Visible = true;
            pnlMensaje.CssClass = "alert alert-" + tipo + " text-center shadow-sm";
            lblMensaje.Text = mensaje;
        }

        private void redirigirAlListado()
        {
            ClientScript.RegisterStartupScript(
                this.GetType(),
                "redirigir",
                "setTimeout(function(){ window.location='AdministrarArticulos.aspx'; }, 3000);",
                true
            );
        }

    }
}