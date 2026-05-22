#Region "├┴┘├┴┘├┴┘├┴┘├┴┘|├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘   DEPENDENCIAS   ├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘"

'RECURSOS DEL CMF
Imports System.Globalization
Imports System.Security.Cryptography.Xml
Imports System.Threading.Tasks
Imports AuxiliarDatosExpedienteElectronico64
Imports ConstructorExpedienteElectronico64.Syn.Documento
Imports Gsol.krom
'OBJETOS BIDIMENSIONALES (ODF's.  Dependencias Krombase/SQL Server)

Imports Gsol.Web.Components
Imports Gsol.Web.Components.FormControl.ButtonbarModality
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Rec.Globals
'OBJETOS DIMENSIONALES (ODS's) Dependencias en MongoDB
Imports Rec.Globals.Controllers

Imports Rec.Globals.Utils
Imports Sax.Web
'UTILERIAS/RECURSOS ADICIONALES
Imports Sax.Web.ControladorBackend.Cookies
Imports Sax.Web.ControladorBackend.Datos
Imports Syn.CustomBrokers.Controllers
Imports Syn.Documento
Imports Syn.Documento.Componentes.Campo
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Operaciones
Imports Syn.Utils
Imports Syn.Utils.Organismo
Imports Wma.Exceptions
Imports Wma.Exceptions.TagWatcher
Imports Wma.Exceptions.TagWatcher.TypeStatus

#End Region

Public Class Ges022_ExpedienteElectronico
    Inherits ControladorBackend

#Region "████████████████████████████████████████   Atributos locales  ██████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████
    Private _totalExpedientes As List(Of AuxiliarDatosExpedienteElectronico)

    Private _tagwatcher As TagWatcher

    Private _controladorExpedienteElectronico As IControladorExpedienteElectronico
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

            .DataObject = New ConstructorExpededienteElectronico(True)

            .addFilter(SeccionesExpedienteElectronico.SEXPE1, CamposExpedienteElectronico.CP_RAZON_SOCIAL_CLIENTE, "Razón social cliente")

        End With
        ''DEBEN SER BUSQUEDAS MÁS ESPECIFICAS, POR EL MOMENTO DEJALO ASI
        _controladorExpedienteElectronico = New ControladorExpedienteElectronico

        _tagwatcher = New TagWatcher

        _tagwatcher = _controladorExpedienteElectronico.ObtenerExpedientes()

        If _tagwatcher.Status = TypeStatus.Ok Then

            Dim listaExpedientesElectronicos_ = _tagwatcher.ObjectReturned

            SetVars("listaExpedientesElectronicos_", _tagwatcher.ObjectReturned)

            label_num_cerrrados.Text = 0

            label_num_abiertos.Text = listaExpedientesElectronicos_.Count()

            label_num_vacios.Text = 0

        End If

        HabilitarControles()

    End Sub



    'ASIGNACION PARA CONTROLES AUTOMÁTICOS
    Public Overrides Function Configuracion() As TagWatcher

        Return New TagWatcher(1)

    End Function

    'EVENTOS PARA LA INSERCIÓN DE DATOS
    Public Overrides Function AntesRealizarInsercion(ByVal session_ As IClientSessionHandle) As TagWatcher

        Dim tagwatcher_ As TagWatcher


        '      ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas con transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒

        If session_ IsNot Nothing Then

            '  ██████inicio███████        Logica de negocios local      ████████████████████████


        Else  '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas sin transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 

            tagwatcher_ = New TagWatcher

            tagwatcher_.SetOK()

        End If

        Return tagwatcher_

    End Function

    Public Overrides Sub RealizarInsercion(ByRef documentoElectronico_ As DocumentoElectronico)

    End Sub

    Public Overrides Function DespuesRealizarInsercion() As TagWatcher

        Return New TagWatcher(Ok)

    End Function

    'EVENTOS PARA MODIFICACIÓN DE DATOS
    Public Overrides Function AntesRealizarModificacion(ByVal session_ As IClientSessionHandle) As TagWatcher

        Dim tagwatcher_ As TagWatcher

        '     ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas con transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒

        If session_ IsNot Nothing Then

            '  ██████inicio███████        Logica de negocios local      ████████████████████████

            '  ████████fin█████████        Logica de negocios local      ███████████████████████

        Else  '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas sin transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 

            tagwatcher_ = New TagWatcher

            tagwatcher_.SetOK()

        End If

        Return tagwatcher_

    End Function


    Public Overrides Function DespuesRealizarModificacion() As TagWatcher

        Return New TagWatcher(Ok)

    End Function

    'EVENTOS PARA PRESENTACIÓN DE DATOS EN FRONTEND
    Public Overrides Sub PreparaModificacion(ByRef documentoElectronico_ As DocumentoElectronico)

        With documentoElectronico_

        End With

    End Sub

    'EVENTO PARA CONFIRMACION DE BORRADO DE DOCUMENTO
    Private Sub ProcesarBorrado()

        'DisplayAlert("Eliminar Documento", "¿Esta seguro(a) que desea eliminar este documento?", "__dDocument", "Continuar", "Cancelar")

    End Sub

    'EVENTO PARA BORRAR UN DOCUMENTO
    Public Overrides Sub AceptaConfirmacionDialogo(argument_ As String)

    End Sub

    'EVENTOS DE BÚSQUEDA GENREAL
    Public Overridable Sub BusquedaGeneral(ByVal sender As Object, ByVal e As EventArgs)

        'Dim idcliente_ = ObjectId.Parse(sender.Value)

        ''ESTE VALOR DEBE SER DINÁMICO, TIENES QUE IR A BUSCAR EL OBJETID REAL DEL CLIENTE, YA QUE ESTA BUSQUEDA
        ''TOMA EL OBJECT ID PERO DEL EXPEDIENTE ELECTRONICO, NO DEL CLIENTE
        Dim idcliente_ = ObjectId.Parse("67770aded07ff673b3cf5579")

        HabilitarControles()

        ''VAMOS A BUSCAR TODOS LOS EXPEDIENTES DEL CLIENTE

        _controladorExpedienteElectronico = New ControladorExpedienteElectronico

        _tagwatcher = New TagWatcher
        Dim entorno_ = IControladorExpedienteElectronico.EnvironmentsExpediente.Veracruz ''Debe tomarlo de la sesion
        _tagwatcher = _controladorExpedienteElectronico.ObtenerExpedientesPorCliente(idcliente_, entorno_)

        If _tagwatcher.Status = TypeStatus.Ok Then

            panelentrada.Visible = False

            panelresultadoscliente.Visible = True

            Dim listaExpedientes_ = DirectCast(_tagwatcher.ObjectReturned, List(Of AuxiliarDatosExpedienteElectronico))

            label_num_activos.Text = listaExpedientes_.Count
            Dim i = 0
            For Each expediente_ In listaExpedientes_

                ReferenciaDinamica("RKU25-0235", expediente_.razonsocialCliente, expediente_.owner_name, "1d")

            Next

        End If

    End Sub

    Private Function FormatearTexto(texto As String) As String
        If texto.Contains("-") Then
            Dim partes() As String = texto.Split("-"c)
            ' partes(0) = "RKU25"
            ' partes(1) = "02354"

            Return partes(0) & "-<span style='font-weight:bold;'>" & partes(1) & "</span>"
        Else
            Return texto
        End If
    End Function

    Private Sub ReferenciaDinamica(referencia As String, cliente As String, owner_name As String, fecha As String)
        ' --- Contenedor principal ---
        Dim divCol As New HtmlGenericControl("div")
        divCol.Attributes("class") = "col-lg-12 col-md-12 col-12"

        Dim divNumberContainer As New HtmlGenericControl("div")
        divNumberContainer.Attributes("class") = "number-container"

        Dim divStatLabel As New HtmlGenericControl("div")
        divStatLabel.Attributes("class") = "stat-label text-secondary d-flex gap-2 align-items-center mt-3"

        Dim referenciaActiva_ = IIf(referencia IsNot Nothing, FormatearTexto(referencia), "Sin referencia")

        ' --- Labels ASP.NET ---
        Dim lblReferencia As New Label()
        lblReferencia.ID = "referenciaExpediente"
        lblReferencia.CssClass = "d-inline text-purple-light"
        lblReferencia.Text = $"{referenciaActiva_}"

        Dim lblCliente As New Label()
        lblCliente.ID = "clienteownerExpediente"
        lblCliente.CssClass = "d-inline text-cursive"
        lblCliente.Text = $"&nbsp;&nbsp;{cliente} | {owner_name}  "

        Dim colorActive = IIf(referencia IsNot Nothing, "#782360", "#E3E3E3")

        Dim lblEnlace As New Label()
        lblEnlace.ID = "enlacereferenciaExpediente"
        lblEnlace.CssClass = "d-inline"
        lblEnlace.Text = $"&nbsp;&nbsp;<svg xmlns='http://www.w3.org/2000/svg' height='3rem' viewBox='0 -960 960 960' width='3rem' fill='{colorActive}'><path d='M260-160q-91 0-155.5-63T40-377q0-78 47-139t123-78q23-81 85.5-136T440-797v323l-64-62-56 56 160 160 160-160-56-56-64 62v-323q103 14 171.5 92.5T760-520q69 8 114.5 59.5T920-340q0 75-52.5 127.5T740-160H260Z'/></svg>"

        Dim lblFecha As New Label()
        lblFecha.ID = "fechaapertura"
        lblFecha.CssClass = "d-inline font-weight-bold text-bold-day"
        lblFecha.Text = $"&nbsp;&nbsp;{fecha}"

        ' --- Armar jerarquía ---
        divStatLabel.Controls.Add(lblReferencia)
        divStatLabel.Controls.Add(lblCliente)
        divStatLabel.Controls.Add(lblEnlace)
        divStatLabel.Controls.Add(lblFecha)

        divNumberContainer.Controls.Add(divStatLabel)
        divCol.Controls.Add(divNumberContainer)

        ' Agregar al panel ASP.NET
        PanelExpedientes.Controls.Add(divCol)
    End Sub

    Public Overrides Sub DespuesBuquedaGeneralConDatos()

    End Sub

    Public Overrides Sub DespuesBuquedaGeneralSinDatos()

        HabilitarControles()

    End Sub

    Private Sub HabilitarControles()
        Recientes.Enabled = True
        GeneralesExpedientes.Enabled = True
        BtnBuscarExpediente.Enabled = True
        BtnVer10mas.Enabled = True
        BtnDescargaMasiva.Enabled = True
        BtnVerMasSinReferencia.Enabled = True
        BtnVerMasVencidos.Enabled = True
        BtnVerMasActivos.Enabled = True
        BtnVerMasVacios.Enabled = True
        swcexpedientespropios.Enabled = True
        swcexpedientesabiertos.Enabled = True
    End Sub

    'EVENTOS DE MANTENIMIENTO
    Public Overrides Sub LimpiaSesion()

        SetVars("isEditing", Nothing)
        Statements.ObjectSession = Nothing

    End Sub

    Public Overrides Sub Limpiar()

    End Sub

#End Region

#Region "████████████████  QUINTA CAPA  █████████       Reglas locales         ██████████████████████████████"
    '       * Aquí se pueden colocar los eventos de los componentes, funciones o metodos exclusios del modulo
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    ' Evento para marcar el modulo como pagina de inicio
    Protected Async Sub BtnLocalizar_Click(sender As Object, e As EventArgs) Handles BtnLocalizar.Click
        BtnLocalizar.Enabled = True

        ''ESTA FUNCION REDIBUJA TODA LA BUSQUEDA ANTERIOR, YA QUE SINO EL POST DE BOTON LA ELIMINA
        ''DEBE CAMBIAR DINAMICAMENTE
        ReferenciaDinamica("RKU25-0235", "COLGATE PALMOLIVE S.A. DE C.V.", "CLAUDIA VARGAS", "1d")
        DownloadPackage.Visible = True

        ''AQUI HAREMOS EL LLAMADO A LA API PARA BUSCAR LA REFERENCIA - SOLO HARE UN DUMMY --
        Dim referencia_ = dbcreferenciaexpediente.Value
        Dim idcliente_ = ObjectId.Parse("67770aded07ff673b3cf5579")
        Dim tipouso_ = sctipousoexpediente.Value
        Await BuscarPaqueteGCS(referencia_, idcliente_, tipouso_)

    End Sub

    Private Async Function BuscarPaqueteGCS(ByVal referencia_ As String,
                                            ByVal idcliente_ As ObjectId,
                                            ByVal tipouso_ As String) As Task
        ''Convertir el documento subido a base64
        _controladorExpedienteElectronico = New ControladorExpedienteElectronico()

        '_listaDocumentosGCS = New List(Of DocumentoElectronicoApiStorage)

        ' Dim documentos_base64_ As List(Of String) = ControladorExpedienteElectronico.ConvertirArchivosABase64(listIdsdocumentos)

        ''MANDAR A LLAMAR MAS VECES

        Dim environment_ = 1 ''1 = Veracruz | Debe cambiar segun la oficina

        'For Each itemDocBase64_ In documentos_base64_

        '    Dim _iddocumento_GCS = ObjectId.GenerateNewId().ToString

        '    ' _listaDocumentosGCS.Add(GenerarPayloadApiGCS(_iddocumento_GCS, itemDocBase64_, datosCliente_, environment_))

        'Next

        ' Dim resp_ As TagWatcher = Await _controladorExpedienteElectronico.SubirDocumentosGCS(_listaDocumentosGCS, datosCliente_.Taxid, environment_)

    End Function


    Protected Sub BtnBuscarExpediente_Click(sender As Object, e As EventArgs)
        panelresultadoscliente.Visible = True
        DescargarUnExpediente.Visible = True
        BtnLocalizar.Enabled = True
        'dbcreferenciaexpediente.LabelDetail = "RKU25-0235"
        'dbcreferenciaexpediente.Value = "RKU25-0235"
        dbcreferenciaexpediente.Enabled = True
        dbcreferenciaexpediente.ValueDetail = "COLGATE PALMOLIVE S.A. DE C.V."
        sctipousoexpediente.Enabled = True
        Dim lista As New List(Of SelectOption)

        ReferenciaDinamica("RKU25-0235", "COLGATE PALMOLIVE S.A. DE C.V.", "CLAUDIA VARGAS", "1d")

        lista.Add(New SelectOption _
                               With {
                                    .Value = "TipoUso2",
                                    .Text = "TipoUso2"})

        sctipousoexpediente.DataSource = lista

        sctipousoexpediente.Value = "TipoUso2"

    End Sub

    Protected Sub sctipousoexpediente_Click()
        panelresultadoscliente.Visible = True
        ReferenciaDinamica("RKU25-0235", "COLGATE PALMOLIVE S.A. DE C.V.", "CLAUDIA VARGAS", "1d")

    End Sub

    Protected Sub sctipousoexpediente_TextChanged()
        panelresultadoscliente.Visible = True
        ReferenciaDinamica("RKU25-0235", "COLGATE PALMOLIVE S.A. DE C.V.", "CLAUDIA VARGAS", "1d")

    End Sub

    Protected Sub sctipousoexpediente_SelectedIndexChanged()
        panelresultadoscliente.Visible = True
        ReferenciaDinamica("RKU25-0235", "COLGATE PALMOLIVE S.A. DE C.V.", "CLAUDIA VARGAS", "1d")
    End Sub

    Protected Sub DescargarExpediente_Click(sender As Object, e As EventArgs)
        ReferenciaDinamica("RKU25-0235", "COLGATE PALMOLIVE S.A. DE C.V.", "CLAUDIA VARGAS", "1d")
        BtnLocalizar.Enabled = True
    End Sub







#End Region

#Region "██████ Vinculación sexta capa  █████████       SAX      ████████████████████████████████████████████"
    '    ██████   Controladores utilizados                     Documentos por coding para MongoDB      ██████
    '    ██████    1.ControladorEmpresas                        1. En Empresa:                         ██████
    '    ██████    2.ControladorRecursosAduanales                  a). Domicilios                      ██████
    '    ██████    3.ControladorSecuencias                         b). Contactos                       ██████
    '    ██████                                                                                        ██████
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

#End Region

End Class


Public Class Documento
    Public Property Codigo As String
    Public Property Cliente As String
    Public Property Icono As String
    Public Property Dias As String
End Class