Imports System.IO
Imports MongoDB.Bson
Imports Syn.CustomBrokers.Controllers.Digitalization
Imports Syn.Documento
Imports Wma.Exceptions

Public Interface IControladorFacturaComercial : Inherits IDisposable

#Region "Enum"
    Enum Disponibilidades

        SinDefinir = 0

        Cerrado = 1

        Abierto = 2

    End Enum

    Enum Modalidades

        Interno = 0

        Externo = 1

    End Enum

    Enum TipoOperaciones

        SinDefinir = 0

        Importacion = 1

        Exportacion = 2

    End Enum

    'Enum TipoCargasFactura

    '    SinDefinir = 0

    '    ImpoManual = 1

    '    ImpoIA = 2

    '    ImpoSubdivision = 3

    '    ExpoManual = 4

    '    ExpoCFDI = 5

    'End Enum

#End Region

#Region "Propiedades"
    Property FacturasComerciales As List(Of ConstructorFacturaComercial)

    ReadOnly Property Documento As DocumentoElectronico

    ReadOnly Property Documentos As List(Of DocumentoElectronico)

    Property Estado As TagWatcher

    Property ModalidadTrabajo As Modalidades

    WriteOnly Property ConservarFacturas As Boolean

    Property Entorno As Integer

    Property DisponibilidadRecurso As Disponibilidades

    ReadOnly Property FactorConfiabilidadIA As Double

    Property TipoOperacion As TipoOperaciones

    Property EnvironmentOnline As Int32

    ' Property TipoCargaFactura As TipoCargasFactura

#End Region

#Region "Métodos"
    Sub ReiniciarControlador(Optional ByVal entorno_ As Integer = 1)

    Sub CargaFacturas(ByVal documentoDigital_ As MemoryStream)
    Sub CargaFacturas(ByVal documentoDigital_ As List(Of MemoryStream))

#End Region

#Region "Funciones"
    Function ActualizarDatosAcuseValor(ByVal idFactura_ As ObjectId,
                                       ByVal valoresAcuseValor_ As Dictionary(Of [Enum], String)) As TagWatcher
    Function ListaFacturas(ByVal idFactura_ As ObjectId) As TagWatcher

    Function ListaFacturas(ByVal idsFacturas_ As List(Of ObjectId)) As TagWatcher

    Function ListaFacturas(ByVal folioFactura_ As String) As TagWatcher

    Function ListaFacturas(ByVal foliosFacturas_ As List(Of String)) As TagWatcher

    Function CargaFacturas(ByVal listaFacturas_ As List(Of ConstructorFacturaComercial)) As TagWatcher

    Function FirmaDigital(Of T)(ByVal idFactura As ObjectId) As String

    Function FacturaDisponible(ByVal idFactura_ As ObjectId) As Boolean

    Function FacturaDisponible(ByVal folioFactura_ As String) As Boolean

    Function TotalIncrementables(fechaMoneda_ As Date) As TagWatcher

    Function TotalIncrementables(ByVal idFactura_ As ObjectId,
                                 fechaMoneda_ As Date) As TagWatcher

    Function TotalIncrementables(ByVal idsFacturas_ As List(Of ObjectId),
                                 fechaMoneda_ As Date) As TagWatcher

    Function TotalIncrementables(ByVal folioFactura_ As String,
                                 fechaMoneda_ As Date) As TagWatcher

    Function TotalIncrementables(ByVal foliosFacturas_ As List(Of String),
                                 fechaEntrada_ As Date) As TagWatcher


    Function ListarIncrementables(fechaMoneda_ As Date) As TagWatcher

    Function ListarIncrementables(ByVal idFactura_ As ObjectId,
                                 fechaMoneda_ As Date) As TagWatcher

    Function ListarIncrementables(ByVal idsFacturas_ As List(Of ObjectId),
                                 fechaMoneda_ As Date) As TagWatcher

    Function ListarIncrementables(ByVal folioFactura_ As String,
                                 fechaMoneda_ As Date) As TagWatcher

    Function ListarIncrementables(ByVal foliosFacturas_ As List(Of String),
                                 fechaEntrada_ As Date) As TagWatcher

    Function ListaIncrementables() As TagWatcher

    Function ListaIncrementables(ByVal idFactura_ As ObjectId) As TagWatcher

    Function ListaIncrementables(ByVal idsFacturas_ As List(Of ObjectId)) As TagWatcher

    Function ListaIncrementables(ByVal folioFactura_ As String) As TagWatcher

    Function ListaIncrementables(ByVal foliosFacturas_ As List(Of String)) As TagWatcher

    Function ListaIncoterms() As TagWatcher

    Function ListaIncoterms(ByVal idFactura_ As ObjectId) As TagWatcher

    Function ListaIncoterms(ByVal idsFacturas_ As List(Of ObjectId)) As TagWatcher

    Function ListaIncoterms(ByVal folioFactura_ As String) As TagWatcher

    Function ListaIncoterms(ByVal foliosFacturas_ As List(Of String)) As TagWatcher

    Function ListaPartidas() As TagWatcher

    Function ListaPartidas(ByVal idFactura_ As ObjectId) As TagWatcher

    Function ListaPartidas(ByVal idsFacturas_ As List(Of ObjectId)) As TagWatcher

    Function ListaPartidas(ByVal folioFactura_ As String) As TagWatcher

    Function ListaPartidas(ByVal foliosFacturas_ As List(Of String)) As TagWatcher

    Function ListaCamposFacturaComercial(ByVal idFactura_ As ObjectId,
                         ByVal seccionesCampos_ As Dictionary(Of [Enum],
                         List(Of [Enum]))) As TagWatcher

    Function ListaCamposFacturaComercial(ByVal idsFacturas_ As List(Of ObjectId),
                         ByVal seccionesCampos_ As Dictionary(Of [Enum],
                         List(Of [Enum]))) As TagWatcher

    Function ListaCamposFacturaComercial(ByVal folioFactura_ As String,
                         ByVal seccionesCampos_ As Dictionary(Of [Enum],
                         List(Of [Enum]))) As TagWatcher

    Function ListaCamposFacturaComercial(ByVal foliosFacturas_ As List(Of String),
                         ByVal seccionesCampos_ As Dictionary(Of [Enum],
                         List(Of [Enum]))) As TagWatcher

    Function ConsultaValorDolaresFactura(fechaMoneda_ As Date) As TagWatcher

    Function ConsultaValorDolaresFactura(ByVal idFactura_ As ObjectId,
                                         fechaMoneda_ As Date) _
                                         As TagWatcher

    Function ConsultaValorDolaresFactura(ByVal idsFacturas_ As List(Of ObjectId),
                                         fechaMoneda_ As Date) As TagWatcher

    Function ConsultaValorDolaresFactura(ByVal folioFactura_ As String,
                                         fechaMoneda_ As Date) As TagWatcher

    Function ListaFacturasProveedor(ByVal idProveedor_ As ObjectId, ByVal idCliente_ As ObjectId,
                                    Optional facturapublicada_ As Boolean = True) As TagWatcher

    Function ConsultaValorDolaresFactura(ByVal foliosFacturas_ As List(Of String),
                                         fechaMoneda_ As Date) As TagWatcher

    Function ConsultaPLNFactura(ByVal consulta_ As String) As BsonDocument

    Function ObtenerEstructura(ByVal numeroFactura_ As String) As TagWatcher

    Function ConsultarExistenciaFacturaComercial(ByVal numeroFactura_ As String,
                                                 ByVal idProveedor As ObjectId,
                                                 ByVal fechaFactura_ As String) As TagWatcher
    Function ConsultarExistenciaFacturaComercial(ByVal numeroFactura_ As String,
                                                 ByVal idCliente_ As ObjectId,
                                                 ByVal idProveedor_ As ObjectId,
                                                 ByVal fechaFactura_ As String) As TagWatcher

    Function GenerarSubdivisionFacturaComercial(ByVal constructorFacturaComercial_ As ConstructorFacturaComercial,
                                                ByVal objectidPreasignacionFacturaOriginal_ As ObjectId) As TagWatcher

    Function ObtenerFacturasComercialesPublicadas(ByVal listaObjectsIdFacturas_ As List(Of ObjectId)) As TagWatcher

    Function ObtenerFacturasComercialesSinVincularPedimento(ByVal listaObjectsIdFacturas_ As List(Of ObjectId)) As TagWatcher

    Function AsociarFacturasPedimento(listaObjectIdFacturas_ As List(Of ObjectId), idPedimento As ObjectId) As TagWatcher

    Function ValidarMarcaFactura(idFacturas_ As List(Of ObjectId), idPedimento As ObjectId) As TagWatcher

    Function BuscarAcuseValor(idFactura_ As ObjectId) As TagWatcher

    Function DeserializarCFDI(xml_ As String, user_ As String) As TagWatcher

    Function DesasociarFacturasPedimento(listaObjectidFacturas_ As List(Of ObjectId), idPedimento As ObjectId) As TagWatcher

    Function AsociarDocumentos(operacionGenerica_ As OperacionGenerica,
                               listadocumentosAsociados_ As List(Of DocumentoAsociado)) As TagWatcher

    Function ObtenerFacturasComercialesPublicadasParaPedimento(ByVal listaObjectsIdFacturas_ As List(Of ObjectId)) As TagWatcher

    Function ListaFacturasProveedorParaPedimento(ByVal idProveedor_ As ObjectId, ByVal idCliente_ As ObjectId) As TagWatcher

    Function GenerarFacturaComercialDesdeComercialInvoiceAnalizer(ByVal comercialinvoiceAnalizer_ As CommercialInvoiceAnalysis,
                                                                  ByVal userGenero_ As String) As TagWatcher

    Function GenerarFacturaComercialDesdeComercialInvoiceAnalizer(ByVal comercialinvoiceAnalizer_ As CommercialInvoiceAnalysis,
                                                                  ByVal userGenero_ As String, ByVal idCustomOperGenerica_ As ObjectId) As TagWatcher

    Function GenerarFacturaComercialSubdividible(ByVal constructorFacturaComercial_ As ConstructorFacturaComercial,
                                                 ByVal objectidPreasignacionFacturaOriginal_ As ObjectId) As TagWatcher

    Function GenerarFacturaComercialDesdeSubdivision(ByVal comercialinvoiceSubdividida_ As SubdivisionFacturaComercial,
                                                     ByVal idOperGenericaOriginal_ As ObjectId,
                                                     ByVal idCustomOperGenerica_ As ObjectId,
                                                     ByVal userGenero_ As String) As TagWatcher

#End Region

End Interface