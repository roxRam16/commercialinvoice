<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/FrontEnd/Modulos/Home.Master" CodeBehind="Ges022_ExpedienteElectronico.aspx.vb" Inherits=".Ges022_ExpedienteElectronico" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentFindbar" runat="server">

    <% If IsPopup = False Then %>

    <GWC:FindbarControl Label="Buscar cliente" ID="__SYSTEM_CONTEXT_FINDER" runat="server" OnClick="BusquedaGeneral" />

    <% End If %>

    <link rel="stylesheet" type="text/css" href="Estilos.css" />

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentCompanyList" runat="server">

    <% If IsPopup = False Then %>

    <GWC:SelectControl CssClass="col-auto company-list-select" runat="server" SearchBarEnabled="false" ID="__SYSTEM_ENVIRONMENT" OnSelectedIndexChanged="CambiarEmpresa" />

    <% End If %>
</asp:Content>

<asp:Content runat="server" ID="Content4" ContentPlaceHolderID="contentBody">
    <div class="d-flex">

        <GWC:FormControl runat="server" ID="__SYSTEM_MODULE_FORM" HasAutoSave="false" Label="<span style='color:#321761'>Expediente</span><span style='color:#782360;'>&nbsp;electrónico</span>" OnCheckedChanged="MarcarPagina">

            <Buttonbar runat="server" OnClick="EventosBotonera" Visible="false">
                <DropdownButtons>
                    <GWC:ButtonItem Text="Descargar" />
                    <GWC:ButtonItem Text="Imprimir" />
                    <GWC:ButtonItem Text="Mandar por Correo" />
                </DropdownButtons>
            </Buttonbar>


            <Fieldsets>
                <GWC:FieldsetControl runat="server" ID="GeneralesExpedientes" Label="Generales" Enabled="true">
                    <ListControls>
                        <asp:Panel runat="server" CssClass="dashboard-banner w-100 mb-5" ID="panelGeneral">
                          <div class="row g-0 h-100 d-flex justify-content-between">

                            <!-- Expedientes -->
                            <div class="col-lg-5 col-md-6 col-12 col-sm-12 expedientes-section">
                                <div class="section-content">
                                    <div class="row g-0 numbers-row justify-content-center">
                                        <div class="col-lg-12">
                                            <div class="row justify-content-center">
                                                  <div class="col-lg-12 col-md-12 col-12 p-0">
                                                       <h6 class="section-title text-secondary ">Expedientes</h6>
                                                  </div>
                                                 <!--Aqui va lo de adentro-->
                                                <div class="col-lg-5 col-md-6 col-12 col-sm-12">
                                                    <div class="row justify-content-center">
                                                        <div class="col-lg-6 col-4 number-column">
                                                            <div class="number-container">
                                                                <h2 class="stat-number text-secondary">
                                                                    <asp:Label runat="server" ID="label_num_cerrrados" Text="0"></asp:Label>
                                                                </h2>
                                                                <small class="stat-label text-muted">
                                                                    <asp:Label runat="server" ID="label_text_cerrrados" Text="Cerrados"></asp:Label>
                                                                </small>
                                                            </div>
                                                        </div>
                                                        <div class="col-lg-6 col-4 number-column with-separator">
                                                            <div class="number-container">
                                                                <h2 class="stat-number text-purple font-weight-bold">
                                                                    <asp:Label runat="server" ID="label_num_abiertos" Text="0"></asp:Label>
                                                                </h2>
                                                                <small class="stat-label text-purple">
                                                                    <asp:Label runat="server" ID="label_text_abiertos" Text="Abiertos"></asp:Label>
                                                                </small>
                                                            </div>
                                                        </div>
                                                        <div class="col-lg-10 col-md-12 col-12 d-flex justify-content-center mt-3">
                                                        
                                                                 <asp:Button runat="server" 
                                                                     Type="button"
                                                                     CssClass="btn btn-outline-purple btn-sm rounded-pill custom-btn-small" 
                                                                     Text="Descarga masiva" 
                                                                     ID="BtnDescargaMasiva" />
                                                            
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-lg-3 col-md-6 col-12 number-column with-separator">
                                                    <div class="row justify-content-center">
                                                        <div class="col-lg-12 col-md-12 col-12 p-0 ">
                                                          <div class="number-container">
                                                              <h2 class="stat-number text-secondary">
                                                                  <asp:Label runat="server" ID="label_num_vacios" Text="0"></asp:Label>
                                                              </h2>
                                                              <small class="stat-label text-muted">Vacíos</small>
                                                          </div>
                                                        </div>

                                                         <div class="col-lg-10 col-md-12 col-12 d-flex justify-content-center mt-3">
                                                          
                                                                <asp:Button runat="server" 
                                                                    CssClass="btn btn-outline-gray btn-sm rounded-pill custom-btn-small" 
                                                                    Text="Ver más" 
                                                                    ID="BtnVerMasVacios" />
                                                       
                                                         </div>
                                                    </div>
                                                </div>

                                                <!--fin va lo de adentro-->
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- Documentos del Cliente -->
                            <div class="col-lg-5 col-md-6 col-12 col-sm-12 documentos-section">
                                <div class="section-content">
                                    <div class="row g-0 numbers-row justify-content-center">
                                        <div class="col-lg-12 col-md-6 col-12 col-sm-12">
                                            <div class="row justify-content-center">
                                                <div class="col-lg-12 col-md-12 col-12 p-0">
                                                     <h6 class="section-title text-secondary">Documentos del <span class="text-bold">cliente</span></h6>
                                                </div>
                                                 <!--Aqui va lo de adentro-->
                                                <div class="col-lg-1"></div>
                                                <div class="col-lg-3 col-md-4 col-12 col-sm-12">
                                                    <div class="row d-flex justify-content-center">
                                                        <div class="col-lg-12 col-4 number-column">
                                                            <div class="number-container">
                                                                <h2 class="stat-number text-secondary">
                                                                    <asp:Label runat="server" ID="label_num_activos" Text="0"></asp:Label>
                                                                </h2>
                                                                <small class="stat-label text-muted">Activos</small>
                                                                  
                                                            </div>
                                                        </div>
                                                        <div class="col-lg-10 col-md-12 col-12 d-flex justify-content-center mt-3">
                                                           
                                                                <asp:Button runat="server" 
                                                                    CssClass="btn btn-outline-gray btn-sm rounded-pill custom-btn-small" 
                                                                    Text="Ver más" 
                                                                    ID="BtnVerMasActivos" />
                                                           
                                                        </div>
                                                    </div>
                                                </div>

                                                 <div class="col-lg-4 col-md-4 col-12 col-sm-12">
                                                     <div class="row d-flex justify-content-center">
                                                         <div class="col-lg-12 col-4 number-column with-separator">
                                                            <div class="number-container">
                                                                <h2 class="stat-number text-secondary">
                                                                    <asp:Label runat="server" ID="label_num_vencidos" Text="0"></asp:Label>
                                                                </h2>
                                                                <small class="stat-label text-muted">Vencidos</small>
                                                            </div>
                                                        </div>

                                                         <div class="col-lg-10 col-md-12 col-12 d-flex justify-content-center mt-3">
                                                          
                                                                <asp:Button runat="server" 
                                                                    CssClass="btn btn-outline-gray btn-sm rounded-pill custom-btn-small" 
                                                                    Text="Ver más" 
                                                                    ID="BtnVerMasVencidos" />
                                                         
                                                         </div>
                                                     </div>
                                                 </div>

                                                 <div class="col-lg-4 col-md-4 col-12 col-sm-12">
                                                      <div class="row d-flex justify-content-center">
                                                         <div class="col-lg-12 col-md-12 col-12 number-column with-separator">
                                                              <div class="number-container">
                                                                  <h2 class="stat-number text-purple font-weight-bold">
                                                                      <asp:Label runat="server" ID="label_num_sinreferencia" Text="0"></asp:Label>
                                                                  </h2>
                                                                  <small class="stat-label text-purple">Sin referencia</small>
                                                              </div>
                                                          </div>

                                                           <div class="col-lg-10 col-md-12 col-12 d-flex justify-content-center mt-3">
                                                                  <asp:Button runat="server" 
                                                                      CssClass="btn btn-outline-gray btn-sm rounded-pill custom-btn-small" 
                                                                      Text="Ver más" 
                                                                      ID="BtnVerMasSinReferencia" />
                                                           </div>
                                                      </div>
                                                 </div>
                                                 <!--fin va lo de adentro-->
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        </asp:Panel>
                    </ListControls>
                </GWC:FieldsetControl>
            </Fieldsets>

            <Fieldsets>
                <GWC:FieldsetControl runat="server" ID="Recientes" Label="Recientes" Enabled="false">
                      <ListControls>
                        <asp:Panel runat="server" CssClass="dashboard-banner w-100 mb-5" ID="banner_expediente_recientes_cliente">
                            <div class="row g-0 d-flex justify-content-between">

                              <div class="col-lg-11 col-md-12 col-12 col-sm-12">
                                    <div class="section-content">
                                        <div class="row g-0 numbers-row ">
                                
                                                     <!--Aqui va lo de adentro-->
                                                    <div class="col-lg-12 col-md-12 col-12 col-sm-12 d-flex justify-content-center">
                                                      
                                                        <div class="row d-flex">

                                                            <div class="col-lg-12">
                                                                 <h6 class="section-title-secondary text-secondary">Expediente recientes del cliente</h6>
                                                            </div>

                                                            <div class="col-lg-2 col-md-12 col-12 col-xs-12">
                                                              <div class="row">
                                                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 mt-4">
                                                                    <GWC:SwitchControl runat="server" ID="swcexpedientesabiertos" CssClass="mt-5" Label="Sólo abiertos" OnText="Sí" OffText="No" />
                                                                </asp:Panel>
                                                                   <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 mt-5">
                                                                      <GWC:SwitchControl runat="server" ID="swcexpedientespropios" CssClass="" Label="Mis expedientes" OnText="Sí" OffText="No" />
                                                                  </asp:Panel>
                                                              </div>
                                                           </div>

                                                           <asp:Panel runat="server" CssClass="col-lg-8 col-md-12 col-12 col-xs-12" ID="panelentrada" Visible="true">
                                                              <div class="row justify-content-center">
                                                                  <div class="col-lg-7 col-md-12 col-12 col-xs-12 mt-5">
                                                                       <p class="text-secondary mt-5 text-center pt-5" style="opacity:0.5; font-size:2.4rem">
                                                                           <asp:Label runat="server" ID="textoentrada" Text="Datos no disponibles" Visible="true"></asp:Label>
                                                                       </p>
                                                                  </div>
                                                              </div>
                                                          </asp:Panel>

                                                          <asp:Panel runat="server" CssClass="col-lg-10 col-md-12 col-12 col-xs-12" ID="panelresultadoscliente" Visible="false">
                                                              <asp:Panel runat="server" CssClass="row" ID="PanelExpedientes"></asp:Panel>
                                                          </asp:Panel>

                                                           <asp:Panel runat="server" CssClass="col-lg-12 col-md-12 col-12 col-sm-12 mt-5">
                                                               <div class="row d-flex justify-content-end">
                                                                     <div class="col-lg-2 col-md-6 col-6 d-flex justify-content-end">

                                                                         <GWC:ButtonControl runat="server"  
                                                                             TypeButton="button" 
                                                                             CssClass="btn btn-outline-purple btn-sm rounded-pill custom-btn-small"
                                                                             Text="Buscar expediente"  onclick="BtnBuscarExpediente_Click" ID="BtnBuscarExpediente"/>
                                                                         
                                                                   <%--      <asp:Button runat="server" 
                                                                            CssClass="btn btn-outline-purple btn-sm rounded-pill custom-btn-small" 
                                                                            Text="Buscar expediente" 
                                                                            ID="BtnBuscarExpediente" onclick="BtnBuscarExpediente_Click" UseSubmitBehavior="false"/>--%>
                                                                     </div>

                                                                     <div class="col-lg-1 col-md-6 col-6 d-flex p-0">
                                                                             <asp:Button runat="server" 
                                                                                CssClass="btn btn-outline-gray btn-sm rounded-pill custom-btn-small" 
                                                                                Text="Ver 10 más" 
                                                                                ID="BtnVer10mas" UseSubmitBehavior="false"/>
                                                                     </div>
                                                               </div>
                                                           </asp:Panel>

                                                        </div>
                                                    </div>

                                                    <!--fin va lo de adentro-->
                                               <%-- </div>
                                            </div>--%>
                                        </div>
                                    </div>
                              </div>

    
                            </div>
                        </asp:Panel>
                     </ListControls>
                </GWC:FieldsetControl>
            </Fieldsets>

            <Fieldsets>
                <GWC:FieldsetControl runat="server" ID="DescargarUnExpediente" Label="Descargar un expediente" Visible="false">
                     <ListControls>
                        <asp:Panel runat="server" CssClass="dashboard-banner w-100 mb-5 p-5" ID="Panel1">
                            <div class="row g-0 h-100 d-flex justify-content-center mb-5 p-5">

                                 <div class="col-lg-5 col-md-12 col-12">
                                    <GWC:DualityBarControl runat="server" CssClass="col-xs-12 col-12 col-md-12 col-lg-12" ID="dbcreferenciaexpediente" Label="Referencia" LabelDetail="" Value="" ValueDetail=""/>
                                 </div>

                                  <div class="col-lg-4 col-md-12 col-12 mt-5 d-flex justify-content-center">
                                     <GWC:SelectControl runat="server" 
                                         CssClass="col-xs-12 col-12 col-md-12 col-lg-12 mt-5" 
                                         ID="sctipousoexpediente" 
                                         SearchBarEnabled="true" 
                                         LocalSearch="false" Label="Tipo uso" 
                                         OnClick="sctipousoexpediente_Click" 
                                         OnTextChanged="sctipousoexpediente_TextChanged" OnSelectedIndexChanged="sctipousoexpediente_SelectedIndexChanged"></GWC:SelectControl>
                                  </div>

                                  <div class="col-lg-2 col-md-12 col-12 d-flex justify-content-end mt-5">
                                      <div class="row d-flex">
                                          <div class="col-lg-5 pt-5">

                                              <GWC:ButtonControl runat="server" TypeButton="button"  CssClass="btn-utils-expediente"  id="BtnLocalizar" OnClick="BtnLocalizar_Click" Text="Localizar"/>
                                           
                                          <%--   <button type="button" type="button" class="btn-utils-expediente" id="BtnLocalizar">
                                                   <svg xmlns='http://www.w3.org/2000/svg' height='5rem' viewBox='0 -960 960 960' width='5rem' fill='#8B5FBF'><path d='M80-200v-60h400v60H80Zm0-210v-60h200v60H80Zm0-210v-60h200v60H80Zm758 420L678-360q-26 20-56 30t-62 10q-83 0-141.5-58.5T360-520q0-83 58.5-141.5T560-720q83 0 141.5 58.5T760-520q0 32-10 62t-30 56l160 160-42 42ZM559.76-380Q618-380 659-420.76q41-40.77 41-99Q700-578 659.24-619q-40.77-41-99-41Q502-660 461-619.24q-41 40.77-41 99Q420-462 460.76-421q40.77 41 99 41Z'/></svg>
                                                   <small class="text-muted txt-bold">Localizar</small>
                                           </button>--%>
                                          </div>

                                         <div class="col-lg-4">
                                              <%--   <button type="button" class="btn-utils-expediente" id="Download">
                                                     <svg xmlns='http://www.w3.org/2000/svg' height='5rem' viewBox='0 -960 960 960' width='5rem' fill='#E1E1E1'><path d='M251-160q-88 0-149.5-61.5T40-371q0-78 50-137t127-71q18-90 83-150t151-68v349l-83-83-43 43 156 156 156-156-43-43-83 83v-349q101 11 169 90t68 185v24q72-2 122 46.5T920-329q0 69-50 119t-119 50H251Z'/></svg>
                                                     <small class="text-muted txt-bold">Download</small>
                                                 </button>--%>
                                            </div>
                                      </div>  
                                </div>
                            </div>
                        </asp:Panel>


                     
                        <GWC:CardControl runat="server" ID="DownloadPackage" ClientIDMode="Static"  Visible="false" CssClass="container m-0 p-0 mt-5 mb-5">
                            <ListControls>
                               
                                    <asp:Panel runat="server" CssClass="w-100 row p-0" ID="Panel2">
                                 
                                        <div class="col-lg-2 col-4">
                                            <div class="number-container">
                                                <div class="text-secondary  d-flex gap-2 align-items-end mt-3">
                                                    <asp:Label runat="server" ID="label62" CssClass="d-inline cl_Num__Tarjeta_principal" 
                                                        Text="<svg xmlns='http://www.w3.org/2000/svg' height='40px' viewBox='0 -960 960 960' width='40px' fill='#ffffff'><path d='M160-80v-554h130v-96q0-78.85 55.61-134.42Q401.21-920 480.11-920q78.89 0 134.39 55.58Q670-808.85 670-730v96h130v554H160Zm320.17-200q31.83 0 54.33-22.03T557-355q0-30-22.67-54.5t-54.5-24.5q-31.83 0-54.33 24.5t-22.5 55q0 30.5 22.67 52.5t54.5 22ZM350-634h260v-96q0-54.17-37.88-92.08-37.88-37.92-92-37.92T388-822.08q-38 37.91-38 92.08v96Z'/></svg>"></asp:Label>
                                                </div>
                                                <small class="text-muted txt-bold">6d cerrados</small>
                                            </div>
                                    </div>
                                                
                                    <div class="col-lg-4 col-4">
                                           
                                         <div class="stat-label text-secondary  d-flex gap-2 align-items-center">
                                        <asp:Label runat="server" ID="CardNameCliente" CssClass="d-flex text-card mb-3" Text=""></asp:Label>
                                        <asp:Label runat="server" ID="CardNameExpediente" CssClass="d-flex mb-3 txt-16 text-bold-light" Text="Expediente de comercio exterior"></asp:Label>
                                        <asp:Label runat="server" ID="CardNameOwner" CssClass="d-flex text-cursive txt-16 mb-5" Text="Claudia Vargas, Veracruz Ver     <a href='#' class='enlace-btn'><span class='txt-subrayado text-bold pl-5'><u>Ver más...</u></span></a>"></asp:Label>
                                        <asp:Label ranat="server" ID="CardLink" CssClass="" Text=""></asp:Label>
                              <%--          <a href="#" class="d-flex txt-16">https://gdrive.es/exp/ume345345-342344.zip</a>--%>
                                    </div>
                                                   
                                    </div>

                                   <div class="row mt-3">
                                        <div class="col-lg-2"></div>
                                            <div class="col-lg-1 ml-5 mr-5">
                                                <asp:Button runat="server" 
                                                    CssClass="btn btn-outline-purple btn-sm rounded-pill custom-btn-small" 
                                                    Text="Explorar contenido" 
                                                    ID="Button5" />
                                            </div>

                                            <div class="col-lg-1 ">
                                                <asp:Button runat="server" 
                                                    CssClass="btn btn-outline-purple btn-sm rounded-pill custom-btn-small" 
                                                    Text="Compartir " 
                                                    ID="Button8" />
                                             </div>

                                            <div class="col-lg-6"></div>
                                                <div class="col-lg-1 d-flex justify-content-end">
                                                       <GWC:ButtonControl runat="server" TypeButton="button"  CssClass="btn-utils-expediente-purple"  id="DescargarExpediente" OnClick="DescargarExpediente_Click" Text="Descargar"/>
                                                <%--    <button type="button" class="btn-utils-expediente">
                                                        <svg xmlns='http://www.w3.org/2000/svg' height='5rem' viewBox='0 -960 960 960' width='5rem' fill='#782360'><path d='M251-160q-88 0-149.5-61.5T40-371q0-78 50-137t127-71q18-90 83-150t151-68v349l-83-83-43 43 156 156 156-156-43-43-83 83v-349q101 11 169 90t68 185v24q72-2 122 46.5T920-329q0 69-50 119t-119 50H251Z'/></svg>
                                                        <small class="text-muted txt-bold d-flex">Download</small>
                                                    </button>--%>
                                                </div>
                                            </div>
                                    </asp:Panel>
                                 
                            </ListControls>
                        </GWC:CardControl>
                   



                     </ListControls>
                </GWC:FieldsetControl>
            </Fieldsets>


<%--            <Fieldsets>
                <GWC:FieldsetControl runat="server" ID="DescargaExpedientePorPeriodos" Label="Descargar expediente por periodos">
                </GWC:FieldsetControl>
            </Fieldsets>--%>

            <Fieldsets>
                <GWC:FieldsetControl runat="server" ID="ObtenerExpedientePorPeriodos" Label="Obtener expediente por periodos" Visible="false">
                     <ListControls>
   
                        <asp:Panel runat="server" CssClass="dashboard-banner w-100 mb-5" ID="PanelObtenerExpedientePorPeriodos"  >
                             <div class="row g-0 h-100 d-flex justify-content-between">

                               <div class="col-lg-11 col-md-12 col-12 col-sm-12">
                                     <div class="section-content">
                                         <div class="row g-0 numbers-row justify-content-center">
                                                <!--Aqui va lo de adentro-->
                                                <div class="col-lg-12 col-md-12 col-12 col-sm-12">
                                                    <div class="row d-flex justify-content-between">

                                                    <div class="col-lg-12 col-md-12 col-12 mb-3">
                                                        <h6 class="section-title-secondary text-secondary ">También te podría interesar <span class="txt-light">ver pedimentos</span></h6>
                                                </div>

                                                <asp:Panel runat="server" CssClass="col-lg-8 col-md-8 col-8 col-xs-8" ID="panel3" Visible="true">
                                                <div class="row justify-content-center">
                                                    <div class="col-lg-7 col-md-12 col-12 col-xs-12 mt-5">
                                                            <p class="text-secondary mt-5" style="opacity:0.5">
                                                                <asp:Label runat="server" ID="Label1" Text="No disponibles" Visible="true"></asp:Label>
                                                            </p>
                                                    </div>
                                                </div>
                                            </asp:Panel>



                                                <%--   <div class="col-lg-4 col-4">
                                                        <div class="number-container">
                                                                <div class="stat-label text-secondary  d-flex gap-2 align-items-center mt-3">
                                                                    <asp:Label runat="server" ID="label27" CssClass="d-inline text-purple-light" Text="RKU25-<span style='font-weight:bold'>02354</span> <svg xmlns='http://www.w3.org/2000/svg' height='20px' viewBox='0 -960 960 960' width='20px' fill='#424242'><path d='M144-144v-672h336v72H216v528h528v-264h72v336H144Zm243-192-51-51 357-357H576v-72h240v240h-72v-117L387-336Z'/></svg>"></asp:Label>
                                                                    <asp:Label runat="server" ID="label34" CssClass="d-inline text-cursive" Text="Unilever | Claudia Vargas"></asp:Label>
                         
                                                                </div>
                                                            </div>

                                                        <div class="number-container">
                                                            <div class="stat-label text-secondary  d-flex gap-2 align-items-center mt-3">
                                                                <asp:Label runat="server" ID="label45" CssClass="d-inline text-purple-light" Text="RKU25-<span style='font-weight:bold'>02354</span> <span><svg xmlns='http://www.w3.org/2000/svg' height='20px' viewBox='0 -960 960 960' width='20px' fill='#424242'><path d='M120-120v-720h360v80H200v560h560v-280h80v360H120Zm268-212-56-56 372-372H560v-80h280v280h-80v-144L388-332Z'/></svg></span>"></asp:Label>
                                                                <asp:Label runat="server" ID="label46" CssClass="d-inline text-cursive" Text="Unilever | Claudia Vargas"></asp:Label>
                                                            </div>
                                                        </div>

                                                        <div class="number-container">
                                                            <div class="stat-label text-secondary  d-flex gap-2 align-items-center mt-3">
                                                                <asp:Label runat="server" ID="label49" CssClass="d-inline text-purple-light" Text="RKU25-<span style='font-weight:bold'>02354</span> <span><svg xmlns='http://www.w3.org/2000/svg' height='20px' viewBox='0 -960 960 960' width='20px' fill='#424242'><path d='M120-120v-720h360v80H200v560h560v-280h80v360H120Zm268-212-56-56 372-372H560v-80h280v280h-80v-144L388-332Z'/></svg></span>"></asp:Label>
                                                                <asp:Label runat="server" ID="label50" CssClass="d-inline text-cursive" Text="Unilever | Claudia Vargas"></asp:Label>
                                                            </div>
                                                        </div>

                                                    </div>

                                                    <div class="col-lg-4 col-4">
                                                        <div class="number-container">
                                                                <div class="stat-label text-secondary  d-flex gap-2 align-items-center mt-3">
                                                                    <asp:Label runat="server" ID="label35" CssClass="d-inline text-purple-light" Text="RKU25-<span style='font-weight:bold'>02354</span> <span><svg xmlns='http://www.w3.org/2000/svg' height='20px' viewBox='0 -960 960 960' width='20px' fill='#424242'><path d='M120-120v-720h360v80H200v560h560v-280h80v360H120Zm268-212-56-56 372-372H560v-80h280v280h-80v-144L388-332Z'/></svg></span>"></asp:Label>
                                                                    <asp:Label runat="server" ID="label36" CssClass="d-inline text-cursive" Text="Unilever | Claudia Vargas"></asp:Label>
                         
                                                                </div>
                                                            </div>

                                                        <div class="number-container">
                                                            <div class="stat-label text-secondary  d-flex gap-2 align-items-center mt-3">
                                                                <asp:Label runat="server" ID="label47" CssClass="d-inline text-purple-light" Text="RKU25-<span style='font-weight:bold'>02354</span> <span><svg xmlns='http://www.w3.org/2000/svg' height='20px' viewBox='0 -960 960 960' width='20px' fill='#424242'><path d='M120-120v-720h360v80H200v560h560v-280h80v360H120Zm268-212-56-56 372-372H560v-80h280v280h-80v-144L388-332Z'/></svg></span>"></asp:Label>
                                                                <asp:Label runat="server" ID="label48" CssClass="d-inline text-cursive" Text="Unilever | Claudia Vargas"></asp:Label>
                                                            </div>
                                                        </div>

                                                        <div class="number-container">
                                                            <div class="stat-label text-secondary  d-flex gap-2 align-items-center mt-3">
                                                                <asp:Label runat="server" ID="label51" CssClass="d-inline text-purple-light" Text="RKU25-<span style='font-weight:bold'>02354</span> <span><svg xmlns='http://www.w3.org/2000/svg' height='20px' viewBox='0 -960 960 960' width='20px' fill='#424242'><path d='M120-120v-720h360v80H200v560h560v-280h80v360H120Zm268-212-56-56 372-372H560v-80h280v280h-80v-144L388-332Z'/></svg></span>"></asp:Label>
                                                                <asp:Label runat="server" ID="label52" CssClass="d-inline text-cursive" Text="Unilever | Claudia Vargas"></asp:Label>
                                                            </div>
                                                        </div>

                                                    </div>

                                                    <div class="col-lg-4 col-4">
                                                        <div class="number-container">
                                                                <div class="stat-label text-secondary  d-flex gap-2 align-items-center mt-3">
                                                                    <asp:Label runat="server" ID="label53" CssClass="d-inline text-purple-light" Text="RKU25-<span style='font-weight:bold'>02354</span> <span><svg xmlns='http://www.w3.org/2000/svg' height='20px' viewBox='0 -960 960 960' width='20px' fill='#424242'><path d='M120-120v-720h360v80H200v560h560v-280h80v360H120Zm268-212-56-56 372-372H560v-80h280v280h-80v-144L388-332Z'/></svg></span>"></asp:Label>
                                                                    <asp:Label runat="server" ID="label54" CssClass="d-inline text-cursive" Text="Unilever | Claudia Vargas"></asp:Label>
                                                                </div>
                                                            </div>

                                                        <div class="number-container">
                                                            <div class="stat-label text-secondary  d-flex gap-2 align-items-center mt-3">
                                                                <asp:Label runat="server" ID="label55" CssClass="d-inline text-purple-light" Text="RKU25-<span style='font-weight:bold'>02354</span> <span><svg xmlns='http://www.w3.org/2000/svg' height='20px' viewBox='0 -960 960 960' width='20px' fill='#424242'><path d='M120-120v-720h360v80H200v560h560v-280h80v360H120Zm268-212-56-56 372-372H560v-80h280v280h-80v-144L388-332Z'/></svg></span>"></asp:Label>
                                                                <asp:Label runat="server" ID="label56" CssClass="d-inline text-cursive" Text="Unilever | Claudia Vargas"></asp:Label>
                                                            </div>
                                                        </div>

                                                        <div class="number-container">
                                                            <div class="stat-label text-secondary  d-flex gap-2 align-items-center mt-3">
                                                                <asp:Label runat="server" ID="label57" CssClass="d-inline text-purple-light" Text="RKU25-<span style='font-weight:bold'>02354</span> <span><svg xmlns='http://www.w3.org/2000/svg' height='20px' viewBox='0 -960 960 960' width='20px' fill='#424242'><path d='M120-120v-720h360v80H200v560h560v-280h80v360H120Zm268-212-56-56 372-372H560v-80h280v280h-80v-144L388-332Z'/></svg></span>"></asp:Label>
                                                                <asp:Label runat="server" ID="label58" CssClass="d-inline text-cursive" Text="Unilever | Claudia Vargas"></asp:Label>
                                                            </div>
                                                        </div>

                                                    </div>--%>

                                                    </div>
                                                </div>

                                                <!--fin va lo de adentro-->
                                         </div>
                                     </div>
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
