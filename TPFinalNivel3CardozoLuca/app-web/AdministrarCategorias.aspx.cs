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
    public partial class AdministrarCategorias : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
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

            if (!IsPostBack)
            {
                cargarCategorias();
            }
        }

        private void cargarCategorias()
        {
            CategoriaNegocio negocio = new CategoriaNegocio();

            try
            {
                List<Categoria> lista = negocio.listar();

                dgvCategorias.DataSource = lista;
                dgvCategorias.DataBind();

                pnlMensaje.Visible = false;
            }
            catch (Exception ex)
            {
                mostrarMensaje("No se pudieron cargar las categorías: " + ex.Message, "danger");
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Page.Validate("Categoria");

            if (!Page.IsValid)
                return;

            CategoriaNegocio negocio = new CategoriaNegocio();

            try
            {
                string descripcion = txtDescripcion.Text.Trim();

                int idCategoria = int.Parse(hfIdCategoria.Value);

                if (idCategoria == 0)
                {
                    negocio.agregar(descripcion);
                    mostrarMensaje("La categoría fue agregada correctamente.", "success");
                }
                else
                {
                    Categoria categoria = new Categoria();

                    categoria.Id = idCategoria;
                    categoria.Descripcion = descripcion;

                    negocio.modificar(categoria);

                    mostrarMensaje("La categoría fue modificada correctamente.", "success");
                }

                limpiarFormulario();
                recargarGrilla();
            }
            catch (Exception ex)
            {
                mostrarMensaje(ex.Message, "danger");
            }
        }

        protected void dgvCategorias_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idCategoria;

            if (!int.TryParse(e.CommandArgument.ToString(), out idCategoria))
            {
                mostrarMensaje("El identificador de la categoría no es válido.", "danger");
                return;
            }

            if (e.CommandName == "EditarCategoria")
            {
                cargarCategoriaParaEditar(idCategoria);
            }
            else if (e.CommandName == "EliminarCategoria")
            {
                eliminarCategoria(idCategoria);
            }
        }

        private void cargarCategoriaParaEditar(int idCategoria)
        {
            CategoriaNegocio negocio = new CategoriaNegocio();

            try
            {
                List<Categoria> lista = negocio.listar();

                Categoria seleccionada = null;

                foreach (Categoria categoria in lista)
                {
                    if (categoria.Id == idCategoria)
                    {
                        seleccionada = categoria;
                        break;
                    }
                }

                if (seleccionada == null)
                {
                    mostrarMensaje("No se encontró la categoría seleccionada.", "danger");
                    return;
                }

                hfIdCategoria.Value = seleccionada.Id.ToString();
                txtDescripcion.Text = seleccionada.Descripcion;
                lblTituloFormulario.Text = "Modificar categoría";
                btnGuardar.Text = "💾 Guardar cambios";
                btnGuardar.CssClass = "btn btn-primary px-4";
                btnCancelar.Visible = true;
                pnlMensaje.Visible = false;
            }
            catch (Exception ex)
            {
                mostrarMensaje("No se pudo cargar la categoría: " + ex.Message, "danger");
            }
        }

        private void eliminarCategoria(int idCategoria)
        {
            CategoriaNegocio negocio = new CategoriaNegocio();

            try
            {
                negocio.eliminar(idCategoria);

                limpiarFormulario();
                recargarGrilla();

                mostrarMensaje("La categoría fue eliminada correctamente.", "success");
            }
            catch (Exception ex)
            {
                mostrarMensaje(ex.Message, "danger");
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiarFormulario();
            pnlMensaje.Visible = false;
        }

        protected void dgvCategorias_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvCategorias.PageIndex = e.NewPageIndex;
            cargarCategorias();
        }

        private void limpiarFormulario()
        {
            hfIdCategoria.Value = "0";
            txtDescripcion.Text = "";

            lblTituloFormulario.Text = "Agregar categoría";

            btnGuardar.Text = "➕ Agregar categoría";

            btnGuardar.CssClass = "btn btn-success px-4";

            btnCancelar.Visible = false;
        }

        private void recargarGrilla()
        {
            CategoriaNegocio negocio = new CategoriaNegocio();

            List<Categoria> lista = negocio.listar();

            dgvCategorias.DataSource = lista;
            dgvCategorias.DataBind();
        }

        private void mostrarMensaje(string mensaje, string tipo)
        {
            pnlMensaje.Visible = true;
            pnlMensaje.CssClass = "alert alert-" + tipo + " text-center shadow-sm";
            lblMensaje.Text = mensaje;
        }

    }
}