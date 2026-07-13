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

    <asp:Panel ID="pnlError" runat="server" CssClass="alert alert-danger" Visible="false">
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </asp:Panel>

    <asp:Panel ID="pnlSinArticulos" runat="server" CssClass="alert alert-info text-center" Visible="false">
        No hay artículos disponibles para mostrar.
    </asp:Panel>

    <div class="row row-cols-1 row-cols-sm-2 row-cols-md-3 row-cols-lg-4 g-4">

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

</asp:Content>
