<%@ Page Title="" Language="vb" Async="true" AutoEventWireup="false" MasterPageFile="~/FrontEnd/Modulos/Home.Master" CodeBehind="Ges022-001-AcuseValor.aspx.vb" Inherits=".Ges022_001_AcuseValor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentFindbar" runat="server">
<% If IsPopup = False Then %>
    <GWC:FindbarControl Label="Buscar Acuse de Valor" ID="__SYSTEM_CONTEXT_FINDER" runat="server" OnClick="BusquedaGeneral"/>
<% End If %>

    <style>

        .cl_Secciones {
            opacity: .6;
            color: #757575;      
            font-size: 24px;
            font-weight: bold;
        }
        
        .cl_Tarjeta {      
            font-size: 24px;
            font-weight: bold;   
            color: #432776;               
            display: flex;        
	        justify-content: center;
            align-items: center;   
            
        }

        .cl_Num__Tarjeta {
            background-color: #432776;            
            color: #fff;
            display: flex;        
            border-radius: 50%;           
	        justify-content: center;            
            align-items: center;
            width: 60px;
            height: 60px;
        }

        .cl_Num__Tarjeta {
            font-size: 2.4em;
            font-weight: bold;
        }

        .sc__Subdivision {
            display: block !important;
            border:1px solid red !important;
            
            
        }

        .customsizetextarea {
           height: 2.4em;
        }

     
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentCompanyList" runat="server">
<% If IsPopup = False Then %>
    <GWC:SelectControl CssClass="col-auto company-list-select" runat="server" SearchBarEnabled="false" ID="__SYSTEM_ENVIRONMENT" OnSelectedIndexChanged="CambiarEmpresa"/>
<% End If %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentBody" runat="server">
   <input type="hidden" id="hdnDialogResponse" name="hdnDialogResponse" value="" />
    <iframe id="frameDescarga" style="display:none;"></iframe>
    <div class="d-flex">
        <GWC:FormControl runat="server" ID="__SYSTEM_MODULE_FORM" HasAutoSave="false" Label="Acuse de Valor" OnCheckedChanged="MarcarPagina">
            <Buttonbar runat="server" OnClick="EventosBotonera">
                <DropdownButtons>
                    <GWC:ButtonItem Text="Adendar"/>
                    <GWC:ButtonItem Text="Limpiar"/>
                    <GWC:ButtonItem Text="Descargar Acuse PDF"/>
                    <GWC:ButtonItem Text="Descargar XML"/>
                    <GWC:ButtonItem Text="Descargar Representación Impresa PDF"/>
                    <GWC:ButtonItem Text="Prueba EDOCUMENT VUCEM"/>
                    <GWC:ButtonItem Text="Acerca de.." ID="btnOtro" Enabled="false"/>
                </DropdownButtons>
            </Buttonbar>   
            <Fieldsets>
               <GWC:FieldsetControl runat="server" ID="fscGenerales" Label="Generales">
                    <ListControls>

                           
<%--<GWC:SelectControl runat="server" ID="scAsignadoA" CssClass="col-xs-12 col-md-6 mb-5" KeyField="i_Cve_EjecutivosMisEmpresas" DisplayField="t_NombreCompleto" Dimension="EjecutivosMiEmpresa" Label="Asignado a" Enabled="false"></GWC:SelectControl>--%>
                        <GWC:SelectControl runat="server" CssClass="col-xs-6 col-md-6 col-lg-8 mt-5" ID="scNumeroFactura" Label="Folios de Factura" Enabled="false" SearchBarEnabled="false" LocalSearch="false" Visible="False" OnSelectedIndexChanged="CargarFacturaSeleccionada"/>

                        <GWC:DualityBarControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-6" ID="dbcNumFacturaAcuseValor" Label="Número de Factura/Folio del Documento" LabelDetail="Acuse de Valor" 
                            OnClick="dbc_NumFacturaAcuseValor_Click" EnabledButton="true" ExternalAutoPostBack="False"/>
                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 col-lg-6">
                            <GWC:SwitchControl runat="server" ID="swcTipoOperacion" CssClass="col-xs-6 col-md-6 col-lg-4" Label="Tipo de operación" OnText="Importación" OffText="Exportación" Checked="true" OnCheckedChanged="EnroqueProveedorCliente"   LabelVisible="true"/>
                                                        <GWC:SelectControl runat="server" CssClass="col-xs-6 col-md-6 col-lg-8 mt-5" ID="scTipoDocumento" Label="Tipo de documento" Enabled="false" SearchBarEnabled="false" LocalSearch="false" >
                                  <Options >
                                         <GWC:SelectOption Value="1" Text="Factura"/>
                                         <GWC:SelectOption Value="3" Text="Carta Factura"/>
                                  </Options>
                            </GWC:SelectControl>
                        </asp:Panel>
                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 mt-5 mb-5">
                            <GWC:SelectControl runat="server" ID="scTipoMoneda" CssClass="col-xs-12 col-md-3 col-lg-3 mt-5"  Label="Moneda"  SearchBarEnabled="true" LocalSearch="false" Rules="required"  OnTextChanged="sc_TipoMoneda_TextChanged" OnClick="sc_TipoMoneda_Click" OnSelectedIndexChanged="sc_TipoMoneda_SelectedIndesxChanged">

                            </GWC:SelectControl> 
                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mt-5" ID="icFechaExpedicion" Rules="require" Type="Text" Format="Calendar" Name="icFechaExpedicion" Label="Expedición Documento"  />
                            <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 col-lg-6" ID="pSubdivisionCertificado">
                                <GWC:SwitchControl runat="server" ID="swcSubdivision" CssClass="col-xs-6 col-md-6 col-lg-3  d-flex justify-content-end" Label="Subdivisión" OnText="Sí" OffText="No" Checked="false" LabelVisible="true" Enabled="false"/>
                                <GWC:SwitchControl runat="server" ID="swcRelacionFactura" CssClass="col-xs-6 col-md-6 col-lg-3" Label="Relación de Facturas" OnText="Sí" OffText="No" Checked="false"  LabelVisible="true"  Visible="false"/>
                                <GWC:SwitchControl runat="server" ID="swcCertificadoOrigen" CssClass="col-xs-6 col-md-6 col-lg-5  d-flex justify-content-end" Label="Certificado Origen" OnText="Sí" OffText="No" Checked="false" OnCheckedChanged="swc_CertificadoOrigen_CheckedChanged" LabelVisible="true" Enabled="false"/>
                            </asp:Panel>
                        </asp:Panel>
                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12">
                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-6 mt-5 mb-5 solid-textarea" ID="icObservaciones" Type="TextArea" Format="SinDefinir" Name="icObservaciones" UpperCase="true" Label="Observaciones" />
                          
                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-6 mt-5 mt-5 mb-5" ID="icExpotadorAutorizado" Type="Text" Format="SinDefinir" Name="icExpotadorAutorizado" Label="Exportador Autorizado" Visible="false" />
                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 mt-4">
                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-6 mt-5 mb-5 p-0 d-flex justify-content-end" ID="icedocument" Type="Text" Format="SinDefinir" Name="icedocument" Label="Adenda" Visible="false" />
                        </asp:Panel>

                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" Label="Emisor" ID="fsProveedor">
                    <ListControls>
                         <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 px-0 mt-2 py-5">
                         
                                <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-6 mt-2 mb-5 mt-5" Priority="false" Label="Nombre/Razón Social" ID="fbcProveedor" RequiredSelect="true" OnClick="fbc_Proveedor_Click" OnTextChanged="fbc_Proveedor_TextChanged" OnClickClose="fbc_Proveedor_ClickClose"/>

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mt-2  col-lg-6 mb-5 mt-5 solid-textarea" ID="icDireccionProveedor" Type="TextArea" Enabled="False" Format="SinDefinir" Name="icDireccionProveedor" Label="Dirección Fiscal"  />    
                             
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mt-2 col-lg-6 mb-5 mt-5" ID="icIDFiscalProveedor" Type="Text" Format="SinDefinir" Enabled="False" Name="ic_IDFiscalProveedor" Label="IDFiscal/TaxNumber/RFC/CURP" />

                        </asp:Panel>

                       
                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" Label="Destinatario" ID="fs_Destinatario">
                    <ListControls>
                         <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 px-0 mt-2 py-5">
                         
                                <GWC:FindboxControl runat="server" ID="fbcDestinatario" CssClass="col-xs-12 col-md-6 col-lg-6 mt-5 mb-5" Priority="false" Label="Nombre/Razón Social" OnClick="fbc_Proveedor_Click" OnTextChanged="fbc_Destinatario_TextChanged" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-6 mb-5 mt-5 solid-textarea" ID="icDireccionDestinatario" Type="TextArea" Format="SinDefinir" Name="ic_DireccionDestinatario" Enabled="False" Label="Dirección Fiscal" />
                             
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-6 mt-5 mb-5" ID="icIDFiscalDestinatario" Type="Text" Format="SinDefinir" Name="ic_IDFiscalDestinatario" Enabled="False" Label="IDFiscal/TaxNumber/RFC/CURP" />
                            
                        </asp:Panel>
                    </ListControls>
                </GWC:FieldsetControl>
                <GWC:FieldsetControl  runat="server" Label="Partidas - Acuse de Valor" ID="fspartidas">
                      <ListControls>
                        <GWC:PillboxControl runat="server"  ID="pbPartidasAcuseValor" KeyField="indice" CssClass="col-xs-12" OnCheckedChange="pb_PartidasAcuseValor_CheckedChange" OnClick="pb_PartidasAcuseValor_Click" >
                           <ListControls>
                               <asp:Panel runat="server" CssClass="col-md-1 d-flex align-items-center flex-column margin-bottom">
                                        <asp:Label runat="server" ID="lbTarjeta" Text="No." class="cl_Tarjeta col-xs-12 col-md-1"></asp:Label>
                                        <asp:Label runat="server" ID="lbNumeroAcuseValor" class="cl_Num__Tarjeta col-xs-12 col-md-1" Text="0"></asp:Label>
                               </asp:Panel>                           
                               <asp:Panel runat="server" CssClass="d-flex align-items-center">
                                        <asp:Label runat="server" ID="lbFactura" Text="Partida - Factura" Visible="True" CssClass="w-100 cl_Secciones"></asp:Label>
                               </asp:Panel>
                               <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 p-0">
                                         <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 mb-5">
                                               <div class="col-xs-12 col-md-6 mt-3 p-0">
                                                    <GWC:InputControl runat="server" ID="icDescripcionAcuseValor" CssClass="col-xs-12 col-md-12 mt-3 mb-5 solid-textarea" Type="TextArea" UpperCase="true" Name="icDescripcionAcuseValor" Label="Descripción" />
                                               </div>
                                               <div class="col-xs-12 col-md-6 mt-3 p-0">
                                                    <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 mb-5">

                                                          <GWC:InputControl runat="server" ID="icCantidadAcuseValor" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Format="Real" Name="icCantidadAcuseValor" Label="Cantidad" onTextChanged="CalcularTotal" ExternalAutoPostBack="true"/>

                                                          <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="scUnidadAcuseValor" Name="scUnidadAcuseValor" SearchBarEnabled="true" LocalSearch="false" Label="Unidad" OnTextChanged="sc_UnidadAcuseValor_TextChanged" OnClick="sc_UnidadAcuseValor_Click">

                                                          </GWC:SelectControl> 
                                                    </asp:Panel>
                                                    <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 mb-1">

                                                            <GWC:InputControl runat="server" ID="icPrecioUnitarioAcuseValor" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Name="icPrecioUnitarioAcuseValor" Format="MoneyDecimal" Label="Precio unitario" onTextChanged="CalcularTotal" ExternalAutoPostBack="true" />

                                                            <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5 " ID="scMonedaPrecioUnitarioPartida"  Name="scMonedaPrecioUnitarioPartida" SearchBarEnabled="true" LocalSearch="false" Label="Tipo de Moneda" OnTextChanged="sc_TipoMoneda_TextChanged" OnSelectedIndexChanged="CalcularTotal">

                                                            </GWC:SelectControl>                                             
                                                    </asp:Panel>  
                                                    <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 mb-1">
                                                              <GWC:InputControl runat="server" ID="icValorFacturaPartida" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Name="icValorFacturaPartida" Label="Total" Enabled="false" />
                                                              <GWC:InputControl runat="server" ID="icValorDolaresPartida" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Name="icValorDolaresPartida" Label="Total Dólares" />
                                                    </asp:Panel>
                                               </div>
                                         </asp:Panel>
                               </asp:Panel>                                               
                               <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 p-0">
                                    <asp:Panel runat="server" class="col-xs-12 p-0">
                                        <asp:Label runat="server" ID="lbMercancia" Text="Partida - Detalle mercancía" Visible="True" CssClass="w-100 cl_Secciones"></asp:Label>
                                    </asp:Panel>     
                                    <div class="col-xs-12 mt-3 p-0">
                                        <GWC:InputControl runat="server" ID="icMarcaAcuseValor" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" Name="icMarcaAcuseValor" Label="Marca" />
                                        <GWC:InputControl runat="server" ID="icModeloAcuseValor" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" Name="icModeloAcuseValor" Label="Modelo"/>
                                        <GWC:InputControl runat="server" ID="icSubmodeloAcuseValor" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" Name="icSubmodeloAcuseValor" Label="Submodelo" />
                                        <GWC:InputControl runat="server" ID="icNumeroSerieAcuseValor" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" Name="icNumeroSerieAcuseValor" Label="Número de serie" />
                                    </div> 
                                </asp:Panel>
                           </ListControls>
                         </GWC:PillboxControl>
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fs_Configuracion" Label="Configuración">
                    <ListControls>

                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 p-0">

                                    <div class="row">
 
                                    <div class="col-xs-12 col-md-12 col-lg-12 mt-3 p-0">
                                  
                                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5 " ID="scSelloCliente" SearchBarEnabled="true" LocalSearch="false" Label="Sello" />
                                      
                                        <GWC:InputControl runat="server" ID="icPatenteAduanal" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Name="icPatenteAduanal" Label="Patente" />
                                    
                                        <GWC:CatalogControl CssClass="w-100" ID="ccDatosConsulta" runat="server" Collapsed="true" KeyField="indice">
                                          
                                            <Columns>
                                        
                                              <GWC:InputControl runat="server" ID="icRFCConsulta" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Name="icRFCConsulta" Label="Rfc de Consulta" />
                                        
                                              <GWC:InputControl runat="server" ID="icRazonSocialConsulta" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Name="icRazonSocialConsulta" Label="Razón Social" />

                                             <GWC:InputControl runat="server" ID="icEmailConsulta" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Name="icEmailConsulta" Label="Email de Consulta" />
                                        
                                           </Columns>

                                       </GWC:CatalogControl>

                                    </div> 

                                 </div>

                                </asp:Panel>
                    </ListControls>
                </GWC:FieldsetControl>
            </Fieldsets>
        </GWC:FormControl>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
