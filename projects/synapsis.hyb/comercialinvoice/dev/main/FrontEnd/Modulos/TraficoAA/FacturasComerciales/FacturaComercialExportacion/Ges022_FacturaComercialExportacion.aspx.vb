#Region "├┴┘├┴┘├┴┘├┴┘├┴┘|├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘   DEPENDENCIAS   ├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘"

'RECURSOS DEL CMF
Imports System.Globalization
'Imports System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip
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
'Imports Rec.Globals.Empresa
Imports Sax.Web
'UTILERIAS/RECURSOS ADICIONALES
Imports Sax.Web.ControladorBackend.Cookies
Imports Sax.Web.ControladorBackend.Datos
Imports Syn.CustomBrokers.Controllers
Imports Syn.Documento
Imports Syn.Documento.Componentes.Campo
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Nucleo.RecursosComercioExterior.CamposFacturaComercial
Imports Syn.Nucleo.Recursos.CamposClientes
Imports Syn.Nucleo.RecursosComercioExterior.CamposAcuseValor
Imports Syn.Operaciones
Imports Syn.Utils.Organismo
Imports Wma.Exceptions
Imports Wma.Exceptions.TagWatcher
Imports Wma.Exceptions.TagWatcher.TypeStatus
'Imports SharpCompress.Common
Imports MongoDB.Driver.Linq
Imports System.Runtime.ExceptionServices
Imports System.Security.Permissions
Imports Rec.Globals.Utils
Imports Syn.Nucleo.RecursosComercioExterior.SecuenciasComercioExterior
Imports Rec.Globals.Empresas
Imports Syn.Documento.Componentes
Imports Syn.Utils
Imports Ia.Analysis
Imports Gsol
Imports WebGrease.Css
Imports System.Threading.Tasks
Imports System.IO
Imports Syn.CustomBrokers.Controllers.Digitalization
Imports iText.StyledXmlParser.Jsoup.Select.Evaluator
Imports Microsoft.Ajax.Utilities
Imports Customer = Syn.CustomBrokers.Controllers.Digitalization.Customer
Imports Supplier = Syn.CustomBrokers.Controllers.Digitalization.Supplier
Imports Item = Syn.CustomBrokers.Controllers.Digitalization.Item
Imports ConsigneeDetails = Syn.CustomBrokers.Controllers.Digitalization.ConsigneeDetails
Imports iText.StyledXmlParser.Css.Selector


#End Region

Public Class Ges022_FacturaComercialExportacion
    Inherits ControladorBackend

#Region "████████████████████████████████████████   Atributos locales  ██████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    Private _utils As UtilsFacturaComercial

    Private _tagwatcher As TagWatcher

    Private _domicilioSeleccionado As Domicilio

    Private _proveedorSeleccionado As AuxiliarProveedor

    Private _lista As List(Of SelectOption)

    Private _listaCamposMonedas As List(Of Object)

    Private _monedaUSD As String

    Private _objectidmonedaUSD As String

    Private _dataSourceMoneda As List(Of SelectOption)

    Private _resultMoneda As List(Of MonedaGlobal)

    Private _resultadoMonedaPais As List(Of moneda)

    Private _secuencia As Secuencia

    Private _controladorSecuencias As IControladorSecuencia

    Private _loginUsuario As Dictionary(Of String, String)

    Private _datosCliente As ClienteFacturaComercial

    Private _constructorCliente As ConstructorCliente

    Private _productoAuxiliar As AuxiliarProducto

    Private _facturaComercialUI As Dictionary(Of String, Object)

    Private _esDestinatario As Boolean

    Private _datosDestinatario As AuxiliarDestinatario

    Private _domicilioDestinatario As Domicilio

    Private _controladorFacturaComercial As IControladorFacturaComercial

    Private _listaDomicilioDestinatario As List(Of SelectOption)

    Private _pillboxControl As PillboxControl

    Private _auxiliarDestinatario As AuxiliarDestinatario

    Private _controladorFactura As ControladorFacturaComercial


#End Region
#Region "████████████████████████████████████████   Constructores  ██████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████
    Sub New()

        'Dim officeOnline_ = Statements.GetOfficeOnline

        'If officeOnline_ Is Nothing Then

        '    Statements.SetEnvironmentOnline(1)

        'End If



    End Sub


#End Region
#Region "██████ Vinculación c/capas inf █████████       SAX      ████████████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

#Region "Inicializa"
    Public Overrides Sub Inicializa()

        With Buscador

            .DataObject = New ConstructorFacturaComercial(True)

            .addFilter(SeccionesFacturaComercial.SFAC1, CA_NUMERO_FACTURA, "Número de factura | Folio fiscal (UUID)")

            .addFilter(SeccionesFacturaComercial.SFAC1, CA_NUMERO_ACUSEVALOR, "Acuse de Valor")

            .addFilter(SeccionesFacturaComercial.SFAC2, CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, "Emisor")

            .MetadatosFilter = New Dictionary(Of [Enum], String) From {{CamposFacturaComercial.CP_TIPO_OPERACION, 2}}

        End With

        If Not Page.IsPostBack Then

            Session("_pbPartidasItems") = PillboxControl.ToolbarModality.Default

        End If

        '_monedaUSD = "USD"

        '_objectidmonedaUSD = "635acf25a8210bfa0d58434e"

        _listaCamposMonedas = New List(Of Object) _
                            From {scMonedaFactura,
                            scMonedaMercancia,
                            scMonedaMercanciaItemPartida,
                            scMonedaPrecioUnitarioPartida,
                            scMonedaValorAgregadoPartida}

        pbPartidasItems.Modality = Session("_pbPartidasItems")

        '  Generales
        fbcIncoterm.DataEntity = New krom.Anexo22()

        ' Datos del proveedor
        scMetodoValoracion.DataEntity = New krom.Anexo22()

        ' Partidas
        scMetodoValoracionPartida.DataEntity = New krom.Anexo22()

        icFechaAcuseValor.Enabled = False

        icFraccionArancelaria.Enabled = False

        icFraccionNico.Enabled = False

        icTipoCargaDatos.Value = Nothing

        lbModoCapturaIA.Visible = False

        lbModoCapturaIAEditar.Visible = False

        lbModoCapturaManual.Visible = False

        lbModoCapturaManualNuevo.Visible = True

        _tagwatcher = New TagWatcher

        _utils = New UtilsFacturaComercial

        _domicilioSeleccionado = New Domicilio

        _proveedorSeleccionado = New AuxiliarProveedor

        _lista = New List(Of SelectOption)

        _resultMoneda = New List(Of MonedaGlobal)

        _resultadoMonedaPais = New List(Of moneda)

        _controladorSecuencias = New ControladorSecuencia

        _secuencia = New Secuencia

        _loginUsuario = New Dictionary(Of String, String)

        _datosCliente = New ClienteFacturaComercial

        _constructorCliente = New ConstructorCliente

        _productoAuxiliar = New AuxiliarProducto

        _facturaComercialUI = New Dictionary(Of String, Object) From {
       {"invoicenumber", dbcNumFacturaAcuseValor},
       {"invoicedate", icFechaFactura},
       {"invoiceseries", icFolioFactura},
       {"customername", fbcCliente},
       {"incoterm", fbcIncoterm},
       {"value", icValorFactura},
       {"invoicecurrency", scMonedaFactura},
       {"totalinvoice", icValorMercancia},
       {"totalweight", icPesoTotal},
       {"packages", icBultos},
       {"purchaseorder", icOrdenCompra},
       {"suppliername", fbcCompradorReceptor},
       {"address", scDomicilioCompradorReceptor},
       {"partnumber", fbcProducto},
       {"origincountry", fbcPaisPartida},
       {"quantity", icCantidadComercial},
       {"unit", scUnidadMedidaComercial},
       {"unitprice", icPrecioUnitario},
       {"description", icDescripcionPartidaOriginal}}

        CycleLifeType = LifeCycleTypes.Automatic

    End Sub




#End Region
#Region "Botoneras"

    Public Overrides Sub BotoneraClicNuevo()
        'PanelProcesamiento.Enabled = True

        PanelProcesamiento.Visible = True

        lbModoCapturaManualNuevo.Visible = True

        lbModoCapturaManual.Visible = False

        scVinculacion.DataSource = _utils.Vinculacion()

        scVinculacion.Value = 0

        If OperacionGenerica IsNot Nothing Then

        End If

        '_monedaUSD = "USD"

        '_objectidmonedaUSD = "635acf25a8210bfa0d58434e"

        _listaCamposMonedas = New List(Of Object) _
                            From {scMonedaFactura,
                            scMonedaMercancia,
                            scMonedaValorAgregadoPartida,
                            scMonedaMercanciaItemPartida,
                            scMonedaPrecioUnitarioPartida}

        PreparaTarjetero(PillboxControl.ToolbarModality.Simple, pbPartidasItems)

        ' CargarMonedaPorDefault()

        MetodoValoracionInicial()

        icFraccionArancelaria.Enabled = False

        icFraccionNico.Enabled = False

        swcEnajenacion.Checked = True
        'swcEsDestinatario.Checked = True


    End Sub

    Public Overrides Sub BotoneraClicGuardar()

        Dim modoEditando_ As Boolean = False

        If GetVars("isEditing") IsNot Nothing Then

            If GetVars("isEditing") = True Then

                modoEditando_ = True

            End If

        End If

        If modoEditando_ = False Then

            ' If _utils.ExisteFacturaComercial(dbcNumFacturaAcuseValor.Value, fbcCompradorReceptor.Value, icFechaFactura.Value) Then

            ' DisplayMessage("Ya registraste esta factura", StatusMessage.Fail)

            ' Else

            If VerificarObjectIdValidoRegistros() Then

                If Not ProcesarTransaccion(Of ConstructorFacturaComercial)().Status = TypeStatus.Errors Then : End If

            Else

                CamposRequeridos()

                'estado_.SetError(Me, "Campos con * son requeridos")

                'SetError("Campos con * son requeridos", estado_)

                DisplayMessage("Campos con * son requeridos", StatusMessage.Fail)

            End If

        Else

            Dim tipoCaptura_ = 0

            If GetVars("_tipoCaptura") IsNot Nothing Then

                tipoCaptura_ = GetVars("_tipoCaptura")

            End If

            If tipoCaptura_ = 1 Then

                If Not ProcesarTransaccion(Of ConstructorFacturaComercial)().Status = TypeStatus.Errors Then : End If

            Else


                If VerificarObjectIdValidoRegistros() Then

                    If Not ProcesarTransaccion(Of ConstructorFacturaComercial)().Status = TypeStatus.Errors Then : End If

                Else

                    CamposRequeridos()

                    'estado_.SetError(Me, "Campos con * son requeridos")

                    'SetError("Campos con * son requeridos", estado_)

                    DisplayMessage("Campos con * son requeridos", StatusMessage.Fail)


                End If

                'CamposRequeridos()

                'DisplayMessage("Campos con * son requeridos", StatusMessage.Fail)

            End If

        End If

    End Sub


    'No es, al parecer este guardar es del boton de la ventana hije:V
    Public Overrides Function BotoneraClicGuardar_ProcesoInterno() As TagWatcher

        Dim estado_ As New TagWatcher(Ok)

        Dim modoEditando_ As Boolean = False

        If GetVars("isEditing") IsNot Nothing Then

            If GetVars("isEditing") = True Then

                modoEditando_ = True

            End If

        End If

        If modoEditando_ = False Then

            ' If _utils.ExisteFacturaComercial(dbcNumFacturaAcuseValor.Value, fbcCompradorReceptor.Value, icFechaFactura.Value) Then

            ' DisplayMessage("Ya registraste esta factura", StatusMessage.Fail)

            ' Else

            If VerificarObjectIdValidoRegistros() Then

                If Not ProcesarTransaccion(Of ConstructorFacturaComercial)().Status = TypeStatus.Errors Then : End If

            Else

                CamposRequeridos()

                estado_.SetError(Me, "Campos con * son requeridos")

                SetError("Campos con * son requeridos", estado_)

                DisplayMessage("Campos con * son requeridos", StatusMessage.Fail)

                Return estado_

            End If

        Else

            If VerificarObjectIdValidoRegistros() Then

                If Not ProcesarTransaccion(Of ConstructorFacturaComercial)().Status = TypeStatus.Errors Then : End If

            Else

                CamposRequeridos()

                estado_.SetError(Me, "Campos con * son requeridos")

                SetError("Campos con * son requeridos", estado_)

                DisplayMessage("Campos con * son requeridos", StatusMessage.Fail)

                Return estado_

            End If

        End If

        Return estado_

    End Function

    Public Overrides Function DespuesClickSalirDescartar() As TagWatcher

        Dim estado_ As New TagWatcher(Ok)

        'Dim modoEditando_ As Boolean = False

        'If GetVars("isEditing") IsNot Nothing Then

        '    If GetVars("isEditing") = True Then

        '        modoEditando_ = True

        '    End If

        'End If

        'If modoEditando_ = False Then


        MetodoValoracionInicial()
        scVinculacion.DataSource = _utils.Vinculacion()
        scVinculacion.Value = 0

        ' End If


        Return estado_

    End Function

    Public Overrides Sub BotoneraClicEditar()

        '_monedaUSD = "USD"

        '_objectidmonedaUSD = "635acf25a8210bfa0d58434e"

        '_listaCamposMonedas = New List(Of Object) _
        '                    From {scMonedaFactura,
        '                    scMonedaMercancia,
        '                    scMonedaValorAgregadoPartida,
        '                    scMonedaMercanciaItemPartida,
        '                    scMonedaPrecioUnitarioPartida}

        ' PreparaBotonera(FormControl.ButtonbarModality.Draft)

        If icTipoCargaDatos.Value = "1" Then

            lbModoCapturaIA.Visible = False

            lbModoCapturaManual.Visible = False

            lbModoCapturaManualNuevo.Visible = False

            lbModoCapturaIAEditar.Visible = True

            AvisosVerificacionObjectIdValido()

            'PENDIENTE
            ' _utils = New UtilsFacturaComercial

            ' Dim factura_ = _utils.ObtenerCommercialInvoiceAnalizer(dbcNumFacturaAcuseValor.Value)

            'OcultarTarjetaAlertaIA()

            ' _utils.EjecutarAnalisisIAAsync(factura_, _facturaComercialUI)

            '  EjecutarBusquedaDeCamposClave()

        Else

            lbModoCapturaIA.Visible = False

            lbModoCapturaIAEditar.Visible = False

            lbModoCapturaManual.Visible = False

            lbModoCapturaManualNuevo.Visible = True

        End If

        icFechaAcuseValor.Enabled = False

        PanelProcesamiento.Visible = False

        'PanelProcesamiento.Enabled = False

        PreparaTarjetero(PillboxControl.ToolbarModality.Advanced, pbPartidasItems)

    End Sub

    Public Overrides Function AgregarComponentesBloqueadosInicial() As List(Of WebControl)
        'Dim bloqueados_ As New List(Of WebControl) From {icFechaAcuseValor, scDomicilioCompradorReceptor, scDomicilioCompradorDestinatario}
        Dim bloqueados_ As New List(Of WebControl) From {icFechaAcuseValor, scDomicilioCompradorReceptor, scUnidadMedidaTarifa}
        Return bloqueados_
    End Function

    Public Overrides Function AgregarComponentesBloqueadosEdicion() As List(Of WebControl)
        'Dim bloqueadosEdicion_ As New List(Of WebControl) From {dbcNumFacturaAcuseValor, icFechaAcuseValor, scDomicilioCompradorReceptor, scDomicilioCompradorDestinatario}
        Dim bloqueadosEdicion_ As New List(Of WebControl) From {dbcNumFacturaAcuseValor, icFechaAcuseValor, scDomicilioCompradorReceptor, scUnidadMedidaTarifa}
        Return bloqueadosEdicion_
    End Function

    Public Overrides Sub BotoneraClicOtros(ByVal IndexSelected_ As Integer)

        Dim index_ = IndexSelected_

        If IndexSelected_ = 5 Then

            If icTipoCargaDatos.Value = "1" Then

                lbModoCapturaIAEditar.Visible = False

                lbModoCapturaManual.Visible = False

                lbModoCapturaManualNuevo.Visible = False

                lbModoCapturaIA.Visible = True

            Else

                lbModoCapturaManualNuevo.Visible = False

                lbModoCapturaIA.Visible = False

                lbModoCapturaIAEditar.Visible = False

                lbModoCapturaManual.Visible = True

            End If

        ElseIf IndexSelected_ = 10 Then

            If VerificarObjectIdValidoRegistros() Then

                ntPublicarFacturaComercial.Visible = True

            Else

                CamposRequeridos()

                DisplayMessage("Campos con * son requeridos", StatusMessage.Warning)

            End If

        ElseIf IndexSelected_ = 11 Then

            ''CHECAR EL COMPORTAMIENT DE ESTE BOTÓN

            If GetVars("isEditing") IsNot Nothing AndAlso GetVars("isEditing") = True Then

                DisplayMessage("No es posible realizar esta acción en edición de factura comercial", StatusMessage.Warning)

            Else

                'If GetVars("OperationMode") IsNot Nothing AndAlso GetVars("OperationMode") = "N" Then


                'SetVars("ActivaControles", False) : ActivaControles(GetVars("ActivaControles"))

                PanelProcesamiento.Visible = True

                fcCFDI.Value = Nothing
                extraerCFDI.Enabled = True
                CerrarIntegrador.Enabled = True

                SetVars("OperationMode", Nothing)

                SetVars("isEditing", False)

                SetVars("CfdiCargado", False)

                'End If

            End If

        End If

    End Sub

    Public Overrides Sub BotoneraClicPublicar()
        '''ANTES DE GENERAR MI PUBICACION, VOI A VALIDAR POR OTRO LADO CAMPOS IMPORTANTES PARA PODER PUBLICAR
        '''YA QUE ESTA FUNCION SI O SI FIRMA, SIN IMPORTARLE NADA DE LA VIDA :(
        '''TRISTE PERO SINCERO :V
        'DisplayMessage("🎉 Factura comercial exportación ha sido publicada", StatusMessage.Success)

        'If icTipoCargaDatos.Value = "1" Then

        '    lbModoCapturaIAEditar.Visible = False

        '    lbModoCapturaManual.Visible = False

        '    lbModoCapturaManualNuevo.Visible = False

        '    lbModoCapturaIA.Visible = True

        'Else

        '    lbModoCapturaManualNuevo.Visible = False

        '    lbModoCapturaIA.Visible = False

        '    lbModoCapturaIAEditar.Visible = False

        '    lbModoCapturaManual.Visible = True

        'End If

        'PreparaBotonera(FormControl.ButtonbarModality.Protected)

        'PreparaTarjetero(PillboxControl.ToolbarModality.Simple, pbPartidasItems)

    End Sub


#End Region
#Region "Configuracion inicial"
    'ASIGNACION PARA CONTROLES AUTOMÁTICOS
    Public Overrides Function Configuracion() As TagWatcher

        'If GetVars("_tipoCaptura") IsNot Nothing Then

        '    If GetVars("_tipoCaptura") = 2 Then

        '    End If


        'Encabezado
        [Set](icTipoCargaDatos, CP_TIPO_CARGA_DATOS, propiedadDelControl_:=PropiedadesControl.Valor)

        [Set](dbcNumFacturaAcuseValor, CA_NUMERO_FACTURA, propiedadDelControl_:=PropiedadesControl.Valor)

        [Set](dbcNumFacturaAcuseValor, CA_NUMERO_FACTURA, propiedadDelControl_:=PropiedadesControl.Valor, esRequerido_:=True)

        '[Set](dbcNumFacturaAcuseValor, CA_NUMERO_ACUSEVALOR, propiedadDelControl_:=PropiedadesControl.ValueDetail)



        [Set](icFechaFactura, CA_FECHA_FACTURA, esRequerido_:=True)
        ' [Set](icFechaFactura, CA_FECHA_FACTURA)

        [Set](icFechaAcuseValor, CA_FECHA_ACUSEVALOR) ''este es requerido si existe, pero el campo esta bloqieado, aun asi debo asegurarme que se ponga al publicar siempre y cuando exista

        '[Set](icidcove, CP_ID_ACUSEVALOR)

        [Set](icPesoTotal, CP_PESO_TOTAL)

        [Set](icBultos, CP_BULTOS)

        [Set](icOrdenCompra, CP_ORDEN_COMPRA)

        [Set](icReferenciaCliente, CP_REFERENCIA_CLIENTE)

        [Set](icFolioFactura, CamposFacturaComercial.CP_SERIE_FOLIO_FACTURA)

        [Set](swcEnajenacion, CP_APLICA_ENAJENACION, propiedadDelControl_:=PropiedadesControl.Checked)

        'Cliente
        ''HAY QUE PONERLO DINAMICO, HASTA PUBLICAR
        '[Set](fbcCliente, CamposClientes.CA_RAZON_SOCIAL, propiedadDelControl_:=PropiedadesControl.Valor, esRequerido_:=True)
        [Set](fbcCliente, CamposClientes.CA_RAZON_SOCIAL, propiedadDelControl_:=PropiedadesControl.Valor)
        '[Set](fbcCliente, CamposClientes.CA_RAZON_SOCIAL, propiedadDelControl_:=PropiedadesControl.Text, asignarA_:=TiposAsignacion.ValorPresentacion)

        [Set](fbcCliente, CamposClientes.CA_RAZON_SOCIAL, propiedadDelControl_:=PropiedadesControl.Text,
              asignarA_:=TiposAsignacion.ValorPresentacion)

        'Datos de factura
        ''PONERLO DINAMICO
        '[Set](fbcPais, CA_PAIS_FACTURACION, propiedadDelControl_:=PropiedadesControl.Valor, esRequerido_:=True)
        '[Set](fbcPais, CA_PAIS_FACTURACION, propiedadDelControl_:=PropiedadesControl.Valor)

        ''[Set](fbcPais, CA_PAIS_FACTURACION, propiedadDelControl_:=PropiedadesControl.Text, asignarA_:=TiposAsignacion.ValorPresentacion)

        '[Set](fbcPais, CA_PAIS_FACTURACION, propiedadDelControl_:=PropiedadesControl.Text, asignarA_:=TiposAsignacion.ValorPresentacion, esRequerido_:=True)

        ''DINAMICO HASTA PUBLICCAR
        '[Set](fbcIncoterm, CA_CVE_INCOTERM, propiedadDelControl_:=PropiedadesControl.Valor, esRequerido_:=True)
        [Set](fbcIncoterm, CA_CVE_INCOTERM, propiedadDelControl_:=PropiedadesControl.Valor)
        ' [Set](fbcIncoterm, CA_CVE_INCOTERM, propiedadDelControl_:=PropiedadesControl.Text, asignarA_:=TiposAsignacion.ValorPresentacion)

        [Set](fbcIncoterm, CA_CVE_INCOTERM, propiedadDelControl_:=PropiedadesControl.Text,
              asignarA_:=TiposAsignacion.ValorPresentacion)

        [Set](icValorFactura, CP_VALOR_FACTURA, esRequerido_:=True)

        [Set](scMonedaFactura, CA_MONEDA_FACTURACION, esRequerido_:=True)

        [Set](icValorMercancia, CP_VALOR_MERCANCIA, esRequerido_:=True)

        [Set](scMonedaMercancia, CP_MONEDA_VALOR_MERCANCIA, esRequerido_:=True)
        '[Set](icValorFactura, CP_VALOR_FACTURA)

        '[Set](scMonedaFactura, CA_MONEDA_FACTURACION)

        '[Set](icValorMercancia, CP_VALOR_MERCANCIA)

        '[Set](scMonedaMercancia, CP_MONEDA_VALOR_MERCANCIA)

        'Comprador - receptor
        ''DINAMICO AL PUBLICAR
        '[Set](fbcCompradorReceptor, CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Valor, esRequerido_:=True)
        [Set](fbcCompradorReceptor, CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Valor)

        [Set](fbcCompradorReceptor, CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Text,
              asignarA_:=TiposAsignacion.ValorPresentacion)

        '[Set](fbcCompradorReceptor, CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Text,
        '      asignarA_:=TiposAsignacion.ValorPresentacion)

        '[Set](scDomicilioCompradorReceptor, CamposProveedorOperativo.CP_ID_DOMICILIO_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Valor)

        '[Set](scDomicilioCompradorReceptor, CamposProveedorOperativo.CA_DOMICILIO_FISCAL,
        '      propiedadDelControl_:=PropiedadesControl.Valor, asignarA_:=TiposAsignacion.ValorPresentacion, esRequerido_:=True)
        ''DINAMICO
        '[Set](scVinculacion, CA_CVE_VINCULACION, propiedadDelControl_:=PropiedadesControl.Valor, esRequerido_:=True)
        [Set](scVinculacion, CA_CVE_VINCULACION, propiedadDelControl_:=PropiedadesControl.Valor)
        '[Set](scVinculacion, CA_CVE_VINCULACION, propiedadDelControl_:=PropiedadesControl.Valor)

        'DINAMICO
        '[Set](scMetodoValoracion, CP_CVE_METODO_VALORACION, propiedadDelControl_:=PropiedadesControl.Valor, esRequerido_:=True)
        [Set](scMetodoValoracion, CP_CVE_METODO_VALORACION, propiedadDelControl_:=PropiedadesControl.Valor)
        '[Set](scMetodoValoracion, CP_CVE_METODO_VALORACION, propiedadDelControl_:=PropiedadesControl.Valor)
        '[Set](swcEsDestinatario, CamposProveedorOperativo.CP_DESTINATARIO_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Checked)

        'Comprador - destinatario
        ''SON OBLIGATORIOS EN FUNCION DEL SWITCH??? Y SI LO LLENAN MANUAL, MAS BIEN EL DOMICILIO
        '[Set](fbcCompradorDestinatario, CamposDestinatario.CA_RAZON_SOCIAL, propiedadDelControl_:=PropiedadesControl.Valor, esRequerido_:=True)

        '[Set](fbcCompradorDestinatario, CamposDestinatario.CA_RAZON_SOCIAL, propiedadDelControl_:=PropiedadesControl.Text, asignarA_:=TiposAsignacion.ValorPresentacion, esRequerido_:=True)

        '[Set](fbcCompradorDestinatario, CamposDestinatario.CA_TAX_ID, propiedadDelControl_:=PropiedadesControl.Valor)

        '[Set](fbcCompradorDestinatario, CamposDestinatario.CA_RAZON_SOCIAL, propiedadDelControl_:=PropiedadesControl.Text, asignarA_:=TiposAsignacion.ValorPresentacion)

        '[Set](scDomicilioCompradorDestinatario, CamposDestinatario.CP_ID_DOMICILIO_DESTINATARIO, propiedadDelControl_:=PropiedadesControl.Valor)

        ' [Set](scDomicilioCompradorDestinatario, CamposDestinatario.CA_DOMICILIO_FISCAL_DESTINATARIO, propiedadDelControl_:=PropiedadesControl.Text, asignarA_:=TiposAsignacion.ValorPresentacion)

        If pbPartidasItems.PageIndex > 0 Then

            lbNumero.Text = pbPartidasItems.PageIndex.ToString()

        End If

        [Set](fbcProducto, CA_NUMERO_PARTE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icCantidadComercial, CA_CANTIDAD_COMERCIAL_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scUnidadMedidaComercial, CA_UNIDAD_MEDIDA_COMERCIAL_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icDescripcionPartidaOriginal, CA_DESCRIPCION_PARTE_PARTIDA_ORIGINAL, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icDescripcionPartida, CA_DESCRIPCION_PARTE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)


        [Set](icValorMercanciaItem, CA_VALOR_MERCANCIA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scMonedaMercanciaItemPartida, CA_MONEDA_MERCANCIA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)


        [Set](icPrecioUnitario, CA_PRECIO_UNITARIO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scMonedaPrecioUnitarioPartida, CP_MONEDA_PRECIO_UNITARIO, propiedadDelControl_:=PropiedadesControl.Ninguno)


        [Set](icValorAgregadoPartida, CP_VALOR_AGREGADO_ITEM, propiedadDelControl_:=PropiedadesControl.Ninguno) ''Aqui cambiar a valor agregado

        [Set](scMonedaValorAgregadoPartida, CP_MONEDA_VALOR_AGREGADO_ITEM, propiedadDelControl_:=PropiedadesControl.Ninguno) ''Aqui cambiar moneda agregado


        [Set](icPesoNeto, CA_PESO_NETO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icDescripcionCOVE, CA_DESCRIPCION_COVE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](fbcPaisPartida, CA_PAIS_DESTINO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scMetodoValoracionPartida, CA_CVE_METODO_VALORACION_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icOrdenCompraPartida, CP_ORDEN_COMPRA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        ' Partida - clasificación
        [Set](icObjectIdProducto, CamposFacturaComercial.CP_OBJECTID_PRODUCTOS, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icFraccionArancelaria, CA_FRACCION_ARANCELARIA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icFraccionNico, CA_FRACCION_NICO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icCantidadTarifa, CA_CANTIDAD_TARIFA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scUnidadMedidaTarifa, CA_UNIDAD_MEDIDA_TARIFA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        'Partida -detalle mercancía
        [Set](icLote, CA_LOTE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icNumeroSerie, CA_NUMERO_SERIE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icMarca, CA_MARCA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icModelo, CA_MODELO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icSubmodelo, CA_SUBMODELO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icKilometraje, CA_KILOMETRAJE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](coTimeStamp, CamposProducto.CP_TIMESTAMP, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](pbPartidasItems, Nothing, seccion_:=SeccionesFacturaComercial.SFAC4)

        If icTipoCargaDatos.Value = "1" Then

            lbModoCapturaIAEditar.Visible = False

            lbModoCapturaManual.Visible = False

            lbModoCapturaManualNuevo.Visible = False

            lbModoCapturaIA.Visible = True

            SetVars("_tipoCaptura", 1)

        ElseIf icTipoCargaDatos.Value = "2" Then

            lbModoCapturaManualNuevo.Visible = False

            lbModoCapturaIA.Visible = False

            lbModoCapturaIAEditar.Visible = False

            lbModoCapturaManual.Visible = True

            SetVars("_tipoCaptura", 2)

        Else

            lbModoCapturaManual.Visible = False

            lbModoCapturaIA.Visible = False

            lbModoCapturaIAEditar.Visible = False

            lbModoCapturaManualNuevo.Visible = True

            SetVars("_tipoCaptura", 2)

        End If


        Return New TagWatcher(1)

    End Function


#End Region
#Region "Eventos inserción y modificación de datos"
    'EVENTOS PARA LA INSERCIÓN DE DATOS
    Public Overrides Function AntesRealizarInsercion(ByVal session_ As IClientSessionHandle) As TagWatcher

        Dim tagwatcher_ As New TagWatcher(Ok)

        '      ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas con transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒
        If session_ IsNot Nothing Then

            '  ██████inicio███████        Logica de negocios local      ████████████████████████

            tagwatcher_.SetOK()

        Else  '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas sin transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 


            tagwatcher_.SetOK()

        End If

        Return tagwatcher_

    End Function

    Public Overrides Function RealizarInsercion_ProcesoInterno(ByRef documentoElectronico_ As DocumentoElectronico) As TagWatcher

        Dim tagwatcher_ As New TagWatcher(Ok)

        _tagwatcher = Nothing

        _tagwatcher = New TagWatcher

        _controladorSecuencias = Nothing

        _controladorSecuencias = New ControladorSecuencia()

        Dim environmentid_ As Int32 = ListaEmpresas.Value

        _tagwatcher = _controladorSecuencias.Generar(nombre_:=SecuenciasComercioExterior.FacturasComerciales.ToString,
                                                     tipoSecuencia_:=1,
                                                     compania_:=1,
                                                     area_:=1,
                                                     subtipoSecuencia_:=1,
                                                     enviroment_:=environmentid_)

        If _tagwatcher.Status = TypeStatus.Ok Then

            _secuencia = New Secuencia

            _secuencia = DirectCast(_tagwatcher.ObjectReturned, Secuencia)

        End If

        _loginUsuario = Session("DatosUsuario")

        _datosCliente = New ClienteFacturaComercial

        If GetVars("_datosCliente") IsNot Nothing Then

            _datosCliente = DirectCast(GetVars("_datosCliente"), ClienteFacturaComercial)

        End If

        With documentoElectronico_
            .Campo(CamposFacturaComercial.CP_TIPO_OPERACION).Valor = 2
            .Campo(CamposFacturaComercial.CP_TIPO_OPERACION).ValorPresentacion = "Exportacion"
            'If lbModoCapturaIA.Visible = True Then
            '    .Campo(CP_TIPO_CARGA_DATOS).Valor = 1
            '    .Campo(CP_TIPO_CARGA_DATOS).ValorPresentacion = "Carga CFDI"
            'Else
            '    .Campo(CP_TIPO_CARGA_DATOS).Valor = 2
            '    .Campo(CP_TIPO_CARGA_DATOS).ValorPresentacion = "Carga manual"
            'End If
            .UsuarioGenerador = _loginUsuario("Nombre")

            '.Id = _secuencia._id.ToString

            .IdDocumento = _secuencia.sec
            .FolioDocumento = dbcNumFacturaAcuseValor.Value.ToUpper()
            .FolioOperacion = _secuencia.sec
            .TipoPropietario = SecuenciasComercioExterior.FacturasComerciales.ToString

            .NombrePropietario = fbcCliente.Text.ToUpper()


            If _datosCliente.claveempresacliente <> "" Then

                .IdPropietario = _datosCliente.claveempresacliente 'se debe agregar desde el cliente

                .ObjectIdPropietario = New ObjectId(_datosCliente.idcliente)

            Else

                .IdPropietario = 0

                .ObjectIdPropietario = Nothing

            End If

        End With

        _proveedorSeleccionado = New AuxiliarProveedor

        _proveedorSeleccionado = DirectCast(GetVars("ProveedorSeleccionado_"), AuxiliarProveedor)

        OperacionNueva.DocumentosAsociados = New List(Of DocumentoAsociado)

        Dim documentosAsociadoCliente_ As New DocumentoAsociado With {
        .analisisconsistencia = 1,
        .identificadorrecurso = "ConstructorCliente",
        ._iddocumentoasociado = ObjectId.Parse(fbcCliente.Value),
        .firmaelectronica = _datosCliente.firmaElectronicaCliente}

        OperacionNueva.DocumentosAsociados.add(documentosAsociadoCliente_)

        Dim documentosAsociadoProveedor_ As New DocumentoAsociado With {
        .analisisconsistencia = 1,
        .identificadorrecurso = "ConstructorProveedoresOperativos",
        ._iddocumentoasociado = ObjectId.Parse(fbcCompradorReceptor.Value),
        .firmaelectronica = _proveedorSeleccionado._firmaElectrónica}

        OperacionNueva.DocumentosAsociados.add(documentosAsociadoProveedor_)

        Return tagwatcher_

    End Function

    Public Overrides Function DespuesRealizarInsercion() As TagWatcher

        lbModoCapturaManual.Visible = True

        lbModoCapturaManualNuevo.Visible = False

        lbModoCapturaIA.Visible = False

        lbModoCapturaIAEditar.Visible = False

        Return New TagWatcher(Ok)

    End Function

    'EVENTOS PARA MODIFICACIÓN DE DATOS
    Public Overrides Function AntesRealizarModificacion(ByVal session_ As IClientSessionHandle) As TagWatcher

        Dim tagwatcher_ As New TagWatcher(Ok)

        '     ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas con transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒

        If session_ IsNot Nothing Then

            '  ██████inicio███████        Logica de negocios local      ████████████████████████

            tagwatcher_.SetOK()


        Else  '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas sin transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 


            tagwatcher_.SetOK()

        End If

        Return tagwatcher_

    End Function


    Public Overrides Function DespuesRealizarModificacion() As TagWatcher
        ' Acciones después de realizar la modificación exitosamente
        Dim tagwatcher_ As New TagWatcher(Ok)

        Dim tipoCargaDatos_ = 2

        If icTipoCargaDatos.Value IsNot Nothing And icTipoCargaDatos.Value <> "" Then

            tipoCargaDatos_ = icTipoCargaDatos.Value

        Else

            tipoCargaDatos_ = GetVars("_tipoCaptura")

        End If

        If tipoCargaDatos_ = 1 Or tipoCargaDatos_ = "1" Then

            lbModoCapturaIAEditar.Visible = False

            lbModoCapturaManual.Visible = False

            lbModoCapturaManualNuevo.Visible = False

            lbModoCapturaIA.Visible = True

            'If GetVars("_datosCliente") IsNot Nothing Then

            '    Dim datosCliente_ = DirectCast(GetVars("_datosCliente"), ClienteFacturaComercial)

            '    Dim actualizarDocumentoElectronicoRaiz_ As New TagWatcher

            '    If GetVars("idDocumentoElectronico_") IsNot Nothing Then

            '        Dim environmentid_ As Int32 = ListaEmpresas.Value

            '        Dim controladorFacturaComercial_ As New ControladorFacturaComercial(IControladorFacturaComercial.TipoOperaciones.Exportacion, environmentid_)


            '        Dim idDocumentoElectronico As ObjectId = GetVars("idDocumentoElectronico_")

            '        Dim datosFuente_ As New DatosFuente With
            '                        {.IdPropietario = datosCliente_.clavecliente,
            '                        .NombrePropietario = fbcCliente.Text,
            '                        .ObjectdIdPropietario = ObjectId.Parse(datosCliente_.idcliente)}

            '        actualizarDocumentoElectronicoRaiz_ = controladorFacturaComercial_.
            '                ActualizarFuenteDocumentoElectronicoFacturaComercial(datosFuente_, idDocumentoElectronico)

            '    End If

            'End If

        Else

            lbModoCapturaManualNuevo.Visible = False

            lbModoCapturaIA.Visible = False

            lbModoCapturaIAEditar.Visible = False

            lbModoCapturaManual.Visible = True

        End If

        Return tagwatcher_

    End Function

    'EVENTOS PARA PRESENTACIÓN DE DATOS EN FRONTEND
    Public Overrides Function PreparaModificacion_ProcesoInterno(ByRef documentoElectronico_ As DocumentoElectronico) As TagWatcher

        Dim tagwatcher_ As New TagWatcher(Ok)

        With documentoElectronico_

        End With

        Return tagwatcher_

    End Function

    Public Overrides Function DespuesOperadorDatosProcesar_ProcesoInterno(ByRef documentoElectronico_ As DocumentoElectronico) As TagWatcher

        Dim tagwatcher_ As New TagWatcher(Ok)

        Dim modoEditando_ As Boolean = False

        If GetVars("isEditing") IsNot Nothing Then

            If GetVars("isEditing") = True Then

                modoEditando_ = True

            End If

        End If

        With documentoElectronico_

            If OperacionGenerica IsNot Nothing Then

                SetVars("idDocumentoElectronico_", OperacionGenerica.Id)

            End If

            _datosCliente = Nothing

            _proveedorSeleccionado = Nothing

            _domicilioSeleccionado = Nothing

            If GetVars("_datosCliente") IsNot Nothing Then

                _datosCliente = New ClienteFacturaComercial

                _datosCliente = DirectCast(GetVars("_datosCliente"), ClienteFacturaComercial)

            End If

            If _datosCliente IsNot Nothing Then

                If modoEditando_ Then

                    Dim documentosAsociadoCliente_ As New DocumentoAsociado With {
                        .analisisconsistencia = 1,
                        .identificadorrecurso = "ConstructorCliente",
                        ._iddocumentoasociado = ObjectId.Parse(fbcCliente.Value),
                        .firmaelectronica = _datosCliente.firmaElectronicaCliente}

                    Dim environmentid_ As Int32 = ListaEmpresas.Value

                    _controladorFactura = New ControladorFacturaComercial(IControladorFacturaComercial.TipoOperaciones.Exportacion, environmentid_)

                    Dim documentoManualAsociadoCliente_ As TagWatcher = _controladorFactura.ActualizarDocumentosAsociadosFacturaComercial(documentosAsociadoCliente_, OperacionGenerica.Id)

                    Dim datosFuente_ As New DatosFuente With
                                            {.IdPropietario = _datosCliente.clavecliente,
                                            .NombrePropietario = fbcCliente.Text,
                                            .ObjectdIdPropietario = ObjectId.Parse(_datosCliente.idcliente)}

                    Dim actualizarDocumentoElectronicoRaiz_ As TagWatcher = _controladorFactura.
                                    ActualizarFuenteDocumentoElectronicoFacturaComercial(datosFuente_, OperacionGenerica.Id)

                End If

                With .Seccion(SeccionesFacturaComercial.SFAC1)

                    .Campo(CamposClientes.CP_OBJECTID_CLIENTE).Valor = ObjectId.Parse(_datosCliente.idcliente)

                    .Campo(CamposClientes.CP_CVE_CLIENTE).Valor = _datosCliente.claveempresacliente

                    .Campo(CamposClientes.CA_RFC_CLIENTE).Valor = ToUpperSafe(_datosCliente.rfc)

                    .Campo(CamposClientes.CA_CURP_CLIENTE).Valor = ToUpperSafe(_datosCliente.curp)

                    .Campo(CamposClientes.CA_TAX_ID).Valor = ToUpperSafe(_datosCliente.rfc)

                    .Campo(CamposDomicilio.CA_PAIS).Valor = ToUpperSafe(_datosCliente.pais)

                    .Campo(CamposDomicilio.CA_CVE_PAIS).Valor = ToUpperSafe(_datosCliente.clavepais)

                    .Campo(CamposDomicilio.CP_ID_DOMICILIO).Valor = _datosCliente._iddomicilio

                    .Campo(CamposDomicilio.CP_SEC_DOMICILIO).Valor = _datosCliente.sec

                    .Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor = ToUpperSafe(_datosCliente.domicilioPresentacion)

                    .Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).ValorPresentacion = Nothing

                    .Campo(CamposDomicilio.CA_CALLE).Valor = ToUpperSafe(_datosCliente.calle)

                    .Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor = ToUpperSafe(_datosCliente.numeroexterior)

                    .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor = ToUpperSafe(_datosCliente.numerointerior)

                    If _datosCliente.numeroexterior <> "" AndAlso _datosCliente.numerointerior <> "" Then

                        .Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor = $"{ToUpperSafe(_datosCliente.numeroexterior)} - {ToUpperSafe(_datosCliente.numerointerior)}"

                    End If

                    .Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor = ToUpperSafe(_datosCliente.codigopostal)

                    .Campo(CamposDomicilio.CA_COLONIA).Valor = ToUpperSafe(_datosCliente.colonia)

                    .Campo(CamposDomicilio.CA_LOCALIDAD).Valor = ToUpperSafe(_datosCliente.localidad)

                    .Campo(CamposDomicilio.CA_CIUDAD).Valor = ToUpperSafe(_datosCliente.ciudad)

                    .Campo(CamposDomicilio.CA_MUNICIPIO).Valor = ToUpperSafe(_datosCliente.municipio)

                    .Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor = ToUpperSafe(_datosCliente.cveEntidadfederativa)

                    .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor = ToUpperSafe(_datosCliente.entidadfederativa)

                    .Campo(CamposDomicilio.CA_ENTIDAD_MUNICIPIO).Valor = ToUpperSafe(_datosCliente.municipio)

                End With

            End If

            With .Seccion(SeccionesFacturaComercial.SFAC1)

                Dim aplicaEnajecacion_ As Boolean = False

                If swcEnajenacion.Checked Then

                    aplicaEnajecacion_ = True

                End If

                .Campo(CamposFacturaComercial.CP_APLICA_ENAJENACION).Valor = aplicaEnajecacion_

                .Campo(CamposFacturaComercial.CA_FECHA_FACTURA).ValorPresentacion = icFechaFactura.Value

                .Campo(CamposFacturaComercial.CP_ORDEN_COMPRA).Valor = ToUpperSafe(icOrdenCompra.Value)

                .Campo(CamposFacturaComercial.CP_REFERENCIA_CLIENTE).Valor = ToUpperSafe(icReferenciaCliente.Value)

                .Campo(CamposFacturaComercial.CP_SERIE_FOLIO_FACTURA).Valor = ToUpperSafe(icFolioFactura.Value)


                Dim tipoCargaDatos_ As Int32 = IIf(GetVars("_tipoCaptura") IsNot Nothing, GetVars("_tipoCaptura"), 2)

                Dim tipoCargaDatosPresentacion_ = IIf(tipoCargaDatos_ = 2, "Carga manual", "Carga CFDI")

                .Campo(CamposFacturaComercial.CP_TIPO_CARGA_DATOS).Valor = tipoCargaDatos_

                .Campo(CamposFacturaComercial.CP_TIPO_CARGA_DATOS).ValorPresentacion = tipoCargaDatosPresentacion_


                If modoEditando_ = False Then

                    .Campo(CamposFacturaComercial.CA_APLICA_SUBDIVISION).Valor = False

                    .Campo(CamposFacturaComercial.CP_MARCADO_PEDIMENTO).Valor = False

                    .Campo(CamposFacturaComercial.CP_ID_PEDIMENTO_ASOCIADO).Valor = ObjectId.Parse("000000000000000000000000")

                    .Campo(CamposFacturaComercial.CP_APLICA_INCREMENTABLES).Valor = False

                    .Campo(CamposFacturaComercial.CP_ID_FACTURA_ORIGINAL).Valor = ObjectId.Parse("000000000000000000000000")

                    .Campo(CamposFacturaComercial.CP_NUMERO_FACTURA_SUBDIVISION).Valor = Nothing

                End If

            End With


            If GetVars("ProveedorSeleccionado_") IsNot Nothing Then

                _proveedorSeleccionado = New AuxiliarProveedor

                _proveedorSeleccionado = DirectCast(GetVars("ProveedorSeleccionado_"), AuxiliarProveedor)

            End If

            If GetVars("DomicilioProveedorSeleccionado_") IsNot Nothing Then

                _domicilioSeleccionado = New Domicilio

                _domicilioSeleccionado = DirectCast(GetVars("DomicilioProveedorSeleccionado_"), Domicilio)

            End If

            If _proveedorSeleccionado IsNot Nothing Then

                If modoEditando_ Then

                    Dim documentosAsociadoProveedor_ As New DocumentoAsociado With {
                    .analisisconsistencia = 1,
                    .identificadorrecurso = "ConstructorProveedoresOperativos",
                    ._iddocumentoasociado = ObjectId.Parse(fbcCompradorReceptor.Value),
                    .firmaelectronica = _proveedorSeleccionado._firmaElectrónica}


                    Dim environmentid_ As Int32 = ListaEmpresas.Value

                    _controladorFactura = New ControladorFacturaComercial(IControladorFacturaComercial.TipoOperaciones.Exportacion, environmentid_)

                    Dim documentoManualAsociadoProveedor_ As TagWatcher = _controladorFactura.ActualizarDocumentosAsociadosFacturaComercial(documentosAsociadoProveedor_, OperacionGenerica.Id)

                End If

                With .Seccion(SeccionesFacturaComercial.SFAC2)

                    ''de mientras en lo que se resuelve
                    If _proveedorSeleccionado._cvepais IsNot Nothing AndAlso _proveedorSeleccionado._cvepais <> "" Then

                        Dim paisProveedor_ = ControladorPaises.ConsultarListaPaisesPorClaveISO(_proveedorSeleccionado._cvepais)

                        If paisProveedor_.Status = TypeStatus.Ok Then

                            ' Dim aquih_ = paisProveedor_.ObjectReturned

                            .Campo(CamposDomicilio.CA_PAIS).Valor = paisProveedor_.ObjectReturned(0)._id.ToString

                        End If

                    Else

                        .Campo(CamposDomicilio.CA_PAIS).Valor = _proveedorSeleccionado._pais

                    End If

                    .Campo(CamposDomicilio.CA_PAIS).ValorPresentacion = _proveedorSeleccionado._pais

                    .Campo(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR).Valor = fbcCompradorReceptor.Value

                    .Campo(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR).ValorPresentacion = ToUpperSafe(fbcCompradorReceptor.Text)


                    .Campo(CamposProveedorOperativo.CP_ID_PROVEEDOR).Valor = _proveedorSeleccionado.id

                    .Campo(CamposProveedorOperativo.CP_CVE_PROVEEDOR).Valor = _proveedorSeleccionado._clave

                    .Campo(CamposDomicilio.CA_CVE_PAIS).Valor = _proveedorSeleccionado._cvepais

                    If _proveedorSeleccionado._esextranjero Then

                        .Campo(CamposProveedorOperativo.CA_RFC_PROVEEDOR).Valor = _proveedorSeleccionado._taxid

                        .Campo(CamposProveedorOperativo.CA_CURP_PROVEEDOR).Valor = Nothing

                        .Campo(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR).Valor = _proveedorSeleccionado.idtaxid

                        .Campo(CamposProveedorOperativo.CA_CVE_TAX_ID_PROVEEDOR).Valor = ToUpperSafe(_proveedorSeleccionado._taxid)

                    Else
                        If _proveedorSeleccionado._rfc Is Nothing Then

                            .Campo(CamposProveedorOperativo.CA_RFC_PROVEEDOR).Valor = ToUpperSafe(_proveedorSeleccionado._taxid)

                        Else
                            If _proveedorSeleccionado._rfc <> "" Then

                                .Campo(CamposProveedorOperativo.CA_RFC_PROVEEDOR).Valor = ToUpperSafe(_proveedorSeleccionado._rfc)

                            Else

                                .Campo(CamposProveedorOperativo.CA_RFC_PROVEEDOR).Valor = ToUpperSafe(_proveedorSeleccionado._taxid)

                            End If

                        End If

                        .Campo(CamposProveedorOperativo.CA_CURP_PROVEEDOR).Valor = _proveedorSeleccionado._curp

                        .Campo(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR).Valor = _proveedorSeleccionado.idtaxid

                        .Campo(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR).ValorPresentacion = ToUpperSafe(_proveedorSeleccionado._taxid)
                    End If

                    If _domicilioSeleccionado IsNot Nothing Then

                        .Campo(CamposProveedorOperativo.CP_ID_DOMICILIO_PROVEEDOR).Valor = _domicilioSeleccionado._iddomicilio.ToString
                        .Campo(CamposProveedorOperativo.CP_SEC_DOMICILIO_PROVEEDOR).Valor = _domicilioSeleccionado.sec
                        .Campo(CamposProveedorOperativo.CA_DOMICILIO_FISCAL).Valor = _domicilioSeleccionado.domicilioPresentacion
                        .Campo(CamposDomicilio.CA_CALLE).Valor = ToUpperSafe(_domicilioSeleccionado.calle)
                        .Campo(CamposDomicilio.CA_CIUDAD).Valor = ToUpperSafe(_domicilioSeleccionado.ciudad)
                        .Campo(CamposDomicilio.CA_COLONIA).Valor = ToUpperSafe(_domicilioSeleccionado.colonia)
                        .Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor = _domicilioSeleccionado.codigopostal
                        .Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor = ToUpperSafe(_domicilioSeleccionado.numeroexterior)
                        .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor = ToUpperSafe(_domicilioSeleccionado.numerointerior)
                        .Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor = $"{ToUpperSafe(_domicilioSeleccionado.numeroexterior)} - {ToUpperSafe(_domicilioSeleccionado.numerointerior)}"
                        .Campo(CamposDomicilio.CA_LOCALIDAD).Valor = ToUpperSafe(_domicilioSeleccionado.localidad)
                        .Campo(CamposDomicilio.CA_MUNICIPIO).Valor = ToUpperSafe(_domicilioSeleccionado.municipio)
                        .Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor = ToUpperSafe(_domicilioSeleccionado.cveEntidadfederativa)
                        .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor = ToUpperSafe(_domicilioSeleccionado.entidadfederativa)
                        .Campo(CamposDomicilio.CA_ENTIDAD_MUNICIPIO).Valor = ToUpperSafe(_domicilioSeleccionado.municipio)
                    End If

                End With

            End If

            Dim NodosItems_ = .Seccion(SeccionesFacturaComercial.SFAC4)

            Dim x_ As Integer = 0

            For Each nodo_ In NodosItems_.Nodos

                ''NO GUARDA EL OBJECTID del producto porque pillbox no reconoce los inputs ocultos

                If Not ObjectId.Parse(pbPartidasItems.DataSource(x_).Item("fbcProducto").Item("Value")) = ObjectId.Empty Then

                    If pbPartidasItems.DataSource(x_).Item("fbcProducto").Count > 0 Then

                        nodo_.Campo(CamposFacturaComercial.CP_OBJECTID_PRODUCTOS).Valor = ObjectId.Parse(pbPartidasItems.DataSource(x_).Item("fbcProducto").Item("Value"))

                    End If

                    nodo_.Campo(CamposFacturaComercial.CP_NUMERO_PARTIDA).Valor = CInt(pbPartidasItems.DataSource(x_).Item("identidad"))

                    For Each i_ In pbPartidasItems.DataSource(x_)

                        If i_.Key = "icOrdenCompraPartida" Then

                            nodo_.Campo(CamposFacturaComercial.CP_ORDEN_COMPRA_PARTIDA).Valor = ToUpperSafe(i_.Value)

                        End If

                        If i_.Key = "icDescripcionPartida" Then

                            nodo_.Campo(CamposFacturaComercial.CA_DESCRIPCION_PARTE_PARTIDA).Valor = ToUpperSafe(i_.Value)

                        End If

                        If i_.Key = "icDescripcionCOVE" Then

                            nodo_.Campo(CamposFacturaComercial.CA_DESCRIPCION_COVE_PARTIDA).Valor = ToUpperSafe(i_.Value)

                        End If

                        If i_.Key = "icLote" Then

                            nodo_.Campo(CamposFacturaComercial.CA_LOTE_PARTIDA).Valor = ToUpperSafe(i_.Value)

                        End If

                        If i_.Key = "icNumeroSerie" Then

                            nodo_.Campo(CamposFacturaComercial.CA_NUMERO_SERIE_PARTIDA).Valor = ToUpperSafe(i_.Value)

                        End If

                        If i_.Key = "icModelo" Then

                            nodo_.Campo(CamposFacturaComercial.CA_MODELO_PARTIDA).Valor = ToUpperSafe(i_.Value)

                        End If

                        If i_.Key = "icMarca" Then

                            nodo_.Campo(CamposFacturaComercial.CA_MARCA_PARTIDA).Valor = ToUpperSafe(i_.Value)

                        End If


                        If i_.Key = "icSubmodelo" Then

                            nodo_.Campo(CamposFacturaComercial.CA_SUBMODELO_PARTIDA).Valor = ToUpperSafe(i_.Value)

                        End If

                        If i_.Key = "coTimeStamp" Then

                            System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice:TIMESTAMP DESPUES DATOS PROCESAR {i_.Value}")

                            nodo_.Campo(CamposProducto.CP_TIMESTAMP).Valor = i_.Value

                        End If

                    Next

                    x_ += 1

                End If

            Next

        End With

        Return tagwatcher_

    End Function

    ''PASARLO A UTILS
    Private Function ToUpperSafe(valor As String) As String
        If String.IsNullOrWhiteSpace(valor) Then Return ""
        Return valor.ToUpper()
    End Function

    Public Overrides Function DespuesBuquedaGeneralConDatos_ProcesoInterno() As TagWatcher

        Dim tagwatcher_ As New TagWatcher(Ok)

        If OperacionGenerica.Publicado = True And OperacionGenerica.FirmaElectronica IsNot Nothing Then

            PreparaBotonera(FormControl.ButtonbarModality.Protected)

        End If

        Dim constructorFacturaComercial_ = DirectCast(OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente, ConstructorFacturaComercial)

        Dim tipoCaptura_ = constructorFacturaComercial_.Seccion(SeccionesFacturaComercial.SFAC1).Campo(CamposFacturaComercial.CP_TIPO_CARGA_DATOS).Valor

        SetVars("_tipoCaptura", tipoCaptura_)

        'Dim elproveedoresdestinatario_ = constructorFacturaComercial_.Seccion(SeccionesFacturaComercial.SFAC2).Campo(CamposProveedorOperativo.CP_DESTINATARIO_PROVEEDOR).Valor

        Dim datasourceproveedor_ = New SelectOption With {.Value = constructorFacturaComercial_.Seccion(SeccionesFacturaComercial.SFAC2).Campo(CamposProveedorOperativo.CP_ID_DOMICILIO_PROVEEDOR).Valor,
          .Text = constructorFacturaComercial_.Seccion(SeccionesFacturaComercial.SFAC2).Campo(CamposProveedorOperativo.CA_DOMICILIO_FISCAL).Valor}

        scDomicilioCompradorReceptor.DataSource = New List(Of SelectOption) From {datasourceproveedor_}

        scDomicilioCompradorReceptor.Value = constructorFacturaComercial_.Seccion(SeccionesFacturaComercial.SFAC2).Campo(CamposProveedorOperativo.CP_ID_DOMICILIO_PROVEEDOR).Valor

        SetVars("idDocumentoElectronico_", OperacionGenerica.Id)

        If tipoCaptura_ = 1 Or tipoCaptura_ = "1" Then

            AvisosVerificacionObjectIdValido()

            lbModoCapturaIAEditar.Visible = False

            lbModoCapturaManual.Visible = False

            lbModoCapturaManualNuevo.Visible = False

            lbModoCapturaIA.Visible = True

        Else

            lbModoCapturaManual.Visible = True

            lbModoCapturaManualNuevo.Visible = False

            lbModoCapturaIA.Visible = False

            lbModoCapturaIAEditar.Visible = False

        End If

        icFechaAcuseValor.Enabled = False

        PanelProcesamiento.Visible = False

        PreparaTarjetero(PillboxControl.ToolbarModality.Default, pbPartidasItems)

        Return tagwatcher_

    End Function

    Public Overrides Function DespuesBuquedaGeneralSinDatos_ProcesoInterno() As TagWatcher

        Dim tagwatcher_ As New TagWatcher(Ok)

        lbModoCapturaManual.Visible = True

        lbModoCapturaManualNuevo.Visible = False

        lbModoCapturaIA.Visible = False

        lbModoCapturaIAEditar.Visible = False

        icFechaAcuseValor.Enabled = False

        PanelProcesamiento.Visible = False

        PreparaTarjetero(PillboxControl.ToolbarModality.Advanced, pbPartidasItems)

        Return tagwatcher_

    End Function

#End Region

#Region "Eventos de mantenimiento"
    'EVENTOS DE MANTENIMIENTO
    Public Overrides Sub LimpiaSesion()

        SetVars("isEditing", Nothing)

        SetVars("_datosCliente", Nothing)

        SetVars("_domicilioCliente", Nothing)

        SetVars("_listaDomiciliosProveedores", Nothing)

        SetVars("_datosReceptorProveedor", Nothing)

        SetVars("_listaDomiciliosDestinatario", Nothing)

        SetVars("_datosDestinatario", Nothing)

        SetVars("_listaConstructorProductos", Nothing)

        SetVars("ProveedorSeleccionado_", Nothing)

        SetVars("DomicilioProveedorSeleccionado_", Nothing)

        SetVars("DomicilioDestinatarioSeleccionado_", Nothing)

        SetVars("DestinatarioSeleccionado_", Nothing)

        SetVars("ActivaControles", Nothing)

        SetVars("OperationMode", Nothing)

        SetVars("isEditing", Nothing)

        SetVars("CfdiCargado", Nothing)

        HttpRuntime.Cache.Remove("cacheListaMonedas")

        HttpRuntime.Cache.Remove("cacheListaUnidadesMedida")

        Statements.ObjectSession = Nothing

        SetVars("ListaProductos", Nothing)

    End Sub

    Public Overrides Sub Limpiar()

        _utils.Dispose()

        _tagwatcher = Nothing

        _domicilioSeleccionado = Nothing

        _proveedorSeleccionado = Nothing

        _lista = Nothing

        _listaCamposMonedas = Nothing

        _monedaUSD = Nothing

        _objectidmonedaUSD = Nothing

        _dataSourceMoneda = Nothing

        _resultMoneda = Nothing

        _resultadoMonedaPais = Nothing

        _secuencia = Nothing

        _controladorSecuencias = Nothing

        _loginUsuario = Nothing

        _datosCliente = Nothing

        _constructorCliente = Nothing

        _utils.LimpiarMonedas(_listaCamposMonedas)

        scDomicilioCompradorReceptor.Value = Nothing

        scDomicilioCompradorReceptor.DataSource = Nothing

        _controladorFactura = Nothing

        'scDomicilioCompradorDestinatario.Value = Nothing

        'scDomicilioCompradorDestinatario.DataSource = Nothing

        lbModoCapturaIA.Visible = False
        lbModoCapturaIAEditar.Visible = False
        lbModoCapturaManual.Visible = False
        lbModoCapturaManualNuevo.Visible = True

        PreparaTarjetero(PillboxControl.ToolbarModality.Default, pbPartidasItems)

    End Sub

    Sub MostrarBotones(ByVal sender_ As ButtonbarControl, ByVal e As EventArgs)

        If OperacionGenerica IsNot Nothing Then

            If Not OperacionGenerica.Publicado Then

                '  btiPublicar.Visible = True

            End If

        End If

    End Sub

#End Region

#End Region


#Region "████████████████  QUINTA CAPA  █████████       Reglas locales         ██████████████████████████████"
    '       * Aquí se pueden colocar los eventos de los componentes, funciones o metodos exclusios del modulo
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

#Region "Cliente"
    Private Sub BuscarCliente()

        _lista = _utils.ObtenerListaClientePorControlador(Trim(fbcCliente.Text))

        If _lista IsNot Nothing Then

            If _lista.Count > 0 Then

                fbcCliente.DataSource = _lista

            Else

                DisplayMessage("Cliente no encontrado", StatusMessage.Fail)

            End If

        End If

    End Sub

    Protected Sub fbcCliente_TextChanged(sender As Object, e As EventArgs)

        If fbcCliente.Value = "" Then

            BuscarCliente()

        End If

    End Sub

    Protected Sub fbcCliente_ClickSearch(sender As Object, e As EventArgs)

        If fbcCliente.Value = "" OrElse fbcCliente.Value = "000000000000000000000000" Then

            BuscarCliente()

        End If

    End Sub

    Protected Sub fbcCliente_Click(sender As Object, e As EventArgs)

        If fbcCliente.Value <> "" Then

            CargarDatosClienteSesion()

            'DisplayMessage("Cliente establecido correctamente", StatusMessage.Info)

        End If

    End Sub

    Protected Sub CargarDatosClienteSesion()

        Dim datoscliente_ = _utils.DatosCliente(fbcCliente.Value)

        datoscliente_.idcliente = fbcCliente.Value

        SetVars("_datosCliente", datoscliente_)

    End Sub
#End Region


#Region "Pais"

    'Private Sub CargarPais()

    '    fbcPais.DataSource = _utils.ObtenerListaPaises(fbcPais.Text)

    '    CargarMonedaPorDefault(objectIdPais_:=fbcPais.Value)

    'End Sub

    Private Sub CargarPaisPartida()

        fbcPaisPartida.DataSource = _utils.ObtenerListaPaises(fbcPaisPartida.Text)

    End Sub

#End Region
    'Protected Sub fbcPais_TextChanged(sender As Object, e As EventArgs)

    '    CargarPais()

    'End Sub

    Protected Sub fbcPaisPartida_ClickSearch(sender As Object, e As EventArgs)

        CargarPaisPartida()

    End Sub

    'Protected Sub fbcPais_ClickSearch(sender As Object, e As EventArgs)

    '    CargarPais()

    'End Sub

    Protected Sub fbcPaisPartida_TextChanged(sender As Object, e As EventArgs)

        'Checar si cambia el país aqui del país principal

        CargarPaisPartida()

    End Sub

#End Region


    Private Sub BuscarProveedor()

        'Dim cfdiCargado_ = False

        'If GetVars("CfdiCargado") IsNot Nothing Then

        '    cfdiCargado_ = GetVars("CfdiCargado")

        'End If

        'Dim tagwatcher_ As New TagWatcher

        Dim listaProveedoresOperativos_ As List(Of AuxiliarProveedor)

        Dim listaDataSource_ As New List(Of SelectOption)

        'If fbcCompradorReceptor.Value = "" OrElse cfdiCargado_ = True Then
        'If fbcCompradorReceptor.Value = "" Then

        Dim tagwatcher_ As TagWatcher = _utils.ObtenerListaProveedoresOperativosPorControlador(fbcCompradorReceptor.Text)

        If tagwatcher_.Status = TypeStatus.Ok Then

            listaProveedoresOperativos_ = DirectCast(tagwatcher_.ObjectReturned, List(Of AuxiliarProveedor))

            If listaProveedoresOperativos_.Count > 0 Then

                Dim proveedoresActivos_ = New List(Of AuxiliarProveedor)

                For Each item_ In listaProveedoresOperativos_

                    If item_._activo Then

                        listaDataSource_.Add(New SelectOption With
                              {.Value = item_.idtaxid,
                               .Text = $"{item_._razonsocial.ToUpper()} | {item_._taxid.ToUpper()}"})

                        proveedoresActivos_.Add(item_)
                    End If

                Next


                If proveedoresActivos_.Count > 0 Then

                    SetVars("listaProveedoresOperativos_", listaProveedoresOperativos_)

                Else

                    DisplayMessage("Comprador no encontrado", StatusMessage.Fail)

                End If

            End If

        Else

            DisplayMessage("Comprador no encontrado", StatusMessage.Fail)

        End If

        fbcCompradorReceptor.DataSource = listaDataSource_


        'End If

    End Sub


#Region "Comprador"
    Protected Sub fbcCompradorReceptor_TextChanged(sender As Object, e As EventArgs)


        'MANA
        If fbcCompradorReceptor.Value = "" Then

            BuscarProveedor()

        End If


    End Sub

    Protected Sub fbcCompradorReceptor_Click(sender As Object, e As EventArgs)

        Dim modoEditando_ As Boolean = False

        If GetVars("isEditing") IsNot Nothing Then

            If GetVars("isEditing") = True Then

                modoEditando_ = True

            End If

        End If

        If modoEditando_ = False Then

            Dim environmentid_ As Int32 = ListaEmpresas.Value

            If _utils.ExisteFacturaComercial(dbcNumFacturaAcuseValor.Value, fbcCompradorReceptor.Value, icFechaFactura.Value, environmentid_) Then

                DisplayMessage("Esta factura ya ha sido registrada", StatusMessage.Fail)

            End If

        End If

        ''Buscar los datos del proveedor en el controlador proveedores

        If fbcCompradorReceptor.Value <> "" Then

            Dim proveedores_ As List(Of AuxiliarProveedor) = GetVars("listaProveedoresOperativos_")

            Dim proveedorseleccionado_ = proveedores_.Where(Function(x) x.idtaxid.Equals(fbcCompradorReceptor.Value)).First

            ''AGREGAR EL PROVEEDOR SELECCIONADO A LA SESSION
            SetVars("ProveedorSeleccionado_", proveedorseleccionado_)

            ' If proveedorseleccionado_._activo Then ProveedorOnline() Else ProveedorOffline()

            Dim datasource_ As New List(Of SelectOption)

            datasource_.Add(New SelectOption With
                         {.Value = proveedorseleccionado_._domicilio._iddomicilio.ToString,
                          .Text = proveedorseleccionado_._domicilio.domicilioPresentacion.ToUpper})

            scDomicilioCompradorReceptor.DataSource = datasource_

            'If scDomicilioCompradorReceptor.DataSource.Count = 1 Then

            scDomicilioCompradorReceptor.Value = datasource_.Last.Value

            _domicilioSeleccionado = proveedorseleccionado_._domicilio

            ''AGREGAR EL PROVEEDOR SELECCIONADO A LA SESSION
            SetVars("DomicilioProveedorSeleccionado_", _domicilioSeleccionado)

            If fbcCliente.Value <> "" Then

                ''OBTENER VINCULACIÓN ENTRE CLIENTE Y PROVEEDOR
                Dim vinculacionesDisponibles_ = proveedorseleccionado_._listavinculaciones

                If vinculacionesDisponibles_ IsNot Nothing Then

                    If vinculacionesDisponibles_.Count > 0 Then
                        Dim vinculacionencontrada_ = vinculacionesDisponibles_.
                        Where(Function(x) x.idProveedor.Equals(fbcCompradorReceptor.Value) And x.idClienteVinculado.Equals(New ObjectId(fbcCliente.Value))).
                        Select(Function(y) New With {Key .cvevinculacion_ = y.cveVinculacion, Key .vinculacion_ = y.vinculacion}).ToList()

                        If vinculacionencontrada_.Count > 0 Then

                            scVinculacion.DataSource = New List(Of SelectOption)

                            Dim opciones_ As New List(Of SelectOption)

                            For Each item_ In vinculacionencontrada_

                                opciones_.Add(New SelectOption With {
                                    .Value = item_.cvevinculacion_,
                                    .Text = item_.vinculacion_
                                })
                            Next

                            scVinculacion.DataSource = opciones_

                            scVinculacion.Value = vinculacionencontrada_(0).cvevinculacion_

                        End If

                    End If

                End If

            End If

            ' DisplayMessage("Comprador establecido correctamente", StatusMessage.Info)

        Else

            scDomicilioCompradorReceptor.Value = Nothing

            scDomicilioCompradorReceptor.DataSource = Nothing

            SetVars("ProveedorSeleccionado_", Nothing)

            SetVars("DomicilioProveedorSeleccionado_", Nothing)

            'swcEsDestinatario.Checked = True

            DisplayMessage("Comprador no encontrado", StatusMessage.Fail)

        End If

    End Sub

    'Protected Function fbcCompradorReceptor_SelectedIndexChanged(sender As Object, e As EventArgs)
    'End Function

    'Protected Sub scDomicilioCompradorReceptor_SelectedIndexChanged(sender As Object, e As EventArgs)

    'End Sub

    'Protected Sub scDomicilioCompradorReceptor_TextChanged(sender As Object, e As EventArgs)

    'End Sub

    'Protected Sub scDomicilioCompradorReceptor_Click(sender As Object, e As EventArgs)

    'End Sub

#End Region
#Region "Destinatarios"

    'Protected Sub swcEsDestinatario_CheckedChanged(sender As Object, e As EventArgs)

    '    If swcEsDestinatario.Checked Then

    '        fscCompradorDestinatario.Visible = False

    '        ''El proveedor es destinatario?
    '        SetVars("EsDestinatario_", True)

    '    Else

    '        fscCompradorDestinatario.Visible = True

    '        SetVars("EsDestinatario_", False)

    '        fbcCompradorDestinatario.Text = Nothing

    '        fbcCompradorDestinatario.Value = Nothing

    '        scDomicilioCompradorDestinatario.Value = Nothing

    '        scDomicilioCompradorDestinatario.DataSource = Nothing

    '    End If

    'End Sub

    'Protected Sub fbcCompradorDestinatario_TextChanged(sender As Object, e As EventArgs)

    '    Dim tagwatcher_ As New TagWatcher

    '    Dim listaDestinatarios_ As List(Of AuxiliarDestinatario)

    '    Dim listaDataSource_ As New List(Of SelectOption)

    '    If fbcCompradorDestinatario.Value = "" Then

    '        tagwatcher_ = _utils.ObtenerDestinatariosPorControlador(fbcCompradorDestinatario.Text)

    '        If tagwatcher_.Status = TypeStatus.Ok Then

    '            listaDestinatarios_ = DirectCast(tagwatcher_.ObjectReturned, List(Of AuxiliarDestinatario))

    '            If listaDestinatarios_.Count > 0 Then

    '                Dim destinatariosActivos_ = New List(Of AuxiliarDestinatario)

    '                For Each item_ In listaDestinatarios_

    '                    If item_._activo Then

    '                        listaDataSource_.Add(New SelectOption With
    '                        {.Value = item_.idtaxid,
    '                         .Text = $"{item_._razonsocial} | {item_._taxid}"})

    '                        destinatariosActivos_.Add(item_)

    '                    End If

    '                Next

    '                SetVars("listaDestinatarios_", destinatariosActivos_)

    '            End If

    '        Else

    '            DisplayMessage("Destinatario no encontrado", StatusMessage.Fail)

    '        End If

    '        fbcCompradorDestinatario.DataSource = listaDataSource_

    '    End If

    'End Sub

    'Protected Sub fbcCompradorDestinatario_Click(sender As Object, e As EventArgs)

    '    If fbcCompradorDestinatario.Value <> "" Then

    '        Dim destinatarios_ As List(Of AuxiliarDestinatario) = GetVars("listaDestinatarios_")

    '        Dim destinatarioseleccionado_ = destinatarios_.Where(Function(x) x.idtaxid.Equals(fbcCompradorDestinatario.Value)).First

    '        ''AGREGAR EL PROVEEDOR SELECCIONADO A LA SESSION
    '        SetVars("DestinatarioSeleccionado_", destinatarioseleccionado_)

    '        Dim datasource_ As New List(Of SelectOption)

    '        datasource_.Add(New SelectOption With
    '                     {.Value = destinatarioseleccionado_._listadomiciliosconTaxid.FirstOrDefault._iddomicilio.ToString,
    '                      .Text = destinatarioseleccionado_._listadomiciliosconTaxid.FirstOrDefault.domicilioPresentacion.ToUpper})

    '        scDomicilioCompradorDestinatario.DataSource = datasource_

    '        scDomicilioCompradorDestinatario.Value = datasource_.Last.Value

    '        _domicilioSeleccionado = destinatarioseleccionado_._listadomiciliosconTaxid.FirstOrDefault

    '        ''AGREGAR EL PROVEEDOR SELECCIONADO A LA SESSION
    '        SetVars("DomicilioDestinatarioSeleccionado_", _domicilioSeleccionado)

    '        'DisplayMessage("Destinatario establecido correctamente", StatusMessage.Info)

    '    End If

    'End Sub

    'Protected Sub scDomicilioCompradorDestinatario_SelectedIndexChanged(sender As Object, e As EventArgs)

    'End Sub

    'Protected Sub scDomicilioCompradorDestinatario_TextChanged(sender As Object, e As EventArgs)

    'End Sub

    'Protected Sub scDomicilioCompradorDestinatario_Click(sender As Object, e As EventArgs)

    'End Sub

#End Region

#Region "Vinculacion"
    Protected Sub scVinculacion_Click(sender As Object, e As EventArgs)
        scVinculacion.DataSource = _utils.Vinculacion()
    End Sub
#End Region

#Region "Metodo de valoracion"
    Protected Sub scMetodoValoracion_Click(sender As Object, e As EventArgs)
    End Sub

#End Region

#Region "Producto"

    'Protected Sub BuscarProducto()

    '    System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: ENTRANDO A BUSCAR PRODUCTO")

    '    Dim cfdiCargado_ = False

    '    If GetVars("CfdiCargado") IsNot Nothing Then

    '        cfdiCargado_ = GetVars("CfdiCargado")

    '    End If

    '    If fbcProducto.Value = "" OrElse cfdiCargado_ = True Then
    '        'If fbcProducto.Value = "" Then

    '        Dim estado_ As TagWatcher = _utils.BuscarProductos(fbcProducto.Text.ToUpper(), fbcCliente.Value, fbcCompradorReceptor.Value)

    '        System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: ESTADO DEL PRODUCTO {estado_}")

    '        If estado_ IsNot Nothing AndAlso estado_.Status = TypeStatus.Ok Then
    '            System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: PRODUCTO OK")
    '            ListaProductosEncontrados(estado_.ObjectReturned)
    '        Else

    '            DisplayMessage("Producto no disponible", StatusMessage.Fail)

    '        End If

    '    End If

    'End Sub

    Protected Sub fbcProducto_TextChanged(sender As Object, e As EventArgs)

        If fbcProducto.Value = "" Then

            Dim estado_ As TagWatcher = _utils.BuscarProductos(fbcProducto.Text.ToUpper(), fbcCliente.Value, fbcCompradorReceptor.Value)

            System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: ESTADO DEL PRODUCTO {estado_}")

            If estado_ IsNot Nothing AndAlso estado_.Status = TypeStatus.Ok Then
                System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: PRODUCTO OK")
                ListaProductosEncontrados(estado_.ObjectReturned)
            Else

                DisplayMessage("Producto no disponible", StatusMessage.Fail)

            End If

        End If


    End Sub


    Protected Sub ListaProductosEncontrados(ByVal productosEncontrados_ As List(Of AuxiliarProducto))

        Dim listaDataSource_ As New List(Of SelectOption)

        System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: PRODUCTO  ENCONTRADO ES: {productosEncontrados_}")

        For Each item_ In productosEncontrados_

            Dim partes_ As New List(Of String) From {
                item_._idKrom,
                item_._numeroParte.ToUpper()
            }

            If item_._descripcion <> "" Then

                partes_.Add(item_._descripcion.ToUpper())

            End If

            If item_._alias <> "" Then

                If item_._nombrecomercial <> "" Then

                    partes_.Add($"{item_._alias.ToUpper()} ({item_._nombrecomercial.ToUpper()})")

                Else

                    partes_.Add(item_._alias.ToUpper())

                End If

            Else

                If item_._nombrecomercial <> "" Then

                    partes_.Add(item_._nombrecomercial.ToUpper())

                End If

            End If

            listaDataSource_.Add(New SelectOption With {
                    .Value = item_.id.ToString(),
                    .Text = String.Join(" | ", partes_)
})
        Next
        If listaDataSource_.Count > 0 Then
            SetVars("ListaProductos", productosEncontrados_)
            fbcProducto.DataSource = listaDataSource_
        Else
            DisplayMessage("Producto no encontrado", StatusMessage.Fail)
        End If
    End Sub
    Protected Sub fbcProducto_Click(sender As Object, e As EventArgs)

        If fbcProducto.Text <> "" Then
            If fbcProducto.Value <> "" Then
                Dim listaProductos_ = DirectCast(GetVars("ListaProductos"), List(Of AuxiliarProducto))
                Dim productoText_ = fbcProducto.Text.Split("|")
                Dim idKrom_ = Integer.Parse(productoText_(0))
                Dim idproducto_ = fbcProducto.Value

                ' Filtra por id Y por idKrom al mismo tiempo
                Dim productoseleccionado_ = listaProductos_.FirstOrDefault(Function(x) x.id.ToString() = idproducto_ AndAlso x._idKrom = idKrom_)

                If productoseleccionado_ Is Nothing Then
                    DisplayMessage("No se encontró el producto seleccionado.", StatusMessage.Fail)
                    Exit Sub
                End If

                ' Lista auxiliar
                Dim listaAuxiliarProductos_ As List(Of AuxiliarProducto)
                If GetVars("ListaAuxliarProductos_") IsNot Nothing Then
                    listaAuxiliarProductos_ = DirectCast(GetVars("ListaAuxliarProductos_"), List(Of AuxiliarProducto))
                Else
                    listaAuxiliarProductos_ = New List(Of AuxiliarProducto)
                End If

                ' Asignar valores al UI directamente desde productoseleccionado_
                ' (ya no hace falta el If idKrom porque el FirstOrDefault ya lo garantiza)
                icDescripcionPartidaOriginal.Value = productoseleccionado_._descripcion
                icDescripcionPartida.Value = productoseleccionado_._nombrecomercial
                icDescripcionCOVE.Value = productoseleccionado_._descripcioncove
                icObjectIdProducto.Value = productoseleccionado_.id.ToString()

                icFraccionArancelaria.DataSource = New List(Of SelectOption) From {
                    New SelectOption With {
                        .Value = productoseleccionado_._fraccionArancelaria,
                        .Text = $"{productoseleccionado_._fraccionArancelaria} - {productoseleccionado_._fraccionArancelariaPresentacion.ToUpper()}"
                    }
                }
                icFraccionArancelaria.Value = productoseleccionado_._fraccionArancelaria

                icFraccionNico.DataSource = New List(Of SelectOption) From {
                    New SelectOption With {
                        .Value = productoseleccionado_._nico,
                        .Text = $"{productoseleccionado_._nico} - {productoseleccionado_._nicoPresentacion.ToUpper()}"
                    }
                }
                icFraccionNico.Value = productoseleccionado_._nico

                If productoseleccionado_._cveunidadmedida IsNot Nothing Then
                    scUnidadMedidaTarifa.DataSource = New List(Of SelectOption) From {
                        New SelectOption With {
                            .Value = productoseleccionado_._cveunidadmedida,
                            .Text = $"{productoseleccionado_._cveunidadmedida} - {productoseleccionado_._unidadmedidapresentacion}"
                        }
                    }
                    scUnidadMedidaTarifa.Value = productoseleccionado_._cveunidadmedida
                End If

                ' Construir auxiliarProducto
                Dim auxiliarProducto_ = New AuxiliarProducto With {
                    .id = productoseleccionado_.id,
                    ._idKrom = productoseleccionado_._idKrom,
                    ._timestamp = productoseleccionado_._timestamp
                }
                coTimeStamp.Value = productoseleccionado_._timestamp

                listaAuxiliarProductos_.Add(auxiliarProducto_)

                ' Tooltip
                icFraccionArancelaria.ToolTip = productoseleccionado_._status
                icFraccionArancelaria.ToolTipExpireTime = 60
                icFraccionArancelaria.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkInfo
                icFraccionArancelaria.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
                icFraccionArancelaria.ShowToolTip()

                SetVars("ListaAuxliarProductos_", listaAuxiliarProductos_)  ' <-- corregido el nombre de la key
            End If
        Else
            icObjectIdProducto.Value = Nothing
            icDescripcionPartida.Value = Nothing
            icDescripcionPartidaOriginal.Value = Nothing
            icDescripcionCOVE.Value = Nothing
            icFraccionArancelaria.DataSource = Nothing
            icFraccionNico.Value = Nothing
            icFraccionNico.DataSource = Nothing
            scUnidadMedidaTarifa.Value = Nothing
            scUnidadMedidaTarifa.DataSource = Nothing
        End If

        ''REVISA AQUI PORFI
        ' System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: PRODUCTO HICISTE CLICK")


        'If fbcProducto.Text <> "" Then
        '    If fbcProducto.Value <> "" Then

        '        ' System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: ENTRANDO AL PRODUCTO")

        '        Dim listaProductos_ = DirectCast(GetVars("ListaProductos"), List(Of AuxiliarProducto))
        '        Dim productoText_ = fbcProducto.Text.Split("|")
        '        Dim idKrom_ = Integer.Parse(productoText_(0))

        '        Dim idproducto_ = fbcProducto.Value

        '        Dim productoseleccionado_ = listaProductos_.Single(Function(x) x.id.ToString() = idproducto_)

        '        Dim listaAuxiliarProductos_ As List(Of AuxiliarProducto)

        '        If GetVars("ListaAuxliarProductos_") IsNot Nothing Then

        '            listaAuxiliarProductos_ = DirectCast(GetVars("ListaAuxliarProductos_"), List(Of AuxiliarProducto))

        '        Else

        '            listaAuxiliarProductos_ = New List(Of AuxiliarProducto)

        '        End If

        '        Dim auxiliarProducto_ = New AuxiliarProducto

        '        '  System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice:EL PRODUCTO SELECCIONADO ES: { productoseleccionado_._idKrom}")

        '        With productoseleccionado_

        '            If productoseleccionado_._idKrom = idKrom_ Then

        '                icDescripcionPartidaOriginal.Value = IIf(._descripcion IsNot Nothing OrElse ._descripcion <> "", ._descripcion.ToUpper(), ._descripcion)
        '                icDescripcionPartida.Value = IIf(._nombrecomercial IsNot Nothing OrElse ._nombrecomercial <> "", ._nombrecomercial.ToUpper(), ._nombrecomercial)
        '                icDescripcionCOVE.Value = IIf(._descripcioncove IsNot Nothing OrElse ._descripcioncove <> "", ._descripcioncove.ToUpper(), ._descripcioncove)

        '            End If

        '            icObjectIdProducto.Value = .id.ToString

        '            icFraccionArancelaria.DataSource = New List(Of SelectOption) From {New SelectOption With
        '                                 {.Value = productoseleccionado_._fraccionArancelaria,
        '                                  .Text = $"{ productoseleccionado_._fraccionArancelaria} - { productoseleccionado_._fraccionArancelariaPresentacion.ToUpper()}"}}

        '            icFraccionArancelaria.Value = productoseleccionado_._fraccionArancelaria

        '            icFraccionNico.DataSource = New List(Of SelectOption) From {New SelectOption With
        '                                 {.Value = productoseleccionado_._nico,
        '                                  .Text = $"{ productoseleccionado_._nico} - { productoseleccionado_._nicoPresentacion.ToUpper()}"}}

        '            icFraccionNico.Value = ._nico

        '            If productoseleccionado_._cveunidadmedida IsNot Nothing Then
        '                scUnidadMedidaTarifa.DataSource = New List(Of SelectOption) From {New SelectOption With
        '                    {.Value = productoseleccionado_._cveunidadmedida,
        '                    .Text = $"{productoseleccionado_._cveunidadmedida} - {productoseleccionado_._unidadmedidapresentacion}"}}
        '                scUnidadMedidaTarifa.Value = productoseleccionado_._cveunidadmedida
        '            End If
        '            auxiliarProducto_.id = .id
        '            auxiliarProducto_._idKrom = ._idKrom ''DEBE SER LA SECUENCIA


        '            System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice:EL PRODUCTO LLEGA CON ESTE TIMESTAMP { productoseleccionado_._timestamp}")
        '            auxiliarProducto_._timestamp = ._timestamp
        '            coTimeStamp.Value = productoseleccionado_._timestamp

        '            listaAuxiliarProductos_.Add(auxiliarProducto_)
        '            icFraccionArancelaria.ToolTip = productoseleccionado_._status
        '            icFraccionArancelaria.ToolTipExpireTime = 60
        '            icFraccionArancelaria.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkInfo
        '            icFraccionArancelaria.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
        '            icFraccionArancelaria.ShowToolTip()
        '            SetVars("ListaProductos", listaAuxiliarProductos_)

        '            ' DisplayMessage("Producto establecido correctamente", OkInfo)

        '        End With
        '    End If
        'Else
        '    icObjectIdProducto.Value = Nothing

        '    icDescripcionPartida.Value = Nothing

        '    icFraccionArancelaria.Value = Nothing

        '    icFraccionArancelaria.DataSource = Nothing

        '    icFraccionNico.Value = Nothing

        '    icFraccionNico.DataSource = Nothing

        '    scUnidadMedidaTarifa.Value = Nothing

        '    scUnidadMedidaTarifa.DataSource = Nothing

        '    icDescripcionPartidaOriginal.Value = Nothing

        '    icDescripcionPartida.Value = Nothing

        '    icDescripcionCOVE.Value = Nothing

        'End If

    End Sub

#End Region

#Region "Unidad de medida"
    Protected Sub scUnidadMedidaComercial_TextChanged(sender As Object, e As EventArgs)

        _utils.CargaUnidades(scUnidadMedidaComercial, ControladorUnidadesMedida.TiposUnidad.Comercial, 10)

    End Sub

    Protected Sub scUnidadMedidaComercial_Click(sender As Object, e As EventArgs)

        _utils.CargaUnidades(scUnidadMedidaComercial, ControladorUnidadesMedida.TiposUnidad.Comercial)

        icCantidadTarifa.Value = Nothing

    End Sub

    Protected Sub scUnidadMedidaComercial_SelectedIndexChanged(sender As Object, e As EventArgs)

        If scUnidadMedidaTarifa.Value <> "" Then

            Dim unidadMedidaTarifa_ = scUnidadMedidaTarifa.Value

            If unidadMedidaTarifa_ = scUnidadMedidaComercial.Value Then

                icCantidadTarifa.Value = icCantidadComercial.Value

            End If

        Else

            icCantidadTarifa.Value = Nothing

        End If

    End Sub

    Protected Sub scUnidadMedidaTarifa_TextChanged(sender As Object, e As EventArgs)

        _utils.CargaUnidades(scUnidadMedidaTarifa, ControladorUnidadesMedida.TiposUnidad.Comercial)

    End Sub

    Protected Sub scUnidadMedidaTarifa_Click(sender As Object, e As EventArgs)

        _utils.CargaUnidades(scUnidadMedidaTarifa, ControladorUnidadesMedida.TiposUnidad.Comercial)

    End Sub

#End Region

#Region "Pillbox"
    Protected Sub pbPartidasItems_CheckedChange(sender As Object, e As EventArgs)

        Dim tipoCaptura_ = GetVars("_tipoCaptura")

        Select Case pbPartidasItems.ToolbarAction

            Case PillboxControl.ToolbarActions.Nuevo

                lbNumero.Text = (pbPartidasItems.PageIndex).ToString()

                CargarMonedaPorDefault(moneda_:=Nothing, objectIdPais_:=GetVars("idPais"))

                _dataSourceMoneda = New List(Of SelectOption) _
                 From {New SelectOption With {.Value = scMonedaFactura.Value, .Text = scMonedaFactura.Text}}


            Case PillboxControl.ToolbarActions.Anterior

                lbNumero.Text = (pbPartidasItems.PageIndex).ToString()

                'CargarMetodoValoracion(scMetodoValoracionPartida)

                AvisosVerificacionObjectIdValido()

            Case PillboxControl.ToolbarActions.Siguiente

                lbNumero.Text = (pbPartidasItems.PageIndex).ToString()

                'CargarMetodoValoracion(scMetodoValoracionPartida)

                AvisosVerificacionObjectIdValido()

        End Select

    End Sub

    Protected Sub pbPartidasItems_Click(sender As Object, e As EventArgs)


        Select Case pbPartidasItems.ToolbarAction

            Case PillboxControl.ToolbarActions.Nuevo

                lbNumero.Text = (pbPartidasItems.PageIndex).ToString()

                'Dim aquiui_ = pbPartidasItems.DataSource(0).Item("fbcPaisPartida").Item("Value")

                fbcPaisPartida.Value = pbPartidasItems.DataSource(0).Item("fbcPaisPartida").Item("Value")

                fbcPaisPartida.Text = pbPartidasItems.DataSource(0).Item("fbcPaisPartida").Item("Text")

                'fbcPaisPartida.DataSource = New List(Of SelectOption) _
                '    From {New SelectOption With {.Value = pbPartidasItems.DataSource(0).Item("fbcPaisPartida").Item("Value"), .Text = pbPartidasItems.DataSource(0).Item("fbcPaisPartida").Item("Text")}}

                scMonedaMercanciaItemPartida.DataSource = New List(Of SelectOption) _
                    From {New SelectOption With {.Value = pbPartidasItems.DataSource(0).Item("scMonedaMercanciaItemPartida").Item("Value"), .Text = pbPartidasItems.DataSource(0).Item("scMonedaMercanciaItemPartida").Item("Text")}}

                scMonedaMercanciaItemPartida.Value = pbPartidasItems.DataSource(0).Item("scMonedaMercanciaItemPartida").Item("Value")

                scMonedaValorAgregadoPartida.DataSource = New List(Of SelectOption) _
                    From {New SelectOption With {.Value = pbPartidasItems.DataSource(0).Item("scMonedaValorAgregadoPartida").Item("Value"), .Text = pbPartidasItems.DataSource(0).Item("scMonedaValorAgregadoPartida").Item("Text")}}

                scMonedaValorAgregadoPartida.Value = pbPartidasItems.DataSource(0).Item("scMonedaValorAgregadoPartida").Item("Value")

                scMonedaPrecioUnitarioPartida.DataSource = New List(Of SelectOption) _
                    From {New SelectOption With {.Value = pbPartidasItems.DataSource(0).Item("scMonedaPrecioUnitarioPartida").Item("Value"), .Text = pbPartidasItems.DataSource(0).Item("scMonedaPrecioUnitarioPartida").Item("Text")}}

                scMonedaPrecioUnitarioPartida.Value = pbPartidasItems.DataSource(0).Item("scMonedaPrecioUnitarioPartida").Item("Value")

                MetodoValoracionInicial()

            Case PillboxControl.ToolbarActions.Anterior

                lbNumero.Text = (pbPartidasItems.PageIndex).ToString()
                AvisosVerificacionObjectIdValido()

            Case PillboxControl.ToolbarActions.Siguiente

                lbNumero.Text = (pbPartidasItems.PageIndex).ToString()
                AvisosVerificacionObjectIdValido()

        End Select



    End Sub

#End Region

#Region "PUBLICAR"
    Private Function FirmarDocumentoPublicar() As Task(Of TagWatcher)

        ''AQUI ENTONCES VOY A VALIDAR TODO ANTES DE PUBLICAR
        Dim tagwatcher_ As New TagWatcher

        Dim tipoMensaje_ As StatusMessage = StatusMessage.Info

        _utils = New UtilsFacturaComercial

        If OperacionGenerica IsNot Nothing Then

            Dim environmentid_ As Int32 = ListaEmpresas.Value
            ''ESTO ES PARA FIRMAR LA FACTURA
            tagwatcher_ = _utils.PublicarFacturaComercialAsync(DirectCast(OperacionGenerica, OperacionGenerica), Buscador,
                                                               IControladorFacturaComercial.TipoOperaciones.Exportacion, environmentid_).Result

            If tagwatcher_.Status = TypeStatus.OkInfo Then

                tipoMensaje_ = StatusMessage.Success : tagwatcher_.SetOKInfo(Me, "✨ Factura comercial exportación ha sido publicada")

            Else

                tipoMensaje_ = StatusMessage.Fail : tagwatcher_.SetError(Me, "Factura no publicada")

            End If

        Else

            tipoMensaje_ = StatusMessage.Fail : tagwatcher_.SetError(Me, "Factura no publicada")

        End If

        Return Task.FromResult(tagwatcher_)

    End Function
#End Region

#Region "CFDI"
    Protected Sub fcCFDI_ChooseFile(sender As PropiedadesDocumento, e As EventArgs)

        If sender.nombrearchivo IsNot Nothing Then

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

                .formatoarchivo = PropiedadesDocumento.FormatosArchivo.xml

            End With

        End If

    End Sub

    Public Function LoadCfdiXml(ByVal objectidFile_ As List(Of ObjectId)) As String
        ''AHORITA SERA DESDE AQUI COMO PRUEBA, YA LUEGO DESDE EL COMPONENTE HIJE
        Try
            ' 1. Obtener los bytes originales
            Dim docBytes As Byte() = (New ControladorDocumento).GetDocument(objectidFile_(0)).ObjectReturned

            If docBytes Is Nothing OrElse docBytes.Length = 0 Then Throw New Exception("Archivo vacío")

            ' 2. Convertir a string usando UTF8
            Dim xmlTexto As String = Encoding.UTF8.GetString(docBytes)

            ' 3. ELIMINAR EL ERROR DE POSICIÓN 1, LÍNEA 1
            ' Buscamos el primer menor que "<" para ignorar cualquier caracter invisible (BOM)
            Dim indexRaw = xmlTexto.IndexOf("<")
            If indexRaw >= 0 Then
                xmlTexto = xmlTexto.Substring(indexRaw).Trim()
            End If

            Return xmlTexto
        Catch ex As Exception
            Throw New Exception("Error al cargar y limpiar XML: " & ex.Message)
        End Try

    End Function

    Protected Sub extraerCDFI_Click(sender As Object, e As EventArgs)

        Dim listaDocumentos_ As List(Of Newtonsoft.Json.Linq.JObject) =
            Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of Newtonsoft.Json.Linq.JObject))(fcCFDI.Value)

        Dim listaIdsDocumento_ As New List(Of ObjectId)

        If listaDocumentos_ IsNot Nothing Then

            If listaDocumentos_.Count = 1 Then

                Dim totaldocumentossinprocesar_ = listaDocumentos_.Count

                Dim extension_ As String = Nothing

                For Each documento_ In listaDocumentos_

                    extension_ = Path.GetExtension(documento_.SelectToken("fileName"))

                    listaIdsDocumento_.Add(ObjectId.Parse(documento_.SelectToken("fileId").ToString()))

                Next

                '''REEMPLZAR POR LA API DE RODRI
                '''VALIDAR QUE SEA UN .XML

                If extension_ = ".xml" OrElse extension_ = ".XML" Then

                    Dim cfdi_str = LoadCfdiXml(listaIdsDocumento_)

                    Dim tagwatcher_ As New TagWatcher
                    System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: Entrando a deserealizar")

                    Dim environmentid_ As Int32 = ListaEmpresas.Value

                    _controladorFacturaComercial = New ControladorFacturaComercial(IControladorFacturaComercial.TipoOperaciones.Exportacion, environmentid_)

                    _loginUsuario = Session("DatosUsuario")

                    tagwatcher_ = _controladorFacturaComercial.DeserializarCFDI(cfdi_str, _loginUsuario("Nombre"))

                    If tagwatcher_.Status = TypeStatus.Ok Then

                        Dim estadoFactura_ As New TagWatcher

                        DisplayMessage("CFDI(xml) integrado correctamente", StatusMessage.Info)

                        InternalProcess = ProccessStatus.Active

                        Dim response_ = DirectCast(tagwatcher_.ObjectReturned, ResponseOperacion)

                        estadoFactura_ = ExecuteProcess(Function() BusquedaGeneralParams(response_.id.ToString, Nothing), "Detenido búsqueda general")

                        If estadoFactura_.Status = TypeStatus.Ok Then

                            PanelProcesamiento.Visible = False

                            SetVars("ActivaControles", False) : ActivaControles(GetVars("ActivaControles"))

                            SetVars("OperationMode", "E")

                            SetVars("isEditing", True)

                            SetVars("CfdiCargado", True)

                            SetVars("_tipoCaptura", 1)

                            lbModoCapturaManual.Visible = False
                            lbModoCapturaManualNuevo.Visible = False
                            lbModoCapturaIA.Visible = False

                            lbModoCapturaIAEditar.Visible = True

                            dbcNumFacturaAcuseValor.Enabled = False


                            ''PENDIENTE LA MEJOR YA NO
                            ''MANDAMOS A CREAR LAS VARIABLES DE SESION DE LO QUE EXISTA:

                            Dim constructorFacturaComercial_ = DirectCast(response_.OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente, ConstructorFacturaComercial)

                            ''AQUI HAREMOS EL JALE

                            Dim objectidZero_ = "000000000000000000000000"

                            MetodoValoracionInicial()

                            scVinculacion.DataSource = _utils.Vinculacion()

                            scVinculacion.Value = 0
                            ' AvisosVerificacionObjectIdValido()

                        Else

                            PanelProcesamiento.Visible = True

                            PanelProcesamiento.Dispose()

                            SetVars("ActivaControles", True) : ActivaControles(GetVars("ActivaControles"))

                            SetVars("OperationMode", "N")

                            SetVars("isEditing", False)

                            SetVars("CfdiCargado", False)

                            dbcNumFacturaAcuseValor.Enabled = True

                            DisplayMessage($"{estadoFactura_.ObjectReturned}", StatusMessage.Fail)

                        End If

                    Else

                        PanelProcesamiento.Visible = True

                        PanelProcesamiento.Dispose()

                        SetVars("ActivaControles", True) : ActivaControles(GetVars("ActivaControles"))

                        SetVars("OperationMode", "N")

                        SetVars("isEditing", False)

                        SetVars("CfdiCargado", False)

                        dbcNumFacturaAcuseValor.Enabled = True

                        DisplayMessage($"{tagwatcher_.ObjectReturned}", StatusMessage.Fail)

                    End If

                Else

                    DisplayMessage("CFDI(xml) no válido", StatusMessage.Fail)

                End If

            Else

                DisplayMessage("Solo se permite subir un CFDI(xml) por factura", StatusMessage.Fail)

            End If

        Else

            DisplayMessage("Agregue un CFDI(xml) para comenzar", StatusMessage.Info)

        End If

    End Sub

    Private Function VerificarObjectIdValidoRegistros() As Boolean

        Dim objectidZero_ = "000000000000000000000000"

        If fbcCliente.Value = objectidZero_ OrElse fbcCliente.Value = "" Then

            Return False

        End If

        If dbcNumFacturaAcuseValor.Value = "" Then

            Return False

        End If

        If icFechaFactura.Value = "" Then


            Return False

        End If

        If fbcIncoterm.Value = objectidZero_ OrElse fbcIncoterm.Value = "" Then


            Return False

        End If


        'If fbcPais.Value = objectidZero_ OrElse fbcPais.Value = "" Then


        '    Return False

        'End If


        If icValorFactura.Value <> "" AndAlso icValorFactura.Value <> 0 Then

            If scMonedaFactura.Value = objectidZero_ OrElse scMonedaFactura.Value = "" Then


                Return False

            End If

        Else

            Return False

        End If


        If icValorMercancia.Value <> "" AndAlso icValorMercancia.Value <> 0 Then

            If scMonedaMercancia.Value = objectidZero_ OrElse scMonedaMercancia.Value = "" Then


                Return False

            End If

        Else


            Return False

        End If

        If fbcCompradorReceptor.Text <> "" Then

            If fbcCompradorReceptor.Value = objectidZero_ OrElse fbcCompradorReceptor.Value = "" Then


                Return False

            End If

            If scDomicilioCompradorReceptor.Value = objectidZero_ Or scDomicilioCompradorReceptor.Value = "" Then

                Return False

            End If

        Else


            Return False

        End If


        If fbcProducto.Value = "" OrElse fbcProducto.Value = objectidZero_ Then

            If icFraccionArancelaria.Value = "" Then

                Return False

            End If

            If icFraccionNico.Value = "" Then


                Return False

            End If

            If icCantidadTarifa.Value <> "" Then

                If scUnidadMedidaTarifa.Value = "" Then


                    Return False

                End If

            End If

            If icDescripcionPartidaOriginal.Value = "" Then


                Return False

            End If

            If icDescripcionPartida.Value = "" Then


                Return False

            End If

        Else

            If icFraccionArancelaria.Value = "" Then


                Return False

            End If

            If icFraccionNico.Value = "" Then


                Return False

            End If

            If icCantidadTarifa.Value <> "" Then

                If scUnidadMedidaTarifa.Value = "" Then

                    Return False

                End If

            Else


                Return False

            End If

            If icDescripcionPartidaOriginal.Value = "" Then


                Return False

            End If

            If icDescripcionPartida.Value = "" Then


                Return False

            End If

        End If


        If scUnidadMedidaTarifa.Value <> "" Then

            If icCantidadTarifa.Value = "" Then


                Return False

            End If

        End If

        If icCantidadComercial.Value <> "" Then

            If scUnidadMedidaComercial.Value = objectidZero_ OrElse scUnidadMedidaComercial.Value = "" Then


                Return False

            End If

        End If


        If icValorMercanciaItem.Value <> "" AndAlso icValorMercanciaItem.Value <> 0 Then

            If scMonedaMercanciaItemPartida.Value = "" Then


                Return False

            End If

        Else

            Return False

        End If

        If icPrecioUnitario.Value <> "" AndAlso icPrecioUnitario.Value <> 0 Then

            If scMonedaPrecioUnitarioPartida.Value = "" Then


                Return False

            End If

        Else


            Return False

        End If

        If icValorAgregadoPartida.Value <> "" Then

            If scMonedaValorAgregadoPartida.Value = "" Then

                Return False

            End If

        End If

        If fbcPaisPartida.Value = "" OrElse fbcPaisPartida.Value = objectidZero_ Then

            Return False

        End If

        Return True

    End Function

    Private Sub CamposRequeridos()

        Dim objectidZero_ = "000000000000000000000000"

        If fbcCliente.Value = objectidZero_ OrElse fbcCliente.Value = "" Then

            ConfiguracionRequerido(fbcCliente)

        End If

        If dbcNumFacturaAcuseValor.Value = "" Then

            ConfiguracionRequerido(dbcNumFacturaAcuseValor)

        End If

        If icFechaFactura.Value = "" Then

            ConfiguracionRequerido(icFechaFactura)

        End If

        If fbcIncoterm.Value = objectidZero_ OrElse fbcIncoterm.Value = "" Then

            ConfiguracionRequerido(fbcIncoterm)

        End If

        If String.IsNullOrEmpty(icValorFactura.Value) Then

            ConfiguracionRequerido(icValorFactura)

            ConfiguracionRequerido(scMonedaFactura)

        Else

            If Not String.IsNullOrEmpty(icValorFactura.Value) OrElse icValorFactura.Value > 0 Then

                If scMonedaFactura.Value = objectidZero_ OrElse scMonedaFactura.Value = "" Then

                    ConfiguracionRequerido(scMonedaFactura)

                End If

            Else

                ConfiguracionZero(icValorFactura)

            End If

        End If

        If String.IsNullOrEmpty(icValorMercancia.Value) Then

            ConfiguracionRequerido(icValorMercancia)

            ConfiguracionRequerido(scMonedaMercancia)

        Else

            If Not String.IsNullOrEmpty(icValorMercancia.Value) OrElse icValorMercancia.Value > 0 Then

                If scMonedaMercancia.Value = objectidZero_ OrElse scMonedaMercancia.Value = "" Then

                    ConfiguracionRequerido(scMonedaMercancia)

                End If

            Else

                ConfiguracionZero(icValorMercancia)

            End If

        End If

        If fbcCompradorReceptor.Text <> "" Then

            If fbcCompradorReceptor.Value = objectidZero_ OrElse fbcCompradorReceptor.Value = "" Then

                ConfiguracionRequerido(fbcCompradorReceptor)

            End If

            If scDomicilioCompradorReceptor.Value = objectidZero_ Or scDomicilioCompradorReceptor.Value = "" Then

                ConfiguracionRequerido(scDomicilioCompradorReceptor)

            End If

        Else

            ConfiguracionRequerido(fbcCompradorReceptor)

            ConfiguracionRequerido(scDomicilioCompradorReceptor)

        End If


        If fbcProducto.Value = "" OrElse fbcProducto.Value = objectidZero_ Then

            ConfiguracionRequerido(fbcProducto)

            If icFraccionArancelaria.Value = "" Then

                ConfiguracionRequerido(icFraccionArancelaria)

            End If

            If icFraccionNico.Value = "" Then

                ConfiguracionRequerido(icFraccionNico)

            End If

            If icCantidadTarifa.Value <> "" Then

                If scUnidadMedidaTarifa.Value = "" Then

                    ConfiguracionRequerido(scUnidadMedidaTarifa)

                End If

            End If

            If icDescripcionPartidaOriginal.Value = "" Then

                ConfiguracionRequerido(icDescripcionPartidaOriginal)

            End If

            If icDescripcionPartida.Value = "" Then

                ConfiguracionRequerido(icDescripcionPartida)

            End If

        Else

            If icFraccionArancelaria.Value = "" Then

                ConfiguracionRequerido(icFraccionArancelaria)

            End If

            If icFraccionNico.Value = "" Then

                ConfiguracionRequerido(icFraccionNico)

            End If

            If icCantidadTarifa.Value <> "" Then

                If scUnidadMedidaTarifa.Value = "" Then

                    ConfiguracionRequerido(scUnidadMedidaTarifa)

                End If

            Else

                ConfiguracionRequerido(icCantidadTarifa)

                ConfiguracionRequerido(scUnidadMedidaTarifa)

            End If

            If icDescripcionPartidaOriginal.Value = "" Then

                ConfiguracionRequerido(icDescripcionPartidaOriginal)

            End If

            If icDescripcionPartida.Value = "" Then

                ConfiguracionRequerido(icDescripcionPartida)

            End If

        End If

        If fbcPaisPartida.Value = "" OrElse fbcPaisPartida.Value = objectidZero_ Then

            ConfiguracionRequerido(fbcPaisPartida)

        End If


        If scUnidadMedidaTarifa.Value <> "" Then

            If icCantidadTarifa.Value = "" Then

                ConfiguracionRequerido(icCantidadTarifa)

            End If

        End If

        If icCantidadTarifa.Value = "" Then

            ConfiguracionRequerido(icCantidadTarifa)

        End If

        If icCantidadComercial.Value = "" Then

            ConfiguracionRequerido(icCantidadComercial)

        End If


        If icCantidadComercial.Value <> "" Then

            If scUnidadMedidaComercial.Value = objectidZero_ OrElse scUnidadMedidaComercial.Value = "" Then

                ConfiguracionRequerido(scUnidadMedidaComercial)

            End If

        End If


        If icValorMercanciaItem.Value <> "" AndAlso icValorMercanciaItem.Value <> 0 Then

            If scMonedaMercanciaItemPartida.Value = "" Then

                ConfiguracionRequerido(scMonedaMercanciaItemPartida)

            End If

        Else

            If icValorMercanciaItem.Value = "" Then

                ConfiguracionRequerido(icValorMercanciaItem)

                ConfiguracionRequerido(scMonedaMercanciaItemPartida)

            Else

                ConfiguracionZero(icValorMercanciaItem)

            End If

        End If



        If String.IsNullOrEmpty(icValorMercanciaItem.Value) Then

            ConfiguracionRequerido(icValorMercanciaItem)

            ConfiguracionRequerido(scMonedaMercanciaItemPartida)

        Else

            If Not String.IsNullOrEmpty(icValorMercanciaItem.Value) OrElse icValorMercanciaItem.Value > 0 Then

                If scMonedaMercanciaItemPartida.Value = "" Then

                    ConfiguracionRequerido(scMonedaMercanciaItemPartida)

                End If

            Else

                ConfiguracionZero(icValorMercanciaItem)

            End If

        End If

        If String.IsNullOrEmpty(icPrecioUnitario.Value) Then

            ConfiguracionRequerido(icPrecioUnitario)

            ConfiguracionRequerido(scMonedaPrecioUnitarioPartida)


        Else

            If Not String.IsNullOrEmpty(icPrecioUnitario.Value) OrElse icPrecioUnitario.Value > 0 Then

                If scMonedaPrecioUnitarioPartida.Value = "" Then

                    ConfiguracionRequerido(scMonedaPrecioUnitarioPartida)

                End If

            Else

                ConfiguracionZero(icPrecioUnitario)

            End If

        End If

        If icValorAgregadoPartida.Value <> "" Then

            If scMonedaValorAgregadoPartida.Value = "" Then

                ConfiguracionRequerido(scMonedaValorAgregadoPartida)

            End If

        End If

    End Sub

    Private Sub MensajeValorMercanciaMayor()

        icValorMercancia.ToolTip = $"{icValorMercancia.Label} supera el valor de {icValorFactura.Label}"

        icValorMercancia.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors

        icValorMercancia.ToolTipExpireTime = 60

        icValorMercancia.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

        icValorMercancia.ShowToolTip()

        icValorFactura.ToolTip = $"{icValorFactura.Label}"

        icValorFactura.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors

        icValorFactura.ToolTipExpireTime = 60

        icValorFactura.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

        icValorFactura.ShowToolTip()

    End Sub

    Private Sub MensajeValorMercanciaItemMayor()

        icValorMercanciaItem.ToolTip = $"{icValorMercanciaItem.Label} supera el valor de {icValorMercancia.Label}"

        icValorMercanciaItem.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors

        icValorMercanciaItem.ToolTipExpireTime = 60

        icValorMercanciaItem.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

        icValorMercanciaItem.ShowToolTip()


        icValorMercancia.ToolTip = $"{icValorMercancia.Label}"

        icValorMercancia.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors

        icValorMercancia.ToolTipExpireTime = 60

        icValorMercancia.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

        icValorMercancia.ShowToolTip()

    End Sub

    Private Sub ConfiguracionRequerido(ByVal object_ As Object)

        object_.ToolTip = $"{ object_.Label} es requerido"

        object_.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors

        object_.ToolTipExpireTime = 60

        object_.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

        object_.ShowToolTip()

    End Sub

    Private Sub ConfiguracionZero(ByVal object_ As Object)

        object_.ToolTip = $"{ object_.Label} debe ser mayor a 0"

        object_.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors

        object_.ToolTipExpireTime = 60

        object_.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

        object_.ShowToolTip()

    End Sub

    Private Sub AvisosVerificacionObjectIdValido()

        Dim modoEditando_ As Boolean = False

        If GetVars("isEditing") IsNot Nothing Then

            If GetVars("isEditing") = True Then

                modoEditando_ = True

            End If

        End If


        If modoEditando_ Then

            Dim objectidZero_ = "000000000000000000000000"

            If fbcCliente.Value = objectidZero_ Or fbcCliente.Value = "" Then

                fbcCliente.ToolTip = $"Verificar {fbcCliente.Label} en registros de SYNAPSIS"

                fbcCliente.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkBut

                fbcCliente.ToolTipExpireTime = 60

                fbcCliente.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

                fbcCliente.ShowToolTip()

            End If


            If fbcIncoterm.Value = objectidZero_ Or fbcIncoterm.Value = "" Then

                fbcIncoterm.ToolTip = $"Verificar { fbcIncoterm.Label} en registros de SYNAPSIS"

                fbcIncoterm.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkBut

                fbcIncoterm.ToolTipExpireTime = 60

                fbcIncoterm.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

                fbcIncoterm.ShowToolTip()

            End If


            'If fbcPais.Value = objectidZero_ Or fbcPais.Value = "" Then

            '    fbcPais.ToolTip = $"Verificar { fbcPais.Label} en registros de SYNAPSIS"

            '    fbcPais.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkBut

            '    fbcPais.ToolTipExpireTime = 60

            '    fbcPais.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

            '    fbcPais.ShowToolTip()

            'End If

            If scMonedaFactura.Value = objectidZero_ Then

                scMonedaFactura.ToolTip = $"Verificar { scMonedaFactura.Label} en registros de SYNAPSIS"

                scMonedaFactura.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkBut

                scMonedaFactura.ToolTipExpireTime = 60

                scMonedaFactura.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

                scMonedaFactura.ShowToolTip()

            End If

            If scMonedaMercancia.Value = objectidZero_ Then

                scMonedaMercancia.ToolTip = $"Verificar { scMonedaMercancia.Label} en registros de SYNAPSIS"

                scMonedaMercancia.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkBut

                scMonedaMercancia.ToolTipExpireTime = 60

                scMonedaMercancia.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

                scMonedaMercancia.ShowToolTip()

            End If


            If fbcCompradorReceptor.Text <> "" Then

                If fbcCompradorReceptor.Value = objectidZero_ Then

                    fbcCompradorReceptor.ToolTip = $"Verificar {fbcCompradorReceptor.Label} en registros de SYNAPSIS"

                    fbcCompradorReceptor.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkBut

                    fbcCompradorReceptor.ToolTipExpireTime = 60

                    fbcCompradorReceptor.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

                    fbcCompradorReceptor.ShowToolTip()

                End If

                If scDomicilioCompradorReceptor.Value = objectidZero_ Or scDomicilioCompradorReceptor.Value = "" Then

                    scDomicilioCompradorReceptor.ToolTip = $"Verificar { scDomicilioCompradorReceptor.Label} en registros de SYNAPSIS"

                    scDomicilioCompradorReceptor.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkBut

                    scDomicilioCompradorReceptor.ToolTipExpireTime = 60

                    scDomicilioCompradorReceptor.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

                    scDomicilioCompradorReceptor.ShowToolTip()

                End If

            Else

                fbcCompradorReceptor.ToolTip = $"Verificar {fbcCompradorReceptor.Label} en registros de SYNAPSIS"

                fbcCompradorReceptor.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkBut

                fbcCompradorReceptor.ToolTipExpireTime = 60

                fbcCompradorReceptor.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

                fbcCompradorReceptor.ShowToolTip()

                scDomicilioCompradorReceptor.ToolTip = $"Verificar { scDomicilioCompradorReceptor.Label} en registros de SYNAPSIS"

                scDomicilioCompradorReceptor.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkBut

                scDomicilioCompradorReceptor.ToolTipExpireTime = 60

                scDomicilioCompradorReceptor.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

                scDomicilioCompradorReceptor.ShowToolTip()

            End If

            If icObjectIdProducto.Value = objectidZero_ Then

                fbcProducto.ToolTip = $"Verificar { fbcProducto.Label} en registros de SYNAPSIS"

                fbcProducto.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkBut

                fbcProducto.ToolTipExpireTime = 60

                fbcProducto.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

                fbcProducto.ShowToolTip()


                icFraccionArancelaria.ToolTip = $"Verificar { icFraccionArancelaria.Label} en registros de SYNAPSIS"

                icFraccionArancelaria.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkBut

                icFraccionArancelaria.ToolTipExpireTime = 60

                icFraccionArancelaria.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

                icFraccionArancelaria.ShowToolTip()


                icFraccionNico.ToolTip = $"Verificar { icFraccionNico.Label} en registros de SYNAPSIS"

                icFraccionNico.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkBut

                icFraccionNico.ToolTipExpireTime = 60

                icFraccionNico.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

                icFraccionNico.ShowToolTip()


                scUnidadMedidaTarifa.ToolTip = $"Verificar { scUnidadMedidaTarifa.Label} en registros de SYNAPSIS"

                scUnidadMedidaTarifa.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkBut

                scUnidadMedidaTarifa.ToolTipExpireTime = 60

                scUnidadMedidaTarifa.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

                scUnidadMedidaTarifa.ShowToolTip()


                icDescripcionPartidaOriginal.ToolTip = $"Verificar { icDescripcionPartidaOriginal.Label} en registros de SYNAPSIS"

                icDescripcionPartidaOriginal.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkBut

                icDescripcionPartidaOriginal.ToolTipExpireTime = 60

                icDescripcionPartidaOriginal.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

                icDescripcionPartidaOriginal.ShowToolTip()


                icDescripcionPartida.ToolTip = $"Verificar {icDescripcionPartida.Label} en registros de SYNAPSIS"

                icDescripcionPartida.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkBut

                icDescripcionPartida.ToolTipExpireTime = 60

                icDescripcionPartida.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

                icDescripcionPartida.ShowToolTip()


            End If


            'If fbcCompradorDestinatario.Text IsNot Nothing Then

            '    If fbcCompradorDestinatario.Text <> "" Then

            '        If fbcCompradorDestinatario.Value = objectidZero_ Then

            '            fbcCompradorDestinatario.ToolTip = $"Verificar { fbcProducto.Label} en registros de SYNAPSIS"

            '            fbcCompradorDestinatario.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkBut

            '            fbcCompradorDestinatario.ToolTipExpireTime = 60

            '            fbcCompradorDestinatario.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

            '            fbcCompradorDestinatario.ShowToolTip()

            '        End If

            '    End If


            '    If scDomicilioCompradorDestinatario.Value = objectidZero_ Then

            '        scDomicilioCompradorDestinatario.ToolTip = $"Verificar { scDomicilioCompradorDestinatario.Label} en registros de SYNAPSIS"

            '        scDomicilioCompradorDestinatario.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkBut

            '        scDomicilioCompradorDestinatario.ToolTipExpireTime = 60

            '        scDomicilioCompradorDestinatario.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

            '        scDomicilioCompradorDestinatario.ShowToolTip()

            '    End If

            'End If

            If scUnidadMedidaComercial.Value = objectidZero_ Then

                scUnidadMedidaComercial.ToolTip = $"Verificar { scUnidadMedidaComercial.Label} en registros de SYNAPSIS"

                scUnidadMedidaComercial.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkBut

                scUnidadMedidaComercial.ToolTipExpireTime = 60

                scUnidadMedidaComercial.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

                scUnidadMedidaComercial.ShowToolTip()

            End If

        End If

    End Sub

#End Region

#Region "IA"
    Private Sub EjecutarBusquedaDeCamposClave()
        'ComprobarClienteIA()
        'ComprabarPais()
        ComprobarMoneda()
        ComprobarIncorterm()
        ComprobarProveedor()
        ComprobarProducto()
    End Sub

    Private Sub ComprobarProducto()

    End Sub

    Protected Sub ComprobarIncorterm()
        CargarIncoterm(fbcIncoterm.Text
                       )
    End Sub

    Protected Sub CargarIncoterm(ByVal claveIncoterm_ As String)

        _tagwatcher = ControladorFacturaComercial.ObtenerIncoterm(claveIncoterm_)

        If _tagwatcher.Status = TypeStatus.Ok Then

            fbcIncoterm.DataSource = Nothing

            fbcIncoterm.Value = _tagwatcher.ObjectReturned

            fbcIncoterm.Text = _tagwatcher.ObjectReturned

        Else

            _utils.AvisoVerificacionManual(New List(Of Object) From {fbcIncoterm})

        End If

    End Sub

    'Private Sub ComprabarPais()

    '    If fbcPais.Value = "" Or fbcPais.Text = "" Then

    '    Else
    '        ''buscar el país
    '        Dim listaPaises_ = _utils.ObtenerListaPaises(fbcPais.Text)

    '        If listaPaises_.Count > 0 Then

    '            fbcPais.DataSource = listaPaises_

    '            CargarMonedaPorDefault(objectIdPais_:=fbcPais.Value)
    '        Else

    '            AvisoRegistroNoEncontrado(New List(Of Object) From {fbcPais})

    '        End If

    '    End If

    'End Sub

    Protected Sub AvisoRegistroNoEncontrado(ByVal listaCampos_ As List(Of Object))
        'Dim camposFacturaUI_ As New Dictionary(Of Object, String) From {
        '    {icValorFactura, "valor factura"},
        '    {dbcNumFacturaAcuseValor, "número factura"},
        '    {icFechaFactura, "fecha factura"},
        '    {fbcCompradorReceptor, "proveedor"},
        '    {scDomicilioCompradorReceptor, "domicilio proveedor"},
        '    {fbcCliente, "cliente"},
        '    {fbcProducto, "número de parte"},
        '    {icDescripcionPartida, "descripcion producto"},
        '    {fbcPais, "país"}
        '}

        'For Each campo_ In listaCampos_
        '    If Not campo_.Label.Contains("✨") Then
        '        campo_.Label &= " ✨"
        '    End If

        '    If camposFacturaUI_.ContainsKey(campo_) Then
        '        campo_.ToolTip = $"Syn: Registrar {camposFacturaUI_(campo_)}"
        '    Else
        '        campo_.ToolTip = $"Syn: Registrar valor"
        '    End If

        '    campo_.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkInfo
        '    campo_.ToolTipExpireTime = 10000
        '    campo_.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
        '    campo_.ShowToolTip()
        'Next
    End Sub

    Private Sub ComprobarMoneda()

        If scMonedaFactura.Value <> "" Then

            If scMonedaFactura.Value = _monedaUSD Then

                _dataSourceMoneda = New List(Of SelectOption) _
                    From {New SelectOption With {.Value = _objectidmonedaUSD, .Text = _monedaUSD}}

                _utils.LLenarMonedas(_dataSourceMoneda, _listaCamposMonedas)

                For Each campo_ In _listaCamposMonedas

                    If Not campo_.Label.Contains("✨") Then

                        campo_.Label &= " ✨"

                    End If

                    campo_.ToolTip = "SYN: Verificar valor moneda"

                    campo_.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkInfo

                    campo_.ToolTipExpireTime = 10000

                    campo_.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

                    campo_.ShowToolTip()
                Next

            Else
                ''VERIFICAR SI EXISTE EL PAIS, SINO SOLO LA MONEDA QUE ES
                'If fbcPais.Value <> "" Then

                '    CargarMonedaPorDefault(objectIdPais_:=fbcPais.Value)

                'Else

                '    AvisoRegistroNoEncontrado(New List(Of Object) From {_listaCamposMonedas})

                'End If

            End If
        Else

            AvisoRegistroNoEncontrado(New List(Of Object) From {_listaCamposMonedas})

        End If

    End Sub

    Private Sub ComprobarProveedor()

        If fbcCompradorReceptor.Value = "000000000000000000000000" Or fbcCompradorReceptor.Value = "" Then

            _utils.AvisoVerificacionManual(New List(Of Object) From {fbcCompradorReceptor})

        End If

        If scDomicilioCompradorReceptor.Value = "000000000000000000000000" Or scDomicilioCompradorReceptor.Value = "" Then

            _utils.AvisoVerificacionManual(New List(Of Object) From {scDomicilioCompradorReceptor})

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

#Region "Monedas"
    Protected Sub scMonedaFactura_Click(sender As Object, e As EventArgs)

        _utils.BusquedaMonedas(sender)

    End Sub

    Protected Sub scMonedaFactura_SelectedIndexChanged(sender As Object, e As EventArgs)

        'If scMonedaFactura.Value <> "" Then
        '    MostrarNombreMoneda(scMonedaFactura.Value, scMonedaFactura)
        'End If

        _dataSourceMoneda = New List(Of SelectOption) _
                    From {New SelectOption With {.Value = scMonedaFactura.Value, .Text = scMonedaFactura.Text}}

        _utils.LLenarMonedas(_dataSourceMoneda, _listaCamposMonedas)

        'Try
        '    Dim estadoMoneda_ = _utils.ObtenerDatosMoneda(ObjectId.Parse(scMonedaFactura.Value))

        '    If estadoMoneda_.Status = TypeStatus.Ok Then

        '        Dim datosmoneda_ = estadoMoneda_.ObjectReturned

        '        MostrarMonedaCompleta(scMonedaFactura, datosmoneda_.nombremonedaesp)
        '        MostrarMonedaCompleta(scMonedaMercancia, datosmoneda_.nombremonedaesp)
        '        MostrarMonedaCompleta(scMonedaMercanciaItemPartida, datosmoneda_.nombremonedaesp)
        '        MostrarMonedaCompleta(scMonedaPrecioUnitarioPartida, datosmoneda_.nombremonedaesp)
        '        MostrarMonedaCompleta(scMonedaValorAgregadoPartida, datosmoneda_.nombremonedaesp)
        '    End If


        'Catch ex As Exception

        '    DisplayMessage("Favor de intentarlo más tarde", StatusMessage.Warning)

        'End Try

    End Sub

    Protected Sub scMonedaMercancia_Click(sender As Object, e As EventArgs)

        _utils.BusquedaMonedas(sender)

    End Sub

    Protected Sub scMonedaFacturaPartida_Click(sender As Object, e As EventArgs)

        _utils.BusquedaMonedas(sender)

    End Sub

    Protected Sub scMonedaMercanciaPartida_Click(sender As Object, e As EventArgs)

        _utils.BusquedaMonedas(sender)

    End Sub

    Protected Sub scMonedaPrecioUnitarioPartida_Click(sender As Object, e As EventArgs)

        _utils.BusquedaMonedas(sender)

    End Sub

    Protected Sub scMonedaValorDolaresPartida_Click(sender As Object, e As EventArgs)

        _utils.BusquedaMonedas(sender)

    End Sub


    Protected Sub scMonedaValorUnitarioPartida_Click(sender As Object, e As EventArgs)

        _utils.BusquedaMonedas(sender)

    End Sub

    Protected Sub CargarMonedaPorDefault(Optional ByVal moneda_ As String = "USD",
                                         Optional ByVal objectIdPais_ As String = Nothing)

        _utils.LimpiarMonedas(_listaCamposMonedas)

        'If moneda_ = _monedaUSD And fbcPais.Value = "" Then

        '    _dataSourceMoneda = New List(Of SelectOption) From {New SelectOption _
        '        With {.Value = _objectidmonedaUSD, .Text = moneda_}}

        'End If

        If moneda_ = _monedaUSD Then

            _dataSourceMoneda = New List(Of SelectOption) From {New SelectOption _
                With {.Value = _objectidmonedaUSD, .Text = moneda_}}

        End If

        If icTipoCargaDatos.Value = "1" Then

            If moneda_ <> _monedaUSD Then

                _resultMoneda = _utils.ObtenerListaMonedas(moneda_)

                If _resultMoneda(0) IsNot Nothing Then

                    _dataSourceMoneda = New List(Of SelectOption) _
                        From {New SelectOption With {.Value = _resultMoneda(0)._id.ToString,
                                                    .Text = _resultMoneda(0).aliasmoneda(0).Valor}}
                Else

                    _dataSourceMoneda = New List(Of SelectOption) _
                        From {New SelectOption With {.Value = "", .Text = moneda_}}

                    _utils.AvisoVerificacionManual(New List(Of Object) From {scMonedaFactura, scMonedaMercancia})

                End If

            End If

        Else

            If objectIdPais_ <> Nothing Then

                _resultadoMonedaPais = _utils.ObtenerMonedasPorPais(objectIdPais_)

                _dataSourceMoneda = New List(Of SelectOption) _
                    From {New SelectOption With {.Value = _resultadoMonedaPais(0)._idmoneda.ToString, .Text = _resultadoMonedaPais(0).claveISO}}

                'Try
                '    Dim estadoMoneda_ = _utils.ObtenerDatosMoneda(_resultadoMonedaPais(0)._idmoneda)

                '    If estadoMoneda_.Status = TypeStatus.Ok Then

                '        Dim datosmoneda_ = estadoMoneda_.ObjectReturned

                '        MostrarMonedaCompleta(scMonedaFactura, datosmoneda_.nombremonedaesp)
                '        MostrarMonedaCompleta(scMonedaMercancia, datosmoneda_.nombremonedaesp)
                '        MostrarMonedaCompleta(scMonedaMercanciaItemPartida, datosmoneda_.nombremonedaesp)
                '        MostrarMonedaCompleta(scMonedaPrecioUnitarioPartida, datosmoneda_.nombremonedaesp)
                '        MostrarMonedaCompleta(scMonedaValorAgregadoPartida, datosmoneda_.nombremonedaesp)
                '    End If

                'Catch ex As Exception

                '    DisplayMessage("Favor de intentarlo más tarde", StatusMessage.Warning)

                'End Try

            End If

        End If

        _utils.LLenarMonedas(_dataSourceMoneda, _listaCamposMonedas)
    End Sub


#End Region
#Region "Método valoración"

    Protected Sub MetodoValoracionInicial()

        Dim listaMetodoValoracion_ As List(Of SelectOption) = New List(Of SelectOption) _
                                                            From {New SelectOption With {.Value = 1, .Text = "0 - VALOR COMERCIAL (EXP)."}}

        With scMetodoValoracion

            .DataSource = listaMetodoValoracion_

            .Value = 1
        End With

        With scMetodoValoracionPartida

            .DataSource = listaMetodoValoracion_

            .Value = 1

        End With

    End Sub

#End Region

#Region "ComprobarValoresFacturacomercial"
    'La suma del "valor factura" de los ítems, no debe rebasar el valor factura del encabezado, o no debe rebasar en más de 2 por aquello de los decimales,
    'El "valor mercancía" de los ítems deben ser menor o igual al valor factura del ítem, 
    'El "valor mercancía" del encabezado debe ser menor o igual al valor factura del encabezado.

    Protected Function ComprobarValorMercanciaVSValorFactura() As Boolean
        Dim valorMercancia_ As Decimal
        Dim valorFactura_ As Decimal

        Decimal.TryParse(icValorMercancia.Value, NumberStyles.Number, CultureInfo.InvariantCulture, valorMercancia_)
        Decimal.TryParse(icValorFactura.Value, NumberStyles.Number, CultureInfo.InvariantCulture, valorFactura_)

        Return valorMercancia_ <= valorFactura_
    End Function

    Protected Function ComprobarSumaValorMercanciaItemsVSValorMercanciaGeneral() As Boolean
        Dim totalValorMercanciaItems_ As Decimal = 0

        For Each nodo_ In pbPartidasItems.DataSource
            For Each item_ In nodo_
                If item_.Key = "icValorMercanciaItem" Then
                    Dim valor_ As Decimal
                    If Decimal.TryParse(item_.Value,
NumberStyles.Number,
                                    CultureInfo.InvariantCulture,
                                    valor_) Then
                        totalValorMercanciaItems_ += valor_
                    End If
                End If
            Next
        Next

        Dim valorGeneral_ As Decimal
        Decimal.TryParse(icValorMercancia.Value, NumberStyles.Number, CultureInfo.InvariantCulture, valorGeneral_)

        Return totalValorMercanciaItems_ <= valorGeneral_
    End Function


    'Protected Function ComprobarValorMercanciaVSValorFactura() As Boolean

    '    If Decimal.Parse(icValorMercancia.Value) <= Decimal.Parse(icValorFactura.Value) Then

    '        Return True

    '    End If

    '    Return False

    'End Function

    'Protected Function ComprobarSumaValorMercanciaItemsVSValorMercanciaGeneral() As Boolean

    '    ''OBTENER LA SUMA DE TODOS LOS ITEMS DEL PILLBOX

    '    Dim totalValorMercanciaItems_ = 0

    '    For Each nodo_ In pbPartidasItems.DataSource

    '        ''NO GUARDA EL OBJECTID del producto porque pillbox no reconoce los inputs ocultos
    '        For Each item_ In nodo_

    '            If item_.Key = "icValorMercanciaItem" Then

    '                totalValorMercanciaItems_ += Decimal.Parse(item_.Value)

    '            End If

    '        Next
    '    Next

    '    If totalValorMercanciaItems_ <= Decimal.Parse(icValorMercancia.Value) Then

    '        Return True

    '    End If

    '    Return False

    'End Function

#End Region
#Region "TOOLTIPS - AVISOS"
    Protected Sub MostrarDescripciones(ByVal texto_ As String)
        icFraccionNico.ToolTip = texto_
        icFraccionNico.ToolTipStatus = IUIControl.ToolTipTypeStatus.Ok
        icFraccionArancelaria.ToolTipExpireTime = 6
        icFraccionNico.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
        icFraccionNico.ShowToolTip()
    End Sub

    Protected Sub AvisoFraccion(ByVal aviso_ As String)
        icFraccionArancelaria.ToolTip = "Estatus clasificación: " & aviso_
        icFraccionArancelaria.ToolTipExpireTime = 6
        icFraccionArancelaria.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
        icFraccionArancelaria.ShowToolTip()
    End Sub

    Protected Sub MsgValidacionRazonsocial()
        With fbcCompradorReceptor
            .ToolTip = "👉 Indique una razón social"
            .ToolTipExpireTime = 6
            .ToolTipStatus = IUIControl.ToolTipTypeStatus.OkInfo
            .ToolTipModality = IUIControl.ToolTipModalities.Ondemand
            .ShowToolTip()
        End With
    End Sub

    'Protected Sub MsgAltaDestinatario()
    '    With fbcCompradorDestinatario
    '        .ToolTip = "📣 Debe dar de alta el destinatario"
    '        .ToolTipExpireTime = 6
    '        .ToolTipStatus = IUIControl.ToolTipTypeStatus.OkInfo
    '        .ToolTipModality = IUIControl.ToolTipModalities.Ondemand
    '        .ShowToolTip()
    '    End With
    'End Sub

    Protected Sub MsgErrorNumeroParte()
        With fbcProducto
            .ToolTip = "👉 Indique número de parte"
            .ToolTipExpireTime = 6
            .ToolTipStatus = IUIControl.ToolTipTypeStatus.OkInfo
            .ToolTipModality = IUIControl.ToolTipModalities.Ondemand
            .ShowToolTip()
        End With
    End Sub
    Protected Sub MsgErrorCliente()

        With fbcCliente
            .ToolTip = "Selecciona un cliente"
            .ToolTipExpireTime = 6
            .ToolTipStatus = IUIControl.ToolTipTypeStatus.OkBut
            .ToolTipModality = IUIControl.ToolTipModalities.Ondemand
            .ShowToolTip()
        End With

    End Sub



#End Region

#Region "Validaciones"


    Protected Function EstructuraCommercialInvoiceTemporal() As CommercialInvoiceAnalysis

        Dim factura_ As CommercialInvoiceAnalysis = New CommercialInvoiceAnalysis

        With factura_
            .invoicenumber = "FACT02"
            .invoicedate = "2026-05-08"
            .invoiceseries = ""
            .customername = "KOSTAL Mexicana S.A. de C.V."
            .suppliername = "Kostal (Shanghai) Management Co., Ltd."
            .invoicecountry = "CHN"
            .totalinvoice = 88480
            .invoicecurrency = "USD"
            .customer = New Customer With {
                .customerid = 0, ''ESTOS DATOS YO CREO QUE HARE LA BUSQUEDA DESDE ANTES PORQUE ESTO SE VA A SELECCIONAR ANTES
                .customername = "KOSTAL Mexicana S.A. de C.V.",
                .rfc = "CTM990421TL98",
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
                .supliername = "Kostal (Shanghai) Management Co., Ltd.",
                .taxid = "913100005665894247",
                .address = "ROOM 1001, 3.0RD FLOOR, INCUBATION BUILDING, HAINAN ECO-SOFTWARE PARK, LAOCHENG TOWN, CHENGMAI COUNTY, HAINAN PROVINCE, 571900 CHINA",
                .street = "ROOM 1001, 3RD FLOOR, INCUBATION BUILDING, HAINAN ECO-SOFTWARE PARK,",
                .externalnumber = "",
                .internalnumber = "",
                .zipcode = "571900",
                .locality = "",
                .city = "CHENGMAI COUNTY",
                .state = "HAINAN PROVINCE",
                .country = "CHN"}
            .items = New List(Of Item) _
                From {New Item With {
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
        New Item With {
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

    Protected Function SubdivisionFacturaDummy() As SubdivisionFacturaComercial

        Dim facturaSubdividida_ As SubdivisionFacturaComercial = New SubdivisionFacturaComercial

        Dim detalleUser_ As New DetalleUser

        With detalleUser_
            .fecha = Date.Parse("2026-05-20T16:34:38.443Z")
            .user_id = ObjectId.Parse("678Ff337d54fb3f46d0cec72")
            .user = "rosario.ramirez@kromaduanal.com"
        End With

        Dim itemSub_ As New ItemSubdividido

        With itemSub_
            .sec = 1
            .id_producto = ObjectId.Parse("69e6a6a0792afe841c493fd8")
            .numero_partida = 1
            .numero_parte = "7615108090"
            .numeropartecompleto = "144 | 7615108090 | B6890682 FP28 B P-CHEF GLOSSY SHINE PEAR | SARTEN (SARTEN)"
            .cantidad_comercial_requerida = 50
            .unidad_comercial = "1 - KILO"
            .descripcion_partida = "JUEGO DE SARTENES"
            .precio_unitario = 1.5
            .moneda_precio_unitario = "USD"
            .valor_mercancia = 75
            .moneda_mercancia = "USD"
            .cantidad_tarifa_requerida = 50
            .unidad_medida_tarifa = "1 - KILO"
            .estado = 1
            .cve_unidad_medida_comercial = "1"
            .idmoneda_valor_mercancia_original = "635acf25a8210bfa0d58434e"
            .idmoneda_precio_unitario_original = "635acf25a8210bfa0d58434e"
            .cve_unidad_medida_tarifa = "1"
            .descripcion_merca_original = "SET FP24/30 EASY COOK'N CLEAN"
            .descripcion_merca_cove = "SET FP24/30 EASY COOK'N CLEAN"
            .idmoneda_val_fact_partida = "635acf25a8210bfa0d58434e"
            .moneda_val_fact_partida = "USD"
            .peso_neto_partida = 12
            .pais_origen = "USA - ESTADOS UNIDOS DE AMÉRICA"
            .id_pais_origen = "635acf26a8210bfa0d58434f"
            .cve_metodo_val_partida = "1"
            .metodo_val_partida = "1 - TRANSACCIÓN DE LAS MERCANCIAS."
            .orden_compra_partida = "ORF"
            .fraccion = "76151002"
            .fraccion_descripcion = "76151002 - ARTÍCULOS DE USO DOMÉSTICO Y SUS PARTES; ESPONJAS, ESTROPAJOS, GUANTES Y ARTÍCULOS SIMILARES PARA FREGAR, LUSTRAR O USOS..."
            .nico = "02"
            .nico_descripcion = "02 - OLLAS, SARTENES Y BATERÍAS DE ALUMINIO...."
            .lote_part = "LOTE"
            .numero_serie_part = "SERIE"
            .marca_part = "MARCA"
            .modelo_part = "MODELO"
            .submodelo_part = "SUBMODELO"
            .kilometraje_part = "12"
            .timestamp_part = "1776723615"
            .val_fact_partida = 75
            .identity = 2
        End With

        With facturaSubdividida_
            .id = ObjectId.Parse("6a0de29e8a3c62a391e19e4f")
            .id_fact_sub = ObjectId.Parse("6a0dd5104913c8b54493320b")
            .sec = 2
            .numerofactura_subdivision = "FACTURA-SUBDIVISION-New-SUB002"
            .numerofactura_original = "FACTURA-SUBDIVISION-New"
            .creacion = "2026-05-20T16:34:38.443Z"
            .actualizacion = "2026-05-20T16:34:56.452Z"
            .cantidad_unidad_comercial_total = 50
            .unidad_medida_comercial_total = "1 - KILO"
            .valor_mercancia_total = 75
            .moneda_valor_mercancia_total = "USD"
            .generado_por = detalleUser_
            .publicado = True
            .url_subdivision = "https://synapsis#"
            .items = New List(Of ItemSubdividido) From {itemSub_}
            .idfacturaoriginal = ObjectId.Parse("6a0dd5064913c8b54493320a")
            .valorfactura_general = 75
            .cve_moneda_valorfactura = "635acf25a8210bfa0d58434e"
            .cve_moneda_mercancia = "635acf25a8210bfa0d58434e"

        End With

        Return facturaSubdividida_

    End Function

    Protected Sub CerrarIntegrador_Click(sender As Object, e As EventArgs)

        PanelProcesamiento.Visible = False

        fcCFDI.Value = Nothing

        'Dim environmentid_ As Int32 = ListaEmpresas.Value

        '_controladorFacturaComercial = New ControladorFacturaComercial(IControladorFacturaComercial.TipoOperaciones.Importacion, environmentid_)

        'Dim facturaSubdividida_ As SubdivisionFacturaComercial = SubdivisionFacturaDummy()

        '_loginUsuario = Session("DatosUsuario")

        'Dim objectidCustom_ As ObjectId = ObjectId.Parse("6a0dd5104913c8b54493320b")

        'Dim objectIdOriginal_ As ObjectId = ObjectId.Parse("6a0dd5064913c8b54493320a")

        'Dim estado_ = _controladorFacturaComercial.GenerarFacturaComercialDesdeSubdivision(facturaSubdividida_,
        '                                                                                   objectIdOriginal_, objectidCustom_, _loginUsuario("Nombre"))

        'Dim aqui_ = estado_


        'DisplayMessage("Factura subdividible generada ok", StatusMessage.Success)

        '

        'System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: objectid: {objectidCustom_}")

        'Dim listfacturas_ = New List(Of ObjectId) From {ObjectId.Parse("69fd1fb2b2026cb04f93c718"), ObjectId.Parse("69fccc70460ee38a4ac60a7b"), ObjectId.Parse("69fba278c72ac5c161d63167")}

        '' Dim tag_ As TagWatcher = _controladorFacturaComercial.ObtenerFacturasComercialesPublicadasParaPedimento(listfacturas_)

        ''Dim tag_ As TagWatcher = _controladorFacturaComercial.ListaFacturasProveedorParaPedimento(ObjectId.Parse("699f6fdc365b4c26e03ab823"), ObjectId.Parse("699f552313794e533d63abd7"))

        ''Dim tag_ As TagWatcher = _controladorFacturaComercial.GenerarFacturaComercialDesdeComercialInvoiceAnalizer(comercialinvoiceAnalizer_:=EstructuraCommercialInvoiceTemporal(),
        ''                                                                                                           userGenero_:="rosario.ramirez@kromaduanal.com",
        ''                                                                                                           idCustomOperGenerica_:=objectidCustom_)

        'Dim tag_ As TagWatcher = _controladorFacturaComercial.ObtenerFacturasComercialesSinVincularPedimento(listfacturas_)
        'Dim aqio_ = tag_

        'Dim environment_ As Int32 = ListaEmpresas.Value

        '_icontroladorFactura = New ControladorFacturaComercial(IControladorFacturaComercial.TipoOperaciones.Importacion, environment_)

        '_icontroladorFactura.EnvironmentOnline = ListaEmpresas.Value

        'Dim factura_ As String = "VENTO"

        'Dim FACT_ As ObjectId = ObjectId.Parse("69fd1fb2b2026cb04f93c718")

        'Dim tagwatcher_ As TagWatcher = _controladorFacturaComercial.ListaCamposFacturaComercial(FACT_, New Dictionary(Of [Enum], List(Of [Enum])) _
        '                From {{SeccionesFacturaComercial.SFAC1, New List(Of [Enum]) From {CamposFacturaComercial.CA_NUMERO_FACTURA,
        '                                                                                  CamposFacturaComercial.CA_FECHA_FACTURA,
        '                                                                                  CamposFacturaComercial.CP_TIPO_OPERACION,
        '                                                                                  CamposClientes.CP_OBJECTID_CLIENTE,
        '                                                                                  CamposClientes.CA_RAZON_SOCIAL,
        '                                                                                  CamposClientes.CA_TAX_ID,
        '                                                                                  CamposClientes.CA_RFC_CLIENTE,
        '                                                                                  CamposFacturaComercial.CA_MONEDA_FACTURACION,
        '                                                                                  CamposFacturaComercial.CP_APLICA_ENAJENACION,
        '                                                                                  CamposFacturaComercial.CA_APLICA_SUBDIVISION
        '                        }},
        '                        {SeccionesFacturaComercial.SFAC2, New List(Of [Enum]) From {CamposProveedorOperativo.CP_ID_PROVEEDOR,
        '                                                                                    CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR,
        '                                                                                    CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR,
        '                                                                                    CamposProveedorOperativo.CA_RFC_PROVEEDOR
        '                        }},
        '                        {SeccionesFacturaComercial.SFAC4, Nothing}})


        'Dim aca_ = tagwatcher_

        'Dim proveedor_ = ObjectId.Parse("699f6fdc365b4c26e03ab823")
        '''Dim cliente_ = ObjectId.Parse("699f552313794e533d63abd7")
        ''''Dim cliente2_ = ObjectId.Parse("698611f18c0d1239d20bb9fc")
        ''''Dim proveedor2_ = ObjectId.Parse("68ffb59cc759512c75155425")
        ''''Dim cliente3_ = ObjectId.Parse("698b8bf99f03981e854e4729")
        ''''Dim proveedor3_ = ObjectId.Parse("699338368d5904f20c047fe2")

        'Dim pedimento = ObjectId.Parse("69c2d1e80f3e6fa16dfb7d6a")



        'Dim pedimento = ObjectId.Parse("000000000000000000000000")


        ''''Dim pedimento2 = ObjectId.Parse("698cdb83b20c8ee96757b3e3")
        ''''Dim pedimento3 = ObjectId.Parse("697cdbe00e79102af104aaf6")

        '''''EXPORTACION

        ''

        ''Dim factura4_ = ObjectId.Parse("698cdd746616ef4452c2645e")
        ''Dim factura5_ = ObjectId.Parse("69949d6b8178a5a1c7c4b344")
        'Dim factura_ = ObjectId.Parse("69c2e151d419b1a749905d37")


        '''IMPORTACION
        'Dim factura_ = ObjectId.Parse("69e94e58786dde2dd8161acd")
        'Dim factura2_ = ObjectId.Parse("69e9147f786dde2dd8161aca")
        'Dim factura3_ = ObjectId.Parse("69e92395786dde2dd8161acb")
        'Dim listfacturas_ = New List(Of ObjectId) From {factura_, factura2_, factura3_}
        'Dim pedimento_ = ObjectId.Parse("69eba09e6652f49b26fe68d2")


        '_controladorFacturaComercial = New ControladorFacturaComercial(IControladorFacturaComercial.TipoOperaciones.Exportacion)

        '_controladorFacturaComercial.EnvironmentOnline = ListaEmpresas.Value

        'Dim aqui_ As TagWatcher = _controladorFacturaComercial.ListaFacturasProveedorParaPedimento(ObjectId.Parse("69f92e35306ab05ce1bb1390"),
        '                                                                                           ObjectId.Parse("69fa098a6631a21f6f197d7e"))

        'Dim aca_ = aqui_




        ''Dim tag_ As TagWatcher = _controladorFacturaComercial.AsociarFacturasPedimento(listfacturas_, pedimento_)
        'Dim tag_ As TagWatcher = _controladorFacturaComercial.DesasociarFacturasPedimento(listfacturas_, pedimento_)
        'Dim aqui = tag_
        ' listasFacturas_.Add(factura2_)


        ''Dim fecha_ As Date = Date.Parse("2026-02-23")

        ''tagwatcher_ = _controladorFacturaComercial.ListarIncrementables(listasFacturas_, fecha_)
        ''tagwatcher_ = _controladorFacturaComercial.BuscarAcuseValor(factura_)
        ''tagwatcher_ = _controladorFacturaComercial.ListaFacturasProveedor(proveedor_, cliente_)

        '_controladorFacturaComercial = New ControladorFacturaComercial()

        '_loginUsuario = Session("DatosUsuario")

        'Dim tagwatcher_ As TagWatcher = _controladorFacturaComercial.GenerarFacturaComercialDesdeComercialInvoiceAnalizer(EstructuraCommercialInvoiceTemporal(),
        '                                                                                                                  _loginUsuario("Nombre"))
        'If tagwatcher_.Status = TypeStatus.Ok Then

        '    DisplayMessage($"Factura con IA generada correctamente")

        'Else

        '    DisplayMessage("No se ha podido generar factura con IA", StatusMessage.Fail)

        'End If

        'Dim factura_ = ObjectId.Parse("69b42eff244727f4cd3caa95")
        'Dim factura2_ = ObjectId.Parse("69c2c29cf655679d0b8e7bb3")

        'Dim listasFacturas_ As List(Of ObjectId) = New List(Of ObjectId)

        'listasFacturas_.Add(factura_)
        'listasFacturas_.Add(factura2_)
        '_controladorFacturaComercial = New ControladorFacturaComercial()

        'Dim asociar As TagWatcher = _controladorFacturaComercial.AsociarFacturasPedimento(listasFacturas_, pedimento)

        'If asociar.Status = TypeStatus.Ok Then

        '    Dim response_ = asociar.ObjectReturned

        '    System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: Pedimento asociado correctamente")

        '    DisplayMessage($"Pedimento asociado correctamente")

        'ElseIf asociar.Status = TypeStatus.OkInfo Then

        '    Dim response_ = DirectCast(asociar.ObjectReturned, List(Of Response))

        '    For Each item_ In response_

        '        If item_.RecursoAdicional IsNot Nothing Then

        '            System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: {item_.Mensaje}. Pedimento diferente {item_.RecursoAdicional._idPedimentoAsociado}")
        '            DisplayMessage($"{item_.Mensaje}. Pedimento {item_.RecursoAdicional._idPedimentoAsociado}", StatusMessage.Info)
        '        Else

        '            System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: {item_.Mensaje}")
        '            DisplayMessage($"{item_.Mensaje}", StatusMessage.Info)
        '        End If

        '    Next

        'Else

        '    System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: {asociar.ObjectReturned}")

        '    DisplayMessage($"{asociar.ObjectReturned}", StatusMessage.Warning)

        'End If




        'Dim tagwatcher_ As New TagWatcher

        'Dim factura_ = ObjectId.Parse("69b42eff244727f4cd3caa95")
        'Dim factura2_ = ObjectId.Parse("69c2c29cf655679d0b8e7bb3")

        'Dim listasFacturas_ As List(Of ObjectId) = New List(Of ObjectId)

        'listasFacturas_.Add(factura_)
        'listasFacturas_.Add(factura2_)
        '_controladorFacturaComercial = New ControladorFacturaComercial()

        'Dim desasociar As TagWatcher = _controladorFacturaComercial.DesasociarFacturasPedimento(listasFacturas_, pedimento)

        'If desasociar.Status = TypeStatus.Ok Then

        '    System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: {desasociar.ObjectReturned}")

        '    DisplayMessage($"{desasociar.ObjectReturned}", StatusMessage.Info)

        'ElseIf desasociar.Status = TypeStatus.OkInfo Then

        '    Dim response_ = DirectCast(desasociar.ObjectReturned, List(Of Response))

        '    For Each item_ In response_

        '        If item_.RecursoAdicional IsNot Nothing Then

        '            System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: {item_.Mensaje}. Pedimento {item_.RecursoAdicional._idPedimentoAsociado}")
        '            DisplayMessage($"{item_.Mensaje}. Pedimento {item_.RecursoAdicional._idPedimentoAsociado}", StatusMessage.Info)
        '        Else

        '            System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: {item_.Mensaje}")
        '            DisplayMessage($"{item_.Mensaje}", StatusMessage.Info)
        '        End If

        '    Next



        'Else

        '    System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: {desasociar.ObjectReturned}")

        '    DisplayMessage($"{desasociar.ObjectReturned}", StatusMessage.Warning)

        'End If



        'tagwatcher_ = _controladorFacturaComercial.ObtenerFacturasComercialesPublicadasParaPedimento(listasFacturas_)

        'Dim aqui_ = tagwatcher_

        'Dim tagwatchera_ As TagWatcher = _controladorFacturaComercial.ListaFacturasProveedorParaPedimento(ObjectId.Parse("69c2ddfb8d1ceaac2fd936a9"), ObjectId.Parse("69b18d2ab732e28267037289"))


        'Dim aqaa_ = tagwatchera_
        ''tagwatcher_ = _controladorFacturaComercial.ObtenerFacturasComercialesSinVincularPedimento(listasFacturas_)


        'If tagwatcher_.Status = TypeStatus.Ok Then

        '    '    Dim auxAcuse_ = DirectCast(tagwatcher_.ObjectReturned, DatosAcuseValor)

        '    Dim result_ = tagwatcher_.ObjectReturned

        '    DisplayMessage($"Resultado correcto: - {result_}")

        'Else

        '    Dim resultNegative_ = tagwatcher_.ObjectReturned

        '    DisplayMessage($"Atención!!: {tagwatcher_.LastMessage}", StatusMessage.Warning)

        '    '    Dim acuseValor_ = New DatosAcuseValor

        '    '    acuseValor_ = DirectCast(tagwatcher_.ObjectReturned, DatosAcuseValor)

        '    '    Dim aqui = acuseValor_._acuseValor

        '    '    DisplayMessage("Ya se hizo lo que estabas haciendo hije")
        'End If

        ''Dim aqui_ = tagwatcher_

    End Sub

    Protected Sub scMonedaValorAgregadoPartida_Click(sender As Object, e As EventArgs)

        _utils.BusquedaMonedas(sender)

    End Sub

    Protected Sub scMonedaMercanciaItemPartida_Click(sender As Object, e As EventArgs)
        _utils.BusquedaMonedas(sender)
    End Sub

    Protected Sub scMonedaMercancia_SelectedIndexChanged(sender As Object, e As EventArgs)
        'If scMonedaMercancia.Value <> "" Then
        '    MostrarNombreMoneda(scMonedaMercancia.Value, scMonedaMercancia)
        'End If
    End Sub

    Protected Sub MostrarMonedaCompleta(ByVal campoMoneda_ As Object, ByVal moneda_ As String)

        campoMoneda_.ToolTip = moneda_
        campoMoneda_.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkInfo
        campoMoneda_.ToolTipModality = IUIControl.ToolTipModalities.Classic
        campoMoneda_.ShowToolTip()

    End Sub

    'Protected Sub MostrarNombreMoneda(ByVal objectidMoneda As String, ByVal object_ As Object)

    '    ''VAMOS A CACHEARLO POR SI SIEMPRE ES LA MISMA PARA NO ESTAR HACIENDO HITS A LO BRUTO :V

    '    Dim cacheKey_ = "cacheNombreMonedaCompleto"

    '    Dim cacheData_ = CType(HttpRuntime.Cache(cacheKey_), Dictionary(Of String, String))

    '    ' Si no hay cache O si el objectId cambió, ir a Mongo
    '    If cacheData_ Is Nothing OrElse cacheData_("objectId") <> objectidMoneda.ToString() Then

    '        _utils = New UtilsFacturaComercial

    '        Dim estadoMoneda_ = _utils.ObtenerDatosMoneda(ObjectId.Parse(objectidMoneda))

    '        If estadoMoneda_.Status = TypeStatus.Ok Then

    '            Dim datosmoneda_ = DirectCast(estadoMoneda_.ObjectReturned, MonedaGlobal)

    '            Dim nuevoCacheData_ = New Dictionary(Of String, String) From {
    '                {"objectId", objectidMoneda.ToString()},
    '                {"nombreMoneda", datosmoneda_.nombremonedaesp}
    '            }

    '            HttpRuntime.Cache.Insert(cacheKey_, nuevoCacheData_, Nothing,
    '                                     DateTime.Now.AddMinutes(5),
    '                                     Caching.Cache.NoSlidingExpiration)

    '            MostrarMonedaCompleta(object_, datosmoneda_.nombremonedaesp)

    '        End If

    '    Else

    '        Dim nombreMoneda_ As String = cacheData_("nombreMoneda")

    '        ' Dim objectId_ As String = cacheData_("objectId")

    '        MostrarMonedaCompleta(object_, nombreMoneda_)

    '    End If

    'End Sub

    Protected Sub MostrarNombreMoneda(ByVal objectidMoneda As String, ByVal object_ As Object)
        Dim claveCache_ As String = $"cacheNombreMoneda_{objectidMoneda}"
        Dim nombreMoneda_ As String = CType(HttpRuntime.Cache(claveCache_), String)

        If nombreMoneda_ Is Nothing Then
            _utils = New UtilsFacturaComercial
            Dim estadoMoneda_ = _utils.ObtenerDatosMoneda(ObjectId.Parse(objectidMoneda))
            If estadoMoneda_.Status = TypeStatus.Ok Then
                Dim datosmoneda_ = DirectCast(estadoMoneda_.ObjectReturned, MonedaGlobal)
                nombreMoneda_ = datosmoneda_.nombremonedaesp
                HttpRuntime.Cache.Insert(claveCache_, nombreMoneda_, Nothing,
                                     DateTime.Now.AddMinutes(5),
                                     Caching.Cache.NoSlidingExpiration)
            End If
        End If

        If nombreMoneda_ IsNot Nothing Then
            MostrarMonedaCompleta(object_, nombreMoneda_)
        End If
    End Sub

    Protected Sub scMonedaMercanciaItemPartida_SelectedIndexChanged(sender As Object, e As EventArgs)
        'If scMonedaMercanciaItemPartida.Value <> "" Then
        '    MostrarNombreMoneda(scMonedaMercanciaItemPartida.Value, scMonedaMercanciaItemPartida)
        'End If
    End Sub

    Protected Sub scMonedaPrecioUnitarioPartida_SelectedIndexChanged(sender As Object, e As EventArgs)
        'If scMonedaPrecioUnitarioPartida.Value <> "" Then
        '    MostrarNombreMoneda(scMonedaPrecioUnitarioPartida.Value, scMonedaPrecioUnitarioPartida)
        'End If
    End Sub

    Protected Sub scMonedaValorAgregadoPartida_SelectedIndexChanged(sender As Object, e As EventArgs)
        'If scMonedaValorAgregadoPartida.Value <> "" Then
        '    MostrarNombreMoneda(scMonedaValorAgregadoPartida.Value, scMonedaValorAgregadoPartida)
        'End If
    End Sub

    Protected Sub fbcCompradorReceptor_ClickClose(sender As Object, e As EventArgs)

        fbcCompradorReceptor.Value = Nothing

        fbcCompradorReceptor.DataSource = Nothing

        scDomicilioCompradorReceptor.Value = Nothing

        scVinculacion.Value = Nothing

        ''LLENAR DE NUEVO LA VINCULACION

        scVinculacion.DataSource = _utils.Vinculacion()

        scVinculacion.Value = 0

        scMetodoValoracion.Value = Nothing

        scMetodoValoracionPartida.Value = Nothing

        ''PONER DE NUEVO EL METODO DE VALORACION 0

        MetodoValoracionInicial()

        'If fbcCompradorDestinatario.Value <> "" Then

        '    swcEsDestinatario.Checked = True

        '    SetVars("EsDestinatario_", True)

        'End If

        SetVars("listaProveedoresOperativos_", Nothing)

        SetVars("ProveedorSeleccionado_", Nothing)

        SetVars("DomicilioProveedorSeleccionado_", Nothing)

    End Sub

    'Protected Sub fbcCompradorDestinatario_ClickClose(sender As Object, e As EventArgs)

    '    scDomicilioCompradorDestinatario.Value = Nothing

    '    scDomicilioCompradorDestinatario.DataSource = Nothing

    '    fbcCompradorDestinatario.Value = Nothing

    '    fbcCompradorDestinatario.DataSource = Nothing

    '    SetVars("listaDestinatarios_", Nothing)

    '    SetVars("DestinatarioSeleccionado_", Nothing)

    '    SetVars("DomicilioDestinatarioSeleccionado_", Nothing)

    'End Sub

    Protected Sub fbcProducto_ClickClose(sender As Object, e As EventArgs)
        fbcProducto.Value = Nothing
        fbcProducto.DataSource = Nothing
        icObjectIdProducto.Value = Nothing
        icFraccionArancelaria.Value = Nothing
        icFraccionNico.Value = Nothing
        scUnidadMedidaTarifa.Value = Nothing
        icDescripcionPartidaOriginal.Value = Nothing
        icDescripcionPartida.Value = Nothing
        icDescripcionCOVE.Value = Nothing

        SetVars("ListaAuxliarProductos_", Nothing)
    End Sub


    Protected Sub ntPublicarFacturaComercial_Click(sender As Object, e As EventArgs)

        ntPublicarFacturaComercial.Visible = False

        DisplayMessage("Factura no publicada", StatusMessage.Info)

    End Sub


    Protected Sub ntPublicarFacturaComercial_ClickTwo(sender As Object, e As EventArgs)

        ntPublicarFacturaComercial.Visible = False

        If VerificarObjectIdValidoRegistros() Then

            If ComprobarSumaValorMercanciaItemsVSValorMercanciaGeneral() Then

                If ComprobarValorMercanciaVSValorFactura() Then

                    Dim estatus_ = FirmarDocumentoPublicar()

                    If estatus_.Result.MessagesList.Count > 1 Then

                        If estatus_.Result.Status = TypeStatus.OkInfo Then

                            DisplayMessage("✨ Factura comercial exportación publicada exitosamente", StatusMessage.Success)

                            PreparaBotonera(FormControl.ButtonbarModality.Protected)

                            OperacionGenerica.FirmaElectronica = estatus_.Result.ObjectReturned

                            OperacionGenerica.Publicado = True

                            SetVars("ActivaControles", False) : ActivaControles(GetVars("ActivaControles"))

                            DespuesPublicacionExitosa(estatus_.Result)

                            If icTipoCargaDatos.Value = "1" Then

                                lbModoCapturaIAEditar.Visible = False

                                lbModoCapturaManual.Visible = False

                                lbModoCapturaManualNuevo.Visible = False

                                lbModoCapturaIA.Visible = True

                            Else

                                lbModoCapturaManualNuevo.Visible = False

                                lbModoCapturaIA.Visible = False

                                lbModoCapturaIAEditar.Visible = False

                                lbModoCapturaManual.Visible = True

                            End If

                            PreparaBotonera(FormControl.ButtonbarModality.Protected)

                            PreparaTarjetero(PillboxControl.ToolbarModality.Simple, pbPartidasItems)
                        Else

                            DisplayMessage("No fue posible publicar la factura, revise la información", StatusMessage.Fail)

                        End If


                    End If

                Else

                    MensajeValorMercanciaMayor()

                    DisplayMessage($"{icValorMercancia.Label} supera el valor de {icValorFactura.Label}", StatusMessage.Fail)

                End If

            Else

                MensajeValorMercanciaItemMayor()

                DisplayMessage($"La suma total de {icValorMercanciaItem.Label} supera el valor de {icValorMercancia.Label}", StatusMessage.Fail)

            End If


        Else

            AvisosVerificacionObjectIdValido()

            DisplayMessage("No fue posible publicar la factura, revise la información", StatusMessage.Fail)

        End If

    End Sub

    Protected Sub dbcNumFacturaAcuseValor_TextChanged(sender As Object, e As EventArgs)

        dbcNumFacturaAcuseValor.Value = dbcNumFacturaAcuseValor.Value.ToUpper

    End Sub


    Protected Sub fbcCliente_ClickClose(sender As Object, e As EventArgs)

        SetVars("_datosCliente", Nothing)

    End Sub

    Protected Sub icCantidadComercial_TextChanged(sender As Object, e As EventArgs)
        '  DisplayMessage("Hiciste click")
    End Sub

    Protected Sub fbcCompradorReceptor_ClickSearch(sender As Object, e As EventArgs)

        If fbcCompradorReceptor.Value = "" OrElse fbcCompradorReceptor.Value = "000000000000000000000000" Then

            BuscarProveedor()

        End If

    End Sub

    'Protected Sub fbcPais_ClickClose(sender As Object, e As EventArgs)

    '    fbcPais.Value = Nothing

    '    fbcPais.DataSource = Nothing

    '    For Each campoMoneda_ In _listaCamposMonedas

    '        campoMoneda_.Value = Nothing

    '        campoMoneda_.DataSource = Nothing

    '    Next

    'End Sub

    'Protected Sub fbcPais_Click(sender As Object, e As EventArgs)

    '    For Each campoMoneda_ In _listaCamposMonedas

    '        MostrarNombreMoneda(fbcPais.Value, campoMoneda_)

    '    Next

    'End Sub

    Protected Sub fbcIncoterm_ClickSearch(sender As Object, e As EventArgs)

    End Sub

    Protected Sub fbcProducto_ClickSearch(sender As Object, e As EventArgs)

        ''If fbcProducto.Value = "" OrElse fbcProducto.Value = "000000000000000000000000" Then
        ''System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: PRODUCTO BUSCANDO CON LUPA")
        'If fbcProducto.Value = "" OrElse fbcProducto.Value = "000000000000000000000000" Then

        Dim estado_ As TagWatcher = _utils.BuscarProductos(fbcProducto.Text.ToUpper(), fbcCliente.Value, fbcCompradorReceptor.Value)

        ''System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: ESTADO DEL PRODUCTO {estado_}")

        If estado_ IsNot Nothing AndAlso estado_.Status = TypeStatus.Ok Then
            ''System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: PRODUCTO OK")
            ListaProductosEncontrados(estado_.ObjectReturned)
        Else

            DisplayMessage("Producto no disponible", StatusMessage.Fail)

        End If

        'End If

        '' End If

    End Sub

    Protected Sub scMetodoValoracion_SelectedIndexChanged(sender As Object, e As EventArgs)

        ' scMetodoValoracionPartida.Value = scMetodoValoracion.Value

    End Sub

#End Region

#End Region

End Class
