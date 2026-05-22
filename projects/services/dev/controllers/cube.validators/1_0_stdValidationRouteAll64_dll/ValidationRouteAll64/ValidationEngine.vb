Imports System.ComponentModel
Imports System.Reflection
Imports Cube.ValidatorReport
Imports Rec.Globals.Controllers
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior

Public Class ValidationEngine
    Inherits ValidationRoute

    ' Propiedad predeterminada
    Public Overrides ReadOnly Property route(Optional route_ As IValidationRoute.ValidationRoutes? = Nothing,
                                             Optional type_ As IValidationRoute.ValidationDocuments? = Nothing) As ValidatorReport

        Get

            Dim validationRUVA As IValidationRoute

            If _quality = IValidationRoute.ValidationQuality.Undefined Then

                _quality = IValidationRoute.ValidationQuality.SYNQUALITY

            End If

            Select Case route_

                Case IValidationRoute.ValidationRoutes.RUVA1

                    If _quality = IValidationRoute.ValidationQuality.SYNQUALITY Then

                        validationRUVA = New VRUVA001_SYNA22CustomsDeclarationDoc

                    Else

                        validationRUVA = New VRUVA001_STDA22CustomsDeclarationDoc

                    End If

                Case IValidationRoute.ValidationRoutes.RUVA2

                    If _quality = IValidationRoute.ValidationQuality.SYNQUALITY Then

                        validationRUVA = New VRUVA002_SYNA22CustomsDeclarationDoc

                    Else

                        'validationRUVA = New VRUVA001_STDA22CustomsDeclarationDoc

                    End If

                Case IValidationRoute.ValidationRoutes.RUVA3

                    If _quality = IValidationRoute.ValidationQuality.SYNQUALITY Then

                        validationRUVA = New VRUVA003_SYNA22CustomsDeclarationDoc

                    Else

                        'validationRUVA = New VRUVA001_STDA22CustomsDeclarationDoc

                    End If

                Case IValidationRoute.ValidationRoutes.RUVA4

                    If _quality = IValidationRoute.ValidationQuality.SYNQUALITY Then

                        validationRUVA = New VRUVA004_SYNA22CustomsDeclarationDoc

                    Else

                        validationRUVA = New VRUVA004_STDA22CustomsDeclarationDoc

                    End If

                Case IValidationRoute.ValidationRoutes.RUVA5

                    If _quality = IValidationRoute.ValidationQuality.SYNQUALITY Then

                        validationRUVA = New VRUVA005_SYNA22CustomsDeclarationDoc

                    Else

                        ' validationRUVA = New VRUVA004_STDA22CustomsDeclarationDoc

                    End If

                Case IValidationRoute.ValidationRoutes.RUVA6

                    If _quality = IValidationRoute.ValidationQuality.SYNQUALITY Then

                        validationRUVA = New VRUVA006_SYNA22CustomsDeclarationDoc

                    Else

                        'validationRUVA = New VRUVA006_STDA22CustomsDeclarationDoc

                    End If

                Case IValidationRoute.ValidationRoutes.RUVA7

                    If _quality = IValidationRoute.ValidationQuality.SYNQUALITY Then

                        validationRUVA = New VRUVA007_SYNA22CustomsDeclarationDoc

                    Else

                        'validationRUVA = New VRUVA006_STDA22CustomsDeclarationDoc

                    End If

                Case IValidationRoute.ValidationRoutes.RUVA9

                    If _quality = IValidationRoute.ValidationQuality.SYNQUALITY Then

                        validationRUVA = New VRUVA009_SYNA22CustomsDeclarationDoc

                    Else

                        'validationRUVA = New VRUVA006_STDA22CustomsDeclarationDoc

                    End If

                Case IValidationRoute.ValidationRoutes.RUVA10

                    If _quality = IValidationRoute.ValidationQuality.SYNQUALITY Then

                        validationRUVA = New VRUVA010_SYNA22CustomsDeclarationDoc

                    Else

                        'validationRUVA = New VRUVA006_STDA22CustomsDeclarationDoc

                    End If

                Case IValidationRoute.ValidationRoutes.RUVA11

                    If _quality = IValidationRoute.ValidationQuality.SYNQUALITY Then

                        validationRUVA = New VRUVA011_SYNA22CustomsDeclarationDoc

                    Else

                        'validationRUVA = New VRUVA006_STDA22CustomsDeclarationDoc

                    End If

                Case IValidationRoute.ValidationRoutes.RUVA12

                    If _quality = IValidationRoute.ValidationQuality.SYNQUALITY Then

                        validationRUVA = New VRUVA012_SYNA22CustomsDeclarationDoc

                    Else

                        'validationRUVA = New VRUVA006_STDA22CustomsDeclarationDoc

                    End If

                Case Else

                    Select Case type_

                        Case IValidationRoute.ValidationDocuments.DOVA

                            If _document.TipoDocumentoElectronico = TiposDocumentoElectronico.FacturaComercial Then

                                validationRUVA = New VRUVA000_STDCommercialInvoice

                            Else

                                validationRUVA = New VRUVA000_STDBL

                            End If

                        Case Else

                            If _document.TipoDocumentoElectronico = TiposDocumentoElectronico.PedimentoNormal Then

                                validationRUVA = New VRUVA001_SYNA22CustomsDeclarationDoc

                            Else

                                validationRUVA = New VRUVA000_STDCommercialInvoice

                            End If

                    End Select

            End Select

            Return validationRUVA.ValidateDocument(_document).route

        End Get

    End Property

End Class


