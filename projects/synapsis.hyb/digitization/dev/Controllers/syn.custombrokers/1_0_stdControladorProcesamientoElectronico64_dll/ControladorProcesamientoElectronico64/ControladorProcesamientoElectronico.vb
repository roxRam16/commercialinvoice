Imports System.IO
'Imports DocumentosCargados = Ia.Pln.IControllerChatGPT.DocumentoCargado
'Imports ListaTransformes = Ia.Pln.IControllerDocumentAnalyzer.Transformer
Imports System.Xml.Serialization
Imports gsol
Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Rec.Globals.Utils
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior
Imports Wma.Exceptions
Imports Wma.Exceptions.TagWatcher
Imports SubTipoDocumentoElectronico = Syn.CustomBrokers.Controllers.IControladorProcesamientoElectronico.ListaTiposDocumentos

Public Class ControladorProcesamientoElectronico
    Implements IControladorProcesamientoElectronico, IDisposable

#Region "PROPIEDADES PRIVADAS"
    Private disposedValue As Boolean
    Private _secuencia As ISecuencia
    Private _controladorSecuencias As IControladorSecuencia
    Private _documentoElectronico As DocumentoElectronico
    Private _espacioTrabajo As IEspacioTrabajo
    Private _tipoDocumento As TiposDocumentoElectronico
    Private _subtipoDocumento As SubTipoDocumentoElectronico
    'Private _documentAnalizer As IControllerDocumentAnalyzer
    Private _listaDocumentos As List(Of MemoryStream)
    Private _operacionGenerica As OperacionGenerica
    Private _estructuraGenerica As Object
    Private _collectionGenerica As Object
    Private _collectionEstructura As Object

    Private _controladorFacturaComercial As IControladorFacturaComercial
    Private _controladorProveedor As CtrlProveedoresOperativos
    Private _controladorCliente As IControladorClientes
    Private _controladorProductos As IControladorProductos

    Private _estructuraProveedor As AuxiliarProveedor
    Private _estructuraCliente As EstructuraCliente
    Private _facturaComercialIA As CommercialInvoiceAnalysis

#End Region

#Region "PROPIEDADES PÚBLICAS"
    Public Property Estado As TagWatcher _
        Implements IControladorProcesamientoElectronico.Estado
    Public Property EstadoAsync As Task(Of TagWatcher) _
        Implements IControladorProcesamientoElectronico.EstadoAsync
    Public Property Temperature As Integer _
        Implements IControladorProcesamientoElectronico.Temperature
    'Public Property Transformer As ListaTransformes _
    '    Implements IControladorProcesamientoElectronico.Transformer
    'Public Property DocumentoCargado As DocumentosCargados _
    '    Implements IControladorProcesamientoElectronico.DocumentoCargado

#End Region

#Region "CONSTRUCTORES"
    Sub New(ByVal tipoDocumento_ As TiposDocumentoElectronico)
        Inicializa(tipoDocumento_)
    End Sub

    Sub New(ByVal tipoDocumento_ As TiposDocumentoElectronico,
            ByVal subtipoDocumento_ As SubTipoDocumentoElectronico,
            Optional ByVal espacioTrabajo_ As IEspacioTrabajo = Nothing)
        Inicializa(tipoDocumento_, subtipoDocumento_, espacioTrabajo_)
    End Sub

    Private Sub Inicializa(ByVal tipoDocumento_ As TiposDocumentoElectronico,
                           Optional ByVal subtipoDocumento_ As SubTipoDocumentoElectronico = SubTipoDocumentoElectronico.SIN_DEFINIR,
                           Optional ByVal espacioTrabajo_ As IEspacioTrabajo = Nothing)
        _tipoDocumento = tipoDocumento_
        _subtipoDocumento = subtipoDocumento_
        _espacioTrabajo = espacioTrabajo_
        _listaDocumentos = New List(Of MemoryStream)
        'Temperature = 70
        'Transformer = ListaTransformes.GptVsTextract
        'DocumentoCargado = DocumentosCargados.FacturaImportacion
        Estado = New TagWatcher
    End Sub
#End Region

#Region "MÉTODOS PRIVADOS"
    Protected Function GenerarSecuenciaDocumentoElectronico(ByVal tipoSecuencia_ As String) As Secuencia

        _controladorSecuencias = New ControladorSecuencia

        _secuencia = New Secuencia

        Dim estadoSecuencia_ As TagWatcher = New TagWatcher

        estadoSecuencia_ = _controladorSecuencias.Generar(tipoSecuencia_, 1, 1, 1, 1)

        If estadoSecuencia_.Status = TypeStatus.Ok Then

            _secuencia = estadoSecuencia_.ObjectReturned

        End If

        Return _secuencia

    End Function

    Protected Function GenerarOperacionGenericaProcesamientoElectronico(ByVal estructura_ As DocumentoElectronicoApiStorage) As OperacionGenerica

    End Function

    Protected Function ObtenerInfoCliente(ByVal ObjectIdCliente_ As ObjectId) As TagWatcher

        _controladorCliente = New ControladorClientes

        Dim clienteObtenido_ As TagWatcher = _controladorCliente.Consultar(ObjectIdCliente_)

        Dim tagwatcher_ As TagWatcher = Nothing

        If clienteObtenido_.Status = TypeStatus.Ok Then

            _estructuraCliente = New EstructuraCliente

            Dim datosCliente_ = DirectCast(clienteObtenido_.ObjectReturned.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente, ConstructorCliente)

            'With datosCliente_

            With datosCliente_.Seccion(SeccionesClientes.SCS1)
                _estructuraCliente.id = datosCliente_.ObjectIdPropietario.ToString
                _estructuraCliente.customername = .Campo(CamposClientes.CA_RAZON_SOCIAL).Valor
                _estructuraCliente.cve_cliente = datosCliente_.FolioDocumento
                _estructuraCliente.rfc = .Campo(CamposClientes.CA_RFC_CLIENTE).Valor
                _estructuraCliente.street = .Campo(CamposDomicilio.CA_CALLE).Valor
                _estructuraCliente.externalnumber = .Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor
                _estructuraCliente.internalnumber = .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor
                _estructuraCliente.colonia = .Campo(CamposDomicilio.CA_COLONIA).Valor
                _estructuraCliente.locality = .Campo(CamposDomicilio.CA_LOCALIDAD).Valor
                _estructuraCliente.city = .Campo(CamposDomicilio.CA_CIUDAD).Valor
                _estructuraCliente.municipio = .Campo(CamposDomicilio.CA_MUNICIPIO).Valor
                _estructuraCliente.cveEntidadFederativa = .Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor
                _estructuraCliente.entidadFederativa = .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor
                _estructuraCliente.cvePais = .Campo(CamposDomicilio.CA_CVE_PAIS).Valor
                _estructuraCliente.country = .Campo(CamposDomicilio.CA_PAIS).Valor
                _estructuraCliente.zipcode = .Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor
                _estructuraCliente.address = .Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor
            End With
            tagwatcher_ = New TagWatcher
            tagwatcher_.ObjectReturned = _estructuraCliente
            tagwatcher_.SetOK()
        Else
            tagwatcher_.SetError(Me, "Cliente no registrado")

        End If

        Return tagwatcher_

    End Function

    Protected Function ObtenerInfoProveedor(ByVal RazonSocial_ As String) As TagWatcher
        _controladorProveedor = New CtrlProveedoresOperativos
        Return _controladorProveedor.ObtenerDatosProveedorPorRazonSocial(RazonSocial_)
    End Function

    Protected Function ObtenerListadoProductosPorClienteProveedor(ByVal listaItems_ As List(Of Item),
                                                                  ByVal idCliente_ As String) As TagWatcher
        _controladorProductos = New ControladorProductos()

        Dim listaNumeroPartes_ As New List(Of String)

        For Each item_ In listaItems_
            listaNumeroPartes_.Add(item_.partnumber)
        Next

        Dim result_ As TagWatcher = _controladorProductos.BuscarProductosPorNumeroParte(listaNumeroPartes_, idCliente_)
        '  Dim result_ As TagWatcher = _controladorProductos.BuscarProductosPorNumeroParte(listaNumeroPartes_, "64e7ad27f544af8dfd407efd")
        Return result_

    End Function

    Protected Function ObtenerDatosDestinatario(ByVal RazonSocial_ As String) As Object

    End Function

    Protected Function ObtenerDatosProducto(ByVal RazonSocial_ As String) As Object

    End Function

    Protected Function ObtenerDatosIncorterm(ByVal Incoterm_ As String) As Object

    End Function

    Protected Function ObtenerDatosPais(ByVal Pais_ As String) As Object

    End Function

    Protected Function ObtenerDatosMoneda(ByVal Moneda_ As String) As Object

    End Function

    Protected Function VerificarExistenciaFactura(ByVal numeroFactura_ As String,
                                                  ByVal objectIdCliente_ As ObjectId,
                                                  ByVal objectIdProveedor_ As ObjectId,
                                                  ByVal fechaFactura_ As String,
                                                  Optional ByVal tipoOperacion_ As IControladorFacturaComercial.TipoOperaciones =
                                                  IControladorFacturaComercial.TipoOperaciones.Importacion) As Boolean
        ''Buscar si existe
        ''Si esta publicada o no
        ''O si es un borrador, ver como acoplar los datos que le faltan

        _controladorFacturaComercial = New ControladorFacturaComercial(tipoOperacion_)
        Estado = New TagWatcher
        Estado = _controladorFacturaComercial.ConsultarExistenciaFacturaComercial(numeroFactura_, objectIdCliente_, objectIdProveedor_, fechaFactura_)

        If Estado.Status = TypeStatus.OkBut Then

            Dim data_ = Estado.ObjectReturned

            Return True

        End If

        Return False

    End Function


    Protected Function GenerarOperacionGenericaFacturaComercial(ByVal estructura_ As CommercialInvoiceAnalysis,
                                                                ByRef objectIdCliente_ As ObjectId) As TagWatcher

        With Estado

            _secuencia = New Secuencia

            _secuencia = GenerarSecuenciaDocumentoElectronico(SecuenciasComercioExterior.FacturasComerciales.ToString)

            Dim obtenerInfoCliente_ As TagWatcher = ObtenerInfoCliente(objectIdCliente_)

            Dim razonSocialCliente_ As String = estructura_.customername

            Dim razonSocialProveedor_ As String = estructura_.suppliername

            Dim existeCliente_ As Boolean = False

            _estructuraCliente = New EstructuraCliente

            If obtenerInfoCliente_.Status = TypeStatus.Ok Then

                _estructuraCliente = DirectCast(obtenerInfoCliente_.ObjectReturned, EstructuraCliente)

                razonSocialCliente_ = _estructuraCliente.customername

                existeCliente_ = True

            End If

            Dim obtenerInfoProveedor_ As TagWatcher = ObtenerInfoProveedor(estructura_.suppliername)

            Dim existeProveedor_ As Boolean = False

            _estructuraProveedor = New AuxiliarProveedor

            If obtenerInfoProveedor_.Status = TypeStatus.Ok Then

                _estructuraProveedor = DirectCast(obtenerInfoProveedor_.ObjectReturned, AuxiliarProveedor)

                razonSocialProveedor_ = _estructuraProveedor._razonsocial

                existeProveedor_ = True

                ''VAMOS A REVISAR SI EL PROVEEDOR ESTA ACTIVO, SINO, ENTONCES MANDAMOS EL MENSAJE QUE NO ESTA ACTIVO EL PROVEEDOR
                If _estructuraProveedor._activo = False Then
                    estructura_.analysis.messages.Add(New Ia.Analysis.Messages With {
                        .id = estructura_.analysis.messages.Count + 1,
                        .type = "warning",
                        .field = "suppliername",
                        .value = "0",
                        .message = "Proveedor no activo",
                        .confidence = 88.88,
                        .source = "Synapsis"})
                End If
            Else
                ''VAMOS A GENERAR MENSAJES A ESTATRUCTURA PONIENDO QUE EL PROVEEDOR NO EXISTE EN LOS CATALAGOS
                estructura_.analysis.messages.Add(New Ia.Analysis.Messages With {
                        .id = estructura_.analysis.messages.Count + 1,
                        .type = "alert",
                        .field = "suppliername",
                        .value = Nothing,
                        .message = "Registrar proveedor",
                        .confidence = 88.88,
                        .source = "Synapsis"})
            End If

            ''BUSCAMOS SI LA FACTURA YA EXISTE, SI ESTÁ PUBLICADA O NO, PA VER QUE HACER, OFICINA, CLIENTE, PROVEEDOR, FECHA
            ''AGUANTA LA OFICINA
            Dim existeFactura_ = False

            If existeProveedor_ Then

                existeFactura_ = VerificarExistenciaFactura(estructura_.invoicenumber,
                                                            objectIdCliente_,
                                                            ObjectId.Parse(_estructuraProveedor.id),
                                                            estructura_.invoicedate)
            Else
                ''AQUI LO QUE PROPONGO HACER ES VERIFICAR QUE ESTA FACTURA NO HAYA YA SIDO YA PROCESADA CON
                ''ESE NUMERO DE FACCTURA EN LA COLECCION DE COMERCIAL INVOICE
                ''CON EL NÚMERO DE FACTURA, EL CLIENTE, LA FECHA, EL PROVEEDOR POR TAXID 
                ''PENDIENTES!!!
            End If

            If existeFactura_ <> True Then

                _documentoElectronico = New ConstructorFacturaComercial

                With _documentoElectronico

                    With .Seccion(SeccionesFacturaComercial.SFAC1)
                        Try
                            If _subtipoDocumento = SubTipoDocumentoElectronico.FACTURA_COMERCIAL_IMPORTACION_PDF Or
                            _subtipoDocumento = SubTipoDocumentoElectronico.FACTURA_COMERCIAL_IMPORTACION_CFDI Then
                                .Campo(CamposFacturaComercial.CP_TIPO_OPERACION).Valor = 1
                                .Campo(CamposFacturaComercial.CP_TIPO_OPERACION).ValorPresentacion = "Importacion"
                            Else
                                .Campo(CamposFacturaComercial.CP_TIPO_OPERACION).Valor = 2
                                .Campo(CamposFacturaComercial.CP_TIPO_OPERACION).ValorPresentacion = "Exportacion"
                            End If

                            .Campo(CamposFacturaComercial.CP_TIPO_CARGA_DATOS).Valor = 1 '2 es carga manual
                            .Campo(CamposFacturaComercial.CP_TIPO_CARGA_DATOS).ValorPresentacion = "Carga IA"
                            .Campo(CamposFacturaComercial.CA_NUMERO_FACTURA).Valor = estructura_.invoicenumber
                            .Campo(CamposAcuseValor.CA_NUMERO_ACUSEVALOR).Valor = Nothing
                            .Campo(CamposFacturaComercial.CA_FECHA_FACTURA).Valor = Date.Parse(estructura_.invoicedate)
                            .Campo(CamposFacturaComercial.CA_FECHA_FACTURA).ValorPresentacion = estructura_.invoicedate
                            .Campo(CamposAcuseValor.CA_FECHA_ACUSEVALOR).Valor = Nothing
                            .Campo(CamposFacturaComercial.CP_SERIE_FOLIO_FACTURA).Valor = estructura_.invoiceseries
                            .Campo(CamposFacturaComercial.CP_SERIE_FOLIO_FACTURA).ValorPresentacion = estructura_.invoiceseries

                            ''BUSCAMOS LOS DATOS DEL CLIENTE, PARA QUE SE LLENE CON ESOS VALORES, SINO, CON LOS QUE ARROJE EL TEXTRAT                                                                                              

                            .Campo(CamposClientes.CP_OBJECTID_CLIENTE).Valor = If(existeCliente_, ObjectId.Parse(_estructuraCliente.id), Nothing)
                            .Campo(CamposClientes.CA_RAZON_SOCIAL).ValorPresentacion = If(existeCliente_, _estructuraCliente.customername, estructura_.customername)
                            .Campo(CamposClientes.CP_CVE_CLIENTE).Valor = If(existeCliente_, _estructuraCliente.cve_cliente, Nothing)
                            .Campo(CamposClientes.CA_TAX_ID).Valor = If(existeCliente_, _estructuraCliente.taxid, Nothing)
                            .Campo(CamposClientes.CA_RFC_CLIENTE).Valor = If(existeCliente_, _estructuraCliente.rfc, estructura_.customer.rfc)
                            .Campo(CamposClientes.CA_CURP_CLIENTE).Valor = If(existeCliente_, _estructuraCliente.curp, Nothing)
                            .Campo(CamposDomicilio.CP_ID_DOMICILIO).Valor = If(existeCliente_, _estructuraCliente.id_domicilio, Nothing)
                            .Campo(CamposDomicilio.CP_SEC_DOMICILIO).Valor = If(existeCliente_, _estructuraCliente.sec_domicilio, Nothing)
                            .Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor = If(existeCliente_, _estructuraCliente.address, estructura_.customer.address)
                            .Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).ValorPresentacion = If(existeCliente_, _estructuraCliente.address, estructura_.customer.address)
                            .Campo(CamposDomicilio.CA_CALLE).Valor = If(existeCliente_, _estructuraCliente.street, estructura_.customer.street)
                            .Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor = If(existeCliente_, _estructuraCliente.externalnumber, estructura_.customer.externalnumber)
                            .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor = If(existeCliente_, _estructuraCliente.internalnumber, estructura_.customer.internalnumber)
                            .Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor = If(existeCliente_, $"{_estructuraCliente.externalnumber} {_estructuraCliente.internalnumber}", $"{estructura_.customer.externalnumber} {estructura_.customer.internalnumber}")
                            .Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor = If(existeCliente_, _estructuraCliente.zipcode, estructura_.customer.zipcode)
                            .Campo(CamposDomicilio.CA_LOCALIDAD).Valor = If(existeCliente_, _estructuraCliente.locality, estructura_.customer.locality)
                            .Campo(CamposDomicilio.CA_CIUDAD).Valor = If(existeCliente_, _estructuraCliente.city, estructura_.customer.city)
                            .Campo(CamposDomicilio.CA_MUNICIPIO).Valor = If(existeCliente_, _estructuraCliente.municipio, estructura_.customer.locality)
                            .Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor = If(existeCliente_, _estructuraCliente.cveEntidadFederativa, estructura_.customer.state)
                            .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor = If(existeCliente_, _estructuraCliente.entidadFederativa, estructura_.customer.state)
                            .Campo(CamposDomicilio.CA_CVE_PAIS).Valor = If(existeCliente_, _estructuraCliente.cvePais, estructura_.customer.country)
                            .Campo(CamposDomicilio.CA_PAIS).Valor = If(existeCliente_, _estructuraCliente.country, estructura_.customer.country)

                            ''BUSCAR INCOTERM
                            .Campo(CamposFacturaComercial.CA_CVE_INCOTERM).Valor = Nothing
                            .Campo(CamposFacturaComercial.CA_CVE_INCOTERM).ValorPresentacion = estructura_.additionaldetails.incoterm

                            ''BUSCAR PAIS
                            .Campo(CamposFacturaComercial.CA_PAIS_FACTURACION).Valor = estructura_.invoicecountry

                            .Campo(CamposFacturaComercial.CP_VALOR_FACTURA).Valor = estructura_.totalinvoice
                            .Campo(CamposFacturaComercial.CP_VALOR_MERCANCIA).Valor = estructura_.totalinvoice

                            ''BUSCAR LA MONEDA
                            .Campo(CamposFacturaComercial.CA_MONEDA_FACTURACION).Valor = estructura_.invoicecurrency
                            .Campo(CamposFacturaComercial.CA_MONEDA_FACTURACION).ValorPresentacion = estructura_.invoicecurrency
                            .Campo(CamposFacturaComercial.CP_MONEDA_VALOR_MERCANCIA).Valor = estructura_.invoicecurrency

                            .Campo(CamposFacturaComercial.CP_BULTOS).Valor = estructura_.additionaldetails.packages
                            .Campo(CamposFacturaComercial.CP_PESO_TOTAL).Valor = estructura_.additionaldetails.totalweight
                            .Campo(CamposFacturaComercial.CP_ORDEN_COMPRA).Valor = estructura_.additionaldetails.purchaseorder
                            .Campo(CamposFacturaComercial.CP_REFERENCIA_CLIENTE).Valor = estructura_.additionaldetails.customerreference
                            .Campo(CamposFacturaComercial.CP_APLICA_ENAJENACION).Valor = False
                            .Campo(CamposFacturaComercial.CA_APLICA_SUBDIVISION).Valor = False
                            .Campo(CamposFacturaComercial.CP_APLICA_INCREMENTABLES).Valor = False
                            .Campo(CamposAcuseValor.CP_ID_ACUSEVALOR).Valor = Nothing
                            .Campo(CamposFacturaComercial.CP_MARCADO_PEDIMENTO).Valor = False
                            .Campo(CamposFacturaComercial.CP_ID_PEDIMENTO_ASOCIADO).Valor = Nothing

                        Catch ex As Exception
                            Dim aqui = ex
                        End Try
                    End With

                    With .Seccion(SeccionesFacturaComercial.SFAC2)
                        Try
                            ''BUSCAR EL PROVEEDOR
                            .Campo(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR).ValorPresentacion = If(existeProveedor_, _estructuraProveedor._razonsocial, estructura_.suppliername)
                            .Campo(CamposProveedorOperativo.CP_ID_PROVEEDOR).Valor = If(existeProveedor_, _estructuraProveedor.id, "000000000000000000000000")
                            .Campo(CamposProveedorOperativo.CP_CVE_PROVEEDOR).Valor = If(existeProveedor_, _estructuraProveedor._clave, Nothing)
                            .Campo(CamposDomicilio.CA_PAIS).Valor = If(existeProveedor_, _estructuraProveedor._cvepais, estructura_.supplier.country)
                            .Campo(CamposDomicilio.CA_PAIS).ValorPresentacion = If(existeProveedor_, _estructuraProveedor._pais, estructura_.supplier.country)
                            .Campo(CamposProveedorOperativo.CA_RFC_PROVEEDOR).Valor = If(existeProveedor_, _estructuraProveedor._rfc, estructura_.supplier.taxid)
                            .Campo(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR).Valor = If(existeProveedor_, _estructuraProveedor._taxid, estructura_.supplier.taxid)

                            ''BUSCAR EL DOMICILIO POR TAXID
                            Dim existedomicilioProveedor_ = False

                            Dim domicilioProveedorPorTaxid_ = _estructuraProveedor._listadomiciliosconTaxid.
                                    FirstOrDefault(Function(d) d.taxid = estructura_.supplier.taxid)

                            If existeProveedor_ AndAlso domicilioProveedorPorTaxid_ IsNot Nothing Then

                                existedomicilioProveedor_ = True

                            Else
                                estructura_.analysis.messages.Add(New Ia.Analysis.Messages With {
                                      .id = estructura_.analysis.messages.Count + 1,
                                      .type = "alert",
                                      .field = "address",
                                      .value = Nothing,
                                      .message = "Domicilio de proveedor no encontrado",
                                      .confidence = 88.88,
                                      .source = "Synapsis"})

                            End If

                            .Campo(CamposProveedorOperativo.CP_ID_DOMICILIO_PROVEEDOR).Valor = If(domicilioProveedorPorTaxid_ IsNot Nothing, domicilioProveedorPorTaxid_.id, "000000000000000000000000")
                            .Campo(CamposProveedorOperativo.CA_CURP_PROVEEDOR).Valor = Nothing
                            .Campo(CamposProveedorOperativo.CP_SEC_DOMICILIO_PROVEEDOR).Valor = If(domicilioProveedorPorTaxid_ IsNot Nothing, domicilioProveedorPorTaxid_.sec, Nothing)
                            .Campo(CamposProveedorOperativo.CA_DOMICILIO_FISCAL).ValorPresentacion = If(domicilioProveedorPorTaxid_ IsNot Nothing, domicilioProveedorPorTaxid_.domicilioPresentacion, estructura_.supplier.address)
                            .Campo(CamposProveedorOperativo.CA_DOMICILIO_FISCAL).Valor = If(domicilioProveedorPorTaxid_ IsNot Nothing, domicilioProveedorPorTaxid_.domicilioPresentacion, estructura_.supplier.address)
                            .Campo(CamposDomicilio.CA_CALLE).Valor = If(domicilioProveedorPorTaxid_ IsNot Nothing, domicilioProveedorPorTaxid_.calle, estructura_.supplier.street)
                            .Campo(CamposDomicilio.CA_CIUDAD).Valor = If(domicilioProveedorPorTaxid_ IsNot Nothing, domicilioProveedorPorTaxid_.ciudad, estructura_.supplier.city)
                            .Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor = If(domicilioProveedorPorTaxid_ IsNot Nothing, domicilioProveedorPorTaxid_.codigopostal, estructura_.supplier.zipcode)
                            .Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor = If(domicilioProveedorPorTaxid_ IsNot Nothing, domicilioProveedorPorTaxid_.numeroexterior, estructura_.supplier.externalnumber)
                            .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor = If(domicilioProveedorPorTaxid_ IsNot Nothing, domicilioProveedorPorTaxid_.numerointerior, estructura_.supplier.internalnumber)
                            .Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor = If(domicilioProveedorPorTaxid_ IsNot Nothing, $"{domicilioProveedorPorTaxid_.numeroexterior} {domicilioProveedorPorTaxid_.numeroexterior}", $"{estructura_.supplier.externalnumber} {estructura_.supplier.internalnumber}")
                            .Campo(CamposDomicilio.CA_LOCALIDAD).Valor = If(domicilioProveedorPorTaxid_ IsNot Nothing, domicilioProveedorPorTaxid_.localidad, estructura_.supplier.locality)
                            .Campo(CamposDomicilio.CA_CIUDAD).Valor = If(domicilioProveedorPorTaxid_ IsNot Nothing, domicilioProveedorPorTaxid_.ciudad, estructura_.supplier.city)
                            .Campo(CamposDomicilio.CA_MUNICIPIO).Valor = If(domicilioProveedorPorTaxid_ IsNot Nothing, domicilioProveedorPorTaxid_.municipio, Nothing)
                            .Campo(CamposDomicilio.CA_ENTIDAD_MUNICIPIO).Valor = If(domicilioProveedorPorTaxid_ IsNot Nothing, domicilioProveedorPorTaxid_.cveMunicipio, Nothing)
                            .Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor = If(domicilioProveedorPorTaxid_ IsNot Nothing, domicilioProveedorPorTaxid_.cveEntidadfederativa, Nothing)
                            .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor = If(domicilioProveedorPorTaxid_ IsNot Nothing, domicilioProveedorPorTaxid_.entidadfederativa, estructura_.supplier.state)
                            .Campo(CamposDomicilio.CA_CVE_PAIS).Valor = If(domicilioProveedorPorTaxid_ IsNot Nothing, domicilioProveedorPorTaxid_.cvePais, estructura_.supplier.country)
                            .Campo(CamposDomicilio.CA_PAIS).Valor = If(domicilioProveedorPorTaxid_ IsNot Nothing, domicilioProveedorPorTaxid_.pais, estructura_.supplier.country)

                            ''BUSCAR SI HAY VINCULACION CON EL CLIENTE
                            .Campo(CamposFacturaComercial.CA_CVE_VINCULACION).Valor = Nothing

                            ''BUSCAR SI HAY MÉTODO DE VALORACIÓN
                            .Campo(CamposFacturaComercial.CP_CVE_METODO_VALORACION).Valor = Nothing

                            .Campo(CamposFacturaComercial.CA_APLICA_CERTIFICADO).Valor = False
                            .Campo(CamposFacturaComercial.CP_NOMBRE_CERTIFICADOR).Valor = Nothing
                            .Campo(CamposProveedorOperativo.CP_DESTINATARIO_PROVEEDOR).Valor = False

                        Catch ex As Exception
                            Dim error_ = ex
                        End Try
                    End With

                    If _subtipoDocumento = SubTipoDocumentoElectronico.FACTURA_COMERCIAL_EXPORTACION_CFDI Or
                            _subtipoDocumento = SubTipoDocumentoElectronico.FACTURA_COMERCIAL_EXPORTACION_PDF Then
                        If estructura_.consigneedetails IsNot Nothing Then
                            With .Seccion(SeccionesFacturaComercial.SFAC3)
                                ''SI ES DE EXPO, BUSCAR EL DESTINATARIO
                                .Campo(CamposDestinatario.CA_RAZON_SOCIAL).ValorPresentacion = estructura_.consigneedetails.consigneedetailsname
                                .Campo(CamposDestinatario.CP_ID_DESTINATARIO).Valor = Nothing
                                .Campo(CamposDestinatario.CP_CVE_DESTINATARIO).Valor = Nothing
                                .Campo(CamposDomicilio.CA_PAIS).Valor = estructura_.consigneedetails.country
                                .Campo(CamposDomicilio.CA_PAIS).ValorPresentacion = estructura_.consigneedetails.country
                                .Campo(CamposDestinatario.CA_RFC_DESTINATARIO).Valor = estructura_.consigneedetails.taxid
                                .Campo(CamposDestinatario.CA_TAX_ID).Valor = estructura_.consigneedetails.taxid
                                .Campo(CamposDestinatario.CA_DOMICILIO_FISCAL_DESTINATARIO).Valor = estructura_.consigneedetails.address
                                .Campo(CamposDestinatario.CA_DOMICILIO_FISCAL_DESTINATARIO).ValorPresentacion = estructura_.consigneedetails.address
                                .Campo(CamposDomicilio.CA_CALLE).Valor = estructura_.consigneedetails.street
                                .Campo(CamposDomicilio.CA_CIUDAD).Valor = estructura_.consigneedetails.city
                                .Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor = estructura_.consigneedetails.zipcode
                                .Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor = estructura_.consigneedetails.externalnumber
                                .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor = estructura_.consigneedetails.internalnumber
                                .Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor = $"{estructura_.consigneedetails.externalnumber} {estructura_.consigneedetails.internalnumber}"
                                .Campo(CamposDomicilio.CA_LOCALIDAD).Valor = estructura_.consigneedetails.locality
                                .Campo(CamposDomicilio.CA_CIUDAD).Valor = estructura_.consigneedetails.city
                                .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor = estructura_.consigneedetails.state
                                .Campo(CamposDomicilio.CA_CVE_PAIS).Valor = estructura_.consigneedetails.country
                            End With
                        End If
                    End If

                    'Dim listaProductosEncontrados_ = ObtenerListadoProductosPorClienteProveedor(estructura_.items, razonSocialCliente_, razonSocialProveedor_)
                    ''PENDIENTE, QUIZAS SEA MEJOR UTILIZARLO EN 5TA CAPA :V
                    'Dim listaProductosEncontrados_ = ObtenerListadoProductosPorClienteProveedor(estructura_.items, objectIdCliente_.ToString())

                    Dim i_ = 0
                    For Each item_ In estructura_.items
                        Dim partida_ = .Seccion(SeccionesFacturaComercial.SFAC4).Partida(_documentoElectronico)
                        With partida_
                            .Campo(CamposFacturaComercial.CP_OBJECTID_PRODUCTOS).Valor = Nothing
                            .Campo(CamposFacturaComercial.CP_NUMERO_PARTIDA).Valor = Nothing
                            .Campo(CamposFacturaComercial.CA_NUMERO_PARTE_PARTIDA).Valor = item_.partnumber
                            .Campo(CamposFacturaComercial.CA_NUMERO_PARTE_PARTIDA).ValorPresentacion = item_.partnumber
                            .Campo(CamposFacturaComercial.CA_CANTIDAD_COMERCIAL_PARTIDA).Valor = item_.quantity
                            .Campo(CamposFacturaComercial.CA_UNIDAD_MEDIDA_COMERCIAL_PARTIDA).Valor = item_.unit
                            .Campo(CamposFacturaComercial.CA_UNIDAD_MEDIDA_COMERCIAL_PARTIDA).ValorPresentacion = item_.unit
                            .Campo(CamposFacturaComercial.CA_DESCRIPCION_PARTE_PARTIDA).Valor = item_.description
                            .Campo(CamposFacturaComercial.CA_DESCRIPCION_PARTE_PARTIDA_ORIGINAL).Valor = item_.description
                            .Campo(CamposFacturaComercial.CA_VALOR_FACTURA_PARTIDA).Valor = item_.total
                            .Campo(CamposFacturaComercial.CP_MONEDA_FACTURA_PARTIDA).Valor = item_.currency
                            .Campo(CamposFacturaComercial.CP_MONEDA_FACTURA_PARTIDA).ValorPresentacion = item_.currency
                            .Campo(CamposFacturaComercial.CA_VALOR_DOLARES_PARTIDA).Valor = Nothing
                            .Campo(CamposFacturaComercial.CP_MONEDA_VALOR_DOLARES_PARTIDA).Valor = Nothing
                            .Campo(CamposFacturaComercial.CA_VALOR_MERCANCIA_PARTIDA).Valor = item_.value
                            .Campo(CamposFacturaComercial.CA_MONEDA_MERCANCIA_PARTIDA).Valor = item_.currency
                            .Campo(CamposFacturaComercial.CA_MONEDA_MERCANCIA_PARTIDA).ValorPresentacion = item_.currency
                            .Campo(CamposFacturaComercial.CA_VALOR_UNITARIO_PARTIDA).Valor = Nothing
                            .Campo(CamposFacturaComercial.CA_MONEDA_VALOR_UNITARIO_PARTIDA).Valor = Nothing
                            .Campo(CamposFacturaComercial.CA_PRECIO_UNITARIO_PARTIDA).Valor = Nothing
                            .Campo(CamposFacturaComercial.CP_MONEDA_PRECIO_UNITARIO).Valor = Nothing
                            .Campo(CamposFacturaComercial.CA_PESO_NETO_PARTIDA).Valor = item_.netweight
                            .Campo(CamposFacturaComercial.CA_DESCRIPCION_COVE_PARTIDA).Valor = Nothing
                            .Campo(CamposFacturaComercial.CP_APLICA_DESCRIPCION_ORIGINAL_MERCANCIA_PEDIMENTO).Valor = False
                            .Campo(CamposFacturaComercial.CP_APLICA_DESCRIPCION_COVE_PARTIDA).Valor = False
                            .Campo(CamposFacturaComercial.CA_PAIS_DESTINO_PARTIDA).Valor = item_.destinationcountry
                            .Campo(CamposFacturaComercial.CA_PAIS_DESTINO_PARTIDA).ValorPresentacion = item_.destinationcountry
                            .Campo(CamposFacturaComercial.CA_PAIS_ORIGEN_PARTIDA).Valor = item_.origincountry
                            .Campo(CamposFacturaComercial.CA_PAIS_ORIGEN_PARTIDA).ValorPresentacion = item_.origincountry
                            .Campo(CamposFacturaComercial.CA_CVE_METODO_VALORACION_PARTIDA).Valor = Nothing
                            .Campo(CamposFacturaComercial.CP_ORDEN_COMPRA_PARTIDA).ValorPresentacion = item_.purchaseorder
                            .Campo(CamposFacturaComercial.CP_CANTIDAD_FACTURA_PARTIDA).Valor = item_.quantity
                            .Campo(CamposFacturaComercial.CP_UNIDAD_MEDIDA_FACTURA_PARTIDA).Valor = item_.unit
                            .Campo(CamposFacturaComercial.CP_UNIDAD_MEDIDA_FACTURA_PARTIDA).ValorPresentacion = item_.unit
                            .Campo(CamposFacturaComercial.CA_FRACCION_ARANCELARIA_PARTIDA).Valor = Nothing
                            .Campo(CamposFacturaComercial.CA_CANTIDAD_TARIFA_PARTIDA).Valor = Nothing
                            .Campo(CamposFacturaComercial.CA_UNIDAD_MEDIDA_TARIFA_PARTIDA).Valor = Nothing
                            .Campo(CamposFacturaComercial.CA_FRACCION_NICO_PARTIDA).Valor = Nothing
                            .Campo(CamposFacturaComercial.CA_LOTE_PARTIDA).Valor = Nothing
                            .Campo(CamposFacturaComercial.CA_NUMERO_SERIE_PARTIDA).Valor = Nothing
                            .Campo(CamposFacturaComercial.CA_MARCA_PARTIDA).Valor = Nothing
                            .Campo(CamposFacturaComercial.CA_MODELO_PARTIDA).Valor = Nothing
                            .Campo(CamposFacturaComercial.CA_SUBMODELO_PARTIDA).Valor = Nothing
                            .Campo(CamposFacturaComercial.CA_KILOMETRAJE_PARTIDA).Valor = Nothing
                            .Campo(CamposGlobales.CP_IDENTITY).Valor = i_
                        End With
                        i_ += 1
                    Next

                    With .Seccion(SeccionesFacturaComercial.SFAC5) ''QUIZAS HAYA QUE DETALLAR MEJOR
                        If estructura_.additionaldetails.incrementalvalues IsNot Nothing Then
                            If estructura_.additionaldetails.incrementalvalues.Count <> 0 Then
                                For Each item_ In estructura_.additionaldetails.incrementalvalues
                                    .Campo(CamposFacturaComercial.CA_FLETES).Valor = item_.incremental
                                    .Campo(CamposFacturaComercial.CA_SEGURO).Valor = item_.incremental
                                    .Campo(CamposFacturaComercial.CA_EMBALAJES).Valor = item_.incremental
                                    .Campo(CamposFacturaComercial.CA_OTROS_INCREMENTABLES).Valor = item_.incremental
                                    .Campo(CamposFacturaComercial.CA_DESCUENTOS).Valor = item_.incremental
                                Next
                            End If
                        End If

                        .Campo(CamposFacturaComercial.CA_MONEDA_FLETES).ValorPresentacion = estructura_.invoicecurrency
                        .Campo(CamposFacturaComercial.CA_MONEDA_FLETES).Valor = estructura_.invoicecurrency
                        .Campo(CamposFacturaComercial.CA_MONEDA_SEGUROS).Valor = estructura_.invoicecurrency
                        .Campo(CamposFacturaComercial.CA_MONEDA_SEGUROS).ValorPresentacion = estructura_.invoicecurrency
                        .Campo(CamposFacturaComercial.CA_MONEDA_EMBALAJES).Valor = estructura_.invoicecurrency
                        .Campo(CamposFacturaComercial.CA_MONEDA_EMBALAJES).ValorPresentacion = estructura_.invoicecurrency
                        .Campo(CamposFacturaComercial.CA_MONEDA_OTROS_INCREMENTABLES).Valor = estructura_.invoicecurrency
                        .Campo(CamposFacturaComercial.CA_MONEDA_OTROS_INCREMENTABLES).ValorPresentacion = estructura_.invoicecurrency
                        .Campo(CamposFacturaComercial.CA_MONEDA_DESCUENTOS).Valor = estructura_.invoicecurrency
                        .Campo(CamposFacturaComercial.CA_MONEDA_DESCUENTOS).ValorPresentacion = estructura_.invoicecurrency
                    End With

                    .UsuarioGenerador = "IA"
                    .Id = _secuencia._id.ToString
                    .IdDocumento = _secuencia.sec
                    .FolioDocumento = estructura_.invoicenumber
                    .FolioOperacion = _secuencia.sec
                    .TipoPropietario = SecuenciasComercioExterior.FacturasComerciales.ToString

                    If existeCliente_ Then
                        Dim cveClientenumero As Integer
                        .NombrePropietario = _estructuraCliente.customername

                        If Integer.TryParse(_estructuraCliente.cve_cliente, cveClientenumero) Then

                            .IdPropietario = cveClientenumero

                        Else

                            .IdPropietario = Nothing

                        End If

                        .ObjectIdPropietario = ObjectId.Parse(_estructuraCliente.id)

                    Else
                        .NombrePropietario = estructura_.customername
                        .IdPropietario = Nothing
                        .ObjectIdPropietario = Nothing
                    End If

                    .TipoDocumentoElectronico = _tipoDocumento
                    .Metadatos = New List(Of CampoGenerico) From { .Campo(CamposFacturaComercial.CA_CVE_INCOTERM)}

                End With

                Dim operacionGenerica_ As New OperacionGenerica(_documentoElectronico)

                With operacionGenerica_
                    .FolioOperacion = _secuencia.sec
                End With

                .ObjectReturned = New ResponseOperacion With {
                    .OperacionGenerica = operacionGenerica_,
                    .CommercialInvoice = estructura_
                    }

                .SetOK()

            Else

                estructura_.analysis.messages.Add(New Ia.Analysis.Messages With {
                   .id = estructura_.analysis.messages.Count + 1,
                   .type = "alert",
                   .field = "invoicenumber",
                   .value = Nothing,
                   .message = "Factura comercial ya registrada",
                   .confidence = 88.88,
                   .source = "Synapsis"})

                .ObjectReturned = New ResponseOperacion With {
                    .OperacionGenerica = Nothing,
                    .CommercialInvoice = estructura_
                    }

                .SetOKBut(Me, "No se generó operación genérica")

            End If

        End With

        Return Estado

    End Function

    Protected Function GuardarEstructuraDocumentoElectronico(ByVal estructuraDocumentoElectronico_ As _
                                                             CommercialInvoiceAnalysis) As TagWatcher
        Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = _espacioTrabajo}

            With Estado

                Try

                    Dim operationsDB_ = enlaceDatos_.GetMongoCollection(Of CommercialInvoiceAnalysis)("ComercialInvoicesAnalysis")

                    Dim result_ = operationsDB_.InsertOneAsync(estructuraDocumentoElectronico_)

                    If result_.Id <> Nothing Then

                        .SetOK()

                    End If

                Catch ex As Exception

                    .SetError(Me, "Ocurrió un error al insertar")

                End Try

            End With

        End Using

        Return Estado

    End Function


    Protected Function GenerarOperacionGenerica(Of T)(ByRef estructura_ As T) As OperacionGenerica
        Dim estructuraGenerica_ As Object
        Select Case _tipoDocumento
            Case TiposDocumentoElectronico.FacturaComercial
                estructuraGenerica_ = DirectCast(CType(estructura_, Object), CommercialInvoiceAnalysis)
                ' _operacionGenerica = GenerarOperacionGenericaFacturaComercial(estructuraGenerica_)
                'Case TiposDocumentoElectronico.ProcesamientoElectronicoDocumento
                '    estructuraGenerica_ = DirectCast(CType(estructura_, Object), DocumentoElectronicoApiStorage)
                '    _operacionGenerica = GenerarOperacionGenericaProcesamientoElectronico(estructuraGenerica_)

        End Select
        Return _operacionGenerica
    End Function

    Protected Function GuardarDocumento(Of T)(ByVal estructura_ As T, ByVal entidad_ As Object) As TagWatcher

        Dim operacionGenerica_ As OperacionGenerica = GenerarOperacionGenerica(Of T)(estructura_)

        Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos(21) With {.EspacioTrabajo = _espacioTrabajo}

            Dim client = enlaceDatos_.GetMongoClient

            Using session = client.StartSession()

                session.StartTransaction()

                With Estado

                    Try
                        Dim operacionesCollection =
                            enlaceDatos_.GetMongoCollection(Of OperacionGenerica)(
                                (New ConstructorFacturaComercial).GetType.Name)

                        operacionesCollection.InsertOne(session, operacionGenerica_)

                        Dim analysisCollection =
                            enlaceDatos_.GetMongoCollection(Of CommercialInvoiceAnalysis)(
                                "Reg012CommercialInvoicesAnalysis")

                        Dim estructuraGenerica_ =
                            DirectCast(CType(estructura_, Object), CommercialInvoiceAnalysis)

                        analysisCollection.InsertOne(session, estructuraGenerica_)

                        session.CommitTransaction()

                        .ObjectReturned = "Registro insertado ok"

                        .SetOK()

                    Catch ex As Exception

                        session.AbortTransaction()

                        .SetError(Me, "Documento no insertado")

                    End Try

                End With

            End Using

            'Dim collection_ = enlaceDatos_.GetMongoCollection(Of OperacionGenerica)((New ConstructorFacturaComercial).GetType.Name)

            'Dim result_ = collection_.InsertOneAsync(operacionGenerica_)

            'Dim operationsDB_ = enlaceDatos_.GetMongoCollection(Of CommercialInvoiceAnalysis)("CommercialInvoicesAnalysis")

            'Dim estructuraGenerica_ = DirectCast(CType(estructura_, Object), CommercialInvoiceAnalysis)

            'Dim result2_ = operationsDB_.InsertOneAsync(estructuraGenerica_)

            'If result2_.Id <> Nothing Then

            '    .SetOK()

            'End If
            'With Estado
            '    If result_.Id <> Nothing Then
            '        ''LO VAMOS A INTENTAR HACER MAS DESPUES POR TRANSACCION
            '        Dim estructuraGenerica_ = DirectCast(CType(estructura_, Object), CommercialInvoiceAnalysis)
            '        GuardarEstructuraDocumentoElectronico(estructuraGenerica_)
            '        .ObjectReturned = result_
            '        .SetOK()
            '    Else
            '        .SetOKBut(Me, "Documento no insertado")
            '    End If
            'End With
        End Using

        Return Estado

    End Function

    Protected Function GuardarPreasignacionFacturaCommercial(ByRef estructura_ As CommercialInvoiceAnalysis,
                                                             ByRef objectIdCliente_ As ObjectId) As TagWatcher

        ''GENERAMOS EL DOCUMENTO ELECTRONICO U OPERACION GENERICA 'MANUAL'

        Estado = GenerarOperacionGenericaFacturaComercial(estructura_, objectIdCliente_)

        If Estado.Status = TypeStatus.Ok Then

            Dim resultOperation_ As ResponseOperacion = DirectCast(Estado.ObjectReturned, ResponseOperacion)

            Dim operacionGenerica_ As OperacionGenerica = resultOperation_.OperacionGenerica

            Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos(21) With {.EspacioTrabajo = _espacioTrabajo}

                Dim client = enlaceDatos_.GetMongoClient

                Using session = client.StartSession()

                    session.StartTransaction()

                    With Estado

                        Try
                            Dim operacionesCollection =
                            enlaceDatos_.GetMongoCollection(Of OperacionGenerica)(
                                (New ConstructorFacturaComercial).GetType.Name)

                            operacionesCollection.InsertOne(session, operacionGenerica_)

                            Dim analysisCollection =
                            enlaceDatos_.GetMongoCollection(Of CommercialInvoiceAnalysis)(
                                "Reg012CommercialInvoicesAnalysis")

                            Dim estructuraGenerica_ =
                            DirectCast(CType(estructura_, Object), CommercialInvoiceAnalysis)

                            analysisCollection.InsertOne(session, estructuraGenerica_)

                            session.CommitTransaction()

                            .ObjectReturned = "Registro insertado ok"

                            .SetOK()

                        Catch ex As Exception

                            session.AbortTransaction()

                            .SetError(Me, "Documento no insertado")

                        End Try

                    End With

                End Using

            End Using

        End If
        Return Estado

    End Function

    Private Function DeserializeCFDI(xml_ As String) As CFDIFacturaComercial _
        Implements IControladorProcesamientoElectronico.DeserializeCFDI

        Dim serializer As New XmlSerializer(GetType(CFDIFacturaComercial))

        Using reader As New StringReader(xml_)
            Return CType(serializer.Deserialize(reader), CFDIFacturaComercial)
        End Using

    End Function

    Private Function GenerarCommercialInvoiceDesdeCFDI(cfdi As CFDIFacturaComercial) As CommercialInvoiceAnalysis _
        Implements IControladorProcesamientoElectronico.GenerarCommercialInvoiceDesdeCFDI

        _facturaComercialIA = New CommercialInvoiceAnalysis

        With _facturaComercialIA
            .invoicenumber = cfdi.Folio
            .invoicedate = cfdi.Fecha.ToString("yyyy-MM-dd")
            .invoicecurrency = cfdi.Moneda
            .totalinvoice = cfdi.Total
            .invoicecountry = cfdi.Receptor.ResidenciaFiscal
            .invoiceseries = cfdi.UUID
            .customername = cfdi.Emisor.Nombre
            .suppliername = cfdi.Receptor.Nombre
            ' CUSTOMER
            .customer = New Customer With {
                .customerid = Nothing,
                .customername = cfdi.Emisor.Nombre,
                .rfc = cfdi.Emisor.Rfc,
                .country = cfdi.EmisorDomicilio.Pais,
                .address = $"{cfdi.EmisorDomicilio.Calle} {cfdi.EmisorDomicilio.NumeroExterior} {cfdi.EmisorDomicilio.NumeroInterior} {cfdi.EmisorDomicilio.Ciudad} {cfdi.EmisorDomicilio.Localidad} {cfdi.EmisorDomicilio.Estado} {cfdi.EmisorDomicilio.CodigoPostal} {cfdi.EmisorDomicilio.Pais}",
                .city = cfdi.EmisorDomicilio.Ciudad,
                .externalnumber = cfdi.EmisorDomicilio.NumeroExterior,
                .internalnumber = cfdi.EmisorDomicilio.NumeroInterior,
                .locality = cfdi.EmisorDomicilio.Localidad,
                .state = cfdi.EmisorDomicilio.Estado,
                .street = cfdi.EmisorDomicilio.Calle,
                .zipcode = cfdi.EmisorDomicilio.CodigoPostal
            }
            ' SUPPLIER
            .supplier = New Supplier With {
                .supplierid = Nothing,
                .supliername = cfdi.Receptor.Nombre,
                .taxid = cfdi.Receptor.Rfc,
                .country = cfdi.ReceptorDomicilio.Pais,
                .address = $"{cfdi.ReceptorDomicilio.Calle} {cfdi.ReceptorDomicilio.NumeroExterior} {cfdi.ReceptorDomicilio.NumeroInterior} {cfdi.ReceptorDomicilio.Ciudad} {cfdi.ReceptorDomicilio.Localidad} {cfdi.ReceptorDomicilio.Estado} {cfdi.ReceptorDomicilio.CodigoPostal} {cfdi.ReceptorDomicilio.Pais}",
                .city = cfdi.ReceptorDomicilio.Ciudad,
                .externalnumber = cfdi.ReceptorDomicilio.NumeroExterior,
                .internalnumber = cfdi.ReceptorDomicilio.NumeroInterior,
                .locality = cfdi.ReceptorDomicilio.Localidad,
                .state = cfdi.ReceptorDomicilio.Estado,
                .street = cfdi.ReceptorDomicilio.Calle,
                .zipcode = cfdi.ReceptorDomicilio.CodigoPostal
            }
            ' ITEMS
            .items = New List(Of Syn.CustomBrokers.Controllers.Item)

            Dim secuencia As Integer = 1

            Dim i As Integer = 0

            For Each c In cfdi.Conceptos
                .items.Add(New Syn.CustomBrokers.Controllers.Item With {
                    .sec = secuencia,
                    .partnumber = c.NoIdentificacion,
                    .description = c.Descripcion,
                    .quantity = c.Cantidad,
                    .unitprice = c.ValorUnitario,
                    .total = c.Importe,
                    .currency = cfdi.Moneda,
                    .usdvalue = c.Importe,
                    .productid = Nothing,
                    .sku = cfdi.Complemento.ComercioExterior.Mercancias(i).NoIdentificacion,
                    .unit = cfdi.Complemento.ComercioExterior.Mercancias(i).UnidadAduana,
                    .value = cfdi.Complemento.ComercioExterior.Mercancias(i).ValorUnitarioAduana,
                    .discount = Nothing,
                    .netweight = Nothing,
                    .purchaseorder = Nothing,
                    .destinationcountry = Nothing,
                    .origincountry = Nothing
                })
                secuencia += 1
                i += 1
            Next

            ' ADDITIONAL DETAILS
            .additionaldetails = New AdditionalDetails With {
                .incoterm = If(cfdi.Complemento?.ComercioExterior?.Incoterm, Nothing),
                .totalweight = 0
            }

            ''DESTINATARIO
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

            .processdate = Date.Now.ToString("yyyy-MM-dd")

            .confidence = 0.85

            .score = 85

            .environmentid = 0

            .info = "CFDI deserealizado - AWS Textract - CHATGPT - RRR. "
            .analysis = New Ia.Analysis.Analysis With {
            .processdate = Date.Now.ToString("yyyy-MM-dd"),
            .environmentid = 0,
            .confidence = 0.85,
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
                .field = "invoicenumber",
                .value = cfdi.Folio,
                .message = "Folio extraido de cfdi",
                .confidence = 0.95,
                .source = "Details"
                },
              New Ia.Analysis.Messages With {
                .type = "info",
                .action = "extract",
                .field = "invoicedate",
                .value = cfdi.Fecha.ToString("yyyy-MM-dd"),
                .message = "Fecha extraida de cfdi",
                .confidence = 0.95,
                .source = "Details"
              },
              New Ia.Analysis.Messages With {
                .type = "info",
                .action = "extract",
                .field = "invoicecurrency",
                .value = cfdi.Moneda,
                .message = "Moneda extraída de cfdi",
                .confidence = 0.95,
                .source = "Details"
              },
              New Ia.Analysis.Messages With {
                .type = "info",
                .action = "extract",
                .field = "totalinvoice",
                .value = cfdi.Total,
                .message = "Total extraído de cfdi.",
                .confidence = 0.95,
                .source = "Details"
              },
              New Ia.Analysis.Messages With {
                .type = "info",
                .action = "extract",
                .field = "invoicecountry",
               .value = cfdi.Receptor.ResidenciaFiscal,
                .message = "País extraido de cfdi",
                .confidence = 0.95,
                .source = "Details"
              },
              New Ia.Analysis.Messages With {
                .type = "info",
                .action = "extract",
                .field = "incoterm",
                .value = cfdi.Complemento?.ComercioExterior?.Incoterm,
                .message = "Incoterm extraído de cfdi",
                .confidence = 0.7,
                .source = "Header"
              },
              New Ia.Analysis.Messages With {
                .type = "info",
                .action = "extract",
                .field = "invoiceseries",
                .value = cfdi.UUID,
                .message = "UUID extraído de cfdi",
                .confidence = 0.7,
                .source = "Header"
              },
              New Ia.Analysis.Messages With {
                .type = "alert",
                .action = "review",
                .field = "suppliername",
                .value = cfdi.Receptor.Nombre,
                .message = "Receptor extraído de cfdi",
                .confidence = 0,
                .source = "synapsis"
              }}}
            .score = 88.89

        End With

        Return _facturaComercialIA

    End Function



    Protected Async Function GuardarDocumentoAsyn(ByVal session_ As IClientSessionHandle) As Task(Of TagWatcher)

        'Dim estructura_ As New CommercialInvoiceAnalysis With {
        '    .invoicenumber = "XXXXX",
        '    .analysis = New Ia.Analysis.Analysis,
        '    .additionaldetails = New AdditionalDetails,
        '    .invoicecurrency = "xxxx",
        '    .confidence = 22.2,
        '    .consigneedetails = New ConsigneeDetails,
        '    .customer = New Customer,
        '    .customername = "ZZZZZ",
        '    .items = New List(Of Item),
        '    .supplier = New Supplier,
        '    .totalinvoice = 22223.45,
        '    .processdate = "2024/11/22",
        '    .suppliername = "AQQQ",
        '    .score = 34.2,
        '    .invoicecountry = "MEX",
        '    .invoicedate = "2024/11/23",
        '    .invoiceseries = "qqqqqqww",
        '    .environmentid = 2,
        '    .info = "wwww"}

        'Dim operacionGenerica_ As OperacionGenerica = GenerarOperacionGenerica(Of CommercialInvoiceAnalysis)(estructura_)
        'Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = _espacioTrabajo}
        '    Using entidadDatos_ As IEntidadDatos = New ConstructorFacturaComercial()
        '        'Dim client As MongoClient = New MongoClient(ConnectionString)
        '        Using session As IClientSessionHandle = Await enlaceDatos_.GetMongoClient.StartSessionAsync()
        '            'Try
        '            ' Iniciar la transacción
        '            session.StartTransaction()
        '            Dim collection_ = enlaceDatos_.GetMongoCollection(Of OperacionGenerica)((New ConstructorFacturaComercial).GetType.Name)
        '            Await collection_.InsertOneAsync(session_, operacionGenerica_)
        '            Dim collection2_ = enlaceDatos_.GetMongoCollection(Of CommercialInvoiceAnalysis)("CommercialInvoicesAnalysis")
        '            Await collection2_.InsertOneAsync(estructura_)

        '            Await session.CommitTransactionAsync()
        '            ' Catch ex As Exception
        '            ' En caso de error, abortar la transacción
        '            ' Console.WriteLine("Error en la transacción: " & ex.Message)
        '            'Await session.AbortTransactionAsync()
        '            'End Try
        '            Estado.ObjectReturned = "OK"
        '        End Using
        '        'With Estado
        '        '    If result_.Id <> Nothing Then
        '        '        ''LO VAMOS A INTENTAR HACER MAS DESPUES POR TRANSACCION
        '        '        Dim estructuraGenerica_ = DirectCast(CType(estructura_, Object), CommercialInvoiceAnalysis)
        '        '        GuardarEstructuraDocumentoElectronico(estructuraGenerica_)
        '        '        .ObjectReturned = result_
        '        '        .SetOK()
        '        '    Else
        '        '        .SetOKBut(Me, "Documento no insertado")
        '        '    End If
        '        'End With
        '    End Using
        'End Using
        'Return Estado
    End Function


    'Protected Function ProcesarDocumentoPorIA(ByVal documento_ As MemoryStream) As TagWatcher
    '    _listaDocumentos = New List(Of MemoryStream) From {documento_}
    '    _documentAnalizer = New ControllerDocumentAnalyzer(70, ListaTransformes.GptVsTextract)
    '    Estado = _documentAnalizer.ProcessDocumentAsync(Of CommercialInvoiceAnalysis)(_listaDocumentos, DocumentosCargados.FacturaImportacion).Result
    '    Return Estado
    'End Function

    Protected Function ProcesarDocumentosPorIA(Of T)(ByVal listaDocumentos_ As List(Of MemoryStream)) As TagWatcher
        '_documentAnalizer = New ControllerDocumentAnalyzer(Temperature, Transformer)
        'Estado = _documentAnalizer.ProcessDocumentAsync(Of T)(listaDocumentos_, DocumentoCargado).Result
        'Return Estado
    End Function

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: eliminar el estado administrado (objetos administrados)
                _secuencia = Nothing
                _controladorSecuencias = Nothing
                _documentoElectronico = Nothing
                _espacioTrabajo = Nothing
                _tipoDocumento = Nothing
                '_documentAnalizer = Nothing
                _listaDocumentos = Nothing
                _collectionGenerica = Nothing
            End If

            ' TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
            ' TODO: establecer los campos grandes como NULL
            disposedValue = True
        End If
    End Sub

    ' TODO: reemplazar el finalizador solo si "Dispose(disposing As Boolean)" tiene código para liberar los recursos no administrados
    Protected Overrides Sub Finalize()
        ' No cambie este código. Coloque el código de limpieza en el método "Dispose(disposing As Boolean)".
        Dispose(disposing:=False)
        MyBase.Finalize()
    End Sub
#End Region

#Region "MÉTODOS PÚBLICOS"

    Public Sub Dispose() Implements IDisposable.Dispose
        ' No cambie este código. Coloque el código de limpieza en el método "Dispose(disposing As Boolean)".
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub

    Public Sub ReiniciarControlador() _
        Implements IControladorProcesamientoElectronico.ReiniciarControlador
        Inicializa(TiposDocumentoElectronico.SinDefinir, Nothing)
    End Sub

    Public Function GenerarDocumento(Of T)(estructura_ As T) As TagWatcher _
        Implements IControladorProcesamientoElectronico.GenerarDocumento
        With Estado
            If estructura_ IsNot Nothing Then
                Select Case _tipoDocumento
                    Case TiposDocumentoElectronico.FacturaComercial

                        Dim estructuraGenerica_ = DirectCast(CType(estructura_, Object), CommercialInvoiceAnalysis)
                        _collectionGenerica = New ConstructorFacturaComercial
                        GuardarDocumento(Of CommercialInvoiceAnalysis)(estructuraGenerica_, New ConstructorFacturaComercial)
                    Case TiposDocumentoElectronico.ProcesamientoElectronicoDocumento
                        'Dim estructuraGenerica_ = DirectCast(CType(estructura_, Object), DocumentoElectronicoApiStorage)
                        'GuardarDocumento(Of DocumentoElectronicoApiStorage)(estructuraGenerica_, New ConstructorProcesamientoElectDocumentos)
                End Select
            Else
                .SetError(Me, "Archivo no ha sido procesado correctamente")
            End If
        End With
        Return Estado
    End Function
    Public Function GuardarEstructuraDocumento(Of T)(estructura_ As T) As TagWatcher _
        Implements IControladorProcesamientoElectronico.GuardarEstructuraDocumento
        Throw New NotImplementedException()
    End Function

    Public Function ProcesarArchivoConIA(archivo_ As MemoryStream) As TagWatcher _
        Implements IControladorProcesamientoElectronico.ProcesarArchivoConIA
        'With Estado
        '    If archivo_ IsNot Nothing Then
        '        _listaDocumentos = New List(Of MemoryStream) From {archivo_}
        '        ProcesarDocumentosPorIA(Of CommercialInvoiceAnalysis)(_listaDocumentos)
        '    Else
        '        .SetError(Me, "Archivo no ha sido procesado correctamente")
        '    End If
        'End With
        'Return Estado
    End Function

    Public Function ProcesarArchivosConIA(archivos_ As List(Of MemoryStream)) As TagWatcher _
        Implements IControladorProcesamientoElectronico.ProcesarArchivosConIA
        Throw New NotImplementedException()
    End Function

    Public Function ProcesarCFDI(archivo_ As MemoryStream) As TagWatcher _
        Implements IControladorProcesamientoElectronico.ProcesarCFDI

        'Dim leerArchivoCFDI = New gsol.LeerArchivoXML32()

        'leerArchivoCFDI.RutaArchivo = ""

        'Dim tiposArchivos_ = ILeerArchivo.TiposAutomaticos.OtroTipo
        'leerArchivoCFDI.LeerXML(tipoArchivoSistema_:=tiposArchivos_)

        'Throw New NotImplementedException()
        ''Dim ok = GuardarDocumentoAsyn(Nothing)
    End Function

    Public Function ProcesarCFDIs(archivo_ As List(Of MemoryStream)) As TagWatcher _
        Implements IControladorProcesamientoElectronico.ProcesarCFDIs
        Throw New NotImplementedException()
    End Function

    Public Shared Function ListaTiposDocumentos() As TagWatcher
        'Dim estado_ = New TagWatcher
        'Dim ListaDocumentos_ As Dictionary(Of Int16, String) = New Dictionary(Of Int16, String)
        'With ListaDocumentos_
        '    .Add(SubTipoDocumentoElectronico.SIN_DEFINIR, "SELECCIONE")
        '    .Add(SubTipoDocumentoElectronico.FACTURA_COMERCIAL_IMPORTACION_PDF, "FACTURA COMERCIAL IMP (.pdf)")
        '    .Add(SubTipoDocumentoElectronico.FACTURA_COMERCIAL_EXPORTACION_PDF, "FACTURA COMERCIAL EXP (.pdf)")
        '    .Add(SubTipoDocumentoElectronico.FACTURA_COMERCIAL_IMPORTACION_CFDI, "FACTURA COMERCIAL IMP (.cfdi)")
        '    .Add(SubTipoDocumentoElectronico.FACTURA_COMERCIAL_EXPORTACION_CFDI, "FACTURA COMERCIAL IMP (.cfdi)")
        'End With
        'With estado_
        '    .ObjectReturned = ListaDocumentos_
        '    .SetOK()
        'End With
        'Return estado_
    End Function

    Public Function GenerarFacturaComercial(estructura_ As CommercialInvoiceAnalysis,
                                            objectidCliente_ As ObjectId) As TagWatcher _
        Implements IControladorProcesamientoElectronico.GenerarFacturaComercial

        With Estado

            If estructura_ IsNot Nothing Then

                If Not objectidCliente_ = ObjectId.Empty Then

                    GuardarPreasignacionFacturaCommercial(estructura_, objectidCliente_)

                Else

                    .SetOKBut(Me, "Objectid del cliente es requerido")

                End If
            Else

                .SetOKBut(Me, "Commercial invoice es requerida")

            End If

        End With

        Return Estado

    End Function

#End Region

End Class

Public Class ResponseOperacion
    Public Property OperacionGenerica As OperacionGenerica
    Public Property CommercialInvoice As CommercialInvoiceAnalysis
End Class

Public Class EstructuraCliente
    Inherits Customer
    Public Property id As String
    Public Property cve_cliente As String
    Public Property taxid As String
    Public Property curp As String
    Public Property id_domicilio As String
    Public Property sec_domicilio As String
    Public Property colonia As String
    Public Property municipio As String
    Public Property cveEntidadFederativa As String
    Public Property entidadFederativa As String
    Public Property cvePais As String
End Class


