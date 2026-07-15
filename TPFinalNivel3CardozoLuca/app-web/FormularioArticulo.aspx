<%@ Page Title="Formulario de artículo" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="FormularioArticulo.aspx.cs" Inherits="app_web.FormularioArticulo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .contenedor-formulario {
            max-width: 950px;
            margin: 0 auto;
        }

        .imagen-preview {
            width: 100%;
            height: 300px;
            object-fit: contain;
            background-color: white;
            padding: 15px;
        }

        .campo-obligatorio::after {
            content: " *";
            color: #dc3545;
        }

        .validator {
            margin-top: 4px;
        }
    </style>

    <script type="text/javascript">

        const urlPlaceholder = '<%= UrlPlaceholder %>';

        function actualizarVistaPreviaUrl(url) {

            const imagen = document.getElementById("imgPreview");
            const archivo = document.getElementById("fuImagen");

            // Si el usuario escribe una URL se limpia cualquier archivo seleccionado.
            if (url && url.trim() !== "") {
                archivo.value = "";
            }

            if (!url || url.trim() === "") {
                imagen.src = urlPlaceholder;
                return;
            }

            imagen.onerror = function () {
                this.onerror = null;
                this.src = urlPlaceholder;
            };

            imagen.src = url.trim();
        }

        function actualizarVistaPreviaArchivo(input) {

            const imagen = document.getElementById("imgPreview");
            const txtUrl = document.getElementById("txtImagen");

            if (!input.files || input.files.length === 0) {
                return;
            }

            // Si se selecciona un archivo, se limpia la URL externa.
            txtUrl.value = "";

            const archivo = input.files[0];
            const lector = new FileReader();

            lector.onload = function (evento) {
                imagen.src = evento.target.result;
            };

            lector.readAsDataURL(archivo);
        }

        document.addEventListener("DOMContentLoaded", function () {

            const imagen = document.getElementById("imgPreview");

            imagen.onerror = function () {
                this.onerror = null;
                this.src = urlPlaceholder;
            };

        });

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="contenedor-formulario">

        <div class="text-center mb-4">
            <asp:Label ID="lblTitulo" runat="server" Text="Agregar artículo" CssClass="h1 d-block"> </asp:Label>
            <asp:Label ID="lblSubtitulo" runat="server" Text="Cargue los datos del nuevo artículo" CssClass="text-muted"> </asp:Label>
        </div>

        <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="alert text-center shadow-sm">
            <asp:Label ID="lblMensaje" runat="server"> </asp:Label>
        </asp:Panel>

        <asp:Panel ID="pnlFormulario" runat="server">

            <div class="card shadow-sm">

                <div class="card-header bg-dark text-white text-center">
                    <h5 class="mb-0">Datos del artículo</h5>
                </div>

                <div class="card-body p-4">

                    <asp:ValidationSummary ID="validationSummaryArticulo" runat="server" ValidationGroup="Articulo" CssClass="alert alert-danger" HeaderText="Revisá los siguientes errores:" DisplayMode="BulletList" />

                    <div class="row g-4">

                        <div class="col-lg-7">

                            <div class="row g-3">

                                <div class="col-md-4">

                                    <label class="form-label campo-obligatorio">Código</label>
                                    <asp:TextBox ID="txtCodigo" runat="server" CssClass="form-control" MaxLength="3" placeholder="Ej: A12"> </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvCodigo" runat="server" ControlToValidate="txtCodigo" ErrorMessage="El código es obligatorio." Text="El código es obligatorio." CssClass="text-danger validator" ValidationGroup="Articulo" Display="Dynamic"> </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revCodigo" runat="server" ControlToValidate="txtCodigo" ValidationExpression="^[A-Za-z][0-9]{2}$" ErrorMessage="El código debe tener una letra seguida de dos números. Ejemplo: A12." Text="Formato inválido. Ejemplo: A12." CssClass="text-danger validator" ValidationGroup="Articulo" Display="Dynamic"> </asp:RegularExpressionValidator>

                                </div>

                                <div class="col-md-8">

                                    <label class="form-label campo-obligatorio">Nombre</label>
                                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" MaxLength="50" placeholder="Nombre del artículo"> </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ControlToValidate="txtNombre" ErrorMessage="El nombre es obligatorio." Text="El nombre es obligatorio." CssClass="text-danger validator" ValidationGroup="Articulo" Display="Dynamic"> </asp:RequiredFieldValidator>

                                </div>

                                <div class="col-md-6">

                                    <label class="form-label campo-obligatorio">Marca</label>
                                    <asp:DropDownList ID="ddlMarca" runat="server" CssClass="form-select"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvMarca" runat="server" ControlToValidate="ddlMarca" InitialValue="0" ErrorMessage="Debe seleccionar una marca." Text="Debe seleccionar una marca." CssClass="text-danger validator" ValidationGroup="Articulo" Display="Dynamic"> </asp:RequiredFieldValidator>

                                </div>

                                <div class="col-md-6">

                                    <label class="form-label campo-obligatorio">Categoría</label>
                                    <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-select"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvCategoria" runat="server" ControlToValidate="ddlCategoria" InitialValue="0" ErrorMessage="Debe seleccionar una categoría." Text="Debe seleccionar una categoría." CssClass="text-danger validator" ValidationGroup="Articulo" Display="Dynamic"> </asp:RequiredFieldValidator>

                                </div>

                                <div class="col-12">

                                    <label class="form-label">Descripción</label>
                                    <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" MaxLength="150" placeholder="Descripción opcional del artículo"> </asp:TextBox>

                                </div>

                                <div class="col-12">

                                    <label class="form-label">URL de la imagen</label>
                                    <asp:TextBox ID="txtImagen" runat="server" ClientIDMode="Static" CssClass="form-control" placeholder="https://ejemplo.com/imagen.jpg" oninput="actualizarVistaPreviaUrl(this.value)"> </asp:TextBox>
                                    <small class="text-muted">Podés pegar una URL externa o seleccionar un archivo debajo.</small>

                                </div>

                                <div class="col-12">

                                    <label class="form-label">Imagen desde archivo</label>
                                    <asp:FileUpload ID="fuImagen" runat="server" ClientIDMode="Static" CssClass="form-control" accept=".jpg,.jpeg,.png,.webp" onchange="actualizarVistaPreviaArchivo(this)" />
                                    <small class="text-muted">Formatos permitidos: JPG, JPEG, PNG y WEBP. Tamaño máximo: 5 MB.</small>
                                    <asp:HiddenField ID="hfImagenActual" runat="server" />

                                </div>

                                <div class="col-md-6">

                                    <label class="form-label">Precio</label>

                                    <div class="input-group">

                                        <span class="input-group-text">$</span>

                                        <asp:TextBox ID="txtPrecio" runat="server" CssClass="form-control" TextMode="Number" min="0" placeholder="Precio opcional"> </asp:TextBox>

                                    </div>

                                </div>

                            </div>

                        </div>

                        <div class="col-lg-5">

                            <label class="form-label d-block text-center">Vista previa</label>

                            <asp:Image ID="imgPreview" runat="server" CssClass="imagen-preview border rounded" AlternateText="Vista previa del artículo" ClientIDMode="Static" />
                            <p class="text-muted small text-center mt-2">
                                La vista previa se actualizará al escribir una URL o seleccionar un archivo.
                            </p>

                        </div>

                    </div>

                </div>

                <div class="card-footer bg-white">

                    <div class="d-flex justify-content-center flex-wrap gap-3 py-2">

                        <asp:Button ID="btnGuardar" runat="server" Text="💾 Agregar artículo" CssClass="btn btn-success btn-lg px-4" ValidationGroup="Articulo" OnClick="btnGuardar_Click" />
                        <asp:Button ID="btnEliminar" runat="server" Text="🗑️ Eliminar artículo" CssClass="btn btn-danger btn-lg px-4" Visible="false" CausesValidation="false" OnClientClick="return confirm('¿Está seguro de que desea eliminar permanentemente este artículo? Esta acción no se puede deshacer.');" OnClick="btnEliminar_Click" />
                        <a href="AdministrarArticulos.aspx" class="btn btn-outline-secondary btn-lg px-4">↩ Volver </a>

                    </div>

                </div>

            </div>

        </asp:Panel>

    </div>

</asp:Content>
