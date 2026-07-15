<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="app_web.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .contenedor-login {
            min-height: calc(100vh - 100px);
        }

        .card-login {
            width: 100%;
            max-width: 420px;
        }

        .validator {
            margin-top: 4px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="contenedor-login d-flex justify-content-center align-items-center">

        <div class="card card-login shadow">

            <div class="card-header bg-dark text-white text-center">

                <h2 class="mb-0">Iniciar sesión</h2>

            </div>

            <div class="card-body p-4">

                <asp:ValidationSummary ID="validationSummaryLogin" runat="server" ValidationGroup="Login" CssClass="alert alert-danger" HeaderText="Revisá los siguientes errores:" DisplayMode="BulletList" />

                <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert alert-danger text-center">

                    <asp:Label ID="lblError" runat="server"> </asp:Label>

                </asp:Panel>

                <div class="mb-3">

                    <label class="form-label">Email</label>

                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" MaxLength="100" placeholder="usuario@email.com"> </asp:TextBox>

                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="El email es obligatorio." Text="El email es obligatorio." CssClass="text-danger validator" ValidationGroup="Login" Display="Dynamic"> </asp:RequiredFieldValidator>

                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ErrorMessage="El formato del email no es válido." Text="El formato del email no es válido." CssClass="text-danger validator" ValidationGroup="Login" Display="Dynamic"> </asp:RegularExpressionValidator>

                </div>

                <div class="mb-3">

                    <label class="form-label">Contraseña</label>

                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" MaxLength="50" placeholder="Contraseña"> </asp:TextBox>

                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="La contraseña es obligatoria." Text="La contraseña es obligatoria." CssClass="text-danger validator" ValidationGroup="Login" Display="Dynamic"> </asp:RequiredFieldValidator>

                </div>

                <div class="d-grid gap-2">

                    <asp:Button ID="btnLogin" runat="server" Text="🔑 Ingresar" CssClass="btn btn-primary" ValidationGroup="Login" OnClick="btnLogin_Click" />

                    <a href="Home.aspx" class="btn btn-outline-secondary">🏠 Volver a la página principal</a>

                </div>

                <hr class="my-4" />

                <div class="text-center">

                    <p class="text-muted mb-2">¿Todavía no tenés una cuenta?</p>

                    <a href="RegistroUser.aspx" class="btn btn-success">📝 Crear cuenta</a>

                </div>

            </div>

        </div>

    </div>

</asp:Content>
