Imports Wma.Exceptions
Imports MongoDB.Bson

Public Interface IControllerChatGPT

    Enum DocumentoCargado
        BL = 1
        FacturaImportacion = 2
        FacturaExportacion = 3
    End Enum

#Region "Properties"

    Property Status As TagWatcher

    Property ApiKey As String

#End Region

#Region "Funciones"

    Function DocumentAnalyzer(Of T)(ByVal document_ As List(Of Byte())) As Task(Of TagWatcher)
    Function ProcessData(Of T)(ByVal documentJson As BsonDocument) As TagWatcher
    Function DocumentAnalyzer(Of T)(ByVal document_ As List(Of Byte()), prompt_ As String) As Task(Of TagWatcher)
    Function ProcessData(Of T)(ByVal documentJson As BsonDocument, prompt_ As String) As TagWatcher
    Function GetResponse(ByVal operationNumber_ As ObjectId) As TagWatcher
    Function AskToChatGPT(message As String) As String
    Sub AddDirectives(newDirectives As String)
    Sub SetGptParameters(temperaturaAnalysis_ As Int32, Optional documentoCargado_ As IControllerChatGPT.DocumentoCargado = IControllerChatGPT.DocumentoCargado.BL)

#End Region

End Interface
