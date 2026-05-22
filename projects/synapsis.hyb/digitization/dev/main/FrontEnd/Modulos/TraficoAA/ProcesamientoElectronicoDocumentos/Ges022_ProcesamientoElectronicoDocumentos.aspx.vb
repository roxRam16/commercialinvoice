
#Region "├┴┘├┴┘├┴┘├┴┘├┴┘|├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘   DEPENDENCIAS   ├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘"

'RECURSOS DEL CMF
Imports Syn.Documento
Imports MongoDB.Driver
Imports Wma.Exceptions
Imports Gsol.Web.Components
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior.CamposProcesamientoElectDocumentos
Imports Syn.Nucleo.RecursosComercioExterior.SecuenciasComercioExterior
Imports Wma.Exceptions.TagWatcher
Imports Wma.Exceptions.TagWatcher.TypeStatus
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Nucleo.Recursos.CamposDomicilio
'UTILERIAS/RECURSOS ADICIONALES
Imports Sax.Web
'OBJETOS DIMENSIONALES (ODS's) Dependencias en MongoDB
Imports Rec.Globals
Imports Rec.Globals.Utils
'OBJETOS BIDIMENSIONALES (ODF's.  Dependencias Krombase/SQL Server)
Imports MongoDB.Bson
Imports ControladorEmpresas = Rec.Globals.Controllers.Empresas.ControladorEmpresas
Imports Empresa = Rec.Globals.Empresas.Empresa
Imports Syn.Utils
Imports System.IO
Imports MongoDB.Bson.Serialization.Attributes
Imports System.Web.Services.Description
Imports Syn.CustomBrokers.Controllers
Imports Newtonsoft.Json.Linq
Imports Newtonsoft.Json
Imports RestSharp
Imports Rec.Globals.Controllers
Imports System.Web.Caching
Imports System.Threading.Tasks
Imports MongoDB.Driver.Encryption
Imports System.Text.Json
Imports Microsoft.Owin.BuilderProperties
Imports System.Reflection.Emit




#End Region

Public Class Ges022_ProcesamientoElectronicoDocumentos
    Inherits ControladorBackend
#Region "████████████████████████████████████████   Atributos locales  ██████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    Private _controladorDocumentos As IControladorDocumento

    Private _controladorProcesamientoElectronico As IControladorProcesamientoElectronico

    Private _controladorExpedienteElectronico As IControladorExpedienteElectronico

    Private _controladorClientes As IControladorClientes

    Private _lista As List(Of SelectOption)

    Private _tagwatcher As TagWatcher

    Private _constructorClienteBusqueda As ControladorBusqueda(Of ConstructorCliente)

    Private _payloadAPI As DocumentoElectronicoApiStorage

    Private _loginUsuario As Dictionary(Of String, String)

    Private _auxDatosCliente As AuxDatosCliente

    Private _cacheListaDocumentos As List(Of SelectOption)

    Private _listaDocumentosGCS As List(Of DocumentoElectronicoApiStorage)


#End Region

#Region "████████████████████████████████████████   Constructores  ██████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████
    Sub New()
        Dim officeOnline_ = Statements.GetOfficeOnline

        If officeOnline_ Is Nothing Then

            Statements.SetEnvironmentOnline(1)

        End If

    End Sub

#End Region

#Region "██████ Vinculación c/capas inf █████████       SAX      ████████████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████
    Public Overrides Sub Inicializa()
        With Buscador

            .DataObject = New ConstructorProcesamientoElectDocumentos(True)

            .addFilter(SeccionesProcesamientoElectDocumentos.SPED1, CamposProcesamientoElectDocumentos.CP_FOLIO_PROCESAMIENTO, "Folio de procesamiento")

        End With

        If OperacionGenerica IsNot Nothing Then
            '    Dim _cantidadDetalles = OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Seccion(SeccionesProcesamientoElectDocumentos.SPED2)
        End If

        _loginUsuario = New Dictionary(Of String, String)

    End Sub

    'ASIGNACION PARA CONTROLES AUTOMÁTICOS
    Public Overrides Function Configuracion() As TagWatcher
        ''Datos generales
        [Set](fbcrazonsocialcliente, CamposProcesamientoElectDocumentos.CP_RAZON_SOCIAL_CLIENTE, propiedadDelControl_:=PropiedadesControl.Valor)

        [Set](sctipodocumento, CP_TIPO_DOCUMENTO_PROCESADO, propiedadDelControl_:=PropiedadesControl.Valor)

        Return New TagWatcher(1)

    End Function

    Public Overrides Sub BotoneraClicNuevo()
        Dim modoEditando_ As Boolean = False
        If GetVars("isEditing") IsNot Nothing Then
            If GetVars("isEditing") = True Then
                modoEditando_ = True
            End If
        End If

        If OperacionGenerica IsNot Nothing Then
        End If

        'fsDatosGenerales.Enabled = True

        'PanelProcesamiento.Visible = True

        'PanelButtonIA.Visible = True

        'fsDatosGenerales.Enabled = True

        'folioprocesamiento.Text = Nothing

        'lbtotaldocumentosabiertos.Text = Nothing

        'lbtotaldocumentoscerrrados.Text = Nothing

        'IconProcesoIniciado.Visible = False

        'IconProcesoTerminado.Visible = False

        ''LabelTxtDocumento.Text = Nothing

        ''LabelScore.Text = Nothing

        'PanelControlesCliente.Enabled = True

        'panelentrada.Visible = True

        'titulodocumentosprocesados.Visible = False

        'panelfolio1.Visible = False

        'panelfolio2.Visible = False

        'panelfolio3.Visible = False

        'dbdocumentosterminados.Visible = False

        'fechaprocesamiento.Text = Nothing

        'fcprocesamientofiles.Value = Nothing

    End Sub

    Public Overrides Sub BotoneraClicGuardar()

        If Not ProcesarTransaccion(Of ConstructorProcesamientoElectDocumentos)().Status = TypeStatus.Errors Then : End If

    End Sub

    Public Overrides Sub BotoneraClicEditar()

    End Sub

    Public Overrides Sub BotoneraClicBorrar()

    End Sub

    Public Overrides Sub BotoneraClicOtros(IndexSelected_ As Integer)
        If IndexSelected_ = 7 Then

        ElseIf IndexSelected_ = 8 Then

        End If
    End Sub

    'EVENTOS PARA LA INSERCIÓN DE DATOS
    Public Overrides Function AntesRealizarInsercion(ByVal session_ As IClientSessionHandle) As TagWatcher
        Dim tagwatcher_ As New TagWatcher
        If session_ IsNot Nothing Then
            tagwatcher_.SetOK()
        Else
            tagwatcher_.SetOK()
        End If
        Return tagwatcher_
    End Function

    Public Overrides Sub RealizarInsercion(ByRef documentoElectronico_ As DocumentoElectronico)
        With documentoElectronico_

        End With
    End Sub

    Public Overrides Function DespuesRealizarInsercion() As TagWatcher
        Return New TagWatcher(Ok)
    End Function

    'EVENTOS PARA MODIFICACIÓN DE DATOS
    Public Overrides Function AntesRealizarModificacion(ByVal session_ As IClientSessionHandle) As TagWatcher
        Dim tagwatcher_ As New TagWatcher
        If session_ IsNot Nothing Then '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas con transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 
            tagwatcher_.SetOK()
        Else  '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas sin transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 
            tagwatcher_.SetOK()
        End If
        Return tagwatcher_
    End Function

    Public Overrides Sub PreparaModificacion(ByRef documentoElectronico_ As DocumentoElectronico)

    End Sub

    Public Overrides Sub RealizarModificacion(ByRef documentoElectronico_ As DocumentoElectronico)

    End Sub

    Public Overrides Sub DespuesOperadorDatosProcesar(ByRef documentoElectronico_ As DocumentoElectronico)
        With documentoElectronico_

        End With

    End Sub

    Public Overrides Function DespuesRealizarModificacion() As TagWatcher

        Return New TagWatcher(Ok)
    End Function

    'EVENTOS PARA PRESENTACIÓN DE DATOS EN FRONTEND
    Public Overrides Sub DespuesBuquedaGeneralConDatos()

    End Sub

    Public Overrides Sub DespuesBuquedaGeneralSinDatos()

    End Sub
    'EVENTOS DE MANTENIMIENTO
    Public Overrides Sub LimpiaSesion()

        HttpRuntime.Cache.Remove("cacheListaDocumentos")

    End Sub

    Public Overrides Sub Limpiar()

    End Sub
#End Region

#Region "████████████████  QUINTA CAPA  █████████       Reglas locales         ██████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████
    'EVENTO PARA REGRESAR CONTROLES POR CADA ACCIÓN DE TARJETA
    Protected Sub fbcrazonsocialcliente_Click(sender As Object, e As EventArgs)

        If fbcrazonsocialcliente.Text <> "" Then

            PanelProcesamiento.Enabled = True

            SubirdocumentosGCS.Enabled = True

        End If

    End Sub

    Protected Sub fbcrazonsocialcliente_TextChanged(sender As Object, e As EventArgs)

        If fbcrazonsocialcliente.Value = "" Then

            _lista = New List(Of SelectOption)

            _constructorClienteBusqueda = New ControladorBusqueda(Of ConstructorCliente)

            _lista = _constructorClienteBusqueda.Buscar(fbcrazonsocialcliente.Text,
                                                           New Filtro With {.IdSeccion = SeccionesClientes.SCS1, .IdCampo = CamposClientes.CA_RAZON_SOCIAL})
            If _lista IsNot Nothing Then

                If _lista.Count > 0 Then

                    fbcrazonsocialcliente.DataSource = _lista

                Else

                    MsgRazonSocialNoExiste()

                End If

            Else

                MsgRazonSocialNoExiste()

            End If

        End If

    End Sub

    Protected Async Sub sctipodocumento_Click(sender As Object, e As EventArgs) Handles sctipodocumento.Click

        _cacheListaDocumentos = New List(Of SelectOption)

        _cacheListaDocumentos = CType(HttpRuntime.Cache("cacheListaDocumentos"), List(Of SelectOption))

        If _cacheListaDocumentos Is Nothing Then

            _controladorExpedienteElectronico = New ControladorExpedienteElectronico()

            Dim resp_ As TagWatcher = Await _controladorExpedienteElectronico.ObtenerCatalogoTipoUso1Api()

            If resp_.Status = TypeStatus.Ok Then

                'Aquí ya NO es String, sino JObject
                Dim json As JObject = CType(resp_.ObjectReturned, JObject)

                Dim documentos As JArray = CType(json("response"), JArray)

                _lista = New List(Of SelectOption)

                For Each doc As JObject In documentos

                    _lista.Add(New SelectOption With {.Value = doc("code"), .Text = $"{doc("nomenclatura")} - {doc("description")} - {doc("extension")}"})

                Next

                _cacheListaDocumentos = _lista

                HttpRuntime.Cache.Insert("cacheListaDocumentos", _cacheListaDocumentos, Nothing, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration)

            End If

        End If

        sctipodocumento.DataSource = _cacheListaDocumentos

    End Sub

    Protected Sub sctipodocumento_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Public Overrides Function AgregarComponentesBloqueadosInicial() As List(Of WebControl)
        Dim bloqueados_ As New List(Of WebControl) From {fbcrazonsocialcliente, sctipodocumento}
        Return bloqueados_
    End Function

    Public Overrides Function AgregarComponentesBloqueadosEdicion() As List(Of WebControl)
        Dim bloqueadosEdicion_ As New List(Of WebControl) From {fbcrazonsocialcliente, sctipodocumento}
        Return bloqueadosEdicion_
    End Function


    Protected Sub fcDocumento_ChooseFile(sender As PropiedadesDocumento, e As EventArgs)

    End Sub

    Protected Sub btnXML_Click(sender As Object, e As EventArgs)

    End Sub

    Protected Sub fcprocesamientofiles_ChooseFile(sender As Object, e As EventArgs)

        Dim id = ObjectId.GenerateNewId().ToString

        _loginUsuario = Session("DatosUsuario")

        With sender

            ._idpropietario = id

            .nombrepropietario = _loginUsuario("Nombre")

            .tipovinculacion = PropiedadesDocumento.TiposVinculacion.AgenciaAduanal

            .datosadicionales = New InformacionDocumento With {
                          .foliodocumento = Nothing,
                          .tipodocumento = InformacionDocumento.TiposDocumento.SinDefinir,
                          .datospropietario = New InformacionPropietario With {
                          .nombrepropietario = _loginUsuario("Nombre"),
                          ._id = id
            }}

            'De momento, en lo que funciona el componente de otra forma :V
            'Ya luego le pones la que corresponda :V
            ' Obtener la extensión (todo después del último espacio o guion)
            If sctipodocumento.Text IsNot Nothing Then

                Dim partes() As String = sctipodocumento.Text.Split("-"c)

                Dim extension As String = partes(partes.Length - 1).Trim()

                If extension = ".pdf" Then

                    .formatoarchivo = PropiedadesDocumento.FormatosArchivo.pdf

                Else

                    .formatoarchivo = PropiedadesDocumento.FormatosArchivo.xml

                End If

            End If

        End With

    End Sub

    Protected Async Sub SubirdocumentosGCS_Click(sender As Object, e As EventArgs) Handles SubirdocumentosGCS.Click
        ''DEBE CAMBIAR EL FRONTEND CON ACCIONES
        'DisplayMessage("👉 Documentos subidos a GCS correctamente", StatusMessage.Success)
        'PreparaBotonera(FormControl.ButtonbarModality.Default)
        'PanelControlesCliente.Enabled = False
        'panelentrada.Visible = False
        'titulodocumentosprocesados.Visible = True
        'PanelProcesarDocumentosCIA.Visible = True

        'PanelProcesamiento.Enabled = False
        'PanelProcesamiento.Visible = False
        'SubirdocumentosGCS.Enabled = False
        'PanelButtonIA.Visible = False

        'lockopen.Visible = False
        'lockclosed.Visible = True

        'panelfolio1.Visible = True
        'panelfolio2.Visible = True
        'panelfolio3.Visible = True

        'dbdocumentosterminados.Visible = True

        'folioprocesamiento.Text = "--- ----"

        'Dim fechaActual As DateTime = DateTime.UtcNow

        'fechaprocesamiento.Text = fechaActual.ToString()

        'IconProcesoIniciado.Visible = True

        ''STEP 1
        ''GENERAR EL PAYLOAD
        '''Obtener el documento subido de mongodb

        'Dim listaDocumentos_ As List(Of Newtonsoft.Json.Linq.JObject) =
        '    Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of Newtonsoft.Json.Linq.JObject))(fcprocesamientofiles.Value)

        'Dim listaIdsDocumento_ As New List(Of ObjectId)

        'If listaDocumentos_ IsNot Nothing Then

        '    Dim totaldocumentossinprocesar_ = listaDocumentos_.Count

        '    lbtotaldocumentoscerrrados.Text = "0"

        '    lbtotaldocumentosabiertos.Text = totaldocumentossinprocesar_

        '    For Each documento_ In listaDocumentos_

        '        'TextLabelArchivo.Text = documento_.Last.First
        '        AgregarDocumentoEnProceso(documento_.Last.First)

        '        listaIdsDocumento_.Add(ObjectId.Parse(documento_.SelectToken("fileId").ToString()))

        '    Next

        '    ''Obtenemos los datos del cliente seleccionado
        '    Dim datosCliente_ = ObtenerDatosClienteSeleccionado(ObjectId.Parse(fbcrazonsocialcliente.Value))

        '    Dim unused = Task.Run(Async Function()
        '                              Try
        '                                  Await AgregarDocumentosGCS(listaIdsDocumento_, datosCliente_)

        '                              Catch ex As Exception
        '                                  ' Loguear error (BD, archivo, etc.)
        '                              End Try
        '                          End Function)



        '    'TerminarTarea()
        '    ' 👉 Programar segundo postback en 2 segundos
        '    'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DispararTerminar",
        '    '    "setTimeout(function(){ __doPostBack('" & btnTerminarTarea.UniqueID & "', ''); }, 10000);", True)
        'End If
    End Sub

    'Protected Async Sub ProcesarConIA_Click(sender As Object, e As EventArgs) Handles ProcesarConIA.Click
    '    '''DEBE CAMBIAR EL FRONTEND CON ACCIONES
    '    'DisplayMessage("Enviando documentos...", StatusMessage.Info)
    '    'PreparaBotonera(FormControl.ButtonbarModality.Default)
    '    'PanelControlesCliente.Enabled = False
    '    'panelentrada.Visible = False
    '    'titulodocumentosprocesados.Visible = True

    '    'PanelProcesamiento.Enabled = False
    '    'PanelProcesamiento.Visible = False
    '    'ProcesarConIA.Enabled = False
    '    'PanelButtonIA.Visible = False

    '    'lockopen.Visible = False
    '    'lockclosed.Visible = True

    '    'panelfolio1.Visible = True
    '    'panelfolio2.Visible = True
    '    'panelfolio3.Visible = True

    '    'dbdocumentosterminados.Visible = True

    '    'folioprocesamiento.Text = "--- ----"

    '    'Dim fechaActual As DateTime = DateTime.Now

    '    'fechaprocesamiento.Text = fechaActual.ToString()

    '    'IconProcesoIniciado.Visible = True

    '    ''STEP 1
    '    ''GENERAR EL PAYLOAD
    '    '''Obtener el documento subido de mongodb

    '    'Dim listaDocumentos_ As List(Of Newtonsoft.Json.Linq.JObject) =
    '    '    Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of Newtonsoft.Json.Linq.JObject))(fcprocesamientofiles.Value)

    '    'Dim listaIdsDocumento_ As New List(Of ObjectId)

    '    'If listaDocumentos_ IsNot Nothing Then

    '    '    Dim totaldocumentossinprocesar_ = listaDocumentos_.Count
    '    '    lbtotaldocumentoscerrrados.Text = "0"
    '    '    lbtotaldocumentosabiertos.Text = totaldocumentossinprocesar_

    '    '    For Each documento_ In listaDocumentos_

    '    '        'TextLabelArchivo.Text = documento_.Last.First
    '    '        AgregarDocumentoEnProceso(documento_.Last.First)

    '    '        listaIdsDocumento_.Add(ObjectId.Parse(documento_.SelectToken("fileId").ToString()))

    '    '    Next

    '    '    ''Obtenemos los datos del cliente seleccionado
    '    '    Dim datosCliente_ = ObtenerDatosClienteSeleccionado(ObjectId.Parse(fbcrazonsocialcliente.Value))

    '    '    ' 2️⃣ Retraso opcional antes de llamar al backend
    '    '    'Await Task.Delay(500) ' Espera 0.5 segundos para que se vea el frontend actualizado

    '    '    'TerminarTarea(listaIdsDocumento_(0), datosCliente_)
    '    '    ' 3️⃣ Llamar al backend
    '    '    ' 2️⃣ Ejecutar backend en otro hilo
    '    '    Dim unused = Task.Run(Async Function()
    '    '                              Try
    '    '                                  Await ProcesarDatosConAPIS(listaIdsDocumento_(0), datosCliente_)

    '    '                              Catch ex As Exception
    '    '                                  ' Loguear error (BD, archivo, etc.)
    '    '                              End Try
    '    '                          End Function)

    '    '    'TerminarTarea()
    '    '    ' 👉 Programar segundo postback en 2 segundos
    '    '    'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DispararTerminar",
    '    '    '    "setTimeout(function(){ __doPostBack('" & btnTerminarTarea.UniqueID & "', ''); }, 10000);", True)
    '    'End If
    'End Sub

    Private Sub AgregarDocumentoTerminado(documento As String, score As String)
        '' Panel principal
        'Dim pnlDoc As New Panel()
        'pnlDoc.CssClass = "row mb-5"
        'pnlDoc.Visible = True

        '' Columna izquierda (Score)
        'Dim divColIzq As New HtmlGenericControl("div")
        'divColIzq.Attributes.Add("class", "col-lg-2 col-md-6 col-6 col-xs-6")
        'divColIzq.Style.Add("border-right", "1px solid #e1e1e1")

        'Dim divInnerIzq As New HtmlGenericControl("div")
        'divInnerIzq.Attributes.Add("class", "d-flex align-items-center justify-content-end")

        'Dim spanScore As New HtmlGenericControl("span")
        'spanScore.Attributes.Add("class", "d-inline-flex align-items-center gap-1 color-green score-value")

        '' Label del Score
        'Dim lblScore As New Label()
        'lblScore.ID = "LabelScore_" & Guid.NewGuid().ToString("N")
        'lblScore.CssClass = "" ' Sin cambiar clases
        'lblScore.Text = score

        '' Span con "Score"
        'Dim spanScoreTxt As New HtmlGenericControl("span")
        'spanScoreTxt.Attributes.Add("class", "label-icon-score")
        'spanScoreTxt.InnerText = "Score"

        '' Agregar al span principal
        'spanScore.Controls.Add(lblScore)
        'spanScore.Controls.Add(spanScoreTxt)

        'divInnerIzq.Controls.Add(spanScore)
        'divColIzq.Controls.Add(divInnerIzq)
        'pnlDoc.Controls.Add(divColIzq)

        '' Columna derecha (Nombre del documento)
        'Dim divColDer As New HtmlGenericControl("div")
        'divColDer.Attributes.Add("class", "col-lg-10 col-md-6 col-6 col-xs-6")

        'Dim lblDocumento As New Label()
        'lblDocumento.ID = "LabelTxtDocumento_" & Guid.NewGuid().ToString("N")
        'lblDocumento.CssClass = "font-weight-bold text-bold-day text-purple-light"
        'lblDocumento.Text = documento

        'divColDer.Controls.Add(lblDocumento)
        'pnlDoc.Controls.Add(divColDer)

        '' Agregar al contenedor existente
        'dbdocumentosterminados.Controls.Add(pnlDoc)
    End Sub


    Private Sub AgregarDocumentoEnProceso(nombreArchivo As String)
        '' Contenedor principal del documento
        'Dim pnlDoc As New Panel()
        'pnlDoc.CssClass = "row mb-5"

        '' Columna del ícono
        'Dim divIcon As New HtmlGenericControl("div")
        'divIcon.Attributes("class") = "col-lg-2 col-md-6 col-6 col-xs-6"
        'divIcon.Style.Add("border-right", "1px solid #e1e1e1")

        'Dim divFlex As New HtmlGenericControl("div")
        'divFlex.Attributes("class") = "d-flex align-items-center justify-content-center"

        'Dim spanIcon As New HtmlGenericControl("span")
        'spanIcon.Attributes("class") = "d-inline-flex align-items-center gap-0"
        'spanIcon.Style.Add("margin", "0")
        'spanIcon.Style.Add("padding", "0")
        'spanIcon.Style.Add("position", "relative")

        '' SVG como string (puedes poner el mismo que tenías)
        'Dim svgHtml As String = "<svg xmlns='http://www.w3.org/2000/svg' height='2.5rem' viewBox='0 -960 960 960' width='2.5rem' fill='#782360'><path d='M151.33-370.67q-46.66 0-79-32.47Q40-435.61 40-482t32.47-78.86q32.47-32.47 78.86-32.47V-702q0-27 19.84-46.83Q191-768.67 218-768.67h150.67q0-46.66 32.47-79Q433.61-880 480-880t78.86 32.47q32.47 32.47 32.47 78.86H742q27 0 46.83 19.84Q808.67-729 808.67-702v108.67q46.66 0 79 32.47Q920-528.39 920-482t-32.47 78.86q-32.47 32.47-78.86 32.47v184q0 27-19.84 46.84Q769-120 742-120H218q-27 0-46.83-19.83-19.84-19.84-19.84-46.84v-184ZM348.82-464q19.51 0 33.01-13.66 13.5-13.65 13.5-33.16 0-19.51-13.65-33.01-13.66-13.5-33.17-13.5t-33.01 13.65Q302-530.02 302-510.51t13.66 33.01q13.65 13.5 33.16 13.5Zm262.67 0q19.51 0 33.01-13.66 13.5-13.65 13.5-33.16 0-19.51-13.66-33.01-13.65-13.5-33.16-13.5-19.51 0-33.01 13.65-13.5 13.66-13.5 33.17t13.65 33.01q13.66 13.5 33.17 13.5ZM348-283.33h264q14.17 0 23.75-9.62t9.58-23.83q0-14.22-9.58-23.72-9.58-9.5-23.75-9.5H348q-14.17 0-23.75 9.62-9.58 9.61-9.58 23.83 0 14.22 9.58 23.72 9.58 9.5 23.75 9.5Z'/></svg>"
        'spanIcon.InnerHtml = svgHtml & "<span class='label-icon'>Cargado</span>"

        'divFlex.Controls.Add(spanIcon)
        'divIcon.Controls.Add(divFlex)
        'pnlDoc.Controls.Add(divIcon)

        '' Columna del nombre de archivo
        'Dim divLabel As New HtmlGenericControl("div")
        'divLabel.Attributes("class") = "col-lg-10 col-md-6 col-6 col-xs-6 pt-4"

        'Dim lbl As New Label()
        'lbl.ID = "lblArchivo_" & Guid.NewGuid().ToString("N")
        'lbl.CssClass = "font-weight-bold text-bold-day"
        'lbl.Text = nombreArchivo

        'divLabel.Controls.Add(lbl)
        'pnlDoc.Controls.Add(divLabel)

        '' Agregar al contenedor principal
        'IconProcesoIniciado.Controls.Add(pnlDoc)
    End Sub


    'Protected Overrides Sub Render(writer As HtmlTextWriter)
    '    ' Registrar el postback del botón oculto para que sea válido
    '    ClientScript.RegisterForEventValidation(btnTerminarTarea.UniqueID)
    '    MyBase.Render(writer)
    'End Sub


    Protected Sub btnTerminarTarea_Click(sender As Object, e As EventArgs)
        TerminarTarea()
    End Sub

    Private Sub TerminarTarea()
        fsDatosGenerales.Enabled = False
        Dim documento_ = "SNTON8390127193 - 3A"
        Dim score_ = "70.1%"
        folioprocesamiento.Text = "FOLIO-XXXX-2025"
        lbtotaldocumentosabiertos.Text = "0"
        lbtotaldocumentoscerrrados.Text = "1"
        IconProcesoIniciado.Visible = False
        IconProcesoTerminado.Visible = True

        AgregarDocumentoTerminado(documento_, score_)

        DisplayMessage("Documentos enviados correctamente", StatusMessage.Success)

        ' JS para animar fade-in de los labels e icono
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "fadeInFinal",
            "e.style.opacity=0; e.style.transition='opacity 0.5s';" &
            "setTimeout(function(){ e.style.opacity=1; }, 50);" &
            "s.style.opacity=0; s.style.transition='opacity 0.5s';" &
            "setTimeout(function(){ s.style.opacity=1; }, 50);", True)

    End Sub

    Private Function GenerarPayloadApiGCS(ByVal iddocumentGCS_ As String,
                                          ByVal documento_base64_ As String,
                                          ByVal datosCliente_ As AuxDatosCliente,
                                          ByVal environment_ As Int16) As DocumentoElectronicoApiStorage
        ''Generamos el payload para la API
        _payloadAPI = New DocumentoElectronicoApiStorage

        _loginUsuario = Session("DatosUsuario")

        Dim tipodocumento_ = sctipodocumento.Value

        Dim tipooperacion_ = IIf(swcesimportacion.Checked, "Importacion", "Exportacion")

        With _payloadAPI
            ._id = iddocumentGCS_
            .file_model_64 = New List(Of FileBase64) From {
                    New FileBase64 With {
                                    .filename_64 = "documento.pdf", 'Deberia ser dinámico que lo entregue el componente
                                    .content_type = "application/pdf", 'Aqui igual, ya que no siempre son pdf
                                    .base64_data = documento_base64_ ' ← Base64 real aquí
                                }
                }
            .customerid = datosCliente_.Customerid
            .customer_name = datosCliente_.Customername
            .taxid = datosCliente_.Taxid
            .environmentid = environment_
            .business_unit = datosCliente_.Businessunit ''esto debe cambiar ya que ahora mismo no los trae el cliente
            .business_unitid = datosCliente_.BusinessUnitid ''no lo trae el cliente
            .section = "Operations" ''No mover
            .use_type = "TipoUso1" ''No mover
            .use_type_description = "Expediente de comercio exterior" ''Se recomienda no mover
            .document_type = tipodocumento_
            .content_type = "application/pdf" ''Esto debe ser dinamico
            .sub_type = "Factura"
            .description = "Factura comercio exterior"
            .file_name_origin = "documento.pdf"
            .owner = New Owner With {
                    .userid = "64e7ad27f544af8dfd407efe", ''Aqui debe ser el id de la session
                    .name = _loginUsuario("Nombre"),
                    .user = _loginUsuario("Correo")
                }
            .content_tag = New List(Of String) From {"Factura comercial importacion"}
            .upload_year = DateTime.Now.Year
            .month_upload = DateTime.Now.Month
            .bucket_type = "Standard" ''No mover
            .type_operation = tipooperacion_
        End With

        Return _payloadAPI

    End Function

    Private Async Function AgregarDocumentosGCS(ByVal listIdsdocumentos As List(Of ObjectId),
                                                ByVal datosCliente_ As AuxDatosCliente) As Task
        ''Convertir el documento subido a base64
        _controladorExpedienteElectronico = New ControladorExpedienteElectronico()

        _listaDocumentosGCS = New List(Of DocumentoElectronicoApiStorage)

        Dim documentos_base64_ As List(Of String) = ControladorExpedienteElectronico.ConvertirArchivosABase64(listIdsdocumentos)

        ''MANDAR A LLAMAR MAS VECES

        Dim environment_ = 1 ''1 = Veracruz | Debe cambiar segun la oficina

        For Each itemDocBase64_ In documentos_base64_

            Dim _iddocumento_GCS = ObjectId.GenerateNewId().ToString

            _listaDocumentosGCS.Add(GenerarPayloadApiGCS(_iddocumento_GCS, itemDocBase64_, datosCliente_, environment_))

        Next

        Dim resp_ As TagWatcher = Await _controladorExpedienteElectronico.SubirDocumentosGCS(_listaDocumentosGCS, datosCliente_.Taxid, environment_)

    End Function

#Region "Métodos privados"
    Private Function ObtenerDatosClienteSeleccionado(ByVal objectIdCliente_ As ObjectId) As AuxDatosCliente

        _auxDatosCliente = New AuxDatosCliente

        _controladorClientes = New ControladorClientes()

        _tagwatcher = New TagWatcher

        Try

            _tagwatcher = _controladorClientes.Consultar(objectIdCliente_)

            If _tagwatcher.Status = TypeStatus.Ok Then

                Dim cliente_ = _tagwatcher.ObjectReturned.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

                With cliente_.Seccion(SeccionesClientes.SCS1)

                    _auxDatosCliente.Customerid = _tagwatcher.ObjectReturned.Id.ToString

                    _auxDatosCliente.Customername = .Campo(CamposClientes.CA_RAZON_SOCIAL).Valor

                    _auxDatosCliente.Businessunit = "01_Maquinaria"

                    _auxDatosCliente.BusinessUnitid = 10454

                    If .Campo(CamposClientes.CA_RFC_CLIENTE).Valor IsNot Nothing Then

                        _auxDatosCliente.Taxid = .Campo(CamposClientes.CA_RFC_CLIENTE).Valor

                    Else

                        _auxDatosCliente.Taxid = .Campo(CamposClientes.CA_TAX_ID).Valor

                    End If

                End With

            End If

        Catch ex As Exception

            _tagwatcher.SetError()

        End Try

        Return _auxDatosCliente

    End Function
#End Region

#Region "Avisos / Tooltips"

    Protected Sub MsgRazonSocialNoExiste()

        With fbcrazonsocialcliente
            .ToolTip = "👉 Razón social no encontrada."
            .ToolTipExpireTime = 6
            .ToolTipStatus = IUIControl.ToolTipTypeStatus.OkInfo
            .ToolTipModality = IUIControl.ToolTipModalities.Ondemand
            .ShowToolTip()
        End With

    End Sub

    Public Shared Function GetJson() As String
        Dim basePath = AppDomain.CurrentDomain.BaseDirectory
        Dim filePath = Path.Combine(basePath, "FrontEnd\Modulos\TraficoAA\ProcesamientoElectronicoDocumentos\dummyCommercialIA.json")

        If Not File.Exists(filePath) Then
            Throw New FileNotFoundException("No se encontró el JSON dummy", filePath)
        End If

        Return File.ReadAllText(filePath)
    End Function

    Public Function LoadCfdiXml() As String

        Dim basePath = AppDomain.CurrentDomain.BaseDirectory
        Dim filePath = Path.Combine(
        basePath,
        "FrontEnd\Modulos\TraficoAA\ProcesamientoElectronicoDocumentos\cdfi_dummy.xml"
        )

        If Not File.Exists(filePath) Then
            Throw New FileNotFoundException("No se encontró el CFDI XML", filePath)
        End If

        ' CFDI viene en UTF-8, mejor explícito
        Return File.ReadAllText(filePath, Encoding.UTF8)

    End Function

    Protected Function EstructuraCommercialInvoiceTemporal() As CommercialInvoiceAnalysis

        Dim factura_ As CommercialInvoiceAnalysis = New CommercialInvoiceAnalysis

        With factura_
            .invoicenumber = "FACTURA-IMPO-2026-A1"
            .invoicedate = "2026-01-07"
            .invoiceseries = ""
            .customername = "INDUSTRIAS MICHELIN S.A. DE C.V."
            .suppliername = "HAINAN HAIZHIWO TECHNOLOGY DEVELOPMENT CO., LTD."
            .invoicecountry = "CHN"
            .totalinvoice = 88480
            .invoicecurrency = "USD"
            .customer = New Customer With {
                .customerid = 0, ''ESTOS DATOS YO CREO QUE HARE LA BUSQUEDA DESDE ANTES PORQUE ESTO SE VA A SELECCIONAR ANTES
                .customername = "INDUSTRIAS MICHELIN S.A. DE C.V.",
                .rfc = "IMI9709082M5",
                .address = "AV. 5 DE FEBRERO, #2113-A, FRACC.INDUSTRIAL BENITO JUAREZ, QUERETARO, QRO.CP.76120 MEXICO",
                .street = "AV. 5 DE FEBRERO, #2113-A, FRACC. INDUSTRIAL BENITO JUAREZ,",
                .externalnumber = "",
                .internalnumber = "",
                .zipcode = "76120",
                .city = "QUERETARO",
                .locality = "",
                .state = "QRO",
                .country = "MEX"
              }
            .supplier = New Supplier With {
                .supplierid = 0,
                .supliername = "HAINAN HAIZHIWO TECHNOLOGY DEVELOPMENT CO., LTD.",
                .taxid = "91460000MA7G4H547Y",
                .address = "ROOM 1001, 3.0RD FLOOR, INCUBATION BUILDING, HAINAN ECO-SOFTWARE PARK, LAOCHENG TOWN, CHENGMAI COUNTY, HAINAN PROVINCE, 571900 CHINA",
                .street = "ROOM 1001, 3RD FLOOR, INCUBATION BUILDING, HAINAN ECO-SOFTWARE PARK,",
                .externalnumber = "",
                .internalnumber = "",
                .zipcode = "571900",
                .locality = "",
                .city = "CHENGMAI COUNTY",
                .state = "HAINAN PROVINCE",
                .country = "CHN"}
            .items = New List(Of Syn.CustomBrokers.Controllers.Item) _
                From {New Syn.CustomBrokers.Controllers.Item With {
                    .sec = 0,
                      .productid = "",
                      .sku = "",
                      .partnumber = "MFD1394675A",
                      .quantity = 56,
                      .unit = "MT",
                      .description = "MFD1394675A MFD CH02413BB SIL160 BIG BAG ZTBMC 56 MT SILICA RHASIL HD165MP",
                      .total = 88480,
                      .currency = "USD",
                      .usdvalue = 88480,
                      .value = 88480,
                      .discount = "0.0",
                      .unitprice = 1580,
                      .netweight = 0,
                      .purchaseorder = "8390138091",
                      .destinationcountry = "MEX",
                      .origincountry = "CHN"
        },
        New Syn.CustomBrokers.Controllers.Item With {
                    .sec = 0,
                      .productid = "",
                      .sku = "",
                      .partnumber = "parte",
                      .quantity = 56,
                      .unit = "MT",
                      .description = "parte",
                      .total = 88480,
                      .currency = "USD",
                      .usdvalue = 88480,
                      .value = 88480,
                      .discount = "0.0",
                      .unitprice = 1580,
                      .netweight = 0,
                      .purchaseorder = "8390138091",
                      .destinationcountry = "MEX",
                      .origincountry = "CHN"
        }}
            .additionaldetails = New AdditionalDetails With {
            .purchaseorder = "8390138091",
            .totalweight = 56,
            .packages = "0",
            .incoterm = "FOB",
            .customerreference = "",
            .incrementalvalues = New List(Of IncrementalValue)
           }
            .consigneedetails = New ConsigneeDetails With {
            .consigneedetailsname = "INDUSTRIAS MICHELIN S.A. DE C.V.",
            .taxid = "IMI9709082M5",
            .address = "AV. 5 DE FEBRERO, #2113-A, FRACC. INDUSTRIAL BENITO JUAREZ, QUERETARO, QRO. CP.76120 MEXICO",
            .street = "AV. 5 DE FEBRERO, #2113-A, FRACC. INDUSTRIAL BENITO JUAREZ,",
            .externalnumber = "",
            .internalnumber = "",
            .zipcode = "76120",
            .locality = "",
            .city = "QUERETARO",
            .state = "QRO",
            .country = "MEX"
            }
            .processdate = "2024-06-18"
            .environmentid = 0
            .confidence = 0.95
            .info = "Factura extraída de AWS Textract. Se identificó un solo ítem de mercancía. El total en palabras y números coincide. Incoterm no identificado con claridad, se deja null."
            .analysis = New Ia.Analysis.Analysis With {
            .processdate = "2024-06-18",
            .environmentid = 0,
            .confidence = 0.95,
            .gptanalysis = True,
            .gpttokensupload = 0,
            .gpttokensdownload = 0,
            .textractanalysis = True,
            .textractpages = 1,
            .quantitydifferences = 0,
            .temperature = 0,
            .messages = New List(Of Ia.Analysis.Messages) From {
             New Ia.Analysis.Messages With {
                .type = "info",
                .action = "extract",
                .field = "quantity",
                .value = "56",
                .message = "Cantidad extraída de la descripción del producto (56 MT).",
                .confidence = 0.95,
                .source = "Details"
              },
                    New Ia.Analysis.Messages With {
                        .type = "info",
                        .action = "extract",
                        .field = "unitprice",
                        .value = "1580.00",
                        .message = "Precio unitario extraído de la columna UNIT PRICE (USD1580.00/MT).",
                        .confidence = 0.95,
                        .source = "Details"
                      },
              New Ia.Analysis.Messages With {
                .type = "info",
                .action = "extract",
                .field = "value",
                .value = "88480.00",
                .message = "Valor total extraído de la columna AMOUNT (USD88480.00).",
                .confidence = 0.95,
                .source = "Details"
              },
              New Ia.Analysis.Messages With {
                .type = "info",
                .action = "extract",
                .field = "invoicecurrency",
                .value = "USD",
                .message = "Moneda extraída de los campos de monto y precio unitario.",
                .confidence = 0.95,
                .source = "Details"
              },
              New Ia.Analysis.Messages With {
                .type = "info",
                .action = "extract",
                .field = "invoicecountry",
               .value = "USA",
                .message = "País de moneda deducido de USD.",
                .confidence = 0.95,
                .source = "Details"
              },
              New Ia.Analysis.Messages With {
                .type = "info",
                .action = "extract",
                .field = "incoterm",
                .value = "null",
                .message = "Incoterm no identificado con claridad en la factura.",
                .confidence = 0.7,
                .source = "Header"
              },
                New Ia.Analysis.Messages With {
                .type = "info",
                .action = "extract",
                .field = "incoterm",
                .value = "null",
                .message = "Incoterm no identificado con claridad en la factura.",
                .confidence = 0.7,
                .source = "Header"
              },
               New Ia.Analysis.Messages With {
                .type = "info",
                .action = "extract",
                .field = "invoicedate",
                .value = "null",
                .message = "invoicedate no identificado con claridad en la factura.",
                .confidence = 0.7,
                .source = "Header"
              },
                  New Ia.Analysis.Messages With {
                .type = "info",
                .action = "extract",
                .field = "invoiceseries",
                .value = "null",
                .message = "invoiceseries no identificado con claridad en la factura.",
                .confidence = 0.7,
                .source = "Header"
              },
               New Ia.Analysis.Messages With {
                .type = "info",
                .action = "extract",
                .field = "totalweight",
                .value = "null",
                .message = "totalweight no identificado con claridad en la factura.",
                .confidence = 0.7,
                .source = "Header"
              },
              New Ia.Analysis.Messages With {
                .type = "alert",
                .action = "review",
                .field = "invoicenumber",
                .value = "25HZWRHA002IM010",
                .message = "Validar no hubo coincidencia",
                .confidence = 0,
                .source = "synapsis"
              }}}
            .score = 88.89

        End With

        Return factura_
    End Function

    Protected Function EstructuraCommercialInvoiceCFDITemporal() As CommercialInvoiceAnalysis

        Dim cfdi_str = LoadCfdiXml()

        _controladorProcesamientoElectronico = New ControladorProcesamientoElectronico(TiposDocumentoElectronico.FacturaComercial,
                                                                                       IControladorProcesamientoElectronico.ListaTiposDocumentos.FACTURA_COMERCIAL_EXPORTACION_CFDI)

        Dim cfdi_deserealizado = _controladorProcesamientoElectronico.DeserializeCFDI(cfdi_str)

        ''REVISALA PORQUE TE FALTA LLENAR MAS CAMPOS, OSEA TODOS LOS DE LA FACTURA
        Return _controladorProcesamientoElectronico.GenerarCommercialInvoiceDesdeCFDI(cfdi_deserealizado)

    End Function

    Protected Sub BtnProcesarDocumentosCIA_Click(sender As Object, e As EventArgs)
        ''AQUI HAREMOS LA SIMULACION DEL PROCESAMIENTO
        Dim idCliente_ = "64e7ad27f544af8dfd407efd"

        ''dummy factura impo
        _controladorProcesamientoElectronico = New ControladorProcesamientoElectronico(TiposDocumentoElectronico.FacturaComercial,
                                                                                       IControladorProcesamientoElectronico.ListaTiposDocumentos.FACTURA_COMERCIAL_IMPORTACION_PDF)
        Dim factura_ As CommercialInvoiceAnalysis = EstructuraCommercialInvoiceTemporal()
        Dim estado_ As TagWatcher = _controladorProcesamientoElectronico.GenerarFacturaComercial(factura_, ObjectId.Parse(idCliente_))

        ''dummy factura expo
        '_controladorProcesamientoElectronico = New ControladorProcesamientoElectronico(TiposDocumentoElectronico.FacturaComercial,
        '                                                                               IControladorProcesamientoElectronico.ListaTiposDocumentos.FACTURA_COMERCIAL_EXPORTACION_CFDI)

        'Dim factura_ As CommercialInvoiceAnalysis = EstructuraCommercialInvoiceCFDITemporal()
        'Dim estado_ As TagWatcher = _controladorProcesamientoElectronico.GenerarFacturaComercial(factura_, ObjectId.Parse(idCliente_))

        If estado_.Status = TypeStatus.Ok Then

            DisplayMessage($"Factura procesada {factura_.invoicenumber}")

        Else

            DisplayMessage($"Factura ya registrada", StatusMessage.Info)

        End If


    End Sub
#End Region

#End Region

#Region "██████ Vinculación sexta capa  █████████       SAX      ████████████████████████████████████████████"
    '    ██████   Controladores utilizados                     Documentos por coding para MongoDB      ██████
    '    ██████    1.ControladorEmpresas                        1. En Empresa:                         ██████
    '    ██████    2.ControladorRecursosAduanales                  a). Domicilios                      ██████
    '    ██████    3.ControladorSecuencias                         b). Contactos                       ██████
    '    ██████                                                                                        ██████
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████
    'Protected Function GenerarSecuencia() As Secuencia
    '    'Dim secuencia_ As New Secuencia
    '    'Dim controladorSecuencias_ As ControladorSecuencia = New ControladorSecuencia
    '    'Dim tagwatcher_ As TagWatcher = controladorSecuencias_.Generar(SecuenciasComercioExterior.ProveedoresOperativos.ToString, 1, 1, 1, 1, 1)
    '    'If tagwatcher_.Status = TypeStatus.Ok Then
    '    '    secuencia_ = DirectCast(tagwatcher_.ObjectReturned, Secuencia)
    '    '    secuencia_.nombre = SecuenciasComercioExterior.ProveedoresOperativos.ToString
    '    'End If
    '    'Return secuencia_
    'End Function




    'Public Overrides Sub BotoneraClicOtros(ByVal IndexSelected_ As Integer)

    '    If IndexSelected_ = 10 Then

    '        Dim pdfBytes_ As Byte() = File.ReadAllBytes("C:\TEMP\Ejemplo_BL.pdf")

    '        Dim ms_ As New MemoryStream(pdfBytes_)

    '    End If

    'End Sub







    'Protected Sub MsgEstadoCompletado()
    '    lbEstadoCompletado.ToolTip = "Estado completado"
    '    lbEstadoCompletado.ToolTipExpireTime = 4
    '    fcRazonSocial.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkInfo
    '    fcRazonSocial.ToolTipModality = IUIControl.ToolTipModalities.Interactive
    '    fcRazonSocial.ShowToolTip()
    'End Sub

#End Region
End Class

<Serializable()>
Public Class FsChuck
    Public Property _id As ObjectId
    <BsonIgnoreIfNull>
    Public Property files_id As ObjectId
    <BsonIgnoreIfNull>
    Public Property n As Int32
    <BsonIgnoreIfNull>
    Public Property data As data

End Class
<Serializable()>
Public Class data
    Public Property binary As Binary

End Class
<Serializable()>
Public Class Binary
    Public Property base64 As String
    Public Property subType As String

End Class

Public Class AuxDatosCliente
    Public Property Customerid As String
    Public Property Customername As String
    Public Property Businessunit As String
    Public Property BusinessUnitid As Int16
    Public Property Taxid As String
End Class