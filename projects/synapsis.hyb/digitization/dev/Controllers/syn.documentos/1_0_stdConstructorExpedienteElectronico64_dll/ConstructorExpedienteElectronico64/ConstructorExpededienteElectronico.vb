Imports gsol.krom
Imports MongoDB.Bson
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Syn.Documento.Componentes.Campo.TiposDato
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior

Namespace Syn.Documento

    <Serializable()>
    Public Class ConstructorExpededienteElectronico
        Inherits EntidadDatosDocumento
        Implements ICloneable

#Region "Attributes"

#End Region

#Region "Builders"
        Sub New()
            Inicializa(Nothing,
                        TiposDocumentoElectronico.ExpedienteElectronicoDocumentos,
                        True)
        End Sub

        Sub New(ByVal construir_ As Boolean,
                Optional ByVal documentoElectronico_ As DocumentoElectronico = Nothing)
            Inicializa(documentoElectronico_,
                       TiposDocumentoElectronico.ExpedienteElectronicoDocumentos,
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
                         TiposDocumentoElectronico.ExpedienteElectronicoDocumentos)
        End Sub

#End Region

#Region "Methods"
        Public Overrides Sub ConstruyeEncabezado()

            ' Encabezado principal de la factura comercial
            _estructuraDocumento(TiposBloque.Encabezado) = New List(Of Nodo)

            ' Construye las secciones 

            ConstruyeSeccion(seccionEnum_:=SeccionesExpedienteElectronico.SEXPE1,
                             tipoBloque_:=TiposBloque.Encabezado,
                             conCampos_:=True)

            'ConstruyeSeccion(seccionEnum_:=SeccionesExpedienteElectronico.SEXPE2,
            '               tipoBloque_:=TiposBloque.Encabezado,
            '               conCampos_:=False)
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

            ConstruyeSeccion(seccionEnum_:=SeccionesExpedienteElectronico.SEXPE2,
                            tipoBloque_:=TiposBloque.Cuerpo,
                            conCampos_:=False)

            ConstruyeSeccion(seccionEnum_:=SeccionesExpedienteElectronico.SEXPE3,
                            tipoBloque_:=TiposBloque.Cuerpo,
                            conCampos_:=False)


            'ConstruyeSeccion(seccionEnum_:=SeccionesFacturaComercial.SFAC6,
            '                tipoBloque_:=TiposBloque.Cuerpo,
            '                conCampos_:=True)

            'ConstruyeSeccion(seccionEnum_:=SeccionesFacturaComercial.SFAC7,
            '                tipoBloque_:=TiposBloque.Cuerpo,
            '                conCampos_:=True)

            'ConstruyeSeccion(seccionEnum_:=SeccionesFacturaComercial.SFAC8,
            '                tipoBloque_:=TiposBloque.Cuerpo,
            '                conCampos_:=True)

            'ConstruyeSeccion(seccionEnum_:=SeccionesFacturaComercial.SFAC9,
            '                tipoBloque_:=TiposBloque.Cuerpo,
            '                conCampos_:=True)

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
                Case SeccionesExpedienteElectronico.SEXPE1
                    Return New List(Of Nodo) From {
                                            Item(CamposExpedienteElectronico.CP_ID_CLIENTE, IdObject),
                                            Item(CamposExpedienteElectronico.CP_RAZON_SOCIAL_CLIENTE, Texto),
                                            Item(CamposExpedienteElectronico.CP_TAXID_CLIENTE, Texto),
                                            Item(CamposExpedienteElectronico.CP_ID_ENVIRONMENT, Entero),
                                            Item(CamposExpedienteElectronico.CP_ENVIRONMENT, Texto),
                                            Item(CamposExpedienteElectronico.CP_BUSSINESS_UNID_ID, Entero),
                                            Item(CamposExpedienteElectronico.CP_BUSSINESS_UNIT, Texto),
                                            Item(CamposExpedienteElectronico.CP_TOTAL_REFERENCIAS_CERRADAS, Entero),
                                            Item(CamposExpedienteElectronico.CP_TOTAL_REFERENCIAS_ABIERTAS, Entero),
                                            Item(CamposExpedienteElectronico.CP_TOTAL_DOCUMENTOS_SIN_REFERENCIA, Entero),
                                            Item(CamposExpedienteElectronico.CP_DIGITAL_KEY_ID_EXPEDIENTE, IdObject),
                                            Item(CamposExpedienteElectronico.CP_DIGITAL_KEY_EXPEDIENTE, Texto),
                                            Item(CamposExpedienteElectronico.CP_OWNER_ID_EXPEDIENTE, IdObject),
                                            Item(CamposExpedienteElectronico.CP_OWNER_USER_EMAIL_EXPEDIENTE, Texto),
                                            Item(CamposExpedienteElectronico.CP_OWNER_NAME_EXPEDIENTE, Texto),
                                            Item(CamposExpedienteElectronico.CP_FECHA_APERTURA_EXPEDIENTE, Fecha),
                                            Item(CamposExpedienteElectronico.CP_ULTIMA_ACTUALIZACION_EXPEDIENTE, Fecha)
                                          }
               ' Mensajes
                Case SeccionesExpedienteElectronico.SEXPE2
                    Return New List(Of Nodo) From {
                                                Item(CamposExpedienteElectronico.CP_SEC_MESSAGE, Entero),
                                                Item(CamposExpedienteElectronico.CP_TIPO_MESSAGE, Entero),
                                                Item(CamposExpedienteElectronico.CP_MESSAGE, Texto),
                                                Item(CamposExpedienteElectronico.CP_NIVEL_MESSAGE, Texto),
                                                Item(CamposExpedienteElectronico.CP_STATUS_MESSAGE, Entero)
                    }

                'Archivos sueltos
                Case SeccionesExpedienteElectronico.SEXPE3
                    Return New List(Of Nodo) From {
                          Item(CamposExpedienteElectronico.CP_REFERENCIA_ID_DOCUMENTO, IdObject),
                          Item(CamposExpedienteElectronico.CP_REFERENCIA_DOCUMENTO, Texto),
                          Item(CamposExpedienteElectronico.CP_TOTAL_DOCUMENTOS_CERRADOS, Entero),
                          Item(CamposExpedienteElectronico.CP_TOTAL_DOCUMENTOS_ABIERTOS, Entero),
                          Item(CamposExpedienteElectronico.CP_ESTATUS_REFERENCIA, Booleano)
                        }

                Case Else

                    _tagWatcher.SetError(Me, "La sección con clave:" & idSeccion_ & " no esta configurada.")

            End Select

            Return Nothing

        End Function

#End Region

    End Class

End Namespace
