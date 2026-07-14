<%@ Page Title="Administrar marcas" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="AdministrarMarcas.aspx.cs" Inherits="app_web.AdministrarMarcas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .contenedor-marcas {
            max-width: 850px;
            margin: 0 auto;
        }

        .tabla-marcas {
            text-align: center;
        }

            .tabla-marcas th,
            .tabla-marcas td {
                text-align: center;
                vertical-align: middle;
            }

        .campo-obligatorio::after {
            content: " *";
            color: #dc3545;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="contenedor-marcas">

        <div class="text-center mb-4">

            <h1 class="mb-1">Administrar marcas</h1>

            <p class="text-muted mb-0">
                Agregar, modificar y eliminar marcas del catálogo
            </p>

        </div>

        <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="alert text-center shadow-sm">
            <asp:Label ID="lblMensaje" runat="server"></asp:Label>
        </asp:Panel>

        <div class="card shadow-sm mb-4">

            <div class="card-header bg-dark text-white text-center">
                <asp:Label ID="lblTituloFormulario" runat="server" Text="Agregar marca" CssClass="h5 mb-0"></asp:Label>
            </div>

            <div class="card-body">

                <asp:HiddenField ID="hfIdMarca" runat="server" Value="0" />

                <div class="row justify-content-center">

                    <div class="col-12 col-md-7">

                        <label class="form-label campo-obligatorio">Descripción</label>
                        <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" MaxLength="50" placeholder="Ej: Samsung"> </asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvDescripcion" runat="server" ControlToValidate="txtDescripcion" ErrorMessage="La descripción es obligatoria." Text="La descripción es obligatoria." CssClass="text-danger mt-1" ValidationGroup="Marca" Display="Dynamic"></asp:RequiredFieldValidator>

                    </div>

                </div>

                <div class="d-flex justify-content-center flex-wrap gap-3 mt-4">

                    <asp:Button ID="btnGuardar" runat="server" Text="➕ Agregar marca" CssClass="btn btn-success px-4" ValidationGroup="Marca" OnClick="btnGuardar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="↩ Cancelar edición" CssClass="btn btn-outline-secondary px-4" CausesValidation="false" Visible="false" OnClick="btnCancelar_Click" />

                </div>

            </div>

        </div>

        <div class="table-responsive">

            <asp:GridView ID="dgvMarcas" runat="server" AutoGenerateColumns="false"
                CssClass="table table-striped table-hover table-bordered tabla-marcas"
                DataKeyNames="Id" EmptyDataText="No hay marcas registradas."
                AllowPaging="true" PageSize="5" OnPageIndexChanging="dgvMarcas_PageIndexChanging"
                OnRowCommand="dgvMarcas_RowCommand">

                <columns>

                    <asp:BoundField DataField="Id" HeaderText="ID" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                    <asp:TemplateField HeaderText="Acciones">

                        <itemstyle horizontalalign="Center" width="260px" />

                        <itemtemplate>

                            <div class="d-flex justify-content-center gap-2">

                                <asp:LinkButton ID="btnEditar" runat="server" Text="✏️ Editar" CssClass="btn btn-warning btn-sm" CommandName="EditarMarca" CommandArgument='<%# Eval("Id") %>' CausesValidation="false"> </asp:LinkButton>
                                <asp:LinkButton ID="btnEliminar" runat="server" Text="🗑️ Eliminar" CssClass="btn btn-danger btn-sm" CommandName="EliminarMarca" CommandArgument='<%# Eval("Id") %>' CausesValidation="false" OnClientClick="return confirm('¿Está seguro de que desea eliminar permanentemente esta marca?');"> </asp:LinkButton>

                            </div>

                        </itemtemplate>

                    </asp:TemplateField>

                </columns>

                <pagerstyle horizontalalign="Center" cssclass="text-center" />

            </asp:GridView>

        </div>

    </div>

</asp:Content>
