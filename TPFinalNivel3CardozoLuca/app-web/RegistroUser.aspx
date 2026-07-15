<%@ Page Title="Registrarse" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="RegistroUser.aspx.cs" Inherits="app_web.RegistroUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .contenedor-registro {
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

        const urlAvatarPlaceholder = '<%= UrlAvatarPlaceholder %>';

        function mostrarImagenPerfil(input) {

            const imagen = document.getElementById("imgPreviewPerfil");

            if (!input.files || input.files.length === 0) {

                imagen.src = urlAvatarPlaceholder;
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

    <div class="contenedor-registro">

        <div class="text-center mb-4">

            <h1>Crear una cuenta</h1>

            <p class="text-muted">
                Registrate para guardar tus artículos favoritos.
            </p>

        </div>

        <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="alert text-center shadow-sm">
            <asp:Label ID="lblMensaje" runat="server"> </asp:Label>
        </asp:Panel>

        <div class="card shadow-sm">

            <div class="card-header bg-dark text-white text-center">
                <h5 class="mb-0">Datos del usuario</h5>
            </div>

            <div class="card-body p-4">

                <asp:ValidationSummary ID="validationSummaryRegistro" runat="server" ValidationGroup="Registro" CssClass="alert alert-danger" HeaderText="Revisá los siguientes errores:" DisplayMode="BulletList" />

                <div class="row g-4">

                    <div class="col-lg-7">

                        <div class="row g-3">

                            <div class="col-12">

                                <label class="form-label campo-obligatorio">Email</label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" MaxLength="100" placeholder="usuario@email.com"> </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="El email es obligatorio." Text="El email es obligatorio." CssClass="text-danger validator" ValidationGroup="Registro" Display="Dynamic"> </asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ErrorMessage="El formato del email no es válido." Text="El formato del email no es válido." CssClass="text-danger validator" ValidationGroup="Registro" Display="Dynamic"> </asp:RegularExpressionValidator>

                            </div>

                            <div class="col-md-6">

                                <label class="form-label campo-obligatorio">Contraseña</label>
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" MaxLength="50" placeholder="Contraseña"> </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="La contraseña es obligatoria." Text="La contraseña es obligatoria." CssClass="text-danger validator" ValidationGroup="Registro" Display="Dynamic"> </asp:RequiredFieldValidator>

                            </div>

                            <div class="col-md-6">

                                <label class="form-label campo-obligatorio">Confirmar contraseña</label>
                                <asp:TextBox ID="txtConfirmarPassword" runat="server" CssClass="form-control" TextMode="Password" MaxLength="50" placeholder="Repita la contraseña"> </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvConfirmarPassword" runat="server" ControlToValidate="txtConfirmarPassword" ErrorMessage="Debe confirmar la contraseña." Text="Debe confirmar la contraseña." CssClass="text-danger validator" ValidationGroup="Registro" Display="Dynamic"> </asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvPassword" runat="server" ControlToValidate="txtConfirmarPassword" ControlToCompare="txtPassword" Operator="Equal" Type="String" ErrorMessage="Las contraseñas no coinciden." Text="Las contraseñas no coinciden." CssClass="text-danger validator" ValidationGroup="Registro" Display="Dynamic"> </asp:CompareValidator>

                            </div>

                            <div class="col-md-6">

                                <label class="form-label">Nombre</label>
                                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" MaxLength="50" placeholder="Nombre opcional"> </asp:TextBox>

                            </div>

                            <div class="col-md-6">

                                <label class="form-label">Apellido</label>
                                <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" MaxLength="50" placeholder="Apellido opcional"> </asp:TextBox>

                            </div>

                            <div class="col-12">

                                <label class="form-label">Imagen de perfil</label>
                                <asp:FileUpload ID="fuImagenPerfil" runat="server" CssClass="form-control" ClientIDMode="Static" accept=".jpg,.jpeg,.png,.webp" onchange="mostrarImagenPerfil(this)" />
                                <small class="text-muted">Formatos permitidos: JPG, JPEG, PNG y WEBP. Tamaño máximo: 5 MB.</small>

                            </div>

                        </div>

                    </div>

                    <div class="col-lg-5 text-center">

                        <label class="form-label d-block">Vista previa</label>
                        <asp:Image ID="imgPreviewPerfil" runat="server" ClientIDMode="Static" CssClass="imagen-perfil-preview border rounded-circle" AlternateText="Vista previa de la imagen de perfil" />

                    </div>

                </div>

            </div>

            <div class="card-footer bg-white">

                <div class="d-flex justify-content-center flex-wrap gap-3 py-2">

                    <asp:Button ID="btnRegistrar" runat="server" Text="📝 Crear cuenta" CssClass="btn btn-success btn-lg px-4" ValidationGroup="Registro" OnClick="btnRegistrar_Click" />
                    <a href="Login.aspx" class="btn btn-outline-secondary btn-lg px-4">Ya tengo una cuenta</a>

                </div>

            </div>

        </div>

    </div>

</asp:Content>
