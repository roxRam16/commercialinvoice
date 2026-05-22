Imports System.Collections.Specialized.BitVector32
Imports System.ComponentModel
Imports System.Net.NetworkInformation
Imports System.Reflection
Imports System.Runtime.Remoting.Messaging
Imports System.Text.RegularExpressions
Imports Cube
Imports Cube.ValidatorReport
Imports Cube.Validators
Imports Rec.Globals.Controllers
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Syn.Nucleo.RecursosComercioExterior
Imports Wma.Exceptions

Public MustInherit Class ValidationRoute
    Implements IValidationRoute, IDisposable

#Region "Attributes"

    Protected _validationtarget As IValidationRoute.ValidationRoutes

    Protected _checkfields As List(Of [Enum])

    Protected _document As DocumentoElectronico

    Protected _cube As ICubeController

    Protected _borderfields As Dictionary(Of MultiKeyItem, List(Of CheckedField))

    Protected _fieldvalues As Dictionary(Of String, String)

    Protected _status As TagWatcher

    Protected _report As ValidatorReport

    Protected _validationpanel As validationpanel

    Protected _coincontroller As IControladorMonedas

    Protected _cubeslice As ICubeController.CubeSlices

    Protected _quality As IValidationRoute.ValidationQuality

#End Region


#Region "Properties"

    Public Property validationtarget As IValidationRoute.ValidationRoutes _
                                         Implements IValidationRoute.validationtarget

        Get
            Return _validationtarget

        End Get

        Set(value As IValidationRoute.ValidationRoutes)

            _validationtarget = value

        End Set

    End Property

    Public ReadOnly Property document As DocumentoElectronico _
                                             Implements IValidationRoute.document

        Get

            Return _document

        End Get

    End Property

    Public ReadOnly Property status As TagWatcher _
                                    Implements IValidationRoute.status

        Get

            Return _status

        End Get

    End Property

    Public ReadOnly Property report As ValidatorReport _
                                      Implements IValidationRoute.report

        Get

            Return _report

        End Get

    End Property

    Public ReadOnly Property validationpanel As validationpanel _
                                             Implements IValidationRoute.validationpanel

        Get
            Return _validationpanel

        End Get

    End Property

    Public ReadOnly Property folioperacion As String _
                                             Implements IValidationRoute.folioperacion

        Get

            Return _document.FolioOperacion

        End Get

    End Property
    Public MustOverride ReadOnly Property route(Optional route_ As IValidationRoute.ValidationRoutes? = Nothing,
                                                Optional type_ As IValidationRoute.ValidationDocuments? = Nothing) As ValidatorReport _
                                                               Implements IValidationRoute.route
    Public ReadOnly Property validationstatus As TagWatcher _
                                                Implements IValidationRoute.validationstatus
        Get
            Throw New NotImplementedException()
        End Get
    End Property


    Default Public Property ValidationRoute(quality_ As IValidationRoute.ValidationQuality?) As IValidationRoute _
                                                                                               Implements IValidationRoute.ValidationRoute
        Get

            If quality_ Is Nothing Then

                _quality = IValidationRoute.ValidationQuality.STDQUALITY

            Else

                _quality = quality_

            End If


            Return Me

        End Get

        Set(value As IValidationRoute)



        End Set

    End Property

#End Region

#Region "Builders"

    Sub New()

        _cube = New CubeController

        _cubeslice = ICubeController.CubeSlices.A22

    End Sub

#End Region

#Region "Methods"

    Private Function GetAttributeValue(section_ As [Enum],
                                       field_ As [Enum],
                                       Optional valor_ As Boolean = True) As String

        Dim attribute_ = _document.Seccion(Convert.ToInt32(section_)).Attribute(Convert.ToInt32(field_))

        If attribute_ Is Nothing Then

            Return Nothing

        Else

            If valor_ Then


                If attribute_.Valor Is Nothing Then

                    Return ""

                Else

                    Return attribute_.Valor.ToString

                End If

            Else

                If attribute_.ValorPresentacion Is Nothing Then

                    If attribute_.Valor Is Nothing Then

                        Return ""

                    Else

                        Return attribute_.Valor.ToString

                    End If

                Else

                    Return attribute_.ValorPresentacion.ToString

                End If

            End If

        End If

    End Function

    Private Function GetReports(Of T)() As ValidatorReport

        Return _report

    End Function

    Protected Function GetEnumDescription(ByVal EnumConstant As [Enum]) As String 'Implements GetEnumDescription
        Dim fi As FieldInfo = EnumConstant.GetType().GetField(EnumConstant.ToString())
        Dim attr() As DescriptionAttribute =
                      DirectCast(fi.GetCustomAttributes(GetType(DescriptionAttribute),
                      False), DescriptionAttribute())

        If attr.Length > 0 Then

            Return attr(0).Description

        Else

            Return EnumConstant.ToString()

        End If

    End Function

    Protected Function [Set](sectionSource_ As [Enum],
                                            fieldSource_ As [Enum],
                                            breakOnEmpty_ As Boolean) As Boolean

        Dim found_ As Boolean

        Dim section_ As Int32 = Convert.ToInt32(sectionSource_)

        Dim field_ As Int32 = Convert.ToInt32(fieldSource_)

        Dim Nodo_ = _document.
                            Seccion(section_).
                            Campo(field_)

        Dim value_ As String = If(Nodo_ Is Nothing,
                                 "",
                                 If(Nodo_.Valor Is Nothing,
                                    "",
                                    If(Nodo_.Valor.ToString,
                                       "")))

        found_ = If(value_.ToString = "",
                            False,
                            True)

        Dim sectionfield_ = "S" &
                            section_ &
                            "." &
                            fieldSource_.ToString &
                            ".0"

        _borderfields(New MultiKeyItem With {.fieldpedimento = fieldSource_}) = New List(Of CheckedField) From {New CheckedField With {.value = value_.ToString,
                                                                                           .found = found_,
                                                                                           .breakonempty = breakOnEmpty_,
                                                                                           .roomname = _cubeslice.ToString &
                                                                                                       "." &
                                                                                                       fieldSource_.ToString,
                                                                                           .runat = IValidationRoute.RunAt.LOCALCONDITIONS,
                                                                                           .formulafieldname = sectionfield_,
                                                                                           .requiredfields = Nothing,
                                                                                           .conditions = Nothing,
                                                                                           .dependencies = New List(Of Boolean)
                                                                                              }
                                                                   }

        _fieldvalues(sectionfield_) = value_.ToString

        If breakOnEmpty_ Then

            If Not found_ Then

                _report.SetDetailReport(AdviceTypesReport.Information,
                                     "Validación detenida Falta el campo " & fieldSource_.ToString,
                                     Chr(13) & GetEnumDescription(_validationtarget) & Chr(13) &
                                     "Folio de Operación:" & _document.FolioOperacion & Chr(13),
                                     TriggerSourceTypes.Route
                                     )

            End If

            Return Not found_

        Else

            Return False

        End If

    End Function

    Protected Function [Set](sectionSource_ As [Enum],
                                            fieldSource_ As [Enum],
                                            breakOnEmpty_ As Boolean,
                                            runAt_ As IValidationRoute.RunAt,
                                            Optional roomNameExt_ As String = Nothing,
                                            Optional conditions_ As List(Of String) = Nothing) As Boolean

        Dim found_ As Boolean

        Dim section_ As Int32 = Convert.ToInt32(sectionSource_)

        Dim field_ As Int32 = Convert.ToInt32(fieldSource_)

        'Dim sectionsFather_ As Object = sectionsFatherSource_
        If roomNameExt_ Is Nothing Then

            roomNameExt_ = ""

        End If


        Dim Nodo_ = _document.
                            Seccion(section_).
                            Campo(field_)

        Dim value_ As String = If(Nodo_ Is Nothing,
                                 "",
                                 If(Nodo_.Valor Is Nothing,
                                    "",
                                    If(Nodo_.Valor.ToString,
                                       "")))

        found_ = If(value_.ToString = "",
                            False,
                            True)

        Dim sectionfield_ = "S" &
                                    section_ &
                                    "." &
                                    fieldSource_.ToString &
                                    ".0"

        _borderfields(New MultiKeyItem With {.fieldpedimento = fieldSource_}) = New List(Of CheckedField) From {New CheckedField With {.value = value_.ToString,
                                                                                           .found = found_,
                                                                                           .breakonempty = breakOnEmpty_,
                                                                                           .roomname = _cubeslice.ToString &
                                                                                                       "." &
                                                                                                       fieldSource_.ToString &
                                                                                                       roomNameExt_,
                                                                                           .runat = runAt_,
                                                                                           .formulafieldname = sectionfield_,
                                                                                           .requiredfields = Nothing,
                                                                                           .conditions = conditions_,
                                                                                           .dependencies = New List(Of Boolean)
                                                                                              }
                                                                   }

        _fieldvalues(sectionfield_) = value_.ToString

        If breakOnEmpty_ Then

            If Not found_ Then

                _report.SetDetailReport(AdviceTypesReport.Information,
                                     "Validación detenida Falta el campo " & fieldSource_.ToString,
                                     Chr(13) & GetEnumDescription(_validationtarget) & Chr(13) &
                                     "Folio de Operación:" & _document.FolioOperacion & Chr(13),
                                     TriggerSourceTypes.Route
                                     )

            End If

            Return Not found_

        Else

            Return False

        End If

    End Function

    Protected Function [Set](sectionSource_ As [Enum],
                                            fieldSource_ As [Enum],
                                            breakOnEmpty_ As Boolean,
                                            runat_ As IValidationRoute.RunAt,
                                            recurring_ As Boolean,
                                            Optional roomNameExt_ As String = Nothing,
                                            Optional conditions_ As List(Of String) = Nothing,
                                            Optional sectionsFather_ As List(Of [Enum]) = Nothing) As Boolean


        Dim found_ As Boolean

        Dim section_ As Int32 = Convert.ToInt32(sectionSource_)

        Dim field_ As Int32 = Convert.ToInt32(fieldSource_)

        If roomNameExt_ Is Nothing Then

            roomNameExt_ = ""

        End If

        If sectionsFather_ Is Nothing Then

            Dim sectionrunat_ = _document.Seccion(section_)

            Dim index_ = 0

            If breakOnEmpty_ Then

                found_ = False

            End If

            Dim multikey_ As New MultiKeyItem With {.fieldpedimento = fieldSource_}

            _borderfields(multikey_) = New List(Of CheckedField)

            If sectionrunat_ IsNot Nothing Then

                For Each Nodorunat_ In sectionrunat_.Nodos

                    Dim value_ As String

                    value_ = If(Nodorunat_.Nodos(0).Nodos Is Nothing, "",
                             If(DirectCast(Nodorunat_.Campo(field_), Campo).Valor Is Nothing, "",
                             If(DirectCast(Nodorunat_.Campo(field_), Campo).Valor.ToString, "")))


                    found_ = If(value_.ToString = "", False, True)

                    If Not found_ And breakOnEmpty_ Then

                        Exit For

                    Else

                        Dim sectionfield_ = "S" &
                        section_ &
                        "." &
                        fieldSource_.ToString &
                        "." &
                        index_

                        _borderfields(multikey_).Add(New CheckedField With {.value = value_.ToString,
                                                                            .found = found_,
                                                                            .breakonempty = breakOnEmpty_,
                                                                            .roomname = _cubeslice.ToString &
                                                                                               "." &
                                                                                               fieldSource_.ToString &
                                                                                               roomNameExt_,
                                                                            .runat = runat_,
                                                                            .formulafieldname = sectionfield_,
                                                                            .requiredfields = Nothing,
                                                                            .conditions = conditions_,
                                                                            .dependencies = New List(Of Boolean)
                                                                 })

                        _fieldvalues(sectionfield_) = value_.ToString

                        index_ += 1

                    End If

                Next

                Dim sectionfieldUnless_ = "S" &
                        section_ &
                        "." &
                        fieldSource_.ToString &
                        ".0"

                If Not _fieldvalues.ContainsKey(sectionfieldUnless_) Then

                    _fieldvalues(sectionfieldUnless_) = ""

                End If

            End If

        Else

            Dim sectionDadrunat_ = _document.Seccion(Convert.ToInt32(sectionsFather_(0)))

            Dim dadIndex_ = 0

            For Each NodoFhaterrunat_ In sectionDadrunat_.Nodos

                Dim index_ = 0

                If breakOnEmpty_ Then

                    found_ = False

                End If

                Dim multikey_ As New MultiKeyItem With {.fieldpedimento = fieldSource_, .indexplus = dadIndex_}

                _borderfields(multikey_) = New List(Of CheckedField)

                Dim foundSection_ = NodoFhaterrunat_.Seccion(Convert.ToInt32(sectionSource_))

                If foundSection_ Is Nothing Then

                    found_ = False

                Else

                    For Each Nodorunat_ In foundSection_.Nodos

                        Dim value_ As String = If(Nodorunat_.Nodos(0).Nodos Is Nothing, "",
                                               If(DirectCast(Nodorunat_.Campo(field_), Campo).Valor Is Nothing, "",
                                               If(DirectCast(Nodorunat_.Campo(field_), Campo).Valor.ToString, "")))

                        found_ = If(value_.ToString = "", False, True)

                        If Not found_ And breakOnEmpty_ Then

                            Exit For

                        Else

                            Dim sectionfield_ = "S" &
                                section_ &
                                "." &
                                fieldSource_.ToString &
                                "." &
                                dadIndex_ &
                                 "." &
                                index_

                            _borderfields(multikey_).Add(New CheckedField With {.value = value_.ToString,
                                                                                          .found = found_,
                                                                                          .breakonempty = breakOnEmpty_,
                                                                                          .roomname = _cubeslice.ToString &
                                                                                                       "." &
                                                                                                       fieldSource_.ToString &
                                                                                                      roomNameExt_,
                                                                                          .runat = True,
                                                                                          .formulafieldname = sectionfield_,
                                                                                           .requiredfields = Nothing,
                                                                                           .conditions = Nothing,
                                                                                           .dependencies = New List(Of Boolean)
                                                                         })

                            _fieldvalues(sectionfield_) = value_.ToString

                            index_ += 1

                        End If

                    Next

                    Dim sectionfieldUnless_ = "S" &
                                            section_ &
                                          "." &
                                         fieldSource_.ToString &
                                          ".0.0"

                    If Not _fieldvalues.ContainsKey(sectionfieldUnless_) Then

                        _fieldvalues(sectionfieldUnless_) = ""

                    End If

                End If



                dadIndex_ += 1

            Next


        End If



        If breakOnEmpty_ Then

            If Not found_ Then

                _report.SetDetailReport(AdviceTypesReport.Information,
                                     "Validación detenida Falta el campo " & fieldSource_.ToString,
                                     Chr(13) & GetEnumDescription(_validationtarget) & Chr(13) &
                                     "Folio de Operación:" & _document.FolioOperacion & Chr(13),
                                     TriggerSourceTypes.Route
                                     )

            End If

            Return Not found_

        Else

            Return False

        End If


    End Function

    Protected Function [Set](sectionSource_ As [Enum],
                                            fieldSource_ As [Enum],
                                            breakOnEmpty_ As Boolean,
                                            recurring_ As Boolean,
                                            Optional sectionsFather_ As List(Of [Enum]) = Nothing) As Boolean


        Dim found_ As Boolean

        Dim section_ As Int32 = Convert.ToInt32(sectionSource_)

        Dim field_ As Int32 = Convert.ToInt32(fieldSource_)

        If sectionsFather_ Is Nothing Then

            Dim sectionrunat_ = _document.Seccion(section_)

            Dim index_ = 0

            If breakOnEmpty_ Then

                found_ = False

            End If

            Dim multikey_ As New MultiKeyItem With {.fieldpedimento = fieldSource_}

            _borderfields(multikey_) = New List(Of CheckedField)

            If sectionrunat_ IsNot Nothing Then

                For Each Nodorunat_ In sectionrunat_.Nodos

                    Dim value_ As String

                    value_ = If(Nodorunat_.Nodos(0).Nodos Is Nothing, "",
                             If(DirectCast(Nodorunat_.Campo(field_), Campo).Valor Is Nothing, "",
                             If(DirectCast(Nodorunat_.Campo(field_), Campo).Valor.ToString, "")))


                    found_ = If(value_.ToString = "", False, True)

                    If Not found_ And breakOnEmpty_ Then

                        Exit For

                    Else

                        Dim sectionfield_ = "S" &
                        section_ &
                        "." &
                        fieldSource_.ToString &
                        "." &
                        index_

                        _borderfields(multikey_).Add(New CheckedField With {.value = value_.ToString,
                                                                            .found = found_,
                                                                            .breakonempty = breakOnEmpty_,
                                                                            .roomname = _cubeslice.ToString &
                                                                                               "." &
                                                                                               fieldSource_.ToString,
                                                                            .runat = IValidationRoute.RunAt.LOCALCONDITIONS,
                                                                            .formulafieldname = sectionfield_,
                                                                            .requiredfields = Nothing,
                                                                            .conditions = Nothing,
                                                                            .dependencies = New List(Of Boolean)
                                                                 })

                        _fieldvalues(sectionfield_) = value_.ToString

                        index_ += 1

                    End If

                Next

            End If

        Else

            Dim sectionDadrunat_ = _document.Seccion(Convert.ToInt32(sectionsFather_(0)))

            Dim dadIndex_ = 0

            For Each NodoFhaterrunat_ In sectionDadrunat_.Nodos

                Dim index_ = 0

                If breakOnEmpty_ Then

                    found_ = False

                End If

                Dim multikey_ As New MultiKeyItem With {.fieldpedimento = fieldSource_, .indexplus = dadIndex_}



                Dim foundSection_ = NodoFhaterrunat_.Seccion(Convert.ToInt32(sectionSource_))

                If foundSection_ Is Nothing Then

                    found_ = False

                Else

                    _borderfields(multikey_) = New List(Of CheckedField)

                    For Each Nodorunat_ In foundSection_.Nodos

                        Dim value_ As String = If(Nodorunat_.Nodos(0).Nodos Is Nothing, "",
                                               If(DirectCast(Nodorunat_.Campo(field_), Campo).Valor Is Nothing, "",
                                               If(DirectCast(Nodorunat_.Campo(field_), Campo).Valor.ToString, "")))

                        found_ = If(value_.ToString = "", False, True)

                        If Not found_ And breakOnEmpty_ Then

                            Exit For

                        Else

                            Dim sectionfield_ = "S" &
                                section_ &
                                "." &
                                fieldSource_.ToString &
                                "." &
                                dadIndex_ &
                                 "." &
                                index_
                            _borderfields(multikey_).Add(New CheckedField With {.value = value_.ToString,
                                                                                          .found = found_,
                                                                                          .breakonempty = breakOnEmpty_,
                                                                                          .roomname = _cubeslice.ToString &
                                                                                                       "." &
                                                                                                       fieldSource_.ToString,
                                                                                          .runat = IValidationRoute.RunAt.LOCALCONDITIONS,
                                                                                          .formulafieldname = sectionfield_,
                                                                                           .requiredfields = Nothing,
                                                                                           .conditions = Nothing,
                                                                                           .dependencies = New List(Of Boolean)
                                                                         })

                            _fieldvalues(sectionfield_) = value_.ToString

                            index_ += 1

                        End If

                    Next

                End If




                dadIndex_ += 1

            Next


        End If



        If breakOnEmpty_ Then

            If Not found_ Then

                _report.SetDetailReport(AdviceTypesReport.Information,
                                     "Validación detenida Falta el campo " & fieldSource_.ToString,
                                     Chr(13) & GetEnumDescription(_validationtarget) & Chr(13) &
                                     "Folio de Operación:" & _document.FolioOperacion & Chr(13),
                                     TriggerSourceTypes.Route
                                     )

            End If

            Return Not found_

        Else

            Return False

        End If


    End Function

    Protected Function [Set](sectionSource_ As [Enum],
                                                 fieldSource_ As [Enum],
                                                 breakOnEmpty_ As Boolean,
                                                 runat_ As IValidationRoute.RunAt,
                                                 dateCurrent_ As Date,
                                                 Optional roomNameExt_ As String = "",
                                                 Optional conditions_ As List(Of String) = Nothing) As Boolean

        Dim found_ As Boolean

        Dim section_ As Int32 = Convert.ToInt32(sectionSource_)

        Dim field_ As Int32 = Convert.ToInt32(fieldSource_)

        Dim Nodo_ = _document.
                            Seccion(section_).
                            Campo(field_)

        Dim value_ As String = If(Nodo_ Is Nothing,
                                  "",
                                  If(Nodo_.Valor Is Nothing,
                                     "",
                                      If(Nodo_.Valor.ToString,
                                  "")))

        If value_ <> "" Then

            value_ = Date.
                         Parse(value_).
                         ToString("yyyy-MM-dd")

        End If

        found_ = If(value_.ToString = "",
                            False,
                            True)

        Dim sectionfield_ = "S" &
                                    section_ &
                                    "." &
                                    fieldSource_.ToString &
                                    ".0"

        _borderfields(New MultiKeyItem With {.fieldpedimento = fieldSource_}) = New List(Of CheckedField) From {New CheckedField With {.value = value_.ToString,
                                                                                           .found = found_,
                                                                                           .breakonempty = breakOnEmpty_,
                                                                                           .roomname = _cubeslice.ToString &
                                                                                                       "." &
                                                                                                       fieldSource_.ToString &
                                                                                                       roomNameExt_,
                                                                                           .runat = runat_,
                                                                                           .formulafieldname = sectionfield_,
                                                                                           .requiredfields = Nothing,
                                                                                           .conditions = conditions_,
                                                                                           .dependencies = New List(Of Boolean)
                                                                                              }
                                                                   }

        _fieldvalues(sectionfield_) = value_.ToString



        If breakOnEmpty_ Then

            If Not found_ Then

                _report.SetDetailReport(AdviceTypesReport.Information,
                                     "Validación detenida Falta el campo " & fieldSource_.ToString,
                                     Chr(13) & GetEnumDescription(_validationtarget) & Chr(13) &
                                     "Folio de Operación:" & _document.FolioOperacion & Chr(13),
                                     TriggerSourceTypes.Route
                                     )

            End If

            Return Not found_

        Else

            Return False

        End If


    End Function
    Protected Function [Set](sectionSource_ As [Enum],
                                            fieldSource_ As [Enum],
                                            breakOnEmpty_ As Boolean,
                                            runat_ As IValidationRoute.RunAt,
                                            recurring_ As Boolean,
                                            dateCurrent_ As Date,
                                            Optional roomNameExt_ As String = "",
                                            Optional conditions_ As List(Of String) = Nothing,
                                            Optional sectionsFather_ As List(Of [Enum]) = Nothing) As Boolean

        Dim found_ As Boolean

        Dim section_ As Int32 = Convert.ToInt32(sectionSource_)

        Dim field_ As Int32 = Convert.ToInt32(fieldSource_)

        'Dim sectionsFather_ As Object = sectionsFatherSource_

        If sectionsFather_ Is Nothing Then

            Dim sectionrunat_ = _document.Seccion(section_)

            Dim index_ = 0

            If breakOnEmpty_ Then

                found_ = False

            End If

            Dim multikey_ As New MultiKeyItem With {.fieldpedimento = fieldSource_}

            _borderfields(multikey_) = New List(Of CheckedField)

            If sectionrunat_ IsNot Nothing Then

                For Each Nodorunat_ In sectionrunat_.Nodos

                    Dim value_ As String = If(Nodorunat_.Nodos(0).Nodos Is Nothing, "",
                                                      If(DirectCast(Nodorunat_.Campo(field_), Campo).Valor Is Nothing,
                                                         "",
                                                         If(DirectCast(Nodorunat_.Campo(field_), Campo).Valor.ToString,
                                                            "")))

                    If value_ <> "" Then

                        value_ = Date.Parse(value_).ToString("yyyy-MM-dd")

                    End If

                    found_ = If(value_.ToString = "", False, True)

                    If Not found_ And breakOnEmpty_ Then

                        Exit For

                    Else

                        Dim sectionfield_ = "S" &
                                                 section_ &
                                                 "." &
                                                 fieldSource_.ToString &
                                                 "." &
                                                 index_

                        _borderfields(multikey_).Add(New CheckedField With {.value = value_.ToString,
                                                                                    .found = found_,
                                                                                    .breakonempty = breakOnEmpty_,
                                                                                    .roomname = _cubeslice.ToString &
                                                                                                       "." &
                                                                                                       fieldSource_.ToString &
                                                                                                      roomNameExt_,
                                                                                    .runat = runat_,
                                                                                    .formulafieldname = sectionfield_,
                                                                                    .requiredfields = Nothing,
                                                                                    .conditions = conditions_,
                                                                                    .dependencies = New List(Of Boolean)
                                                                         })

                        _fieldvalues(sectionfield_) = value_.ToString

                        index_ += 1

                    End If

                Next

            End If

        Else

            Dim sectionDadrunat_ = _document.Seccion(Convert.ToInt32(sectionsFather_(0)))

            Dim dadIndex_ = 0

            For Each NodoFhaterrunat_ In sectionDadrunat_.Nodos

                Dim index_ = 0

                If breakOnEmpty_ Then

                    found_ = False

                End If

                Dim multikey_ As New MultiKeyItem With {.fieldpedimento = fieldSource_, .indexplus = dadIndex_}


                Dim foundSection_ = NodoFhaterrunat_.Seccion(Convert.ToInt32(sectionSource_))

                If foundSection_ Is Nothing Then

                    found_ = False

                Else

                    _borderfields(multikey_) = New List(Of CheckedField)

                    For Each Nodorunat_ In foundSection_.Nodos

                        Dim value_ As String = If(Nodorunat_.Nodos(0).Nodos Is Nothing, "",
                                                     If(DirectCast(Nodorunat_.Campo(field_), Campo).Valor Is Nothing, "",
                                                     If(DirectCast(Nodorunat_.Campo(field_), Campo).Valor.ToString, "")))

                        If value_ <> "" Then

                            value_ = Date.Parse(value_).ToString("yyyy-MM-dd")

                        End If

                        found_ = If(value_.ToString = "", False, True)

                        If Not found_ And breakOnEmpty_ Then

                            Exit For

                        Else

                            Dim sectionfield_ = "S" &
                                section_ &
                                "." &
                                fieldSource_.ToString &
                                "." &
                                dadIndex_ &
                                 "." &
                                index_

                            _borderfields(multikey_).Add(New CheckedField With {.value = value_.ToString,
                                                                                          .found = found_,
                                                                                          .breakonempty = breakOnEmpty_,
                                                                                          .roomname = _cubeslice.ToString &
                                                                                                       "." &
                                                                                                       fieldSource_.ToString &
                                                                                                      roomNameExt_,
                                                                                          .runat = runat_,
                                                                                          .formulafieldname = sectionfield_,
                                                                                           .requiredfields = Nothing,
                                                                                           .conditions = conditions_,
                                                                                           .dependencies = New List(Of Boolean)
                                                                         })

                            _fieldvalues(sectionfield_) = value_.ToString

                            index_ += 1

                        End If

                    Next

                End If




                dadIndex_ += 1

            Next


        End If

        If breakOnEmpty_ Then

            If Not found_ Then

                _report.SetDetailReport(AdviceTypesReport.Information,
                                     "Validación detenida Falta el campo " & fieldSource_.ToString,
                                     Chr(13) & GetEnumDescription(_validationtarget) & Chr(13) &
                                     "Folio de Operación:" & _document.FolioOperacion & Chr(13),
                                     TriggerSourceTypes.Route
                                     )

            End If

            Return Not found_

        Else

            Return False

        End If

    End Function
    Protected Function [Set](sectionSource_ As [Enum],
                                                fieldSource_ As [Enum],
                                                breakOnEmpty_ As Boolean,
                                                runat_ As IValidationRoute.RunAt,
                                                isPresentationValue_ As Boolean,
                                                isReverse_ As Boolean,
                                                Optional length_ As Int32? = Nothing,
                                                Optional roomNameExt_ As String = "",
                                                Optional conditions_ As List(Of String) = Nothing) As Boolean

        Dim found_ As Boolean

        Dim section_ As Int32 = Convert.ToInt32(sectionSource_)

        Dim field_ As Int32 = Convert.ToInt32(fieldSource_)

        Dim Nodo_ = _document.
                     Seccion(section_).
                     Campo(field_)

        Dim value_ As String


        value_ = If(Nodo_ Is Nothing,
                             "",
                             If(Nodo_.ValorPresentacion Is Nothing,
                                "",
                                If(Nodo_.ValorPresentacion.ToString,
                                   "")))

        If length_ IsNot Nothing Then

            If isReverse_ Then

                value_ = value_.Substring(value_.Length - length_,
                                                  length_)

            Else

                value_ = value_.Substring(0,
                                                  length_)

            End If

        End If

        found_ = If(value_.ToString = "",
                            False,
                            True)

        Dim sectionfield_ = "S" &
                                    section_ &
                                    "." &
                                    fieldSource_.ToString &
                                    ".0"

        _borderfields(New MultiKeyItem With {.fieldpedimento = fieldSource_}) = New List(Of CheckedField) From {New CheckedField With {.value = value_.ToString,
                                                                                   .found = found_,
                                                                                   .breakonempty = breakOnEmpty_,
                                                                                   .roomname = _cubeslice.ToString &
                                                                                               "." &
                                                                                               fieldSource_.ToString &
                                                                                               roomNameExt_,
                                                                                   .runat = runat_,
                                                                                   .formulafieldname = sectionfield_,
                                                                                   .requiredfields = Nothing,
                                                                                   .conditions = conditions_,
                                                                                   .dependencies = New List(Of Boolean)
                                                                                      }
                                                           }

        _fieldvalues(sectionfield_) = value_.ToString

        If breakOnEmpty_ Then

            If Not found_ Then

                _report.SetDetailReport(AdviceTypesReport.Information,
                                     "Validación detenida Falta el campo " & fieldSource_.ToString,
                                     Chr(13) & GetEnumDescription(_validationtarget) & Chr(13) &
                                     "Folio de Operación:" & _document.FolioOperacion & Chr(13),
                                     TriggerSourceTypes.Route
                                     )

            End If

            Return Not found_

        Else

            Return False

        End If

    End Function
    Protected Function [Set](sectionSource_ As [Enum],
                                                 fieldSource_ As [Enum],
                                                 breakOnEmpty_ As Boolean,
                                                 runat_ As IValidationRoute.RunAt,
                                                 isPresentationValue_ As Boolean,
                                                 isReverse_ As Boolean,
                                                 recurring_ As Boolean,
                                                 Optional length_ As Int32? = Nothing,
                                                 Optional roomNameExt_ As String = "",
                                                 Optional requiredfields_ As List(Of String) = Nothing,
                                                 Optional conditions_ As List(Of String) = Nothing,
                                                 Optional sectionsFather_ As List(Of [Enum]) = Nothing) As Boolean

        Dim found_ As Boolean

        Dim section_ As Int32 = Convert.ToInt32(sectionSource_)

        Dim field_ As Int32 = Convert.ToInt32(fieldSource_)

        If sectionsFather_ Is Nothing Then

            Dim sectionrunat_ = _document.Seccion(section_)

            Dim index_ = 0

            If breakOnEmpty_ Then

                found_ = False

            End If

            Dim multikey_ As New MultiKeyItem With {.fieldpedimento = fieldSource_}

            _borderfields(multikey_) = New List(Of CheckedField)

            If sectionrunat_ IsNot Nothing Then

                For Each Nodorunat_ In sectionrunat_.Nodos

                    Dim value_ As String = If(Nodorunat_.Nodos(0).Nodos Is Nothing,
                                              "",
                                              If(DirectCast(Nodorunat_.Campo(field_), Campo).ValorPresentacion Is Nothing,
                                                 "",
                                                 If(DirectCast(Nodorunat_.Campo(field_), Campo).ValorPresentacion.ToString,
                                                  "")))


                    If length_ IsNot Nothing Then

                        If isReverse_ Then

                            value_ = value_.Substring(value_.Length - length_, length_)

                        Else

                            value_ = value_.Substring(0, length_)

                        End If

                    End If

                    found_ = If(value_.ToString = "", False, True)

                    If Not found_ And breakOnEmpty_ Then

                        Exit For

                    Else

                        Dim sectionfield_ = "S" &
                                            section_ &
                                            "." &
                                            fieldSource_.ToString &
                                            "." &
                                            index_

                        _borderfields(multikey_).Add(New CheckedField With {.value = value_.ToString,
                                                                            .found = found_,
                                                                            .breakonempty = breakOnEmpty_,
                                                                            .roomname = _cubeslice.ToString &
                                                                                               "." &
                                                                                               fieldSource_.ToString &
                                                                                              roomNameExt_,
                                                                            .runat = runat_,
                                                                            .formulafieldname = sectionfield_,
                                                                            .requiredfields = requiredfields_,
                                                                            .conditions = conditions_,
                                                                            .dependencies = New List(Of Boolean)
                                                                 })

                        _fieldvalues(sectionfield_) = value_.ToString

                        index_ += 1

                    End If

                Next

            End If

        Else

            Dim sectionDadrunat_ = _document.Seccion(Convert.ToInt32(sectionsFather_(0)))

            Dim dadIndex_ = 0

            For Each NodoFhaterrunat_ In sectionDadrunat_.Nodos

                Dim index_ = 0

                If breakOnEmpty_ Then

                    found_ = False

                End If

                Dim multikey_ As New MultiKeyItem With {.fieldpedimento = fieldSource_, .indexplus = dadIndex_}

                Dim foundSection_ = NodoFhaterrunat_.Seccion(Convert.ToInt32(sectionSource_))

                If foundSection_ Is Nothing Then

                    found_ = False

                Else

                    _borderfields(multikey_) = New List(Of CheckedField)

                    For Each Nodorunat_ In foundSection_.Nodos

                        Dim value_ As String = If(Nodorunat_.Nodos(0).Nodos Is Nothing, "",
                         If(DirectCast(Nodorunat_.Campo(field_), Campo).ValorPresentacion Is Nothing, "",
                         If(DirectCast(Nodorunat_.Campo(field_), Campo).ValorPresentacion.ToString, "")))

                        If length_ IsNot Nothing Then

                            If isReverse_ Then

                                value_ = value_.Substring(value_.Length - length_, length_)

                            Else

                                value_ = value_.Substring(0, length_)

                            End If

                        End If

                        found_ = If(value_.ToString = "", False, True)

                        If Not found_ And breakOnEmpty_ Then

                            Exit For

                        Else

                            Dim sectionfield_ = "S" &
                                section_ &
                                "." &
                                fieldSource_.ToString &
                                "." &
                                dadIndex_ &
                                "." &
                                index_
                            _borderfields(multikey_).Add(New CheckedField With {.value = value_.ToString,
                                                                                          .found = found_,
                                                                                          .breakonempty = breakOnEmpty_,
                                                                                          .roomname = _cubeslice.ToString &
                                                                                                       "." &
                                                                                                       fieldSource_.ToString &
                                                                                                      roomNameExt_,
                                                                                          .runat = runat_,
                                                                                          .formulafieldname = sectionfield_,
                                                                                           .requiredfields = requiredfields_,
                                                                                           .conditions = conditions_,
                                                                                           .dependencies = New List(Of Boolean)
                                                                         })

                            _fieldvalues(sectionfield_) = value_.ToString

                            index_ += 1

                        End If

                    Next

                End If


                dadIndex_ += 1

            Next


        End If

        If breakOnEmpty_ Then

            If Not found_ Then

                _report.SetDetailReport(AdviceTypesReport.Information,
                                     "Validación detenida Falta el campo " & fieldSource_.ToString,
                                     Chr(13) & GetEnumDescription(_validationtarget) & Chr(13) &
                                     "Folio de Operación:" & _document.FolioOperacion & Chr(13),
                                     TriggerSourceTypes.Route
                                     )

            End If

            Return Not found_

        Else

            Return False

        End If


    End Function
    Protected Sub [Set](external_ As List(Of String))

        Dim sectionfield_ = If(external_(0).IndexOf(".") <> -1, external_(0).ToString, external_(0).ToString &
            ".0")

        _fieldvalues(sectionfield_) = external_(1)

    End Sub
    Protected Function [Set](sectionSource_ As [Enum],
                                                 fieldSource_ As [Enum],
                                                 breakOnEmpty_ As Boolean,
                                                 runat_ As IValidationRoute.RunAt,
                                                 meanBoolean_ As List(Of String),
                                                 recurring_ As Boolean,
                                                 Optional valorPresentacion_ As Boolean = False,
                                                 Optional length_ As Int32 = -1,
                                                 Optional reverse_ As Boolean = False,
                                                 Optional dateType_ As Boolean = False,
                                                 Optional roomNameExt_ As String = "",
                                                 Optional fullSection_ As Boolean = False,
                                                 Optional conditions_ As List(Of String) = Nothing,
                                                 Optional sectionsFather_ As List(Of [Enum]) = Nothing) As Boolean

        Dim found_ As Boolean

        Dim section_ As Int32 = Convert.ToInt32(sectionSource_)

        Dim field_ As Int32 = Convert.ToInt32(fieldSource_)

        'Dim sectionsFather_ As Object = sectionsFatherSource_

        If fullSection_ Then

            If sectionsFather_ Is Nothing Then

                Dim sectionrunat_ = _document.Seccion(section_)

                Dim index_ = 0

                If breakOnEmpty_ Then

                    found_ = False

                End If

                Dim multikey_ As New MultiKeyItem With {.fieldpedimento = fieldSource_}

                _borderfields(multikey_) = New List(Of CheckedField)

                If sectionrunat_ IsNot Nothing Then

                    For Each Nodorunat_ In sectionrunat_.Nodos

                        Dim value_ As String

                        If valorPresentacion_ Then

                            value_ = If(Nodorunat_.Nodos(0).Nodos Is Nothing, "",
                             If(DirectCast(Nodorunat_.Campo(field_), Campo).ValorPresentacion Is Nothing, "",
                             If(DirectCast(Nodorunat_.Campo(field_), Campo).ValorPresentacion.ToString, "")))

                            If length_ <> -1 Then

                                If length_ <> -1 Then

                                    If reverse_ Then

                                        value_ = value_.Substring(value_.Length - length_, length_)

                                    Else

                                        value_ = value_.Substring(0, length_)

                                    End If

                                End If

                            End If

                        Else

                            value_ = If(Nodorunat_.Nodos(0).Nodos Is Nothing, "",
                             If(DirectCast(Nodorunat_.Campo(field_), Campo).Valor Is Nothing, "",
                             If(DirectCast(Nodorunat_.Campo(field_), Campo).Valor.ToString, "")))

                            If length_ <> -1 Then

                                If reverse_ Then

                                    value_ = value_.Substring(value_.Length - length_, length_)

                                Else

                                    value_ = value_.Substring(0, length_)

                                End If

                            Else

                                If dateType_ Then

                                    If value_ <> "" Then

                                        value_ = Date.Parse(value_).ToString("yyyy-MM-dd")

                                    End If

                                End If

                            End If

                        End If

                        found_ = If(value_.ToString = "", False, True)

                        If Not found_ And breakOnEmpty_ Then

                            Exit For

                        Else

                            Dim sectionfield_ = "S" &
                            section_ &
                            "." &
                            fieldSource_.ToString &
                            "." &
                            index_

                            If meanBoolean_ IsNot Nothing Then

                                If value_.ToUpper = "TRUE" Then

                                    value_ = meanBoolean_(0)

                                Else

                                    value_ = meanBoolean_(1)

                                End If

                            End If

                            _borderfields(multikey_).Add(New CheckedField With {.value = value_.ToString,
                                                                                .found = found_,
                                                                                .breakonempty = breakOnEmpty_,
                                                                                .roomname = _cubeslice.ToString &
                                                                                                   "." &
                                                                                                   fieldSource_.ToString &
                                                                                                  roomNameExt_,
                                                                                .runat = runat_,
                                                                                .formulafieldname = sectionfield_,
                                                                                .requiredfields = Nothing,
                                                                                .conditions = conditions_,
                                                                                .dependencies = New List(Of Boolean)
                                                                     })

                            _fieldvalues(sectionfield_) = value_.ToString

                            index_ += 1

                        End If

                    Next

                End If

            Else

                Dim sectionDadrunat_ = _document.Seccion(Convert.ToInt32(sectionsFather_(0)))

                Dim dadIndex_ = 0

                For Each NodoFhaterrunat_ In sectionDadrunat_.Nodos

                    Dim index_ = 0

                    If breakOnEmpty_ Then

                        found_ = False

                    End If

                    Dim multikey_ As New MultiKeyItem With {.fieldpedimento = fieldSource_, .indexplus = dadIndex_}

                    Dim foundSection_ = NodoFhaterrunat_.Seccion(Convert.ToInt32(sectionSource_))

                    If foundSection_ Is Nothing Then

                        found_ = False

                    Else

                        _borderfields(multikey_) = New List(Of CheckedField)

                        For Each Nodorunat_ In NodoFhaterrunat_.Nodos

                            Dim value_ As String

                            If valorPresentacion_ Then

                                value_ = If(Nodorunat_.Nodos(0).Nodos Is Nothing, "",
                             If(DirectCast(Nodorunat_.Campo(field_), Campo).ValorPresentacion Is Nothing, "",
                             If(DirectCast(Nodorunat_.Campo(field_), Campo).ValorPresentacion.ToString, "")))

                                If length_ <> -1 Then

                                    If reverse_ Then

                                        value_ = value_.Substring(value_.Length - length_, length_)

                                    Else

                                        value_ = value_.Substring(0, length_)

                                    End If

                                End If

                            Else

                                value_ = If(Nodorunat_.Nodos(0).Nodos Is Nothing, "",
                             If(DirectCast(Nodorunat_.Campo(field_), Campo).Valor Is Nothing, "",
                             If(DirectCast(Nodorunat_.Campo(field_), Campo).Valor.ToString, "")))

                                If length_ <> -1 Then

                                    If reverse_ Then

                                        value_ = value_.Substring(value_.Length - length_, length_)

                                    Else

                                        value_ = value_.Substring(0, length_)

                                    End If

                                Else


                                    If dateType_ Then

                                        If value_ <> "" Then

                                            value_ = Date.Parse(value_).ToString("yyyy-MM-dd")

                                        End If

                                    End If

                                End If

                            End If

                            found_ = If(value_.ToString = "", False, True)

                            If Not found_ And breakOnEmpty_ Then

                                Exit For

                            Else

                                Dim sectionfield_ = "S" &
                            section_ &
                            "." &
                            fieldSource_.ToString &
                            "." &
                            dadIndex_ &
                             "." &
                            index_

                                If meanBoolean_ IsNot Nothing Then

                                    If value_.ToUpper = "TRUE" Then

                                        value_ = meanBoolean_(0)

                                    Else

                                        value_ = meanBoolean_(1)

                                    End If

                                End If

                                _borderfields(multikey_).Add(New CheckedField With {.value = value_.ToString,
                                                                                      .found = found_,
                                                                                      .breakonempty = breakOnEmpty_,
                                                                                      .roomname = _cubeslice.ToString &
                                                                                                   "." &
                                                                                                   fieldSource_.ToString &
                                                                                                  roomNameExt_,
                                                                                      .runat = runat_,
                                                                                      .formulafieldname = sectionfield_,
                                                                                       .requiredfields = Nothing,
                                                                                       .conditions = conditions_,
                                                                                       .dependencies = New List(Of Boolean)
                                                                     })

                                _fieldvalues(sectionfield_) = value_.ToString

                                index_ += 1

                            End If

                        Next

                    End If


                    dadIndex_ += 1

                Next


            End If

        Else

            Dim Nodo_ = _document.
                            Seccion(section_).
                            Campo(field_)

            Dim value_ As String

            If valorPresentacion_ Then

                value_ = If(Nodo_ Is Nothing,
                             "",
                             If(Nodo_.ValorPresentacion Is Nothing,
                                "",
                                If(Nodo_.ValorPresentacion.ToString,
                                   "")))

                If length_ <> -1 Then

                    If reverse_ Then

                        value_ = value_.Substring(value_.Length - length_,
                                                      length_)

                    Else

                        value_ = value_.Substring(0,
                                                      length_)

                    End If

                End If

            Else

                value_ = If(Nodo_ Is Nothing,
                                "",
                                If(Nodo_.Valor Is Nothing,
                                   "",
                                   If(Nodo_.Valor.ToString,
                                      "")))

                If length_ <> -1 Then


                    If reverse_ Then

                        value_ = value_.Substring(value_.Length - length_,
                                                      length_)

                    Else

                        value_ = value_.Substring(0,
                                                      length_)

                    End If

                Else

                    If dateType_ Then

                        If value_ <> "" Then

                            value_ = Date.
                                         Parse(value_).
                                         ToString("yyyy-MM-dd")

                        End If

                    End If

                End If

            End If

            found_ = If(value_.ToString = "",
                            False,
                            True)

            Dim sectionfield_ = "S" &
                                    section_ &
                                    "." &
                                    fieldSource_.ToString &
                                    ".0"

            If meanBoolean_ IsNot Nothing Then

                If value_.ToUpper = "TRUE" Then

                    value_ = meanBoolean_(0)

                Else

                    value_ = meanBoolean_(1)

                End If

            End If

            _borderfields(New MultiKeyItem With {.fieldpedimento = fieldSource_}) = New List(Of CheckedField) From {New CheckedField With {.value = value_.ToString,
                                                                                           .found = found_,
                                                                                           .breakonempty = breakOnEmpty_,
                                                                                           .roomname = _cubeslice.ToString &
                                                                                                       "." &
                                                                                                       fieldSource_.ToString &
                                                                                                       roomNameExt_,
                                                                                           .runat = runat_,
                                                                                           .formulafieldname = sectionfield_,
                                                                                           .requiredfields = Nothing,
                                                                                           .conditions = conditions_,
                                                                                           .dependencies = New List(Of Boolean)
                                                                                              }
                                                                   }

            _fieldvalues(sectionfield_) = value_.ToString

        End If

        If breakOnEmpty_ Then

            If Not found_ Then

                _report.SetDetailReport(AdviceTypesReport.Information,
                                     "Validación detenida Falta el campo " & fieldSource_.ToString,
                                     Chr(13) & GetEnumDescription(_validationtarget) & Chr(13) &
                                     "Folio de Operación:" & _document.FolioOperacion & Chr(13),
                                     TriggerSourceTypes.Route
                                     )

            End If

            Return Not found_

        Else

            Return False

        End If


    End Function


    'Protected Function SetValueStockElement(sectionSource_ As [Enum],
    '                                        fieldSource_ As [Enum],
    '                                        breakOnEmpty_ As Boolean,
    '                                        Optional external_ As List(Of String) = Nothing,
    '                                        Optional valorPresentacion_ As Boolean = False,
    '                                        Optional length_ As Int32 = -1,
    '                                        Optional reverse_ As Boolean = False,
    '                                        Optional dateType_ As Boolean = False,
    '                                        Optional roomNameExt_ As String = "",
    '                                        Optional runat_ As Boolean = True,
    '                                        Optional fullSection_ As Boolean = False,
    '                                        Optional requiredfields_ As List(Of String) = Nothing,
    '                                        Optional conditions_ As List(Of String) = Nothing,
    '                                        Optional dependencies_ As List(Of Boolean) = Nothing,
    '                                        Optional sectionsFather_ As List(Of [Enum]) = Nothing,
    '                                        Optional meanBoolean As List(Of String) = Nothing) As Boolean

    '    Dim found_ As Boolean

    '    Dim section_ As Int32 = Convert.ToInt32(sectionSource_)

    '    Dim field_ As Int32 = Convert.ToInt32(fieldSource_)

    '    'Dim sectionsFather_ As Object = sectionsFatherSource_

    '    If dependencies_ Is Nothing Then

    '        dependencies_ = New List(Of Boolean)

    '    End If

    '    If external_ IsNot Nothing Then

    '        Dim sectionfield_ = If(external_(0).IndexOf(".") <> -1, external_(0).ToString, external_(0).ToString &
    '            ".0")

    '        _fieldvalues(sectionfield_) = external_(1)

    '        found_ = True

    '    Else

    '        If fullSection_ Then

    '            If sectionsFather_ Is Nothing Then

    '                Dim sectionrunat_ = _document.Seccion(section_)

    '                Dim index_ = 0

    '                If breakOnEmpty_ Then

    '                    found_ = False

    '                End If

    '                Dim multikey_ As New MultiKeyItem With {.fieldpedimento = fieldSource_}

    '                _borderfields(multikey_) = New List(Of CheckedField)

    '                If sectionrunat_ IsNot Nothing Then

    '                    For Each Nodorunat_ In sectionrunat_.Nodos

    '                        Dim value_ As String

    '                        If valorPresentacion_ Then

    '                            value_ = If(Nodorunat_.Nodos(0).Nodos Is Nothing, "",
    '                             If(DirectCast(Nodorunat_.Campo(field_), Campo).ValorPresentacion Is Nothing, "",
    '                             If(DirectCast(Nodorunat_.Campo(field_), Campo).ValorPresentacion.ToString, "")))

    '                            If length_ <> -1 Then

    '                                If length_ <> -1 Then

    '                                    If reverse_ Then

    '                                        value_ = value_.Substring(value_.Length - length_, length_)

    '                                    Else

    '                                        value_ = value_.Substring(0, length_)

    '                                    End If

    '                                End If

    '                            End If

    '                        Else

    '                            value_ = If(Nodorunat_.Nodos(0).Nodos Is Nothing, "",
    '                             If(DirectCast(Nodorunat_.Campo(field_), Campo).Valor Is Nothing, "",
    '                             If(DirectCast(Nodorunat_.Campo(field_), Campo).Valor.ToString, "")))

    '                            If length_ <> -1 Then

    '                                If reverse_ Then

    '                                    value_ = value_.Substring(value_.Length - length_, length_)

    '                                Else

    '                                    value_ = value_.Substring(0, length_)

    '                                End If

    '                            Else

    '                                If dateType_ Then

    '                                    If value_ <> "" Then

    '                                        value_ = Date.Parse(value_).ToString("yyyy-MM-dd")

    '                                    End If

    '                                End If

    '                            End If

    '                        End If

    '                        found_ = If(value_.ToString = "", False, True)

    '                        If Not found_ And breakOnEmpty_ Then

    '                            Exit For

    '                        Else

    '                            Dim sectionfield_ = "S" &
    '                            section_ &
    '                            "." &
    '                            fieldSource_.ToString &
    '                            "." &
    '                            index_

    '                            If meanBoolean IsNot Nothing Then

    '                                If value_.ToUpper = "TRUE" Then

    '                                    value_ = meanBoolean(0)

    '                                Else

    '                                    value_ = meanBoolean(1)

    '                                End If

    '                            End If

    '                            _borderfields(multikey_).Add(New CheckedField With {.value = value_.ToString,
    '                                                                                .found = found_,
    '                                                                                .breakOnEmpty_ = breakOnEmpty_,
    '                                                                                .roomname = _cubeslice.ToString &
    '                                                                                                   "." &
    '                                                                                                   fieldSource_.ToString &
    '                                                                                                  roomNameExt_,
    '                                                                                .runat = runat_,
    '                                                                                .formulafieldname = sectionfield_,
    '                                                                                .requiredfields = requiredfields_,
    '                                                                                .conditions = conditions_,
    '                                                                                .dependencies = dependencies_
    '                                                                     })

    '                            _fieldvalues(sectionfield_) = value_.ToString

    '                            index_ += 1

    '                        End If

    '                    Next

    '                End If

    '            Else

    '                Dim sectionDadrunat_ = _document.Seccion(Convert.ToInt32(sectionsFather_(0)))

    '                Dim dadIndex_ = 0

    '                For Each NodoFhaterrunat_ In sectionDadrunat_.Nodos

    '                    Dim index_ = 0

    '                    If breakOnEmpty_ Then

    '                        found_ = False

    '                    End If

    '                    Dim multikey_ As New MultiKeyItem With {.fieldpedimento = fieldSource_, .indexplus = dadIndex_}

    '                    _borderfields(multikey_) = New List(Of CheckedField)

    '                    For Each Nodorunat_ In NodoFhaterrunat_.Nodos

    '                        Dim value_ As String

    '                        If valorPresentacion_ Then

    '                            value_ = If(Nodorunat_.Nodos(0).Nodos Is Nothing, "",
    '                             If(DirectCast(Nodorunat_.Campo(field_), Campo).ValorPresentacion Is Nothing, "",
    '                             If(DirectCast(Nodorunat_.Campo(field_), Campo).ValorPresentacion.ToString, "")))

    '                            If length_ <> -1 Then

    '                                If reverse_ Then

    '                                    value_ = value_.Substring(value_.Length - length_, length_)

    '                                Else

    '                                    value_ = value_.Substring(0, length_)

    '                                End If

    '                            End If

    '                        Else

    '                            value_ = If(Nodorunat_.Nodos(0).Nodos Is Nothing, "",
    '                             If(DirectCast(Nodorunat_.Campo(field_), Campo).Valor Is Nothing, "",
    '                             If(DirectCast(Nodorunat_.Campo(field_), Campo).Valor.ToString, "")))

    '                            If length_ <> -1 Then

    '                                If reverse_ Then

    '                                    value_ = value_.Substring(value_.Length - length_, length_)

    '                                Else

    '                                    value_ = value_.Substring(0, length_)

    '                                End If

    '                            Else


    '                                If dateType_ Then

    '                                    If value_ <> "" Then

    '                                        value_ = Date.Parse(value_).ToString("yyyy-MM-dd")

    '                                    End If

    '                                End If

    '                            End If

    '                        End If

    '                        found_ = If(value_.ToString = "", False, True)

    '                        If Not found_ And breakOnEmpty_ Then

    '                            Exit For

    '                        Else

    '                            Dim sectionfield_ = "S" &
    '                            section_ &
    '                            "." &
    '                            fieldSource_.ToString &
    '                            "." &
    '                            index_

    '                            If meanBoolean IsNot Nothing Then

    '                                If value_.ToUpper = "TRUE" Then

    '                                    value_ = meanBoolean(0)

    '                                Else

    '                                    value_ = meanBoolean(1)

    '                                End If

    '                            End If

    '                            _borderfields(multikey_).Add(New CheckedField With {.value = value_.ToString,
    '                                                                                      .found = found_,
    '                                                                                      .breakOnEmpty_ = breakOnEmpty_,
    '                                                                                      .roomname = _cubeslice.ToString &
    '                                                                                                   "." &
    '                                                                                                   fieldSource_.ToString &
    '                                                                                                  roomNameExt_,
    '                                                                                      .runat = runat_,
    '                                                                                      .formulafieldname = sectionfield_,
    '                                                                                       .requiredfields = requiredfields_,
    '                                                                                       .conditions = conditions_,
    '                                                                                       .dependencies = dependencies_
    '                                                                     })

    '                            _fieldvalues(sectionfield_ &
    '                                         "." &
    '                                         dadIndex_) = value_.ToString

    '                            index_ += 1

    '                        End If

    '                    Next

    '                    dadIndex_ += 1

    '                Next


    '            End If

    '        Else

    '            Dim Nodo_ = _document.
    '                        Seccion(section_).
    '                        Campo(field_)

    '            Dim value_ As String

    '            If valorPresentacion_ Then

    '                value_ = If(Nodo_ Is Nothing,
    '                         "",
    '                         If(Nodo_.ValorPresentacion Is Nothing,
    '                            "",
    '                            If(Nodo_.ValorPresentacion.ToString,
    '                               "")))

    '                If length_ <> -1 Then

    '                    If reverse_ Then

    '                        value_ = value_.Substring(value_.Length - length_,
    '                                                  length_)

    '                    Else

    '                        value_ = value_.Substring(0,
    '                                                  length_)

    '                    End If

    '                End If

    '            Else

    '                value_ = If(Nodo_ Is Nothing,
    '                            "",
    '                            If(Nodo_.Valor Is Nothing,
    '                               "",
    '                               If(Nodo_.Valor.ToString,
    '                                  "")))

    '                If length_ <> -1 Then


    '                    If reverse_ Then

    '                        value_ = value_.Substring(value_.Length - length_,
    '                                                  length_)

    '                    Else

    '                        value_ = value_.Substring(0,
    '                                                  length_)

    '                    End If

    '                Else

    '                    If dateType_ Then

    '                        If value_ <> "" Then

    '                            value_ = Date.
    '                                     Parse(value_).
    '                                     ToString("yyyy-MM-dd")

    '                        End If

    '                    End If

    '                End If

    '            End If

    '            found_ = If(value_.ToString = "",
    '                        False,
    '                        True)

    '            Dim sectionfield_ = "S" &
    '                                section_ &
    '                                "." &
    '                                fieldSource_.ToString &
    '                                ".0"

    '            If meanBoolean IsNot Nothing Then

    '                If value_.ToUpper = "TRUE" Then

    '                    value_ = meanBoolean(0)

    '                Else

    '                    value_ = meanBoolean(1)

    '                End If

    '            End If

    '            _borderfields(New MultiKeyItem With {.fieldpedimento = fieldSource_}) = New List(Of CheckedField) From {New CheckedField With {.value = value_.ToString,
    '                                                                                       .found = found_,
    '                                                                                       .breakOnEmpty_ = breakOnEmpty_,
    '                                                                                       .roomname = _cubeslice.ToString &
    '                                                                                                   "." &
    '                                                                                                   fieldSource_.ToString &
    '                                                                                                   roomNameExt_,
    '                                                                                       .runat = runat_,
    '                                                                                       .formulafieldname = sectionfield_,
    '                                                                                       .requiredfields = requiredfields_,
    '                                                                                       .conditions = conditions_,
    '                                                                                       .dependencies = dependencies_
    '                                                                                          }
    '                                                               }

    '            _fieldvalues(sectionfield_) = value_.ToString

    '        End If

    '    End If

    '    If breakOnEmpty_ Then

    '        If Not found_ Then

    '            _report.SetDetailReport(AdviceTypesReport.Information,
    '                                 "Validación detenida Falta el campo " & fieldSource_.ToString,
    '                                 Chr(13) & GetEnumDescription(_validationtarget) & Chr(13) &
    '                                 "Folio de Operación:" & _document.FolioOperacion & Chr(13),
    '                                 TriggerSourceTypes.Route
    '                                 )

    '        End If

    '        Return Not found_

    '    Else

    '        Return False

    '    End If


    'End Function

    Private Function ObtenerIndice(fieldName_ As String) As String

        If Regex.IsMatch(fieldName_, "\.+.+.") Or Regex.IsMatch(fieldName_, "\.+.") Then

            Dim positionPoint_ = fieldName_.IndexOf(".")

            Dim subFieldName_ = fieldName_.Substring(positionPoint_ + 1)

            positionPoint_ = subFieldName_.IndexOf(".")

            Return subFieldName_.Substring(positionPoint_)

        Else

            Return fieldName_.Substring(fieldName_.IndexOf("."))

        End If

    End Function
    Protected Function ValidateStockElement(ByRef message_ As String) As Boolean

        Dim validation_ As ValidatorReport

        Dim useNoticed_ As Int32 = 0

        Dim dependencia_ As New List(Of Boolean)

        For Each key_ In _borderfields.Keys

            If dependencia_.Count = 0 Then

                Dim index_ = 0

                For Each element_ In _borderfields(key_)

                    If element_.runat = IValidationRoute.RunAt.LOCALCONDITIONS Then

                        'If key_.fieldpedimento.ToString = CamposPedimento.CA_EFECTIVO.ToString Then

                        '    Dim algo_ = 4

                        'End If


                        If element_.conditions Is Nothing Then

                            'If _fieldvalues(element_.formulafieldname) = "" Then

                            '    message_ &= Chr(13) &
                            '            "Falta Especificar el valor del campo " &
                            '             element_.formulafieldname

                            'End If

                        Else

                            Select Case element_.conditions(0)

                                Case "="

                                    If _fieldvalues(element_.formulafieldname) <> element_.conditions(1) Then

                                        dependencia_ = element_.dependencies

                                        message_ &= Chr(13) &
                                            "El valor del campo '" &
                                            element_.formulafieldname &
                                            "'(" &
                                            _fieldvalues(element_.formulafieldname) &
                                            ") debe ser igual a " &
                                            element_.conditions(1)

                                    End If

                                Case "<>", "!="

                                    If _fieldvalues(element_.formulafieldname) = element_.conditions(1) Then

                                        dependencia_ = element_.dependencies

                                        message_ &= Chr(13) &
                                            "El valor del campo '" &
                                            element_.formulafieldname &
                                            "'(" &
                                            _fieldvalues(element_.formulafieldname) &
                                            ") debe ser diferente de " &
                                            element_.conditions(1)

                                    End If

                                Case ">="

                                    Dim doubleParse_ As Double

                                    If Double.TryParse(element_.conditions(1), doubleParse_) Then

                                        If _fieldvalues(element_.formulafieldname) < doubleParse_ Then

                                            dependencia_ = element_.dependencies

                                            message_ &= Chr(13) &
                                                "El valor del campo '" &
                                                element_.formulafieldname &
                                                "'(" &
                                                _fieldvalues(element_.formulafieldname) &
                                                ") debe ser mayor o igual a " &
                                                element_.conditions(1)

                                        End If

                                    Else

                                        If _fieldvalues(element_.formulafieldname) < element_.conditions(1) Then

                                            dependencia_ = element_.dependencies

                                            message_ &= Chr(13) &
                                            "El valor del campo '" &
                                            element_.formulafieldname &
                                            "'(" &
                                            _fieldvalues(element_.formulafieldname) &
                                            ") debe ser mayor o igual a " &
                                            element_.conditions(2)

                                        End If

                                    End If



                                Case "<="

                                    Dim doubleParse_ As Double

                                    If Double.TryParse(element_.conditions(1), doubleParse_) Then

                                        If _fieldvalues(element_.formulafieldname) > doubleParse_ Then

                                            dependencia_ = element_.dependencies

                                            message_ &= Chr(13) &
                                                "El valor del campo '" &
                                                element_.formulafieldname &
                                                "'(" &
                                                _fieldvalues(element_.formulafieldname) &
                                                ") debe ser menor o igual a " &
                                                element_.conditions(1)

                                        End If

                                    Else

                                        If _fieldvalues(element_.formulafieldname) > element_.conditions(1) Then

                                            dependencia_ = element_.dependencies

                                            message_ &= Chr(13) &
                                                "El valor del campo '" &
                                                element_.formulafieldname &
                                                "'(" &
                                                _fieldvalues(element_.formulafieldname) &
                                                ") debe ser menor o igual a " &
                                                element_.conditions(1)

                                        End If

                                    End If

                                Case ">"

                                    Dim doubleParse_ As Double

                                    If Double.TryParse(element_.conditions(1), doubleParse_) Then

                                        If _fieldvalues(element_.formulafieldname) <= doubleParse_ Then

                                            dependencia_ = element_.dependencies

                                            message_ &= Chr(13) &
                                                "El valor del campo '" &
                                                element_.formulafieldname &
                                                "'(" &
                                                _fieldvalues(element_.formulafieldname) &
                                                ") debe ser mayor a " &
                                                element_.conditions(1)

                                        End If

                                    Else

                                        If _fieldvalues(element_.formulafieldname) <= element_.conditions(1) Then

                                            dependencia_ = element_.dependencies

                                            message_ &= Chr(13) &
                                            "El valor del campo '" &
                                            element_.formulafieldname &
                                            "'(" &
                                            _fieldvalues(element_.formulafieldname) &
                                            ") debe ser mayor a " &
                                            element_.conditions(2)

                                        End If

                                    End If

                                Case "<"

                                    Dim doubleParse_ As Double

                                    If Double.TryParse(element_.conditions(1), doubleParse_) Then

                                        If _fieldvalues(element_.formulafieldname) >= doubleParse_ Then

                                            dependencia_ = element_.dependencies

                                            message_ &= Chr(13) &
                                                "El valor del campo '" &
                                                element_.formulafieldname &
                                                "'(" &
                                                _fieldvalues(element_.formulafieldname) &
                                                ") debe ser menor a " &
                                                element_.conditions(1)

                                        End If

                                    Else

                                        If _fieldvalues(element_.formulafieldname) >= element_.conditions(1) Then

                                            dependencia_ = element_.dependencies

                                            message_ &= Chr(13) &
                                                "El valor del campo '" &
                                                element_.formulafieldname &
                                                "'(" &
                                                _fieldvalues(element_.formulafieldname) &
                                                ") debe ser menor a " &
                                                element_.conditions(1)

                                        End If

                                    End If


                            End Select

                        End If

                    Else

                        'If key_.fieldpedimento.ToString = CamposPedimento.CA_EFECTIVO.ToString Then

                        '    Dim algo_ = 4

                        'End If

                        If element_.roomname = "A22.CA_CVE_IDENTIFICADOR_PARTIDA" Then

                            Dim checa_ = 4


                        End If

                        Dim formulaFieldNameIndex_ As String = Nothing

                        If Regex.IsMatch(element_.formulafieldname, ".*\.\d\.\d$") Then

                            formulaFieldNameIndex_ = element_.formulafieldname

                        Else


                        End If


                        validation_ = _cube.
                                      RunRoom(Of String)(element_.roomname,
                                                         _fieldvalues,
                                                         useType_:=ICubeController.UseType.VALIDATION,
                                                         requieredfields_:=element_.requiredfields,
                                                         preferIndex_:=index_,
                                                        formulaFieldName_:=formulaFieldNameIndex_)

                        If validation_.result IsNot Nothing Then

                            If validation_.result.Count > 0 Then

                                If validation_.result(0) = "OK" Then

                                    If _fieldvalues(element_.formulafieldname) = "" Then

                                        useNoticed_ = 0

                                    Else

                                        useNoticed_ = 1

                                    End If

                                Else

                                    Dim positionCurrent_ = ObtenerIndice(element_.formulafieldname)

                                    dependencia_ = element_.dependencies

                                    _validationpanel = New validationpanel

                                    Dim messagePanel_ = ""

                                    If validation_.messages Is Nothing Then

                                        messagePanel_ = Chr(13) &
                                       "Falta Especificar el valor del campo " &
                                       key_.fieldpedimento.ToString

                                        message_ &= messagePanel_

                                    Else

                                        If validation_.messages(0) <> "" Then

                                            messagePanel_ = Chr(13) & validation_.messages(0)

                                            For Each breakOnEmpty_field_ In element_.requiredfields




                                                '_fieldvalues(breakOnEmpty_field_ & "." & index_),
                                                '            _fieldvalues(breakOnEmpty_field_ & ".0")) Then

                                                messagePanel_ = messagePanel_.
                                                Replace("$" & breakOnEmpty_field_,
                                                       If(_fieldvalues.ContainsKey(breakOnEmpty_field_ & positionCurrent_),
                                                       _fieldvalues(breakOnEmpty_field_ & positionCurrent_),
                                                        _fieldvalues(breakOnEmpty_field_ & ".0")))


                                            Next

                                            message_ &= messagePanel_

                                        Else

                                            Dim position_ = 0

                                            If element_.requiredfields Is Nothing Then

                                                messagePanel_ = Chr(13) &
                                                                                       "Error en el campo " &
                                                                                       element_.formulafieldname &
                                                                                       "Valor Inválido '" &
                                                                                        _fieldvalues(element_.formulafieldname) &
                                                                                       "'"

                                                message_ &= messagePanel_

                                            Else

                                                messagePanel_ = Chr(13) &
                                                                                       "Error en el campo " &
                                                                                       element_.formulafieldname &
                                                                                       " Valor Inválido '" &
                                                                                        _fieldvalues(element_.formulafieldname) &
                                                                                       "' para la relación:" &
                                                                                          Chr(13)

                                                message_ &= messagePanel_

                                                For Each breakOnEmpty_Field_ In element_.requiredfields

                                                    'Dim patron_ As String = "\.\d+\.\d+$"

                                                    'Dim patron2_ As String = "\." & index_ & "$"


                                                    'Dim fieldwoth_ As List(Of String) = _fieldvalues.Keys.Where(Function(e) e.Contains(breakOnEmpty_Field_) And Regex.IsMatch(e, patron_) And Regex.IsMatch(e, patron2_)).ToList

                                                    'Dim algo_ = 3

                                                    'Dim fieldFound_ As String = breakOnEmpty_Field_ & "." & index_

                                                    'If breakOnEmpty_Field_.Contains("CA_CVE_IDENTIFICADOR_PARTIDA") Then



                                                    'End If

                                                    'If fieldwoth_.Count > 0 Then

                                                    '    fieldFound_ = fieldwoth_.Where(Function(e) e.Contains(breakOnEmpty_Field_) And Regex.IsMatch(e, patron_) And Regex.IsMatch(e, patron2_)).ToList

                                                    'End If

                                                    Dim breakOnEmpty_FieldValue_ = If(_fieldvalues.ContainsKey(breakOnEmpty_Field_ & positionCurrent_),
                                                                             _fieldvalues(breakOnEmpty_Field_ & positionCurrent_),
                                                                             If(_fieldvalues.ContainsKey(breakOnEmpty_Field_ & ".0"), _fieldvalues(breakOnEmpty_Field_ & ".0"), ""))

                                                    If element_.errormessages Is Nothing Then

                                                        messagePanel_ =
                                                        breakOnEmpty_Field_ &
                                                        "'" &
                                                        breakOnEmpty_FieldValue_ &
                                                        "'-"
                                                        message_ &= messagePanel_

                                                    Else

                                                        element_.errormessages(0) = element_.errormessages(0).Replace("MSG" & position_, breakOnEmpty_FieldValue_).Replace("$" &
                                                                                                                                                                                          breakOnEmpty_Field_,
                                                                                                                                                                                           breakOnEmpty_FieldValue_)

                                                        element_.errormessages(1) = element_.errormessages(1).Replace("MSG" & position_, breakOnEmpty_FieldValue_).Replace("$" &
                                                                                                                                                                                          breakOnEmpty_Field_,
                                                                                                                                                                                           breakOnEmpty_FieldValue_)

                                                    End If

                                                    position_ += 1

                                                Next

                                                If element_.requiredfields.Count > 0 Then

                                                    message_ = message_.Substring(0, message_.Length - 1)

                                                End If


                                            End If

                                            If element_.errormessages IsNot Nothing Then

                                                If _fieldvalues(element_.requiredfields(0)) = "" Then

                                                    messagePanel_ = element_.errormessages(0)

                                                    message_ &= messagePanel_

                                                    useNoticed_ = 0

                                                Else

                                                    useNoticed_ = 1

                                                    messagePanel_ = element_.errormessages(1)

                                                    message_ &= messagePanel_

                                                End If

                                            End If


                                        End If

                                    End If

                                    'Dim field_ = element_.formulafieldname.Substring(element_.formulafieldname.IndexOf(".") + 1)

                                    validationpanel.SetValidationPanel(key_.fieldpedimento,
                                                           _validationtarget,
                                                           "",
                                                           Nothing,
                                                           "originalfourier",
                                                           validationpanel.PanelError.Alert,
                                                           1,
                                                           1,
                                                           "TRAFICO",
                                                           _fieldvalues(element_.formulafieldname),
                                                            messagePanel_, 1)
                                    '_cubedatos.set

                                End If

                            End If

                        Else

                            Exit For


                        End If



                    End If

                        index_ += 1

                Next


            Else

                dependencia_.RemoveAt(0)

            End If

        Next

        Return False

    End Function

    Public Function validateDocument(pedimento_ As DocumentoElectronico) As IValidationRoute _
                                          Implements IValidationRoute.ValidateDocument

        _document = pedimento_

        Return Me

    End Function

    Public Function validate(Of T)(pedimento_ As DocumentoElectronico,
                                          route_ As IValidationRoute.ValidationRoutes) As TagWatcher _
                                          Implements IValidationRoute.Validate

        Dim diccionario_ As New Dictionary(Of String, String)

        For campo_ = 1 To 15000

            Dim camposPedimento_ As CamposPedimento

            If campo_ = 1017 Then

                Dim algopasa_ = 1000

            End If

            If [Enum].TryParse(Of CamposPedimento)(campo_, camposPedimento_) Then

                For seccion_ = 1 To 100

                    Dim nodo_ = If(pedimento_.Seccion(seccion_) Is Nothing, Nothing, pedimento_.Seccion(seccion_).Campo(camposPedimento_))

                    If nodo_ IsNot Nothing Then

                        If diccionario_.ContainsKey(camposPedimento_.ToString) Then

                            diccionario_(camposPedimento_.ToString) = diccionario_(camposPedimento_.ToString) & ",S" & seccion_

                        Else

                            diccionario_(camposPedimento_.ToString) = "S" & seccion_

                        End If

                    End If

                Next

            Else

                Dim CamposVOCE_ As CamposVOCE

                If [Enum].TryParse(Of CamposPedimento)(campo_, CamposVOCE_) Then

                    For seccion_ = 1 To 100

                        Dim nodo_ = If(pedimento_.Seccion(seccion_) Is Nothing, Nothing, pedimento_.Seccion(seccion_).Campo(CamposVOCE_))

                        If nodo_ IsNot Nothing Then

                            If diccionario_.ContainsKey(CamposVOCE_.ToString) Then

                                diccionario_(CamposVOCE_.ToString) = diccionario_(CamposVOCE_.ToString) & ",S" & seccion_

                            Else

                                diccionario_(CamposVOCE_.ToString) = "S" & seccion_

                            End If

                        End If

                    Next

                Else

                    Dim CamposReferencia_ As CamposReferencia

                    If [Enum].TryParse(Of CamposPedimento)(campo_, CamposReferencia_) Then

                        For seccion_ = 1 To 100

                            Dim nodo_ = If(pedimento_.Seccion(seccion_) Is Nothing, Nothing, pedimento_.Seccion(seccion_).Campo(CamposReferencia_))

                            If nodo_ IsNot Nothing Then

                                If diccionario_.ContainsKey(CamposReferencia_.ToString) Then

                                    diccionario_(CamposReferencia_.ToString) = diccionario_(CamposReferencia_.ToString) & ",S" & seccion_

                                Else

                                    diccionario_(CamposReferencia_.ToString) = "S" & seccion_

                                End If

                            End If

                        Next

                    Else

                        Dim CamposFacturaComercial_ As CamposFacturaComercial

                        If [Enum].TryParse(Of CamposPedimento)(campo_, CamposFacturaComercial_) Then

                            For seccion_ = 1 To 100

                                Dim nodo_ = If(pedimento_.Seccion(seccion_) Is Nothing, Nothing, pedimento_.Seccion(seccion_).Campo(CamposFacturaComercial_))

                                If nodo_ IsNot Nothing Then

                                    If diccionario_.ContainsKey(CamposFacturaComercial_.ToString) Then

                                        diccionario_(CamposFacturaComercial_.ToString) = diccionario_(CamposFacturaComercial_.ToString) & ",S" & seccion_

                                    Else

                                        diccionario_(CamposFacturaComercial_.ToString) = "S" & seccion_

                                    End If

                                End If

                            Next

                        Else

                            Dim CamposAcuseValor_ As CamposAcuseValor

                            If [Enum].TryParse(Of CamposPedimento)(campo_, CamposAcuseValor_) Then

                                For seccion_ = 1 To 100

                                    Dim nodo_ = If(pedimento_.Seccion(seccion_) Is Nothing, Nothing, pedimento_.Seccion(seccion_).Campo(CamposAcuseValor_))

                                    If nodo_ IsNot Nothing Then

                                        If diccionario_.ContainsKey(CamposAcuseValor_.ToString) Then

                                            diccionario_(CamposAcuseValor_.ToString) = diccionario_(CamposAcuseValor_.ToString) & ",S" & seccion_

                                        Else

                                            diccionario_(CamposAcuseValor_.ToString) = "S" & seccion_

                                        End If

                                    End If

                                Next

                            Else

                                Dim CamposProveedorOperativo_ As CamposProveedorOperativo

                                If [Enum].TryParse(Of CamposPedimento)(campo_, CamposProveedorOperativo_) Then

                                    For seccion_ = 1 To 100

                                        Dim nodo_ = If(pedimento_.Seccion(seccion_) Is Nothing, Nothing, pedimento_.Seccion(seccion_).Campo(CamposProveedorOperativo_))

                                        If nodo_ IsNot Nothing Then

                                            If diccionario_.ContainsKey(CamposProveedorOperativo_.ToString) Then

                                                diccionario_(CamposProveedorOperativo_.ToString) = diccionario_(CamposProveedorOperativo_.ToString) & ",S" & seccion_

                                            Else

                                                diccionario_(CamposProveedorOperativo_.ToString) = "S" & seccion_

                                            End If

                                        End If

                                    Next

                                Else

                                    Dim CamposDestinatario_ As CamposDestinatario

                                    If [Enum].TryParse(Of CamposPedimento)(campo_, CamposDestinatario_) Then

                                        For seccion_ = 1 To 100

                                            Dim nodo_ = If(pedimento_.Seccion(seccion_) Is Nothing, Nothing, pedimento_.Seccion(seccion_).Campo(CamposDestinatario_))

                                            If nodo_ IsNot Nothing Then

                                                If diccionario_.ContainsKey(CamposDestinatario_.ToString) Then

                                                    diccionario_(CamposDestinatario_.ToString) = diccionario_(CamposDestinatario_.ToString) & ",S" & seccion_

                                                Else

                                                    diccionario_(CamposDestinatario_.ToString) = "S" & seccion_

                                                End If

                                            End If

                                        Next

                                    Else

                                        Dim CamposRevalidacion_ As CamposRevalidacion

                                        If [Enum].TryParse(Of CamposPedimento)(campo_, CamposRevalidacion_) Then

                                            For seccion_ = 1 To 100

                                                Dim nodo_ = If(pedimento_.Seccion(seccion_) Is Nothing, Nothing, pedimento_.Seccion(seccion_).Campo(CamposRevalidacion_))

                                                If nodo_ IsNot Nothing Then

                                                    If diccionario_.ContainsKey(CamposRevalidacion_.ToString) Then

                                                        diccionario_(CamposRevalidacion_.ToString) = diccionario_(CamposRevalidacion_.ToString) & ",S" & seccion_

                                                    Else

                                                        diccionario_(CamposRevalidacion_.ToString) = "S" & seccion_

                                                    End If

                                                End If

                                            Next

                                        Else

                                            Dim CamposViajes_ As CamposViajes

                                            If [Enum].TryParse(Of CamposPedimento)(campo_, CamposViajes_) Then

                                                For seccion_ = 1 To 100

                                                    Dim nodo_ = If(pedimento_.Seccion(seccion_) Is Nothing, Nothing, pedimento_.Seccion(seccion_).Campo(CamposViajes_))

                                                    If nodo_ IsNot Nothing Then

                                                        If diccionario_.ContainsKey(CamposViajes_.ToString) Then

                                                            diccionario_(CamposViajes_.ToString) = diccionario_(CamposViajes_.ToString) & ",S" & seccion_

                                                        Else

                                                            diccionario_(CamposViajes_.ToString) = "S" & seccion_

                                                        End If

                                                    End If

                                                Next

                                            Else

                                                Dim CamposProducto_ As CamposProducto

                                                If [Enum].TryParse(Of CamposPedimento)(campo_, CamposProducto_) Then

                                                    For seccion_ = 1 To 100

                                                        Dim nodo_ = If(pedimento_.Seccion(seccion_) Is Nothing, Nothing, pedimento_.Seccion(seccion_).Campo(CamposProducto_))

                                                        If nodo_ IsNot Nothing Then

                                                            If diccionario_.ContainsKey(CamposProducto_.ToString) Then

                                                                diccionario_(CamposProducto_.ToString) = diccionario_(CamposProducto_.ToString) & ",S" & seccion_

                                                            Else

                                                                diccionario_(CamposProducto_.ToString) = "S" & seccion_

                                                            End If

                                                        End If

                                                    Next

                                                Else

                                                    Dim CamposTarifaArancelaria_ As CamposTarifaArancelaria

                                                    If [Enum].TryParse(Of CamposPedimento)(campo_, CamposTarifaArancelaria_) Then

                                                        For seccion_ = 1 To 100

                                                            Dim nodo_ = If(pedimento_.Seccion(seccion_) Is Nothing, Nothing, pedimento_.Seccion(seccion_).Campo(CamposTarifaArancelaria_))

                                                            If nodo_ IsNot Nothing Then

                                                                If diccionario_.ContainsKey(CamposTarifaArancelaria_.ToString) Then

                                                                    diccionario_(CamposTarifaArancelaria_.ToString) = diccionario_(CamposTarifaArancelaria_.ToString) & ",S" & seccion_

                                                                Else

                                                                    diccionario_(CamposTarifaArancelaria_.ToString) = "S" & seccion_

                                                                End If

                                                            End If

                                                        Next

                                                    Else

                                                        Dim CamposManifestacionValor_ As CamposManifestacionValor

                                                        If [Enum].TryParse(Of CamposPedimento)(campo_, CamposManifestacionValor_) Then

                                                            For seccion_ = 1 To 100

                                                                Dim nodo_ = If(pedimento_.Seccion(seccion_) Is Nothing, Nothing, pedimento_.Seccion(seccion_).Campo(CamposManifestacionValor_))

                                                                If nodo_ IsNot Nothing Then

                                                                    If diccionario_.ContainsKey(CamposManifestacionValor_.ToString) Then

                                                                        diccionario_(CamposManifestacionValor_.ToString) = diccionario_(CamposManifestacionValor_.ToString) & ",S" & seccion_

                                                                    Else

                                                                        diccionario_(CamposManifestacionValor_.ToString) = "S" & seccion_

                                                                    End If

                                                                End If

                                                            Next

                                                        Else

                                                            Dim CamposProcesamientoElectDocumentos_ As CamposProcesamientoElectDocumentos

                                                            If [Enum].TryParse(Of CamposPedimento)(campo_, CamposProcesamientoElectDocumentos_) Then

                                                                For seccion_ = 1 To 100

                                                                    Dim nodo_ = If(pedimento_.Seccion(seccion_) Is Nothing, Nothing, pedimento_.Seccion(seccion_).Campo(CamposProcesamientoElectDocumentos_))

                                                                    If nodo_ IsNot Nothing Then

                                                                        If diccionario_.ContainsKey(CamposProcesamientoElectDocumentos_.ToString) Then

                                                                            diccionario_(CamposProcesamientoElectDocumentos_.ToString) = diccionario_(CamposProcesamientoElectDocumentos_.ToString) & ",S" & seccion_

                                                                        Else

                                                                            diccionario_(CamposProcesamientoElectDocumentos_.ToString) = "S" & seccion_

                                                                        End If

                                                                    End If

                                                                Next

                                                            Else


                                                            End If

                                                        End If

                                                    End If

                                                End If

                                            End If

                                        End If

                                    End If

                                End If

                            End If

                        End If

                    End If



                End If

            End If





        Next



        _status = New TagWatcher With {.ObjectReturned = diccionario_}

        'Select Case route_

        '    Case IValidationRoute.ValidationRoutes.RUVA1

        '        Return New TagWatcher With {.ObjectReturned = runatRoute1(pedimento_)}

        '    Case IValidationRoute.ValidationRoutes.RUVA4

        '        Return New TagWatcher With {.ObjectReturned = runatRoute4(pedimento_)}

        '    Case IValidationRoute.ValidationRoutes.RUVA21


        '        Return New TagWatcher With {.ObjectReturned = runatRoute21(pedimento_)}

        '    Case Else

        '        Return New TagWatcher With {.ObjectReturned = runatRoute21(pedimento_)}

        'End Select

        Return _status

    End Function

    Public Function ObtenerNombreCampo(nodo_ As Nodo) As String

        If nodo_.DescripcionTipoNodo = Nodo.TiposNodo.Campo.ToString Then



        End If


        Return ""

    End Function





#Region "IDisposable Support"

    Private disposedValue As Boolean ' Para detectar llamadas redundantes

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)

        If Not Me.disposedValue Then

            If disposing Then

                ' TODO: eliminar estado administrado (objetos administrados).

            End If

            'PONES LAS PROPIEDADES DE TU CLASE EN VACÏO

            With Me

                ._coincontroller.Dispose()

                ._document.Dispose()

                ._report.Dispose()

                ._validationpanel = Nothing

                ._status = Nothing

                ._borderfields = Nothing

            End With

            ' TODO: liberar recursos no administrados (objetos no administrados) e invalidar Finalize() below.
            ' TODO: Establecer campos grandes como Null.
        End If

        Me.disposedValue = True

    End Sub

    ' TODO: invalidar Finalize() sólo si la instrucción Dispose(ByVal disposing As Boolean) anterior tiene código para liberar recursos no administrados.
    'Protected Overrides Sub Finalize()
    '    ' No cambie este código. Ponga el código de limpieza en la instrucción Dispose(ByVal disposing As Boolean) anterior.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' Visual Basic agregó este código para implementar correctamente el modelo descartable.
    Public Sub Dispose() Implements IDisposable.Dispose

        ' ESTO SERÏA LO QUE PONDRÏAS EN TU DISPOSE PERSONALIZADO

        Dispose(True)

        GC.SuppressFinalize(Me)

    End Sub

#End Region

#End Region

End Class




