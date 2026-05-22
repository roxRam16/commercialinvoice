Imports Cube
Imports Cube.Validators
Imports Rec.Globals.Controllers
Imports Syn.Documento
Imports Syn.Nucleo.RecursosComercioExterior

Public Class VRUVA004_STDA22CustomsDeclarationDoc
    Inherits ValidationRUVA

    Sub New()

        _validationtarget = IValidationRoute.ValidationRoutes.RUVA4

        _cubeslice = ICubeController.CubeSlices.A22

    End Sub


    Protected Overrides Function LoadInitialStockElement() As ValidatorReport

        _report = New ValidatorReport

        _borderfields = New Dictionary(Of MultiKeyItem, List(Of CheckedField))

        _fieldvalues = New Dictionary(Of String, String)

        ''#1
        'If SetValueStockElement(SeccionesPedimento.ANS1,
        '                        CamposPedimento.CA_DESTINO_ORIGEN,
        '                        True,
        '                        validate_:=False,
        '                        conditions_:=New List(Of String) From {"", ""}) Then

        '    Return _report

        'End If

        ''#2
        'If SetValueStockElement(SeccionesPedimento.ANS1,
        '                        CamposPedimento.CA_ADUANA_ENTRADA_SALIDA,
        '                        True,
        '                        validate_:=False,
        '                        conditions_:=New List(Of String) From {"", ""}) Then

        '    Return _report

        'End If

        ''#3
        'If SetValueStockElement(SeccionesPedimento.ANS1,
        '                        CamposPedimento.CA_MEDIO_TRANSPORTE,
        '                        True,
        '                        validate_:=False,
        '                        conditions_:=New List(Of String) From {"", ""}) Then

        '    Return _report

        'End If

        ''#4
        'SetValueStockElement(SeccionesPedimento.ANS1,
        '                     CamposPedimento.CA_PESO_BRUTO,
        '                     False,
        '                     requiredFields_:=New List(Of String) From
        '                                       {"S1.CA_PESO_BRUTO"})

        ''#5
        'SetValueStockElement(SeccionesPedimento.ANS1,
        '                     CamposPedimento.CA_MARCAS_NUMEROS_TOTAL_BULTOS,
        '                     False,
        '                     validate_:=False)

        ''#6
        'If SetValueStockElement(SeccionesPedimento.ANS1,
        '                        CamposPedimento.CA_CLAVE_SAD,
        '                        True,
        '                        validate_:=False,
        '                        conditions_:=New List(Of String) From {"", ""}) Then

        '    Return _report

        'End If

        ''#7
        'If SetValueStockElement(SeccionesPedimento.ANS1,
        '                        CamposPedimento.CA_MEDIO_TRANSPORTE_ARRIBO,
        '                        True,
        '                        validate_:=False,
        '                        conditions_:=New List(Of String) From {"", ""}) Then

        '    Return _report

        'End If

        ''#8
        'If SetValueStockElement(SeccionesPedimento.ANS1,
        '                        CamposPedimento.CA_MEDIO_TRANSPORTE_SALIDA,
        '                        True,
        '                        validate_:=False,
        '                        conditions_:=New List(Of String) From {"", ""}) Then

        '    Return _report

        'End If

        '''#9
        ''SetValueStockElement(DocumentoElectronico_,
        ''                     SeccionesPedimento.ANS3,
        ''                     CamposPedimento.CA_RFC_IOE,
        ''                     False)

        '''#10
        ''SetValueStockElement(DocumentoElectronico_,
        ''                     SeccionesPedimento.ANS3,
        ''                     CamposPedimento.CA_CURP_IOE,
        ''                     False)

        ''#9 
        'SetValueStockElement(SeccionesPedimento.ANS3,
        '                     CamposPedimento.CA_RFC_IOE,
        '                     False,
        '                     roomNameExt_:="_MIXTO")

        ''#10
        'SetValueStockElement(SeccionesPedimento.ANS3,
        '                     CamposPedimento.CA_CURP_IOE,
        '                     False,
        '                     validate_:=False)



        ''#11
        'If SetValueStockElement(SeccionesPedimento.ANS3,
        '                        CamposPedimento.CA_RAZON_SOCIAL_IOE,
        '                        True,
        '                        validate_:=False,
        '                        conditions_:=New List(Of String) From {"", ""}) Then

        '    Return _report

        'End If

        ''#12
        'If SetValueStockElement(SeccionesPedimento.ANS3,
        '                        CamposPedimento.CA_DOMICILIO_IOE,
        '                        True,
        '                        validate_:=False,
        '                        conditions_:=New List(Of String) From {"", ""}) Then

        '    Return _report

        'End If

        ''#13
        'If SetValueStockElement(SeccionesPedimento.ANS1,
        '                        CamposPedimento.CA_TIPO_OPERACION,
        '                        True,
        '                        validate_:=False,
        '                        conditions_:=New List(Of String) From {"", ""}) Then

        '    Return _report

        'End If

        ''#14
        'If SetValueStockElement(SeccionesPedimento.ANS1,
        '                        CamposPedimento.CA_CVE_PEDIMENTO,
        '                        True,
        '                        validate_:=False,
        '                        conditions_:=New List(Of String) From {"", ""}) Then

        '    Return _report

        'End If

        ''#15
        'If SetValueStockElement(SeccionesPedimento.ANS1,
        '                        CamposPedimento.CA_REGIMEN,
        '                        True,
        '                        validate_:=False,
        '                        conditions_:=New List(Of String) From {"", ""}) Then

        '    Return _report

        'End If

        ''#16
        'If SetValueStockElement(SeccionesPedimento.ANS1,
        '                        CamposPedimento.CA_VALOR_ADUANA,
        '                        True,
        '                        conditions_:=New List(Of String) From {"0"},
        '                        validate_:=False) Then

        '    Return _report

        'End If

        ''#17
        'If SetValueStockElement(SeccionesPedimento.ANS1,
        '                        CamposPedimento.CA_VALOR_SEGUROS,
        '                        True,
        '                        conditions_:=New List(Of String) From {"0"},
        '                        validate_:=False) Then

        '    Return _report

        'End If

        ''#18
        'If SetValueStockElement(SeccionesPedimento.ANS1,
        '                        CamposPedimento.CA_SEGUROS,
        '                        True,
        '                        conditions_:=New List(Of String) From {"0"},
        '                        validate_:=False) Then

        '    Return _report

        'End If

        ''#19
        'If SetValueStockElement(SeccionesPedimento.ANS1,
        '                        CamposPedimento.CA_FLETES,
        '                        True,
        '                        conditions_:=New List(Of String) From {"0"},
        '                        validate_:=False) Then

        '    Return _report

        'End If

        ''#20
        'If SetValueStockElement(SeccionesPedimento.ANS1,
        '                        CamposPedimento.CA_EMBALAJES,
        '                        True,
        '                        conditions_:=New List(Of String) From {"0"},
        '                        validate_:=False) Then

        '    Return _report

        'End If

        ''#21
        'If SetValueStockElement(SeccionesPedimento.ANS1,
        '                        CamposPedimento.CA_OTROS_INCREMENTABLES,
        '                        True,
        '                        conditions_:=New List(Of String) From {"0"},
        '                        validate_:=False) Then

        '    Return _report

        'End If

        ''#22
        'SetValueStockElement(SeccionesPedimento.ANS10,
        '                     CamposPedimento.CA_ID_FISCAL_PROVEEDOR,
        '                     False,
        '                     roomNameExt_:="_NORMAL_EXPORTACION",
        '                     fullSection_:=True)

        ''#23
        'SetValueStockElement(SeccionesPedimento.ANS10,
        '                     CamposPedimento.CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_POC,
        '                     False,
        '                     fullSection_:=True)

        ''#24
        'SetValueStockElement(SeccionesPedimento.ANS10,
        '                     CamposPedimento.CA_DOMICILIO_POC,
        '                     False,
        '                     fullSection_:=True)

        ''#25
        'SetValueStockElement(SeccionesPedimento.ANS13,
        '                     CamposPedimento.CA_CFDI_FACTURA,
        '                     False, fullSection_:=True)

        ''#26
        'SetValueStockElement(SeccionesPedimento.ANS13,
        '                     CamposPedimento.CA_FECHA_FACTURA,
        '                     False,
        '                     dateType_:=True,
        '                     fullSection_:=True)

        ''#27
        'SetValueStockElement(SeccionesPedimento.ANS13,
        '                     CamposPedimento.CA_CVE_MONEDA_FACTURA,
        '                     False,
        '                     valorPresentacion_:=True,
        '                     length_:=3,
        '                     reverse_:=True,
        '                     fullSection_:=True)

        ''#28
        'SetValueStockElement(SeccionesPedimento.ANS13,
        '                     CamposPedimento.CA_MONTO_MONEDA_FACTURA,
        '                     False,
        '                     fullSection_:=True)

        ''#29
        'SetValueStockElement(SeccionesPedimento.ANS13,
        '                     CamposPedimento.CA_NUMERO_ACUSE_DE_VALOR,
        '                     False,
        '                     roomNameExt_:="_NORMAL_EXPORTACION",
        '                     fullSection_:=True)

        ''#29
        'SetValueStockElement(SeccionesPedimento.ANS13,
        '                     CamposPedimento.CP_APLICA_ENAJENACION,
        '                     False,
        '                    validate_:=False,
        '                    fullSection_:=True)

        ''#30
        'SetValueStockElement(SeccionesPedimento.ANS13,
        '                     CamposPedimento.CA_INCOTERM,
        '                     False,
        '                     fullSection_:=True)

        ''#31
        'SetValueStockElement(SeccionesPedimento.ANS11,
        '                     CamposPedimento.CA_NOMBRE_RAZON_SOCIAL_DESTINATARIO,
        '                     False,
        '                     fullSection_:=True)

        ''#32
        'SetValueStockElement(SeccionesPedimento.ANS11,
        '                     CamposPedimento.CA_ID_FISCAL_DESTINATARIO,
        '                     False, fullSection_:=True)

        ''#33
        'SetValueStockElement(SeccionesPedimento.ANS11,
        '                     CamposPedimento.CA_DOMICILIO_DESTINATARIO,
        '                     False, fullSection_:=True)

        ''#34
        'If SetValueStockElement(SeccionesPedimento.ANS44,
        '                        CamposPedimento.CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_AA,
        '                        True,
        '                        validate_:=False) Then

        '    Return _report

        'End If

        ''#35
        'If SetValueStockElement(SeccionesPedimento.ANS44,
        '                        CamposPedimento.CA_RFC_AA,
        '                        True,
        '                        validate_:=False) Then

        '    Return _report

        'End If

        ''#36
        'If SetValueStockElement(SeccionesPedimento.ANS44,
        '                        CamposPedimento.CA_CURP_AA_REPRESENTANTE_LEGAL,
        '                        True,
        '                        validate_:=False) Then

        '    Return _report

        'End If

        ''#37
        'SetValueStockElement(SeccionesPedimento.ANS44,
        '                     CamposPedimento.CA_NOMBRE_MANDATARIO_REPRESENTANTE_AA,
        '                     False)

        ''#37
        'SetValueStockElement(SeccionesPedimento.ANS44,
        '                     CamposPedimento.CA_TIPO_FIGURA,
        '                     False, validate_:=False)



        ''#38
        'SetValueStockElement(SeccionesPedimento.ANS44,
        '                     CamposPedimento.CA_RFC_MANDATARIO_AA_REPRESENTANTE_ALMACEN,
        '                     False)

        ''#39
        'SetValueStockElement(SeccionesPedimento.ANS44,
        '                     CamposPedimento.CA_CURP_MANDATARIO_AA_REPRESENTANTE_ALMACEN,
        '                     False)

        ''#40
        'If SetValueStockElement(SeccionesPedimento.ANS44,
        '                        CamposPedimento.CA_PATENTE,
        '                        True) Then

        '    Return _report

        'End If

        ''#41
        'If SetValueStockElement(SeccionesPedimento.ANS44,
        '                        CamposPedimento.CA_EFIRMA,
        '                        True) Then

        '    Return _report

        'End If

        ''#42
        'If SetValueStockElement(SeccionesPedimento.ANS44,
        '                        CamposPedimento.CA_CERTIFICADO_FIRMA,
        '                        True) Then

        '    Return _report

        'End If

        ''#43
        'If SetValueStockElement(SeccionesPedimento.ANS43,
        '                        CamposPedimento.CA_FIN_PEDIMENTO,
        '                        True,
        '                        validate_:=False) Then

        '    Return _report

        'End If

        ''#44
        'If SetValueStockElement(SeccionesPedimento.ANS14,
        '                        CamposPedimento.CA_FECHA_PAGO,
        '                        True,
        '                        dateType_:=True) Then

        '    Return _report

        'End If

        ''#45
        'SetValueStockElement(SeccionesPedimento.ANS18,
        '                     CamposPedimento.CA_CVE_IDENTIFICADOR,
        '                     False,
        '                     fullSection_:=True)

        ''#46
        'SetValueStockElement(SeccionesPedimento.ANS18,
        '                     CamposPedimento.CA_COMPLEMENTO_1,
        '                     False,
        '                     fullSection_:=True)

        ''#47
        'SetValueStockElement(SeccionesPedimento.ANS18,
        '                     CamposPedimento.CA_COMPLEMENTO_2,
        '                     False,
        '                     fullSection_:=True)

        ''#48
        'SetValueStockElement(SeccionesPedimento.ANS18,
        '                     CamposPedimento.CA_COMPLEMENTO_3,
        '                     False,
        '                     fullSection_:=True)

        ''#49
        'If SetValueStockElement(SeccionesPedimento.ANS1,
        '                        CamposPedimento.CA_NUMERO_PEDIMENTO_COMPLETO,
        '                        True) Then

        '    Return _report

        'End If

        ''#49
        'SetValueStockElement(SeccionesPedimento.ANS1,
        '                     CamposPedimento.CA_ANIO_VALIDACION,
        '                     False,
        '                     validate_:=False)


        ''#50
        'If SetValueStockElement(SeccionesPedimento.ANS14,
        '                        CamposPedimento.CA_FECHA_PRESENTACION,
        '                        True,
        '                        dateType_:=True) Then

        '    Return _report

        'End If

        ''#51
        'SetValueStockElement(SeccionesPedimento.ANS14,
        '                     CamposPedimento.CA_FECHA_EXTRACCION,
        '                     False,
        '                     dateType_:=True)

        ''#52
        'SetValueStockElement(SeccionesPedimento.ANS12,
        '                     CamposPedimento.CA_ID_TRANSPORTE,
        '                     False,
        '                     fullSection_:=True,
        '                      validate_:=False)

        ''#53
        'SetValueStockElement(SeccionesPedimento.ANS12,
        '                     CamposPedimento.CA_CVE_PAIS_TRANSPORTE,
        '                     False,
        '                     fullSection_:=True)

        ''#54
        'SetValueStockElement(SeccionesPedimento.ANS15,
        '                     CamposPedimento.CA_NUMERO_CANDADO,
        '                     False,
        '                     fullSection_:=True)

        ''#55
        'SetValueStockElement(SeccionesPedimento.ANS16,
        '                     CamposPedimento.CA_GUIA_MANIFIESTO_BL,
        '                     False,
        '                     roomNameExt_:="_NORMAL_EXPORTACION")

        ''#56
        'SetValueStockElement(SeccionesPedimento.ANS16,
        '                     CamposPedimento.CA_MASTER_HOUSE,
        '                     False,
        '                     fullSection_:=True,
        '                     meanBoolean:=New List(Of String) From {"M", "H"})

        ''#57
        'SetValueStockElement(SeccionesPedimento.ANS17,
        '                     CamposPedimento.CA_NUMERO_CONTENEDOR_FERROCARRIL_NUMERO_ECONOMICO,
        '                     False,
        '                     validate_:=False)

        ''#58
        'SetValueStockElement(SeccionesPedimento.ANS17,
        '                     CamposPedimento.CA_CVE_TIPO_CONTENEDOR,
        '                     False)

        ''#59
        'SetValueStockElement(SeccionesPedimento.ANS23,
        '                     CamposPedimento.CA_OBSERVACIONES_PEDIMENTO,
        '                     False,
        '                     validate_:=False)

        ''#60
        'SetValueStockElement(SeccionesPedimento.ANS20,
        '                     CamposPedimento.CA_FECHA_PEDIMENTO_ORIGINAL,
        '                     False,
        '                     dateType_:=True,
        '                     roomNameExt_:="_NORMAL_EXPORTACION")

        ''#61
        'SetValueStockElement(SeccionesPedimento.ANS20,
        '                     CamposPedimento.CA_CVE_PEDIMENTO_ORIGINAL,
        '                     False)

        ''#62
        'If SetValueStockElement(SeccionesPedimento.ANS1,
        '                        CamposPedimento.CA_TIPO_CAMBIO,
        '                        True) Then

        '    Return _report

        'End If

        '_coincontroller = New ControladorMonedas

        'Dim fecha_ = Date.Parse(If(_fieldvalues("S14.CA_FECHA_PRESENTACION.0") = "", Date.Now.ToString, _fieldvalues("S14.CA_FECHA_PRESENTACION.0"))).AddDays(-1)

        'Dim factorchanges_ = _coincontroller.ObtenerFactorTipodeCambio("USD", fecha_.ToString("yyyy-MM-dd")).ObjectReturned

        'Dim change_ = If(factorchanges_(1) Is Nothing, "0", factorchanges_(1).tipocambio.ToString)

        ''#62
        'SetValueStockElement(SeccionesPedimento.ANS1,
        '                     CamposPedimento.CA_TIPO_CAMBIO,
        '                     False,
        '                     external_:=New List(Of String) From {"CA_TIPO_CAMBIO_CURRENT", change_})

        ''#63
        'SetValueStockElement(SeccionesPedimento.ANS13,
        '                     CamposPedimento.CA_FACTOR_MONEDA,
        '                     False,
        '                     fullSection_:=True)

        'Dim index_ = 0

        'For Each factor_ In _borderfields.Keys.Where(Function(e) e.fieldpedimento.ToString = CamposPedimento.CA_FECHA_FACTURA.ToString)

        '    factorchanges_ = _coincontroller.ObtenerFactorTipodeCambio(_fieldvalues("S13.CA_CVE_MONEDA_FACTURA." & index_), fecha_).ObjectReturned

        '    '#63
        '    SetValueStockElement(SeccionesPedimento.ANS13,
        '                         CamposPedimento.CA_FACTOR_MONEDA,
        '                         False,
        '                         external_:=New List(Of String) From {"CA_FACTOR_MONEDA_CURRENT." & index_,
        '                                                               factorchanges_(0).factor.ToString})

        '    index_ = index_ + 1

        'Next


        ''#64
        'SetValueStockElement(SeccionesPedimento.ANS20,
        '                     CamposPedimento.CA_NUMERO_PEDIMENTO_ORIGINAL_COMPLETO,
        '                     False)

        ''#64
        'SetValueStockElement(SeccionesPedimento.ANS20,
        '                     CamposPedimento.CA_FECHA_PEDIMENTO_ORIGINAL,
        '                     False,
        '                     dateType_:=True,
        '                     validate_:=False)

        ''#64
        'SetValueStockElement(SeccionesPedimento.ANS20,
        '                     CamposPedimento.CA_ANIO_VALIDACION_2_ORIGINAL,
        '                     False,
        '                     validate_:=False)

        ''#64
        'SetValueStockElement(SeccionesPedimento.ANS20,
        '                     CamposPedimento.CA_ADUANA_DESPACHO_ORIGINAL_2,
        '                     False,
        '                     validate_:=False)

        ''#64
        'SetValueStockElement(SeccionesPedimento.ANS20,
        '                     CamposPedimento.CA_PATENTE_ORIGINAL,
        '                     False,
        '                     validate_:=False)

        ''#64
        'SetValueStockElement(SeccionesPedimento.ANS20,
        '                     CamposPedimento.CA_NUMERO_PEDIMENTO_ORIGINAL_7_DIGITOS,
        '                     False,
        '                     validate_:=False)

        ''#65
        'SetValueStockElement(SeccionesPedimento.ANS13,
        '                     CamposPedimento.CA_MONTO_USD,
        '                     False,
        '                     fullSection_:=True)

        ''#66
        'If SetValueStockElement(SeccionesPedimento.ANS24,
        '                        CamposPedimento.CA_SECUENCIA_PARTIDA,
        '                        True,
        '                        validate_:=False,
        '                        fullSection_:=True) Then

        '    Return _report

        'End If

        ''#67
        'If SetValueStockElement(SeccionesPedimento.ANS24,
        '                        CamposPedimento.CA_FRACCION_ARANCELARIA_PARTIDA,
        '                        True,
        '                        validate_:=False,
        '                        fullSection_:=True) Then

        '    Return _report

        'End If

        ''#68
        'If SetValueStockElement(SeccionesPedimento.ANS24,
        '                        CamposPedimento.CA_NICO_PARTIDA,
        '                        True,
        '                        validate_:=False,
        '                        fullSection_:=True) Then

        '    Return _report

        'End If

        ''#69
        'If SetValueStockElement(SeccionesPedimento.ANS24,
        '                        CamposPedimento.CA_CVE_VINCULACION,
        '                        True,
        '                        roomNameExt_:="_NORMAL_EXPORTACION") Then

        '    Return _report

        'End If

        ''#70
        'If SetValueStockElement(SeccionesPedimento.ANS24,
        '                        CamposPedimento.CA_CVE_METODO_VALORACION_PARTIDA,
        '                        True,
        '                        roomNameExt_:="_NORMAL_EXPORTACION",
        '                        fullSection_:=True) Then

        '    Return _report

        'End If

        ''#71
        'If SetValueStockElement(SeccionesPedimento.ANS24,
        '                        CamposPedimento.CA_CVE_UMC_PARTIDA,
        '                        True,
        '                        validate_:=False,
        '                        fullSection_:=True) Then

        '    Return _report

        'End If

        ''#72
        'If SetValueStockElement(SeccionesPedimento.ANS24,
        '                        CamposPedimento.CA_CANTIDAD_UMC_PARTIDA,
        '                        True,
        '                        fullSection_:=True) Then

        '    Return _report

        'End If

        ''#73
        'If SetValueStockElement(SeccionesPedimento.ANS24,
        '                        CamposPedimento.CA_CVE_UMT_PARTIDA,
        '                        True,
        '                        validate_:=False,
        '                        fullSection_:=True) Then

        '    Return _report

        'End If

        ''#74
        'If SetValueStockElement(SeccionesPedimento.ANS24,
        '                        CamposPedimento.CA_CANTIDAD_UMT_PARTIDA,
        '                        True,
        '                         fullSection_:=True) Then

        '    Return _report

        'End If

        ''#75
        'If SetValueStockElement(SeccionesPedimento.ANS24,
        '                        CamposPedimento.CA_CVE_PAIS_VENDEDOR_PARTIDA,
        '                        True,
        '                        validate_:=False,
        '                        fullSection_:=True) Then

        '    Return _report

        'End If

        ''#76
        'If SetValueStockElement(SeccionesPedimento.ANS24,
        '                        CamposPedimento.CA_CVE_PAIS_ORIGEN_PARTIDA,
        '                        True,
        '                        validate_:=False,
        '                        fullSection_:=True) Then

        '    Return _report

        'End If

        ''#77
        'If SetValueStockElement(SeccionesPedimento.ANS24,
        '                        CamposPedimento.CA_DESCRIPCION_MERCANCIA_PARTIDA,
        '                        True,
        '                        validate_:=False,
        '                        fullSection_:=True) Then

        '    Return _report

        'End If

        ''#78
        'If SetValueStockElement(SeccionesPedimento.ANS24,
        '                        CamposPedimento.CA_PRECIO_PAGADO_PARTIDA,
        '                        True,
        '                        roomNameExt_:="_NORMAL_EXPORTACION",
        '                        fullSection_:=True) Then

        '    Return _report

        'End If

        ''#79
        'SetValueStockElement(SeccionesPedimento.ANS29,
        '                     CamposPedimento.CA_CVE_CONTRIBUCION_PARTIDA,
        '                     False,
        '                     fullSection_:=True)

        ''#80
        'SetValueStockElement(SeccionesPedimento.ANS29,
        '                     CamposPedimento.CA_CVE_TIPO_TASA_PARTIDA,
        '                     False,
        '                     fullSection_:=True)

        ''#81
        'SetValueStockElement(SeccionesPedimento.ANS29,
        '                     CamposPedimento.CA_TASA_PARTIDA,
        '                     False,
        '                     fullSection_:=True)

        ''#82
        'SetValueStockElement(SeccionesPedimento.ANS29,
        '                     CamposPedimento.CA_FORMA_PAGO_PARTIDA,
        '                     False,
        '                     fullSection_:=True)

        ''#83
        'If SetValueStockElement(SeccionesPedimento.ANS24,
        '                        CamposPedimento.CA_VALOR_ADUANA_PARTIDA,
        '                        True,
        '                        fullSection_:=True,
        '                        validate_:=False,
        '                        conditions_:=New List(Of String) From {"0"}) Then

        '    Return _report

        'End If

        ''#83
        'If SetValueStockElement(SeccionesPedimento.ANS24,
        '                        CamposPedimento.CA_VALOR_DOLAR_PARTIDA,
        '                        True,
        '                        validate_:=False,
        '                        fullSection_:=True) Then

        '    Return _report

        'End If

        ''#84
        'If SetValueStockElement(SeccionesPedimento.ANS24,
        '                        CamposPedimento.CA_VALOR_COMERCIAL_PARTIDA,
        '                        True,
        '                        validate_:=False,
        '                        fullSection_:=True) Then

        '    Return _report

        'End If

        ''#85
        'If SetValueStockElement(SeccionesPedimento.ANS24,
        '                        CamposPedimento.CA_PRECIO_UNITARIO_PARTIDA,
        '                        True,
        '                        validate_:=False,
        '                        fullSection_:=True) Then

        '    Return _report

        'End If

        ''#86
        'SetValueStockElement(SeccionesPedimento.ANS24,
        '                     CamposPedimento.CA_VALOR_AGREGADO_PARTIDA,
        '                     False,
        '                     validate_:=False,
        '                     fullSection_:=True)

        ''#87
        'SetValueStockElement(SeccionesPedimento.ANS24,
        '                     CamposPedimento.CA_MARCA_PARTIDA,
        '                     False,
        '                     roomNameExt_:="_NORMAL_EXPORTACION",
        '                     fullSection_:=True)

        ''#88
        'SetValueStockElement(SeccionesPedimento.ANS24,
        '                     CamposPedimento.CA_MODELO_PARTIDA,
        '                     False,
        '                     fullSection_:=True)

        ''#89
        'SetValueStockElement(SeccionesPedimento.ANS24,
        '                     CamposPedimento.CA_CODIGO_PRODUCTO_PARTIDA,
        '                     False,
        '                     fullSection_:=True,
        '                     validate_:=False)

        ''#90
        'SetValueStockElement(SeccionesPedimento.ANS25,
        '                     CamposPedimento.CA_VINCULACION_NUMERO_SERIE_PARTIDA,
        '                     False)

        ''#91
        'SetValueStockElement(SeccionesPedimento.ANS25,
        '                     CamposPedimento.CA_KILOMETRAJE_PARTIDA,
        '                     False)

        ''#92
        'SetValueStockElement(SeccionesPedimento.ANS26,
        '                     CamposPedimento.CA_CVE_PERMISO,
        '                     False,
        '                     validate_:=False)

        ''#93
        'SetValueStockElement(SeccionesPedimento.ANS26,
        '                     CamposPedimento.CA_NUMERO_PERMISO,
        '                     False)

        ''#94
        'SetValueStockElement(SeccionesPedimento.ANS26,
        '                     CamposPedimento.CA_FIRMA_ELECTRONICA_PERMISO,
        '                     False,
        '                     validate_:=False)

        ''#95
        'SetValueStockElement(SeccionesPedimento.ANS26,
        '                     CamposPedimento.CA_VALOR_USD_PARTIDA,
        '                     False,
        '                     fullSection_:=True)

        ''#96
        'SetValueStockElement(SeccionesPedimento.ANS26,
        '                     CamposPedimento.CA_CANTIDAD_UMT_UMC,
        '                     False,
        '                     fullSection_:=True)

        ''#97
        'SetValueStockElement(SeccionesPedimento.ANS27,
        '                     CamposPedimento.CA_CVE_IDENTIFICADOR_PARTIDA,
        '                     False,
        '                     fullSection_:=True)

        ''#98
        'SetValueStockElement(SeccionesPedimento.ANS27,
        '                     CamposPedimento.CA_COMPLEMENTO_1_PARTIDA,
        '                     False,
        '                     fullSection_:=True)

        ''#99
        'SetValueStockElement(SeccionesPedimento.ANS27,
        '                     CamposPedimento.CA_COMPLEMENTO_2_PARTIDA,
        '                     False,
        '                     fullSection_:=True)

        ''#100
        'SetValueStockElement(SeccionesPedimento.ANS27,
        '                     CamposPedimento.CA_COMPLEMENTO_3_PARTIDA,
        '                     False,
        '                     fullSection_:=True)

        ''#101
        'SetValueStockElement(SeccionesPedimento.ANS36,
        '                     CamposPedimento.CA_OBSERVACIONES_PARTIDA,
        '                     False,
        '                     validate_:=False,
        '                     fullSection_:=True)

        ''#102
        'If SetValueStockElement(SeccionesPedimento.ANS1,
        '                        CamposPedimento.CA_VALOR_DOLARES,
        '                        True,
        '                        validate_:=False) Then

        '    Return _report

        'End If

        ''#103
        'SetValueStockElement(SeccionesPedimento.ANS29,
        '                     CamposPedimento.CA_IMPORTE_PARTIDA,
        '                     False,
        '                     fullSection_:=True)

        ''#104
        'If SetValueStockElement(SeccionesPedimento.ANS43,
        '                        CamposPedimento.CA_NUMERO_TOTAL_PARTIDAS,
        '                        True,
        '                        validate_:=False) Then

        '    Return _report

        'End If

        ''#105
        'SetValueStockElement(SeccionesPedimento.ANS6,
        '                     CamposPedimento.CA_CONTRIBUCION,
        '                     False,
        '                     fullSection_:=True)

        ''#106
        'SetValueStockElement(SeccionesPedimento.ANS6,
        '                     CamposPedimento.CA_CVE_TIPO_TASA,
        '                     False,
        '                     fullSection_:=True)

        ''#107
        'SetValueStockElement(SeccionesPedimento.ANS6,
        '                     CamposPedimento.CA_TASA,
        '                     False,
        '                     fullSection_:=True)

        ''#108
        'SetValueStockElement(SeccionesPedimento.ANS55,
        '                     CamposPedimento.CA_CONCEPTO,
        '                     False,
        '                     fullSection_:=True)

        ''#109
        'SetValueStockElement(SeccionesPedimento.ANS55,
        '                     CamposPedimento.CA_FORMA_PAGO,
        '                     False,
        '                     fullSection_:=True)

        ''#110
        'SetValueStockElement(SeccionesPedimento.ANS55,
        '                     CamposPedimento.CA_IMPORTE,
        '                     False,
        '                     fullSection_:=True)

        ''#111
        'SetValueStockElement(SeccionesPedimento.ANS7,
        '                     CamposPedimento.CA_EFECTIVO,
        '                     False)

        ''#112
        'SetValueStockElement(SeccionesPedimento.ANS7,
        '                     CamposPedimento.CA_OTROS,
        '                     False)

        ''#113
        'SetValueStockElement(SeccionesPedimento.ANS7,
        '                     CamposPedimento.CA_TOTAL,
        '                     False)

        ''#114
        'If SetValueStockElement(SeccionesPedimento.ANS1,
        '                        CamposPedimento.CA_CVE_PREVALIDADOR,
        '                        True,
        '                        validate_:=False) Then

        '    Return _report

        'End If

        ''#115
        'SetValueStockElement(SeccionesPedimento.ANS1,
        '                     CamposPedimento.CA_CERTIFICACION,
        '                     False,
        '                     validate_:=False)

        ''#116 
        'SetValueStockElement(SeccionesPedimento.ANS9,
        '                     CamposPedimento.CA_PATENTE,
        '                     False)

        ''#117 
        'SetValueStockElement(SeccionesPedimento.ANS9,
        '                     CamposPedimento.CA_NUMERO_PEDIMENTO,
        '                     False)

        ''#118
        'SetValueStockElement(SeccionesPedimento.ANS9,
        '                     CamposPedimento.CA_CLAVE_SAD,
        '                     False,
        '                     roomNameExt_:="_LINEA_CAPTURA")

        ''#119
        'SetValueStockElement(SeccionesPedimento.ANS9,
        '                     CamposPedimento.CA_PAGO_ELECTRONICO,
        '                     False)

        ''#120
        'SetValueStockElement(SeccionesPedimento.ANS9,
        '                     CamposPedimento.CA_NOMBRE_INSTITUCION_BANCARIA,
        '                     False)

        ''#121
        'SetValueStockElement(SeccionesPedimento.ANS9,
        '                     CamposPedimento.CA_LINEA_CAPTURA,
        '                     False)

        ''#122
        'SetValueStockElement(SeccionesPedimento.ANS9,
        '                     CamposPedimento.CA_EFECTIVO,
        '                     False,
        '                     roomNameExt_:="_LINEA_CAPTURA")

        ''#123
        'SetValueStockElement(SeccionesPedimento.ANS9,
        '                     CamposPedimento.CA_FECHA_PAGO,
        '                     False,
        '                     dateType_:=True,
        '                     roomNameExt_:="_LINEA_CAPTURA")

        ''#124
        'SetValueStockElement(SeccionesPedimento.ANS9,
        '                     CamposPedimento.CA_NUMERO_OPERACION_BANCARIA,
        '                     False)

        ''#125
        'SetValueStockElement(SeccionesPedimento.ANS9,
        '                     CamposPedimento.CA_NUMERO_TRANSACCION_SAT,
        '                     False)

        ''#126
        'SetValueStockElement(SeccionesPedimento.ANS9,
        '                     CamposPedimento.CA_MEDIO_PRESENTACION,
        '                     False)

        ''#127
        'SetValueStockElement(SeccionesPedimento.ANS9,
        '                     CamposPedimento.CA_MEDIO_RECEPCION_COBRO,
        '                     False)

        ''#128
        'If SetValueStockElement(SeccionesPedimento.ANS1,
        '                        CamposPedimento.CA_ACUSE_ELECTRONICO_VALIDACION,
        '                        True,
        '                        validate_:=False) Then

        '    Return _report

        'End If

        ''#129
        'If SetValueStockElement(SeccionesPedimento.ANS1,
        '                        CamposPedimento.CA_CODIGO_BARRAS,
        '                        True,
        '                        validate_:=False) Then

        '    Return _report

        'End If

        ''#130
        'SetValueStockElement(SeccionesPedimento.ANS9,
        '                     CamposPedimento.CA_DEPOSITO_REFERENCIADO,
        '                     False)

        ''#131
        'SetValueStockElement(SeccionesPedimento.ANS1,
        '                     CamposPedimento.CA_FECHA_VALIDACION,
        '                     False,
        '                     dateType_:=True,
        '                     validate_:=False)

        Return _report

    End Function


End Class
