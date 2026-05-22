Imports Rec.Globals.Controllers
Imports Syn.Nucleo.RecursosComercioExterior.SeccionesPedimento
Imports Syn.Nucleo.RecursosComercioExterior.CamposPedimento
Imports Wma.Exceptions.TagWatcher
Imports Cube.Validators.IValidationRoute.RunAt
Imports Syn.Nucleo.RecursosComercioExterior

Public Class VRUVA000_STDCommercialInvoice
    Inherits ValidationRUVA

    Sub New()

        _validationtarget = Nothing

        _cubeslice = ICubeController.CubeSlices.A22

    End Sub

    Protected Overrides Function LoadInitialStockElement() As ValidatorReport

        '#1
        If [Set](ANS1, CA_DESTINO_ORIGEN, breakOnEmpty_:=True) Then Return _report

        '#2
        If [Set](ANS1, CA_ADUANA_ENTRADA_SALIDA, breakOnEmpty_:=True) Then Return _report

        '#3
        If [Set](ANS1, CA_MEDIO_TRANSPORTE, breakOnEmpty_:=True) Then Return _report

        '#4
        [Set](ANS1, CA_PESO_BRUTO, breakOnEmpty_:=False, runAt_:=ROOMCUBE, roomNameExt_:="_CASE2")

        '#5
        [Set](ANS1, CA_MARCAS_NUMEROS_TOTAL_BULTOS, breakOnEmpty_:=False)

        '#6
        If [Set](ANS1, CA_CLAVE_SAD, breakOnEmpty_:=True) Then Return _report

        '#7
        If [Set](ANS1, CA_MEDIO_TRANSPORTE_ARRIBO, breakOnEmpty_:=True) Then Return _report

        '#8
        If [Set](ANS1, CA_MEDIO_TRANSPORTE_SALIDA, breakOnEmpty_:=True) Then Return _report

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

        '#10
        [Set](ANS3, CA_CURP_IOE, breakOnEmpty_:=False)



        '#11
        If [Set](ANS3, CA_RAZON_SOCIAL_IOE, breakOnEmpty_:=True) Then Return _report

        '#12
        If [Set](ANS3, CA_DOMICILIO_IOE, breakOnEmpty_:=True) Then Return _report

        '#13
        If [Set](ANS1, CA_TIPO_OPERACION, breakOnEmpty_:=True) Then Return _report

        '#14
        If [Set](ANS1, CA_CVE_PEDIMENTO, breakOnEmpty_:=True) Then Return _report

        '#15
        If [Set](ANS1, CA_REGIMEN, breakOnEmpty_:=True) Then Return _report

        '#16
        [Set](ANS10, CA_ID_FISCAL_PROVEEDOR, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True, roomNameExt_:="_CASE2")

        '#17
        [Set](ANS10, CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_POC, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#18
        [Set](ANS10, CA_PAIS_POC, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#18
        [Set](ANS10, CA_DOMICILIO_POC, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#19
        [Set](ANS10, CA_VINCULACION, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#20
        [Set](ANS13, CA_CFDI_FACTURA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#21
        [Set](ANS13, CA_FECHA_FACTURA, breakOnEmpty_:=False, runat_:=ROOMCUBE, dateCurrent_:=Now, recurring_:=True)

        '#22
        [Set](ANS13, CA_CVE_MONEDA_FACTURA, breakOnEmpty_:=False, runat_:=ROOMCUBE, isPresentationValue_:=True, isReverse_:=True, recurring_:=True, length_:=3)

        '#23
        [Set](ANS13, CA_MONTO_MONEDA_FACTURA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#24
        [Set](ANS13, CA_NUMERO_ACUSE_DE_VALOR, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#25
        [Set](ANS13, CP_APLICA_ENAJENACION, breakOnEmpty_:=False, recurring_:=True)

        '#25
        [Set](ANS13, CA_INCOTERM, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        ''#26
        '[Set](DocumentoElectronico_,
        '                     ANS11,
        '                     CA_NOMBRE_RAZON_SOCIAL_DESTINATARIO,
        '                     False,
        '                     fullsection_:=True)

        ''#32
        '[Set](DocumentoElectronico_,
        '                     ANS11,
        '                     CA_ID_FISCAL_DESTINATARIO,
        '                     False, fullsection_:=True)

        ''#33
        '[Set](DocumentoElectronico_,
        '                     ANS11,
        '                     CA_DOMICILIO_DESTINATARIO,
        '                     False, fullsection_:=True)

        ''#34
        'If [Set](DocumentoElectronico_,
        '                        ANS44,
        '                        CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_AA,
        '                        True,
        '                        runAt_:=LOCALCONDITIONS) Then

        '    Return _report

        'End If

        '#26
        If [Set](ANS44, CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_AA, breakOnEmpty_:=True) Then Return _report

        '#27
        If [Set](ANS44, CA_RFC_AA, breakOnEmpty_:=True) Then Return _report

        '#28
        If [Set](ANS44, CA_CURP_AA_REPRESENTANTE_LEGAL, breakOnEmpty_:=True) Then Return _report

        '#29
        [Set](ANS44, CA_NOMBRE_MANDATARIO_REPRESENTANTE_AA, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#29
        [Set](ANS44, CA_TIPO_FIGURA, breakOnEmpty_:=False)

        '#30
        [Set](ANS44, CA_RFC_MANDATARIO_AA_REPRESENTANTE_ALMACEN, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#31
        [Set](ANS44, CA_CURP_MANDATARIO_AA_REPRESENTANTE_ALMACEN, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#32
        If [Set](ANS44, CA_PATENTE, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#33
        If [Set](ANS44, CA_EFIRMA, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#34
        If [Set](ANS44, CA_CERTIFICADO_FIRMA, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#35
        If [Set](ANS43, CA_FIN_PEDIMENTO, breakOnEmpty_:=True) Then Return _report

        '#36
        If [Set](ANS14, CA_FECHA_PAGO, breakOnEmpty_:=True, runat_:=ROOMCUBE, dateCurrent_:=Now) Then Return _report

        '#37
        [Set](ANS18, CA_CVE_IDENTIFICADOR, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#38
        [Set](ANS18, CA_COMPLEMENTO_1, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#39
        [Set](ANS18, CA_COMPLEMENTO_2, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#40
        [Set](ANS18, CA_COMPLEMENTO_3, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#41
        If [Set](ANS1, CA_NUMERO_PEDIMENTO_COMPLETO, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#41
        [Set](ANS1, CA_ANIO_VALIDACION, breakOnEmpty_:=False)


        '#42
        If [Set](ANS14, CA_FECHA_ENTRADA, breakOnEmpty_:=True, runat_:=ROOMCUBE, dateCurrent_:=Now) Then Return _report

        '#43
        [Set](ANS14, CA_FECHA_EXTRACCION, breakOnEmpty_:=False, runat_:=ROOMCUBE, dateCurrent_:=Now, roomNameExt_:="_CASE2")
        '#44
        [Set](ANS14, CA_FECHA_ORIGINAL, breakOnEmpty_:=False, runat_:=ROOMCUBE, dateCurrent_:=Now)

        '#45
        [Set](ANS12, CA_ID_TRANSPORTE, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#46
        [Set](ANS12, CA_CVE_PAIS_TRANSPORTE, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#47
        [Set](ANS15, CA_NUMERO_CANDADO, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#48
        [Set](ANS16, CA_GUIA_MANIFIESTO_BL, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#49
        [Set](ANS16, CA_MASTER_HOUSE, breakOnEmpty_:=False, runat_:=ROOMCUBE, meanBoolean_:=New List(Of String) From {"M", "H"}, recurring_:=True)

        '#50
        [Set](ANS17, CA_NUMERO_CONTENEDOR_FERROCARRIL_NUMERO_ECONOMICO, breakOnEmpty_:=False)

        '#51
        [Set](ANS17, CA_CVE_TIPO_CONTENEDOR, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#52
        [Set](ANS23, CA_OBSERVACIONES_PEDIMENTO, breakOnEmpty_:=False)

        '#53
        [Set](ANS20, CA_FECHA_PEDIMENTO_ORIGINAL, breakOnEmpty_:=False, runat_:=ROOMCUBE, dateCurrent_:=Now)

        '#54
        [Set](ANS20, CA_CVE_PEDIMENTO_ORIGINAL, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#55
        If [Set](ANS1, CA_TIPO_CAMBIO, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '_coincontroller = New ControladorMonedas

        'Dim fecha_ = Date.Parse(If(_fieldvalues("S14.CA_FECHA_ENTRADA.0") = "",
        '                            Date.Now.ToString,
        '                            _fieldvalues("S14.CA_FECHA_ENTRADA.0"))).
        '                            AddDays(-1)

        'Dim factorchanges_ = _coincontroller.ObtenerFactorTipodeCambio("USD",
        '                                                               fecha_.ToString("yyyy-MM-dd")).ObjectReturned

        'Dim change_ = If(factorchanges_(1) Is Nothing, "0", factorchanges_(1).tipocambio.ToString)

        ''#55
        '[Set](external_:=New List(Of String) From {"CA_TIPO_CAMBIO_CURRENT", change_})

        ''#56
        '[Set](ANS13,
        '                         CA_FACTOR_MONEDA,
        '                         breakOnEmpty_:=False,
        '                         runAt_:=ROOMCUBE,
        '                         recurring_:=True)

        'Dim index_ = 0

        'For Each factor_ In _borderfields.Where(Function(ch) ch.Key.fieldpedimento.ToString = CA_FECHA_FACTURA.ToString).
        '                                  SelectMany(Function(s) s.Value).
        '                                  ToList()

        '    factorchanges_ = _coincontroller.ObtenerFactorTipodeCambio(_fieldvalues("S13.CA_CVE_MONEDA_FACTURA." & index_), fecha_).ObjectReturned

        '    '#56
        '    [Set](external_:=New List(Of String) From {"CA_FACTOR_MONEDA_CURRENT." & index_, factorchanges_(0).factor.ToString})

        '    index_ += 1

        'Next

        _coincontroller = New ControladorMonedas

        Dim fecha_ = Date.Parse(If(_fieldvalues("S14.CA_FECHA_ENTRADA.0") = "",
                                Date.Now.ToString,
                                _fieldvalues("S14.CA_FECHA_ENTRADA.0"))).AddDays(-1)

        Dim tagWatcher_ = _coincontroller.ObtenerFactorTipodeCambio("USD",
                                                                       fecha_.ToString("yyyy-MM-dd"))
        Dim factorchanges_

        If tagWatcher_.Status = TypeStatus.Ok Then

            factorchanges_ = tagWatcher_.ObjectReturned

            Dim change_ = If(factorchanges_(1) Is Nothing,
                         "0",
                         factorchanges_(1).tipocambio.ToString)

            '#62
            [Set](external_:=New List(Of String) From {"CA_TIPO_CAMBIO_CURRENT",
                                                                          change_})

            '#63
            [Set](ANS13, CA_FACTOR_MONEDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        Else

            Return _report

        End If

        Dim index_ = 0

        For Each factor_ In _borderfields.
                            Keys.
                            Where(Function(e) e.fieldpedimento.ToString = CA_FECHA_FACTURA.ToString)

            tagWatcher_ = _coincontroller.
                             ObtenerFactorTipodeCambio(_fieldvalues("S13.CA_CVE_MONEDA_FACTURA." & index_),
                                                       fecha_)

            If tagWatcher_.Status = TypeStatus.Ok Then


                factorchanges_ = _coincontroller.
                             ObtenerFactorTipodeCambio(_fieldvalues("S13.CA_CVE_MONEDA_FACTURA." & index_),
                                                       fecha_).
                             ObjectReturned

                '#63
                [Set](external_:=New List(Of String) From {"CA_FACTOR_MONEDA_CURRENT." & index_, factorchanges_(0).factor.ToString})

            Else

                Return _report

            End If


            index_ = index_ + 1

        Next

        '#57
        [Set](ANS20, CA_NUMERO_PEDIMENTO_ORIGINAL_COMPLETO, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#58
        [Set](ANS13, CA_MONTO_USD, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#59
        If [Set](ANS3, CA_SEGUROS, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {">=", "0"}) Then Return _report

        '#60
        If [Set](ANS3, CA_FLETES, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {">=", "0"}) Then Return _report

        '#61
        If [Set](ANS3, CA_EMBALAJES, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {">=", "0"}) Then Return _report

        '#62
        If [Set](ANS3, CA_OTROS_INCREMENTABLES, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {">=", "0"}) Then Return _report

        '#63
        If [Set](ANS3, CA_TRANSPORTE_DECREMENTABLES, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {">=", "0"}) Then Return _report

        '#64
        If [Set](ANS3, CA_SEGURO_DECREMENTABLES, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {">=", "0"}) Then Return _report

        '#65
        If [Set](ANS3, CA_CARGA_DECREMENTABLES, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {">=", "0"}) Then Return _report

        '#66
        If [Set](ANS3, CA_DESCARGA_DECREMENTABLES, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {">=", "0"}) Then Return _report

        '#67
        If [Set](ANS3, CA_OTROS_DECREMENTABLES, breakOnEmpty_:=True, runAt_:=LOCALCONDITIONS, conditions_:=New List(Of String) From {">=", "0"}) Then Return _report

        '#68
        If [Set](ANS1, CA_VALOR_ADUANA, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#69
        If [Set](ANS24, CA_SECUENCIA_PARTIDA, breakOnEmpty_:=True, recurring_:=True) Then Return _report

        '#70
        If [Set](ANS24, CA_FRACCION_ARANCELARIA_PARTIDA, breakOnEmpty_:=True, recurring_:=True) Then Return _report

        '#71
        If [Set](ANS24, CA_NICO_PARTIDA, breakOnEmpty_:=True, recurring_:=True) Then Return _report

        '#72
        If [Set](ANS24, CamposProveedorOperativo.CA_CVE_VINCULACION, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#73
        If [Set](ANS24, CA_CVE_METODO_VALORACION_PARTIDA, breakOnEmpty_:=True, runat_:=ROOMCUBE, recurring_:=True) Then Return _report

        '#74
        If [Set](ANS24, CA_CVE_UMC_PARTIDA, breakOnEmpty_:=True, recurring_:=True) Then Return _report

        '#75
        If [Set](ANS24, CA_CANTIDAD_UMC_PARTIDA, breakOnEmpty_:=True, runat_:=ROOMCUBE, recurring_:=True) Then Return _report

        '#76
        If [Set](ANS24, CA_CVE_UMT_PARTIDA, breakOnEmpty_:=True, recurring_:=True) Then Return _report

        '#77
        If [Set](ANS24, CA_CANTIDAD_UMT_PARTIDA, breakOnEmpty_:=True, runat_:=ROOMCUBE, recurring_:=True) Then Return _report

        '#78
        If [Set](ANS24, CA_CVE_PAIS_VENDEDOR_PARTIDA, breakOnEmpty_:=True, runat_:=ROOMCUBE, recurring_:=True) Then Return _report

        '#79
        If [Set](ANS24, CA_CVE_PAIS_ORIGEN_PARTIDA, breakOnEmpty_:=True, recurring_:=True) Then Return _report

        '#80
        If [Set](ANS24, CA_DESCRIPCION_MERCANCIA_PARTIDA, breakOnEmpty_:=True, recurring_:=True) Then Return _report

        '#81
        If [Set](ANS24, CA_PRECIO_PAGADO_PARTIDA, breakOnEmpty_:=True, runat_:=ROOMCUBE, recurring_:=True) Then Return _report

        '#82
        [Set](ANS29, CA_CVE_CONTRIBUCION_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#83
        [Set](ANS29, CA_CVE_TIPO_TASA_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#84
        [Set](ANS29, CA_TASA_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#85
        [Set](ANS30, CA_FORMA_PAGO_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#86
        If [Set](ANS1, CA_VALOR_DOLARES, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#87
        If [Set](ANS24, CA_VALOR_DOLAR_PARTIDA, breakOnEmpty_:=True, recurring_:=True) Then Return _report

        '#87
        If [Set](ANS24, CA_VALOR_COMERCIAL_PARTIDA, breakOnEmpty_:=True, recurring_:=True) Then Return _report

        '#87
        If [Set](ANS1, CA_PRECIO_PAGADO_VALOR_COMERCIAL, breakOnEmpty_:=True, runAt_:=ROOMCUBE) Then Return _report

        '#88
        [Set](ANS3, CA_VALOR_SEGUROS, breakOnEmpty_:=False)

        '#89
        If [Set](ANS24, CA_VALOR_ADUANA_PARTIDA, breakOnEmpty_:=True, runat_:=LOCALCONDITIONS, recurring_:=True, conditions_:=New List(Of String) From {"", "", "", "0"}) Then Return _report

        '#90
        If [Set](ANS24, CA_PRECIO_UNITARIO_PARTIDA, breakOnEmpty_:=True, recurring_:=True) Then Return _report

        '#91
        [Set](ANS24, CA_MARCA_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#92
        [Set](ANS24, CA_MODELO_PARTIDA, breakOnEmpty_:=False, recurring_:=True)

        '#93
        [Set](ANS24, CA_CODIGO_PRODUCTO_PARTIDA, breakOnEmpty_:=False, recurring_:=True)

        '#94
        [Set](ANS25, CA_VINCULACION_NUMERO_SERIE_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#95
        [Set](ANS25, CA_KILOMETRAJE_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#96
        [Set](ANS26, CA_CVE_PERMISO, breakOnEmpty_:=False)

        '#97
        [Set](ANS26, CA_NUMERO_PERMISO, breakOnEmpty_:=False)

        '#98
        [Set](ANS26, CA_FIRMA_ELECTRONICA_PERMISO, breakOnEmpty_:=False)

        '#99
        [Set](ANS26, CA_VALOR_USD_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)


        '#100
        [Set](ANS26, CA_CANTIDAD_UMT_UMC, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#101
        [Set](ANS27, CA_CVE_IDENTIFICADOR_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#102
        [Set](ANS27, CA_COMPLEMENTO_1_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#103
        [Set](ANS27, CA_COMPLEMENTO_2_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#104
        [Set](ANS27, CA_COMPLEMENTO_3_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#105
        [Set](ANS36, CA_OBSERVACIONES_PARTIDA, breakOnEmpty_:=False, recurring_:=True)

        '#106
        [Set](ANS28, CA_CVE_TIPO_GARANTIA_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#107
        [Set](ANS28, CA_INSTITUCION_EMISORA_GARANTIA_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#108
        [Set](ANS28, CA_FECHA_EXPIRACION_CONSTANCIA_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, dateCurrent_:=Now, recurring_:=True)

        '#109
        [Set](ANS28, CA_NUMERO_CUENTA_GARANTIA_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#110
        [Set](ANS28, CA_FOLIO_CONSTANCIA_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#111
        [Set](ANS28, CA_MONTO_TOTAL_CONSTANCIA_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#112
        [Set](ANS28, CA_PRECIO_ESTIMADO_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)


        '#113
        [Set](ANS28, CA_CANTIDAD_UMT_PRECIO_ESTIMADO_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#114
        [Set](ANS30, CA_IMPORTE_PARTIDA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#115
        If [Set](ANS43, CA_NUMERO_TOTAL_PARTIDAS, breakOnEmpty_:=True) Then Return _report

        '#116
        [Set](ANS6, CA_CONTRIBUCION, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#117
        [Set](ANS6, CA_CVE_TIPO_TASA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#118
        [Set](ANS6, CA_TASA, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#119
        [Set](ANS55, CA_CONCEPTO, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#120
        [Set](ANS55, CA_FORMA_PAGO, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#121
        [Set](ANS55, CA_IMPORTE, breakOnEmpty_:=False, runat_:=ROOMCUBE, recurring_:=True)

        '#122
        [Set](ANS7, CA_EFECTIVO, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#123
        [Set](ANS7, CA_OTROS, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#124
        [Set](ANS7, CA_TOTAL, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#125
        If [Set](ANS1, CA_CVE_PREVALIDADOR, breakOnEmpty_:=True) Then Return _report

        '#126 
        [Set](ANS9, CA_PATENTE, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#117 
        [Set](ANS9, CA_NUMERO_PEDIMENTO, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#128
        [Set](ANS9, CA_CLAVE_SAD, breakOnEmpty_:=False, runAt_:=ROOMCUBE, roomNameExt_:="_LINEA_CAPTURA")

        '#129
        [Set](ANS9, CA_PAGO_ELECTRONICO, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#130
        [Set](ANS9, CA_NOMBRE_INSTITUCION_BANCARIA, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#131
        [Set](ANS9, CA_LINEA_CAPTURA, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#132
        [Set](ANS9, CA_EFECTIVO, breakOnEmpty_:=False, runAt_:=ROOMCUBE, roomNameExt_:="_LINEA_CAPTURA")

        '#133
        [Set](ANS9, CA_FECHA_PAGO, breakOnEmpty_:=False, runat_:=ROOMCUBE, dateCurrent_:=Now, roomNameExt_:="_LINEA_CAPTURA")

        '#134
        [Set](ANS9, CA_NUMERO_OPERACION_BANCARIA, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#135
        [Set](ANS9, CA_NUMERO_TRANSACCION_SAT, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#136
        [Set](ANS9, CA_MEDIO_PRESENTACION, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#137
        [Set](ANS9, CA_MEDIO_RECEPCION_COBRO, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#138
        If [Set](ANS1, CA_ACUSE_ELECTRONICO_VALIDACION, breakOnEmpty_:=True) Then Return _report

        '#139
        If [Set](ANS1, CA_CODIGO_BARRAS, breakOnEmpty_:=True) Then Return _report

        '#140
        [Set](ANS9, CA_DEPOSITO_REFERENCIADO, breakOnEmpty_:=False, runAt_:=ROOMCUBE)

        '#141
        [Set](ANS1, CA_FECHA_VALIDACION, breakOnEmpty_:=False, runat_:=IValidationRoute.RunAt.LOCALCONDITIONS, dateCurrent_:=Now)

        '#142
        [Set](ANS20, CA_ANIO_VALIDACION_ORIGINAL, breakOnEmpty_:=False)

        '#143
        [Set](ANS20, CA_ADUANA_DESPACHO_ORIGINAL_2, False)

        '#144
        [Set](ANS20, CA_PATENTE_ORIGINAL, breakOnEmpty_:=False)

        '#145
        [Set](ANS20, CA_NUMERO_PEDIMENTO_ORIGINAL_7_DIGITOS, breakOnEmpty_:=False)

        '#146
        [Set](ANS1, CA_CERTIFICACION, breakOnEmpty_:=False)

        Return _report

    End Function


End Class
