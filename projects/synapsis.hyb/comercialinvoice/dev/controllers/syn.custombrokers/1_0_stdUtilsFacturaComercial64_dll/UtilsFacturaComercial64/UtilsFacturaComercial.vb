Imports System.Runtime.Serialization
Imports System.Web
Imports System.Web.Caching
Imports Gsol.krom
Imports Gsol.Web.Components
Imports Gsol.Web.Template.FormularioGeneralWeb
Imports Microsoft.SqlServer.Server
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports MongoDB.Driver
Imports Rec.Globals
Imports Rec.Globals.Controllers
Imports Rec.Globals.Empresas
Imports Rec.Globals.Utils
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Utils
Imports Wma.Exceptions
Imports Wma.Exceptions.TagWatcher


Public Class UtilsFacturaComercial
    Implements IDisposable

#Region "Propiedades privadas"

    Private disposedValue As Boolean

    Private _secuencia As ISecuencia

    Private _controladorSecuencias As IControladorSecuencia

    Private _controladorFacturaComercial As IControladorFacturaComercial

    Private _controladorProductos As IControladorProductos

    Private _controladorMonedas As IControladorMonedas

    Private _controladorTigie As IControladorTIGIE

    Private _controladorProveedorOperativo As CtrlProveedoresOperativos

    Private _controladorPaises As ControladorPaises

    Private _constructorProveedorOperativo As _
        ControladorBusqueda(Of ConstructorProveedoresOperativos)

    Private _constructorClienteBusqueda As ControladorBusqueda(Of ConstructorCliente)

    Private _constructorCliente As ConstructorCliente

    Private _constructorDestinatarioBusqueda As ControladorBusqueda(Of ConstructorDestinatario)

    Private _constructorDestinatario As ConstructorDestinatario

    Private _listaDataSource As List(Of SelectOption)

    Private _estado As TagWatcher

    Private _datosProveedor As AuxiliarProveedor

    Private _listaProveedoresOperativos As List(Of AuxiliarProveedor)

    Private _listaDomiciliosProveedor As List(Of SelectOption)

    Private _listamonedas As List(Of MonedaGlobal)

    Private _monedasoficialesporpais As Pais

    Private _datosCliente As ClienteFacturaComercial

    Private _vinculacionRecursos As ControladorRecursosAduanalesGral

    Private _productoAuxiliar As AuxiliarProducto

    Private _cacheListaUnidadesMedida As List(Of SelectOption)

    Private _listaUnidadMedida As List(Of UnidadMedida)

    Private _clienteControlador As IControladorClientes

    Private _cacheCommercialInvoiceAnalizer As CommercialInvoiceAnalysis

    Private _commercialInvoiceAnalizer As CommercialInvoiceAnalysis

    Private _controladorFirmas As ControladorFirmaElectronica

    Private _controladorDocumentosAsociados As ControladorDocumentosAsociados

    Private _listaProductos As List(Of AuxiliarProducto)

    Private _controladorMoneda As ControladorMonedas

    Private _controladorClientes As IControladorClientes


#End Region

    Sub New()

    End Sub

    Public Function Test() As TagWatcher

        With _estado

            .SetOK()

        End With

        Return _estado

    End Function


#Region "Datos factura comercial"
    'SOLO IMPO
    Public Function ExisteFacturaComercial(ByVal numeroFacturaComercial_ As String,
                                           ByVal objectidProveedorOperativo_ As String,
                                           ByVal fechaFacturaComercial_ As String,
                                           ByVal environmentOnline_ As Int32) As Boolean

        If numeroFacturaComercial_ <> "" And objectidProveedorOperativo_ <> "" And fechaFacturaComercial_ <> "" Then

            _estado = Nothing

            _estado = New TagWatcher

            _controladorFacturaComercial = New ControladorFacturaComercial(IControladorFacturaComercial.TipoOperaciones.Importacion,
                                                                           environmentOnline_)

            _estado = _controladorFacturaComercial.ConsultarExistenciaFacturaComercial(numeroFacturaComercial_,
                                                                                       New ObjectId(objectidProveedorOperativo_), fechaFacturaComercial_)
            If _estado.Status = TypeStatus.OkBut Then

                Return True

            End If

        End If

        Return False

    End Function
#End Region

#Region "Datos Cliente"
    Public Function ObtenerListaClientesPorConstructor(ByVal razonSocialCliente_ As String) As List(Of SelectOption)

        _constructorClienteBusqueda = Nothing

        _constructorClienteBusqueda = New ControladorBusqueda(Of ConstructorCliente)

        _listaDataSource = Nothing

        _listaDataSource = New List(Of SelectOption)

        _listaDataSource = _constructorClienteBusqueda.Buscar(razonSocialCliente_,
                                                              New Filtro With {.IdSeccion = SeccionesClientes.SCS1, .IdCampo = CamposClientes.CA_RAZON_SOCIAL})
        Return _listaDataSource

    End Function

    Public Function ObtenerListaClientePorControlador(ByVal razonSocialCliente_ As String) As List(Of SelectOption)

        _listaDataSource = Nothing

        _listaDataSource = New List(Of SelectOption)

        _controladorClientes = New ControladorClientes

        Dim estadoControladorClientes_ As New TagWatcher

        estadoControladorClientes_ = _controladorClientes.Consultar(razonSocialCliente_)

        If estadoControladorClientes_.Status = TypeStatus.Ok Then

            Dim listaDocumentosClientes_ = DirectCast(estadoControladorClientes_.ObjectReturned, List(Of OperacionGenerica))

            For Each documentoElectronico_ In listaDocumentosClientes_

                Dim constructorCliente_ = DirectCast(documentoElectronico_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente, ConstructorCliente)

                If constructorCliente_.Seccion(SeccionesClientes.SCS1).Campo(CamposClientes.CP_CLIENTE_HABILITADO).Valor Then

                    _listaDataSource.Add(New SelectOption With {.Value = documentoElectronico_.Id.ToString,
                                         .Text = $"{documentoElectronico_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.NombreCliente} | {documentoElectronico_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.IdCliente}"})

                End If

            Next

        End If

        Return _listaDataSource

    End Function

    Public Function DatosCliente(ByVal razonsocialCliente_ As String,
                                 Optional ByVal busquedaPorObjectID As Boolean = True) As ClienteFacturaComercial

        _datosCliente = Nothing

        _datosCliente = New ClienteFacturaComercial

        Dim objectIDCliente = Nothing

        Dim firmaelectronica_ As String = Nothing

        If razonsocialCliente_ IsNot Nothing Then

            Try
                _constructorCliente = Nothing

                _constructorCliente = New ConstructorCliente

                If busquedaPorObjectID = False Then

                    _clienteControlador = New ControladorClientes()

                    _estado = _clienteControlador.Consultar(razonsocialCliente_)

                    If _estado.Status = TypeStatus.Ok Then

                        objectIDCliente = _estado.ObjectReturned(0).Id.ToString

                        firmaelectronica_ = _estado.ObjectReturned(0).FirmaElectronica

                        _constructorCliente = DirectCast(_estado.ObjectReturned(0).Borrador.Folder.ArchivoPrincipal.Dupla.Fuente, ConstructorCliente)

                    End If

                Else
                    _constructorClienteBusqueda = Nothing

                    _constructorClienteBusqueda = New ControladorBusqueda(Of ConstructorCliente)

                    _estado = _constructorClienteBusqueda.ObtenerDocumento(razonsocialCliente_)

                    If _estado.Status = TypeStatus.Ok Then

                        _constructorCliente = DirectCast(_estado.ObjectReturned.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente, ConstructorCliente)

                        firmaelectronica_ = _estado.ObjectReturned.FirmaElectronica

                    End If

                End If

                With _constructorCliente
                    ''CHECAR
                    If busquedaPorObjectID Then

                        _datosCliente.idcliente = razonsocialCliente_

                    Else

                        _datosCliente.idcliente = objectIDCliente

                    End If

                    _datosCliente.clavecliente = _constructorCliente.FolioOperacion

                    ''ESTE SI ESTA
                    _datosCliente.claveempresacliente = .Campo(CamposClientes.CP_CVE_EMPRESA).Valor

                    _datosCliente.rfc = .Campo(CamposClientes.CA_RFC_CLIENTE).Valor
                    ''ETE NO
                    _datosCliente.taxid = .Campo(CamposClientes.CA_TAX_ID).Valor
                    ''ETE debe de
                    _datosCliente.curp = .Campo(CamposClientes.CA_CURP_CLIENTE).Valor

                    If .Campo(CamposClientes.CP_ID_EMPRESA).Valor IsNot Nothing Then

                        _datosCliente.idempresacliente = .Campo(CamposClientes.CP_ID_EMPRESA).Valor.ToString

                    End If

                    _datosCliente.clavepais = .Campo(CamposDomicilio.CA_CVE_PAIS).Valor

                    _datosCliente.pais = .Campo(CamposDomicilio.CA_PAIS).Valor

                    '_datosCliente.clavepais = "MEX"

                    '_datosCliente.pais = "MÉXICO (ESTADOS UNIDOS MEXICANOS)"

                    _datosCliente.domicilioPresentacion = .Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor

                    _datosCliente.calle = .Campo(CamposDomicilio.CA_CALLE).Valor

                    _datosCliente.numeroexterior = .Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor

                    _datosCliente.numerointerior = .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor

                    _datosCliente.ciudad = .Campo(CamposDomicilio.CA_CIUDAD).Valor

                    _datosCliente.localidad = .Campo(CamposDomicilio.CA_LOCALIDAD).Valor

                    _datosCliente.codigopostal = .Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor

                    _datosCliente.localidad = .Campo(CamposDomicilio.CA_LOCALIDAD).Valor

                    _datosCliente.colonia = .Campo(CamposDomicilio.CA_COLONIA).Valor

                    _datosCliente.entidadfederativa = .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).ValorPresentacion

                    _datosCliente.cveEntidadfederativa = .Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor

                    _datosCliente.cveMunicipio = .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor

                    _datosCliente.municipio = .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor

                    _datosCliente._iddomicilio = .Campo(CamposDomicilio.CP_ID_DOMICILIO).Valor

                    _datosCliente.sec = .Campo(CamposDomicilio.CP_SEC_DOMICILIO).Valor

                    _datosCliente.firmaElectronicaCliente = firmaelectronica_

                End With

            Catch ex As Exception

                Dim aqui_ = ex

            End Try

        End If

        Return _datosCliente

    End Function

#End Region

#Region "Datos paises"

    Public Function ObtenerListaPaises(ByVal pais_ As String) As List(Of SelectOption)

        _listaDataSource = Nothing

        _listaDataSource = New List(Of SelectOption)

        _listaDataSource = ControladorPaises.BuscarPaises(New List(Of Pais), pais_)

        Return _listaDataSource

    End Function

#End Region

#Region "Datos monedas"
    Public Function ObtenerMonedasPorPais(ByVal objectidpais_ As String) As List(Of moneda)

        _monedasoficialesporpais = Nothing

        _monedasoficialesporpais = New Pais

        _monedasoficialesporpais = ControladorPaises.BuscarPais(New ObjectId(objectidpais_))

        Return _monedasoficialesporpais.monedasoficiales

    End Function

    Public Function ObtenerListaMonedas(ByVal moneda_ As String) As List(Of MonedaGlobal)

        _controladorMonedas = Nothing

        _controladorMonedas = New ControladorMonedas

        _listamonedas = Nothing

        _listamonedas = New List(Of MonedaGlobal)

        _listamonedas = _controladorMonedas.BuscarMonedas(New List(Of String) From {moneda_})

        Return _listamonedas

    End Function




    Public Sub LLenarMonedas(ByVal _dataSource As List(Of SelectOption),
                                ByVal _listaCampos As List(Of Object))

        If _listaCampos IsNot Nothing Then

            If _dataSource IsNot Nothing Then

                For Each campo_ In _listaCampos

                    With campo_

                        .DataSource = _dataSource

                        .Value = _dataSource(0).Value

                    End With

                    'MostrarNombreMoneda(_dataSource(0).Value, campo_)


                Next

            End If

        End If

    End Sub

    Public Sub LimpiarMonedas(ByVal listaCamposMoneda_ As List(Of Object))

        If listaCamposMoneda_ IsNot Nothing Then

            If listaCamposMoneda_.Count > 0 Then

                For Each campo_ In listaCamposMoneda_

                    campo_.DataSource = Nothing

                Next

            End If

        End If

    End Sub

    Public Function ObtenerDatosMoneda(ByVal objectIdMoneda_ As ObjectId) As TagWatcher

        _estado = Nothing

        _estado = New TagWatcher

        _controladorMonedas = Nothing

        _controladorMonedas = New ControladorMonedas

        '_estado = _controladorMoneda.ObtenerDatosMonedaPorObjectId(objectIdMoneda_)

        Try
            Dim listamonedas_ = _controladorMonedas.BuscarMonedas("", objectIdMoneda_)

            If listamonedas_.Count > 0 Then

                _estado.ObjectReturned = DirectCast(listamonedas_.LastOrDefault, MonedaGlobal)

                _estado.SetOK()

            Else

                _estado.SetOKBut(Me, "Inténtelo más tarde")

            End If

        Catch ex As Exception

            _estado.SetOKBut(Me, $"Ha ocurrido un error {ex}")

        End Try

        Return _estado

    End Function

    Public Function BusquedaMonedas(ByRef control_ As SelectControl) As List(Of SelectOption)
        Dim cacheListaMonedas_ As List(Of SelectOption) = CType(HttpRuntime.Cache("cacheListaMonedas"), List(Of SelectOption))
        If cacheListaMonedas_ Is Nothing Then
            cacheListaMonedas_ = ControladorPaises.BuscarTodasMonedas("")
            HttpRuntime.Cache.Insert("cacheListaMonedas", cacheListaMonedas_, Nothing, DateTime.Now.AddMinutes(5), Caching.Cache.NoSlidingExpiration)
        End If
        If cacheListaMonedas_.Count > 0 Then
            control_.DataSource = cacheListaMonedas_
        End If
        Return cacheListaMonedas_
    End Function
#End Region

#Region "Datos productos/fracciones arancelarías"
    Public Function BuscarProductos(ByVal producto_ As String,
                                    ByVal idcliente_ As String,
                                    ByVal idproveedor_ As String) As TagWatcher

        Dim estado_ As New TagWatcher

        Dim objectId_ As ObjectId = Nothing

        _controladorProductos = Nothing

        _controladorProductos = New ControladorProductos()

        Dim objCliente_ As ObjectId = Nothing

        If idcliente_ <> "" Then

            objCliente_ = ObjectId.Parse(idcliente_)

        End If

        Dim objProveedor_ As ObjectId = Nothing

        If idproveedor_ <> "" Then

            objProveedor_ = ObjectId.Parse(idproveedor_)

        End If

        Dim estadoProductosClienteProveedor_ As TagWatcher = _controladorProductos.Consultar(producto_, objCliente_, objProveedor_)

        If estadoProductosClienteProveedor_.Status = TypeStatus.Ok Then

            Return estadoProductosClienteProveedor_

        Else

            Dim estadoProductosCliente_ As TagWatcher = _controladorProductos.Consultar(producto_, objCliente_, objectId_)

            If estadoProductosCliente_.Status = TypeStatus.Ok Then

                Return estadoProductosCliente_

            Else

                Dim estadoProductoSolo_ As TagWatcher = _controladorProductos.Consultar(producto_, objectId_)

                If estadoProductoSolo_.Status = TypeStatus.Ok Then

                    Return estadoProductoSolo_

                End If

            End If

        End If

        Return estado_

    End Function

    Public Function ObtenerProductoPorObjectId(ByVal objectidProducto_ As String) As AuxiliarProducto

        _estado = Nothing

        _estado = New TagWatcher

        _controladorProductos = Nothing

        _controladorProductos = New ControladorProductos

        _estado = _controladorProductos.ConsultarOne(New ObjectId(objectidProducto_))

        _productoAuxiliar = Nothing

        _productoAuxiliar = New AuxiliarProducto

        If _estado.Status = TypeStatus.Ok Then

            _productoAuxiliar = DirectCast(_estado.ObjectReturned, AuxiliarProducto)

        End If

        Return _productoAuxiliar

    End Function

#End Region

#Region "Datos del proveedor operativo"

    Public Function ObtenerListaProveedoresOperativosPorContructor(ByVal razonSocialProveedor_ As String) As List(Of SelectOption)

        _constructorProveedorOperativo = Nothing

        _constructorProveedorOperativo = New ControladorBusqueda(Of ConstructorProveedoresOperativos)

        _listaDataSource = Nothing

        _listaDataSource = New List(Of SelectOption)

        _listaDataSource = _constructorProveedorOperativo.Buscar(razonSocialProveedor_,
                                                                 New Filtro With {.IdSeccion = SeccionesProvedorOperativo.SPRO1, .IdCampo = CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR})

        Return _listaDataSource

    End Function

    Public Function ObtenerListaProveedoresOperativosPorControlador(ByVal razonSocialProveedor_ As String) As TagWatcher 'List(Of SelectOption)

        _estado = Nothing

        _estado = New TagWatcher

        Try
            _controladorProveedorOperativo = Nothing

            _controladorProveedorOperativo = New CtrlProveedoresOperativos

            _estado = _controladorProveedorOperativo.ObtenerProveedoresPorRazonSocialPais(razonSocialProveedor_)

        Catch ex As Exception

            _estado.SetError(Me, $"500 - Internal server error {ex}")

        End Try

        Return _estado

    End Function

    Public Function ObtenerDatosProveedorPorID(ByVal ObjectIdProveedor_ As String) As AuxiliarProveedor

        _controladorProveedorOperativo = Nothing

        _controladorProveedorOperativo = New CtrlProveedoresOperativos

        _estado = Nothing

        _estado = New TagWatcher

        _estado = _controladorProveedorOperativo.ObtenerDatosTarjetaPorObjectId(New ObjectId(ObjectIdProveedor_))

        _datosProveedor = Nothing

        _datosProveedor = New AuxiliarProveedor

        If _estado.Status = TypeStatus.Ok Then

            _datosProveedor = DirectCast(_estado.ObjectReturned, AuxiliarProveedor)

        End If

        Return _datosProveedor

    End Function

    Public Function EnlistarDomiciliosProveedor(ByVal listadomiciliosproveedor_ As _
                                                List(Of DomiciliosTaxid)) As List(Of SelectOption)

        _listaDomiciliosProveedor = Nothing

        _listaDomiciliosProveedor = New List(Of SelectOption)

        If listadomiciliosproveedor_ IsNot Nothing Then

            For Each domicilio_ In listadomiciliosproveedor_

                _listaDomiciliosProveedor.Add(New SelectOption _
                                              With {.Value = domicilio_._iddomicilio.ToString, .Text = domicilio_.domicilioPresentacion})

            Next

        End If

        Return _listaDomiciliosProveedor

    End Function

#End Region


#Region "Vinculaciones"
    Public Function Vinculacion() As List(Of SelectOption)

        _vinculacionRecursos = Nothing

        _vinculacionRecursos = New ControladorRecursosAduanalesGral

        _vinculacionRecursos = ControladorRecursosAduanalesGral.Buscar(ControladorRecursosAduanalesGral.TiposRecurso.Anexo22)

        _listaDataSource = Nothing

        _listaDataSource = New List(Of SelectOption)

        Dim vinculaciones_ = From data In _vinculacionRecursos.tiposvinculacion
                             Where data.archivado = False And data.estado = 1
                             Select data._idvinculacion, data.descripcion, data.descripcioncorta

        If vinculaciones_.Count > 0 Then

            _listaDataSource = New List(Of SelectOption)

            For index_ As Int32 = 0 To vinculaciones_.Count - 1

                _listaDataSource.Add(New SelectOption With
                             {.Value = vinculaciones_(index_)._idvinculacion,
                              .Text = vinculaciones_(index_)._idvinculacion.ToString & " - " & vinculaciones_(index_).descripcioncorta})
            Next

        End If

        Return _listaDataSource

    End Function
#End Region

#Region "Unidades de Medida"
    Public Function CargaUnidades(ByRef control_ As SelectControl,
                               ByVal tipoUnidad_ As ControladorUnidadesMedida.TiposUnidad,
                               Optional ByVal top_ As Int32 = 0) As List(Of SelectOption)

        Dim claveCache_ As String = $"cacheListaUnidadesMedida_{tipoUnidad_}"
        _cacheListaUnidadesMedida = CType(HttpRuntime.Cache(claveCache_), List(Of SelectOption))

        If _cacheListaUnidadesMedida Is Nothing Then
            Dim listaUnidades_ As List(Of UnidadMedida) = ControladorUnidadesMedida.BuscarUnidades(tipoUnidad_, control_.SuggestedText, top_)
            If listaUnidades_.Count > 0 Then
                _cacheListaUnidadesMedida = ControladorUnidadesMedida.ToSelectOption(listaUnidades_,
                                                                                  ControladorUnidadesMedida.TipoSelectOption.CveMXnombreoficiales)
                HttpRuntime.Cache.Insert(claveCache_, _cacheListaUnidadesMedida, Nothing, DateTime.Now.AddMinutes(5), Caching.Cache.NoSlidingExpiration)
            End If
        End If

        control_.DataSource = _cacheListaUnidadesMedida
        Return control_.DataSource

    End Function
#End Region

#Region "Disposable"
    Protected Overridable Sub Dispose(disposing As Boolean)

        If Not disposedValue Then

            If disposing Then
                ' TODO: eliminar el estado administrado (objetos administrados)

                _secuencia = Nothing

                _controladorSecuencias = Nothing

                '_controladorFacturaComercial.Dispose()

                '_controladorProductos.Dispose()

                '_controladorMonedas.Dispose()

                '_constructorProveedorOperativo.Dispose()

                '_constructorCliente.Dispose()

                _controladorTigie = Nothing

                _controladorPaises = Nothing

                _listaDataSource = Nothing

                _estado = Nothing

                _datosProveedor = Nothing

                _listaProveedoresOperativos = Nothing

                _listaDomiciliosProveedor = Nothing

                _constructorClienteBusqueda = Nothing

                _listamonedas = Nothing

                _monedasoficialesporpais = Nothing

                _cacheCommercialInvoiceAnalizer = Nothing

                HttpRuntime.Cache.Remove("cacheListaMonedas")

                HttpRuntime.Cache.Remove("cacheListaUnidadesMedida")

                HttpRuntime.Cache.Remove("cacheCommercialInvoiceAnalizer")

            End If

            ' TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
            ' TODO: establecer los campos grandes como NULL
            disposedValue = True

        End If

    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' No cambie este código. Coloque el código de limpieza en el método "Dispose(disposing As Boolean)".
        Dispose(disposing:=True)

        GC.SuppressFinalize(Me)

    End Sub
#End Region

#Region "Avisos y enums"
    'Public Sub MensajeOk(ByVal campo_ As Object, ByVal mensaje_ As String)

    '    With campo_

    '        .ToolTip = mensaje_
    '        .ToolTipStatus = IUIControl.ToolTipTypeStatus.Ok
    '        .ToolTipExpireTime = 6
    '        .ToolTipModality = IUIControl.ToolTipModalities.Classic
    '        .ShowToolTip()

    '    End With

    'End Sub


    'Public Sub MensajeInfo(ByVal campo_ As Object, ByVal mensaje_ As String)

    '    With campo_

    '        .ToolTip = mensaje_
    '        .ToolTipStatus = IUIControl.ToolTipTypeStatus.OkInfo
    '        .ToolTipExpireTime = 6
    '        .ToolTipModality = IUIControl.ToolTipModalities.Classic
    '        .ShowToolTip()

    '    End With

    'End Sub

    'Public Sub MensajeErrors(ByVal campo_ As Object, ByVal mensaje_ As String)

    '    With campo_
    '        .ToolTip = mensaje_
    '        .ToolTipStatus = IUIControl.ToolTipTypeStatus.Errors
    '        .ToolTipModality = IUIControl.ToolTipModalities.Classic
    '        .ShowToolTip()
    '    End With

    'End Sub

    Public Sub AvisoVerificacionManual(ByVal listaCampos_ As List(Of Object))

        Dim aviso_ = "SYN: - Verifique valor"

        For Each campo_ In listaCampos_

            With campo_

                .Label = $"{ .Label} ✨"

                .ToolTip = aviso_

                .ToolTipStatus = IUIControl.ToolTipTypeStatus.OkInfo

                .ToolTipExpireTime = 10000

                .ToolTipModality = IUIControl.ToolTipModalities.Ondemand

                .ShowToolTip()

            End With
        Next

    End Sub

#End Region

#Region "IA"
    Public Function ObtenerCommercialInvoiceAnalizer(ByVal numeroFactura_ As String,
                                                     ByVal environmentOnline_ As Int32) As CommercialInvoiceAnalysis

        Dim claveCache_ As String = $"cacheCommercialInvoiceAnalizer_{numeroFactura_}"

        _cacheCommercialInvoiceAnalizer = CType(HttpRuntime.Cache(claveCache_), CommercialInvoiceAnalysis)

        If _cacheCommercialInvoiceAnalizer Is Nothing Then

            _cacheCommercialInvoiceAnalizer = DirectCast(ObtenerFacturacomercialDB(numeroFactura_, environmentOnline_), CommercialInvoiceAnalysis)

            HttpRuntime.Cache.Insert(claveCache_, _cacheCommercialInvoiceAnalizer, Nothing, DateTime.Now.AddMinutes(5), Caching.Cache.NoSlidingExpiration)

        End If

        Return _cacheCommercialInvoiceAnalizer

    End Function

    Public Sub EjecutarAnalisisIAAsync(ByVal analyzer_ As CommercialInvoiceAnalysis,
                                       ByVal facturaComercialUI_ As Dictionary(Of String, Object))
        Try
            ' Trabajo pesado fuera del hilo UI
            Dim mensajesPorCampo = AgruparMensajesPorCampo(analyzer_)

            PintarMensajesEnUI(mensajesPorCampo, facturaComercialUI_)

        Catch ex As Exception
            ' Log silencioso
            Dim aqui = ex
        End Try

    End Sub

    Private Function ObtenerFacturacomercialDB(ByVal numeroFacturaComercial As String,
                                               ByVal environmentOnline_ As Int32) As CommercialInvoiceAnalysis

        _controladorFacturaComercial = Nothing

        _estado = Nothing

        _controladorFacturaComercial = New ControladorFacturaComercial(IControladorFacturaComercial.TipoOperaciones.Importacion, environmentOnline_)

        _estado = New TagWatcher

        _estado = _controladorFacturaComercial.ObtenerEstructura(numeroFacturaComercial)

        _commercialInvoiceAnalizer = New CommercialInvoiceAnalysis

        If _estado.Status = TypeStatus.Ok Then

            _commercialInvoiceAnalizer = DirectCast(_estado.ObjectReturned(0), CommercialInvoiceAnalysis)

        End If

        Return _commercialInvoiceAnalizer

    End Function

    Private Function AgruparMensajesPorCampo(facturaComercialAnalizer_ As CommercialInvoiceAnalysis) As Dictionary(Of String, List(Of String))

        Dim resultado As New Dictionary(Of String, List(Of String))

        For Each msg In facturaComercialAnalizer_.analysis.messages

            If Not resultado.ContainsKey(msg.field) Then
                resultado(msg.field) = New List(Of String)
            End If

            resultado(msg.field).Add(msg.message)

        Next

        Return resultado

    End Function

    Private Sub PintarMensajesEnUI(ByVal mensajesPorCampo As Dictionary(Of String, List(Of String)),
                                   ByVal facturaComercialUI_ As Dictionary(Of String, Object))

        Dim facturaComercialUX_ As New Dictionary(Of String, Object) From {
            {"invoicenumber", "Serie/Folio de factura"},
            {"invoicedate", "Fecha de factura"},
            {"invoiceseries", "Número de factura/Folio fiscal"},
            {"customername", "Cliente"},
            {"invoicecountry", "País"},
            {"incoterm", "Incoterm"},
            {"value", "Valor factura"},
            {"invoicecurrency", "Moneda"},
            {"totalinvoice", "Valor mercancía"},
            {"totalweight", "Peso total(Kg)"},
            {"packages", "Bultos"},
            {"purchaseorder", "Orden de compra"},
            {"suppliername", "Proveedor"},
            {"address", "Domicilio fiscal proveedor"},
            {"partnumber", "Número de parte/alias"},
            {"origincountry", "País de origen"},
            {"quantity", "Cantidad comercial"},
            {"unit", "Unidad de Medida Comercial(UMC)"},
            {"unitprice", "Precio unitario"},
            {"description", "Descripcion mercancía original"}}

        For Each campo In mensajesPorCampo

            Dim aqui_ = campo.Value

            If facturaComercialUI_.ContainsKey(campo.Key) Then

                Dim campoUI_ = facturaComercialUI_(campo.Key)

                If Not campoUI_.Label.Contains("✨") Then
                    campoUI_.Label &= " ✨"
                End If

                Dim campoUX_ = facturaComercialUX_(campo.Key)

                Dim mensajeUX_ = campo.Value(0)

                campoUI_.ToolTip =
                "IA: " &
                String.Join(vbCrLf & "• ", $"{campoUX_}: - {mensajeUX_}")

                campoUI_.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkInfo

                campoUI_.ToolTipExpireTime = 15000

                campoUI_.ToolTipModality = IUIControl.ToolTipModalities.Ondemand

                campoUI_.ShowToolTip()

            End If
        Next

    End Sub
#End Region

#Region "Publicar factura comercial"
    ''Poner privado
    Public Function ValidarFirmasProductos(constructorFacturaComercial As ConstructorFacturaComercial) As TagWatcher

        Dim tagwatcher_ As New TagWatcher

        ''DEBES HACER EL PROCESO CON LO QUE TE MANDE DON SERGIO, AL PARECER SI SURGE ALGUNA INCONSISTENCIA, DEBERAS MARCAR EL PRODUCTO.
        _controladorProductos = Nothing

        _controladorProductos = New ControladorProductos

        Dim seccionProductos_ = constructorFacturaComercial.Seccion(SeccionesFacturaComercial.SFAC4).Nodos

        Dim respuesta_ As Boolean = False

        Dim listaProductosAComprobar_ As New Dictionary(Of String, String)

        For Each item_ In seccionProductos_

            Dim numeroParte_ As String = item_.Campo(CamposFacturaComercial.CA_NUMERO_PARTE_PARTIDA).ValorPresentacion

            Dim partes_ = numeroParte_.Split("|"c)

            Dim numeroidKrom_ = partes_(0).Trim() ' "196"

            Dim timeStamp_ As String = item_.Campo(CamposProducto.CP_TIMESTAMP).Valor

            If Not listaProductosAComprobar_.ContainsKey(numeroidKrom_) Then

                listaProductosAComprobar_(numeroidKrom_) = timeStamp_

            End If

            ' System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: el idkrom es: {numeroidKrom_}")

        Next

        Dim comprobarFirmaProductos_ As TagWatcher = _controladorProductos.VerificaCambios(listaProductosAComprobar_)

       ' System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: la lista es: {listaProductosAComprobar_}")

        If comprobarFirmaProductos_.Status = TypeStatus.Ok Then

            tagwatcher_.SetOK()

        Else

            If comprobarFirmaProductos_.ObjectReturned IsNot Nothing AndAlso comprobarFirmaProductos_.ObjectReturned.Count > 0 Then

                ''HAY QUE DESTEGER CUAL ESTA MAL
                For Each item_ In comprobarFirmaProductos_.ObjectReturned

                    Dim aqui_ = item_

                Next

                tagwatcher_.SetOKBut(Me, "Hay datos inconsistentes en los productos.")

            End If

        End If

        Return tagwatcher_

    End Function
    Private Function ValidarFirmasProveedores(documentosAsociados_ As List(Of DocumentoAsociado)) As TagWatcher
        ''CHECA BIEN ESTO, NO ESTA BIEN ...
        'docAsoc_.firmaelectronica, docAsoc_.identificadorrecurso, docAsoc_._iddocumentoasociado
        Dim tagwatcher_ As New TagWatcher

        Try

            Dim ruta = "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts.Cuerpo.Nodos.Nodos.Nodos.Nodos.Valor"

            Dim validaFirmas_ = False

            Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos(27)

                For Each documentoAsociado_ In documentosAsociados_
                    ''DESCOMENTA TODO ESTO CUANDO TENGAS DATOS REALES XFI
                    'Dim coleccionOperaciones_ = enlaceDatos_.GetMongoCollection(Of OperacionGenerica)(documentoAsociado_.identificadorrecurso)

                    'Dim filtro_ = Builders(Of OperacionGenerica).Filter.Eq(Of ObjectId)(ruta, documentoAsociado_._iddocumentoasociado)

                    'Dim resultados_ = coleccionOperaciones_.Find(filtro_).First

                    'Dim partida_ As Partida = Nothing

                    'If documentoAsociado_.identificadorrecurso.Equals("ConstructorProveedoresOperativos") Then

                    '    Dim seccion_ = resultados_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Seccion(SeccionesProvedorOperativo.SPRO2)

                    '    partida_ = seccion_.Nodos.Where(Function(x) x.Attribute(CamposProveedorOperativo.CP_ID_PROVEEDOR).Valor.Equals(documentoAsociado_._iddocumentoasociado)).First

                    '    'ElseIf documentoAsociado_.identificadorrecurso.Equals("ConstructorDestinatario") Then

                    '    '    Dim seccion_ = resultados_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Seccion(SeccionesDestinatarios.SDES2)

                    '    '    partida_ = seccion_.Nodos.Where(Function(x) x.Attribute(CamposDestinatario.CP_ID_DESTINATARIO).Valor.Equals(documentoAsociado_._iddocumentoasociado)).First

                    'End If

                    'If partida_.Attribute(CamposProveedorOperativo.CP_FIRMA_ELECTRONICA) IsNot Nothing Then

                    '    If partida_.Attribute(CamposProveedorOperativo.CP_FIRMA_ELECTRONICA).Valor = documentoAsociado_.firmaelectronica Then

                    '        validaFirmas_ = True

                    '    End If

                    'End If

                    validaFirmas_ = True

                Next

                If validaFirmas_ Then

                    tagwatcher_.SetOK()

                Else

                    tagwatcher_.SetError(Me, $"Existen inconsistencias en las firmas de los proveedores y/o destinatarios, revisar.")

                End If

                tagwatcher_.ObjectReturned = validaFirmas_

            End Using

        Catch ex As Exception

            tagwatcher_.SetError(Me, $"Ha ocurrido un error_  {ex}")

        End Try

        Return tagwatcher_

    End Function

    Public Async Function PublicarFacturaComercialAsync(ByVal operaciongenerica_ As OperacionGenerica,
                                                        ByVal buscador_ As FindbarControl,
                                                        ByVal tipoOperacion_ As IControladorFacturaComercial.TipoOperaciones,
                                                        ByVal environmentOnline_ As Int32,
                                                        Optional existeSubdivision_ As Boolean = False) As Task(Of TagWatcher)

        _controladorFacturaComercial = Nothing

        _controladorFacturaComercial = New ControladorFacturaComercial(tipoOperacion_, environmentOnline_)

        _controladorFirmas = New ControladorFirmaElectronica

        _controladorDocumentosAsociados = New ControladorDocumentosAsociados

        Dim tipoMensaje_ As StatusMessage = StatusMessage.Info

        Dim estadoFirmarDocumentos_ As TagWatcher = New TagWatcher

        Dim tagwatcher_ As TagWatcher = _controladorDocumentosAsociados.ValidarConsistenciaDocumentosAsociadosInternos(operaciongenerica_.Borrador.Folder.DocumentosAsociados)

        If tagwatcher_.Status = TypeStatus.Ok And tagwatcher_.MessagesList.Count = 0 Then

            If operaciongenerica_.Borrador.Folder.DocumentosAsociados Is Nothing Then

                operaciongenerica_.Borrador.Folder.DocumentosAsociados = New List(Of DocumentoAsociado)

            End If

            Dim estadoValidarFirmasProveedores_ As TagWatcher = ValidarFirmasProveedores(operaciongenerica_.Borrador.Folder.DocumentosAsociados)

            Dim estadoValidarFirmasProductos_ As TagWatcher = ValidarFirmasProductos(DirectCast(operaciongenerica_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente, ConstructorFacturaComercial))

            If estadoValidarFirmasProveedores_.Status = TypeStatus.Ok AndAlso estadoValidarFirmasProductos_.Status = TypeStatus.Ok Then

                Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos _
                    With {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

                    Using session_ = Await enlaceDatos_.GetMongoClient().StartSessionAsync().ConfigureAwait(False)

                        session_.StartTransaction()

                        Dim espacioTrabajo_ = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")

                        If buscador_.DataObject.SubscriptionsGroup IsNot Nothing Then

                            If enlaceDatos_.EliminarSuscripciones(operaciongenerica_.Id,
                                                              buscador_.DataObject.SubscriptionsGroup,
                                                              session_).Status = TypeStatus.Ok Then

                                estadoFirmarDocumentos_ = enlaceDatos_.FirmarDocumento(buscador_.DataObject.GetType.Name,
                                                                       operaciongenerica_.Id,
                                                                       espacioTrabajo_.MisCredenciales.ClaveUsuario,
                                                                       session_)

                                If estadoFirmarDocumentos_.Status = TypeStatus.Ok Then

                                    If existeSubdivision_ Then

                                        Dim estadoSubdivision_ As TagWatcher = _controladorFacturaComercial.GenerarFacturaComercialSubdividible(operaciongenerica_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente,
                                                                                                                                               operaciongenerica_.Id)
                                        ''PENDIENTE DE VER COMO DIGIEVOLUCIONA ESA COLECCION
                                        If estadoSubdivision_.Status = TypeStatus.Ok Then

                                            tipoMensaje_ = StatusMessage.Info : tagwatcher_.SetOKInfo(Me, "Factura publicada exitosamente.")

                                            session_.CommitTransaction()
                                        Else
                                            tipoMensaje_ = StatusMessage.Fail : tagwatcher_.SetError(Me, "Factura no publicada")

                                            session_.AbortTransaction()
                                        End If

                                    Else
                                        tipoMensaje_ = StatusMessage.Info : tagwatcher_.SetOKInfo(Me, "Factura publicada exitosamente.")

                                        session_.CommitTransaction()

                                    End If

                                Else

                                    tipoMensaje_ = StatusMessage.Fail : tagwatcher_.SetError(Me, "Factura no publicada")

                                    session_.AbortTransaction()

                                End If

                            Else

                                tipoMensaje_ = StatusMessage.Fail : tagwatcher_.SetError(Me, "Ha ocurrido un error")

                                session_.AbortTransaction()

                            End If

                        Else

                            Dim estatusFirmarDocumentos_ As TagWatcher = New TagWatcher

                            estatusFirmarDocumentos_ = enlaceDatos_.FirmarDocumento(buscador_.DataObject.GetType.Name,
                                                                       operaciongenerica_.Id,
                                                                       espacioTrabajo_.MisCredenciales.ClaveUsuario,
                                                                       session_)

                            If estatusFirmarDocumentos_.Status = TypeStatus.Ok Then

                                If existeSubdivision_ Then

                                    Dim estadoSubdivision_ As TagWatcher = _controladorFacturaComercial.GenerarFacturaComercialSubdividible(operaciongenerica_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente, operaciongenerica_.Id)

                                    If estadoSubdivision_.Status = TypeStatus.Ok Then

                                        tipoMensaje_ = StatusMessage.Info : tagwatcher_.SetOKInfo(Me, "Factura publicada exitosamente.")

                                        session_.CommitTransaction()
                                    Else
                                        tipoMensaje_ = StatusMessage.Fail : tagwatcher_.SetError(Me, "Factura no publicada")

                                        session_.AbortTransaction()
                                    End If

                                Else

                                    tipoMensaje_ = StatusMessage.Info : tagwatcher_.SetOKInfo(Me, "Factura publicada exitosamente.")

                                    session_.CommitTransaction()

                                End If

                            Else

                                tipoMensaje_ = StatusMessage.Fail : tagwatcher_.SetError(Me, "Factura no publicada")

                                session_.AbortTransaction()

                            End If

                        End If

                        'DisplayMessage(IIf(estatusFirmarDocumentos_.Status = TypeStatus.Ok, "Publicado exitosamente.", tagwatcher_.ErrorDescription), tipoMensaje_)

                        'Return estatusFirmarDocumentos_

                    End Using

                End Using

            Else

                tipoMensaje_ = StatusMessage.Fail : tagwatcher_.SetError(Me, "Firmas de documentos no válidas")

            End If

        End If

        Return tagwatcher_

    End Function
#End Region

End Class

Public Class ClienteFacturaComercial
    Inherits Rec.Globals.Empresas.Domicilio
    Implements IDisposable, ICloneable

    <BsonIgnoreIfNull>
    Private disposedValue As Boolean

    <BsonIgnoreIfNull>
    Public Property idempresacliente As String

    <BsonIgnoreIfNull>
    Public Property claveempresacliente As String

    <BsonIgnoreIfNull>
    Public Property idcliente As String

    <BsonIgnoreIfNull>
    Public Property clavecliente As String

    <BsonIgnoreIfNull>
    Public Property rfc As String

    <BsonIgnoreIfNull>
    Public Property taxid As String

    <BsonIgnoreIfNull>
    Public Property curp As String

    <BsonIgnoreIfNull>
    Public Property clavepais As String

    <BsonIgnoreIfNull>
    Public Property pais As String

    <BsonIgnoreIfNull>
    Public Property firmaElectronicaCliente As String

    Protected Overridable Sub Dispose(disposing As Boolean)

        If Not disposedValue Then

            If disposing Then
                ' TODO: eliminar el estado administrado (objetos administrados)

                Me.idempresacliente = Nothing

                Me.claveempresacliente = Nothing

                Me.idcliente = Nothing

                Me.clavecliente = Nothing

                Me.rfc = Nothing

                Me.taxid = Nothing

                Me.curp = Nothing

                Me.clavepais = Nothing

                Me.pais = Nothing

            End If

            ' TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
            ' TODO: establecer los campos grandes como NULL
            disposedValue = True

        End If

    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' No cambie este código. Coloque el código de limpieza en el método "Dispose(disposing As Boolean)".
        Dispose(disposing:=True)

        GC.SuppressFinalize(Me)

    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Throw New NotImplementedException()
    End Function

End Class
