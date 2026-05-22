<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LoginRegister.aspx.vb" Inherits="WebRegister" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="fWebRegister" runat="server">
        <div>
            <h2>Registarse</h2>
            <label>Email:</label>
            <asp:TextBox ID="tbEmail" runat="server"></asp:TextBox><br />
            <label>Contraseña:</label>
            <asp:TextBox ID="tbContrasena" TextMode="Password" runat="server"></asp:TextBox><br />
            <label>Confirmar Contraseña:</label>
            <asp:TextBox ID="tbConfirmarContrasena" TextMode="Password" runat="server"></asp:TextBox><br />
            <label>Teléfono:</label>
            <asp:TextBox ID="tbPhoneNumber" TextMode="Phone" runat="server"></asp:TextBox><br />
            <label>Nombre:</label>
            <asp:TextBox ID="tbName" runat="server"></asp:TextBox><br />
            <label>Apellidos:</label>
            <asp:TextBox ID="tbLastName" runat="server"></asp:TextBox><br />
            <asp:CheckBox ID="ckAdminSynApsis" Text="Administrador Synapsis" runat="server"></asp:CheckBox><br />
            <asp:CheckBox ID="ckAdminKromBaseWeb" Text="Administrador KromBaseWeb" runat="server"></asp:CheckBox><br />
            <asp:Button ID="btRegister" Text="Registrar" runat="server" OnClick="Register_Click" />
            <asp:Button ID="btAsignarPrivilegios" Text="Asignar Privilegios" runat="server" OnClick="AsignarPrivilegios_Click" />
            <asp:Button ID="btLogOut" Text="Cerrar Sesión" runat="server" OnClick="LogOut_Click" />
           <asp:Button ID="btLogin" Text="Ir a Iniciar Sesión" runat="server" OnClick="GoStartSesion_Click" />
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
