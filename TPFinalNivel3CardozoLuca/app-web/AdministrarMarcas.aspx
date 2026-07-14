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

        <div class="card shadow-sm mb-4">

            <div class="card-header bg-dark text-white text-center">
                <h5 class="mb-0">Buscar y ordenar</h5>
            </div>

            <div class="card-body">

                <div class="row g-3 justify-content-center">

                    <div class="col-12 col-md-5">
                        <label class="form-label">Descripción</label>
                        <asp:TextBox ID="txtFiltroDescripcion" runat="server" CssClass="form-control" placeholder="Buscar por descripción"> </asp:TextBox>
                    </div>

                    <div class="col-12 col-md-3">
                        <label class="form-label">Ordenar por</label>
                        <asp:DropDownList ID="ddlOrden" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Sin orden" Value="" />
                            <asp:ListItem Text="Descripción (A-Z)" Value="Asc" />
                            <asp:ListItem Text="Descripción (Z-A)" Value="Desc" />
                        </asp:DropDownList>
                    </div>

                </div>

                <div class="d-flex justify-content-center gap-3 mt-4">

                    <asp:Button ID="btnBuscar" runat="server" Text="🔍 Buscar" CssClass="btn btn-primary px-4" CausesValidation="false" OnClick="btnBuscar_Click" />
                    <asp:Button ID="btnRecargar" runat="server" Text="🔄 Recargar" CssClass="btn btn-outline-secondary px-4" CausesValidation="false" OnClick="btnRecargar_Click" />

                </div>

            </div>

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
                        <asp:RequiredFieldValidator ID="rfvDescripcion" runat="server" ControlToValidate="txtDescripcion" ErrorMessage="La descripción es obligatoria." Text="La descripción es obligatoria." CssClass="text-danger mt-1" ValidationGroup="Marca" Display="Dynamic" EnableClientScript="false"></asp:RequiredFieldValidator>

                    </div>

                </div>

                <div class="d-flex justify-content-center flex-wrap gap-3 mt-4">

                    <asp:Button ID="btnGuardar" runat="server" Text="➕ Agregar marca" CssClass="btn btn-success px-4" ValidationGroup="Marca" CausesValidation="true" OnClick="btnGuardar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="↩ Cancelar edición" CssClass="btn btn-outline-secondary px-4" CausesValidation="false" Visible="false" OnClick="btnCancelar_Click" />

                </div>

            </div>

        </div>

        <div class="table-responsive">

            <asp:GridView ID="dgvMarcas" runat="server" AutoGenerateColumns="false"
                CssClass="table table-striped table-hover table-bordered tabla-marcas"
                DataKeyNames="Id" EmptyDataText="No hay marcas registradas."
                AllowPaging="true" PageSize="10" OnPageIndexChanging="dgvMarcas_PageIndexChanging"
                OnRowCommand="dgvMarcas_RowCommand">

                <Columns>

                    <asp:BoundField DataField="Id" HeaderText="ID" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                    <asp:TemplateField HeaderText="Acciones">

                        <ItemStyle HorizontalAlign="Center" Width="260px" />

                        <ItemTemplate>

                            <div class="d-flex justify-content-center gap-2">

                                <asp:LinkButton ID="btnEditar" runat="server" Text="✏️ Editar" CssClass="btn btn-warning btn-sm" CommandName="EditarMarca" CommandArgument='<%# Eval("Id") %>' CausesValidation="false"> </asp:LinkButton>
                                <asp:LinkButton ID="btnEliminar" runat="server" Text="🗑️ Eliminar" CssClass="btn btn-danger btn-sm" CommandName="EliminarMarca" CommandArgument='<%# Eval("Id") %>' CausesValidation="false" OnClientClick="return confirm('¿Está seguro de que desea eliminar permanentemente esta marca?');"> </asp:LinkButton>

                            </div>

                        </ItemTemplate>

                    </asp:TemplateField>

                </Columns>

                <PagerStyle HorizontalAlign="Center" CssClass="text-center" />

            </asp:GridView>

        </div>

    </div>

</asp:Content>
