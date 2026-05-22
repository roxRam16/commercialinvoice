<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/FrontEnd/Modulos/Home.Master" CodeBehind="Ges022_ProcesamientoElectronicoDocumentos.aspx.vb" Inherits=".Ges022_ProcesamientoElectronicoDocumentos"  Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentFindbar" runat="server">

    <% If IsPopup = False Then %>

      <GWC:FindbarControl Label="Buscar por folio de carga" ID="__SYSTEM_CONTEXT_FINDER" runat="server" OnClick="BusquedaGeneral" />

    <% End If %>

   <link rel="stylesheet" type="text/css" href="Estilos.css" />

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentCompanyList" runat="server">
    <% If IsPopup = False Then %>

        <GWC:SelectControl CssClass="col-auto company-list-select" runat="server" SearchBarEnabled="false" ID="__SYSTEM_ENVIRONMENT" OnSelectedIndexChanged="CambiarEmpresa"/>

    <% End If %>
</asp:Content>

<asp:Content runat="server" ID="Content4" ContentPlaceHolderID="contentBody">
        <GWC:FormControl runat="server" ID="__SYSTEM_MODULE_FORM" Label="<span style='color:#321761'>Procesamiento electrónico de</span><span style='color:#782360;'>&nbsp;documentos</span>" HasAutoSave="false" OnCheckedChanged="MarcarPagina">

                <Buttonbar runat="server" OnClick="EventosBotonera">
                    <DropdownButtons>
                        <GWC:ButtonItem Text="Descargar" />
                        <GWC:ButtonItem Text="Imprimir" />
                        <GWC:ButtonItem Text="Mandar por Correo" />
                    </DropdownButtons>
                </Buttonbar>

             <Fieldsets>
                <GWC:FieldsetControl ID="fsDatosGenerales" runat="server" Label="Área de integración" Priority="true" CssClass="mb-0" style="margin-top:0px !important;" >
                  <ListControls>
                       <%--Inicio encabezado--%>
                           <asp:Panel runat="server" CssClass="dashboard-banner w-100 mb-5 mt-5 p-5" ID="banner_general">
                               <div class="row w-100 mb-5">
                                    <asp:Panel runat="server" CssClass="col-lg-12 col-md-12 col-12 col-xs-12"  id="titulodocumentosprocesados" Visible="False">
                                        <div class="row ">
                                            <div class="col-lg-1 col-md-10 col-12 col-xs-12"></div>

                                            <div class="col-lg-9 col-md-10 col-9 col-xs-9">
                                                <h6 class="section-title-secondary text-secondary">Documentos procesados</h6>
                                            </div>

                                            <div class="col-lg-1 col-md-1 col-1 col-xs-1"></div>
                                            <div class="col-lg-1 col-md-1 col-1 col-xs-1">
                                             <asp:Panel runat="server" CssClass="col-lg-12 col-md-12 col-12 pt-5 d-flex justify-content-end" id="lockopen" Visible="true">
                                                 <div><svg xmlns="http://www.w3.org/2000/svg" height="24px" viewBox="0 -960 960 960" width="24px" fill="#7e61b0"><path d="M240-80q-33 0-56.5-23.5T160-160v-400q0-33 23.5-56.5T240-640h360v-80q0-50-35-85t-85-35q-42 0-73.5 25.5T364-751q-4 14-16.5 22.5T320-720q-17 0-28.5-11t-8.5-26q11-68 66.5-115.5T480-920q83 0 141.5 58.5T680-720v80h40q33 0 56.5 23.5T800-560v400q0 33-23.5 56.5T720-80H240Zm240-200q33 0 56.5-23.5T560-360q0-33-23.5-56.5T480-440q-33 0-56.5 23.5T400-360q0 33 23.5 56.5T480-280Z"/></svg></div>
                                              </asp:Panel>

                                             <asp:Panel runat="server" CssClass="col-lg-12 col-md-12 col-12 pt-5 d-flex justify-content-end" id="lockclosed" Visible="false">
                                                <div><svg xmlns="http://www.w3.org/2000/svg" height="24px" viewBox="0 -960 960 960" width="24px" fill="#6C757D"><path d="M360-640h240v-80q0-50-35-85t-85-35q-50 0-85 35t-35 85v80ZM240-80q-33 0-56.5-23.5T160-160v-400q0-33 23.5-56.5T240-640h40v-80q0-83 58.5-141.5T480-920q83 0 141.5 58.5T680-720v80h40q33 0 56.5 23.5T800-560v6q0 17-13.5 28t-31.5 8q-8-1-17-1.5t-18-.5q-117 0-198.5 81.5T440-240q0 29 6.5 57.5T464-128q8 17-1.5 32.5T436-80H240Zm480 40q-83 0-141.5-58.5T520-240q0-83 58.5-141.5T720-440q83 0 141.5 58.5T920-240q0 83-58.5 141.5T720-40Zm20-208v-92q0-8-6-14t-14-6q-8 0-14 6t-6 14v91q0 8 3 15.5t9 13.5l60 60q6 6 14 6t14-6q6-6 6-14t-6-14l-60-60Z"/></svg></div>
                                             </asp:Panel>
                                            </div>
                                        </div>
                                    </asp:Panel>

                                    <asp:Panel runat="server" CssClass="col-lg-6 col-md-12 col-12 col-xs-12 mt-5" ID="panelentrada" Visible="true">
                                        <div class="row justify-content-center" style="border-right:1px solid #e2e2e2">
                                            <div class="col-lg-7 col-md-12 col-12 col-xs-12 mt-5">
                                                 <p class="text-secondary stat-folio mt-5" style="opacity:0.5">
                                                     <asp:Label runat="server" ID="textoentrada" Text="Busca tu folio o sube tus documentos" Visible="true"></asp:Label>
                                                 </p>
                                            </div>
                                        </div>
                                    </asp:Panel>

                                    <asp:Panel runat="server" CssClass="col-lg-3 col-md-12 col-12 col-xs-12 mt-5" ID="panelfolio1" Visible="false">
                                          <div class="row">
                                              <div class="col-lg-12 col-md-12 col-12 col-sm-12">
                                                <div class="row p-0 m-0 mb-5 ml-5">
                                                    <div class="col-lg-12 col-md-12 col-12 col-sm-12" style="position:relative">
                                                        <h4 class="text-purple font-weight-bold stat-folio">
                                                            <asp:Label runat="server" ID="folioprocesamiento" Text=""></asp:Label>
                                                        </h4>
                                                        <small class="text-muted" style="display:block; position:absolute; top:80%;">
                                                            <asp:Label runat="server" ID="label2" Text="Folio de carga"></asp:Label>
                                                        </small>
                                                    </div>
                                                 </div>

                                                <div class="row p-0 mt-5 ml-5">
                                                     <div class="col-lg-12 col-md-12 col-12 col-sm-12" style="position:relative;">
                                                         <h4 class="text-secondary stat-folio">
                                                             <asp:Label runat="server" ID="fechaprocesamiento" Text=""></asp:Label>
                                                         </h4>
                                                         <small class="text-muted" style="display:block; position:absolute; top:80%">
                                                             <asp:Label runat="server" ID="fechaprocesamientolabel" Text="Inicio de procesamiento"></asp:Label>
                                                         </small>
                                                     </div>
                                                  </div>
                                             </div>
                                          </div>
                                    </asp:Panel>

                                    <asp:Panel runat="server" CssClass="col-lg-1 col-md-6 col-12 col-xs-12 mt-5" style="border-right:1px solid #e2e2e2" ID="panelfolio2" Visible="false">
                                         <div class="row">
                                            <div class="col-lg-12 col-md-12 col-12 col-sm-12" style="position:relative;">
                                                <h2 class="stat-number text-secondary">
                                                    <asp:Label runat="server" ID="lbtotaldocumentoscerrrados" Text="0"></asp:Label>
                                                </h2>
                                                <small class="stat-label text-muted" style="display:block; position:absolute; top:90%;">
                                                    <asp:Label runat="server" ID="label_text_cerrrados" Text="terminados"></asp:Label>
                                                </small>
                                            </div>
                                         </div>
                                   </asp:Panel>

                                    <asp:Panel runat="server" CssClass="col-lg-2 col-md-6 col-12 col-xs-12 mt-5" ID="panelfolio3" Visible="false">
                                         <div class="row">
                                            <div class="col-lg-12 col-md-12 col-12 col-sm-12" style="position:relative; border-right:1px solid #e2e2e2">
                                                    <h2 class="stat-number text-purple-light" >
                                                        <asp:Label runat="server" ID="lbtotaldocumentosabiertos" Text="0"></asp:Label>
                                                    </h2>
                                                    <small class="stat-label text-purple-light" style="display:block; position:absolute; top:90%;"> 
                                                        <asp:Label runat="server" ID="label_text_abiertos" Text="pendientes"></asp:Label>
                                                    </small>
                                            </div>
                                         </div>
                                    </asp:Panel>

                                    <asp:Panel runat="server" CssClass="col-lg-6 col-md-12 col-12 col-xs-12 mt-5" ID="PanelControlesCliente" Enabled="true">
                                        <div class="row">
                                          <div class="col-lg-12 col-md-12 col-12 col-sm-12 mb-5">
                                              <div class="row">
                                                <GWC:FindboxControl runat="server" CssClass="col-lg-12 col-md-12 col-12 col-sm-12" ID="fbcrazonsocialcliente" Label="Razón social del cliente" RequiredSelect="true" OnClick="fbcrazonsocialcliente_Click" OnTextChanged="fbcrazonsocialcliente_TextChanged"/>
                                              </div>
                                          </div>

                                         <div class="col-lg-9 col-md-12 col-12 col-sm-12">
                                             <div class="row">
                                              <GWC:SelectControl runat="server" CssClass="col-lg-12 col-md-12 col-12 col-sm-12" ID="sctipodocumento" SearchBarEnabled="true" LocalSearch="false" Label="Documento" OnClick="sctipodocumento_Click" OnTextChanged="sctipodocumento_TextChanged"></GWC:SelectControl>
                                             </div>
                                         </div>

                                          <div class="col-lg-3 col-md-12 col-12 col-sm-12">
                                              <div class="row">
                                                <GWC:SwitchControl runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 align-content-center" ID="swcesimportacion" Label="Importación" OnText="Sí" OffText="No" checked="True"/>
                                              </div>
                                          </div>

                                        </div>
                                    </asp:Panel>
                                </div>
                            </asp:Panel>
                       <%--Fin encabezado--%>


                       <%--Inicio filecontrol--%>
                        <asp:Panel runat="server" CssClass="mt-5 w-100 mb-5 p-0" ID="PanelProcesamiento">
                             <GWC:FileControl runat="server" Label="Arrastre y suelte sus documentos aquí" CssClass="col-12 col-md-12 col-lg-12" ID="fcprocesamientofiles"  Dragable="true" OnChooseFile="fcprocesamientofiles_ChooseFile"/>
                        </asp:Panel>
                        <%--Fin filecontrol--%>

                       <%--Inicio Botones de acción--%>
                        <asp:Panel runat="server" CssClass="w-100 mb-5" ID="PanelButtonIA"  Visible="false">
                            <asp:Panel runat="server" CssClass="row justify-content-end">

                                  <asp:Panel runat="server" CssClass="col-lg-2 d-flex justify-content-end" Visible="false">
                                        <asp:Button runat="server" 
                                           CssClass="btn boder-outline-green btn-sm rounded-pill custom-btn-small" 
                                           Text="Subir documentos" 
                                           ID="SubirdocumentosGCS"  OnClick="SubirdocumentosGCS_Click" />
                                 </asp:Panel>

                                <%--<asp:Button ID="btnTerminarTarea" runat="server" OnClick="btnTerminarTarea_Click" style="display:none;" />--%>
                                 
                            </asp:Panel>
                        </asp:Panel>
                      <%--Fin Botones de acción--%>

                      <%--Inicio documentos determinados--%>
                        <asp:Panel runat="server" CssClass="dashboard-banner w-100 mb-5 mt-5 pb-5" ID="dbdocumentosterminados" Visible="false">
                              <div class="row">
                                    <div class="col-lg-1 col-md-10 col-12 col-xs-12"></div>
                                     <div class="col-lg-9 col-md-12 col-12 col-xs-12 mt-5">
                                         <h6 class="section-title-secondary text-secondary ml-5">Documentos subidos a GCS</h6>
                                       <%--  <h6 class="section-title-secondary text-secondary ml-5">Documentos enviados</h6>--%>
                                     </div>
                                 </div>

                              <div class="row mt-5 mb-5">
                                <div class="col-lg-2 col-md-12 col-12"></div>

                                <asp:Panel runat="server" CssClass="col-lg-4 col-md-12 col-12" style="border-right:1px solid #a4a4a4">
                                      <%--Inicio Factura 1--%>
                                       <asp:Panel runat="server" CssClass="row mb-5" ID="IconProcesoTerminado" Visible="false">
                                 
                                        </asp:Panel>

                                       <asp:Panel runat="server" CssClass="row mb-5" ID="IconProcesoIniciado" Visible="false">
      
                                       </asp:Panel>
                                      <%--Fin Factura 1--%>
                                </asp:Panel>

                               <div class="col-lg-1 col-md-12 col-12"></div>

                                <asp:Panel runat="server" CssClass="col-lg-4 col-md-12 col-12">
                                    <%--FACTURAS PEDIENTES--%>
                                </asp:Panel>
                              </div>
                        </asp:Panel>

                       <asp:Panel runat="server" CssClass="w-100 mb-5" ID="PanelProcesarDocumentosCIA">
                         <asp:Panel runat="server" CssClass="row justify-content-end">
                               <asp:Panel runat="server" CssClass="col-lg-2 d-flex justify-content-end">
                                     <asp:Button runat="server" 
                                        CssClass="btn boder-outline-green btn-sm rounded-pill custom-btn-small" 
                                        Text="Procesar C/I" 
                                        ID="BtnProcesarDocumentosCIA"  OnClick="BtnProcesarDocumentosCIA_Click" Enabled="true"/>
                              </asp:Panel>
                         </asp:Panel>
                        </asp:Panel>


                     <%--Fin documentos determinados--%>
                    </ListControls>
                </GWC:FieldsetControl>
            </Fieldsets>
        </GWC:FormControl>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server"></asp:Content>

