<%@ Page Title="Detalle del artículo" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="DetalleArticulo.aspx.cs" Inherits="app_web.DetalleArticulo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .imagen-detalle {
            width: 100%;
            max-height: 450px;
            object-fit: contain;
            background-color: #ffffff;
            padding: 20px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:Panel ID="pnlError" runat="server" CssClass="alert alert-danger" Visible="false">
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </asp:Panel>

    <asp:Panel ID="pnlDetalle" runat="server" Visible="false">

        <div class="row g-4">

            <div class="col-md-5">
                <asp:Image ID="imgArticulo" runat="server" CssClass="imagen-detalle border rounded" AlternateText="Imagen del artículo" />
            </div>

            <div class="col-md-7">
                <h1>
                    <asp:Label ID="lblNombre" runat="server"></asp:Label>
                </h1>

                <p class="fs-3 fw-bold text-success">
                    <asp:Label ID="lblPrecio" runat="server"></asp:Label>
                </p>

                <hr />

                <dl class="row">
                    <dt class="col-sm-4">Código</dt>
                    <dd class="col-sm-8">
                        <asp:Label ID="lblCodigo" runat="server"></asp:Label>
                    </dd>
                    <dt class="col-sm-4">Marca</dt>
                    <dd class="col-sm-8">
                        <asp:Label ID="lblMarca" runat="server"></asp:Label>
                    </dd>
                    <dt class="col-sm-4">Categoría</dt>
                    <dd class="col-sm-8">
                        <asp:Label ID="lblCategoria" runat="server"></asp:Label>
                    </dd>
                    <dt class="col-sm-4">Descripción</dt>
                    <dd class="col-sm-8">
                        <asp:Label ID="lblDescripcion" runat="server"></asp:Label>
                    </dd>
                </dl>

                <div class="mt-4">
                    <a href="Home.aspx" class="btn btn-secondary">Volver al catálogo
                    </a>
                </div>

            </div>

        </div>

    </asp:Panel>

</asp:Content>
