Imports Cube
Imports Cube.ValidatorReport
Imports Cube.Validators
Imports Syn.Documento

Public MustInherit Class ValidationRUVA
    Inherits ValidationRoute


    Public Overrides ReadOnly Property route(Optional route_ As IValidationRoute.ValidationRoutes? = Nothing,
                                             Optional type_ As IValidationRoute.ValidationDocuments? = Nothing) As ValidatorReport

        Get

            Dim elementMessage_ As New Dictionary(Of String, String)

            Dim message_ = ""

            Dim validation_ As New ValidatorReport

            Dim useNoticed_ = -1

            _report = New ValidatorReport

            _borderfields = New Dictionary(Of MultiKeyItem, List(Of CheckedField))

            _fieldvalues = New Dictionary(Of String, String)

            validation_ = LoadInitialStockElement()

            If validation_.details Is Nothing Then

                ValidateStockElement(message_)

                If message_ = "" Then

                    message_ = "FELICIDADES SIN ERRORES"

                End If



                validation_.SetDetailReport(AdviceTypesReport.Information,
                                         message_,
                                         Chr(13) & GetEnumDescription(validationtarget) & Chr(13) &
                                         "Folio de Operación:" & _document.FolioOperacion & Chr(13),
                                         TriggerSourceTypes.Route
                                         )


            Else


            End If


            Return validation_


        End Get

    End Property

    Protected MustOverride Function LoadInitialStockElement() As ValidatorReport


End Class

