using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

                if (!string.IsNullOrWhiteSpace(articulo.Imagen))
                {
                    hfImagenActual.Value = articulo.Imagen;
                    imgPreview.ImageUrl = articulo.Imagen;

                    if (articulo.Imagen.StartsWith("http://") || articulo.Imagen.StartsWith("https://"))
                    {
                        // Es una URL externa.
                        txtImagen.Text = articulo.Imagen;
                    }
                    else
                    {
                        // Es un archivo local.
                        txtImagen.Text = "";
                    }
                }
                else
                {
                    hfImagenActual.Value = "";
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
                    string imagenAnterior = hfImagenActual.Value;
                    articulo.Id = IdArticulo;
                    negocio.modificar(articulo);
                    if (imagenAnterior != articulo.Imagen)
                    {
                        eliminarImagenFisica(imagenAnterior);
                    }
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


            string nuevaImagenArchivo = guardarImagenArchivo();

            if (!string.IsNullOrWhiteSpace(nuevaImagenArchivo))
            {
                // Se seleccionó un archivo nuevo.
                articulo.Imagen = nuevaImagenArchivo;
            }
            else if (!string.IsNullOrWhiteSpace(txtImagen.Text))
            {
                // Se ingresó una URL externa.
                articulo.Imagen = txtImagen.Text.Trim();
            }
            else if (!string.IsNullOrWhiteSpace(hfImagenActual.Value))
            {
                // Se conserva la imagen que ya tenía.
                articulo.Imagen = hfImagenActual.Value;
            }
            else
            {
                // No tiene imagen.
                articulo.Imagen = null;
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
                Articulo articulo = negocio.buscarPorId(IdArticulo);

                if (articulo == null)
                {
                    mostrarMensaje("No se encontró el artículo solicitado.", "danger");
                    return;
                }

                negocio.eliminar(IdArticulo);
                eliminarImagenFisica(articulo.Imagen);
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

            hfImagenActual.Value = "";

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

        private string guardarImagenArchivo()
        {
            if (!fuImagen.HasFile)
                return null;

            string extension = Path.GetExtension(fuImagen.FileName).ToLower();

            bool extensionValida = extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".webp";

            if (!extensionValida)
            {
                throw new Exception("La imagen debe tener formato JPG, JPEG, PNG o WEBP.");
            }

            int tamanioMaximo = 5 * 1024 * 1024;

            if (fuImagen.PostedFile.ContentLength > tamanioMaximo)
            {
                throw new Exception("La imagen no puede superar los 5 MB.");
            }

            string nombreArchivo = Guid.NewGuid().ToString() + extension;

            string carpetaFisica = Server.MapPath("~/Images/Articulos/");

            if (!Directory.Exists(carpetaFisica))
            {
                Directory.CreateDirectory(carpetaFisica);
            }

            string rutaFisica = Path.Combine(carpetaFisica, nombreArchivo);

            fuImagen.SaveAs(rutaFisica);

            return "~/Images/Articulos/" + nombreArchivo;
        }

        private void eliminarImagenFisica(string rutaImagen)
        {
            if (string.IsNullOrWhiteSpace(rutaImagen))
                return;

            if (!rutaImagen.StartsWith("~/Images/Articulos/"))
                return;

            string rutaFisica = Server.MapPath(rutaImagen);

            if (File.Exists(rutaFisica))
            {
                File.Delete(rutaFisica);
            }
        }

    }
}