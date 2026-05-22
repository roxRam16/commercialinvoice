
#Region "├┴┘├┴┘├┴┘├┴┘├┴┘|├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘   DEPENDENCIAS   ├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘"

'RECURSOS DEL CMF
Imports Syn.Documento
Imports MongoDB.Driver
Imports Wma.Exceptions
Imports Gsol.Web.Components
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior.CamposProveedorOperativo
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
Imports Syn.Documento.Componentes.Campo
Imports Gsol
'Imports System.ServiceModel.Channels
Imports Gsol.krom
Imports MongoDB.Bson
'Imports System.Windows.Forms.VisualStyles
Imports Syn.Documento.Componentes
Imports Syn.Nucleo
Imports Syn.Custombrokers.Controllers
Imports Rec.Globals.Empresas
Imports Rec.Globals.Controllers.Empresas
'Imports Rec.Globals.Utils.Secuencias
Imports System.Linq.Expressions
Imports Rec.Globals.Controllers
'Imports SharpCompress.Archives
Imports System.Web.UI.WebControls.Expressions
Imports System.Xml.Serialization
Imports Syn.Utils
#End Region

Public Class Ges022_001_ProveedorNacional
    Inherits ControladorBackend
#Region "████████████████████████████████████████   Atributos locales  ██████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    Private _cantidadDetalles As Int32

    Private _lista As List(Of SelectOption)

    Private _tagwatcher As TagWatcher

    Private _espacioTrabajo As IEspacioTrabajo

    Private _empresaNacional As EmpresaNacional

    Private _controladorEmpresas As IControladorEmpresas

    Private _ultimoDomicilio As List(Of Rec.Globals.Empresas.Domicilio)

    Private _opcionesLista As List(Of SelectOption)

    Private _pillboxControl As PillboxControl

    Private _secuencia As ISecuencia

    Private _controladorSecuencias As IControladorSecuencia

    Private _listaDomicilios As List(Of Rec.Globals.Empresas.Domicilio)

    Private _listahistorialDomicilios As List(Of HistorialDomicilios)

    Private _utils As UtileriaProveedores

    Private _datosAdicionalesActuales As List(Of Dictionary(Of String, String))

    Private _auxiliarProveedor As AuxiliarProveedor

    Private _empresa As IEmpresa

    Private _loginUsuario As Dictionary(Of String, String)

    Private _datospaismexicano As PaisDomicilio

#End Region

#Region "████████████████████████████████████████   Constructores  ██████████████████████████████████████████"
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

            .DataObject = New ConstructorProveedoresOperativos(True)

            .addFilter(SeccionesProvedorOperativo.SPRO1, CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, "Proveedor")

            .addFilter(SeccionesProvedorOperativo.SPRO2, CamposProveedorOperativo.CA_RFC_PROVEEDOR, "RFC")

        End With

        If OperacionGenerica IsNot Nothing Then

            _cantidadDetalles = OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Seccion(SeccionesProvedorOperativo.SPRO2).CantidadPartidas

        End If

        scMetodoValoracion.DataEntity = New krom.Anexo22()

        scIncoterm.DataEntity = New krom.Anexo22()

        icClave.Text = ""

        swcTipoPersona.Checked = True

        _utils = New UtileriaProveedores

    End Sub

    'ASIGNACION PARA CONTROLES AUTOMÁTICOS
    Public Overrides Function Configuracion() As TagWatcher

        Dim modoEditando_ As Boolean = False

        If GetVars("isEditing") IsNot Nothing Then

            If GetVars("isEditing") = True Then

                modoEditando_ = True

            End If

        End If
        'Datos generales (SeccionesProveedorOperativo.SPRO1)

        [Set](fcRazonSocial, CA_RAZON_SOCIAL_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Text)

        [Set](icRFC, CA_RFC_PROVEEDOR)

        [Set](icCveRfc, CA_CVE_RFC_PROVEEDOR)

        [Set](swcTipoPersona, CamposProveedorOperativo.CP_TIPO_PERSONA_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Checked)

        [Set](swcHabilitadoProveedor, CamposProveedorOperativo.CP_PROVEEDOR_HABILITADO, propiedadDelControl_:=PropiedadesControl.Checked)

        If swcTipoPersona.Checked = False Then

            [Set](icCURP, CA_CURP_PROVEEDOR)

            [Set](icCveCurp, CA_CVE_CURP_PROVEEDOR)

        End If

        'Detalle proveedor
        If pbDetalleProveedor.PageIndex > 0 Then

            'lbNumero.Text = pbDetalleProveedor.PageIndex.ToString()

        End If

        [Set](icIdDomicilio, CamposProveedorOperativo.CP_ID_DOMICILIO_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icSecDomicilio, CamposProveedorOperativo.CP_SEC_DOMICILIO_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icIdTarjeta, CamposProveedorOperativo.CP_ID_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icFirmaTarjeta, CamposProveedorOperativo.CP_FIRMA_ELECTRONICA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icIdPais, CamposDomicilio.CA_ID_PAIS, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icCvePais, CamposDomicilio.CA_CVE_PAIS, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icPais, CamposDomicilio.CA_PAIS, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scDomicilio, CamposProveedorOperativo.CA_DOMICILIO_FISCAL, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icCalle, CamposDomicilio.CA_CALLE, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icNumeroExterior, CamposDomicilio.CA_NUMERO_EXTERIOR, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icNumeroInterior, CamposDomicilio.CA_NUMERO_INTERIOR, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icNumeroExtInt, CamposDomicilio.CA_NUMERO_EXT_INT, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icCodigoPostal, CamposDomicilio.CA_CODIGO_POSTAL, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icColonia, CamposDomicilio.CA_COLONIA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icLocalidad, CamposDomicilio.CA_LOCALIDAD, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icCiudad, CamposDomicilio.CA_CIUDAD, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icCveMunicipio, CamposDomicilio.CA_ENTIDAD_MUNICIPIO, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icMunicipio, CamposDomicilio.CA_MUNICIPIO, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icCveEntidadFederativa, CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icEntidadFederativa, CamposDomicilio.CA_ENTIDAD_FEDERATIVA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icestadoproveedor, CamposProveedorOperativo.CA_ESTADO_DOMICILIO_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icdomicilioarchivadoproveedor, CamposProveedorOperativo.CA_DOMICILIO_ARCHIVADO_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icmotivoarchivadoproveedor, CamposProveedorOperativo.CA_MOTIVO_ARCHIVADO_DOMICILIO_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](fechaarchivadoproveedor, CamposProveedorOperativo.CA_FECHA_ARCHIVADO_DOMICILIO_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](pbDetalleProveedor, Nothing, seccion_:=SeccionesProvedorOperativo.SPRO2)

        ' Vinculaciones con clientes (SeccionesProvedorOperativo.SPRO4)
        [Set](scClienteVinculacion, CP_ID_CLIENTE_VINCULACION, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scTaxIdVinculacion, CP_RFC_PROVEEDOR_VINCULACION, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scVinculacion, CA_CVE_VINCULACION, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icPorcentajeVinculacion, CP_PORCENTAJE_VINCULACION, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](ccVinculaciones, Nothing, seccion_:=SeccionesProvedorOperativo.SPRO4)

        'Configuración adicional (SeccionesProvedorOperativo.SPRO5)
        [Set](scClienteConfiguracion, CP_ID_CLIENTE_CONFIGURACION, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scTaxIdConfiguracion, CP_RFC_PROVEEDOR_CONFIGURACION, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scMetodoValoracion, CA_CVE_METODO_VALORACION, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scIncoterm, CA_CVE_INCOTERM, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](ccConfiguracionAdicional, Nothing, seccion_:=SeccionesProvedorOperativo.SPRO5)

        If modoEditando_ Then

            EstadoConexion()

        End If

        Return New TagWatcher(1)

    End Function

    Public Overrides Sub BotoneraClicNuevo()

        If OperacionGenerica IsNot Nothing Then

        End If

        LimpiaSesion()

        If pbDetalleProveedor.PageIndex > 0 Then

            'lbNumero.Text = pbDetalleProveedor.PageIndex.ToString()

        End If

        PreparaTarjetero(PillboxControl.ToolbarModality.Default, pbDetalleProveedor)

        swcTipoPersona.Checked = True

        fsVinculaciones.Visible = False

        fsConfiguracionAdicional.Visible = False

        fsHistorialDomicilios.Visible = False

        ''PONER EN LOS INPUTS LOS DATOS DEL PAIS
        Dim pais_ = _utils.ObtenerDatosPaisMexicano()

        icPais.Value = pais_.paisPresentacion

        icIdPais.Value = pais_.idpais.ToString

        icCvePais.Value = pais_.pais

        icPais.Enabled = False

        pbDetalleProveedor.Enabled = True

        swcHabilitadoProveedor.Checked = False

    End Sub

    Public Overrides Sub BotoneraClicGuardar()

        Dim modoEditando_ As Boolean = False

        If GetVars("isEditing") IsNot Nothing Then

            If GetVars("isEditing") = True Then

                modoEditando_ = True

            End If

        End If

        If modoEditando_ = False Then

            If fcRazonSocial.Text = "" Then

                MsgValidacionRazonsocialVacio()

            ElseIf icRFC.Value = "" Then

                MsgValidacionRfcVacio()

            ElseIf icCalle.Value = "" Then

                MsgValidacionCalleVacio()

            ElseIf icPais.Value = "" Then

                MsgValidacionPaisVacio()

            Else

                If BuscarSiExisterProveedor() Then

                    aviso.Visible = True

                Else

                    If Not ProcesarTransaccion(Of ConstructorProveedoresOperativos)().Status = TypeStatus.Errors Then : End If

                End If

            End If

        Else

            If Not ProcesarTransaccion(Of ConstructorProveedoresOperativos)().Status = TypeStatus.Errors Then : End If

        End If

    End Sub

    Public Overrides Sub BotoneraClicEditar()

        PreparaTarjetero(PillboxControl.ToolbarModality.Advanced, pbDetalleProveedor)

        fcRazonSocial.Enabled = False

        icPais.Enabled = False

        swcHabilitadoProveedor.Visible = True

        swcTipoPersona.Enabled = False

        icRFC.Enabled = False

        SetVars("rfcProveedorActual", icRFC.Value)

    End Sub

    Public Overrides Sub BotoneraClicBorrar()

    End Sub

    Public Overrides Sub BotoneraClicOtros(IndexSelected_ As Integer)
        Dim indice_ As Integer = 0
        If GetVars("_indice") IsNot Nothing Then
            indice_ = Int(GetVars("_indice"))
        End If
        If IndexSelected_ = 7 Then
            ConfigurarDomicilios.Visible = True
            ConfigurarDomicilios.Enabled = True
            VaciarFormulario(indice_)
        ElseIf IndexSelected_ = 8 Then
            VaciarFormulario(indice_)
        End If
    End Sub

    Private Sub VaciarFormulario(ByVal indice_ As Integer)
        Dim pillboxControl_ As PillboxControl = pbDetalleProveedor
        pillboxControl_.SetPillbox(Sub(pillbox_ As PillBox)
                                       pillbox_.SetIndice(pillboxControl_.KeyField, indice_)
                                       pillbox_.SetFiled(False)
                                       If GetVars("isEditing") Is Nothing Then
                                           icRFC.Value = Nothing
                                           icCURP.Value = Nothing
                                           swcTipoPersona.Checked = True

                                           icCveRfc.Value = Nothing
                                           icCveCurp.Value = Nothing
                                           swcTipoPersona.Checked = True

                                       Else
                                           swcTipoPersona.Checked = pbDetalleProveedor.DataSource(0).Item("swcTipoPersona")

                                       End If
                                       icCalle.Value = Nothing
                                       icNumeroExterior.Value = Nothing
                                       icNumeroInterior.Value = Nothing
                                       icCodigoPostal.Value = Nothing
                                       icColonia.Value = Nothing
                                       icLocalidad.Value = Nothing
                                       icCiudad.Value = Nothing
                                       icMunicipio.Value = Nothing
                                       icEntidadFederativa.Value = Nothing
                                       icIdDomicilio.Value = Nothing
                                       icSecDomicilio.Value = Nothing
                                       scDomicilio.Value = Nothing
                                       icNumeroExtInt.Value = Nothing
                                       icCveMunicipio.Value = Nothing
                                       icCveEntidadFederativa.Value = Nothing
                                   End Sub)
    End Sub

    'EVENTOS PARA LA INSERCIÓN DE DATOS
    Public Overrides Function AntesRealizarInsercion(ByVal session_ As IClientSessionHandle) As TagWatcher

        Dim tagwatcher_ As New TagWatcher

        If session_ IsNot Nothing Then

            If fcRazonSocial.Value <> "" Then

                GuardarEmpresaNacional(session_)

            Else

                GuardarEmpresaNacional(session_, esempresanueva_:=True)

            End If

            tagwatcher_.SetOK()

        Else

            tagwatcher_.SetOK()

        End If

        Return tagwatcher_

    End Function

    Public Overrides Sub RealizarInsercion(ByRef documentoElectronico_ As DocumentoElectronico)

        _empresaNacional = New EmpresaNacional

        If GetVars("_empresaNacional") IsNot Nothing Then

            _empresaNacional = DirectCast(GetVars("_empresaNacional"), EmpresaNacional)

        End If

        _loginUsuario = New Dictionary(Of String, String)

        _loginUsuario = Session("DatosUsuario")

        _secuencia = New Secuencia

        _secuencia = _utils.GenerarSecuencia(SecuenciasComercioExterior.ProveedoresOperativos)

        With documentoElectronico_

            .Id = _secuencia._id.ToString

            .Campo(CamposProveedorOperativo.CP_ID_PROVEEDOR).Valor = _secuencia._id

            icClave.Text = _secuencia.sec

            .Campo(CamposProveedorOperativo.CP_ID_EMPRESA).Valor = _empresaNacional._id

            .Campo(CamposProveedorOperativo.CP_CVE_EMPRESA).Valor = _empresaNacional._idempresa

            .Campo(CamposProveedorOperativo.CP_CVE_PROVEEDOR).Valor = _secuencia.sec

            .Campo(CamposProveedorOperativo.CP_TIPO_PROVEEDOR).Valor = True

            .Campo(CamposProveedorOperativo.CP_TIPO_PROVEEDOR).ValorPresentacion = "PROVEEDOR NACIONAL"

            .Campo(CamposProveedorOperativo.CP_PROVEEDOR_HABILITADO).Valor = False

            .Campo(CamposProveedorOperativo.CP_PROVEEDOR_HABILITADO).ValorPresentacion = "Offline"

            If swcTipoPersona.Checked Then

                .Campo(CamposProveedorOperativo.CP_TIPO_PERSONA_PROVEEDOR).Valor = True

                .Campo(CamposProveedorOperativo.CP_TIPO_PERSONA_PROVEEDOR).ValorPresentacion = "MORAL"

                .Campo(CamposProveedorOperativo.CA_CVE_RFC_PROVEEDOR).Valor = _empresaNacional._idrfc.ToString

            Else
                .Campo(CamposProveedorOperativo.CP_TIPO_PERSONA_PROVEEDOR).Valor = False

                .Campo(CamposProveedorOperativo.CP_TIPO_PERSONA_PROVEEDOR).ValorPresentacion = "FÍSICA"

                .Campo(CamposProveedorOperativo.CA_CVE_CURP_PROVEEDOR).Valor = _empresaNacional._idcurp.ToString

            End If

            .UsuarioGenerador = _loginUsuario("Nombre")

            .IdDocumento = _secuencia.sec

            .FolioDocumento = _secuencia.sec 'DUDA

            .FolioOperacion = _secuencia.sec 'DUDA

            .TipoPropietario = _secuencia.nombre

            .NombrePropietario = _empresaNacional.razonsocial

            .IdPropietario = _empresaNacional._idempresa

            .ObjectIdPropietario = _empresaNacional._id

        End With

        Dim controladorFirma_ As New ControladorFirmaElectronica

        pbDetalleProveedor.ForEach(Sub(pillbox_ As PillBox)
                                       Dim id_ = ObjectId.GenerateNewId()
                                       pbDetalleProveedor.setValueInvisible(icIdTarjeta, pillbox_.GetIdentity, id_)
                                       pbDetalleProveedor.setValueInvisible(icFirmaTarjeta, pillbox_.GetIdentity, controladorFirma_.Generar(id_, 1))
                                   End Sub)

    End Sub

    Public Overrides Function DespuesRealizarInsercion() As TagWatcher

        RegresarControlesPorDefault()

        ConfigurarDomicilios.Visible = False

        swcHabilitadoProveedor.Visible = True

        swcTipoPersona.Enabled = False

        icRFC.Enabled = False

        Return New TagWatcher(Ok)

    End Function

    'EVENTOS PARA MODIFICACIÓN DE DATOS
    Public Overrides Function AntesRealizarModificacion(ByVal session_ As IClientSessionHandle) As TagWatcher

        Dim tagwatcher_ As New TagWatcher

        If session_ IsNot Nothing Then '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas con transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 

            GuardarEmpresaNacional(session_)

            tagwatcher_.SetOK()

        Else  '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas sin transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 

            tagwatcher_.SetOK()

        End If

        Return tagwatcher_

    End Function

    Public Overrides Sub PreparaModificacion(ByRef documentoElectronico_ As DocumentoElectronico)

        Dim idempresa_ As ObjectId = Nothing

        If documentoElectronico_ IsNot Nothing Then

            idempresa_ = documentoElectronico_.Seccion(SeccionesProvedorOperativo.SPRO1).Campo(CamposProveedorOperativo.CP_ID_EMPRESA).Valor

            _cantidadDetalles = _cantidadDetalles = documentoElectronico_.Seccion(SeccionesProvedorOperativo.SPRO2).CantidadPartidas

        End If

        ''BUSCAR LA EMPRESA ACTUAL
        _tagwatcher = New TagWatcher

        _empresaNacional = New EmpresaNacional

        Try

            _tagwatcher = _utils.BuscarEmpresaPorObjectId(idempresa_.ToString, IControladorEmpresas.TiposEmpresas.Nacional)

            If _tagwatcher.Status = TypeStatus.Ok Then

                _empresaNacional = DirectCast(_tagwatcher.ObjectReturned, EmpresaNacional)

                SetVars("_empresaNacional", _empresaNacional)

                Dim listaempresasnacionales_ As New List(Of EmpresaNacional) From {_empresaNacional}

                SetVars("_listaempresastemporales", listaempresasnacionales_)

                Dim pais_ = _utils.ObtenerDatosPaisMexicano()

                Dim data_ = _utils.ListaDomiciliosPorPais(_empresaNacional._id.ToString, pais_.idpais.ToString, IControladorEmpresas.TiposEmpresas.Nacional)

                SetVars("_listaDomiciliosNacionales", data_)

            End If

            CargarHistorialDomicilios()

            swcHabilitadoProveedor.Visible = True

            ConfigurarDomicilios.Enabled = False

            ConfigurarDomicilios.Visible = False

            swcTipoPersona.Enabled = False

            fcRazonSocial.Enabled = False

            icRFC.Enabled = False

        Catch ex As Exception

            DisplayMessage("Favor de intentarlo más tarde", StatusMessage.Fail)

        End Try

    End Sub

    Public Overrides Sub RealizarModificacion(ByRef documentoElectronico_ As DocumentoElectronico)

        Dim controladorFirma_ As New ControladorFirmaElectronica

        Dim documento_ = documentoElectronico_

        pbDetalleProveedor.ForEach(Sub(pillbox_ As PillBox)

                                       Dim cambios_ = False

                                       If icIdTarjeta.Value.Count <= 0 Then

                                           Dim id_ = ObjectId.GenerateNewId()

                                           pbDetalleProveedor.setValueInvisible(icIdTarjeta, pillbox_.GetIdentity, id_)

                                           pbDetalleProveedor.setValueInvisible(icFirmaTarjeta, pillbox_.GetIdentity, controladorFirma_.Generar(id_, 1))

                                       End If

                                       validaCambiosTarjeta(pillbox_, documento_, cambios_)

                                       If cambios_ Then

                                           pbDetalleProveedor.setValueInvisible(icFirmaTarjeta, pillbox_.GetIdentity, controladorFirma_.Generar(ObjectId.Parse(pillbox_.GetControlValue(icIdTarjeta)), 1))

                                       End If

                                   End Sub)


    End Sub

    Public Overrides Sub DespuesOperadorDatosProcesar(ByRef documentoElectronico_ As DocumentoElectronico)

        With documentoElectronico_

            Dim modoEditando_ As Boolean = False

            If GetVars("isEditing") IsNot Nothing Then

                If GetVars("isEditing") = True Then

                    modoEditando_ = True

                End If

            End If

            If modoEditando_ Then

                If swcHabilitadoProveedor.Checked Then

                    .Campo(CamposProveedorOperativo.CP_PROVEEDOR_HABILITADO).Valor = True

                    .Campo(CamposProveedorOperativo.CP_PROVEEDOR_HABILITADO).ValorPresentacion = "Online"

                Else

                    .Campo(CamposProveedorOperativo.CP_PROVEEDOR_HABILITADO).Valor = False

                    .Campo(CamposProveedorOperativo.CP_PROVEEDOR_HABILITADO).ValorPresentacion = "Offline"

                End If

            End If

            '_listaDomicilios = New List(Of Rec.Globals.Empresas.Domicilio)

            _empresaNacional = New EmpresaNacional

            If GetVars("_empresaNacional") IsNot Nothing Then

                _empresaNacional = DirectCast(GetVars("_empresaNacional"), EmpresaNacional)

            End If

            _auxiliarProveedor = New AuxiliarProveedor

            If GetVars("_auxiliarproveedor") IsNot Nothing Then

                _auxiliarProveedor = DirectCast(GetVars("_auxiliarproveedor"), AuxiliarProveedor)

            End If

            'LISTA DOMICILIOS PROVEEDORES NACIONALES
            Dim domiciliosproveedor_ = pbDetalleProveedor.DataSource

            Dim i_ = 1

            Dim indice_ = 0

            If _auxiliarProveedor._listadomiciliosconTaxid IsNot Nothing Then

                If _auxiliarProveedor._listadomiciliosconTaxid.Count > 0 Then

                    For Each item_ In domiciliosproveedor_

                        With .Seccion(SeccionesProvedorOperativo.SPRO2).Partida(numeroSecuencia_:=i_)

                            .Campo(CamposProveedorOperativo.CP_ID_DOMICILIO_PROVEEDOR).Valor = _auxiliarProveedor._listadomiciliosconTaxid(indice_)._iddomicilio.ToString

                            .Campo(CamposProveedorOperativo.CP_SEC_DOMICILIO_PROVEEDOR).Valor = _auxiliarProveedor._listadomiciliosconTaxid(indice_).sec

                            .Campo(CamposDomicilio.CA_CVE_PAIS).Valor = _auxiliarProveedor._listadomiciliosconTaxid(indice_).cvePais

                            .Campo(CamposDomicilio.CA_ID_PAIS).Valor = _auxiliarProveedor._listadomiciliosconTaxid(indice_).idpais

                            .Campo(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR).Valor = _auxiliarProveedor._listadomiciliosconTaxid(indice_).clavetaxid

                            .Campo(CamposProveedorOperativo.CA_CVE_TAX_ID_PROVEEDOR).Valor = _auxiliarProveedor._listadomiciliosconTaxid(indice_).clavetaxid

                            .Campo(CamposProveedorOperativo.CA_DOMICILIO_FISCAL).Valor = _auxiliarProveedor._listadomiciliosconTaxid(indice_).domicilioPresentacion

                            .Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor = _auxiliarProveedor._listadomiciliosconTaxid(indice_).numeroexterior + " - " + _auxiliarProveedor._listadomiciliosconTaxid(indice_).numerointerior

                            .Campo(CamposDomicilio.CA_ENTIDAD_MUNICIPIO).Valor = _auxiliarProveedor._listadomiciliosconTaxid(indice_).cveMunicipio

                            .Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor = _auxiliarProveedor._listadomiciliosconTaxid(indice_).cveEntidadfederativa

                            .Campo(CamposProveedorOperativo.CA_ESTADO_DOMICILIO_PROVEEDOR).Valor = CBool(_auxiliarProveedor._listadomiciliosconTaxid(indice_).estado)

                            .Campo(CamposProveedorOperativo.CA_DOMICILIO_ARCHIVADO_PROVEEDOR).Valor = _auxiliarProveedor._listadomiciliosconTaxid(indice_).archivado

                        End With

                        i_ += 1

                        indice_ += 1

                    Next

                End If

            End If

        End With

    End Sub

    Public Overrides Function DespuesRealizarModificacion() As TagWatcher

        RegresarControlesPorDefault()

        Return New TagWatcher(Ok)

    End Function

    'EVENTOS PARA PRESENTACIÓN DE DATOS EN FRONTEND
    Public Overrides Sub DespuesBuquedaGeneralConDatos()

        _tagwatcher = New TagWatcher

        swcHabilitadoProveedor.Visible = True

        swcTipoPersona.Enabled = False

        fsVinculaciones.Visible = True

        fsConfiguracionAdicional.Visible = True

        fsHistorialDomicilios.Visible = True

        With OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

            With .Seccion(SeccionesProvedorOperativo.SPRO1)

                icClave.Text = ""

                icClave.Text = .Campo(CamposProveedorOperativo.CP_CVE_PROVEEDOR).Valor

                swcHabilitadoProveedor.Checked = .Campo(CamposProveedorOperativo.CP_PROVEEDOR_HABILITADO).Valor

                Dim datosproveedoractual_ = _utils.ObtenerDatosProveedorDesdeControlador(OperacionGenerica.Id)

                SetVars("_auxiliarproveedor", datosproveedoractual_)

            End With

        End With

        PreparaTarjetero(PillboxControl.ToolbarModality.Default, pbDetalleProveedor)

        SetVars("pbdetallesdomiciliosAnteriores", pbDetalleProveedor)

        fsVinculaciones.Enabled = True

        fsConfiguracionAdicional.Enabled = True

        fsHistorialDomicilios.Enabled = True

        pbDetalleProveedor.Enabled = True

        fcRazonSocial.Enabled = False

        icRFC.Enabled = False

        swcTipoPersona.Enabled = False

        If swcTipoPersona.Checked = False Then

            icCURP.Visible = True

        Else

            icCURP.Visible = False

        End If

        If swcHabilitadoProveedor.Checked Then

            pbDetalleProveedor.Enabled = False

        Else

            pbDetalleProveedor.Enabled = True

        End If

        EstadoConexion()

    End Sub

    Public Overrides Sub DespuesBuquedaGeneralSinDatos()

        If OperacionGenerica IsNot Nothing Then

            swcHabilitadoProveedor.Visible = True

            swcTipoPersona.Enabled = False

            With OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

                With .Seccion(SeccionesProvedorOperativo.SPRO1)

                    icClave.Text = ""

                    icClave.Text = .Campo(CamposProveedorOperativo.CP_CVE_PROVEEDOR).Valor

                    swcHabilitadoProveedor.Checked = .Campo(CamposProveedorOperativo.CP_PROVEEDOR_HABILITADO).Valor

                    If swcTipoPersona.Checked = False Then

                        icCURP.Visible = True

                    Else

                        icCURP.Visible = False

                    End If

                End With

            End With

            Session("_tbDetalleProveedor") = PillboxControl.ToolbarModality.Default : pbDetalleProveedor.Modality = Session("_tbDetalleProveedor")

            PreparaTarjetero(PillboxControl.ToolbarModality.Default, pbDetalleProveedor)

            SetVars("pbdetallesdomiciliosAnteriores", pbDetalleProveedor)

            fcRazonSocial.Enabled = False

            icRFC.Enabled = False

            swcTipoPersona.Enabled = False

            Dim datosproveedoractual_ = _utils.ObtenerDatosProveedorDesdeControlador(OperacionGenerica.Id)

            SetVars("_auxiliarproveedor", datosproveedoractual_)

            SetVars("_listaDomiciliosNacionales", datosproveedoractual_._listadomiciliosconTaxid)

            EstadoConexion()

            CargarHistorialDomicilios()

            fsVinculaciones.Visible = True

            fsConfiguracionAdicional.Visible = True

            fsHistorialDomicilios.Visible = True

        End If

    End Sub
    'EVENTOS DE MANTENIMIENTO

    Public Sub EstadoConexion()
        ' Visual Basic
        Dim estadoConexion_ As String

        If swcHabilitadoProveedor.Visible Then

            If swcHabilitadoProveedor.Checked Then

                estadoConexion_ = "<span style='color:#757575; font-size:1.2rem'>🟣 Online</span>"

            Else

                estadoConexion_ = "<span style='color:#757575; font-size:1.2rem'>⚪ Offline</span>"

            End If

            online.Text = estadoConexion_

        End If

    End Sub

    Public Overrides Sub LimpiaSesion()

        SetVars("_empresaNacional", Nothing)

        SetVars("_listaEmpresasTemporales", Nothing)

        SetVars("_listaDomicilios", Nothing)

        SetVars("_listaDomiciliosActuales", Nothing)

        SetVars("opcionesLista_", Nothing)

        SetVars("_indice", Nothing)

        SetVars("_cveUltimoDomicilio", Nothing)

        SetVars("pbdetallesdomiciliosAnteriores", Nothing)

        SetVars("isEditing", Nothing)

        Statements.ObjectSession = Nothing

        SetVars("_empresaInternacional", Nothing)

        SetVars("_listaEmpresasTemporales", Nothing)

        SetVars("_listaDomicilios", Nothing)

        SetVars("_listaDomiciliosActuales", Nothing)

        SetVars("_opcionesLista", Nothing)

        SetVars("_indice", Nothing)

        SetVars("_cveUltimoDomicilio", Nothing)

        SetVars("_datosAdicionalesActuales", Nothing)

    End Sub

    Public Overrides Sub Limpiar()

        _lista = Nothing

        _tagwatcher = Nothing

        _espacioTrabajo = Nothing

        _empresaNacional = Nothing

        _controladorEmpresas = Nothing

        ccDomiciliosFiscales.DataSource = Nothing

        ccVinculaciones.DataSource = Nothing

        ccConfiguracionAdicional.DataSource = Nothing

        _cantidadDetalles = Nothing

        scDomiciliosRegistrados.DataSource = Nothing

        scDomiciliosRegistrados.Value = Nothing

        pbDetalleProveedor.DataSource = Nothing

        ConfigurarDomicilios.Visible = False

        icPais.Value = Nothing

        icCURP.Visible = False

    End Sub
#End Region

#Region "████████████████  QUINTA CAPA  █████████       Reglas locales         ██████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████
    'EVENTO PARA REGRESAR CONTROLES POR CADA ACCIÓN DE TARJETA
    Public Sub RegresarControles(Optional ByVal opcion_ As Int32 = 1)

        If pbDetalleProveedor.PageIndex > 0 Then

            ' lbNumero.Text = pbDetalleProveedor.PageIndex.ToString()

        End If

    End Sub


#Region "Buscar empresas por razon social"
    Protected Sub fcRazonSocial_TextChanged(sender As Object, e As EventArgs)

        _lista = New List(Of SelectOption)

        _datospaismexicano = New PaisDomicilio

        _lista = _utils.ListarEmpresasPorRazonSocial(fcRazonSocial.Text,
                                                     tipoempresa_:=IControladorEmpresas.TiposEmpresas.Nacional)

        If _lista.Count > 0 Then

            fcRazonSocial.DataSource = _lista

            pbDetalleProveedor.Enabled = True

            ConfiguracionDomicilioNacional()

        Else

            MsgNoExisteEmpresa()

        End If

    End Sub

    Protected Sub fcRazonSocial_Click(sender As Object, e As EventArgs)

        If fcRazonSocial.Text <> "" Then

            If BuscarSiExisterProveedor() Then

                MsgValidacionRazonsocial()

                ConfigurarDomicilios.Visible = False

                pbDetalleProveedor.Enabled = False

            Else

                ConfigurarDomicilios.Visible = True

                ConfigurarDomicilios.Enabled = True

                _datospaismexicano = New PaisDomicilio

                _datospaismexicano = _utils.ObtenerDatosPaisMexicano()

                If fcRazonSocial.Value <> "" Then

                    _tagwatcher = _utils.BuscarEmpresaPorObjectId(fcRazonSocial.Value, IControladorEmpresas.TiposEmpresas.Nacional)

                    If _tagwatcher.Status = TypeStatus.Ok Then

                        _empresaNacional = DirectCast(_tagwatcher.ObjectReturned, EmpresaNacional)

                        SetVars("_empresaNacional", _empresaNacional)

                        icCveRfc.Value = _empresaNacional._idrfc.ToString

                        icRFC.Value = _empresaNacional.rfc

                    End If

                End If

                With _datospaismexicano

                    icPais.Value = .paisPresentacion

                    icIdPais.Value = .idpais.ToString

                    icCvePais.Value = .pais

                End With

                ListarDomiciliosPorPais(IControladorEmpresas.TiposEmpresas.Nacional)

            End If

        Else

            aviso.Visible = False

            SetVars("_listaEmpresasTemporales", Nothing)

            SetVars("_opcionesLista", Nothing)

            Limpiar()

            LimpiarTarjetero()

            icPais.Value = Nothing

            scDomiciliosRegistrados.Value = Nothing

            scDomiciliosRegistrados.Visible = False

            lbTitle.Visible = False

            pbDetalleProveedor.Enabled = False

            ConfigurarDomicilios.Visible = False

            ConfigurarDomicilios.Enabled = False

            icPais.Value = Nothing

            icIdPais.Value = Nothing

            icCvePais.Value = Nothing

        End If

        'If fcRazonSocial.Text <> "" Then

        '    If BuscarSiExisterProveedor() = False Then

        '        LlenarFormulario()

        '        ConfigurarDomicilios.Visible = True

        '        ConfigurarDomicilios.Enabled = True

        '    Else

        '        MsgValidacionRazonsocial()

        '    End If

        'Else

        '    aviso.Visible = False

        '    ConfigurarDomicilios.Visible = False

        '    swcTipoPersona.Checked = True

        '    SetVars("_listaEmpresasTemporales", Nothing)

        '    SetVars("opcionesLista_", Nothing)

        '    Limpiar()

        '    LimpiarTarjetero()

        '    icPais.Value = Nothing

        '    scDomiciliosRegistrados.Value = Nothing

        '    scDomiciliosRegistrados.Visible = False

        '    lbTitle.Visible = False

        '    pbDetalleProveedor.Enabled = False

        '    ConfigurarDomicilios.Visible = False

        '    ConfigurarDomicilios.Enabled = False

        '    ConfigurarDomicilios.Visible = False

        '    ConfigurarDomicilios.Enabled = False

        '    icIdPais.Value = Nothing

        '    icCvePais.Value = Nothing

        'End If

    End Sub

#End Region

    Protected Function BuscarSiExisterProveedor() As Boolean

        Dim buscarProveedorExistente_ As New ControladorBusqueda(Of ConstructorProveedoresOperativos)

        Dim lista_ = buscarProveedorExistente_.Buscar(fcRazonSocial.Text,
                                                              New Filtro _
                                                              With {.IdSeccion = SeccionesProvedorOperativo.SPRO1,
                                                                    .IdCampo = CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR})

        If lista_ IsNot Nothing Then

            If lista_.Count > 0 Then

                Return True

            End If

        End If

        Return False

    End Function

    'EVENTOS PARA CONTROLAR QUE PASA CON EL TIPO DE USO
    Protected Sub swcTipoUso_CheckedChanged(sender As Object, e As EventArgs)

    End Sub

    'EVENTOS PARA CONTROLAR QUE PASA CON EL TIPO DE PERSONA
    Protected Sub swcTipoPersona_CheckedChanged(sender As Object, e As EventArgs)

        If swcTipoPersona.Checked Then

            icCURP.Visible = False

            icCURP.Value = Nothing

        Else

            icCURP.Visible = True

        End If

    End Sub


    'EVENTOS PARA CARGAR LOS CLIENTES EN LAS LISTAS
    Protected Sub scClienteVinculacion_Click(sender As Object, e As EventArgs)

        Dim controlador_ As New ControladorBusqueda(Of ConstructorCliente)

        Dim lista_ As List(Of SelectOption) = controlador_.Buscar(scClienteVinculacion.SuggestedText, New Filtro With {.IdSeccion = SeccionesClientes.SCS1, .IdCampo = CamposClientes.CA_RAZON_SOCIAL})

        scClienteVinculacion.DataSource = lista_

    End Sub

    Protected Sub scClienteVinculacion_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub scClienteConfiguracion_Click(sender As Object, e As EventArgs)

        Dim controlador_ As New ControladorBusqueda(Of ConstructorCliente)

        Dim lista_ As List(Of SelectOption) = controlador_.Buscar(scClienteConfiguracion.SuggestedText,
                                                                  New Filtro With {.IdSeccion = SeccionesClientes.SCS1, .IdCampo = CamposClientes.CA_RAZON_SOCIAL})

        scClienteConfiguracion.DataSource = lista_

    End Sub

    Protected Sub scClienteConfiguracion_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub scTaxIdVinculacion_Click(sender As Object, e As EventArgs)

        scTaxIdVinculacion.DataSource = IdentificadoresProveedor()

    End Sub

    Protected Sub scVinculacion_Click(sender As Object, e As EventArgs)

        Dim recursos_ As ControladorRecursosAduanalesGral =
            ControladorRecursosAduanalesGral.Buscar(ControladorRecursosAduanalesGral.TiposRecurso.Anexo22)

        Dim vinculaciones_ = From data In recursos_.tiposvinculacion
                             Where data.archivado = False And data.estado = 1
                             Select data._idvinculacion, data.descripcion, data.descripcioncorta

        If vinculaciones_.Count > 0 Then

            Dim dataSource_ As New List(Of SelectOption)

            For index_ As Int32 = 0 To vinculaciones_.Count - 1

                dataSource_.Add(New SelectOption With
                             {.Value = vinculaciones_(index_)._idvinculacion,
                              .Text = vinculaciones_(index_)._idvinculacion.ToString & " - " & vinculaciones_(index_).descripcioncorta})

            Next

            scVinculacion.DataSource = dataSource_

        End If

    End Sub

    Protected Sub scVinculacion_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Function IdentificadoresProveedor() As List(Of SelectOption)

        Dim listaIdentificadores_ As New List(Of SelectOption)

        pbDetalleProveedor.ForEach(Sub(pillbox_ As PillBox)
                                       listaIdentificadores_.Add(New SelectOption With {
                                                                 .Text = icRFC.Value,
                                                                 .Value = pillbox_.GetIndice(pbDetalleProveedor.KeyField)})
                                   End Sub)

        Return listaIdentificadores_

    End Function

    Protected Sub scTaxIdConfiguracion_Click(sender As Object, e As EventArgs)

        scTaxIdConfiguracion.DataSource = IdentificadoresProveedor()

    End Sub

    'EVENTOs PARA MODIFICAR EL LABEL CUANDO CAMBIA DE POSICIÓN EL TARJETERO
    Protected Sub pbDetalleProveedor_CheckedChange(sender As Object, e As EventArgs)

        RegresarControles()

    End Sub

    Protected Sub pbDetalleProveedor_Click(sender As Object, e As EventArgs)

        RegresarControles()

        Select Case pbDetalleProveedor.ToolbarAction

            Case PillboxControl.ToolbarActions.Nuevo

                'lbNumero.Text = pbDetalleProveedor.PageIndex.ToString()

                Dim itemActual_ As Integer = pbDetalleProveedor.PageIndex

                _empresaNacional = New EmpresaNacional

                Dim opcionesLista_ As New List(Of SelectOption)

                If GetVars("_empresaNacional") IsNot Nothing Then

                    _empresaNacional = DirectCast(GetVars("_empresaNacional"), EmpresaNacional)

                End If

                Dim listaCvesDomiciliosActuales As New List(Of String)

                pbDetalleProveedor.ForEach(Sub(pillbox_ As PillBox)
                                               If pillbox_.GetControlValue(icIdDomicilio) IsNot Nothing Then
                                                   listaCvesDomiciliosActuales.Add(pillbox_.GetControlValue(icIdDomicilio))
                                               End If
                                           End Sub)

                Dim domiciliosDesdeEmpresa_ As List(Of Rec.Globals.Empresas.Domicilio) = _empresaNacional.paisesdomicilios.Last.domicilios

                For Each item_ In domiciliosDesdeEmpresa_

                    If Not listaCvesDomiciliosActuales.Contains(item_._iddomicilio.ToString) Then

                        opcionesLista_.Add(New SelectOption With
                            {
                                .Value = item_._iddomicilio.ToString,
                                .Text = item_.domicilioPresentacion
                            })

                    End If

                Next

                If opcionesLista_.Count > 0 Then

                    ConfigurarDomicilios.Visible = True

                    scDomiciliosRegistrados.DataSource = opcionesLista_

                    scDomiciliosRegistrados.Value = opcionesLista_.First.Value

                    SetVars("opcionesLista_", opcionesLista_)

                    SetVars("_indice", itemActual_)

                    SetVars("_cveUltimoDomicilio", opcionesLista_.First.Value)

                End If

                icPais.Value = pbDetalleProveedor.DataSource(0).Item("icPais")

                icCvePais.Value = pbDetalleProveedor.DataSource(0).Item("icCvePais")

                icIdPais.Value = pbDetalleProveedor.DataSource(0).Item("icIdPais")

            Case Else

        End Select

    End Sub

    Protected Sub scDomicilios_SelectedIndexChanged(sender As Object, e As EventArgs)
        'Aqui vamos a determinar si vamos por los domiclios a mongo, que yo creo que si, woa a ver
    End Sub

    Protected Sub scDomicilios_Click(sender As Object, e As EventArgs)

    End Sub

    Protected Sub icMunicipio_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub swcUtilizarDatos_CheckedChanged(sender As Object, e As EventArgs)

    End Sub

    Sub VerificaCheckDomicilio(Optional ByVal opcion_ As Int32 = 1)

    End Sub

    'EVENTOS PARA CARGAR LAS VINCULACIONES
    Protected Sub rdSeleccionarDomicilio_CheckedChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub btnTipoDomicilio_Click(sender As Object, e As EventArgs)

        Dim modoEditando_ As Boolean = False

        If GetVars("isEditing") IsNot Nothing Then

            If GetVars("isEditing") = True Then

                modoEditando_ = True

            End If

        End If

        If fcRazonSocial.Value <> "" Then

            If BuscarSiExisterProveedor() = False Then

                If GetVars("_empresaNacional") Is Nothing Then

                    Dim datosempresaseleccionada_ = _utils.BuscarEmpresaPorObjectId(fcRazonSocial.Value, IControladorEmpresas.TiposEmpresas.Nacional)

                    SetVars("_empresaNacional", datosempresaseleccionada_.ObjectReturned)

                    LlenandoTarjeteroNacional()

                Else

                    LlenandoTarjeteroNacional()

                End If

            End If

        Else

            ''SI NO LA CREAMOS VACIA YA QUE ES NUEVA NUEVA LA EMRPESA
            'MsgValidacionRazonsocialVacio()
            SetVars("_empresaNacional", Nothing)

            ''CERRAR EL SUGERENCIA DEL DOMICILIO
            ConfigurarDomicilios.Visible = False

            ''PONER EN LOS INPUTS LOS DATOS DEL PAIS
            Dim pais_ = _utils.ObtenerDatosPaisMexicano()
            icPais.Value = pais_.paisPresentacion
            icIdPais.Value = pais_.idpais.ToString
            icCvePais.Value = pais_.pais

            ''HABILIATAR EL PILLBOX
            pbDetalleProveedor.Enabled = True

        End If

    End Sub


    Protected Sub scDomiciliosRegistrados_SelectedIndexChanged(sender As Object, e As EventArgs)

        If GetVars("_listaDomiciliosNacionales") IsNot Nothing Then

            scDomiciliosRegistrados.DataSource = GetVars("_listaDomiciliosNacionales")

        End If
    End Sub

    Protected Sub scDomiciliosRegistrados_TextChanged(sender As Object, e As EventArgs)
        If GetVars("_listaDomiciliosNacionales") IsNot Nothing Then

            scDomiciliosRegistrados.DataSource = GetVars("_listaDomiciliosNacionales")

        End If
    End Sub

    Protected Sub scDomiciliosRegistrados_Click(sender As Object, e As EventArgs)

        If GetVars("_listaDomiciliosNacionales") IsNot Nothing Then

            scDomiciliosRegistrados.DataSource = GetVars("_listaDomiciliosNacionales")

        End If

    End Sub
#End Region

#Region "██████ Vinculación sexta capa  █████████       SAX      ████████████████████████████████████████████"
    '    ██████   Controladores utilizados                     Documentos por coding para MongoDB      ██████
    '    ██████    1.ControladorEmpresas                        1. En Empresa:                         ██████
    '    ██████    2.ControladorRecursosAduanales                  a). Domicilios                      ██████
    '    ██████    3.ControladorSecuencias                         b). Contactos                       ██████
    '    ██████                                                                                        ██████
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    Private Sub validaCambiosTarjeta(ByRef pillbox_ As PillBox, ByVal documeto_ As DocumentoElectronico, ByRef swCambio_ As Boolean)

        Dim partida_ = documeto_.Seccion(SeccionesProvedorOperativo.SPRO2).Nodos(pillbox_.GetIdentity - 1)

        If Not pillbox_.GetControlValue(icCalle).Equals(partida_.Attribute(CamposDomicilio.CA_CALLE).Valor) Then

            If partida_.Attribute(CamposDomicilio.CA_CALLE).Valor IsNot Nothing Then

                swCambio_ = True

                Exit Sub

            End If

        End If

        If Not pillbox_.GetControlValue(icNumeroExterior).Equals(partida_.Attribute(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor) Then

            If partida_.Attribute(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor IsNot Nothing Then

                swCambio_ = True

                Exit Sub

            End If

        End If

        If Not pillbox_.GetControlValue(icNumeroInterior).Equals(partida_.Attribute(CamposDomicilio.CA_NUMERO_INTERIOR).Valor) Then

            If partida_.Attribute(CamposDomicilio.CA_NUMERO_INTERIOR).Valor IsNot Nothing Then

                swCambio_ = True

                Exit Sub

            End If

        End If

        If Not pillbox_.GetControlValue(icCodigoPostal).Equals(partida_.Attribute(CamposDomicilio.CA_CODIGO_POSTAL).Valor) Then

            If partida_.Attribute(CamposDomicilio.CA_CODIGO_POSTAL).Valor IsNot Nothing Then

                swCambio_ = True

                Exit Sub

            End If

        End If

        If Not pillbox_.GetControlValue(icColonia).Equals(partida_.Attribute(CamposDomicilio.CA_COLONIA).Valor) Then

            If partida_.Attribute(CamposDomicilio.CA_COLONIA).Valor IsNot Nothing Then

                swCambio_ = True

                Exit Sub

            End If

        End If

        If Not pillbox_.GetControlValue(icLocalidad).Equals(partida_.Attribute(CamposDomicilio.CA_LOCALIDAD).Valor) Then

            If partida_.Attribute(CamposDomicilio.CA_LOCALIDAD).Valor IsNot Nothing Then

                swCambio_ = True

                Exit Sub

            End If

        End If

        If Not pillbox_.GetControlValue(icCiudad).Equals(partida_.Attribute(CamposDomicilio.CA_CIUDAD).Valor) Then

            If partida_.Attribute(CamposDomicilio.CA_CIUDAD).Valor IsNot Nothing Then

                swCambio_ = True

                Exit Sub

            End If

        End If

        If Not pillbox_.GetControlValue(icMunicipio).Equals(partida_.Attribute(CamposDomicilio.CA_MUNICIPIO).Valor) Then

            If partida_.Attribute(CamposDomicilio.CA_MUNICIPIO).Valor IsNot Nothing Then

                swCambio_ = True

                Exit Sub

            End If

        End If

        If Not pillbox_.GetControlValue(icEntidadFederativa).Equals(partida_.Attribute(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor) Then

            If partida_.Attribute(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor IsNot Nothing Then

                swCambio_ = True

                Exit Sub

            End If

        End If

        If Not pillbox_.GetControlValue(icPais).Equals(partida_.Attribute(CamposDomicilio.CA_PAIS).Valor) Then

            If partida_.Attribute(CamposDomicilio.CA_PAIS).Valor IsNot Nothing Then

                swCambio_ = True

                Exit Sub

            End If

        End If

    End Sub


    Protected Sub BloquearCampos(ByVal value As Boolean)

        icRFC.Enabled = value

        swcTipoPersona.Enabled = value

        icCalle.Enabled = value

        icNumeroExterior.Enabled = value

        icNumeroInterior.Enabled = value

        icCodigoPostal.Enabled = value

        icColonia.Enabled = value

        icLocalidad.Enabled = value

        icCiudad.Enabled = value

        icMunicipio.Enabled = value

        icEntidadFederativa.Enabled = value

    End Sub

    Public Overrides Function AgregarComponentesBloqueadosInicial() As List(Of WebControl)

        Dim bloqueados_ As New List(Of WebControl) From {icPais}

        Return bloqueados_

    End Function

    Public Overrides Function AgregarComponentesBloqueadosEdicion() As List(Of WebControl)

        Dim bloqueadosEdicion_ As New List(Of WebControl) From {fcRazonSocial, swcTipoPersona, icRFC, icCURP, icPais}

        Return bloqueadosEdicion_

    End Function

    Private Sub CambiarDomicilio(ByVal cveDomicilio_ As String, Optional ByVal indice_ As Integer = 0)

        _tagwatcher = New TagWatcher

        _controladorEmpresas = New Rec.Globals.Controllers.Empresas.ControladorEmpresas(New EspacioTrabajo()) With {.ListaEmpresas = New List(Of IEmpresa)}

        _empresaNacional = New EmpresaNacional

        _empresaNacional = DirectCast(GetVars("_empresaNacional"), EmpresaNacional)

        With _controladorEmpresas

            .Modalidad = IControladorEmpresas.Modalidades.Intrinseca

            .ListaEmpresas.Add(_empresaNacional)

            _tagwatcher = .ConsultarDomicilio(_empresaNacional._id, New ObjectId(cveDomicilio_))

        End With

        Try

            If _tagwatcher.Status = TypeStatus.Ok Then

                Dim domicilioSeleccionado = DirectCast(_tagwatcher.ObjectReturned, List(Of Rec.Globals.Empresas.Domicilio))

                If indice_ <> 0 Then

                    LlenarTarjetero(domicilioSeleccionado, indice_)

                Else

                    CargarTarjetero(domicilioSeleccionado)

                End If

            End If

        Catch ex As Exception

            DisplayMessage("No hay cambios", StatusMessage.Fail)

        End Try

    End Sub

    Private Function GenerarRfc() As Rec.Globals.Empresas.Rfc

        Dim rfc_ As New Rec.Globals.Empresas.Rfc

        pbDetalleProveedor.ForEach(Sub(pillbox_ As PillBox)
                                       rfc_ = New Rec.Globals.Empresas.Rfc(pillbox_.GetControlValue(icRFC))
                                       pillbox_.SetControlValue(icCveRfc, rfc_.idrfc.ToString)
                                   End Sub)

        Return rfc_

    End Function

    Protected Sub LlenandoTarjeteroNacional()

        Dim modoEditando_ As Boolean = False

        If GetVars("isEditing") IsNot Nothing Then

            If GetVars("isEditing") = True Then

                modoEditando_ = True

            End If

        End If

        ''VALIDAR QUE SE LLENE ESTE CAMPO, SINO QUIERE DECIR QUE NO EXISTE Y POR LO TANTO ENVIAREMOS UNA INFORMACION
        If icPais.Value <> "" Then
            'icPais
            'icCvePais
            'icIdPais
            pbDetalleProveedor.Enabled = True

            If icIdTarjeta.Value Is Nothing Then

                Dim controladorFirma_ As New ControladorFirmaElectronica

                Dim id_ = ObjectId.GenerateNewId()

                icIdTarjeta.Value = id_.ToString

                icFirmaTarjeta.Value = controladorFirma_.Generar(id_, 1)

            End If

            Dim domicilioSeleccionado_ As String = Nothing

            If scDomiciliosRegistrados.Value = "" Then

                If GetVars("_domiciliounicoNacional") IsNot Nothing Then

                    domicilioSeleccionado_ = GetVars("_domiciliounicoNacional")

                Else

                    domicilioSeleccionado_ = scDomiciliosRegistrados.Value

                End If
            Else

                domicilioSeleccionado_ = scDomiciliosRegistrados.Value

            End If

            Dim indice_ As Integer = pbDetalleProveedor.PageIndex

            If modoEditando_ Then

                If indice_ <> 0 Then

                    If domicilioSeleccionado_ = "" Then

                        ConfigurarDomicilios.Visible = False

                    Else

                        CambiarDomicilioNacional(domicilioSeleccionado_, indice_)

                        ConfigurarDomicilios.Visible = False

                    End If

                End If

            Else

                If domicilioSeleccionado_ = "" Then

                    LimpiarTarjetero()

                Else

                    CambiarDomicilioNacional(domicilioSeleccionado_, indice_)

                End If

                ConfigurarDomicilios.Visible = False

            End If

        Else

            ''VALIDAR QUE EL PAIS EXISTA
            MsgValidacionPaisValido()

        End If

    End Sub

    Private Sub CambiarDomicilioNacional(ByVal cveDomicilio_ As String,
                                 Optional ByVal indice_ As Integer = 0)

        _empresaNacional = New EmpresaNacional

        If GetVars("_empresaNacional") IsNot Nothing Then

            _empresaNacional = DirectCast(GetVars("_empresaNacional"), EmpresaNacional)

        End If

        ''OBTENER EL DOMICILIO SELECCIONADO
        Dim domicilioseleccionado_ = _utils.ObtenerDomicilioEnPais(_empresaNacional, New ObjectId(icIdPais.Value), New ObjectId(cveDomicilio_))

        If domicilioseleccionado_ IsNot Nothing Then

            pbDetalleProveedor.Enabled = True

            If indice_ <> 0 Then

                LlenarTarjetero(New List(Of Domicilio) From {domicilioseleccionado_}, indice_)

            Else

                CargarTarjetero(New List(Of Domicilio) From {domicilioseleccionado_})

            End If

        End If

    End Sub

    Protected Sub GuardarEmpresaNacional(ByVal session_ As IClientSessionHandle)

        Dim modoEditando_ As Boolean = False

        If GetVars("isEditing") IsNot Nothing Then

            If GetVars("isEditing") = True Then

                modoEditando_ = True

            End If

        End If

        Dim existerfc_ As Boolean = True

        Dim existepais_ As Boolean = True

        Dim existedomicilio_ As Boolean = True

        Dim coinciderfc_ As Boolean = True

        Dim coincidedomicilio_ As Boolean = True

        _auxiliarProveedor = New AuxiliarProveedor

        _empresaNacional = New EmpresaNacional

        _tagwatcher = New TagWatcher

        If GetVars("_empresaNacional") IsNot Nothing Then

            _empresaNacional = DirectCast(GetVars("_empresaNacional"), EmpresaNacional)

        Else

            _tagwatcher = _utils.BuscarEmpresaPorObjectId(fcRazonSocial.Value, IControladorEmpresas.TiposEmpresas.Nacional)

            _empresaNacional = _tagwatcher.ObjectReturned

        End If

        If GetVars("_auxiliarproveedor") IsNot Nothing Then
            ''ES PORQUE ES UNA MODADLIDAD DE ACTUALIZAR EL PROVEEDOR
            _auxiliarProveedor = DirectCast(GetVars("_auxiliarproveedor"), AuxiliarProveedor)

        Else
            ''ES ALTA PROVEEDOR
            _auxiliarProveedor._razonsocial = _empresaNacional.razonsocial

            _auxiliarProveedor._listadomiciliosconTaxid = New List(Of DomiciliosTaxid)

        End If

        Dim item_ = 0

        Dim totalDomicilios_ = pbDetalleProveedor.DataSource.Count() - 1

        pbDetalleProveedor.ForEach(Sub(pillbox_ As PillBox)

                                       ''PAIS Y DOMICILIOS
                                       Dim idpais_ = pillbox_.GetControlValue(icIdPais)

                                       Dim cvepais_ = pillbox_.GetControlValue(icCvePais)

                                       Dim pais_ = pillbox_.GetControlValue(icPais)

                                       Dim iddomicilio_ = pillbox_.GetControlValue(icIdDomicilio)

                                       Dim domiciliopresentacion_ = pillbox_.GetControlValue(scDomicilio)

                                       Dim domicilioAux_ = New Domicilio

                                       With domicilioAux_

                                           If pillbox_.GetControlValue(icIdDomicilio) <> "" Then

                                               ._iddomicilio = ObjectId.Parse(pillbox_.GetControlValue(icIdDomicilio))

                                           End If

                                           .calle = pillbox_.GetControlValue(icCalle)

                                           .numeroexterior = pillbox_.GetControlValue(icNumeroExterior)

                                           .numerointerior = pillbox_.GetControlValue(icNumeroInterior)

                                           .colonia = pillbox_.GetControlValue(icColonia)

                                           .codigopostal = pillbox_.GetControlValue(icCodigoPostal)

                                           .ciudad = pillbox_.GetControlValue(icCiudad)

                                           .localidad = pillbox_.GetControlValue(icLocalidad)

                                           .cveMunicipio = pillbox_.GetControlValue(icCveMunicipio)

                                           .municipio = pillbox_.GetControlValue(icMunicipio)

                                           .cveEntidadfederativa = pillbox_.GetControlValue(icCveEntidadFederativa)

                                           '.entidadfederativa = pillbox_.GetControlValue
                                           '.entidadfederativa = pillbox_.GetControlValue(icEntidadFederativa)

                                           If pillbox_.GetControlValue(icSecDomicilio) <> "" Then

                                               .sec = Int(pillbox_.GetControlValue(icSecDomicilio))

                                           End If

                                           ''FALTA CARGAR EL estado proveedor

                                           If pillbox_.GetControlValue(icestadoproveedor) <> "" Then

                                               ''VALOR VIENE COMO BOOLEANO

                                               .estado = IIf(pillbox_.GetControlValue(icestadoproveedor) = "True", 1, 0)


                                           End If

                                           '.archivado = pillbox_.GetControlValue(archivado)
                                       End With

                                       Dim domicilioPresentacionAux_ = _utils.GenerarDomicilioPresentacion(domicilioAux_, pais_)

                                       'pillbox_.GetControlValue(icestadoproveedor)
                                       'pillbox_.GetControlValue(icdomicilioarchivadoproveedor)
                                       'pillbox_.GetControlValue(icmotivoarchivadoproveedor)
                                       'pillbox_.GetControlValue(fechaarchivadoproveedor)

                                       If iddomicilio_ <> "" Then

                                           existedomicilio_ = _utils.ExisteDomicilioEnPaisNacional(_empresaNacional, ObjectId.Parse(iddomicilio_))

                                           If existedomicilio_ = False Then

                                               ''GENERAMOS LA ESTRUCTURA DEL DOMICILIO
                                               iddomicilio_ = Nothing

                                               domicilioAux_._iddomicilio = New ObjectId()

                                               'domicilioAux_.sec = 1
                                               ''IMPORTANTE MANDAR EL DOMICILIO ESPECIFICO
                                               _empresaNacional = _utils.GenerarEmpresaNacionalConDomicilioNuevo(_empresaNacional, domicilioAux_, pais_)

                                           Else

                                               If domiciliopresentacion_ <> "" Then

                                                   If domiciliopresentacion_ = domicilioPresentacionAux_ Then

                                                       coincidedomicilio_ = _utils.ExisteDomicilioPresentacionEnDomiciliosNacionales(_empresaNacional, ObjectId.Parse(iddomicilio_), domiciliopresentacion_)

                                                       If coincidedomicilio_ = False Then

                                                           ''GENERAMOS LA ESTRUCTURA DEL DOMICILIO
                                                           iddomicilio_ = Nothing

                                                           domicilioAux_._iddomicilio = ObjectId.GenerateNewId()

                                                           ''IMPORTANTE MANDAR EL DOMICILIO ESPECIFICO
                                                           _empresaNacional = _utils.GenerarEmpresaNacionalConDomicilioNuevo(_empresaNacional, domicilioAux_, pais_)

                                                       End If

                                                   Else

                                                       ''GENERAMOS LA ESTRUCTURA DEL DOMICILIO
                                                       iddomicilio_ = Nothing

                                                       domicilioAux_._iddomicilio = ObjectId.GenerateNewId()

                                                       ''IMPORTANTE MANDAR EL DOMICILIO ESPECIFICO
                                                       _empresaNacional = _utils.GenerarEmpresaNacionalConDomicilioNuevo(_empresaNacional, domicilioAux_, pais_)

                                                   End If

                                               End If

                                           End If

                                       Else
                                           ''GENERAMOS LA ESTRUCTURA DEL DOMICILIO
                                           domicilioAux_._iddomicilio = ObjectId.GenerateNewId()

                                           ''IMPORTANTE MANDAR EL DOMICILIO ESPECIFICO
                                           _empresaNacional = _utils.GenerarEmpresaNacionalConDomicilioNuevo(_empresaNacional, domicilioAux_, pais_)

                                       End If

                                       ''AGREGAMOS LO QUE CAMBIE TAMBIEN PARA LOS DATOS DEL DESTINATARIO
                                       ''RECUERDA QUE ESTA CLASE SOLO ES UNA TRANSPORTADORA DE DATOS LOCAL
                                       Dim ultimoDomicilio_ As New Domicilio

                                       If iddomicilio_ <> "" AndAlso iddomicilio_ <> Nothing Then

                                           ultimoDomicilio_ = _empresaNacional.paisesdomicilios _
                                                                                          .Where(Function(p) p.idpais = ObjectId.Parse(idpais_)) _
                                                                                          .SelectMany(Function(p) p.domicilios) _
                                                                                          .FirstOrDefault(Function(x) x._iddomicilio.Equals(ObjectId.Parse(iddomicilio_)))

                                       Else
                                           ''EL ID DEL DOMICILIO ESTA VACIO, ES PORQUE ES UN DOMICILIO NUEVO
                                           ultimoDomicilio_ = _empresaNacional.paisesdomicilios _
                                                                                   .Where(Function(p) p.idpais = ObjectId.Parse(idpais_)) _
                                                                                   .SelectMany(Function(p) p.domicilios) _
                                                                                   .LastOrDefault()

                                       End If


                                       ''CONSIDERAR LLENAR TODA LA CLASE DE auxiliarDomicilio

                                       ''SACAR LOS CASOS EN MODIFICIACION

                                       ''QUIZAS HACER UNA FUNCION QUE VAYA ARCHIVAR EL DOMICILIO ESE ACTUAL
                                       ''SOLO PARA QUE A LA HORA QUE PILLBOX NO LO PINTE
                                       ''PORQUE VA ESTAR ARCHIVADO AUTOMATICAMENTE.
                                       ''Y ADEMAS EN SU LUGAR QUEDE LO NUEVO QUE SE ALTERÓ
                                       With _auxiliarProveedor

                                           If modoEditando_ Then

                                               ._listadomiciliosconTaxid(item_)._iddomicilio = ultimoDomicilio_._iddomicilio

                                               ._listadomiciliosconTaxid(item_).sec = totalDomicilios_ + 1

                                               ._listadomiciliosconTaxid(item_).domicilioPresentacion = ultimoDomicilio_.domicilioPresentacion

                                               ._listadomiciliosconTaxid(item_).clavetaxid = icCveRfc.Value

                                               ._listadomiciliosconTaxid(item_).taxid = icRFC.Value

                                               ._listadomiciliosconTaxid(item_).cvePais = cvepais_

                                               ._listadomiciliosconTaxid(item_).idpais = idpais_

                                               ._listadomiciliosconTaxid(item_).numeroexterior = ultimoDomicilio_.numeroexterior

                                               ._listadomiciliosconTaxid(item_).numerointerior = ultimoDomicilio_.numerointerior

                                           Else

                                               Dim auxDomicilioConTaxid_ = New DomiciliosTaxid

                                               With auxDomicilioConTaxid_

                                                   ._iddomicilio = ultimoDomicilio_._iddomicilio

                                                   .sec = totalDomicilios_ + 1

                                                   .domicilioPresentacion = ultimoDomicilio_.domicilioPresentacion

                                                   .clavetaxid = icCveRfc.Value

                                                   .taxid = icRFC.Value

                                                   .cvePais = cvepais_

                                                   .idpais = idpais_

                                                   .numeroexterior = ultimoDomicilio_.numeroexterior

                                                   .numerointerior = ultimoDomicilio_.numerointerior

                                               End With

                                               _auxiliarProveedor._listadomiciliosconTaxid.Add(auxDomicilioConTaxid_)

                                           End If

                                       End With
                                       item_ += 1
                                   End Sub)

        SetVars("_empresaNacional", _empresaNacional)

        SetVars("_auxiliarproveedor", _auxiliarProveedor)

    End Sub

    Protected Sub GuardarEmpresaNacional(ByVal session_ As IClientSessionHandle,
                                         ByVal esempresanueva_ As Boolean)

        Dim modoEditando_ As Boolean = False

        _auxiliarProveedor = New AuxiliarProveedor

        _tagwatcher = New TagWatcher

        If GetVars("isEditing") IsNot Nothing Then

            If GetVars("isEditing") = True Then

                modoEditando_ = True

            End If

        End If

        ''Es nueva la empresa
        _auxiliarProveedor = _utils.ObtenerDatosProveedorDesdePillbox(fcRazonSocial.Text, pbDetalleProveedor)

        With _auxiliarProveedor

            _empresaNacional = New EmpresaNacional

            _empresaNacional = _utils.GenerarEstructuraEmpresa(._razonsocial, ._taxid, ._listadomiciliosconTaxid.Last, IEmpresaNacional.TiposPersona.Moral, IControladorEmpresas.TiposEmpresas.Nacional)

            _tagwatcher = _utils.GuardarEmpresa(_empresaNacional, Nothing)

            If _tagwatcher.Status = TypeStatus.Ok Then

                _auxiliarProveedor.id = _empresaNacional._id.ToString

                _auxiliarProveedor._listadomiciliosconTaxid.Last.clavetaxid = _empresaNacional.rfcs.Last.idrfc.ToString

                _auxiliarProveedor._listadomiciliosconTaxid.Last.taxid = _empresaNacional.rfcs.Last.rfc

                _auxiliarProveedor._listadomiciliosconTaxid.Last._iddomicilio = _empresaNacional.paisesdomicilios.Last.domicilios.Last._iddomicilio

                _auxiliarProveedor._listadomiciliosconTaxid.Last.sec = _empresaNacional.paisesdomicilios.Last.domicilios.Last.sec

                _auxiliarProveedor._listadomiciliosconTaxid.Last.domicilioPresentacion = _empresaNacional.paisesdomicilios.Last.domicilios.Last.domicilioPresentacion

                SetVars("_empresaNacional", _empresaNacional)

            End If

            SetVars("_auxiliarproveedor", _auxiliarProveedor)

        End With

    End Sub

    Private Function Vinculacion() As List(Of SelectOption)

        Dim recursos_ As ControladorRecursosAduanalesGral =
            ControladorRecursosAduanalesGral.Buscar(ControladorRecursosAduanalesGral.TiposRecurso.Anexo22)

        Dim vinculaciones_ = From data In recursos_.tiposvinculacion
                             Where data.archivado = False And data.estado = 1
                             Select data._idvinculacion, data.descripcion, data.descripcioncorta

        If vinculaciones_.Count > 0 Then

            Dim dataSource_ As New List(Of SelectOption)

            For index_ As Int32 = 0 To vinculaciones_.Count - 1

                dataSource_.Add(New SelectOption With
                             {.Value = vinculaciones_(index_)._idvinculacion,
                              .Text = vinculaciones_(index_)._idvinculacion.ToString & " - " & vinculaciones_(index_).descripcioncorta})

            Next

            Return dataSource_

        End If

        Return Nothing

    End Function

    'EVENTO PARA LEER EL HISTORIAL DE DOMICILIO
    Protected Sub CargarHistorialDomicilios()

        Dim i = 1

        ccDomiciliosFiscales.ForEach(Sub(pillbox_ As PillBox)
                                         ccDomiciliosFiscales.SetRow(Sub(catalogRow_ As CatalogRow)
                                                                         catalogRow_.SetIndice(ccDomiciliosFiscales.KeyField, i)
                                                                         catalogRow_.SetColumn(icTaxIDRFC, icRFC.Value)
                                                                         catalogRow_.SetColumn(icDomicilio, pillbox_.GetControlValue(scDomicilio))
                                                                         catalogRow_.SetColumn(swcArchivarDomicilio, pillbox_.IsFiled())
                                                                     End Sub)
                                         i += 1
                                     End Sub)

        ccDomiciliosFiscales.CatalogDataBinding()

    End Sub

    Protected Sub LLenarListaDomiciliosEmpresas(ByVal indice_ As Integer)

        _empresaNacional = New EmpresaNacional

        Dim opcionesLista_ As New List(Of SelectOption)

        If GetVars("_empresaNacional") IsNot Nothing Then

            _empresaNacional = DirectCast(GetVars("_empresaNacional"), EmpresaNacional)

        End If

        _pillboxControl = New PillboxControl

        _pillboxControl = pbDetalleProveedor

        Dim domiciliosDesdeEmpresa_ As List(Of Rec.Globals.Empresas.Domicilio) = _empresaNacional.paisesdomicilios.Last.domicilios

        For Each item_ In domiciliosDesdeEmpresa_

            opcionesLista_.Add(New SelectOption With
                                {
                                    .Value = item_._iddomicilio.ToString,
                                    .Text = item_.domicilioPresentacion
                                })

        Next

        If opcionesLista_.Count > 0 And scDomiciliosRegistrados.DataSource Is Nothing Then

            scDomiciliosRegistrados.DataSource = opcionesLista_

            scDomiciliosRegistrados.Value = opcionesLista_.First.Value

            SetVars("opcionesLista_", opcionesLista_)

            SetVars("_indice", indice_)

        End If

    End Sub

    Protected Sub LlenarFormulario()

        Dim modoEditando_ As Boolean = False

        If GetVars("isEditing") IsNot Nothing Then

            If GetVars("isEditing") = True Then

                modoEditando_ = True

            End If

        End If

        Dim aqui = GetVars("_empresaNacional")

        ''VALIDAR QUE SE LLENE ESTE CAMPO, SINO QUIERE DECIR QUE NO EXISTE Y POR LO TANTO ENVIAREMOS UNA INFORMACION
        If icPais.Value <> "" Then
            'icPais
            'icCvePais
            'icIdPais
            pbDetalleProveedor.Enabled = True

            Dim domicilioSeleccionado_ As String = Nothing

            If scDomiciliosRegistrados.Value = "" Then

                If GetVars("_domiciliounicoNacional") IsNot Nothing Then

                    domicilioSeleccionado_ = GetVars("_domiciliounicoNacional")

                Else

                    domicilioSeleccionado_ = scDomiciliosRegistrados.Value

                End If
            Else

                domicilioSeleccionado_ = scDomiciliosRegistrados.Value

            End If

            Dim indice_ As Integer = pbDetalleProveedor.PageIndex

            If modoEditando_ Then

                If indice_ <> 0 Then

                    If domicilioSeleccionado_ = "" Then

                        ConfigurarDomicilios.Visible = False

                    Else

                        CambiarDomicilioNacional(domicilioSeleccionado_, indice_)

                        ConfigurarDomicilios.Visible = False

                    End If

                End If

            Else

                If domicilioSeleccionado_ = "" Then

                    LimpiarTarjetero()

                Else

                    CambiarDomicilioNacional(domicilioSeleccionado_, indice_)

                End If

                ConfigurarDomicilios.Visible = False

            End If

        Else

            ''VALIDAR QUE EL PAIS EXISTA
            MsgValidacionPaisValido()

        End If

        '_tagwatcher = New TagWatcher

        '_empresaNacional = New EmpresaNacional

        '_ultimoDomicilio = New List(Of Rec.Globals.Empresas.Domicilio)

        '_opcionesLista = New List(Of SelectOption)

        '_pillboxControl = New PillboxControl

        '_controladorEmpresas = New Rec.Globals.Controllers.Empresas.ControladorEmpresas(New EspacioTrabajo()) With {
        '    .ListaEmpresas = New List(Of IEmpresa)
        '}

        'If GetVars("_listaEmpresasTemporales") IsNot Nothing Then

        '    Dim listaEmpresasNacionales_ As List(Of EmpresaNacional) = DirectCast(GetVars("_listaEmpresasTemporales"), List(Of EmpresaNacional))

        '    If listaEmpresasNacionales_.Count > 0 Then

        '        _empresaNacional = DirectCast(listaEmpresasNacionales_.Find(Function(x) x.razonsocial = fcRazonSocial.Text), EmpresaNacional)

        '        SetVars("_empresaNacional", _empresaNacional)

        '        _controladorEmpresas.Modalidad = IControladorEmpresas.Modalidades.Intrinseca

        '        _controladorEmpresas.ListaEmpresas.Add(_empresaNacional)

        '        _tagwatcher = _controladorEmpresas.ConsultarDomicilios(_empresaNacional._id)

        '        If _tagwatcher.Status = TypeStatus.Ok Then

        '            Dim listaDomicilios_ As List(Of Rec.Globals.Empresas.Domicilio) = DirectCast(_tagwatcher.ObjectReturned, List(Of Rec.Globals.Empresas.Domicilio))

        '            If listaDomicilios_.Count > 0 Then

        '                _ultimoDomicilio.Add(listaDomicilios_.Last)

        '                For Each item_ In listaDomicilios_

        '                    _opcionesLista.Add(New SelectOption With
        '                                        {
        '                                            .Value = item_._iddomicilio.ToString,
        '                                            .Text = item_.domicilioPresentacion
        '                                        })

        '                Next

        '                If _opcionesLista.Count > 0 And scDomiciliosRegistrados.DataSource Is Nothing Then

        '                    scDomiciliosRegistrados.DataSource = _opcionesLista

        '                    scDomiciliosRegistrados.Value = _opcionesLista.Last.Value

        '                    icRFC.Value = _empresaNacional.rfc

        '                    icCveRfc.Value = _empresaNacional._idrfc.ToString

        '                    If _empresaNacional.curp <> "" Then

        '                        swcTipoPersona.Checked = False

        '                        icCURP.Visible = True

        '                        icCURP.Value = _empresaNacional.curp

        '                        icCveCurp.Value = _empresaNacional._idcurp.ToString

        '                    End If

        '                    SetVars("opcionesLista_", _opcionesLista)

        '                    SetVars("listaDomicilios_", listaDomicilios_)

        '                    SetVars("_cveUltimoDomicilio", _opcionesLista.Last.Value)

        '                End If

        '            End If

        '        End If

        '    Else

        '        SetVars("_listaEmpresasTemporales", Nothing)

        '    End If

        'Else

        '    SetVars("_listaEmpresasTemporales", Nothing)

        'End If

    End Sub

    Sub LlenarTarjetero(ByVal domicilio_ As List(Of Rec.Globals.Empresas.Domicilio), ByVal indice_ As Integer)

        _pillboxControl = New PillboxControl

        _pillboxControl = pbDetalleProveedor

        For Each item_ In domicilio_

            _pillboxControl.SetPillbox(Sub(pillbox_ As PillBox)

                                           pillbox_.SetIndice(_pillboxControl.KeyField, indice_)

                                           pillbox_.SetFiled(False)

                                           icCalle.Value = item_.calle

                                           icNumeroExterior.Value = item_.numeroexterior

                                           icNumeroInterior.Value = item_.numerointerior

                                           icCodigoPostal.Value = item_.codigopostal

                                           icColonia.Value = item_.colonia

                                           icLocalidad.Value = item_.localidad

                                           icCiudad.Value = item_.ciudad

                                           icMunicipio.Value = item_.municipio

                                           icEntidadFederativa.Value = item_.entidadfederativa

                                           scDomicilio.Value = item_.domicilioPresentacion

                                           icIdDomicilio.Value = item_._iddomicilio.ToString

                                           icSecDomicilio.Value = item_.sec

                                       End Sub)
        Next

    End Sub

    Sub CargarTarjetero(ByVal domicilio_ As List(Of Rec.Globals.Empresas.Domicilio),
                        Optional ByVal indice_ As Integer = 0)

        _empresaNacional = New EmpresaNacional

        If GetVars("_empresaNacional") IsNot Nothing Then

            _empresaNacional = DirectCast(GetVars("_empresaNacional"), EmpresaNacional)

        End If

        _pillboxControl = New PillboxControl

        _pillboxControl = pbDetalleProveedor

        _pillboxControl.ClearRows()

        Dim controladorFirma_ As New ControladorFirmaElectronica

        For Each item_ In domicilio_

            Dim id_ = ObjectId.GenerateNewId()

            _pillboxControl.SetPillbox(Sub(pillbox_ As PillBox)
                                           pillbox_.SetIndice(_pillboxControl.KeyField, indice_)
                                           pillbox_.SetFiled(False)
                                           pillbox_.SetControlValue(icIdTarjeta, id_.ToString)
                                           pillbox_.SetControlValue(icFirmaTarjeta, controladorFirma_.Generar(id_, 1))
                                           pillbox_.SetControlValue(icIdPais, _empresaNacional.paisesdomicilios.Last.idpais.ToString)
                                           pillbox_.SetControlValue(icCvePais, _empresaNacional.paisesdomicilios.Last.pais)
                                           pillbox_.SetControlValue(icPais, _empresaNacional.paisesdomicilios.Last.paisPresentacion)
                                           pillbox_.SetControlValue(icCalle, item_.calle)
                                           pillbox_.SetControlValue(icNumeroExterior, item_.numeroexterior)
                                           pillbox_.SetControlValue(icNumeroInterior, item_.numerointerior)
                                           pillbox_.SetControlValue(icCodigoPostal, item_.codigopostal)
                                           pillbox_.SetControlValue(icColonia, item_.colonia)
                                           pillbox_.SetControlValue(icLocalidad, item_.localidad)
                                           pillbox_.SetControlValue(icCiudad, item_.ciudad)
                                           pillbox_.SetControlValue(icMunicipio, item_.municipio)
                                           'pillbox_.SetControlValue(icEntidadFederativa, item_.entidadfederativa)
                                           pillbox_.SetControlValue(icIdDomicilio, item_._iddomicilio.ToString)
                                           pillbox_.SetControlValue(icSecDomicilio, item_.sec)
                                           pillbox_.SetControlValue(scDomicilio, item_.domicilioPresentacion)
                                           pillbox_.SetControlValue(icNumeroExtInt, item_.numeroexterior & " - " & item_.numerointerior)
                                           pillbox_.SetControlValue(icCveMunicipio, item_.cveMunicipio)
                                           pillbox_.SetControlValue(icCveEntidadFederativa, item_.cveEntidadfederativa)
                                           indice_ += 1
                                       End Sub)
        Next

        pbDetalleProveedor = _pillboxControl

        pbDetalleProveedor.PillBoxDataBinding()

        SetVars("_listaDomicilios", pbDetalleProveedor.DataSource)

    End Sub

    Sub LimpiarTarjetero()

        Dim pillboxControl_ As PillboxControl = pbDetalleProveedor

        pillboxControl_.ClearRows()

        pillboxControl_.SetPillbox(Sub(pillbox_ As PillBox)

                                       pillbox_.SetIndice(pillboxControl_.KeyField, 1)

                                       pillbox_.SetFiled(False)

                                       pillbox_.SetControlValue(icIdTarjeta, Nothing)

                                       pillbox_.SetControlValue(icFirmaTarjeta, Nothing)

                                       pillbox_.SetControlValue(icPais, Nothing)

                                       pillbox_.SetControlValue(icIdPais, Nothing)

                                       pillbox_.SetControlValue(icCvePais, Nothing)

                                       pillbox_.SetControlValue(icCalle, Nothing)

                                       pillbox_.SetControlValue(icNumeroExterior, Nothing)

                                       pillbox_.SetControlValue(icNumeroInterior, Nothing)

                                       pillbox_.SetControlValue(icCodigoPostal, Nothing)

                                       pillbox_.SetControlValue(icColonia, Nothing)

                                       pillbox_.SetControlValue(icLocalidad, Nothing)

                                       pillbox_.SetControlValue(icCiudad, Nothing)

                                       pillbox_.SetControlValue(icMunicipio, Nothing)

                                       pillbox_.SetControlValue(icEntidadFederativa, Nothing)

                                       pillbox_.SetControlValue(icIdDomicilio, Nothing)

                                       pillbox_.SetControlValue(icSecDomicilio, Nothing)

                                       pillbox_.SetControlValue(scDomicilio, Nothing)

                                       pillbox_.SetControlValue(icNumeroExtInt, Nothing)

                                       pillbox_.SetControlValue(icCveMunicipio, Nothing)

                                       pillbox_.SetControlValue(icCveEntidadFederativa, Nothing)

                                   End Sub)

        pbDetalleProveedor = pillboxControl_

        pbDetalleProveedor.PillBoxDataBinding()

        SetVars("_listaDomicilios", Nothing)

    End Sub

    Protected Sub ConfiguracionDomicilioNacional()

        ConfigurarDomicilios.Visible = True

        ConfigurarDomicilios.Enabled = True

        ConfigurarDomicilios.Visible = False

        ConfigurarDomicilios.Enabled = False

        _datospaismexicano = _utils.ObtenerDatosPaisMexicano()

        With _datospaismexicano

            icPais.Value = .paisPresentacion

            icIdPais.Value = .idpais.ToString

            icCvePais.Value = .pais

            SetVars("_idpais", .idpais.ToString)

        End With

        ListarDomiciliosPorPais(IControladorEmpresas.TiposEmpresas.Nacional)

    End Sub

    Protected Sub ListarDomiciliosPorPais(Optional ByVal tipoempresa_ As IControladorEmpresas.TiposEmpresas =
                                          IControladorEmpresas.TiposEmpresas.Nacional)

        If fcRazonSocial.Value <> "" Then

            Dim idpais_ = GetVars("_idpais")

            Dim data_ = _utils.ListaDomiciliosPorPais(fcRazonSocial.Value, idpais_, tipoempresa_)

            If data_.Count > 0 Then

                If data_.Count = 1 Then

                    scDomiciliosRegistrados.DataSource = data_

                    scDomiciliosRegistrados.Value = data_(0).Value

                    SetVars("_domiciliounicoNacional", data_(0).Value)
                    ''UTILIZARE VARIABLE DE SESION YA QUE EL COMPONENTE NO ME ESTA GUARDANDO EL VALOR
                Else

                    scDomiciliosRegistrados.DataSource = data_

                    MsgSeleccioneDomicilioProveedor()
                    'ListaDomiciliosNacionales.Value = data_.Last.Value

                    SetVars("_listaDomiciliosNacionales", data_)
                    'ListaDomiciliosNacionales

                End If

            Else

                scDomiciliosRegistrados.DataSource = Nothing

                scDomiciliosRegistrados.Value = Nothing

                ConfigurarDomicilios.Visible = False

            End If

        Else

            scDomiciliosRegistrados.DataSource = Nothing

            scDomiciliosRegistrados.Visible = False

            ConfigurarDomicilios.Visible = False

            pbDetalleProveedor.Enabled = True

        End If

    End Sub

    Protected Sub RegresarControlesPorDefault()

        Dim modoEditando_ As Boolean = False

        If GetVars("isEditing") IsNot Nothing Then

            If GetVars("isEditing") = True Then

                modoEditando_ = True

            End If

        End If

        If pbDetalleProveedor.PageIndex > 0 Then

            If pbDetalleProveedor.PageIndex > 0 Then

                'lbNumero.Text = pbDetalleProveedor.PageIndex.ToString()

            End If

            Session("_tbDetalleProveedor") = PillboxControl.ToolbarModality.Default : pbDetalleProveedor.Modality = Session("_tbDetalleProveedor")

            If modoEditando_ Then

                PreparaTarjetero(PillboxControl.ToolbarModality.Advanced, pbDetalleProveedor)

                If swcTipoPersona.Checked = False Then

                    icCURP.Visible = True

                End If

                fcRazonSocial.Enabled = False

                icRFC.Enabled = False

                icCURP.Enabled = False

                swcTipoPersona.Enabled = False

            Else

                PreparaTarjetero(PillboxControl.ToolbarModality.Default, pbDetalleProveedor)

                swcTipoPersona.Checked = True

                If swcTipoPersona.Checked = False Then

                    icCURP.Visible = True

                Else

                    icCURP.Visible = False

                End If

            End If

            fsVinculaciones.Enabled = True

            fsConfiguracionAdicional.Enabled = True

            fsHistorialDomicilios.Enabled = True

            scDomiciliosRegistrados.DataSource = Nothing

            icClave.Enabled = False

            fsHistorialDomicilios.Visible = True

            fsVinculaciones.Visible = True

            fsConfiguracionAdicional.Visible = True

            CargarHistorialDomicilios()

        End If

    End Sub

    Protected Function GenerarSecuencia() As Secuencia

        _tagwatcher = New TagWatcher

        _secuencia = New Secuencia

        _controladorSecuencias = New ControladorSecuencia

        _tagwatcher = _controladorSecuencias.Generar(SecuenciasComercioExterior.ProveedoresOperativos.ToString, 1, 1, 1, 1, 1)

        If _tagwatcher.Status = TypeStatus.Ok Then

            _secuencia = DirectCast(_tagwatcher.ObjectReturned, Secuencia)

            _secuencia.nombre = SecuenciasComercioExterior.ProveedoresOperativos.ToString

        End If

        Return _secuencia

    End Function

#Region "Avisos / Tooltips"

    Protected Sub MsgNoExisteEmpresa()

        With fcRazonSocial
            .ToolTip = "👉 Razón social libre."
            .ToolTipExpireTime = 6
            .ToolTipStatus = IUIControl.ToolTipTypeStatus.Ok
            .ToolTipModality = IUIControl.ToolTipModalities.Ondemand
            .ShowToolTip()
        End With

    End Sub

    Protected Sub MsgExisteDestinatario()

        With fcRazonSocial
            .ToolTip = "🔴 Ya existe este destinatario"
            .ToolTipExpireTime = 6
            .ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors
            .ToolTipModality = IUIControl.ToolTipModalities.Ondemand
            .ShowToolTip()
        End With

    End Sub



    Protected Sub MsgValidacionRazonsocial()

        fcRazonSocial.ToolTip = "Este proveedor ya existe. "

        fcRazonSocial.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors

        fcRazonSocial.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

        fcRazonSocial.ShowToolTip()

    End Sub

    Protected Sub MsgValidacionRazonsocialVacio()

        fcRazonSocial.ToolTip = "Indica una razón social. "

        fcRazonSocial.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors

        fcRazonSocial.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

        fcRazonSocial.ShowToolTip()

    End Sub

    Protected Sub MsgValidacionRfcVacio()

        icRFC.ToolTip = "Indica un rfc. "

        icRFC.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors

        icRFC.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

        icRFC.ShowToolTip()

    End Sub

    Protected Sub MsgValidacionCalleVacio()

        icCalle.ToolTip = "Indica una calle. "

        icCalle.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors

        icCalle.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

        icCalle.ShowToolTip()

    End Sub

    Protected Sub MsgValidacionPaisVacio()

        icPais.ToolTip = "Indica un país. "

        icPais.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors

        icPais.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

        icPais.ShowToolTip()

    End Sub

    Protected Sub MsgSeleccioneDomicilioProveedor()
        With scDomiciliosRegistrados
            .ToolTip = "👉 Indica un domicilio. "
            .ToolTipExpireTime = 4
            .ToolTipStatus = IUIControl.ToolTipTypeStatus.OkInfo
            .ToolTipModality = IUIControl.ToolTipModalities.Ondemand
            .ShowToolTip()
        End With
    End Sub

    Protected Sub MsgSeleccioneEntidad()
        With scDomiciliosRegistrados
            .ToolTip = "👉 Indica una entidad federativa"
            .ToolTipExpireTime = 4
            .ToolTipStatus = IUIControl.ToolTipTypeStatus.OkInfo
            .ToolTipModality = IUIControl.ToolTipModalities.Ondemand
            .ShowToolTip()
        End With
    End Sub

    Protected Sub MsgValidacionPaisValido()

        With icPais
            .ToolTip = "🔴 País no válido."
            .ToolTipExpireTime = 4
            .ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors
            .ToolTipModality = IUIControl.ToolTipModalities.Ondemand
            .ShowToolTip()
        End With

    End Sub

    Protected Sub swcHabilitadoProveedor_CheckedChanged(sender As Object, e As EventArgs)

        If swcHabilitadoProveedor.Checked Then

            DisplayMessage("🟣 Proveedor Online", StatusMessage.Info)

            pbDetalleProveedor.Enabled = False

            EstadoConexion()

        Else

            DisplayMessage("⚪ Proveedor Offline", StatusMessage.Info)

            pbDetalleProveedor.Enabled = True

            EstadoConexion()

        End If

        swcTipoPersona.Enabled = False

    End Sub

    Protected Sub icEntidadFederativa_TextChanged(sender As Object, e As EventArgs)

        _lista = New List(Of SelectOption)

        Dim valuePais_ = icIdPais.Value

        If valuePais_ IsNot Nothing Or valuePais_ <> "" Then

            _lista = _utils.ObtenerListaEntidadesFederativas(ObjectId.Parse(valuePais_))

            If _lista.Count > 0 Then

                icEntidadFederativa.DataSource = _lista

            End If

        End If

    End Sub

    Protected Sub icEntidadFederativa_Click(sender As Object, e As EventArgs)
        _lista = New List(Of SelectOption)

        Dim valuePais_ = icIdPais.Value

        If valuePais_ IsNot Nothing Or valuePais_ <> "" Then

            _lista = _utils.ObtenerListaEntidadesFederativas(ObjectId.Parse(valuePais_))

            If _lista.Count > 0 Then

                icEntidadFederativa.DataSource = _lista

            End If

        End If
    End Sub

    'Protected Sub Prueba_Click(sender As Object, e As EventArgs)
    '    Dim aqui = True
    '    'Dim entidadesFederativas_ = New ControladorPaises()
    '    Dim pais_ = ObjectId.Parse("635acf2ba8210bfa0d5843f3")
    '    Dim tag As New TagWatcher
    '    tag = ControladorPaises.ObtenerListaEntidadesFederativas(pais_, "VER")
    '    Dim listaEntidades_ = tag.ObjectReturned
    'End Sub
#End Region
#End Region

End Class

Public Class HistorialDomicilios

    Property _iddomicilio As String

    Property _secdomicilio As Integer

    Property _taxiddomicilio As String

    Property _rfcdomicilio As String

    Property _domiciliofiscal As String

    Property _loginusuario As String

    Property enviromentusuario As Integer

    Property _estadodomicilio As Boolean

    Property _archivadodomicilio As Boolean

    Property _motivoArchivado As String

    Property _fechaarchivado As String

End Class