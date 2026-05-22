<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AsignarPrivilegios.aspx.vb" Inherits="AsignarPrivilegios" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>

<%--        <style>
        body { font-family: Arial, sans-serif; margin: 20px; }
        .checkbox-group { border: 1px solid #ccc; padding: 15px; border-radius: 5px; margin-bottom: 20px; }
        .checkbox-group h3 { margin-top: 0; color: #333; }
        .checkbox-group div { margin-bottom: 10px; }
        .result-panel { background-color: #f0f0f0; padding: 15px; border-radius: 5px; border: 1px solid #ddd; }
        .error-message { color: red; font-weight: bold; margin-top: 10px; }
    </style>--%>

        <style>
        body { font-family: Arial, sans-serif; margin: 20px; }

        /* Contenedor Flexbox */
        .group-container {
            display: flex; /* Habilita Flexbox */
            justify-content: space-between; /* Distribuye el espacio entre los elementos */
            gap: 20px; /* Espacio entre los grupos (navegadores modernos) */
            margin-bottom: 20px;
        }

        /* Estilos para cada grupo individual */
        .checkbox-group {
            border: 1px solid #ccc;
            padding: 15px;
            border-radius: 5px;
            flex: 1; /* Hace que cada grupo tome el mismo espacio disponible */
            min-width: 250px; /* Para asegurar que no se hagan demasiado pequeños en pantallas chicas */
            box-sizing: border-box; /* Incluye padding y border en el width */
        }

        .checkbox-group h3 { margin-top: 0; color: #333; }
        .checkbox-group div { margin-bottom: 10px; } /* Espacio entre los checkboxes */

        .result-panel { background-color: #f0f0f0; padding: 15px; border-radius: 5px; border: 1px solid #ddd; }
        .error-message { color: red; font-weight: bold; margin-top: 10px; }

        /* Media Queries para responsividad (opcional pero recomendado) */
        @media (max-width: 768px) {
            .group-container {
                flex-direction: column; /* Apila los grupos verticalmente en pantallas pequeñas */
            }
            .checkbox-group {
                width: 100%;
                margin-right: 0;
            }
        }
    </style>

    <link href="/FrontEnd/Librerias/BootstrapV4/css/bootstrap.min.css" rel="stylesheet" />

    <link href="/FrontEnd/Librerias/BootstrapV4/css/now-ui-kit.css" rel="stylesheet" />
</head>
<body>
    <form id="fWebPrivilegios" runat="server">
        <div>
            <h2>Asignar Privilegios</h2>
            <label>Email:</label>
            <asp:TextBox ID="tbEmail" runat="server"></asp:TextBox><br />
            <asp:CheckBox ID="ckAdminSynApsis" Text="Administrador Synapsis" runat="server" AutoPostBack="true" OnCheckedChanged="CargarPermisosAdmin"></asp:CheckBox><br />
            <asp:CheckBox ID="ckAdminKromBaseWeb" Text="Administrador KromBaseWeb" runat="server"></asp:CheckBox><br />
           <div class="group-container">
             <div class="col-md-4 checkbox-group">
                  <h3>OFICINAS:</h3>
                  <asp:CheckBox ID="ckbTodas" Text="TODAS" runat="server"></asp:CheckBox><br />
                  <asp:CheckBox ID="ckbVirtual" Text="Virtual" runat="server"></asp:CheckBox><br />
                  <asp:CheckBox ID="ckbVeracruz" Text="Veracruz" runat="server"></asp:CheckBox><br />
                  <asp:CheckBox ID="ckbManzanillo" Text="Manzanillo" runat="server"></asp:CheckBox><br />
                  <asp:CheckBox ID="ckbLazaroCardenas" Text="Lázaro Cárdenas" runat="server"></asp:CheckBox><br />
                  <asp:CheckBox ID="ckbAltamira" Text="Altamira" runat="server"></asp:CheckBox><br />
                  <asp:CheckBox ID="ckbAICM" Text="AICM" runat="server"></asp:CheckBox><br />
                  <asp:CheckBox ID="ckbAIFA" Text="CDMX" runat="server"></asp:CheckBox><br />
                  <asp:CheckBox ID="ckbToluca" Text="Toluca" runat="server"></asp:CheckBox><br />
                  <asp:CheckBox ID="ckbNuevoLaredo" Text="Nuevo Laredo" runat="server"></asp:CheckBox><br />
                  <asp:CheckBox ID="ckbColombia" Text="Colombia" runat="server"></asp:CheckBox><br />
                  <asp:CheckBox ID="ckbSanLuis" Text="San Luis Potosí" runat="server"></asp:CheckBox><br />
            </div>
           <div class="group-container">
             <div class="col-md-4 checkbox-group">
                  <h3>Segmento SynCatalogs:</h3>
                  <asp:CheckBox ID="ckSynapsisPanIzqModClientes" Text="Clientes" runat="server"></asp:CheckBox><br />
            </div>
             <div class="col-md-4 checkbox-group">
                  <h3>Segmento SynOperations:</h3>
                  <asp:CheckBox ID="ckSynapsisPanIzqModApendices" Text="Apendices" runat="server"></asp:CheckBox><br />
                  <asp:CheckBox ID="ckSynapsisPanIzqModPedimentos" Text="Pedimentos" runat="server"></asp:CheckBox><br />
            </div>
            <div class="col-md-4 checkbox-group">
                  <h3>Segmento SynReferences:</h3>
                  <asp:CheckBox ID="ckGuiasMaritimas" Text="Guías Marítimas" runat="server"></asp:CheckBox><br />
                  <asp:CheckBox ID="ckGuiasAereas" Text="Guías Aéreas" runat="server"></asp:CheckBox><br />
                  <asp:CheckBox ID="ckSynapsisPanIzqModRegistroReferencias" Text="Referencias" runat="server"></asp:CheckBox><br />
                  <asp:CheckBox ID="ckSynapsisPanIzqRevalidacion" Text="Revalidación" runat="server"></asp:CheckBox><br />
                  <asp:CheckBox ID="ckSynapsisPanIzqModViajes" Text="Control de Viajes" runat="server"></asp:CheckBox><br />
            </div>

</div>
            <div class="group-container">
            <div class="col-md-4 checkbox-group">
                  <h3>Segmento SynControls:</h3>
                  <asp:CheckBox ID="ckSynapsisConsolidados" Text="Consolidados" runat="server"></asp:CheckBox><br />
                  <asp:CheckBox ID="ckSynapsisCopiasSimples" Text="Copias Simples" runat="server"></asp:CheckBox><br />
                  <asp:CheckBox ID="ckSynapsisPartesII" Text="Partes II" runat="server"></asp:CheckBox><br />
                  <asp:CheckBox ID="ckSynapsisPrevios" Text="Programación de Previos" runat="server"></asp:CheckBox><br />
            </div>
            <div class="checkbox-group">
                  <h3>Segmento SynCommercialInvoice:</h3>
                  <asp:CheckBox ID="ckSynapsisPanIzqAcuseValor" Text="Acuse de Valor" runat="server"></asp:CheckBox><br />
                  <asp:CheckBox ID="ckSynapsisPanIzqModRegistroFacturasExp" Text="Factura Comercial Exportacion" runat="server"></asp:CheckBox><br />
                  <asp:CheckBox ID="ckSynapsisPanIzqModRegistroFacturasImp" Text="Factura Comercial Importación" runat="server"></asp:CheckBox><br />
                  <asp:CheckBox ID="ckSynapsisSubDivisionFactura" Text="Subdivisión de Factura" runat="server"></asp:CheckBox><br />
            </div>
            <div class="checkbox-group">
                  <h3>Segmento SynProducts:</h3>
                  <asp:CheckBox ID="ckSynapsisPanIzqModRegistroProveedoresImp" Text="Proveedores Extranjeros" runat="server"></asp:CheckBox><br />
                  <asp:CheckBox ID="ckSynapsisPanIzqModRegistroProveedoresExp" Text="Proveedores Nacionales" runat="server"></asp:CheckBox><br />
                  <asp:CheckBox ID="ckSynapsisDestinatarios" Text="Destinatarios" runat="server"></asp:CheckBox><br />
                  <asp:CheckBox ID="ckSynapsisPanIzqModTarifaArancelaria" Text="Tarifa Arencelaria" runat="server"></asp:CheckBox><br />
                  <asp:CheckBox ID="ckSynapsisPanIzqModRegistroProductos" Text="Productos" runat="server"></asp:CheckBox><br />
            </div>

                </div>

             <div class="group-container">
            <div class="checkbox-group">
                  <h3>Segmento SynReporting:</h3>
            <asp:CheckBox ID="ckSynapsisPanIzqModBusquedaGeneral" Text="Busqueda general" runat="server"></asp:CheckBox><br />
            </div>
            <div class="checkbox-group">
                  <h3>Segmento SynDigitization:</h3>
                  <asp:CheckBox ID="ckSynapsisPanIzqProcesamientoElectronico" Text="Procesamiento Electronico" runat="server"></asp:CheckBox><br />
            </div>
            <div class="checkbox-group">
                  <h3>Segmento SynService:</h3>
                  <asp:CheckBox ID="ckSynapsisPanIzqCuboDatos" Text="Cubo de Datos" runat="server"></asp:CheckBox><br />
            </div>
            </div>
            <asp:Button ID="btAsignarPrivilegios" Text="Asignar Privilegios" runat="server" OnClick="AsignarPrivilegios_Click" />
            <asp:Button ID="btLogin" Text="Ir a Iniciar Sesión" runat="server" OnClick="GoStartSesion_Click" />
        </div>
    </form>
</body>

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
</html>
