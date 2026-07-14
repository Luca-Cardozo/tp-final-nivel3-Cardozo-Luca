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
    public partial class AdministrarMarcas : System.Web.UI.Page
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
                cargarMarcas();
            }
        }

        private void cargarMarcas()
        {
            MarcaNegocio negocio = new MarcaNegocio();

            try
            {
                List<Marca> lista = negocio.listar();

                dgvMarcas.DataSource = lista;
                dgvMarcas.DataBind();

                pnlMensaje.Visible = false;
            }
            catch (Exception ex)
            {
                mostrarMensaje("No se pudieron cargar las marcas: " + ex.Message, "danger");
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Page.Validate("Marca");

            if (!Page.IsValid)
                return;

            MarcaNegocio negocio = new MarcaNegocio();

            try
            {
                string descripcion = txtDescripcion.Text.Trim();

                int idMarca = int.Parse(hfIdMarca.Value);

                if (idMarca == 0)
                {
                    negocio.agregar(descripcion);
                    mostrarMensaje("La marca fue agregada correctamente.", "success");
                }
                else
                {
                    Marca marca = new Marca();

                    marca.Id = idMarca;
                    marca.Descripcion = descripcion;

                    negocio.modificar(marca);

                    mostrarMensaje("La marca fue modificada correctamente.", "success");
                }

                limpiarFormulario();
                recargarGrilla();
            }
            catch (Exception ex)
            {
                mostrarMensaje(ex.Message, "danger");
            }
        }

        protected void dgvMarcas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idMarca;

            if (!int.TryParse(e.CommandArgument.ToString(), out idMarca))
            {
                mostrarMensaje("El identificador de la marca no es válido.", "danger");
                return;
            }

            if (e.CommandName == "EditarMarca")
            {
                cargarMarcaParaEditar(idMarca);
            }
            else if (e.CommandName == "EliminarMarca")
            {
                eliminarMarca(idMarca);
            }
        }

        private void cargarMarcaParaEditar(int idMarca)
        {
            MarcaNegocio negocio = new MarcaNegocio();

            try
            {
                List<Marca> lista = negocio.listar();

                Marca seleccionada = null;

                foreach (Marca marca in lista)
                {
                    if (marca.Id == idMarca)
                    {
                        seleccionada = marca;
                        break;
                    }
                }

                if (seleccionada == null)
                {
                    mostrarMensaje("No se encontró la marca seleccionada.", "danger");
                    return;
                }

                hfIdMarca.Value = seleccionada.Id.ToString();
                txtDescripcion.Text = seleccionada.Descripcion;
                lblTituloFormulario.Text = "Modificar marca";
                btnGuardar.Text = "💾 Guardar cambios";
                btnGuardar.CssClass = "btn btn-primary px-4";
                btnCancelar.Visible = true;
                pnlMensaje.Visible = false;
            }
            catch (Exception ex)
            {
                mostrarMensaje("No se pudo cargar la marca: " + ex.Message, "danger");
            }
        }

        private void eliminarMarca(int idMarca)
        {
            MarcaNegocio negocio = new MarcaNegocio();

            try
            {
                negocio.eliminar(idMarca);

                limpiarFormulario();
                recargarGrilla();

                mostrarMensaje("La marca fue eliminada correctamente.", "success");
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

        protected void dgvMarcas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvMarcas.PageIndex = e.NewPageIndex;
            cargarMarcas();
        }

        private void limpiarFormulario()
        {
            hfIdMarca.Value = "0";
            txtDescripcion.Text = "";

            lblTituloFormulario.Text = "Agregar marca";

            btnGuardar.Text = "➕ Agregar marca";

            btnGuardar.CssClass = "btn btn-success px-4";

            btnCancelar.Visible = false;
        }

        private void recargarGrilla()
        {
            MarcaNegocio negocio = new MarcaNegocio();

            List<Marca> lista = negocio.listar();

            dgvMarcas.DataSource = lista;
            dgvMarcas.DataBind();
        }

        private void mostrarMensaje(string mensaje, string tipo)
        {
            pnlMensaje.Visible = true;
            pnlMensaje.CssClass = "alert alert-" + tipo + " text-center shadow-sm";
            lblMensaje.Text = mensaje;
        }

    }
}