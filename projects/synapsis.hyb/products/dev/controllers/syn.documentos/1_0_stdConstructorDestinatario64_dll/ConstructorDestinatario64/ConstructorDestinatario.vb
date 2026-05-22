Imports gsol.krom
Imports MongoDB.Bson
Imports Syn.Documento.Componentes
Imports Syn.Documento.Componentes.Campo.TiposDato
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Nucleo.RecursosComercioExterior.CamposDestinatario

Namespace Syn.Documento


    <Serializable()>
    Public Class ConstructorDestinatario
        Inherits EntidadDatosDocumento
        Implements ICloneable


#Region "Attributes"

#End Region

#Region "Builders"

        Sub New()
            ''SOLO PARA RESPALDAR
            Inicializa(Nothing,
                        TiposDocumentoElectronico.Destinatarios,
                        True)

        End Sub
        Sub New(ByVal construir_ As Boolean,
                Optional ByVal documentoElectronico_ As DocumentoElectronico = Nothing)
            ''SOLO PARA RESPALDAR
            Inicializa(documentoElectronico_,
                       TiposDocumentoElectronico.Destinatarios,
                       construir_)

        End Sub
        'Public Sub New(ByVal folioDocumento_ As String,
        '               ByVal referencia_ As String,
        '               ByVal idCliente_ As Int32,
        '               ByVal nombreCliente_ As String
        '               )

        '    Inicializa(folioDocumento_,
        '                 referencia_,
        '                 idCliente_,
        '                 nombreCliente_,
        '                 TiposDocumentoElectronico.ProveedoresOperativos)

        'End Sub

        'NEW
        Public Sub New(ByVal folioDocumento_ As String,
                       ByVal referencia_ As String,
                       ByVal tipoPropietario_ As String,
                       ByVal nombrePropietario_ As String,
                       ByVal idPropietario_ As Int32,
                       ByVal objectIdPropietario_ As ObjectId,
                       ByVal metadatos_ As List(Of CampoGenerico)
                      )
            ''SOLO PARA RESPALDAR
            Inicializa(folioDocumento_,
                         referencia_,
                         tipoPropietario_,
                         nombrePropietario_,
                         idPropietario_,
                         objectIdPropietario_,
                         metadatos_,
                         TiposDocumentoElectronico.Destinatarios)

        End Sub

#End Region

#Region "Methods"

        Public Sub ConfiguracionNotificaciones()

            'SubscriptionsGroup =
            '   New List(Of subscriptionsgroup) _
            '      From {
            '             New subscriptionsgroup With
            '             {
            '              .active = True,
            '              .toresource = "Vt002EjecutivosMiEmpresa",
            '              ._foreignkeyname = "_id",
            '              ._foreignkey = New ObjectId,
            '              .subscriptions =
            '                New subscriptions With
            '                  {
            '                    .namespaces = New List(Of [namespace]) From
            '                    {
            '                     nsp(1, "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente"),
            '                     nsp(2, "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts.Encabezado.$[].Nodos.$[].Nodos.$[].Nodos.$[elem]")
            '                    },
            '                   .fields = New List(Of fieldInfo) From
            '                    {
            '                    field(CamposClientes.CP_CVE_EMPRESA, nsp:=2, attr:="Valor"),
            '                    field(CamposClientes.CA_RFC_CLIENTE, nsp:=1, attr:="Valor"),
            '                    field(CamposClientes.CA_CURP_CLIENTE, nsp:=1, attr:="Valor")
            '                    }
            '                 }
            '             },
            '             New subscriptionsgroup With
            '             {
            '              .active = True,
            '              .toresource = "[SynapsisN].[dbo].[Vt022AduanaSeccionA01]",
            '              ._foreignkeyname = "_id",
            '              ._foreignkey = New ObjectId,
            '              .subscriptions =
            '                New subscriptions With
            '                  {
            '                    .namespaces = New List(Of [namespace]) From
            '                    {
            '                     nsp(1, "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente"),
            '                     nsp(2, "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts.Encabezado.$[].Nodos.$[].Nodos.$[].Nodos.$[elem]")
            '                    },
            '                   .fields = New List(Of fieldInfo) From
            '                   {
            '                    field(CamposClientes.CP_CVE_ADUANA_SECCION, nsp:=2, attr:="Valor")
            '                   }
            '                }
            '             }
            '           }

        End Sub
        Public Overrides Sub ConstruyeEncabezado()

            _estructuraDocumento(TiposBloque.Encabezado) = New List(Of Nodo)

            ' Encabezado principal de la referencia
            ConstruyeSeccion(seccionEnum_:=SeccionesDestinatarios.SDES1,
                             tipoBloque_:=TiposBloque.Encabezado,
                             conCampos_:=True)

        End Sub
        Public Overrides Sub ConstruyeCuerpo()

            _estructuraDocumento(TiposBloque.Cuerpo) = New List(Of Nodo)

            ConstruyeSeccion(seccionEnum_:=SeccionesDestinatarios.SDES2,
                 tipoBloque_:=TiposBloque.Cuerpo,
                 conCampos_:=False)

            ConstruyeSeccion(seccionEnum_:=SeccionesDestinatarios.SDES3,
                tipoBloque_:=TiposBloque.Cuerpo,
                conCampos_:=False)

        End Sub

#End Region

#Region "Funciones"
        Public Overrides Function ObtenerCamposSeccion(ByVal idSeccion_ As Integer) As List(Of Nodo)

            Select Case idSeccion_

                'Generales
                Case SeccionesDestinatarios.SDES1
                    Return New List(Of Nodo) From {
                                                    Item(CP_ID_EMPRESA, IdObject), 'OBJECTID EMPRESA
                                                    Item(CP_CVE_EMPRESA, Entero), 'CLAVE DE EMPRESA
                                                    Item(CamposDestinatario.CP_ID_DESTINATARIO, IdObject),
                                                    Item(CamposDestinatario.CP_CVE_DESTINATARIO, Entero),
                                                    Item(CamposDestinatario.CA_RAZON_SOCIAL, Texto),
                                                    Item(CamposDestinatario.CA_DESTINATARIO_HABILITADO, Booleano),
                                                    Item(CamposDestinatario.CP_TIPO_DESTINATARIO, Booleano)
                                                }

                'Domicilios destinatarios
                Case SeccionesDestinatarios.SDES2
                    Return New List(Of Nodo) From {
                                                    Item(CamposDestinatario.CP_ID_DESTINATARIO, IdObject),
                                                    Item(CamposDestinatario.CA_TAX_ID, Texto),
                                                    Item(CamposDestinatario.CA_CVE_TAX_ID_DESTINATARIO, Texto),
                                                    Item(CamposDomicilio.CA_ID_PAIS, Texto),
                                                    Item(CamposDomicilio.CA_CVE_PAIS, Texto),
                                                    Item(CamposDomicilio.CA_PAIS, Texto),
                                                    Item(CamposDestinatario.CP_ID_DOMICILIO_DESTINATARIO, Texto),
                                                    Item(CamposDestinatario.CP_SEC_DOMICILIO_DESTINATARIO, Texto),
                                                    Item(CamposDestinatario.CA_DOMICILIO_FISCAL_DESTINATARIO, Texto),
                                                    Item(CamposDomicilio.CA_CALLE, Texto, longitud_:=80),
                                                    Item(CamposDomicilio.CA_NUMERO_EXTERIOR, Texto, longitud_:=10),
                                                    Item(CamposDomicilio.CA_NUMERO_INTERIOR, Texto, longitud_:=10),
                                                    Item(CamposDomicilio.CA_NUMERO_EXT_INT, Texto, longitud_:=20),
                                                    Item(CamposDomicilio.CA_CODIGO_POSTAL, Texto, longitud_:=10),
                                                    Item(CamposDomicilio.CA_COLONIA, Texto, longitud_:=80),
                                                    Item(CamposDomicilio.CA_LOCALIDAD, Texto, longitud_:=80),
                                                    Item(CamposDomicilio.CA_CIUDAD, Texto, longitud_:=80),
                                                    Item(CamposDomicilio.CA_ENTIDAD_MUNICIPIO, Texto, longitud_:=80),
                                                    Item(CamposDomicilio.CA_MUNICIPIO, Texto, longitud_:=80),
                                                    Item(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA, Texto, longitud_:=80),
                                                    Item(CamposDomicilio.CA_ENTIDAD_FEDERATIVA, Texto, longitud_:=80),
                                                    Item(CamposProducto.CP_MOTIVO, Texto, 250),
                                                    Item(CamposDestinatario.CA_ESTADO_DOMICILIO_DESTINATARIO, Booleano),
                                                    Item(CamposDestinatario.CA_DOMICILIO_ARCHIVADO_DESTINATARIO, Texto),
                                                    Item(CamposDestinatario.CA_MOTIVO_ARCHIVADO_DOMICILIO_DESTINATARIO, Texto),
                                                    Item(CamposDestinatario.CA_FECHA_ARCHIVADO_DOMICILIO_DESTINATARIO, Texto),
                                                    Item(CamposProveedorOperativo.CP_FIRMA_ELECTRONICA, Texto),
                                                    Item(CamposGlobales.CP_IDENTITY, Entero) 'ES PARA EL TARJETERO
                                                }

                'Historial Domicilios físcales
                Case SeccionesDestinatarios.SDES3
                    Return New List(Of Nodo) From {
                                                    Item(CP_ID_DOMICILIO_DESTINATARIO, Texto),
                                                    Item(CA_TAX_ID, Texto),
                                                    Item(CA_DOMICILIO_FISCAL_DESTINATARIO, Texto),
                                                    Item(CA_ESTADO_DOMICILIO_DESTINATARIO, Booleano),
                                                    Item(CA_DOMICILIO_ARCHIVADO_DESTINATARIO, Texto),
                                                    Item(CA_MOTIVO_ARCHIVADO_DOMICILIO_DESTINATARIO, Texto),
                                                    Item(CA_FECHA_ARCHIVADO_DOMICILIO_DESTINATARIO, Texto)
                                                }
                Case Else

                    _tagWatcher.SetError(Me, "La sección con clave:" & idSeccion_ & " no esta configurada.")

            End Select

            Return Nothing

        End Function

#End Region

    End Class

End Namespace
