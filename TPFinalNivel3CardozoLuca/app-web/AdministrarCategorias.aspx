<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="AdministrarCategorias.aspx.cs" Inherits="app_web.AdministrarCategorias" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .contenedor-categorias {
            max-width: 850px;
            margin: 0 auto;
        }

        .tabla-categorias {
            text-align: center;
        }

            .tabla-categorias th,
            .tabla-categorias td {
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

    <div class="contenedor-categorias">

        <div class="text-center mb-4">

            <h1 class="mb-1">Administrar categorías</h1>

            <p class="text-muted mb-0">
                Agregar, modificar y eliminar categorías del catálogo
            </p>

        </div>

        <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="alert text-center shadow-sm">
            <asp:Label ID="lblMensaje" runat="server"></asp:Label>
        </asp:Panel>

        <div class="card shadow-sm mb-4">

            <div class="card-header bg-dark text-white text-center">
                <asp:Label ID="lblTituloFormulario" runat="server" Text="Agregar categoría" CssClass="h5 mb-0"></asp:Label>
            </div>

            <div class="card-body">

                <asp:HiddenField ID="hfIdCategoria" runat="server" Value="0" />

                <div class="row justify-content-center">

                    <div class="col-12 col-md-7">

                        <label class="form-label campo-obligatorio">Descripción</label>
                        <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" MaxLength="50" placeholder="Ej: Celulares"> </asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvDescripcion" runat="server" ControlToValidate="txtDescripcion" ErrorMessage="La descripción es obligatoria." Text="La descripción es obligatoria." CssClass="text-danger mt-1" ValidationGroup="Categoria" Display="Dynamic" EnableClientScript="false"></asp:RequiredFieldValidator>

                    </div>

                </div>

                <div class="d-flex justify-content-center flex-wrap gap-3 mt-4">

                    <asp:Button ID="btnGuardar" runat="server" Text="➕ Agregar categoría" CssClass="btn btn-success px-4" ValidationGroup="Categoria" CausesValidation="true" OnClick="btnGuardar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="↩ Cancelar edición" CssClass="btn btn-outline-secondary px-4" CausesValidation="false" Visible="false" OnClick="btnCancelar_Click" />

                </div>

            </div>

        </div>

        <div class="table-responsive">

            <asp:GridView ID="dgvCategorias" runat="server" AutoGenerateColumns="false"
                CssClass="table table-striped table-hover table-bordered tabla-categorias"
                DataKeyNames="Id" EmptyDataText="No hay categorías registradas."
                AllowPaging="true" PageSize="10" OnPageIndexChanging="dgvCategorias_PageIndexChanging"
                OnRowCommand="dgvCategorias_RowCommand">

                <Columns>

                    <asp:BoundField DataField="Id" HeaderText="ID" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                    <asp:TemplateField HeaderText="Acciones">

                        <ItemStyle HorizontalAlign="Center" Width="260px" />

                        <ItemTemplate>

                            <div class="d-flex justify-content-center gap-2">

                                <asp:LinkButton ID="btnEditar" runat="server" Text="✏️ Editar" CssClass="btn btn-warning btn-sm" CommandName="EditarCategoria" CommandArgument='<%# Eval("Id") %>' CausesValidation="false"> </asp:LinkButton>
                                <asp:LinkButton ID="btnEliminar" runat="server" Text="🗑️ Eliminar" CssClass="btn btn-danger btn-sm" CommandName="EliminarCategoria" CommandArgument='<%# Eval("Id") %>' CausesValidation="false" OnClientClick="return confirm('¿Está seguro de que desea eliminar permanentemente esta categoría?');"> </asp:LinkButton>

                            </div>

                        </ItemTemplate>

                    </asp:TemplateField>

                </Columns>

                <PagerStyle HorizontalAlign="Center" CssClass="text-center" />

            </asp:GridView>

        </div>

    </div>

</asp:Content>
