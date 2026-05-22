<%@ Page Title="" Language="vb" Async="true" AutoEventWireup="false" MasterPageFile="~/FrontEnd/Modulos/Home.Master" CodeBehind="Ges003-001-FacturasComerciales.aspx.vb" Inherits=".Ges003_001_FacturasComerciales" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentFindbar" runat="server">

    <% If IsPopup = False Then %>

    <GWC:FindbarControl Label="Buscar factura de importación" ID="__SYSTEM_CONTEXT_FINDER" runat="server" OnClick="BusquedaGeneral" />

    <% End If %>

<link rel="stylesheet" type="text/css" href="Estilos.css" />

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentCompanyList" runat="server">

    <% If IsPopup = False Then %>

 <GWC:SelectControl CssClass="col-auto company-list-select" runat="server" SearchBarEnabled="false" ID="__SYSTEM_ENVIRONMENT" OnSelectedIndexChanged="CambiarEmpresa" />

 <% End If %>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentBody" runat="server">
    <input type="hidden" id="hdnDialogResponse" name="hdnDialogResponse" value="" />

    <div class="d-flex">

        <GWC:FormControl runat="server" ID="__SYSTEM_MODULE_FORM" HasAutoSave="false" Label="<span style='color:#321761'>Factura comercial</span><span style='color:#782360;'>&nbsp;importación</span>" OnCheckedChanged="MarcarPagina">

            <Buttonbar runat="server" OnClick="EventosBotonera" Onload ="MostrarBotones" ID="bbBotonera">
                <DropdownButtons>
                    <GWC:ButtonItem Text="Descargar" Enabled="False"/>
                    <GWC:ButtonItem Text="Imprimir" Enabled="False"/>
                    <GWC:ButtonItem Text="Mandar por Correo" Enabled="False"/>
                    <GWC:ButtonItem Text="Publicar" ID="btiPublicar"/>
                    <GWC:ButtonItem Text="Acerca de" Enabled="False"/>
                </DropdownButtons>
            </Buttonbar>

            <Fieldsets>
                <GWC:FieldsetControl runat="server" ID="fscGenerales" Label="Generales">

                    <ListControls>

                        <GWC:NotifyControl runat="server" ID="ntPublicarFacturaComercial" Important="true" Visible="false" 
                            Color="normal" Icon="info" Title="¡Atención!" Message="✨ ¿Está apunto de publicar esta factura, desea continuar?" 
                            TextButton="Cancelar" TextButtonTwo="Si, publicar" OnClick="ntPublicarFacturaComercial_Click" OnClickTwo="ntPublicarFacturaComercial_ClickTwo"/>

                        <asp:Panel runat="server" CssClass="col-xs-2 col-md-6 col-lg-1 align-content-center" ID="lbModoCapturaIA" Style="padding-left: 4rem" Visible="false">
                            <asp:Label runat="server" class="cl_Num__Tarjeta_principal cl_Num__Tarjeta_IA col-md-1 align-content-center" Text="<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 640 512'><path fill='#ffffff' d='M320 0c17.7 0 32 14.3 32 32V96H472c39.8 0 72 32.2 72 72V440c0 39.8-32.2 72-72 72H168c-39.8 0-72-32.2-72-72V168c0-39.8 32.2-72 72-72H288V32c0-17.7 14.3-32 32-32zM208 384c-8.8 0-16 7.2-16 16s7.2 16 16 16h32c8.8 0 16-7.2 16-16s-7.2-16-16-16H208zm96 0c-8.8 0-16 7.2-16 16s7.2 16 16 16h32c8.8 0 16-7.2 16-16s-7.2-16-16-16H304zm96 0c-8.8 0-16 7.2-16 16s7.2 16 16 16h32c8.8 0 16-7.2 16-16s-7.2-16-16-16H400zM264 256a40 40 0 1 0 -80 0 40 40 0 1 0 80 0zm152 40a40 40 0 1 0 0-80 40 40 0 1 0 0 80zM48 224H64V416H48c-26.5 0-48-21.5-48-48V272c0-26.5 21.5-48 48-48zm544 0c26.5 0 48 21.5 48 48v96c0 26.5-21.5 48-48 48H576V224h16z'/></svg>"></asp:Label>
                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-xs-2 col-md-6 col-lg-1 align-content-center" ID="lbModoCapturaIAEditar" Style="padding-left: 4rem" Visible="false">
                            <asp:Label runat="server" class="cl_Num__Tarjeta_principal cl_Num__Tarjeta_gray -+ col-md-1 align-content-center" Text="<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 640 512'><path fill='#ffffff' d='M320 0c17.7 0 32 14.3 32 32V96H472c39.8 0 72 32.2 72 72V440c0 39.8-32.2 72-72 72H168c-39.8 0-72-32.2-72-72V168c0-39.8 32.2-72 72-72H288V32c0-17.7 14.3-32 32-32zM208 384c-8.8 0-16 7.2-16 16s7.2 16 16 16h32c8.8 0 16-7.2 16-16s-7.2-16-16-16H208zm96 0c-8.8 0-16 7.2-16 16s7.2 16 16 16h32c8.8 0 16-7.2 16-16s-7.2-16-16-16H304zm96 0c-8.8 0-16 7.2-16 16s7.2 16 16 16h32c8.8 0 16-7.2 16-16s-7.2-16-16-16H400zM264 256a40 40 0 1 0 -80 0 40 40 0 1 0 80 0zm152 40a40 40 0 1 0 0-80 40 40 0 1 0 0 80zM48 224H64V416H48c-26.5 0-48-21.5-48-48V272c0-26.5 21.5-48 48-48zm544 0c26.5 0 48 21.5 48 48v96c0 26.5-21.5 48-48 48H576V224h16z'/></svg>"></asp:Label>
                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-xs-2 col-md-6 col-lg-1 align-content-center" ID="lbModoCapturaManual" Style="padding-left: 4rem" Visible="false">
                            <asp:Label runat="server" class="cl_Num__Tarjeta_principal cl_Num__Tarjeta_blue  col-md-1 align-content-center" Text="<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 512 512'><path fill='#ffffff' d='M224 0c17.7 0 32 14.3 32 32V240H192V32c0-17.7 14.3-32 32-32zm96 160c17.7 0 32 14.3 32 32v64c0 17.7-14.3 32-32 32s-32-14.3-32-32V192c0-17.7 14.3-32 32-32zm64 64c0-17.7 14.3-32 32-32s32 14.3 32 32v64c0 17.7-14.3 32-32 32s-32-14.3-32-32V224zM93.3 51.2L175.9 240H106.1L34.7 76.8C27.6 60.6 35 41.8 51.2 34.7s35.1 .3 42.1 16.5zm27 221.3l-.2-.5h69.9H216c22.1 0 40 17.9 40 40s-17.9 40-40 40H160c-8.8 0-16 7.2-16 16s7.2 16 16 16h56c39.8 0 72-32.2 72-72l0-.6c9.4 5.4 20.3 8.6 32 8.6c13.2 0 25.4-4 35.6-10.8c8.7 24.9 32.5 42.8 60.4 42.8c11.7 0 22.6-3.1 32-8.6V352c0 88.4-71.6 160-160 160H226.3c-42.4 0-83.1-16.9-113.1-46.9l-11.6-11.6C77.5 429.5 64 396.9 64 363V336c0-32.7 24.6-59.7 56.3-63.5z'/></svg>"></asp:Label>
                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-xs-2 col-md-6 col-lg-1 align-content-center" ID="lbModoCapturaManualNuevo" Style="padding-left: 4rem" Visible="true">
                            <asp:Label runat="server" class="cl_Num__Tarjeta_principal cl_Num__Tarjeta_gray  col-md-1 align-content-center" Text="<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 512 512'><path fill='#ffffff' d='M224 0c17.7 0 32 14.3 32 32V240H192V32c0-17.7 14.3-32 32-32zm96 160c17.7 0 32 14.3 32 32v64c0 17.7-14.3 32-32 32s-32-14.3-32-32V192c0-17.7 14.3-32 32-32zm64 64c0-17.7 14.3-32 32-32s32 14.3 32 32v64c0 17.7-14.3 32-32 32s-32-14.3-32-32V224zM93.3 51.2L175.9 240H106.1L34.7 76.8C27.6 60.6 35 41.8 51.2 34.7s35.1 .3 42.1 16.5zm27 221.3l-.2-.5h69.9H216c22.1 0 40 17.9 40 40s-17.9 40-40 40H160c-8.8 0-16 7.2-16 16s7.2 16 16 16h56c39.8 0 72-32.2 72-72l0-.6c9.4 5.4 20.3 8.6 32 8.6c13.2 0 25.4-4 35.6-10.8c8.7 24.9 32.5 42.8 60.4 42.8c11.7 0 22.6-3.1 32-8.6V352c0 88.4-71.6 160-160 160H226.3c-42.4 0-83.1-16.9-113.1-46.9l-11.6-11.6C77.5 429.5 64 396.9 64 363V336c0-32.7 24.6-59.7 56.3-63.5z'/></svg>"></asp:Label>
                        </asp:Panel>

                        <GWC:InputControl runat="server" ID="icTipoCargaDatos" Type="Text" Name="icTipoCargaDatos" Label="Tipo Carga Datos" Visible="false" />

                        <GWC:DualityBarControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-5 campo-requerido" ID="dbcNumFacturaCOVE" Label="*Número de factura" LabelDetail="Acuse de valor" OnClick="dbcNumFacturaCOVE_Click" 
                            Rules="maxlegth[150]" VisibleButton="false"   OnTextChanged="dbcNumFacturaCOVE_TextChanged"  ExternalAutoPostBack="False" />
                      <%--  <GWC:InputControl runat="server" Type="Hide" id="icidcove"/>--%>

                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-6 mt-5 p-0">
                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-6 mt-5 campo-requerido datepicker2" 
                                      ID="icFechaFacturaImpo" Type="Text" Format="Calendar"
                                      Name="icFechaFacturaImpo" Label="*Fecha de factura"  ExternalAutoPostBack="False"/>

                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-6 mt-5" ID="icFechaCOVE" Type="Text" 
                                Format="Calendar" Name="icFechaCOVE" Label="Fecha de acuse de valor" Enabled="False"/>
                        </asp:Panel>

                           <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 mt-5">

                            <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-6 campo-requerido" ID="fbcCliente" 
                                Label="*Cliente" HasDetails="true" Rules="required" RequiredSelect="true" OnTextChanged="fbcCliente_TextChanged" OnClick="fbcCliente_Click"
                                OnClickClose="fbcCliente_ClickClose" Search="true" OnClickSearch="fbcCliente_ClickSearch"/>

                            <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-12 col-lg-6 campo-requerido" ID="fbcIncoterm" Label="*Incoterm" HasDetails="true" Rules="required" RequiredSelect="true" KeyField="i_Cve_TerminoFacturacion" DisplayField="t_ValorPresentacion" 
                                Dimension="Vt022TerminosFacturacionA14" />
                         
                            </asp:Panel>

                  <%--      <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-12 col-lg-6 mt-5 campo-requerido" ID="fbcPais" Label="*País moneda" RequiredSelect="true" OnClick="fbcPais_Click" OnTextChanged="fbcPais_TextChanged" 
                            OnClickClose="fbcPais_ClickClose"/>--%>
                        
                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 col-lg-3 mt-5 pt-5">

                            <GWC:InputControl runat="server" 
                                CssClass="col-xs-8 col-md-8 p-0 input-border-right campo-requerido" 
                                ID="icValorFactura" 
                                Type="Text" 
                                Name="icValorFactura" 
                                Label="*Valor factura" 
                                Format="Real" />

                            <GWC:SelectControl runat="server" CssClass="col-xs-4 col-md-4 p-0 pl-1" ID="scMonedaFactura" 
                                SearchBarEnabled="true" LocalSearch="True" Label="Moneda" OnClick="scMonedaFactura_Click" 
                                OnSelectedIndexChanged="scMonedaFactura_SelectedIndexChanged">
                            </GWC:SelectControl>
                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 col-lg-3 mt-5 pt-5">
                            <GWC:InputControl runat="server" CssClass="col-xs-8 col-md-8 p-0 input-border-right campo-requerido" ID="icValorMercancia" Type="Text" Name="icValorMercancia" Label="*Valor mercancía" Format="Real" />
                            <GWC:SelectControl runat="server" CssClass="col-xs-4 col-md-4 p-0 pl-1" ID="scMonedaMercancia" SearchBarEnabled="true" LocalSearch="true" Label="Moneda" 
                                OnClick="scMonedaMercancia_Click" OnSelectedIndexChanged="scMonedaMercancia_SelectedIndexChanged">
                            </GWC:SelectControl>
                        </asp:Panel>

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mt-5 pt-5" ID="icPesoTotal" Type="Text" Format="Real" Name="icPesoTotal" Label="Peso total (Kg)" />
                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mt-5 pt-5" ID="icBultos" Type="Text" Label="Bultos" Format="Real" />
                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mt-5 mb-5" ID="icOrdenCompra" Type="Text" Label="Orden de compra"  />
                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mt-5 mb-5" ID="icReferenciaCliente" Type="Text" Label="Referencia del cliente"  />
                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mt-5" ID="icFolioFactura" Type="Text" Label="Serie | Folio de factura"  />
                         <GWC:SwitchControl runat="server" CssClass="col-xs-3 col-md-2 col-lg-1 mt-3 align-content-center" ID="swcEnajenacion" Label="Enajenación" OnText="Sí" OffText="No" LabelVisible="true"/>
                        <GWC:SwitchControl runat="server" CssClass="col-xs-3 col-md-2 col-lg-1 mt-3 align-content-center" ID="swcSubdivision" Label="Subdivisión" OnText="Sí" OffText="No" LabelVisible="true" />

                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fscProveedor" Label="Proveedor" Detail="Proveedor" CssClass="mt-5 p-0 mb-5">
                    <ListControls>
                        <GWC:FindboxControl runat="server" CssClass="col-lg-6 col-md-6 col-xs-12 campo-requerido" ID="fbcProveedor" Label="*Razón social proveedor" HasDetails="true" 
                            Rules="required" RequiredSelect="true" OnClick="fbcProveedor_Click" OnTextChanged="fbcProveedor_TextChanged" OnSelectedIndexChanged="fbcProveedor_SelectedIndexChanged" 
                            OnClickClose="fbcProveedor_ClickClose" Search="true" OnClickSearch="fbcProveedor_ClickSearch"
                            />
                        <GWC:SelectControl runat="server" CssClass="col-lg-6 col-md-6 col-xs-12 campo-requerido" ID="scDomiciliosProveedor" Label="*Domicilio fiscal" OnTextChanged="scDomiciliosProveedor_OnTextChanged" OnSelectedIndexChanged="scDomiciliosProveedor_SelectedIndexChanged" OnClick="scDomiciliosProveedor_Click" SearchBarEnabled="true" LocalSearch="true"></GWC:SelectControl>
                        <GWC:SelectControl runat="server" CssClass="col-lg-6 col-md-6 col-xs-12 mt-5" ID="scVinculacion" Label="Vinculación" OnClick="scVinculacion_Click">
                        </GWC:SelectControl>
                        <GWC:SelectControl runat="server" CssClass="col-lg-6 col-md-6 col-xs-12 mt-5" ID="scMetodoValoracion" 
                            Label="Método de valoración"  SearchBarEnabled="true" LocalSearch="true" OnClick="scMetodoValoracion_Click" OnTextChanged="scMetodoValoracion_TextChanged" OnSelectedIndexChanged="scMetodoValoracion_SelectedIndexChanged">
                        </GWC:SelectControl>
                        <GWC:SwitchControl runat="server" ID="swcFungeCertificado" CssClass="col-lg-2 col-md-6 col-12 col-xs-12 mt-5 mb-5 justify-content-center ml-1" Label="Certifica origen" OnText="Sí" OffText="No" OnCheckedChanged="swcFungeCertificado_CheckedChanged" LabelVisible="true"/>
                        <GWC:FindboxControl runat="server" CssClass="col-lg-4 col-md-6 col-12 col-xs-12 mt-5 mb-5" ID="fbcProveedorCertificado" Label="Proveedor que   certifica origen" HasDetails="false" OnTextChanged="fbcProveedorCertificado_TextChanged" />
                
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fscPartidas" Label="Items" CssClass="mt-5">
                    <ListControls>
                        <GWC:PillboxControl runat="server" ID="pbPartidas" KeyField="indice" CssClass="col-xs-12" OnClick="pbPartidas_Click" OnCheckedChange="pbPartidas_CheckedChange">
                            <ListControls>
                                <asp:Panel runat="server" CssClass="col-md-1 col-lg-1 col-xs-2 d-flex align-items-center flex-column margin-bottom">
                                    <asp:Label runat="server" ID="lbNumero" class="cl_Num__Tarjeta" Text="0"></asp:Label>
                                </asp:Panel>
                                <GWC:InputControl runat="server" ID="icObjectIdPartida" Label="ObjectId Partida" Format="MongoObjectId" Visible="False"/>
                                <GWC:FindboxControl runat="server" CssClass="col-xs-10 col-md-5 mt-5 campo-requerido" ID="fbcProducto" 
                                    Label="*Número de parte | Descripción | Alias" HasDetails="true" RequiredSelect="true" OnTextChanged="fbcProducto_TextChanged" OnClick="fbcProducto_Click" 
                                    OnClickClose="fbcProducto_ClickClose" Search="true" OnClickSearch="fbcProducto_ClickSearch"/>
                                <GWC:InputControl runat="server" ID="icCantidadComercial" CssClass="col-xs-12 col-md-3 col-lg-3 mt-5" Type="Text" Format="Real" Name="icCantidadComercial" Label="*Cantidad comercial" />
                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mt-5" ID="scUnidadMedidaComercial" SearchBarEnabled="true" LocalSearch="true" Label="*Unidad Medida Comercial (UMC)" OnClick="scUnidadMedidaComercial_Click" OnTextChanged="scUnidadMedidaComercial_TextChanged" OnSelectedIndexChanged="scUnidadMedidaComercial_SelectedIndexChanged" />
                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 col-lg-3 mb-3 mt-4">
                                    <GWC:InputControl runat="server" ID="icValorfacturaPartida" CssClass="col-xs-8 col-md-8 col-lg-8 p-0 input-border-right campo-requerido" Type="Text" Name="icValorfacturaPartida" Label="*Valor factura item" Format="Real"/>
                                    <GWC:SelectControl runat="server" CssClass="col-xs-4 col-md-4 col-lg-4 p-0 pl-1" ID="scMonedaFacturaPartida" SearchBarEnabled="true" LocalSearch="true" Label="Moneda" 
                                        OnClick="scMonedaFacturaPartida_Click" OnSelectedIndexChanged="scMonedaFacturaPartida_SelectedIndexChanged">
                                    </GWC:SelectControl>
                                </asp:Panel>
                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 col-lg-3 mb-3 mt-4">
                                    <GWC:InputControl runat="server" ID="icValorMercanciaPartida" CssClass="col-xs-8 col-md-8 col-lg-8 p-0 input-border-right campo-requerido" Type="Text" Name="icValorMercanciaPartida" Label="*Valor mercancía item" Format="Real" />
                                    <GWC:SelectControl runat="server" CssClass="col-xs-4 col-md-4 col-lg-4 p-0 pl-1" ID="scMonedaMercanciaPartida" SearchBarEnabled="true" LocalSearch="true" Label="Moneda" 
                                        OnClick="scMonedaMercanciaPartida_Click" OnSelectedIndexChanged="scMonedaMercanciaPartida_SelectedIndexChanged">
                                    </GWC:SelectControl>
                                </asp:Panel>

                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 col-lg-3 mb-3 mt-4">
                                    <GWC:InputControl runat="server" ID="icPrecioUnitario" CssClass="col-xs-8 col-md-8 col-lg-8 p-0 input-border-right campo-requerido" Type="Text" Name="icPrecioUnitario" Label="*Precio unitario" Format="Real" />
                                    <GWC:SelectControl runat="server" CssClass="col-xs-4 col-md-4 col-lg-4 p-0 pl-1" ID="scMonedaPrecioUnitarioPartida" SearchBarEnabled="true" LocalSearch="True" Label="Moneda" 
                                        OnClick="scMonedaPrecioUnitarioPartida_Click" OnSelectedIndexChanged="scMonedaPrecioUnitarioPartida_SelectedIndexChanged">
                                    </GWC:SelectControl>
                                </asp:Panel>

                                <GWC:InputControl runat="server" ID="icPesoNeto" CssClass="col-xs-12 col-md-6 col-lg-3 mb-3 mt-4" Type="Text" Format="Real" Name="icPesoNeto" Label="Peso Neto (Kg)" Rules="real" />

                                <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-12 col-lg-6 mt-3 mb-5" ID="fbcPaisPartida" Label="*País origen" RequiredSelect="true" OnTextChanged="fbcPaisPartida_TextChanged" OnClick="fbcPaisPartida_Click" />

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-3 mt-3 mb-5" 
                                    ID="scMetodoValoracionPartida" Label="Método de valoración"  />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-3 mt-3 mb-5" ID="icOrdenCompraPartida" Label="Orden de compra" Rules="maxlegth[150]"  />

                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 mt-5 mb-5">
                                    <asp:Label runat="server" ID="lbClasificacion" Text="Clasificación" Visible="True" CssClass="w-100 cl_Secciones mt-5"></asp:Label>
                                </asp:Panel>

                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12">
                                  
                                    <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mb-4 mt-4" ID="icFraccionArancelaria" Label="*Fracción arancelaria" Name="icFraccionArancelaria" Enabled="false" />
                                    <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mb-4 mt-4" ID="icFraccionNico" Label="*Nico" Name="icFraccionNico" Enabled="false" />
                                    <GWC:InputControl runat="server" ID="icCantidadTarifa" CssClass="col-xs-12 col-md-4 col-lg-3 mb-4 mt-4" Type="Text" Format="Real" Name="icCantidadTarifa" Label="*Cantidad tarifa" />
                                    <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mb-4 mt-4" ID="scUnidadMedidaTarifa" SearchBarEnabled="true" LocalSearch="True" Label="*Unidad Medida Tarifa (UMT)" OnClick="scUnidadMedidaTarifa_Click" OnTextChanged="scUnidadMedidaTarifa_TextChanged" Enabled="false">
                                    </GWC:SelectControl>
                                </asp:Panel>

                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 mb-5">

                                    <GWC:InputControl runat="server" ID="icDescripcionPartidaOriginal" CssClass="solid-textarea mt-5" Type="TextArea" Format="SinDefinir" Label="Descripción de mercancía original" Enabled="False"   />

                                </asp:Panel>

                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 mb-5">

                                    <GWC:InputControl runat="server" ID="icDescripcionPartida" CssClass="solid-textarea mt-5 campo-requerido" Format="SinDefinir" Type="TextArea" Label="*Descripción de mercancía en pedimento"  />

                                </asp:Panel>

                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 mb-5">
                                    <GWC:InputControl runat="server" ID="icDescripcionCOVE" CssClass="mt-5 solid-textarea" Type="TextArea" Format="SinDefinir" Label="Descripción de acuse de valor"  />
                                </asp:Panel>

                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 mt-5 mb-5">
                                    <asp:Label runat="server" ID="lbMercancia" Text="Detalle mercancía" Visible="True" CssClass="w-100 cl_Secciones mt-5"></asp:Label>
                                </asp:Panel>

                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12">
                                    <GWC:InputControl runat="server" ID="icLote" CssClass="col-xs-12 col-md-3 mb-4 mt-4" Type="Text" Name="icLote" Label="Lote"   Rules="maxlegth[150]"/>
                                    <GWC:InputControl runat="server" ID="icNumeroSerie" CssClass="col-xs-12 col-md-3 mb-4 mt-4" Type="Text" Name="icNumeroSerie" Label="Número de serie"   Rules="maxlegth[150]"/>
                                    <GWC:InputControl runat="server" ID="icMarca" CssClass="col-xs-12 col-md-3 mb-4 mt-4" Type="Text" Name="icMarca" Label="Marca"   Rules="maxlegth[150]"/>
                                    <GWC:InputControl runat="server" ID="icModelo" CssClass="col-xs-12 col-md-3 mb-4 mt-4" Type="Text" Name="icModelo" Label="Modelo"   Rules="maxlegth[150]"/>
                                    <GWC:InputControl runat="server" ID="icSubmodelo" CssClass="col-xs-12 col-md-3 mb-4" Type="Text" Name="icSubmodelo" Label="Submodelo"   Rules="maxlegth[150]"/>
                                    <GWC:InputControl runat="server" ID="icKilometraje" CssClass="col-xs-12 col-md-3 mb-4" Type="Text" Name="icKilometraje" Label="Kilometraje" Format="Real"   Rules="maxlegth[150]"/>
                                    <GWC:InputControl runat="server" ID="coTimeStamp" CssClass="" Type="Hide"  Name="coTimeStamp" Label="TIMESTAMP"/>
                                </asp:Panel>
                            </ListControls>
                        </GWC:PillboxControl>
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fscIncrementables" Label="Incrementables" CssClass="mt-5 mb-5 pb-5">
                    <ListControls>
                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-3 mt-5">
                            <GWC:InputControl runat="server" CssClass="col-xs-6 col-md-6 col-lg-7 p-0 input-border-right" Type="Text" ID="icFletes" Label="Fletes" Format="Real"/>
                            <GWC:SelectControl runat="server" CssClass="col-xs-6 col-md-6 col-lg-5 p-0 pl-1" ID="scMonedaFletes" 
                                SearchBarEnabled="true" LocalSearch="true" Label="Moneda" OnClick="scMonedaFletes_Click" 
                                OnSelectedIndexChanged="scMonedaFletes_SelectedIndexChanged">
                            </GWC:SelectControl>
                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-3 mt-5">
                            <GWC:InputControl runat="server" CssClass="col-xs-6 col-md-6 p-0 input-border-right" Type="Text" ID="icSeguros" Label="Seguros" Format="Real"/>
                            <GWC:SelectControl runat="server" CssClass="col-xs-6 col-md-6 p-0 pl-1" ID="scMonedaSeguros" SearchBarEnabled="true" LocalSearch="true" Label="Moneda" 
                                OnClick="scMonedaSeguros_Click" OnSelectedIndexChanged="scMonedaSeguros_SelectedIndexChanged">
                            </GWC:SelectControl>
                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-3 mt-5">
                            <GWC:InputControl runat="server" CssClass="col-xs-6 col-md-6 p-0 input-border-right" Type="Text" ID="icEmbalajes" Label="Embalajes" Format="Real"/>
                            <GWC:SelectControl runat="server" CssClass="col-xs-6 col-md-6 p-0 pl-1" ID="scMonedaEmbalajes" SearchBarEnabled="true" LocalSearch="true" Label="Moneda" 
                                OnClick="scMonedaEmbalajes_Click" OnSelectedIndexChanged="scMonedaEmbalajes_SelectedIndexChanged">
                            </GWC:SelectControl>
                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-3 mt-5">
                            <GWC:InputControl runat="server" CssClass="col-xs-6 col-md-7 p-0 input-border-right" Type="Text" ID="icOtrosIncrementables" Label="Otros incrementables" Format="Real"/>
                            <GWC:SelectControl runat="server" CssClass="col-xs-6 col-md-5 p-0 pl-1" ID="scMonedaOtrosIncrementables" SearchBarEnabled="true" LocalSearch="true" Label="Moneda" 
                                OnClick="scMonedaOtrosIncrementables_Click" OnSelectedIndexChanged="scMonedaOtrosIncrementables_SelectedIndexChanged">
                            </GWC:SelectControl>
                        </asp:Panel>

<%--                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-2 mt-5">
                            <GWC:InputControl runat="server" CssClass="col-xs-6 col-md-6 p-0 input-border-right" Type="Text" ID="icDescuentos" Label="Descuentos" Format="Real"/>
                            <GWC:SelectControl runat="server" CssClass="col-xs-6 col-md-6 p-0 pl-1" ID="scMonedaDescuentos" SearchBarEnabled="true" LocalSearch="true" Label="Moneda" 
                                OnClick="scMonedaDescuentos_Click" OnSelectedIndexChanged="scMonedaDescuentos_SelectedIndexChanged">
                            </GWC:SelectControl>
                        </asp:Panel>--%>

                    </ListControls>
                </GWC:FieldsetControl>


<%--                <GWC:FieldsetControl runat="server" ID="FieldsetControl1" Label="" Detail="" CssClass="mt-5 p-0 mb-5">
                    <ListControls>


                        <asp:Panel runat="server" CssClass="row p-5 m-5" Style="border: 1px solid #cecdcd; border-radius: 14px; margin-bottom: 20px; padding-bottom: 25px!important">

                            <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 mt-5">
                                <asp:Label runat="server" ID="Label1" Text="También te podría interesar" CssClass="w-100 cl_Secciones mt-5 ml-5"></asp:Label>
                            </asp:Panel>

                            <asp:Panel runat="server" CssClass="col-xs-12 col-md-1 col-lg-1 mt-5 mb-5"></asp:Panel>


                            <asp:Panel runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mt-5 mb-5">

                                <a href="#">
                                    <asp:Label runat="server" ID="Label2" Text="Catálogo de <span class='resaltado pr-5'>clientes</span> <svg xmlns='http://www.w3.org/2000/svg' height='15' width='15' viewBox='0 0 512 512'><path fill='#909090' d='M320 0c-17.7 0-32 14.3-32 32s14.3 32 32 32l82.7 0L201.4 265.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0L448 109.3l0 82.7c0 17.7 14.3 32 32 32s32-14.3 32-32l0-160c0-17.7-14.3-32-32-32L320 0zM80 32C35.8 32 0 67.8 0 112L0 432c0 44.2 35.8 80 80 80l320 0c44.2 0 80-35.8 80-80l0-112c0-17.7-14.3-32-32-32s-32 14.3-32 32l0 112c0 8.8-7.2 16-16 16L80 448c-8.8 0-16-7.2-16-16l0-320c0-8.8 7.2-16 16-16l112 0c17.7 0 32-14.3 32-32s-14.3-32-32-32L80 32z'/></svg>" CssClass="w-100 footer-interes"></asp:Label></a>

                                <a href="https://localhost:14330/FrontEnd/Modulos/TraficoAA/Productos/Ges022-001-RegistroProductos.aspx">
                                    <asp:Label runat="server" ID="Label3" Text="Catálogo de <span class='resaltado pr-5'>productos</span> <svg xmlns='http://www.w3.org/2000/svg' height='15' width='15' viewBox='0 0 512 512'><path fill='#909090' d='M320 0c-17.7 0-32 14.3-32 32s14.3 32 32 32l82.7 0L201.4 265.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0L448 109.3l0 82.7c0 17.7 14.3 32 32 32s32-14.3 32-32l0-160c0-17.7-14.3-32-32-32L320 0zM80 32C35.8 32 0 67.8 0 112L0 432c0 44.2 35.8 80 80 80l320 0c44.2 0 80-35.8 80-80l0-112c0-17.7-14.3-32-32-32s-32 14.3-32 32l0 112c0 8.8-7.2 16-16 16L80 448c-8.8 0-16-7.2-16-16l0-320c0-8.8 7.2-16 16-16l112 0c17.7 0 32-14.3 32-32s-14.3-32-32-32L80 32z'/></svg>" CssClass="w-100 footer-interes"></asp:Label></a>

                            </asp:Panel>

                            <asp:Panel runat="server" CssClass="col-xs-12 col-md-1 col-lg-1 mt-5 mb-5" Style="border-left: 1px solid #cecdcd;">
                            </asp:Panel>

                            <asp:Panel runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mt-5 mb-5" Style="border-right: 1px solid #cecdcd;">

                                <a href="#">
                                    <asp:Label runat="server" ID="Label4" Text="Proveedores <span class='resaltado pr-5'>nacionales</span> <svg xmlns='http://www.w3.org/2000/svg' height='15' width='15' viewBox='0 0 512 512'><path fill='#909090' d='M320 0c-17.7 0-32 14.3-32 32s14.3 32 32 32l82.7 0L201.4 265.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0L448 109.3l0 82.7c0 17.7 14.3 32 32 32s32-14.3 32-32l0-160c0-17.7-14.3-32-32-32L320 0zM80 32C35.8 32 0 67.8 0 112L0 432c0 44.2 35.8 80 80 80l320 0c44.2 0 80-35.8 80-80l0-112c0-17.7-14.3-32-32-32s-32 14.3-32 32l0 112c0 8.8-7.2 16-16 16L80 448c-8.8 0-16-7.2-16-16l0-320c0-8.8 7.2-16 16-16l112 0c17.7 0 32-14.3 32-32s-14.3-32-32-32L80 32z'/></svg>" CssClass="w-100 footer-interes"></asp:Label></a>

                                <a href="#">
                                    <asp:Label runat="server" ID="Label5" Text="Proveedores <span class='resaltado pr-5'>extranjeros</span> <svg xmlns='http://www.w3.org/2000/svg' height='15' width='15'  viewBox='0 0 512 512'><path fill='#909090' d='M320 0c-17.7 0-32 14.3-32 32s14.3 32 32 32l82.7 0L201.4 265.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0L448 109.3l0 82.7c0 17.7 14.3 32 32 32s32-14.3 32-32l0-160c0-17.7-14.3-32-32-32L320 0zM80 32C35.8 32 0 67.8 0 112L0 432c0 44.2 35.8 80 80 80l320 0c44.2 0 80-35.8 80-80l0-112c0-17.7-14.3-32-32-32s-32 14.3-32 32l0 112c0 8.8-7.2 16-16 16L80 448c-8.8 0-16-7.2-16-16l0-320c0-8.8 7.2-16 16-16l112 0c17.7 0 32-14.3 32-32s-14.3-32-32-32L80 32z'/></svg>" CssClass="w-100 footer-interes"></asp:Label></a>

                            </asp:Panel>

                            <asp:Panel runat="server" CssClass="col-xs-12 col-md-1 col-lg-1 mt-5 mb-5"></asp:Panel>

                            <asp:Panel runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mt-5 mb-5">

                                <a href="#">
                                    <asp:Label runat="server" ID="Label6" Text="Acuse de <span class='resaltado pr-5'>valor</span> <svg xmlns='http://www.w3.org/2000/svg' height='15' width='15'  viewBox='0 0 512 512'><path fill='#909090' d='M320 0c-17.7 0-32 14.3-32 32s14.3 32 32 32l82.7 0L201.4 265.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0L448 109.3l0 82.7c0 17.7 14.3 32 32 32s32-14.3 32-32l0-160c0-17.7-14.3-32-32-32L320 0zM80 32C35.8 32 0 67.8 0 112L0 432c0 44.2 35.8 80 80 80l320 0c44.2 0 80-35.8 80-80l0-112c0-17.7-14.3-32-32-32s-32 14.3-32 32l0 112c0 8.8-7.2 16-16 16L80 448c-8.8 0-16-7.2-16-16l0-320c0-8.8 7.2-16 16-16l112 0c17.7 0 32-14.3 32-32s-14.3-32-32-32L80 32z'/></svg>" CssClass="w-100 footer-interes"></asp:Label></a>

                            </asp:Panel>

                        </asp:Panel>

                    </ListControls>

                </GWC:FieldsetControl>--%>

            </Fieldsets>
        </GWC:FormControl>
    </div>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
