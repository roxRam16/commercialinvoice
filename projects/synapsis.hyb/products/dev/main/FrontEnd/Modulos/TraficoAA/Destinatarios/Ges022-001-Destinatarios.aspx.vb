#Region "├┴┘├┴┘├┴┘├┴┘├┴┘|├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘   DEPENDENCIAS   ├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘"

'RECURSOS DEL CMF
Imports System.Globalization
Imports Gsol
Imports Gsol.krom
'OBJETOS BIDIMENSIONALES (ODF's.  Dependencias Krombase/SQL Server)
Imports Gsol.krom.Anexo22.Vt022AduanaSeccionA01
Imports Gsol.Web.Components
Imports Gsol.Web.Components.FormControl.ButtonbarModality
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Rec.Globals
'OBJETOS DIMENSIONALES (ODS's) Dependencias en MongoDB
Imports Rec.Globals.Controllers
Imports Rec.Globals.Controllers.Empresas
Imports Rec.Globals.Empresas

'Imports Rec.Globals.Empresa
Imports Rec.Globals.Utils
Imports Sax.Web
'UTILERIAS/RECURSOS ADICIONALES
Imports Sax.Web.ControladorBackend.Cookies
Imports Sax.Web.ControladorBackend.Datos
Imports Syn.Custombrokers.Controllers
Imports Syn.Documento
Imports Syn.Documento.Componentes
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

Public Class Ges022_001_Destinatarios
    Inherits ControladorBackend

#Region "████████████████████████████████████████   Atributos locales  ██████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    Private _cantidadDetalles As Int32

    Private _tagwatcher As TagWatcher

    Private _empresaInternacional As EmpresaInternacional

    Private _empresaNacional As EmpresaNacional

    Private _controladorEmpresas As Rec.Globals.Controllers.Empresas.IControladorEmpresas

    Private _listaDomicilios As List(Of Rec.Globals.Empresas.Domicilio)

    Private _datosAdicionalesActuales As List(Of Dictionary(Of String, String))

    Private _pillboxControl As PillboxControl

    Private _secuencia As Secuencia

    Private _controladorSecuencias As IControladorSecuencia

    Private _listaEmpresasInternacional As List(Of EmpresaInternacional)

    Private _utils As UtileriaProveedores

    Private _lista As List(Of SelectOption)

    Private _auxiliarDestinatario As AuxiliarDestinatario

    Private _empresa As IEmpresa

    Private _loginUsuario As Dictionary(Of String, String)

    Private _datospaismexicano As PaisDomicilio

#End Region

#Region "██████ Vinculación c/capas inf █████████       SAX      ████████████████████████████████████████████"
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

    Public Overrides Sub Inicializa()

        With Buscador

            .DataObject = New ConstructorDestinatario(True)

            .addFilter(SeccionesDestinatarios.SDES1, CamposDestinatario.CA_RAZON_SOCIAL, "Destinatario")

            .addFilter(SeccionesDestinatarios.SDES2, CamposDestinatario.CA_TAX_ID, "TAXID")

        End With

        If OperacionGenerica IsNot Nothing Then

            _cantidadDetalles = OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Seccion(SeccionesDestinatarios.SDES2).CantidadPartidas

        End If

        icClave.Text = ""

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

        [Set](fcRazonSocialDestinatario, CamposDestinatario.CA_RAZON_SOCIAL, propiedadDelControl_:=PropiedadesControl.Text)

        [Set](swcHabilitado, CamposDestinatario.CA_DESTINATARIO_HABILITADO, propiedadDelControl_:=PropiedadesControl.Checked)

        'Detalle proveedor
        If pbDetalleDomicilioDestinatario.PageIndex > 0 Then

            lbNumero.Text = pbDetalleDomicilioDestinatario.PageIndex.ToString()

        End If

        [Set](icTaxid, CamposDestinatario.CA_TAX_ID, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icCveTaxid, CamposDestinatario.CA_CVE_TAX_ID_DESTINATARIO, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icIdDomicilio, CamposDestinatario.CP_ID_DOMICILIO_DESTINATARIO, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icSecDomicilio, CamposDestinatario.CP_SEC_DOMICILIO_DESTINATARIO, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icIdPais, CamposDomicilio.CA_ID_PAIS, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icCvePais, CamposDomicilio.CA_CVE_PAIS, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icPais, CamposDomicilio.CA_PAIS, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scDomicilio, CamposDestinatario.CA_DOMICILIO_FISCAL_DESTINATARIO, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icIdTarjeta, CamposDestinatario.CP_ID_DESTINATARIO, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icFirmaTarjeta, CamposProveedorOperativo.CP_FIRMA_ELECTRONICA, propiedadDelControl_:=PropiedadesControl.Ninguno)

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

        [Set](icestadoproveedor, CamposDestinatario.CA_ESTADO_DOMICILIO_DESTINATARIO, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icdomicilioarchivadoproveedor, CamposDestinatario.CA_DOMICILIO_ARCHIVADO_DESTINATARIO, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icmotivoarchivadoproveedor, CamposDestinatario.CA_MOTIVO_ARCHIVADO_DOMICILIO_DESTINATARIO, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](fechaarchivadoproveedor, CamposDestinatario.CA_FECHA_ARCHIVADO_DOMICILIO_DESTINATARIO, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](pbDetalleDomicilioDestinatario, Nothing, seccion_:=SeccionesDestinatarios.SDES2)

        If modoEditando_ Then

            EstadoConexion()

        End If

        Return New TagWatcher(1)

    End Function

    Public Overrides Sub BotoneraClicNuevo()
        If OperacionGenerica IsNot Nothing Then
        End If

        LimpiaSesion()

        If pbDetalleDomicilioDestinatario.PageIndex > 0 Then

            lbNumero.Text = pbDetalleDomicilioDestinatario.PageIndex.ToString()

        End If

        PreparaTarjetero(PillboxControl.ToolbarModality.Simple, pbDetalleDomicilioDestinatario)

        fsHistorialDomicilios.Visible = False

        icPais.Enabled = False

        pbDetalleDomicilioDestinatario.Enabled = False

        swcHabilitado.Checked = False

        fcRazonSocialDestinatario.Enabled = True

    End Sub


    Public Overrides Sub BotoneraClicGuardar()

        Dim modoEditando_ As Boolean = False

        If GetVars("isEditing") IsNot Nothing Then

            If GetVars("isEditing") = True Then

                modoEditando_ = True

            End If

        End If

        If modoEditando_ = False Then

            If fcRazonSocialDestinatario.Text = "" Then

                MsgValidacionRazonsocialVacio()

            ElseIf icCalle.Value = "" Then

                MsgValidacionCalleVacio()

            ElseIf icPais.Value = "" Then

                'MsgValidacionPaisVacio()

            Else

                If BuscarSiExisteDestinatario() Then

                    'aviso.Visible = True
                    DisplayMessage("Destinatario ya registrado.", StatusMessage.Fail)

                Else

                    If Not ProcesarTransaccion(Of ConstructorProveedoresOperativos)().Status = TypeStatus.Errors Then : End If

                End If

            End If

        Else

            If Not ProcesarTransaccion(Of ConstructorProveedoresOperativos)().Status = TypeStatus.Errors Then : End If

        End If

    End Sub

    Public Overrides Sub BotoneraClicEditar()

        PreparaTarjetero(PillboxControl.ToolbarModality.Advanced, pbDetalleDomicilioDestinatario)

        icClave.Enabled = False

        fcRazonSocialDestinatario.Enabled = False

        icPais.Enabled = False

    End Sub

    'EVENTOS PARA LA INSERCIÓN DE DATOS
    Public Overrides Function AntesRealizarInsercion(ByVal session_ As IClientSessionHandle) As TagWatcher

        Dim tagwatcher_ As New TagWatcher

        If session_ IsNot Nothing Then

            If swcbuscarempresa.Checked Then

                If fcRazonSocialDestinatario.Value <> "" Then

                    GuardarEmpresaInternacional(session_)

                Else

                    GuardarEmpresaInternacional(session_, esempresanueva_:=True)

                End If

                tagwatcher_.SetOK()
            Else

                If fcRazonSocialDestinatario.Value <> "" Then

                    GuardarEmpresaNacional(session_)

                Else

                    GuardarEmpresaNacional(session_, esempresanueva_:=True)

                End If

                tagwatcher_.SetOK()

            End If

        Else

            tagwatcher_.SetOK()

        End If

        Return tagwatcher_

    End Function

    Public Overrides Sub RealizarInsercion(ByRef documentoElectronico_ As DocumentoElectronico)

        _tagwatcher = New TagWatcher

        If GetVars("_empresa") IsNot Nothing Then

            If swcbuscarempresa.Checked Then

                _empresa = New EmpresaInternacional

                _empresa = DirectCast(GetVars("_empresa"), EmpresaInternacional)

            Else

                _empresa = New EmpresaNacional

                _empresa = DirectCast(GetVars("_empresa"), EmpresaNacional)

            End If

        End If

        _loginUsuario = New Dictionary(Of String, String)

        _loginUsuario = Session("DatosUsuario")

        _secuencia = New Secuencia

        _secuencia = _utils.GenerarSecuencia(SecuenciasComercioExterior.Destinatarios)

        With documentoElectronico_

            .Id = _secuencia._id.ToString

            .Campo(CamposDestinatario.CP_ID_DESTINATARIO).Valor = _secuencia._id

            icClave.Text = _secuencia.sec

            .Campo(CamposDestinatario.CP_ID_EMPRESA).Valor = _empresa._id

            .Campo(CamposDestinatario.CP_CVE_EMPRESA).Valor = _empresa._idempresa

            .Campo(CamposDestinatario.CP_CVE_DESTINATARIO).Valor = _secuencia.sec

            .Campo(CamposDestinatario.CA_DESTINATARIO_HABILITADO).Valor = False

            .Campo(CamposDestinatario.CA_DESTINATARIO_HABILITADO).ValorPresentacion = "Offline"

            If swcbuscarempresa.Checked Then

                .Campo(CamposDestinatario.CP_TIPO_DESTINATARIO).Valor = True

                .Campo(CamposDestinatario.CP_TIPO_DESTINATARIO).ValorPresentacion = "EXTRANJERO"

            Else

                .Campo(CamposDestinatario.CP_TIPO_DESTINATARIO).Valor = False

                .Campo(CamposDestinatario.CP_TIPO_DESTINATARIO).ValorPresentacion = "NACIONAL"

            End If

            .UsuarioGenerador = _loginUsuario("Nombre")

            .IdDocumento = _secuencia.sec

            .FolioDocumento = _secuencia.sec 'DUDA

            .FolioOperacion = _secuencia.sec 'DUDA

            .TipoPropietario = _secuencia.nombre

            .NombrePropietario = _empresa.razonsocial

            .IdPropietario = _empresa._idempresa

            .ObjectIdPropietario = _empresa._id

        End With

        Dim controladorFirma_ As New ControladorFirmaElectronica

        Dim pill_ = New PillBox

        pbDetalleDomicilioDestinatario.ForEach(Sub(pillbox_ As PillBox)
                                                   Dim id_ = ObjectId.GenerateNewId()
                                                   pbDetalleDomicilioDestinatario.setValueInvisible(icIdTarjeta, pillbox_.GetIdentity, id_)
                                                   pbDetalleDomicilioDestinatario.setValueInvisible(icFirmaTarjeta, pillbox_.GetIdentity, controladorFirma_.Generar(id_, 1))
                                               End Sub)

    End Sub

    Public Overrides Function DespuesRealizarInsercion() As TagWatcher

        RegresarControlesPorDefault()

        swcbuscarempresa.Visible = False

        swcHabilitado.Visible = True

        fcRazonSocialDestinatario.Enabled = False

        Return New TagWatcher(Ok)

    End Function

    'EVENTOS PARA MODIFICACIÓN DE DATOS
    Public Overrides Function AntesRealizarModificacion(ByVal session_ As IClientSessionHandle) As TagWatcher

        Dim tagwatcher_ As New TagWatcher

        If session_ IsNot Nothing Then '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas con transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 

            Dim esempresaextranjera_ = True

            If GetVars("_esempresaextranjera") IsNot Nothing Then

                esempresaextranjera_ = GetVars("_esempresaextranjera")

            End If

            If esempresaextranjera_ Then

                GuardarEmpresaInternacional(session_)

            Else

                GuardarEmpresaNacional(session_)

            End If

            tagwatcher_.SetOK()

        Else  '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas sin transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 

            tagwatcher_.SetOK()

        End If

        Return tagwatcher_

    End Function

    Public Overrides Function DespuesRealizarModificacion() As TagWatcher

        Return New TagWatcher(Ok)

    End Function

    'EVENTOS PARA PRESENTACIÓN DE DATOS EN FRONTEND
    Public Overrides Sub PreparaModificacion(ByRef documentoElectronico_ As DocumentoElectronico)

        Dim idempresa_ As ObjectId = Nothing

        Dim esempresaextranjera_ = True

        If documentoElectronico_ IsNot Nothing Then

            idempresa_ = documentoElectronico_.Seccion(SeccionesDestinatarios.SDES1).Campo(CamposDestinatario.CP_ID_EMPRESA).Valor

            _cantidadDetalles = documentoElectronico_.Seccion(SeccionesDestinatarios.SDES2).CantidadPartidas

            esempresaextranjera_ = documentoElectronico_.Seccion(SeccionesDestinatarios.SDES1).Campo(CamposDestinatario.CP_TIPO_DESTINATARIO).Valor

        End If

        ''BUSCAR LA EMPRESA ACTUAL
        _tagwatcher = New TagWatcher

        If esempresaextranjera_ Then
            ''CHECA PORQUE QUIZAS DEBAS ACOTAR A PAÍS
            _empresaInternacional = New EmpresaInternacional

            Dim cvePais_ As String = icPais.Value.Substring(0, 3)

            Try

                _tagwatcher = _utils.BuscarEmpresaPorObjectId(idempresa_.ToString)

                If _tagwatcher.Status = TypeStatus.Ok Then

                    _empresaInternacional = DirectCast(_tagwatcher.ObjectReturned, EmpresaInternacional)

                    Dim listaempresasinternacionales_ As New List(Of EmpresaInternacional) From {_empresaInternacional}

                    SetVars("_empresa", _empresaInternacional)

                    SetVars("_listaempresastemporales", listaempresasinternacionales_)

                End If

                CargarHistorialDomicilios()

                ConfigurarDomicilios.Enabled = False

                ConfigurarDomicilios.Visible = False

            Catch ex As Exception

                DisplayMessage("Favor de intentarlo más tarde", StatusMessage.Fail)

            End Try

        Else
            _empresaNacional = New EmpresaNacional

            Try

                _tagwatcher = _utils.BuscarEmpresaPorObjectId(idempresa_.ToString, IControladorEmpresas.TiposEmpresas.Nacional)

                If _tagwatcher.Status = TypeStatus.Ok Then

                    _empresaNacional = DirectCast(_tagwatcher.ObjectReturned, EmpresaNacional)

                    SetVars("_empresa", _empresaNacional)

                    Dim listaempresasnacionales_ As New List(Of EmpresaNacional) From {_empresaNacional}

                    SetVars("_listaempresastemporales", listaempresasnacionales_)

                    Dim pais_ = _utils.ObtenerDatosPaisMexicano()

                    Dim data_ = _utils.ListaDomiciliosPorPais(_empresaNacional._id.ToString, pais_.idpais.ToString, IControladorEmpresas.TiposEmpresas.Nacional)

                    SetVars("_listaDomiciliosNacionales", data_)

                End If

                CargarHistorialDomicilios()

                ConfigurarDomicilios.Enabled = False

                ConfigurarDomicilios.Visible = False

            Catch ex As Exception

                DisplayMessage("Favor de intentarlo más tarde", StatusMessage.Fail)

            End Try

        End If

    End Sub

    Public Overrides Sub RealizarModificacion(ByRef documentoElectronico_ As DocumentoElectronico)

        Dim controladorFirma_ As New ControladorFirmaElectronica

        Dim documento_ = documentoElectronico_

        pbDetalleDomicilioDestinatario.ForEach(Sub(pillbox_ As PillBox)
                                                   Dim cambios_ = False

                                                   If pillbox_.GetControlValue(icIdTarjeta) Is Nothing Then

                                                       Dim id_ = ObjectId.GenerateNewId()

                                                       pbDetalleDomicilioDestinatario.setValueInvisible(icIdTarjeta, pillbox_.GetIdentity, id_)

                                                       pbDetalleDomicilioDestinatario.setValueInvisible(icFirmaTarjeta, pillbox_.GetIdentity, controladorFirma_.Generar(id_, 1))

                                                   Else

                                                       validaCambiosTarjeta(pillbox_, documento_, cambios_)

                                                   End If

                                                   If cambios_ Then

                                                       pbDetalleDomicilioDestinatario.setValueInvisible(icFirmaTarjeta, pillbox_.GetIdentity, controladorFirma_.Generar(ObjectId.Parse(pillbox_.GetControlValue(icIdTarjeta)), 1))

                                                   End If
                                               End Sub)

    End Sub

    Public Overrides Sub DespuesBuquedaGeneralConDatos()

        _tagwatcher = New TagWatcher

        swcbuscarempresa.Visible = False

        fsHistorialDomicilios.Visible = IIf(_cantidadDetalles > 0, True, False)

        PreparaTarjetero(PillboxControl.ToolbarModality.Default, pbDetalleDomicilioDestinatario)

        fcRazonSocialDestinatario.Enabled = False

        With OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

            With .Seccion(SeccionesDestinatarios.SDES1)

                fcRazonSocialDestinatario.Value = .Campo(CamposDestinatario.CP_ID_EMPRESA).Valor.ToString

                icClave.Text = ""

                icClave.Text = .Campo(CamposDestinatario.CP_CVE_DESTINATARIO).Valor

                swcHabilitado.Visible = True

                swcHabilitado.Checked = .Campo(CamposDestinatario.CA_DESTINATARIO_HABILITADO).Valor

                Dim datosdestinatarioactual_ = _utils.ObtenerDatosDestinatarioDesdeControlador(OperacionGenerica.Id)

                Dim tipoempresa_ = .Campo(CamposDestinatario.CP_TIPO_DESTINATARIO).Valor

                SetVars("_esempresaextranjera", tipoempresa_)

                SetVars("_auxiliardestinatario", datosdestinatarioactual_)

            End With

        End With

        EstadoConexion()

    End Sub

    Public Overrides Sub DespuesBuquedaGeneralSinDatos()

        If OperacionGenerica IsNot Nothing Then

            With OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

                With .Seccion(SeccionesDestinatarios.SDES1)

                    icClave.Text = ""

                    icClave.Text = .Campo(CamposDestinatario.CP_CVE_DESTINATARIO).Valor

                    Dim tipoempresa_ = .Campo(CamposDestinatario.CP_TIPO_DESTINATARIO).Valor

                    SetVars("_esempresaextranjera", tipoempresa_)

                End With

            End With

            Session("_tbDetalleProveedor") = PillboxControl.ToolbarModality.Default : pbDetalleDomicilioDestinatario.Modality = Session("_tbDetalleProveedor")

            PreparaTarjetero(PillboxControl.ToolbarModality.Default, pbDetalleDomicilioDestinatario)

            pbDetalleDomicilioDestinatario.Enabled = True

            fcRazonSocialDestinatario.Enabled = False

            swcbuscarempresa.Visible = False

            Dim datosdestinatarioactual_ = _utils.ObtenerDatosDestinatarioDesdeControlador(OperacionGenerica.Id)

            SetVars("_auxiliardestinatario", datosdestinatarioactual_)

            SetVars("_listaDomiciliosNacionales", datosdestinatarioactual_._listadomiciliosconTaxid)

            EstadoConexion()

        End If

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

                    .Campo(CamposDestinatario.CA_DESTINATARIO_HABILITADO).Valor = True

                    .Campo(CamposDestinatario.CA_DESTINATARIO_HABILITADO).ValorPresentacion = "Online"

                Else

                    .Campo(CamposDestinatario.CA_DESTINATARIO_HABILITADO).Valor = False

                    .Campo(CamposDestinatario.CA_DESTINATARIO_HABILITADO).ValorPresentacion = "Offline"

                End If

                SetVars("tipoDestinatario_", .Campo(CamposDestinatario.CP_TIPO_DESTINATARIO).Valor)

            End If

            _empresa = IIf(.Campo(CamposDestinatario.CP_TIPO_DESTINATARIO).Valor, New EmpresaInternacional, New EmpresaNacional)

            If GetVars("_empresa") IsNot Nothing Then

                _empresa = GetVars("_empresa")

            End If

            _auxiliarDestinatario = New AuxiliarDestinatario

            If GetVars("_auxiliardestinatario") IsNot Nothing Then

                _auxiliarDestinatario = DirectCast(GetVars("_auxiliardestinatario"), AuxiliarDestinatario)

            End If

            'LISTA DOMICILIOS DESTINATARIOS
            Dim domiciliosdestinatario_ = pbDetalleDomicilioDestinatario.DataSource

            Dim i_ = 1

            Dim indice_ = 0

            If _auxiliarDestinatario._listadomiciliosconTaxid IsNot Nothing Then

                If _auxiliarDestinatario._listadomiciliosconTaxid.Count > 0 Then

                    For Each item_ In domiciliosdestinatario_

                        With .Seccion(SeccionesDestinatarios.SDES2).Partida(numeroSecuencia_:=i_)

                            ''LO PONDRÉ ASI HASTA QUE VEA QUE SHOW,
                            ''O SIMPLEMENTE NO DEJE MODIFICAR ESOS CAMPOS, CREO QUE SERÁ LO MAS SANO
                            ''POR DISEÑO NO DEJARÉ HACER ESO
                            ''PIENSA BIEN COMO SOLUCIONAR ESTO, ES LA CLAVE DE ESTE DISEÑO
                            ''If item_("icIdDomicilio") <> _auxiliarDestinatario._listadomiciliosconTaxid(indice_)._iddomicilio.ToString Then

                            .Campo(CamposDestinatario.CP_ID_DOMICILIO_DESTINATARIO).Valor = _auxiliarDestinatario._listadomiciliosconTaxid(indice_)._iddomicilio.ToString

                            .Campo(CamposDestinatario.CP_SEC_DOMICILIO_DESTINATARIO).Valor = _auxiliarDestinatario._listadomiciliosconTaxid(indice_).sec

                            .Campo(CamposDomicilio.CA_CVE_PAIS).Valor = _auxiliarDestinatario._listadomiciliosconTaxid(indice_).cvePais

                            .Campo(CamposDomicilio.CA_ID_PAIS).Valor = _auxiliarDestinatario._listadomiciliosconTaxid(indice_).idpais

                            .Campo(CamposDestinatario.CA_CVE_TAX_ID_DESTINATARIO).Valor = _auxiliarDestinatario._listadomiciliosconTaxid(indice_).clavetaxid

                            .Campo(CamposDestinatario.CA_DOMICILIO_FISCAL_DESTINATARIO).Valor = _auxiliarDestinatario._listadomiciliosconTaxid(indice_).domicilioPresentacion

                            .Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor = $"{_auxiliarDestinatario._listadomiciliosconTaxid(indice_).numeroexterior} -  {_auxiliarDestinatario._listadomiciliosconTaxid(indice_).numerointerior}"

                            .Campo(CamposDomicilio.CA_ENTIDAD_MUNICIPIO).Valor = _auxiliarDestinatario._listadomiciliosconTaxid(indice_).cveMunicipio

                            .Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor = _auxiliarDestinatario._listadomiciliosconTaxid(indice_).cveEntidadfederativa

                            .Campo(CamposDestinatario.CA_ESTADO_DOMICILIO_DESTINATARIO).Valor = CBool(_auxiliarDestinatario._listadomiciliosconTaxid(indice_).estado)

                            .Campo(CamposDestinatario.CA_DOMICILIO_ARCHIVADO_DESTINATARIO).Valor = _auxiliarDestinatario._listadomiciliosconTaxid(indice_).archivado

                            '.Campo(CamposDestinatario.CA_MOTIVO_ARCHIVADO_DOMICILIO_DESTINATARIO).Valor = _auxiliarDestinatario._listadomiciliosconTaxid(indice_).mo

                            '.Campo(CamposDestinatario.CA_FECHA_ARCHIVADO_DOMICILIO_DESTINATARIO).Valor = _auxiliarDestinatario._listadomiciliosconTaxid(indice_)("fechaarchivado_")

                        End With

                        i_ += 1

                        indice_ += 1

                    Next

                End If

            End If

            ''HACER UNA LISTA PARA OBTENER SOLO LOS DOMICILIOS ARCHIVADOS Y SOLO ESO MOSTRAR

        End With

    End Sub

    'EVENTOS DE MANTENIMIENTO
    Public Overrides Sub LimpiaSesion()

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

        _cantidadDetalles = Nothing

        pbDetalleDomicilioDestinatario.DataSource = Nothing

        icClave.Text = ""

        swcHabilitado.Checked = False

        swcHabilitado.Visible = False

        _datospaismexicano = Nothing

    End Sub

#End Region

#Region "████████████████  QUINTA CAPA  █████████       Reglas locales         ██████████████████████████████"
    '       * Aquí se pueden colocar los eventos de los componentes, funciones o metodos exclusios del modulo
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

#End Region

#Region "Buscar empresas por razon social"
    Protected Sub swcbuscarempresa_CheckedChanged(sender As Object, e As EventArgs)

        ConfigurarDomiciliosNacionales.Visible = False

        ConfigurarDomicilios.Visible = False

        fcRazonSocialDestinatario.DataSource = Nothing

        fcRazonSocialDestinatario.Value = Nothing

        fcRazonSocialDestinatario.Text = Nothing

    End Sub

    Protected Sub fcRazonSocialDestinatario_TextChanged(sender As Object, e As EventArgs)

        _lista = New List(Of SelectOption)

        _datospaismexicano = New PaisDomicilio

        If swcbuscarempresa.Checked Then

            _lista = _utils.ListarEmpresasPorRazonSocial(fcRazonSocialDestinatario.Text)

        Else

            _lista = _utils.ListarEmpresasPorRazonSocial(fcRazonSocialDestinatario.Text,
                                                         tipoempresa_:=IControladorEmpresas.TiposEmpresas.Nacional)

        End If

        If _lista.Count > 0 Then

            fcRazonSocialDestinatario.DataSource = _lista

            If swcbuscarempresa.Checked = False Then

                pbDetalleDomicilioDestinatario.Enabled = True

                ConfiguracionDomicilioNacional()

            End If

        Else
            MsgNoExisteEmpresa()

            If swcbuscarempresa.Checked Then

                ConfigurarDomicilios.Visible = True

                ConfigurarDomicilios.Enabled = True

                ConfigurarDomiciliosNacionales.Visible = False

                ConfigurarDomiciliosNacionales.Enabled = False

                icPais.Value = Nothing

                icIdPais.Value = Nothing

                icCvePais.Value = Nothing

                ''REVISAR PORQUE SE BORRA TODO
                lbTitle.Visible = True

                fcPaises.DataSource = Nothing

                fcPaises.Text = Nothing

                fcPaises.Visible = True

            Else

                ConfiguracionDomicilioNacional()

            End If

        End If

    End Sub

    Protected Sub ConfiguracionDomicilioNacional()

        ConfigurarDomiciliosNacionales.Visible = True

        ConfigurarDomiciliosNacionales.Enabled = True

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

    Protected Sub fcRazonSocialDestinatario_Click(sender As Object, e As EventArgs)

        If fcRazonSocialDestinatario.Text <> "" Then

            If BuscarSiExisteDestinatario() Then

                MsgValidacionRazonsocial()

                ConfigurarDomiciliosNacionales.Visible = False

                ConfigurarDomicilios.Visible = False

                pbDetalleDomicilioDestinatario.Enabled = False

                ' scDomiciliosRegistrados.Visible = False
            Else

                If swcbuscarempresa.Checked Then

                    ConfigurarDomicilios.Visible = True

                    ConfigurarDomicilios.Enabled = True

                    ConfigurarDomiciliosNacionales.Visible = False

                    ConfigurarDomiciliosNacionales.Enabled = False

                    ''REVISAR PORQUE SE BORRA TODO
                    lbTitle.Visible = True

                    fcPaises.DataSource = Nothing

                    fcPaises.Text = Nothing

                    fcPaises.Visible = True

                    icPais.Value = Nothing

                    icIdPais.Value = Nothing

                    icCvePais.Value = Nothing

                Else

                    ConfigurarDomiciliosNacionales.Visible = True

                    ConfigurarDomiciliosNacionales.Enabled = True

                    _datospaismexicano = New PaisDomicilio

                    _datospaismexicano = _utils.ObtenerDatosPaisMexicano()

                    With _datospaismexicano

                        icPais.Value = .paisPresentacion

                        icIdPais.Value = .idpais.ToString

                        icCvePais.Value = .pais

                    End With

                    ConfigurarDomicilios.Visible = False

                    ConfigurarDomicilios.Enabled = False

                    ListarDomiciliosPorPais(IControladorEmpresas.TiposEmpresas.Nacional)

                End If

            End If

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

            pbDetalleDomicilioDestinatario.Enabled = False

            ConfigurarDomicilios.Visible = False

            ConfigurarDomicilios.Enabled = False

            ConfigurarDomiciliosNacionales.Visible = False

            ConfigurarDomiciliosNacionales.Enabled = False

            icPais.Value = Nothing

            icIdPais.Value = Nothing

            icCvePais.Value = Nothing

        End If

    End Sub

#End Region

#Region "Paises"

    Protected Sub fcPaises_Click(sender As Object, e As EventArgs)

        If fcPaises.Text <> "" Then

            Dim pais_ = GetVars("_idpais")


            If fcPaises.Value = pais_ Then

                ListarDomiciliosPorPais(IControladorEmpresas.TiposEmpresas.Nacional)

            Else

                scDomiciliosRegistrados.DataSource = Nothing

                scDomiciliosRegistrados.Visible = True

                ListarDomiciliosPorPais()

            End If

        Else

            ConfigurarDomiciliosNacionales.Visible = False

            ListaDomiciliosNacionales.DataSource = Nothing

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

#End Region

    Protected Function BuscarSiExisteDestinatario() As Boolean

        Dim buscarDestinatarioExistente_ As New ControladorBusqueda(Of ConstructorDestinatario)

        Dim lista_ = buscarDestinatarioExistente_.Buscar(fcRazonSocialDestinatario.Text,
                                                              New Filtro _
                                                              With {.IdSeccion = SeccionesDestinatarios.SDES1,
                                                                    .IdCampo = CamposDestinatario.CA_RAZON_SOCIAL})
        If lista_ IsNot Nothing Then

            If lista_.Count > 0 Then

                ' aviso.Visible = True

                Return True

            End If

        End If

        Return False

    End Function


    Function CargaPaises(ByRef control_ As FindboxControl) As List(Of SelectOption)

        Dim paisesTemporales_ As New List(Of Pais)

        Dim lista_ As List(Of SelectOption) = ControladorPaises.BuscarPaises(paisesTemporales_, control_.Text)

        control_.DataSource = lista_

        Return lista_

    End Function


    Sub LimpiarTarjetero()

        Dim pillboxControl_ As PillboxControl = pbDetalleDomicilioDestinatario

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

        pbDetalleDomicilioDestinatario = pillboxControl_

        pbDetalleDomicilioDestinatario.PillBoxDataBinding()

        SetVars("_listaDomicilios", Nothing)

    End Sub

    Public Sub RegresarControles(Optional ByVal opcion_ As Int32 = 1)

        If pbDetalleDomicilioDestinatario.PageIndex > 0 Then

            lbNumero.Text = pbDetalleDomicilioDestinatario.PageIndex.ToString()

        End If

    End Sub


    Protected Sub pbDetalleDomicilioDestinatario_CheckedChange(sender As Object, e As EventArgs)

        RegresarControles()

    End Sub

    Protected Sub pbDetalleDomicilioDestinatario_Click(sender As Object, e As EventArgs)

        RegresarControles()

        Select Case pbDetalleDomicilioDestinatario.ToolbarAction

            Case PillboxControl.ToolbarActions.Nuevo

                With pbDetalleDomicilioDestinatario

                    lbNumero.Text = .PageIndex.ToString()

                    Dim itemActual_ As Integer = .PageIndex

                    Dim index_ As Integer = itemActual_ - 2

                    If GetVars("_esempresaextranjera") IsNot Nothing Then

                        If GetVars("_esempresaextranjera") = True Then

                            ConfigurarDomicilios.Enabled = True

                            ConfigurarDomicilios.Visible = True

                            ConfigurarDomiciliosNacionales.Enabled = False

                            ConfigurarDomiciliosNacionales.Visible = False

                        Else
                            ConfigurarDomiciliosNacionales.Enabled = True

                            ConfigurarDomiciliosNacionales.Visible = True

                            ConfigurarDomicilios.Enabled = False

                            ConfigurarDomicilios.Visible = False

                            Dim pais_ = _utils.ObtenerDatosPaisMexicano()

                            icIdPais.Value = pais_.idpais.ToString

                            icCvePais.Value = pais_.pais

                            icPais.Value = pais_.paisPresentacion

                        End If

                    End If

                    icTaxid.Value = ""

                End With

                If GetVars("_auxiliardestinatario") IsNot Nothing Then

                    Dim auxDomicilioTaxid_ As New DomiciliosTaxid

                    _auxiliarDestinatario = DirectCast(GetVars("_auxiliardestinatario"), AuxiliarDestinatario)

                    _auxiliarDestinatario._listadomiciliosconTaxid.Add(auxDomicilioTaxid_)

                End If

                SetVars("_auxiliardestinatario", _auxiliarDestinatario)

        End Select

    End Sub

    Protected Sub RegresarControlesPorDefault()

        Dim modoEditando_ As Boolean = False

        If GetVars("isEditing") IsNot Nothing Then

            If GetVars("isEditing") = True Then

                modoEditando_ = True

            End If

        End If

        If pbDetalleDomicilioDestinatario.PageIndex > 0 Then

            lbNumero.Text = pbDetalleDomicilioDestinatario.PageIndex.ToString()

        End If

        Session("_tbDetalleProveedor") = PillboxControl.ToolbarModality.Default : pbDetalleDomicilioDestinatario.Modality = Session("_tbDetalleProveedor")

        If modoEditando_ Then

            PreparaTarjetero(PillboxControl.ToolbarModality.Advanced, pbDetalleDomicilioDestinatario)

            fsDatosGenerales.Enabled = True

            fsHistorialDomicilios.Enabled = True

        Else

            PreparaTarjetero(PillboxControl.ToolbarModality.Default, pbDetalleDomicilioDestinatario)

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


        CargarHistorialDomicilios()

    End Sub

    Protected Sub CargarHistorialDomicilios()

        Dim i = 1

        pbDetalleDomicilioDestinatario.ForEach(Sub(pillbox_ As PillBox)
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

    Private Function ListarEmpresas(Of T)() As List(Of SelectOption)

        Dim lista_ As New List(Of SelectOption)

        _tagwatcher = New TagWatcher

        _controladorEmpresas = New Rec.Globals.Controllers.Empresas.ControladorEmpresas(New EspacioTrabajo(),
                                                                                            Rec.Globals.Controllers.Empresas.IControladorEmpresas.TiposEmpresas.Internacional) _
                                                                                            With {.ListaEmpresas = New List(Of IEmpresa)}
        With _controladorEmpresas

            _tagwatcher = .Consultar(fcRazonSocialDestinatario.Text)

            If _tagwatcher.Status = TypeStatus.Ok Then

                Dim listaempresas_ = _tagwatcher.ObjectReturned

                If listaempresas_.count > 0 Then

                    SetVars("_listaEmpresasTemporales", listaempresas_)

                    For Each item_ In listaempresas_
                        lista_.Add(New SelectOption _
                               With {
                                    .Value = item_._id.ToString,
                                    .Text = item_.razonsocial})
                    Next

                End If

            End If

        End With

        Return lista_

    End Function

    Private Sub CambiarDomicilioNacional(ByVal cveDomicilio_ As String,
                                 Optional ByVal indice_ As Integer = 0)

        _empresaNacional = New EmpresaNacional

        If GetVars("_empresa") IsNot Nothing Then

            _empresaNacional = DirectCast(GetVars("_empresa"), EmpresaNacional)

        End If

        ''OBTENER EL DOMICILIO SELECCIONADO
        Dim domicilioseleccionado_ = _utils.ObtenerDomicilioEnPais(_empresaNacional, New ObjectId(icIdPais.Value), New ObjectId(cveDomicilio_))

        If domicilioseleccionado_ IsNot Nothing Then

            pbDetalleDomicilioDestinatario.Enabled = True

            If indice_ <> 0 Then

                LlenarTarjetero(New List(Of Domicilio) From {domicilioseleccionado_},
                                indice_,
                                _empresaNacional.rfc,
                                _empresaNacional._idrfc.ToString)

            Else

                CargarTarjetero(New List(Of Domicilio) From {domicilioseleccionado_})

            End If

        End If

    End Sub


    Private Sub CambiarDomicilio(ByVal cveDomicilio_ As String, Optional ByVal indice_ As Integer = 0)
        ''DEBE SER EMPRESA EXTRANJERA
        _empresaInternacional = New EmpresaInternacional

        If GetVars("_empresa") IsNot Nothing Then

            _empresaInternacional = DirectCast(GetVars("_empresa"), EmpresaInternacional)

        End If

        ''OBTENER EL DOMICILIO SELECCIONADO
        Dim domicilioseleccionado_ = _utils.ObtenerDomicilioEnPais(_empresaInternacional, New ObjectId(icIdPais.Value), New ObjectId(cveDomicilio_))

        If domicilioseleccionado_ IsNot Nothing Then

            pbDetalleDomicilioDestinatario.Enabled = True

            If indice_ <> 0 Then

                LlenarTarjetero(New List(Of Domicilio) From {domicilioseleccionado_},
                                indice_,
                                _empresaInternacional.taxids.Last().taxid,
                                _empresaInternacional.taxids.Last().idtaxid.ToString)

            Else

                CargarTarjetero(New List(Of Domicilio) From {domicilioseleccionado_})

            End If

        End If

    End Sub

    Sub LlenarTarjetero(ByVal domicilio_ As List(Of Rec.Globals.Empresas.Domicilio),
                        ByVal indice_ As Integer,
                        Optional ByVal taxid_ As String = Nothing,
                        Optional ByVal cvetaxid_ As String = Nothing)

        _pillboxControl = New PillboxControl

        _pillboxControl = pbDetalleDomicilioDestinatario

        For Each item_ In domicilio_

            _pillboxControl.SetPillbox(Sub(pillbox_ As PillBox)

                                           pillbox_.SetIndice(_pillboxControl.KeyField, indice_)

                                           pillbox_.SetFiled(False)

                                           icTaxid.Value = taxid_

                                           icCveTaxid.Value = cvetaxid_

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

        Dim pillboxControl_ As PillboxControl = pbDetalleDomicilioDestinatario

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

        pbDetalleDomicilioDestinatario = pillboxControl_

        pbDetalleDomicilioDestinatario.PillBoxDataBinding()

        SetVars("_listaDomicilios", pbDetalleDomicilioDestinatario.DataSource)

    End Sub

    Protected Sub ListarDomiciliosPorPais(Optional ByVal tipoempresa_ As IControladorEmpresas.TiposEmpresas =
                                          IControladorEmpresas.TiposEmpresas.Internacional)

        If fcRazonSocialDestinatario.Value <> "" Then

            Dim idpais_ = GetVars("_idpais")

            If fcPaises.Value IsNot Nothing AndAlso fcPaises.Value <> "" Then

                idpais_ = fcPaises.Value

            End If

            Dim data_ = _utils.ListaDomiciliosPorPais(fcRazonSocialDestinatario.Value, idpais_, tipoempresa_)

            If data_.Count > 0 Then

                If tipoempresa_ = IControladorEmpresas.TiposEmpresas.Internacional Then

                    If data_.Count = 1 Then

                        scDomiciliosRegistrados.DataSource = data_

                        scDomiciliosRegistrados.Value = data_(0).Value
                    Else

                        scDomiciliosRegistrados.DataSource = data_

                    End If

                Else

                    If data_.Count = 1 Then

                        ListaDomiciliosNacionales.DataSource = data_

                        ListaDomiciliosNacionales.Value = data_(0).Value

                        SetVars("_domiciliounicoNacional", data_(0).Value)
                        ''UTILIZARE VARIABLE DE SESION YA QUE EL COMPONENTE NO ME ESTA GUARDANDO EL VALOR
                    Else

                        ListaDomiciliosNacionales.DataSource = data_

                        MsgSeleccioneDomicilioDestinatario()
                        'ListaDomiciliosNacionales.Value = data_.Last.Value

                        SetVars("_listaDomiciliosNacionales", data_)
                        'ListaDomiciliosNacionales

                    End If

                End If

            Else

                If tipoempresa_ = IControladorEmpresas.TiposEmpresas.Internacional Then

                    scDomiciliosRegistrados.DataSource = Nothing

                    scDomiciliosRegistrados.Visible = False

                Else

                    ListaDomiciliosNacionales.DataSource = Nothing

                    ListaDomiciliosNacionales.Value = Nothing

                    ConfigurarDomiciliosNacionales.Visible = False

                End If

            End If

        Else

            scDomiciliosRegistrados.DataSource = Nothing

            scDomiciliosRegistrados.Visible = False

            ListaDomiciliosNacionales.DataSource = Nothing

            ListaDomiciliosNacionales.Value = Nothing

            ConfigurarDomiciliosNacionales.Visible = False

            pbDetalleDomicilioDestinatario.Enabled = True

        End If

    End Sub

#Region "Limpieza"

    Sub LimpiarListaDomiciliosActuales()

        lbtitleDomicilios.Visible = False

        scDomiciliosRegistrados.Visible = False

        scDomiciliosRegistrados.DataSource = Nothing

        SetVars("_opcionesLista", Nothing)

        SetVars("_listaDomicilios", Nothing)

    End Sub

    Public Overrides Function AgregarComponentesBloqueadosEdicion() As List(Of WebControl)

        Return New List(Of WebControl) From {icClave, fcRazonSocialDestinatario, icPais}

    End Function

#End Region


#Region "Avisos / Tooltips"

    Protected Sub swcHabilitado_CheckedChanged(sender As Object, e As EventArgs)

        If swcHabilitado.Checked Then

            DisplayMessage("🟣 Destinatario online")

            EstadoConexion()

        Else

            DisplayMessage("⚪ Destinatario offline", StatusMessage.Info)

            EstadoConexion()

        End If

    End Sub

    Public Sub EstadoConexion()
        ' Visual Basic
        Dim estadoConexion_ As String

        If swcHabilitado.Visible Then

            If swcHabilitado.Checked Then

                estadoConexion_ = "<span style='color:#757575; font-size:1.2rem'>🟣 Online</span>"

            Else

                estadoConexion_ = "<span style='color:#757575; font-size:1.2rem'>⚪ Offline</span>"

            End If

            online.Text = estadoConexion_

        End If

    End Sub

    Protected Sub MsgValidacionRazonsocial()

        With fcRazonSocialDestinatario
            .ToolTip = "🔴 Destinatario ya registrado. "
            .ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors
            .ToolTipModality = IUIControl.ToolTipModalities.Ondemand
            .ShowToolTip()
        End With

    End Sub

    Protected Sub MsgValidacionRazonsocialVacio()

        With fcRazonSocialDestinatario
            .ToolTip = "👉 Indica una razón social. "
            .ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors
            .ToolTipModality = IUIControl.ToolTipModalities.Ondemand
            .ShowToolTip()
        End With

    End Sub

    Protected Sub MsgValidacionCalleVacio()

        With icCalle
            .ToolTip = "👉 Indica una calle. "
            .ToolTipExpireTime = 4
            .ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors
            .ToolTipModality = IUIControl.ToolTipModalities.Ondemand
            .ShowToolTip()
        End With

    End Sub

    Protected Sub MsgValidacionPaisVacio()

        With icPais
            .ToolTip = "👉 Indica un país. "
            .ToolTipExpireTime = 4
            .ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors
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

    Protected Sub MsgValidacionPaisValidoControl()

        With fcPaises
            .ToolTip = "🔴 País no válido."
            .ToolTipExpireTime = 4
            .ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors
            .ToolTipModality = IUIControl.ToolTipModalities.Ondemand
            .ShowToolTip()
        End With

    End Sub

    Protected Sub MsgValidacionPais()

        With fcPaises
            .ToolTip = "👉 Indica un país. "
            .ToolTipExpireTime = 4
            .ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors
            .ToolTipModality = IUIControl.ToolTipModalities.Ondemand
            .ShowToolTip()
        End With

    End Sub

    Protected Sub MsgSeleccioneDomicilioDestinatario()
        With ListaDomiciliosNacionales
            .ToolTip = "👉 Indica un domicilio. "
            .ToolTipExpireTime = 4
            .ToolTipStatus = IUIControl.ToolTipTypeStatus.OkInfo
            .ToolTipModality = IUIControl.ToolTipModalities.Ondemand
            .ShowToolTip()
        End With
    End Sub

    Protected Sub MsgNoExisteEmpresa()

        With fcRazonSocialDestinatario
            .ToolTip = "👉 Razón social libre."
            .ToolTipExpireTime = 6
            .ToolTipStatus = IUIControl.ToolTipTypeStatus.Ok
            .ToolTipModality = IUIControl.ToolTipModalities.Ondemand
            .ShowToolTip()
        End With

    End Sub

    Protected Sub MsgExisteDestinatario()

        With fcRazonSocialDestinatario
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

#Region "██████ Vinculación sexta capa  █████████       SAX      ████████████████████████████████████████████"
    '    ██████   Controladores utilizados                     Documentos por coding para MongoDB      ██████
    '    ██████    1.ControladorEmpresas                        1. En Empresa:                         ██████
    '    ██████    2.ControladorRecursosAduanales                  a). Domicilios                      ██████
    '    ██████    3.ControladorSecuencias                         b). Contactos                       ██████
    '    ██████                                                                                        ██████
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

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

        _auxiliarDestinatario = New AuxiliarDestinatario

        _empresaNacional = New EmpresaNacional

        _tagwatcher = New TagWatcher

        If GetVars("_empresa") IsNot Nothing Then

            _empresaNacional = DirectCast(GetVars("_empresa"), EmpresaNacional)

        Else

            _tagwatcher = _utils.BuscarEmpresaPorObjectId(fcRazonSocialDestinatario.Value, IControladorEmpresas.TiposEmpresas.Nacional)

            _empresaNacional = _tagwatcher.ObjectReturned

        End If

        If GetVars("_auxiliardestinatario") IsNot Nothing Then
            ''ES PORQUE ES UNA MODADLIDAD DE ACTUALIZAR EL DESTINATARIO
            _auxiliarDestinatario = DirectCast(GetVars("_auxiliardestinatario"), AuxiliarDestinatario)

        Else
            ''ES ALTA DESTINATARIO
            _auxiliarDestinatario._razonsocial = _empresaNacional.razonsocial

            _auxiliarDestinatario._listadomiciliosconTaxid = New List(Of DomiciliosTaxid)

        End If

        Dim item_ = 0

        Dim totalDomicilios_ = pbDetalleDomicilioDestinatario.DataSource.Count() - 1


        pbDetalleDomicilioDestinatario.ForEach(Sub(pillbox_ As PillBox)
                                                   ''AQUI ES EL RFC
                                                   Dim idrfc_ = pillbox_.GetControlValue(icCveTaxid)

                                                   Dim rfc_ = pillbox_.GetControlValue(icTaxid)

                                                   ''PAIS Y DOMICILIOS
                                                   Dim idpais_ = pillbox_.GetControlValue(icIdPais)

                                                   Dim cvepais_ = pillbox_.GetControlValue(icCvePais)

                                                   Dim pais_ = pillbox_.GetControlValue(icPais)

                                                   If idrfc_ <> "" Then
                                                       ''VERIFICAMOS QUE EXISTA EN LA EMPRESA
                                                       existerfc_ = _utils.ExisteidRFCEmpresa(_empresaNacional, ObjectId.Parse(idrfc_))

                                                       If existerfc_ = False Then
                                                           ''GENEREMOS NUEVA ESTRUCTURA DE RFC Y LA VAMOS AÑADIR AL 
                                                           ''LA ESTRUCTURA DE LA EMPRESA
                                                           _empresaNacional = _utils.GenerarEmpresaNacionalConEstructuraRFCNuevo(_empresaNacional, rfc_)

                                                           idrfc_ = _empresaNacional._idrfc.ToString

                                                       Else

                                                           coinciderfc_ = _utils.CoincideRFCEmpresa(_empresaNacional, rfc_)

                                                           If coinciderfc_ = False Then

                                                               ''GENERAMOS TODA LA ESTRCUTURA DEL RFC
                                                               ''Y LA AGREGAMOS A LA ESTRUCTURA DE LA EMPRESA
                                                               _empresaNacional = _utils.GenerarEmpresaNacionalConEstructuraRFCNuevo(_empresaNacional, rfc_)

                                                               idrfc_ = _empresaNacional._idrfc.ToString

                                                           End If

                                                       End If

                                                   Else

                                                       coinciderfc_ = _utils.CoincideRFCEmpresa(_empresaNacional, rfc_)

                                                       If coinciderfc_ = False Then

                                                           ''GENERAMOS TODA LA ESTRCUTURA DEL RFC
                                                           ''Y LA AGREGAMOS A LA ESTRUCTURA DE LA EMPRESA
                                                           _empresaNacional = _utils.GenerarEmpresaNacionalConEstructuraRFCNuevo(_empresaNacional, rfc_)

                                                           idrfc_ = _empresaNacional._idrfc.ToString

                                                       Else

                                                           ''AQUI DEBE TOMAR EL ID ACTUAL DE ESE RFC DE LA EMPRESA
                                                           idrfc_ = _empresaNacional._idrfc.ToString

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
                                                   With _auxiliarDestinatario

                                                       If modoEditando_ Then

                                                           ._listadomiciliosconTaxid(item_)._iddomicilio = ultimoDomicilio_._iddomicilio

                                                           ._listadomiciliosconTaxid(item_).sec = totalDomicilios_ + 1

                                                           ._listadomiciliosconTaxid(item_).domicilioPresentacion = ultimoDomicilio_.domicilioPresentacion

                                                           ._listadomiciliosconTaxid(item_).clavetaxid = idrfc_

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

                                                               .clavetaxid = idrfc_

                                                               .cvePais = cvepais_

                                                               .idpais = idpais_

                                                               .numeroexterior = ultimoDomicilio_.numeroexterior

                                                               .numerointerior = ultimoDomicilio_.numerointerior

                                                           End With

                                                           _auxiliarDestinatario._listadomiciliosconTaxid.Add(auxDomicilioConTaxid_)

                                                       End If

                                                   End With
                                                   item_ += 1
                                               End Sub)

        SetVars("_empresa", _empresaNacional)

        SetVars("_auxiliardestinatario", _auxiliarDestinatario)

    End Sub

    Protected Sub GuardarEmpresaNacional(ByVal session_ As IClientSessionHandle,
                                         ByVal esempresanueva_ As Boolean)

        Dim modoEditando_ As Boolean = False

        _auxiliarDestinatario = New AuxiliarDestinatario

        _tagwatcher = New TagWatcher

        If GetVars("isEditing") IsNot Nothing Then

            If GetVars("isEditing") = True Then

                modoEditando_ = True

            End If

        End If

        ''Es nueva la empresa
        _auxiliarDestinatario = _utils.ObtenerDatosDesdePillbox(fcRazonSocialDestinatario.Text, pbDetalleDomicilioDestinatario)

        With _auxiliarDestinatario

            _empresaNacional = New EmpresaNacional

            _empresaNacional = _utils.GenerarEstructuraEmpresa(._razonsocial, ._taxid, ._listadomiciliosconTaxid.Last, IEmpresaNacional.TiposPersona.Moral, IControladorEmpresas.TiposEmpresas.Nacional)

            _tagwatcher = _utils.GuardarEmpresa(_empresaNacional, Nothing)

            If _tagwatcher.Status = TypeStatus.Ok Then

                _auxiliarDestinatario.id = _empresaNacional._id.ToString

                _auxiliarDestinatario._listadomiciliosconTaxid.Last.clavetaxid = _empresaNacional.rfcs.Last.idrfc.ToString

                _auxiliarDestinatario._listadomiciliosconTaxid.Last._iddomicilio = _empresaNacional.paisesdomicilios.Last.domicilios.Last._iddomicilio

                _auxiliarDestinatario._listadomiciliosconTaxid.Last.sec = _empresaNacional.paisesdomicilios.Last.domicilios.Last.sec

                _auxiliarDestinatario._listadomiciliosconTaxid.Last.domicilioPresentacion = _empresaNacional.paisesdomicilios.Last.domicilios.Last.domicilioPresentacion

                SetVars("_empresa", _empresaNacional)

            End If

            SetVars("_auxiliardestinatario", _auxiliarDestinatario)

        End With

    End Sub

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

        _auxiliarDestinatario = New AuxiliarDestinatario

        _empresaInternacional = New EmpresaInternacional

        _tagwatcher = New TagWatcher

        If GetVars("_empresa") IsNot Nothing Then

            _empresaInternacional = DirectCast(GetVars("_empresa"), EmpresaInternacional)

        Else

            _tagwatcher = _utils.BuscarEmpresaPorObjectId(fcRazonSocialDestinatario.Value, IControladorEmpresas.TiposEmpresas.Internacional)

            _empresaInternacional = _tagwatcher.ObjectReturned

        End If

        If GetVars("_auxiliardestinatario") IsNot Nothing Then
            ''ES PORQUE ES UNA MODADLIDAD DE ACTUALIZAR EL DESTINATARIO
            _auxiliarDestinatario = DirectCast(GetVars("_auxiliardestinatario"), AuxiliarDestinatario)

        Else
            ''ES ALTA DESTINATARIO
            '  _auxiliarDestinatario.id = _empresaInternacional._id.ToString

            _auxiliarDestinatario._razonsocial = _empresaInternacional.razonsocial

            _auxiliarDestinatario._listadomiciliosconTaxid = New List(Of DomiciliosTaxid)

        End If

        Dim item_ = 0

        Dim totalDomicilios_ = pbDetalleDomicilioDestinatario.DataSource.Count() - 1

        pbDetalleDomicilioDestinatario.ForEach(Sub(pillbox_ As PillBox)

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

                                                   With _auxiliarDestinatario

                                                       If modoEditando_ Then

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

                                                           _auxiliarDestinatario._listadomiciliosconTaxid.Add(auxDomicilioConTaxid_)

                                                       End If

                                                   End With

                                                   item_ += 1

                                               End Sub)

        SetVars("empresa_", _empresaInternacional)

        SetVars("_auxiliardestinatario", _auxiliarDestinatario)

    End Sub

    Protected Sub GuardarEmpresaInternacional(ByVal session_ As IClientSessionHandle, ByVal esempresanueva_ As Boolean)

        _auxiliarDestinatario = New AuxiliarDestinatario

        _empresaInternacional = New EmpresaInternacional

        _tagwatcher = New TagWatcher

        ''Es nueva la empresa
        _auxiliarDestinatario = _utils.ObtenerDatosDesdePillbox(fcRazonSocialDestinatario.Text, pbDetalleDomicilioDestinatario)

        With _auxiliarDestinatario

            _empresaInternacional = _utils.GenerarEstructuraEmpresa(._razonsocial, ._taxid, ._listadomiciliosconTaxid.Last)

            _tagwatcher = _utils.GuardarEmpresa(_empresaInternacional, Nothing, cvePais_:= ._listadomiciliosconTaxid.Last.cvePais)

            If _tagwatcher.Status = TypeStatus.Ok Then

                Dim paisBuscado_ = ._listadomiciliosconTaxid.Last.idpais

                Dim ultimoDomicilio_ = _empresaInternacional.paisesdomicilios _
                                            .Where(Function(p) p.idpais = ObjectId.Parse(paisBuscado_)) _
                                            .Select(Function(p) p.domicilios.LastOrDefault()) _
                                            .FirstOrDefault()

                _auxiliarDestinatario._listadomiciliosconTaxid.Last._iddomicilio = ultimoDomicilio_._iddomicilio

                _auxiliarDestinatario._listadomiciliosconTaxid.Last.sec = ultimoDomicilio_.sec

                _auxiliarDestinatario._listadomiciliosconTaxid.Last.numeroexterior = ultimoDomicilio_.numeroexterior

                _auxiliarDestinatario._listadomiciliosconTaxid.Last.numerointerior = ultimoDomicilio_.numerointerior

                _auxiliarDestinatario._listadomiciliosconTaxid.Last.domicilioPresentacion = ultimoDomicilio_.domicilioPresentacion

                _auxiliarDestinatario._listadomiciliosconTaxid.Last.clavetaxid = _empresaInternacional.taxids.Last.idtaxid.ToString

                SetVars("_empresa", _empresaInternacional)

                SetVars("_auxiliardestinatario", _auxiliarDestinatario)

            Else

                SetVars("_empresa", Nothing)

                SetVars("_auxiliardestinatario", Nothing)

            End If

        End With

    End Sub

#End Region
#Region "Aplicar domicilios"
    ''BOTON APLICAR DOMICILIO EMPRESA INTERNACIONAL

    Private Sub validaCambiosTarjeta(ByRef pillbox_ As PillBox, ByVal documeto_ As DocumentoElectronico, ByRef swCambio_ As Boolean)

        Dim partida_ = documeto_.Seccion(SeccionesProvedorOperativo.SPRO2).Nodos(pillbox_.GetIdentity - 1)

        If Not pillbox_.GetControlValue(icTaxid).Equals(partida_.Attribute(CamposDestinatario.CA_TAX_ID).Valor) Then

            If partida_.Attribute(CamposDestinatario.CA_TAX_ID).Valor IsNot Nothing Then

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

    Protected Sub btnTipoDomicilio_Click(sender As Object, e As EventArgs)

        Dim modoEditando_ As Boolean = False

        If GetVars("isEditing") IsNot Nothing Then

            If GetVars("isEditing") = True Then

                modoEditando_ = True

            End If

        End If

        If fcPaises.Text <> "" Then

            If modoEditando_ Then

                ''ESTAMOS EN MODO EDICIÓN Y AQUI YA DEBE EXISTIR UNA VARIABLE DE SESION EMPRESAS
                ''LA CUAL SE OBTIENE AL HACER UNA BUSQUEDA CON DATOS
                ''O BUSQUEDA SIN DATOS
                LlenandoTarjetero()

            Else
                ''SI SE TRATA DE UNA EMRPESA SUGERIDA, AFUERZAS TENDRA UN ID
                ''POR TANTO , PODREMOS CREAR LA VARIABLE DE SESION
                If fcRazonSocialDestinatario.Value <> "" Then

                    If BuscarSiExisteDestinatario() = False Then

                        Dim datosempresaseleccionada_ = _utils.BuscarEmpresaPorObjectId(fcRazonSocialDestinatario.Value)

                        SetVars("_empresa", datosempresaseleccionada_.ObjectReturned)

                        LlenandoTarjetero()

                        MsgIndicaTaxid()

                    End If

                Else

                    ''SI NO LA CREAMOS VACIA YA QUE ES NUEVA NUEVA LA EMRPESA
                    'MsgValidacionRazonsocialVacio()

                    SetVars("_empresa", Nothing)

                    ''CERRAR EL SUGERENCIA DEL DOMICILIO
                    ConfigurarDomicilios.Visible = False

                    ''PONER EN LOS INPUTS LOS DATOS DEL PAIS
                    icPais.Value = fcPaises.Text
                    icIdPais.Value = fcPaises.Value
                    icCvePais.Value = fcPaises.Text.Substring(0, 3)

                    ''HABILIATAR EL PILLBOX
                    pbDetalleDomicilioDestinatario.Enabled = True

                    ''PONDRÉ UN MENSAJE PARA INDICAR TAXID SI ES QUE LLEVA
                    MsgIndicaTaxid()

                    ''LIMPIAR LOS DATOS
                    fcPaises.Value = Nothing
                    fcPaises.DataSource = Nothing

                End If

            End If

        Else

            If fcRazonSocialDestinatario.Text = "" Then

                MsgValidacionRazonsocialVacio()

            End If

            MsgValidacionPais()

        End If

    End Sub

    Protected Sub btnAplicarDomicilioNacional_Click(sender As Object, e As EventArgs)

        Dim modoEditando_ As Boolean = False

        If GetVars("isEditing") IsNot Nothing Then

            If GetVars("isEditing") = True Then

                modoEditando_ = True

            End If

        End If

        If GetVars("_empresa") Is Nothing Then

            Dim datosempresaseleccionada_ = _utils.BuscarEmpresaPorObjectId(fcRazonSocialDestinatario.Value, IControladorEmpresas.TiposEmpresas.Nacional)

            SetVars("_empresa", datosempresaseleccionada_.ObjectReturned)

        End If

        If modoEditando_ Then

            LlenandoTarjeteroNacional()

        Else

            If fcRazonSocialDestinatario.Text <> "" Then

                If BuscarSiExisteDestinatario() = False Then

                    LlenandoTarjeteroNacional()

                Else

                    MsgValidacionRazonsocialVacio()

                End If

            Else

                If fcRazonSocialDestinatario.Text = "" Then

                    MsgValidacionRazonsocialVacio()

                Else

                    LlenandoTarjeteroNacional()

                End If

            End If

        End If

    End Sub
#End Region

#Region "LLenar tarjetero cuando clikean aceptar domicilio"
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

            pbDetalleDomicilioDestinatario.Enabled = True

            If icIdTarjeta.Value Is Nothing Then

                Dim controladorFirma_ As New ControladorFirmaElectronica

                Dim id_ = ObjectId.GenerateNewId()

                icIdTarjeta.Value = id_.ToString

                icFirmaTarjeta.Value = controladorFirma_.Generar(id_, 1)

            End If

            Dim domicilioSeleccionado_ As String = scDomiciliosRegistrados.Value

            Dim indice_ As Integer = pbDetalleDomicilioDestinatario.PageIndex

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

                    If domicilioSeleccionado_ = "" Then

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
            pbDetalleDomicilioDestinatario.Enabled = True

            Dim domicilioSeleccionado_ As String = Nothing

            If ListaDomiciliosNacionales.Value = "" Then

                If GetVars("_domiciliounicoNacional") IsNot Nothing Then

                    domicilioSeleccionado_ = GetVars("_domiciliounicoNacional")

                Else

                    domicilioSeleccionado_ = ListaDomiciliosNacionales.Value

                End If
            Else

                domicilioSeleccionado_ = ListaDomiciliosNacionales.Value

            End If

            Dim indice_ As Integer = pbDetalleDomicilioDestinatario.PageIndex

            If modoEditando_ Then

                If indice_ <> 0 Then

                    If domicilioSeleccionado_ = "" Then

                        ConfigurarDomiciliosNacionales.Visible = False

                    Else

                        CambiarDomicilioNacional(domicilioSeleccionado_, indice_)

                        ConfigurarDomiciliosNacionales.Visible = False

                    End If

                End If

            Else

                If domicilioSeleccionado_ = "" Then

                    LimpiarTarjetero()

                Else

                    CambiarDomicilioNacional(domicilioSeleccionado_, indice_)

                End If

                ConfigurarDomiciliosNacionales.Visible = False

            End If

        Else

            ''VALIDAR QUE EL PAIS EXISTA
            MsgValidacionPaisValido()

        End If

    End Sub

    Protected Sub ListaDomiciliosNacionales_TextChanged(sender As Object, e As EventArgs)

        If GetVars("_listaDomiciliosNacionales") IsNot Nothing Then

            ListaDomiciliosNacionales.DataSource = GetVars("_listaDomiciliosNacionales")

        End If

    End Sub

    Protected Sub ListaDomiciliosNacionales_Click(sender As Object, e As EventArgs)

        If GetVars("_listaDomiciliosNacionales") IsNot Nothing Then

            ListaDomiciliosNacionales.DataSource = GetVars("_listaDomiciliosNacionales")

        End If
    End Sub

    Protected Sub ListaDomiciliosNacionales_SelectedIndexChanged(sender As Object, e As EventArgs)

        If GetVars("_listaDomiciliosNacionales") IsNot Nothing Then

            ListaDomiciliosNacionales.DataSource = GetVars("_listaDomiciliosNacionales")

        End If

    End Sub
#End Region


End Class
