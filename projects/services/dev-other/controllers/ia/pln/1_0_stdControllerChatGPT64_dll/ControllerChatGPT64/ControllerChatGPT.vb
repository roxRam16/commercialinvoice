Imports Wma.Exceptions
Imports MongoDB.Bson
Imports System.Net
Imports Newtonsoft.Json
Imports RestSharp
Imports Newtonsoft.Json.Linq
Imports System.Threading
Imports Ia.Analysis
Imports System.Text.Json.Nodes

Public Class ControllerChatGPT
    Implements IControllerChatGPT



    Private _prompt As String

    Private _response_format As Object

    Private _directives As String

    Private _client As RestClient

    Private _temperature As Double

    Private _maxTokens As Int32

    Private _messages As Object

    Public Property ApiKey As String Implements IControllerChatGPT.ApiKey

    Public Property Status As TagWatcher Implements IControllerChatGPT.Status

    Sub New(temperaturaAnalysis_ As Int32, Optional documentoCargado_ As IControllerChatGPT.DocumentoCargado = IControllerChatGPT.DocumentoCargado.BL)

        SetGptParameters(temperaturaAnalysis_, documentoCargado_)

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        ' Desactivar la validación de certificados SSL (solo para desarrollo)
        ServicePointManager.ServerCertificateValidationCallback =
            Function(sender, certificate, chain, sslPolicyErrors) True

        ' Base URL for the ChatGPT API
        _client = New RestClient("https://api.openai.com/v1/")

    End Sub

    Public Async Function DocumentAnalyzer(Of T)(document_ As List(Of Byte())) As Task(Of TagWatcher) Implements IControllerChatGPT.DocumentAnalyzer

        Dim imageMessages_ As New List(Of List(Of Object))()

        Dim request_ As TagWatcher

        For Each doc_ In document_
            Dim image_ = New List(Of Object)
            imageMessages_.Add(image_)
            image_.Add(New With {
                .type = "image_url",
                .image_url = New With {
                    .url = "data:image/jpeg;base64," & Convert.ToBase64String(doc_),
                    .detail = "high"
                }
            })
        Next

        _messages = New List(Of Object) From {
            New With {
                .role = "system",
                .content = _directives & " Asegúrate de identificar todos los ítems de cada imagen sin omitir ninguno, incluso si el formato varía ligeramente entre páginas."
            }
        }

        Dim count_ = 1

        For Each imageMessage_ In imageMessages_

            _messages.Add(New With {
                .role = "user",
                .content = imageMessage_
            })

            _messages.add(New With {
                .role = "user",
                .content = _prompt & "En la imagen, analiza todos los ítems y devuélvelos en la estructura final en la estructura especificada, sin omitir ningún ítem de la lista. el campo temperature manejalo como entero "
            })

            Dim body_ = New With {
                .model = "gpt-4o",
                .messages = _messages,
                .temperature = _temperature,
                .response_format = _response_format
            }

            request_ = ExecuteRequestConversacional(Of T)(body_, imageMessages_.Count, count_)

            count_ += 1

        Next

        Return request_

    End Function

    Public Function ProcessData(Of T)(documentJson As BsonDocument) As TagWatcher Implements IControllerChatGPT.ProcessData

        Dim json_ = documentJson.ToJson()

        Dim parsed_ = JObject.Parse(json_)

        Dim details_ = parsed_("Details")

        Dim header_ = parsed_("Header")

        Dim promt2_ = "Vas a recibir un json sacado de aws textract, es necesario que lo analices por favor y respondas con la estructura que te dare"

        Dim request_ As TagWatcher = Nothing

        If details_ IsNot Nothing AndAlso details_.Count > 20 Then

            Dim blocks_ As New List(Of JArray)

            For i As Integer = 0 To details_.Count - 1 Step 20

                blocks_.Add(JArray.FromObject(details_.Skip(i).Take(20)))

            Next

            Dim count_ = 1

            Dim jsonObjects_ As New List(Of JObject)

            For Each block_ In blocks_

                Dim newJson_ = New JObject()

                If count_ = 1 Then

                    newJson_("Header") = header_

                End If

                newJson_("Details") = block_

                jsonObjects_.Add(newJson_)

                count_ += 1

            Next

            count_ = 1

            For Each jsonObject_ In jsonObjects_

                _messages = New List(Of Object) From {
                    New With {
                        .role = "system",
                        .content = _directives & " No juntes los items si dos o más fields son iguales necesito que no los juntes, si recibes 20 items salen 20 items"
                    },
                    New With {
                        .role = "user",
                        .content = jsonObject_.ToString()
                    },
                    New With {
                        .role = "user",
                        .content = _prompt
                    }
                }

                Dim body_ = New With {
                .model = "gpt-4o-mini",
                .messages = _messages,
                .max_tokens = 16000,
                .temperature = _temperature,
                .response_format = _response_format
            }

                request_ = ExecuteRequestConversacional(Of T)(body_, jsonObjects_.Count, count_)

                count_ += 1

            Next

            Return request_
        Else

            Dim body_ = New With {
            .model = "gpt-4o-mini",
            .messages = New Object() {
                New With {
                    .role = "system",
                    .content = _directives
                },
                New With {
                    .role = "user",
                    .content = promt2_
                },
                New With {
                    .role = "user",
                    .content = json_
                },
                New With {
                    .role = "user",
                    .content = _prompt
                }
            },
            .max_tokens = 16000,
            .temperature = _temperature,
            .response_format = _response_format
        }

            Return ExecuteRequest(Of T)(body_)

        End If

    End Function

    Public Async Function DocumentAnalyzer(Of T)(document_ As List(Of Byte()), prompt_ As String) As Task(Of TagWatcher) Implements IControllerChatGPT.DocumentAnalyzer

        Dim imageMessages_ As New List(Of List(Of Object))()

        Dim request_ As TagWatcher

        _prompt = prompt_

        For Each doc_ In document_
            Dim image_ = New List(Of Object)
            imageMessages_.Add(image_)
            image_.Add(New With {
                .type = "image_url",
                .image_url = New With {
                    .url = "data:image/jpeg;base64," & Convert.ToBase64String(doc_),
                    .detail = "high"
                }
            })
        Next

        _messages = New List(Of Object) From {
            New With {
                .role = "system",
                .content = _directives & " Asegúrate de identificar todos los ítems de cada imagen sin omitir ninguno, incluso si el formato varía ligeramente entre páginas."
            }
        }

        Dim count_ = 1

        For Each imageMessage_ In imageMessages_

            _messages.Add(New With {
                .role = "user",
                .content = imageMessage_
            })

            _messages.add(New With {
                .role = "user",
                .content = _prompt
            })

            Dim body_ = New With {
                .model = "gpt-4o",
                .messages = _messages,
                .temperature = _temperature,
                .max_tokens = _maxTokens
            }

            request_ = ExecuteRequestConversacional(Of T)(body_, imageMessages_.Count, count_)

            count_ += 1

        Next

        Return request_

    End Function

    Public Function ProcessData(Of T)(documentJson As BsonDocument, prompt_ As String) As TagWatcher Implements IControllerChatGPT.ProcessData

        _prompt = prompt_

        Dim json_ = documentJson.ToJson()

        Dim parsed_ = JObject.Parse(json_)

        Dim details_ = parsed_("Details")

        Dim header_ = parsed_("Header")

        Dim promt2_ = "Vas a recibir un json sacado de aws textract, es necesario que lo analices por favor y respondas con la estructura que te dare"

        Dim request_ As TagWatcher = Nothing

        If details_ IsNot Nothing AndAlso details_.Count > 20 Then

            Dim blocks_ As New List(Of JArray)

            For i As Integer = 0 To details_.Count - 1 Step 20

                blocks_.Add(JArray.FromObject(details_.Skip(i).Take(20)))

            Next

            Dim count_ = 1

            Dim jsonObjects_ As New List(Of JObject)

            For Each block_ In blocks_

                Dim newJson_ = New JObject()

                If count_ = 1 Then

                    newJson_("Header") = header_

                End If

                newJson_("Details") = block_

                jsonObjects_.Add(newJson_)

                count_ += 1

            Next

            count_ = 1

            For Each jsonObject_ In jsonObjects_

                _messages = New List(Of Object) From {
                    New With {
                        .role = "system",
                        .content = _directives & " No juntes los items si dos o más fields son iguales necesito que no los juntes, si recibes 20 items salen 20 items"
                    },
                    New With {
                        .role = "user",
                        .content = jsonObject_.ToString()
                    },
                    New With {
                        .role = "user",
                        .content = _prompt
                    }
                }

                Dim body_ = New With {
                .model = "gpt-4o-mini",
                .messages = _messages,
                .max_tokens = 16000,
                .temperature = _temperature
            }

                request_ = ExecuteRequestConversacional(Of T)(body_, jsonObjects_.Count, count_)

                count_ += 1

            Next

            Return request_
        Else

            Dim body_ = New With {
            .model = "gpt-4o-mini",
            .messages = New Object() {
                New With {
                    .role = "system",
                    .content = _directives
                },
                New With {
                    .role = "user",
                    .content = promt2_
                },
                New With {
                    .role = "user",
                    .content = json_
                },
                New With {
                    .role = "user",
                    .content = _prompt
                }
            },
            .max_tokens = 16000,
            .temperature = _temperature
        }

            Return ExecuteRequest(Of T)(body_)

        End If

    End Function

    Public Function AskToChatGPT(message As String) As String _
        Implements IControllerChatGPT.AskToChatGPT

        Dim request_ = New RestRequest("chat/completions", Method.Post)

        request_.AddHeader("Authorization", "Bearer " & _ApiKey)

        request_.AddHeader("Content-Type", "application/json")

        request_.AddJsonBody(New With {
            .model = "gpt-4o",
            .temperature = _temperature,
            .max_tokens = _maxTokens,
            .messages = New Object() {
                New With {.role = "system", .content = _prompt},
                New With {.role = "user", .content = message}
            }
        })

        ' Enviar la solicitud y esperar la respuesta
        Dim response_ = _client.Execute(request_)

        If response_.IsSuccessful Then

            Return response_.Content

        Else

            Return "Error: " & response_.StatusCode.ToString() & " - " & response_.ErrorMessage

        End If

    End Function

    Public Function GetResponse(operationNumber_ As ObjectId) As TagWatcher Implements IControllerChatGPT.GetResponse

        Throw New NotImplementedException()

    End Function

    Private Function ExecuteRequest(Of T)(body_ As Object) As TagWatcher

        Dim request_ = New RestRequest("chat/completions", Method.Post)

        request_.AddHeader("Authorization", "Bearer " & _ApiKey)

        request_.AddHeader("Content-Type", "application/json")

        request_.AddJsonBody(JsonConvert.SerializeObject(body_))

        Dim response_ = _client.Execute(request_)

        If response_.IsSuccessful Then

            Dim jsonResponse_ As JObject = JObject.Parse(response_.Content)

            Dim rawContent_ As String = jsonResponse_("choices")(0)("message")("content").ToString()

            rawContent_ = rawContent_.Replace("```json", "").Replace("```", "").Trim()

            If rawContent_.Count > 150 Then

                _Status.ObjectReturned = JsonConvert.DeserializeObject(Of T)(rawContent_)

                _Status.SetOK()

            Else

                _Status.ErrorDescription = "Gpt: " & rawContent_

                _Status.ObjectReturned = "Gpt: " & rawContent_

                _Status.SetError()


            End If

            Return _Status

        Else

            If response_.StatusCode = 429 Then

                Thread.Sleep(3000)

                _Status = ExecuteRequest(Of T)(body_)

            Else

                _Status.ErrorDescription = "Error de gpt: " & response_.StatusCode.ToString() & " - " & response_.ErrorMessage

                _Status.ObjectReturned = "Error de gpt: " & response_.StatusCode.ToString() & " - " & response_.ErrorMessage

                _Status.SetError()

            End If



            Return _Status

        End If

        Return Status

    End Function

    Private Function ExecuteRequestConversacional(Of T)(body_ As Object, pages_ As Int16, page_ As Int16) As TagWatcher

        Dim request_ = New RestRequest("chat/completions", Method.Post)

        request_.AddHeader("Authorization", "Bearer " & _ApiKey)

        request_.AddHeader("Content-Type", "application/json")

        request_.AddJsonBody(JsonConvert.SerializeObject(body_))

        Dim response_ = _client.Execute(request_)

        If response_.IsSuccessful Then

            Dim jsonResponse_ As JObject = JObject.Parse(response_.Content)

            Dim rawContent_ As String = jsonResponse_("choices")(0)("message")("content").ToString()

            If pages_ = page_ Then

                rawContent_ = rawContent_.Replace("```json", "").Replace("```", "").Trim()

                If rawContent_.Count > 150 Then

                    If page_ > 1 Then

                        Dim result_ As Object = _Status.ObjectReturned

                        Dim jObject_ As JObject = ConvertToJObject(result_)

                        Dim newStruct_ = JObject.Parse(rawContent_)

                        result_ = Combine_structures(jObject_, newStruct_)

                        result_.Remove("_id")

                        rawContent_ = result_.ToString

                    End If
                    _Status.ObjectReturned = JsonConvert.DeserializeObject(Of T)(rawContent_)

                    _Status.SetOK()

                Else

                    _Status.ErrorDescription = "Gpt: " & rawContent_

                    _Status.ObjectReturned = "Gpt: " & rawContent_

                    _Status.SetError()


                End If

                Return _Status

            Else

                _messages.RemoveAt(1)

                _messages.RemoveAt(1)

                rawContent_ = rawContent_.Replace("```json", "").Replace("```", "").Trim()

                If page_ > 1 Then

                    Dim result_ As Object = _Status.ObjectReturned

                    Dim jObject_ As JObject = ConvertToJObject(result_)

                    Dim newStruct_ = JObject.Parse(rawContent_)

                    result_ = Combine_structures(jObject_, newStruct_)

                    result_.Remove("_id")

                    rawContent_ = result_.ToString

                End If

                _Status.ObjectReturned = JsonConvert.DeserializeObject(Of T)(rawContent_)

                _Status.SetOK()

            End If

        Else

            If response_.StatusCode = 429 Then

                Thread.Sleep(3000)

                _Status = ExecuteRequestConversacional(Of T)(body_, pages_, page_)

            Else

                _Status.ErrorDescription = "Error de gpt: " & response_.StatusCode.ToString() & " - " & response_.ErrorMessage

                _Status.ObjectReturned = "Error de gpt: " & response_.StatusCode.ToString() & " - " & response_.ErrorMessage

                _Status.SetError()

            End If



            Return _Status

        End If

        Return Status

    End Function

    Private Function Combine_structures(initialStructure As JObject, newStructure As JObject) As JObject
        ' Manejo de "items" por separado
        Dim initialItems As JArray = If(initialStructure("items"), New JArray())
        Dim newItems As JArray = If(newStructure("items"), New JArray())

        ' Encontrar el máximo ID existente
        Dim maxSec_ As Integer = initialItems.Select(Function(item) CType(item("sec"), Integer)).DefaultIfEmpty(0).Max()

        ' Añadir los nuevos elementos reasignándoles IDs consecutivos
        For Each newItem In newItems
            maxSec_ += 1
            Dim clonedItem As JObject = DirectCast(newItem.DeepClone(), JObject)
            clonedItem("sec") = maxSec_
            initialItems.Add(clonedItem)
        Next

        ' Reasignar los "items" actualizados a la estructura inicial
        initialStructure("items") = initialItems

        ' Combinar propiedades genéricas
        For Each propertyInNewStructure In newStructure.Properties()
            Dim propertyName = propertyInNewStructure.Name

            ' Ignorar "items" porque ya fue manejado
            If propertyName = "items" Then Continue For

            Dim propertyValue = propertyInNewStructure.Value

            If initialStructure.ContainsKey(propertyName) Then
                ' Si ya existe en la estructura inicial, no sobrescribir
                Continue For
            Else
                ' Si no existe, añadir la propiedad nueva
                initialStructure(propertyName) = propertyValue
            End If
        Next

        ' Devolver la estructura combinada
        Return initialStructure
    End Function

    Private Function ConvertToJObject(Of T)(obj As T) As JObject
        ' Si el objeto es un valor simple (JValue), lo envuelves dentro de un JObject
        If TypeOf obj Is JValue Then
            Return New JObject(New JProperty("value", obj))
        End If

        ' Si no es un JValue, se convierte normalmente a JObject
        Return JObject.FromObject(obj)
    End Function
    Public Overridable Sub SetGptParameters(temperaturaAnalysis_ As Int32,
                                            Optional documentoCargado_ As IControllerChatGPT.DocumentoCargado = IControllerChatGPT.DocumentoCargado.BL) _
        Implements IControllerChatGPT.SetGptParameters

        'ApiKey = "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" '"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"



        '"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"

        '"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"

        '"sk-2BuvAhq2pG36PHrYTjABQdCTO8GyAfKrEZnuoJbV7FT3BlbkFJ0lSjf7E3GdqDGkvuuUIiikTHF45Y7Iz6ZTocsOeEoA"

        'Dim RKU_ = "sk-jd67Gq9geFyQzxZN2KnJCw_viWyxGI1VzfGoCycSqPT3BlbkFJqIkMFgJV7psc9D3q8JEAu4xDWZm9WSUrMSLH7WiiwA"
        'Dim SAP_ = "sk-ykzWKMBF-MHlJyT0VrUn8F8tjIDPgkYcEiQKWdTW8iT3BlbkFJ2xzt-GkJsCwpBVHbkDU2X4PhObE-jc8LCwNBpJY5EA"
        'Dim CEG_ = "sk-b34K1q0dlBVhwfKf3UWmlVnuHUODF0S4s6Ipk9SWdMT3BlbkFJlY06FUFD550Fv_-hHcUnL5ane4V7t0itmProuBvvMA"
        'Dim DAI_ = "sk-BT226E_uVsBNdfpTr70tlGRYWGf5AzTFj_5-7TSUJRT3BlbkFJWBX2WMH8j8enl_xWhnLKyVmU3zoLQX6UgdskQ6oBsA"
        'Dim TOL_ = "sk-UDGaVKKIaqHD_iQ4mJ90ugwss7O8HEdwEEnQ_38puST3BlbkFJ89FTgROobVP11qL3uQsr8Yzt4MZBqh4ZpQAGX2OVgA"
        'Dim ALC_ = "sk--NACHgQ_djFX94sWK8liOxJ5eHwJpl2WzddlgXlMAuT3BlbkFJMm57NeWQx89Hf2mdc80X3YSB9lJ2PQt2BkrQ7eWnAA"
        'Dim SFI_ = "sk-11x4n5VPfXnLYmWIK7G7lO0XNLXXuTCucDbFBlN3VwT3BlbkFJDWEmf4vMNcrjLS0v5D2L8HbnEPZVunU4TRwk-UiugA"
        'Dim SFC_ = "sk-ak69QDV7EA0chkq3_4J2L4JZhibs5ISEidMVQzdRQ6T3BlbkFJTjb7Td3llwNw89VLbtUlweld_ofh0BMPIKR4CmHoMA"
        'Dim AIFA_ = "sk-VL0sFVC7NoYqq0SVqt-ReuwKgq5U8U7D1QR0wh6OmDT3BlbkFJipD_QZHGx7dFXEsWwFlMTb7g4-x3XDghLlgNPQoioA"



        Select Case documentoCargado_
            Case IControllerChatGPT.DocumentoCargado.BL
                _prompt = "Actúa como un experto en comercio exterior mexicano. Analiza el conocimiento de embarque (BL) proporcionado y responde en formato JSON puro sin incluir caracteres de nueva línea (\n) ni bloques de código como ```json, solo analiza las imágenes enviadas junto a este prompt. Responde solo con el contenido en la siguiente estructura específica:" &
                            "{" &
                            "    _operacion: {" &
                            "        _materialPeligroso:""debe decir true si es si o false si es no""" &
                            "    }," &
                            "    _importacion: {" &
                            "        _guia :{" &
                            "            _listaGuias : [{" &
                            "                _guia : ""numero de guia, pon especial atención aquí de no cambiar ni saltar caracteres, repito pon especial atención aquí por favor""," &
                            "                _transportista: ""nombre del transportista""," &
                            "                _tipoCarga : ""el tipo de la carga""," &
                            "                _pais: ""pais de origen""," &
                            "                perBruto : ""peso bruto, el peso total, solo numero sin separacion de miles y respetar el punto decimal""," &
                            "                _unidadMedida : ""unidade de medida""," &
                            "                _salidaOrigen: ""Fecha Salida de origen o de abordaje y similares, en formato dd/mm/yyyy, si no la encuentras omite este nodo""," &
                            "                _descripcionMercancia: ""Descripcion de la mercancia""," &
                            "                _consignatario : ""poner la razon social o nombre del consignatario, solo el consignatario o consignee, no el nombre del buque""" &
                            "            }]" &
                            "        }" &
                            "    }" &
                            "    _confiabilidad: ""0-100%""," &
                            "}"

            Case IControllerChatGPT.DocumentoCargado.FacturaImportacion

                _directives = "Actúa como un experto en comercio exterior mexicano. Analiza la factura comercial de importación proporcionada y responde exclusivamente en formato JSON puro, sin incluir caracteres de nueva línea (\n) ni bloques de código como ```json. Analiza TODAS las imágenes o archivos enviados junto a este mensaje. Responde estrictamente en la estructura específica proporcionada." &
                            "**Es extremadamente importante que:**
                                1. Proceses TODOS los ítems de la factura de forma individual. Cada fila en la tabla de ítems corresponde a un ítem único. No combines, agrandes, ni excluyas ítems duplicados. Si la factura tiene 107 ítems, tu respuesta debe contener exactamente 107 ítems.
                                2. Extrae los valores con precisión de las columnas correspondientes:
                                   - `quantity`: toma el valor exacto de la columna de cantidades.
                                   - `unitprice`: extrae el precio unitario.
                                   - `value`: toma el valor total del ítem (por fila).
                                3. Si algún campo no está presente:
                                   - Para valores numéricos, coloca `0`.
                                   - Para valores tipo string, coloca `null` (sin comillas).
                                4. Todos los ítems deben mantenerse separados, incluso si tienen descripciones o detalles similares. Cada fila es un registro independiente, sin combinaciones.
                                5. Asegúrate de que los países estén en formato ISO Alpha-3 (tres letras). Si no puedes determinar el país, coloca `null` (sin comillas).
                                6. Los campos `invoicecurrency` y `countrycurrency` son obligatorios y deben estar presentes:
                                   - `invoicecurrency` debe corresponder a la moneda indicada en la factura.
                                   - Si no se menciona explícitamente un país para `countrycurrency`, dedúcelo lógicamente del campo `invoicecurrency`.
                                7. El campo `incoterm` debe ser cuidadosamente identificado. Si no está presente, coloca `null` (sin comillas).
                                8. Verifica especialmente los campos `partnumber` y `description`:
                                   - Excluye fracciones arancelarias o cualquier formato irrelevante (`####.##.##.##` o `######.##.##`)."


                _response_format = New With {
                    .type = "json_schema",
                    .json_schema = New With {
                        .name = "InvoiceAnalysisSchema",
                        .strict = True,
                        .schema = New With {
                            .type = "object",
                            .properties = New With {
                                .processdate = New With {.type = "string"},
                                .confidence = New With {.type = "number"},
                                .invoicenumber = New With {.type = "string"},
                                .invoicedate = New With {.type = "string"},
                                .invoiceseries = New With {.type = "string"},
                                .customername = New With {.type = "string"},
                                .suppliername = New With {.type = "string"},
                                .invoicecountry = New With {.type = "string"},
                                .totalinvoice = New With {.type = "number"},
                                .invoicecurrency = New With {.type = "string"},
                                .countrycurrency = New With {.type = "string"},
                                .info = New With {.type = "string"},
                                .analysis = New With {
                                    .type = "object",
                                    .additionalProperties = False,
                                    .required = New String() {"confidence", "gptanalysis", "gpttokensupload", "gpttokensdownload", "textractanalysis", "textractpages", "quantitydifferences", "temperature", "messages"},
                                    .properties = New With {
                                        .confidence = New With {.type = "number"},
                                        .gptanalysis = New With {.type = "boolean"},
                                        .gpttokensupload = New With {.type = "number"},
                                        .gpttokensdownload = New With {.type = "number"},
                                        .textractanalysis = New With {.type = "boolean"},
                                        .textractpages = New With {.type = "number"},
                                        .quantitydifferences = New With {.type = "number"},
                                        .temperature = New With {.type = "number"},
                                        .messages = New With {
                                            .type = "array",
                                            .items = New With {
                                                .type = "object",
                                                .additionalProperties = False,
                                                .required = New String() {"id", "type", "value", "action", "object", "field", "message", "confidence", "source"},
                                                .properties = New With {
                                                    .id = New With {.type = "string"},
                                                    .type = New With {.type = "string"},
                                                    .action = New With {.type = "string"},
                                                    .object = New With {.type = "string"},
                                                    .field = New With {.type = "string"},
                                                    .value = New With {.type = "string"},
                                                    .message = New With {.type = "string"},
                                                    .confidence = New With {.type = "number"},
                                                    .source = New With {.type = "string"}
                                                }
                                            }
                                        }
                                    }
                                },
                                .customer = New With {
                                    .type = "object",
                                    .required = New String() {"customername", "rfc", "address", "street", "externalnumber", "internalnumber", "zipcode", "city", "locality", "state", "country"},
                                    .additionalProperties = False,
                                    .properties = New With {
                                        .customername = New With {.type = "string"},
                                        .rfc = New With {.type = "string"},
                                        .address = New With {.type = "string"},
                                        .street = New With {.type = "string"},
                                        .externalnumber = New With {.type = "string"},
                                        .internalnumber = New With {.type = "string"},
                                        .zipcode = New With {.type = "string"},
                                        .city = New With {.type = "string"},
                                        .locality = New With {.type = "string"},
                                        .state = New With {.type = "string"},
                                        .country = New With {.type = "string"}
                                    }
                                },
                                .supplier = New With {
                                    .type = "object",
                                    .required = New String() {"supliername", "taxid", "address", "street", "externalnumber", "internalnumber", "zipcode", "locality", "city", "state", "country"},
                                    .additionalProperties = False,
                                    .properties = New With {
                                        .supliername = New With {.type = "string"},
                                        .taxid = New With {.type = "string"},
                                        .address = New With {.type = "string"},
                                        .street = New With {.type = "string"},
                                        .externalnumber = New With {.type = "string"},
                                        .internalnumber = New With {.type = "string"},
                                        .zipcode = New With {.type = "string"},
                                        .locality = New With {.type = "string"},
                                        .city = New With {.type = "string"},
                                        .state = New With {.type = "string"},
                                        .country = New With {.type = "string"}
                                    }
                                },
                                .items = New With {
                                    .type = "array",
                                    .items = New With {
                                        .type = "object",
                                        .required = New String() {"sec", "sku", "partnumber", "quantity", "unit", "description", "total", "currency", "usdvalue", "value", "discount", "unitprice", "netweight", "purchaseorder", "destinationcountry", "origincountry"},
                                        .additionalProperties = False,
                                        .properties = New With {
                                            .sec = New With {.type = "number"},
                                            .sku = New With {.type = "string"},
                                            .partnumber = New With {.type = "string"},
                                            .quantity = New With {.type = "number"},
                                            .unit = New With {.type = "string"},
                                            .description = New With {.type = "string"},
                                            .total = New With {.type = "number"},
                                            .currency = New With {.type = "string"},
                                            .usdvalue = New With {.type = "number"},
                                            .value = New With {.type = "number"},
                                            .discount = New With {.type = "number"},
                                            .unitprice = New With {.type = "number"},
                                            .netweight = New With {.type = "number"},
                                            .purchaseorder = New With {.type = "string"},
                                            .destinationcountry = New With {.type = "string"},
                                            .origincountry = New With {.type = "string"}
                                        }
                                    }
                                },
                                .additionaldetails = New With {
                                    .type = "object",
                                    .required = New String() {"incoterm", "purchaseorder", "totalweight", "packages", "customerreference", "incrementalvalues"},
                                    .additionalProperties = False,
                                    .properties = New With {
                                        .purchaseorder = New With {.type = "string"},
                                        .totalweight = New With {.type = "number"},
                                        .packages = New With {.type = "number"},
                                        .incoterm = New With {.type = "string"},
                                        .customerreference = New With {.type = "string"},
                                        .incrementalvalues = New With {
                                            .type = "array",
                                            .items = New With {
                                                .type = "object",
                                                .additionalProperties = False,
                                                .required = New String() {"id", "Freight", "currency", "info"},
                                                .properties = New With {
                                                    .id = New With {.type = "string"},
                                                    .Freight = New With {.type = "number"},
                                                    .currency = New With {.type = "string"},
                                                    .info = New With {.type = "string"}
                                                }
                                            }
                                        }
                                    }
                                },
                                .consigneedetails = New With {
                                    .type = "object",
                                    .required = New String() {"consigneedetailsname", "taxid", "address", "street", "externalnumber", "internalnumber", "zipcode", "locality", "city", "state", "country"},
                                    .additionalProperties = False,
                                    .properties = New With {
                                        .consigneedetailsname = New With {.type = "string"},
                                        .taxid = New With {.type = "string"},
                                        .address = New With {.type = "string"},
                                        .street = New With {.type = "string"},
                                        .externalnumber = New With {.type = "string"},
                                        .internalnumber = New With {.type = "string"},
                                        .zipcode = New With {.type = "string"},
                                        .locality = New With {.type = "string"},
                                        .city = New With {.type = "string"},
                                        .state = New With {.type = "string"},
                                        .country = New With {.type = "string"}
                                    }
                                }
                            },
                            .required = New String() {"processdate", "confidence", "invoicenumber", "invoicedate", "invoiceseries", "customername", "suppliername", "invoicecountry", "totalinvoice", "invoicecurrency", "countrycurrency", "info", "analysis", "customer", "supplier", "items", "additionaldetails", "consigneedetails"},
                            .additionalProperties = False
                        }
                    }
                }

                _prompt =
                    "Recibiste una o más imagenes que representan una factura comercial o un json que igual representa una factura comercial, responde solo con el contenido en la siguiente estructura específica:" '&
                    '"    ""processdate"": ""Fecha en que se procesa (fecha actual)""," &
                    '"    ""confidence"": ""si evaluaste imagenes considera esto: nivel de confianza de la informacion capturada en numero del 0 al 100. Si evaluaste un JSON entonces pon el promedio de confidence de los campos usados""," &
                    '"    ""invoicenumber"": ""Número de factura""," &
                    '"    ""invoicedate"": ""Fecha de factura en formato YYYY/MM/DD, si no encuentras el valor, usa null sin comillas""," &
                    '"    ""invoiceseries"": ""Serie de la factura""," &
                    '"    ""customername"": ""Nombre del cliente""," &
                    '"    ""suppliername"": ""Nombre del proveedor""," &
                    '"    ""invoicecountry"": ""País de factura""," &
                    '"    ""totalinvoice"": ""Total de la factura (en tipo de dato double)""," &
                    '"    ""invoicecurrency"": ""Moneda""," &
                    '"    ""countrycurrency"": ""Devuélveme el país asociado a la moneda del nodo 'invoicecurrency'. Es muy importante que el código devuelto esté en formato ISO Alpha-3""," &
                    '"    ""info"": ""información general sobre la factura en un máximo de 12 palabras""," &
                    '"    ""analysis"": {" &
                    '"        ""confidence"": ""nivel de confianza de la informacion capturada en numero del 0 al 100""," &
                    '"        ""gptanalysis"": true," &
                    '"        ""gpttokensupload"": ""quiero lo que tienes en la respuesta en Content.usage.prompt_tokens (en entero)""," &
                    '"        ""gpttokensdownload"": "" quiero lo que tienes en la respuesta en Content.usage.completion_tokens (en entero)""," &
                    '"        ""textractanalysis"": si recibiste imagenes para analizar responde false, de lo contrario responde true," &
                    '"        ""textractpages"": 0," &
                    '"        ""quantitydifferences"": 0," &
                    '"        ""temperature"": la temperatura que se uso en tipo de dato entero," &
                    '"        ""messages"": ""(Aquí vas a poner los campos en los que tengas dudas siempre respetando este formato, tambien cuando recibas un json_, verifica el campo confidence de ese json_ y si el menos que " & temperaturaAnalysis_ & " debes incluir ese campo aquí como type warning, action : info y source textract )""[{" &
                    '"            ""id"": ""numero de mensaje incrementable""," &
                    '"            ""type"": ""warning""," &
                    '"            ""action"": ""info""," &
                    '"            ""object"": ""en caso que sea un item aqui agrega el nodo partida : numero del item""," &
                    '"            ""field"": ""nombre del campo que dudaste""," &
                    '"            ""value"": ""valor del campo""," &
                    '"            ""message"": ""tu explicacion del porque la duda""," &
                    '"            ""confidence"": 0," &
                    '"            ""source"": ""gpto""" &
                    '"        }]" &
                    '"    }," &
                    '"    ""customer"": {" &
                    '"        ""customername"": ""Nombre del cliente""," &
                    '"        ""rfc"": ""rfc del cliente""," &
                    '"        ""address"": ""direccion del cliente""," &
                    '"        ""street"": ""calle""," &
                    '"        ""externalnumber"": ""numero externo""," &
                    '"        ""internalnumber"": ""numero interno""," &
                    '"        ""zipcode"": ""codigo postal cliente""," &
                    '"        ""city"": ""ciudad cliente""," &
                    '"        ""locality"": ""localidad cliente""," &
                    '"        ""state"": ""estado cliente""," &
                    '"        ""country"": ""pais cliente""" &
                    '"    }," &
                    '"    ""supplier"": {" &
                    '"        ""supliername"": ""nombre proveedor""," &
                    '"        ""taxid"": ""tax id proveedor""," &
                    '"        ""address"": ""direccion proveedor""," &
                    '"        ""street"": ""calle proveedor""," &
                    '"        ""externalnumber"": ""numero externo proveedor""," &
                    '"        ""internalnumber"": ""numero interno proveedor""," &
                    '"        ""zipcode"": ""codigo postal proveedor""," &
                    '"        ""locality"": ""localidad proveedor""," &
                    '"        ""city"": ""ciudad proveedor""," &
                    '"        ""state"": ""estado proveedor""," &
                    '"        ""country"": ""pais proveedor""" &
                    '"    }," &
                    '"    ""items"": [{" &
                    '"        ""sec"": ""secuencia del item(tipo de dato: entero)""," &
                    '"        ""sku"": ""sku""," &
                    '"        ""partnumber"": ""numero de parte""," &
                    '"        ""quantity"": ""cantidad (tipo de dato: entero)""," &
                    '"        ""unit"": ""unidad medida representada con la abreviatura del Sistema Internacional de Unidades""," &
                    '"        ""description"": ""descripcion""," &
                    '"        ""total"": ""Total (tipo dato: real)""," &
                    '"        ""currency"": ""moneda""," &
                    '"        ""usdvalue"": ""Valor en dolares (tipo dato: real) si no lo encuentras pon 0""," &
                    '"        ""value"": ""valor (tipo dato: real) si no lo encuentras pon 0""," &
                    '"        ""discount"": ""descuento (tipo dato: real) si no lo encuentras pon 0""," &
                    '"        ""unitprice"": ""Precio unitario (tipo dato: real) si no lo encuentras pon 0""," &
                    '"        ""netweight"": ""peso neto (tipo dato: real) si no lo encuentras pon 0""," &
                    '"        ""purchaseorder"": ""Orden de compra específico del item""," &
                    '"        ""destinationcountry"": ""pais destino en iso alpha 3 code""," &
                    '"        ""origincountry"": ""pais origen de item en iso alpha 3 code""" &
                    '"    }]," &
                    '"    ""additionaldetails"": {" &
                    '"        ""purchaseorder"": ""Orden de compra general de la factura""," &
                    '"        ""totalweight"": ""peso total de la factura (tipo dato: real) si no lo encuentras pon 0""," &
                    '"        ""packages"": ""numero de paquetes o bultos (tipo dato: entero) si no lo encuentras pon 0""," &
                    '"        ""incoterm"": ""incoterm, 3 caracteres si no lo encuentras el campo es nulo""," &
                    '"        ""customerreference"": ""referencia del cliente""," &
                    '"        ""incrementalvalues"": ""Aquí necesito que busques los incrementables, si no encuentras no pongas este campo""[{" &
                    '"            ""id"": ""consecutivo""," &
                    '"            ""Freight"": ""monto""," &
                    '"            ""currency"": ""moneda""," &
                    '"            ""info"": ""informacion""" &
                    '"        }]" &
                    '"    }," &
                    '"    ""consigneedetails"": {" &
                    '"        ""consigneedetailsname"": ""nombre del consignatario""," &
                    '"        ""taxid"": ""tax id/ rfc del consignatario""," &
                    '"        ""address"": ""direccion del consignatario""," &
                    '"        ""street"": ""calle del consignatario""," &
                    '"        ""externalnumber"": ""numero externo del consignatario""," &
                    '"        ""internalnumber"": ""numero interno del consignatario""," &
                    '"        ""zipcode"": ""codigo postal del consignatario""," &
                    '"        ""locality"": ""localidad del consignatario""," &
                    '"        ""city"": ""ciudad del consignatario""," &
                    '"        ""state"": ""estado del consignatario""," &
                    '"        ""country"": ""pais del consignatario""" &
                    '"    }" &
                    '"}"


            Case IControllerChatGPT.DocumentoCargado.FacturaExportacion
                _prompt = "Actúa como un experto en comercio exterior mexicano. Analiza la factura comercial de importación proporcionada y responde en formato JSON puro sin incluir caracteres de nueva línea (\n) ni bloques de código como ```json, solo analiza las imágenes enviadas junto a este prompt. Responde solo con el contenido en la siguiente estructura específica:" &
                            "{" &
                            "    generales: {" &
                            "        numeroFactura:""numero o folio de la factura""," &
                            "        fechaFactura:""fecha de emision de la factura, en formato dd/mm/yyyy, si no la encuentras omite este nodo""," &
                            "        razonSocial:""razon social o nombre del cliente""," &
                            "        taxIdCliente:""Tax ID o rfc del cliente""," &
                            "        domicilio:""domicilio del cliente""," &
                            "        claveIncoterm:""clave del incoterm""," &
                            "        valorFactura:""valor total de la factura""," &
                            "        pesoTotal:""peso total de la factura""" &
                            "    }," &
                            "    proveedor: {" &
                            "        razonSocial:""razon social o nombre del proveedor""," &
                            "        rfcCliente:""Tax ID del proveedor o emisor de la factura""," &
                            "        domicilio:""domicilio del proveedor""," &
                            "        pais:""pais del proveedor""" &
                            "    }," &
                            "    partidas:  [{ aqui se tiene una lista de las partidas de la factura con los siguientes datos" &
                            "        numeroPartida : ""numero de la partida""," &
                            "        numeroParte: ""numero de parte""," &
                            "        valorPartida : ""valor de la partida""," &
                            "        moneda: ""tipo de moneda de la partida""," &
                            "        pesoNeto : ""peso neto de la partida""," &
                            "        precioUnitario : ""precio unitario en la partida""," &
                            "        cantidad: ""cantidad en la partida""," &
                            "        unidadMedida: ""unidade de medida""," &
                            "        descripcion : ""descripcion de la partida""" &
                            "    }]" &
                            "    _confiabilidad: ""0-100%""," &
                            "    _Observaciones: ""Poner todos los campos que creas requieren una revisión visual""," &
                            "}"

        End Select

        _Status = New TagWatcher

        _temperature = 0.2

        _maxTokens = 15000

    End Sub

    Public Sub AddDirectives(newDirectives As String) _
        Implements IControllerChatGPT.AddDirectives

        _directives += " " & newDirectives

    End Sub

End Class

