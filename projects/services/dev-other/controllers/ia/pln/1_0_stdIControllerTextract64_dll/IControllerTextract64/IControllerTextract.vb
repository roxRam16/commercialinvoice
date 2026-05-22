Imports Wma.Exceptions

Public Interface IControllerTextract

    Property Status As TagWatcher

    Property SecretKeyGPT As String

    Property DirectivesGPT As String

    Function DocumentAnalyzer(Of T)(document_ As Byte(), Optional prompt_ As String = Nothing) As Task(Of TagWatcher)

    Function setKeys(secretKey_ As String, accesKey_ As String)

End Interface
