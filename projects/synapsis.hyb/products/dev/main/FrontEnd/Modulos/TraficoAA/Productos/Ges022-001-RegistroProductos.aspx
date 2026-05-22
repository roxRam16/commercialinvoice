<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/FrontEnd/Modulos/Home.Master" CodeBehind="Ges022-001-RegistroProductos.aspx.vb" Inherits=".Ges022_001_RegistroProductos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentFindbar" runat="server">

    <% If IsPopup = False Then %>

    <GWC:FindbarControl Label="Buscar Producto" ID="__SYSTEM_CONTEXT_FINDER" runat="server" OnClick="BusquedaGeneral" />

    <% End If %>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentCompanyList" runat="server">
    <% If IsPopup = False Then %>

    <GWC:SelectControl CssClass="col-auto company-list-select" runat="server" SearchBarEnabled="false" ID="__SYSTEM_ENVIRONMENT" OnSelectedIndexChanged="CambiarEmpresa" />

    <% End If %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="contentBody" runat="server">

    <link rel="stylesheet" type="text/css" href="estilos.css" />

    <div class="d-flex">
        <GWC:FormControl HasAutoSave="false" ID="__SYSTEM_MODULE_FORM" runat="server" Label="<span style='color:#321761'>Catálogo de</span><span style='color:#782360;'>&nbsp;productos</span>" OnCheckedChanged="MarcarPagina">
            <Buttonbar runat="server" OnClick="EventosBotonera">
                <DropdownButtons>
                    <GWC:ButtonItem Text="Descargar" />
                    <GWC:ButtonItem Text="Imprimir" />
                    <GWC:ButtonItem Text="Mandar por Correo" />
                    <GWC:ButtonItem Text="Buscar fracción arancelaria" />
                </DropdownButtons>
            </Buttonbar>
            <Fieldsets>
                <GWC:FieldsetControl ID="fscDatosGenerales" runat="server" Label="Datos generales">
                    <ListControls>
                        <GWC:InputControl runat="server" ID="icNombreComercial" CssClass="col-xs-9 col-md-10 col-lg-5" Label="Nombre comercial" Type="TextArea" />
                        <GWC:SwitchControl runat="server" ID="swcEstadoProducto" CssClass="col-xs-3 col-md-2 col-lg-1 mt-40" OnText="Si" OffText="No" Label="Habilitado" />
<%--                        <GWC:ImageControl runat="server" ID="icMuestraProducto" CssClass="col-xs-12 col-md-6 col-lg-6" Visible="False" Height="80px" Aspect="Cover" OnLoad="ActualizaImagen">
                        </GWC:ImageControl>
                        <GWC:FileControl runat="server" Label="Subir imagen" CssClass="col-xs-12 col-md-6  col-lg-6" ID="fcImagenProducto" OnChooseFile="fcImagenProducto_ChooseFile" Dragable="true" />--%>
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl ID="fscClasificacion" runat="server" Label="Clasificación">
                    <ListControls>

                        <GWC:CardControl runat="server" ID="BuscarFraccionesArancelarias" Visible="False" CssClass="container m-0 p-0 mb-5">
                            <ListControls>
                                <asp:Panel runat="server" CssClass="m-0 mt-0 p-0" ID="Panel1">
                                    <asp:Label runat="server" ID="lbTitle" Text="Buscar fracción arancelaría" CssClass="col-xs-12 col-md-12 col-lg-12 w-100 mb-3 title_Card"></asp:Label>
                                    <GWC:FindboxControl runat="server" ID="fbcFraccionArancelaria" CssClass="col-xs-11 col-md-11 col-lg-11 mb-5 pt-5" Label="Fracción arancelaria - Descripción | Nico - Descripción" OnTextChanged="fbc_FraccionArancelaria_TextChanged" OnClick="fbc_FraccionArancelaria_Click" RequiredSelect="true" />
                                    <GWC:ButtonControl runat="server" CssClass="col-xs-1 col-1 col-md-1 col-lg-1 mt-5 mb-3 d-flex justify-content-center" ID="btnAplicarFraccionArancelaria" Label="Aplicar" OnClick="btnAplicarFraccionArancelaria_Click" Enabled="False" />
                                </asp:Panel>
                            </ListControls>
                        </GWC:CardControl>

                        <asp:Panel runat="server" CssClass="d-flex justify-content-end w-100" ID="PanelBotonArchivado" Visible="False">

                            <asp:Button runat="server" ID="btnArchivar" CssClass="btnAction btnArchivar" OnClick="btnArchivar_Click" />

                            <asp:Button runat="server" ID="btnRestaurar" CssClass="btnAction btnRestaurar" OnClick="btnRestaurar_Click" style="opacity:0.8"/>

                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="row fieldset">

                            <asp:Panel runat="server" CssClass="col-xs-3 col-md-1 col-lg-1 align-content-center" ID="lbEstadoActivo" Visible="false">
                                <asp:Label runat="server" class="cl_Num__Tarjeta_principal cl_Num__Tarjeta_active  col-lg-12 col-md-12 col-xs-12 align-content-center estado" Style="margin-left: 30%" data-toggle="tooltip" title="Autorizado" Text="<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 448 512'><path fill='#ffffff' d='M438.6 105.4c12.5 12.5 12.5 32.8 0 45.3l-256 256c-12.5 12.5-32.8 12.5-45.3 0l-128-128c-12.5-12.5-12.5-32.8 0-45.3s32.8-12.5 45.3 0L160 338.7 393.4 105.4c12.5-12.5 32.8-12.5 45.3 0z'/></svg>"></asp:Label>
                                <span style="color: #808080; opacity: 0.6; font-weight: bold; text-align: center; font-size: 18px;" class="d-block col-lg-12 col-md-12 col-xs-12 pt-3">Autorizado</span>
                            </asp:Panel>

                            <asp:Panel runat="server" CssClass="col-xs-3 col-md-1 col-lg-1 align-content-center estado" ID="lbEstadoPreliminar" Visible="false">
                                <asp:Label runat="server" class="cl_Num__Tarjeta_principal cl_Num__Tarjeta_preliminar  col-md-1 align-content-center estado" Style="margin-left: 30%" data-toggle="tooltip" title="Preliminar" Text="<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 512 512'><path fill='#ffffff' d='M256 0c4.6 0 9.2 1 13.4 2.9L457.7 82.8c22 9.3 38.4 31 38.3 57.2c-.5 99.2-41.3 280.7-213.6 363.2c-16.7 8-36.1 8-52.8 0C57.3 420.7 16.5 239.2 16 140c-.1-26.2 16.3-47.9 38.3-57.2L242.7 2.9C246.8 1 251.4 0 256 0zm0 66.8l0 378.1C394 378 431.1 230.1 432 141.4L256 66.8s0 0 0 0z'/></svg>"></asp:Label>
                                <span style="color: #808080; opacity: 0.6; font-weight: bold; text-align: center; font-size: 18px;" class="d-block col-lg-12 col-md-12 col-xs-12 pt-3">Preliminar</span>
                            </asp:Panel>

                            <asp:Panel runat="server" CssClass="col-xs-3 col-md-1 col-lg-1 align-content-center" ID="lbEstadoClasificado" Visible="false">
                                <asp:Label runat="server" class="cl_Num__Tarjeta_principal cl_Num__Tarjeta_clasificado  col-md-1 align-content-center estado" Style="margin-left: 30%" data-toggle="tooltip" title="Clasificado" Text="<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 576 512'><path fill='#ffffff' d='M112 0C99.1 0 87.4 7.8 82.5 19.7l-66.7 160-13.3 32c-6.8 16.3 .9 35 17.2 41.8s35-.9 41.8-17.2L66.7 224l90.7 0 5.1 12.3c6.8 16.3 25.5 24 41.8 17.2s24-25.5 17.2-41.8l-13.3-32-66.7-160C136.6 7.8 124.9 0 112 0zm18.7 160l-37.3 0L112 115.2 130.7 160zM256 32l0 96 0 96c0 17.7 14.3 32 32 32l80 0c44.2 0 80-35.8 80-80c0-23.1-9.8-43.8-25.4-58.4c6-11.2 9.4-24 9.4-37.6c0-44.2-35.8-80-80-80L288 0c-17.7 0-32 14.3-32 32zm96 64l-32 0 0-32 32 0c8.8 0 16 7.2 16 16s-7.2 16-16 16zm-32 64l32 0 16 0c8.8 0 16 7.2 16 16s-7.2 16-16 16l-48 0 0-32zM566.6 310.6c12.5-12.5 12.5-32.8 0-45.3s-32.8-12.5-45.3 0L352 434.7l-73.4-73.4c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3l96 96c12.5 12.5 32.8 12.5 45.3 0l192-192z'/></svg>"></asp:Label>
                                <span style="color: #808080; opacity: 0.6; font-weight: bold; text-align: center; font-size: 18px;" class="d-block col-lg-12 col-md-12 col-xs-12 pt-3">Clasificado</span>
                            </asp:Panel>

                            <asp:Panel runat="server" CssClass="col-xs-3 col-md-1 col-lg-1 align-content-center" ID="lbEstadoSuprimido" Visible="false">
                                <asp:Label runat="server" class="cl_Num__Tarjeta_principal cl_Num__Tarjeta_suprimido  col-md-1 align-content-center estado" Style="margin-left: 30%" data-toggle="tooltip" title="Suprimido" Text="<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 448 512'><path fill='#ffffff' d='M432 256c0 17.7-14.3 32-32 32L48 288c-17.7 0-32-14.3-32-32s14.3-32 32-32l352 0c17.7 0 32 14.3 32 32z'/></svg>"></asp:Label>
                                <span style="color: #808080; opacity: 0.6; font-weight: bold; text-align: center; font-size: 18px;" class="d-block col-lg-12 col-md-12 col-xs-12 pt-3">Suprimido</span>
                            </asp:Panel>

                            <asp:Panel runat="server" CssClass="col-xs-3 col-md-1 col-lg-1 align-content-center" ID="lbEstadoDefault" Visible="true">
                                <asp:Label runat="server" class="cl_Num__Tarjeta_principal cl_Num__Tarjeta_sinestado  col-md-1 align-content-center estado" Style="margin-left: 30%" data-toggle="tooltip" title="Sin estado" Text="<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 448 512'><path fill='#ffffff' d='M438.6 105.4c12.5 12.5 12.5 32.8 0 45.3l-256 256c-12.5 12.5-32.8 12.5-45.3 0l-128-128c-12.5-12.5-12.5-32.8 0-45.3s32.8-12.5 45.3 0L160 338.7 393.4 105.4c12.5-12.5 32.8-12.5 45.3 0z'/></svg>"></asp:Label>
                            </asp:Panel>

                            <GWC:InputControl runat="server" ID="icFraccionArancelaria" CssClass="col-xs-9 col-md-2 col-lg-2 mb-5" Label="Fracción arancelaría" Type="Text" />

                            <GWC:InputControl runat="server" ID="icFraccionArancelariaDescripcion" CssClass="col-xs-12 col-md-9 col-lg-9 mb-5" Label="Descripción" Type="Text" />

                            <div class="col-xs-12 col-md-1 col-lg-1"></div>

                            <GWC:InputControl runat="server" ID="icNico" CssClass="col-xs-12 col-md-3 col-lg-2 mb-5 mt-3" Label="Nico" Type="Text" />

                            <GWC:InputControl runat="server" ID="icNicoDescripcion" CssClass="col-xs-12 col-md-9 col-lg-9 mb-5 mt-3" Label="Descripción" Type="Text" />

                            <GWC:InputControl runat="server" ID="icFechaRegistro" CssClass="col-xs-12 col-md-6 col-lg-6 mb-5 mt-5" Label="Fecha de registro" Type="Text" Format="Calendar" Enabled="false" />

                            <GWC:SelectControl runat="server" ID="scEstatus" CssClass="col-xs-12 col-md-6 col-lg-6 mb-5 mt-5" Label="Estatus" OnSelectedIndexChanged="scEstatus_SelectedIndexChanged">
                                <Options>
                                    <GWC:SelectOption Value="1" Text="Autorizado" />
                                    <GWC:SelectOption Value="2" Text="Preliminar" />
                                    <GWC:SelectOption Value="3" Text="Clasificado" />
                                    <GWC:SelectOption Value="4" Text="Suprimido" />
                                </Options>
                            </GWC:SelectControl>
                            
                            <GWC:InputControl runat="server" ID="icObservaciones" CssClass="col-xs-12 col-md-12 col-lg-12  mb-5 " Label="Observación" Type="TextArea" />

                            <GWC:InputControl runat="server" ID="icMotivo" CssClass="col-xs-12 col-md-12 col-lg-12 mb-5 mt-5" Label="Motivo" Type="TextArea" Visible="false" />

                            <asp:Panel runat="server" CssClass="d-flex justify-content-end w-100">
                                <asp:Button runat="server" CssClass="btnAction btnSubmit btn-archivado-producto" ID="btn_ConfirmarArchivado" Text="Confirmar" Visible="false" OnClick="btn_ConfirmarArchivado_Click"/>
                            </asp:Panel>

                        </asp:Panel>
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl ID="fscDescripcionFacturas" runat="server" Detail="Descripciones en facturas comerciales" Label="Descripciones">
                    <ListControls>
                        <GWC:PillboxControl runat="server" CssClass="col-xs-12" ID="pbcDescipcionesFacturas" KeyField="indice">
                            <ListControls>

                                <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-6 mb-4" ID="fbcCliente" Label="Cliente" OnTextChanged="fbc_Cliente_TextChanged" RequiredSelect="true" />
                                <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-6 mb-4" ID="fbcProveedor" Label="Proveedor" OnTextChanged="fbc_Proveedor_TextChanged" RequiredSelect="true" />

                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 mt-5 mb-5">
                                    <asp:Label runat="server" ID="lbDescripciones" Text="Descripciones" CssClass="w-100 cl_Secciones mt-5"></asp:Label>
                                </asp:Panel>

                                
                     <GWC:CatalogControl runat="server" ID="ccDescipcionesFacturas" CssClass="col custom_cat_3" KeyField="indice" CanDelete="true" CanAdd="true" CanClone="false" CanPrint="false" >
                                    <Columns>
                                        <GWC:InputControl Type="Text" runat="server" ID="icIdKrom" Label="Id Krom" Enabled="false" ExternalAutoPostBack="false"/>
                                        <GWC:InputControl Type="Text" runat="server" ID="icNumeroParte" Label="Número de parte" ExternalAutoPostBack="false"/>
                                        <GWC:InputControl runat="server" ID="icDescripcion" Label="Descripción" Type="Text" ExternalAutoPostBack="false"/>

                                        <GWC:SwitchControl runat="server" ID="swcAplicaCove" OnText="Si" OffText="No" Label="Aplica COVE" Checked="true" />
                                       
                                        <GWC:InputControl runat="server" ID="icDescripcionCove" Label="Descripción COVE" Type="Text" ExternalAutoPostBack="false"/>
                                        <GWC:InputControl Type="Text" runat="server" ID="icAlias" Label="Alias" ExternalAutoPostBack="false"/>
<%--                                        <GWC:SelectControl runat="server" ID="scTipoAlias" Label="Tipo Alias" LocalSearch="true">
                                            <Options>
                                                <GWC:SelectOption Value="1" Text="Modelo" />
                                                <GWC:SelectOption Value="2" Text="Submodelo" />
                                                <GWC:SelectOption Value="3" Text="Número de série" />
                                                <GWC:SelectOption Value="4" Text="SKU" />
                                                <GWC:SelectOption Value="5" Text="Código interno" />
                                            </Options>
                                        </GWC:SelectControl>--%>
                                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-5 mt-2 mb-5 p-0 d-flex justify-content-end" ID="scTipoAliasUNO" Label="Tipo Alias">
                                              <Options >
                                                            <GWC:SelectOption Value="1" Text="Modelo" />
                                                            <GWC:SelectOption Value="2" Text="Submodelo" />
                                                            <GWC:SelectOption Value="3" Text="Número de série" />
                                                            <GWC:SelectOption Value="4" Text="SKU" />
                                                            <GWC:SelectOption Value="5" Text="Código interno" />
                                              </Options>
                                        </GWC:SelectControl>
                                                </Columns>
                                            </GWC:CatalogControl>

                            </ListControls>
                        </GWC:PillboxControl>
                        <asp:Panel runat="server" CssClass="w-100 fieldset custom_cat_3_content">
                        </asp:Panel>
                    </ListControls>
                </GWC:FieldsetControl>
                <GWC:FieldsetControl ID="fscHistoriales" runat="server" Label="Historiales" Visible="false">
                    <ListControls>

                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 mt-5 mb-5">
                            <asp:Label runat="server" ID="Label2" Text="Historial de descripciones" CssClass="w-100 cl_Secciones mt-5"></asp:Label>
                        </asp:Panel>

                        <GWC:CatalogControl runat="server" ID="ccHistorialClasificacion" CssClass="w-100" KeyField="indice" UserInteraction="false">
                            <Columns>
                                <GWC:InputControl Type="Text" runat="server" ID="icHistoricoFraccion" Label="Fracción-Nico" />
                                <GWC:InputControl Type="Text" runat="server" ID="icHistoricoMotivo" Label="Motivo" />
                                <GWC:InputControl Type="Text" runat="server" ID="icHistoricoFechaModificacion" Label="Fecha Modificacion" />
                                <GWC:InputControl Type="Text" runat="server" ID="icHistoricoUsuario" Label="Clasificador" />
                                <GWC:InputControl Type="Text" runat="server" ID="icHistoricoOficina" Label="Oficina" />
                            </Columns>
                        </GWC:CatalogControl>



                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 mt-5 mb-5">
                            <asp:Label runat="server" ID="Label1" Text="Historial de clasificaciones" CssClass="w-100 cl_Secciones mt-5"></asp:Label>
                        </asp:Panel>

                        <GWC:CatalogControl runat="server" ID="ccHistorialDescripciones" CssClass="w-100" KeyField="indice" UserInteraction="false">
                            <Columns>
                                <GWC:InputControl Type="Text" runat="server" ID="icHistoricoNumeroParte" Label="Número de parte" />
                                <GWC:InputControl Type="Text" runat="server" ID="icHistoricoDescripcion" Label="Descripción" />
                                <GWC:InputControl Type="Text" runat="server" ID="icHistoricoProveedor" Label="Proveedor" />
                                <GWC:InputControl Type="Text" runat="server" ID="icHistoricoCliente" Label="Cliente" />
                                <GWC:InputControl Type="Text" runat="server" ID="icHistoricoFechaModificacionDescripciones" Label="Fecha Modificacion" />
                                <GWC:InputControl Type="Text" runat="server" ID="icHistoricoUsuarioDescripciones" Label="Clasificador" />
                                <GWC:InputControl Type="Text" runat="server" ID="icHistoricoOficinaDescripciones" Label="Oficina" />
                                <%--<GWC:InputControl Type="Text" runat="server" ID="icHistoricoFechaArchivado" Label="Fecha archivado"/>--%>
                            </Columns>
                        </GWC:CatalogControl>



                    </ListControls>
                </GWC:FieldsetControl>

   <%--       <GWC:FieldsetControl runat="server" ID="FieldsetControl1" Label="" Detail="" CssClass="mt-5 p-0 mb-5">
            <ListControls>
               <asp:Panel runat="server" CssClass="row p-5 m-5" Style="border: 1px solid #cecdcd; border-radius: 14px; margin-bottom: 20px; padding-bottom:25px!important">

                   <asp:Panel runat="server" CssClass="col-xs-12 col-12 col-md-12 col-lg-12 mt-5">
                       <asp:Label runat="server" ID="Label3" Text="También te podría interesar" CssClass="w-100 cl_Secciones mt-5 ml-5"></asp:Label>
                   </asp:Panel>

                   <asp:Panel runat="server" CssClass="col-xs-1 col-1 col-md-1 col-lg-1 mt-5 mb-5"></asp:Panel>

                   <asp:Panel runat="server" CssClass="col-xs-11 col-4 col-md-4 col-lg-3 mt-5 mb-5">

                       <a href="#">
                           <asp:Label runat="server" ID="Label4" Text="Catálogo de <span class='resaltado pr-5'>clientes</span> <svg xmlns='http://www.w3.org/2000/svg' height='15' width='15' viewBox='0 0 512 512'><path fill='#909090' d='M320 0c-17.7 0-32 14.3-32 32s14.3 32 32 32l82.7 0L201.4 265.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0L448 109.3l0 82.7c0 17.7 14.3 32 32 32s32-14.3 32-32l0-160c0-17.7-14.3-32-32-32L320 0zM80 32C35.8 32 0 67.8 0 112L0 432c0 44.2 35.8 80 80 80l320 0c44.2 0 80-35.8 80-80l0-112c0-17.7-14.3-32-32-32s-32 14.3-32 32l0 112c0 8.8-7.2 16-16 16L80 448c-8.8 0-16-7.2-16-16l0-320c0-8.8 7.2-16 16-16l112 0c17.7 0 32-14.3 32-32s-14.3-32-32-32L80 32z'/></svg>" CssClass="w-100 footer-interes"></asp:Label></a>

                       <a href="#">
                           <asp:Label runat="server" ID="Label5" Text="Catálogo de <span class='resaltado pr-5'>TIGIE</span> <svg xmlns='http://www.w3.org/2000/svg' height='15' width='15' viewBox='0 0 512 512'><path fill='#909090' d='M320 0c-17.7 0-32 14.3-32 32s14.3 32 32 32l82.7 0L201.4 265.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0L448 109.3l0 82.7c0 17.7 14.3 32 32 32s32-14.3 32-32l0-160c0-17.7-14.3-32-32-32L320 0zM80 32C35.8 32 0 67.8 0 112L0 432c0 44.2 35.8 80 80 80l320 0c44.2 0 80-35.8 80-80l0-112c0-17.7-14.3-32-32-32s-32 14.3-32 32l0 112c0 8.8-7.2 16-16 16L80 448c-8.8 0-16-7.2-16-16l0-320c0-8.8 7.2-16 16-16l112 0c17.7 0 32-14.3 32-32s-14.3-32-32-32L80 32z'/></svg>" CssClass="w-100 footer-interes"></asp:Label></a>

                   </asp:Panel>

                   <asp:Panel runat="server" CssClass="col-xs-12 col-md-1 col-lg-1 mt-5 mb-5"  style=" border-left:1px solid #cecdcd;">
                   
                   </asp:Panel>

                   <asp:Panel runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mt-5 mb-5"  style=" border-right:1px solid #cecdcd;">

                       <a href="#">
                           <asp:Label runat="server" ID="Label6" Text="Proveedores <span class='resaltado pr-5'>nacionales</span> <svg xmlns='http://www.w3.org/2000/svg' height='15' width='15' viewBox='0 0 512 512'><path fill='#909090' d='M320 0c-17.7 0-32 14.3-32 32s14.3 32 32 32l82.7 0L201.4 265.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0L448 109.3l0 82.7c0 17.7 14.3 32 32 32s32-14.3 32-32l0-160c0-17.7-14.3-32-32-32L320 0zM80 32C35.8 32 0 67.8 0 112L0 432c0 44.2 35.8 80 80 80l320 0c44.2 0 80-35.8 80-80l0-112c0-17.7-14.3-32-32-32s-32 14.3-32 32l0 112c0 8.8-7.2 16-16 16L80 448c-8.8 0-16-7.2-16-16l0-320c0-8.8 7.2-16 16-16l112 0c17.7 0 32-14.3 32-32s-14.3-32-32-32L80 32z'/></svg>" CssClass="w-100 footer-interes"></asp:Label></a>

                       <a href="#">
                           <asp:Label runat="server" ID="Label7" Text="Proveedores <span class='resaltado pr-5'>extranjeros</span> <svg xmlns='http://www.w3.org/2000/svg' height='15' width='15'  viewBox='0 0 512 512'><path fill='#909090' d='M320 0c-17.7 0-32 14.3-32 32s14.3 32 32 32l82.7 0L201.4 265.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0L448 109.3l0 82.7c0 17.7 14.3 32 32 32s32-14.3 32-32l0-160c0-17.7-14.3-32-32-32L320 0zM80 32C35.8 32 0 67.8 0 112L0 432c0 44.2 35.8 80 80 80l320 0c44.2 0 80-35.8 80-80l0-112c0-17.7-14.3-32-32-32s-32 14.3-32 32l0 112c0 8.8-7.2 16-16 16L80 448c-8.8 0-16-7.2-16-16l0-320c0-8.8 7.2-16 16-16l112 0c17.7 0 32-14.3 32-32s-14.3-32-32-32L80 32z'/></svg>" CssClass="w-100 footer-interes"></asp:Label></a>

                   </asp:Panel>

                    <asp:Panel runat="server" CssClass="col-xs-1 col-1 col-md-1 col-lg-1 mt-5 mb-5" ></asp:Panel>

                   <asp:Panel runat="server" CssClass="col-xs-11 col-4 col-md-4 col-lg-3 mt-5 mb-5">

                       <a href="#"><asp:Label runat="server" ID="Label8" Text="Factura comercial <span class='resaltado pr-5'>Importación</span> <svg xmlns='http://www.w3.org/2000/svg' height='15' width='15'  viewBox='0 0 512 512'><path fill='#909090' d='M320 0c-17.7 0-32 14.3-32 32s14.3 32 32 32l82.7 0L201.4 265.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0L448 109.3l0 82.7c0 17.7 14.3 32 32 32s32-14.3 32-32l0-160c0-17.7-14.3-32-32-32L320 0zM80 32C35.8 32 0 67.8 0 112L0 432c0 44.2 35.8 80 80 80l320 0c44.2 0 80-35.8 80-80l0-112c0-17.7-14.3-32-32-32s-32 14.3-32 32l0 112c0 8.8-7.2 16-16 16L80 448c-8.8 0-16-7.2-16-16l0-320c0-8.8 7.2-16 16-16l112 0c17.7 0 32-14.3 32-32s-14.3-32-32-32L80 32z'/></svg>" CssClass="w-100 footer-interes"></asp:Label></a>

                        <a href="#"><asp:Label runat="server" ID="Label9" Text="Factura comercial <span class='resaltado pr-5'>Exportación</span> <svg xmlns='http://www.w3.org/2000/svg' height='15' width='15'  viewBox='0 0 512 512'><path fill='#909090' d='M320 0c-17.7 0-32 14.3-32 32s14.3 32 32 32l82.7 0L201.4 265.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0L448 109.3l0 82.7c0 17.7 14.3 32 32 32s32-14.3 32-32l0-160c0-17.7-14.3-32-32-32L320 0zM80 32C35.8 32 0 67.8 0 112L0 432c0 44.2 35.8 80 80 80l320 0c44.2 0 80-35.8 80-80l0-112c0-17.7-14.3-32-32-32s-32 14.3-32 32l0 112c0 8.8-7.2 16-16 16L80 448c-8.8 0-16-7.2-16-16l0-320c0-8.8 7.2-16 16-16l112 0c17.7 0 32-14.3 32-32s-14.3-32-32-32L80 32z'/></svg>" CssClass="w-100 footer-interes"></asp:Label></a>

                   </asp:Panel>

               </asp:Panel>

           </ListControls>

          </GWC:FieldsetControl>--%>

   </Fieldsets>

        </GWC:FormControl>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">
    <script>
        $(() => {

            $("body").on("click", "input[type='checkbox']", (e) => {

                const tr = e.target.closest(".__row");

                if (tr) {

                    if (e.target.checked) {

                        const desc = tr.querySelector("td:nth-child(4) input");

                        const desccove = tr.querySelector("td:nth-child(6) input");

                        desccove.value = desc.value;

                    }

                }

            });

        });


        $('.estado').tooltip({
            placement: 'top',  // Puedes cambiar la ubicación si lo deseas
            trigger: 'hover'   // Puedes cambiar el tipo de trigger (click, focus, etc.)
        });

    </script>
</asp:Content>
