Imports System.ComponentModel
Imports System.Runtime.Serialization

Namespace Syn.Nucleo

    <Serializable()>
    Public Class RecursosComercioExterior
        Inherits Recursos

        '***** - COMENTARIOS IMPORTANTES (By: GSC 12/02/2025) - *****
        '¡NO INCLUIR LAS SIGUIENTES NUMERACIONES EN ESTOS ENUMS! PORQUE SE USAN EN EL ENUM DE RECURSOS
        'Y HACEN AMBIGUEDAD CUANDO SE USAN JUNTOS.
        'GLOBALES = 10000 - 10099   |   DOMICILIOS = 1000 - 1099    |   CLIENTES = 2000 - 2500

#Region "Attributes"

#End Region

        'UN = Sin definir
        'AN = Anexo22
        'VO = VOCE
        'SG = Generico
        'SCS = Clientes
        'SFAC = Factura Comercial
        'SPRO = Secciones Proveedor Operativo

        Enum SecuenciasComercioExterior
            ProveedoresOperativos
            FacturasComerciales
            AcusesValor
            Pedimentos
            SubdivisionFacturaComercial
            Productos
            ProcesamientoElectronico
            IdKrom
            Destinatarios
            ExpedienteElectronico
        End Enum

        Enum SeccionesPedimento

            <EnumMember> <Description("Sin definir")> UNS00 = 0

            '#############################  SECCIONES ÚNICAS DEL ANEXO 22 ##################################
            <EnumMember> <Description("Encabezado principal del pedimento")> ANS1 = 1
            <EnumMember> <Description("Encabezado para páginas secundarias del pedimento")> ANS2 = 2
            <EnumMember> <Description("Datos del importador/exportador")> ANS3 = 3
            <EnumMember> <Description("Incrementables/decrementables")> ANS4 = 4
            <EnumMember> <Description("Datos generales del pedimento complementario")> ANS5 = 5
            <EnumMember> <Description("Prueba suficiente")> ANS6 = 6
            <EnumMember> <Description("Tasas a nivel pedimento")> ANS7 = 7
            <EnumMember> <Description("Cuadro de liquidación")> ANS8 = 8
            <EnumMember> <Description("Desglose de contribuciones del cuadro de liquidación")> ANS9 = 9
            <EnumMember> <Description("Informe de la industria automotriz")> ANS10 = 10
            <EnumMember> <Description("Deposito referenciado - línea de captura - información del pago (Validación)")> ANS11 = 11
            <EnumMember> <Description("Datos del proveedor/comprador")> ANS12 = 12
            <EnumMember> <Description("Datos del destinatario")> ANS13 = 13
            <EnumMember> <Description("Datos del transporte y transportista")> ANS14 = 14
            <EnumMember> <Description("CFDi/documento equivalente")> ANS15 = 15
            <EnumMember> <Description("Acuse de valor")> ANS16 = 16
            <EnumMember> <Description("Fechas")> ANS17 = 17
            <EnumMember> <Description("Candados")> ANS18 = 18
            <EnumMember> <Description("Guias, manifiestos, conocimientos de embarque o documentos de transporte")> ANS19 = 19
            <EnumMember> <Description("Contenedores/Equipo ferrocarril/Número economico del vehiculo")> ANS20 = 20
            <EnumMember> <Description("Identificadores (Nivel pedimento)")> ANS21 = 21
            <EnumMember> <Description("Cuentas aduaneras y cuentas aduaneras de garantia (Nivel pedimento)")> ANS22 = 22
            <EnumMember> <Description("Descargos")> ANS23 = 23
            <EnumMember> <Description("Detalle de partidas para descargos")> ANS24 = 24
            <EnumMember> <Description("Compensaciones")> ANS25 = 25
            <EnumMember> <Description("Documentos que amparan las formas de pago distintas a efectivo (Pago virtual)")> ANS26 = 26
            <EnumMember> <Description("Observaciones (nivel pedimento)")> ANS27 = 27
            <EnumMember> <Description("Partidas")> ANS28 = 28
            <EnumMember> <Description("Mercancías")> ANS29 = 29
            <EnumMember> <Description("Regulaciones y restricciones no arancelarias")> ANS30 = 30
            <EnumMember> <Description("Identificadores (Nivel partida)")> ANS31 = 31
            <EnumMember> <Description("Cuentas aduaneras de garantia (Nivel partida)")> ANS32 = 32
            <EnumMember> <Description("Tasas y contribuciones (nivel partida)")> ANS33 = 33
            <EnumMember> <Description("Contribuciones (nivel partida)")> ANS34 = 34
            <EnumMember> <Description("Partidas informe de la industria automotriz")> ANS35 = 35
            <EnumMember> <Description("Determinación de contribuciones a nivel partida al amparo del Art. 2.5 del T-MEC")> ANS36 = 36
            <EnumMember> <Description("Detalle de importación a EUA/CAN al amparo del Art. 2.5 del T-MEC")> ANS37 = 37
            <EnumMember> <Description("Determinación y/o pago de contribuciones por aplicación del Art. 2.5 del T-MEC en el pedimento de exportación (Retorno)")> ANS38 = 38
            <EnumMember> <Description("Pago de contribuciones a nivel partida por aplicación del Art. 2.5 del T-MEC")> ANS39 = 39
            <EnumMember> <Description("Observaciones (Nivel partida)")> ANS40 = 40
            <EnumMember> <Description("Rectificaciones")> ANS41 = 41
            <EnumMember> <Description("Diferencias de contribuciones (Nivel pedimento)")> ANS42 = 42
            <EnumMember> <Description("Desglose de diferencias del cuadro de diferencias de contribuciones")> ANS43 = 43
            <EnumMember> <Description("Encabezado para determinación de contribuciones a nivel partida para pedimentos complementarios al amparo del Art. T-MEC.")> ANS44 = 44
            <EnumMember> <Description("Encabezado para determinación de contribuciones a nivel partida para pedimentos complementarios al amparo del los articulos 14 de la decision o 15 del TLCAELC")> ANS45 = 45
            <EnumMember> <Description("Instructivo de llenado del pedimento de tránsito para el transbordo")> ANS46 = 46
            <EnumMember> <Description("Fin de pedimento")> ANS47 = 47
            <EnumMember> <Description("Pie de página del pedimento y datos del AA/Representante Legas/Almacen")> ANS48 = 48
            <EnumMember> <Description("Facturas/acuse de valor")> ANS49 = 49

            '#############################  SECCIONES ÚNICAS DEL ANEXO 22 ##################################

        End Enum

        Public Enum SeccionesFacturaComercial
            <EnumMember> <Description("Sin definir")> SFAC0 = 0

            '#############################  SECCIONES ÚNICAS DE LA FACTURA COMERCIAL ##################################
            <EnumMember> <Description("Generales")> SFAC1 = 1
            <EnumMember> <Description("Datos del proveedor")> SFAC2 = 2
            <EnumMember> <Description("Datos del destinatario")> SFAC3 = 3
            <EnumMember> <Description("Partidas")> SFAC4 = 4
            <EnumMember> <Description("Incrementables")> SFAC5 = 5
            '<EnumMember> <Description("Subdivisión")> SFAC6 = 6
            'Se comenta para que un futuro se pueda dividir la forma de guardar los item de la factura (si es que es optimo)
            '<EnumMember> <Description("Partida - factura")> SFAC7 = 7
            '<EnumMember> <Description("Partida - clasificación")> SFAC8 = 8
            '<EnumMember> <Description("Partida - detalle mercancía")> SFAC9 = 9
            '#############################  SECCIONES ÚNICAS  DE LA FACTURA COMERCIAL ##################################

        End Enum

        Public Enum SeccionesAcuseValor
            <EnumMember> <Description("Sin definir")> SAcuseValor0 = 0

            '#############################  SECCIONES ÚNICAS DE MODULO DE ACUSE DE VALOR ##################################
            <EnumMember> <Description("Generales")> SAcuseValor1 = 1
            <EnumMember> <Description("Datos del proveedor")> SAcuseValor2 = 2
            <EnumMember> <Description("Datos del destinatario")> SAcuseValor3 = 3
            <EnumMember> <Description("Partidas y Detalles")> SAcuseValor4 = 4
            <EnumMember> <Description("Configuración")> SAcuseValor5 = 5

            '#############################  SECCIONES ÚNICAS  DE MÓDULO DE ACUSE DE VALOR ##################################

        End Enum

        Public Enum SeccionesReferencias
            <EnumMember> <Description("Sin definir")> SREF0 = 0
            <EnumMember> <Description("Generales")> SREF1 = 1
            <EnumMember> <Description("Cliente")> SREF2 = 2
            <EnumMember> <Description("Tracking")> SREF3 = 3
            <EnumMember> <Description("Fechas")> SREF4 = 4
            <EnumMember> <Description("Documentos")> SREF5 = 5
            '<EnumMember> <Description("Documentos")> SREF6 = 6
            '<EnumMember> <Description("Detalle guías múltiples")> SREF7 = 7
            '<EnumMember> <Description("Detalle guía simple")> SREF8 = 8
        End Enum

        Public Enum SeccionesProvedorOperativo
            <EnumMember> <Description("Sin definir")> SPRO0 = 0

            '#############################  SECCIONES ÚNICAS DEL PROVEEDOR OPERATIVO ##################################
            <EnumMember> <Description("Generales")> SPRO1 = 1
            <EnumMember> <Description("DetalleProveedor")> SPRO2 = 2
            <EnumMember> <Description("DomiciliosFiscales")> SPRO3 = 3
            <EnumMember> <Description("VinculacionesClientes")> SPRO4 = 4
            <EnumMember> <Description("ConfiguracionAdicional")> SPRO5 = 5
            '#############################  SECCIONES ÚNICAS  DEL PROVEEDOR OPERATIVO ##################################
        End Enum

        Public Enum SeccionesRevalidacion
            <EnumMember> <Description("Sin definir")> SREV0 = 0
            <EnumMember> <Description("Generales")> SREV1 = 1
            <EnumMember> <Description("DatosRevalidacion")> SREV2 = 2
            <EnumMember> <Description("CargaSuelta")> SREV3 = 3
            <EnumMember> <Description("Contenedores")> SREV4 = 4
        End Enum

        Public Enum SeccionesViajes
            <EnumMember> <Description("Sin definir")> SVIA0 = 0
            <EnumMember> <Description("Generales")> SVIA1 = 1
            <EnumMember> <Description("DatosOperativos")> SVIA2 = 2
            <EnumMember> <Description("DatosAdicionales")> SVIA3 = 3
            <EnumMember> <Description("Referencias")> SVIA4 = 4
        End Enum

        Public Enum SeccionesProducto
            <EnumMember> <Description("Sin definir")> SPTO0 = 0
            <EnumMember> <Description("Generales")> SPTO1 = 1
            <EnumMember> <Description("Clasificacion")> SPTO2 = 2
            <EnumMember> <Description("DescipcionesFacturas")> SPTO3 = 3
            <EnumMember> <Description("Historico Clasificacion")> SPTO4 = 4
            <EnumMember> <Description("Catalogo DescripcionesFacturas")> SPTO5 = 5
            <EnumMember> <Description("Histórico de descripciones")> SPTO6 = 6

        End Enum

        Public Enum SeccionesTarifaArancelaria
            <EnumMember> <Description("Sin definir")> TIGIE0 = 0
            <EnumMember> <Description("Generales")> TIGIE1 = 1
            <EnumMember> <Description("Importacion")> TIGIE2 = 2
            <EnumMember> <Description("Exportacion")> TIGIE3 = 3
            <EnumMember> <Description("Regulaciones Arancelarias")> TIGIE4 = 4
            <EnumMember> <Description("Regulaciones no Arancelarias")> TIGIE5 = 5

            <EnumMember> <Description("Tratados comerciales")> TIGIE6 = 6
            <EnumMember> <Description("Paised afiliados")> TIGIE7 = 7
            <EnumMember> <Description("Cupos arancel")> TIGIE8 = 8
            <EnumMember> <Description("IEPS")> TIGIE9 = 9
            <EnumMember> <Description("Cuotas compensatorias")> TIGIE10 = 10
            <EnumMember> <Description("Precios estimados")> TIGIE11 = 11

            <EnumMember> <Description("Permisos instituciones")> TIGIE12 = 12
            <EnumMember> <Description("Permisos")> TIGIE13 = 13
            <EnumMember> <Description("Normas oficiales mexicanas")> TIGIE14 = 14
            <EnumMember> <Description("Anexos")> TIGIE15 = 15
            <EnumMember> <Description("Embargos")> TIGIE16 = 16
            <EnumMember> <Description("Cupos Mínimos")> TIGIE17 = 17
            <EnumMember> <Description("Padron Sectorial")> TIGIE18 = 18

            <EnumMember> <Description("Impuestos")> TIGIE19 = 19
            <EnumMember> <Description("Identificadores")> TIGIE20 = 20
            <EnumMember> <Description("Preferencias")> TIGIE21 = 21
            <EnumMember> <Description("ALADIS")> TIGIE22 = 22
            <EnumMember> <Description("ALADIS paises")> TIGIE23 = 23

        End Enum

        Public Enum SeccionesManifestacionValor
            <EnumMember> <Description("Sin definir")> SMV0 = 0



            '#############################  SECCIONES ÚNICAS DE LA MANIFESTACIÓN DE VALOR ##################################
            <EnumMember> <Description("Generales")> SMV1 = 1
            <EnumMember> <Description("Datos del proveedor")> SMV2 = 2
            <EnumMember> <Description("Datos del importador")> SMV3 = 3
            <EnumMember> <Description("facturas")> SMV4 = 4
            <EnumMember> <Description("Incrementables")> SMV5 = 5
            <EnumMember> <Description("Anexos")> SMV6 = 6
            <EnumMember> <Description("Valor de tansacción")> SMV7 = 7
            <EnumMember> <Description("Anexa Doc art 66")> SMV8 = 8
            <EnumMember> <Description("No anexa Doc art 66")> SMV9 = 9
            <EnumMember> <Description("Anexa Doc art 65")> SMV10 = 10
            <EnumMember> <Description("No anexa Doc art 65")> SMV11 = 11
            '#############################  SECCIONES ÚNICAS  DE LA MANIFESTACIÓN DE VALOR ##################################



        End Enum


        Public Enum SeccionesProcesamientoElectDocumentos
            <EnumMember> <Description("Sin definir")> SPED0 = 0

            '#############################  SECCIONES ÚNICAS DEL PROCESAMIENTO ELECTRÓNICO DOCUMENTOS ##################################

            <EnumMember> <Description("Generales")> SPED1 = 1
            <EnumMember> <Description("Procesar documentos")> SPED2 = 2
            <EnumMember> <Description("Mensajes documentos procesados")> SPED3 = 3
            <EnumMember> <Description("Documentos procesados")> SPED4 = 4

            '#############################  SECCIONES ÚNICAS DEL PROCESAMIENTO ELECTRÓNICO DOCUMENTOS ##################################

        End Enum

        Public Enum SeccionesSubdivisionFacturaComercial

            <EnumMember> <Description("Sin definir")> SSFC0 = 0

            '#############################  SECCIONES ÚNICAS DE LA SUBDIVISION DE FACTURA COMERCIAL ##################################

            <EnumMember> <Description("Generales")> SSFC1 = 1
            <EnumMember> <Description("Más información")> SSFC2 = 2
            <EnumMember> <Description("Cierre subdivisión")> SSFC3 = 3
            <EnumMember> <Description("Factura original")> SSFC4 = 4
            <EnumMember> <Description("Items factura original")> SSFC5 = 5
            <EnumMember> <Description("Incrementables factura comercial")> SSFC6 = 6
            <EnumMember> <Description("Detalles subdivisión")> SSFC7 = 7
            <EnumMember> <Description("Items asociados")> SSFC8 = 8

            '#############################  SECCIONES ÚNICAS DE LA SUBDIVISIÓN DE FACTURA COMERCIAL ##################################

        End Enum

        Public Enum SeccionesControlConsolidados
            <EnumMember> <Description("Sin definir")> SCC0 = 0


            '#############################  SECCIONES ÚNICAS DEL CONTROL DE CONSOLIDADOS ##################################
            <EnumMember> <Description("Aviso Consolidado")> SCC1 = 1
            <EnumMember> <Description("Remesas")> SCC2 = 2
            <EnumMember> <Description("Contenedores")> SCC3 = 3
            <EnumMember> <Description("Items")> SCC4 = 4
            <EnumMember> <Description("Proveedor")> SCC5 = 5
            <EnumMember> <Description("Destinatario")> SCC6 = 6
            '#############################  SECCIONES ÚNICAS DEL CONTROL DE CONSOLIDADOS ##################################



        End Enum


        Public Enum SeccionesCopiasSimples


            <EnumMember> <Description("Sin definir")> SCS0 = 0
            <EnumMember> <Description("Generales")> SCS1 = 1
            <EnumMember> <Description("Copias simples")> SCS2 = 2
        End Enum

        Public Enum SeccionesGuias
            <EnumMember> <Description("Sin definir")> SGUI0 = 0

            '#############################  SECCIONES ÚNICAS DE LAS GUIAS MASTER ##################################
            <EnumMember> <Description("Generales guia")> SGUI1 = 1
            <EnumMember> <Description("Contenedores")> SGUI2 = 2
            <EnumMember> <Description("Carga suelta")> SGUI3 = 3
            <EnumMember> <Description("Guias relacionadas")> SGUI4 = 4
            '<EnumMember> <Description("Historico viajes")> SGUI5 = 5
            '<EnumMember> <Description("Pie")> SGM4 = 6

            '#############################  SECCIONES ÚNICAS  DE LAS GUIAS MASTER ##################################
        End Enum

        Public Enum SeccionesProgramacionPrevios
            <EnumMember> <Description("Sin definir")> SPP0 = 0
            <EnumMember> <Description("Generales")> SPP1 = 1
            <EnumMember> <Description("Previo")> SPP2 = 2
            <EnumMember> <Description("Fotografías")> SPP3 = 3
            <EnumMember> <Description("Discrepancias")> SPP4 = 4

        End Enum

        Public Enum SeccionesDestinatarios

            <EnumMember> <Description("Sin definir")> SDES0 = 0
            <EnumMember> <Description("Generales")> SDES1 = 1
            <EnumMember> <Description("DomiciliosFiscalesDestinatarios")> SDES2 = 2
            <EnumMember> <Description("HistorialDomiciliosFiscalesDestinatarios")> SDES3 = 3

        End Enum

        Public Enum SeccionesPartesII


            <EnumMember> <Description("Sin definir")> SPII0 = 0
            <EnumMember> <Description("Generales")> SPII1 = 1
            <EnumMember> <Description("PartesII")> SPII2 = 2
            <EnumMember> <Description("DetallesPartesII")> SPII3 = 3

        End Enum

        Public Enum SeccionesControlViajes
            <EnumMember> <Description("Sin definir")> SCVI0 = 0
            <EnumMember> <Description("Tracking")> SCVI1 = 1
            <EnumMember> <Description("Viajes")> SCVI2 = 2
            <EnumMember> <Description("Guias relacionadas")> SCVI3 = 3
        End Enum

        Public Enum SeccionesExpedienteElectronico
            <EnumMember> <Description("Sin definir")> SEXPE0 = 0

            '#############################  SECCIONES ÚNICAS EXPEDIENTE ELECTRÓNICO ##################################
            <EnumMember> <Description("Generales")> SEXPE1 = 1
            <EnumMember> <Description("ExpedientesRecientes")> SEXPE2 = 2
            <EnumMember> <Description("DescargarExpediente")> SEXPE3 = 3
            <EnumMember> <Description("DescargarExpedientePorPeriodos")> SEXPE4 = 4
            <EnumMember> <Description("ObtenerExpedientePorPeriodos")> SEXPE5 = 5

            '#############################  SECCIONES ÚNICAS EXPEDIENTE ELECTRÓNICO ##################################
        End Enum

        Public Enum CamposPedimento
            'Región del 1 - 999

            'Abreviaciones genenerales
            'IOE = IMPORTADOR O EXPORTADOR
            'POC = PROVEEDOR O COMPRADOR
            'AAD = AGENTE ADUANAL
            'SAD = SECCIÓN ADUANA DE DESPACHO
            'PC  = PEDIMENTO COMPLEMENTARIO
            'CB  = CODIGO DE BARRAS
            'PE  = PAGO ELECTRÓNICO

            <EnumMember> <Description("SIN DEFINIR")> SIN_DEFINIR = 0

            '#############################  CAMPOS ÚNICOS DE LA AUTORIDAD ##################################

            'SECCION DE GENERALES 1 - 40 ANS1
            <EnumMember> <Description("NÚMERO PEDIMENTO COMPLETO")> CA_NUMERO_PEDIMENTO_COMPLETO = 1
            <EnumMember> <Description("TIPO OPERACIÓN")> CA_TIPO_OPERACION = 2
            <EnumMember> <Description("CLAVE PEDIMENTO")> CA_CVE_PEDIMENTO = 3
            <EnumMember> <Description("REGIMEN")> CA_REGIMEN = 4
            <EnumMember> <Description("DESTINO/ORIGEN")> CA_DESTINO_ORIGEN = 5
            <EnumMember> <Description("TIPO CAMBIO")> CA_TIPO_CAMBIO = 6
            <EnumMember> <Description("PESO BRUTO")> CA_PESO_BRUTO = 7
            <EnumMember> <Description("ADUANA ENTRADA/SALIDA")> CA_ADUANA_ENTRADA_SALIDA = 8
            <EnumMember> <Description("MEDIO DE TRANSPORTE")> CA_MEDIO_TRANSPORTE = 9
            <EnumMember> <Description("MEDIO DE TRANSPORTE ARRIBO")> CA_MEDIO_TRANSPORTE_ARRIBO = 10
            <EnumMember> <Description("MEDIO DE TRANSPORTE SALIDA")> CA_MEDIO_TRANSPORTE_SALIDA = 11
            <EnumMember> <Description("VALOR DOLARES")> CA_VALOR_DOLARES = 12
            <EnumMember> <Description("VALOR ADUANA")> CA_VALOR_ADUANA = 13
            <EnumMember> <Description("PRECIO PAGAGO")> CA_PRECIO_PAGADO = 14
            <EnumMember> <Description("VALOR COMERCIAL")> CA_VALOR_COMERCIAL = 15
            <EnumMember> <Description("CLAVE DE LA SECCIÓN ADUANERA DESPACHO")> CA_CLAVE_SAD = 16
            <EnumMember> <Description("MARCAS, NÚMEROS Y TOTAL DE BULTOS")> CA_MARCAS_NUMEROS_TOTAL_BULTOS = 17
            <EnumMember> <Description("NÚMERO DE PEDIMENTO 7 DIGITOS")> CA_NUMERO_PEDIMENTO = 18
            <EnumMember> <Description("CLAVE TIPO DE OPERACIÓN")> CA_CVE_TIPO_OPERACION = 19
            <EnumMember> <Description("PATENTE O AUTORIZACION")> CA_PATENTE = 20

            'SECCIÓN DATOS DEL IMPORTADOR/EXPORTADOR 41 - 80
            <EnumMember> <Description("RFC DEL IMPORTADOR/EXPORTADOR")> CA_RFC_IOE = 41
            <EnumMember> <Description("CURP DEL IMPORTADOR/EXPORTADOR")> CA_CURP_IOE = 42
            <EnumMember> <Description("NOMBRE, DENOMINACIÓN SOCIAL DEL IMPORTADOR/EXPORTADOR")> CA_RAZON_SOCIAL_IOE = 43
            <EnumMember> <Description("DOMICILIO DEL IMPORTADOR / EXPORTADOR")> CA_DOMICILIO_IOE = 44
            <EnumMember> <Description("CALLE IMPORTADOR/EXPORTADOR")> CA_CALLE_IOE = 45
            <EnumMember> <Description("NÚMERO INTERIOR")> CA_NUMERO_INTERIOR_IOE = 46
            <EnumMember> <Description("NÚMERO EXTERIOR")> CA_NUMERO_EXTERIOR_IOE = 47
            <EnumMember> <Description("CÓDIGO POSTAL")> CA_CODIGO_POSTAL_IOE = 48
            <EnumMember> <Description("MUNICIPIO/CIUDAD")> CA_MUNICIPIO_CIUDAD_IOE = 49
            <EnumMember> <Description("ENTIDAD FEDERATIVA")> CA_ENTIDAD_FEDERATIVA_IOE = 50
            <EnumMember> <Description("PAÍS DEL IMPORTADOR O EXPORTADOR")> CA_PAIS_IOE = 51

            'SECCIÓN INCREMENTABLES/DECREMENTABLES 81 - 100
            <EnumMember> <Description("VALOR SEGUROS")> CA_VALOR_SEGUROS = 81
            <EnumMember> <Description("SEGUROS")> CA_SEGUROS = 82
            <EnumMember> <Description("FLETES")> CA_FLETES = 83
            <EnumMember> <Description("EMBALAJES")> CA_EMBALAJES = 84
            <EnumMember> <Description("OTROS INCREMENTABLES")> CA_OTROS_INCREMENTABLES = 85
            <EnumMember> <Description("TRANSPORTE DECREMENTABLES")> CA_TRANSPORTE_DECREMENTABLES = 86
            <EnumMember> <Description("SEGURO DECREMENTABLES")> CA_SEGURO_DECREMENTABLES = 87
            <EnumMember> <Description("CARGA DECREMENTABLES")> CA_CARGA_DECREMENTABLES = 88
            <EnumMember> <Description("DESCARGA DECREMENTABLES")> CA_DESCARGA_DECREMENTABLES = 89
            <EnumMember> <Description("OTROS DECREMENTABLES")> CA_OTROS_DECREMENTABLES = 90

            'SECCIÓN FECHAS 101 - 120
            <EnumMember> <Description("FECHA ENTRADA")> CA_FECHA_ENTRADA = 101
            <EnumMember> <Description("FECHA PAGO")> CA_FECHA_PAGO = 102
            <EnumMember> <Description("FECHA EXTRACCIÓN")> CA_FECHA_EXTRACCION = 103
            <EnumMember> <Description("FECHA PRESENTACION")> CA_FECHA_PRESENTACION = 104
            <EnumMember> <Description("FECHA IMP.EUA/CAN")> CA_FECHA_IMPORTACION_EUA_CAN = 105
            <EnumMember> <Description("FECHA ORIGINAL")> CA_FECHA_ORIGINAL = 106

            'TASAS Y CONTRIBUCIONES 121 - 140
            <EnumMember> <Description("CONTRIBUCIÓN")> CA_CONTRIBUCION = 121
            <EnumMember> <Description("CLAVE TIPO TASA")> CA_CVE_TIPO_TASA = 122
            <EnumMember> <Description("TASA")> CA_TASA = 123
            <EnumMember> <Description("CONCEPTO")> CA_CONCEPTO = 124
            <EnumMember> <Description("FORMA DE PAGO")> CA_FORMA_PAGO = 125
            <EnumMember> <Description("IMPORTE")> CA_IMPORTE = 126
            <EnumMember> <Description("EFECTIVO")> CA_EFECTIVO = 127
            <EnumMember> <Description("OTROS")> CA_OTROS = 128
            <EnumMember> <Description("TOTAL")> CA_TOTAL = 129
            <EnumMember> <Description("CLAVE CONCEPTO NIVEL PEDIMENTO")> CA_CVE_CONCEPTO_NIVEL_PEDIMENTO = 130
            <EnumMember> <Description("DESCRIPCIÓN CONCEPTO")> CA_DESCRIPCION_CONCEPTO = 131
            <EnumMember> <Description("CLAVE CONCEPTO TASA PEDIMENTO")> CA_CVE_CONCEPTO_TASA_PEDIMENTO = 132

            'DATOS DEL AA/REPRESENTANTE LEGAS/ALMACEN 141 - 160
            <EnumMember> <Description("NOMBRE, DENOMINACIÓN O RAZÓN SOCIAL")> CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_AA = 141
            <EnumMember> <Description("RFC")> CA_RFC_AA = 142
            <EnumMember> <Description("CURP")> CA_CURP_AA_REPRESENTANTE_LEGAL = 143
            <EnumMember> <Description("NOMBRE")> CA_NOMBRE_MANDATARIO_REPRESENTANTE_AA = 144
            <EnumMember> <Description("RFC")> CA_RFC_MANDATARIO_AA_REPRESENTANTE_ALMACEN = 145
            <EnumMember> <Description("CURP")> CA_CURP_MANDATARIO_AA_REPRESENTANTE_ALMACEN = 146
            <EnumMember> <Description("FIRMA ELECTRÓNICA AVANZADA")> CA_EFIRMA = 147
            <EnumMember> <Description("NÚMERO DE SERIE DEL CERTIFICADO")> CA_CERTIFICADO_FIRMA = 148
            <EnumMember> <Description("TIPO DE FIGURA")> CA_TIPO_FIGURA = 149

            'DATOS PROVEEDOR/COMPRADOR 161 - 200
            <EnumMember> <Description("IDENTIFICACIÓN FISCAL")> CA_ID_FISCAL_POC = 161
            <EnumMember> <Description("NOMBRE, DENOMINACIÓN O RAZON SOCIAL")> CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_POC = 162
            <EnumMember> <Description("DOMICILIO")> CA_DOMICILIO_POC = 163
            <EnumMember> <Description("VINCULACIÓN")> CA_VINCULACION = 164
            <EnumMember> <Description("NÚMERO CFDI O DOCUMENTO EQUIVALENTE")> CA_CFDI_FACTURA = 165
            <EnumMember> <Description("FECHA")> CA_FECHA_FACTURA = 166
            <EnumMember> <Description("INCOTERM")> CA_INCOTERM = 167
            <EnumMember> <Description("MONEDA FACTURA")> CA_CVE_MONEDA_FACTURA = 168
            <EnumMember> <Description("VALOR MONEDA FACTURA")> CA_MONTO_MONEDA_FACTURA = 169
            <EnumMember> <Description("FACTOR MONEDA FACTURA")> CA_FACTOR_MONEDA = 170
            <EnumMember> <Description("VALOR DOLARES USD")> CA_MONTO_USD = 171
            <EnumMember> <Description("NÚMERO DE ACUSE DE VALOR")> CA_NUMERO_ACUSE_VALOR = 172
            <EnumMember> <Description("CALLE PROVEEDOR/COMPRADOR")> CA_CALLE_POC = 173
            <EnumMember> <Description("NÚMERO INTERIOR PROVEEDOR/COMPRADOR")> CA_NUMERO_INTERIOR_POC = 174
            <EnumMember> <Description("NÚMERO EXTERIOR PROVEEDOR/COMPRADOR")> CA_NUMERO_EXTERIOR_POC = 175
            <EnumMember> <Description("CÓDIGO POSTAL PROVEEDOR/COMPRADOR")> CA_CODIGO_POSTAL_POC = 176
            <EnumMember> <Description("MUNICIPIO/CIUDAD PROVEEDOR/COMPRADOR")> CA_MUNICIPIO_CIUDAD_POC = 177
            <EnumMember> <Description("ENTIDAD FEDERATIVA PROVEEDOR/COMPRADOR")> CA_ENTIDAD_FEDERATIVA_POC = 178
            <EnumMember> <Description("PAÍS PROVEEDOR/COMPRADOR")> CA_PAIS_POC = 179

            'DATOS DESTINATARIO 201 - 220
            <EnumMember> <Description("IDENTIFICACIÓN FISCAL")> CA_ID_FISCAL_DESTINATARIO = 201
            <EnumMember> <Description("NOMBRE, DENOMINACION O RAZON SOCIAL")> CA_NOMBRE_RAZON_SOCIAL_DESTINATARIO = 202
            <EnumMember> <Description("DOMICILIO")> CA_DOMICILIO_DESTINATARIO = 203
            <EnumMember> <Description("CALLE DESTINATARIO")> CA_CALLE_DESTINATARIO = 204
            <EnumMember> <Description("NÚMERO INTERIOR DESTINATARIO")> CA_NUMERO_INTERIOR_DESTINATARIO = 205
            <EnumMember> <Description("NÚMERO EXTERIOR DESTINATARIO")> CA_NUMERO_EXTERIOR_DESTINATARIO = 206
            <EnumMember> <Description("CÓDIGO POSTAL DESTINATARIO")> CA_CODIGO_POSTAL_DESTINATARIO = 207
            <EnumMember> <Description("MUNICIPIO/CIUDAD DESTINATARIO")> CA_MUNICIPIO_CIUDAD_DESTINATARIO = 208
            <EnumMember> <Description("PAÍS DE DESTINATARIO")> CA_PAIS_DESTINATARIO = 209

            'DATOS TRANSPORTE 221 - 240
            <EnumMember> <Description("IDENTIFICACIÓN")> CA_ID_TRANSPORTE = 221
            <EnumMember> <Description("PAÍS")> CA_CVE_PAIS_TRANSPORTE = 222
            <EnumMember> <Description("TRANSPORTISTA")> CA_NOMBRE_RAZON_SOCIAL_TRANSPORTE = 223
            <EnumMember> <Description("RFC")> CA_RFC_TRANSPORTE = 224
            <EnumMember> <Description("CURP")> CA_CURP_TRANSPORTE = 225
            <EnumMember> <Description("DOMICILIO/CIUDAD/ESTADO")> CA_DOMICILIO_TRANSPORTE = 226
            '<EnumMember> <Description("TOTAL DE BULTOS")> CA_TOTAL_BULTOS = 227 'para tránsitos

            'DATOS CANDADOS 241 - 260
            <EnumMember> <Description("NÚMERO DE CANDADO")> CA_NUMERO_CANDADO = 241
            <EnumMember> <Description("1RA. REVISIÓN")> CA_NUMERO_CANDADO_1RA = 242
            <EnumMember> <Description("2DA. REVISIÓN")> CA_NUMERO_CANDADO_2DA = 243

            'DATOS GUIAS 261 - 280
            <EnumMember> <Description("NÚMERO (GUIA/CONOCIMIENTO EMBARQUE) DOCUMENTOS DE TRANSPORTE")> CA_GUIA_MANIFIESTO_BL = 261
            <EnumMember> <Description("ID TIPO DE GUIA")> CA_MASTER_HOUSE = 262

            'DATOS CONTENEDORES 281 - 300
            <EnumMember> <Description("NÚMERO DE CONTENEDOR/EQUIPO FERROCARRIL/NÚMERO ECONÓMICO DEL VEHICULO.")> CA_NUMERO_CONTENEDOR = 281
            <EnumMember> <Description("TIPO DE CONTENEDOR/EQUIPO FERROCARRIL/NÚMERO ECONÓMICO DEL VEHICULO.")> CA_CVE_TIPO_CONTENEDOR = 282

            'DATOS IDENTIFICADORES PEDIMENTO 301 - 320
            <EnumMember> <Description("CLAVE IDENTIFICADOR")> CA_CVE_IDENTIFICADOR = 301
            <EnumMember> <Description("COMPLEMENTO IDENTIFICADOR 1")> CA_COMPLEMENTO_1 = 302
            <EnumMember> <Description("COMPLEMENTO IDENTIFICADOR 2")> CA_COMPLEMENTO_2 = 303
            <EnumMember> <Description("COMPLEMENTO IDENTIFICADOR 3")> CA_COMPLEMENTO_3 = 304

            'DATOS OBSERVACIONES PEDIMENTO 321 - 325 
            <EnumMember> <Description("OBSERVACIONES")> CA_OBSERVACIONES_PEDIMENTO = 321

            'PARTIDAS 326 - 400
            <EnumMember> <Description("SECUENCIA")> CA_SECUENCIA_PARTIDA = 326
            <EnumMember> <Description("FRACCIÓN ARANCELARIA")> CA_FRACCION_ARANCELARIA_PARTIDA = 327
            <EnumMember> <Description("SUBDIVISIÓN/NÚMERO IDENTIFICACIÓN COMERCIAL")> CA_NICO_PARTIDA = 328
            <EnumMember> <Description("MÉTODO VALORACIÓN")> CA_CVE_METODO_VALORACION_PARTIDA = 329
            <EnumMember> <Description("UMC")> CA_CVE_UMC_PARTIDA = 330
            <EnumMember> <Description("CANTIDAD UMC")> CA_CANTIDAD_UMC_PARTIDA = 331
            <EnumMember> <Description("UMT")> CA_CVE_UMT_PARTIDA = 332
            <EnumMember> <Description("CANTIDAD UMT")> CA_CANTIDAD_UMT_PARTIDA = 333
            <EnumMember> <Description("PAÍS VENDEDOR")> CA_CVE_PAIS_VENDEDOR_PARTIDA = 334
            <EnumMember> <Description("PAÍS ORIGEN")> CA_CVE_PAIS_ORIGEN_PARTIDA = 335
            <EnumMember> <Description("PAÍS COMPRADOR")> CA_CVE_PAIS_COMPRADOR_PARTIDA = 336
            <EnumMember> <Description("PAÍS DESTINO")> CA_CVE_PAIS_DESTINO_PARTIDA = 337
            <EnumMember> <Description("DESCRIPCION (RENGLONES VARIABLES SEGUN SE REQUIERA)")> CA_DESCRIPCION_MERCANCIA_PARTIDA = 338
            <EnumMember> <Description("VALOR ADUANA PARTDA")> CA_VALOR_ADUANA_PARTIDA = 339
            <EnumMember> <Description("VALOR DOLARES USD")> CA_VALOR_DOLAR_PARTIDA = 340
            <EnumMember> <Description("IMPORTE PRECIO PAGADO")> CA_PRECIO_PAGADO_PARTIDA = 341
            <EnumMember> <Description("VALOR COMERCIAL")> CA_VALOR_COMERCIAL_PARTIDA = 342
            <EnumMember> <Description("PRECIO UNITARIO")> CA_PRECIO_UNITARIO_PARTIDA = 343
            <EnumMember> <Description("VALOR AGREGADO")> CA_VALOR_AGREGADO_PARTIDA = 344
            <EnumMember> <Description("MARCA")> CA_MARCA_PARTIDA = 345
            <EnumMember> <Description("MODELO")> CA_MODELO_PARTIDA = 346
            <EnumMember> <Description("CODIGO PRODUCTO")> CA_CODIGO_PRODUCTO_PARTIDA = 347
            <EnumMember> <Description("CLAVE VINCULACIÓN PARTIDA")> CA_CVE_VINCULACION_PARTIDA = 348
            <EnumMember> <Description("ENTIDAD FEDERATIVA DE ORIGEN")> CA_ENTIDAD_FEDERATIVA_ORIGEN = 349
            <EnumMember> <Description("ENTIDAD FEDERATIVA DE DESTINO")> CA_ENTIDAD_FEDERATIVA_DESTINO = 350
            <EnumMember> <Description("ENTIDAD FEDERATIVA DEL COMPRADOR")> CA_ENTIDAD_FEDERATIVA_COMPRADOR = 351
            <EnumMember> <Description("ENTIDAD FEDERATIVA DEL VENDEDOR")> CA_ENTIDAD_FEDERATIVA_VENDEDOR = 352
            <EnumMember> <Description("CLAVE CONTRIBUCIÓN A NIVEL PARTIDA")> CA_CVE_CONTRIBUCION_PARTIDA = 353
            <EnumMember> <Description("CONTRIBUCIÓN A NIVEL PARTIDA")> CA_CONTRIBUCION_PARTIDA = 354
            <EnumMember> <Description("CLAVE TIPO TASA PARTIDA")> CA_CVE_TIPO_TASA_PARTIDA = 355
            <EnumMember> <Description("TASA PARTIDA")> CA_TASA_PARTIDA = 356
            <EnumMember> <Description("FORMA DE PAGO PARTIDA")> CA_FORMA_PAGO_PARTIDA = 357
            <EnumMember> <Description("IMPORTE PARTIDA")> CA_IMPORTE_PARTIDA = 358
            <EnumMember> <Description("NIV/NÚMERO SERIE")> CA_NIV_NUMERO_SERIE_PARTIDA = 359
            <EnumMember> <Description("KILOMETRAJE")> CA_KILOMETRAJE_PARTIDA = 360
            <EnumMember> <Description("PERMISO")> CA_CVE_PERMISO = 361
            <EnumMember> <Description("NÚMERO DE PERMISO")> CA_NUMERO_PERMISO = 362
            <EnumMember> <Description("FIRMA DESCARGO")> CA_FIRMA_ELECTRONICA_PERMISO = 363
            <EnumMember> <Description("VALOR COMERCIAL USD")> CA_VALOR_USD_PARTIDA = 364
            <EnumMember> <Description("CANTIDAD UMT/UMC.")> CA_CANTIDAD_UMT_UMC = 365
            <EnumMember> <Description("IDENTIFICADOR")> CA_CVE_IDENTIFICADOR_PARTIDA = 366
            <EnumMember> <Description("COMPLEMENTO 1")> CA_COMPLEMENTO_1_PARTIDA = 367
            <EnumMember> <Description("COMPLEMENTO 2")> CA_COMPLEMENTO_2_PARTIDA = 368
            <EnumMember> <Description("COMPLEMENTO 3")> CA_COMPLEMENTO_3_PARTIDA = 369
            <EnumMember> <Description("CLAVE GARANTIA")> CA_CVE_TIPO_GARANTIA_PARTIDA = 370
            <EnumMember> <Description("INSTITUCIÓN EMISORA")> CA_INSTITUCION_EMISORA_GARANTIA_PARTIDA = 371
            <EnumMember> <Description("FECHA CONSTANCIA")> CA_FECHA_EXPIRACION_CONSTANCIA_PARTIDA = 372
            <EnumMember> <Description("NUMERO DE CUENTA")> CA_NUMERO_CUENTA_GARANTIA_PARTIDA = 373
            <EnumMember> <Description("FOLIO CONSTANCIA")> CA_FOLIO_CONSTANCIA_PARTIDA = 374
            <EnumMember> <Description("TOTAL DEPOSITO")> CA_MONTO_TOTAL_CONSTANCIA_PARTIDA = 375
            <EnumMember> <Description("PRECIO ESTIMADO")> CA_PRECIO_ESTIMADO_PARTIDA = 376
            <EnumMember> <Description("CANTIDAD UMT PRECIO ESTIMADO")> CA_CANTIDAD_UMT_PRECIO_ESTIMADO_PARTIDA = 377
            <EnumMember> <Description("OBSERVACIONES A NIVEL PARTIDA")> CA_OBSERVACIONES_PARTIDA = 378

            'DATOS CUENTAS ADUANERAS NIVEL PEDIMENTO 401 - 420
            <EnumMember> <Description("CUENTAS ADUANERAS - TIPO CUENTA")> CA_CVE_CUENTA_ADUANERA_GARANTIA = 401
            <EnumMember> <Description("CUENTAS ADUANERAS - CLAVE DE GARANTIA")> CA_CVE_TIPO_GARANTIA = 402
            <EnumMember> <Description("CUENTAS ADUANERAS - INSTITUCION EMISORA")> CA_INSTITUCION_EMISORA_GARANTIA = 403
            <EnumMember> <Description("CUENTAS ADUANERAS - NUMERO DE CONTRATO")> CA_NUMERO_CONTRATO_GARANTIA = 404
            <EnumMember> <Description("CUENTAS ADUANERAS - FOLIO CONSTANCIA")> CA_FOLIO_CONSTANCIA_GARANTIA = 405
            <EnumMember> <Description("CUENTAS ADUANERAS - TOTAL DEPOSITO")> CA_IMPORTE_TOTAL_CONSTANCIA_GARANTIA = 406
            <EnumMember> <Description("CUENTAS ADUANERAS - FECHA CONSTANCIA")> CA_FECHA_EMISION_CONSTANCIA_GARANTIA = 407
            <EnumMember> <Description("CUENTAS ADUANERAS - CANTIDAD UNIDAD MEDIDA PRECIO ESTIMADO")> CA_CANTIDAD_UMT_PRECIO_ESTIMADO_GARANTIA = 408
            <EnumMember> <Description("CUENTAS ADUANERAS - VALOR UNITARIO DEL TITULO")> CA_VALOR_UNITARIO_TITULO_GARANTIA = 409
            <EnumMember> <Description("CUENTAS ADUANERAS - TÍTULOS ASIGNADOS")> CA_TITULOS_ASIGNADOS_GARANTIA = 410

            'DATOS DESCARGOS 421 - 440
            <EnumMember> <Description("DESCARGOS - NÚMERO PEDIMENTO ORIGINAL COMPLETO")> CA_NUMERO_PEDIMENTO_ORIGINAL_COMPLETO_DESCARGO = 421
            <EnumMember> <Description("DESCARGOS - FECHA DE OPERACION ORIGINAL")> CA_FECHA_PEDIMENTO_ORIGINAL_DESCARGO = 422
            <EnumMember> <Description("DESCARGOS - CLAVE PEDIMENTO ORIGINAL")> CA_CVE_PEDIMENTO_ORIGINAL_DESCARGO = 423
            <EnumMember> <Description("DESCARGOS - FRACCIÓN ORIGINAL")> CA_FRACCION_ORIGINAL_DESCARGO = 424
            <EnumMember> <Description("DESCARGOS - UNIDAD DE MEDIDA ORIGINAL")> CA_UM_ORIGINAL_DESCARGO = 425
            <EnumMember> <Description("DESCARGOS - CANTIDAD DE MERCANCIA EN UMT DE DESCARGO")> CA_CANTIDAD_MERCANCIA_UMT_DESCARGO = 426
            <EnumMember> <Description("DESCARGOS - PATENTE ORIGINAL")> CA_PATENTE_ORIGINAL_DESCARGO = 427
            <EnumMember> <Description("DESCARGOS - NÚMERO DE PEDIMENTO ORIGINAL 7 DIGITOS")> CA_NUMERO_PEDIMENTO_ORIGINAL_DESCARGO = 428
            <EnumMember> <Description("DESCARGOS - ADUANA DESPACHO ORIGINAL")> CA_ADUANA_DESPACHO_ORIGINAL_DESCARGO = 429
            <EnumMember> <Description("DESCARGOS - ANIO VALIDACION ORIGINAL")> CA_ANIO_VALIDACION_ORIGINAL_DESCARGO = 430

            'DATOS COMPENSACIONES 441 - 460
            <EnumMember> <Description("COMPENSACIONES - FECHA PAGO OPERACION ORIGINAL")> CA_FECHA_PAGO_ORIGINAL_COMPENSACION = 441
            <EnumMember> <Description("COMPENSACIONES - IMPORTE DEL GRAVAMEN")> CA_IMPORTE_GRAVAMEN_COMPENSACION = 442
            <EnumMember> <Description("COMPENSACIONES - CLAVE CONCEPTO PARA COMPENSACION")> CA_CVE_CONCEPTO_COMPENSACION = 443
            <EnumMember> <Description("COMPENSACIONES - CONTRIBUCIÓN")> CA_CONTRIBUCION_COMPENSACION = 444
            <EnumMember> <Description("COMPENSACIONES - NÚMERO PEDIMENTO ORIGINAL COMPLETO")> CA_NUMERO_PEDIMENTO_ORIGINAL_COMPLETO_COMPENSACION = 445
            <EnumMember> <Description("COMPENSACIONES - ANIO VALIDACION ORIGINAL")> CA_ANIO_VALIDACION_ORIGINAL_COMPENSACION = 446
            <EnumMember> <Description("COMPENSACIONES - PATENTE ORIGINAL")> CA_PATENTE_ORIGINAL_COMPENSACION = 447
            <EnumMember> <Description("COMPENSACIONES - ADUANA DESPACHO ORIGINAL")> CA_ADUANA_DESPACHO_ORIGINAL_COMPENSACION = 448
            <EnumMember> <Description("COMPENSACIONES - NÚMERO DE PEDIMENTO ORIGINAL 7 DIGITOS")> CA_NUMERO_PEDIMENTO_ORIGINAL_COMPENSACION = 449

            'DATOS PAGOS VIRTUALES 461 - 480
            <EnumMember> <Description("PAGOS VIRTUALES - DEPENDENCIA O INSTITUCIÓN EMISORA")> CA_INSTITUCION_EMISORA_PAGO_VIRTUAL = 461
            <EnumMember> <Description("PAGOS VIRTUALES - NÚMERO DEL DOCUMENTO")> CA_NUMERO_DOCUMENTO_PAGO_VIRTUAL = 462
            <EnumMember> <Description("PAGOS VIRTUALES - FECHA DEL DOCUMENTO")> CA_FECHA_EXPEDICION_DOCUMENTO_PAGO_VIRTUAL = 463
            <EnumMember> <Description("PAGOS VIRTUALES - IMPORTE DEL DOCUMENTO")> CA_IMPORTE_DOCUMENTO_PAGO_VIRTUAL = 464
            <EnumMember> <Description("PAGOS VIRTUALES - SALDO DISPONIBLE")> CA_SALDO_DISPONIBLE_DOCUMENTO_PAGO_VIRTUAL = 465
            <EnumMember> <Description("PAGOS VIRTUALES - IMPORTE A PAGAR")> CA_IMPORTE_PAGADO_PAGO_VIRTUAL = 466
            <EnumMember> <Description("PAGOS VIRTUALES - FORMA DE PAGO")> CA_FORMA_PAGO_VIRTUAL = 467

            'RECTIFICACIONES 481 - 500
            <EnumMember> <Description("RECTIFICACIONES - CLAVE CON LA QUE SE TRATA AL PEDIMENTO DE RECTIFICACIÓN")> CA_CVE_PEDIMENTO_RECTIFICACION = 481
            <EnumMember> <Description("RECTIFICACIONES - FECHA DE PAGO DE LA RECTIFICACIÓN")> CA_FECHA_PAGO_PEDIMENTO_RECTIFICACION = 482
            <EnumMember> <Description("RECTIFICACIONES - CLAVE PEDIMENTO ORIGINAL")> CA_CVE_PEDIMENTO_ORIGINAL_RECTIFICACION = 483
            <EnumMember> <Description("RECTIFICACIONES - PATENTE ORIGINAL")> CA_PATENTE_ORIGINAL_RECTIFICACION = 484
            <EnumMember> <Description("RECTIFICACIONES - NÚMERO DE PEDIMENTO ORIGINAL 7 DÍGITOS")> CA_NUMERO_PEDIMENTO_ORIGINAL_RECTIFICACION = 485
            <EnumMember> <Description("RECTIFICACIONES - AÑO VALIDACIÓN ORIGINAL")> CA_ANIO_VALIDACION_ORIGINAL_RECTIFICACION = 486
            <EnumMember> <Description("RECTIFICACIONES - ADUANA DESPACHO ORIGINAL")> CA_ADUANA_DESPACHO_ORIGINAL_RECTIFICACION = 487
            <EnumMember> <Description("RECTIFICACIONES - NÚMERO DE PEDIMENTO ORIGINAL COMPLETO")> CA_NUMERO_PEDIMENTO_ORIGINAL_COMPLETO_RECTIFICACION = 488
            <EnumMember> <Description("RECTIFICACIONES - FECHA DEL PEDIMENTO ORIGINAL O ULTIMA RECTIFICACIÓN")> CA_FECHA_PEDIMENTO_ORIGINAL_RECTIFICACION = 489


            'DIFERENCIAS 501 - 520
            <EnumMember> <Description("DIFERENCIA - EFECTIVO")> CA_EFECTIVO_DIFERENCIA = 501
            <EnumMember> <Description("DIFERENCIA - OTROS")> CA_OTROS_DIFERENCIA = 502
            <EnumMember> <Description("DIFERENCIA - TOTALES")> CA_TOTAL_DIFERENCIA = 503
            <EnumMember> <Description("DIFERENCIA - IMPORTE")> CA_IMPORTE_DIFERENCIA = 504
            <EnumMember> <Description("DIFERENCIA - CONCEPTO DE CONTRIBUCIONES")> CA_CONTRIBUCION_DIFERENCIA = 505
            <EnumMember> <Description("DIFERENCIA - CLAVE CONCEPTO DE CONTRIBUCIONES")> CA_CVE_CONTRIBUCION_DIFERENCIA = 506
            <EnumMember> <Description("DIFERENCIA - FORMA DE PAGO")> CA_FORMA_PAGO_DIFERENCIA = 507

            'VALIDACIÓN 521 - 540
            <EnumMember> <Description("CLAVE DEL PREVALIDADOR")> CA_CVE_PREVALIDADOR = 521
            <EnumMember> <Description("NOMBRE DEL PREVALIDADOR")> CA_NOMBRE_PREVALIDADOR = 522
            <EnumMember> <Description("AÑO DE LA VALIDACION")> CA_ANIO_VALIDACION = 523
            'PENDIENTES ACTUALMENTE NO SE USARAN YA QUE SON RESPUESTA O HASTA QUE SE ARMA EL M3
            '<EnumMember> <Description("ACUSE ELECTRÓNICO DE VALIDACIÓN")> CA_ACUSE_ELECTRONICO_VALIDACION = 524
            '<EnumMember> <Description("CERTIFICACIÓN")> CA_CERTIFICACION = 525
            '<EnumMember> <Description("NOMBRE DEL ARCHIVO")> CA_NOMBRE_ARCHIVO = 522
            '<EnumMember> <Description("NÚMERO DE LA SEMANA")> CA_NUMERO_SEMANA = 526
            '<EnumMember> <Description("ARCHIVO VALIDACIÓN")> CA_ARCHIVO_VALIDACION = 527

            'PRUEBA SUFICIENTE 541 - 560 SIN REVISIÓN
            '<EnumMember> <Description("PRUEBA SUFICIENTE - PAIS DESTINO")> CA_CVE_PAIS_DESTINO_PRUEBA_SUFICIENTE = 541
            '<EnumMember> <Description("PRUEBA SUFICIENTE - NUM. PEDIMENTO EUA/CAN.")> CA_NUMERO_PEDIMENTO_PRUEBA_SUFICIENTE = 542
            '<EnumMember> <Description("PRUEBA SUFICIENTE - CLAVE DEL TIPO PRUEBA")> CA_CVE_PRUEBA_SUFICIENTE = 543
            '<EnumMember> <Description("PRUEBA SUFICIENTE - FECHA DOCUMENTO")> CA_FECHA_DOCUMENTO_PRUEBA_SUFICIENTE = 544

            'PEDIMENTO COMPLEMENTARIO 561 - 590 SIN REVISIÓN
            '<EnumMember> <Description("PC ART. 2.5 T-MEC - TOTAL ARAN. EUA/CAN.")> CA_MONTO_IMPUESTO_PAGADO_EU_CAN_PC = 561
            '<EnumMember> <Description("PC ART. 2.5 T-MEC - MONTO EXENTO.")> CA_MONTO_IMPORTE_CONFORME_CAMPO_4_PC = 562
            '<EnumMember> <Description("PC ART. 2.5 T-MEC - FRACC. EUA/CAN.")> CA_FRACCION_ARANCELARIA_BIEN_FINAL_EU_CAN_PC = 563
            '<EnumMember> <Description("PC ART. 2.5 T-MEC - TASA EUA/CAN.")> CA_TASA_IMPORTE_PAGADO_EU_CAN_PC = 564
            '<EnumMember> <Description("PC ART. 2.5 T-MEC - ARAN. EUA/CAN.")> CA_MONTO_IMPORTE_PAGADO_EU_CAN_PC = 565
            '<EnumMember> <Description("PC ART. 14/15 TLCAELC/ACC - ADUANA")> CA_ADUANA_DESPACHO_SIN_SECCION_PC = 566
            '<EnumMember> <Description("PC - MONEDA IGI")> CA_MONEDA_IGI_PARTIDA_PC = 567
            '<EnumMember> <Description("PC - MONTO IGI")> CA_MONTO_IGI_PARTIDA_PC = 568
            '<EnumMember> <Description("PC - UNIDAD MEDIDA TARIFA EUA/CAN")> CA_UMT_EUA_CAN_PC = 569
            '<EnumMember> <Description("PC - CANTIDAD DE MERCANCÍA EN UMT EUA/CAN")> CA_CANTIDAD_MERCANCIA_UMT_EUA_CAN_PC = 570
            '<EnumMember> <Description("PC - VALOR MERCANCIAS NO ORIGINARIAS.")> CA_MONTO_MERCANCIAS_NO_ORIGINARIAS_PARTIDA = 571

            'CÓDIGO DE BARRAS 591 - 620 SIN REVISIÓN
            '<EnumMember> <Description("CÓDIGO DE BARRAS")> CA_CODIGO_BARRAS = 591
            '<EnumMember> <Description("CÓDIGO DE BARRAS - Llenar con 0")> CA_0 = 592
            '<EnumMember> <Description("CÓDIGO DE BARRAS - IMPORTE DE DERECHO DE TRAMITE ADUANERO")> CA_IMPORTE_DTA_CB = 593
            '<EnumMember> <Description("CÓDIGO DE BARRAS - PARA OPERACIONES AL AMPARO DE LA REGLA 3.1.40., DECLARAR: 3")> CA_REGLA_3_1_40_CB = 594
            '<EnumMember> <Description("CÓDIGO DE BARRAS - CLAVE DEL RECINTO FISCALIZADO, CONFORME AL APENDICE 6Ley Aduanera DE ESTE ANEXO")> CA_RECINTO_FISCALIZADO_CB = 595
            '<EnumMember> <Description("CÓDIGO DE BARRAS - PARA OPERACIONES CONFORME AL TERCER PARRAFO DE LA REGLA 2.3.8.Ley Aduanera, DECLARAR EL NUMERO DEL CONTENEDOR QUE CONTIENE LAS MERCANCIAS")> CA_REGLA_2_3_8_CONTENEDOR_CB = 596
            '<EnumMember> <Description("CÓDIGO DE BARRAS - PARA OPERACIONES CONFORME A LA REGLA 1.9.12.Ley Aduanera, DECLARAR EL NUMERO DE IDENTIFICACION DEL EQUIPO FERROVIARIO O NUMERO DE CONTENEDOR")> CA_REGLA_1_9_12_ID_EQ_FERROVIARIO_CONTENEDOR_CB = 597
            '<EnumMember> <Description("CÓDIGO DE BARRAS - LA CANTIDAD DE MERCANCIA EN UNIDADES DE COMERCIALIZACION AMPARADA POR LA PARTE II")> CA_CANTIDAD_UMC_PARTE_II_CB = 598
            '<EnumMember> <Description("CÓDIGO DE BARRAS - PARA OPERACIONES AL AMPARO DE LA REGLA 1.9.12.Ley Aduanera, SE DEBERA DECLARAR EL TOTAL DE GUIAS DE EMBARQUE (NIUS) AMPARADAS POR LA PARTE II")> CA_REGLA_1_9_12_TOTAL_GUIAS_PARTE_II_CB = 599
            '<EnumMember> <Description("CÓDIGO DE BARRAS - PARA OPERACIONES AL AMPARO DE LA REGLA. 1.9.12.Ley Aduanera, SE DEBERA DECLARAR EL NUMERO DE IDENTIFICACION UNICO (NIU) DE LA GUIA DE EMBARQUE")> CA_REGLA_1_9_12_NIU_GUIA_CB = 600
            '<EnumMember> <Description("CÓDIGO DE BARRAS - NUMERO CONSECUTIVO QUE EL AGENTE ADUANAL O LA AGENCIA ADUANAL ASIGNE A LA PARTE II")> CA_NUMERO_CONSECUTIVO_PARTE_II_CB = 601
            '<EnumMember> <Description("CÓDIGO DE BARRAS - LA CANTIDAD DE MERCANCIA EN UNIDADES TIGIE AMPARADA POR LA COPIA SIMPLE")> CA_CANTIDAD_UMC_COPIA_SIMPLE_CB = 602
            '<EnumMember> <Description("CÓDIGO DE BARRAS - PARA OPERACIONES AL AMPARO DE LA REGLA 1.9.12.Ley Aduanera, SE DEBERA DECLARAR EL TOTAL DE GUIAS DE EMBARQUE (NIUS) AMPARADAS POR LA COPIA SIMPLE")> CA_REGLA_1_9_12_TOTAL_GUIAS_COPIA_SIMPLE_CB = 603
            '<EnumMember> <Description("CÓDIGO DE BARRAS - CONSECUTIVO QUE EL AGENTE ADUANAL O LA AGENCIA ADUANAL ASIGNE A LA COPIA SIMPLE.")> CA_NUMERO_CONSECUTIVO_COPIA_SIMPLE_CB = 604
            '<EnumMember> <Description("CÓDIGO DE BARRAS - PARA OPERACIONES DE LA REGLA 3.1.21.Ley Aduanera, FRACCIÓN II, INCISO d), DECLARAR EL PESO BRUTO DE LA MERCANCÍA AMPARADA POR LA COPIA SIMPLE")> CA_REGLA_3_1_21_PESO_BRUTO_MERCANCIA_COPIA_SIMPLE_CB = 605
            '<EnumMember> <Description("CÓDIGO DE BARRAS - NUMERO DEL ACUSE DE VALOR EMITIDO POR VENTANILLA DIGITAL")> CA_NUMERO_ACUSE_VUCEM_CB = 606
            '<EnumMember> <Description("CÓDIGO DE BARRAS - LA CANTIDAD DE MERCANCIA EN UNIDADES DE COMERCIALIZACION AMPARADA EN LA REMESA")> CA_CANTIDAD_UMC_REMESA_CB = 607
            '<EnumMember> <Description("CÓDIGO DE BARRAS - PARA OPERACIONES AL AMPARO DE LA REGLA 1.9.12.Ley Aduanera, SE DEBERA DECLARAR EL TOTAL DE GUIAS DE EMBARQUE (NIUS) AMPARADAS POR LA REMESA")> CA_REGLA_1_9_12_TOTAL_GUIAS_REMESA_CB = 608
            '<EnumMember> <Description("CÓDIGO DE BARRAS - NUMERO CONSECUTIVO QUE EL AGENTE ADUANAL O LA AGENCIA ADUANAL ASIGNE A LA REMESA DEL PEDIMENTO CONSOLIDADO")> CA_NUMERO_CONSECUTIVO_REMESA_CB = 609
            '<EnumMember> <Description("CÓDIGO DE BARRAS - NUMERO DEL ACUSE DE VALOR DE LA RELACION DE CFDI O DOCUMENTOS EQUIVALENTES EMITIDO POR VENTANILLA DIGITAL")> CA_NUMERO_ACUSE_RELACION_CFDI_DOCUMENTO_EQUIVALENTE_CB = 610
            '<EnumMember> <Description("CÓDIGO DE BARRAS - VALOR EN DOLARES DEL TOTAL DE CDFI O DOCUMENTOS EQUIVALENTES AMPARADOS EN LA RELACION DE CDFI O DOCUMENTOS EQUIVALENTES")> CA_VALOR_DOLARES_TOTAL_CFDI_DOCUMENTO_EQUIVALENTE_CB = 611

            'PAGO ELECTRÓNICO 621 - 640 SIN REVISIÓN
            '<EnumMember> <Description("PAGO ELECTRONICO")> CA_PAGO_ELECTRONICO = 621
            '<EnumMember> <Description("PAGO ELECTRONICO - NOMBRE DE LA INSTITUCIÓN BANCARIA")> CA_NOMBRE_INSTITUCION_BANCARIA = 622
            '<EnumMember> <Description("PAGO ELECTRONICO - LINEA DE CAPTURA")> CA_LINEA_CAPTURA = 623
            '<EnumMember> <Description("PAGO ELECTRONICO - NÚMERO DE OPERACIÓN BANCARIA")> CA_NUMERO_OPERACION_BANCARIA = 624
            '<EnumMember> <Description("PAGO ELECTRONICO - NÚMERO DE TRANSACCIÓN SAT")> CA_NUMERO_TRANSACCION_SAT = 625
            '<EnumMember> <Description("PAGO ELECTRONICO - MEDIO DE PRESENTACIÓN")> CA_MEDIO_PRESENTACION = 626
            '<EnumMember> <Description("PAGO ELECTRONICO - MEDIO DE RECEPCIÓN/COBRO")> CA_MEDIO_RECEPCION_COBRO = 627
            '<EnumMember> <Description("PAGO ELECTRONICO - ACUSE ELECTRÓNICO DE PAGO")> CA_ACUSE_ELECTRONICO_PAGO = 628
            '<EnumMember> <Description("PAGO ELECTRONICO - ARCHIVO DE PAGO")> CA_ARCHIVO_PAGO = 629
            '<EnumMember> <Description("PAGO ELECTRONICO - DEPÓSITO REFERENCIADO E IMPRESIÓN DEL PAGO ELECTRÓNICO CONFORME AL APENDICE 23")> CA_DEPOSITO_REFERENCIADO = 630

            'FIN DE PEDIMENTO 641 - 650 SIN REVISIÓN
            '<EnumMember> <Description("FIN DEL PEDIMENTO.")> CA_FIN_PEDIMENTO = 641
            '<EnumMember> <Description("NUMERO TOTAL DE PARTIDAS")> CA_NUMERO_TOTAL_PARTIDAS = 642

            'NUEVA SECCIÓN 651 - 670
            '#############################  CAMPOS ÚNICOS DE LA AUTORIDAD ##################################

            '#############################  CAMPOS ÚNICOS PROPIOS ##################################
            'ESTOS CAMPOS INICIAN DEL 1100 - 1999
            <EnumMember> <Description("NÚMERO DE LA REFERENCIA")> CP_REFERENCIA = 1100
            <EnumMember> <Description("MODALIDAD/ADUANA/PATENTE")> CP_MODALIDAD_ADUANA_PATENTE = 1101
            <EnumMember> <Description("MODALIDAD")> CP_MODALIDAD = 1102
            <EnumMember> <Description("EJECUTIVO DE CUENTA")> CP_EJECUTIVO_CUENTA = 1103
            <EnumMember> <Description("CLAVE DEL IMPORTADOR/EXPORTADOR")> CP_CLAVE_IOE = 1104
            <EnumMember> <Description("ID DE LA CONTRIBUCIÓN")> CP_ID_CONTRIBUCION_PARTIDA = 1105
            <EnumMember> <Description("ESTADO DE LA CONTRIBUCIÓN")> CP_ESTADO_CONTRIBUCION_PARTIDA = 1106
            <EnumMember> <Description("ID DEL PERMISO")> CP_ID_PERMISO_PARTIDA = 1107
            <EnumMember> <Description("ESTADO DEL PERMISO")> CP_ESTADO_PERMISO_PARTIDA = 1108
            <EnumMember> <Description("ID DEL IDENTIFICADOR")> CP_ID_IDENTIFICADOR_PARTIDA = 1109
            <EnumMember> <Description("ESTADO DE LA PARTIDA")> CP_ESTADO_IDENTIFICADOR_PARTIDA = 1110
            <EnumMember> <Description("RUTA DE VALIDACIÓN")> CP_RUTA_VALIDACION = 1111
            <EnumMember> <Description("TIPO DE PEDIMENTO")> CP_TIPO_PEDIMENTO = 1112
            <EnumMember> <Description("FRACCIÓN ARANCELARIA 10 DÍGITOS")> CP_FRACCION_ARANCELARIA_PARTIDA = 1113
            <EnumMember> <Description("APLICA ENAJENACIÓN")> CP_APLICA_ENAJENACION = 1114
            <EnumMember> <Description("TIPO DE PERSONA DEL IMPORTADOR O EXPORTADOR")> CP_TIPO_PERSONA_IOE = 1115
            <EnumMember> <Description("TIPO DE DESPACHO QUE SE USARA CON RESPECTO A LA REFERENCIA")> CP_TIPO_DESPACHO = 1116
            <EnumMember> <Description("VALOR DE LOS ITEMS EN FACTURA")> CP_VALOR_FACTURA_PARTIDA = 1117
            <EnumMember> <Description("INCREMENTABLE - MONEDA DEL SEGURO")> CP_MONEDA_SEGUROS = 1118
            <EnumMember> <Description("INCREMENTABLE - FACTOR DEL SEGURO")> CP_FACTOR_SEGUROS = 1119
            <EnumMember> <Description("INCREMENTABLE - MONEDA DEL FLETE")> CP_MONEDA_FLETES = 1120
            <EnumMember> <Description("INCREMENTABLE - FACTOR DEL FLETE")> CP_FACTOR_FLETES = 1121
            <EnumMember> <Description("INCREMENTABLE - MONEDA DE OTROS")> CP_MONEDA_OTROS_INCREMENTABLES = 1122
            <EnumMember> <Description("INCREMENTABLE - FACTOR DE OTROS")> CP_FACTOR_OTROS_INCREMENTABLES = 1123
            <EnumMember> <Description("INCREMENTABLE - MONEDA DE EMBALAJES")> CP_MONEDA_EMBALAJES = 1124
            <EnumMember> <Description("INCREMENTABLE - FACTOR DE EMBALAJES")> CP_FACTOR_EMBALAJES = 1125
            <EnumMember> <Description("DECREMENTABLE - MONEDA DEL SEGURO")> CP_MONEDA_SEGUROS_DECREMENTABLES = 1126
            <EnumMember> <Description("DECREMENTABLE - FACTOR DEL SEGURO")> CP_FACTOR_SEGUROS_DECREMENTABLES = 1127
            <EnumMember> <Description("DECREMENTABLE - MONEDA DEL TRANSPORTE")> CP_MONEDA_TRANSPORTE = 1128
            <EnumMember> <Description("DECREMENTABLE - FACTOR DEL TRANSPORTE")> CP_FACTOR_TRANSPORTE = 1129
            <EnumMember> <Description("DECREMENTABLE - MONEDA DE OTROS")> CP_MONEDA_OTROS_DECREMENTABLES = 1130
            <EnumMember> <Description("DECREMENTABLE - FACTOR DE OTROS")> CP_FACTOR_OTROS_DECREMENTABLES = 1131
            <EnumMember> <Description("DECREMENTABLE - MONEDA DE CARGA")> CP_MONEDA_CARGA = 1132
            <EnumMember> <Description("DECREMENTABLE - FACTOR DE CARGA")> CP_FACTOR_CARGA = 1133
            <EnumMember> <Description("DECREMENTABLE - MONEDA DE DESCARGA")> CP_MONEDA_DESCARGA = 1134
            <EnumMember> <Description("DECREMENTABLE - FACTOR DE DESCARGA")> CP_FACTOR_DESCARGA = 1135
            <EnumMember> <Description("RECTIFICACIONES - CONTRIBUCIÓN ORIGINAL")> CP_CONTRIBUCION_ORIGINAL_RECTIFICACIONES = 1136
            <EnumMember> <Description("RECTIFICACIONES - CLAVE DE LA CONTRIBUCIÓN ORIGINAL")> CP_CVE_CONTRIBUCION_ORIGINAL_RECTIFICACIONES = 1137
            <EnumMember> <Description("RECTIFICACIONES - FORMA DE PAGO DE LA CONTRIBUCIÓN ORIGINAL")> CP_FORMA_PAGO_ORIGINAL_RECTIFICACIONES = 1138
            <EnumMember> <Description("INPC DEL MES ANTERIOR SEGÚN EL DOF")> CP_INPC_MES_ANTERIOR_DOF = 1139
            <EnumMember> <Description("INPC DEL MES MÁS ANTIGUO AL PERIODO ANTERIOR SEGÚN EL DOF")> CP_INPC_MES_ANTIGUO_DOF = 1140
            <EnumMember> <Description("PARTIDA - PRECIO DE REFERENCIA POR PARTIDA SEGÚN EL DOF")> CP_PRECIO_REFERENCIA_PARTIDA_DOF = 1141
            <EnumMember> <Description("AÑO EN CURSO")> CP_ANIO_CURSO = 1142
            <EnumMember> <Description("COMPENSACIONES - NÚMERO PEDIMENTO ORIGINAL PARTIDA")> CP_PEDIMENTO_ORIGINAL_COMPENSACION_PARTIDA = 1143
            <EnumMember> <Description("AÑO DE VEHÍCULO")> CP_ANIO_VEHICULO_ISAN_PARTIDA = 1144
            <EnumMember> <Description("TIPO CAMBIO FECHA EXTRACCIÓN")> CP_TIPO_CAMBIO_EXTRACCION = 1145
            <EnumMember> <Description("VALOR AGREGADO - MONTO MONEDA ORIGINAL PARTIDA")> CP_VALOR_AGREGADO_MONTO_ORIGINAL_PARTIDA = 1146
            <EnumMember> <Description("VALOR AGREGADO - MONEDA ORIGINAL PARTIDA")> CP_VALOR_AGREGADO_MONEDA_ORIGINAL_PARTIDA = 1147
            <EnumMember> <Description("VALOR AGREGADO - FACTOR CONVERSIÓN PARTIDA")> CP_VALOR_AGREGADO_FACTOR_CONVERSION_PARTIDA = 1148
            <EnumMember> <Description("DECREMENTABLE - MONTO MONEDA ORIGINAL DEL TRANSPORTE")> CP_MONTO_ORIGINAL_TRANSPORTE = 1149
            <EnumMember> <Description("DECREMENTABLE - MONTO MONEDA ORIGINAL DEL SEGURO")> CP_MONTO_ORIGINAL_SEGUROS_DECREMENTABLES = 1150
            <EnumMember> <Description("DECREMENTABLE - MONTO MONEDA ORIGINAL DE OTROS")> CP_MONTO_ORIGINAL_OTROS_DECREMENTABLES = 1151
            <EnumMember> <Description("DECREMENTABLE - MONTO MONEDA ORIGINAL DE CARGA")> CP_MONTO_ORIGINAL_CARGA = 1152
            <EnumMember> <Description("DECREMENTABLE - MONTO MONEDA ORIGINAL DE DESCARGA")> CP_MONTO_ORIGINAL_DESCARGA = 1153
            <EnumMember> <Description("INCREMENTABLE - MONTO MONEDA ORIGINAL DEL FLETE")> CP_MONTO_ORIGINAL_FLETES = 1154
            <EnumMember> <Description("INCREMENTABLE - MONTO MONEDA ORIGINAL DE EMBALAJES")> CP_MONTO_ORIGINAL_EMBALAJES = 1155
            <EnumMember> <Description("INCREMENTABLE - MONTO MONEDA ORIGINAL DE OTROS")> CP_MONTO_ORIGINAL_OTROS_INCREMENTABLES = 1156
            <EnumMember> <Description("INCREMENTABLE - MONTO MONEDA ORIGINAL DE SEGUROS")> CP_MONTO_ORIGINAL_SEGUROS_INCREMENTABLES = 1157
            <EnumMember> <Description("OBJECTID DEL CLIENTE")> CP_ID_IOE = 1158
            <EnumMember> <Description("CLAVE DEL PROVEEDOR/COMPRADOR")> CP_CLAVE_POC = 1159
            <EnumMember> <Description("OBJECTID DEL PROVEEDOR/COMPRADOR")> CP_ID_POC = 1160
            <EnumMember> <Description("CLAVE DEL DESTINATARIO")> CP_CLAVE_DESTINATARIO = 1161
            <EnumMember> <Description("OBJECTID DEL DESTINATARIO")> CP_ID_DESTINATARIO = 1162
            <EnumMember> <Description("OBJECTID DE LA REFERENCIA")> CP_ID_REFERENCIA = 1163
            <EnumMember> <Description("OBJECTID DE LA FACTURA")> CP_ID_FACTURA = 1164
            <EnumMember> <Description("CLAVE DEL TRANSPORTE")> CP_CLAVE_TRANSPORTE = 1165
            <EnumMember> <Description("OBJECTID DEL TRANSPORTE")> CP_ID_TRANSPORTE = 1166
            <EnumMember> <Description("OBJECTID DEL ACUSE DE VALOR")> CP_ID_ACUSE_VALOR = 1167
            <EnumMember> <Description("OBJECTID DE LA GUIA")> CP_ID_GUIA = 1168
            <EnumMember> <Description("OBJECTID DEL CONTENEDOR")> CP_ID_CONTENEDOR = 1169
            <EnumMember> <Description("OBJECTID DEL PEDIMENTO ORIGINAL")> CP_ID_PEDIMENTO_ORIGINAL = 1170
            <EnumMember> <Description("SECUENCIA PARTIDA DEL PEDIMENTO ORIGINAL")> CP_SECUENCIA_PARTIDA_ORIGINAL = 1171

            '#############################  CAMPOS ÚNICOS PROPIOS ##################################

        End Enum

        '<DataContract()>
        Public Enum CamposVOCE

            '#############################  CAMPOS ÚNICOS DEL VOCE ##################################
            'Abreviaciones genenerales
            'IOE = IMPORTADOR O EXPORTADOR
            'PRO = PROVEEDOR
            'AAD = AGENTE ADUANAL
            'SAD = SECCIÓN ADUANA DE DESPACHO
            'SAE = SECCIÓN ADUANA DE ENTRADA
            'SAS = SECCIÓN ADUANA DE SALIDA
            'SES = SECCIÓN ADUANA DE ENTRADA O SALIDA
            '******************************************
            '500 = INICIO DEL PEDIMENTO
            <EnumMember> <Description("Sin definir")> SinDefinir = 0
            <EnumMember> <Description("Clave del tipo de registro.")> ClaveDelTipoDeRegistro = 1
            <EnumMember> <Description("Tipo de Movimiento.")> TipoDeMovimientoS0VC002 = 2
            <EnumMember> <Description("Patente o autorización")> PatenteOAutorizacion = 3
            <EnumMember> <Description("Número de pedimento.")> NumeroDePedimento = 4
            <EnumMember> <Description("Aduana-sección de despacho.")> AduanaSeccionDeDespacho = 5
            <EnumMember> <Description("Acuse electrónico de validación")> AcuseElectronicoDeValidacion = 6
            '501 = DATOS GENERALES
            <EnumMember> <Description("Tipo de operación.")> TipoDeOperacion = 7
            <EnumMember> <Description("Clave de pedimento.")> ClaveDePedimento = 8
            <EnumMember> <Description("Aduana-sección de entrada o salida.")> AduanaSeccionDeEntradaOSalida = 9
            <EnumMember> <Description("CURP del importador o exportador")> CURPDelIOE = 10
            <EnumMember> <Description("RFC del importador o exportador")> RFCDelIOE = 11
            <EnumMember> <Description("CURP del agente aduanal, representante legal, apodarado o mandatario.")> CURPAgenteAduanalUOtro = 12
            <EnumMember> <Description("Tipo de cambio")> TipoDeCambio = 13
            <EnumMember> <Description("Importe del pago de fletes")> ImporteDelPagoDeFletes = 14
            <EnumMember> <Description("Importe del pago de primas de seguros")> ImporteDelPagoDePrimasDeSeguros = 15
            <EnumMember> <Description("Importe del pago de embalajes")> ImporteDelPagoDeEmbalajes = 16
            <EnumMember> <Description("Importe del pago de otros incrementables")> ImporteDelPagoDeOtrosIncrementables = 17

            <EnumMember> <Description("Uso futuro")> UsoFuturo = 18
            <EnumMember> <Description("Peso bruto total de la mercancía")> PesoBrutoTotalDeLaMercancia = 19
            <EnumMember> <Description("Medio de transporte de salida de la aduana-sección de salida")> MedioDeTransporteDeSalidaDeLaSAS = 20
            <EnumMember> <Description("Medio de transporte de arribo a la aduana-sección de arribo")> MedioDeTransporteDeArriboALaAduanaSeccionDeArribo = 21
            <EnumMember> <Description("Medio de transporte utilizado a la entrada o salida de la mercancía a territorio nacional")> MedioDeTransporteUtilizadoALaEntradaOSalidaDeLaMercanciaATerritorioNacional = 22
            <EnumMember> <Description("Origen o destino de la mercancía")> OrigenODestinoDeLaMercancia = 23

            <EnumMember> <Description("Nombre del importador o exportador")> NombreDelIOE = 24
            <EnumMember> <Description("Calle del domicilio del importador o exportador")> CalleDelDomicilioDelIOE = 25
            <EnumMember> <Description("Número interior del domicilio del importador o exportador")> NumeroInteriorDelDomicilioDelIOE = 26
            <EnumMember> <Description("Número exterior del domicilio del importador o exportador")> NumeroExteriorDelDomicilioDelIOE = 27
            <EnumMember> <Description("Código postal del domicilio fiscal del importador o exportador")> CodigoPostalDelDomicilioFiscalDelIOE = 28
            <EnumMember> <Description("Municipio del domicilio fiscal del importador o exportador")> MunicipioDelDomicilioFiscalDelIOE = 29
            <EnumMember> <Description("Entidad federativa del domicilio del importador o exportador")> EntidadFederativaDelIOE = 30
            <EnumMember> <Description("País del domicilio fiscal del importador o exportador")> PaisDelDomicilioFiscalDelIOE = 31
            <EnumMember> <Description("RFC de quien emite el CFDi o documento equivalente de los servicios de operación")> RFCDelEmisorCFDI = 32

            <EnumMember> <Description("Decrementables por fletes")> DecrementablesPorFletes = 33
            <EnumMember> <Description("Decrementables por seguros")> DecrementablesPorSeguros = 34
            <EnumMember> <Description("Decrementables por carga")> DecrementablesPorCarga = 35
            <EnumMember> <Description("Decrementables por descarga")> DecrementablesPorDescarga = 36
            <EnumMember> <Description("Otros decrementables")> OtrosDecrementables = 37
            '502 = TRANSPORTE
            '<EnumMember> <Description("Clave del tipo de registro")> ClaveDelTipoDeRegistro = 1
            '<EnumMember> <Description("Número de pedimento.")> NumeroDePedimento = 4
            <EnumMember> <Description("RFC del transportista")> RFCDelTransportista = 38
            <EnumMember> <Description("CURP del transportista")> CURPDelTransportista = 39
            <EnumMember> <Description("Nombre del transportista")> NombreDelTransportista = 40
            <EnumMember> <Description("País del medio de transporte")> PaisDelMedioDeTransporte = 41
            <EnumMember> <Description("Identificador del transporte")> IdentificadorDelTransporte = 42
            <EnumMember> <Description("Total del bultos")> TotalDeBultos = 43
            <EnumMember> <Description("Domicilio fiscal del transportista")> DomicilioFiscalDelTransportista = 44
            '503 = GUÍAS ( para Array )
            '<EnumMember> <Description("Clave del tipo de registro")> ClaveDelTipoDeRegistro = 1
            '<EnumMember> <Description("Número de pedimento.")> NumeroDePedimento = 4
            <EnumMember> <Description("Número de guia, manifiesto o conocimiento de embarque")> NumeroDeGuia = 45
            <EnumMember> <Description("Identificador de la guía")> IdentificadorDeLaGuia = 46
            '504 = CONTENEDORES ( para Array )
            '<EnumMember> <Description("Clave del tipo de registro")> ClaveDelTipoDeRegistro = 1
            '<EnumMember> <Description("Número de pedimento.")> NumeroDePedimento = 4
            <EnumMember> <Description("Número de contenedor")> NumeroDeContenedor = 47
            <EnumMember> <Description("Tipo de contenedor")> TipoDeContenedor = 48

        End Enum

        'Campos referencia
        '        <JsonConverter(TypeOf (StringEnumConverter))>
        '<BsonRepresentation(BsonType.String)>
        '<BsonRepresentation(BsonType.String)>
        Public Enum CamposReferencia
            'Región del 2501 - 2999 ESTA ES SU NUEVA NUMERACIÓN

            '#############################  CAMPOS ÚNICOS DE LA REFERENCIA ##################################
            'Abreviaciones genenerales
            'IOE = IMPORTADOR O EXPORTADOR
            'PRO = PROVEEDOR
            'AAD = AGENTE ADUANAL
            'SAD = SECCIÓN ADUANA DE DESPACHO
            'SAE = SECCIÓN ADUANA DE ENTRADA
            'SAS = SECCIÓN ADUANA DE SALIDA
            'SES = SECCIÓN ADUANA DE ENTRADA O SALIDA
            '******************************************
            'GENERALES
            <EnumMember> <Description("SIN DEFINIR")> SIN_DEFINIR = 2501

            <EnumMember> <Description("REFERENCIA")> CP_REFERENCIA = 2502
            <EnumMember> <Description("MATERIAL PELIGROSO")> CP_MATERIAL_PELIGROSO = 2503
            <EnumMember> <Description("RECTIFICACION")> CP_RECTIFICACION = 2504
            <EnumMember> <Description("TIPO DE PEDIMENTO")> CP_TIPO_PEDIMENTO = 2505
            <EnumMember> <Description("DESADUANAMIENTO")> CP_DESADUANAMIENTO = 2506
            <EnumMember> <Description("DESCRIPCION MERCANCIA COMPLETA")> CP_DESCRIPCION_MERCANCIA_COMPLETA = 2507
            <EnumMember> <Description("TIPO CARGA AGENCIA")> CP_TIPO_CARGA_AGENCIA = 2508
            <EnumMember> <Description("FECHA DE APERTURA")> CP_FECHA_APERTURA = 2509
            <EnumMember> <Description("FECHA DE LA PROFORMA")> CP_FECHA_PROFORMA = 2510
            <EnumMember> <Description("FECHA DE CIERRE")> CP_FECHA_CIERRE_DOCUMENTAL = 2511
            <EnumMember> <Description("FECHA DE PAGO")> CP_FECHA_PAGO = 2512
            <EnumMember> <Description("FECHA ULTIMO DESPACHO")> CP_FECHA_ULTIMO_DESPACHO = 2513
            <EnumMember> <Description("FECHA SALIDA")> CP_FECHA_SALIDA = 2514
            <EnumMember> <Description("FECHA DE PREVIO")> CP_FECHA_PREVIO = 2515
            <EnumMember> <Description("FECHA DE ETD")> CP_FECHA_ETD = 2517
            <EnumMember> <Description("FECHA DE CIERRE FISICO")> CP_FECHA_CIERRE_FISICO = 2518
            <EnumMember> <Description("FECHA DE ETA")> CP_FECHA_ETA = 2519
            <EnumMember> <Description("FECHA DE REVALIDACION")> CP_FECHA_REVALIDACION = 2520
            <EnumMember> <Description("ID DOCUMENTO")> CP_ID_DOCUMENTO = 2521
            <EnumMember> <Description("NOMBRE DOCUMENTO")> CP_NOMBRE_DOCUMENTO = 2522
            <EnumMember> <Description("TIPO DOCUMENTO")> CP_TIPO_DOCUMENTO = 2523
            <EnumMember> <Description("ESTADO REFERENCIA")> CP_ESTADO_REFERENCIA = 2524
            <EnumMember> <Description("TIPO DEL DESPACHO")> CP_TIPO_DESPACHO = 2525
            <EnumMember> <Description("PEDIMENTO ORIGINAL")> CP_PEDIMENTO_ORIGINAL = 2526
            <EnumMember> <Description("ID DE LA REFERENCIA")> CP_ID_REFERENCIA = 2527
            <EnumMember> <Description("FECHA DE FONDEO")> CP_FECHA_FONDEO = 2528
            <EnumMember> <Description("FECHA DE ATRAQUE")> CP_FECHA_ATRAQUE = 2529
            <EnumMember> <Description("FECHA DESPACHO")> CP_FECHA_DESPACHO = 2530
            '<EnumMember> <Description("NUMERO GUIA")> CP_NUMERO_GUIA = 2521
            '<EnumMember> <Description("TRANSPORTISTA")> CP_TRANSPORTISTA = 2522
            '<EnumMember> <Description("PAIS")> CP_PAIS = 2523
            '<EnumMember> <Description("TIPO DE CARGA")> CP_TIPO_CARGA = 2524
            '<EnumMember> <Description("PESO BRUTO")> CP_PESOBRUTO = 2525
            '<EnumMember> <Description("UNIDAD DE MEDIDA")> CP_UNIDADMEDIDA = 2526
            '<EnumMember> <Description("RECINTO FISCAL")> CP_RECINTO_FISCAL = 2527
            '<EnumMember> <Description("TIPO DE GUIA")> CP_TIPO_GUIA = 2528
            '<EnumMember> <Description("FECHA SALIDA DE ORIGEN")> CP_FECHA_SALIDA_ORIGEN = 2529
            '<EnumMember> <Description("DESCRIPCION MERCANCIA")> CP_DESCRIPCION_MERCANCIA = 2530
            '<EnumMember> <Description("SW DESPACHO")> CP_SW_DESADUANAMIENTO = 2535
            '<EnumMember> <Description("GUIA MULTIPLE")> CP_GUIA_MULTIPLE = 2536
            '<EnumMember> <Description("NUMERO GUIA")> CP_NUMERO_GUIA_MULTIPLE = 2537
            '<EnumMember> <Description("TRANSPORTISTA")> CP_TRANSPORTISTA_MULTIPLE = 2538
            '<EnumMember> <Description("PAIS")> CP_PAIS_MULTIPLE = 2539
            '<EnumMember> <Description("TIPO DE CARGA")> CP_TIPO_CARGA_MULTIPLE = 2540
            '<EnumMember> <Description("PESO BRUTO")> CP_PESOBRUTO_MULTIPLE = 2541
            '<EnumMember> <Description("UNIDAD DE MEDIDA")> CP_UNIDADMEDIDA_MULTIPLE = 2542
            '<EnumMember> <Description("TIPO DE GUIA")> CP_TIPO_GUIA_MULTIPLE = 2543
            '<EnumMember> <Description("FECHA SALIDA DE ORIGEN")> CP_FECHA_SALIDA_ORIGEN_MULTIPLE = 2544
            '<EnumMember> <Description("DESCRIPCION MERCANCIA")> CP_DESCRIPCION_MERCANCIA_MULTIPLE = 2545
            '<EnumMember> <Description("NOMBRE DEL CONSIGNATARIO")> CP_CONSIGNATARIO = 2546
            '<EnumMember> <Description("NOMBRE DEL CONSIGNATARIO")> CP_CONSIGNATARIO_MULTIPLE = 2547

        End Enum

        Public Enum CamposFacturaComercial
            'Región del 3000 - 3999
            <EnumMember> <Description("Sin definir")> SIN_DEFINIR = 3000

            <EnumMember> <Description("Número de factura/Folio fiscal")> CA_NUMERO_FACTURA = 3001
            <EnumMember> <Description("Fecha de factura")> CA_FECHA_FACTURA = 3002
            <EnumMember> <Description("Orden de compra")> CP_ORDEN_COMPRA = 3003
            <EnumMember> <Description("Clave país de facturación")> CA_CVE_PAIS_FACTURACION = 3004
            <EnumMember> <Description("País de facturación")> CA_PAIS_FACTURACION = 3005
            <EnumMember> <Description("Tipo de operación")> CP_TIPO_OPERACION = 3006
            <EnumMember> <Description("Clave Incoterm")> CA_CVE_INCOTERM = 3007
            <EnumMember> <Description("Valor factura")> CP_VALOR_FACTURA = 3008
            <EnumMember> <Description("Moneda factura")> CA_MONEDA_FACTURACION = 3009
            <EnumMember> <Description("Valor mercancía")> CP_VALOR_MERCANCIA = 3010
            <EnumMember> <Description("Moneda valor mercancia")> CP_MONEDA_VALOR_MERCANCIA = 3011
            <EnumMember> <Description("Peso total (kg)")> CP_PESO_TOTAL = 3012
            <EnumMember> <Description("Enajenación")> CP_APLICA_ENAJENACION = 3013
            <EnumMember> <Description("Subdivisión")> CA_APLICA_SUBDIVISION = 3014
            <EnumMember> <Description("Serie/folio de la factura")> CP_SERIE_FOLIO_FACTURA = 3015
            <EnumMember> <Description("Clave vinculación")> CA_CVE_VINCULACION = 3016
            <EnumMember> <Description("Método de valoración")> CP_CVE_METODO_VALORACION = 3017
            <EnumMember> <Description("Funge como certificado")> CA_APLICA_CERTIFICADO = 3018
            <EnumMember> <Description("Nombre del certificador")> CP_NOMBRE_CERTIFICADOR = 3019

            'Partidas de Factura
            <EnumMember> <Description("Número de partida")> CP_NUMERO_PARTIDA = 3020
            <EnumMember> <Description("Número de parte")> CA_NUMERO_PARTE_PARTIDA = 3021
            <EnumMember> <Description("Valor factura")> CA_VALOR_FACTURA_PARTIDA = 3022
            <EnumMember> <Description("Moneda factura")> CP_MONEDA_FACTURA_PARTIDA = 3023
            <EnumMember> <Description("Valor mercancía")> CA_VALOR_MERCANCIA_PARTIDA = 3024
            <EnumMember> <Description("Moneda mercancía")> CA_MONEDA_MERCANCIA_PARTIDA = 3025
            <EnumMember> <Description("Clave método de valoración")> CA_CVE_METODO_VALORACION_PARTIDA = 3026
            <EnumMember> <Description("Peso neto (Kg)")> CA_PESO_NETO_PARTIDA = 3027
            <EnumMember> <Description("Precio unitario")> CA_PRECIO_UNITARIO_PARTIDA = 3028
            <EnumMember> <Description("País de origen")> CA_PAIS_ORIGEN_PARTIDA = 3029
            <EnumMember> <Description("Cantidad factura")> CP_CANTIDAD_FACTURA_PARTIDA = 3030
            <EnumMember> <Description("Unidad de medida factura")> CP_UNIDAD_MEDIDA_FACTURA_PARTIDA = 3031
            <EnumMember> <Description("Descripción")> CA_DESCRIPCION_PARTE_PARTIDA = 3032
            <EnumMember> <Description("Aplica para COVE")> CP_APLICA_DESCRIPCION_COVE_PARTIDA = 3033
            <EnumMember> <Description("Cantidad comercial")> CA_CANTIDAD_COMERCIAL_PARTIDA = 3034
            <EnumMember> <Description("Unidad de medida comercial")> CA_UNIDAD_MEDIDA_COMERCIAL_PARTIDA = 3035
            <EnumMember> <Description("Descripción COVE")> CA_DESCRIPCION_COVE_PARTIDA = 3036
            <EnumMember> <Description("Fracción arancelaria")> CA_FRACCION_ARANCELARIA_PARTIDA = 3037
            <EnumMember> <Description("Cantidad tarifa")> CA_CANTIDAD_TARIFA_PARTIDA = 3038
            <EnumMember> <Description("Unidad de medida tarifa")> CA_UNIDAD_MEDIDA_TARIFA_PARTIDA = 3039
            <EnumMember> <Description("Nico")> CA_FRACCION_NICO_PARTIDA = 3040
            <EnumMember> <Description("Lote")> CA_LOTE_PARTIDA = 3041
            <EnumMember> <Description("Número de serie")> CA_NUMERO_SERIE_PARTIDA = 3042
            <EnumMember> <Description("Marca")> CA_MARCA_PARTIDA = 3043
            <EnumMember> <Description("Modelo")> CA_MODELO_PARTIDA = 3044
            <EnumMember> <Description("Submodelo")> CA_SUBMODELO_PARTIDA = 3045
            <EnumMember> <Description("Kilometraje")> CA_KILOMETRAJE_PARTIDA = 3046
            <EnumMember> <Description("Fletes")> CA_FLETES = 3047
            <EnumMember> <Description("Moneda fletes")> CA_MONEDA_FLETES = 3048
            <EnumMember> <Description("Seguros")> CA_SEGURO = 3049
            <EnumMember> <Description("Moneda seguros")> CA_MONEDA_SEGUROS = 3050
            <EnumMember> <Description("Embalajes")> CA_EMBALAJES = 3051
            <EnumMember> <Description("Moneda embalajes")> CA_MONEDA_EMBALAJES = 3052
            <EnumMember> <Description("Otros incrementables")> CA_OTROS_INCREMENTABLES = 3053
            <EnumMember> <Description("Moneda otros")> CA_MONEDA_OTROS_INCREMENTABLES = 3054
            <EnumMember> <Description("Descuentos")> CA_DESCUENTOS = 3055
            <EnumMember> <Description("Moneda descuentos")> CA_MONEDA_DESCUENTOS = 3056
            <EnumMember> <Description("Orden de compra partida")> CP_ORDEN_COMPRA_PARTIDA = 3057
            <EnumMember> <Description("Moneda precio unitario")> CP_MONEDA_PRECIO_UNITARIO = 3058
            <EnumMember> <Description("Referencia de cliente")> CP_REFERENCIA_CLIENTE = 3059
            <EnumMember> <Description("ObjectId Factura Comercial")> CP_OBJECTID_FACTURA = 3060
            <EnumMember> <Description("ObjectId Productos")> CP_OBJECTID_PRODUCTOS = 3061
            <EnumMember> <Description("ObjectId Fraccion")> CP_OBJECTID_FRACCION = 3062
            <EnumMember> <Description("País destino")> CA_PAIS_DESTINO_PARTIDA = 3063
            <EnumMember> <Description("Tipo de carga de datos")> CP_TIPO_CARGA_DATOS = 3064
            <EnumMember> <Description("Bultos")> CP_BULTOS = 3065
            <EnumMember> <Description("Valor dolares partida")> CA_VALOR_DOLARES_PARTIDA = 3066
            <EnumMember> <Description("Moneda valor dolares partida")> CP_MONEDA_VALOR_DOLARES_PARTIDA = 3067
            <EnumMember> <Description("Valor unitario partida")> CA_VALOR_UNITARIO_PARTIDA = 3068
            <EnumMember> <Description("Moneda valor unitario partida")> CA_MONEDA_VALOR_UNITARIO_PARTIDA = 3069
            <EnumMember> <Description("Descripción original")> CA_DESCRIPCION_PARTE_PARTIDA_ORIGINAL = 3070
            <EnumMember> <Description("Aplican incrementables")> CP_APLICA_INCREMENTABLES = 3071
            <EnumMember> <Description("Aplica descripcion original mercancia pedimento")> CP_APLICA_DESCRIPCION_ORIGINAL_MERCANCIA_PEDIMENTO = 3072
            <EnumMember> <Description("Indicador si está marcado por algún pedimento")> CP_MARCADO_PEDIMENTO = 3073
            <EnumMember> <Description("ObjectId del pedimento al que fue asociado")> CP_ID_PEDIMENTO_ASOCIADO = 3074
        End Enum


        Public Enum CamposAcuseValor
            'Región del 4000 - 4999
            <EnumMember> <Description("Sin definir")> SIN_DEFINIR = 4000
            <EnumMember> <Description("ObjectID FacturaComercial")> CP_ID_FACTURA_ACUSEVALOR = 4001
            <EnumMember> <Description("Número COVE SYSTEM")> CP_NUMERO_SYSTEM_ACUSEVALOR = 4002
            <EnumMember> <Description("FOLIO COVE")> CA_NUMERO_ACUSEVALOR = 4003
            <EnumMember> <Description("Tipo de Documento")> CP_TIPO_DOCUMENTO_ACUSEVALOR = 4004
            <EnumMember> <Description("Fecha COVE")> CA_FECHA_ACUSEVALOR = 4005
            <EnumMember> <Description("Relación de Facturas")> CA_RELACION_FACTURA_ACUSEVALOR = 4006
            <EnumMember> <Description("Número del exportador autorizado")> CA_NUMERO_EXPORTADOR_ACUSEVALOR = 4007
            <EnumMember> <Description("Observaciones")> CA_OBSERVACIONES_ACUSEVALOR = 4008
            <EnumMember> <Description("ObjectID Proveedor")> CP_ID_Proveedor_ACUSEVALOR = 4009
            <EnumMember> <Description("ObjectID Destinatario")> CP_ID_Destinatario_ACUSEVALOR = 4010

            'Partidas de ACUSEVALOR
            <EnumMember> <Description("Descripción COVE")> CA_DESCRIPCION_PARTIDA_ACUSEVALOR = 4011
            <EnumMember> <Description("Unidad de medida COVE")> CA_UNIDAD_MEDIDA_FACTURA_PARTIDA_ACUSEVALOR = 4012
            <EnumMember> <Description("Valor mercancía dólares")> CA_VALOR_MERCANCIA_PARTIDA_DOLARES_ACUSEVALOR = 4013

            'Detalles Partida ACUSEVALOR
            'TODOS ESTOS DATOS ESTÄN EN LA FACTURA SON NUMERO DE SERIE, MARCA, MODELO Y SUBMODELO

            'Configuración ACUSEVALOR
            <EnumMember> <Description("Sello del Importador")> CA_SELLO_ACUSEVALOR = 4014
            <EnumMember> <Description("Patente del Agente Aduanal")> CA_PATENTE_ACUSEVALOR = 4015
            <EnumMember> <Description("RFC'S de Consulta")> CA_RFC_CONSULTA_ACUSEVALOR = 4016
            <EnumMember> <Description("E-mail de Consulta")> CP_EMAIL_CONSULTA_ACUSEVALOR = 4017
            <EnumMember> <Description("ID del Acuse de Valor")> CP_ID_ACUSEVALOR = 4018

        End Enum

        Public Enum CamposProveedorOperativo
            'Región del 5000 - 5999
            <EnumMember> <Description("Sin definir")> SIN_DEFINIR = 5000

            <EnumMember> <Description("Identificador de proveedor operativo")> CP_CVE_PROVEEDOR = 5001
            <EnumMember> <Description("Identificador de empresa")> CP_CVE_EMPRESA = 5002
            <EnumMember> <Description("Nombre o razón social")> CA_RAZON_SOCIAL_PROVEEDOR = 5003
            <EnumMember> <Description("Tipo de uso")> CP_TIPO_USO = 5004

            <EnumMember> <Description("Lista detalle proveedor operativo")> CP_DETALLE_PROVEEDOR = 5005
            <EnumMember> <Description("Identificador detalle proveedor operativo")> CP_SECUENCIA_PROVEEDOR = 5006
            <EnumMember> <Description("Taxid")> CA_TAX_ID_PROVEEDOR = 5007
            <EnumMember> <Description("RFC")> CA_RFC_PROVEEDOR = 5008
            <EnumMember> <Description("Curp (OPCIONAL)")> CA_CURP_PROVEEDOR = 5009

            <EnumMember> <Description("Lista de domicilios fiscales")> CP_DOMICILIOS_FISCALES = 5010
            <EnumMember> <Description("Secuencia domicilio proveedor")> CP_SECUENCIA_PROVEEDOR_DOMICILIO = 5011
            <EnumMember> <Description("Tax id del domicilio")> CP_TAX_ID_DOMICILIO = 5012
            <EnumMember> <Description("RFC del domicilio")> CP_RFC_PROVEEDOR_DOMICILIO = 5013
            <EnumMember> <Description("ObjectID Domicilio")> CP_ID_DOMICILIOS = 5014
            <EnumMember> <Description("Domicilio fiscal proveedor")> CA_DOMICILIO_FISCAL = 5015
            <EnumMember> <Description("Domicilio archivado")> CP_ARCHIVADO_DOMICILIO = 5016

            <EnumMember> <Description("Lista de vinculaciones con clientes")> CP_VINCULACIONES = 5017
            <EnumMember> <Description("Identificador del cliente a vincular")> CP_ID_CLIENTE_VINCULACION = 5018
            <EnumMember> <Description("Tax id vinculado")> CP_TAX_ID_VINCULACION = 5019
            <EnumMember> <Description("RFC vinculado")> CP_RFC_PROVEEDOR_VINCULACION = 5020
            <EnumMember> <Description("Identificador de la vinculación")> CA_CVE_VINCULACION = 5021
            <EnumMember> <Description("Vinculación")> CP_VINCULACION = 5022
            <EnumMember> <Description("Porcetanje de la vinculación")> CP_PORCENTAJE_VINCULACION = 5023

            <EnumMember> <Description("Lista de configuraciones")> CP_CONFIGURACIONES = 5024
            <EnumMember> <Description("Identificador del cliente a configurar")> CP_ID_CLIENTE_CONFIGURACION = 5025
            <EnumMember> <Description("Tax id configurado")> CP_TAX_ID_CONFIGURACION = 5026
            <EnumMember> <Description("RFC configurado")> CP_RFC_PROVEEDOR_CONFIGURACION = 5027
            <EnumMember> <Description("Identificador del metódo de valoración")> CA_CVE_METODO_VALORACION = 5028
            <EnumMember> <Description("Metódo de valoración")> CP_METODO_VALORACION = 5029
            <EnumMember> <Description("Identificador del termino de facturación (INCOTERM)")> CA_CVE_INCOTERM = 5030
            <EnumMember> <Description("Termino de facturación (INCOTERM)")> CP_INCOTERM = 5031

            <EnumMember> <Description("ObjectID Empresa")> CP_ID_EMPRESA = 5032
            <EnumMember> <Description("ObjectID Proveedor")> CP_ID_PROVEEDOR = 5033
            <EnumMember> <Description("Tipo Persona")> CP_TIPO_PERSONA_PROVEEDOR = 5034
            <EnumMember> <Description("Destinatario")> CP_DESTINATARIO_PROVEEDOR = 5035

            <EnumMember> <Description("Id Taxid")> CA_CVE_TAX_ID_PROVEEDOR = 5036
            <EnumMember> <Description("Id RFC")> CA_CVE_RFC_PROVEEDOR = 5037
            <EnumMember> <Description("Id Curp")> CA_CVE_CURP_PROVEEDOR = 5038
            <EnumMember> <Description("Tipo proveedor")> CP_TIPO_PROVEEDOR = 5039

            <EnumMember> <Description("Id domicilio proveedor")> CP_ID_DOMICILIO_PROVEEDOR = 5040
            <EnumMember> <Description("Sec domicilio proveedor")> CP_SEC_DOMICILIO_PROVEEDOR = 5041
            <EnumMember> <Description("Proveedor habilitado")> CP_PROVEEDOR_HABILITADO = 5042
            <EnumMember> <Description("Estado domicilio proveedor")> CA_ESTADO_DOMICILIO_PROVEEDOR = 5043
            <EnumMember> <Description("Domicilio archivado proveedor")> CA_DOMICILIO_ARCHIVADO_PROVEEDOR = 5044
            <EnumMember> <Description("Domicilio archivado proveedor")> CA_MOTIVO_ARCHIVADO_DOMICILIO_PROVEEDOR = 5045
            <EnumMember> <Description("Fecha archivado proveedor")> CA_FECHA_ARCHIVADO_DOMICILIO_PROVEEDOR = 5046
            <EnumMember> <Description("Firma Electronica")> CP_FIRMA_ELECTRONICA = 5047

        End Enum

        Public Enum CamposDestinatario
            'Región del 6000 - 6999
            <EnumMember> <Description("Sin definir")> SIN_DEFINIR = 6000

            <EnumMember> <Description("Nombre o razón social")> CA_RAZON_SOCIAL = 6001
            <EnumMember> <Description("Tax ID")> CA_TAX_ID = 6002
            <EnumMember> <Description("RFC")> CA_RFC_DESTINATARIO = 6003

            <EnumMember> <Description("ObjectID Empresa")> CP_ID_EMPRESA = 6004
            <EnumMember> <Description("Identificador de empresa")> CP_CVE_EMPRESA = 6005

            <EnumMember> <Description("ObjectID Destinatario")> CP_ID_DESTINATARIO = 6006
            <EnumMember> <Description("Clave Destinatario")> CP_CVE_DESTINATARIO = 6007

            <EnumMember> <Description("Id domicilio destinatario")> CP_ID_DOMICILIO_DESTINATARIO = 6008
            <EnumMember> <Description("Sec domicilio destinatario")> CP_SEC_DOMICILIO_DESTINATARIO = 6009
            <EnumMember> <Description("Domicilio destinatario")> CA_DOMICILIO_FISCAL_DESTINATARIO = 6010
            <EnumMember> <Description("Destinatario habilitado")> CA_DESTINATARIO_HABILITADO = 6011
            <EnumMember> <Description("Estado domicilio destinatario")> CA_ESTADO_DOMICILIO_DESTINATARIO = 6012
            <EnumMember> <Description("Domicilio archivado destinatario")> CA_DOMICILIO_ARCHIVADO_DESTINATARIO = 6013
            <EnumMember> <Description("Motivo archivado domicilio destinatario")> CA_MOTIVO_ARCHIVADO_DOMICILIO_DESTINATARIO = 6014
            <EnumMember> <Description("Fecha archivado domicilio destinatario")> CA_FECHA_ARCHIVADO_DOMICILIO_DESTINATARIO = 6015
            <EnumMember> <Description("Object id del tax id del destinatario")> CA_CVE_TAX_ID_DESTINATARIO = 6016
            <EnumMember> <Description("Tipo de destinario Extranjero o Nacional")> CP_TIPO_DESTINATARIO = 6017
        End Enum

        Public Enum CamposRevalidacion
            'Región del 7000 - 7999
            <EnumMember> <Description("Sin definir")> SIN_DEFINIR = 7000
            '<EnumMember> <Description("Referencia")> CP_REFERENCIA = 7001
            '<EnumMember> <Description("Nombre o razón social")> CA_RAZON_SOCIAL = 7002
            <EnumMember> <Description("No. Guia Master")> CP_NO_GUIA_MASTER = 7003 ' <----
            <EnumMember> <Description("Revalidado")> CP_REVALIDADO = 7004
            <EnumMember> <Description("Fecha Revalidacion")> CP_FECHA_REVALIDACION = 7005
            <EnumMember> <Description("Tipo de Carga")> CP_TIPO_CARGA = 7006 '<--- ControladorAtipico
            <EnumMember> <Description("BL Revalidado")> CP_ID_BLREVALIDADO = 7007 'Tipo ObjectID

            <EnumMember> <Description("Clase Carga")> CP_CLASE_CARGA = 7008 '<--- evualar...
            <EnumMember> <Description("Cantidad Carga")> CP_CANTIDAD_CARGA = 7009
            <EnumMember> <Description("Peso Carga")> CP_PESO_CARGA = 7010

            <EnumMember> <Description("No. Contenedor")> CP_CONTENEDOR = 7011 'MARCASY NUMEROS <--
            <EnumMember> <Description("Tamaño Contenedor")> CP_TAMANO_CONTENEDOR = 7012 'TAMAÑO <--
            <EnumMember> <Description("Peso Contenedor")> CP_PESO_CONTENEDOR = 7013 '????
        End Enum

        Public Enum CamposViajes
            'Región del 8000 - 8999
            <EnumMember> <Description("Sin definir")> SIN_DEFINIR = 8000
            <EnumMember> <Description("Tipo de Transporte")> CP_TIPO_TRANSPORTE = 8001 '
            <EnumMember> <Description("Tipo de Operación")> CP_TIPO_OPERACION = 8002
            <EnumMember> <Description("Nave/Buque")> CP_NAVE_BUQUE = 8003
            <EnumMember> <Description("Naviera/Aereolinea")> CP_NAVIERA_AEREOLINEA = 8004
            <EnumMember> <Description("Reexpedidora/Forwarding")> CP_REEXPEDIDORA_FORWARDING = 8005
            <EnumMember> <Description("Folio de Capitania")> CP_FOLIO_CAPITANIA = 8006
            <EnumMember> <Description("Número de Viaje")> CP_NUMERO_VIAJE = 8007
            <EnumMember> <Description("Puerto Extranjero")> CP_PUERTO_EXTRANGERO = 8008
            <EnumMember> <Description("Fecha de Salida Origen")> CP_FECHA_SALIDA_ORIGEN = 8009
            <EnumMember> <Description("Fecha ETA")> CP_FECHA_ETA = 8010
            <EnumMember> <Description("Fecha ETD")> CP_FECHA_ETD = 8011
            <EnumMember> <Description("Fecha de Fondeo")> CP_FECHA_FONDEO = 8012
            <EnumMember> <Description("Fecha de Atraque")> CP_FECHA_ATRAQUE = 8013
            <EnumMember> <Description("Fecha de Cieere de Documento")> CP_FECHA_CIERRE_DOCUMENTO = 8014
            <EnumMember> <Description("Fecha de Presentación")> CP_FECHA_PRESENTACION = 8015
            '<EnumMember> <Description("Referencia")> CP_REFERENCIA = 8012
            '<EnumMember> <Description("Operación")> CP_OPERACION = 8013
            '<EnumMember> <Description("Cliente")> CA_RAZON_SOCIAL = 8014
            '<EnumMember> <Description("Estatus")> CP_ESTATUS = 8015
            '<EnumMember> <Description("Ejecutivo")> CP_EJECUTIVO = 8016
        End Enum

        Public Enum CamposProducto
            'Región del 9000 - 9999
            <EnumMember> <Description("Sin definir")> SIN_DEFINIR = 9000
            <EnumMember> <Description("Nombre Comercial")> CP_NOMBRE_COMERCIAL = 9001
            <EnumMember> <Description("Habilitado")> CP_HABILITADO = 9002
            <EnumMember> <Description("Fracción Arancelaria")> CP_FRACCION_ARANCELARIA = 9003
            <EnumMember> <Description("Nico")> CP_NICO = 9004
            <EnumMember> <Description("Fecha de Registro")> CP_FECHA_REGISTRO = 9005
            <EnumMember> <Description("Estatus")> CP_ESTATUS = 9006
            <EnumMember> <Description("Observaciones")> CP_OBSERVACION = 9007
            <EnumMember> <Description("Motivo de Archivado")> CP_MOTIVO = 9008
            <EnumMember> <Description("Id Krom")> CP_IDKROM = 9009
            <EnumMember> <Description("Número de Parte")> CP_NUMERO_PARTE = 9010
            <EnumMember> <Description("Alias")> CP_ALIAS = 9011
            <EnumMember> <Description("Descripción")> CP_DESCRIPCION = 9012
            <EnumMember> <Description("Aplica COVE")> CP_APLICACOVE = 9013
            <EnumMember> <Description("Descripcion COVE")> CP_DESCRIPCION_COVE = 9014
            <EnumMember> <Description("Fecha Modificacion")> CP_FECHA_MODIFICACION = 9015
            <EnumMember> <Description("Fecha Modificacion")> CP_TIPO_ALIAS = 9016
            <EnumMember> <Description("ObjectId Producto")> CP_OBJECTID_PRODUCTO = 9017
            <EnumMember> <Description("Descripcion Fracción Arancelaria")> CP_DESCRIPCION_FRACCION_ARANCELARIA = 9018
            <EnumMember> <Description("Descripcion Nico")> CP_DESCRIPCION_NICO = 9019
            <EnumMember> <Description("Ruta del archivo de la muestra del producto")> CP_RUTA_ARCHIVO_MUESTRA = 9020
            <EnumMember> <Description("Login del usuario que actualiza")> CP_LOGIN_USUARIO = 9021
            <EnumMember> <Description("Enviroment de la oficina donde se actualiza")> CP_ENVIRONMENT = 9022
        End Enum

        Public Enum CamposTarifaArancelaria
            ''Región del 10100 - 11000
            ''Generales
            <EnumMember> <Description("Sin definir")> SIN_DEFINIR = 10100
            <EnumMember> <Description("Numero fraccion arancelaria")> CA_FRACCION_ARANCELARIA = 10101
            <EnumMember> <Description("Numero nico")> CA_NICO = 10102
            <EnumMember> <Description("Descripcion fraccion arancelaria")> CA_DESCRIPCION_FRACCION_ARANCELARIA = 10103
            <EnumMember> <Description("Descripcion nico")> CA_DESCRIPCION_NICO = 10104
            <EnumMember> <Description("Clave unidad de medida")> CA_CLAVE_UNIDAD_MEDIDA = 10152 'Se mantiene enum anterior
            <EnumMember> <Description("Unidad de medida")> CA_UNIDAD_MEDIDA = 10116 'Se mantiene enum anterior
            <EnumMember> <Description("Unidad de medida nombre corto")> CA_UNIDAD_MEDIDA_CORTO = 10162 'Se mantiene enum anterior
            <EnumMember> <Description("Material peligroso")> CA_MATERIAL_PELIGROSO = 10105
            <EnumMember> <Description("Material vulnerable")> CA_MATERIAL_VULNERABLE = 10106
            <EnumMember> <Description("Material sensible")> CA_MATERIAL_SENSIBLE = 10107
            <EnumMember> <Description("Seccion")> CA_SECCION = 10108
            <EnumMember> <Description("Capitulo")> CA_CAPITULO = 10109
            <EnumMember> <Description("Partida")> CA_PARTIDA = 10110
            <EnumMember> <Description("Subpartida")> CA_SUBPARTIDA = 10111
            <EnumMember> <Description("ObjectId historico")> CP_ID_HISTORICO = 10112
            <EnumMember> <Description("Fecha actualización")> CP_FECHA_ACTUALIZACION = 10113
            <EnumMember> <Description("Fecha publicación")> CA_FECHA_PUBLICACION = 10114
            <EnumMember> <Description("Fecha entrada en vigor")> CA_FECHA_ENTRADA_VIGOR = 10115
            <EnumMember> <Description("Fecha fin")> CA_FECHA_FIN = 10117

            <EnumMember> <Description("Object Id Importacion")> CP_ID_IMPORTACION = 10118
            <EnumMember> <Description("Object Id Exportacion")> CP_ID_EXPORTACION = 10119
            <EnumMember> <Description("Firma electronica importacion")> CP_FIRMA_ELECTRONICA_IMPORTACION = 10120
            <EnumMember> <Description("Firma electronica exportacion")> CP_FIRMA_ELECTRONICA_EXPORTACION = 10121

            ''RAs
            ''Impuestos
            <EnumMember> <Description("ObjectId impuesto")> CP_ID_IMPUESTO = 10122
            <EnumMember> <Description("Clave impuesto")> CA_CLAVE_IMPUESTO = 10123
            <EnumMember> <Description("Nombre impuesto")> CA_NOMBRE_IMPUESTO = 10124
            <EnumMember> <Description("Abreviacion impuesto")> CA_ABREVIACION_IMPUESTO = 10125
            <EnumMember> <Description("Clave tipo tasa")> CA_CLAVE_TIPO_TASA = 10126
            <EnumMember> <Description("Tipo tasa")> CA_TIPO_TASA = 10127
            <EnumMember> <Description("Tasa ")> CA_TASA = 10128
            '''Fechas

            ''Tratados
            <EnumMember> <Description("ObjectId tratado")> CP_ID_TRATADO = 10129
            <EnumMember> <Description("Nombre tratado")> CA_NOMBRE_TRATADO = 10130
            <EnumMember> <Description("Abreviacion tratado")> CA_ABREVIACION_TRATADO = 10131
            <EnumMember> <Description("Clave identificador")> CA_CLAVE_IDENTIFICADOR = 10132
            <EnumMember> <Description("Identificador")> CA_IDENTIFICADOR = 10133

            <EnumMember> <Description("ObjectId pais")> CP_ID_PAIS = 10134
            <EnumMember> <Description("Clave M3 Pais")> CA_CLAVE_M3_PAIS = 10135
            <EnumMember> <Description("Nombre pais")> CA_NOMBRE_PAIS = 10136
            <EnumMember> <Description("Complemento 1")> CA_COMPLEMENTO1 = 10137
            <EnumMember> <Description("Complemento 2")> CA_COMPLEMENTO2 = 10138
            <EnumMember> <Description("Complemento 3")> CA_COMPLEMENTO3 = 10139

            <EnumMember> <Description("ObjectId preferencia")> CP_ID_PREFERENCIA = 10140
            '''Tasas
            <EnumMember> <Description("Observacion")> CA_OBSERVACION = 10141
            <EnumMember> <Description("Id nota")> CP_IDNOTA = 10142
            <EnumMember> <Description("Nota")> CP_NOTA = 10143
            '''Fechas

            ''Cupos arancel
            <EnumMember> <Description("ObjectId cupo arancel")> CP_ID_CUPO_ARANCEL = 10144
            ''''Paises
            <EnumMember> <Description("Icono pais")> CA_ICONO_PAIS = 10145
            <EnumMember> <Description("Total cupo")> CA_TOTAL_CUPO = 10146
            ''''Tasas
            <EnumMember> <Description("Arancel fuera")> CA_ARANCEL_FUERA = 10147
            ''''Unidad de Medida
            ''''Observacion
            <EnumMember> <Description("Nota")> CA_NOTA = 10148
            ''''Fechas

            ''IEPS
            <EnumMember> <Description("ObjectId IEPS")> CP_ID_IEPS = 10149
            <EnumMember> <Description("Categoria")> CA_CATEGORIA = 10150
            <EnumMember> <Description("Tipo")> CA_TIPO = 10151
            ''''Tasa
            <EnumMember> <Description("Cuota")> CA_CUOTA = 10153
            ''''UnidadDeMedida
            ''''Observacion
            ''''Fechas

            ''CuotasCompensatorias
            <EnumMember> <Description("ObjectId cuotas compensatorias")> CP_ID_CC = 10154
            <EnumMember> <Description("Empresa")> CA_EMPRESA = 10155
            ''''Paises
            ''''Cuota
            <EnumMember> <Description("Clave tipo cuota")> CA_CLAVE_TIPO_CUOTA = 10156
            <EnumMember> <Description("Tipo cuota")> CA_TIPO_CUOTA = 10157
            <EnumMember> <Description("Precio referencia")> CA_PRECIO_REFERENCIA = 10158
            <EnumMember> <Description("Cuantia margen discriminacion")> CA_CUANTIA_MARGEN_DISCRIMINACION = 10159
            <EnumMember> <Description("Acotacion")> CA_ACOTACION = 10160
            ''''Nota
            ''''Fechas

            ''PreciosEstimados
            <EnumMember> <Description("ObjectId precios estimados")> CP_ID_PE = 10161
            <EnumMember> <Description("Precio")> CA_PRECIO = 10163
            ''''Unidades Medida
            <EnumMember> <Description("Descripcion")> CA_DESCRIPCION = 10164
            ''''Fechas

            ''Aladis
            <EnumMember> <Description("ObjectId aladi")> CP_ID_ALADI = 10165
            <EnumMember> <Description("Numero aladi")> CA_NUMERO_ALADI = 10166
            <EnumMember> <Description("Nombre Aladi")> CA_NOMBRE_ALADI = 10167
            ''''identificadores
            ''''Paises
            ''''Complementos
            ''''IdHistorico
            ''''Tasas
            ''''Observacion
            ''''Notas
            ''''Fechas

            ''RRNAs
            ''Permisos
            <EnumMember> <Description("ObjectId Permisos")> CP_ID_PERMISO = 10168
            <EnumMember> <Description("Clave")> CA_CLAVE = 10169
            <EnumMember> <Description("Permiso")> CA_PERMISO = 10170
            ''''Acotacion
            ''''Fechas

            ''Normas
            <EnumMember> <Description("ObjectId Norma")> CP_ID_NORMA = 10171
            <EnumMember> <Description("Norma")> CA_NORMA = 10172
            '''''Descripcion
            ''''Acotacion
            ''''Fechas
            ''''Identificadores

            ''Anexos
            <EnumMember> <Description("ObjectId Anexo")> CP_ID_ANEXO = 10173
            <EnumMember> <Description("Numero")> CA_NUMERO = 10174
            <EnumMember> <Description("Nombre")> CA_NOMBRE = 10175
            ''''Descripcion
            ''''Acotacion
            ''''Fechas

            ''Embargos
            <EnumMember> <Description("ObjectId Embargo")> CP_ID_EMBARGO = 10176
            ''''Paises e icono
            <EnumMember> <Description("Aplicacion")> CA_APLICACION = 10177
            ''''Ácotacion
            <EnumMember> <Description("Mercancia")> CA_MERCANCIA = 10178
            ''''Fechas

            ''Cupos Maximos
            <EnumMember> <Description("ObjectId Cupo máximo")> CP_ID_CUPO_NO_ARANCELARIO = 10179
            ''''paises
            ''''unidades medida
            <EnumMember> <Description("Cupo")> CA_CUPO = 10180
            ''''Descripcion
            ''''Fechas

            ''Padron sectorial
            <EnumMember> <Description("Sector")> CA_SECTOR = 10181
            <EnumMember> <Description("Anexo")> CA_ANEXO = 10182
            ''''Acotacion
            ''''Descripcion
            ''''Fechas
            '''

        End Enum


        Public Enum CamposManifestacionValor
            'Región del 11000 - 11200
            <EnumMember> <Description("Sin definir")> SIN_DEFINIR = 11000



            <EnumMember> <Description("Número de manifestación")> CA_NUMERO_MANIFESTACION = 11001
            <EnumMember> <Description("Fecha de manifestación")> CA_FECHA_MANIFESTACION = 11002
            <EnumMember> <Description("Presenta anexos")> CA_HAY_ANEXOS = 11003
            <EnumMember> <Description("Precio pagado")> CA_VALOR_PAGADO = 11004
            <EnumMember> <Description("Conceptos que no integran valor de transaccion")> CA_CONCEPTOS_NO_VALOR = 11005
            <EnumMember> <Description("Anexa documentos Art 66")> CA_ANEXA_DOC_66 = 11006
            <EnumMember> <Description("Numero del anexo")> CA_NUM_ANEXO = 11007
            <EnumMember> <Description("Factura o documento anexado")> CP_FACTURA_DOCUMENTO_66 = 11008
            <EnumMember> <Description("Mercancía")> CA_MERCANCIA_66 = 11009
            <EnumMember> <Description("Factura o documento comercial")> CA_FAC_DOC_COMERCIAL_66 = 11010
            <EnumMember> <Description("Importe y moneda")> CA_IMPORTE_MONEDA_66 = 11011
            <EnumMember> <Description("Concepto del cargo")> CA_CONCEPTO_CARGO_66 = 11012
            <EnumMember> <Description("Precio pagado comprende conceptos señalados en art 65")> CA_PRECIO_COMPRENDE_ART_65 = 11013
            <EnumMember> <Description("Acompañar las facturas")> CA_ACOMPANA_FACT = 11014
            <EnumMember> <Description("Anexa documentos Art 65")> CA_ANEXA_DOC_65 = 11015
            <EnumMember> <Description("Periodicidad")> CA_PERIODICIDAD = 11016
            <EnumMember> <Description("Representante legal")> CA_REPRESENTANTE_LEGAL = 11017
            <EnumMember> <Description("Fecha manifestación")> CA_FECHA_MANIFESTACION_66 = 11018
            <EnumMember> <Description("Numero del anexo")> CA_NUM_ANEXO_65 = 110019
            <EnumMember> <Description("Factura o documento anexado")> CP_FACTURA_DOCUMENTO_65 = 11020
            <EnumMember> <Description("Mercancía")> CA_MERCANCIA_65 = 11021
            <EnumMember> <Description("Factura o documento comercial")> CA_FAC_DOC_COMERCIAL_65 = 11022
            <EnumMember> <Description("Importe y moneda")> CA_IMPORTE_MONEDA_65 = 11023
            <EnumMember> <Description("Concepto del cargo")> CA_CONCEPTO_CARGO_65 = 11024
        End Enum
        Public Enum CamposControlConsolidados
            'Región del 11100 - 11200
            <EnumMember> <Description("Sin definir")> SIN_DEFINIR = 11100

            <EnumMember> <Description("Periodicidad")> CP_PERIODICIDAD = 11101
            <EnumMember> <Description("Fecha de apertura")> CP_FECHA_APERTURA = 11102
            <EnumMember> <Description("Fecha de cierre estimado")> CP_FECHA_CIERRE_ESTIMADO = 11103
            <EnumMember> <Description("Estatus control consolidados")> CP_ESTATUS = 11104
            <EnumMember> <Description("Número de remesa")> CP_NUMERO_REMESA = 11105
            <EnumMember> <Description("Acuse de valor")> CP_ACUSE_VALOR = 11106
            <EnumMember> <Description("Valor de la mercancia")> CP_VALOR_MERCANCIA = 11107
            <EnumMember> <Description("Fecha del despacho")> CP_FECHA_DESPACHO = 11108
            <EnumMember> <Description("Color al desaduanar")> CP_COLOR_DESADUANAMIENTO = 11109
            <EnumMember> <Description("Número económico del vehículo")> CP_NUMERO_ECONOMICO_VEHICULO = 11110
            <EnumMember> <Description("Peso bruto")> CP_PESO_BRUTO = 11111
            <EnumMember> <Description("Número de bultos")> CP_NUMERO_BULTOS = 11112
            <EnumMember> <Description("Marca")> CP_MARCA = 11113
            <EnumMember> <Description("Observaciones remesa")> CP_OBSERVACIONES = 11114
            <EnumMember> <Description("Número del contenedor o identificador")> CP_CONTENEDOR = 11115
            <EnumMember> <Description("Tipo de contenedor")> CP_TIPO_CONTENEDOR = 11116
            <EnumMember> <Description("Identificador del candado")> CP_CANDADO = 11117
            <EnumMember> <Description("Color del candado")> CP_COLOR_CANDADO = 11118
            <EnumMember> <Description("Fecha de creacion remesa")> CP_CREACION = 11119
            <EnumMember> <Description("Tipo de cambio")> CP_TIPO_CAMBIO = 11120
            <EnumMember> <Description("Placas")> CP_PLACAS = 11121
        End Enum

        Public Enum CamposProcesamientoElectDocumentos
            'Región del 12000 - 12999
            <EnumMember> <Description("Sin definir")> SIN_DEFINIR = 12000

            <EnumMember> <Description("Pre-referencia procesamiento electrónico docto")> CP_PREREFERENCIA_DOCUMENTO_PROCESADO = 12001
            <EnumMember> <Description("Razón social cliente")> CP_RAZON_SOCIAL_CLIENTE = 12002
            <EnumMember> <Description("Clave cliente")> CP_CLAVE_CLIENTE = 12003
            <EnumMember> <Description("ObjectId cliente")> CP_OBJECTID_CLIENTE = 12004
            <EnumMember> <Description("Tipo operación")> CP_TIPO_OPERACION = 12005
            <EnumMember> <Description("Estado procesamiento electrónico docto")> CP_ESTADO_DOCUMENTO_PROCESADO = 12006
            <EnumMember> <Description("Clave o secuencia procesamiento electrónico docto")> CP_CLAVE_DOCUMENTO_PROCESADO = 12007
            <EnumMember> <Description("Tipo de documentos procesar")> CP_TIPO_DOCUMENTO_PROCESADO = 12008
            <EnumMember> <Description("Documentos procesados")> CP_DOCUMENTO_PROCESADO = 12009
            <EnumMember> <Description("Detalle documento")> CP_DETALLE_DOCUMENTO_PROCESADO = 12010
            <EnumMember> <Description("Tipo de detalle documento")> CP_TIPO_DETALLE_DOCUMENTO_PROCESADO = 12011
            <EnumMember> <Description("Estado de detalle documento procesado")> CP_ESTADO_DETALLE_DOCUMENTO_PROCESADO = 12012
            <EnumMember> <Description("Documento electronico o coleccion generada")> CP_DOCUMENTO_ELECTRONICO_GENERADO = 12013
            <EnumMember> <Description("Tipo de procesamiento")> CP_TIPO_PROCESAMIENTO = 12014
            <EnumMember> <Description("ObjectId documento procesado")> CP_OBJECTID_DOCUMENTO_PROCESADO = 12015
            <EnumMember> <Description("Folio de procesamiento")> CP_FOLIO_PROCESAMIENTO = 12016
            <EnumMember> <Description("Total documentos procesados")> CP_TOTAL_DOCUMENTOS_PROCESADOS = 12017
            <EnumMember> <Description("Total de documentos sin procesar")> CP_TOTAL_DOCUMENTOS_SIN_PROCESAR = 12018
            <EnumMember> <Description("Fecha procesamiento")> CP_FECHA_PROCESAMIENTO = 12019
            <EnumMember> <Description("Business unit cliente")> CP_BUSINESS_UNIT = 12020
            <EnumMember> <Description("Clave business unit cliente")> CP_CP_BUSINESS_UNITID = 12021
            <EnumMember> <Description("Usuario proceso")> CP_USUARIO_PROCESO = 12022
            <EnumMember> <Description("Email usuario proceso")> CP_EMAIL_USUARIO_PROCESO = 12023
            <EnumMember> <Description("ObjectId usuario proceso")> CP_OBJECTID_USUARIO_PROCESO = 12024
            <EnumMember> <Description("Environment")> CP_ENVIRONMENT = 12025
            <EnumMember> <Description("Id environment")> CP_ID_ENVIRONMENT = 12026
            <EnumMember> <Description("Package o tipo de uso documento proceso")> CP_TIPO_USO_DOCUMENTO_PROCESADO = 12027
            <EnumMember> <Description("Taxid cliente")> CP_TAXID_CLIENTE = 12028
        End Enum

        Public Enum CamposSubdivisionFacturaComercial

            'Región del 13000 - 13999
            <EnumMember> <Description("Sin definir")> SIN_DEFINIR = 13000
            <EnumMember> <Description("ObjectId Subdivisión")> CP_OBJECTID_SUBDIVISION = 13001
            <EnumMember> <Description("Porcentaje utilizado factura")> CP_PORCENTAJE_UTILIZADO_FACTURA = 13002
            <EnumMember> <Description("Monto utilizado de la factura")> CP_MONTO_UTILIZADO_FACTURA = 13003
            <EnumMember> <Description("Ultima subdivisión")> CP_ULTIMA_SUBDIVISION = 13004
            <EnumMember> <Description("Más información")> CP_MAS_INFORMACION = 13005
            <EnumMember> <Description("Cierre de subdivisión")> CP_CIERRE_SUBDIVISION = 13006
            <EnumMember> <Description("Tipo de cierre de subdivisión")> CP_TIPO_CIERRE_SUBDIVISION = 13007
            <EnumMember> <Description("Fecha de cierre de subdivisión")> CP_FECHA_CIERRE_SUBDIVISION = 13008
            <EnumMember> <Description("Motivo de cierre de subdivisión")> CP_MOTIVO_CIERRE_SUBDIVISION = 1309
            <EnumMember> <Description("Datos de factura comercial original")> CP_DATOS_FACTURA_COMERCIAL_ORIGINAL = 13010
            <EnumMember> <Description("Items factura comercial original")> CP_ITEMS_FACTURA_COMERCIAL_ORIGINAL = 13011
            <EnumMember> <Description("Secuencia de item de factura comercial original")> CP_SEC_ITEM_FACTURA_COMERCIAL_ORIGINAL = 13012
            <EnumMember> <Description("Tipo de subdivision")> CP_TIPO_SUBDIVISION = 13013
            <EnumMember> <Description("Numero parcialidad subdivision")> CP_NUMERO_PARCIALIDAD = 13014
            <EnumMember> <Description("Saldo pendiente")> CP_SALDO_PENDIENTE_SUBDIVISION = 13015
            <EnumMember> <Description("Detalles de la subdivisión")> CP_DETALLES_SUBDIVISION = 13016
            <EnumMember> <Description("Secuencia detalles de la subdivisión")> CP_SECUENCIA_DETALLES_SUBDIVISION = 13017
            <EnumMember> <Description("Clave de secuencia subdivision")> CP_CLAVE_SECUENCIA_DETALLES_SUBDIVISION = 13018
            <EnumMember> <Description("Descripcion de secuencia subdivision")> CP_DESCRIPCION_SECUENCIA_SUBDIVISION = 13019
            <EnumMember> <Description("Numero pedimento subdivision")> CP_NUMERO_PEDIMENTO_SUBDIVISION = 13020
            <EnumMember> <Description("Fecha asociación de pedimento")> CP_FECHA_ASOCIACION_PEDIMENTO = 13021
            <EnumMember> <Description("Estado de subdivision")> CP_ESTADO_ASOCIACION_PEDIMENTO = 13022
            <EnumMember> <Description("Items asociados a subdivision")> CP_ITEMS_ASOCIADOS_SUBDIVISION = 13023
            <EnumMember> <Description("ObjectId item asociado a subdivision")> CP_OBJECTID_ITEM_ASOCIADO_SUBDIVISION = 13024
            <EnumMember> <Description("Tipo de subdivision en item")> CP_TIPO_SUBDIVISION_ITEM = 13025
            <EnumMember> <Description("Numero parcialidad de subdivision en item")> CP_NUMERO_PARCIALIDAD_ITEM = 13026
            <EnumMember> <Description("Saldo pendiente de subdivision en item")> CP_SALDO_PENDIENTE_ITEM = 13027
            <EnumMember> <Description("Estado de la factura respecto a la subdivisión")> CP_ESTADO_FACTURA_CON_SUBDIVISION = 13028

        End Enum


        Public Enum CamposCopiasSimples

            'Región del 14000 - 14999

            <EnumMember> <Description("Sin definir")> SIN_DEFINIR = 14000
            <EnumMember> <Description("Fecha autorización")> CP_FECHA_AUTORIZACION = 14001
            <EnumMember> <Description("Oficio autorización")> CP_OFICIO_AUTORIZACION = 14002
            <EnumMember> <Description("Vigencia")> CP_VIGENCIA = 14003
            <EnumMember> <Description("Peso total")> CP_PESO_TOTAL = 14004
            <EnumMember> <Description("Número copia simple")> CP_NUMERO_COPIA_SIMPLE = 14005
            <EnumMember> <Description("Placa")> CP_PLACA = 14006
            <EnumMember> <Description("Número económico")> CP_NUMERO_ECONOMICO = 14007
            <EnumMember> <Description("Peso")> CP_PESO = 14008
            <EnumMember> <Description("Fecha Despacho")> CP_FECHA_DESPACHO = 14009
            <EnumMember> <Description("Impreso")> CP_IMPRESO = 14010

        End Enum

        Public Enum CamposProgramacionPrevios
            ' Región del 15000 - 15999
            <EnumMember> <Description("Sin definir")> SIN_DEFINIR = 15000
            <EnumMember> <Description("Estatus")> CP_ESTATUS_PREVIO = 15001
            <EnumMember> <Description("Identificador")> CP_IDENTIFICADOR = 15002
            <EnumMember> <Description("Programación")> CP_FECHA_PROGRAMACION = 15003
            <EnumMember> <Description("Tipo")> CP_TIPO_PREVIO = 15004
            <EnumMember> <Description("Contenedores")> CP_CONTENEDORES = 15005
            <EnumMember> <Description("Asiganado a")> CP_ASIGNADO_A = 15006
            <EnumMember> <Description("Inicio")> CP_FECHA_INICIO = 15007
            <EnumMember> <Description("Finalizado")> CP_FECHA_FINALIZO = 15008
            <EnumMember> <Description("Modalidad")> CP_MODALIDAD_ADUANA = 15009
            <EnumMember> <Description("Observaciones generales")> CP_OBSERVACIONES_GENERALES = 15010
            <EnumMember> <Description("Observaciones discrepancias")> CP_OBSERVACIONES_DISCREPANCIAS = 15011
            <EnumMember> <Description("Discrepancias")> CP_DISCREPANCIAS = 15012

            <EnumMember> <Description("Fotografías")> CP_FOTOGRAFIAS = 15013
            <EnumMember> <Description("Nombre de la fotografía")> CP_NOMBRE_FOTOGRAFIA = 15014
            <EnumMember> <Description("Fecha de carga")> CP_FECHA_CARGA_FOTOGRAFIA = 15015
            <EnumMember> <Description("Tipo de fotografía")> CP_TIPO_FOTOGRAFIA = 15016

            <EnumMember> <Description("Característica")> CP_CARACTERISTICA_PREVIO = 15017
            <EnumMember> <Description("Valor en la mercancía")> CP_VALOR_MERCANCIA = 15018
            <EnumMember> <Description("Valor en el documento")> CP_VALOR_DOCUMENTO = 15019

            <EnumMember> <Description("Ruta del archivo")> CP_RUTA_FOTOGRAFIA = 15020
            <EnumMember> <Description("Observación de fotografía")> CP_OBSERVACION_FOTOGRAFIA = 15021

            <EnumMember> <Description("Recinto")> CP_RECINTO_FISCALIZADO = 15022
            <EnumMember> <Description("Sección")> CP_SECCION_RECINTO_FISCALIZADO = 15023
        End Enum

        Public Enum CamposControlViajes
            'Región del 16000 - 16999
            <EnumMember> <Description("Sin definir")> SIN_DEFINIR = 16000 '
            <EnumMember> <Description("Id del viaje")> CP_ID_VIAJE = 16001
            <EnumMember> <Description("Nombre del buque y la naviera")> CP_BUQUE_NAVIERA = 16002
            <EnumMember> <Description("Bandera de origen del viaje")> CP_BANDERA_ORIGEN = 16003
            <EnumMember> <Description("Bandera destino del viaje")> CP_BANDERA_DESTINO = 16004
            <EnumMember> <Description("Pais origen")> CP_PAIS_ORIGEN_VIAJE = 16005
            <EnumMember> <Description("Folio de Capitania")> CP_FOLIO_CAPITANIA = 16006
            <EnumMember> <Description("Número de Viaje")> CP_NUMERO_VIAJE = 16007
            <EnumMember> <Description("Fecha ETA")> CP_FECHA_ETA = 16008
            <EnumMember> <Description("Fecha ETD")> CP_FECHA_ETD = 16009
            <EnumMember> <Description("Fecha de Fondeo")> CP_FECHA_FONDEO = 16010
            <EnumMember> <Description("Fecha de Atraque")> CP_FECHA_ATRAQUE = 16011
            <EnumMember> <Description("Estatus del viaje")> CP_STATUS = 16012
            <EnumMember> <Description("Control del monitoreo del viaje")> CP_MONITOREAR = 16013
            <EnumMember> <Description("Observaciones viaje")> CP_OBSERVACIONES = 16014
            <EnumMember> <Description("Puerto salida")> CP_PUERTO_SALIDA = 16015
            <EnumMember> <Description("Puerto de llegada")> CP_PUERTO_LLEGADA = 16016
            <EnumMember> <Description("Id de la guía")> CP_ID_GUIA = 16017
            <EnumMember> <Description("Estado de la guía en el viaje")> CP_ESTADO_GUIA_VIAJE = 16018
        End Enum


        Public Enum CamposGuias
            'Región del 17000 - 17999
            <EnumMember> <Description("Sin definir")> SIN_DEFINIR = 17000

            <EnumMember> <Description("Numero de documento")> CP_NUMERO_DOCUMENTO_TRANSPORTE = 17001
            <EnumMember> <Description("Modalidad de transporte")> CP_MODALIDAD_TRANSPORTE = 17002
            <EnumMember> <Description("Subdivisiones")> CP_SUBDIVISIONES = 17003
            <EnumMember> <Description("Documento")> CP_ARCHIVO_DOCUMENTO_TRANSPORTE = 17004
            <EnumMember> <Description("Peso Bruto Total")> CA_PESO_BRUTO_DOC = 17005
            <EnumMember> <Description("Tipo de documento M/H")> CP_MASTER_HOUSE = 17006
            <EnumMember> <Description("Tipo de carga")> CP_CONTENERIZADA = 17007

            <EnumMember> <Description("ObjectId transportista")> CP_OBJECTID_TRANSPORTISTA = 17008
            <EnumMember> <Description("Transportista")> CP_TRANSPORTISTA = 17009
            <EnumMember> <Description("ObjectId transporte")> CP_OBJECTID_TRANSPORTE = 17010
            <EnumMember> <Description("Nombre del transporte")> CP_TRANSPORTE = 17011
            <EnumMember> <Description("ObjectId viaje")> CP_OBJECTID_NUMERO_VIAJE = 17012
            <EnumMember> <Description("Numero de viaje")> CP_NUMERO_VIAJE = 17013

            <EnumMember> <Description("ObjectId Consignatario")> CP_OBJECTID_CONSIGNATARIO = 17014
            <EnumMember> <Description("Consignatario")> CP_CONSIGNATARIO = 17015

            <EnumMember> <Description("Puerto de carga")> CP_PUERTO_CARGA = 17016
            <EnumMember> <Description("Puerto de descarga")> CP_PUERTO_DESCARGA = 17017

            <EnumMember> <Description("Revalidado")> CP_REVALIDADO = 17018
            <EnumMember> <Description("Documento revalidado")> CP_ARCHIVO_DOCUMENTO_REVALIDADO = 17019
            <EnumMember> <Description("Fecha de revalidación")> CP_FECHA_REVALIDADO = 17020

            <EnumMember> <Description("Numero de contenedor")> CP_NUMERO_CONTENEDOR = 17021
            <EnumMember> <Description("Tipo de contenedor")> CP_TIPO_CONTENEDOR = 17022
            <EnumMember> <Description("Peso bruto contenedor")> CP_PESO_BRUTO_CONTENEDOR = 17023
            <EnumMember> <Description("Detalle")> CP_INFO_CONTENEDOR = 17024

            <EnumMember> <Description("Numero de tarja")> CP_NUMERO_TARJA = 17025
            <EnumMember> <Description("Numero de piezas ")> CP_NUMERO_PIEZAS_CARGASUELTA = 17026
            <EnumMember> <Description("Tipo de embalaje")> CP_EMBALAJE_CARGASUELTA = 17027
            <EnumMember> <Description("Peso bruto carga suelta")> CP_PESO_BRUTO_CARGASUELTA = 17028
            <EnumMember> <Description("Detalle")> CP_INFO_CARGASUELTA = 17029

            <EnumMember> <Description("Guia Relacionada")> CP_GUIA_RELACIONADA = 17030
            <EnumMember> <Description("Modalidad de transporte")> CP_TIPO_RELACIONADA_MH = 17031

            <EnumMember> <Description("Clave ISO# Pais del puerto de descarga")> CP_ISO3_PAIS_PUERTO_CARGA = 17032

        End Enum

        Public Enum CamposPartesII

            'Región del 18000 - 18999
            <EnumMember> <Description("Sin definir")> SIN_DEFINIR = 18000
            <EnumMember> <Description("Datos del vehiculo")> CP_DATOS_VEHICULO = 18001
            <EnumMember> <Description("Número Partes II")> CP_NUMERO_PARTESII = 18002

        End Enum

        Public Enum CamposExpedienteElectronico

            'Región del 19000 - 19999
            <EnumMember> <Description("Sin definir")> SIN_DEFINIR = 19000
            <EnumMember> <Description("Customer id")> CP_ID_CLIENTE = 19001
            <EnumMember> <Description("Customer name")> CP_RAZON_SOCIAL_CLIENTE = 19002
            <EnumMember> <Description("Customer taxid")> CP_TAXID_CLIENTE = 19003
            <EnumMember> <Description("Environment id")> CP_ID_ENVIRONMENT = 19004
            <EnumMember> <Description("Environment")> CP_ENVIRONMENT = 19005
            <EnumMember> <Description("Bussiness unit id")> CP_BUSSINESS_UNID_ID = 19006
            <EnumMember> <Description("Bussiness unit")> CP_BUSSINESS_UNIT = 19007
            <EnumMember> <Description("Messages")> CP_MESSAGE = 19008
            <EnumMember> <Description("Messages Type")> CP_TIPO_MESSAGE = 19009
            <EnumMember> <Description("Messages Status")> CP_STATUS_MESSAGE = 19010
            <EnumMember> <Description("Messages Sec")> CP_SEC_MESSAGE = 19011
            <EnumMember> <Description("Messages Level")> CP_NIVEL_MESSAGE = 19012 '1 EXPEDIENTE, 2 DOCUMENTO
            <EnumMember> <Description("Reference id")> CP_REFERENCIA_ID_DOCUMENTO = 19013
            <EnumMember> <Description("Reference")> CP_REFERENCIA_DOCUMENTO = 19014
            <EnumMember> <Description("Total files closed")> CP_TOTAL_DOCUMENTOS_CERRADOS = 19015
            <EnumMember> <Description("Total files opened")> CP_TOTAL_DOCUMENTOS_ABIERTOS = 19016
            <EnumMember> <Description("Total files without reference")> CP_TOTAL_DOCUMENTOS_SIN_REFERENCIA = 19017
            <EnumMember> <Description("Status file")> CP_ESTATUS_DOCUMENTO = 19018
            <EnumMember> <Description("Reference id")> CP_REFERENCIA_ID_EXPEDIENTE = 19019
            <EnumMember> <Description("Reference")> CP_REFERENCIA_EXPEDIENTE = 19020
            <EnumMember> <Description("Total reference closed")> CP_TOTAL_REFERENCIAS_CERRADAS = 19021
            <EnumMember> <Description("Total reference opened")> CP_TOTAL_REFERENCIAS_ABIERTAS = 19022
            <EnumMember> <Description("Fecha apertura")> CP_FECHA_APERTURA_EXPEDIENTE = 19023
            <EnumMember> <Description("Last updated ")> CP_ULTIMA_ACTUALIZACION_EXPEDIENTE = 19024
            <EnumMember> <Description("Digitalkey id")> CP_DIGITAL_KEY_ID_EXPEDIENTE = 19025
            <EnumMember> <Description("Digitalkey")> CP_DIGITAL_KEY_EXPEDIENTE = 19026
            <EnumMember> <Description("Owner id")> CP_OWNER_ID_EXPEDIENTE = 19027
            <EnumMember> <Description("Owner user email")> CP_OWNER_USER_EMAIL_EXPEDIENTE = 19028
            <EnumMember> <Description("Owner name")> CP_OWNER_NAME_EXPEDIENTE = 19029
            <EnumMember> <Description("Status referencia")> CP_ESTATUS_REFERENCIA = 19030
        End Enum


#Region "Builders"
        Sub New()

        End Sub

#End Region

#Region "Methods"


#End Region

    End Class

End Namespace