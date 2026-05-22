
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
'OBJETOS BIDIMENSIONALES (ODF's.  Dependencias Krombase/SQL Server)
Imports Syn.Documento.Componentes.Campo
Imports Gsol
'Imports System.ServiceModel.Channels
Imports Gsol.krom
Imports MongoDB.Bson
'Imports System.Windows.Forms.VisualStyles
Imports Syn.Documento.Componentes
Imports Syn.Nucleo
Imports Syn.CustomBrokers.Controllers
Imports Rec.Globals.Empresas
Imports Rec.Globals.Controllers.Empresas
Imports Rec.Globals.Utils
Imports System.Linq.Expressions
Imports Rec.Globals.Controllers
'Imports SharpCompress.Archives
Imports System.Web.UI.WebControls.Expressions
Imports System.Xml.Serialization
Imports System.Runtime.InteropServices
'Imports System.Windows.Forms
Imports Syn.Utils
#End Region

Public Class Ges022_001_ProveedorExtranjero
    Inherits ControladorBackend
#Region "████████████████████████████████████████   Atributos locales  ██████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████
    Private _cantidadDetalles As Int32

    Private _tagwatcher As TagWatcher

    Private _empresaInternacional As EmpresaInternacional

    Private _controladorEmpresas As Rec.Globals.Controllers.Empresas.IControladorEmpresas

    Private _listaDomicilios As List(Of Rec.Globals.Empresas.Domicilio)

    Private _datosAdicionalesActuales As List(Of Dictionary(Of String, String))

    Private _pillboxControl As PillboxControl

    Private _secuencia As Secuencia

    Private _controladorSecuencias As IControladorSecuencia

    Private _listaEmpresasInternacional As List(Of EmpresaInternacional)

    Private _utils As UtileriaProveedores

    Private _lista As List(Of SelectOption)

    Private _auxiliarProveedor As AuxiliarProveedor

    Private _loginUsuario As Dictionary(Of String, String)
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
            .DataObject = New ConstructorProveedoresOperativos(True)
            .addFilter(SeccionesProvedorOperativo.SPRO1, CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, "Proveedor")
            .addFilter(SeccionesProvedorOperativo.SPRO2, CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR, "TAXID")
        End With

        If OperacionGenerica IsNot Nothing Then

            _cantidadDetalles = OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Seccion(SeccionesProvedorOperativo.SPRO2).CantidadPartidas

        End If

        Dim pillboxControl_ As PillboxControl = pbDetalleProveedorInternacional

        scMetodoValoracion.DataEntity = New krom.Anexo22()

        scIncoterm.DataEntity = New krom.Anexo22()

        icClave.Text = ""

        _utils = New UtileriaProveedores

    End Sub

    'ASIGNACION PARA CONTROLES AUTOMÁTICOS
    Public Overrides Function Configuracion() As TagWatcher
        'Datos generales (SeccionesProveedorOperativo.SPRO1)

        [Set](fcRazonSocial, CA_RAZON_SOCIAL_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Text)

        [Set](swcHabilitado, CamposProveedorOperativo.CP_PROVEEDOR_HABILITADO, propiedadDelControl_:=PropiedadesControl.Checked)

        'Detalle proveedor
        If pbDetalleProveedorInternacional.PageIndex > 0 Then

            lbNumero.Text = pbDetalleProveedorInternacional.PageIndex.ToString()

        End If

        [Set](icTaxid, CA_TAX_ID_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icIdTarjeta, CamposProveedorOperativo.CP_ID_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icFirmaTarjeta, CamposProveedorOperativo.CP_FIRMA_ELECTRONICA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icCveTaxid, CA_CVE_TAX_ID_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icIdDomicilio, CamposProveedorOperativo.CP_ID_DOMICILIO_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icSecDomicilio, CamposProveedorOperativo.CP_SEC_DOMICILIO_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)

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

        [Set](icMunicipio, CamposDomicilio.CA_MUNICIPIO, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icCveMunicipio, CamposDomicilio.CA_ENTIDAD_MUNICIPIO, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icCveEntidadFederativa, CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icEntidadFederativa, CamposDomicilio.CA_ENTIDAD_FEDERATIVA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icestadoproveedor, CamposProveedorOperativo.CA_ESTADO_DOMICILIO_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icdomicilioarchivadoproveedor, CamposProveedorOperativo.CA_DOMICILIO_ARCHIVADO_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icmotivoarchivadoproveedor, CamposProveedorOperativo.CA_MOTIVO_ARCHIVADO_DOMICILIO_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](fechaarchivadoproveedor, CamposProveedorOperativo.CA_FECHA_ARCHIVADO_DOMICILIO_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](pbDetalleProveedorInternacional, Nothing, seccion_:=SeccionesProvedorOperativo.SPRO2)

        ' Vinculaciones con clientes (SeccionesProvedorOperativo.SPRO4)
        [Set](scClienteVinculacion, CamposProveedorOperativo.CP_ID_CLIENTE_VINCULACION, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scTaxIdVinculacion, CamposProveedorOperativo.CP_TAX_ID_VINCULACION, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scVinculacion, CamposProveedorOperativo.CA_CVE_VINCULACION, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icPorcentajeVinculacion, CamposProveedorOperativo.CP_PORCENTAJE_VINCULACION, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](ccVinculaciones, Nothing, seccion_:=SeccionesProvedorOperativo.SPRO4)


        'Configuración adicional (SeccionesProvedorOperativo.SPRO5)
        [Set](scClienteConfiguracion, CamposProveedorOperativo.CP_ID_CLIENTE_CONFIGURACION, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scTaxIdConfiguracion, CamposProveedorOperativo.CP_TAX_ID_CONFIGURACION, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scMetodoValoracion, CamposProveedorOperativo.CA_CVE_METODO_VALORACION, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scIncoterm, CamposProveedorOperativo.CA_CVE_INCOTERM, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](ccConfiguracionAdicional, Nothing, seccion_:=SeccionesProvedorOperativo.SPRO5)

        EstadoConexion()

        Return New TagWatcher(1)

    End Function

    Public Overrides Sub BotoneraClicNuevo()
        If OperacionGenerica IsNot Nothing Then
        End If

        LimpiaSesion()

        If pbDetalleProveedorInternacional.PageIndex > 0 Then

            lbNumero.Text = pbDetalleProveedorInternacional.PageIndex.ToString()

        End If

        PreparaTarjetero(PillboxControl.ToolbarModality.Simple, pbDetalleProveedorInternacional)

        fsVinculaciones.Visible = False

        fsConfiguracionAdicional.Visible = False

        fsHistorialDomicilios.Visible = False

        icPais.Enabled = False

        ConfigurarDomicilios.Visible = True

        ConfigurarDomicilios.Enabled = True

        pbDetalleProveedorInternacional.Enabled = False

        swcHabilitado.Checked = False

        fcRazonSocial.Enabled = True

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

            ElseIf icCalle.Value = "" Then

                MsgValidacionCalleVacio()

            ElseIf icPais.Value = "" Then

                MsgValidacionPaisVacio()

            Else

                If BuscarSiExisterProveedor() Then

                    'aviso.Visible = True
                    DisplayMessage("Proveedor ya registrado.", StatusMessage.Fail)

                Else

                    If Not ProcesarTransaccion(Of ConstructorProveedoresOperativos)().Status = TypeStatus.Errors Then : End If

                End If

            End If

        Else

            If Not ProcesarTransaccion(Of ConstructorProveedoresOperativos)().Status = TypeStatus.Errors Then : End If

        End If

    End Sub

    Public Overrides Sub BotoneraClicEditar()

        PreparaTarjetero(PillboxControl.ToolbarModality.Advanced, pbDetalleProveedorInternacional)

        icClave.Enabled = False

        fcRazonSocial.Enabled = False

        icPais.Enabled = False

    End Sub

    Public Overrides Sub BotoneraClicBorrar()

    End Sub

    Public Overrides Sub BotoneraClicOtros(IndexSelected_ As Integer)

        'Dim itemActual_ As Integer = pbDetalleProveedorInternacional.PageIndex

        'If IndexSelected_ = 7 Then

        '    ConfigurarDomicilios.Visible = True

        '    'AQUI
        'ElseIf IndexSelected_ = 8 Then

        '    VaciarFormulario(itemActual_)

        'End If

    End Sub

    Private Sub VaciarFormulario(ByVal indice_ As Integer)
        Dim pillboxControl_ As PillboxControl = pbDetalleProveedorInternacional

        pillboxControl_.SetPillbox(Sub(pillbox_ As PillBox)

                                       pillbox_.SetIndice(pillboxControl_.KeyField, indice_)

                                       pillbox_.SetFiled(False)

                                       icTaxid.Value = Nothing

                                       icCveTaxid.Value = Nothing

                                       icCalle.Value = Nothing

                                       icNumeroExterior.Value = Nothing

                                       icNumeroInterior.Value = Nothing

                                       icCodigoPostal.Value = Nothing

                                       icColonia.Value = Nothing

                                       icLocalidad.Value = Nothing

                                       icCiudad.Value = Nothing

                                       icMunicipio.Value = Nothing

                                       icEntidadFederativa.Value = Nothing

                                       scDomicilio.Value = Nothing

                                   End Sub)
    End Sub

    'EVENTOS PARA LA INSERCIÓN DE DATOS
    Public Overrides Function AntesRealizarInsercion(ByVal session_ As IClientSessionHandle) As TagWatcher

        Dim tagwatcher_ As New TagWatcher

        If session_ IsNot Nothing Then

            GuardarEmpresaInternacional(session_, esempresanueva_:=True)

            tagwatcher_.SetOK()

        Else

            tagwatcher_.SetOK()

        End If

        Return tagwatcher_

    End Function

    Public Overrides Sub RealizarInsercion(ByRef documentoElectronico_ As DocumentoElectronico)

        _tagwatcher = New TagWatcher

        _empresaInternacional = New EmpresaInternacional

        If GetVars("_empresaInternacional") IsNot Nothing Then

            _empresaInternacional = DirectCast(GetVars("_empresaInternacional"), EmpresaInternacional)

        End If

        _loginUsuario = New Dictionary(Of String, String)

        _loginUsuario = Session("DatosUsuario")

        _secuencia = New Secuencia

        _secuencia = _utils.GenerarSecuencia(SecuenciasComercioExterior.ProveedoresOperativos)

        With documentoElectronico_

            .Id = _secuencia._id.ToString

            .Campo(CamposProveedorOperativo.CP_ID_PROVEEDOR).Valor = _secuencia._id

            icClave.Text = _secuencia.sec

            .Campo(CamposProveedorOperativo.CP_ID_EMPRESA).Valor = _empresaInternacional._id

            .Campo(CamposProveedorOperativo.CP_CVE_EMPRESA).Valor = _empresaInternacional._idempresa

            .Campo(CamposProveedorOperativo.CP_CVE_PROVEEDOR).Valor = _secuencia.sec

            .Campo(CamposProveedorOperativo.CP_TIPO_PROVEEDOR).Valor = False

            .Campo(CamposProveedorOperativo.CP_TIPO_PROVEEDOR).ValorPresentacion = "PROVEEDOR EXTRANJERO"

            .Campo(CamposProveedorOperativo.CP_PROVEEDOR_HABILITADO).Valor = False

            .Campo(CamposProveedorOperativo.CP_PROVEEDOR_HABILITADO).ValorPresentacion = "Offline"

            .UsuarioGenerador = _loginUsuario("Nombre")

            .IdDocumento = _secuencia.sec

            .FolioDocumento = _secuencia.sec 'DUDA

            .FolioOperacion = _secuencia.sec 'DUDA

            .TipoPropietario = _secuencia.nombre

            .NombrePropietario = _empresaInternacional.razonsocial

            .IdPropietario = _empresaInternacional._idempresa

            .ObjectIdPropietario = _empresaInternacional._id

        End With

        Dim controladorFirma_ As New ControladorFirmaElectronica

        pbDetalleProveedorInternacional.ForEach(Sub(pillbox_ As PillBox)
                                                    Dim id_ = ObjectId.GenerateNewId()
                                                    pbDetalleProveedorInternacional.setValueInvisible(icIdTarjeta, pillbox_.GetIdentity, id_)
                                                    pbDetalleProveedorInternacional.setValueInvisible(icFirmaTarjeta, pillbox_.GetIdentity, controladorFirma_.Generar(id_, 1))
                                                End Sub)

    End Sub

    Public Overrides Function DespuesRealizarInsercion() As TagWatcher

        RegresarControlesPorDefault()

        swcHabilitado.Visible = True

        fcRazonSocial.Enabled = False

        Return New TagWatcher(Ok)

    End Function

    'EVENTOS PARA MODIFICACIÓN DE DATOS
    Public Overrides Function AntesRealizarModificacion(ByVal session_ As IClientSessionHandle) As TagWatcher

        Dim tagwatcher_ As New TagWatcher

        If session_ IsNot Nothing Then '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas con transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 

            GuardarEmpresaInternacional(session_)

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

            _cantidadDetalles = documentoElectronico_.Seccion(SeccionesProvedorOperativo.SPRO2).CantidadPartidas

        End If

        ''BUSCAR LA EMPRESA ACTUAL
        _tagwatcher = New TagWatcher

        _empresaInternacional = New EmpresaInternacional

        Dim cvePais_ As String = icPais.Value.Substring(0, 3)

        Try

            _tagwatcher = _utils.BuscarEmpresaPorObjectId(idempresa_.ToString)

            If _tagwatcher.Status = TypeStatus.Ok Then

                _empresaInternacional = DirectCast(_tagwatcher.ObjectReturned, EmpresaInternacional)

                Dim listaempresasinternacionales_ As New List(Of EmpresaInternacional) From {_empresaInternacional}

                SetVars("_empresaInternacional", _empresaInternacional)

                SetVars("_listaempresastemporales", listaempresasinternacionales_)

            End If

            CargarHistorialDomicilios()

            ConfigurarDomicilios.Enabled = False

            ConfigurarDomicilios.Visible = False

        Catch ex As Exception

            DisplayMessage("Favor de intentarlo más tarde", StatusMessage.Fail)

        End Try

    End Sub

    Public Overrides Sub RealizarModificacion(ByRef documentoElectronico_ As DocumentoElectronico)

        'pbDetalleProveedorInternacional.ForEach(Sub(pillbox_ As PillBox)
        '                                            pbDetalleProveedorInternacional.setValueInvisible(icIdTarjeta, pillbox_.GetIdentity, ObjectId.GenerateNewId())
        '                                        End Sub)

        Dim controladorFirma_ As New ControladorFirmaElectronica

        Dim documento_ = documentoElectronico_

        pbDetalleProveedorInternacional.ForEach(Sub(pillbox_ As PillBox)

                                                    Dim cambios_ = False

                                                    If pillbox_.GetControlValue(icIdTarjeta) Is Nothing Then

                                                        Dim id_ = ObjectId.GenerateNewId()

                                                        pbDetalleProveedorInternacional.setValueInvisible(icIdTarjeta, pillbox_.GetIdentity, id_)

                                                        pbDetalleProveedorInternacional.setValueInvisible(icFirmaTarjeta, pillbox_.GetIdentity, controladorFirma_.Generar(id_, 1))

                                                    Else

                                                        validaCambiosTarjeta(pillbox_, documento_, cambios_)

                                                    End If

                                                    If cambios_ Then

                                                        pbDetalleProveedorInternacional.setValueInvisible(icFirmaTarjeta, pillbox_.GetIdentity, controladorFirma_.Generar(ObjectId.Parse(pillbox_.GetControlValue(icIdTarjeta)), 1))

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

                If swcHabilitado.Checked Then

                    .Campo(CamposProveedorOperativo.CP_PROVEEDOR_HABILITADO).Valor = True

                    .Campo(CamposProveedorOperativo.CP_PROVEEDOR_HABILITADO).ValorPresentacion = "Online"

                Else

                    .Campo(CamposProveedorOperativo.CP_PROVEEDOR_HABILITADO).Valor = False

                    .Campo(CamposProveedorOperativo.CP_PROVEEDOR_HABILITADO).ValorPresentacion = "Offline"

                End If

                SetVars("tipoProveedor_", .Campo(CamposProveedorOperativo.CP_TIPO_PROVEEDOR).Valor)

            End If

            _empresaInternacional = New EmpresaInternacional

            If GetVars("_empresaInternacional") IsNot Nothing Then

                _empresaInternacional = DirectCast(GetVars("_empresaInternacional"), EmpresaInternacional)

            End If

            If GetVars("_auxiliarproveedor") IsNot Nothing Then

                _auxiliarProveedor = DirectCast(GetVars("_auxiliarproveedor"), AuxiliarProveedor)

            End If

            'LISTA DOMICILIOS PROVEEDORES
            Dim proveedores_ = pbDetalleProveedorInternacional.DataSource

            Dim domiciliosproveedor_ = pbDetalleProveedorInternacional.DataSource

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

                            .Campo(CamposProveedorOperativo.CA_CVE_TAX_ID_PROVEEDOR).Valor = _auxiliarProveedor._listadomiciliosconTaxid(indice_).clavetaxid

                            .Campo(CamposProveedorOperativo.CA_DOMICILIO_FISCAL).Valor = _auxiliarProveedor._listadomiciliosconTaxid(indice_).domicilioPresentacion

                            .Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor = $"{_auxiliarProveedor._listadomiciliosconTaxid(indice_).numeroexterior} -  {_auxiliarProveedor._listadomiciliosconTaxid(indice_).numerointerior}"

                            .Campo(CamposDomicilio.CA_ENTIDAD_MUNICIPIO).Valor = _auxiliarProveedor._listadomiciliosconTaxid(indice_).cveMunicipio

                            .Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor = _auxiliarProveedor._listadomiciliosconTaxid(indice_).cveEntidadfederativa

                            .Campo(CamposProveedorOperativo.CA_ESTADO_DOMICILIO_PROVEEDOR).Valor = CBool(_auxiliarProveedor._listadomiciliosconTaxid(indice_).estado)

                            .Campo(CamposProveedorOperativo.CA_DOMICILIO_ARCHIVADO_PROVEEDOR).Valor = _auxiliarProveedor._listadomiciliosconTaxid(indice_).archivado

                            i_ += 1

                            indice_ += 1

                        End With

                    Next

                End If

            End If

            ''HACER UNA LISTA PARA OBTENER SOLO LOS DOMICILIOS ARCHIVADOS Y SOLO ESO MOSTRAR

        End With

    End Sub

    Public Overrides Function DespuesRealizarModificacion() As TagWatcher

        RegresarControlesPorDefault()

        Return New TagWatcher(Ok)

    End Function

    'EVENTOS PARA PRESENTACIÓN DE DATOS EN FRONTEND
    Public Overrides Sub DespuesBuquedaGeneralConDatos()

        fsVinculaciones.Visible = IIf(_cantidadDetalles > 0, True, False)

        fsConfiguracionAdicional.Visible = IIf(_cantidadDetalles > 0, True, False)

        fsHistorialDomicilios.Visible = IIf(_cantidadDetalles > 0, True, False)

        PreparaTarjetero(PillboxControl.ToolbarModality.Default, pbDetalleProveedorInternacional)

        fcRazonSocial.Enabled = False

        With OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

            With .Seccion(SeccionesProvedorOperativo.SPRO1)

                fcRazonSocial.Value = .Campo(CP_ID_EMPRESA).Valor.ToString

                icClave.Text = ""

                icClave.Text = .Campo(CamposProveedorOperativo.CP_CVE_PROVEEDOR).Valor

                swcHabilitado.Visible = True

                swcHabilitado.Checked = .Campo(CP_PROVEEDOR_HABILITADO).Valor

                Dim datosproveedoractual_ = _utils.ObtenerDatosProveedorDesdeControlador(OperacionGenerica.Id)

                SetVars("_auxiliarproveedor", datosproveedoractual_)

            End With

        End With

        EstadoConexion()

    End Sub

    Public Overrides Sub DespuesBuquedaGeneralSinDatos()

        If OperacionGenerica IsNot Nothing Then

            With OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

                With .Seccion(SeccionesProvedorOperativo.SPRO1)

                    icClave.Text = ""

                    icClave.Text = .Campo(CamposProveedorOperativo.CP_CVE_PROVEEDOR).Valor

                End With

            End With

            Session("_tbDetalleProveedor") = PillboxControl.ToolbarModality.Default : pbDetalleProveedorInternacional.Modality = Session("_tbDetalleProveedor")

            PreparaTarjetero(PillboxControl.ToolbarModality.Default, pbDetalleProveedorInternacional)

            pbDetalleProveedorInternacional.Enabled = True

            fcRazonSocial.Enabled = False

            Dim datosproveedoractual_ = _utils.ObtenerDatosProveedorDesdeControlador(OperacionGenerica.Id)

            SetVars("_auxiliarproveedor", datosproveedoractual_)

            EstadoConexion()

        End If

    End Sub
    'EVENTOS DE MANTENIMIENTO
    Public Overrides Sub LimpiaSesion()

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

        _tagwatcher = Nothing

        _empresaInternacional = Nothing

        If _controladorEmpresas IsNot Nothing Then

            _controladorEmpresas.ReiniciarControlador()

            _controladorEmpresas = Nothing

        End If

        _listaDomicilios = Nothing

        _datosAdicionalesActuales = Nothing

        _pillboxControl = Nothing

        ccDomiciliosFiscales.DataSource = Nothing

        ccVinculaciones.DataSource = Nothing

        ccConfiguracionAdicional.DataSource = Nothing

        _cantidadDetalles = Nothing

        scDomiciliosRegistrados.DataSource = Nothing

        scDomiciliosRegistrados.Value = Nothing

        'pbDetalleProveedorInternacional.DataSource = Nothing

        icClave.Text = ""

        swcHabilitado.Checked = False

        swcHabilitado.Visible = False

    End Sub
#End Region

#Region "████████████████  QUINTA CAPA  █████████       Reglas locales         ██████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████
    'EVENTO PARA REGRESAR CONTROLES POR CADA ACCIÓN DE TARJETA
    Public Sub RegresarControles(Optional ByVal opcion_ As Int32 = 1)

        If pbDetalleProveedorInternacional.PageIndex > 0 Then

            lbNumero.Text = pbDetalleProveedorInternacional.PageIndex.ToString()

        End If

    End Sub

#Region "Buscar empresas por razon social"
    'EVENTOS PARA LA RAZON SOCIAL
    Protected Sub fcRazonSocial_TextChanged(sender As Object, e As EventArgs)

        _lista = New List(Of SelectOption)


        _lista = _utils.ListarEmpresasPorRazonSocial(fcRazonSocial.Text)


        If _lista.Count > 0 Then

            fcRazonSocial.DataSource = _lista

        Else

            MsgNoExisteEmpresa()

        End If

    End Sub

    Protected Sub fcRazonSocial_Click(sender As Object, e As EventArgs)

        If fcRazonSocial.Text <> "" Then

            If BuscarSiExisterProveedor() Then

                MsgValidacionRazonsocial()

            End If

            ConfigurarDomicilios.Visible = True
        Else

            aviso.Visible = False

            SetVars("_listaEmpresasTemporales", Nothing)

            SetVars("_opcionesLista", Nothing)

            Limpiar()

            LimpiarTarjetero()

            icPais.Value = Nothing

            fcPaises.Value = Nothing

            fcPaises.Text = Nothing

            scDomiciliosRegistrados.Value = Nothing

            scDomiciliosRegistrados.Visible = False

            lbtitleDomicilios.Visible = False

            pbDetalleProveedorInternacional.Enabled = False

            ConfigurarDomicilios.Visible = False

        End If

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

                'aviso.Visible = True
                'MsgValidacionRazonsocial()
                Return True

            End If

        End If

        Return False

    End Function

    Protected Sub fcPaises_Click(sender As Object, e As EventArgs)

        If fcPaises.Text <> "" Then

            scDomiciliosRegistrados.DataSource = Nothing

            scDomiciliosRegistrados.Visible = True

            ListarDomiciliosPorPais()

        Else

            scDomiciliosRegistrados.DataSource = Nothing

            scDomiciliosRegistrados.Visible = False

            icPais.Value = Nothing

            icCvePais.Value = Nothing

            icIdPais.Value = Nothing

        End If

    End Sub

    Protected Sub fcPaises_TextChanged(sender As Object, e As EventArgs)

        CargaPaises(sender)

    End Sub

    Function CargaPaises(ByRef control_ As FindboxControl) As List(Of SelectOption)

        Dim paisesTemporales_ As New List(Of Pais)

        Dim lista_ As List(Of SelectOption) = ControladorPaises.BuscarPaises(paisesTemporales_, control_.Text)

        control_.DataSource = lista_

        Return lista_

    End Function

    'EVENTOS PARA CONTROLAR QUE PASA CON EL TIPO DE USO
    Protected Sub swcTipoUso_CheckedChanged(sender As Object, e As EventArgs)

    End Sub

    'EVENTOS PARA CONTROLAR QUE PASA CON EL TIPO DE PERSONA
    Protected Sub swcDestinatario_CheckedChanged(sender As Object, e As EventArgs)

    End Sub

    Sub VerificaCheckTipoUso(Optional ByVal opcion_ As Int32 = 1)

    End Sub

    'EVENTOS PARA CARGAR LOS CLIENTES EN LAS LISTAS
    Protected Sub scClienteVinculacion_Click(sender As Object, e As EventArgs)

        Dim controladorCliente_ As New ControladorClientes

        Dim tw_ = controladorCliente_.Consultar(scClienteVinculacion.SuggestedText)

        Dim lista_ As New List(Of SelectOption)

        If tw_.Status = TypeStatus.Ok Then

            Dim clientes_ = tw_.ObjectReturned

            For Each cliente_ As OperacionGenerica In clientes_

                With cliente_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

                    Dim selectOption_ As New SelectOption

                    selectOption_.Value = cliente_.Id.ToString

                    selectOption_.Text = .NombreCliente

                    lista_.Add(selectOption_)

                End With

            Next

        End If

        'Dim controlador_ As New ControladorBusqueda(Of ConstructorCliente)

        'Dim lista_ As List(Of SelectOption) = controlador_.Buscar(scClienteVinculacion.SuggestedText, New Filtro _
        '                                                          With {.IdSeccion = SeccionesClientes.SCS1, .IdCampo = CamposClientes.CA_RAZON_SOCIAL})

        scClienteVinculacion.DataSource = lista_

    End Sub

    Protected Sub scClienteVinculacion_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub scClienteConfiguracion_Click(sender As Object, e As EventArgs)

        Dim controladorCliente_ As New ControladorClientes

        Dim tw_ = controladorCliente_.Consultar(scClienteConfiguracion.SuggestedText)

        Dim lista_ As New List(Of SelectOption)

        If tw_.Status = TypeStatus.Ok Then

            Dim clientes_ = tw_.ObjectReturned

            For Each cliente_ As OperacionGenerica In clientes_

                With cliente_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

                    Dim selectOption_ As New SelectOption

                    selectOption_.Value = cliente_.Id.ToString

                    selectOption_.Text = .NombreCliente

                    lista_.Add(selectOption_)

                End With

            Next

        End If

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

        pbDetalleProveedorInternacional.ForEach(Sub(pillbox_ As PillBox)
                                                    listaIdentificadores_.Add(New SelectOption With {
                                                                 .Text = pillbox_.GetControlValue(icTaxid),
                                                                 .Value = pillbox_.GetIndice(pbDetalleProveedorInternacional.KeyField)})
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

        Select Case pbDetalleProveedorInternacional.ToolbarAction

            Case PillboxControl.ToolbarActions.Nuevo

                With pbDetalleProveedorInternacional

                    lbNumero.Text = .PageIndex.ToString()

                    Dim itemActual_ As Integer = .PageIndex

                    Dim index_ As Integer = itemActual_ - 2

                    ConfigurarDomicilios.Enabled = True

                    ConfigurarDomicilios.Visible = True

                    icTaxid.Value = ""

                End With

            Case Else

        End Select

    End Sub

    Protected Sub ConfiguradorDomicilios(ByVal indice_ As Integer, ByVal pais_ As String)

        'If GetVars("_empresaInternacional") IsNot Nothing Then

        '    _empresaInternacional = New EmpresaInternacional

        '    _empresaInternacional = DirectCast(GetVars("_empresaInternacional"), EmpresaInternacional)

        '    Dim opcionesLista_ As New List(Of SelectOption)

        '    Dim listaCvesDomiciliosActuales As New List(Of String)

        '    pbDetalleProveedorInternacional.ForEach(Sub(pillbox_ As PillBox)
        '                                                If pillbox_.GetControlValue(icIdDomicilio) IsNot Nothing Then

        '                                                    listaCvesDomiciliosActuales.Add(pillbox_.GetControlValue(icIdDomicilio))

        '                                                End If

        '                                            End Sub)

        '    Dim domiciliosDesdeEmpresa_ As List(Of Rec.Globals.Empresas.Domicilio) = _empresaInternacional.paisesdomicilios.Where(Function(x) x.pais = pais_).
        '                                                                                                                    SelectMany(Function(y) y.domicilios).ToList()

        '    For Each item_ In domiciliosDesdeEmpresa_

        '        If Not listaCvesDomiciliosActuales.Contains(item_._iddomicilio.ToString) Then

        '            opcionesLista_.Add(New SelectOption With
        '                {
        '                    .Value = item_._iddomicilio.ToString,
        '                    .Text = item_.domicilioPresentacion
        '                })

        '        End If

        '    Next

        '    If opcionesLista_.Count > 0 Then

        '        ConfigurarDomicilios.Visible = True

        '        ConfigurarDomicilios.Enabled = True

        '        opcionesLista_.Add(New SelectOption With {
        '                        .Value = "DOMICILIO NUEVO",
        '                        .Text = "DOMICILIO NUEVO"
        '                       })

        '        Dim modoEditando_ As Boolean = False

        '        If GetVars("isEditing") IsNot Nothing Then

        '            If GetVars("isEditing") = True Then

        '                modoEditando_ = True

        '            End If

        '        End If

        '        SetVars("_opcionesLista", opcionesLista_)

        '        SetVars("_indice", indice_)

        '        SetVars("_cveUltimoDomicilio", opcionesLista_.Last.Value)

        '        If modoEditando_ Then

        '            If opcionesLista_.Count > 0 Then

        '                lbtitleDomicilios.Visible = True

        '                scDomiciliosRegistrados.Visible = True

        '            End If

        '        End If

        '        scDomiciliosRegistrados.DataSource = opcionesLista_

        '        scDomiciliosRegistrados.Value = opcionesLista_.Last.Value

        '    End If

        'Else

        '    ConfigurarDomicilios.Visible = False

        '    scDomiciliosRegistrados.DataSource = Nothing

        '    SetVars("_opcionesLista", Nothing)

        '    SetVars("_indice", Nothing)

        '    SetVars("_cveUltimoDomicilio", Nothing)

        'End If

        '
        If GetVars("_empresaInternacional") IsNot Nothing Then


            _empresaInternacional = New EmpresaInternacional

            ''AQUI OBTIENES LA EMPRESA ORIGINAL Y VIRGEN
            _empresaInternacional = DirectCast(GetVars("_empresaInternacional"), EmpresaInternacional)

            Dim opcionesLista_ As New List(Of SelectOption)

            Dim listaCvesDomiciliosActuales As New List(Of String)

            ' Obtener claves de domicilios ya visibles VAMOS POR LOS DOMICILIOS DE ESTE PROVEEDORE CON EL CONTROLADOR DE PROVEEDORES
            ''UTILIZA EL DOCUMENTO ELECTRONICO PORFI
            'pbDetalleProveedorInternacional.ForEach(Sub(pillbox_ As PillBox)
            '                                            If pillbox_.GetControlValue(icIdDomicilio) IsNot Nothing Then
            '                                                listaCvesDomiciliosActuales.Add(pillbox_.GetControlValue(icIdDomicilio))
            '                                            End If
            '                                        End Sub)


            Dim domiciliosProveedorActuales = OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Seccion(SeccionesProvedorOperativo.SPRO2).Nodos

            For Each item_ In domiciliosProveedorActuales

                Dim aqqiuquiq = item_

                ' listaCvesDomiciliosActuales.Add(item_)
                'item_.Campo(CamposProveedorOperativo.CP_ID_DOMICILIO_PROVEEDOR).Valor

            Next

            ' Normalizar nombre del país seleccionado
            Dim paisNormalizado As String = pais_.Trim().ToUpper()

            ' Buscar el país en la lista
            Dim paisSeleccionado = _empresaInternacional.paisesdomicilios.
                FirstOrDefault(Function(x) x.pais IsNot Nothing AndAlso x.pais.Trim().ToUpper() = paisNormalizado)

            If paisSeleccionado IsNot Nothing AndAlso paisSeleccionado.domicilios IsNot Nothing Then

                For Each domicilio_ In paisSeleccionado.domicilios
                    If Not listaCvesDomiciliosActuales.Contains(domicilio_._iddomicilio.ToString) Then
                        opcionesLista_.Add(New SelectOption With {
                            .Value = domicilio_._iddomicilio.ToString,
                            .Text = domicilio_.domicilioPresentacion
                        })
                    End If
                Next

            End If

            ' Si hay domicilios disponibles
            If opcionesLista_.Count > 0 Then

                ConfigurarDomicilios.Visible = True

                ConfigurarDomicilios.Enabled = True

                ' Agregar opción para nuevo domicilio
                opcionesLista_.Add(New SelectOption With {
                    .Value = "DOMICILIO NUEVO",
                    .Text = "DOMICILIO NUEVO"
                })

                ' Revisar si está en modo edición
                Dim modoEditando_ As Boolean = False

                If GetVars("isEditing") IsNot Nothing AndAlso CBool(GetVars("isEditing")) Then
                    modoEditando_ = True
                End If

                SetVars("_opcionesLista", opcionesLista_)

                SetVars("_indice", indice_)

                SetVars("_cveUltimoDomicilio", opcionesLista_.Last().Value)

                If modoEditando_ Then

                    lbtitleDomicilios.Visible = True

                    scDomiciliosRegistrados.Visible = True

                End If

                scDomiciliosRegistrados.DataSource = opcionesLista_

                scDomiciliosRegistrados.Value = opcionesLista_.Last().Value

            Else
                ' No hay domicilios disponibles
                ConfigurarDomicilios.Visible = False

                scDomiciliosRegistrados.DataSource = Nothing

                SetVars("_opcionesLista", Nothing)

                SetVars("_indice", Nothing)

                SetVars("_cveUltimoDomicilio", Nothing)

            End If

            'Else
            '    ' Empresa no encontrada
            '    ConfigurarDomicilios.Visible = False
            '    scDomiciliosRegistrados.DataSource = Nothing
            '    SetVars("_opcionesLista", Nothing)
            '    SetVars("_indice", Nothing)
            '    SetVars("_cveUltimoDomicilio", Nothing)
        End If


    End Sub

    Protected Sub scDomicilios_SelectedIndexChanged(sender As Object, e As EventArgs)
        'Aqui vamos a determinar si vamos por los domiclios a mongo, que yo creo que si, woa a ver
    End Sub

    Protected Sub scDomicilios_Click(sender As Object, e As EventArgs)

    End Sub

    Protected Sub icMunicipio_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub icEntidadFederativa_TextChanged(sender As Object, e As EventArgs)

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

        If fcPaises.Text <> "" Then

            If modoEditando_ Then

                LlenandoTarjetero()

            Else

                If fcRazonSocial.Value <> "" Then

                    If BuscarSiExisterProveedor() = False Then

                        Dim datosempresaseleccionada_ = _utils.BuscarEmpresaPorObjectId(fcRazonSocial.Value)
                        ''checa si es empresaExtranjera
                        SetVars("_empresaInternacional", datosempresaseleccionada_.ObjectReturned)

                        LlenandoTarjetero()

                        MsgIndicaTaxid()

                    End If

                Else

                    ''SI NO LA CREAMOS VACIA YA QUE ES NUEVA NUEVA LA EMRPESA
                    'MsgValidacionRazonsocialVacio()

                    SetVars("_empresaInternacional", Nothing)

                    ''CERRAR EL SUGERENCIA DEL DOMICILIO
                    ConfigurarDomicilios.Visible = False

                    ''PONER EN LOS INPUTS LOS DATOS DEL PAIS
                    icPais.Value = fcPaises.Text
                    icIdPais.Value = fcPaises.Value
                    icCvePais.Value = fcPaises.Text.Substring(0, 3)

                    ''HABILIATAR EL PILLBOX
                    pbDetalleProveedorInternacional.Enabled = True

                    ''PONDRÉ UN MENSAJE PARA INDICAR TAXID SI ES QUE LLEVA
                    MsgIndicaTaxid()

                    ''LIMPIAR LOS DATOS
                    fcPaises.Value = Nothing
                    fcPaises.DataSource = Nothing

                End If

            End If

        Else

            If fcRazonSocial.Text = "" Then

                MsgValidacionRazonsocialVacio()

            End If

            MsgValidacionPais()

        End If

    End Sub

    Protected Sub LlenandoTarjetero()

        Dim modoEditando_ As Boolean = False

        If GetVars("isEditing") IsNot Nothing Then

            If GetVars("isEditing") = True Then

                modoEditando_ = True

            End If

        End If

        icPais.Value = fcPaises.Text

        icCvePais.Value = fcPaises.Text.Substring(0, 3)

        ''VALIDAR QUE SE LLENE ESTE CAMPO, SINO QUIERE DECIR QUE NO EXISTE Y POR LO TANTO ENVIAREMOS UNA INFORMACION
        If fcPaises.Value <> "" Then

            icIdPais.Value = fcPaises.Value

            pbDetalleProveedorInternacional.Enabled = True

            If icIdTarjeta.Value Is Nothing Then

                Dim controladorFirma_ As New ControladorFirmaElectronica

                Dim id_ = ObjectId.GenerateNewId()

                icIdTarjeta.Value = id_.ToString

                icFirmaTarjeta.Value = controladorFirma_.Generar(id_, 1)

            End If

            Dim domicilioSeleccionado_ As String = scDomiciliosRegistrados.Value

                Dim indice_ As Integer = pbDetalleProveedorInternacional.PageIndex

                If modoEditando_ Then

                    If indice_ <> 0 Then

                        If domicilioSeleccionado_ = "" Then

                            ConfigurarDomicilios.Visible = False

                        Else

                            CambiarDomicilio(domicilioSeleccionado_, indice_)

                            ConfigurarDomicilios.Visible = False

                        End If

                    End If

                Else

                    If scDomiciliosRegistrados.Visible = True Then

                        If domicilioSeleccionado_ = "" Or domicilioSeleccionado_ = "DOMICILIO NUEVO" Then

                            LimpiarTarjetero()

                        Else

                            domicilioSeleccionado_ = scDomiciliosRegistrados.Value

                            CambiarDomicilio(domicilioSeleccionado_, indice_)

                        End If

                    End If

                    ConfigurarDomicilios.Visible = False

                End If

            Else

                MsgValidacionPaisValidoControl()

            MsgValidacionPaisValido()

        End If

    End Sub

    'Protected Sub MsgValidacionRazonsocial()
    '    fcRazonSocial.ToolTip = "Debes indicar una razón social. "
    '    fcRazonSocial.ToolTipExpireTime = 4
    '    fcRazonSocial.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors
    '    fcRazonSocial.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
    '    fcRazonSocial.ShowToolTip()
    'End Sub

    Protected Sub MsgValidacionPais()
        fcPaises.ToolTip = "Indica un país. "
        fcPaises.ToolTipExpireTime = 4
        fcPaises.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors
        fcPaises.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
        fcPaises.ShowToolTip()
    End Sub


    'Protected Sub scDomiciliosRegistrados_SelectedIndexChanged(sender As Object, e As EventArgs)
    '    MsgBox("aqui 1")
    'End Sub

    Protected Sub scDomiciliosRegistrados_TextChanged(sender As Object, e As EventArgs)
        'MsgBox("aqui 2")
        'Dim opcionesLista_ = DirectCast(GetVars("_opcionesLista"), List(Of SelectOption))

        'scDomiciliosRegistrados.DataSource = opcionesLista_

        'scDomiciliosRegistrados.Value = opcionesLista_.Last.Value
        If GetVars("_listaDomicilios") IsNot Nothing Then

            scDomiciliosRegistrados.DataSource = GetVars("_listaDomicilios")

        End If

    End Sub

    Protected Sub scDomiciliosRegistrados_Click(sender As Object, e As EventArgs)

        'scDomiciliosRegistrados.Visible = True

        'Dim opcionesLista_ = DirectCast(GetVars("_opcionesLista"), List(Of SelectOption))

        'scDomiciliosRegistrados.DataSource = opcionesLista_

        'scDomiciliosRegistrados.Value = opcionesLista_.Last.Value
        If GetVars("_listaDomicilios") IsNot Nothing Then

            scDomiciliosRegistrados.DataSource = GetVars("_listaDomicilios")

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
    'Public Overrides Function AgregarComponentesBloqueadosInicial() As List(Of WebControl)
    '    'Return New List(Of WebControl) From {icClave, icPais}
    'End Function

    Private Sub validaCambiosTarjeta(ByRef pillbox_ As PillBox, ByVal documeto_ As DocumentoElectronico, ByRef swCambio_ As Boolean)

        Dim partida_ = documeto_.Seccion(SeccionesProvedorOperativo.SPRO2).Nodos(pillbox_.GetIdentity - 1)

        If Not pillbox_.GetControlValue(icTaxid).Equals(partida_.Attribute(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR).Valor) Then

            If partida_.Attribute(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR).Valor IsNot Nothing Then

                swCambio_ = True

                Exit Sub

            End If

        End If

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

    Public Overrides Function AgregarComponentesBloqueadosEdicion() As List(Of WebControl)
        Return New List(Of WebControl) From {icClave, fcRazonSocial, icPais}
    End Function

    Private Sub CambiarDomicilio(ByVal cveDomicilio_ As String, Optional ByVal indice_ As Integer = 0)

        _empresaInternacional = New EmpresaInternacional

        If GetVars("_empresaInternacional") IsNot Nothing Then

            _empresaInternacional = DirectCast(GetVars("_empresaInternacional"), EmpresaInternacional)

        End If

        ''OBTENER EL DOMICILIO SELECCIONADO
        Dim domicilioseleccionado_ = _utils.ObtenerDomicilioEnPais(_empresaInternacional, New ObjectId(icIdPais.Value), New ObjectId(cveDomicilio_))

        If domicilioseleccionado_ IsNot Nothing Then

            pbDetalleProveedorInternacional.Enabled = True

            If indice_ <> 0 Then

                LlenarTarjetero(New List(Of Domicilio) From {domicilioseleccionado_},
                                indice_)

            Else

                CargarTarjetero(New List(Of Domicilio) From {domicilioseleccionado_})

            End If

        End If

    End Sub

    ''FUNCIONES GUARDAR EMPRESA

    Protected Sub GuardarEmpresaInternacional(ByVal session_ As IClientSessionHandle)

        Dim modoEditando_ As Boolean = False

        If GetVars("isEditing") IsNot Nothing Then

            If GetVars("isEditing") = True Then

                modoEditando_ = True

            End If

        End If

        Dim existetaxid_ As Boolean = True

        Dim existepais_ As Boolean = True

        Dim existedomicilio_ As Boolean = True

        Dim coincidetaxid_ As Boolean = True

        Dim coincidedomicilio_ As Boolean = True

        _auxiliarProveedor = New AuxiliarProveedor

        _empresaInternacional = New EmpresaInternacional

        _tagwatcher = New TagWatcher

        If GetVars("_empresaInternacional") IsNot Nothing Then

            _empresaInternacional = DirectCast(GetVars("_empresaInternacional"), EmpresaInternacional)

        Else

            _tagwatcher = _utils.BuscarEmpresaPorObjectId(fcRazonSocial.Value, IControladorEmpresas.TiposEmpresas.Internacional)

            _empresaInternacional = _tagwatcher.ObjectReturned

        End If

        If GetVars("_auxiliarproveedor") IsNot Nothing Then
            ''ES PORQUE ES UNA MODADLIDAD DE ACTUALIZAR EL DESTINATARIO
            _auxiliarProveedor = DirectCast(GetVars("_auxiliarproveedor"), AuxiliarProveedor)

        Else
            ''ES ALTA DESTINATARIO
            '  _auxiliarDestinatario.id = _empresaInternacional._id.ToString

            _auxiliarProveedor._razonsocial = _empresaInternacional.razonsocial

            _auxiliarProveedor._listadomiciliosconTaxid = New List(Of DomiciliosTaxid)

        End If

        Dim item_ = 0

        Dim totalDomicilios_ = pbDetalleProveedorInternacional.DataSource.Count() - 1

        pbDetalleProveedorInternacional.ForEach(Sub(pillbox_ As PillBox)

                                                    'TAXID
                                                    Dim idtaxid_ = pillbox_.GetControlValue(icCveTaxid)

                                                    Dim taxid_ = pillbox_.GetControlValue(icTaxid)

                                                    ''PAIS Y DOMICILIOS
                                                    Dim idpais_ = pillbox_.GetControlValue(icIdPais)

                                                    Dim cvepais_ = pillbox_.GetControlValue(icCvePais)

                                                    Dim pais_ = pillbox_.GetControlValue(icPais)

                                                    If idtaxid_ <> "" Then
                                                        ''VERIFICAMOS QUE EXISTA EN LA EMPRESA
                                                        existetaxid_ = _utils.ExisteTaxId(_empresaInternacional, ObjectId.Parse(idtaxid_))

                                                        If existetaxid_ = False Then

                                                            ''GENEREMOS NUEVA ESTRUCTURA DE TAXID Y LA VAMOS AÑADIR AL 
                                                            ''LA ESTRUCTURA DE LA EMPRESA
                                                            _empresaInternacional = _utils.GenerarEmpresaInternacionalConEstructuraTaxidNuevo(_empresaInternacional, cvepais_, taxid_)

                                                            idtaxid_ = _empresaInternacional.taxids.Last.idtaxid.ToString

                                                        Else

                                                            coincidetaxid_ = _utils.ExisteTaxEnActuales(_empresaInternacional, taxid_)

                                                            If coincidetaxid_ = False Then
                                                                ''GENERAMOS TODA LA ESTRCUTURA DEL TAXID
                                                                ''Y LA AGREGAMOS A LA ESTRUCTURA DE LA EMPRESA
                                                                _empresaInternacional = _utils.GenerarEmpresaInternacionalConEstructuraTaxidNuevo(_empresaInternacional, cvepais_, taxid_)

                                                                idtaxid_ = _empresaInternacional.taxids.Last.idtaxid.ToString

                                                            End If

                                                        End If

                                                    Else

                                                        coincidetaxid_ = _utils.ExisteTaxEnActuales(_empresaInternacional, taxid_)

                                                        If coincidetaxid_ = False Then
                                                            ''GENERAMOS TODA LA ESTRCUTURA DEL TAXID
                                                            ''Y LA AGREGAMOS A LA ESTRUCTURA DE LA EMPRESA
                                                            _empresaInternacional = _utils.GenerarEmpresaInternacionalConEstructuraTaxidNuevo(_empresaInternacional, cvepais_, taxid_)

                                                            idtaxid_ = _empresaInternacional.taxids.Last.idtaxid.ToString
                                                        Else

                                                            idtaxid_ = _empresaInternacional.taxids.Last.idtaxid.ToString

                                                        End If

                                                    End If

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

                                                        .entidadfederativa = pillbox_.GetControlValue(icEntidadFederativa)

                                                        If pillbox_.GetControlValue(icSecDomicilio) <> "" Then

                                                            .sec = Int(pillbox_.GetControlValue(icSecDomicilio))

                                                        End If

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

                                                    If idpais_ <> "" Then
                                                        ''VERIFICAMOS QUE EXISTA EL PAIS
                                                        existepais_ = _utils.ExistePaisEmpresa(_empresaInternacional, ObjectId.Parse(idpais_))
                                                        ''LUEGO VERIFICAMOS QUE EXISTA EL DOMICILIO
                                                        If existepais_ Then

                                                            If iddomicilio_ <> "" Then

                                                                existedomicilio_ = _utils.ExisteDomicilioEnPais(_empresaInternacional,
                                                                                                                   ObjectId.Parse(idpais_),
                                                                                                                   ObjectId.Parse(iddomicilio_))

                                                                If existedomicilio_ = False Then

                                                                    ''GENERAMOS LA ESTRUCTURA DEL DOMICILIO
                                                                    iddomicilio_ = Nothing

                                                                    domicilioAux_._iddomicilio = New ObjectId()
                                                                    ''IMPORTANTE MANDAR EL DOMICILIO ESPECIFICO
                                                                    _empresaInternacional = _utils.GenerarEmpresaInternacionalConDomicilioNuevo(_empresaInternacional, ObjectId.Parse(idpais_), cvepais_, pais_, domicilioAux_)

                                                                Else

                                                                    If domiciliopresentacion_ <> "" Then

                                                                        If domiciliopresentacion_ = domicilioPresentacionAux_ Then

                                                                            coincidedomicilio_ = _utils.ExisteDomicilioPresentacionEnDomicilios(_empresaInternacional, ObjectId.Parse(idpais_), ObjectId.Parse(iddomicilio_), domiciliopresentacion_)

                                                                            If coincidedomicilio_ = False Then

                                                                                ''Y CAMBIAR LA SECUENCIA DEL DOMICILIO PORQUE ASI NO SIRVE
                                                                                Dim secuenciaDomicilio_ = pillbox_.GetControlValue(icSecDomicilio)

                                                                                iddomicilio_ = Nothing

                                                                                ''GENERAMOS LA ESTRUCTURA DEL DOMICILIO
                                                                                domicilioAux_._iddomicilio = ObjectId.GenerateNewId()

                                                                                'domicilioAux_.sec = Int(secuenciaDomicilio_) + 1
                                                                                ''IMPORTANTE MANDAR EL DOMICILIO ESPECIFICO
                                                                                _empresaInternacional = _utils.GenerarEmpresaInternacionalConDomicilioNuevo(_empresaInternacional, ObjectId.Parse(idpais_), cvepais_, pais_, domicilioAux_)

                                                                            End If

                                                                        Else
                                                                            iddomicilio_ = Nothing

                                                                            domicilioAux_._iddomicilio = ObjectId.GenerateNewId()

                                                                            _empresaInternacional = _utils.GenerarEmpresaInternacionalConDomicilioNuevo(_empresaInternacional, ObjectId.Parse(idpais_), cvepais_, pais_, domicilioAux_)

                                                                        End If

                                                                    End If

                                                                End If

                                                            Else
                                                                ''GENERAMOS LA ESTRUCTURA DEL DOMICILIO
                                                                ''GENERAMOS LA ESTRUCTURA DEL DOMICILIO
                                                                domicilioAux_._iddomicilio = ObjectId.GenerateNewId()

                                                                'domicilioAux_.sec = 1
                                                                ''IMPORTANTE MANDAR EL DOMICILIO ESPECIFICO
                                                                _empresaInternacional = _utils.GenerarEmpresaInternacionalConDomicilioNuevo(_empresaInternacional, ObjectId.Parse(idpais_), cvepais_, pais_, domicilioAux_)

                                                            End If

                                                        Else

                                                            domicilioAux_._iddomicilio = ObjectId.GenerateNewId()

                                                            _empresaInternacional = _utils.GenerarEmpresaInternacionalConDomicilioNuevo(_empresaInternacional, ObjectId.Parse(idpais_), cvepais_, pais_, domicilioAux_, False)


                                                        End If

                                                    Else
                                                        ''GENERAMOS LA ESTRUCTURA DEL PAIS DOMICILIOS
                                                        ''Y LA AGREGAMOS A LA EMPRESA
                                                        _empresaInternacional = _utils.GenerarEmpresaInternacionalConDomicilioNuevo(_empresaInternacional, ObjectId.Parse(idpais_), cvepais_, pais_, domicilioAux_, False)

                                                    End If

                                                    ''AGREGAMOS LO QUE CAMBIE TAMBIEN PARA LOS DATOS DEL DESTINATARIO
                                                    ''RECUERDA QUE ESTA CLASE SOLO ES UNA TRANSPORTADORA DE DATOS LOCAL
                                                    Dim ultimoDomicilio_ As New Domicilio

                                                    If iddomicilio_ <> "" AndAlso iddomicilio_ <> Nothing Then

                                                        ultimoDomicilio_ = _empresaInternacional.paisesdomicilios _
                                                                          .Where(Function(p) p.idpais = ObjectId.Parse(idpais_)) _
                                                                          .SelectMany(Function(p) p.domicilios) _
                                                                          .FirstOrDefault(Function(x) x._iddomicilio.Equals(ObjectId.Parse(iddomicilio_)))
                                                    Else

                                                        ultimoDomicilio_ = _empresaInternacional.paisesdomicilios _
                                                                           .Where(Function(p) p.idpais = ObjectId.Parse(idpais_)) _
                                                                           .Select(Function(p) p.domicilios.LastOrDefault()) _
                                                                           .FirstOrDefault()

                                                    End If

                                                    With _auxiliarProveedor

                                                        If modoEditando_ Then

                                                            If ._listadomiciliosconTaxid.Count > item_ Then

                                                                ._listadomiciliosconTaxid(item_)._iddomicilio = ultimoDomicilio_._iddomicilio

                                                                ._listadomiciliosconTaxid(item_).sec = totalDomicilios_ + 1

                                                                ._listadomiciliosconTaxid(item_).domicilioPresentacion = ultimoDomicilio_.domicilioPresentacion

                                                                ._listadomiciliosconTaxid(item_).clavetaxid = idtaxid_

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

                                                                    .clavetaxid = idtaxid_

                                                                    .cvePais = cvepais_

                                                                    .idpais = idpais_

                                                                    .sec = ultimoDomicilio_.sec

                                                                    .numeroexterior = ultimoDomicilio_.numeroexterior

                                                                    .numerointerior = ultimoDomicilio_.numerointerior

                                                                End With

                                                                _auxiliarProveedor._listadomiciliosconTaxid.Add(auxDomicilioConTaxid_)

                                                            End If

                                                        Else

                                                                Dim auxDomicilioConTaxid_ = New DomiciliosTaxid

                                                            With auxDomicilioConTaxid_

                                                                ._iddomicilio = ultimoDomicilio_._iddomicilio

                                                                .sec = totalDomicilios_ + 1

                                                                .domicilioPresentacion = ultimoDomicilio_.domicilioPresentacion

                                                                .clavetaxid = idtaxid_

                                                                .cvePais = cvepais_

                                                                .idpais = idpais_

                                                                .sec = ultimoDomicilio_.sec

                                                                .numeroexterior = ultimoDomicilio_.numeroexterior

                                                                .numerointerior = ultimoDomicilio_.numerointerior

                                                            End With

                                                            _auxiliarProveedor._listadomiciliosconTaxid.Add(auxDomicilioConTaxid_)

                                                        End If

                                                    End With

                                                    item_ += 1

                                                End Sub)

        SetVars("_empresaInternacional", _empresaInternacional)

        SetVars("_auxiliarproveedor", _auxiliarProveedor)

    End Sub

    Protected Sub GuardarEmpresaInternacional(ByVal session_ As IClientSessionHandle, ByVal esempresanueva_ As Boolean)

        _empresaInternacional = GetVars("_empresaInternacional")

        _auxiliarProveedor = New AuxiliarProveedor

        _tagwatcher = New TagWatcher

        ''Es nueva la empresa
        _auxiliarProveedor = _utils.ObtenerDatosProveedorDesdePillbox(fcRazonSocial.Text, pbDetalleProveedorInternacional)

        With _auxiliarProveedor

            If _empresaInternacional Is Nothing Then

                _empresaInternacional = New EmpresaInternacional

                _empresaInternacional = _utils.GenerarEstructuraEmpresa(._razonsocial, ._taxid, ._listadomiciliosconTaxid.Last)

                SetVars("_empresaInternacional", _empresaInternacional)

            End If

            _tagwatcher = _utils.GuardarEmpresa(_empresaInternacional, Nothing, cvePais_:= ._listadomiciliosconTaxid.Last.cvePais)

            If _tagwatcher.Status = TypeStatus.Ok Then

                Dim paisBuscado_ = ._listadomiciliosconTaxid.Last.idpais

                Dim ultimoDomicilio_ = _empresaInternacional.paisesdomicilios _
                                            .Where(Function(p) p.idpais = ObjectId.Parse(paisBuscado_)) _
                                            .Select(Function(p) p.domicilios.LastOrDefault()) _
                                            .FirstOrDefault()

                _auxiliarProveedor._listadomiciliosconTaxid.Last._iddomicilio = ultimoDomicilio_._iddomicilio

                _auxiliarProveedor._listadomiciliosconTaxid.Last.sec = ultimoDomicilio_.sec

                _auxiliarProveedor._listadomiciliosconTaxid.Last.numeroexterior = ultimoDomicilio_.numeroexterior

                _auxiliarProveedor._listadomiciliosconTaxid.Last.numerointerior = ultimoDomicilio_.numerointerior

                _auxiliarProveedor._listadomiciliosconTaxid.Last.domicilioPresentacion = ultimoDomicilio_.domicilioPresentacion

                _auxiliarProveedor._listadomiciliosconTaxid.Last.clavetaxid = _empresaInternacional.taxids.Last.idtaxid.ToString


                SetVars("_auxiliarproveedor", _auxiliarProveedor)

            Else

                SetVars("_empresaInternacional", Nothing)

                SetVars("_auxiliarproveedor", Nothing)

            End If

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

        pbDetalleProveedorInternacional.ForEach(Sub(pillbox_ As PillBox)
                                                    ccDomiciliosFiscales.SetRow(Sub(catalogRow_ As CatalogRow)
                                                                                    catalogRow_.SetIndice(ccDomiciliosFiscales.KeyField, i)
                                                                                    catalogRow_.SetColumn(icTaxIDRFC, pillbox_.GetControlValue(icTaxid))
                                                                                    catalogRow_.SetColumn(icDomicilio, pillbox_.GetControlValue(scDomicilio))
                                                                                    catalogRow_.SetColumn(swcArchivarDomicilio, pillbox_.IsFiled())
                                                                                End Sub)
                                                    i += 1
                                                End Sub)

        ccDomiciliosFiscales.CatalogDataBinding()

    End Sub



    Protected Sub LLenarListaDomiciliosEmpresas(ByVal indice_ As Integer)

        _empresaInternacional = New EmpresaInternacional

        Dim opcionesLista_ As New List(Of SelectOption)

        If GetVars("_empresaInternacional") IsNot Nothing Then

            _empresaInternacional = DirectCast(GetVars("_empresaInternacional"), EmpresaInternacional)

        End If

        _pillboxControl = New PillboxControl

        _pillboxControl = pbDetalleProveedorInternacional

        Dim domiciliosDesdeEmpresa_ As List(Of Rec.Globals.Empresas.Domicilio) = _empresaInternacional.paisesdomicilios.Last.domicilios

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

            SetVars("_opcionesLista", opcionesLista_)

            SetVars("_indice", indice_)

        End If

    End Sub

    Protected Sub LlenarFormulario(ByVal pais_ As String)

        Dim modoEditando_ As Boolean = False

        If GetVars("isEditing") IsNot Nothing Then

            If GetVars("isEditing") = True Then

                modoEditando_ = True

            End If

        End If

        _tagwatcher = New TagWatcher

        _empresaInternacional = New EmpresaInternacional

        Dim ultimoDomicilio_ As New List(Of Rec.Globals.Empresas.Domicilio)

        Dim cvePais_ As String = pais_.Substring(0, 3)

        _controladorEmpresas = New Rec.Globals.Controllers.Empresas.ControladorEmpresas(New EspacioTrabajo(),
                                                                                             IControladorEmpresas.TiposEmpresas.Internacional,
                                                                                              paisEmpresa_:=cvePais_) _
                                                                                             With {.ListaEmpresas = New List(Of IEmpresa)}

        If GetVars("_listaEmpresasTemporales") IsNot Nothing Then

            _listaEmpresasInternacional = New List(Of EmpresaInternacional)

            _listaEmpresasInternacional = DirectCast(GetVars("_listaEmpresasTemporales"), List(Of EmpresaInternacional))

            If _listaEmpresasInternacional.Count > 0 Then

                Dim opcionesLista_ As New List(Of SelectOption)

                _pillboxControl = New PillboxControl

                _empresaInternacional = DirectCast(_listaEmpresasInternacional.Find(Function(x) x.razonsocial = fcRazonSocial.Text), EmpresaInternacional)

                If _empresaInternacional IsNot Nothing Then

                    SetVars("_empresaInternacional", _empresaInternacional)

                    _controladorEmpresas.Modalidad = IControladorEmpresas.Modalidades.Intrinseca

                    _controladorEmpresas.PaisEmpresa = cvePais_

                    _controladorEmpresas.ListaEmpresas.Add(_empresaInternacional)

                    _tagwatcher = _controladorEmpresas.ConsultarDomicilios(_empresaInternacional._id)

                    If _tagwatcher.Status = TypeStatus.Ok Then

                        Dim listaDomicilios_ As List(Of Rec.Globals.Empresas.Domicilio) = DirectCast(_tagwatcher.ObjectReturned, List(Of Rec.Globals.Empresas.Domicilio))

                        If listaDomicilios_.Count > 0 Then

                            ultimoDomicilio_.Add(listaDomicilios_.Last)

                            'AQUI VERIFICAMOS QUE NO EXISTAN EN EL TARJETERO

                            Dim listadomiciliosActuales_ As New List(Of String)

                            Dim domiciliosProveedorActuales_ As New List(Of Nodo)

                            If modoEditando_ Then

                                domiciliosProveedorActuales_ = OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Seccion(SeccionesProvedorOperativo.SPRO2).Nodos

                                For Each item_ In domiciliosProveedorActuales_

                                    ''debe ser id y pais
                                    If cvePais_ = item_.Campo(CamposDomicilio.CA_CVE_PAIS).Valor Then

                                        listadomiciliosActuales_.Add(item_.Campo(CamposProveedorOperativo.CP_ID_DOMICILIO_PROVEEDOR).Valor)

                                    End If

                                Next

                            End If


                            Dim i_ = 0

                            For Each item_ In listaDomicilios_

                                If listadomiciliosActuales_.Count > 0 Then

                                    For i_ = 0 To listadomiciliosActuales_.Count - 1

                                        If item_._iddomicilio.ToString <> listadomiciliosActuales_(i_) Then

                                            opcionesLista_.Add(New SelectOption With
                                                       {
                                                           .Value = item_._iddomicilio.ToString,
                                                           .Text = item_.domicilioPresentacion
                                                       })

                                        End If

                                        i_ += 1

                                    Next

                                Else

                                    opcionesLista_.Add(New SelectOption With
                                                    {
                                                        .Value = item_._iddomicilio.ToString,
                                                        .Text = item_.domicilioPresentacion
                                                    })

                                End If

                            Next

                            If opcionesLista_.Count > 0 And scDomiciliosRegistrados.DataSource Is Nothing Then

                                lbtitleDomicilios.Visible = True

                                scDomiciliosRegistrados.Visible = True

                                SetVars("_listaDomicilios", listaDomicilios_)

                                SetVars("_opcionesLista", opcionesLista_)

                                opcionesLista_.Add(New SelectOption With
                                                    {
                                                        .Value = "DOMICILIO NUEVO",
                                                        .Text = "DOMICILIO NUEVO"
                                                    })


                                scDomiciliosRegistrados.DataSource = opcionesLista_

                            Else

                                LimpiarListaDomiciliosActuales()

                            End If

                        Else

                            LimpiarListaDomiciliosActuales()

                        End If

                    Else

                        LimpiarListaDomiciliosActuales()

                    End If

                Else

                    LimpiarListaDomiciliosActuales()

                End If

            Else

                LimpiarListaDomiciliosActuales()

            End If

        Else

            LimpiarListaDomiciliosActuales()

        End If

    End Sub

    Sub LimpiarListaDomiciliosActuales()

        lbtitleDomicilios.Visible = False

        scDomiciliosRegistrados.Visible = False

        scDomiciliosRegistrados.DataSource = Nothing

        SetVars("_opcionesLista", Nothing)

        SetVars("_listaDomicilios", Nothing)

    End Sub

    Sub LlenarTarjetero(ByVal domicilio_ As List(Of Rec.Globals.Empresas.Domicilio),
                        ByVal indice_ As Integer)

        _pillboxControl = New PillboxControl

        _pillboxControl = pbDetalleProveedorInternacional

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

        _empresaInternacional = New EmpresaInternacional

        If GetVars("_empresaInternacional") IsNot Nothing Then

            _empresaInternacional = DirectCast(GetVars("_empresaInternacional"), EmpresaInternacional)

        End If

        Dim cvePais_ As String = fcPaises.Text.Substring(0, 3)

        Dim domicilioPorPais_ = _empresaInternacional.paisesdomicilios.Where(Function(x) x.pais = cvePais_).AsEnumerable.ToList

        Dim pillboxControl_ As PillboxControl = pbDetalleProveedorInternacional

        pillboxControl_.ClearRows()

        Dim controladorFirma_ As New ControladorFirmaElectronica

        For Each item_ In domicilio_

            Dim id_ = ObjectId.GenerateNewId()

            pillboxControl_.SetPillbox(Sub(pillbox_ As PillBox)

                                           pillbox_.SetControlValue(icIdTarjeta, id_.ToString)

                                           pillbox_.SetControlValue(icFirmaTarjeta, controladorFirma_.Generar(id_, 1))

                                           pillbox_.SetControlValue(icTaxid, _empresaInternacional.taxids.Last.taxid)

                                           pillbox_.SetControlValue(icCveTaxid, _empresaInternacional.taxids.Last.idtaxid.ToString)

                                           pillbox_.SetControlValue(icIdPais, _empresaInternacional.paisesdomicilios.Last.idpais.ToString)

                                           pillbox_.SetControlValue(icCvePais, _empresaInternacional.paisesdomicilios.Last.pais)

                                           pillbox_.SetControlValue(icPais, _empresaInternacional.paisesdomicilios.Last.paisPresentacion)

                                           pillbox_.SetControlValue(icCalle, item_.calle)

                                           pillbox_.SetControlValue(icNumeroExterior, item_.numeroexterior)

                                           pillbox_.SetControlValue(icNumeroInterior, item_.numerointerior)

                                           pillbox_.SetControlValue(icCodigoPostal, item_.codigopostal)

                                           pillbox_.SetControlValue(icColonia, item_.colonia)

                                           pillbox_.SetControlValue(icLocalidad, item_.localidad)

                                           pillbox_.SetControlValue(icCiudad, item_.ciudad)

                                           pillbox_.SetControlValue(icMunicipio, item_.municipio)

                                           pillbox_.SetControlValue(icEntidadFederativa, item_.entidadfederativa)

                                           pillbox_.SetControlValue(icIdDomicilio, item_._iddomicilio.ToString)

                                           pillbox_.SetControlValue(icSecDomicilio, item_.sec)

                                           pillbox_.SetControlValue(scDomicilio, item_.domicilioPresentacion)

                                           pillbox_.SetControlValue(icNumeroExtInt, $"{item_.numeroexterior} - {item_.numerointerior}")

                                           pillbox_.SetControlValue(icCveMunicipio, item_.cveMunicipio)

                                           pillbox_.SetControlValue(icCveEntidadFederativa, item_.cveEntidadfederativa)

                                           indice_ += 1

                                       End Sub)
        Next

        pbDetalleProveedorInternacional = pillboxControl_

        pbDetalleProveedorInternacional.PillBoxDataBinding()

        SetVars("_listaDomicilios", pbDetalleProveedorInternacional.DataSource)

    End Sub

    Sub LimpiarTarjetero()

        Dim pillboxControl_ As PillboxControl = pbDetalleProveedorInternacional

        pillboxControl_.ClearRows()

        pillboxControl_.SetPillbox(Sub(pillbox_ As PillBox)
                                       pillbox_.SetIndice(pillboxControl_.KeyField, 1)

                                       pillbox_.SetFiled(False)

                                       pillbox_.SetControlValue(icIdTarjeta, Nothing)

                                       pillbox_.SetControlValue(icFirmaTarjeta, Nothing)

                                       pillbox_.SetControlValue(icTaxid, Nothing)

                                       pillbox_.SetControlValue(icCveTaxid, Nothing)

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

        pbDetalleProveedorInternacional = pillboxControl_

        pbDetalleProveedorInternacional.PillBoxDataBinding()

        SetVars("_listaDomicilios", Nothing)

    End Sub

    Protected Sub RegresarControlesPorDefault()

        Dim modoEditando_ As Boolean = False

        If GetVars("isEditing") IsNot Nothing Then

            If GetVars("isEditing") = True Then

                modoEditando_ = True

            End If

        End If

        If pbDetalleProveedorInternacional.PageIndex > 0 Then

            lbNumero.Text = pbDetalleProveedorInternacional.PageIndex.ToString()

        End If

        Session("_tbDetalleProveedor") = PillboxControl.ToolbarModality.Default : pbDetalleProveedorInternacional.Modality = Session("_tbDetalleProveedor")

        If modoEditando_ Then

            PreparaTarjetero(PillboxControl.ToolbarModality.Advanced, pbDetalleProveedorInternacional)

            fsDatosGenerales.Enabled = True

            fsVinculaciones.Enabled = True

            fsConfiguracionAdicional.Enabled = True

            fsHistorialDomicilios.Enabled = True

        Else

            PreparaTarjetero(PillboxControl.ToolbarModality.Default, pbDetalleProveedorInternacional)

            fsVinculaciones.Enabled = False

            fsConfiguracionAdicional.Enabled = False

            fsHistorialDomicilios.Enabled = False

        End If

        fcPaises.Value = Nothing

        fcPaises.Text = Nothing

        lbtitleDomicilios.Visible = False

        scDomiciliosRegistrados.Value = Nothing

        scDomiciliosRegistrados.Visible = False

        ConfigurarDomicilios.Visible = False

        scDomiciliosRegistrados.DataSource = Nothing

        fsHistorialDomicilios.Visible = True

        fsVinculaciones.Visible = True

        fsConfiguracionAdicional.Visible = True

        CargarHistorialDomicilios()

    End Sub

    Protected Sub ListarDomiciliosPorPais(Optional ByVal tipoempresa_ As IControladorEmpresas.TiposEmpresas =
                                          IControladorEmpresas.TiposEmpresas.Internacional)

        If fcRazonSocial.Value <> "" Then

            Dim idpais_ = GetVars("_idpais")

            If fcPaises.Value IsNot Nothing AndAlso fcPaises.Value <> "" Then

                idpais_ = fcPaises.Value

            End If

            Dim data_ = _utils.ListaDomiciliosPorPais(fcRazonSocial.Value, idpais_, tipoempresa_)

            If data_.Count > 0 Then

                If tipoempresa_ = IControladorEmpresas.TiposEmpresas.Internacional Then

                    If data_.Count = 1 Then

                        scDomiciliosRegistrados.DataSource = data_

                        scDomiciliosRegistrados.Value = data_(0).Value
                    Else

                        scDomiciliosRegistrados.DataSource = data_

                    End If

                End If

            Else

                If tipoempresa_ = IControladorEmpresas.TiposEmpresas.Internacional Then

                    scDomiciliosRegistrados.DataSource = Nothing

                    scDomiciliosRegistrados.Visible = False

                End If

            End If

        Else

            scDomiciliosRegistrados.DataSource = Nothing

            scDomiciliosRegistrados.Visible = False

            pbDetalleProveedorInternacional.Enabled = True

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

    Protected Sub MsgValidacionRazonsocial()
        fcRazonSocial.ToolTip = "Proveedor ya registrado. "
        'fcRazonSocial.ToolTipExpireTime = 4
        fcRazonSocial.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors
        fcRazonSocial.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
        fcRazonSocial.ShowToolTip()
    End Sub

    Protected Sub MsgValidacionRazonsocialVacio()
        fcRazonSocial.ToolTip = "Indica una razón social. "
        'fcRazonSocial.ToolTipExpireTime = 4
        fcRazonSocial.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors
        fcRazonSocial.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
        fcRazonSocial.ShowToolTip()
    End Sub

    Protected Sub MsgValidacionCalleVacio()
        icCalle.ToolTip = "Indica una calle. "
        icCalle.ToolTipExpireTime = 4
        icCalle.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors
        icCalle.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
        icCalle.ShowToolTip()
    End Sub

    Protected Sub MsgValidacionPaisVacio()
        icPais.ToolTip = "Indica un país. "
        icPais.ToolTipExpireTime = 4
        icPais.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors
        icPais.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
        icPais.ShowToolTip()
    End Sub

    Protected Sub MsgValidacionPaisValido()
        icPais.ToolTip = "País no válido."
        icPais.ToolTipExpireTime = 4
        icPais.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors
        icPais.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
        icPais.ShowToolTip()
    End Sub

    Protected Sub MsgValidacionPaisValidoControl()
        fcPaises.ToolTip = "País no válido."
        fcPaises.ToolTipExpireTime = 4
        fcPaises.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors
        fcPaises.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
        fcPaises.ShowToolTip()
    End Sub

    Protected Sub swcHabilitado_CheckedChanged(sender As Object, e As EventArgs)

        If swcHabilitado.Checked Then

            DisplayMessage("🟣 Proveedor Online", StatusMessage.Info)

            'pbDetalleProveedor.Enabled = False

            EstadoConexion()

        Else

            DisplayMessage("⚪ Proveedor Offline", StatusMessage.Info)

            'pbDetalleProveedor.Enabled = True

            EstadoConexion()

        End If

    End Sub

    Public Sub EstadoConexion()
        ' Visual Basic
        Dim estadoConexion_ As String

        If swcHabilitado.Visible Then

            If swcHabilitado.Checked Then

                estadoConexion_ = "<span style='color:#757575; font-size:1.2rem'>🟣 Online</span>"
                pbDetalleProveedorInternacional.Enabled = False
                'fsVinculaciones.Enabled = False
                'fsConfiguracionAdicional.Enabled = False
                'fsHistorialDomicilios.Enabled = False
            Else

                estadoConexion_ = "<span style='color:#757575; font-size:1.2rem'>⚪ Offline</span>"
                pbDetalleProveedorInternacional.Enabled = True
                'fsVinculaciones.Enabled = True
                'fsConfiguracionAdicional.Enabled = True
                'fsHistorialDomicilios.Enabled = True
            End If

            online.Text = estadoConexion_

        End If

    End Sub

#End Region


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

    Protected Sub MsgIndicaTaxid()

        With icTaxid
            .ToolTip = "👉 Indica TAXID"
            .ToolTipExpireTime = 6
            .ToolTipStatus = IUIControl.ToolTipTypeStatus.OkInfo
            .ToolTipModality = IUIControl.ToolTipModalities.Ondemand
            .ShowToolTip()
        End With

    End Sub

#End Region
End Class