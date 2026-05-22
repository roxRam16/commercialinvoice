<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="VisualizaDatos.aspx.vb" Inherits="WebRegister" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="fWebRegister" runat="server">
        <div>
            <h2>VisualizarDatos</h2>
            <label>Ruta Carpeta/Archivo:</label>
            <asp:TextBox ID="tbCarpetaArchivo" runat="server"></asp:TextBox><br />
            <label>Carpetas:</label>
            <asp:TextBox ID="tbCarpetasDentro" runat="server"></asp:TextBox><br />
            <label>Archivos/Contenido:</label>
            <asp:TextBox ID="tbArchivosContenido" runat="server"></asp:TextBox><br />
            <asp:Button ID="btMostrar" Text="Mostrar Carpetas y Archivos" runat="server" OnClick="MotrarCarpetaArchivo_Click" />
            <asp:Button ID="btAsignarPrivilegios" Text="Checa Paso por Constructor" runat="server" OnClick="AsignarPrivilegios_Click" />
            <asp:Button ID="btContenido" Text="Mostrar Contenido del Archivo" runat="server" OnClick="MostrarContenidoArchivo_Click" />
        </div>
    </form>
</body>

<!-- Core JS Files -->
<script src="/FrontEnd/Librerias/JQuery/jquery-3.3.1.js" type="text/javascript"></script>
<script src="/FrontEnd/Librerias/Tether/tether.min.js" type="text/javascript"></script>
<script src="/FrontEnd/Librerias/BootstrapV4/js/bootstrap.min.js" type="text/javascript"></script>

<!-- Plugin for Switches, full documentation here: http://www.jque.re/plugins/version3/bootstrap.switch/ -->
<script src="/FrontEnd/Librerias/BootstrapV4/js/bootstrap-switch.js"></script>

<!-- Plugin for the Sliders, full documentation here: http://refreshless.com/nouislider/ -->
<script src="/FrontEnd/Librerias/NouiSlider/nouislider.min.js" type="text/javascript"></script>

<!-- Plugin for the DatePicker, full documentation here: https://github.com/uxsolutions/bootstrap-datepicker -->
<script src="/FrontEnd/Librerias/BootstrapV4/js/bootstrap-datepicker.js" type="text/javascript"></script>

<!-- Control Center for Now Ui Kit: parallax effects, scripts for the example pages etc -->
<script src="/FrontEnd/Librerias/BootstrapV4/js/now-ui-kit.js" type="text/javascript"></script>

<!-- Krom components -->
<script src="/FrontEnd/Librerias/Krom/js/KromComponentes.js"></script>

<!-- Krom plugins -->
<script src="/FrontEnd/Librerias/Krom/js/KROM-Plugins.js"></script>

<script>

    var mensaje_ = "<%= Session("fallaLogin_")%>"

    if (mensaje_ != "") {

        $.KromMessage("danger", mensaje_)

        <% Session("fallaLogin_") = ""%>

    }

    //Con esta variable se obtienen los mensaje entre paginas
    var flashdata_ = '<%= Session("flashdata")%>'

    if (flashdata_ != null && flashdata_ != '') {

        $.KromMessage("info", flashdata_)

        <% Session.Contents.Remove("flashdata")%>

    }

    document.querySelectorAll('.clear').forEach((a) => {

        a.addEventListener('click', (e) => {

            e.preventDefault();

            e.target.closest('label').querySelector('input').value = "";

            submitPerm();

        });

    });
    </script>

</html>
