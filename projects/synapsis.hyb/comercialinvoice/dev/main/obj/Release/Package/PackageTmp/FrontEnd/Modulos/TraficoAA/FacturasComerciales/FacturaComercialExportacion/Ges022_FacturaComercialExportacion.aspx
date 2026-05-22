<%@ Page Title="" Language="vb" Async="true" AutoEventWireup="false" MasterPageFile="~/FrontEnd/Modulos/Home.Master" CodeBehind="Ges022_FacturaComercialExportacion.aspx.vb" Inherits=".Ges022_FacturaComercialExportacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentFindbar" runat="server">

    <% If IsPopup = False Then %>

    <GWC:FindbarControl Label="Buscar factura de exportación" ID="__SYSTEM_CONTEXT_FINDER" runat="server" OnClick="BusquedaGeneral" />

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

        <GWC:FormControl runat="server" ID="__SYSTEM_MODULE_FORM" HasAutoSave="false" Label="<span style='color:#321761'>Factura comercial</span><span style='color:#782360;'>&nbsp;exportación</span>">
            
          <Buttonbar runat="server" OnClick="EventosBotonera" Onload ="MostrarBotones" ID="bbBotonera">
              <DropdownButtons>
                     <GWC:ButtonItem Text="Descargar" Enabled="False"/>
                    <GWC:ButtonItem Text="Imprimir" Enabled="False"/>
                    <GWC:ButtonItem Text="Mandar por Correo" Enabled="False"/>
                    <GWC:ButtonItem Text="Publicar" ID="btiPublicar"/>
                    <GWC:ButtonItem Text="Cargar CFDI" ID="verIntegradorCFDI"/>
                    <GWC:ButtonItem Text="Acerca de" Enabled="False"/>
              </DropdownButtons>
          </Buttonbar>


         <Fieldsets>

                <GWC:FieldsetControl runat="server" ID="fscGenerales" Label="Generales">

                    <ListControls >

                      <GWC:NotifyControl runat="server" ID="ntPublicarFacturaComercial" Important="true" Visible="false" 
                         Color="normal" Icon="info" Title="¡Atención!" Message="✨ ¿Está apunto de publicar esta factura, desea continuar?" 
                         TextButton="Cancelar" TextButtonTwo="Si, publicar" OnClick="ntPublicarFacturaComercial_Click" OnClickTwo="ntPublicarFacturaComercial_ClickTwo"/>


                        <asp:Panel runat="server" CssClass="mt-5 w-100 mb-5 p-0" ID="PanelProcesamiento" Visible="False" Enabled="true">
                          <GWC:FileControl runat="server" Label="Arrastra y suelta tu CFDI aquí para llenar la factura" CssClass="col-12 col-md-12 col-lg-12 CFDI-custom" ID="fcCFDI" Dragable="true" OnChooseFile="fcCFDI_ChooseFile" ShowButtonsTitle="false" Modality="Default" />

                           <asp:Panel runat="server" CssClass="w-100 mb-5" ID="PanelButtonIA" >
                               <asp:Panel runat="server" CssClass="row justify-content-end">
                                   <asp:Panel runat="server" CssClass="col-lg-2 col-md-1 col-6 d-flex justify-content-end" >
                                       <asp:Button runat="server" 
                                           CssClass="btn boder-outline-purple btn-sm rounded-pill custom-btn-small" 
                                           Text="Aplicar" 
                                           ID="extraerCFDI"
                                           Enabled="true"
                                           OnClick="extraerCDFI_Click"/>
                 
                                   <asp:Button runat="server" 
                                       CssClass="btn boder-outline-grey btn-sm rounded-pill custom-btn-small" 
                                       Text="Cerrar" 
                                       Enabled="true"
                                       ID="CerrarIntegrador" OnClick="CerrarIntegrador_Click"/>
                                   </asp:Panel>
                               </asp:Panel>
                           </asp:Panel>
                       </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-xs-2 col-md-6 col-lg-1 align-content-center" ID="lbModoCapturaIA" Style="padding-left: 4rem" Visible="false">
                             <asp:Label runat="server" class="cl_Num__Tarjeta_principal cl_Num__Tarjeta_IA col-md-1 align-content-center" 
                                    Text="<svg xmlns='http://www.w3.org/2000/svg' height='100' width='100' viewBox='0 0 640 640'><path fill='#ffffff' d='M176 544C96.5 544 32 479.5 32 400C32 336.6 73 282.8 129.9 263.5C128.6 255.8 128 248 128 240C128 160.5 192.5 96 272 96C327.4 96 375.5 127.3 399.6 173.1C413.8 164.8 430.4 160 448 160C501 160 544 203 544 256C544 271.7 540.2 286.6 533.5 299.7C577.5 320 608 364.4 608 416C608 486.7 550.7 544 480 544L176 544zM337 255C327.6 245.6 312.4 245.6 303.1 255L231.1 327C221.7 336.4 221.7 351.6 231.1 360.9C240.5 370.2 255.7 370.3 265 360.9L296 329.9L296 432C296 445.3 306.7 456 320 456C333.3 456 344 445.3 344 432L344 329.9L375 360.9C384.4 370.3 399.6 370.3 408.9 360.9C418.2 351.5 418.3 336.3 408.9 327L336.9 255z'/></svg>"></asp:Label>
                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-xs-2 col-md-6 col-lg-1 align-content-center" ID="lbModoCapturaIAEditar" Style="padding-left: 4rem" Visible="false">
                            <asp:Label runat="server" class="cl_Num__Tarjeta_principal cl_Num__Tarjeta_gray -+ col-md-1 align-content-center" 
                                Text="<svg xmlns='http://www.w3.org/2000/svg' height='100' width='100' viewBox='0 0 640 640'><path fill='#ffffff' d='M176 544C96.5 544 32 479.5 32 400C32 336.6 73 282.8 129.9 263.5C128.6 255.8 128 248 128 240C128 160.5 192.5 96 272 96C327.4 96 375.5 127.3 399.6 173.1C413.8 164.8 430.4 160 448 160C501 160 544 203 544 256C544 271.7 540.2 286.6 533.5 299.7C577.5 320 608 364.4 608 416C608 486.7 550.7 544 480 544L176 544zM337 255C327.6 245.6 312.4 245.6 303.1 255L231.1 327C221.7 336.4 221.7 351.6 231.1 360.9C240.5 370.2 255.7 370.3 265 360.9L296 329.9L296 432C296 445.3 306.7 456 320 456C333.3 456 344 445.3 344 432L344 329.9L375 360.9C384.4 370.3 399.6 370.3 408.9 360.9C418.2 351.5 418.3 336.3 408.9 327L336.9 255z'/></svg>"></asp:Label>
                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-xs-2 col-md-6 col-lg-1 align-content-center" ID="lbModoCapturaManual" Style="padding-left: 4rem" Visible="false">
                            <asp:Label runat="server" class="cl_Num__Tarjeta_principal cl_Num__Tarjeta_blue  col-md-1 align-content-center" 
                                Text="<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 512 512'><path fill='#ffffff' d='M224 0c17.7 0 32 14.3 32 32V240H192V32c0-17.7 14.3-32 32-32zm96 160c17.7 0 32 14.3 32 32v64c0 17.7-14.3 32-32 32s-32-14.3-32-32V192c0-17.7 14.3-32 32-32zm64 64c0-17.7 14.3-32 32-32s32 14.3 32 32v64c0 17.7-14.3 32-32 32s-32-14.3-32-32V224zM93.3 51.2L175.9 240H106.1L34.7 76.8C27.6 60.6 35 41.8 51.2 34.7s35.1 .3 42.1 16.5zm27 221.3l-.2-.5h69.9H216c22.1 0 40 17.9 40 40s-17.9 40-40 40H160c-8.8 0-16 7.2-16 16s7.2 16 16 16h56c39.8 0 72-32.2 72-72l0-.6c9.4 5.4 20.3 8.6 32 8.6c13.2 0 25.4-4 35.6-10.8c8.7 24.9 32.5 42.8 60.4 42.8c11.7 0 22.6-3.1 32-8.6V352c0 88.4-71.6 160-160 160H226.3c-42.4 0-83.1-16.9-113.1-46.9l-11.6-11.6C77.5 429.5 64 396.9 64 363V336c0-32.7 24.6-59.7 56.3-63.5z'/></svg>"></asp:Label>
                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-xs-2 col-md-6 col-lg-1 align-content-center" ID="lbModoCapturaManualNuevo" Style="padding-left: 4rem" Visible="true">
                            <asp:Label runat="server" class="cl_Num__Tarjeta_principal cl_Num__Tarjeta_gray  col-md-1 align-content-center" 
                                Text="<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 512 512'><path fill='#ffffff' d='M224 0c17.7 0 32 14.3 32 32V240H192V32c0-17.7 14.3-32 32-32zm96 160c17.7 0 32 14.3 32 32v64c0 17.7-14.3 32-32 32s-32-14.3-32-32V192c0-17.7 14.3-32 32-32zm64 64c0-17.7 14.3-32 32-32s32 14.3 32 32v64c0 17.7-14.3 32-32 32s-32-14.3-32-32V224zM93.3 51.2L175.9 240H106.1L34.7 76.8C27.6 60.6 35 41.8 51.2 34.7s35.1 .3 42.1 16.5zm27 221.3l-.2-.5h69.9H216c22.1 0 40 17.9 40 40s-17.9 40-40 40H160c-8.8 0-16 7.2-16 16s7.2 16 16 16h56c39.8 0 72-32.2 72-72l0-.6c9.4 5.4 20.3 8.6 32 8.6c13.2 0 25.4-4 35.6-10.8c8.7 24.9 32.5 42.8 60.4 42.8c11.7 0 22.6-3.1 32-8.6V352c0 88.4-71.6 160-160 160H226.3c-42.4 0-83.1-16.9-113.1-46.9l-11.6-11.6C77.5 429.5 64 396.9 64 363V336c0-32.7 24.6-59.7 56.3-63.5z'/></svg>"></asp:Label>
                        </asp:Panel>

                        <GWC:InputControl runat="server" ID="icTipoCargaDatos" Type="Text" Name="icTipoCargaDatos" Label="Tipo Carga Datos" Visible="false" />

                        <GWC:DualityBarControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-5 lowercase" ID="dbcNumFacturaAcuseValor" Label="*Número de factura | Folio fiscal (UUID)" 
                            LabelDetail="Acuse de valor" 
                            Rules="maxlegth[150]" VisibleButton="false"   OnTextChanged="dbcNumFacturaAcuseValor_TextChanged" ExternalAutoPostBack="False"/>
                        
                   <%--     <GWC:InputControl runat="server" Type="Hide" id="icidcove" />--%>
                            
                       <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-6 mt-5 p-0">

                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-6 mt-5  " ID="icFechaFactura" 
                                Type="Text" Format="Calendar" Label="*Fecha factura" ExternalAutoPostBack="False"/>

                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-6 mt-5 icFechaAcuseValor" 
                                ID="icFechaAcuseValor" Type="Text"  
                               Label="Fecha acuse valor" 
                                Enabled="false"  />

                        </asp:Panel>

                          <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 mt-5">

                            <%--  CLIENTE--%>
                            <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-6" ID="fbcCliente" Label="*Cliente"  OnTextChanged="fbcCliente_TextChanged" OnClick="fbcCliente_Click"
                                OnClickClose="fbcCliente_ClickClose" Search="true" OnClickSearch="fbcCliente_ClickSearch" />
                            
                            <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-6" ID="fbcIncoterm" Label="*Incoterm" Rules="" RequiredSelect="true" 
                                KeyField="i_Cve_TerminoFacturacion" DisplayField="t_ValorPresentacion" Dimension="Vt022TerminosFacturacionA14" Search="true" OnClickSearch="fbcIncoterm_ClickSearch"/>

                            </asp:Panel>
                           <%-- <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-6 mt-5  " ID="fbcPais" Label="*País moneda"  OnTextChanged="fbcPais_TextChanged"
                                OnClickClose="fbcPais_ClickClose" Search="true" OnClickSearch="fbcPais_ClickSearch"/>--%>

                            <asp:Panel runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mt-5 pt-5">
                               <GWC:InputControl runat="server" CssClass="col-xs-8 col-md-8 p-0 input-border-right" ID="icValorFactura" Type="Text"  Label="*Valor factura" Format="Real" Rules=""/>
                               <GWC:SelectControl runat="server" CssClass="col-xs-4 col-md-4 p-0 pl-1" ID="scMonedaFactura" SearchBarEnabled="true" LocalSearch="true" Label="Moneda" OnClick="scMonedaFactura_Click" 
                                   OnSelectedIndexChanged="scMonedaFactura_SelectedIndexChanged">
                                </GWC:SelectControl>
                             </asp:Panel>

                            <asp:Panel runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mt-5 pt-5">
                                <GWC:InputControl runat="server" CssClass="col-xs-8 col-md-8 p-0 input-border-right" ID="icValorMercancia" Type="Text" Label="*Valor mercancía" Format="Real"/>
                                <GWC:SelectControl runat="server" CssClass="col-xs-4 col-md-4 p-0 pl-1" ID="scMonedaMercancia" SearchBarEnabled="true" LocalSearch="true" Label="Moneda" 
                                    OnClick="scMonedaMercancia_Click" OnSelectedIndexChanged="scMonedaMercancia_SelectedIndexChanged"></GWC:SelectControl>
                         </asp:Panel>

                          <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mt-5 pt-5" ID="icPesoTotal" Type="Text"  Label="Peso total (Kg)" Format="Real"/>
                         <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mt-5 pt-5" ID="icBultos" Type="Text" Label="Bultos" Format="Real"/>
                         <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mt-5 mb-5 lowercase" ID="icOrdenCompra" Type="Text"  
                             Label="Orden de compra" Rules="maxlegth[150]"  />
                         <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mt-5 mb-5 lowercase" ID="icReferenciaCliente" Type="Text"  
                             Label="Referencia del cliente" Rules="maxlegth[150]"   />
                        <GWC:InputControl runat="server" CssClass="col-xs-9 col-md-5 col-lg-5 mt-5" ID="icFolioFactura" Type="Text" Label="Serie | Folio de factura"  />
                         <GWC:SwitchControl runat="server" CssClass="col-xs-3 col-md-1 col-lg-1" ID="swcEnajenacion" Label="Enajenación" OnText="Sí" OffText="No" LabelVisible="True"/>
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fscCompradorReceptor" CssClass="mt-5 mb-5" Label="Comprador" Detail="Comprador">
                   <ListControls>
                         <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-6  col-lg-6 mt-5  " ID="fbcCompradorReceptor" Label="*Razón social comprador" OnClick="fbcCompradorReceptor_Click" OnTextChanged="fbcCompradorReceptor_TextChanged" 
                             OnSelectedIndexChanged="fbcCompradorReceptor_SelectedIndexChanged" Rules="" OnClickClose="fbcCompradorReceptor_ClickClose" Search="true" OnClickSearch="fbcCompradorReceptor_ClickSearch" />
                         <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-6 mt-5  " ID="scDomicilioCompradorReceptor" Label="*Domicilio" Enabled="true"></GWC:SelectControl>
                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-6 mt-5" ID="scVinculacion" Label="Vinculación" OnClick="scVinculacion_Click"></GWC:SelectControl>
                        <GWC:SelectControl runat="server" CssClass="col-xs-8 col-md-6 col-lg-6 mt-5" ID="scMetodoValoracion" Label="Método de valoración" 
                            Onclick="scMetodoValoracion_Click" KeyField="i_Cve_MetodoValoracion" DisplayField="t_ClaveDescripcion" Dimension="Vt022MetodosValoracionA11" OnSelectedIndexChanged="scMetodoValoracion_SelectedIndexChanged" ></GWC:SelectControl>
                       <%-- <GWC:SwitchControl runat="server" CssClass="col-xs-4 col-md-1 col-lg-1 d-flex justify-content-center" ID="swcEsDestinatario" Label="Es destinatario" OnText="Sí" OffText="No" OnCheckedChanged="swcEsDestinatario_CheckedChanged" LabelVisible="True"/>--%>
                   </ListControls>
                </GWC:FieldsetControl>

<%--                <GWC:FieldsetControl runat="server" ID="fscCompradorDestinatario" Label="Destinatario" CssClass="mt-5 mb-5" Detail="Destinatario" Visible="false">
                         <ListControls>
                                 <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-6 mt-5 mb-5" ID="fbcCompradorDestinatario" Label="Razón social destinatario" 
                                     OnClick="fbcCompradorDestinatario_Click" OnTextChanged="fbcCompradorDestinatario_TextChanged"  OnClickClose="fbcCompradorDestinatario_ClickClose" />
                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mt-5 mb-5" ID="scDomicilioCompradorDestinatario"  Label="Domicilio" OnClick="scDomicilioCompradorDestinatario_Click" OnTextChanged="scDomicilioCompradorDestinatario_TextChanged" 
                                    OnSelectedIndexChanged="scDomicilioCompradorDestinatario_SelectedIndexChanged" Enabled="false"></GWC:SelectControl>
                       </ListControls>
                </GWC:FieldsetControl>--%>

                 <%--ITEMS--%>
                <GWC:FieldsetControl runat="server" ID="fscItems" Label="Items" CssClass="mt-5">
                    <ListControls>
                        <GWC:PillboxControl runat="server" ID="pbPartidasItems"  KeyField="indice" CssClass="col-lg-12 col-md-12 col-xs-12 mt-5" 
                            OnClick="pbPartidasItems_Click" OnCheckedChange="pbPartidasItems_CheckedChange" AutoNavigate="false">
                            <ListControls> 
                                   <asp:Panel runat="server" CssClass="col-md-1 col-lg-1 col-xs-2 d-flex align-items-center flex-column margin-bottom">
                                        <asp:Label runat="server" ID="lbNumero" class="cl_Num__Tarjeta" Text="0"></asp:Label>
                                    </asp:Panel>
                                 <GWC:InputControl runat="server" ID="icObjectIdProducto" Label="ObjectId Partida" Visible="False"/>
                                <GWC:FindboxControl runat="server" CssClass="col-xs-10 col-md-5 mt-5  " ID="fbcProducto" Label="*Número de parte | Descripción | Alias"  OnClick="fbcProducto_Click" 
                                    OnTextChanged="fbcProducto_TextChanged" HasDetails="true" RequiredSelect="true"   OnClickClose="fbcProducto_ClickClose" Search="true" OnClickSearch="fbcProducto_ClickSearch"/>
                                <GWC:InputControl runat="server" ID="icCantidadComercial" CssClass="col-xs-12 col-md-3 col-lg-3 mt-5" Type="Text"  Label="*Cantidad comercial" Format="Real" OnTextChanged="icCantidadComercial_TextChanged"/>
                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mt-5" ID="scUnidadMedidaComercial" SearchBarEnabled="true" LocalSearch="false" Label="*Unidad Medida Comercial (UMC)" 
                                    OnClick="scUnidadMedidaComercial_Click" OnTextChanged="scUnidadMedidaComercial_TextChanged" OnSelectedIndexChanged="scUnidadMedidaComercial_SelectedIndexChanged"  />

                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 col-lg-3 mb-3 mt-3">
                                    <GWC:InputControl runat="server" ID="icValorMercanciaItem" CssClass="col-xs-8 col-md-8 col-lg-8 p-0 input-border-right" Type="Text"  Label="*Valor mercancia item" Format="Real"/>
                                    <GWC:SelectControl runat="server" CssClass="col-xs-4 col-md-4 col-lg-4 p-0 pl-1" ID="scMonedaMercanciaItemPartida" SearchBarEnabled="true" 
                                        LocalSearch="true" Label="Moneda" OnClick="scMonedaMercanciaItemPartida_Click" OnSelectedIndexChanged="scMonedaMercanciaItemPartida_SelectedIndexChanged"> </GWC:SelectControl>
                                </asp:Panel>

                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 col-lg-3 mb-3 mt-3">
                                    <GWC:InputControl runat="server" ID="icPrecioUnitario" CssClass="col-xs-8 col-md-8 col-lg-8 p-0 input-border-right" Type="Text" Label="*Precio unitario" Format="Real"/>
                                    <GWC:SelectControl runat="server" CssClass="col-xs-4 col-md-4 col-lg-4 p-0 pl-1" ID="scMonedaPrecioUnitarioPartida" SearchBarEnabled="true" LocalSearch="true" 
                                        Label="Moneda" OnClick="scMonedaPrecioUnitarioPartida_Click" OnSelectedIndexChanged="scMonedaPrecioUnitarioPartida_SelectedIndexChanged"/>
                                </asp:Panel>

                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 col-lg-3 mb-3 mt-3">
                                   <GWC:InputControl runat="server" ID="icValorAgregadoPartida"  CssClass="col-xs-8 col-md-8 col-lg-8 p-0 input-border-right" Type="Text"  Label="Valor agregado" Format="Real"/>
                                   <GWC:SelectControl runat="server" CssClass="col-xs-4 col-md-4 col-lg-4 p-0 pl-1" ID="scMonedaValorAgregadoPartida" 
                                       SearchBarEnabled="true" LocalSearch="true" Label="Moneda" OnClick="scMonedaValorAgregadoPartida_Click" OnSelectedIndexChanged="scMonedaValorAgregadoPartida_SelectedIndexChanged"></GWC:SelectControl>
                                </asp:Panel>

                                <GWC:InputControl runat="server" ID="icPesoNeto" CssClass="col-xs-12 col-md-6 col-lg-3 mb-3 mt-3" Type="Text" Label="Peso Neto (Kg)" Format="Real"/>

                                <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-12 col-lg-6 mt-3 mb-5" ID="fbcPaisPartida" Label="*País destino"  RequiredSelect="true" OnTextChanged="fbcPaisPartida_TextChanged" Search="true" OnClickSearch="fbcPaisPartida_ClickSearch"/>

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-3 mt-3 mb-5" ID="scMetodoValoracionPartida" Label="Método de valoración" KeyField="i_Cve_MetodoValoracion" DisplayField="t_ClaveDescripcion" Dimension="Vt022MetodosValoracionA11" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-3 mt-3 mb-5" ID="icOrdenCompraPartida" 
                                    Label="Orden de compra" Rules="maxlegth[150]"  />

                                 <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 mt-2">
                                     <asp:Label runat="server" ID="lbClasificacion" Text="Clasificación" Visible="True" CssClass="w-100 cl_Secciones mt-5"></asp:Label>
                                 </asp:Panel>

                                 <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12">
                                    
                                     <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mb-4 mt-4" ID="icFraccionArancelaria" Label="*Fracción arancelaria" Enabled="false" />
                                     <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mb-4 mt-4" ID="icFraccionNico"  Label="*Nico" Enabled="false"/>
                                     <GWC:InputControl runat="server" ID="icCantidadTarifa" CssClass="col-xs-12 col-md-3 col-lg-3 mb-4 mt-4" Type="Text"  Label="*Cantidad tarifa" Format="Real"/>
                                     <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mb-4 mt-4" ID="scUnidadMedidaTarifa"  SearchBarEnabled="true" LocalSearch="True" Label="*Unidad Medida Tarifa (UMT)" OnClick="scUnidadMedidaTarifa_Click" OnTextChanged="scUnidadMedidaTarifa_TextChanged" Enabled="false"/>
                                 </asp:Panel>
                                
                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 mb-5">

                                    <GWC:InputControl runat="server" ID="icDescripcionPartidaOriginal" CssClass="solid-textarea mt-5 lowercase" 
                                        Type="TextArea" Format="SinDefinir" Label="*Descripción de mercancía original" Enabled="False"  />

                                </asp:Panel>

                                  <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 mb-5">
                                      <GWC:InputControl runat="server" ID="icDescripcionPartida" CssClass="solid-textarea mt-5 lowercase" 
                                          Type="TextArea" Format="SinDefinir" Label="*Descripción de mercancía en pedimento"  />
                                  </asp:Panel>

                                  <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 mb-5">
                                      <GWC:InputControl runat="server" ID="icDescripcionCOVE" CssClass="mt-5 solid-textarea" Format="SinDefinir" Type="TextArea" 
                                          Label="Descripción de acuse de valor"  />
                                  </asp:Panel>

                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 mt-5 mb-5">
                                    <asp:Label runat="server" ID="lbMercancia" Text="Detalle mercancía" Visible="True" CssClass="w-100 cl_Secciones mt-5"></asp:Label>
                                </asp:Panel>

                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12">
                                    <GWC:InputControl runat="server" ID="icLote" CssClass="col-xs-12 col-md-3 col-lg-3 mt-4" Type="Text"  Label="Lote" Rules="maxlegth[150]" 
                                        />
                                    <GWC:InputControl runat="server" ID="icNumeroSerie" CssClass="col-xs-12 col-md-3 col-lg-3 mt-4 lowercase" Type="Text" 
                                        Label="Número de serie" Rules="maxlegth[150]"   />
                                    <GWC:InputControl runat="server" ID="icMarca" CssClass="col-xs-12 col-md-3 col-lg-3 mt-4 lowercase" Type="Text" 
                                        Label="Marca" Rules="maxlegth[150]"  />
                                    <GWC:InputControl runat="server" ID="icModelo" CssClass="col-xs-12 col-md-3 col-lg-3 mt-4 lowercase" Type="Text" 
                                        Label="Modelo" Rules="maxlegth[150]"    />
                                    <GWC:InputControl runat="server" ID="icSubmodelo" CssClass="col-xs-12 col-md-3 col-lg-3 mt-3 lowercase" Type="Text" 
                                        Label="Submodelo" Rules="maxlegth[150]"  />
                                    <GWC:InputControl runat="server" ID="icKilometraje" CssClass="col-xs-12 col-md-3 col-lg-3 mt-3 lowercase" Type="Text" 
                                        Label="Kilometraje" Format="Real" Rules="maxlegth[150]"  />
                                    <GWC:InputControl runat="server" ID="coTimeStamp" CssClass="" Type="Hide"  Name="coTimeStamp" Label="TIMESTAMP" />
                                </asp:Panel>

                            </ListControls>
                        </GWC:PillboxControl>
                    </ListControls>
                </GWC:FieldsetControl>


<%--                    <GWC:FieldsetControl runat="server" ID="FieldsetControl1" Label="" Detail="" CssClass="mt-5 p-0 mb-5">
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
                                         <asp:Label runat="server" ID="Label7" Text="Destinatarios <span class='resaltado pr-5'></span> <svg xmlns='http://www.w3.org/2000/svg' height='15' width='15'  viewBox='0 0 512 512'><path fill='#909090' d='M320 0c-17.7 0-32 14.3-32 32s14.3 32 32 32l82.7 0L201.4 265.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0L448 109.3l0 82.7c0 17.7 14.3 32 32 32s32-14.3 32-32l0-160c0-17.7-14.3-32-32-32L320 0zM80 32C35.8 32 0 67.8 0 112L0 432c0 44.2 35.8 80 80 80l320 0c44.2 0 80-35.8 80-80l0-112c0-17.7-14.3-32-32-32s-32 14.3-32 32l0 112c0 8.8-7.2 16-16 16L80 448c-8.8 0-16-7.2-16-16l0-320c0-8.8 7.2-16 16-16l112 0c17.7 0 32-14.3 32-32s-14.3-32-32-32L80 32z'/></svg>" CssClass="w-100 footer-interes"></asp:Label></a>

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