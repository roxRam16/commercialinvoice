Imports Wma.Exceptions
Imports Ia.Analysis
Imports Newtonsoft.Json
Imports MongoDB.Bson
Imports Newtonsoft.Json.Linq

Public Class DocumentResultEvaluator

    Public Property Status As TagWatcher

    Sub New()

        Inicializa()

    End Sub

    Sub Inicializa()

        Status = New TagWatcher

    End Sub

    Public Function CompareResults(ByVal documentTextract_ As Object, ByVal documentGpt_ As Object) As TagWatcher

        Dim messages_ = Nothing

        Dim countAlert_ = 0

        If documentTextract_.analysis.messages IsNot Nothing Then

            messages_ = documentTextract_.analysis.messages

        Else

            documentTextract_.analysis.messages = New List(Of Messages)

            messages_ = documentTextract_.analysis.messages

        End If

        Dim count_ As Int16 = documentTextract_.analysis.messages.Count + 1

        For Each message_ In documentGpt_.analysis.messages

            message_.id = count_

            documentTextract_.analysis.messages.Add(message_)

            count_ += 1

        Next
        '##############################################################################################################
        If documentTextract_.invoicenumber IsNot Nothing And documentTextract_.invoicenumber <> "null" And documentTextract_.invoicenumber <> "" And documentTextract_.invoicenumber <> "NULL" Then

            If documentGpt_.invoicenumber IsNot Nothing And documentGpt_.invoicenumber <> "null" Then

                If documentTextract_.invoicenumber <> documentGpt_.invoicenumber Then

                    Dim message_ As New Messages With {
                        .id = messages_.Count + 1,
                        .type = "alert",
                        .action = "review",
                        .confidence = 0,
                        .source = "synapsis",
                        .field = "invoicenumber",
                        .value = documentGpt_.invoicenumber,
                        .message = "Validar no hubo coincidencia"
                    }

                    messages_.Add(message_)

                    countAlert_ += 1

                End If

            End If

        Else
            If documentGpt_.invoicenumber IsNot Nothing And documentGpt_.invoicenumber <> "null" Then

                documentTextract_.invoicenumber = documentGpt_.invoicenumber

                Dim message_ As New Messages With {
                    .id = messages_.Count + 1,
                    .type = "alert",
                    .action = "review",
                    .confidence = 0,
                    .source = "synapsis",
                    .field = "invoicenumber",
                    .value = documentGpt_.invoicenumber,
                    .message = "Validar textract no dio resultados"
                }

                'messages_.Add(message_)

            End If

        End If

        '##############################################################################################################
        If documentTextract_.invoicedate <> Nothing Then

            If documentGpt_.invoicedate <> Nothing Then

                If documentTextract_.invoicedate <> documentGpt_.invoicedate Then

                    Dim message_ As New Messages With {
                        .id = messages_.Count + 1,
                        .type = "alert",
                        .action = "review",
                        .confidence = 0,
                        .source = "synapsis",
                        .field = "invoicedate",
                        .value = documentGpt_.invoicedate,
                        .message = "Validar no hubo coincidencia"
                    }

                    messages_.Add(message_)

                    countAlert_ += 1

                End If

            End If

        Else
            If documentGpt_.invoicedate <> Nothing Then

                documentTextract_.invoicedate = documentGpt_.invoicedate

                Dim message_ As New Messages With {
                    .id = messages_.Count + 1,
                    .type = "alert",
                    .action = "review",
                    .confidence = 0,
                    .source = "synapsis",
                    .field = "invoicenumber",
                    .value = documentGpt_.invoicedate,
                    .message = "Validar textract no dio resultados"
                }

                'messages_.Add(message_)

            End If

        End If

        '##############################################################################################################
        If documentTextract_.invoicecurrency IsNot Nothing And documentTextract_.invoicecurrency <> "null" And documentTextract_.invoicecurrency <> "NULL" And documentTextract_.invoicecurrency <> "" Then

            If documentGpt_.invoicecurrency IsNot Nothing And documentGpt_.invoicecurrency <> "null" Then

                If documentTextract_.invoicecurrency <> documentGpt_.invoicecurrency Then

                    Dim message_ As New Messages With {
                        .id = messages_.Count + 1,
                        .type = "alert",
                        .action = "review",
                        .confidence = 0,
                        .source = "synapsis",
                        .field = "invoicecurrency",
                        .value = documentGpt_.invoicecurrency,
                        .message = "Validar no hubo coincidencia"
                    }

                    messages_.Add(message_)

                    countAlert_ += 1

                End If

            End If

        Else
            If documentGpt_.invoicecurrency IsNot Nothing And documentGpt_.invoicecurrency <> "null" Then

                documentTextract_.invoicecurrency = documentGpt_.invoicecurrency

                Dim message_ As New Messages With {
                    .id = messages_.Count + 1,
                    .type = "alert",
                    .action = "review",
                    .confidence = 0,
                    .source = "synapsis",
                    .field = "invoicecurrency",
                    .value = documentGpt_.invoicecurrency,
                    .message = "Validar textract no dio resultados"
                }

                'messages_.Add(message_)

            End If

        End If

        '##############################################################################################################
        If documentTextract_.countrycurrency IsNot Nothing And documentTextract_.countrycurrency <> "null" And documentTextract_.countrycurrency <> "NULL" And documentTextract_.countrycurrency <> "" Then

            If documentGpt_.countrycurrency IsNot Nothing And documentGpt_.countrycurrency <> "null" Then

                If documentTextract_.countrycurrency <> documentGpt_.countrycurrency Then

                    Dim message_ As New Messages With {
                        .id = messages_.Count + 1,
                        .type = "alert",
                        .action = "review",
                        .confidence = 0,
                        .source = "synapsis",
                        .field = "countrycurrency",
                        .value = documentGpt_.countrycurrency,
                        .message = "Validar no hubo coincidencia"
                    }

                    messages_.Add(message_)

                    countAlert_ += 1

                End If

            End If

        Else
            If documentGpt_.countrycurrency IsNot Nothing And documentGpt_.countrycurrency <> "null" Then

                documentTextract_.countrycurrency = documentGpt_.countrycurrency

                Dim message_ As New Messages With {
                    .id = messages_.Count + 1,
                    .type = "alert",
                    .action = "review",
                    .confidence = 0,
                    .source = "synapsis",
                    .field = "countrycurrency",
                    .value = documentGpt_.countrycurrency,
                    .message = "Validar textract no dio resultados"
                }

                'messages_.Add(message_)

            End If

        End If

        'If documentTextract_.supplier.supliername IsNot Nothing And documentTextract_.supplier.supliername <> "null" Then

        '    If documentGpt_.supplier.supliername IsNot Nothing And documentGpt_.supplier.supliername <> "null" Then

        '        If documentTextract_.supplier.supliername <> documentGpt_.supplier.supliername Then

        '            Dim message_ As New Messages With {
        '                .id = messages_.Count + 1,
        '                .type = "alert",
        '                .action = "review",
        '                .confidence = 0,
        '                .source = "synapsis",
        '                .field = "supliername",
        '                .value = documentGpt_.supplier.supliername,
        '                .message = "Validar no hubo coincidencia"
        '            }

        '            messages_.Add(message_)

        '        End If

        '    End If

        'Else
        '    If documentGpt_.supplier.supliername IsNot Nothing And documentGpt_.supplier.supliername <> "null" Then

        '        documentTextract_.supplier.supliername = documentGpt_.supplier.supliername

        '        Dim message_ As New Messages With {
        '            .id = messages_.Count + 1,
        '            .type = "alert",
        '            .action = "review",
        '            .confidence = 0,
        '            .source = "synapsis",
        '            .field = "supliername",
        '            .value = documentGpt_.supplier.supliername,
        '            .message = "Validar textract no dio resultados"
        '        }

        '        messages_.Add(message_)

        '    End If

        'End If

        'If documentTextract_.additionaldetails.packages IsNot Nothing And documentTextract_.additionaldetails.packages <> "null" Then

        '    If documentGpt_.additionaldetails.packages IsNot Nothing And documentGpt_.additionaldetails.packages <> "null" Then

        '        If documentTextract_.additionaldetails.packages <> documentGpt_.additionaldetails.packages Then

        '            Dim message_ As New Messages With {
        '                .id = messages_.Count + 1,
        '                .type = "alert",
        '                .action = "review",
        '                .confidence = 0,
        '                .source = "synapsis",
        '                .field = "packages",
        '                .value = documentGpt_.additionaldetails.packages,
        '                .message = "Validar no hubo coincidencia"
        '            }

        '            messages_.Add(message_)

        '        End If

        '    End If

        'Else
        '    If documentGpt_.additionaldetails.packages IsNot Nothing And documentGpt_.additionaldetails.packages <> "null" Then

        '        documentTextract_.additionaldetails.packages = documentGpt_.additionaldetails.packages

        '        Dim message_ As New Messages With {
        '            .id = messages_.Count + 1,
        '            .type = "alert",
        '            .action = "review",
        '            .confidence = 0,
        '            .source = "synapsis",
        '            .field = "additionaldetails.packages",
        '            .value = documentGpt_.additionaldetails.packages,
        '            .message = "Validar textract no dio resultados"
        '        }

        '        messages_.Add(message_)

        '    End If

        'End If
        '##############################################################################################################
        If documentTextract_.additionaldetails.incoterm IsNot Nothing And documentTextract_.additionaldetails.incoterm <> "null" Then

            If documentGpt_.additionaldetails.incoterm IsNot Nothing And documentGpt_.additionaldetails.incoterm <> "null" Then

                If documentTextract_.additionaldetails.incoterm <> documentGpt_.additionaldetails.incoterm Then

                    Dim message_ As New Messages With {
                        .id = messages_.Count + 1,
                        .type = "alert",
                        .action = "review",
                        .confidence = 0,
                        .source = "synapsis",
                        .field = "incoterm",
                        .value = documentGpt_.additionaldetails.incoterm,
                        .message = "Validar no hubo coincidencia"
                    }

                    messages_.Add(message_)

                    countAlert_ += 1

                End If

            End If

        Else

            If documentGpt_.additionaldetails.incoterm IsNot Nothing And documentGpt_.additionaldetails.incoterm <> "null" Then

                documentTextract_.additionaldetails.incoterm = documentGpt_.additionaldetails.incoterm

                Dim message_ As New Messages With {
                    .id = messages_.Count + 1,
                    .type = "alert",
                    .action = "review",
                    .confidence = 0,
                    .source = "synapsis",
                    .field = "incoterm",
                    .value = documentGpt_.additionaldetails.incoterm,
                    .message = "Validar textract no dio resultados"
                }

                'messages_.Add(message_)

            End If

        End If

        count_ = 0

        If documentTextract_.items.Count > 0 Then

            If documentGpt_.items.Count > 0 Then

                For Each item_ In documentTextract_.items

                    If count_ < documentGpt_.items.Count Then
                        '##############################################################################################################
                        If item_.partnumber IsNot Nothing And item_.partnumber <> "null" And item_.partnumber <> "NULL" And item_.partnumber <> "" Then

                            If documentGpt_.items(count_).partnumber IsNot Nothing And documentGpt_.items(count_).partnumber <> "null" And documentGpt_.items(count_).partnumber <> "" Then

                                If item_.partnumber <> documentGpt_.items(count_).partnumber Then

                                    Dim message_ As New Messages With {
                                    .id = messages_.Count + 1,
                                    .type = "alert",
                                    .action = "review",
                                    .confidence = 0,
                                    .source = "synapsis",
                                    .field = "partnumber",
                                    .value = documentGpt_.items(count_).partnumber,
                                    .message = "Validar no hubo coincidencia",
                                    ._object = item_.sec
                                }

                                    messages_.Add(message_)

                                    countAlert_ += 1

                                End If

                            End If

                        Else

                            If documentGpt_.items(count_).partnumber IsNot Nothing And documentGpt_.items(count_).partnumber <> "null" Then

                                item_.partnumber = documentGpt_.items(count_).partnumber

                                Dim message_ As New Messages With {
                                .id = messages_.Count + 1,
                                .type = "alert",
                                .action = "review",
                                .confidence = 0,
                                .source = "synapsis",
                                .field = "partnumber",
                                .value = documentGpt_.items(count_).partnumber,
                                .message = "Validar textract no dio resultados"
                            }

                                'messages_.Add(message_)

                            End If

                        End If

                        '##############################################################################################################
                        If item_.quantity <> Nothing Then

                            If documentGpt_.items(count_).quantity <> Nothing Then

                                If item_.quantity <> documentGpt_.items(count_).quantity Then

                                    Dim message_ As New Messages With {
                                        .id = messages_.Count + 1,
                                        .type = "alert",
                                        .action = "review",
                                        .confidence = 0,
                                        .source = "synapsis",
                                        .field = "quantity",
                                        .value = documentGpt_.items(count_).quantity,
                                        .message = "Validar no hubo coincidencia",
                                        ._object = item_.sec
                                    }

                                    messages_.Add(message_)

                                    countAlert_ += 1

                                End If

                            End If

                        Else
                            If documentGpt_.items(count_).quantity <> Nothing Then

                                item_.quantity = documentGpt_.items(count_).quantity

                                Dim message_ As New Messages With {
                                    .id = messages_.Count + 1,
                                    .type = "alert",
                                    .action = "review",
                                    .confidence = 0,
                                    .source = "synapsis",
                                    .field = "partnumber",
                                    .value = documentGpt_.items(count_).quantity,
                                    .message = "Validar textract no dio resultados"
                                }

                                'messages_.Add(message_)

                            End If

                        End If

                        'If item_.unit IsNot Nothing And item_.unit <> "null" Then

                        '    If documentGpt_.items(count_).unit IsNot Nothing And documentGpt_.items(count_).unit <> "null" Then

                        '        If item_.unit <> documentGpt_.items(count_).unit Then

                        '            Dim message_ As New Messages With {
                        '                .id = messages_.Count + 1,
                        '                .type = "alert",
                        '                .action = "review",
                        '                .confidence = 0,
                        '                .source = "synapsis",
                        '                .field = "unit",
                        '                .value = documentGpt_.items(count_).unit,
                        '                .message = "Validar no hubo coincidencia"
                        '            }

                        '            messages_.Add(message_)

                        '        End If

                        '    End If

                        'Else
                        '    If documentGpt_.items(count_).unit IsNot Nothing And documentGpt_.items(count_).unit <> "null" Then

                        '        item_.unit = documentGpt_.items(count_).unit

                        '        Dim message_ As New Messages With {
                        '            .id = messages_.Count + 1,
                        '            .type = "alert",
                        '            .action = "review",
                        '            .confidence = 0,
                        '            .source = "synapsis",
                        '            .field = "unit",
                        '            .value = documentGpt_.items(count_).unit,
                        '            .message = "Validar textract no dio resultados"
                        '        }

                        '        messages_.Add(message_)

                        '    End If

                        'End If

                        'If item_.description IsNot Nothing Then

                        '    If documentGpt_.items(count_).description IsNot Nothing Then

                        '        If item_.description <> documentGpt_.items(count_).description Then

                        '            Dim message_ As New Messages With {
                        '                .id = messages_.Count + 1,
                        '                .type = "alert",
                        '                .action = "review",
                        '                .confidence = 0,
                        '                .source = "synapsis",
                        '                .field = "description",
                        '                .value = documentGpt_.items(count_).description,
                        '                .message = "Validar no hubo coincidencia"
                        '            }

                        '            messages_.Add(message_)

                        '        End If

                        '    End If

                        'Else
                        '    If documentGpt_.items(count_).description IsNot Nothing Then

                        '        item_.description = documentGpt_.items(count_).description

                        '        Dim message_ As New Messages With {
                        '            .id = messages_.Count + 1,
                        '            .type = "alert",
                        '            .action = "review",
                        '            .confidence = 0,
                        '            .source = "synapsis",
                        '            .field = "description",
                        '            .value = documentGpt_.items(count_).description,
                        '            .message = "Validar textract no dio resultados"
                        '        }

                        '        messages_.Add(message_)

                        '    End If

                        'End If

                        'If item_.total <> Nothing Then

                        '    If documentGpt_.items(count_).total <> Nothing Then

                        '        If item_.total <> documentGpt_.items(count_).total Then

                        '            Dim message_ As New Messages With {
                        '                .id = messages_.Count + 1,
                        '                .type = "alert",
                        '                .action = "review",
                        '                .confidence = 0,
                        '                .source = "synapsis",
                        '                .field = "total",
                        '                .value = documentGpt_.items(count_).total,
                        '                .message = "Validar no hubo coincidencia"
                        '            }

                        '            messages_.Add(message_)

                        '        End If

                        '    End If

                        'Else
                        '    If documentGpt_.items(count_).total <> Nothing Then

                        '        item_.total = documentGpt_.items(count_).total

                        '        Dim message_ As New Messages With {
                        '            .id = messages_.Count + 1,
                        '            .type = "alert",
                        '            .action = "review",
                        '            .confidence = 0,
                        '            .source = "synapsis",
                        '            .field = "total",
                        '            .value = documentGpt_.items(count_).total,
                        '            .message = "Validar textract no dio resultados"
                        '        }

                        '        messages_.Add(message_)

                        '    End If

                        'End If

                        'If item_.currency IsNot Nothing Then

                        '    If documentGpt_.items(count_).currency IsNot Nothing Then

                        '        If item_.currency <> documentGpt_.items(count_).currency Then

                        '            Dim message_ As New Messages With {
                        '                .id = messages_.Count + 1,
                        '                .type = "alert",
                        '                .action = "review",
                        '                .confidence = 0,
                        '                .source = "synapsis",
                        '                .field = "currency",
                        '                .value = documentGpt_.items(count_).currency,
                        '                .message = "Validar no hubo coincidencia"
                        '            }

                        '            messages_.Add(message_)

                        '        End If

                        '    End If

                        'Else
                        '    If documentGpt_.items(count_).currency IsNot Nothing Then

                        '        item_.currency = documentGpt_.items(count_).currency

                        '        Dim message_ As New Messages With {
                        '            .id = messages_.Count + 1,
                        '            .type = "alert",
                        '            .action = "review",
                        '            .confidence = 0,
                        '            .source = "synapsis",
                        '            .field = "currency",
                        '            .value = documentGpt_.items(count_).currency,
                        '            .message = "Validar textract no dio resultados"
                        '        }

                        '        messages_.Add(message_)

                        '    End If

                        'End If

                        '##############################################################################################################
                        If item_.usdvalue <> Nothing Then

                            If documentGpt_.items(count_).usdvalue <> Nothing Then

                                If item_.usdvalue <> documentGpt_.items(count_).usdvalue Then

                                    Dim message_ As New Messages With {
                                        .id = messages_.Count + 1,
                                        .type = "alert",
                                        .action = "review",
                                        .confidence = 0,
                                        .source = "synapsis",
                                        .field = "usdvalue",
                                        .value = documentGpt_.items(count_).usdvalue,
                                        .message = "Validar no hubo coincidencia",
                                        ._object = item_.sec
                                    }

                                    messages_.Add(message_)

                                    countAlert_ += 1

                                End If

                            End If

                        Else
                            If documentGpt_.items(count_).usdvalue <> Nothing Then

                                item_.usdvalue = documentGpt_.items(count_).usdvalue

                                Dim message_ As New Messages With {
                                    .id = messages_.Count + 1,
                                    .type = "alert",
                                    .action = "review",
                                    .confidence = 0,
                                    .source = "synapsis",
                                    .field = "usdvalue",
                                    .value = documentGpt_.items(count_).usdvalue,
                                    .message = "Validar textract no dio resultados"
                                }

                                'messages_.Add(message_)

                            End If

                        End If

                        '##############################################################################################################
                        If item_.value <> Nothing And item_.value > 0 Then

                            If documentGpt_.items(count_).value <> Nothing And documentGpt_.items(count_).value > 0 Then

                                If item_.value <> documentGpt_.items(count_).value Then

                                    Dim message_ As New Messages With {
                                        .id = messages_.Count + 1,
                                        .type = "alert",
                                        .action = "review",
                                        .confidence = 0,
                                        .source = "synapsis",
                                        .field = "value",
                                        .value = documentGpt_.items(count_).value,
                                        .message = "Validar no hubo coincidencia",
                                        ._object = item_.sec
                                    }

                                    messages_.Add(message_)

                                    countAlert_ += 1

                                End If

                            End If

                        Else
                            If documentGpt_.items(count_).value <> Nothing Then

                                item_.value = documentGpt_.items(count_).value

                                Dim message_ As New Messages With {
                                    .id = messages_.Count + 1,
                                    .type = "alert",
                                    .action = "review",
                                    .confidence = 0,
                                    .source = "synapsis",
                                    .field = "value",
                                    .value = documentGpt_.items(count_).value,
                                    .message = "Validar textract no dio resultados"
                                }

                                'messages_.Add(message_)

                            End If

                        End If

                        'If item_.netweight <> Nothing Then

                        '    If documentGpt_.items(count_).netweight <> Nothing Then

                        '        If item_.netweight <> documentGpt_.items(count_).netweight Then

                        '            Dim message_ As New Messages With {
                        '                .id = messages_.Count + 1,
                        '                .type = "alert",
                        '                .action = "review",
                        '                .confidence = 0,
                        '                .source = "synapsis",
                        '                .field = "netweight",
                        '                .value = documentGpt_.items(count_).netweight,
                        '                .message = "Validar no hubo coincidencia"
                        '            }

                        '            messages_.Add(message_)

                        '        End If

                        '    End If

                        'Else
                        '    If documentGpt_.items(count_).netweight <> Nothing Then

                        '        item_.partnumber = documentGpt_.items(count_).netweight

                        '        Dim message_ As New Messages With {
                        '            .id = messages_.Count + 1,
                        '            .type = "alert",
                        '            .action = "review",
                        '            .confidence = 0,
                        '            .source = "synapsis",
                        '            .field = "netweight",
                        '            .value = documentGpt_.items(count_).netweight,
                        '            .message = "Validar textract no dio resultados"
                        '        }

                        '        messages_.Add(message_)

                        '    End If

                        'End If

                        'If item_.purchaseorder IsNot Nothing And item_.purchaseorder <> "null" Then

                        '    If documentGpt_.items(count_).purchaseorder IsNot Nothing And documentGpt_.items(count_).purchaseorder <> "null" Then

                        '        If item_.purchaseorder <> documentGpt_.items(count_).purchaseorder Then

                        '            Dim message_ As New Messages With {
                        '                .id = messages_.Count + 1,
                        '                .type = "alert",
                        '                .action = "review",
                        '                .confidence = 0,
                        '                .source = "synapsis",
                        '                .field = "purchaseorder",
                        '                .value = documentGpt_.items(count_).purchaseorder,
                        '                .message = "Validar no hubo coincidencia"
                        '            }

                        '            messages_.Add(message_)

                        '        End If

                        '    End If

                        'Else
                        '    If documentGpt_.items(count_).purchaseorder IsNot Nothing And documentGpt_.items(count_).purchaseorder <> "null" Then

                        '        item_.purchaseorder = documentGpt_.items(count_).purchaseorder

                        '        Dim message_ As New Messages With {
                        '            .id = messages_.Count + 1,
                        '            .type = "alert",
                        '            .action = "review",
                        '            .confidence = 0,
                        '            .source = "synapsis",
                        '            .field = "purchaseorder",
                        '            .value = documentGpt_.items(count_).purchaseorder,
                        '            .message = "Validar textract no dio resultados"
                        '        }

                        '        messages_.Add(message_)

                        '    End If

                        'End If

                        'If item_.destinationcountry IsNot Nothing Then

                        '    If documentGpt_.items(count_).destinationcountry IsNot Nothing Then

                        '        If item_.destinationcountry <> documentGpt_.items(count_).destinationcountry Then

                        '            Dim message_ As New Messages With {
                        '                .id = messages_.Count + 1,
                        '                .type = "alert",
                        '                .action = "review",
                        '                .confidence = 0,
                        '                .source = "synapsis",
                        '                .field = "destinationcountry",
                        '                .value = documentGpt_.items(count_).destinationcountry,
                        '                .message = "Validar no hubo coincidencia"
                        '            }

                        '            messages_.Add(message_)

                        '        End If

                        '    End If

                        'Else
                        '    If documentGpt_.items(count_).destinationcountry IsNot Nothing Then

                        '        item_.destinationcountry = documentGpt_.items(count_).destinationcountry

                        '        Dim message_ As New Messages With {
                        '            .id = messages_.Count + 1,
                        '            .type = "alert",
                        '            .action = "review",
                        '            .confidence = 0,
                        '            .source = "synapsis",
                        '            .field = "destinationcountry",
                        '            .value = documentGpt_.items(count_).destinationcountry,
                        '            .message = "Validar textract no dio resultados"
                        '        }

                        '        messages_.Add(message_)

                        '    End If

                        'End If


                        '##############################################################################################################
                        If item_.origincountry IsNot Nothing And item_.origincountry <> "null" And item_.origincountry <> "NULL" And item_.origincountry <> "" Then

                            If documentGpt_.items(count_).origincountry IsNot Nothing And documentGpt_.items(count_).origincountry <> "null" And item_.origincountry <> "NULL" Then

                                If item_.origincountry <> documentGpt_.items(count_).origincountry Then

                                    Dim message_ As New Messages With {
                                        .id = messages_.Count + 1,
                                        .type = "alert",
                                        .action = "review",
                                        .confidence = 0,
                                        .source = "synapsis",
                                        .field = "origincountry",
                                        .value = documentGpt_.items(count_).origincountry,
                                        .message = "Validar no hubo coincidencia",
                                    ._object = item_.sec
                                    }

                                    messages_.Add(message_)

                                    countAlert_ += 1

                                End If

                            End If

                        Else
                            If documentGpt_.items(count_).origincountry IsNot Nothing Then

                                item_.origincountry = documentGpt_.items(count_).origincountry

                                Dim message_ As New Messages With {
                                    .id = messages_.Count + 1,
                                    .type = "alert",
                                    .action = "review",
                                    .confidence = 0,
                                    .source = "synapsis",
                                    .field = "origincountry",
                                    .value = documentGpt_.items(count_).origincountry,
                                    .message = "Validar textract no dio resultados"
                                }

                                'messages_.Add(message_)

                            End If

                        End If

                        count_ += 1

                    End If

                Next

            End If

        Else

            If documentGpt_.items.Count > 0 Then

                documentTextract_.items = documentGpt_.items

                Dim message_ As New Messages With {
                    .id = messages_.Count + 1,
                    .type = "alert",
                    .action = "review",
                    .confidence = 0,
                    .source = "synapsis",
                    .field = "items",
                    .value = 0,
                    .message = "Validar textract no dio resultados"
                }

                messages_.Add(message_)

            End If

        End If

        Dim max_ = (documentTextract_.items.Count * 5) + 4

        documentTextract_.score = Math.Round((100 - (countAlert_ / max_ * 100)), 2)

        Status = New TagWatcher(vbOK) With {
            .ObjectReturned = documentTextract_
        }

        documentTextract_.analysis.gpttokensdownload += documentGpt_.analysis.gpttokensdownload

        documentTextract_.analysis.gpttokensupload += documentGpt_.analysis.gpttokensupload

        Dim result_ As Object = Status.ObjectReturned

        Dim jObject_ As JObject = ConvertToJObject(result_)

        Dim x = jObject_.ToJson

        Dim jsonString = JsonConvert.SerializeObject(jObject_)

        'Dim jsonString2 As String = jObject_.ToString

        Return Status

    End Function

    Private Function ConvertToJObject(Of T)(obj As T) As JObject
        ' Si el objeto es un valor simple (JValue), lo envuelves dentro de un JObject
        If TypeOf obj Is JValue Then
            Return New JObject(New JProperty("value", obj))
        End If

        ' Si no es un JValue, se convierte normalmente a JObject
        Return JObject.FromObject(obj)
    End Function

End Class
