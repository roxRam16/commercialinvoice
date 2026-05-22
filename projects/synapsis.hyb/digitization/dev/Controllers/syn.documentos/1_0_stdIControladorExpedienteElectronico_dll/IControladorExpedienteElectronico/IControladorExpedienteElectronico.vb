Imports Wma.Exceptions
Imports Syn.CustomBrokers.Controllers
Imports MongoDB.Bson
Imports System.Runtime.CompilerServices
Imports AuxiliarDatosExpedienteElectronico64

Public Interface IControladorExpedienteElectronico

#Region "PROPIEDADES PRIVADAS"
    Property Estado As TagWatcher

#End Region

#Region "Enum"
    Enum EnvironmentsExpediente
        SinDefinir = 0
        Veracruz = 1
        CDMX = 2
        Manzanillo = 3
        Altamira = 4
        Toluca = 5
        LazaroCardenas = 6
        Tuxpan = 7
        NuevoLaredo = 8
        Nogales = 9
    End Enum
#End Region

#Region "API"
    Function SubirDocumentosGCS(ByVal listadocumentos_ As List(Of DocumentoElectronicoApiStorage),
                                            ByVal taxidcliente_ As String,
                                            ByVal environment_ As Int16) As Task(Of TagWatcher)

    Function AsignarReferenciaDocumentosGCS(ByVal listaIdsDocumentos_ As List(Of ObjectId),
                                            ByVal taxid_cliente As String,
                                            ByVal referencia_ As String) As Task(Of TagWatcher)

    Function DescargarDocumentosGCS() As Task(Of TagWatcher)

    Function DescargarPaqueteGCS() As Task(Of TagWatcher)

    Function ObtenerCatalogoTipoUso1Api() As Task(Of TagWatcher)

    Function RecuperarLinksDescargaDocumento(ByVal storagepath_ As String) As TagWatcher

#End Region

#Region "Controlador"
    Sub ReiniciarControlador()

    Function AbrirExpediente(ByVal datosExpedienteElectronico_ As AuxiliarDatosExpedienteElectronico,
                             Optional ByVal conReferencia_ As Boolean = False) As TagWatcher

    Function AgregarReferenciasAExpediente(ByVal idexpediente_ As ObjectId,
                                           ByVal listaReferencias_ As List(Of AuxliarDatosReferencia)) As TagWatcher

    ''Evaluar el método, listo para habilitar. Crear en implementación
    'Function CerrarExpediente(ByVal idexpediente_ As ObjectId,
    '                          ByVal taxidCliente_ As String) As TagWatcher

    Function CerrarExpediente(ByVal idexpediente_ As ObjectId,
                              ByVal idcliente_ As ObjectId) As TagWatcher

    ''Evaluar el método, listo para habilitar. Crear en implementación
    'Function CerrarReferenciaEnExpediente(ByVal idexpediente_ As ObjectId,
    '                                      ByVal referencia_ As String) As TagWatcher

    Function CerrarReferenciaEnExpediente(ByVal idexpediente_ As ObjectId,
                                          ByVal idreferencia_ As ObjectId) As TagWatcher

    Function ObtenerExpedientes(Optional ByVal environment_ As EnvironmentsExpediente = EnvironmentsExpediente.Veracruz) As TagWatcher

    Function ObtenerExpedientesPorCliente(ByVal idcliente_ As ObjectId,
                                         Optional ByVal environment_ As EnvironmentsExpediente = EnvironmentsExpediente.Veracruz) As TagWatcher

    ''Evaluar el método, listo para habilitar. Crear en implementación
    'Function ObtenerExpedientesPorCliente(ByVal taxidCliente_ As String,
    '                                     Optional ByVal environment_ As String = "Veracruz") As TagWatcher

    Function ObtenerExpedientePorCliente(ByVal idexpediente_ As ObjectId,
                                         ByVal idcliente_ As ObjectId,
                                         Optional ByVal environment_ As EnvironmentsExpediente = EnvironmentsExpediente.Veracruz) As TagWatcher

    ''Evaluar el método, listo para habilitar. Crear en implementación
    'Function ObtenerExpedientePorCliente(ByVal idexpediente_ As ObjectId,
    '                                     ByVal taxidCliente_ As String,
    '                                     Optional ByVal environment_ As String = "Veracruz") As TagWatcher

    Function ObtenerExpedientePorReferencia(ByVal idcliente_ As ObjectId,
                                            ByVal idreferencia_ As ObjectId,
                                            Optional ByVal environment_ As EnvironmentsExpediente = EnvironmentsExpediente.Veracruz) As TagWatcher

    ''Evaluar el método, listo para habilitar. Crear en implementación
    'Function ObtenerExpedientePorReferencia(ByVal taxidCliente_ As String,
    '                                        ByVal referencia_ As String,
    '                                        Optional ByVal environment_ As String = "Veracruz") As TagWatcher

    Function ObtenerExpedientePorOwner(ByVal idcliente_ As ObjectId,
                                       ByVal idowner_ As ObjectId,
                                       Optional ByVal environment_ As EnvironmentsExpediente = EnvironmentsExpediente.Veracruz) As TagWatcher

    ''Evaluar el método, listo para habilitar. Crear en implementación
    'Function ObtenerExpedientePorOwner(ByVal taxidCliente As String,
    '                                   ByVal idowner_ As ObjectId,
    '                                   Optional ByVal environment_ As String = "Veracruz") As TagWatcher

    ''Evaluar el método, listo para habilitar. Crear en implementación
    'Function ObtenerExpedientePorOwner(ByVal taxidCliente As String,
    '                                   ByVal owner_ As String,
    '                                   Optional ByVal environment_ As String = "Veracruz") As TagWatcher

#End Region
End Interface
