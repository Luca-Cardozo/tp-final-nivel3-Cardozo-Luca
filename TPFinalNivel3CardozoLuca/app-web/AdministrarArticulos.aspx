<%@ Page Title="Administrar artículos" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="AdministrarArticulos.aspx.cs" Inherits="app_web.AdministrarArticulos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .contenedor-filtros {
            max-width: 1100px;
            margin: 0 auto;
        }

        .tabla-articulos {
            text-align: center;
        }

            .tabla-articulos th {
                vertical-align: middle;
                text-align: center;
            }

            .tabla-articulos td {
                vertical-align: middle;
                text-align: center;
            }

            .tabla-articulos .btn {
                white-space: nowrap;
            }

            .tabla-articulos .pagination-container table {
                margin: 0 auto;
            }

            .tabla-articulos .pagination-container td {
                padding: 4px;
            }

            .tabla-articulos .pagination-container a,
            .tabla-articulos .pagination-container span {
                display: inline-block;
                padding: 6px 12px;
                border: 1px solid #dee2e6;
                border-radius: 4px;
                text-decoration: none;
            }

            .tabla-articulos .pagination-container span {
                background-color: #0d6efd;
                color: white;
                border-color: #0d6efd;
            }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="d-flex justify-content-between align-items-center mb-4">

        <div class="contenedor-filtros text-center mb-4">
            <h1 class="mb-1">Administrar artículos</h1>
            <p class="text-muted mb-0">
                Consultar, agregar, modificar y eliminar artículos del catálogo
            </p>
        </div>

    </div>

    <div class="card shadow-sm mb-4 contenedor-filtros">

        <div class="card-header bg-dark text-white text-center">
            <h5 class="mb-0">Filtrar artículos</h5>
        </div>

        <div class="card-body">

            <div class="row g-3 justify-content-center">

                <div class="col-12 col-md-3">
                    <label class="form-label">Código</label>
                    <asp:TextBox ID="txtCodigo" runat="server" CssClass="form-control" placeholder="Ej: A12">
                    </asp:TextBox>
                </div>

                <div class="col-12 col-md-3">
                    <label class="form-label">Nombre</label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Ej: Motorola"> </asp:TextBox>
                </div>

                <div class="col-12 col-md-3">
                    <label class="form-label">Marca</label>
                    <asp:DropDownList ID="ddlMarca" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>

            </div>

            <div class="row g-3 justify-content-center mt-1">

                <div class="col-12 col-md-3">
                    <label class="form-label">Categoría</label>
                    <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>

                <div class="col-12 col-md-3">
                    <label class="form-label">Precio mínimo</label>
                    <asp:TextBox ID="txtPrecioMinimo" runat="server" CssClass="form-control" TextMode="Number" step="0.01" min="0" placeholder="Desde"> </asp:TextBox>
                </div>

                <div class="col-12 col-md-3">
                    <label class="form-label">Precio máximo</label>
                    <asp:TextBox ID="txtPrecioMaximo" runat="server" CssClass="form-control" TextMode="Number" step="0.01" min="0" placeholder="Hasta"> </asp:TextBox>
                </div>

            </div>

            <div class="row g-3 justify-content-center mt-1">

                <div class="col-12 col-md-3">
                    <label class="form-label">Ordenar por</label>
                    <asp:DropDownList ID="ddlOrden" runat="server" CssClass="form-select">
                        <asp:ListItem Text="Sin orden" Value="" Selected="True" />
                        <asp:ListItem Text="Nombre (A-Z)" Value="NombreAsc" />
                        <asp:ListItem Text="Nombre (Z-A)" Value="NombreDesc" />
                        <asp:ListItem Text="Precio (menor a mayor)" Value="PrecioAsc" />
                        <asp:ListItem Text="Precio (mayor a menor)" Value="PrecioDesc" />
                    </asp:DropDownList>
                </div>

            </div>

            <div class="d-flex justify-content-center gap-3 mt-4">

                <asp:Button ID="btnBuscar" runat="server" Text="🔍 Buscar" CssClass="btn btn-primary px-4" OnClick="btnBuscar_Click" />
                <asp:Button ID="btnRecargar" runat="server" Text="🔄 Recargar" CssClass="btn btn-outline-secondary px-4" OnClick="btnRecargar_Click" CausesValidation="false" />

            </div>

            <asp:Label
                ID="lblErrorFiltros" runat="server" CssClass="text-danger d-block text-center mt-3">
            </asp:Label>

        </div>

    </div>

    <div class="d-flex justify-content-center mb-4">
        <asp:Button ID="btnAgregarArticulo" runat="server" Text="➕ Agregar artículo" CssClass="btn btn-success btn-lg px-5" OnClick="btnAgregarArticulo_Click" CausesValidation="false" />
    </div>

    <asp:Panel ID="pnlError" runat="server" CssClass="alert alert-danger" Visible="false">

        <asp:Label ID="lblError" runat="server"> </asp:Label>

    </asp:Panel>

    <asp:Panel ID="pnlSinArticulos" runat="server" CssClass="alert alert-info text-center" Visible="false">No se encontraron artículos.</asp:Panel>

    <div class="table-responsive">

        <asp:GridView ID="dgvArticulos" runat="server" AutoGenerateColumns="false"
            CssClass="table table-striped table-hover table-bordered tabla-articulos"
            DataKeyNames="Id" EmptyDataText="No hay artículos disponibles."
            OnRowCommand="dgvArticulos_RowCommand"
            HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
            AllowPaging="true" PageSize="10" OnPageIndexChanging="dgvArticulos_PageIndexChanging">

            <PagerStyle CssClass="pagination-container" HorizontalAlign="Center" />

            <Columns>

                <asp:BoundField DataField="Id" HeaderText="ID" />

                <asp:BoundField DataField="Codigo" HeaderText="Código" />

                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />

                <asp:BoundField DataField="Marca.Descripcion" HeaderText="Marca" />

                <asp:BoundField DataField="Categoria.Descripcion" HeaderText="Categoría" />

                <asp:TemplateField HeaderText="Precio">

                    <ItemTemplate>

                        <span class='<%# Eval("Precio") == null ? "text-muted" : "fw-semibold text-success" %>'>
                            <%# mostrarPrecio(Eval("Precio")) %>
                        </span>

                    </ItemTemplate>

                </asp:TemplateField>

                <asp:TemplateField HeaderText="Acciones">

                    <ItemStyle HorizontalAlign="Center" Width="150px" />

                    <ItemTemplate>

                        <asp:LinkButton ID="btnVerDetalle" runat="server" Text="✏️ Ver / editar" CssClass="btn btn-warning btn-sm" CommandName="VerDetalle" CommandArgument='<%# Eval("Id") %>' CausesValidation="false"> </asp:LinkButton>

                    </ItemTemplate>

                </asp:TemplateField>

            </Columns>

        </asp:GridView>

    </div>

</asp:Content>
