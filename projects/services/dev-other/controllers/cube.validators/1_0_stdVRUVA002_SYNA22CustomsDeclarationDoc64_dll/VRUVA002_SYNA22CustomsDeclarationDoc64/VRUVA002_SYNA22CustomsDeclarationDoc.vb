Imports Rec.Globals.Controllers
Imports Syn.Nucleo.RecursosComercioExterior.SeccionesPedimento
Imports Syn.Nucleo.RecursosComercioExterior.CamposPedimento
Imports Wma.Exceptions.TagWatcher
Imports Cube.Validators.IValidationRoute.RunAt
Imports Cube.ValidatorReport
Imports Syn.Nucleo.RecursosComercioExterior

Public Class VRUVA002_SYNA22CustomsDeclarationDoc
    Inherits ValidationRUVA

    Sub New()

        _validationtarget = IValidationRoute.ValidationRoutes.RUVA2

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
        [Set](ANS1, CA_PESO_BRUTO, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#5
        [Set](ANS1, CA_MARCAS_NUMEROS_TOTAL_BULTOS, breakOnEmpty_:=False)

        '#6
        If [Set](ANS1, CA_CLAVE_SAD, breakOnEmpty_:=True) Then Return _report

        '#7
        If [Set](ANS1, CA_MEDIO_TRANSPORTE_ARRIBO, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {"=", "98"}) Then Return _report

        '#8
        If [Set](ANS1, CA_MEDIO_TRANSPORTE_SALIDA, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {"=", "98"}) Then Return _report

        '#9
        If [Set](ANS1, CA_TIPO_OPERACION, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {"=", "1"}) Then Return _report

        '#10
        If [Set](ANS1, CA_CVE_PEDIMENTO, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {"=", "GC"}) Then Return _report

        '#11
        If [Set](ANS1, CA_REGIMEN, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {"=", "IMD"}) Then Return _report

        '#12
        If [Set](ANS44, CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_AA, breakOnEmpty_:=True) Then Return _report

        '#13
        If [Set](ANS44, CA_RFC_AA, breakOnEmpty_:=True) Then Return _report

        '#14
        If [Set](ANS44, CA_CURP_AA_REPRESENTANTE_LEGAL, breakOnEmpty_:=True) Then Return _report

        '#15"
        [Set](ANS44, CA_TIPO_FIGURA, breakOnEmpty_:=False)

        '#15
        [Set](ANS44, CA_NOMBRE_MANDATARIO_REPRESENTANTE_AA, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#16
        [Set](ANS44, CA_RFC_MANDATARIO_AA_REPRESENTANTE_ALMACEN, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#17
        [Set](ANS44, CA_CURP_MANDATARIO_AA_REPRESENTANTE_ALMACEN, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#18
        If [Set](ANS44, CA_PATENTE, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#19
        If [Set](ANS44, CA_EFIRMA, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#20
        If [Set](ANS44, CA_CERTIFICADO_FIRMA, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#21
        If [Set](ANS43, CA_FIN_PEDIMENTO, breakOnEmpty_:=True) Then Return _report

        '#22
        If [Set](ANS1, CA_NUMERO_PEDIMENTO_COMPLETO, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#23
        If [Set](ANS14, CA_FECHA_PAGO, breakOnEmpty_:=True, runat_:=ROOMCUBE, dateCurrent_:=Now) Then Return _report

        '#24
        If [Set](ANS14, CA_FECHA_ENTRADA, breakOnEmpty_:=True, runat_:=ROOMCUBE, dateCurrent_:=Now) Then Return _report

        '#25
        [Set](ANS3, CA_RFC_IOE, breakOnEmpty_:=False, runAt_:=ROOMCUBE, roomNameExt_:="_MIXTO")

        '#25"
        [Set](ANS3, CA_CURP_IOE, breakOnEmpty_:=False)

        '#26
        If [Set](ANS3, CA_RAZON_SOCIAL_IOE, breakOnEmpty_:=True) Then Return _report

        '#27
        If [Set](ANS3, CA_DOMICILIO_IOE, breakOnEmpty_:=True) Then Return _report

        ''#28
        'If [Set](ANS1, CA_VALOR_ADUANA, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {"=", "0"}) Then Return _report

        '#28
        If [Set](ANS1, CA_TIPO_CAMBIO, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        _coincontroller = New ControladorMonedas

        Dim fecha_ = Date.Parse(If(_fieldvalues("S14.CA_FECHA_ENTRADA.0") = "",
                                Date.Now.ToString,
                                _fieldvalues("S14.CA_FECHA_ENTRADA.0"))).AddDays(-1)

        Dim tagWatcher_ = _coincontroller.ObtenerFactorTipodeCambio("MXN",
                                                                       fecha_.ToString("yyyy-MM-dd"))
        Dim changetype_

        If tagWatcher_.Status = TypeStatus.Ok Then

            changetype_ = tagWatcher_.ObjectReturned

            Dim change_ = If(changetype_(1) Is Nothing,
                         "0",
                         changetype_(1).tipocambio.ToString)

            '#28"
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


        '#29
        [Set](ANS18, CA_CVE_IDENTIFICADOR, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#30
        [Set](ANS18, CA_COMPLEMENTO_1, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#31
        [Set](ANS18, CA_COMPLEMENTO_2, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#32
        [Set](ANS18, CA_COMPLEMENTO_3, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#33
        [Set](ANS23, CA_OBSERVACIONES_PEDIMENTO, breakOnEmpty_:=False)

        '#34
        If [Set](ANS20, CA_FECHA_PEDIMENTO_ORIGINAL, breakOnEmpty_:=True, runat_:=ROOMCUBE, dateCurrent_:=Now, recurring_:=True, roomNameExt_:="_CASE3") Then Return _report

        '#35
        If [Set](ANS20, CA_CVE_PEDIMENTO_ORIGINAL, breakOnEmpty_:=True, runat_:=ROOMCUBE, recurring_:=True) Then Return _report

        '#36
        If [Set](ANS20, CA_NUMERO_PEDIMENTO_ORIGINAL_COMPLETO, breakOnEmpty_:=True, recurring_:=True) Then Return _report

        '#37
        If [Set](ANS24, CA_SECUENCIA_PARTIDA, breakOnEmpty_:=True, runat_:=ROOMCUBE, recurring_:=True) Then Return _report

        '#38
        If [Set](ANS24, CA_FRACCION_ARANCELARIA_PARTIDA, breakOnEmpty_:=True, runat_:=IValidationRoute.RunAt.LOCALCONDITIONS, recurring_:=True, conditions_:=New List(Of String) From {"=", "99999999"}) Then Return _report

        '#39
        If [Set](ANS24, CA_NICO_PARTIDA, breakOnEmpty_:=True, runat_:=IValidationRoute.RunAt.LOCALCONDITIONS, recurring_:=True, conditions_:=New List(Of String) From {"=", "00"}) Then Return _report

        '#40
        If [Set](ANS24, CamposProveedorOperativo.CA_CVE_VINCULACION, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#41
        If [Set](ANS24, CA_CVE_METODO_VALORACION_PARTIDA, breakOnEmpty_:=True, runat_:=LOCALCONDITIONS, recurring_:=True, conditions_:=New List(Of String) From {"<>", "0"}) Then Return _report

        '#42
        If [Set](ANS24, CA_CVE_UMC_PARTIDA, breakOnEmpty_:=True, runat_:=LOCALCONDITIONS, recurring_:=True, conditions_:=New List(Of String) From {"=", "1"}) Then Return _report

        '#43
        If [Set](ANS24, CA_CANTIDAD_UMC_PARTIDA, breakOnEmpty_:=True, runat_:=LOCALCONDITIONS, recurring_:=True, conditions_:=New List(Of String) From {"=", "1"}) Then Return _report

        '#44
        If [Set](ANS24, CA_CVE_UMT_PARTIDA, breakOnEmpty_:=True, runat_:=LOCALCONDITIONS, recurring_:=True, conditions_:=New List(Of String) From {"=", "1"}) Then Return _report

        '#45
        If [Set](ANS24, CA_CANTIDAD_UMT_PARTIDA, breakOnEmpty_:=True, runat_:=ROOMCUBE, recurring_:=True, conditions_:=New List(Of String) From {"=", "1"}) Then Return _report

        '#46
        If [Set](ANS24, CA_CVE_PAIS_VENDEDOR_PARTIDA, breakOnEmpty_:=True, runat_:=LOCALCONDITIONS, recurring_:=True, conditions_:=New List(Of String) From {"=", "KCD"}) Then Return _report

        '#47
        If [Set](ANS24, CA_CVE_PAIS_ORIGEN_PARTIDA, breakOnEmpty_:=True, runat_:=LOCALCONDITIONS, recurring_:=True, conditions_:=New List(Of String) From {"=", "KCD"}) Then Return _report

        '#48
        If [Set](ANS24, CA_DESCRIPCION_MERCANCIA_PARTIDA, breakOnEmpty_:=True, runat_:=LOCALCONDITIONS, recurring_:=True, conditions_:=New List(Of String) From {"=", "AJUSTE DE VALOR"}) Then Return _report

        '#49
        If [Set](ANS1, CA_VALOR_ADUANA, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#50
        [Set](ANS29, CA_CVE_CONTRIBUCION_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True, sectionsFather_:=New List(Of [Enum]) From {ANS24})

        '#51
        [Set](ANS29, CA_CVE_TIPO_TASA_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True, sectionsFather_:=New List(Of [Enum]) From {ANS24})

        '#52
        [Set](ANS29, CA_TASA_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True, sectionsFather_:=New List(Of [Enum]) From {ANS24})

        '#53
        [Set](ANS30, CA_FORMA_PAGO_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True, roomNameExt_:="_CASE2", sectionsFather_:=New List(Of [Enum]) From {ANS24})

        '#54
        If [Set](ANS24, CA_PRECIO_PAGADO_PARTIDA, breakOnEmpty_:=True, recurring_:=True) Then Return _report

        '#55
        If [Set](ANS1, CA_VALOR_DOLARES, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#56
        If [Set](ANS43, CA_NUMERO_TOTAL_PARTIDAS, breakOnEmpty_:=True) Then Return _report

        '#57
        If [Set](ANS1, CA_CVE_PREVALIDADOR, breakOnEmpty_:=True) Then Return _report

        '#58
        If [Set](ANS1, CA_PRECIO_PAGADO_VALOR_COMERCIAL, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#59
        If [Set](ANS24, CA_VALOR_ADUANA_PARTIDA, breakOnEmpty_:=True, runat_:=ROOMCUBE, recurring_:=True) Then Return _report

        '#60
        If [Set](ANS24, CA_PRECIO_UNITARIO_PARTIDA, breakOnEmpty_:=True, runat_:=ROOMCUBE, recurring_:=True) Then Return _report

        '#61
        [Set](ANS30, CA_IMPORTE_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#62
        [Set](ANS6, CA_CONTRIBUCION, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#63
        [Set](ANS6, CA_CVE_TIPO_TASA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#64
        [Set](ANS6, CA_TASA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#65
        [Set](ANS55, CA_CONCEPTO, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#66
        [Set](ANS55, CA_FORMA_PAGO, breakOnEmpty_:=False, runat_:=LOCALCONDITIONS, recurring_:=True, conditions_:=New List(Of String) From {"=", "0"})

        '#67
        [Set](ANS55, CA_IMPORTE, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#58
        [Set](ANS7, CA_EFECTIVO, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#69
        [Set](ANS7, CA_OTROS, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#70
        [Set](ANS7, CA_TOTAL, breakOnEmpty_:=False, runAt_:=ROOMCUBE)


        '#71
        If [Set](ANS9, CA_PATENTE, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#72
        If [Set](ANS9, CA_NUMERO_PEDIMENTO, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#73
        If [Set](ANS9, CA_CLAVE_SAD, breakOnEmpty_:=True, runAt_:=ROOMCUBE, roomNameExt_:="_LINEA_CAPTURA") Then Return _report

        '#74
        If [Set](ANS9, CA_PAGO_ELECTRONICO, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#75
        If [Set](ANS9, CA_NOMBRE_INSTITUCION_BANCARIA, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#76
        If [Set](ANS9, CA_LINEA_CAPTURA, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#77
        If [Set](ANS9, CA_EFECTIVO, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#78
        If [Set](ANS9, CA_FECHA_PAGO, breakOnEmpty_:=True, runat_:=ROOMCUBE, dateCurrent_:=Now, roomNameExt_:="_LINEA_CAPTURA") Then Return _report

        '#79
        If [Set](ANS9, CA_NUMERO_OPERACION_BANCARIA, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#80
        If [Set](ANS9, CA_NUMERO_TRANSACCION_SAT, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#81
        If [Set](ANS9, CA_MEDIO_PRESENTACION, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#82
        If [Set](ANS9, CA_MEDIO_RECEPCION_COBRO, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#83
        If [Set](ANS1, CA_ACUSE_ELECTRONICO_VALIDACION, breakOnEmpty_:=True) Then Return _report

        '#84
        If [Set](ANS1, CA_CODIGO_BARRAS, breakOnEmpty_:=True) Then Return _report

        '#85
        [Set](ANS9, CA_DEPOSITO_REFERENCIADO, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#86
        [Set](ANS1, CA_FECHA_VALIDACION, breakOnEmpty_:=False, runat_:=LOCALCONDITIONS, dateCurrent_:=Now)

        '#87
        [Set](ANS1, CA_ANIO_VALIDACION, breakOnEmpty_:=False)

        '#88"
        If [Set](ANS3, CA_CVE_TIPO_OPERACION, breakOnEmpty_:=True) Then Return _report

        '#89
        If [Set](ANS3, CA_SEGUROS, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {">=", "0"}) Then Return _report

        '#90
        If [Set](ANS3, CA_FLETES, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {">=", "0"}) Then Return _report

        '#91
        If [Set](ANS3, CA_EMBALAJES, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {">=", "0"}) Then Return _report

        '#92
        If [Set](ANS3, CA_OTROS_INCREMENTABLES, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {">=", "0"}) Then Return _report

        '#93
        If [Set](ANS3, CA_TRANSPORTE_DECREMENTABLES, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {">=", "0"}) Then Return _report

        '#94
        If [Set](ANS3, CA_SEGURO_DECREMENTABLES, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {">=", "0"}) Then Return _report

        '#95
        If [Set](ANS3, CA_CARGA_DECREMENTABLES, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {">=", "0"}) Then Return _report

        '#96
        If [Set](ANS3, CA_DESCARGA_DECREMENTABLES, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {">=", "0"}) Then Return _report

        '#97
        If [Set](ANS3, CA_OTROS_DECREMENTABLES, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {">=", "0"}) Then Return _report

        '#98
        If [Set](ANS24, CA_VALOR_COMERCIAL_PARTIDA, breakOnEmpty_:=True, recurring_:=True) Then Return _report

        Return _report

    End Function


End Class
