<%@ Page Title="" Language="vb" Async="true" AutoEventWireup="false" MasterPageFile="~/FrontEnd/Modulos/Home.Master" CodeBehind="Ges022-001-ProveedorNacional.aspx.vb" Inherits=".Ges022_001_ProveedorNacional" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentFindbar" runat="server">

    <% If IsPopup = False Then %>


    <GWC:FindbarControl Label="Buscar Proveedor" ID="__SYSTEM_CONTEXT_FINDER" runat="server" OnClick="BusquedaGeneral" />

    <% End If %>

    <link rel="stylesheet" type="text/css" href="estilos.css" />

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentCompanyList" runat="server">
    <% If IsPopup = False Then %>

    <GWC:SelectControl CssClass="col-auto company-list-select" runat="server" SearchBarEnabled="false" ID="__SYSTEM_ENVIRONMENT" OnSelectedIndexChanged="CambiarEmpresa" />

    <% End If %>
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="contentBody" runat="server">

    <GWC:FormControl runat="server" ID="__SYSTEM_MODULE_FORM" HasAutoSave="false" Label="<span style='color:#321761'>Proveedores/compradores</span><span style='color:#782360;'>&nbsp;nacionales</span> " OnCheckedChanged="MarcarPagina">

        <Buttonbar runat="server" OnClick="EventosBotonera">
            <DropdownButtons>
                <GWC:ButtonItem Text="Domicilios registrados" />
                <GWC:ButtonItem Text="Vaciar domicilio" />
                <GWC:ButtonItem Text="Clonar proveedor" />
            </DropdownButtons>
        </Buttonbar>

        <Fieldsets>
            <GWC:FieldsetControl ID="fsDatosGenerales" runat="server" Label="Datos Generales" Priority="true" CssClass="mb-5">
                <ListControls>

                    <GWC:CardControl runat="server" ID="aviso" CssClass="container wc-card-danger mb-5 mt-0" Visible="False">
                        <ListControls>
                            <asp:Panel runat="server" CssClass="m-0 mt-0 p-0" ID="Panel2">
                                <asp:Label runat="server" ID="lbTitleAviso" Text="<svg xmlns='http://www.w3.org/2000/svg'  height='24' width='24' viewBox='0 0 512 512'><path d='M464 256A208 208 0 1 0 48 256a208 208 0 1 0 416 0zM0 256a256 256 0 1 1 512 0A256 256 0 1 1 0 256zm177.6 62.1C192.8 334.5 218.8 352 256 352s63.2-17.5 78.4-33.9c9-9.7 24.2-10.4 33.9-1.4s10.4 24.2 1.4 33.9c-22 23.8-60 49.4-113.6 49.4s-91.7-25.5-113.6-49.4c-9-9.7-8.4-24.9 1.4-33.9s24.9-8.4 33.9 1.4zM144.4 208a32 32 0 1 1 64 0 32 32 0 1 1 -64 0zm192-32a32 32 0 1 1 0 64 32 32 0 1 1 0-64z'/></svg> &nbsp; Este &nbsp;<span style='font-weight:bold !important'>proveedor</span>&nbsp; ya ha sido &nbsp;<span style='font-weight:bold !important; color:#432776 !important'> registrado </span>.&nbsp;" Visible="True" CssClass="mb-5 title_Card" Style="font-weight: normal !important; font-size: 18px !important; color: #5b5b5b !important"></asp:Label>
                            </asp:Panel>
                        </ListControls>
                    </GWC:CardControl>

                    <asp:Panel runat="server" CssClass="col-xs-12 col-12 col-md-1 col-lg-1 mb-5 media-query-clave">
                        <asp:Label runat="server" CssClass="cl_clave col-lg-10 col-md-10 col-12 col-xs-12 d-flex justify-content-center" ID="Label1" Text="Clave"></asp:Label>
                        <asp:Label runat="server" CssClass="cl_clave cl_num_clave col-lg-10 col-md-10 col-12 col-xs-12 d-flex justify-content-center" ID="icClave" Text=""></asp:Label>
                        <asp:Label runat="server" CssClass="cl_clave cl_online col-lg-10 col-md-10 col-12 col-xs-12 d-flex justify-content-center" ID="online" Text=""></asp:Label>
                    </asp:Panel>

                    <asp:Panel runat="server" CssClass="col-xs-12 col-12 col-md-5 col-lg-4 mb-5 mt-5">
                        <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-12 col-md-12 col-lg-12" ID="fcRazonSocial" KeyField="_id" DisplayField="razonsocial" Label="Razón social" Rules="required|maxlegth[250]|Unique" OnClick="fcRazonSocial_Click" OnTextChanged="fcRazonSocial_TextChanged" />
                    </asp:Panel>

                    <asp:Panel runat="server" CssClass="col-xs-12 col-12 col-md-3 col-lg-2 mb-5 mt-5">
                        <GWC:SwitchControl runat="server" CssClass="col-xs-12 col-12 col-md-12 col-lg-12 d-flex justify-content-center" ID="swcTipoPersona" Label="Persona moral" OnText="Si" OffText="No" OnCheckedChanged="swcTipoPersona_CheckedChanged" Checked="true" />
                       
                    </asp:Panel>

                    <asp:Panel runat="server" CssClass="col-xs-12 col-12 col-md-5 col-lg-3 mb-5 mt-5">
                        <GWC:InputControl runat="server" Type="Hide" ID="icCveRfc" Label="IdRFC" />
                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-12 col-md-12 col-lg-12" Type="Text" ID="icRFC" Label="RFC" Rules="required" />
                    </asp:Panel>

                       <asp:Panel runat="server" CssClass="col-xs-12 col-12 col-md-3 col-lg-2 mb-5 mt-5">
                            <GWC:SwitchControl runat="server" CssClass="col-xs-12 col-12 col-md-12 col-lg-12 d-flex justify-content-center" ID="swcHabilitadoProveedor" Label="Habilitado" OnText="Sí" OffText="No" Checked="False" OnCheckedChanged="swcHabilitadoProveedor_CheckedChanged" Visible="false" />
   
                       </asp:Panel>

                    <asp:Panel runat="server" CssClass="col-xs-12 col-12 col-md-6 col-lg-6 mt-5">
                        <GWC:InputControl runat="server" Type="Hide" ID="icCveCurp" Label="IdCURP" />
                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-12 col-md-12 col-lg-12 mt-5" Type="Text" ID="icCURP" Label="CURP" Visible="False" />
                    </asp:Panel>

                </ListControls>

            </GWC:FieldsetControl>

            <GWC:FieldsetControl runat="server" ID="fsDetalles" Label="Domicilios" Detail="Domicilios" CssClass="mt-5 mb-5">
                <ListControls>
                    <GWC:CardControl runat="server" ID="ConfigurarDomicilios" CssClass="container m-0 p-0 mb-5" Visible="False">
                        <ListControls>
                            <asp:Panel runat="server" CssClass="m-0 mt-0 p-0" ID="Panel1">
                                  <asp:Label runat="server" ID="lbTitle" Text="📌 &nbsp; Seleccionar &nbsp;<span style='font-weight:bold !important'> domicilio</span>&nbsp;" CssClass="mb-5 title_Card" Style="font-weight: normal !important; font-size: 16px !important; color: #321761 !important; opacity:1 !important"></asp:Label>
                                  <GWC:SelectControl runat="server" CssClass="col-xs-12 col-12 col-md-11 col-lg-11 mt-5 mb-5" ID="scDomiciliosRegistrados" LocalSearch="True" style="color:#424242 !important" Label="Domicilios disponibles" OnClick="scDomiciliosRegistrados_Click" OnSelectedIndexChanged="scDomiciliosRegistrados_SelectedIndexChanged" OnTextChanged="scDomiciliosRegistrados_TextChanged"></GWC:SelectControl>
                                <GWC:ButtonControl runat="server" CssClass="col-xs-12 col-12 col-md-1 col-lg-1 mt-0 mb-5 d-flex justify-content-center" ID="btnTipoDomicilio" Label="Aplicar" OnClick="btnTipoDomicilio_Click" />
                            </asp:Panel>
                        </ListControls>
                    </GWC:CardControl>

                    <GWC:PillboxControl runat="server" ID="pbDetalleProveedor" KeyField="indice" CssClass="col-xs-12 m-0 p-0" OnCheckedChange="pbDetalleProveedor_CheckedChange" OnClick="pbDetalleProveedor_Click">
                        <ListControls>
                            <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 mb-5">
                                <asp:Label runat="server" ID="lbMercancia" Text="Domicilio" Visible="True" CssClass="w-100 cl_Secciones"></asp:Label>
                            </asp:Panel>

                            <asp:Panel runat="server" CssClass="mt-5 col-xs-12 col-12 col-md-12 col-lg-12" ID="sectionDomicilio">
                                <GWC:InputControl runat="server" Type="Text" ID="icIdTarjeta" Label="Id tarjeta" Visible ="false" />
                                <GWC:InputControl runat="server" Type="Text" ID="icFirmaTarjeta" Label="firma tarjeta" Visible ="false" />
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-6" Type="Text" ID="icCalle" Label="Calle" Rules="required" />
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3  col-lg-3" Type="Text" ID="icNumeroExterior" Label="Número exterior" />
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3  col-lg-3" Type="Text" ID="icNumeroInterior" Label="Número interior" />
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mb-3 mt-5" Type="Text" ID="icCodigoPostal" Label="Código postal" />
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mb-3 mt-5" Type="Text" ID="icColonia" Label="Colonia" />
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mb-3 mt-5" Type="Text" ID="icLocalidad" Label="Localidad" />
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mb-3 mt-5" Type="Text" ID="icCiudad" Label="Ciudad" />
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mt-3 mt-5" Type="Text" ID="icMunicipio" Label="Municipio" OnTextChanged="icMunicipio_TextChanged" />
                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mt-3 mt-5" Type="Text" ID="icEntidadFederativa" Label="Entidad federativa" OnTextChanged="icEntidadFederativa_TextChanged" OnClick="icEntidadFederativa_Click"/>
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-6 mt-3 mt-5" Type="Text" ID="icPais" Label="País" Rules="required" />

                                <GWC:InputControl runat="server" Type="Hide" ID="icIdDomicilio" Label="IdDomicilio" />
                                <GWC:InputControl runat="server" Type="Hide" ID="icSecDomicilio" Label="SecDomicilio" />

                                <GWC:InputControl runat="server" Type="Hide" ID="scDomicilio" Label="DomicilioFiscal" />
                                <GWC:InputControl runat="server" Type="Hide" ID="icCvePais" Label="CvePais" />
                                <GWC:InputControl runat="server" Type="Hide" ID="icIdPais" Label="IdPais" />
                                <GWC:InputControl runat="server" Type="Hide" ID="icNumeroExtInt" Label="NumExtInt" />
                                <GWC:InputControl runat="server" Type="Hide" ID="icCveMunicipio" Label="CveMunicipio" />
                                <GWC:InputControl runat="server" Type="Hide" ID="icCveEntidadFederativa" Label="CveEntidadFederativa" />

                                <GWC:InputControl runat="server" Type="Hide" ID="icusuriogenero" Label="User genero" />
                                <GWC:InputControl runat="server" Type="Hide" ID="icentorno" Label="Entorno u oficina" />
                                <GWC:InputControl runat="server" Type="Hide" ID="icestadoproveedor" Label="Estado" />
                                <GWC:InputControl runat="server" Type="Hide" ID="icdomicilioarchivadoproveedor" Label="Archivado" />
                                <GWC:InputControl runat="server" Type="Hide" ID="icmotivoarchivadoproveedor" Label="Motivo" />
                                <GWC:InputControl runat="server" Type="Hide" ID="fechaarchivadoproveedor" Label="Fecha" />
                            </asp:Panel>
                        </ListControls>
                    </GWC:PillboxControl>
                </ListControls>
            </GWC:FieldsetControl>

            <GWC:FieldsetControl runat="server" ID="fsVinculaciones" Label="Vinculaciones" Detail="Vinculaciones con clientes" CssClass="mt-5 mb-5">
                <ListControls>
                    <GWC:CatalogControl runat="server" KeyField="indice" ID="ccVinculaciones" CssClass="w-100" Collapsed="true">
                        <Columns>
                            <GWC:SelectControl runat="server" ID="scClienteVinculacion" OnClick="scClienteVinculacion_Click" OnTextChanged="scClienteVinculacion_TextChanged" LocalSearch="false" Label="Cliente">
                            </GWC:SelectControl>
                            <GWC:SelectControl runat="server" ID="scTaxIdVinculacion" Label="RFC proveedor" OnClick="scTaxIdVinculacion_Click"></GWC:SelectControl>
                            <GWC:SelectControl runat="server" ID="scVinculacion" Label="Vinculación" SearchBarEnabled="False" OnClick="scVinculacion_Click" OnTextChanged="scVinculacion_TextChanged">
                            </GWC:SelectControl>
                            <GWC:InputControl runat="server" Type="Text" ID="icPorcentajeVinculacion" Label="Porcentaje" Format="Real" />
                        </Columns>
                    </GWC:CatalogControl>
                </ListControls>
            </GWC:FieldsetControl>

            <GWC:FieldsetControl runat="server" ID="fsConfiguracionAdicional" Label="Configuración" Detail="Configuración adicional" CssClass="mt-5 mb-5">
                <ListControls>
                    <GWC:CatalogControl runat="server" ID="ccConfiguracionAdicional" KeyField="indice" CssClass="w-100" Collapsed="true">
                        <Columns>
                            <GWC:SelectControl runat="server" ID="scTaxIdConfiguracion" Label="RFC proveedor" OnClick="scTaxIdConfiguracion_Click">
                            </GWC:SelectControl>
                            <GWC:SelectControl runat="server" ID="scClienteConfiguracion" OnClick="scClienteConfiguracion_Click" OnTextChanged="scClienteConfiguracion_TextChanged" LocalSearch="false" Label="Cliente">
                            </GWC:SelectControl>
                            <GWC:SelectControl runat="server" ID="scMetodoValoracion" Label="Método de valoración" KeyField="i_Cve_MetodoValoracion" DisplayField="t_ClaveDescripcion" Dimension="Vt022MetodosValoracionA11">
                            </GWC:SelectControl>
                            <GWC:SelectControl runat="server" ID="scIncoterm" Label="INCOTERM" KeyField="i_Cve_TerminoFacturacion" DisplayField="t_ValorPresentacion" Dimension="Vt022TerminosFacturacionA14">
                            </GWC:SelectControl>
                        </Columns>
                    </GWC:CatalogControl>
                </ListControls>
            </GWC:FieldsetControl>

            <GWC:FieldsetControl runat="server" ID="fsHistorialDomicilios" Label="Domicilios" Detail="Historial de domicilios físcales" Collapsed="true" CssClass="mt-5 mb-5">
                <ListControls>
                    <GWC:CatalogControl runat="server" ID="ccDomiciliosFiscales" KeyField="indice" CssClass="w-100" Collapsed="true" UserInteraction="false">
                        <Columns>
                            <GWC:InputControl runat="server" Type="Text" ID="icTaxIDRFC" Label="RFC proveedor" />
                            <GWC:InputControl CssClass="text-align-center" runat="server" Type="Text" ID="icDomicilio" Label="Domicilio físcal" />
                            <GWC:SwitchControl runat="server" ID="swcArchivarDomicilio" Label="Archivado" OnText="Sí" OffText="No" />
                        </Columns>
                    </GWC:CatalogControl>
                </ListControls>
            </GWC:FieldsetControl>
        </Fieldsets>
    </GWC:FormControl>

    <GWC:FieldsetControl runat="server" ID="FieldsetControl1" Label="" Detail="" CssClass="mt-5 p-0 mb-5">
       <%-- <ListControls>
            <asp:Panel runat="server" CssClass="row p-5 m-5" Style="border: 1px solid #cecdcd; border-radius: 14px; margin-bottom: 20px; padding-bottom: 25px!important">

                <asp:Panel runat="server" CssClass="col-xs-12 col-12 col-md-12 col-lg-12 mt-5">
                    <asp:Label runat="server" ID="Label3" Text="También te podría interesar" CssClass="w-100 cl_Secciones mt-5 ml-5"></asp:Label>
                </asp:Panel>

                <asp:Panel runat="server" CssClass="col-xs-1 col-1 col-md-1 col-lg-1 mt-5 mb-5"></asp:Panel>

                <asp:Panel runat="server" CssClass="col-xs-11 col-4 col-md-4 col-lg-3 mt-5 mb-5">

                    <a href="#">
                        <asp:Label runat="server" ID="Label4" Text="Proveedores/compradores <span class='resaltado pr-5'>extranjeros</span> <svg xmlns='http://www.w3.org/2000/svg' height='15' width='15' viewBox='0 0 512 512'><path fill='#909090' d='M320 0c-17.7 0-32 14.3-32 32s14.3 32 32 32l82.7 0L201.4 265.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0L448 109.3l0 82.7c0 17.7 14.3 32 32 32s32-14.3 32-32l0-160c0-17.7-14.3-32-32-32L320 0zM80 32C35.8 32 0 67.8 0 112L0 432c0 44.2 35.8 80 80 80l320 0c44.2 0 80-35.8 80-80l0-112c0-17.7-14.3-32-32-32s-32 14.3-32 32l0 112c0 8.8-7.2 16-16 16L80 448c-8.8 0-16-7.2-16-16l0-320c0-8.8 7.2-16 16-16l112 0c17.7 0 32-14.3 32-32s-14.3-32-32-32L80 32z'/></svg>" CssClass="w-100 footer-interes"></asp:Label></a>

                    <a href="#">
                        <asp:Label runat="server" ID="Label5" Text="Destinatarios <span class='resaltado pr-5'></span> <svg xmlns='http://www.w3.org/2000/svg' height='15' width='15' viewBox='0 0 512 512'><path fill='#909090' d='M320 0c-17.7 0-32 14.3-32 32s14.3 32 32 32l82.7 0L201.4 265.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0L448 109.3l0 82.7c0 17.7 14.3 32 32 32s32-14.3 32-32l0-160c0-17.7-14.3-32-32-32L320 0zM80 32C35.8 32 0 67.8 0 112L0 432c0 44.2 35.8 80 80 80l320 0c44.2 0 80-35.8 80-80l0-112c0-17.7-14.3-32-32-32s-32 14.3-32 32l0 112c0 8.8-7.2 16-16 16L80 448c-8.8 0-16-7.2-16-16l0-320c0-8.8 7.2-16 16-16l112 0c17.7 0 32-14.3 32-32s-14.3-32-32-32L80 32z'/></svg>" CssClass="w-100 footer-interes"></asp:Label></a>

                </asp:Panel>

                <asp:Panel runat="server" CssClass="col-xs-1 col-md-1 col-lg-1 mt-5 mb-5" Style="border-left: 1px solid #cecdcd;">
                </asp:Panel>

                <asp:Panel runat="server" CssClass="col-xs-11 col-md-3 col-lg-3 mt-5 mb-5" Style="border-right: 1px solid #cecdcd;">

                    <a href="#">
                        <asp:Label runat="server" ID="Label6" Text="Factura comercial <span class='resaltado pr-5'>importación</span> <svg xmlns='http://www.w3.org/2000/svg' height='15' width='15' viewBox='0 0 512 512'><path fill='#909090' d='M320 0c-17.7 0-32 14.3-32 32s14.3 32 32 32l82.7 0L201.4 265.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0L448 109.3l0 82.7c0 17.7 14.3 32 32 32s32-14.3 32-32l0-160c0-17.7-14.3-32-32-32L320 0zM80 32C35.8 32 0 67.8 0 112L0 432c0 44.2 35.8 80 80 80l320 0c44.2 0 80-35.8 80-80l0-112c0-17.7-14.3-32-32-32s-32 14.3-32 32l0 112c0 8.8-7.2 16-16 16L80 448c-8.8 0-16-7.2-16-16l0-320c0-8.8 7.2-16 16-16l112 0c17.7 0 32-14.3 32-32s-14.3-32-32-32L80 32z'/></svg>" CssClass="w-100 footer-interes"></asp:Label></a>

                    <a href="#">
                        <asp:Label runat="server" ID="Label7" Text="Factura comercial <span class='resaltado pr-5'>exportación</span> <svg xmlns='http://www.w3.org/2000/svg' height='15' width='15'  viewBox='0 0 512 512'><path fill='#909090' d='M320 0c-17.7 0-32 14.3-32 32s14.3 32 32 32l82.7 0L201.4 265.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0L448 109.3l0 82.7c0 17.7 14.3 32 32 32s32-14.3 32-32l0-160c0-17.7-14.3-32-32-32L320 0zM80 32C35.8 32 0 67.8 0 112L0 432c0 44.2 35.8 80 80 80l320 0c44.2 0 80-35.8 80-80l0-112c0-17.7-14.3-32-32-32s-32 14.3-32 32l0 112c0 8.8-7.2 16-16 16L80 448c-8.8 0-16-7.2-16-16l0-320c0-8.8 7.2-16 16-16l112 0c17.7 0 32-14.3 32-32s-14.3-32-32-32L80 32z'/></svg>" CssClass="w-100 footer-interes"></asp:Label></a>

                </asp:Panel>

                <asp:Panel runat="server" CssClass="col-xs-1 col-1 col-md-1 col-lg-1 mt-5 mb-5"></asp:Panel>

                <asp:Panel runat="server" CssClass="col-xs-11 col-4 col-md-4 col-lg-3 mt-5 mb-5">

                    <a href="#">
                        <asp:Label runat="server" ID="Label8" Text="Acuse de <span class='resaltado pr-5'>Valor</span> <svg xmlns='http://www.w3.org/2000/svg' height='15' width='15'  viewBox='0 0 512 512'><path fill='#909090' d='M320 0c-17.7 0-32 14.3-32 32s14.3 32 32 32l82.7 0L201.4 265.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0L448 109.3l0 82.7c0 17.7 14.3 32 32 32s32-14.3 32-32l0-160c0-17.7-14.3-32-32-32L320 0zM80 32C35.8 32 0 67.8 0 112L0 432c0 44.2 35.8 80 80 80l320 0c44.2 0 80-35.8 80-80l0-112c0-17.7-14.3-32-32-32s-32 14.3-32 32l0 112c0 8.8-7.2 16-16 16L80 448c-8.8 0-16-7.2-16-16l0-320c0-8.8 7.2-16 16-16l112 0c17.7 0 32-14.3 32-32s-14.3-32-32-32L80 32z'/></svg>" CssClass="w-100 footer-interes"></asp:Label></a>

                    <a href="#">
                        <asp:Label runat="server" ID="Label9" Text="Clientes <span class='resaltado pr-5'></span> <svg xmlns='http://www.w3.org/2000/svg' height='15' width='15'  viewBox='0 0 512 512'><path fill='#909090' d='M320 0c-17.7 0-32 14.3-32 32s14.3 32 32 32l82.7 0L201.4 265.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0L448 109.3l0 82.7c0 17.7 14.3 32 32 32s32-14.3 32-32l0-160c0-17.7-14.3-32-32-32L320 0zM80 32C35.8 32 0 67.8 0 112L0 432c0 44.2 35.8 80 80 80l320 0c44.2 0 80-35.8 80-80l0-112c0-17.7-14.3-32-32-32s-32 14.3-32 32l0 112c0 8.8-7.2 16-16 16L80 448c-8.8 0-16-7.2-16-16l0-320c0-8.8 7.2-16 16-16l112 0c17.7 0 32-14.3 32-32s-14.3-32-32-32L80 32z'/></svg>" CssClass="w-100 footer-interes"></asp:Label></a>

                </asp:Panel>

            </asp:Panel>

        </ListControls>--%>

    </GWC:FieldsetControl>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server"></asp:Content>
