<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="AccesoDenegado.aspx.cs" Inherits="app_web.AccesoDenegado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-7 text-center">

                <div class="alert alert-danger shadow-sm">
                    <h1 class="display-1">⛔</h1>
                    <h3>Acceso denegado</h3>
                    <p>
                        No tenés permisos para acceder a esta sección.   
                    </p>
                    <p>
                        Esta funcionalidad está reservada para usuarios administradores.   
                    </p>
                </div>

                <a href="Home.aspx" class="btn btn-primary">Volver al inicio</a>

            </div>
        </div>
    </div>

</asp:Content>
