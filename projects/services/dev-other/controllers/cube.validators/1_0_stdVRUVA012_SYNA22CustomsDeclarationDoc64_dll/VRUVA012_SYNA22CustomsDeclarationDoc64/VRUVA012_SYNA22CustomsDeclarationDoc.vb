Imports Rec.Globals.Controllers
Imports Syn.Nucleo.RecursosComercioExterior.SeccionesPedimento
Imports Syn.Nucleo.RecursosComercioExterior.CamposPedimento
Imports Wma.Exceptions.TagWatcher
Imports Cube.Validators.IValidationRoute.RunAt
Imports Cube.ValidatorReport
Imports Syn.Nucleo.RecursosComercioExterior
Public Class VRUVA012_SYNA22CustomsDeclarationDoc
    Inherits ValidationRUVA

    Sub New()

        _validationtarget = IValidationRoute.ValidationRoutes.RUVA12

        _cubeslice = ICubeController.CubeSlices.A22

        _quality = IValidationRoute.ValidationQuality.SYNQUALITY

    End Sub


    Protected Overrides Function LoadInitialStockElement() As ValidatorReport

        '#1
        If [Set](ANS1, CA_DESTINO_ORIGEN, breakOnEmpty_:=True) Then Return _report

        '#2
        If [Set](ANS1, CA_ADUANA_ENTRADA_SALIDA, breakOnEmpty_:=True) Then Return _report

        '#3
        If [Set](ANS1, CA_MEDIO_TRANSPORTE, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {"=", "98"}) Then Return _report

        '#4
        [Set](ANS1, CA_PESO_BRUTO, breakOnEmpty_:=False, runAt_:=ROOMCUBE, roomNameExt_:="_CASE2")

        '#5
        [Set](ANS1, CA_MARCAS_NUMEROS_TOTAL_BULTOS, breakOnEmpty_:=False)

        '#6
        If [Set](ANS1, CA_CLAVE_SAD, breakOnEmpty_:=True) Then Return _report

        '#7
        If [Set](ANS1, CA_MEDIO_TRANSPORTE_ARRIBO, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {"=", "98"}) Then Return _report

        '#8
        If [Set](ANS1, CA_MEDIO_TRANSPORTE_SALIDA, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {"=", "98"}) Then Return _report

        ''#9
        '[Set](DocumentoElectronico_,
        '                     ANS3,
        '                     CA_RFC_IOE,
        '                     False)

        ''#10
        '[Set](DocumentoElectronico_,
        '                     ANS3,
        '                     CA_CURP_IOE,
        '                     False)

        '#9 
        [Set](ANS3, CA_RFC_IOE, breakOnEmpty_:=False, runAt_:=ROOMCUBE, roomNameExt_:="_MIXTO")

        '#9"
        [Set](ANS3, CA_CURP_IOE, breakOnEmpty_:=False)

        '#10
        If [Set](ANS3, CA_RAZON_SOCIAL_IOE, breakOnEmpty_:=True) Then Return _report

        '#11
        If [Set](ANS3, CA_DOMICILIO_IOE, breakOnEmpty_:=True) Then Return _report

        '#12
        If [Set](ANS1, CA_TIPO_OPERACION, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {"=", "1"}) Then Return _report

        '#13
        If [Set](ANS1, CA_CVE_PEDIMENTO, breakOnEmpty_:=True) Then Return _report

        '#14
        If [Set](ANS1, CA_REGIMEN, breakOnEmpty_:=True) Then Return _report

        '#15
        If [Set](ANS3, CA_VALOR_SEGUROS, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {"=", "0"}) Then Return _report

        '#16
        If [Set](ANS3, CA_SEGUROS, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {"=", "0"}) Then Return _report

        '#17
        If [Set](ANS3, CA_FLETES, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {"=", "0"}) Then Return _report

        '#18
        If [Set](ANS3, CA_EMBALAJES, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {"=", "0"}) Then Return _report

        '#19
        If [Set](ANS3, CA_OTROS_INCREMENTABLES, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {"=", "0"}) Then Return _report

        '#19
        [Set](ANS10, CA_ID_FISCAL_PROVEEDOR, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True, roomNameExt_:="_CASE2")

        '#20
        [Set](ANS10, CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_POC, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#21
        [Set](ANS10, CA_DOMICILIO_POC, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#22
        [Set](ANS10, CA_VINCULACION, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#23
        [Set](ANS13, CA_CFDI_FACTURA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True, sectionsFather_:=New List(Of [Enum]) From {ANS10})

        '#24
        [Set](ANS13, CA_FECHA_FACTURA, breakOnEmpty_:=False, runat_:=ROOMCUBE, dateCurrent_:=Now, recurring_:=True, sectionsFather_:=New List(Of [Enum]) From {ANS10})

        '#25
        [Set](ANS13, CA_CVE_MONEDA_FACTURA, breakOnEmpty_:=False, runat_:=ROOMCUBE, isPresentationValue_:=True, isReverse_:=True, recurring_:=True, length_:=3, sectionsFather_:=New List(Of [Enum]) From {ANS10})

        '#26
        [Set](ANS13, CA_MONTO_MONEDA_FACTURA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True, sectionsFather_:=New List(Of [Enum]) From {ANS10})

        '#27
        [Set](ANS13, CA_NUMERO_ACUSE_DE_VALOR, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True, sectionsFather_:=New List(Of [Enum]) From {ANS10})

        '#28"
        [Set](ANS13, CP_APLICA_ENAJENACION, breakOnEmpty_:=False, recurring_:=True, sectionsFather_:=New List(Of [Enum]) From {ANS10})

        '#28
        [Set](ANS13, CA_INCOTERM, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True, sectionsFather_:=New List(Of [Enum]) From {ANS10})

        '#29
        If [Set](ANS44, CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_AA, breakOnEmpty_:=True) Then Return _report

        '#30
        If [Set](ANS44, CA_RFC_AA, breakOnEmpty_:=True) Then Return _report

        '#31
        If [Set](ANS44, CA_CURP_AA_REPRESENTANTE_LEGAL, breakOnEmpty_:=True) Then Return _report

        '#32
        [Set](ANS44, CA_NOMBRE_MANDATARIO_REPRESENTANTE_AA, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#32"
        [Set](ANS44, CA_TIPO_FIGURA, breakOnEmpty_:=False)

        '#33
        [Set](ANS44, CA_RFC_MANDATARIO_AA_REPRESENTANTE_ALMACEN, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#34
        [Set](ANS44, CA_CURP_MANDATARIO_AA_REPRESENTANTE_ALMACEN, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#35
        If [Set](ANS44, CA_PATENTE, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#36
        If [Set](ANS44, CA_EFIRMA, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#37
        If [Set](ANS44, CA_CERTIFICADO_FIRMA, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#38
        If [Set](ANS43, CA_FIN_PEDIMENTO, breakOnEmpty_:=True) Then Return _report

        '#39
        If [Set](ANS14, CA_FECHA_PAGO, breakOnEmpty_:=True, runat_:=ROOMCUBE, dateCurrent_:=Now) Then Return _report

        '#40
        [Set](ANS18, CA_CVE_IDENTIFICADOR, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#41
        [Set](ANS18, CA_COMPLEMENTO_1, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#42
        [Set](ANS18, CA_COMPLEMENTO_2, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#43
        [Set](ANS18, CA_COMPLEMENTO_3, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#44
        If [Set](ANS1, CA_NUMERO_PEDIMENTO_COMPLETO, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#44"
        [Set](ANS1, CA_ANIO_VALIDACION, breakOnEmpty_:=False)


        '#45
        If [Set](ANS14, CA_FECHA_ENTRADA, breakOnEmpty_:=True, runat_:=ROOMCUBE, dateCurrent_:=Now) Then Return _report

        '#46
        [Set](ANS14, CA_FECHA_EXTRACCION, breakOnEmpty_:=False, runat_:=ROOMCUBE, dateCurrent_:=Now, roomNameExt_:="_CASE2")
        ''#44
        '[Set](ANS14, CA_FECHA_ORIGINAL, breakOnEmpty_:=False, runat_:=ROOMCUBE, dateCurrent_:=Now)

        '#47
        [Set](ANS17, CA_NUMERO_CONTENEDOR_FERROCARRIL_NUMERO_ECONOMICO, breakOnEmpty_:=False)

        '#48
        [Set](ANS17, CA_CVE_TIPO_CONTENEDOR, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#49
        [Set](ANS23, CA_OBSERVACIONES_PEDIMENTO, breakOnEmpty_:=False)

        '#50
        [Set](ANS20, CA_FECHA_PEDIMENTO_ORIGINAL, breakOnEmpty_:=False, runat_:=ROOMCUBE, dateCurrent_:=Now, recurring_:=True)

        '#51
        [Set](ANS20, CA_CVE_PEDIMENTO_ORIGINAL, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#52
        If [Set](ANS1, CA_TIPO_CAMBIO, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        _coincontroller = New ControladorMonedas

        Dim fecha_ = Date.Parse(If(_fieldvalues("S14.CA_FECHA_PAGO.0") = "",
                                Date.Now.ToString,
                                _fieldvalues("S14.CA_FECHA_PAGO.0"))).AddDays(-1)

        Dim tagWatcher_ = _coincontroller.ObtenerFactorTipodeCambio("USD",
                                                                       fecha_.ToString("yyyy-MM-dd"))
        Dim factorchanges_

        If tagWatcher_.Status = TypeStatus.Ok Then

            factorchanges_ = tagWatcher_.ObjectReturned

            Dim change_ = If(factorchanges_(1) Is Nothing,
                         "0",
                         factorchanges_(1).tipocambio.ToString)

            '#52"
            [Set](external_:=New List(Of String) From {"CA_TIPO_CAMBIO_CURRENT",
                                                                          change_})

            '#53
            [Set](ANS13, CA_FACTOR_MONEDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        Else
            _report.SetDetailReport(AdviceTypesReport.Information,
                                     "Validación detenida no se encontró tipo de cambio para la fecha especificada '" &
                                     fecha_ &
                                     "'",
                                     Chr(13) & GetEnumDescription(_validationtarget) & Chr(13) & "Folio de Operación:" & folioperacion & Chr(13),
                                     TriggerSourceTypes.Route
                                     )


            Return _report

        End If

        Dim index_ = 0

        For Each factor_ In _borderfields.
                            Keys.
                            Where(Function(e) e.fieldpedimento.ToString = CA_CVE_MONEDA_FACTURA.ToString)

            For Each elementFactor_ In _borderfields(factor_)

                tagWatcher_ = _coincontroller.
                 ObtenerFactorTipodeCambio(_fieldvalues(elementFactor_.formulafieldname),
                                           fecha_)

                If tagWatcher_.Status = TypeStatus.Ok Then


                    factorchanges_ = tagWatcher_.
                                 ObjectReturned

                    '#54"
                    [Set](external_:=New List(Of String) From {"CA_FACTOR_MONEDA_CURRENT." & index_, factorchanges_(0).factor.ToString})

                Else

                    _report.SetDetailReport(AdviceTypesReport.Information,
                                         "Validación detenida no se encontró tipo de cambio para la fecha especificada '" &
                                         fecha_ &
                                         "'",
                                         Chr(13) & GetEnumDescription(_validationtarget) & Chr(13) & "Folio de Operación:" & folioperacion & Chr(13),
                                         TriggerSourceTypes.Route
                                         )


                    Return _report

                End If

                index_ = index_ + 1

            Next

        Next

        '#54
        [Set](ANS20, CA_NUMERO_PEDIMENTO_ORIGINAL_COMPLETO, breakOnEmpty_:=False, recurring_:=True)

        '#55
        [Set](ANS13, CA_MONTO_USD, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#56
        If [Set](ANS1, CA_VALOR_ADUANA, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report


        '#57
        If [Set](ANS24, CA_SECUENCIA_PARTIDA, breakOnEmpty_:=True, recurring_:=True) Then Return _report

        '#58
        If [Set](ANS24, CA_FRACCION_ARANCELARIA_PARTIDA, breakOnEmpty_:=True, recurring_:=True) Then Return _report

        '#59
        If [Set](ANS24, CA_NICO_PARTIDA, breakOnEmpty_:=True, recurring_:=True) Then Return _report

        '#60
        If [Set](ANS24, CamposProveedorOperativo.CA_CVE_VINCULACION, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#61
        If [Set](ANS24, CA_CVE_METODO_VALORACION_PARTIDA, breakOnEmpty_:=True, runat_:=ROOMCUBE, recurring_:=True) Then Return _report

        '#62
        If [Set](ANS24, CA_CVE_UMC_PARTIDA, breakOnEmpty_:=True, recurring_:=True) Then Return _report

        '#63
        If [Set](ANS24, CA_CANTIDAD_UMC_PARTIDA, breakOnEmpty_:=True, runat_:=ROOMCUBE, recurring_:=True) Then Return _report

        '#64
        If [Set](ANS24, CA_CVE_UMT_PARTIDA, breakOnEmpty_:=True, recurring_:=True) Then Return _report

        '#65
        If [Set](ANS24, CA_CANTIDAD_UMT_PARTIDA, breakOnEmpty_:=True, runat_:=ROOMCUBE, recurring_:=True) Then Return _report

        '#66
        If [Set](ANS24, CA_CVE_PAIS_VENDEDOR_PARTIDA, breakOnEmpty_:=True, runat_:=ROOMCUBE, recurring_:=True) Then Return _report

        '#67
        If [Set](ANS24, CA_CVE_PAIS_ORIGEN_PARTIDA, breakOnEmpty_:=True, recurring_:=True) Then Return _report

        '#68
        If [Set](ANS24, CA_DESCRIPCION_MERCANCIA_PARTIDA, breakOnEmpty_:=True, recurring_:=True) Then Return _report

        '#69
        If [Set](ANS24, CA_PRECIO_PAGADO_PARTIDA, breakOnEmpty_:=True, runat_:=ROOMCUBE, recurring_:=True) Then Return _report

        '#70
        [Set](ANS29, CA_CVE_CONTRIBUCION_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True, sectionsFather_:=New List(Of [Enum]) From {ANS24})

        '#71
        [Set](ANS29, CA_CVE_TIPO_TASA_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True, sectionsFather_:=New List(Of [Enum]) From {ANS24})

        '#72
        [Set](ANS29, CA_TASA_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True, sectionsFather_:=New List(Of [Enum]) From {ANS24})

        '#73
        [Set](ANS30, CA_FORMA_PAGO_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True, sectionsFather_:=New List(Of [Enum]) From {ANS24})

        '#74
        If [Set](ANS1, CA_VALOR_DOLARES, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#74"
        If [Set](ANS24, CA_VALOR_DOLAR_PARTIDA, breakOnEmpty_:=True, recurring_:=True) Then Return _report

        '#75"
        If [Set](ANS24, CA_VALOR_COMERCIAL_PARTIDA, breakOnEmpty_:=True, recurring_:=True) Then Return _report

        '#75
        If [Set](ANS1, CA_PRECIO_PAGADO_VALOR_COMERCIAL, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '76
        If [Set](ANS24, CA_VALOR_ADUANA_PARTIDA, breakOnEmpty_:=True, runat_:=LOCALCONDITIONS, recurring_:=True, conditions_:=New List(Of String) From {"", "", "", "0"}) Then Return _report

        '#77
        If [Set](ANS24, CA_PRECIO_UNITARIO_PARTIDA, breakOnEmpty_:=True, recurring_:=True) Then Return _report

        '#78
        [Set](ANS24, CA_MARCA_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#78
        [Set](ANS24, CA_MODELO_PARTIDA, breakOnEmpty_:=False, recurring_:=True)

        '#80
        [Set](ANS24, CA_CODIGO_PRODUCTO_PARTIDA, breakOnEmpty_:=False, recurring_:=True)

        '#81
        [Set](ANS25, CA_VINCULACION_NUMERO_SERIE_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#82
        [Set](ANS25, CA_KILOMETRAJE_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#83
        [Set](ANS26, CA_CVE_PERMISO, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True, sectionsFather_:=New List(Of [Enum]) From {ANS24})

        '#84
        [Set](ANS26, CA_NUMERO_PERMISO, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True, sectionsFather_:=New List(Of [Enum]) From {ANS24})

        '#85
        [Set](ANS26, CA_FIRMA_ELECTRONICA_PERMISO, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True, sectionsFather_:=New List(Of [Enum]) From {ANS24})

        '#86
        [Set](ANS26, CA_VALOR_USD_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True, sectionsFather_:=New List(Of [Enum]) From {ANS24})

        '#87
        [Set](ANS26, CA_CANTIDAD_UMT_UMC, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True, sectionsFather_:=New List(Of [Enum]) From {ANS24})

        '#88
        [Set](ANS27, CA_CVE_IDENTIFICADOR_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True, sectionsFather_:=New List(Of [Enum]) From {ANS24})

        '#89
        [Set](ANS27, CA_COMPLEMENTO_1_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True, sectionsFather_:=New List(Of [Enum]) From {ANS24})

        '#90
        [Set](ANS27, CA_COMPLEMENTO_2_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True, sectionsFather_:=New List(Of [Enum]) From {ANS24})

        '#91
        [Set](ANS27, CA_COMPLEMENTO_3_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True, sectionsFather_:=New List(Of [Enum]) From {ANS24})

        '#92
        [Set](ANS36, CA_OBSERVACIONES_PARTIDA, breakOnEmpty_:=False, recurring_:=True)

        '#93
        [Set](ANS30, CA_IMPORTE_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True, sectionsFather_:=New List(Of [Enum]) From {ANS24})

        '#94
        If [Set](ANS43, CA_NUMERO_TOTAL_PARTIDAS, breakOnEmpty_:=True) Then Return _report

        '#95
        [Set](ANS6, CA_CONTRIBUCION, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#96
        [Set](ANS6, CA_CVE_TIPO_TASA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#97
        [Set](ANS6, CA_TASA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#98
        [Set](ANS55, CA_CONCEPTO, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#99
        [Set](ANS55, CA_FORMA_PAGO, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#100
        [Set](ANS55, CA_IMPORTE, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#101
        [Set](ANS7, CA_EFECTIVO, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#102
        [Set](ANS7, CA_OTROS, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#103
        [Set](ANS7, CA_TOTAL, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#104
        If [Set](ANS1, CA_CVE_PREVALIDADOR, breakOnEmpty_:=True) Then Return _report

        '#105
        [Set](ANS9, CA_PATENTE, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#106
        [Set](ANS9, CA_NUMERO_PEDIMENTO, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#107
        [Set](ANS9, CA_CLAVE_SAD, breakOnEmpty_:=False, runAt_:=ROOMCUBE, roomNameExt_:="_LINEA_CAPTURA")

        '#108
        [Set](ANS9, CA_PAGO_ELECTRONICO, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#109
        [Set](ANS9, CA_NOMBRE_INSTITUCION_BANCARIA, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#110
        [Set](ANS9, CA_LINEA_CAPTURA, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#111
        [Set](ANS9, CA_EFECTIVO, breakOnEmpty_:=False, runAt_:=ROOMCUBE, roomNameExt_:="_LINEA_CAPTURA")

        '#112
        [Set](ANS9, CA_FECHA_PAGO, breakOnEmpty_:=False, runat_:=ROOMCUBE, dateCurrent_:=Now, roomNameExt_:="_LINEA_CAPTURA")

        '#113
        [Set](ANS9, CA_NUMERO_OPERACION_BANCARIA, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#114
        [Set](ANS9, CA_NUMERO_TRANSACCION_SAT, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#115
        [Set](ANS9, CA_MEDIO_PRESENTACION, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#116
        [Set](ANS9, CA_MEDIO_RECEPCION_COBRO, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#117
        If [Set](ANS1, CA_ACUSE_ELECTRONICO_VALIDACION, breakOnEmpty_:=True) Then Return _report

        '#118
        If [Set](ANS1, CA_CODIGO_BARRAS, breakOnEmpty_:=True) Then Return _report

        '#119
        [Set](ANS9, CA_DEPOSITO_REFERENCIADO, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#120
        [Set](ANS1, CA_FECHA_VALIDACION, breakOnEmpty_:=False, runat_:=IValidationRoute.RunAt.LOCALCONDITIONS, dateCurrent_:=Now)

        '#125
        [Set](ANS1, CA_CERTIFICACION, breakOnEmpty_:=False)

        Return _report


    End Function



End Class
