<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="app_web.Home" %>

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
        <h1>Catálogo de artículos tecnológicos</h1>
        <p class="text-muted">
            Consultá nuestros productos disponibles.
        </p>
    </div>

    <div class="card shadow-sm mb-4">

        <div class="card-header bg-dark text-white text-center">
            <h5 class="mb-0">Filtrar artículos</h5>
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

            </div>

            <div class="row g-3 justify-content-center mt-1">

                <div class="col-12 col-md-3">
                    <label class="form-label">Precio mínimo</label>
                    <asp:TextBox ID="txtPrecioMinimo" runat="server" CssClass="form-control" TextMode="Number" step="0.01" min="0" placeholder="Desde"> </asp:TextBox>

                </div>

                <div class="col-12 col-md-3">
                    <label class="form-label">Precio máximo</label>
                    <asp:TextBox ID="txtPrecioMaximo" runat="server" CssClass="form-control" TextMode="Number" step="0.01" min="0" placeholder="Hasta"> </asp:TextBox>
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

                <asp:Button ID="btnBuscar" runat="server" Text="🔍 Buscar" CssClass="btn btn-primary" OnClick="btnBuscar_Click" />
                <asp:Button ID="btnLimpiar" runat="server" Text="🔄 Recargar" CssClass="btn btn-outline-secondary" OnClick="btnLimpiar_Click" CausesValidation="false" />

            </div>

            <asp:Label ID="lblErrorFiltros" runat="server" CssClass="text-danger d-block mt-3"> </asp:Label>

        </div>

    </div>

    <asp:Panel ID="pnlError" runat="server" CssClass="alert alert-danger" Visible="false">
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </asp:Panel>

    <asp:Panel ID="pnlSinArticulos" runat="server" CssClass="alert alert-info text-center" Visible="false">
        No hay artículos disponibles para mostrar.
    </asp:Panel>

    <div class="row row-cols-1 row-cols-sm-2 row-cols-md-3 row-cols-lg-4 g-4 mb-4">

        <asp:Repeater ID="repArticulos" runat="server">
            <ItemTemplate>
                <div class="col">
                    <div class="card card-articulo">

                        <img src='<%# obtenerImagen(Eval("Imagen")) %>'
                            class="card-img-top imagen-articulo"
                            alt='<%# Eval("Nombre") %>'
                            onerror="this.onerror=null; this.src='<%= UrlPlaceholder %>';" />

                        <div class="card-body d-flex flex-column">

                            <h5 class="card-title nombre-articulo">
                                <%# Eval("Nombre") %>
                            </h5>

                            <p class="card-text fs-5 fw-bold text-success">
                                <%# mostrarPrecio(Eval("Precio")) %>
                            </p>

                            <a
                                href='<%# "DetalleArticulo.aspx?id=" + Eval("Id") %>'
                                class="btn btn-primary mt-auto">Ver detalle
                            </a>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <asp:Panel ID="pnlPaginacion" runat="server" CssClass="d-flex justify-content-center align-items-center gap-3 mt-4" Visible="false">

        <asp:Button ID="btnAnterior" runat="server" Text="⬅ Anterior" CssClass="btn btn-outline-primary" OnClick="btnAnterior_Click" CausesValidation="false" />
        <asp:Label ID="lblPagina" runat="server" CssClass="fw-bold"> </asp:Label>
        <asp:Button ID="btnSiguiente" runat="server" Text="Siguiente ➡" CssClass="btn btn-outline-primary" OnClick="btnSiguiente_Click" CausesValidation="false" />

    </asp:Panel>

</asp:Content>
