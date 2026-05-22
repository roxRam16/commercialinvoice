Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Syn.Utils
Imports Wma.Exceptions

Public Class ControladorPaises

#Region "Propiedades"

    Property t_cvecomercioMX As String

    Property t_cveISOnum As String

    Property t_cveISO2 As String

    Property t_cveISO3 As String

    Property t_nombrepaisesp As String

    Property t_nombrepaising As String

    Property t_nombrepaiscortoesp As String

    Property t_nombrepaiscortoing As String

    Property t_ClaveMoneda As String

    Property t_ClaveISO As String

    Property t_CodigoISO As String

    Property t_NombreMoneda As String

    Property esNuevaMoneda As Boolean

    Property esMonedaCurso As Boolean

#End Region

    'Función para agregar un nuevo país
    Public Function NuevoPais(Optional ByVal session_ As IClientSessionHandle = Nothing) As TagWatcher

        Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos(26)

        Dim operationsDB_ = iEnlace_.GetMongoCollection(Of Pais)("Reg000Paises")

        Dim tagwatcher_ As New TagWatcher

        Dim moneda_ As New moneda With
                                {._idmoneda = ObjectId.GenerateNewId,
                                 .secmoneda = 1,
                                 .cvemonedaA05 = t_ClaveMoneda,
                                 .claveISO = t_ClaveISO,
                                 .codigoISO = t_CodigoISO,
                                 .nombremoneda = t_NombreMoneda,
                                 .estado = 1,
                                 .archivado = False
                                }

        Dim pais_ As New Pais(t_cvecomercioMX,
                              t_cveISOnum,
                              t_cveISO2,
                              t_cveISO3,
                              t_nombrepaisesp,
                              t_nombrepaising,
                              t_nombrepaiscortoesp,
                              t_nombrepaiscortoing,
                              IIf(moneda_ IsNot Nothing, moneda_, Nothing)
                            )

        'Dim pais_ As New Pais With {.cvecomercioMX = t_cvecomercioMX,
        '                            .cveISOnum = t_cveISOnum,
        '                            .cveISO2 = t_cveISO2,
        '                            .cveISO3 = t_cveISO3,
        '                            .nombrepaisesp = t_nombrepaisesp,
        '                            .nombrepaising = t_nombrepaising,
        '                            .nombrepaiscortoesp = t_nombrepaiscortoesp,
        '                            .nombrepaiscortoing = t_nombrepaiscortoing,
        '                            .monedasoficiales = IIf(moneda_ IsNot Nothing, moneda_, Nothing)
        '}

        Dim result_ = operationsDB_.InsertOneAsync(session_, pais_).ConfigureAwait(False)

        With tagwatcher_

            .SetOK()

            .ObjectReturned = pais_

        End With

        Return tagwatcher_

    End Function

    'Función para actualizar un nuevo país
    Public Function ActualizaPais(ByVal pais_ As Pais,
                               Optional ByVal session_ As IClientSessionHandle = Nothing) As TagWatcher

        Dim tagwatcher_ As New TagWatcher

        If pais_ IsNot Nothing Then

            With pais_

                If (.nombrepaisesp <> t_nombrepaisesp) Then

                    .nombrepaisesp = t_nombrepaisesp

                End If

                If (.nombrepaising <> t_nombrepaising) Then

                    .nombrepaising = t_nombrepaising

                End If

                If (.nombrepaiscortoesp <> t_nombrepaiscortoesp) Then

                    .nombrepaiscortoesp = t_nombrepaiscortoesp

                End If

                If (.nombrepaiscortoing <> t_nombrepaiscortoing) Then

                    .nombrepaiscortoing = t_nombrepaiscortoing

                End If

                If (.nombremonedacurso <> t_NombreMoneda And esMonedaCurso) Then

                    .nombremonedacurso = t_NombreMoneda

                End If

                If (.cvemonedacurso <> t_ClaveMoneda And esMonedaCurso) Then

                    .cvemonedacurso = t_ClaveMoneda

                End If

                If esNuevaMoneda = True Then

                    Dim unique_ = From moneda In .monedasoficiales Where
                                                              moneda.claveISO = t_ClaveISO And
                                                              moneda.codigoISO = t_CodigoISO And
                                                              moneda.cvemonedaA05 = t_ClaveMoneda And
                                                              moneda.nombremoneda = t_NombreMoneda

                    If unique_.Count = 0 Then

                        Dim sec_ = pais_.monedasoficiales.Last.secmoneda + 1

                        .monedasoficiales.Add(
                         New moneda With
                            {._idmoneda = ObjectId.GenerateNewId,
                              .secmoneda = sec_,
                              .claveISO = t_ClaveISO,
                              .codigoISO = t_CodigoISO,
                              .cvemonedaA05 = t_ClaveMoneda,
                              .nombremoneda = t_NombreMoneda,
                              .estado = 1,
                              .archivado = False
                            }
                        )

                        System.Web.HttpContext.Current.Session("_secMoneda") = sec_ - 1

                    End If

                End If

            End With

            Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos(26)

            Dim operationsDB_ = iEnlace_.GetMongoCollection(Of Pais)("Reg000Paises")

            Dim filter_ = Builders(Of Pais).Filter.Eq(Function(x) x._id, pais_._id)

            Dim setStructureOfSubs_ = Builders(Of Pais).Update.
                                 Set(Function(x) x.nombrepaisesp, pais_.nombrepaisesp).
                                 Set(Function(x) x.nombrepaising, pais_.nombrepaising).
                                 Set(Function(x) x.nombrepaiscortoesp, pais_.nombrepaiscortoesp).
                                 Set(Function(x) x.nombrepaiscortoing, pais_.nombrepaiscortoing).
                                 Set(Function(x) x.cvemonedacurso, pais_.cvemonedacurso).
                                 Set(Function(x) x.nombremonedacurso, pais_.nombremonedacurso).
                                 AddToSet(Of moneda)("monedasoficiales", pais_.monedasoficiales(System.Web.HttpContext.Current.Session("_secMoneda")))

            Dim result_ = operationsDB_.UpdateOneAsync(session_, filter_, setStructureOfSubs_).Result

            With tagwatcher_

                If result_.MatchedCount <> 0 Then

                    .SetOK()

                ElseIf result_.UpsertedId IsNot Nothing Then

                    .SetOK()

                Else

                    .SetError(Me, "No se generaron cambios")

                End If

            End With

            Return tagwatcher_

        Else

            tagwatcher_.SetError(Me, "No existe una instancia de la empresa")

        End If

        Return tagwatcher_

    End Function

    'Función para buscar países por Aggregate (recomendable)
    Public Shared Function BuscarPaises(ByRef paisestemporal_ As List(Of Pais),
                                   ByVal token_ As String) As Object

        Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos(26)

        Dim operationsDB_ = iEnlace_.GetMongoCollection(Of Pais)("Reg000Paises")

        Dim listaPaises_ As New List(Of SelectOption)

        Dim options_ = New AggregateOptions With {
                .AllowDiskUse = False
        }

        token_ = "\""" & token_ & """\"

        Dim pipeline_ As PipelineDefinition(Of Pais, Pais) = New BsonDocument() {
            New BsonDocument("$match", New BsonDocument().Add("$text", New BsonDocument().Add("$search", token_)))}

        Using cursorPais_ = operationsDB_.AggregateAsync(Of Pais)(pipeline_, options_).Result

            While cursorPais_.MoveNext

                Dim batch_ = cursorPais_.Current

                For Each estatus_ As Pais In batch_

                    listaPaises_.Add(New SelectOption With {.Value = estatus_._id.ToString, .Text = estatus_.cvecomercioMX & " - " & estatus_.nombrepaisesp})

                    paisestemporal_.Add(estatus_)

                Next

            End While

        End Using

        Return listaPaises_

    End Function

    'Función para buscar por Aggregate usando expresiones (no recomendable)
    Public Shared Function BuscarPaisesExpresion(ByRef paisestemporal_ As List(Of Pais),
                                   ByVal token_ As String) As Object

        Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos(26)

        Dim operationsDB_ = iEnlace_.GetMongoCollection(Of Pais)("Reg000Paises")

        Dim listaPaises_ As New List(Of SelectOption)

        Dim hint_ = New BsonString("_idx_nombrepais")

        Dim options_ = New AggregateOptions With {
            .AllowDiskUse = False,
            .Collation = New Collation("es@collation=search", False, CollationCaseFirst.Off, CollationStrength.Primary),
            .Hint = hint_
        }

        token_ = token_.Replace("a", "[a,á,à,ä]").Replace("e", "[e,é,ë]").Replace("i", "[i,í,ï]").Replace("o", "[o,ó,ö,ò]").Replace("u", "[u,ü,ú,ù]")

        Dim pipeline_ As PipelineDefinition(Of Pais, Pais) = New BsonDocument() {
            New BsonDocument(
                "$match", New BsonDocument().Add("$or", New BsonArray From {
                    New BsonDocument From {
                        {"nombrepaisesp", New BsonDocument From {
                            {"$regex", token_},
                            {"$options", "$i"}
                        }
                        }
                    },
                    New BsonDocument From {
                        {"nombrepaiscortoesp", New BsonDocument From {
                            {"$regex", token_},
                            {"$options", "$i"}
                        }
                        }
                    },
                    New BsonDocument From {
                        {"cvecomercioMX", New BsonDocument From {
                            {"$regex", token_},
                            {"$options", "$i"}
                        }
                        }
                    },
                    New BsonDocument From {
                        {"cveISO2", New BsonDocument From {
                            {"$regex", token_},
                            {"$options", "$i"}
                        }
                        }
                    }
                })
            )
        }

        Using cursor_ = operationsDB_.AggregateAsync(Of Pais)(pipeline_, options_).Result

            While cursor_.MoveNext

                Dim batch = cursor_.Current

                For Each estatus_ As Pais In batch

                    listaPaises_.Add(New SelectOption With {.Value = estatus_._id.ToString, .Text = estatus_.cvecomercioMX & " - " & estatus_.nombrepaisesp})

                    paisestemporal_.Add(estatus_)

                Next
            End While

        End Using

        Return listaPaises_

    End Function

    'Función para buscar por Aggregate usando BsonDocument (no recomendable)
    Public Shared Function BuscarPaisesBsonDocument(ByRef paisestemporal_ As List(Of Pais),
                                   ByVal token_ As String) As Object

        Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos(26)

        Dim operationsDB_ = iEnlace_.GetMongoCollection(Of Pais)("Reg000Paises")

        token_ = token_.Replace("a", "[a,á,à,ä]").Replace("e", "[e,é,ë]").Replace("i", "[i,í,ï]").Replace("o", "[o,ó,ö,ò]").Replace("u", "[u,ü,ú,ù]")

        Dim pipeline_ As New List(Of BsonDocument)

        Dim condicion_ = New BsonDocument From {
            {
                "$match", New BsonDocument From {
                    {"estado", 1},
                    {"$or", New BsonArray From {
                        New BsonDocument From {
                            {"nombrepaisesp", New BsonDocument From {
                                {"$regex", token_},
                                {"$options", "i"}
                            }
                            }
                        },
                        New BsonDocument From {
                            {"nombrepaiscortoesp", New BsonDocument From {
                                {"$regex", token_},
                                {"$options", "i"}
                            }
                            }
                        },
                        New BsonDocument From {
                            {"cvecomercioMX", New BsonDocument From {
                                {"$regex", token_},
                                {"$options", "i"}
                            }
                            }
                        },
                        New BsonDocument From {
                            {"cveISO2", New BsonDocument From {
                                {"$regex", token_},
                                {"$options", "i"}
                            }
                            }
                        }
                    }
                    }
                }
            }
        }

        Dim camposConsulta_ = New BsonDocument From {
            {
                "$project", New BsonDocument From {
                    {"_id", 1},
                    {"_idPais", 1},
                    {"cvecomercioMX", 1},
                    {"cveISO2", 1},
                    {"cveISO3", 1},
                    {"nombrepaisesp", 1},
                    {"nombrepaiscortoesp", 1},
                    {"_idmonedacurso", 1},
                    {"cvemonedacurso", 1},
                    {"nombremonedacurso", 1}
                }
            }
        }

        pipeline_.Add(condicion_)

        pipeline_.Add(camposConsulta_)

        Dim resultado_ = operationsDB_.Aggregate(Of BsonDocument)(pipeline_).ToList

        Dim paises_ As New List(Of Pais)

        For Each estatus_ As BsonDocument In resultado_

            paises_.Add(New Pais With {
                                 ._id = New ObjectId(estatus_.GetElement("_id").Value.ToString),
                                 ._idPais = estatus_.GetElement("_idPais").Value.ToString,
                                 .cvecomercioMX = estatus_.GetElement("cvecomercioMX").Value.ToString,
                                 .nombrepaiscortoesp = estatus_.GetElement("nombrepaiscortoesp").Value.ToString,
                                 .nombrepaisesp = estatus_.GetElement("nombrepaisesp").Value.ToString,
                                 ._idmonedacurso = New ObjectId(estatus_.GetElement("_idmonedacurso").Value.ToString),
                                 .cvemonedacurso = estatus_.GetElement("cvemonedacurso").Value.ToString,
                                 .nombremonedacurso = estatus_.GetElement("nombremonedacurso").Value.ToString
            })

        Next

        Dim listaPaises_ As New List(Of SelectOption)

        If paises_ IsNot Nothing And paises_.Count > 0 Then

            For Each pais_ In paises_

                listaPaises_.Add(New SelectOption With {.Value = pais_._id.ToString(), .Text = pais_.cvecomercioMX & " - " & pais_.nombrepaisesp})

            Next

        End If

        paisestemporal_ = paises_

        Return listaPaises_

    End Function

    'Función para buscar países con un Find usando índices (no recomendable)
    Public Shared Function BuscarPaisesFind(ByRef paisestemporal_ As List(Of Pais),
                                   ByVal token_ As String) As List(Of SelectOption)

        Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos(26)

        Dim operationsDB_ = iEnlace_.GetMongoCollection(Of Pais)("Reg000Paises")

        Dim filter_ = Builders(Of Pais).Filter.Text(token_, New TextSearchOptions() With {.DiacriticSensitive = False}) And
                      Builders(Of Pais).Filter.Eq(Function(x) x.estado, 1)

        Dim paises_ As New List(Of Pais)

        Dim listaPaises_ As New List(Of SelectOption)

        paises_ = operationsDB_.Find(filter_).Limit(10).ToList()

        If paises_ IsNot Nothing And paises_.Count > 0 Then

            For Each pais_ In paises_

                listaPaises_.Add(New SelectOption With {.Value = pais_._id.ToString(), .Text = pais_.nombrepaiscortoesp})

            Next

        End If

        paisestemporal_ = paises_

        Return listaPaises_

    End Function

    'Función para buscar pais con find por _id (Recomendable)
    Public Shared Function BuscarPais(ByVal idPais_ As ObjectId) As Pais

        Dim pais_ As Pais = Nothing

        Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos(26)

        Dim operationsDB_ = iEnlace_.GetMongoCollection(Of Pais)("Reg000Paises")

        Dim filter_ = Builders(Of Pais).Filter.Eq(Function(x) x._id, idPais_)

        Dim paises_ As New List(Of Pais)

        Dim resultado_ = operationsDB_.Find(filter_).ToList

        paises_ = resultado_

        If paises_ IsNot Nothing And paises_.Count > 0 Then

            If paises_(0) IsNot Nothing Then

                pais_ = paises_(0)

            End If

        End If

        Return pais_

    End Function

    'Función para buscar monedas dentro de la misma clase (Recomendable)
    Public Shared Function BuscarMonedasOficiales(ByVal pais_ As Pais) As List(Of SelectOption)

        Dim monedas_ As New List(Of SelectOption)

        Dim lista_ = pais_.monedasoficiales

        For Each moneda_ In lista_

            Dim stringMoneda_ As String

            With moneda_

                stringMoneda_ = IIf(.cvemonedaA05 <> "", .cvemonedaA05, .claveISO)

                monedas_.Add(
                                New SelectOption With
                                {.Value = moneda_._idmoneda.ToString(),
                                .Text = stringMoneda_
                                }
                            )

            End With

        Next

        Return monedas_

    End Function

    'Función para buscar en todas las monedas y que no se repitan (Recomendable)
    Public Shared Function BuscarTodasMonedas(ByVal token_ As String) As List(Of SelectOption)

        Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos(26)

        Dim operationsDB_ = iEnlace_.GetMongoCollection(Of Pais)("Reg000Paises")

        Dim monedas_ As New List(Of moneda)

        Dim listaMonedas_ As New List(Of SelectOption)

        Dim pipeline_ As New List(Of BsonDocument)

        Dim condicion_ = New BsonDocument From {
        {
            "$match", New BsonDocument From {
                {"estado", 1},
                {"$or", New BsonArray From {
                    New BsonDocument From {
                        {"cvemonedacurso", New BsonDocument From {
                            {"$regex", token_},
                            {"$options", "i"}
                        }
                        }
                    },
                    New BsonDocument From {
                        {"nombremonedacurso", New BsonDocument From {
                            {"$regex", token_},
                            {"$options", "i"}
                        }
                        }
                    },
                    New BsonDocument From {
                        {"monedasoficiales.cvemonedaA05", New BsonDocument From {
                            {"$regex", token_},
                            {"$options", "i"}
                        }
                        }
                    },
                    New BsonDocument From {
                        {"monedasoficiales.claveISO", New BsonDocument From {
                            {"$regex", token_},
                            {"$options", "i"}
                        }
                        }
                    },
                    New BsonDocument From {
                        {"monedasoficiales.nombremoneda", New BsonDocument From {
                            {"$regex", token_},
                            {"$options", "i"}
                        }
                        }
                    }
                }
                }
            }
        }
        }

        Dim camposConsulta_ = New BsonDocument From {
            {
                "$project", New BsonDocument From {
                    {"_idmonedacurso", 1},
                    {"cvemonedacurso", 1},
                    {"nombremonedacurso", 1},
                    {"monedasoficiales._idmoneda", 1},
                    {"monedasoficiales.cvemonedaA05", 1},
                    {"monedasoficiales.claveISO", 1},
                    {"monedasoficiales.nombremoneda", 1}
                }
            }
        }

        pipeline_.Add(condicion_)

        pipeline_.Add(camposConsulta_)

        Dim resultado_ = operationsDB_.Aggregate(Of BsonDocument)(pipeline_).ToList

        For Each estatus_ As BsonDocument In resultado_

            If Not monedas_.Exists(Function(x) x.cvemonedaA05 = estatus_.GetElement("cvemonedacurso").Value) Then

                monedas_.Add(New moneda With {
                    ._idmoneda = New ObjectId(estatus_.GetElement("_idmonedacurso").Value.ToString),
                    .cvemonedaA05 = estatus_.GetElement("cvemonedacurso").Value.ToString,
                    .nombremoneda = estatus_.GetElement("nombremonedacurso").Value.ToString
                })

            End If

        Next

        If monedas_ IsNot Nothing And monedas_.Count > 0 Then

            For Each moneda_ In monedas_

                If Not listaMonedas_.Exists(Function(x) x.Text = (moneda_.cvemonedaA05 & " - " & moneda_.nombremoneda)) Then

                    listaMonedas_.Add(New SelectOption With {.Value = moneda_._idmoneda.ToString(), .Text = moneda_.cvemonedaA05})

                End If

            Next

        End If

        Return listaMonedas_

    End Function

    ''' <summary>
    ''' EN OBSERVACIÓN, FAVOR DE NO IMPLEMENTAR
    ''' </summary>


    Public Shared Function ConsultarListaPaisesPorClaveISO(ByVal cveISO_ As String) _
            As TagWatcher

        'EN OBSERVACIÓN - FAVOR DE NO IMPLEMENTAR

        Dim Estado_ As TagWatcher = New TagWatcher

        With Estado_

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos(26)

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of Pais)("Reg000Paises")

                Dim filter_ = Builders(Of Pais).Filter.Text(cveISO_) And
                              Builders(Of Pais).Filter.Eq(Function(x) x.estado, 1)

                Dim sort_ = Builders(Of Pais).Sort.Descending("_id")

                Dim result_ = operationsDB_.Find(filter_).
                                            Project(Function(p) _
                                                    New With {
                                                    Key ._id = p._id,
                                                    Key .cveISO3 = p.cveISO3,
                                                    Key .paisPresentacion = p.cveISO3 & " - " & p.nombrepaisesp
                                            }).
                                            Sort(sort_).
                                            Limit(10).
                                            ToList().
                                            AsEnumerable()

                If result_.Count <> 0 Then

                    .ObjectReturned = result_

                    .SetOK()

                Else
                    .SetOKBut(Estado_, "No se encontraron resultados")

                End If

            End Using

        End With

        Return Estado_

    End Function

    ''' <summary>
    ''' ENTIDADES FEDERATIVAS
    ''' </summary>
    ''' 
    Public Shared Function ObtenerEntidadesFederativasPorPais(ByVal objectidPais_ As ObjectId) As TagWatcher

        Dim Estado_ As TagWatcher = New TagWatcher

        Dim listaEntidadesFederativas_ As New List(Of EntidadFederativa)

        With Estado_

            If Not objectidPais_ = ObjectId.Empty Then

                Try

                    Using iEnlace_ As IEnlaceDatos = New EnlaceDatos(26)

                        Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EntidadesFederativasPorPais)("Glo000EntidadesFederativas")

                        Dim filter_ = Builders(Of EntidadesFederativasPorPais).Filter.Eq(Function(x) x.idPais, objectidPais_) And
                                  Builders(Of EntidadesFederativasPorPais).Filter.Eq(Function(y) y.estado, 1)

                        Dim sort_ = Builders(Of EntidadesFederativasPorPais).Sort.Descending("_id")

                        Dim result_ = operationsDB_.Find(filter_).
                                                Project(Function(x) New With {Key .entidadesFederativas = x.entidadesfederativas}).
                                                ToList().
                                                AsEnumerable()

                        If result_.Count <> 0 Then

                            .ObjectReturned = DirectCast(result_.LastOrDefault.entidadesFederativas, List(Of EntidadFederativa))

                            .SetOK()

                        Else
                            .SetOKBut(Estado_, "No se encontraron resultados")

                        End If

                    End Using

                Catch ex As Exception

                    .SetError(Estado_, $"Ha ocurrido un error_ {ex}")

                End Try
            Else

                .SetOKBut(Estado_, "Objectid del país es requerido")

            End If

        End With

        Return Estado_

    End Function

    Public Shared Function ObtenerListaEntidadesFederativas(ByVal ObjectidPais_ As ObjectId,
                                                            ByVal nombreEntidadFederativa_ As String) As TagWatcher

        Dim Estado_ As New TagWatcher

        Dim listaEntidadesFederativas_ As New List(Of EntidadFederativa)

        With Estado_
            If Not ObjectidPais_ = ObjectId.Empty Then

                If nombreEntidadFederativa_ IsNot Nothing Then

                    Try

                        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos(26)

                            Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EntidadesFederativasPorPais)("Glo000EntidadesFederativas")

                            Dim filter_ = Builders(Of EntidadesFederativasPorPais).Filter.Eq(Function(x) x.idPais, ObjectidPais_) And
                                  Builders(Of EntidadesFederativasPorPais).Filter.Eq(Function(y) y.estado, 1)

                            Dim regex_ = New BsonRegularExpression(nombreEntidadFederativa_.Trim(), "i")

                            ''POR SI ME PIDEN VOLAR EL REGEX
                            ''Builders(Of BsonDocument).Filter.Eq(Function(x) x("entidadesfederativas")("entidadfederativa"), nombreEntidadFederativa_)

                            Dim result_ = operationsDB_.Aggregate() _
                            .Match(filter_) _
                            .Unwind("entidadesfederativas") _
                            .Match(Builders(Of BsonDocument).Filter.Regex("entidadesfederativas.entidadfederativa", regex_)) _
                            .Project(Function(x) New With {
                                Key ._idEntidadFederativa = x("entidadesfederativas")("_idEntidadFederativa"),
                                Key .entidadfederativa = x("entidadesfederativas")("entidadfederativa"),
                                Key .abreviatura = x("entidadesfederativas")("abreviatura"),
                                Key .estado = x("entidadesfederativas")("estado")
                            }) _
                            .ToList().AsEnumerable()

                            If result_.Count <> 0 Then

                                For Each item_ In result_
                                    Dim entidadFederativa_ As New EntidadFederativa With {
                                        ._idEntidadFederativa = item_._idEntidadFederativa,
                                        .abreviatura = item_.abreviatura,
                                        .entidadfederativa = item_.entidadfederativa,
                                        .estado = item_.estado
                                 }
                                    listaEntidadesFederativas_.Add(entidadFederativa_)
                                Next

                                .ObjectReturned = listaEntidadesFederativas_

                                .SetOK()

                            Else
                                .SetOKBut(Estado_, "No se encontraron resultados")

                            End If

                        End Using

                    Catch ex As Exception

                        .SetError(Estado_, $"Ha ocurrido un error_ {ex}")

                    End Try
                Else

                    .SetOKBut(Estado_, "Nombre o clave de la entidad federativa es requerido")

                End If

            Else

                .SetOKBut(Estado_, "Objectid del país es requerido")

            End If

        End With

        Return Estado_

    End Function

End Class



Public Class EntidadesFederativasPorPais
    Public Property id As ObjectId

    Public Property entidadesfederativas As List(Of EntidadFederativa)

    Public Property idPais As ObjectId

    Public Property nombrepaisesp As String

    Public Property nombrepaising As String

    Public Property cveISO3 As String

    Public Property archivado As Boolean

    Public Property estatus As Integer

    Public Property estado As Integer

End Class

Public Class EntidadFederativa
    Public Property _idEntidadFederativa As ObjectId

    Public Property abreviatura As String

    Public Property entidadfederativa As String

    Public Property estado As Integer

End Class