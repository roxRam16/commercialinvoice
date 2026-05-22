Imports Wma.Exceptions
Imports MongoDB.Bson
Imports System.IO
Imports Syn.Utils
Imports System.Collections.Generic
Imports Ia.Pln

Public Class ControllerDocumentAnalyzer
    Implements IControllerDocumentAnalyzer

    Private _organismo As Organismo

    Private _gpt As IControllerChatGPT

    Private _textract As IControllerTextract

    Private _tipoTransformer As IControllerDocumentAnalyzer.Transformer

    Private _temperature As Int32

    Private _directivesGPT As String

    Private _secretKeyAWS As String

    Private _accessKeyAWS As String

    Public Property Status As TagWatcher Implements IControllerDocumentAnalyzer.Status

    Public Property SecretKey As String Implements IControllerDocumentAnalyzer.SecretKey

#Region "Constructores"

    Sub New(temperature_ As Int32,
            Optional transformer_ As IControllerDocumentAnalyzer.Transformer = IControllerDocumentAnalyzer.Transformer.GPT)

        _temperature = temperature_

        Inicializa(transformer_)

    End Sub

    Sub Inicializa(Optional transformer_ As IControllerDocumentAnalyzer.Transformer = IControllerDocumentAnalyzer.Transformer.GPT)

        _tipoTransformer = transformer_

        _organismo = New Organismo

        _directivesGPT = Nothing

    End Sub

#End Region

    Public Async Function ProcessDocumentAsync(Of T)(ByVal document_ As List(Of MemoryStream),
                                          Optional ByVal documetoCargado As IControllerChatGPT.DocumentoCargado = IControllerChatGPT.DocumentoCargado.BL,
                                          Optional prompt_ As String = Nothing) As Task(Of TagWatcher) _
                                                Implements IControllerDocumentAnalyzer.ProcessDocumentAsync

        Dim imagenByte_ As List(Of Byte()) = _organismo.ConvertirPDFaByte(document_(0))

        Dim base64String_ As String = Convert.ToBase64String(imagenByte_(0))

        _gpt = New ControllerChatGPT(_temperature, documetoCargado)

        _textract = New ControllerTextract(_temperature, documetoCargado)

        If _directivesGPT IsNot Nothing Then

            _gpt.AddDirectives(_directivesGPT)

            _textract.DirectivesGPT = _directivesGPT

        End If

        If SecretKey IsNot Nothing Then

            _gpt.ApiKey = SecretKey

            _textract.SecretKeyGPT = SecretKey

        Else

            setApiKeys()

            _gpt.ApiKey = SecretKey

            _textract.SecretKeyGPT = SecretKey

        End If

        _textract.setKeys(_secretKeyAWS, _accessKeyAWS)

        '_tipoTransformer = IControllerDocumentAnalyzer.Transformer.Textract_Gpt
        'recurso 
        Select Case _tipoTransformer

            Case IControllerDocumentAnalyzer.Transformer.GPT

                If prompt_ Is Nothing Then

                    Status = _gpt.DocumentAnalyzer(Of T)(imagenByte_).Result

                Else

                    Status = _gpt.DocumentAnalyzer(Of T)(imagenByte_, prompt_).Result

                End If

            Case IControllerDocumentAnalyzer.Transformer.Textract_Gpt

                If prompt_ Is Nothing Then

                    Status = _textract.DocumentAnalyzer(Of T)(document_(0).ToArray).Result

                Else

                    Status = _textract.DocumentAnalyzer(Of T)(document_(0).ToArray, prompt_).Result

                End If

            Case IControllerDocumentAnalyzer.Transformer.GptVsTextract

                Dim documentTextract_ As T

                Dim documentGpt_ As T

                Dim textractTask_ = _textract.DocumentAnalyzer(Of T)(document_(0).ToArray, prompt_)

                Dim gptTask_ As Task(Of TagWatcher)

                If prompt_ Is Nothing Then

                    gptTask_ = _gpt.DocumentAnalyzer(Of T)(imagenByte_)

                Else

                    gptTask_ = _gpt.DocumentAnalyzer(Of T)(imagenByte_, prompt_)

                End If

                ' Esperar a que ambas tareas terminen
                Await Task.WhenAll(textractTask_, gptTask_)

                Dim textractStatus_ As TagWatcher = textractTask_.Result

                Dim gptStatus_ As TagWatcher = gptTask_.Result

                If textractStatus_.Status = TagWatcher.TypeStatus.Ok Then

                    documentTextract_ = textractStatus_.ObjectReturned

                    If gptStatus_.Status = TagWatcher.TypeStatus.Ok Then

                        Dim evaluator_ = New DocumentResultEvaluator

                        documentGpt_ = gptStatus_.ObjectReturned

                        Status = evaluator_.CompareResults(documentTextract_, documentGpt_)

                    End If

                End If

        End Select

        Return Status

    End Function

    Public Sub AddDirectivesGPT(newDirectives As String) _
        Implements IControllerDocumentAnalyzer.AddDirectivesGPT

        _directivesGPT += " " & newDirectives

    End Sub

    Public Function GetResponse(operationNumber As ObjectId) As TagWatcher Implements IControllerDocumentAnalyzer.GetResponse
        Throw New NotImplementedException()
    End Function

    Private Sub setApiKeys()

        Dim statements_ As Sax.SaxStatements = Sax.SaxStatements.GetInstance(13)

        Dim saxappid_ As Int32 = 18

        Dim settingstypestr_ As String = "project"

        Dim servicesGPT = statements_.GetServiceOfficeOnline(settingstypestr_, 18, 1)

        Dim servicesAWS = statements_.GetService(settingstypestr_, 18, 1, 2)

        SecretKey = servicesGPT.environmentsettings(0).security.secretkey

        _accessKeyAWS = servicesAWS.environmentsettings(0).security.accesskey

        _secretKeyAWS = servicesAWS.environmentsettings(0).security.secretkey

    End Sub

End Class
