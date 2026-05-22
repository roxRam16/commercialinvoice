
#Region "├┴┘├┴┘├┴┘├┴┘├┴┘|├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘   DEPENDENCIAS   ├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘"

'RECURSOS DEL CMF
Imports System.Drawing
Imports System.IO
Imports System.Web.Script.Serialization
Imports Gsol.Web.Components
Imports Gsol.Web.Components.PillboxControl.ToolbarModality
Imports MongoDB.Bson
Imports MongoDB.Driver
'OBJETOS DIMENSIONALES (ODS's) Dependencias en MongoDB
Imports Rec.Globals.Controllers
'OBJETOS BIDIMENSIONALES (ODF's.  Dependencias Krombase/SQL Server)
Imports Rec.Globals.Utils
'UTILERIAS/RECURSOS ADICIONALES
Imports Sax.Web
Imports Syn.CustomBrokers.Controllers
Imports Syn.Documento
Imports Syn.CustomBrokers
Imports Syn.Documento.Componentes
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Operaciones
Imports Syn.Utils
Imports Wma.Exceptions
Imports Wma.Exceptions.TagWatcher
Imports Wma.Exceptions.TagWatcher.TypeStatus
Imports System.Net.Mime.MediaTypeNames
Imports System.Web.Services.Description
Imports System.Reflection





'Imports MongoDB.Driver.GridFS

#End Region

Public Class Ges022_001_RegistroProductos
    Inherits ControladorBackend

#Region "████████████████████████████████████████   Atributos locales  ██████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    Private _controladorTigie As IControladorTIGIE

    Private _tagwatcher As TagWatcher

    Private _listaFraccionesArancelarias As List(Of FraccionArancelaria)

    Private _lista As New List(Of SelectOption)

    Private _buscarFraccionArancelaria As ControladorBusqueda(Of ConstructorTIGIE)

    Private _constructorTigie As ConstructorTIGIE

    Private _controladorSecuencias As IControladorSecuencia

    Private _secuencia As ISecuencia

    Private _loginUsuario As Dictionary(Of String, String)

    Private _controlBusquedaCliente As ControladorBusqueda(Of ConstructorCliente)

    Private _controlBusquedaProveedor As ControladorBusqueda(Of ConstructorProveedoresOperativos)

#End Region

#Region "████████████████████████████████████████   Constructores  ██████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████
    Sub New()

        Dim officeOnline_ = Statements.GetOfficeOnline

        If officeOnline_ Is Nothing Then

            Statements.SetEnvironmentOnline(1)

        End If

    End Sub


#End Region


#Region "██████ Vinculación c/capas inf █████████       SAX      ████████████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████
    Public Overrides Sub Inicializa()

        With Buscador

            .DataObject = New ConstructorProducto(True)

            .addFilter(SeccionesProducto.SPTO1, CamposProducto.CP_NOMBRE_COMERCIAL, "Nombre comercial")

        End With

        btnAplicarFraccionArancelaria.Enabled = False

        BloqueaCamposFraccionArancelaria()

    End Sub

    'ASIGNACION PARA CONTROLES AUTOMÁTICOS
    Public Overrides Function Configuracion() As TagWatcher

        [Set](icNombreComercial, CamposProducto.CP_NOMBRE_COMERCIAL)

        [Set](swcEstadoProducto, CamposProducto.CP_HABILITADO, propiedadDelControl_:=PropiedadesControl.Checked)

        '[Set](fcImagenProducto, CamposProducto.CP_RUTA_ARCHIVO_MUESTRA)

        '[Set](fcImagenProducto, CamposProducto.CP_RUTA_ARCHIVO_MUESTRA, asignarA_:=TiposAsignacion.ValorPresentacion, propiedadDelControl_:=PropiedadesControl.Text)

        [Set](icFraccionArancelaria, CamposProducto.CP_FRACCION_ARANCELARIA)

        [Set](icFraccionArancelariaDescripcion, CamposProducto.CP_DESCRIPCION_FRACCION_ARANCELARIA)

        [Set](icNico, CamposProducto.CP_NICO)

        [Set](icNicoDescripcion, CamposProducto.CP_DESCRIPCION_NICO)

        [Set](icFechaRegistro, CamposProducto.CP_FECHA_REGISTRO)

        [Set](scEstatus, CamposProducto.CP_ESTATUS)

        [Set](icObservaciones, CamposProducto.CP_OBSERVACION)

        [Set](fbcCliente, CamposClientes.CA_RAZON_SOCIAL, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](fbcProveedor, CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icIdKrom, CamposProducto.CP_IDKROM, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icNumeroParte, CamposProducto.CP_NUMERO_PARTE, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icAlias, CamposProducto.CP_ALIAS, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scTipoAliasUNO, CamposProducto.CP_TIPO_ALIAS, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scTipoAliasUNO, CamposProducto.CP_TIPO_ALIAS, asignarA_:=TiposAsignacion.ValorPresentacion, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icDescripcion, CamposProducto.CP_DESCRIPCION, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](swcAplicaCove, CamposProducto.CP_APLICACOVE, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icDescripcionCove, CamposProducto.CP_DESCRIPCION_COVE, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](ccDescipcionesFacturas, Nothing, seccion_:=SeccionesProducto.SPTO5, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](pbcDescipcionesFacturas, Nothing, seccion_:=SeccionesProducto.SPTO3)

        btnAplicarFraccionArancelaria.Enabled = False

        BloqueaCamposFraccionArancelaria()

        btnRestaurar.Enabled = False

        btnArchivar.Enabled = False

        Return New TagWatcher(1)

    End Function

    Public Overrides Sub BotoneraClicNuevo()

        PreparaTarjetero(Simple, pbcDescipcionesFacturas)

        btnAplicarFraccionArancelaria.Enabled = False

        BloqueaCamposFraccionArancelaria()

        btnRestaurar.Enabled = False

        btnArchivar.Enabled = False

        swcEstadoProducto.Checked = True

        BuscarFraccionesArancelarias.Visible = True

        icFechaRegistro.Value = Convert.ToDateTime(Now).Date.ToString("yyyy-MM-dd")

    End Sub

    Public Overrides Sub BotoneraClicGuardar()

        If Not ProcesarTransaccion(Of ConstructorProducto)().Status = TypeStatus.Errors Then



        End If

    End Sub

    Public Overrides Sub BotoneraClicEditar()

        PreparaTarjetero(Advanced, pbcDescipcionesFacturas)

        PanelBotonArchivado.Visible = True

        fscHistoriales.Visible = True

        BloqueaCamposFraccionArancelaria()

    End Sub

    Public Overrides Sub BotoneraClicBorrar()


    End Sub

    Public Overrides Sub BotoneraClicOtros(ByVal IndexSelected_ As Integer)

        If IndexSelected_ = 10 Then

            BuscarFraccionesArancelarias.Visible = True

            fbcFraccionArancelaria.Value = Nothing

            fbcFraccionArancelaria.DataSource = Nothing

            BloqueaCamposFraccionArancelaria()

        End If

    End Sub

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Esta metodo se manda llamar al dar clic en cualquiera de las opciones del      '
    ' dropdown en la botonera; recibe el valor indice del boton al que se le ha dado '
    ' clic                                                                           '
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    'EVENTOS PARA LA INSERCIÓN DE DATOS
    Public Overrides Function AntesRealizarInsercion(ByVal session_ As IClientSessionHandle) As TagWatcher

        Dim tagwatcher_ As TagWatcher

        '      ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas con transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒
        If session_ IsNot Nothing Then

            '  ██████inicio███████        Logica de negocios local      ████████████████████████



            '  ████████fin█████████       Logica de negocios local       ███████████████████████

            tagwatcher_ = New TagWatcher

            tagwatcher_.SetOK()


        Else  '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas sin transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 

            tagwatcher_ = New TagWatcher

            tagwatcher_.SetOK()

        End If

        'SincronizarCatalogo()

        Return tagwatcher_

    End Function

    Public Overrides Sub RealizarInsercion(ByRef documentoElectronico_ As DocumentoElectronico)

        _controladorSecuencias = New ControladorSecuencia

        _tagwatcher = New TagWatcher

        _tagwatcher = _controladorSecuencias.Generar(SecuenciasComercioExterior.Productos.ToString,
                                                     1, 1, 1, 1, Statements.GetOfficeOnline()._id)

        _secuencia = New Secuencia

        _secuencia = DirectCast(_tagwatcher.ObjectReturned, Secuencia)

        _loginUsuario = New Dictionary(Of String, String)

        _loginUsuario = Session("DatosUsuario")

        Dim datosCliente_ As String = Nothing

        Dim cveCliente_ As String() = Nothing

        If fbcCliente.Text <> "" Then

            datosCliente_ = fbcCliente.Text

            cveCliente_ = datosCliente_.Split("|"c)

        Else

            datosCliente_ = "No definido"

            cveCliente_ = Nothing

        End If

        With documentoElectronico_

            .Id = _secuencia._id.ToString

            .UsuarioGenerador = _loginUsuario("Nombre")

            .IdDocumento = _secuencia.sec

            .FolioDocumento = 0

            .FolioOperacion = _secuencia.sec

            .TipoPropietario = SecuenciasComercioExterior.Productos.ToString

            .IdCliente = 0

            .NombrePropietario = fbcCliente.Text

            .IdPropietario = CInt(cveCliente_(1).Trim())

            .ObjectIdPropietario = New ObjectId(fbcCliente.Value)

        End With

    End Sub

    Public Overrides Sub DespuesOperadorDatosProcesar(ByRef documentoElectronico_ As DocumentoElectronico)

        _loginUsuario = New Dictionary(Of String, String)

        _loginUsuario = Session("DatosUsuario")

        Dim userName_ As String = _loginUsuario("WebServiceUserID")

        With documentoElectronico_

            'HISTORICO CLASIFICACIÓN

            'With .Seccion(SeccionesProducto.SPTO1)

            '    If fcImagenProducto.Value = "" Then

            '    Else

            '        Dim valores_ = fcImagenProducto.Value.Replace("[{", "").Replace("}]", "").Replace(Chr(34), "").Split(",")

            '        Dim valor_ = valores_(0).Split(":")

            '        If valor_.Count > 1 Then

            '            Dim valorPresentacion_ = valores_(1).Split(":")

            '            .Attribute(CamposProducto.CP_RUTA_ARCHIVO_MUESTRA).Valor = ObjectId.Parse(valor_(1))

            '            .Attribute(CamposProducto.CP_RUTA_ARCHIVO_MUESTRA).ValorPresentacion = valorPresentacion_(1)

            '        Else

            '            .Attribute(CamposProducto.CP_RUTA_ARCHIVO_MUESTRA).Valor = ObjectId.Parse(valor_(0))

            '            .Attribute(CamposProducto.CP_RUTA_ARCHIVO_MUESTRA).ValorPresentacion = ""

            '        End If

            '    End If

            'End With

            With .Seccion(SeccionesProducto.SPTO2)

                'If _unidadMedida IsNot Nothing Then

                '    If _unidadMedida.Count > 0 Then

                '        .Attribute(CamposTarifaArancelaria.CA_CLAVE_UNIDAD_MEDIDA).Valor = _unidadMedida(0)

                '        .Attribute(CamposTarifaArancelaria.CA_UNIDAD_MEDIDA).Valor = _unidadMedida(1)

                '        .Attribute(CamposTarifaArancelaria.CA_UNIDAD_MEDIDA_CORTO).Valor = _unidadMedida(2)

                '    End If

                'End If

                If GetVars("_fraccionSeleccionada") IsNot Nothing Then

                    ''REEEMPLAZAR POR LOS VALORES CORRECTOS, PERO ESTO HASTA QUE TIGIE FUNCIONE CORRECTAMENTE

                    Dim fraccionSeleccionada_ = DirectCast(GetVars("_fraccionSeleccionada"), FraccionArancelaria)

                    .Attribute(CamposTarifaArancelaria.CA_CLAVE_UNIDAD_MEDIDA).Valor = fraccionSeleccionada_.CveUnidadMedida

                    .Attribute(CamposTarifaArancelaria.CA_UNIDAD_MEDIDA).Valor = fraccionSeleccionada_.UnidadMedida

                    .Attribute(CamposTarifaArancelaria.CA_UNIDAD_MEDIDA_CORTO).Valor = fraccionSeleccionada_.UnidadMedidaCorto

                End If

            End With

            Dim hayHistoricoClasificacion_ = If(ccHistorialClasificacion.DataSource Is Nothing, True, If(ccHistorialClasificacion.DataSource.length = 0, True, False))

            If hayHistoricoClasificacion_ Then

                If icFraccionArancelaria.Value <> "" Then

                    With .Seccion(SeccionesProducto.SPTO4).Partida(documentoElectronico_)

                        .Attribute(CamposProducto.CP_FRACCION_ARANCELARIA).Valor = icFraccionArancelaria.Value

                        .Attribute(CamposProducto.CP_NICO).Valor = icNico.Value

                        .Attribute(CamposProducto.CP_FECHA_MODIFICACION).Valor = DateTime.Now

                        .Attribute(CamposProducto.CP_MOTIVO).Valor = "✅ ALTA"

                        .Attribute(CamposProducto.CP_LOGIN_USUARIO).Valor = userName_

                        .Attribute(CamposProducto.CP_ENVIRONMENT).Valor = __SYSTEM_ENVIRONMENT.Value

                        .Attribute(CamposProducto.CP_ENVIRONMENT).ValorPresentacion = __SYSTEM_ENVIRONMENT.Text.ToUpper

                    End With

                End If

            Else
                Dim encontrado_ = False

                Dim item_ = ccHistorialClasificacion.DataSource(0)

                If icFraccionArancelaria.Value & "-" & icNico.Value = item_("icHistoricoFraccion") Then

                    encontrado_ = True
                End If

                If Not encontrado_ Then
                    If icFraccionArancelaria.Value <> "" Then

                        With .Seccion(SeccionesProducto.SPTO4).Partida(documentoElectronico_)

                            .Attribute(CamposProducto.CP_FRACCION_ARANCELARIA).Valor = icFraccionArancelaria.Value

                            .Attribute(CamposProducto.CP_NICO).Valor = icNico.Value

                            .Attribute(CamposProducto.CP_FECHA_MODIFICACION).Valor = DateTime.Now

                            .Attribute(CamposProducto.CP_MOTIVO).Valor = If(icMotivo.Value = "", "🔄 CAMBIO", icMotivo.Value)

                            .Attribute(CamposProducto.CP_LOGIN_USUARIO).Valor = userName_

                            .Attribute(CamposProducto.CP_ENVIRONMENT).Valor = __SYSTEM_ENVIRONMENT.Value

                            .Attribute(CamposProducto.CP_ENVIRONMENT).ValorPresentacion = __SYSTEM_ENVIRONMENT.Text.ToUpper

                        End With
                    End If
                End If
            End If

            Dim Nodos_ = .Seccion(SeccionesProducto.SPTO3)

            For Each nodo_ In Nodos_.Nodos

                Dim nodoDescripciones_ = nodo_.Seccion(SeccionesProducto.SPTO5)
                For Each nodoDescripcion_ In nodoDescripciones_.Nodos

                    With nodoDescripcion_

                        Dim idKrom_ = nodoDescripcion_.Campo(CamposProducto.CP_IDKROM).Valor
                        Dim estado_ = nodoDescripcion_.Campo(CamposProducto.CP_IDKROM).estado

                        If idKrom_ = 0 Then
                            _controladorSecuencias = New ControladorSecuencia

                            _tagwatcher = New TagWatcher

                            _tagwatcher = _controladorSecuencias.Generar(SecuenciasComercioExterior.IdKrom.ToString,
                                                     1, 1, 1, 1, Statements.GetOfficeOnline()._id)
                            _secuencia = New Secuencia
                            _secuencia = DirectCast(_tagwatcher.ObjectReturned, Secuencia)

                            .Campo(CamposProducto.CP_IDKROM).Valor = _secuencia.sec

                            .Campo(CamposProducto.CP_FECHA_MODIFICACION).Valor = Date.Now

                            Dim partida_ = documentoElectronico_.Seccion(SeccionesProducto.SPTO6).Partida(documentoElectronico_)

                            partida_.Campo(CamposProducto.CP_IDKROM).Valor = .Campo(CamposProducto.CP_IDKROM).Valor

                            partida_.Campo(CamposProducto.CP_NUMERO_PARTE).Valor = .Campo(CamposProducto.CP_NUMERO_PARTE).Valor

                            partida_.Campo(CamposProducto.CP_TIPO_ALIAS).Valor = .Campo(CamposProducto.CP_TIPO_ALIAS).Valor

                            partida_.Campo(CamposProducto.CP_APLICACOVE).Valor = .Campo(CamposProducto.CP_APLICACOVE).Valor

                            partida_.Campo(CamposProducto.CP_DESCRIPCION).Valor = .Campo(CamposProducto.CP_DESCRIPCION).Valor

                            partida_.Campo(CamposClientes.CA_RAZON_SOCIAL).Valor = nodo_.Campo(CamposClientes.CA_RAZON_SOCIAL).ValorPresentacion

                            partida_.Campo(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR).Valor = nodo_.Campo(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR).ValorPresentacion

                            partida_.Campo(CamposProducto.CP_FECHA_MODIFICACION).Valor = .Campo(CamposProducto.CP_FECHA_MODIFICACION).Valor
                            partida_.Campo(CamposProducto.CP_LOGIN_USUARIO).Valor = userName_

                            partida_.Campo(CamposProducto.CP_ENVIRONMENT).Valor = __SYSTEM_ENVIRONMENT.Value

                            partida_.Campo(CamposProducto.CP_ENVIRONMENT).ValorPresentacion = __SYSTEM_ENVIRONMENT.Text.ToUpper


                        Else
                            Dim diccionarioDescripciones_ = GetVars("diccionarioDescrionciones_")

                            For Each descripcion_ In diccionarioDescripciones_
                                If descripcion_("icIdKrom") = idKrom_ And estado_ = 1 Then

                                    Dim tipoalias_ = descripcion_("scTipoAlias")

                                    If descripcion_("icNumeroParte") <> .Campo(CamposProducto.CP_NUMERO_PARTE).Valor OrElse
                                       descripcion_("icDescripcion") <> .Campo(CamposProducto.CP_DESCRIPCION).Valor OrElse
                                       descripcion_("swcAplicaCove") <> .Campo(CamposProducto.CP_APLICACOVE).Valor OrElse
                                       descripcion_("icDescripcionCove") <> .Campo(CamposProducto.CP_DESCRIPCION_COVE).Valor OrElse
                                       descripcion_("icAlias") <> .Campo(CamposProducto.CP_ALIAS).Valor Then

                                        Dim partida_ = documentoElectronico_.Seccion(SeccionesProducto.SPTO6).Partida(documentoElectronico_)

                                        partida_.Campo(CamposProducto.CP_IDKROM).Valor = .Campo(CamposProducto.CP_IDKROM).Valor

                                        partida_.Campo(CamposProducto.CP_NUMERO_PARTE).Valor = .Campo(CamposProducto.CP_NUMERO_PARTE).Valor

                                        partida_.Campo(CamposProducto.CP_TIPO_ALIAS).Valor = .Campo(CamposProducto.CP_TIPO_ALIAS).Valor

                                        partida_.Campo(CamposProducto.CP_APLICACOVE).Valor = .Campo(CamposProducto.CP_APLICACOVE).Valor

                                        partida_.Campo(CamposProducto.CP_DESCRIPCION).Valor = .Campo(CamposProducto.CP_DESCRIPCION).Valor

                                        partida_.Campo(CamposClientes.CA_RAZON_SOCIAL).Valor = nodo_.Campo(CamposClientes.CA_RAZON_SOCIAL).ValorPresentacion

                                        partida_.Campo(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR).Valor = nodo_.Campo(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR).ValorPresentacion

                                        partida_.Campo(CamposProducto.CP_FECHA_MODIFICACION).Valor = .Campo(CamposProducto.CP_FECHA_MODIFICACION).Valor

                                        partida_.Campo(CamposProducto.CP_LOGIN_USUARIO).Valor = userName_

                                        partida_.Campo(CamposProducto.CP_ENVIRONMENT).Valor = __SYSTEM_ENVIRONMENT.Value

                                        partida_.Campo(CamposProducto.CP_ENVIRONMENT).ValorPresentacion = __SYSTEM_ENVIRONMENT.Text.ToUpper

                                    End If
                                End If

                            Next

                        End If

                    End With

                Next

            Next

        End With

        ColocaHistóricoClasificaciones(documentoElectronico_)

        ColocaHistóricoDescripciones(documentoElectronico_)

    End Sub

    Public Overrides Function DespuesRealizarInsercion() As TagWatcher

        PanelBotonArchivado.Visible = True

        fscHistoriales.Visible = True

        Return New TagWatcher(Ok)
    End Function


    'EVENTOS PARA MODIFICACIÓN DE DATOS
    Public Overrides Function AntesRealizarModificacion(ByVal session_ As IClientSessionHandle) As TagWatcher

        Dim tagwatcher_ As TagWatcher

        '     ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas con transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒

        If session_ IsNot Nothing Then

            '  ██████inicio███████        Logica de negocios local      ████████████████████████

            tagwatcher_ = New TagWatcher

            tagwatcher_.SetOK()


        Else  '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas sin transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 

            tagwatcher_ = New TagWatcher

            tagwatcher_.SetOK()

        End If

        Return tagwatcher_

    End Function

    Public Overrides Sub RealizarModificacion(ByRef documentoElectronico_ As DocumentoElectronico)

        Dim cveCliente_ As String() = Nothing

        Dim datosCliente_ As String = Nothing

        If fbcCliente.Text <> "" Then

            datosCliente_ = fbcCliente.Text

            cveCliente_ = datosCliente_.Split("|"c)

        Else

            datosCliente_ = "No definido"

            cveCliente_ = Nothing

        End If

        With documentoElectronico_

            .IdCliente = 0

            .NombrePropietario = fbcCliente.Text

            .IdPropietario = CInt(cveCliente_(1).Trim())

            .ObjectIdPropietario = New ObjectId(fbcCliente.Value)

            ''LOS METADATOS NO SE MODIFICAN AL REALIZAR UNA ACTUALIZACIÓN
            '' .Metadatos

        End With

    End Sub

    Public Overrides Function DespuesRealizarModificacion() As TagWatcher

        PanelBotonArchivado.Visible = True

        PanelBotonArchivado.Enabled = True

        ColocaHistóricoClasificaciones(OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente)

        ColocaHistóricoDescripciones(OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente)

        Return New TagWatcher(Ok)

    End Function

    'EVENTOS PARA PRESENTACIÓN DE DATOS EN FRONTEND
    Public Overrides Sub PreparaModificacion(ByRef documentoElectronico_ As DocumentoElectronico)

        'LLENADO DEL HISTORICO DE CLASIFICACIONES

        fscHistoriales.Visible = True

        With documentoElectronico_.Seccion(SeccionesProducto.SPTO1)

            'If fcImagenProducto.Value = "" Or fcImagenProducto.Value = "000000000000000000000000" Then

            'Else

            '    Dim doc As Byte() = (New ControladorDocumento).GetDocument(.Campo(CamposProducto.CP_RUTA_ARCHIVO_MUESTRA).Valor).ObjectReturned

            '    Dim memoryStream_ As New MemoryStream(doc)

            '    Randomize()

            '    Dim filename_ = "prueba" & Rnd(10000) & ".jpg"

            '    'Dim filePath As String = "C:/inetpub/wwwroot/saxtest/sax/projects/synapsis/dev/main/FrontEnd/Recursos/Imgs/" & filename_ ' Ruta del archivo

            '    'Dim filePath As String = "C:/inetpub/wwwroot/saxtest/sax/projects/synapsis.hyb/products/dev/main/FrontEnd/Recursos/Imgs/ModuloProductos/" & filename_
            '    Dim filePath As String = "C:/SAX/projects/synapsis.hyb/products/dev/main/FrontEnd/Recursos/Imgs/ModuloProductos/" & filename_

            '    Dim fileMode As FileMode = FileMode.Create ' Modo de acceso (crear en este caso)

            '    Dim fileStream As FileStream = New FileStream(filePath, fileMode)

            '    memoryStream_.WriteTo(fileStream)

            '    fileStream.Close()

            '    ''REVISAR LA IMAGEN

            '    'fcImagenProducto.CssClass = "col-xs-12 col-md-6 col-lg-6"

            '    'icMuestraProducto.Source = "/FrontEnd/Recursos/Imgs/ModuloProductos/" & filename_

            '    'SetVars("PATH", icMuestraProducto.Source)

            '    'icMuestraProducto.Visible = True

            '    'fcImagenProducto.Visible = False

            'End If

        End With


    End Sub

    Public Overrides Sub DespuesBuquedaGeneralConDatos()

        BuscarFraccionesArancelarias.Visible = False

        BloqueaCamposFraccionArancelaria()

        CambiarEstadoFraccionArancelaria(scEstatus.Value)

        ColocaHistóricoClasificaciones(OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente)

        ColocaHistóricoDescripciones(OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente)

    End Sub

    Public Overrides Sub DespuesBuquedaGeneralSinDatos()

        If OperacionGenerica IsNot Nothing Then

            BuscarFraccionesArancelarias.Visible = False

            BloqueaCamposFraccionArancelaria()

            PanelBotonArchivado.Enabled = True

            PanelBotonArchivado.Visible = True

            PreparaTarjetero([Default], pbcDescipcionesFacturas)

        End If

    End Sub

    Protected Sub BloqueaCamposFraccionArancelaria()

        icFraccionArancelaria.Enabled = False

        icFraccionArancelariaDescripcion.Enabled = False

        icNico.Enabled = False

        icNicoDescripcion.Enabled = False

    End Sub


    'EVENTOS DE MANTENIMIENTO
    Public Overrides Sub LimpiaSesion()

        SetVars("_CatalogsData", Nothing)

        SetVars("_clasificacionArchivados", Nothing)

        SetVars("_fraccionesArancelarias", Nothing)

        SetVars("_nicos", Nothing)

        SetVars("_fraccionSeleccionada", Nothing)

    End Sub

    Public Overrides Sub Limpiar()

        CambiarEstadoFraccionArancelaria(0)
        ''Implementar dispose
        _controladorTigie = Nothing

        _tagwatcher = Nothing

        _listaFraccionesArancelarias = Nothing

        _lista = Nothing

        ccDescipcionesFacturas.DataSource = Nothing

        ccHistorialClasificacion.DataSource = Nothing

        ccHistorialDescripciones.DataSource = Nothing

        ' fcImagenProducto.CssClass = "col-xs-12 col-md-6"

        icNico.Value = Nothing

        icMotivo.Visible = False

        btn_ConfirmarArchivado.Visible = False

        PanelBotonArchivado.Visible = False

        ' icMuestraProducto.Visible = False

        _constructorTigie = Nothing

        ''Ponle dispose
        _controladorSecuencias = Nothing

        _secuencia = Nothing

        _loginUsuario = Nothing

        _controlBusquedaCliente = Nothing

        _controlBusquedaProveedor = Nothing

        BloqueaCamposFraccionArancelaria()

        BuscarFraccionesArancelarias.Visible = False

        fscHistoriales.Visible = False

    End Sub

#End Region

#Region "████████████████  QUINTA CAPA  █████████       Reglas locales         ██████████████████████████████"
    '    ██                                                                                                ██
    '    ██    Pendientes ( al 07/05/2022 )                                                                ██
    '    ██      1. Mejorar la carga de los dropdowns ( hace las consultas en cada postback)               ██
    '    ██      2. Completar el caso de uso de inserción cuando se reutilice una empresa y domicilio      ██
    '    ██      3. Completar la carga de datos del CRUD ( el resto de las secciones )                     ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

#End Region

    Protected Sub fbc_Cliente_TextChanged(sender As Object, e As EventArgs)

        _controlBusquedaCliente = New ControladorBusqueda(Of ConstructorCliente)

        Dim references_ = _controlBusquedaCliente.Buscar(fbcCliente.Text, New Filtro _
                                                  With {.IdSeccion = SeccionesClientes.SCS1, .IdCampo = CamposClientes.CA_RAZON_SOCIAL})

        fbcCliente.DataSource = references_

    End Sub

    Protected Sub fbc_Proveedor_TextChanged(sender As Object, e As EventArgs)

        _controlBusquedaProveedor = New ControladorBusqueda(Of ConstructorProveedoresOperativos)

        Dim references_ = _controlBusquedaProveedor.Buscar(fbcProveedor.Text, New Filtro _
                                                  With {.IdSeccion = SeccionesProvedorOperativo.SPRO1, .IdCampo = CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR})

        fbcProveedor.DataSource = references_

    End Sub

    Protected Sub fbc_FraccionArancelaria_TextChanged(sender As Object, e As EventArgs)

        If fbcFraccionArancelaria.Value = "" Then

            icFraccionArancelaria.Enabled = False

            icFraccionArancelariaDescripcion.Enabled = False

            icNico.Enabled = False

            icNicoDescripcion.Enabled = False

            _controladorTigie = New ControladorTIGIE()

            _tagwatcher = New TagWatcher

            _tagwatcher = _controladorTigie.EnlistarFracciones(fbcFraccionArancelaria.Text)

            If _tagwatcher IsNot Nothing Then

                If _tagwatcher.Status = TypeStatus.Ok Then

                    _listaFraccionesArancelarias = New List(Of FraccionArancelaria)

                    _listaFraccionesArancelarias = _tagwatcher.ObjectReturned

                    SetVars("_fraccionesArancelarias", _listaFraccionesArancelarias)

                    _lista = New List(Of SelectOption)

                    _listaFraccionesArancelarias.ForEach(Sub(ByVal fraccion_ As FraccionArancelaria)

                                                             Dim longitudFinal_ As Integer = Math.Min(fraccion_.DescripcionFraccion.Length, 120)

                                                             Dim longitudFinalNico_ As Integer = Math.Min(fraccion_.NicoDescripcion.Length, 120)

                                                             Dim descripcionFraccion_ = fraccion_.DescripcionFraccion.Substring(0, longitudFinal_) & "..."

                                                             Dim descripcionNico_ = fraccion_.NicoDescripcion.Substring(0, longitudFinalNico_) & "..."

                                                             _lista.Add(New SelectOption With {
                                                                    .Value = fraccion_.Id.ToString,
                                                                    .Text = $"{fraccion_.Fraccion}  |  { descripcionFraccion_}   |   {fraccion_.Nico}   |   { descripcionNico_} "
                                                                    })
                                                         End Sub)

                    fbcFraccionArancelaria.DataSource = _lista

                    btnAplicarFraccionArancelaria.Enabled = True

                Else

                    DisplayMessage("Fracción arancelaria no disponible", StatusMessage.Info)

                End If

            End If

        End If

    End Sub

    Protected Sub fbc_FraccionArancelaria_Click(sender As Object, e As EventArgs)

        If String.IsNullOrEmpty(fbcFraccionArancelaria.Value) Then

            LimpiarCamposFraccionArancelaria()

            btnAplicarFraccionArancelaria.Enabled = False

            icFraccionArancelaria.Enabled = False

            icFraccionArancelariaDescripcion.Enabled = False

            icNico.Enabled = False

            icNicoDescripcion.Enabled = False

        End If
    End Sub

    Protected Sub btnAplicarFraccionArancelaria_Click(sender As Object, e As EventArgs)

        icFraccionArancelaria.Enabled = False

        icFraccionArancelariaDescripcion.Enabled = False

        icNico.Enabled = False

        icNicoDescripcion.Enabled = False

        If Not String.IsNullOrEmpty(fbcFraccionArancelaria.Value) Then
            ''lo tomaremos de la variable de sesion al seleccionar una fraccion

            If GetVars("_fraccionesArancelarias") IsNot Nothing Then

                Dim listaFraccionesArancelarias_ As List(Of FraccionArancelaria) = DirectCast(GetVars("_fraccionesArancelarias"), List(Of FraccionArancelaria))

                Dim fraccionSeleccionada_ = (From item In listaFraccionesArancelarias_
                                             Where item.Id = ObjectId.Parse(fbcFraccionArancelaria.Value)
                                             Select item).FirstOrDefault()

                icFraccionArancelaria.Value = fraccionSeleccionada_.Fraccion

                icFraccionArancelariaDescripcion.Value = fraccionSeleccionada_.DescripcionFraccion

                icNico.Value = fraccionSeleccionada_.Nico

                icNicoDescripcion.Value = fraccionSeleccionada_.NicoDescripcion

                BuscarFraccionesArancelarias.Visible = False

                SetVars("_fraccionSeleccionada", fraccionSeleccionada_)
            End If

        Else

            LimpiarCamposFraccionArancelaria()

        End If

    End Sub


    Protected Sub scEstatus_SelectedIndexChanged(sender As Object, e As EventArgs)

        CambiarEstadoFraccionArancelaria(scEstatus.Value)

    End Sub

    Protected Sub CambiarEstadoFraccionArancelaria(ByVal estado_ As String)

        Select Case estado_

            Case "1"

                lbEstadoActivo.Visible = True

                lbEstadoPreliminar.Visible = False

                lbEstadoClasificado.Visible = False

                lbEstadoSuprimido.Visible = False

                lbEstadoDefault.Visible = False

                DisplayMessage("Estado autorizado aplicado", StatusMessage.Info)

            Case "2"

                lbEstadoPreliminar.Visible = True

                lbEstadoActivo.Visible = False

                lbEstadoClasificado.Visible = False

                lbEstadoSuprimido.Visible = False

                lbEstadoDefault.Visible = False

                DisplayMessage("Estado preliminar aplicado", StatusMessage.Info)

            Case "3"

                lbEstadoClasificado.Visible = True

                lbEstadoPreliminar.Visible = False

                lbEstadoActivo.Visible = False

                lbEstadoSuprimido.Visible = False

                lbEstadoDefault.Visible = False

                DisplayMessage("Estado clasificado aplicado", StatusMessage.Info)

            Case "4"

                lbEstadoSuprimido.Visible = True

                lbEstadoPreliminar.Visible = False

                lbEstadoActivo.Visible = False

                lbEstadoClasificado.Visible = False

                lbEstadoDefault.Visible = False

                DisplayMessage("Estado suprimido aplicado", StatusMessage.Info)

            Case Else

                lbEstadoDefault.Visible = True

                lbEstadoPreliminar.Visible = False

                lbEstadoActivo.Visible = False

                lbEstadoClasificado.Visible = False

                lbEstadoSuprimido.Visible = False

        End Select

    End Sub

    Protected Sub LimpiarCamposFraccionArancelaria()

        CambiarEstadoFraccionArancelaria(0)

        icFraccionArancelaria.Enabled = False

        icFraccionArancelariaDescripcion.Enabled = False

        icNico.Enabled = False

        icNicoDescripcion.Enabled = False

        icFraccionArancelaria.Value = Nothing

        icFraccionArancelariaDescripcion.Value = Nothing

        icNico.Value = Nothing

        icNicoDescripcion.Value = Nothing

    End Sub

    Protected Sub btn_ConfirmarArchivado_Click(sender As Object, e As EventArgs)

        Dim loginUsuario_ As Dictionary(Of String, String) = Session("DatosUsuario")

        Dim userName_ As String = loginUsuario_("WebServiceUserID")

        Dim clasificacionArchivados As Object() = GetVars("_clasificacionArchivados")

        Array.Resize(clasificacionArchivados, clasificacionArchivados.Length + 1)

        clasificacionArchivados(clasificacionArchivados.Length - 1) = New Dictionary(Of String, String) From {
            {ccHistorialClasificacion.KeyField, 0},
            {"icHistoricoFraccion", icFraccionArancelaria.Value & "-" & icNico.Value},
            {"icHistoricoMotivo", icMotivo.Value},
            {"icHistoricoFechaModificacion", Date.Now().ToString("dd/MM/yyyy hh:mm:ss tt")},
            {"icHistoricoUsuario", userName_},
            {"icHistoricoOficina", __SYSTEM_ENVIRONMENT.Text.ToUpper}
        }

        SetVars("_clasificacionArchivados", clasificacionArchivados)

        ccHistorialClasificacion.DataSource = clasificacionArchivados

        BuscarFraccionesArancelarias.Visible = True

        fscHistoriales.Visible = True

        icFraccionArancelaria.Value = Nothing

        icFraccionArancelaria.Enabled = False

        icFraccionArancelariaDescripcion.Value = Nothing

        icFraccionArancelariaDescripcion.Enabled = False

        icNico.Value = Nothing

        icNico.Enabled = False

        icNicoDescripcion.Value = Nothing

        icNicoDescripcion.Enabled = False

        icFechaRegistro.Value = Nothing

        icFechaRegistro.Value = Convert.ToDateTime(Now).Date.ToString("yyyy-MM-dd")

        scEstatus.Value = Nothing

        icObservaciones.Value = Nothing

        icMotivo.Value = Nothing

        icMotivo.Visible = False

        PanelBotonArchivado.Visible = False

        btn_ConfirmarArchivado.Visible = False

        CambiarEstadoFraccionArancelaria(0)

        scEstatus.DataSource = Nothing

        scEstatus.Value = Nothing

        DisplayMessage("Fracción arancelaría archivada", StatusMessage.Info)

    End Sub

#Region "██████ Vinculación sexta capa  █████████       SAX      ████████████████████████████████████████████"
    '    ██████   Controladores utilizados                     Documentos por coding para MongoDB      ██████
    '    ██████    1.ControladorEmpresas                        1. En Empresa:                         ██████
    '    ██████    2.ControladorRecursosAduanales                  a). Domicilios                      ██████
    '    ██████    3.ControladorSecuencias                         b). Contactos                       ██████
    '    ██████                                                                                        ██████
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    Protected Sub fcImagenProducto_ChooseFile(sender As PropiedadesDocumento, e As EventArgs)

        Dim id = ObjectId.GenerateNewId().ToString

        With sender
            ._idpropietario = id
            .nombrepropietario = "ZERG"
            .tipovinculacion = PropiedadesDocumento.TiposVinculacion.AgenciaAduanal
            .datosadicionales = New InformacionDocumento With {
                          .foliodocumento = "00000007",
                          .tipodocumento = InformacionDocumento.TiposDocumento.SinDefinir,
                          .datospropietario = New InformacionPropietario With {
                              .nombrepropietario = "ZERG",
                              ._id = id
                          }
                         }
            ''.formatoarchivo = PropiedadesDocumento.FormatosArchivo.jpg
        End With

        Dim _idDocumento = ObjectId.Parse(id)

        'PROBANDOO()

    End Sub

    Sub ColocaHistóricoDescripciones(documentoElectronico_ As DocumentoElectronico)

        With documentoElectronico_

            With .Seccion(SeccionesProducto.SPTO6)

                Dim cantidadPartidas_ = .Nodos.Count

                ccHistorialDescripciones.ClearRows()

                For indice_ = 1 To cantidadPartidas_

                    Dim partida_ = .Partida(cantidadPartidas_ - indice_ + 1)

                    ccHistorialDescripciones.SetRow(Sub(catalogRow_ As CatalogRow)

                                                        'Define el valor Llave de tu fila

                                                        catalogRow_.SetIndice(ccHistorialDescripciones.KeyField, indice_)

                                                        'de esta manera agregamos todas las columnas de nuestra fila 
                                                        'usando el control asociado a la columna y el valor que se asignara
                                                        catalogRow_.SetColumn(icHistoricoNumeroParte, partida_.Campo(CamposProducto.CP_NUMERO_PARTE).Valor)

                                                        catalogRow_.SetColumn(icHistoricoDescripcion, partida_.Campo(CamposProducto.CP_DESCRIPCION).Valor)

                                                        catalogRow_.SetColumn(icHistoricoCliente, partida_.Campo(CamposClientes.CA_RAZON_SOCIAL).Valor)

                                                        catalogRow_.SetColumn(icHistoricoProveedor, partida_.Campo(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR).Valor)

                                                        catalogRow_.SetColumn(icHistoricoFechaModificacionDescripciones, partida_.Campo(CamposProducto.CP_FECHA_MODIFICACION).Valor)

                                                        catalogRow_.SetColumn(icHistoricoUsuarioDescripciones, partida_.Campo(CamposProducto.CP_LOGIN_USUARIO).Valor)

                                                        catalogRow_.SetColumn(icHistoricoOficinaDescripciones, partida_.Campo(CamposProducto.CP_ENVIRONMENT).ValorPresentacion)

                                                    End Sub)


                Next

                ccHistorialDescripciones.CatalogDataBinding()

            End With

        End With

        Dim haycolumnas_ As Boolean = If(ccDescipcionesFacturas.DataSource Is Nothing, False, If(ccDescipcionesFacturas.DataSource.length = 0, False, True))

        If haycolumnas_ Then

            SetVars("diccionarioDescrionciones_", ccDescipcionesFacturas.DataSource)

        End If

    End Sub

    Sub ColocaHistóricoClasificaciones(documentoElectronico_ As DocumentoElectronico)

        With documentoElectronico_

            With .Seccion(SeccionesProducto.SPTO4)

                Dim cantidadPartidas_ = .Nodos.Count

                ccHistorialClasificacion.ClearRows()

                For indice_ = 1 To cantidadPartidas_

                    Dim partida_ = .Partida(cantidadPartidas_ - indice_ + 1)

                    ccHistorialClasificacion.SetRow(Sub(catalogRow_ As CatalogRow)

                                                        'Define el valor Llave de tu fila

                                                        catalogRow_.SetIndice(ccHistorialClasificacion.KeyField, indice_)

                                                        'de esta manera agregamos todas las columnas de nuestra fila 
                                                        'usando el control asociado a la columna y el valor que se asignara
                                                        catalogRow_.SetColumn(icHistoricoFraccion, partida_.Campo(CamposProducto.CP_FRACCION_ARANCELARIA).Valor &
                                                                                                   "-" &
                                                                                                   partida_.Campo(CamposProducto.CP_NICO).Valor)

                                                        catalogRow_.SetColumn(icHistoricoMotivo, partida_.Campo(CamposProducto.CP_MOTIVO).Valor)

                                                        Dim fechaModificacion_ As String = partida_.Campo(CamposProducto.CP_FECHA_MODIFICACION).Valor.ToString.Replace("-", "/")

                                                        If fechaModificacion_.IndexOf("/") = 4 Then

                                                            fechaModificacion_ = DateTime.ParseExact(fechaModificacion_, "dd/MM/yyyy hh:mm tt", Nothing)

                                                        End If

                                                        catalogRow_.SetColumn(icHistoricoFechaModificacion, fechaModificacion_)

                                                        catalogRow_.SetColumn(icHistoricoUsuario, partida_.Campo(CamposProducto.CP_LOGIN_USUARIO).Valor)

                                                        catalogRow_.SetColumn(icHistoricoOficina, partida_.Campo(CamposProducto.CP_ENVIRONMENT).ValorPresentacion)


                                                    End Sub)


                Next

                ccHistorialClasificacion.CatalogDataBinding()

                SetVars("_clasificacionArchivados", ccHistorialClasificacion.DataSource)

            End With

        End With

    End Sub
    'Sub ActualizaImagen()

    '    icMuestraProducto.Source = GetVars("PATH")

    '    Dim algo_ = fcImagenProducto

    '    Dim algo_2 = 0

    'End Sub

    'Sub PROBANDOO()

    '    Dim algo_ = fcImagenProducto

    '    Dim algo_2 = 0

    'End Sub

    Protected Sub btnArchivar_Click(sender As Object, e As EventArgs)

        icMotivo.Visible = True

        btn_ConfirmarArchivado.Visible = True

    End Sub

    Protected Sub btnRestaurar_Click(sender As Object, e As EventArgs)

        icMotivo.Visible = False

        icMotivo.Value = Nothing

        btn_ConfirmarArchivado.Visible = False

    End Sub




    'Protected Sub btn_Click(sender As Object, e As EventArgs)
    '    Dim listadonumeroParte = New List(Of String) From {"RXRXRX"}

    '    Dim IcontroladorProducto As IControladorProductos = New ControladorProductos
    '    ' Dim numerosParte = IcontroladorProducto.BuscarProductosPorNumeroParte(listadonumeroParte,
    '    ' "64e7ad27f544af8dfd407efd")

    '    Dim obj = New ObjectId("67f9ad0cc37e0a2d40e38762")

    '    Dim unicoPrid = IcontroladorProducto.ConsultarOne(obj)
    'End Sub

#End Region

End Class

'Public Class AuxiliarFraccionArancelaria
'    Property Id As ObjectId
'    Property Fraccion As String
'    Property DescripcionFraccion As String
'    Property Nico As String
'    Property DescripcionNico As String
'    Property UnidadMedida As String
'    Property UnidadMedidaCorto As String
'    Property CveUnidadMedida As String
'    Property FechaFinVigencia As String
'    Property FechaInicioVigencia As String
'    Property FechaPublicacion As String
'End Class
