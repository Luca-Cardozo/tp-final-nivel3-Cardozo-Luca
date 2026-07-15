<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Perfil.aspx.cs" Inherits="app_web.Perfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .contenedor-perfil {
            max-width: 850px;
            margin: 0 auto;
        }

        .imagen-perfil-preview {
            width: 220px;
            height: 220px;
            object-fit: cover;
            background-color: white;
            padding: 5px;
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

        const urlAvatarPlaceholder =
            '<%= UrlAvatarPlaceholder %>';

        function mostrarImagenPerfil(input) {

            const imagen =
                document.getElementById("imgPreviewPerfil");

            if (!input.files ||
                input.files.length === 0) {

                return;
            }

            const archivo = input.files[0];
            const lector = new FileReader();

            lector.onload = function (evento) {
                imagen.src = evento.target.result;
            };

            lector.readAsDataURL(archivo);
        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="contenedor-perfil">

        <div class="text-center mb-4">

            <h1>Mi perfil</h1>
            <p class="text-muted">Modificá tus datos personales.</p>

        </div>

        <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="alert text-center shadow-sm">

            <asp:Label ID="lblMensaje" runat="server"> </asp:Label>

        </asp:Panel>

        <div class="card shadow-sm">

            <div class="card-header bg-dark text-white text-center">

                <h5 class="mb-0">Datos del perfil</h5>

            </div>

            <div class="card-body p-4">

                <asp:ValidationSummary ID="validationSummaryPerfil" runat="server" ValidationGroup="Perfil" CssClass="alert alert-danger" HeaderText="Revisá los siguientes errores:" DisplayMode="BulletList" />

                <div class="row g-4">

                    <div class="col-lg-7">

                        <div class="row g-3">

                            <div class="col-12">

                                <label class="form-label">Email</label>

                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ReadOnly="true"> </asp:TextBox>

                                <small class="text-muted">El email no puede modificarse.</small>

                            </div>

                            <div class="col-md-6">

                                <label class="form-label">Nombre</label>

                                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" MaxLength="50" placeholder="Nombre opcional"> </asp:TextBox>

                            </div>

                            <div class="col-md-6">

                                <label class="form-label">Apellido</label>

                                <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" MaxLength="50" placeholder="Apellido opcional"> </asp:TextBox>

                            </div>

                            <div class="col-md-6">

                                <label class="form-label campo-obligatorio">Nueva contraseña</label>

                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" MaxLength="50" placeholder="Nueva contraseña"> </asp:TextBox>

                                <small class="text-muted">Deje este campo vacío si desea conservar su contraseña actual.</small>

                            </div>

                            <div class="col-md-6">

                                <label class="form-label campo-obligatorio">Confirmar nueva contraseña</label>

                                <asp:TextBox ID="txtConfirmarPassword" runat="server" CssClass="form-control" TextMode="Password" MaxLength="50" placeholder="Repita la nueva contraseña"> </asp:TextBox>

                                <asp:CompareValidator ID="cvPassword" runat="server" ControlToValidate="txtConfirmarPassword" ControlToCompare="txtPassword" Operator="Equal" Type="String" ErrorMessage="Las contraseñas no coinciden." Text="Las contraseñas no coinciden." CssClass="text-danger validator" ValidationGroup="Perfil" Display="Dynamic"> </asp:CompareValidator>

                            </div>

                            <div class="col-12">

                                <label class="form-label">Imagen de perfil</label>

                                <asp:FileUpload ID="fuImagenPerfil" runat="server" CssClass="form-control" ClientIDMode="Static" accept=".jpg,.jpeg,.png,.webp" onchange="mostrarImagenPerfil(this)" />

                                <small class="text-muted">Si no seleccionás una nueva imagen, se conservará la actual. Formatos permitidos: JPG, JPEG, PNG y WEBP. Tamaño máximo: 5 MB.</small>

                                <asp:HiddenField ID="hfImagenActual" runat="server" />

                            </div>

                        </div>

                    </div>

                    <div class="col-lg-5 text-center">

                        <label class="form-label d-block">Imagen actual</label>

                        <asp:Image ID="imgPreviewPerfil" runat="server" ClientIDMode="Static" CssClass="imagen-perfil-preview border rounded-circle" AlternateText="Imagen de perfil" />

                    </div>

                </div>

            </div>

            <div class="card-footer bg-white">

                <div class="d-flex justify-content-center flex-wrap gap-3 py-2">

                    <asp:Button ID="btnGuardar" runat="server" Text="💾 Guardar cambios" CssClass="btn btn-success btn-lg px-4" ValidationGroup="Perfil" OnClick="btnGuardar_Click" />

                    <a href="Home.aspx" class="btn btn-outline-secondary btn-lg px-4">↩ Volver</a>

                </div>

            </div>

        </div>

    </div>

</asp:Content>
