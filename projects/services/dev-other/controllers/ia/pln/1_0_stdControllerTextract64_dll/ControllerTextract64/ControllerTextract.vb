Imports Wma.Exceptions
Imports MongoDB.Bson
Imports System.IO
Imports Newtonsoft.Json
Imports Amazon
Imports Amazon.S3
Imports Amazon.S3.Model
Imports AmazonTextract = Amazon.Textract
Imports AmazonTextractModel = Amazon.Textract.Model
Imports Amazon.Textract.Model
Imports Syn.CustomBrokers.Controllers
Imports Amazon.Textract
Imports Newtonsoft.Json.Linq
Imports System.Net
Imports Amazon.S3.Util.S3EventNotification
Imports System.Text

Public Class ControllerTextract
    Implements IControllerTextract

    Private _accessKey As String

    Private _secretKey As String

    Private _clientTextract As AmazonTextract.IAmazonTextract

    Private Const _bucketName As String = "michelin-bucket" '"my-krombucket-test"

    Private ReadOnly _bucketRegion As RegionEndpoint = RegionEndpoint.USEast2

    Private _clientS3 As IAmazonS3

    Private _temperature As Int32

    Public Property Status As TagWatcher Implements IControllerTextract.Status

    Public Property SecretKeyGPT As String Implements IControllerTextract.SecretKeyGPT

    Public Property DirectivesGPT As String Implements IControllerTextract.DirectivesGPT

    Sub New(temperature_ As Int32, Optional documentoCargado_ As IControllerChatGPT.DocumentoCargado = IControllerChatGPT.DocumentoCargado.FacturaImportacion)

        '_accessKey = "AKIAZ5TC5CTHYFISAOG3" '"AKIA3M7ACVIAKCNGSUVT"

        '_secretKey = "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" '"3xN/H/htTfa70c2+/XSyvbBDRC3QklM2wgPbbzcl"

        '_clientS3 = New AmazonS3Client(_accessKey, _secretKey, _bucketRegion)

        '_clientTextract = New AmazonTextract.AmazonTextractClient(_accessKey, _secretKey, _bucketRegion)

        _temperature = temperature_

        SecretKeyGPT = Nothing

        DirectivesGPT = Nothing

    End Sub

    Public Async Function DocumentAnalyzer(Of T)(document_ As Byte(), Optional prompt_ As String = Nothing) As Task(Of TagWatcher) _
        Implements IControllerTextract.DocumentAnalyzer

        _clientTextract = New AmazonTextractClient(_accessKey, _secretKey, _bucketRegion)

        _clientS3 = New AmazonS3Client(_accessKey, _secretKey, _bucketRegion)

        Dim key_ = "Factura" & Guid.NewGuid().ToString & ".pdf"

        Dim swIncoterm_ = False

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        ' Desactivar la validación de certificados SSL (solo para desarrollo)
        ServicePointManager.ServerCertificateValidationCallback =
            Function(sender, certificate, chain, sslPolicyErrors) True

        Using memoryStream As New MemoryStream(document_)
            Dim putRequest_ As New PutObjectRequest() With {
                .BucketName = _bucketName,
                .Key = key_,
                .InputStream = memoryStream,
                .ContentType = "application/pdf"
            }

            Dim putResponse_ As PutObjectResponse = Await _clientS3.PutObjectAsync(putRequest_).ConfigureAwait(False)

        End Using

        Dim startExpenseAnalysisRequest_ As New StartExpenseAnalysisRequest() With {
            .DocumentLocation = New DocumentLocation With {
                .S3Object = New AmazonTextractModel.S3Object With {
                    .Bucket = _bucketName,
                    .Name = key_
                }
            }
        }
        Dim startExpenseAnalysisResponse_ As StartExpenseAnalysisResponse = Await _clientTextract.StartExpenseAnalysisAsync(startExpenseAnalysisRequest_).ConfigureAwait(False)

        Dim jobId_ As String = startExpenseAnalysisResponse_.JobId

        Dim getResponse_ As GetExpenseAnalysisResponse = Nothing

        Dim jobStatus_ As String = "IN_PROGRESS"

        Do

            Await Task.Delay(2000)

            Dim getRequest As New GetExpenseAnalysisRequest() With {
                .JobId = jobId_
            }

            getResponse_ = Await _clientTextract.GetExpenseAnalysisAsync(getRequest)

            jobStatus_ = getResponse_.JobStatus

        Loop While jobStatus_ = "IN_PROGRESS"

        Dim invoice_ As New Invoice()

        invoice_.Header = New List(Of FieldsHeader)()

        invoice_.Details = New List(Of Items)()

        If getResponse_.ExpenseDocuments.Count > 0 Then

            Dim jsonOutput As String = JsonConvert.SerializeObject(getResponse_.ExpenseDocuments, Formatting.Indented)

            'Dim filePathFeo As String = "C:\zamora\resultado_textractFeo.json"
            'File.WriteAllText(filePathFeo, jsonOutput)

            For Each expenseDocument_ As ExpenseDocument In getResponse_.ExpenseDocuments

                For Each summaryField_ As ExpenseField In expenseDocument_.SummaryFields

                    Dim FieldHeader_ As New FieldsHeader With {
                    .Label = summaryField_.Type?.Text,
                    .Value = summaryField_.ValueDetection?.Text,
                    .confidence = summaryField_.ValueDetection?.Confidence
                }

                    invoice_.Header.Add(FieldHeader_)

                    If summaryField_.Type?.Text.Contains("INCOTERM") Then

                        swIncoterm_ = True

                    End If
                Next

                If expenseDocument_.LineItemGroups IsNot Nothing Then

                    For Each lineItemGroup_ As LineItemGroup In expenseDocument_.LineItemGroups

                        For Each lineItem_ As LineItemFields In lineItemGroup_.LineItems

                            Dim items_ As New Items()

                            items_.Fields = New List(Of Field)()

                            For Each lineItemField_ As ExpenseField In lineItem_.LineItemExpenseFields

                                Dim field_ As New Field With {
                                    .Label = lineItemField_.Type?.Text,
                                    .Value = lineItemField_.ValueDetection?.Text,
                                    .confidence = lineItemField_.ValueDetection?.Confidence
                                }

                                items_.Fields.Add(field_)

                            Next

                            invoice_.Details.Add(items_)

                        Next

                    Next

                End If

                If swIncoterm_ = False Then

                    Dim queryText As String = "Incoterm"
                    Dim queryRequest As New StartDocumentAnalysisRequest() With {
                        .DocumentLocation = New DocumentLocation With {
                            .S3Object = New AmazonTextractModel.S3Object With {
                                .Bucket = _bucketName,
                                .Name = key_
                            }
                        },
                        .FeatureTypes = New List(Of String) From {"QUERIES"},
                        .QueriesConfig = New QueriesConfig With {
                            .Queries = New List(Of Query) From {
                                New Query With {
                                    .Text = queryText,
                                    .Alias = "IncotermQuery"
                                }
                            }
                        }
                    }

                    Dim queryResponse = Await _clientTextract.StartDocumentAnalysisAsync(queryRequest)
                    Dim queryJobId = queryResponse.JobId
                    Dim queryStatus As String = "IN_PROGRESS"
                    Dim queryResults As GetDocumentAnalysisResponse = Nothing

                    Do
                        Await Task.Delay(2000)
                        Dim queryResultRequest As New GetDocumentAnalysisRequest() With {.JobId = queryJobId}
                        queryResults = Await _clientTextract.GetDocumentAnalysisAsync(queryResultRequest)
                        queryStatus = queryResults.JobStatus
                    Loop While queryStatus = "IN_PROGRESS"

                    ' Procesar los resultados del Query si se encuentran
                    Dim detectedIncoterm As String = Nothing
                    For Each block As AmazonTextractModel.Block In queryResults.Blocks
                        If block.BlockType = "QUERY_RESULT" Then
                            'If block.Query.Text = queryText Then
                            detectedIncoterm = block.Text
                            Exit For
                            'End If
                        End If
                    Next

                    Dim FieldHeaderIncoterm_ As New FieldsHeader With {
                        .Label = "INCOTERM",
                        .Value = detectedIncoterm,
                        .confidence = 91
                    }

                    invoice_.Header.Add(FieldHeaderIncoterm_)

                End If

                Dim x = 0

            Next

        End If

        Dim json_ As String = JsonConvert.SerializeObject(invoice_, Formatting.Indented)

        Dim gpt_ As New ControllerChatGPT(_temperature, IControllerChatGPT.DocumentoCargado.FacturaImportacion)

        Using s3Client As New AmazonS3Client(_accessKey, _secretKey, _bucketRegion)

            Dim deleteObjectRequest_ As New DeleteObjectRequest() With {
            .BucketName = _bucketName,
            .Key = key_
        }

            Dim deleteResponse_ As DeleteObjectResponse = Await s3Client.DeleteObjectAsync(deleteObjectRequest_)

        End Using

        If SecretKeyGPT IsNot Nothing Then

            gpt_.ApiKey = SecretKeyGPT

        End If

        If DirectivesGPT IsNot Nothing Then

            gpt_.AddDirectives(DirectivesGPT)

        End If

        'Dim filePath As String = "C:\zamora\resultado_textractPeque.json"
        'File.WriteAllText(filePath, json_)

        If prompt_ Is Nothing Then

            Return gpt_.ProcessData(Of T)(BsonDocument.Parse(json_))

        Else

            Return gpt_.ProcessData(Of T)(BsonDocument.Parse(json_), prompt_)

        End If

    End Function

    Function setKeys(secretKey_ As String, accesKey_ As String) _
        Implements IControllerTextract.setKeys

        _secretKey = secretKey_

        _accessKey = accesKey_

    End Function


End Class

Class Invoice

    Property Header As List(Of FieldsHeader)

    Property Details As List(Of Items)

End Class

Class FieldsHeader

    Property Label As String

    Property Value As String

    Property confidence As Double

End Class

Class Items

    Property Fields As List(Of Field)

End Class

Class Field

    Property Label As String

    Property Value As String

    Property confidence As Double

End Class