Imports MongoDB.Bson
Imports System.IO
Imports Wma.Exceptions

Public Interface IControllerDocumentAnalyzer

#Region "Enums"
    Enum Transformer As Int16
        GPT = 1
        Textract_Gpt = 2
        GptVsTextract = 3
        Otro = 99
    End Enum

#End Region

#Region "Properties"

    Property Status As TagWatcher

    Property SecretKey As String

#End Region

#Region "Funciones"

    Function ProcessDocumentAsync(Of T)(ByVal document_ As List(Of MemoryStream),
                                   Optional ByVal documetoCargado As IControllerChatGPT.DocumentoCargado = IControllerChatGPT.DocumentoCargado.BL,
                                   Optional prompt_ As String = Nothing) As Task(Of TagWatcher)

    Sub AddDirectivesGPT(newDirectives As String)
    Function GetResponse(ByVal operationNumber_ As ObjectId) As TagWatcher

#End Region

End Interface
