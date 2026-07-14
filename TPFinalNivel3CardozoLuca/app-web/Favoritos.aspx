<%@ Page Title="Mis favoritos" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Favoritos.aspx.cs" Inherits="app_web.Favoritos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .card-articulo {
            height: 100%;
            transition: transform 0.2s ease, box-shadow 0.2s ease;
        }

            .card-articulo:hover {
                transform: translateY(-4px);
                box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15);
            }

        .imagen-articulo {
            width: 100%;
            height: 220px;
            object-fit: contain;
            padding: 12px;
            background-color: #ffffff;
        }

        .nombre-articulo {
            min-height: 48px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="text-center mb-4">

        <h1>Mis artículos favoritos</h1>

        <p class="text-muted">
            Consultá y administrá los artículos que guardaste.
        </p>

    </div>

    <div class="card shadow-sm mb-4">

        <div class="card-header bg-dark text-white text-center">
            <h5 class="mb-0">Filtrar favoritos</h5>
        </div>

        <div class="card-body">

            <div class="row g-3 justify-content-center">

                <div class="col-12 col-md-3">

                    <label class="form-label">Nombre</label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Ej: Motorola"> </asp:TextBox>

                </div>

                <div class="col-12 col-md-3">

                    <label class="form-label">Marca</label>
                    <asp:DropDownList ID="ddlMarca" runat="server" CssClass="form-select"></asp:DropDownList>

                </div>

                <div class="col-12 col-md-3">

                    <label class="form-label">Categoría</label>
                    <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-select"></asp:DropDownList>

                </div>

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

                <asp:Button ID="btnBuscar" runat="server" Text="🔍 Buscar" CssClass="btn btn-primary px-4" CausesValidation="false" OnClick="btnBuscar_Click" />
                <asp:Button ID="btnRecargar" runat="server" Text="🔄 Recargar" CssClass="btn btn-outline-secondary px-4" CausesValidation="false" OnClick="btnRecargar_Click" />

            </div>

        </div>

    </div>

    <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="alert text-center shadow-sm">
        <asp:Label ID="lblMensaje" runat="server"> </asp:Label>
    </asp:Panel>

    <asp:Panel ID="pnlSinFavoritos" runat="server" CssClass="alert alert-info text-center" Visible="false">
        Todavía no agregaste ningún artículo a favoritos.
        <div class="mt-3">
            <a href="Home.aspx" class="btn btn-primary">Ir al catálogo</a>
        </div>
    </asp:Panel>

    <div class="row row-cols-1 row-cols-sm-2 row-cols-md-3 row-cols-lg-4 g-4">

        <asp:Repeater ID="repFavoritos" runat="server" OnItemCommand="repFavoritos_ItemCommand">

            <ItemTemplate>

                <div class="col">

                    <div class="card card-articulo">

                        <img src='<%# obtenerImagen(Eval("Imagen")) %>' class="card-img-top imagen-articulo" alt='<%# Eval("Nombre") %>' onerror="this.onerror=null; this.src='<%= UrlPlaceholder %>';" />
                        <div class="card-body d-flex flex-column">
                            <h5 class="card-title nombre-articulo">
                                <%# Eval("Nombre") %>
                            </h5>
                            <p class="text-muted mb-1">
                                <%# Eval("Marca.Descripcion") %>
                            </p>
                            <p class="card-text fs-5 fw-bold text-success">
                                <%# mostrarPrecio(Eval("Precio")) %>
                            </p>
                            <div class="d-grid gap-2 mt-auto">
                                <a
                                    href='<%# "DetalleArticulo.aspx?id=" + Eval("Id") %>'
                                    class="btn btn-primary">Ver detalle
                                </a>
                                <asp:LinkButton ID="btnEliminarFavorito" runat="server" Text="💔 Quitar de favoritos" CssClass="btn btn-outline-danger" CommandName="EliminarFavorito" CommandArgument='<%# Eval("Id") %>' CausesValidation="false" OnClientClick="return confirm('¿Querés quitar este artículo de favoritos?');"> </asp:LinkButton>
                            </div>
                        </div>

                    </div>

                </div>

            </ItemTemplate>

        </asp:Repeater>

    </div>

</asp:Content>
