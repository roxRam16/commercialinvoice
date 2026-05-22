
Imports gsol.krom
Imports MongoDB.Bson
Imports Syn.Documento.Componentes
Imports Syn.Documento.Componentes.Campo.TiposDato
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior

Namespace Syn.Documento
    <Serializable()>
    Public Class ConstructorSubdivisionFacturaComercial
        Inherits EntidadDatosDocumento
        Implements ICloneable

#Region "Builders"
        Sub New()
            Inicializa(Nothing,
                        TiposDocumentoElectronico.SubdivisionFacturaComercial,
                        True)
        End Sub

        Sub New(ByVal construir_ As Boolean,
                Optional ByVal documentoElectronico_ As DocumentoElectronico = Nothing)
            Inicializa(documentoElectronico_,
                       TiposDocumentoElectronico.SubdivisionFacturaComercial,
                       construir_)
        End Sub
        Public Sub New(ByVal folioDocumento_ As String,
                       ByVal referencia_ As String,
                       ByVal tipoPropietario_ As String,
                       ByVal nombrePropietario_ As String,
                       ByVal idPropietario_ As Int32,
                       ByVal objectIdPropietario_ As ObjectId,
                       ByVal metadatos_ As List(Of CampoGenerico)
                      )
            Inicializa(folioDocumento_,
                         referencia_,
                         tipoPropietario_,
                         nombrePropietario_,
                         idPropietario_,
                         objectIdPropietario_,
                         metadatos_,
                         TiposDocumentoElectronico.SubdivisionFacturaComercial)
        End Sub
#End Region

#Region "Methods"
        Public Overrides Sub ConstruyeEncabezado()
            ' Encabezado principal de la factura comercial
            _estructuraDocumento(TiposBloque.Encabezado) = New List(Of Nodo)
            ' Construye las secciones 
            ConstruyeSeccion(seccionEnum_:=SeccionesSubdivisionFacturaComercial.SSFC1,
                             tipoBloque_:=TiposBloque.Encabezado,
                             conCampos_:=True)
            ConstruyeSeccion(seccionEnum_:=SeccionesSubdivisionFacturaComercial.SSFC2,
                           tipoBloque_:=TiposBloque.Encabezado,
                           conCampos_:=True)
            'ConstruyeSeccion(seccionEnum_:=SeccionesSubdivisionFacturaComercial.SSFC3,
            '            tipoBloque_:=TiposBloque.Encabezado,
            '            conCampos_:=True)

        End Sub
        Public Overrides Sub ConstruyeEncabezadoPaginasSecundarias()
            'Construir la parte encabezado para páginas secundarias
            '_estructuraDocumento(TiposBloque.EncabezadoPaginasSecundarias) = New List(Of Nodo)

            'Construir una sección
            'ConstruyeSeccion(seccionDocumento_:=SeccionesGenericas.SGS1,
            '                 tipoBloque_:=TiposBloque.EncabezadoPaginasSecundarias,
            '                 conCampos_:=True)
        End Sub
        Public Overrides Sub ConstruyeCuerpo()
            'Construir la parte de cuerpo de la factura comercial
            _estructuraDocumento(TiposBloque.Cuerpo) = New List(Of Nodo)
            ' Construye las secciones 
            'FALSE porque esta seccion va dentro del componente pillbox
            ConstruyeSeccion(seccionEnum_:=SeccionesSubdivisionFacturaComercial.SSFC4,
                            tipoBloque_:=TiposBloque.Cuerpo,
                            conCampos_:=False)

            ConstruyeSeccion(seccionEnum_:=SeccionesSubdivisionFacturaComercial.SSFC6,
                           tipoBloque_:=TiposBloque.Cuerpo,
                           conCampos_:=True)

            ConstruyeSeccion(seccionEnum_:=SeccionesSubdivisionFacturaComercial.SSFC7,
                          tipoBloque_:=TiposBloque.Cuerpo,
                          conCampos_:=False)

            'ConstruyeSeccion(seccionEnum_:=SeccionesSubdivisionFacturaComercial.SSFC5,
            '                tipoBloque_:=TiposBloque.Cuerpo,
            '                conCampos_:=False)

            'ConstruyeSeccion(seccionEnum_:=SeccionesSubdivisionFacturaComercial.SSFC7,
            '                  tipoBloque_:=TiposBloque.Cuerpo,
            '                  conCampos_:=False)
        End Sub
        Public Overrides Sub ConstruyePiePagina()
            'Construir la parte pie de página
            '_estructuraDocumento(TiposBloque.PiePagina) = New List(Of Nodo)

            'Construir una sección
            'ConstruyeSeccion(seccionDocumento_:=SeccionesGenericas.SGS1,
            '                 tipoBloque_:=TiposBloque.PiePagina,
            '                 conCampos_:=True)
        End Sub
#End Region

#Region "Functions"
        Public Overrides Function ObtenerCamposSeccion(ByVal idSeccion_ As Integer) As List(Of Nodo)
            'Listado de relación sección - campo
            Select Case idSeccion_
                ' Generales
                Case SeccionesSubdivisionFacturaComercial.SSFC1
                    Return New List(Of Nodo) From {
                                            Item(CamposSubdivisionFacturaComercial.CP_OBJECTID_SUBDIVISION, Texto),
                                            Item(CamposFacturaComercial.CP_OBJECTID_FACTURA, Texto, useAsMetadata_:=True),
                                            Item(CamposFacturaComercial.CA_NUMERO_FACTURA, Texto, useAsMetadata_:=True),
                                            Item(CamposClientes.CA_RAZON_SOCIAL, Texto, useAsMetadata_:=True),
                                            Item(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, Texto, useAsMetadata_:=True),
                                            Item(CamposSubdivisionFacturaComercial.CP_PORCENTAJE_UTILIZADO_FACTURA, Texto, longitud_:=10),
                                            Item(CamposSubdivisionFacturaComercial.CP_MONTO_UTILIZADO_FACTURA, Texto, longitud_:=50),
                                            Item(CamposFacturaComercial.CP_VALOR_FACTURA, Real, cantidadEnteros_:=14, cantidadDecimales_:=2),
                                            Item(CamposSubdivisionFacturaComercial.CP_ULTIMA_SUBDIVISION, Texto, longitud_:=12),
                                            Item(CamposSubdivisionFacturaComercial.CP_ESTADO_FACTURA_CON_SUBDIVISION, Texto, longitud_:=50)
                                          }
                Case SeccionesSubdivisionFacturaComercial.SSFC2
                    Return New List(Of Nodo) From {
                                           Item(CamposFacturaComercial.CA_FECHA_FACTURA, Texto, longitud_:=12),
                                           Item(CamposAcuseValor.CA_NUMERO_ACUSEVALOR, Texto),
                                           Item(CamposFacturaComercial.CA_CVE_INCOTERM, Texto),
                                           Item(SeccionesSubdivisionFacturaComercial.SSFC3, True)
                                        }
                Case SeccionesSubdivisionFacturaComercial.SSFC3
                    Return New List(Of Nodo) From {
                                          Item(CamposSubdivisionFacturaComercial.CP_TIPO_CIERRE_SUBDIVISION, Texto),
                                          Item(CamposSubdivisionFacturaComercial.CP_FECHA_CIERRE_SUBDIVISION, Texto),
                                          Item(CamposSubdivisionFacturaComercial.CP_MOTIVO_CIERRE_SUBDIVISION, Texto)
                                       }
                Case SeccionesSubdivisionFacturaComercial.SSFC4
                    Return New List(Of Nodo) From {
                                            Item(CamposFacturaComercial.CP_TIPO_OPERACION, Entero),
                                            Item(CamposFacturaComercial.CP_TIPO_CARGA_DATOS, Entero),
                                            Item(CamposFacturaComercial.CA_NUMERO_FACTURA, Texto, longitud_:=40),
                                            Item(CamposAcuseValor.CA_NUMERO_ACUSEVALOR, Texto, longitud_:=40),
                                            Item(CamposFacturaComercial.CA_FECHA_FACTURA, Fecha),
                                            Item(CamposFacturaComercial.CP_SERIE_FOLIO_FACTURA, Texto),
                                            Item(CamposClientes.CA_RAZON_SOCIAL, Texto, longitud_:=120),
                                            Item(CamposFacturaComercial.CA_CVE_INCOTERM, Texto),
                                            Item(CamposFacturaComercial.CA_APLICA_SUBDIVISION, Booleano),
                                            Item(CamposFacturaComercial.CP_APLICA_ENAJENACION, Booleano, useAsMetadata_:=True),
                                            Item(CamposFacturaComercial.CP_APLICA_INCREMENTABLES, Booleano, useAsMetadata_:=True),
                                            Item(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, Texto),
                                            Item(SeccionesSubdivisionFacturaComercial.SSFC5, False),
                                            Item(SeccionesSubdivisionFacturaComercial.SSFC6, True)
                                          }
                Case SeccionesSubdivisionFacturaComercial.SSFC5
                    Return New List(Of Nodo) From {
                                          Item(CamposFacturaComercial.CP_OBJECTID_PRODUCTOS, Texto),
                                          Item(CamposFacturaComercial.CP_NUMERO_PARTIDA, Entero),
                                          Item(CamposFacturaComercial.CA_NUMERO_PARTE_PARTIDA, Texto, longitud_:=20),
                                          Item(CamposFacturaComercial.CA_CANTIDAD_COMERCIAL_PARTIDA, Real, cantidadEnteros_:=15, cantidadDecimales_:=3),
                                          Item(CamposFacturaComercial.CA_UNIDAD_MEDIDA_COMERCIAL_PARTIDA, Texto),
                                          Item(CamposFacturaComercial.CA_DESCRIPCION_PARTE_PARTIDA, Texto, longitud_:=250),
                                          Item(CamposFacturaComercial.CA_DESCRIPCION_PARTE_PARTIDA_ORIGINAL, Texto, longitud_:=250),
                                          Item(CamposFacturaComercial.CA_VALOR_FACTURA_PARTIDA, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                          Item(CamposFacturaComercial.CP_MONEDA_FACTURA_PARTIDA, Texto, longitud_:=3),
                                          Item(CamposFacturaComercial.CA_VALOR_DOLARES_PARTIDA, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                          Item(CamposFacturaComercial.CP_MONEDA_VALOR_DOLARES_PARTIDA, Texto, longitud_:=3),
                                          Item(CamposFacturaComercial.CA_VALOR_MERCANCIA_PARTIDA, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                          Item(CamposFacturaComercial.CA_MONEDA_MERCANCIA_PARTIDA, Texto, longitud_:=3),
                                          Item(CamposFacturaComercial.CA_VALOR_UNITARIO_PARTIDA, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                          Item(CamposFacturaComercial.CA_MONEDA_VALOR_UNITARIO_PARTIDA, Texto, longitud_:=3),
                                          Item(CamposFacturaComercial.CA_PRECIO_UNITARIO_PARTIDA, Real, cantidadEnteros_:=15, cantidadDecimales_:=5),
                                          Item(CamposFacturaComercial.CP_MONEDA_PRECIO_UNITARIO, Texto, longitud_:=3),
                                          Item(CamposFacturaComercial.CA_PESO_NETO_PARTIDA, Real, cantidadEnteros_:=14, cantidadDecimales_:=3),
                                          Item(CamposFacturaComercial.CA_DESCRIPCION_COVE_PARTIDA, Texto, longitud_:=250),
                                          Item(CamposFacturaComercial.CP_APLICA_DESCRIPCION_COVE_PARTIDA, Texto),
                                          Item(CamposFacturaComercial.CP_APLICA_DESCRIPCION_ORIGINAL_MERCANCIA_PEDIMENTO, Texto),
                                          Item(CamposFacturaComercial.CA_PAIS_DESTINO_PARTIDA, Texto),
                                          Item(CamposFacturaComercial.CA_PAIS_ORIGEN_PARTIDA, Texto, longitud_:=3),
                                          Item(CamposFacturaComercial.CA_CVE_METODO_VALORACION_PARTIDA, Texto, longitud_:=1),
                                          Item(CamposFacturaComercial.CP_ORDEN_COMPRA_PARTIDA, Texto, longitud_:=60),
                                          Item(CamposFacturaComercial.CP_CANTIDAD_FACTURA_PARTIDA, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                          Item(CamposFacturaComercial.CP_UNIDAD_MEDIDA_FACTURA_PARTIDA, Entero),
                                          Item(CamposFacturaComercial.CA_FRACCION_ARANCELARIA_PARTIDA, Texto, longitud_:=8),
                                          Item(CamposFacturaComercial.CA_CANTIDAD_TARIFA_PARTIDA, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                          Item(CamposFacturaComercial.CA_UNIDAD_MEDIDA_TARIFA_PARTIDA, Entero),
                                          Item(CamposFacturaComercial.CA_FRACCION_NICO_PARTIDA, Texto, longitud_:=2),
                                          Item(CamposFacturaComercial.CA_LOTE_PARTIDA, Texto, longitud_:=80),
                                          Item(CamposFacturaComercial.CA_NUMERO_SERIE_PARTIDA, Texto, longitud_:=25),
                                          Item(CamposFacturaComercial.CA_MARCA_PARTIDA, Texto, longitud_:=80),
                                          Item(CamposFacturaComercial.CA_MODELO_PARTIDA, Texto, longitud_:=80),
                                          Item(CamposFacturaComercial.CA_SUBMODELO_PARTIDA, Texto, longitud_:=80),
                                          Item(CamposFacturaComercial.CA_KILOMETRAJE_PARTIDA, Entero),
                                          Item(CamposSubdivisionFacturaComercial.CP_TIPO_SUBDIVISION, Texto),
                                          Item(CamposSubdivisionFacturaComercial.CP_NUMERO_PARCIALIDAD, Entero),
                                          Item(CamposSubdivisionFacturaComercial.CP_SALDO_PENDIENTE_ITEM, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                          Item(CamposGlobales.CP_IDENTITY, Entero)
                                        }
                Case SeccionesSubdivisionFacturaComercial.SSFC6
                    Return New List(Of Nodo) From {
                                            Item(CamposFacturaComercial.CA_FLETES, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                            Item(CamposFacturaComercial.CA_MONEDA_FLETES, Texto, longitud_:=3),
                                            Item(CamposFacturaComercial.CA_SEGURO, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                            Item(CamposFacturaComercial.CA_MONEDA_SEGUROS, Texto, longitud_:=3),
                                            Item(CamposFacturaComercial.CA_EMBALAJES, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                            Item(CamposFacturaComercial.CA_MONEDA_EMBALAJES, Texto, longitud_:=3),
                                            Item(CamposFacturaComercial.CA_OTROS_INCREMENTABLES, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                            Item(CamposFacturaComercial.CA_MONEDA_OTROS_INCREMENTABLES, Texto, longitud_:=3),
                                            Item(CamposFacturaComercial.CA_DESCUENTOS, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                            Item(CamposFacturaComercial.CA_MONEDA_DESCUENTOS, Texto, longitud_:=3)
                    }
                Case SeccionesSubdivisionFacturaComercial.SSFC7
                    Return New List(Of Nodo) From {
                                         Item(CamposSubdivisionFacturaComercial.CP_OBJECTID_SUBDIVISION, Texto),
                                         Item(CamposSubdivisionFacturaComercial.CP_SECUENCIA_DETALLES_SUBDIVISION, Entero),
                                         Item(CamposSubdivisionFacturaComercial.CP_CLAVE_SECUENCIA_DETALLES_SUBDIVISION, Texto, longitud_:=10),
                                         Item(CamposSubdivisionFacturaComercial.CP_DESCRIPCION_SECUENCIA_SUBDIVISION, Texto),
                                         Item(CamposPedimento.CA_NUMERO_PEDIMENTO_COMPLETO, Texto),
                                         Item(CamposSubdivisionFacturaComercial.CP_FECHA_ASOCIACION_PEDIMENTO, Texto),
                                         Item(CamposSubdivisionFacturaComercial.CP_ESTADO_ASOCIACION_PEDIMENTO, Texto),
                                         Item(SeccionesSubdivisionFacturaComercial.SSFC8, False),
                                         Item(CamposGlobales.CP_IDENTITY, Entero)
                                       }
                Case SeccionesSubdivisionFacturaComercial.SSFC8
                    Return New List(Of Nodo) From {
                                        Item(CamposSubdivisionFacturaComercial.CP_SEC_ITEM_FACTURA_COMERCIAL_ORIGINAL, Texto),
                                        Item(CamposFacturaComercial.CP_NUMERO_PARTIDA, Entero),
                                        Item(CamposFacturaComercial.CP_OBJECTID_PRODUCTOS, Texto),
                                        Item(CamposFacturaComercial.CP_NUMERO_PARTIDA, Entero),
                                        Item(CamposFacturaComercial.CA_NUMERO_PARTE_PARTIDA, Texto, longitud_:=20),
                                        Item(CamposFacturaComercial.CA_CANTIDAD_COMERCIAL_PARTIDA, Real, cantidadEnteros_:=15, cantidadDecimales_:=3),
                                        Item(CamposFacturaComercial.CA_UNIDAD_MEDIDA_COMERCIAL_PARTIDA, Texto),
                                        Item(CamposFacturaComercial.CA_DESCRIPCION_PARTE_PARTIDA, Texto, longitud_:=250),
                                        Item(CamposFacturaComercial.CA_DESCRIPCION_PARTE_PARTIDA_ORIGINAL, Texto, longitud_:=250),
                                        Item(CamposFacturaComercial.CA_VALOR_FACTURA_PARTIDA, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                        Item(CamposFacturaComercial.CP_MONEDA_FACTURA_PARTIDA, Texto, longitud_:=3),
                                        Item(CamposFacturaComercial.CA_VALOR_DOLARES_PARTIDA, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                        Item(CamposFacturaComercial.CP_MONEDA_VALOR_DOLARES_PARTIDA, Texto, longitud_:=3),
                                        Item(CamposFacturaComercial.CA_VALOR_MERCANCIA_PARTIDA, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                        Item(CamposFacturaComercial.CA_MONEDA_MERCANCIA_PARTIDA, Texto, longitud_:=3),
                                        Item(CamposFacturaComercial.CA_VALOR_UNITARIO_PARTIDA, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                        Item(CamposFacturaComercial.CA_MONEDA_VALOR_UNITARIO_PARTIDA, Texto, longitud_:=3),
                                        Item(CamposFacturaComercial.CA_PRECIO_UNITARIO_PARTIDA, Real, cantidadEnteros_:=15, cantidadDecimales_:=5),
                                        Item(CamposFacturaComercial.CP_MONEDA_PRECIO_UNITARIO, Texto, longitud_:=3),
                                        Item(CamposFacturaComercial.CA_PESO_NETO_PARTIDA, Real, cantidadEnteros_:=14, cantidadDecimales_:=3),
                                        Item(CamposFacturaComercial.CA_DESCRIPCION_COVE_PARTIDA, Texto, longitud_:=250),
                                        Item(CamposFacturaComercial.CP_APLICA_DESCRIPCION_ORIGINAL_MERCANCIA_PEDIMENTO, Texto),
                                        Item(CamposFacturaComercial.CP_APLICA_DESCRIPCION_COVE_PARTIDA, Texto),
                                        Item(CamposFacturaComercial.CA_PAIS_DESTINO_PARTIDA, Texto),
                                        Item(CamposFacturaComercial.CA_PAIS_ORIGEN_PARTIDA, Texto, longitud_:=3),
                                        Item(CamposFacturaComercial.CA_CVE_METODO_VALORACION_PARTIDA, Texto, longitud_:=1),
                                        Item(CamposFacturaComercial.CP_ORDEN_COMPRA_PARTIDA, Texto, longitud_:=60),
                                        Item(CamposFacturaComercial.CP_CANTIDAD_FACTURA_PARTIDA, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                        Item(CamposFacturaComercial.CP_UNIDAD_MEDIDA_FACTURA_PARTIDA, Entero),
                                        Item(CamposFacturaComercial.CA_FRACCION_ARANCELARIA_PARTIDA, Texto, longitud_:=8),
                                        Item(CamposFacturaComercial.CA_CANTIDAD_TARIFA_PARTIDA, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                        Item(CamposFacturaComercial.CA_UNIDAD_MEDIDA_TARIFA_PARTIDA, Entero),
                                        Item(CamposFacturaComercial.CA_FRACCION_NICO_PARTIDA, Texto, longitud_:=2),
                                        Item(CamposFacturaComercial.CA_LOTE_PARTIDA, Texto, longitud_:=80),
                                        Item(CamposFacturaComercial.CA_NUMERO_SERIE_PARTIDA, Texto, longitud_:=25),
                                        Item(CamposFacturaComercial.CA_MARCA_PARTIDA, Texto, longitud_:=80),
                                        Item(CamposFacturaComercial.CA_MODELO_PARTIDA, Texto, longitud_:=80),
                                        Item(CamposFacturaComercial.CA_SUBMODELO_PARTIDA, Texto, longitud_:=80),
                                        Item(CamposFacturaComercial.CA_KILOMETRAJE_PARTIDA, Entero),
                                        Item(CamposSubdivisionFacturaComercial.CP_TIPO_SUBDIVISION, Texto),
                                        Item(CamposSubdivisionFacturaComercial.CP_NUMERO_PARCIALIDAD, Entero),
                                        Item(CamposSubdivisionFacturaComercial.CP_SALDO_PENDIENTE_ITEM, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                        Item(CamposGlobales.CP_IDENTITY, Entero)
                                       }

                Case Else
                    _tagWatcher.SetError(Me, "La sección con clave:" & idSeccion_ & " no esta configurada.")
            End Select
            Return Nothing
        End Function
#End Region

    End Class

End Namespace

