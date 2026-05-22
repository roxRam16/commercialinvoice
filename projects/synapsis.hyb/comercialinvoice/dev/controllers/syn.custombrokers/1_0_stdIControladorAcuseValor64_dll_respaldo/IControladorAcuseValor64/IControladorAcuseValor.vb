Imports MongoDB.Bson
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Wma.Exceptions

Public Interface IControladorAcuseValor : Inherits IDisposable


#Region "Propiedades"

    Property AcusesValorGenerados As List(Of ConstructorAcuseValor)

    Property BulkCamposPedidos As Dictionary(Of ObjectId, List(Of Nodo))

    Property Estado As TagWatcher

#End Region

#Region "Métodos"

    Function GenerarAcuseValor(constructorAcuseValor_ As ConstructorAcuseValor,
                               certPath_ As String,
                               keyPath_ As String,
                               userName_ As String,
                               certPassword_ As String,
                               webServicePassoword_ As String,
                               Optional adendar_ As Boolean = False) As TagWatcher

    Function GenerarAcuseValor(constructorAcuseValor_ As ConstructorAcuseValor,
                               certBytes_ As Byte(),
                               keyBytes_ As Byte(),
                               userName_ As String,
                               certPassword_ As String,
                               webServicePassoword_ As String,
                               Optional adendar_ As Boolean = False) As TagWatcher

    Function ConsultaAcusesValor(idAcusesValor_ As List(Of ObjectId),
                                 Optional campos_ As Dictionary(Of [Enum], List(Of [Enum])) = Nothing) As TagWatcher

    Function ConsultaAcuseValor(idAcuseValor_ As ObjectId,
                                Optional campos_ As Dictionary(Of [Enum], List(Of [Enum])) = Nothing) As TagWatcher


    Function DescargarXML(idAcuses_ As List(Of ObjectId)) As TagWatcher

    Function DescargarPDF(idAcusesValor_ As List(Of ObjectId)) As TagWatcher

    Function ActualizarIDS(idCOVE As ObjectId) As TagWatcher

    Function ObtenerAcuseValor(idAcuseValor_ As ObjectId) As TagWatcher


    Function EnvioCOVE(documentoElectronico_ As ConstructorAcuseValor,
                             certPath_ As String,
                             keyPath_ As String,
                             userName_ As String,
                             certPassword_ As String,
                             webServicePassoword_ As String) As String

    Function EnvioCOVE(documentoElectronico_ As ConstructorAcuseValor,
                             certBytes As Byte(),
                             keyBytes As Byte(),
                             userName_ As String,
                             certPassword_ As String,
                             webServicePassoword_ As String) As String


    Function ObtenerPDF(resultCove_ As String) As Byte()

    Function ObtenerXML(idAcuseValor_ As ObjectId) As String

    Function ObtenerPDF(idCove As ObjectId) As Byte()

    Function ObtenerPDFEdocument(resultCove_ As String) As Byte()

    Function PruebaEnvioMV(documentoElectronico_ As ConstructorAcuseValor) As String

    Function RegistrarEdocument(fileName_ As String,
                                 edocument_ As Byte(),
                                 email_ As String,
                                 idDocumentType_ As Integer,
                                 rfcConsulta_ As String,
                                 Optional idCustomer_ As ObjectId = Nothing,
                                 Optional reference_ As String = Nothing,
                                 Optional force_ As Boolean = True) As String

    Function ImprimirCove(document_ As ConstructorAcuseValor) As Byte()

    Function ImprimirCove(objectId_ As ObjectId) As Byte()


#End Region

End Interface



