Imports Rec.Globals.Controllers
Imports Syn.Nucleo.RecursosComercioExterior.SeccionesPedimento
Imports Syn.Nucleo.RecursosComercioExterior.CamposPedimento
Imports Wma.Exceptions.TagWatcher
Imports Cube.Validators.IValidationRoute.RunAt
Imports Cube.ValidatorReport
Public Class VRUVA007_SYNA22CustomsDeclarationDoc
    Inherits ValidationRUVA
    Sub New()

        _validationtarget = IValidationRoute.ValidationRoutes.RUVA7

        _cubeslice = ICubeController.CubeSlices.A22

        _quality = IValidationRoute.ValidationQuality.SYNQUALITY

    End Sub


    Protected Overrides Function LoadInitialStockElement() As ValidatorReport

        '#1
        If [Set](ANS1, CA_CLAVE_SAD, breakOnEmpty_:=True) Then Return _report

        '#2"
        If [Set](ANS1, CA_TIPO_OPERACION, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {"=", "2"}) Then Return _report

        '#2
        If [Set](ANS1, CA_CVE_PEDIMENTO, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {"=", "CT"}) Then Return _report

        '#3
        If [Set](ANS1, CA_NUMERO_PEDIMENTO_COMPLETO, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#4
        If [Set](ANS14, CA_FECHA_PAGO, breakOnEmpty_:=True, runat_:=ROOMCUBE, dateCurrent_:=Now) Then Return _report

        '#5
        [Set](ANS14, CA_FECHA_IMPORTACION_EUA_CAN, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#6
        [Set](ANS14, CA_FECHA_PRESENTACION, breakOnEmpty_:=False, runat_:=ROOMCUBE, dateCurrent_:=Now)

        '#7
        If [Set](ANS1, CA_TIPO_CAMBIO, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        _coincontroller = New ControladorMonedas

        Dim fecha_ = Date.Parse(If(_fieldvalues("S14.CA_FECHA_PAGO.0") = "",
                                Date.Now.ToString,
                                _fieldvalues("S14.CA_FECHA_PAGO.0"))).AddDays(-1)

        Dim tagWatcher_ = _coincontroller.ObtenerFactorTipodeCambio("MXN",
                                                                       fecha_.ToString("yyyy-MM-dd"))
        Dim changetype_

        If tagWatcher_.Status = TypeStatus.Ok Then

            changetype_ = tagWatcher_.ObjectReturned

            Dim change_ = If(changetype_(1) Is Nothing,
                         "0",
                         changetype_(1).tipocambio.ToString)

            '#7"
            [Set](external_:=New List(Of String) From {"CA_TIPO_CAMBIO_CURRENT",
                                                                          change_})

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

        '#8
        If [Set](ANS44, CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_AA, breakOnEmpty_:=True) Then Return _report

        '#9
        If [Set](ANS44, CA_RFC_AA, breakOnEmpty_:=True) Then Return _report

        '#10
        If [Set](ANS44, CA_CURP_AA_REPRESENTANTE_LEGAL, breakOnEmpty_:=True) Then Return _report

        '#11"
        [Set](ANS44, CA_TIPO_FIGURA, breakOnEmpty_:=False)

        '#11
        [Set](ANS44, CA_NOMBRE_MANDATARIO_REPRESENTANTE_AA, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#12
        [Set](ANS44, CA_RFC_MANDATARIO_AA_REPRESENTANTE_ALMACEN, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#13
        [Set](ANS44, CA_CURP_MANDATARIO_AA_REPRESENTANTE_ALMACEN, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#14
        If [Set](ANS44, CA_PATENTE, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#15
        If [Set](ANS44, CA_EFIRMA, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#16
        If [Set](ANS44, CA_CERTIFICADO_FIRMA, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#17
        If [Set](ANS43, CA_FIN_PEDIMENTO, breakOnEmpty_:=True) Then Return _report

        '#18
        [Set](ANS3, CA_RFC_IOE, breakOnEmpty_:=False, runAt_:=ROOMCUBE, roomNameExt_:="_MIXTO")

        '#18"
        [Set](ANS3, CA_CURP_IOE, breakOnEmpty_:=False)

        '#19
        If [Set](ANS3, CA_RAZON_SOCIAL_IOE, breakOnEmpty_:=True) Then Return _report

        '#20
        If [Set](ANS3, CA_DOMICILIO_IOE, breakOnEmpty_:=True) Then Return _report

        '#21
        [Set](ANS18, CA_CVE_IDENTIFICADOR, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#22
        [Set](ANS18, CA_COMPLEMENTO_1, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#23
        [Set](ANS18, CA_COMPLEMENTO_2, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#24
        [Set](ANS18, CA_COMPLEMENTO_3, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#25
        [Set](ANS23, CA_OBSERVACIONES_PEDIMENTO, breakOnEmpty_:=False)

        '#26
        If [Set](ANS20, CA_FECHA_PEDIMENTO_ORIGINAL, breakOnEmpty_:=True, runat_:=ROOMCUBE, dateCurrent_:=Now, recurring_:=True, roomNameExt_:="_CASE32") Then Return _report

        '#27
        If [Set](ANS20, CA_CVE_PEDIMENTO_ORIGINAL, breakOnEmpty_:=True, runat_:=ROOMCUBE, recurring_:=True) Then Return _report

        '#28
        If [Set](ANS20, CA_NUMERO_PEDIMENTO_ORIGINAL_COMPLETO, breakOnEmpty_:=True, recurring_:=True) Then Return _report

        '#29
        [Set](ANS39, CA_CVE_PAIS_EXPORTACION, breakOnEmpty_:=False)

        '#30
        [Set](ANS39, CA_NUMERO_DOCUMENTO_IMPORTACION_EU_CAN, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#31
        [Set](ANS39, CA_CVE_PRUEBA, breakOnEmpty_:=False, runAt_:=ROOMCUBE)


        '#32
        If [Set](ANS24, CA_SECUENCIA_PARTIDA, breakOnEmpty_:=True, runat_:=ROOMCUBE, recurring_:=True) Then Return _report

        '#33
        If [Set](ANS24, CA_FRACCION_ARANCELARIA_PARTIDA, breakOnEmpty_:=True) Then Return _report

        '#34
        If [Set](ANS32, CA_MONTO_MERCANCIAS_NO_ORIGINARIAS_PARTIDA, breakOnEmpty_:=True) Then Return _report

        '#35
        If [Set](ANS24, CA_MONTO_IGI_PARTIDA, breakOnEmpty_:=True) Then Return _report

        '#36
        [Set](ANS24, CA_MONTO_IMPUESTO_PAGADO_EU_CAN, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#37
        [Set](ANS24, CA_MONTO_IMPORTE_CONFORME_CAMPO_4, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#38
        [Set](ANS24, CA_FRACCION_ARANCELARIA_BIEN_FINAL_EU_CAN, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#39
        [Set](ANS24, CA_TASA_IMPORTE_PAGADO_EU_CAN, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#40
        [Set](ANS24, CA_MONTO_IMPORTE_PAGADO_EU_CAN, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#41
        [Set](ANS24, CA_UMT_EUA_CAN, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#42
        [Set](ANS24, CA_CANT_MERCANCIA_UMT_EUA_CAN, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#43
        [Set](ANS36, CA_OBSERVACIONES_PARTIDA, breakOnEmpty_:=False, recurring_:=True)

        '#44
        If [Set](ANS30, CA_FORMA_PAGO_PARTIDA, breakOnEmpty_:=True, runat_:=ROOMCUBE, recurring_:=True, roomNameExt_:="_CASE2", sectionsFather_:=New List(Of [Enum]) From {ANS24}) Then Return report

        '#45
        If [Set](ANS30, CA_IMPORTE_PARTIDA, breakOnEmpty_:=True, runat_:=ROOMCUBE, recurring_:=True, sectionsFather_:=New List(Of [Enum]) From {ANS24}) Then Return report

        '#46
        If [Set](ANS6, CA_CONTRIBUCION, breakOnEmpty_:=True, runat_:=ROOMCUBE, recurring_:=True) Then Return report


        '#47
        If [Set](ANS6, CA_CVE_TIPO_TASA, breakOnEmpty_:=True, runat_:=ROOMCUBE, recurring_:=True) Then Return report


        '#48
        If [Set](ANS6, CA_TASA, breakOnEmpty_:=True, runat_:=ROOMCUBE, recurring_:=True) Then Return report

        '#49
        If [Set](ANS55, CA_CONCEPTO, breakOnEmpty_:=True, runat_:=ROOMCUBE, recurring_:=True) Then Return report

        '#50
        If [Set](ANS55, CA_FORMA_PAGO, breakOnEmpty_:=False, runat_:=LOCALCONDITIONS, recurring_:=True, conditions_:=New List(Of String) From {"=", "0"}) Then Return report

        '#51
        If [Set](ANS55, CA_IMPORTE, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True) Then Return report

        '#52
        If [Set](ANS7, CA_EFECTIVO, breakOnEmpty_:=False, runAt_:=ROOMCUBE) Then Return report

        '#53
        If [Set](ANS7, CA_OTROS, breakOnEmpty_:=False, runAt_:=ROOMCUBE) Then Return report

        '#54
        If [Set](ANS7, CA_TOTAL, breakOnEmpty_:=False, runAt_:=ROOMCUBE) Then Return report

        '#55
        If [Set](ANS43, CA_NUMERO_TOTAL_PARTIDAS, breakOnEmpty_:=True) Then Return _report

        '#56
        If [Set](ANS1, CA_CVE_PREVALIDADOR, breakOnEmpty_:=True) Then Return _report

        '#57
        If [Set](ANS9, CA_PATENTE, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#58
        If [Set](ANS9, CA_NUMERO_PEDIMENTO, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#59
        If [Set](ANS9, CA_CLAVE_SAD, breakOnEmpty_:=True, runAt_:=ROOMCUBE, roomNameExt_:="_LINEA_CAPTURA") Then Return _report

        '#60
        If [Set](ANS9, CA_PAGO_ELECTRONICO, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#61
        If [Set](ANS9, CA_NOMBRE_INSTITUCION_BANCARIA, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#62
        If [Set](ANS9, CA_LINEA_CAPTURA, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#63
        If [Set](ANS9, CA_EFECTIVO, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#64
        If [Set](ANS9, CA_FECHA_PAGO, breakOnEmpty_:=True, runat_:=ROOMCUBE, dateCurrent_:=Now, roomNameExt_:="_LINEA_CAPTURA") Then Return _report

        '#65
        If [Set](ANS9, CA_NUMERO_OPERACION_BANCARIA, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#66
        If [Set](ANS9, CA_NUMERO_TRANSACCION_SAT, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#67
        If [Set](ANS9, CA_MEDIO_PRESENTACION, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#68
        If [Set](ANS9, CA_MEDIO_RECEPCION_COBRO, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#69
        If [Set](ANS1, CA_ACUSE_ELECTRONICO_VALIDACION, breakOnEmpty_:=True) Then Return _report

        '#70
        If [Set](ANS1, CA_CODIGO_BARRAS, breakOnEmpty_:=True) Then Return _report

        '#71
        [Set](ANS9, CA_DEPOSITO_REFERENCIADO, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#72
        [Set](ANS1, CA_FECHA_VALIDACION, breakOnEmpty_:=False, runat_:=LOCALCONDITIONS, dateCurrent_:=Now)

        '#73
        [Set](ANS1, CA_ANIO_VALIDACION, breakOnEmpty_:=False)

        '#74
        If [Set](ANS1, CA_VALOR_ADUANA, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {"=", "0"}) Then Return _report


        ''#43
        'If [Set](ANS24, CA_CVE_UMC_PARTIDA, breakOnEmpty_:=True, runat_:=LOCALCONDITIONS, recurring_:=True, conditions_:=New List(Of String) From {"=", "1"}) Then Return _report

        ''#44
        'If [Set](ANS24, CA_CANTIDAD_UMC_PARTIDA, breakOnEmpty_:=True, runat_:=LOCALCONDITIONS, recurring_:=True, conditions_:=New List(Of String) From {"=", "1"}) Then Return _report

        ''#45
        'If [Set](ANS24, CA_CVE_UMT_PARTIDA, breakOnEmpty_:=True, recurring_:=True) Then Return _report

        ''#46
        'If [Set](ANS24, CA_CANTIDAD_UMT_PARTIDA, breakOnEmpty_:=True, runat_:=ROOMCUBE, recurring_:=True) Then Return _report

        ''#47
        'If [Set](ANS24, CA_CVE_PAIS_VENDEDOR_PARTIDA, breakOnEmpty_:=True, runat_:=LOCALCONDITIONS, recurring_:=True, conditions_:=New List(Of String) From {"=", "KCD"}) Then Return _report

        ''#48
        'If [Set](ANS24, CA_CVE_PAIS_ORIGEN_PARTIDA, breakOnEmpty_:=True, runat_:=LOCALCONDITIONS, recurring_:=True, conditions_:=New List(Of String) From {"=", "KCD"}) Then Return _report

        ''#49
        'If [Set](ANS24, CA_DESCRIPCION_MERCANCIA_PARTIDA, breakOnEmpty_:=True, runat_:=LOCALCONDITIONS, recurring_:=True, conditions_:=New List(Of String) From {"=", "AJUSTE DE VALOR"}) Then Return _report

        ''#50
        'If [Set](ANS24, CA_PRECIO_PAGADO_PARTIDA, breakOnEmpty_:=True, runat_:=IValidationRoute.RunAt.ROOMCUBE, recurring_:=True, roomNameExt_:="_CASE1") Then Return _report

        ''#51
        'If [Set](ANS24, CA_VALOR_COMERCIAL_PARTIDA, breakOnEmpty_:=True, runat_:=IValidationRoute.RunAt.ROOMCUBE, recurring_:=True) Then Return _report

        ''#52
        'If [Set](ANS24, CA_VALOR_DOLAR_PARTIDA, breakOnEmpty_:=True, runat_:=LOCALCONDITIONS, recurring_:=True, conditions_:=New List(Of String) From {">", "0"}) Then Return _report

        ''#53
        'If [Set](ANS24, CA_PRECIO_UNITARIO_PARTIDA, breakOnEmpty_:=True, runat_:=IValidationRoute.RunAt.ROOMCUBE, recurring_:=True) Then Return _report

        ''#53"
        'If [Set](ANS3, CA_CVE_TIPO_OPERACION, breakOnEmpty_:=True) Then Return _report

        ''#54
        '[Set](ANS29, CA_CVE_CONTRIBUCION_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        ''#55
        '[Set](ANS29, CA_CVE_TIPO_TASA_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        ''#56
        '[Set](ANS29, CA_TASA_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)




        ''#60
        'If [Set](ANS1, CA_VALOR_DOLARES, breakOnEmpty_:=True, runAt_:=IValidationRoute.RunAt.ROOMCUBE) Then Return _report

        ''#61
        '[Set](ANS6, CA_CONTRIBUCION, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        ''#62
        '[Set](ANS6, CA_CVE_TIPO_TASA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        ''#63
        '[Set](ANS6, CA_TASA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        ''#64
        '[Set](ANS55, CA_CONCEPTO, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        ''#65
        '[Set](ANS55, CA_FORMA_PAGO, breakOnEmpty_:=False, runat_:=LOCALCONDITIONS, recurring_:=True, conditions_:=New List(Of String) From {"=", "0"})

        ''#66
        '[Set](ANS55, CA_IMPORTE, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        ''#57
        '[Set](ANS7, CA_EFECTIVO, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        ''#68
        '[Set](ANS7, CA_OTROS, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        ''#69
        '[Set](ANS7, CA_TOTAL, breakOnEmpty_:=False, runAt_:=ROOMCUBE)



        ''#71
        'If [Set](ANS9, CA_PATENTE, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        ''#72
        'If [Set](ANS9, CA_NUMERO_PEDIMENTO, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        ''#73
        'If [Set](ANS9, CA_CLAVE_SAD, breakOnEmpty_:=True, runAt_:=ROOMCUBE, roomNameExt_:="_LINEA_CAPTURA") Then Return _report

        ''#74
        'If [Set](ANS9, CA_PAGO_ELECTRONICO, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        ''#75
        'If [Set](ANS9, CA_NOMBRE_INSTITUCION_BANCARIA, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        ''#76
        'If [Set](ANS9, CA_LINEA_CAPTURA, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        ''#77
        'If [Set](ANS9, CA_EFECTIVO, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        ''#78
        'If [Set](ANS9, CA_FECHA_PAGO, breakOnEmpty_:=True, runat_:=ROOMCUBE, dateCurrent_:=Now, roomNameExt_:="_LINEA_CAPTURA") Then Return _report

        ''#79
        'If [Set](ANS9, CA_NUMERO_OPERACION_BANCARIA, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        ''#80
        'If [Set](ANS9, CA_NUMERO_TRANSACCION_SAT, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        ''#81
        'If [Set](ANS9, CA_MEDIO_PRESENTACION, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        ''#82
        'If [Set](ANS9, CA_MEDIO_RECEPCION_COBRO, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        ''#83
        'If [Set](ANS1, CA_ACUSE_ELECTRONICO_VALIDACION, breakOnEmpty_:=True) Then Return _report

        ''#84
        'If [Set](ANS1, CA_CODIGO_BARRAS, breakOnEmpty_:=True) Then Return _report

        ''#85
        '[Set](ANS9, CA_DEPOSITO_REFERENCIADO, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        ''#86
        '[Set](ANS1, CA_FECHA_VALIDACION, breakOnEmpty_:=False, runat_:=LOCALCONDITIONS, dateCurrent_:=Now)

        ''#87
        '[Set](ANS1, CA_ANIO_VALIDACION, breakOnEmpty_:=False)

        ''#28
        'If [Set](ANS1, CA_VALOR_ADUANA, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {"=", "0"}) Then Return _report

        ''#29


        Return _report

    End Function


End Class
