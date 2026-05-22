Imports System.Net
Imports System.Net.Http
Imports System.Text
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Syn.Documento
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports Rec.Globals.Controllers
Imports Wma.Exceptions
Imports gsol
Imports System.Security.Policy
Imports System.Runtime.CompilerServices
Imports gsol.krom
Imports Rec.Globals.Utils
Imports Wma.Exceptions.TagWatcher
Imports Syn.Utils
Imports Syn.Nucleo.RecursosComercioExterior
Imports ConstructorExpedienteElectronico64.Syn.Documento
Imports System.Security.AccessControl
Imports AuxiliarDatosExpedienteElectronico64
Imports MongoDB.Bson.Serialization.Attributes
Imports EnvironmentsExpediente = Syn.CustomBrokers.Controllers.IControladorExpedienteElectronico.EnvironmentsExpediente
Imports MongoDB.Driver.Linq.Processors
Imports Syn.Documento.Componentes


Public Class ControladorExpedienteElectronico
    Implements IControladorExpedienteElectronico, ICloneable, IDisposable

#Region "Propiedades privadas"

    Private _controladorDocumentos As IControladorDocumento

    Private _controladorSecuencias As IControladorSecuencia

    Private _espacioTrabajo As IEspacioTrabajo

    Private _secuencia As ISecuencia

    Private _URL_API As String

    Private _APIKEY_API As String

    Private _endpoint As String

    Private _payload As String

    Private disposedValue As Boolean

    Private _documentoElectronico As DocumentoElectronico

    Private _datosExpedienteElectronico As AuxiliarDatosExpedienteElectronico

    Private _listaContructorExpedienteElectronico As List(Of ConstructorExpededienteElectronico)

    Public Property Estado As TagWatcher _
        Implements IControladorExpedienteElectronico.Estado

#End Region

#Region "Constructores"
    Sub New()
        Estado = New TagWatcher

        _URL_API = "https://6058cefac5b5.ngrok-free.app"

        _APIKEY_API = "fb5e505450571430ae11dfc9defcc6f18de3dddb17f485034cc944e77ff77038"

    End Sub

#End Region

#Region "Métodos privados"
    Private Function SerealizarPayload(ByVal payloads_ As List(Of DocumentoElectronicoApiStorage)) As String

        Dim request_ As New RootRequest With {.payloads = payloads_}

        ' Serializar a JSON
        Dim jsonBody_ As String = JsonConvert.SerializeObject(request_, Formatting.Indented)

        Try

            Dim obj As Object = JsonConvert.DeserializeObject(jsonBody_)

        Catch ex As JsonReaderException

            Return "bad"

        End Try

        Return jsonBody_

    End Function


    Protected Function GenerarSecuencia(ByVal tipoSecuencia_ As String) As Secuencia

        _controladorSecuencias = New ControladorSecuencia

        _secuencia = New Secuencia

        Estado = _controladorSecuencias.Generar(tipoSecuencia_, 1, 1, 1, 1)

        If Estado.Status = TypeStatus.Ok Then

            _secuencia = Estado.ObjectReturned

        End If

        Return _secuencia

    End Function

    Protected Function GenerarOperacionGenerica(ByVal estructura_ As AuxiliarDatosExpedienteElectronico) As OperacionGenerica

        _secuencia = New Secuencia

        _secuencia = GenerarSecuencia(SecuenciasComercioExterior.ExpedienteElectronico.ToString)

        _documentoElectronico = New ConstructorExpededienteElectronico

        With _documentoElectronico

            With .Seccion(SeccionesExpedienteElectronico.SEXPE1)
                .Campo(CamposExpedienteElectronico.CP_ID_CLIENTE).Valor = estructura_.idcliente
                .Campo(CamposExpedienteElectronico.CP_RAZON_SOCIAL_CLIENTE).Valor = estructura_.razonsocialCliente
                .Campo(CamposExpedienteElectronico.CP_TAXID_CLIENTE).Valor = estructura_.taxidCliente
                .Campo(CamposExpedienteElectronico.CP_ID_ENVIRONMENT).Valor = estructura_.idenvironment
                .Campo(CamposExpedienteElectronico.CP_ENVIRONMENT).Valor = estructura_.environment
                .Campo(CamposExpedienteElectronico.CP_BUSSINESS_UNID_ID).Valor = estructura_.bussinessUnitId
                .Campo(CamposExpedienteElectronico.CP_BUSSINESS_UNIT).Valor = estructura_.bussinessUnit
                .Campo(CamposExpedienteElectronico.CP_TOTAL_REFERENCIAS_CERRADAS).Valor = estructura_.totalReferenciasCerradas
                .Campo(CamposExpedienteElectronico.CP_TOTAL_REFERENCIAS_ABIERTAS).Valor = estructura_.totalReferenciasAbiertas
                .Campo(CamposExpedienteElectronico.CP_TOTAL_DOCUMENTOS_SIN_REFERENCIA).Valor = estructura_.totalDocumentosSinReferencia
                .Campo(CamposExpedienteElectronico.CP_DIGITAL_KEY_ID_EXPEDIENTE).Valor = estructura_.digitalkeyid
                .Campo(CamposExpedienteElectronico.CP_DIGITAL_KEY_EXPEDIENTE).Valor = estructura_.digitalkey
                .Campo(CamposExpedienteElectronico.CP_OWNER_ID_EXPEDIENTE).Valor = estructura_.ownerid
                .Campo(CamposExpedienteElectronico.CP_OWNER_USER_EMAIL_EXPEDIENTE).Valor = estructura_.owner_user
                .Campo(CamposExpedienteElectronico.CP_OWNER_NAME_EXPEDIENTE).Valor = estructura_.owner_name
                .Campo(CamposExpedienteElectronico.CP_FECHA_APERTURA_EXPEDIENTE).Valor = estructura_.fechaApertura
                .Campo(CamposExpedienteElectronico.CP_ULTIMA_ACTUALIZACION_EXPEDIENTE).Valor = estructura_.ultimaActualizacion
            End With

            Dim i_ = 0
            For Each item_ In estructura_.messages

                Dim partida_ = .Seccion(SeccionesExpedienteElectronico.SEXPE2).Partida(_documentoElectronico)

                With partida_
                    .Campo(CamposExpedienteElectronico.CP_SEC_MESSAGE).Valor = item_.sec
                    .Campo(CamposExpedienteElectronico.CP_TIPO_MESSAGE).Valor = item_.tipo
                    .Campo(CamposExpedienteElectronico.CP_MESSAGE).Valor = item_.message
                    .Campo(CamposExpedienteElectronico.CP_NIVEL_MESSAGE).Valor = item_.nivelMessage
                    .Campo(CamposExpedienteElectronico.CP_STATUS_MESSAGE).Valor = item_.statusMessage
                End With
                i_ += 1
            Next

            .UsuarioGenerador = estructura_.owner_name
            .Id = _secuencia._id.ToString
            .IdDocumento = _secuencia.sec
            .FolioDocumento = estructura_.taxidCliente
            .FolioOperacion = _secuencia.sec
            .TipoPropietario = SecuenciasComercioExterior.ExpedienteElectronico.ToString
            .NombrePropietario = estructura_.razonsocialCliente
            .IdPropietario = 1 ''Clave cliente
            .ObjectIdPropietario = estructura_.idcliente
            .TipoDocumentoElectronico = TiposDocumentoElectronico.ExpedienteElectronicoDocumentos

        End With

        Dim operacionGenerica_ As New OperacionGenerica(_documentoElectronico)

        With operacionGenerica_

            .FolioOperacion = _secuencia.sec

        End With

        Return operacionGenerica_

    End Function

    Protected Function GenerarExpedienteElectronicoCliente(ByVal estructuraExpedienteElectronico_ As AuxiliarDatosExpedienteElectronico) As TagWatcher

        ''Primero verificamos que el expediente exista y este activo
        Dim buscarExpedienteExistente_ = BuscarExpedienteExistente(estructuraExpedienteElectronico_.idcliente,
                                                                    estructuraExpedienteElectronico_.idenvironment)

        ''Antes vamos a generar uno primero, para realizar la busqueda y se genere la colección
        ''Generar desde 0
        ''Solo es un expediente con mucho documentossssss

        Dim operacionGenerica_ As OperacionGenerica = GenerarOperacionGenerica(estructuraExpedienteElectronico_)

        Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = _espacioTrabajo}

            Using entidadDatos_ As IEntidadDatos = New ConstructorExpededienteElectronico

                Dim collection_ = enlaceDatos_.GetMongoCollection(Of OperacionGenerica)((New ConstructorExpededienteElectronico).GetType.Name)

                Dim result_ = collection_.InsertOneAsync(operacionGenerica_)

                With Estado

                    If result_.Id <> Nothing Then

                        .ObjectReturned = result_

                        .SetOK()

                    Else

                        .SetOKBut(Me, "Documento no insertado")

                    End If

                End With

            End Using

        End Using

        Return Estado

    End Function

    Private Function BuscarExpedienteExistente(ByVal objectidCliente As ObjectId, ByVal environment_ As EnvironmentsExpediente) As AuxiliarDatosExpedienteElectronico
        _datosExpedienteElectronico = New AuxiliarDatosExpedienteElectronico


        Return _datosExpedienteElectronico
    End Function

    Private Function ObtenerTodosExpedientes(ByVal environment_ As EnvironmentsExpediente) As TagWatcher

        With Estado

            Try

                Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos(29) With {.EspacioTrabajo = _espacioTrabajo}

                    Using entidadDatos_ As IEntidadDatos = New ConstructorExpededienteElectronico

                        Dim collection_ = enlaceDatos_.GetMongoCollection(Of OperacionGenerica)((New ConstructorExpededienteElectronico).GetType.Name)

                        Dim result_ = collection_.Aggregate().Project(Function(x) _
                                                                          New With {
                                                                            Key .id = x.Id,
                                                                            Key .documentoElectronico = x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente
                                                                          }).ToList
                        If result_.Any() Then

                            _listaContructorExpedienteElectronico = New List(Of ConstructorExpededienteElectronico)

                            result_.AsEnumerable.ToList.ForEach(Sub(x)

                                                                    Dim auxConstructorEE_ As New ConstructorExpededienteElectronico(True, x.documentoElectronico)

                                                                    _listaContructorExpedienteElectronico.Add(auxConstructorEE_)
                                                                End Sub)

                            .ObjectReturned = _listaContructorExpedienteElectronico

                            .SetOK()

                        Else

                            .SetOKBut(Me, "No se encontraron resultados")

                        End If

                    End Using

                End Using

            Catch ex As Exception

                .SetError()

            End Try

        End With

        Return Estado

    End Function

    Private Function BuscarExpedientesPorCliente(ByVal idcliente_ As ObjectId, ByVal environment_ As EnvironmentsExpediente) As TagWatcher

        With Estado

            Try

                Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos(29) With {.EspacioTrabajo = _espacioTrabajo}

                    Using entidadDatos_ As IEntidadDatos = New ConstructorExpededienteElectronico

                        Dim collection_ = enlaceDatos_.GetMongoCollection(Of OperacionGenerica)((New ConstructorExpededienteElectronico).GetType.Name)

                        Dim result_ = collection_.Aggregate().
                                                Project(Function(x) _
                                                 New With {
                                                 Key .id_ = x.Id,
                                                 Key .documentoElectronico_ = x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente,
                                                 Key .seccionEncabezado_ = x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Encabezado")(0).Nodos(0).Nodos(0)
                                                }).
                                                Project(Function(y) New With {
                                                    Key .id = y.id_,
                                                    Key .documentoelectronico = y.documentoElectronico_,
                                                    Key .idcliente = DirectCast(y.seccionEncabezado_.Nodos(0), Campo).Valor
                                                }).
                                                Match(Function(z) z.idcliente.Equals(idcliente_)).ToList

                        ' 

                        If result_.Any() Then

                            '_listaContructorExpedienteElectronico = New List(Of ConstructorExpededienteElectronico)
                            Dim listaDatosExpedienteElectronico_ = New List(Of AuxiliarDatosExpedienteElectronico)

                            result_.AsEnumerable.ToList.ForEach(Sub(x)

                                                                    Dim ConstructorEE_ As New ConstructorExpededienteElectronico(True, x.documentoelectronico)

                                                                    Dim expediente_ As New AuxiliarDatosExpedienteElectronico

                                                                    If ConstructorEE_.Seccion(SeccionesExpedienteElectronico.SEXPE1) IsNot Nothing Then

                                                                        With ConstructorEE_.Seccion(SeccionesExpedienteElectronico.SEXPE1)

                                                                            expediente_._id = x.id
                                                                            expediente_.idcliente = .Campo(CamposExpedienteElectronico.CP_ID_CLIENTE).Valor
                                                                            expediente_.razonsocialCliente = .Campo(CamposExpedienteElectronico.CP_RAZON_SOCIAL_CLIENTE).Valor
                                                                            expediente_.taxidCliente = .Campo(CamposExpedienteElectronico.CP_TAXID_CLIENTE).Valor
                                                                            expediente_.idenvironment = .Campo(CamposExpedienteElectronico.CP_ID_ENVIRONMENT).Valor
                                                                            expediente_.environment = .Campo(CamposExpedienteElectronico.CP_ENVIRONMENT).Valor
                                                                            expediente_.bussinessUnitId = .Campo(CamposExpedienteElectronico.CP_BUSSINESS_UNID_ID).Valor
                                                                            expediente_.bussinessUnit = .Campo(CamposExpedienteElectronico.CP_BUSSINESS_UNIT).Valor
                                                                            expediente_.totalReferenciasCerradas = .Campo(CamposExpedienteElectronico.CP_TOTAL_REFERENCIAS_CERRADAS).Valor
                                                                            expediente_.totalReferenciasAbiertas = .Campo(CamposExpedienteElectronico.CP_TOTAL_REFERENCIAS_ABIERTAS).Valor
                                                                            expediente_.totalDocumentosSinReferencia = .Campo(CamposExpedienteElectronico.CP_TOTAL_DOCUMENTOS_SIN_REFERENCIA).Valor
                                                                            expediente_.digitalkeyid = .Campo(CamposExpedienteElectronico.CP_DIGITAL_KEY_ID_EXPEDIENTE).Valor
                                                                            expediente_.digitalkey = .Campo(CamposExpedienteElectronico.CP_DIGITAL_KEY_EXPEDIENTE).Valor
                                                                            expediente_.ownerid = .Campo(CamposExpedienteElectronico.CP_OWNER_ID_EXPEDIENTE).Valor
                                                                            expediente_.owner_user = .Campo(CamposExpedienteElectronico.CP_OWNER_USER_EMAIL_EXPEDIENTE).Valor
                                                                            expediente_.owner_name = .Campo(CamposExpedienteElectronico.CP_OWNER_NAME_EXPEDIENTE).Valor
                                                                            expediente_.fechaApertura = .Campo(CamposExpedienteElectronico.CP_FECHA_APERTURA_EXPEDIENTE).Valor
                                                                            expediente_.ultimaActualizacion = .Campo(CamposExpedienteElectronico.CP_ULTIMA_ACTUALIZACION_EXPEDIENTE).Valor

                                                                        End With

                                                                    End If
                                                                    listaDatosExpedienteElectronico_.Add(expediente_)

                                                                End Sub)

                            .ObjectReturned = listaDatosExpedienteElectronico_

                            .SetOK()

                        Else

                            .SetOKBut(Me, "No se encontraron resultados")

                        End If

                    End Using

                End Using

            Catch ex As Exception

                .SetError()

            End Try

        End With

        Return Estado

    End Function




#End Region
#Region "Métodos públicos"
    Public Shared Function ConvertirArchivosABase64(ByVal listiddocuments_ As List(Of ObjectId)) As List(Of String)
        ' doc es tu arreglo de bytes
        Dim controladorDocumentos_ = New ControladorDocumento()

        Dim listdocsBase64_ As New List(Of String)

        For Each docItem_ In listiddocuments_
            Dim doc As Byte() = controladorDocumentos_.GetDocument(docItem_).ObjectReturned
            ' Convertir a Base64
            Dim base64String_ As String = Convert.ToBase64String(doc)
            listdocsBase64_.Add(base64String_)
        Next

        Return listdocsBase64_

    End Function

    Public Async Function SubirDocumentosGCS(ByVal listadocumentos_ As List(Of DocumentoElectronicoApiStorage),
                                                       ByVal taxidcliente_ As String,
                                                       ByVal environment_ As Int16) As Task(Of TagWatcher) _
                                                       Implements IControladorExpedienteElectronico.SubirDocumentosGCS
        Try
            _payload = SerealizarPayload(listadocumentos_)

            _endpoint = $"{_URL_API}/api/datastorage/ka/1/t/v1/customers/{taxidcliente_}/environment/{environment_}/stage/docs/add"

            Using client As New HttpClient()

                client.DefaultRequestHeaders.Add("x-api-key", _APIKEY_API)

                Dim content As New StringContent(_payload, Encoding.UTF8, "application/json")

                Dim response As HttpResponseMessage = Await client.PostAsync(_endpoint, content)

                Dim result As String = Await response.Content.ReadAsStringAsync()

                Dim status As HttpStatusCode = response.StatusCode

                Dim statusInt As Integer = CType(status, Integer)

                ' Aquí lo conviertes directo a JObject
                Dim json As JObject = JObject.Parse(result)

                ' Guardas el JObject en tu TagWatcher
                If statusInt = 201 Then

                    ''MANDAR A GENERAR EL CONSTRUCTOR DE EXPEDIENTE ELECTRONICO CON ESA LISTA DE DOCUMENTOS
                    Try
                        ''PENDIENTE PORQUE ES EL EXPEDIENTE EN SI, HAY QUE VERIFICAR QUE EXISTA CON ESE CLIENTE Y OTROS DATOS
                        ''SINO QUE SE GENERE NUEVO ....
                        _datosExpedienteElectronico = New AuxiliarDatosExpedienteElectronico

                        Dim messages_ As New MessagesExpediente

                        With messages_
                            .sec = 1
                            .nivelMessage = "Informativo"
                            .message = $"Se han agregado {listadocumentos_.Count} documentos nuevos al expediente"
                            .statusMessage = 1
                        End With

                        With _datosExpedienteElectronico

                            .idcliente = ObjectId.Parse(listadocumentos_.Last.customerid)

                            .razonsocialCliente = listadocumentos_.Last.customer_name

                            .idenvironment = listadocumentos_.Last.environmentid

                            .environment = listadocumentos_.Last.environment

                            .bussinessUnitId = listadocumentos_.Last.business_unitid

                            .bussinessUnit = listadocumentos_.Last.business_unit
                            ''GENERAR LOS MENSAJES
                            .messages = New List(Of MessagesExpediente)

                            .messages.Add(messages_)

                            .totalReferenciasCerradas = 0

                            .totalReferenciasAbiertas = 0

                            .totalDocumentosSinReferencia = listadocumentos_.Count

                            .ownerid = ObjectId.Parse(listadocumentos_.Last.owner.userid)

                            .owner_user = listadocumentos_.Last.owner.user

                            .owner_name = listadocumentos_.Last.owner.name

                            .fechaApertura = DateTime.UtcNow

                            .ultimaActualizacion = DateTime.UtcNow

                        End With

                        Estado = GenerarExpedienteElectronicoCliente(_datosExpedienteElectronico)

                        If Estado.Status = TypeStatus.Ok Then

                            Estado.ObjectReturned = json

                            Estado.SetOK()

                        End If

                    Catch ex As Exception

                        Estado.SetError()

                    End Try

                Else

                    Estado.ObjectReturned = json

                    Estado.SetError()

                End If

            End Using

        Catch ex As Exception

            Estado.SetError()

        End Try

        Return Estado

    End Function

    Public Async Function ObtenerCatalogoTipoUso1Api() As Task(Of TagWatcher) _
        Implements IControladorExpedienteElectronico.ObtenerCatalogoTipoUso1Api
        Try
            _endpoint = $"{_URL_API}/api/datastorage/ka/1/t/v1/packages/usetype/TipoUso1/documents/info"

            Using client As New HttpClient()

                ' --- Cabecera personalizada ---
                client.DefaultRequestHeaders.Add("x-api-key", _APIKEY_API)
                ' --- Llamada GET ---

                Try
                    Dim response As HttpResponseMessage = Await client.GetAsync(_endpoint)

                    response.EnsureSuccessStatusCode() ' Lanza excepción si no es 2xx

                    Dim responseString As String = Await response.Content.ReadAsStringAsync()

                    ' Aquí lo conviertes directo a JObject
                    Dim json As JObject = JObject.Parse(responseString)

                    ' Guardas el JObject en tu TagWatcher
                    Estado.SetOK()

                    Estado.ObjectReturned = json

                Catch ex As Exception

                    Estado.ObjectReturned = $"Error inesperado: {ex.Message}"

                    Estado.SetError()

                End Try

            End Using

        Catch ex As Exception
            ' Cualquier otro error inesperado
            Estado.ObjectReturned = $"Error inesperado: {ex.Message}"

            Estado.SetError()
        End Try

        Return Estado
    End Function

    Public Sub ReiniciarControlador() _
        Implements IControladorExpedienteElectronico.ReiniciarControlador
        Throw New NotImplementedException()
    End Sub

    Public Function RecuperarLinksDescargaDocumento(storagepath_ As String) As TagWatcher _
        Implements IControladorExpedienteElectronico.RecuperarLinksDescargaDocumento
        Throw New NotImplementedException()
    End Function

    Public Function AbrirExpediente(datosExpedienteElectronico_ As AuxiliarDatosExpedienteElectronico,
                                    Optional conReferencia_ As Boolean = False) As TagWatcher _
                                    Implements IControladorExpedienteElectronico.AbrirExpediente
        With Estado

            If datosExpedienteElectronico_ IsNot Nothing Then

                'GenerarExpedienteElectronico(datosExpedienteElectronico_, conReferencia_)

            Else

                .SetOKBut(Me, "Documento electrónico requerido")

            End If

        End With
    End Function

    Public Function Clone() As Object _
        Implements ICloneable.Clone
        Throw New NotImplementedException()
    End Function

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: eliminar el estado administrado (objetos administrados)
            End If

            ' TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
            ' TODO: establecer los campos grandes como NULL
            disposedValue = True
        End If
    End Sub

    ' ' TODO: reemplazar el finalizador solo si "Dispose(disposing As Boolean)" tiene código para liberar los recursos no administrados
    ' Protected Overrides Sub Finalize()
    '     ' No cambie este código. Coloque el código de limpieza en el método "Dispose(disposing As Boolean)".
    '     Dispose(disposing:=False)
    '     MyBase.Finalize()
    ' End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' No cambie este código. Coloque el código de limpieza en el método "Dispose(disposing As Boolean)".
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub

    Public Function AsignarReferenciaDocumentosGCS(listaIdsDocumentos_ As List(Of ObjectId),
                                                   taxid_cliente As String,
                                                   referencia_ As String) As Task(Of TagWatcher) _
                                                   Implements IControladorExpedienteElectronico.AsignarReferenciaDocumentosGCS
        Throw New NotImplementedException()
    End Function

    Public Function DescargarDocumentosGCS() As Task(Of TagWatcher) _
        Implements IControladorExpedienteElectronico.DescargarDocumentosGCS
        Throw New NotImplementedException()
    End Function

    Public Function DescargarPaqueteGCS() As Task(Of TagWatcher) _
        Implements IControladorExpedienteElectronico.DescargarPaqueteGCS
        Throw New NotImplementedException()
    End Function

    Public Function AgregarReferenciasAExpediente(idexpediente_ As ObjectId,
                                                  listaReferencias_ As List(Of AuxliarDatosReferencia)) As TagWatcher _
                                                  Implements IControladorExpedienteElectronico.AgregarReferenciasAExpediente
        Throw New NotImplementedException()
    End Function

    Public Function CerrarExpediente(idexpediente_ As ObjectId,
                                     idcliente_ As ObjectId) As TagWatcher Implements IControladorExpedienteElectronico.CerrarExpediente
        Throw New NotImplementedException()
    End Function

    Public Function CerrarReferenciaEnExpediente(idexpediente_ As ObjectId,
                                                 idreferencia_ As ObjectId) As TagWatcher Implements IControladorExpedienteElectronico.CerrarReferenciaEnExpediente
        Throw New NotImplementedException()
    End Function

    Public Function ObtenerExpedientes(Optional environment_ As EnvironmentsExpediente = EnvironmentsExpediente.Veracruz) As TagWatcher _
        Implements IControladorExpedienteElectronico.ObtenerExpedientes

        With Estado

            Return ObtenerTodosExpedientes(environment_)

        End With

        Return Estado

    End Function

    Public Function ObtenerExpedientesPorCliente(idcliente_ As ObjectId, Optional environment_ As IControladorExpedienteElectronico.EnvironmentsExpediente = IControladorExpedienteElectronico.EnvironmentsExpediente.Veracruz) As TagWatcher _
        Implements IControladorExpedienteElectronico.ObtenerExpedientesPorCliente

        With Estado

            If Not idcliente_ = ObjectId.Empty Then

                Return BuscarExpedientesPorCliente(idcliente_, environment_)
            Else

                .SetOKBut(Me, "Id Cliente es requerido")

            End If

        End With

        Return Estado

    End Function

    Public Function ObtenerExpedientePorCliente(idexpediente_ As ObjectId, idcliente_ As ObjectId, Optional environment_ As IControladorExpedienteElectronico.EnvironmentsExpediente = IControladorExpedienteElectronico.EnvironmentsExpediente.Veracruz) As TagWatcher _
        Implements IControladorExpedienteElectronico.ObtenerExpedientePorCliente
        Throw New NotImplementedException()
    End Function

    Public Function ObtenerExpedientePorReferencia(idcliente_ As ObjectId, idreferencia_ As ObjectId, Optional environment_ As IControladorExpedienteElectronico.EnvironmentsExpediente = IControladorExpedienteElectronico.EnvironmentsExpediente.Veracruz) As TagWatcher _
        Implements IControladorExpedienteElectronico.ObtenerExpedientePorReferencia
        Throw New NotImplementedException()
    End Function

    Public Function ObtenerExpedientePorOwner(idcliente_ As ObjectId, idowner_ As ObjectId, Optional environment_ As IControladorExpedienteElectronico.EnvironmentsExpediente = IControladorExpedienteElectronico.EnvironmentsExpediente.Veracruz) As TagWatcher _
        Implements IControladorExpedienteElectronico.ObtenerExpedientePorOwner
        Throw New NotImplementedException()
    End Function

#End Region
End Class
