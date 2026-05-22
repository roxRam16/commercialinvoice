Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Runtime.CompilerServices
Imports System.Threading.Tasks
Imports Gsol
Imports Gsol.krom
Imports Gsol.Web.Components
Imports iText.Kernel.Colors
Imports iText.Layout.Properties
Imports iText.StyledXmlParser.Css.Selector
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Org.BouncyCastle.Asn1.Crmf
Imports Org.BouncyCastle.X509
Imports Rec.Globals
Imports Rec.Globals.Controllers
Imports Rec.Globals.Empresas
Imports Rec.Globals.Utils
Imports Sax
Imports Sax.Web
Imports Syn.CustomBrokers.Controllers
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Syn.Nucleo
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Nucleo.RecursosComercioExterior.CamposAcuseValor
Imports Syn.Nucleo.RecursosComercioExterior.CamposFacturaComercial
Imports Syn.Utils
Imports Wma.Exceptions
Imports Wma.Exceptions.TagWatcher
Imports Wma.Exceptions.TagWatcher.TypeStatus
Imports Item = Syn.CustomBrokers.Controllers.Item

Public Class Ges003_001_FacturasComerciales
    Inherits ControladorBackend

#Region "████████████████████████████████████████   Atributos locales  ██████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    Private _sistema As New Syn.Utils.Organismo

    Private _secuencia As ISecuencia

    Private _controladorSecuencias As IControladorSecuencia

    Private _controladorFacturaComercial As IControladorFacturaComercial

    Private _controladorMonedas As IControladorMonedas

    Private _controladorProductos As IControladorProductos

    Private _controladorProveedores As CtrlProveedoresOperativos

    Private _monedas As Object

    Private _pais As Pais

    Private _controladorPaises As ControladorPaises

    Private _constructorCliente As ConstructorCliente

    Private _constructorProveedorOperativo As ControladorBusqueda(Of ConstructorProveedoresOperativos)

    Private _constructorProveedorAux As ConstructorProveedoresOperativos

    Private _loginUsuario As Dictionary(Of String, String)

    Private _datosCliente As ClienteFacturaComercial

    Private _domicilioCliente As Rec.Globals.Empresas.Domicilio

    Private _domicilioProveedor As List(Of Rec.Globals.Empresas.Domicilio)

    Private _datosReceptorProveedor As List(Of Dictionary(Of String, String))

    Private _domiciliosProveedores As List(Of Nodo)

    Private _domicilioAux As Rec.Globals.Empresas.Domicilio

    Private _buscarCliente As New ControladorBusqueda(Of ConstructorCliente)

    Private _lista As List(Of SelectOption)

    Private _paisSeleccionado As Pais

    Private _resultadoMonedaPais As List(Of moneda)

    Private _listaProveedores As Dictionary(Of String, String)

    Private _listaDomiciliosProveedores As List(Of Rec.Globals.Empresas.Domicilio)

    Private _listaDomicilios As List(Of Rec.Globals.Empresas.Domicilio)

    Private _tagwatcher As TagWatcher

    Private _constructorClienteBusqueda As ControladorBusqueda(Of ConstructorCliente)

    Private _buscarPais As List(Of SelectOption)

    Private _buscarProveedor As TagWatcher

    Private _proveedorObtenido As AuxiliarProveedor

    Private _esDestinatario As Boolean

    Private _listaCompradorReceptor As List(Of SelectOption)

    Private _numeroparteItem As String

    Private _descripcionItem As String

    Private _listaNumeroParteProductos As List(Of String)

    Private _listaDescripcionesProductos As List(Of String)

    Private _consultaProducto As String

    Private _filtroProducto As String

    Private _productoAuxiliar As AuxiliarProducto

    Private _listaUnidadMedida As List(Of UnidadMedida)

    Private _cacheListaUnidadesMedida As List(Of SelectOption)

    Private _vinculacionRecursos As ControladorRecursosAduanalesGral

    Private _obtenerDocumentoProveedor As ControladorBusqueda(Of ConstructorProveedoresOperativos)

    Private _constructorProveedor As ConstructorProveedoresOperativos

    Private _utils As UtilsFacturaComercial

    Private _proveedorSeleccionado As AuxiliarProveedor

    Private _domicilioSeleccionadoProveedor As Domicilio

    Private _listaCamposMonedas As List(Of Object)

    Private _monedaUSD As String

    Private _objectidmonedaUSD As String

    Private _dataSourceMoneda As List(Of SelectOption)

    Private _resultMoneda As List(Of MonedaGlobal)

    Private _facturaComercialUI As Dictionary(Of String, Object)

    Private _controladorMoneda As ControladorMonedas

    Private _controladorFactura As ControladorFacturaComercial



    'Private _commercialInvoiceAnalizer As CommercialInvoiceAnalysis

    'Private _cacheCommercialInvoiceAnalizer As CommercialInvoiceAnalysis


#End Region

#Region "████████████████████████████████████████   Constructores  ██████████████████████████████████████"
    '    ██                                                                                            ██
    '    ██                                                                                            ██
    '    ██                                                                                            ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████
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
    'INICIALIZADOR Y BOTONERA
    Public Overrides Sub Inicializa()

        With Buscador

            .DataObject = New ConstructorFacturaComercial(True)

            .addFilter(SeccionesFacturaComercial.SFAC1, CamposFacturaComercial.CA_NUMERO_FACTURA, "Número de factura")

            .addFilter(SeccionesFacturaComercial.SFAC1, CamposAcuseValor.CA_NUMERO_ACUSEVALOR, "Acuse de Valor")

            .addFilter(SeccionesFacturaComercial.SFAC1, CamposFacturaComercial.CP_NUMERO_FACTURA_SUBDIVISION, "Número de factura subdividida")

            .addFilter(SeccionesFacturaComercial.SFAC2, CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, "Proveedor")

            .MetadatosFilter = New Dictionary(Of [Enum], String) From {{CamposFacturaComercial.CP_TIPO_OPERACION, 1}}

        End With

        If Not Page.IsPostBack Then

            Session("_pbPartidas") = PillboxControl.ToolbarModality.Default

        End If

        pbPartidas.Modality = Session("_pbPartidas")

        '  Generales
        fbcIncoterm.DataEntity = New krom.Anexo22()

        '' Datos del proveedor
        'scMetodoValoracion.DataEntity = New krom.Anexo22()

        '' Partidas
        'scMetodoValoracionPartida.DataEntity = New krom.Anexo22()

        icFechaCOVE.Enabled = False

        icFraccionArancelaria.Enabled = False

        icFraccionNico.Enabled = False

        icTipoCargaDatos.Value = Nothing

        lbModoCapturaIA.Visible = False

        lbModoCapturaIAEditar.Visible = False

        lbModoCapturaManual.Visible = False

        lbModoCapturaManualNuevo.Visible = True

        _utils = New UtilsFacturaComercial

        _domicilioSeleccionadoProveedor = New Domicilio

        _proveedorSeleccionado = New AuxiliarProveedor

        _lista = New List(Of SelectOption)

        '_monedaUSD = "USD"

        '_objectidmonedaUSD = "635acf25a8210bfa0d58434e"

        _listaCamposMonedas = New List(Of Object) _
                             From {scMonedaFactura,
                             scMonedaMercancia,
                             scMonedaFacturaPartida,
                             scMonedaMercanciaPartida,
                             scMonedaPrecioUnitarioPartida,
                             scMonedaFletes,
                             scMonedaSeguros,
                             scMonedaEmbalajes,
                             scMonedaOtrosIncrementables}

        _facturaComercialUI = New Dictionary(Of String, Object) From {
          {"invoicenumber", dbcNumFacturaCOVE},
          {"invoicedate", icFechaFacturaImpo},
          {"invoiceseries", icFolioFactura},
          {"customername", fbcCliente},
          {"incoterm", fbcIncoterm},
          {"value", icValorFactura},
          {"invoicecurrency", scMonedaFactura},
          {"totalinvoice", icValorMercancia},
          {"totalweight", icPesoTotal},
          {"packages", icBultos},
          {"purchaseorder", icOrdenCompra},
          {"suppliername", fbcProveedor},
          {"address", scDomiciliosProveedor},
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

        lbModoCapturaManualNuevo.Visible = True

        lbModoCapturaIA.Visible = False

        lbModoCapturaIAEditar.Visible = False

        lbModoCapturaManual.Visible = False

        If OperacionGenerica IsNot Nothing Then

        End If

        PreparaTarjetero(PillboxControl.ToolbarModality.Simple, pbPartidas)

        CargarMonedaPorDefault()

        CargarMetodoValoracion(scMetodoValoracion)

        CargarMetodoValoracion(scMetodoValoracionPartida)

        icFechaCOVE.Enabled = False

        icFraccionArancelaria.Enabled = False

        icFraccionNico.Enabled = False

        scMetodoValoracion.Value = 1

        scMetodoValoracionPartida.Value = 1



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

            ''CHECAMOS SI VIENE DE LA IA

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


                'If VerificarObjectIdValidoRegistros() = False Then

                '    AvisosVerificacionObjectIdValido()

                '    DisplayMessage("Verificar campos en registros de SYNAPSIS", StatusMessage.Warning)

                '    'CamposRequeridos()

                'End If

            Else

                If VerificarObjectIdValidoRegistros() Then

                    If Not ProcesarTransaccion(Of ConstructorFacturaComercial)().Status = TypeStatus.Errors Then : End If

                Else

                    CamposRequeridos()

                    'estado_.SetError(Me, "Campos con * son requeridos")

                    'SetError("Campos con * son requeridos", estado_)

                    DisplayMessage("Campos con * son requeridos", StatusMessage.Fail)


                End If

            End If

            'If VerificarObjectIdValidoRegistros() Then

            '    If Not ProcesarTransaccion(Of ConstructorFacturaComercial)().Status = TypeStatus.Errors Then : End If

            'Else
            '    Dim tipoCaptura_ = 0

            '    If GetVars("_tipoCaptura") IsNot Nothing Then

            '        tipoCaptura_ = GetVars("_tipoCaptura")

            '    End If

            '    If tipoCaptura_ = 1 Then

            '        If VerificarObjectIdValidoRegistros() = False Then

            '            AvisosVerificacionObjectIdValido()

            '            DisplayMessage("Verificar campos en registros de SYNAPSIS", StatusMessage.Warning)

            '            'CamposRequeridos()

            '        End If

            '    Else

            '        CamposRequeridos()

            '        DisplayMessage("Campos con * son requeridos", StatusMessage.Fail)

            '    End If

            'End If

        End If

    End Sub

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

    Public Overrides Sub BotoneraClicEditar()
        ''PENDIENTE PARA CUANDO EL INGE HABILITE EL BOTON DE PUBLICAR
        ' PreparaBotonera(FormControl.ButtonbarModality.Draft)

        'icFechaCOVE.Enabled = False

        If icTipoCargaDatos.Value = "1" Then

            lbModoCapturaIAEditar.Visible = True

            lbModoCapturaIA.Visible = False

            lbModoCapturaManual.Visible = False

            lbModoCapturaManualNuevo.Visible = False

            AvisosVerificacionObjectIdValido()

            '_utils = New UtilsFacturaComercial

            'Dim factura_ = _utils.ObtenerCommercialInvoiceAnalizer(dbcNumFacturaCOVE.Value)

            'OcultarTarjetaAlertaIA()

            '_utils.EjecutarAnalisisIAAsync(factura_, _facturaComercialUI)

            'EjecutarBusquedaDeCamposClave()

        Else

            lbModoCapturaIA.Visible = False

            lbModoCapturaIAEditar.Visible = False

            lbModoCapturaManual.Visible = False

            lbModoCapturaManualNuevo.Visible = True

        End If


        CargarMetodoValoracion(scMetodoValoracion)

        CargarMetodoValoracion(scMetodoValoracionPartida)

        icFechaCOVE.Enabled = False
        'pbPartidas.Enabled = True

        PreparaTarjetero(PillboxControl.ToolbarModality.Advanced, pbPartidas)

    End Sub


    'Public Overrides Sub BotoneraClicPublicar()

    'End Sub


    Public Overrides Function AgregarComponentesBloqueadosInicial() As List(Of WebControl)
        Dim bloqueados_ As New List(Of WebControl) From {icFechaCOVE, scDomiciliosProveedor, scUnidadMedidaTarifa}
        Return bloqueados_
    End Function

    Public Overrides Function AgregarComponentesBloqueadosEdicion() As List(Of WebControl)
        Dim bloqueadosEdicion_ As New List(Of WebControl) From {dbcNumFacturaCOVE, icFechaCOVE, scDomiciliosProveedor, scUnidadMedidaTarifa}
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

        End If

    End Sub

    'Public Overrides Function BotoneraClicPublicar_ProcesoInterno() As TagWatcher

    '    Dim tagwatcher_ As New TagWatcher

    '    If StatusPublicar.Status <> TypeStatus.Ok Then

    '        DisplayMessage(StatusPublicar.LastMessage, StatusMessage.Warning)

    '    End If


    '    'If VerificarObjectIdValidoRegistros() Then

    '    '    ntPublicarFacturaComercial.Visible = True

    '    'Else

    '    '    CamposRequeridos()

    '    '    DisplayMessage("Campos con * son requeridos", StatusMessage.Warning)

    '    'End If

    '    Return tagwatcher_

    'End Function


    Public Overrides Sub BotoneraClicPublicar()




        If VerificarObjectIdValidoRegistros() Then
            ntPublicarFacturaComercial.Visible = True



        Else
            StatusPublicar.Status = TypeStatus.Errors

            CamposRequeridos()
            DisplayMessage("Campos con * son requeridos", StatusMessage.Warning)
        End If

    End Sub

#End Region

#Region "Configuracion inicial"
    'ASIGNACION PARA CONTROLES AUTOMÁTICOS
    Public Overrides Function Configuracion() As TagWatcher

        [Set](icTipoCargaDatos, CP_TIPO_CARGA_DATOS, propiedadDelControl_:=PropiedadesControl.Valor)

        'Datos Generales
        [Set](dbcNumFacturaCOVE, CA_NUMERO_FACTURA, propiedadDelControl_:=PropiedadesControl.Valor, esRequerido_:=True)

        [Set](dbcNumFacturaCOVE, CA_NUMERO_ACUSEVALOR, propiedadDelControl_:=PropiedadesControl.ValueDetail)

        [Set](icFechaFacturaImpo, CA_FECHA_FACTURA, esRequerido_:=True)

        [Set](icFechaCOVE, CA_FECHA_ACUSEVALOR)

        '[Set](icidcove, CP_ID_ACUSEVALOR)

        [Set](icPesoTotal, CP_PESO_TOTAL)

        [Set](icBultos, CP_BULTOS)

        [Set](icOrdenCompra, CP_ORDEN_COMPRA)

        [Set](icReferenciaCliente, CP_REFERENCIA_CLIENTE)

        [Set](icFolioFactura, CamposFacturaComercial.CP_SERIE_FOLIO_FACTURA)

        [Set](swcEnajenacion, CP_APLICA_ENAJENACION, propiedadDelControl_:=PropiedadesControl.Checked)

        [Set](swcSubdivision, CA_APLICA_SUBDIVISION, propiedadDelControl_:=PropiedadesControl.Checked)

        'Cliente
        '[Set](fbcCliente, CamposClientes.CA_RAZON_SOCIAL, propiedadDelControl_:=PropiedadesControl.Valor, esRequerido_:=True)
        [Set](fbcCliente, CamposClientes.CA_RAZON_SOCIAL, propiedadDelControl_:=PropiedadesControl.Valor)

        [Set](fbcCliente, CamposClientes.CA_RAZON_SOCIAL, propiedadDelControl_:=PropiedadesControl.Text,
              asignarA_:=TiposAsignacion.ValorPresentacion)

        'Datos factura
        '[Set](fbcPais, CA_PAIS_FACTURACION, propiedadDelControl_:=PropiedadesControl.Valor, esRequerido_:=True)
        '[Set](fbcPais, CA_PAIS_FACTURACION, propiedadDelControl_:=PropiedadesControl.Valor)

        '[Set](fbcPais, CA_PAIS_FACTURACION, propiedadDelControl_:=PropiedadesControl.Text, asignarA_:=TiposAsignacion.ValorPresentacion, esRequerido_:=True)

        [Set](fbcIncoterm, CA_CVE_INCOTERM, propiedadDelControl_:=PropiedadesControl.Valor)

        [Set](fbcIncoterm, CA_CVE_INCOTERM, propiedadDelControl_:=PropiedadesControl.Text,
              asignarA_:=TiposAsignacion.ValorPresentacion)

        [Set](icValorFactura, CP_VALOR_FACTURA, esRequerido_:=True)

        [Set](scMonedaFactura, CA_MONEDA_FACTURACION, esRequerido_:=True)

        [Set](icValorMercancia, CP_VALOR_MERCANCIA, esRequerido_:=True)

        [Set](scMonedaMercancia, CP_MONEDA_VALOR_MERCANCIA, esRequerido_:=True)

        'Datos del proveedor
        '[Set](fbcProveedor, CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Valor, esRequerido_:=True)
        [Set](fbcProveedor, CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Valor)

        [Set](fbcProveedor, CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR,
              propiedadDelControl_:=PropiedadesControl.Text, asignarA_:=TiposAsignacion.ValorPresentacion)

        [Set](scVinculacion, CA_CVE_VINCULACION, propiedadDelControl_:=PropiedadesControl.Valor)

        [Set](scMetodoValoracion, CP_CVE_METODO_VALORACION, propiedadDelControl_:=PropiedadesControl.Valor)

        [Set](swcFungeCertificado, CA_APLICA_CERTIFICADO, propiedadDelControl_:=PropiedadesControl.Checked)

        [Set](fbcProveedorCertificado, CP_NOMBRE_CERTIFICADOR, propiedadDelControl_:=PropiedadesControl.Text)


        If pbPartidas.PageIndex > 0 Then

            lbNumero.Text = (pbPartidas.PageIndex).ToString()

        End If

        ' [Set](icObjectIdPartida, CamposFacturaComercial.CP_OBJECTID_PRODUCTOS, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](fbcProducto, CA_NUMERO_PARTE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icCantidadComercial, CA_CANTIDAD_COMERCIAL_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scUnidadMedidaComercial, CA_UNIDAD_MEDIDA_COMERCIAL_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icDescripcionPartidaOriginal, CA_DESCRIPCION_PARTE_PARTIDA_ORIGINAL, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icDescripcionPartida, CA_DESCRIPCION_PARTE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icValorfacturaPartida, CA_VALOR_FACTURA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scMonedaFacturaPartida, CP_MONEDA_FACTURA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icValorMercanciaPartida, CA_VALOR_MERCANCIA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scMonedaMercanciaPartida, CA_MONEDA_MERCANCIA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icPrecioUnitario, CA_PRECIO_UNITARIO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scMonedaPrecioUnitarioPartida, CP_MONEDA_PRECIO_UNITARIO, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icPesoNeto, CA_PESO_NETO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icDescripcionCOVE, CA_DESCRIPCION_COVE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](fbcPaisPartida, CA_PAIS_ORIGEN_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scMetodoValoracionPartida, CA_CVE_METODO_VALORACION_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icOrdenCompraPartida, CP_ORDEN_COMPRA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        ' Partida - clasificación
        [Set](icObjectIdPartida, CamposFacturaComercial.CP_OBJECTID_PRODUCTOS, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icFraccionArancelaria, CA_FRACCION_ARANCELARIA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icFraccionNico, CA_FRACCION_NICO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icCantidadTarifa, CA_CANTIDAD_TARIFA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scUnidadMedidaTarifa, CA_UNIDAD_MEDIDA_TARIFA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        ' Partida - detalle mercancía
        [Set](icLote, CA_LOTE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icNumeroSerie, CA_NUMERO_SERIE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icMarca, CA_MARCA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icModelo, CA_MODELO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icSubmodelo, CA_SUBMODELO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icKilometraje, CA_KILOMETRAJE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](coTimeStamp, CamposProducto.CP_TIMESTAMP, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](pbPartidas, Nothing, seccion_:=SeccionesFacturaComercial.SFAC4)

        ' Incrementables
        [Set](icFletes, CA_FLETES)

        [Set](scMonedaFletes, CA_MONEDA_FLETES)

        [Set](icSeguros, CA_SEGURO)

        [Set](scMonedaSeguros, CA_MONEDA_SEGUROS)

        [Set](icEmbalajes, CA_EMBALAJES)

        [Set](scMonedaEmbalajes, CA_MONEDA_EMBALAJES)

        [Set](icOtrosIncrementables, CA_OTROS_INCREMENTABLES)

        [Set](scMonedaOtrosIncrementables, CA_MONEDA_OTROS_INCREMENTABLES)

        '[Set](icDescuentos, CA_DESCUENTOS)

        '[Set](scMonedaDescuentos, CA_MONEDA_DESCUENTOS)

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
            '  ████████fin█████████       Logica de negocios local       ███████████████████████

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

        _loginUsuario = New Dictionary(Of String, String)

        _datosCliente = New ClienteFacturaComercial

        _loginUsuario = Session("DatosUsuario")

        If GetVars("_datosCliente") IsNot Nothing Then

            _datosCliente = DirectCast(GetVars("_datosCliente"), ClienteFacturaComercial)

        End If

        With documentoElectronico_

            .Campo(CamposFacturaComercial.CP_TIPO_OPERACION).Valor = 1

            .Campo(CamposFacturaComercial.CP_TIPO_OPERACION).ValorPresentacion = "Importacion"

            'If lbModoCapturaIA.Visible = True Then

            '    .Campo(CP_TIPO_CARGA_DATOS).Valor = 1

            '    .Campo(CP_TIPO_CARGA_DATOS).ValorPresentacion = "Carga IA"

            'Else

            '    .Campo(CP_TIPO_CARGA_DATOS).Valor = 2

            '    .Campo(CP_TIPO_CARGA_DATOS).ValorPresentacion = "Carga manual"

            'End If

            .UsuarioGenerador = _loginUsuario("Nombre")

            '.Id = _secuencia._id.ToString

            .IdDocumento = _secuencia.sec

            .FolioDocumento = dbcNumFacturaCOVE.Value.ToUpper()

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

        If GetVars("ProveedorSeleccionado_") IsNot Nothing Then

            _proveedorSeleccionado = New AuxiliarProveedor

            _proveedorSeleccionado = DirectCast(GetVars("ProveedorSeleccionado_"), AuxiliarProveedor)

            OperacionNueva.DocumentosAsociados = New List(Of DocumentoAsociado)

            Dim documentosAsociadoCliente_ As New DocumentoAsociado With {
                .analisisconsistencia = 1,
                .identificadorrecurso = "ConstructorCliente",
                ._iddocumentoasociado = ObjectId.Parse(fbcCliente.Value),
                .firmaelectronica = _datosCliente.firmaElectronicaCliente}

            OperacionNueva.DocumentosAsociados.add(documentosAsociadoCliente_)

            Dim documentosAsociado_ As New DocumentoAsociado With {
                        .analisisconsistencia = 1,
                        .identificadorrecurso = "ConstructorProveedoresOperativos",
                        ._iddocumentoasociado = ObjectId.Parse(fbcProveedor.Value),
                        .firmaelectronica = _proveedorSeleccionado._firmaElectrónica}

            OperacionNueva.DocumentosAsociados.add(documentosAsociado_)

        End If

        Return tagwatcher_

    End Function

    Public Overrides Function DespuesRealizarInsercion() As TagWatcher

        lbModoCapturaManual.Visible = True

        lbModoCapturaManualNuevo.Visible = False

        lbModoCapturaIA.Visible = False

        lbModoCapturaIAEditar.Visible = False


        'Dim res_ = _controladorFacturaComercial.GuardarDocumentoAsociado(OperacionGenerica, prov_._firmaElectrónica, OperacionGenerica.Id)

        Return New TagWatcher(Ok)

    End Function

    'EVENTOS PARA LA MODIFICACIÓN DE DATOS
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

            If GetVars("_datosCliente") IsNot Nothing Then

                _datosCliente = New ClienteFacturaComercial

                _datosCliente = DirectCast(GetVars("_datosCliente"), ClienteFacturaComercial)

            End If

            If _datosCliente IsNot Nothing Then

                _datosCliente = New ClienteFacturaComercial

                _datosCliente = DirectCast(GetVars("_datosCliente"), ClienteFacturaComercial)

                If _datosCliente IsNot Nothing Then

                    If modoEditando_ Then

                        Dim documentosAsociadoCliente_ As New DocumentoAsociado With {
                        .analisisconsistencia = 1,
                        .identificadorrecurso = "ConstructorCliente",
                        ._iddocumentoasociado = ObjectId.Parse(fbcCliente.Value),
                        .firmaelectronica = _datosCliente.firmaElectronicaCliente}

                        Dim environmentid_ As Int32 = ListaEmpresas.Value

                        _controladorFactura = New ControladorFacturaComercial(IControladorFacturaComercial.TipoOperaciones.Importacion, environmentid_)

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

                        ''SI O SI DEBES LLENARSE
                        .Campo(CamposClientes.CA_RFC_CLIENTE).Valor = ToUpperSafe(_datosCliente.rfc)

                        ''AL MENOS QUE SE TRATE DE UN CLIENTE FISICO TENDRA UN CURP
                        .Campo(CamposClientes.CA_CURP_CLIENTE).Valor = ToUpperSafe(_datosCliente.curp)

                        ''SIEMPRE PUEDE VENIR VACIO PORQUE SOLO ESTAMOS CON CLIENTES NACIONALES
                        '.Campo(CamposClientes.CA_TAX_ID).Valor = _datosCliente.taxid
                        ''PARA QUE ACUSE DE VALOR NO LE AFECTE, SE LLENARA CON RFC
                        ''SI LE METEN CLIENTES EXTRANEJEROS, ENTONCES COMENTAR ESTA LINEA PORFI C:
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

                        .Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor = _datosCliente.codigopostal

                        .Campo(CamposDomicilio.CA_COLONIA).Valor = ToUpperSafe(_datosCliente.colonia)

                        .Campo(CamposDomicilio.CA_LOCALIDAD).Valor = ToUpperSafe(_datosCliente.localidad)

                        .Campo(CamposDomicilio.CA_CIUDAD).Valor = ToUpperSafe(_datosCliente.ciudad)

                        .Campo(CamposDomicilio.CA_MUNICIPIO).Valor = ToUpperSafe(_datosCliente.municipio)

                        .Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor = ToUpperSafe(_datosCliente.cveEntidadfederativa)

                        .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor = ToUpperSafe(_datosCliente.entidadfederativa)

                        .Campo(CamposDomicilio.CA_ENTIDAD_MUNICIPIO).Valor = ToUpperSafe(_datosCliente.municipio)

                    End With

                End If

            End If

            With .Seccion(SeccionesFacturaComercial.SFAC1)

                .Campo(CamposFacturaComercial.CP_APLICA_INCREMENTABLES).Valor = ComprobacionIncrementables()

                Dim aplicaEnajecacion_ As Boolean = False

                If swcEnajenacion.Checked Then

                    aplicaEnajecacion_ = True

                End If

                Dim aplicaSubdivision_ As Boolean = False

                If swcSubdivision.Checked Then

                    aplicaSubdivision_ = True

                End If

                .Campo(CamposFacturaComercial.CP_APLICA_ENAJENACION).Valor = aplicaEnajecacion_

                .Campo(CamposFacturaComercial.CA_APLICA_SUBDIVISION).Valor = aplicaSubdivision_

                .Campo(CamposFacturaComercial.CA_FECHA_FACTURA).ValorPresentacion = icFechaFacturaImpo.Value

                .Campo(CamposFacturaComercial.CP_ORDEN_COMPRA).Valor = ToUpperSafe(icOrdenCompra.Value)

                .Campo(CamposFacturaComercial.CP_REFERENCIA_CLIENTE).Valor = ToUpperSafe(icReferenciaCliente.Value)

                .Campo(CamposFacturaComercial.CP_SERIE_FOLIO_FACTURA).Valor = ToUpperSafe(icFolioFactura.Value)


                Dim tipoCargaDatos_ As Int32 = IIf(GetVars("_tipoCaptura") IsNot Nothing, GetVars("_tipoCaptura"), 2)

                Dim tipoCargaDatosPresentacion_ = IIf(tipoCargaDatos_ = 2, "Carga manual", "Carga IA")

                .Campo(CamposFacturaComercial.CP_TIPO_CARGA_DATOS).Valor = tipoCargaDatos_

                .Campo(CamposFacturaComercial.CP_TIPO_CARGA_DATOS).ValorPresentacion = tipoCargaDatosPresentacion_


                If modoEditando_ = False Then

                    .Campo(CamposFacturaComercial.CP_MARCADO_PEDIMENTO).Valor = False

                    .Campo(CamposFacturaComercial.CP_ID_PEDIMENTO_ASOCIADO).Valor = ObjectId.Parse("000000000000000000000000")

                    .Campo(CamposFacturaComercial.CP_ID_FACTURA_ORIGINAL).Valor = ObjectId.Parse("000000000000000000000000")

                    .Campo(CamposFacturaComercial.CP_NUMERO_FACTURA_SUBDIVISION).Valor = Nothing

                End If

                '.Campo(CamposFacturaComercial.CP_ID_FACTURA_ORIGINAL).Valor = ObjectId.Parse("000000000000000000000000")

                '.Campo(CamposFacturaComercial.CP_NUMERO_FACTURA_SUBDIVISION).Valor = Nothing

            End With

            _proveedorSeleccionado = Nothing

            _domicilioSeleccionadoProveedor = Nothing

            ''SI ESTA SE OBTIENE, ES PORQUE SE HIZO UN CAMBIO EN PROVEEDOR ASI DE SIMPLE
            If GetVars("ProveedorSeleccionado_") IsNot Nothing Then

                _proveedorSeleccionado = New AuxiliarProveedor

                _proveedorSeleccionado = DirectCast(GetVars("ProveedorSeleccionado_"), AuxiliarProveedor)
            End If

            ''SI ESTA SE OBTIENE, ES PORQUE SE HIZO UN CAMBIO EN PROVEEDOR ASI DE SIMPLE
            If GetVars("DomicilioProveedorSeleccionado_") IsNot Nothing Then

                _domicilioSeleccionadoProveedor = New Domicilio

                _domicilioSeleccionadoProveedor = DirectCast(GetVars("DomicilioProveedorSeleccionado_"), Domicilio)

            End If

            If _proveedorSeleccionado IsNot Nothing Then

                If modoEditando_ Then

                    Dim documentosAsociadoProveedor_ As New DocumentoAsociado With {
                    .analisisconsistencia = 1,
                    .identificadorrecurso = "ConstructorProveedoresOperativos",
                    ._iddocumentoasociado = ObjectId.Parse(fbcProveedor.Value),
                    .firmaelectronica = _proveedorSeleccionado._firmaElectrónica}

                    Dim environmentid_ As Int32 = ListaEmpresas.Value

                    _controladorFactura = New ControladorFacturaComercial(IControladorFacturaComercial.TipoOperaciones.Importacion, environmentid_)


                    Dim documentoManualAsociadoProveedor_ As TagWatcher = _controladorFactura.ActualizarDocumentosAsociadosFacturaComercial(documentosAsociadoProveedor_, OperacionGenerica.Id)

                End If

                With .Seccion(SeccionesFacturaComercial.SFAC2)
                    ''LO AMOS A GUARDAR CON EL TAXID, PARA NO ALTERAR LOS METODO EN CONTROLADOR DE FACTURA QUE SE BUSCAN POR ESTE CAMPO
                    .Campo(CamposProveedorOperativo.CP_ID_PROVEEDOR).Valor = _proveedorSeleccionado.id

                    '.Campo(CamposProveedorOperativo.CP_ID_PROVEEDOR).Valor = _proveedorSeleccionado.idtaxid

                    .Campo(CamposProveedorOperativo.CP_CVE_PROVEEDOR).Valor = _proveedorSeleccionado._clave

                    .Campo(CamposDomicilio.CA_CVE_PAIS).Valor = ToUpperSafe(_proveedorSeleccionado._cvepais)

                    .Campo(CamposDomicilio.CA_PAIS).Valor = ToUpperSafe(_proveedorSeleccionado._pais)

                    If _proveedorSeleccionado._esextranjero Then

                        .Campo(CamposProveedorOperativo.CA_RFC_PROVEEDOR).Valor = ToUpperSafe(_proveedorSeleccionado._taxid)

                        .Campo(CamposProveedorOperativo.CA_CURP_PROVEEDOR).Valor = Nothing

                        .Campo(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR).Valor = _proveedorSeleccionado.idtaxid

                        .Campo(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR).ValorPresentacion = ToUpperSafe(_proveedorSeleccionado._taxid)

                    Else

                        If _proveedorSeleccionado._rfc Is Nothing Then

                            .Campo(CamposProveedorOperativo.CA_RFC_PROVEEDOR).Valor = _proveedorSeleccionado._taxid
                        Else
                            If _proveedorSeleccionado._rfc <> "" Then

                                .Campo(CamposProveedorOperativo.CA_RFC_PROVEEDOR).Valor = ToUpperSafe(_proveedorSeleccionado._rfc)

                            Else

                                .Campo(CamposProveedorOperativo.CA_RFC_PROVEEDOR).Valor = ToUpperSafe(_proveedorSeleccionado._taxid)

                            End If

                        End If

                        .Campo(CamposProveedorOperativo.CA_CURP_PROVEEDOR).Valor = ToUpperSafe(_proveedorSeleccionado._curp)

                        .Campo(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR).Valor = _proveedorSeleccionado.idtaxid

                        .Campo(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR).ValorPresentacion = _proveedorSeleccionado._taxid
                    End If

                    If _domicilioSeleccionadoProveedor IsNot Nothing Then

                        .Campo(CamposProveedorOperativo.CP_ID_DOMICILIO_PROVEEDOR).Valor = _domicilioSeleccionadoProveedor._iddomicilio.ToString
                        .Campo(CamposProveedorOperativo.CP_SEC_DOMICILIO_PROVEEDOR).Valor = _domicilioSeleccionadoProveedor.sec

                        .Campo(CamposProveedorOperativo.CA_DOMICILIO_FISCAL).Valor = ToUpperSafe(_domicilioSeleccionadoProveedor.domicilioPresentacion)

                        .Campo(CamposDomicilio.CA_CALLE).Valor = ToUpperSafe(_domicilioSeleccionadoProveedor.calle)

                        .Campo(CamposDomicilio.CA_CIUDAD).Valor = ToUpperSafe(_domicilioSeleccionadoProveedor.ciudad)

                        .Campo(CamposDomicilio.CA_COLONIA).Valor = ToUpperSafe(_domicilioSeleccionadoProveedor.colonia)

                        .Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor = ToUpperSafe(_domicilioSeleccionadoProveedor.codigopostal)

                        .Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor = ToUpperSafe(_domicilioSeleccionadoProveedor.numeroexterior)

                        .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor = ToUpperSafe(_domicilioSeleccionadoProveedor.numerointerior)

                        .Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor = $"{ToUpperSafe(_domicilioSeleccionadoProveedor.numeroexterior)}  - {ToUpperSafe(_domicilioSeleccionadoProveedor.numerointerior)}"

                        .Campo(CamposDomicilio.CA_LOCALIDAD).Valor = ToUpperSafe(_domicilioSeleccionadoProveedor.localidad)

                        .Campo(CamposDomicilio.CA_MUNICIPIO).Valor = ToUpperSafe(_domicilioSeleccionadoProveedor.municipio)

                        .Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor = ToUpperSafe(_domicilioSeleccionadoProveedor.cveEntidadfederativa)

                        .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor = _domicilioSeleccionadoProveedor.entidadfederativa

                        .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).ValorPresentacion = ToUpperSafe(_domicilioSeleccionadoProveedor.entidadfederativa)

                        .Campo(CamposDomicilio.CA_ENTIDAD_MUNICIPIO).Valor = ToUpperSafe(_domicilioSeleccionadoProveedor.municipio)

                    End If

                End With

            End If

            Dim NodosItems_ = .Seccion(SeccionesFacturaComercial.SFAC4)

            Dim x_ As Integer = 0

            For Each nodo_ In NodosItems_.Nodos

                ''NO GUARDA EL OBJECTID del producto porque pillbox no reconoce los inputs ocultos

                If Not ObjectId.Parse(pbPartidas.DataSource(x_).Item("fbcProducto").Item("Value")) = ObjectId.Empty Then



                    If pbPartidas.DataSource(x_).Item("fbcProducto").Count > 0 Then

                        nodo_.Campo(CamposFacturaComercial.CP_OBJECTID_PRODUCTOS).Valor = ObjectId.Parse(pbPartidas.DataSource(x_).Item("fbcProducto").Item("Value"))

                    End If

                    nodo_.Campo(CamposFacturaComercial.CP_NUMERO_PARTIDA).Valor = CInt(pbPartidas.DataSource(x_).Item("identidad"))

                    For Each i_ In pbPartidas.DataSource(x_)


                        If i_.Key = "icOrdenCompraPartida" Then
                            Dim aca_ = i_.Value
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

                            nodo_.Campo(CamposProducto.CP_TIMESTAMP).Valor = i_.Value

                        End If

                    Next

                    x_ += 1
                End If

            Next

        End With

        Return tagwatcher_

    End Function

    'EVENTO PARA BÚSQUEDA
    Public Overrides Function DespuesBuquedaGeneralConDatos_ProcesoInterno() As TagWatcher

        Dim tagwatcher_ As New TagWatcher(Ok)

        If OperacionGenerica.Publicado = True And OperacionGenerica.FirmaElectronica IsNot Nothing Then

            PreparaBotonera(FormControl.ButtonbarModality.Protected)

        End If

        Dim constructorFacturaComercial_ = DirectCast(OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente, ConstructorFacturaComercial)

        Dim tipoCaptura_ = constructorFacturaComercial_.Seccion(SeccionesFacturaComercial.SFAC1).Campo(CamposFacturaComercial.CP_TIPO_CARGA_DATOS).Valor

        SetVars("_tipoCaptura", tipoCaptura_)

        Dim datasourceproveedor_ = New SelectOption With {.Value = constructorFacturaComercial_.Seccion(SeccionesFacturaComercial.SFAC2).Campo(CamposProveedorOperativo.CP_ID_DOMICILIO_PROVEEDOR).Valor,
          .Text = constructorFacturaComercial_.Seccion(SeccionesFacturaComercial.SFAC2).Campo(CamposProveedorOperativo.CA_DOMICILIO_FISCAL).Valor}

        scDomiciliosProveedor.DataSource = New List(Of SelectOption) From {datasourceproveedor_}

        scDomiciliosProveedor.Value = constructorFacturaComercial_.Seccion(SeccionesFacturaComercial.SFAC2).Campo(CamposProveedorOperativo.CP_ID_DOMICILIO_PROVEEDOR).Valor

        SetVars("idDocumentoElectronico_", OperacionGenerica.Id)

        PreparaTarjetero(PillboxControl.ToolbarModality.Default, pbPartidas)

        If tipoCaptura_ = 1 Or tipoCaptura_ = "1" Then

            'MostrarTarjetaAlertaIA("Verificar campos en registros de SYNAPSIS", "2.3", "16/02/1992", "26")

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

        icFechaCOVE.Enabled = False

        Return tagwatcher_

    End Function

    Public Overrides Function DespuesBuquedaGeneralSinDatos_ProcesoInterno() As TagWatcher

        Dim tagwatcher_ As New TagWatcher(Ok)
        ' BuscarDomiciliosProveedor()
        lbModoCapturaManual.Visible = True

        lbModoCapturaManualNuevo.Visible = False

        lbModoCapturaIA.Visible = False

        lbModoCapturaIAEditar.Visible = False

        PreparaTarjetero(PillboxControl.ToolbarModality.Advanced, pbPartidas)

        Return tagwatcher_

    End Function

#End Region

#Region "Eventos de mantenimiento"
    'EVENTOS DE MANTENIMIENTO
    Public Overrides Sub LimpiaSesion()

        SetVars("_tipoCaptura", Nothing)

        SetVars("isEditing", Nothing)

        SetVars("_datosCliente", Nothing)

        SetVars("_listaDomiciliosProveedores", Nothing)

        SetVars("_datosReceptorProveedor", Nothing)

        SetVars("_listaDomiciliosDestinatario", Nothing)

        SetVars("_datosDestinatario", Nothing)

        SetVars("_listaConstructorProductos", Nothing)

        HttpRuntime.Cache.Remove("cacheListaMonedas")

        HttpRuntime.Cache.Remove("cacheListaUnidadesMedida")

        HttpRuntime.Cache.Remove("cacheCommercialInvoiceAnalizer")

        HttpRuntime.Cache.Remove("cacheNombreMonedaCompleto")

        SetVars("ProveedorSeleccionado_", Nothing)

        SetVars("DomicilioProveedorSeleccionado_", Nothing)

    End Sub

    Public Overrides Sub Limpiar()

        If _controladorFacturaComercial IsNot Nothing Then _controladorFacturaComercial.Dispose()

        If _controladorMonedas IsNot Nothing Then _controladorMonedas.Dispose()

        If _controladorProductos IsNot Nothing Then _controladorProductos.Dispose()

        If _controladorProveedores IsNot Nothing Then _controladorProveedores.Dispose()

        If _buscarCliente IsNot Nothing Then _buscarCliente.Dispose()

        If _constructorProveedorOperativo IsNot Nothing Then _constructorProveedorOperativo.Dispose()

        If _constructorCliente IsNot Nothing Then _constructorCliente.Dispose()

        If _constructorClienteBusqueda IsNot Nothing Then _constructorClienteBusqueda.Dispose()

        scDomiciliosProveedor.Value = Nothing

        scDomiciliosProveedor.DataSource = Nothing

        _utils.LimpiarMonedas(_listaCamposMonedas)

        _sistema = Nothing

        _secuencia = Nothing

        _controladorSecuencias = Nothing

        _controladorFacturaComercial = Nothing

        _controladorMonedas = Nothing

        _controladorProductos = Nothing

        _controladorProveedores = Nothing

        _monedas = Nothing

        _pais = Nothing

        _controladorPaises = Nothing

        _constructorCliente = Nothing

        _constructorProveedorOperativo = Nothing

        _constructorProveedorAux = Nothing

        _constructorClienteBusqueda = Nothing

        _constructorProveedor = Nothing

        _loginUsuario = Nothing

        _datosCliente = Nothing

        _domicilioCliente = Nothing

        _domicilioProveedor = Nothing

        _domiciliosProveedores = Nothing

        _domicilioAux = Nothing

        _datosReceptorProveedor = Nothing

        _buscarCliente = Nothing

        _buscarPais = Nothing

        _buscarProveedor = Nothing

        _proveedorObtenido = Nothing

        _esDestinatario = Nothing

        _listaCompradorReceptor = Nothing

        _lista = Nothing

        _listaDomiciliosProveedores = Nothing

        _listaDomicilios = Nothing

        _listaProveedores = Nothing

        _paisSeleccionado = Nothing

        _resultadoMonedaPais = Nothing

        _tagwatcher = Nothing

        _numeroparteItem = Nothing

        _descripcionItem = Nothing

        _listaNumeroParteProductos = Nothing

        _listaDescripcionesProductos = Nothing

        _consultaProducto = Nothing

        _filtroProducto = Nothing

        _productoAuxiliar = Nothing

        _listaUnidadMedida = Nothing

        _cacheListaUnidadesMedida = Nothing

        _vinculacionRecursos = Nothing

        _obtenerDocumentoProveedor = Nothing

        _utils = Nothing

        _proveedorSeleccionado = Nothing

        _domicilioSeleccionadoProveedor = Nothing

        _listaCamposMonedas = Nothing

        _monedaUSD = Nothing

        _objectidmonedaUSD = Nothing

        _dataSourceMoneda = Nothing

        _resultMoneda = Nothing

        _controladorFactura = Nothing

        HttpRuntime.Cache.Remove("cacheListaMonedas")

        HttpRuntime.Cache.Remove("cacheListaUnidadesMedida")

        HttpRuntime.Cache.Remove("cacheCommercialInvoiceAnalizer")

        HttpRuntime.Cache.Remove("cacheNombreMonedaCompleto")

        lbModoCapturaIA.Visible = False
        lbModoCapturaIAEditar.Visible = False
        lbModoCapturaManual.Visible = False
        lbModoCapturaManualNuevo.Visible = True

        ''REGRESAR LOS CONTROLES DEL PILLBOX A DEFAULT
        PreparaTarjetero(PillboxControl.ToolbarModality.Default, pbPartidas)

        'scMonedaFactura.ToolTip = Nothing
        'scMonedaFactura.ToolTipExpireTime = 1

    End Sub
#End Region

#End Region

#Region "████████████████  QUINTA CAPA  █████████       Reglas locales         ██████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████
#Region "Cliente"
    Protected Sub BuscarCliente()

        'Dim tipoCaptura_ As Int16 = Nothing

        'If GetVars("_tipoCaptura") IsNot Nothing Then

        '    tipoCaptura_ = GetVars("_tipoCaptura")

        'End If

        'If fbcCliente.Value = "" OrElse tipoCaptura_ = 1 Then

        '    _lista = _utils.ObtenerListaClientePorControlador(Trim(fbcCliente.Text))

        '    If _lista IsNot Nothing Then

        '        If _lista.Count > 0 Then

        '            fbcCliente.DataSource = _lista

        '        Else

        '            DisplayMessage("Cliente no encontrado", StatusMessage.Fail)

        '        End If

        '    End If

        'End If
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

    Protected Sub fbcCliente_Click(sender As Object, e As EventArgs)

        If fbcCliente.Value <> "" Then

            CargarDatosClienteSesion()

            ' DisplayMessage("Cliente establecido correctamente", StatusMessage.Info)

        End If

    End Sub

    Protected Sub CargarDatosClienteSesion()

        Dim datoscliente_ = _utils.DatosCliente(fbcCliente.Value)

        datoscliente_.idcliente = fbcCliente.Value

        SetVars("_datosCliente", datoscliente_)

    End Sub



    Private Sub ComprobarClienteIA()

        ''obtener el cliente que ofrece la ia
        Try
            ''hay que buscarlo por razon social, no por objetid
            Dim datoscliente_ = _utils.DatosCliente(fbcCliente.Text, False
                                                    )
            If datoscliente_.idcliente <> "" Then

                SetVars("_datosCliente", datoscliente_)

            Else

                AvisoRegistroNoEncontrado(New List(Of Object) From {fbcCliente})

            End If

        Catch ex As Exception

        End Try

    End Sub

    'Private Sub ComprabarPais()

    '    If fbcPais.Value = "" Or fbcPais.Text = "" Then

    '        AvisoRegistroNoEncontrado(New List(Of Object) From {fbcPais})

    '    Else
    '        ''buscar el país
    '        Dim listaPaises_ = _utils.ObtenerListaPaises(fbcPais.Text)

    '        If listaPaises_.Count > 0 Then

    '            fbcPais.DataSource = listaPaises_

    '            CargarMonedaPorDefault(objectIdPais_:=fbcPais.Value)

    '            'fbcPaisPartida.Value = fbcPais.Value

    '            'fbcPaisPartida.Text = fbcPais.Text
    '        Else

    '            AvisoRegistroNoEncontrado(New List(Of Object) From {fbcPais})

    '        End If

    '    End If

    'End Sub

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

        If fbcProveedor.Value = "000000000000000000000000" Or fbcProveedor.Value = "" Then

            _utils.AvisoVerificacionManual(New List(Of Object) From {fbcProveedor})

        End If

        If scDomiciliosProveedor.Value = "000000000000000000000000" Or scDomiciliosProveedor.Value = "" Then

            _utils.AvisoVerificacionManual(New List(Of Object) From {scDomiciliosProveedor})

        End If

    End Sub


#End Region

#Region "Proveedor"
    Private Sub BuscarProveedor()
        Dim tagwatcher_ = Nothing
        ''VOY A LIMPIAR SIEMPRE EL DOMICILIO, PORQUE ESTA CHIVA NO AGARRA EL BOTONCITO DE BORRAR
        Dim listaProveedoresOperativos_ As List(Of AuxiliarProveedor)

        Dim listaDataSource_ As New List(Of SelectOption)

        tagwatcher_ = _utils.ObtenerListaProveedoresOperativosPorControlador(fbcProveedor.Text)

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

                    DisplayMessage("Proveedor no encontrado", StatusMessage.Fail)

                End If

            End If

        Else

            DisplayMessage("Proveedor no encontrado", StatusMessage.Fail)

        End If

        fbcProveedor.DataSource = listaDataSource_

    End Sub

    Protected Sub fbcProveedor_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub fbcProveedor_TextChanged(sender As Object, e As EventArgs)

        If fbcProveedor.Value = "" Then

            BuscarProveedor()

        End If

    End Sub

    Protected Sub fbcProveedor_Click(sender As Object, e As EventArgs)

        Dim modoEditando_ As Boolean = False

        If GetVars("isEditing") IsNot Nothing Then

            If GetVars("isEditing") = True Then

                modoEditando_ = True

            End If

        End If

        If modoEditando_ = False Then

            Dim environmentid_ As Int32 = ListaEmpresas.Value

            If _utils.ExisteFacturaComercial(dbcNumFacturaCOVE.Value, fbcProveedor.Value, icFechaFacturaImpo.Value, environmentid_) Then

                DisplayMessage("🔴 Factura ya registrada", StatusMessage.Info)

            End If

        End If

        ''Buscar los datos del proveedor en el controlador proveedores

        If fbcProveedor.Value <> "" Then

            Dim proveedores_ As List(Of AuxiliarProveedor) = GetVars("listaProveedoresOperativos_")

            Dim proveedorseleccionado_ = proveedores_.Where(Function(x) x.idtaxid.Equals(fbcProveedor.Value)).First

            ''AGREGAR EL PROVEEDOR SELECCIONADO A LA SESSION
            SetVars("ProveedorSeleccionado_", proveedorseleccionado_)

            'If proveedorseleccionado_._activo Then ProveedorOnline() Else ProveedorOffline()

            Dim datasource_ As New List(Of SelectOption)

            datasource_.Add(New SelectOption With
                         {.Value = proveedorseleccionado_._domicilio._iddomicilio.ToString,
                          .Text = proveedorseleccionado_._domicilio.domicilioPresentacion})

            ''REQUIERO EL DATASOURCE DE LOS DOMICILIOS DEL PROVEEDOR
            'datasource_ = _utils.EnlistarDomiciliosProveedor(proveedorseleccionado_._listadomiciliosconTaxid)

            scDomiciliosProveedor.DataSource = datasource_

            If scDomiciliosProveedor.DataSource.Count = 1 Then

                scDomiciliosProveedor.Value = datasource_.Last.Value

                _domicilioSeleccionadoProveedor = proveedorseleccionado_._domicilio

                ''AGREGAR EL PROVEEDOR SELECCIONADO A LA SESSION
                SetVars("DomicilioProveedorSeleccionado_", _domicilioSeleccionadoProveedor)


            End If

            If fbcCliente.Value <> "" Then

                ''OBTENER VINCULACIÓN ENTRE CLIENTE Y PROVEEDOR
                Dim vinculacionesDisponibles_ = proveedorseleccionado_._listavinculaciones

                If vinculacionesDisponibles_ IsNot Nothing Then

                    If vinculacionesDisponibles_.Count > 0 Then

                        Dim vinculacionencontrada_ = vinculacionesDisponibles_.
                        Where(Function(x) x.idProveedor.Equals(fbcProveedor.Value) And x.idClienteVinculado.Equals(New ObjectId(fbcCliente.Value))).
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

                'OBTENER METODO DE VALORACIÓN ENTRE CLIENTE Y PROVEEDOR
                Dim metodovaloracionDisponibles_ = proveedorseleccionado_._listaconfiguracionesadicionales

                If metodovaloracionDisponibles_ IsNot Nothing Then

                    If metodovaloracionDisponibles_.Count > 0 Then

                        Dim metodoencontrado_ = metodovaloracionDisponibles_.
                         Where(Function(x) x.idProveedor.Equals(fbcProveedor.Value) And x.idclienteConfiguracion.Equals(New ObjectId(fbcCliente.Value))).
                         Select(Function(y) New With {Key .cvemetodovaloracion_ = y.idmetodovaloracion, Key .metodovaloracion_ = y.metodovaloracion}).ToList()

                        If metodoencontrado_.Count > 0 Then

                            scMetodoValoracion.DataSource = New List(Of SelectOption)

                            Dim opcionesmetodo_ As New List(Of SelectOption)

                            For Each item_ In metodoencontrado_

                                opcionesmetodo_.Add(New SelectOption With {.Value = item_.cvemetodovaloracion_,
                                                                            .Text = item_.metodovaloracion_})

                            Next

                            scMetodoValoracion.DataSource = opcionesmetodo_

                            scMetodoValoracion.Value = metodoencontrado_(0).cvemetodovaloracion_

                        End If

                    End If

                End If

            End If

            'DisplayMessage("Proveedor establecido correctamente", StatusMessage.Info)

        Else
            ''FALTA ELIMINAR EL TOOLTIP

            scDomiciliosProveedor.Value = Nothing

            scDomiciliosProveedor.DataSource = Nothing

            ''QUITAR VINCULACION Y MÉTODO DE VALORACIÓN Y SI FUNGE

            scVinculacion.Value = Nothing

            scMetodoValoracion.Value = Nothing

            swcFungeCertificado.Checked = False

            fbcProveedorCertificado.Value = Nothing

            SetVars("ProveedorSeleccionado_", Nothing)

            SetVars("DomicilioProveedorSeleccionado_", Nothing)

            DisplayMessage("Proveedor no disponible", StatusMessage.Warning)

        End If

    End Sub

    Protected Sub fbcProveedorCertificado_TextChanged(sender As Object, e As EventArgs)

        Dim tagwatcher_ = Nothing

        Dim listaProveedoresOperativos_ As List(Of AuxiliarProveedor)

        Dim listaDataSource_ As New List(Of SelectOption)

        If fbcProveedorCertificado.Value = "" Then

            tagwatcher_ = _utils.ObtenerListaProveedoresOperativosPorControlador(fbcProveedorCertificado.Text)

            If tagwatcher_.Status = TypeStatus.Ok Then

                listaProveedoresOperativos_ = DirectCast(tagwatcher_.ObjectReturned, List(Of AuxiliarProveedor))

                If listaProveedoresOperativos_.Count > 0 Then

                    For Each item_ In listaProveedoresOperativos_

                        listaDataSource_.Add(New SelectOption With
                         {.Value = item_.id,
                          .Text = item_._razonsocial})

                    Next

                End If

            End If

            fbcProveedorCertificado.DataSource = listaDataSource_

        End If

    End Sub
#End Region

#Region "Vinculacion"
    Protected Sub scVinculacion_Click(sender As Object, e As EventArgs)

        scVinculacion.DataSource = _utils.Vinculacion()

    End Sub
#End Region

#Region "Pais"

    'Protected Sub fbcPais_Click(sender As Object, e As EventArgs)

    '    If fbcPais.Value = "" Then

    '        CargarMonedaPorDefault()

    '    End If

    'End Sub

    'Protected Sub fbcPais_TextChanged(sender As Object, e As EventArgs)

    '    'CargaPaises(sender)
    '    fbcPais.DataSource = _utils.ObtenerListaPaises(fbcPais.Text)

    '    CargarMonedaPorDefault(objectIdPais_:=fbcPais.Value)

    'End Sub

    Protected Sub fbcPaisPartida_TextChanged(sender As Object, e As EventArgs)

        Dim listaPaises_ = _utils.ObtenerListaPaises(fbcPaisPartida.Text)

        If listaPaises_.Count > 0 Then

            fbcPaisPartida.DataSource = listaPaises_

        Else

            AvisoRegistroNoEncontrado(New List(Of Object) From {fbcPaisPartida})

        End If

    End Sub

    Protected Sub fbcPaisPartida_Click(sender As Object, e As EventArgs)


    End Sub
#End Region

#Region "Unidades de medida"
    Protected Sub scUnidadMedidaComercial_TextChanged(sender As Object, e As EventArgs)

        _utils.CargaUnidades(scUnidadMedidaComercial, ControladorUnidadesMedida.TiposUnidad.Comercial)

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

    Private Sub MensajeValorMercanciaMayor()

        icValorMercancia.ToolTip = $"{icValorMercancia.Label} no puede ser mayor al {icValorFactura.Label}"

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

    Private Sub ConfiguracionZero(ByVal object_ As Object)

        object_.ToolTip = $"{ object_.Label} debe ser mayor a 0"

        object_.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors

        object_.ToolTipExpireTime = 60

        object_.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

        object_.ShowToolTip()

    End Sub

    Private Sub ConfiguracionRequerido(ByVal object_ As Object)

        object_.ToolTip = $"{ object_.Label} es requerido"

        object_.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors

        object_.ToolTipExpireTime = 60

        object_.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

        object_.ShowToolTip()

    End Sub


#End Region

#Region "Pillbox"
    Protected Sub pbPartidas_CheckedChange(sender As Object, e As EventArgs)

        Dim tipoCaptura_ = GetVars("_tipoCaptura")

        Select Case pbPartidas.ToolbarAction

            Case PillboxControl.ToolbarActions.Nuevo

                lbNumero.Text = (pbPartidas.PageIndex).ToString()

                CargarMetodoValoracion(scMetodoValoracionPartida)

                scMetodoValoracionPartida.Value = scMetodoValoracion.Value

            Case PillboxControl.ToolbarActions.Anterior

                lbNumero.Text = (pbPartidas.PageIndex).ToString()

                CargarMetodoValoracion(scMetodoValoracionPartida)

                AvisosVerificacionObjectIdValido()

            Case PillboxControl.ToolbarActions.Siguiente

                lbNumero.Text = (pbPartidas.PageIndex).ToString()

                CargarMetodoValoracion(scMetodoValoracionPartida)

                AvisosVerificacionObjectIdValido()

        End Select

    End Sub

    Protected Sub pbPartidas_Click(sender As Object, e As EventArgs)
        ''QUIZAS ESTE NO FUNCIONE AQUI
        Select Case pbPartidas.ToolbarAction

            Case PillboxControl.ToolbarActions.Nuevo

                lbNumero.Text = (pbPartidas.PageIndex).ToString()

                CargarMetodoValoracion(scMetodoValoracionPartida)

                scMetodoValoracionPartida.Value = scMetodoValoracion.Value

                fbcPaisPartida.Value = pbPartidas.DataSource(0).Item("fbcPaisPartida").Item("Value")

                fbcPaisPartida.Text = pbPartidas.DataSource(0).Item("fbcPaisPartida").Item("Text")

                scMonedaFacturaPartida.DataSource = New List(Of SelectOption) _
                    From {New SelectOption With {.Value = pbPartidas.DataSource(0).Item("scMonedaFacturaPartida")("Value"), .Text = pbPartidas.DataSource(0).Item("scMonedaFacturaPartida")("Text")}}

                scMonedaFacturaPartida.Value = pbPartidas.DataSource(0).Item("scMonedaFacturaPartida")("Value")

                scMonedaMercanciaPartida.DataSource = New List(Of SelectOption) _
                    From {New SelectOption With {.Value = pbPartidas.DataSource(0).Item("scMonedaMercanciaPartida")("Value"), .Text = pbPartidas.DataSource(0).Item("scMonedaMercanciaPartida")("Text")}}

                scMonedaMercanciaPartida.Value = pbPartidas.DataSource(0).Item("scMonedaMercanciaPartida")("Value")

                scMonedaPrecioUnitarioPartida.DataSource = New List(Of SelectOption) _
                    From {New SelectOption With {.Value = pbPartidas.DataSource(0).Item("scMonedaPrecioUnitarioPartida")("Value"), .Text = pbPartidas.DataSource(0).Item("scMonedaPrecioUnitarioPartida")("Text")}}

                scMonedaPrecioUnitarioPartida.Value = pbPartidas.DataSource(0).Item("scMonedaPrecioUnitarioPartida")("Value")

            Case PillboxControl.ToolbarActions.Anterior

                lbNumero.Text = (pbPartidas.PageIndex).ToString()

                CargarMetodoValoracion(scMetodoValoracionPartida)

                AvisosVerificacionObjectIdValido()

            Case PillboxControl.ToolbarActions.Siguiente

                lbNumero.Text = (pbPartidas.PageIndex).ToString()

                CargarMetodoValoracion(scMetodoValoracionPartida)

                AvisosVerificacionObjectIdValido()

        End Select

    End Sub

#End Region

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
        '        MostrarMonedaCompleta(scMonedaFacturaPartida, datosmoneda_.nombremonedaesp)
        '        MostrarMonedaCompleta(scMonedaMercanciaPartida, datosmoneda_.nombremonedaesp)
        '        MostrarMonedaCompleta(scMonedaPrecioUnitarioPartida, datosmoneda_.nombremonedaesp)
        '        MostrarMonedaCompleta(scMonedaFletes, datosmoneda_.nombremonedaesp)
        '        MostrarMonedaCompleta(scMonedaSeguros, datosmoneda_.nombremonedaesp)
        '        MostrarMonedaCompleta(scMonedaEmbalajes, datosmoneda_.nombremonedaesp)
        '        MostrarMonedaCompleta(scMonedaOtrosIncrementables, datosmoneda_.nombremonedaesp)

        '    End If

        'Catch ex As Exception

        '    DisplayMessage("Favor de intentarlo más tarde", StatusMessage.Warning)

        'End Try

    End Sub


    Protected Sub scMonedaMercancia_Click(sender As Object, e As EventArgs)
        _utils.BusquedaMonedas(sender)
    End Sub

    Protected Sub scMonedaMercancia_SelectedIndexChanged(sender As Object, e As EventArgs)
        'If scMonedaMercancia.Value <> "" Then
        '    MostrarNombreMoneda(scMonedaMercancia.Value, scMonedaMercancia)
        'End If
    End Sub

    Protected Sub scMonedaFacturaPartida_Click(sender As Object, e As EventArgs)
        _utils.BusquedaMonedas(sender)
    End Sub

    Protected Sub scMonedaFacturaPartida_SelectedIndexChanged(sender As Object, e As EventArgs)
        'If scMonedaFacturaPartida.Value <> "" Then
        '    MostrarNombreMoneda(scMonedaFacturaPartida.Value, scMonedaFacturaPartida)
        'End If
    End Sub

    Protected Sub scMonedaMercanciaPartida_Click(sender As Object, e As EventArgs)
        _utils.BusquedaMonedas(sender)
    End Sub

    Protected Sub scMonedaMercanciaPartida_SelectedIndexChanged(sender As Object, e As EventArgs)
        'If scMonedaMercanciaPartida.Value <> "" Then
        '    MostrarNombreMoneda(scMonedaMercanciaPartida.Value, scMonedaMercanciaPartida)
        'End If
    End Sub

    Protected Sub scMonedaPrecioUnitarioPartida_Click(sender As Object, e As EventArgs)
        _utils.BusquedaMonedas(sender)
    End Sub

    Protected Sub scMonedaPrecioUnitarioPartida_SelectedIndexChanged(sender As Object, e As EventArgs)
        'If scMonedaPrecioUnitarioPartida.Value <> "" Then
        '    MostrarNombreMoneda(scMonedaPrecioUnitarioPartida.Value, scMonedaPrecioUnitarioPartida)
        'End If
    End Sub

    Protected Sub scMonedaSeguros_Click(sender As Object, e As EventArgs)
        _utils.BusquedaMonedas(sender)
    End Sub

    Protected Sub scMonedaSeguros_SelectedIndexChanged(sender As Object, e As EventArgs)
        'If scMonedaSeguros.Value <> "" Then
        '    MostrarNombreMoneda(scMonedaSeguros.Value, scMonedaSeguros)
        'End If
    End Sub

    Protected Sub scMonedaEmbalajes_Click(sender As Object, e As EventArgs)
        _utils.BusquedaMonedas(sender)
    End Sub

    Protected Sub scMonedaEmbalajes_SelectedIndexChanged(sender As Object, e As EventArgs)
        'If scMonedaEmbalajes.Value <> "" Then
        '    MostrarNombreMoneda(scMonedaEmbalajes.Value, scMonedaEmbalajes)
        'End If
    End Sub

    Protected Sub scMonedaOtrosIncrementables_Click(sender As Object, e As EventArgs)
        _utils.BusquedaMonedas(sender)
    End Sub

    Protected Sub scMonedaOtrosIncrementables_SelectedIndexChanged(sender As Object, e As EventArgs)
        'If scMonedaOtrosIncrementables.Value <> "" Then
        '    MostrarNombreMoneda(scMonedaOtrosIncrementables.Value, scMonedaOtrosIncrementables)
        'End If
    End Sub

    'Protected Sub scMonedaDescuentos_Click(sender As Object, e As EventArgs)
    '    _utils.BusquedaMonedas(sender)
    'End Sub

    'Protected Sub scMonedaDescuentos_SelectedIndexChanged(sender As Object, e As EventArgs)
    '    If scMonedaDescuentos.Value <> "" Then
    '        MostrarNombreMoneda(scMonedaDescuentos.Value, scMonedaDescuentos)
    '    End If
    'End Sub


    Protected Sub scMonedaFletes_SelectedIndexChanged(sender As Object, e As EventArgs)
        'If scMonedaFletes.Value <> "" Then
        '    MostrarNombreMoneda(scMonedaFletes.Value, scMonedaFletes)
        'End If
    End Sub

    Protected Sub scMonedaFletes_Click(sender As Object, e As EventArgs)
        _utils.BusquedaMonedas(sender)
    End Sub

    Protected Sub MostrarNombreMoneda(ByVal objectidMoneda As String, ByVal object_ As Object)

        ''VAMOS A CACHEARLO POR SI SIEMPRE ES LA MISMA PARA NO ESTAR HACIENDO HITS A LO BRUTO :V

        Dim cacheKey_ = "cacheNombreMonedaCompleto"

        Dim cacheData_ = CType(HttpRuntime.Cache(cacheKey_), Dictionary(Of String, String))

        ' Si no hay cache O si el objectId cambió, ir a Mongo
        If cacheData_ Is Nothing OrElse cacheData_("objectId") <> objectidMoneda.ToString() Then

            _utils = New UtilsFacturaComercial

            Dim estadoMoneda_ = _utils.ObtenerDatosMoneda(ObjectId.Parse(objectidMoneda))

            If estadoMoneda_.Status = TypeStatus.Ok Then

                Dim datosmoneda_ = DirectCast(estadoMoneda_.ObjectReturned, MonedaGlobal)

                Dim nuevoCacheData_ = New Dictionary(Of String, String) From {
                    {"objectId", objectidMoneda.ToString()},
                    {"nombreMoneda", datosmoneda_.nombremonedaesp}
                }

                HttpRuntime.Cache.Insert(cacheKey_, nuevoCacheData_, Nothing,
                                         DateTime.Now.AddMinutes(5),
                                         Caching.Cache.NoSlidingExpiration)

                MostrarMonedaCompleta(object_, datosmoneda_.nombremonedaesp)

            End If

        Else

            Dim nombreMoneda_ As String = cacheData_("nombreMoneda")

            ' Dim objectId_ As String = cacheData_("objectId")

            MostrarMonedaCompleta(object_, nombreMoneda_)

        End If

    End Sub

#End Region


#Region "Número de parte"
    Protected Sub fbcProducto_TextChanged(sender As Object, e As EventArgs)

        If fbcProducto.Value = "" Then

            Dim estado_ As TagWatcher

            estado_ = _utils.BuscarProductos(fbcProducto.Text.ToUpper(), fbcCliente.Value, fbcProveedor.Value)

            If estado_ IsNot Nothing AndAlso estado_.Status = TypeStatus.Ok Then

                ListaProductosEncontrados(estado_.ObjectReturned)

            Else

                DisplayMessage("Producto no encontrado", StatusMessage.Fail)

            End If

        End If

    End Sub

    Protected Sub fbcProducto_Click(sender As Object, e As EventArgs)

        ''REVISA AQUI PORFI
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
                icObjectIdPartida.Value = productoseleccionado_.id.ToString()

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
            icObjectIdPartida.Value = Nothing
            icDescripcionPartida.Value = Nothing
            icDescripcionPartidaOriginal.Value = Nothing
            icDescripcionCOVE.Value = Nothing
            icFraccionArancelaria.DataSource = Nothing
            icFraccionNico.Value = Nothing
            icFraccionNico.DataSource = Nothing
            scUnidadMedidaTarifa.Value = Nothing
            scUnidadMedidaTarifa.DataSource = Nothing
        End If

        'If fbcProducto.Text <> "" Then

        '    If fbcProducto.Value <> "" Then

        '        Dim listaProductos_ = DirectCast(GetVars("ListaProductos"), List(Of AuxiliarProducto))

        '        Dim productoText_ = fbcProducto.Text.Split("|")

        '        Dim idKrom_ = Integer.Parse(productoText_(0))

        '        Dim idproducto_ = fbcProducto.Value

        '        'Dim productoseleccionado_ = listaProductos_.FirstOrDefault(Function(x) x.id.ToString() = idproducto_)
        '        Dim productoseleccionado_ = listaProductos_.FirstOrDefault(Function(x) x.id.ToString() = idproducto_ AndAlso x._idKrom = idKrom_)

        '        Dim listaAuxiliarProductos_ As List(Of AuxiliarProducto)

        '        If GetVars("ListaAuxliarProductos_") IsNot Nothing Then

        '            listaAuxiliarProductos_ = DirectCast(GetVars("ListaAuxliarProductos_"), List(Of AuxiliarProducto))

        '        Else

        '            listaAuxiliarProductos_ = New List(Of AuxiliarProducto)

        '        End If

        '        Dim auxiliarProducto_ = New AuxiliarProducto

        '        With productoseleccionado_

        '            If productoseleccionado_._idKrom = idKrom_ Then

        '                icDescripcionPartidaOriginal.Value = ._descripcion
        '                icDescripcionPartida.Value = ._nombrecomercial
        '                icDescripcionCOVE.Value = ._descripcioncove

        '            End If


        '            icObjectIdPartida.Value = .id.ToString

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
        '                                 {.Value = productoseleccionado_._cveunidadmedida,
        '                                  .Text = $"{productoseleccionado_._cveunidadmedida} - {productoseleccionado_._unidadmedidapresentacion}"}}

        '                scUnidadMedidaTarifa.Value = productoseleccionado_._cveunidadmedida

        '            End If

        '            auxiliarProducto_.id = .id

        '            auxiliarProducto_._idKrom = ._idKrom ''DEBE SER LA SECUENCIA

        '            auxiliarProducto_._timestamp = ._timestamp

        '            coTimeStamp.Value = ._timestamp

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

        '    icObjectIdPartida.Value = Nothing

        '    icDescripcionPartida.Value = Nothing

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

    Protected Sub ListaProductosEncontrados(ByVal productosEncontrados_ As List(Of AuxiliarProducto))

        Dim listaDataSource_ As New List(Of SelectOption)
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

            DisplayMessage("Producto no disponible", StatusMessage.Warning)

        End If

    End Sub

    Sub MostrarBotones(ByVal sender_ As ButtonbarControl,
                                  ByVal e As EventArgs)

        If OperacionGenerica IsNot Nothing Then

            If Not OperacionGenerica.Publicado Then

                btiPublicar.Enabled = True


            End If

        End If

    End Sub

    Protected Sub dbcNumFacturaCOVE_Click(sender As Object, e As EventArgs)

    End Sub

#End Region

#Region "Domicilios proveedores"
    Protected Sub swcFungeCertificado_CheckedChanged(sender As Object, e As EventArgs)

        If swcFungeCertificado.Checked Then

            fbcProveedorCertificado.Enabled = False

            fbcProveedorCertificado.Text = Nothing

            fbcProveedorCertificado.Value = Nothing

        Else

            fbcProveedorCertificado.Enabled = True

        End If

    End Sub

    Protected Sub scDomiciliosProveedor_Click(sender As Object, e As EventArgs)

    End Sub

    Protected Sub scDomiciliosProveedor_SelectedIndexChanged(sender As Object, e As EventArgs)

        If scDomiciliosProveedor.Value <> "" Then

            If GetVars("ProveedorSeleccionado_") IsNot Nothing Then

                Dim proveedorSeleccionado_ = DirectCast(GetVars("ProveedorSeleccionado_"), AuxiliarProveedor)

                _domicilioSeleccionadoProveedor = proveedorSeleccionado_._listadomiciliosconTaxid.
                                                  Where(Function(x) x._iddomicilio = New ObjectId(scDomiciliosProveedor.Value)).
                                                  AsEnumerable().
                                                  Last

                ''AGREGAR EL PROVEEDOR SELECCIONADO A LA SESSION
                SetVars("DomicilioProveedorSeleccionado_", _domicilioSeleccionadoProveedor)

            End If

        End If

    End Sub

    Protected Sub scDomiciliosProveedor_OnTextChanged(sender As Object, e As EventArgs)

        If scDomiciliosProveedor.Value <> "" Then

            If GetVars("ProveedorSeleccionado_") IsNot Nothing Then

                Dim proveedorSeleccionado_ = DirectCast(GetVars("ProveedorSeleccionado_"), AuxiliarProveedor)

                _domicilioSeleccionadoProveedor = proveedorSeleccionado_._listadomiciliosconTaxid.
                                                  Where(Function(x) x._iddomicilio = New ObjectId(scDomiciliosProveedor.Value))

                ''AGREGAR EL PROVEEDOR SELECCIONADO A LA SESSION
                SetVars("DomicilioProveedorSeleccionado_", _domicilioSeleccionadoProveedor)

            End If

        End If

    End Sub
#End Region

#Region "Monedas"
    Protected Sub CargaMoneda_Click(sender As Object, e As EventArgs)

        _utils.BusquedaMonedas(sender)

    End Sub

    Protected Sub CargarMonedaPorDefault(Optional ByVal moneda_ As String = "USD",
                                         Optional ByVal objectIdPais_ As String = Nothing)

        _resultMoneda = New List(Of MonedaGlobal)

        _resultadoMonedaPais = New List(Of moneda)

        _utils = New UtilsFacturaComercial

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

                'MostrarMonedaCompleta(scMonedaFactura, _resultadoMonedaPais(0).nombremoneda)
                'MostrarMonedaCompleta(scMonedaMercancia, _resultadoMonedaPais(0).nombremoneda)
                'MostrarMonedaCompleta(scMonedaFacturaPartida, _resultadoMonedaPais(0).nombremoneda)
                'MostrarMonedaCompleta(scMonedaMercanciaPartida, _resultadoMonedaPais(0).nombremoneda)
                'MostrarMonedaCompleta(scMonedaPrecioUnitarioPartida, _resultadoMonedaPais(0).nombremoneda)
                'MostrarMonedaCompleta(scMonedaFletes, _resultadoMonedaPais(0).nombremoneda)
                'MostrarMonedaCompleta(scMonedaSeguros, _resultadoMonedaPais(0).nombremoneda)
                'MostrarMonedaCompleta(scMonedaEmbalajes, _resultadoMonedaPais(0).nombremoneda)
                'MostrarMonedaCompleta(scMonedaOtrosIncrementables, _resultadoMonedaPais(0).nombremoneda)
                'MostrarMonedaCompleta(scMonedaDescuentos, _resultadoMonedaPais(0).nombremoneda)

            End If

        End If

        _utils.LLenarMonedas(_dataSourceMoneda, _listaCamposMonedas)

    End Sub

#End Region

#Region "CARGA POR IA"
    Protected Function ListarDomicilios(domiciliosSeccion_ As List(Of Nodo)) As List(Of Rec.Globals.Empresas.Domicilio)

        _listaDomicilios = New List(Of Rec.Globals.Empresas.Domicilio)

        If domiciliosSeccion_ IsNot Nothing Then

            If domiciliosSeccion_.Count > 0 Then

                For Each nodo_ In domiciliosSeccion_

                    _domicilioAux = New Rec.Globals.Empresas.Domicilio

                    For Each item_ In nodo_.Nodos

                        Dim campo_ As Campo = Nothing

                        campo_ = DirectCast(item_.Nodos(0), Campo)

                        With _domicilioAux

                            Select Case campo_.IDUnico

                                Case CamposDomicilio.CA_CALLE

                                    .calle = campo_.Valor

                                Case CamposProveedorOperativo.CA_DOMICILIO_FISCAL

                                    .domicilioPresentacion = campo_.Valor

                                Case CamposDomicilio.CA_NUMERO_EXTERIOR

                                    .numeroexterior = campo_.Valor

                                Case CamposDomicilio.CA_NUMERO_INTERIOR

                                    .numerointerior = campo_.Valor

                                Case CamposDomicilio.CA_CIUDAD

                                    .ciudad = campo_.Valor

                                Case CamposDomicilio.CA_LOCALIDAD

                                    .localidad = campo_.Valor

                                Case CamposDomicilio.CA_COLONIA

                                    .colonia = campo_.Valor

                                Case CamposDomicilio.CA_CODIGO_POSTAL

                                    .codigopostal = campo_.Valor

                                Case CamposDomicilio.CA_ENTIDAD_FEDERATIVA

                                    .entidadfederativa = campo_.Valor

                                Case CamposDomicilio.CA_ENTIDAD_MUNICIPIO

                                    .municipio = campo_.Valor

                                Case CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA

                                    .cveEntidadfederativa = campo_.Valor

                                Case CamposDomicilio.CA_ENTIDAD_MUNICIPIO

                                    .cveMunicipio = campo_.Valor

                                Case CamposProveedorOperativo.CP_ID_DOMICILIO_PROVEEDOR

                                    ._iddomicilio = New ObjectId(campo_.Valor.ToString)

                                Case CamposProveedorOperativo.CP_SEC_DOMICILIO_PROVEEDOR

                                    .sec = campo_.Valor

                            End Select

                        End With

                    Next

                    _listaDomicilios.Add(_domicilioAux)
                Next

            End If

        End If

        Return _listaDomicilios

    End Function

    Protected Sub MostrarDescripciones(ByVal texto_ As String)
        icFraccionNico.ToolTip = texto_
        icFraccionNico.ToolTipStatus = IUIControl.ToolTipTypeStatus.Ok
        icFraccionArancelaria.ToolTipExpireTime = 6
        icFraccionNico.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
        icFraccionNico.ShowToolTip()
    End Sub

    'Private Sub MostrarTarjetaAlertaIA(info As String,
    '                          score As String,
    '                          fehaprocesamiento As String,
    '                          advertencias As String)
    '    lbinforme.Text = info
    '    lbConfiabilidad.Text = score
    '    'lbProceso.Text = fehaprocesamiento
    '    lbAdvertencia.Text = advertencias
    '    aviso.Visible = True
    'End Sub

    'Private Sub OcultarTarjetaAlertaIA()
    '    lbinforme.Text = Nothing
    '    lbConfiabilidad.Text = Nothing
    '    ' lbProceso.Text = Nothing
    '    lbAdvertencia.Text = Nothing
    '    aviso.Visible = False
    'End Sub

    ''REVISAR ES PARA IA
    Protected Sub CargarProveedor(ByVal datosproveedor_ As Supplier)
        '(IA)
        'With datosproveedor_

        '    scDomiciliosProveedor.DataSource = Nothing

        '    _controladorProveedores = New CtrlProveedoresOperativos()

        '    _buscarProveedor = New TagWatcher

        '    _buscarProveedor = _controladorProveedores.ConsultarOne(datosproveedor_.supliername, datosproveedor_.country, datosproveedor_.street, datosproveedor_.zipcode)

        '    If _buscarProveedor.Status = TypeStatus.Ok Then

        '        If _buscarProveedor.ObjectReturned IsNot Nothing Then

        '            _proveedorObtenido = New AuxiliarProveedor

        '            _proveedorObtenido = _buscarProveedor.ObjectReturned

        '            _datosReceptorProveedor = New List(Of Dictionary(Of String, String))

        '            _esDestinatario = New Boolean

        '            _esDestinatario = True

        '            With _proveedorObtenido

        '                _listaProveedores = New Dictionary(Of String, String) From {
        '                {"RFC_", ._rfc},
        '                {"CURP_", ._curp},
        '                {"CvePais_", ._cvepais},
        '                {"Pais_", ._pais},
        '                {"ObjectIdDomicilio_", ._domicilio._iddomicilio.ToString},
        '                {"RazonSocial_", ._razonsocial},
        '                {"ObjectId_", .id},
        '                {"Cve_", ._clave}}

        '                _esDestinatario = IIf(._esdestinatario = "1", True, False)

        '                _datosReceptorProveedor.Add(_listaProveedores)

        '                _listaDomiciliosProveedores = New List(Of Rec.Globals.Empresas.Domicilio) From {_proveedorObtenido._domicilio}

        '                SetVars("_listaDomiciliosProveedores", _listaDomiciliosProveedores)

        '                SetVars("_datosReceptorProveedor", _datosReceptorProveedor)

        '                fbcProveedor.Value = ._razonsocial

        '                _lista.Add(New SelectOption With
        '                             {.Value = _proveedorObtenido._domicilio._iddomicilio.ToString,
        '                              .Text = _proveedorObtenido._domicilio.domicilioPresentacion})

        '                scDomiciliosProveedor.DataSource = _lista

        '                scDomiciliosProveedor.Value = _lista.Last.Value

        '            End With

        '        End If

        '    Else
        '        _lista.Add(New SelectOption With
        '                     {.Value = datosproveedor_.address,
        '                      .Text = datosproveedor_.address})

        '        fbcProveedor.Value = datosproveedor_.supliername

        '        scDomiciliosProveedor.DataSource = _lista

        '        scDomiciliosProveedor.Value = _lista.Last.Value

        '        If icTipoCargaDatos.Value = "1" Then

        '            AvisoRegistroNoEncontrado(New List(Of Object) From {fbcProveedor, scDomiciliosProveedor})

        '        End If

        '    End If

        'End With

    End Sub

    Protected Sub CargarPais(ByVal pais_ As String, ByVal control_ As FindboxControl)

        _buscarPais = New List(Of SelectOption)

        _buscarPais = ControladorPaises.BuscarPaises(New List(Of Pais), pais_)

        If _buscarPais.Count > 0 Then

            control_.DataSource = Nothing

            control_.Value = _buscarPais(0).Value

            control_.Text = _buscarPais(0).Text

        Else

            'If icTipoCargaDatos.Value = "1" Then

            '    _utils.AvisoVerificacionManual(New List(Of Object) From {control_})

            'End If

        End If

    End Sub

    Protected Sub CargarPaisProducto(ByVal pais_ As String)

        If GetVars("busquedapaisproducto") IsNot Nothing Then

            If GetVars("busquedapaisproducto") = True Then

                SetVars("busquedapaisproducto", True)

                'fbcPaisPartida.Value = GetVars("idPais")

                'fbcPaisPartida.Text = GetVars("textPais")

            End If

        Else

            _lista = ControladorPaises.BuscarPaises(New List(Of Pais), pais_)

            If _lista.Count > 0 Then

                SetVars("busquedapaisproducto", True)

                'fbcPaisPartida.DataSource = Nothing

                'fbcPaisPartida.Value = _lista(0).Value

                'fbcPaisPartida.Text = _lista(0).Text

                SetVars("idPais", _lista(0).Value)

                SetVars("textPais", _lista(0).Text)

            Else

                'If icTipoCargaDatos.Value = "1" Then

                '    _utils.AvisoVerificacionManual(New List(Of Object) From {fbcPaisPartida})

                'End If

            End If

        End If

    End Sub

    Protected Sub CargarProductos(ByVal items_ As List(Of Item),
                                  ByVal razonSocialCliente As String,
                                  ByVal razonSocialProveedor As String)

        _numeroparteItem = fbcProducto.Value

        _descripcionItem = icDescripcionPartida.Value

        CargarPaisProducto(items_(0).origincountry)

        _controladorProductos = New ControladorProductos

        Dim listaNumeroParteProductos_ As New List(Of String)

        Dim listaDescripcionesProductos_ As New List(Of String)

        For Each item_ In items_

            If item_.partnumber <> "" Then

                listaNumeroParteProductos_.Add(item_.partnumber)

            Else

                If item_.description <> "" Then

                    listaDescripcionesProductos_.Add(item_.description)

                End If

            End If

        Next

        If listaNumeroParteProductos_.Count > 0 Then

            Dim resultNumeroParte_ As TagWatcher = _controladorProductos.BuscarProductosPorDescripcion(listaNumeroParteProductos_, fbcCliente.Text, fbcProveedor.Text)

            If resultNumeroParte_.Status <> TypeStatus.Ok Then

                Dim aqui = resultNumeroParte_.ObjectReturned

            Else

                AvisoRegistroNoEncontrado(New List(Of Object) From {fbcProducto, icDescripcionPartida, scUnidadMedidaComercial})

            End If

        End If

        If listaDescripcionesProductos_.Count > 0 Then

            Dim resultDescripciones_ As TagWatcher = _controladorProductos.BuscarProductosPorDescripcion(listaDescripcionesProductos_, fbcCliente.Text, fbcProveedor.Text)

            If resultDescripciones_.Status <> TypeStatus.Ok Then

                Dim aqui = resultDescripciones_.ObjectReturned

            Else

                AvisoRegistroNoEncontrado(New List(Of Object) From {fbcProducto, icDescripcionPartida, scUnidadMedidaComercial})

            End If

        End If

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


    Protected Sub CargarMetodoValoracion(ByRef control_ As SelectControl)

        Dim cacheLista_ = New List(Of SelectOption)

        cacheLista_ = CType(HttpRuntime.Cache("cacheListaMetodoValoracion"), List(Of SelectOption))

        If cacheLista_ Is Nothing Then

            _tagwatcher = Nothing

            _tagwatcher = New TagWatcher

            _tagwatcher = ControladorFacturaComercial.ObtenerListaMetodoValoracion()

            If _tagwatcher.Status = TypeStatus.Ok Then

                Dim datasource_ As New List(Of SelectOption)

                ' Nos aseguramos de que ObjectReturned sea una lista de BsonDocument
                Dim listaDocumentos_ = TryCast(_tagwatcher.ObjectReturned, List(Of BsonDocument))


                If listaDocumentos_ IsNot Nothing AndAlso listaDocumentos_.Count > 0 Then

                    For Each item_ As BsonDocument In listaDocumentos_

                        ' Accedemos a los valores por el nombre de la llave en el BsonDocument
                        Dim clave_ = item_("i_ClaveMetodoValoracion").ToInt32()
                        Dim descripcion_ = item_("t_ClaveDescripcion").ToString()

                        datasource_.Add(New SelectOption With {
                        .Value = clave_,
                        .Text = descripcion_
                    })

                    Next

                End If

                HttpRuntime.Cache.Insert("cacheListaMetodoValoracion", datasource_, Nothing, DateTime.Now.AddMinutes(5), Caching.Cache.NoSlidingExpiration)

                cacheLista_ = datasource_

            End If

        End If

        control_.DataSource = cacheLista_

        'scMetodoValoracionPartida.Value = scMetodoValoracion.Value

    End Sub

    Protected Sub AvisoRegistroNoEncontrado(ByVal listaCampos_ As List(Of Object))
        'Dim camposFacturaUI_ As New Dictionary(Of Object, String) From {
        '    {icValorFactura, "valor factura"},
        '    {dbcNumFacturaCOVE, "número factura"},
        '    {icFechaFactura, "fecha factura"},
        '    {fbcProveedor, "proveedor"},
        '    {scDomiciliosProveedor, "domicilio proveedor"},
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

    Protected Sub AvisoMensajesIA(ByVal campoUI_ As Object, ByVal mensaje_ As String)
        'Dim camposFacturaUI_ As New Dictionary(Of String, Object) From {
        '    {"totalinvoice", icValorFactura},
        '    {"invoicenumber", dbcNumFacturaCOVE},
        '    {"invoicedate", icFechaFactura}
        '}

        'If camposFacturaUI_.ContainsKey(campo_) Then
        '    Dim campoUI_ = camposFacturaUI_(campo_)
        '    campoUI_.ToolTip = $"IA 👉: {mensaje_}"
        '    campoUI_.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors
        '    campoUI_.ToolTipExpireTime = 10000
        '    campoUI_.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
        '    campoUI_.ShowToolTip()
        'End If

        'Dim campoUI_ = camposFacturaUI_(campo_)
        'icSerieFolioFactura.ToolTip = $"🤖 {mensaje_}"
        'icSerieFolioFactura.ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors
        'icSerieFolioFactura.ToolTipExpireTime = 10000
        'icSerieFolioFactura.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
        'icSerieFolioFactura.ShowToolTip()
    End Sub
#End Region

#Region "Incrementables"
    Protected Function ComprobacionIncrementables() As Boolean

        If icFletes.Value IsNot Nothing Then

            If icFletes.Value <> "" Then

                If icFletes.Value > 0 Then

                    Return True

                End If

            End If

        End If

        If icSeguros.Value IsNot Nothing Then

            If icSeguros.Value <> "" Then

                If icSeguros.Value > 0 Then

                    Return True
                End If

            End If

        End If

        If icEmbalajes.Value IsNot Nothing Then

            If icEmbalajes.Value <> "" Then

                If icEmbalajes.Value > 0 Then

                    Return True
                End If

            End If

        End If

        If icOtrosIncrementables.Value IsNot Nothing Then

            If icOtrosIncrementables.Value <> "" Then

                If icOtrosIncrementables.Value > 0 Then

                    Return True

                End If

            End If

        End If

        Return False

    End Function
#End Region

#Region "TOOLTIPS - AVISOS"
    Protected Sub MsgValidacionRazonsocial()
        With fbcProveedor
            .ToolTip = "👉 Indique una razón social."
            .ToolTipExpireTime = 6
            .ToolTipStatus = IUIControl.ToolTipTypeStatus.OkInfo
            .ToolTipModality = IUIControl.ToolTipModalities.Ondemand
            .ShowToolTip()
        End With
    End Sub

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
            .ToolTip = "👉 Indique cliente"
            .ToolTipExpireTime = 6
            .ToolTipStatus = IUIControl.ToolTipTypeStatus.OkInfo
            .ToolTipModality = IUIControl.ToolTipModalities.Ondemand
            .ShowToolTip()
        End With

    End Sub

    Protected Sub MsgStatusFraccion(ByVal aviso_ As String)

        With icFraccionArancelaria
            .ToolTip = "🔵 " & aviso_
            .ToolTipExpireTime = 100
            .ToolTipStatus = IUIControl.ToolTipTypeStatus.OkInfo
            .ToolTipModality = IUIControl.ToolTipModalities.Ondemand
            .ShowToolTip()
        End With

    End Sub

    Protected Sub MostrarMonedaCompleta(ByVal campoMoneda_ As Object, ByVal moneda_ As String)

        campoMoneda_.ToolTip = moneda_
        campoMoneda_.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkInfo
        campoMoneda_.ToolTipModality = IUIControl.ToolTipModalities.Classic
        campoMoneda_.ShowToolTip()

    End Sub


#End Region



#Region "PUBLICAR"
    Private Function FirmarDocumentoPublicar() As Task(Of TagWatcher)
        ''AQUI ENTONCES VOY A VALIDAR TODO ANTES DE PUBLICAR
        Dim tagwatcher_ As TagWatcher = New TagWatcher

        Dim tipoMensaje_ As StatusMessage = StatusMessage.Info

        _utils = New UtilsFacturaComercial

        If swcSubdivision.Checked Then

            If OperacionGenerica IsNot Nothing Then

                ''ESTO ES PARA FIRMAR LA FACTURA
                Dim environmentid_ As Int32 = ListaEmpresas.Value

                tagwatcher_ = _utils.PublicarFacturaComercialAsync(DirectCast(OperacionGenerica, OperacionGenerica), Buscador,
                                                                   IControladorFacturaComercial.TipoOperaciones.Importacion, environmentid_, True).Result

                If tagwatcher_.Status = TypeStatus.OkInfo Then
                    tipoMensaje_ = StatusMessage.Success : tagwatcher_.SetOKInfo(Me, "✨ Factura comercial importación ha sido publicada")

                Else

                    tipoMensaje_ = StatusMessage.Fail : tagwatcher_.SetError(Me, "Factura no publicada")

                End If

            Else

                tipoMensaje_ = StatusMessage.Fail : tagwatcher_.SetError(Me, "Factura no publicada")

            End If

            '        PreparaBotonera(FormControl.ButtonbarModality.Protected)

            '        PreparaTarjetero(PillboxControl.ToolbarModality.Simple, pbPartidas)

        Else
            If OperacionGenerica IsNot Nothing Then

                ''ESTO ES PARA FIRMAR LA FACTURA
                Dim environmentid_ As Int32 = ListaEmpresas.Value

                tagwatcher_ = _utils.PublicarFacturaComercialAsync(DirectCast(OperacionGenerica, OperacionGenerica), Buscador,
                                                                   IControladorFacturaComercial.TipoOperaciones.Importacion, environmentid_).Result

                If tagwatcher_.Status = TypeStatus.OkInfo Then

                    tipoMensaje_ = StatusMessage.Success : tagwatcher_.SetOKInfo(Me, "🎉 Factura comercial importación ha sido publicada")

                Else
                    tipoMensaje_ = StatusMessage.Fail : tagwatcher_.SetError(Me, "Factura no publicada")

                End If

            Else

                tipoMensaje_ = StatusMessage.Fail : tagwatcher_.SetError(Me, "Factura no publicada")
            End If

        End If

        Return Task.FromResult(tagwatcher_)

    End Function
    Protected Sub scMetodoValoracion_Click(sender As Object, e As EventArgs)
        CargarMetodoValoracion(scMetodoValoracion)
    End Sub
    Protected Sub scMetodoValoracion_TextChanged(sender As Object, e As EventArgs)
        ' CargarMetodoValoracion()

        scMetodoValoracionPartida.Value = scMetodoValoracion.Value
    End Sub
    Protected Sub fbcProveedor_ClickClose(sender As Object, e As EventArgs)

        scDomiciliosProveedor.Value = Nothing
        scDomiciliosProveedor.DataSource = Nothing

        fbcProveedor.Value = Nothing
        fbcProveedor.DataSource = Nothing


        scVinculacion.Value = Nothing

        ''LLENAR DE NUEVO LA VINCULACION

        scVinculacion.DataSource = _utils.Vinculacion()

        ' scVinculacion.Value = 0

        scMetodoValoracion.Value = Nothing

        scMetodoValoracionPartida.Value = Nothing

        CargarMetodoValoracion(scMetodoValoracion)

        scMetodoValoracion.Value = 1

        CargarMetodoValoracion(scMetodoValoracionPartida)

        scMetodoValoracionPartida.Value = 1

        SetVars("listaProveedoresOperativos_", Nothing)

        SetVars("ProveedorSeleccionado_", Nothing)

        SetVars("DomicilioProveedorSeleccionado_", Nothing)

    End Sub

    Protected Sub fbcProducto_ClickClose(sender As Object, e As EventArgs)

        icObjectIdPartida.Value = Nothing
        fbcProducto.Value = Nothing
        fbcProducto.DataSource = Nothing

        icFraccionArancelaria.Value = Nothing
        icFraccionNico.Value = Nothing
        scUnidadMedidaTarifa.Value = Nothing
        icDescripcionPartidaOriginal.Value = Nothing
        icDescripcionPartida.Value = Nothing
        icDescripcionCOVE.Value = Nothing

        SetVars("ListaProductos", Nothing)

    End Sub

    Protected Sub ntPublicarFacturaComercial_Click(sender As Object, e As EventArgs)

        ntPublicarFacturaComercial.Visible = False

        DisplayMessage("Factura no publicada", StatusMessage.Info)

    End Sub

    Protected Sub ntPublicarFacturaComercial_ClickTwo(sender As Object, e As EventArgs)

        ntPublicarFacturaComercial.Visible = False

        If VerificarObjectIdValidoRegistros() Then

            If ComprobarSumaValorMercanciaItemsVSValorfacturaPartida() Then

                If ComprobarSumaValorMercanciaItemsVSValorMercanciaGeneral() Then

                    If ComprobarSumaValorFacturaItemsVSValorfactura() Then

                        If ComprobarValorMercanciaVSValorFactura() Then

                            Dim estatus_ = FirmarDocumentoPublicar()

                            If estatus_.Result.MessagesList.Count > 1 Then

                                If estatus_.Result.Status = TypeStatus.OkInfo Then

                                    DisplayMessage("✨ Factura comercial de importación publicada exitosamente", StatusMessage.Success)

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

                                    PreparaTarjetero(PillboxControl.ToolbarModality.Simple, pbPartidas)

                                Else

                                    DisplayMessage("No fue posible publicar la factura, revise la información", StatusMessage.Fail)

                                End If

                            End If

                        Else

                            MensajeValorMercanciaMayor()

                            DisplayMessage($"El valor de {icValorMercancia.Label} supera el valor de {icValorFactura.Label}", StatusMessage.Fail)

                        End If

                    Else

                        MensajeValorFacturaItemMayor()

                        DisplayMessage($"La suma total de {icValorfacturaPartida.Label} supera el valor de {icValorFactura.Label}", StatusMessage.Fail)

                    End If

                Else

                    MensajeValorMercanciaItemMayor()

                    DisplayMessage($"La suma de {icValorMercanciaPartida.Label} supera el valor de {icValorMercancia.Label}", StatusMessage.Fail)

                End If

            Else

                MensajeValorMercanciaItemValorFacturaMayor()

                DisplayMessage($"La suma total de {icValorMercanciaPartida.Label} supera la suma total de {icValorfacturaPartida.Label}", StatusMessage.Fail)

            End If

        Else

            AvisosVerificacionObjectIdValido()

            DisplayMessage("No fue posible publicar la factura, revise la información", StatusMessage.Fail)

        End If

    End Sub

    Private Sub MensajeValorMercanciaItemValorFacturaMayor()

        icValorMercanciaPartida.ToolTip = $"La suma total de {icValorMercanciaPartida.Label} supera la suma total de {icValorfacturaPartida.Label}"

        icValorMercanciaPartida.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkBut

        icValorMercanciaPartida.ToolTipExpireTime = 60

        icValorMercanciaPartida.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

        icValorMercanciaPartida.ShowToolTip()



        icValorfacturaPartida.ToolTip = $"{icValorfacturaPartida.Label}"

        icValorfacturaPartida.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkBut

        icValorfacturaPartida.ToolTipExpireTime = 60

        icValorfacturaPartida.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

        icValorfacturaPartida.ShowToolTip()

    End Sub

    Private Sub MensajeValorMercanciaItemMayor()

        icValorMercanciaPartida.ToolTip = $"{icValorMercanciaPartida.Label} no puede ser mayor al {icValorMercancia.Label}"

        icValorMercanciaPartida.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkBut

        icValorMercanciaPartida.ToolTipExpireTime = 60

        icValorMercanciaPartida.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

        icValorMercanciaPartida.ShowToolTip()

    End Sub

    Private Sub MensajeValorFacturaItemMayor()

        icValorfacturaPartida.ToolTip = $"La suma total de {icValorfacturaPartida.Label} supera el valor de {icValorFactura.Label}"

        icValorfacturaPartida.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkBut

        icValorfacturaPartida.ToolTipExpireTime = 60

        icValorfacturaPartida.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

        icValorfacturaPartida.ShowToolTip()


        icValorFactura.ToolTip = $"{icValorFactura.Label}"

        icValorFactura.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkBut

        icValorFactura.ToolTipExpireTime = 60

        icValorFactura.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

        icValorFactura.ShowToolTip()

    End Sub

    Private Function VerificarObjectIdValidoRegistros() As Boolean

        Dim objectidZero_ = "000000000000000000000000"

        If fbcCliente.Value = objectidZero_ OrElse fbcCliente.Value = "" Then

            Return False

        End If

        If dbcNumFacturaCOVE.Value = "" Then

            Return False

        End If

        If icFechaFacturaImpo.Value = "" Then

            Return False

        End If

        If fbcIncoterm.Value = objectidZero_ OrElse fbcIncoterm.Value = "" Then

            Return False

        End If


        'If fbcPais.Value = objectidZero_ OrElse fbcPais.Value = "" Then

        '    Return False

        'End If


        If icValorFactura.Value <> "" AndAlso icValorFactura.Value > 0 Then

            If scMonedaFactura.Value = objectidZero_ OrElse scMonedaFactura.Value = "" Then

                Return False

            End If

        Else

            Return False

        End If


        If icValorMercancia.Value <> "" AndAlso icValorMercancia.Value > 0 Then

            If scMonedaMercancia.Value = objectidZero_ OrElse scMonedaMercancia.Value = "" Then

                Return False

            End If

        Else

            Return False

        End If

        If fbcProveedor.Text <> "" Then

            If fbcProveedor.Value = objectidZero_ OrElse fbcProveedor.Value = "" Then

                Return False

            End If

            If scDomiciliosProveedor.Value = objectidZero_ Or scDomiciliosProveedor.Value = "" Then

                Return False

            End If

            If scVinculacion.Value = "" Then

                Return False

            End If

            If scMetodoValoracion.Value = "" Then

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


        If icValorfacturaPartida.Value <> "" AndAlso icValorfacturaPartida.Value > 0 Then

            If scMonedaFacturaPartida.Value = "" Then

                Return False

            End If

        Else

            Return False

        End If

        If icPrecioUnitario.Value <> "" AndAlso icPrecioUnitario.Value > 0 Then

            If scMonedaPrecioUnitarioPartida.Value = "" Then

                Return False

            End If

        Else

            Return False

        End If


        If icValorMercanciaPartida.Value <> "" AndAlso icValorMercanciaPartida.Value > 0 Then

            If scMonedaMercanciaPartida.Value = "" Then

                Return False

            End If
        Else

            Return False

        End If

        If fbcPaisPartida.Value = "" OrElse fbcPaisPartida.Value = objectidZero_ Then

            Return False

        End If


        Return True

    End Function

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

            If fbcProveedor.Text <> "" Then

                If fbcProveedor.Value = objectidZero_ Then

                    fbcProveedor.ToolTip = $"Verificar {fbcProveedor.Label} en registros de SYNAPSIS"

                    fbcProveedor.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkBut

                    fbcProveedor.ToolTipExpireTime = 60

                    fbcProveedor.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

                    fbcProveedor.ShowToolTip()

                End If

                If scDomiciliosProveedor.Value = objectidZero_ Or scDomiciliosProveedor.Value = "" Then

                    scDomiciliosProveedor.ToolTip = $"Verificar { scDomiciliosProveedor.Label} en registros de SYNAPSIS"

                    scDomiciliosProveedor.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkBut

                    scDomiciliosProveedor.ToolTipExpireTime = 60

                    scDomiciliosProveedor.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

                    scDomiciliosProveedor.ShowToolTip()

                End If

            Else

                fbcProveedor.ToolTip = $"Verificar {fbcProveedor.Label} en registros de SYNAPSIS"

                fbcProveedor.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkBut

                fbcProveedor.ToolTipExpireTime = 60

                fbcProveedor.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

                fbcProveedor.ShowToolTip()

                scDomiciliosProveedor.ToolTip = $"Verificar { scDomiciliosProveedor.Label} en registros de SYNAPSIS"

                scDomiciliosProveedor.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkBut

                scDomiciliosProveedor.ToolTipExpireTime = 60

                scDomiciliosProveedor.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

                scDomiciliosProveedor.ShowToolTip()

            End If

            If icObjectIdPartida.Value = objectidZero_ Then

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


            If scUnidadMedidaComercial.Value = objectidZero_ Then

                scUnidadMedidaComercial.ToolTip = $"Verificar { scUnidadMedidaComercial.Label} en registros de SYNAPSIS"

                scUnidadMedidaComercial.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkBut

                scUnidadMedidaComercial.ToolTipExpireTime = 60

                scUnidadMedidaComercial.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

                scUnidadMedidaComercial.ShowToolTip()

            End If

        End If

    End Sub


#Region "ComprobarValoresFacturacomercial"

    Private Sub CamposObjectid()
        scUnidadMedidaComercial.ToolTip = $"Verificar { scUnidadMedidaComercial.Label} en registros de SYNAPSIS"

        scUnidadMedidaComercial.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkBut

        scUnidadMedidaComercial.ToolTipExpireTime = 60

        scUnidadMedidaComercial.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

        scUnidadMedidaComercial.ShowToolTip()
    End Sub

    Private Sub CamposRequeridos()

        Dim objectidZero_ = "000000000000000000000000"

        If fbcCliente.Value = objectidZero_ OrElse fbcCliente.Value = "" Then

            ConfiguracionRequerido(fbcCliente)

        End If

        If dbcNumFacturaCOVE.Value = "" Then

            ConfiguracionRequerido(dbcNumFacturaCOVE)

        End If

        If icFechaFacturaImpo.Value = "" Then

            ConfiguracionRequerido(icFechaFacturaImpo)

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

        If fbcProveedor.Text <> "" Then

            If fbcProveedor.Value = objectidZero_ OrElse fbcProveedor.Value = "" Then

                ConfiguracionRequerido(fbcProveedor)

            End If

            If scDomiciliosProveedor.Value = objectidZero_ OrElse scDomiciliosProveedor.Value = "" Then

                ConfiguracionRequerido(scDomiciliosProveedor)

            End If

            If scVinculacion.Value = "" Then

                ConfiguracionRequerido(scVinculacion)

            End If

            If scMetodoValoracion.Value = "" Then

                ConfiguracionRequerido(scMetodoValoracion)

            End If

        Else

            ConfiguracionRequerido(fbcProveedor)

            ConfiguracionRequerido(scDomiciliosProveedor)

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

            If icCantidadTarifa.Value = "" Then

                ConfiguracionRequerido(icCantidadTarifa)

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

            If icCantidadTarifa.Value = "" Then

                ConfiguracionRequerido(icCantidadTarifa)

            End If

            If icDescripcionPartidaOriginal.Value = "" Then

                ConfiguracionRequerido(icDescripcionPartidaOriginal)

            End If

            If icDescripcionPartida.Value = "" Then

                ConfiguracionRequerido(icDescripcionPartida)

            End If

        End If


        If scUnidadMedidaTarifa.Value <> "" Then

            If icCantidadTarifa.Value = "" Then

                ConfiguracionRequerido(icCantidadTarifa)

            End If

        End If

        If icCantidadComercial.Value <> "" Then

            If scUnidadMedidaComercial.Value = objectidZero_ OrElse scUnidadMedidaComercial.Value = "" Then

                ConfiguracionRequerido(scUnidadMedidaComercial)

            End If

        End If

        If icCantidadComercial.Value = "" Then

            ConfiguracionRequerido(icCantidadComercial)

        End If

        If fbcPaisPartida.Value = "" OrElse fbcPaisPartida.Value = objectidZero_ Then

            ConfiguracionRequerido(fbcPaisPartida)

        End If

        If String.IsNullOrEmpty(icValorfacturaPartida.Value) Then

            ConfiguracionRequerido(icValorfacturaPartida)
            ConfiguracionRequerido(scMonedaFacturaPartida)

        Else

            If Not String.IsNullOrEmpty(icValorfacturaPartida.Value) OrElse icValorfacturaPartida.Value > 0 Then

                If scMonedaFacturaPartida.Value = "" Then

                    ConfiguracionRequerido(scMonedaFacturaPartida)

                End If

            Else

                ConfiguracionZero(icValorfacturaPartida)

            End If

        End If

        If String.IsNullOrEmpty(icValorMercanciaPartida.Value) Then

            ConfiguracionRequerido(icValorMercanciaPartida)

            ConfiguracionRequerido(scMonedaMercanciaPartida)

        Else

            If Not String.IsNullOrEmpty(icValorMercanciaPartida.Value) OrElse icValorMercanciaPartida.Value > 0 Then

                If scMonedaMercanciaPartida.Value = "" Then

                    ConfiguracionRequerido(scMonedaMercanciaPartida)

                End If

            Else

                ConfiguracionZero(icValorMercanciaPartida)

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

        If scUnidadMedidaTarifa.Value = "" Then

            ConfiguracionRequerido(scUnidadMedidaTarifa)

        End If

        If scUnidadMedidaComercial.Value = "" Then

            ConfiguracionRequerido(scUnidadMedidaComercial)

        End If

    End Sub

    Private Function ToUpperSafe(valor As String) As String
        If String.IsNullOrWhiteSpace(valor) Then Return ""
        Return valor.ToUpper()
    End Function

    'La suma del "valor factura" de los ítems, no debe rebasar el valor factura del encabezado, o no debe rebasar en más de 2 por aquello de los decimales,
    'El "valor mercancía" de los ítems deben ser menor o igual al valor factura del ítem, 
    'El "valor mercancía" del encabezado debe ser menor o igual al valor factura del encabezado.
    Protected Function ComprobarValorMercanciaVSValorFactura() As Boolean

        If Decimal.Parse(icValorMercancia.Value) > Decimal.Parse(icValorFactura.Value) Then

            Return False

        End If

        Return True

    End Function

    'Protected Function ComprobarSumaValorMercanciaItemsVSValorMercanciaGeneral() As Boolean

    '    ''OBTENER LA SUMA DE TODOS LOS ITEMS DEL PILLBOX

    '    Dim totalValorMercanciaItems_ = 0

    '    For Each nodo_ In pbPartidas.DataSource

    '        ''NO GUARDA EL OBJECTID del producto porque pillbox no reconoce los inputs ocultos
    '        For Each item_ In nodo_

    '            If item_.Key = "icValorMercanciaPartida" Then

    '                totalValorMercanciaItems_ += Decimal.Parse(item_.Value)

    '            End If

    '        Next
    '    Next

    '    If totalValorMercanciaItems_ <= Decimal.Parse(icValorMercancia.Value) Then

    '        Return True

    '    End If

    '    Return False

    'End Function

    'Protected Function ComprobarSumaValorMercanciaItemsVSValorfacturaPartida() As Boolean

    '    ''OBTENER LA SUMA DE TODOS LOS ITEMS DEL PILLBOX

    '    Dim totalValorMercanciaItems_ = 0

    '    Dim totalValorFacturaItems_ = 0

    '    For Each nodo_ In pbPartidas.DataSource

    '        ''NO GUARDA EL OBJECTID del producto porque pillbox no reconoce los inputs ocultos
    '        For Each item_ In nodo_

    '            If item_.Key = "icValorMercanciaPartida" Then

    '                totalValorMercanciaItems_ += Decimal.Parse(item_.Value)

    '            End If

    '            If item_.Key = "icValorfacturaPartida" Then

    '                totalValorFacturaItems_ += Decimal.Parse(item_.Value)

    '            End If

    '        Next

    '    Next

    '    If totalValorMercanciaItems_ <= totalValorFacturaItems_ Then

    '        Return True

    '    End If

    '    Return False

    'End Function

    'Protected Function ComprobarSumaValorFacturaItemsVSValorfactura() As Boolean

    '    ''OBTENER LA SUMA DE TODOS LOS ITEMS DEL PILLBOX

    '    Dim totalValorFacturaItems_ = 0

    '    For Each nodo_ In pbPartidas.DataSource

    '        ''NO GUARDA EL OBJECTID del producto porque pillbox no reconoce los inputs ocultos
    '        For Each item_ In nodo_

    '            If item_.Key = "icValorfacturaPartida" Then

    '                totalValorFacturaItems_ += Decimal.Parse(item_.Value)

    '            End If

    '        Next
    '    Next

    '    If totalValorFacturaItems_ <= Decimal.Parse(icValorFactura.Value) Then

    '        Return True

    '    End If

    '    Return False

    'End Function


    Protected Function ComprobarSumaValorMercanciaItemsVSValorMercanciaGeneral() As Boolean
        Dim totalValorMercanciaItems_ As Decimal = 0

        For Each nodo_ In pbPartidas.DataSource
            For Each item_ In nodo_
                If item_.Key = "icValorMercanciaPartida" Then
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
        Decimal.TryParse(icValorMercancia.Value,
                     NumberStyles.Number,
                     CultureInfo.InvariantCulture,
                     valorGeneral_)

        Return totalValorMercanciaItems_ <= valorGeneral_
    End Function

    Protected Function ComprobarSumaValorMercanciaItemsVSValorfacturaPartida() As Boolean
        Dim totalValorMercanciaItems_ As Decimal = 0
        Dim totalValorFacturaItems_ As Decimal = 0

        For Each nodo_ In pbPartidas.DataSource
            For Each item_ In nodo_
                Dim valor_ As Decimal
                Select Case item_.Key
                    Case "icValorMercanciaPartida"
                        If Decimal.TryParse(item_.Value,
                                        NumberStyles.Number,
                                        CultureInfo.InvariantCulture,
                                        valor_) Then
                            totalValorMercanciaItems_ += valor_
                        End If
                    Case "icValorfacturaPartida"
                        If Decimal.TryParse(item_.Value,
                                        NumberStyles.Number,
                                        CultureInfo.InvariantCulture,
                                        valor_) Then
                            totalValorFacturaItems_ += valor_
                        End If
                End Select
            Next
        Next

        Return totalValorMercanciaItems_ <= totalValorFacturaItems_
    End Function

    Protected Function ComprobarSumaValorFacturaItemsVSValorfactura() As Boolean
        Dim totalValorFacturaItems_ As Decimal = 0

        For Each nodo_ In pbPartidas.DataSource
            For Each item_ In nodo_
                If item_.Key = "icValorfacturaPartida" Then
                    Dim valor_ As Decimal
                    If Decimal.TryParse(item_.Value,
                                    NumberStyles.Number,
                                    CultureInfo.InvariantCulture,
                                    valor_) Then
                        totalValorFacturaItems_ += valor_
                    End If
                End If
            Next
        Next

        Dim valorFactura_ As Decimal
        Decimal.TryParse(icValorFactura.Value,
                     NumberStyles.Number,
                     CultureInfo.InvariantCulture,
                     valorFactura_)

        Return totalValorFacturaItems_ <= valorFactura_
    End Function


    Protected Sub dbcNumFacturaCOVE_TextChanged(sender As Object, e As EventArgs)

        dbcNumFacturaCOVE.Value = dbcNumFacturaCOVE.Value.ToUpper()

    End Sub

    Protected Sub fbcCliente_ClickClose(sender As Object, e As EventArgs)

        SetVars("_datosCliente", Nothing)

    End Sub

    'Protected Sub fbcPais_ClickClose(sender As Object, e As EventArgs)

    '    fbcPais.Value = Nothing

    '    fbcPais.DataSource = Nothing

    '    For Each campoMoneda_ In _listaCamposMonedas

    '        campoMoneda_.Value = Nothing

    '        campoMoneda_.DataSource = Nothing

    '    Next

    'End Sub

    Protected Sub fbcCliente_ClickSearch(sender As Object, e As EventArgs)

        If fbcCliente.Value = "" OrElse fbcCliente.Value = "000000000000000000000000" Then

            BuscarCliente()

        End If

    End Sub

    Protected Sub fbcProveedor_ClickSearch(sender As Object, e As EventArgs)

        If fbcProveedor.Value = "" OrElse fbcProveedor.Value = "000000000000000000000000" Then

            BuscarProveedor()

        End If

    End Sub

    Protected Sub fbcProducto_ClickSearch(sender As Object, e As EventArgs)
        Dim estado_ As TagWatcher = _utils.BuscarProductos(fbcProducto.Text.ToUpper(), fbcCliente.Value, fbcProveedor.Value)

        ''System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: ESTADO DEL PRODUCTO {estado_}")

        If estado_ IsNot Nothing AndAlso estado_.Status = TypeStatus.Ok Then
            ''System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: PRODUCTO OK")
            ListaProductosEncontrados(estado_.ObjectReturned)

        Else

            DisplayMessage("Producto no disponible", StatusMessage.Fail)

        End If

    End Sub

    Protected Sub scMetodoValoracion_SelectedIndexChanged(sender As Object, e As EventArgs)
        scMetodoValoracionPartida.Value = scMetodoValoracion.Value
    End Sub

    'Protected Sub fbcIncoterm_ClickSearch(sender As Object, e As EventArgs)

    'End Sub

    'Protected Sub fbcPais_ClickSearch(sender As Object, e As EventArgs)

    'End Sub

    'Protected Sub fbcProveedor_ClickSearch(sender As Object, e As EventArgs)

    'End Sub

    'Protected Sub fbcProducto_ClickSearch(sender As Object, e As EventArgs)

    'End Sub

#End Region

#End Region

#End Region
#Region "██████ Vinculación sexta capa  █████████       SAX      ████████████████████████████████████████████"
    '    ██████   Controladores utilizados                     Documentos por coding para MongoDB      ██████
    '    ██████    1.ControladorRecursosAduanales                                                      ██████
    '    ██████    2.ControladorSecuencias                                                             ██████
    '    ██████                                                                                        ██████
    '    ██████                                                                                        ██████
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

#End Region
End Class