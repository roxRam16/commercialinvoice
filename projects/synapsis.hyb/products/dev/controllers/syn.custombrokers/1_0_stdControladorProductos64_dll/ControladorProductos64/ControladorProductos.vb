Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Syn.Documento
Imports Wma.Exceptions
Imports gsol
Imports Busqueda = Syn.CustomBrokers.Controllers.IControladorProductos.ListaBusquedas
Imports Disponibilidad = Syn.CustomBrokers.Controllers.IControladorProductos.Disponibilidades
Imports Modalidad = Syn.CustomBrokers.Controllers.IControladorProductos.Modalidades
Imports Estatus = Syn.CustomBrokers.Controllers.IControladorProductos.Estatus
Imports Syn.Documento.Componentes
Imports SeccionProducto = Syn.Nucleo.RecursosComercioExterior.SeccionesProducto
Imports CampoProducto = Syn.Nucleo.RecursosComercioExterior.CamposProducto
Imports Syn.Nucleo.RecursosComercioExterior
Imports System.Linq.Expressions
Imports MongoDB.Bson.Serialization.Attributes
Imports Syn.Utils

<DebuggerDisplay("{GetDebuggerDisplay(),nq}")>
Public Class ControladorProductos
    Implements IControladorProductos, ICloneable

#Region "PROPIEDADES PRIVADAS"

    Private _entorno As Int32

    Private disposedValue As Boolean

    Private _espacioTrabajo As IEspacioTrabajo

    Private _listaDataSource As List(Of SelectOption)

    Private _listaNumerosParte As List(Of Nodo)

    Private _auxiliarProducto As AuxiliarProducto

    Private _auxiliarListaProductos As List(Of AuxiliarProducto)

    Private _historico As HistoricoDescripciones

    Public _pageNumber As Int32

    Public _limitProductos As Int32

#End Region

#Region "PROPIEDADES PUBLICAS"
    Public Property ListaProductos As List(Of ConstructorProducto) _
        Implements IControladorProductos.ListaProductos

    Public Property Producto As Documento.ConstructorProducto _
        Implements IControladorProductos.Producto

    Public Property Estado As TagWatcher _
        Implements IControladorProductos.Estado

    Public Property ModalidadTrabajo As IControladorProductos.Modalidades _
        Implements IControladorProductos.ModalidadTrabajo

    Public Property ConservarProductos As Boolean _
        Implements IControladorProductos.ConservarProductos

    Public Property Entorno As Integer _
        Implements IControladorProductos.Entorno
        Get
            Return _entorno
        End Get
        Set(value As Integer)
            _entorno = value
            ReiniciarControlador(_entorno)
        End Set

    End Property
    Public Property DisponibilidadRecurso As IControladorProductos.Disponibilidades _
        Implements IControladorProductos.DisponibilidadRecurso

    Public Property EstatusRecurso As IControladorProductos.Estatus _
        Implements IControladorProductos.EstatusRecurso

    Public Property BusquedaRecurso As IControladorProductos.ListaBusquedas _
        Implements IControladorProductos.BusquedaRecurso
#End Region

#Region "CONSTRUCTORES"
    Sub New()

        Inicializa(Busqueda.SinDefinir,
                   Modalidad.Externo,
                   Disponibilidad.SinDefinir,
                   Estatus.SinDefinir,
                   conservarProductos_:=True)

    End Sub

    Sub New(ByVal busquedaRecurso_ As Busqueda)

        Inicializa(busquedaRecurso_,
                   Modalidad.Externo,
                   Disponibilidad.SinDefinir,
                   Estatus.SinDefinir,
                   conservarProductos_:=True)

    End Sub

    Sub New(ByVal busquedaRecurso_ As Busqueda,
            ByVal modalidadTrabajo_ As Modalidad,
            ByVal disponibilidadRecurso_ As Disponibilidad,
            ByVal estatusRecurso_ As Estatus,
            ByVal conservarProductos_ As Boolean)

        Inicializa(busquedaRecurso_,
                   modalidadTrabajo_,
                   disponibilidadRecurso_,
                   estatusRecurso_,
                   conservarProductos_)
    End Sub

    Private Sub Inicializa(ByVal busquedaRecurso_ As Busqueda,
                           ByVal modalidadTrabajo_ As Modalidad,
                           ByVal disponibilidadRecurso_ As Disponibilidad,
                           ByVal estatusRecurso_ As Estatus,
                           ByVal conservarProductos_ As Boolean)

        DisponibilidadRecurso = disponibilidadRecurso_

        EstatusRecurso = estatusRecurso_

        BusquedaRecurso = busquedaRecurso_

        ModalidadTrabajo = modalidadTrabajo_

        ConservarProductos = conservarProductos_

        ListaProductos = New List(Of ConstructorProducto)

        Producto = New ConstructorProducto

        Estado = New TagWatcher

        _entorno = 1 'OFICINA

        _pageNumber = 1

        _limitProductos = 10

    End Sub
#End Region

    Protected Overridable Sub Dispose(disposing As Boolean)

        If Not disposedValue Then

            If disposing Then
                ' TODO: eliminar el estado administrado (objetos administrados)
                _espacioTrabajo = Nothing

                disposedValue = Nothing

                _entorno = Nothing

                _limitProductos = Nothing

                _pageNumber = Nothing

                _limitProductos = Nothing

                _listaNumerosParte = Nothing

                _auxiliarProducto = Nothing

                _historico = Nothing

            End If
            ' TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
            ' TODO: establecer los campos grandes como NULL

            disposedValue = True

        End If
    End Sub

    Public Sub ReiniciarControlador(Optional entorno_ As Integer = 1) _
        Implements IControladorProductos.ReiniciarControlador

        Inicializa(Modalidad.Externo,
                   Disponibilidad.SinDefinir,
                   Estatus.SinDefinir,
                   Busqueda.SinDefinir,
                   conservarProductos_:=True)

        _pageNumber = 1

        _limitProductos = 10

    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' No cambie este código. Coloque el código de limpieza en el método "Dispose(disposing As Boolean)".

        Dispose(disposing:=True)

        GC.SuppressFinalize(Me)

    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Throw New NotImplementedException()
    End Function

#Region "CONSULTAS EXTERNAS"
    Private Function BuscarProductosPorNumeroParteCliente(ByVal numeroparte_ As String,
                                                           ByVal razonsocialcliente_ As String) As TagWatcher
        Try

            _auxiliarListaProductos = New List(Of AuxiliarProducto)

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos(27) With {.EspacioTrabajo = _espacioTrabajo}

                Dim collection_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)((New ConstructorProducto).GetType.Name)

                Dim filter_ = Builders(Of OperacionGenerica).Filter.Text(numeroparte_)

                Dim pipeline_ = collection_.Aggregate().
                                            Match(filter_).
                                            Project(Function(y) _
                                                        New With {
                                                        Key .id_ = y.Id,
                                                        Key .seccionEncabezado_ = y.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Encabezado")(0).Nodos(0).Nodos(1),
                                                        Key .cuerpoCliente_ = y.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Cuerpo")(1).Nodos(0).Nodos(0),
                                                        Key .cuerpoNumeroParte_ = y.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Cuerpo")(3).Nodos(0).Nodos(0)}).
                                            Project(Function(z) _
                                                        New With {
                                                        Key .idDocumento_ = z.id_,
                                                        Key .productoHabilitado_ = DirectCast(z.seccionEncabezado_.Nodos(0), Campo).Valor,
                                                        Key .cliente_ = DirectCast(z.cuerpoCliente_.Nodos(0).Nodos(0), Campo).Valor,
                                                        Key .numeroParte_ = DirectCast(z.cuerpoNumeroParte_.Nodos(1).Nodos(0), Campo).Valor,
                                                        Key .alias_ = DirectCast(z.cuerpoNumeroParte_.Nodos(4).Nodos(0), Campo).Valor, ''el valor del nodo debe ser 2
                                                        Key .idkrom_ = DirectCast(z.cuerpoNumeroParte_.Nodos(0).Nodos(0), Campo).Valor}).
                                                        Skip((_pageNumber - 1) * _limitProductos).
                                                        Limit(_limitProductos).ToList

                If pipeline_.Any() Then
                    pipeline_.AsEnumerable.ToList.ForEach(Sub(x)
                                                              If x.productoHabilitado_ And x.cliente_ = razonsocialcliente_ Then

                                                                  Dim auxProducto_ As New AuxiliarProducto _
                                                                  With {
                                                                        .id = x.idDocumento_,
                                                                        ._idKrom = x.idkrom_,
                                                                        ._alias = x.alias_,
                                                                        ._numeroParte = x.numeroParte_,
                                                                        ._estado = x.productoHabilitado_
                                                                  }

                                                                  _auxiliarListaProductos.Add(auxProducto_)

                                                              End If

                                                          End Sub)
                End If

                With Estado

                    If _auxiliarListaProductos.Count > 0 Then

                        .ObjectReturned = _auxiliarListaProductos

                        .SetOK()

                    Else

                        .SetOKBut(Me, "No se encontraron resultados")

                    End If

                End With

            End Using

        Catch ex As Exception

            Return Nothing

        End Try

        Return Estado

    End Function

    Private Function ObtenerProductosPorNumeroParte(ByVal listadoNumerosParte_ As List(Of String),
                                                ByVal idCliente_ As String) As TagWatcher

        With Estado

            _auxiliarListaProductos = New List(Of AuxiliarProducto)

            Try

                Using iEnlace_ As IEnlaceDatos = New EnlaceDatos(27) With {.EspacioTrabajo = _espacioTrabajo}

                    Dim collection_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)((New ConstructorProducto).GetType.Name)

                    Dim listaCondiciones_ As New List(Of FilterDefinition(Of OperacionGenerica))

                    For Each numeroparte_ In listadoNumerosParte_

                        Dim habilitado_ As FilterDefinition(Of OperacionGenerica) = Builders(Of OperacionGenerica).Filter.
                            Eq(Function(x) DirectCast(x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Encabezado")(0).Nodos(0).Nodos(1).Nodos(0), Campo).Valor, True)

                        Dim filtroNumeroParte_ As FilterDefinition(Of OperacionGenerica) = Builders(Of OperacionGenerica).Filter.
                            Eq(Function(x) DirectCast(x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Cuerpo")(3).Nodos(0).Nodos(0).Nodos(1).Nodos(0), Campo).Valor, numeroparte_)

                        Dim filtroCliente_ As FilterDefinition(Of OperacionGenerica) = Builders(Of OperacionGenerica).Filter.
                            Eq(Function(x) DirectCast(x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Cuerpo")(1).Nodos(0).Nodos(0).Nodos(0).Nodos(0), Campo).Valor, idCliente_)

                        Dim filtroCombinado_ As FilterDefinition(Of OperacionGenerica) = Builders(Of OperacionGenerica).Filter.And(filtroNumeroParte_, filtroCliente_, habilitado_)

                        listaCondiciones_.Add(filtroCombinado_)

                    Next

                    Dim pipeline_ = collection_.Aggregate().
                        Match(Builders(Of OperacionGenerica).Filter.Or(listaCondiciones_)).
                        Project(Function(y) _
                                New With {
                                Key .id_ = y.Id,
                                Key .seccionEncabezado_ = y.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Encabezado")(0).Nodos(0).Nodos(1),
                                Key .seccionCuerpo_ = y.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts("Cuerpo")}).
                       Project(Function(z) _
                                New With {
                                Key .idDocumento_ = z.id_,
                                Key .productoHabilitado_ = DirectCast(z.seccionEncabezado_.Nodos(0), Campo).Valor,
                                Key .estado_ = DirectCast(z.seccionCuerpo_(0).Nodos(0).Nodos(5).Nodos(0), Campo).ValorPresentacion,
                                Key .idcliente_ = DirectCast(z.seccionCuerpo_(1).Nodos(0).Nodos(0).Nodos(0).Nodos(0), Campo).Valor,
                                Key .razonsocialcliente_ = DirectCast(z.seccionCuerpo_(1).Nodos(0).Nodos(0).Nodos(0).Nodos(0), Campo).ValorPresentacion,
                                Key .razonsocialproveedor_ = DirectCast(z.seccionCuerpo_(1).Nodos(0).Nodos(0).Nodos(1).Nodos(0), Campo).Valor,
                                Key .numeroParte_ = DirectCast(z.seccionCuerpo_(1).Nodos(0).Nodos(0).Nodos(3).Nodos(0).Nodos(0).Nodos(1).Nodos(0), Campo).Valor,
                                Key .alias_ = DirectCast(z.seccionCuerpo_(1).Nodos(0).Nodos(0).Nodos(3).Nodos(0).Nodos(0).Nodos(2).Nodos(0), Campo).Valor,
                                Key .idkrom_ = DirectCast(z.seccionCuerpo_(1).Nodos(0).Nodos(0).Nodos(3).Nodos(0).Nodos(0).Nodos(0).Nodos(0), Campo).Valor,
                                Key .fraccionArancelaria_ = DirectCast(z.seccionCuerpo_(0).Nodos(0).Nodos(0).Nodos(0), Campo).Valor,
                                Key .descripcionFraccionArancelaria_ = DirectCast(z.seccionCuerpo_(0).Nodos(0).Nodos(1).Nodos(0), Campo).Valor,
                                Key .nico_ = DirectCast(z.seccionCuerpo_(0).Nodos(0).Nodos(2).Nodos(0), Campo).Valor,
                                Key .descripcionNico_ = DirectCast(z.seccionCuerpo_(0).Nodos(0).Nodos(3).Nodos(0), Campo).Valor,
                                Key .descripcion_ = DirectCast(z.seccionCuerpo_(1).Nodos(0).Nodos(0).Nodos(3).Nodos(0).Nodos(0).Nodos(4).Nodos(0), Campo).Valor}).ToList

                    If pipeline_.Any() Then

                        pipeline_.AsEnumerable.
                        ToList.
                        ForEach(Sub(w)
                                    Dim auxProducto_ As New AuxiliarProducto _
                                    With {.id = w.idDocumento_,
                                          ._idKrom = w.idkrom_,
                                          ._alias = w.alias_,
                                          ._descripcion = w.descripcion_,
                                          ._numeroParte = w.numeroParte_,
                                          ._fraccionArancelaria = w.fraccionArancelaria_,
                                          ._fraccionArancelariaPresentacion = $"{w.fraccionArancelaria_}-{w.descripcionFraccionArancelaria_}",
                                          ._nico = w.nico_,
                                          ._nicoPresentacion = $"{w.nico_}-{w.descripcionNico_}",
                                          ._idCliente = w.idcliente_,
                                          ._razonSocialCliente = w.razonsocialcliente_,
                                          ._razonSocialProveedor = w.razonsocialproveedor_,
                                          ._estado = w.productoHabilitado_,
                                          ._status = w.estado_}
                                    _auxiliarListaProductos.Add(auxProducto_)
                                End Sub)

                        .SetOK()
                        .ObjectReturned = _auxiliarListaProductos

                        'If _auxiliarListaProductos.Count > 0 Then



                        '    .SetOK()

                        'Else

                        '    .SetOKBut(Me, "No se encontraron coincidencias")

                        'End If

                    Else

                        .SetOKBut(Me, "No se encontraron coincidencias")

                    End If

                End Using

            Catch ex As Exception

                .SetError(Me, $"Ha ocurrido un error inesperado {ex}")

            End Try

        End With

        Return Estado

    End Function

    Private Function ObtenerProductosPorDescripcion(ByVal listadoDescripciones_ As List(Of String),
                                                    ByVal razonSocialCliente_ As String,
                                                    ByVal razonSocialProveedor_ As String) As TagWatcher

        With Estado

            _auxiliarListaProductos = New List(Of AuxiliarProducto)

            Try

                Using iEnlace_ As IEnlaceDatos = New EnlaceDatos(27) With {.EspacioTrabajo = _espacioTrabajo}

                    Dim collection_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)((New ConstructorProducto).GetType.Name)

                    Dim listaCondiciones_ As New List(Of FilterDefinition(Of OperacionGenerica))

                    For Each descripcion_ In listadoDescripciones_

                        Dim habilitado_ As FilterDefinition(Of OperacionGenerica) = Builders(Of OperacionGenerica).Filter.
                            Eq(Function(x) DirectCast(x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Encabezado")(0).Nodos(0).Nodos(1).Nodos(0), Campo).Valor, True)

                        Dim filtroDescripcion_ As FilterDefinition(Of OperacionGenerica) = Builders(Of OperacionGenerica).Filter.
                            Eq(Function(x) DirectCast(x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Cuerpo")(3).Nodos(0).Nodos(0).Nodos(4).Nodos(0), Campo).Valor, descripcion_)

                        Dim filtroCliente_ As FilterDefinition(Of OperacionGenerica) = Builders(Of OperacionGenerica).Filter.
                                Eq(Function(x) DirectCast(x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Cuerpo")(3).Nodos(0).Nodos(0).Nodos(8).Nodos(0), Campo).Valor, razonSocialCliente_)

                        Dim filtroProveedor_ As FilterDefinition(Of OperacionGenerica) = Builders(Of OperacionGenerica).Filter.
                                Eq(Function(x) DirectCast(x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Cuerpo")(3).Nodos(0).Nodos(0).Nodos(9).Nodos(0), Campo).Valor, razonSocialProveedor_)

                        Dim filtroCombinado_ As FilterDefinition(Of OperacionGenerica) = Builders(Of OperacionGenerica).Filter.And(filtroDescripcion_, filtroCliente_, filtroProveedor_, habilitado_)

                        listaCondiciones_.Add(filtroCombinado_)

                    Next

                    Dim pipeline_ = collection_.Aggregate().
                                                        Match(Builders(Of OperacionGenerica).Filter.Or(listaCondiciones_)).
                                                        Project(Function(y) _
                                                                    New With {
                                                                    Key .id_ = y.Id,
                                                                    Key .seccionEncabezado_ = y.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Encabezado")(0).Nodos(0).Nodos(1),
                                                                    Key .cuerpoEstatus_ = y.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts("Cuerpo")(0).Nodos(0),
                                                                    Key .cuerpoCliente_ = y.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Cuerpo")(1).Nodos(0).Nodos(0),
                                                                    Key .cuerpoFraccion_ = y.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts("Cuerpo")(2).Nodos(0).Nodos(0),
                                                                    Key .cuerpoNumeroParte_ = y.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Cuerpo")(3).Nodos(0).Nodos(0)}).
                                                        Project(Function(z) _
                                                                    New With {
                                                                    Key .idDocumento_ = z.id_,
                                                                    Key .productoHabilitado_ = DirectCast(z.seccionEncabezado_.Nodos(0), Campo).Valor,
                                                                    Key .estado_ = TryCast(z.cuerpoEstatus_.Nodos(5).Nodos(0), Campo).ValorPresentacion,
                                                                    Key .idcliente_ = DirectCast(z.cuerpoCliente_.Nodos(0).Nodos(0), Campo).Valor,
                                                                    Key .razonsocialcliente_ = DirectCast(z.cuerpoNumeroParte_.Nodos(8).Nodos(0), Campo).Valor,
                                                                    Key .razonsocialproveedor_ = DirectCast(z.cuerpoNumeroParte_.Nodos(9).Nodos(0), Campo).Valor,
                                                                    Key .numeroParte_ = DirectCast(z.cuerpoNumeroParte_.Nodos(1).Nodos(0), Campo).Valor,
                                                                    Key .alias_ = DirectCast(z.cuerpoNumeroParte_.Nodos(4).Nodos(0), Campo).Valor, ''el valor del nodo debe ser 2
                                                                    Key .idkrom_ = DirectCast(z.cuerpoNumeroParte_.Nodos(0).Nodos(0), Campo).Valor,
                                                                    Key .fraccionArancelaria_ = TryCast(z.cuerpoFraccion_.Nodos(0).Nodos(0), Campo).Valor,
                                                                    Key .nico_ = TryCast(z.cuerpoFraccion_.Nodos(1).Nodos(0), Campo).Valor,
                                                                    Key .descripcion_ = DirectCast(z.cuerpoNumeroParte_.Nodos(4).Nodos(0), Campo).Valor}).ToList

                    If pipeline_.Any() Then
                        pipeline_.AsEnumerable.
                        ToList.
                        ForEach(Sub(w)
                                    Dim auxProducto_ As New AuxiliarProducto _
                                    With {.id = w.idDocumento_,
                                          ._idKrom = w.idkrom_,
                                          ._alias = w.alias_,
                                          ._descripcion = w.descripcion_,
                                          ._numeroParte = w.numeroParte_,
                                          ._fraccionArancelaria = w.fraccionArancelaria_,
                                          ._nico = w.nico_,
                                          ._idCliente = w.idcliente_,
                                          ._razonSocialCliente = w.razonsocialcliente_,
                                          ._razonSocialProveedor = w.razonsocialproveedor_,
                                          ._estado = w.productoHabilitado_,
                                          ._status = w.estado_}

                                    _auxiliarListaProductos.Add(auxProducto_)

                                End Sub)

                        If _auxiliarListaProductos.Count > 0 Then

                            .ObjectReturned = _auxiliarListaProductos

                            .SetOK()

                        Else

                            .SetOKBut(Me, "No se encontraron coincidencias")

                        End If

                    Else

                        .SetOKBut(Me, "No se encontraron coincidencias")

                    End If

                End Using

            Catch ex As Exception

                .SetError(Me, "Ha ocurrido un error inesperado")

            End Try

        End With

        Return Estado

    End Function

    Private Function ObtenerProducto(ByVal ObjectIdProducto_ As ObjectId,
                                     Optional ByVal Recursivo_ As Boolean = False) As TagWatcher

        Try

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos(27) With {.EspacioTrabajo = _espacioTrabajo}

                Dim collection_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)((New ConstructorProducto).GetType.Name)

                Dim resultado = Nothing

                Dim pipeline_ = collection_.Aggregate().
                                          Match(Function(x) x.Id.Equals(ObjectIdProducto_)).
                                          Project(Function(x) _
                                                        New With {
                                                        Key .id_ = x.Id,
                                                        Key .cuerpoFraccion_ = x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts("Cuerpo")(0).Nodos(0),
                                                        Key .cuerpoNumeroParte_ = x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts("Cuerpo")(3).Nodos(0)
                                                        })

                If Not Recursivo_ Then

                    Dim result_ = pipeline_.Project(Function(y) _
                                                        New With {
                                                        Key .idDocumento_ = y.id_,
                                                        Key .estatus_ = TryCast(y.cuerpoFraccion_.Nodos(5).Nodos(0), Campo).ValorPresentacion,
                                                        Key .fraccionArancelaria_ = TryCast(y.cuerpoFraccion_.Nodos(0).Nodos(0), Campo).Valor,
                                                        Key .nico_ = TryCast(y.cuerpoFraccion_.Nodos(2).Nodos(0), Campo).Valor,
                                                        Key .descripcionFraccion_ = TryCast(y.cuerpoFraccion_.Nodos(1).Nodos(0), Campo).Valor,
                                                        Key .descripcionNico_ = TryCast(y.cuerpoFraccion_.Nodos(3).Nodos(0), Campo).Valor,
                                                        Key .cveunidadmedida_ = TryCast(y.cuerpoFraccion_.Nodos(8).Nodos(0), Campo).Valor,
                                                        Key .unidadmedidacorta_ = TryCast(y.cuerpoFraccion_.Nodos(10).Nodos(0), Campo).Valor,
                                                        Key .unidadmedidacompleta_ = TryCast(y.cuerpoFraccion_.Nodos(9).Nodos(0), Campo).Valor,
                                                        Key .numerosParte_ = y.cuerpoNumeroParte_}).ToList

                    resultado = result_

                Else

                    Dim result_ = pipeline_.Project(Function(y) _
                                                       New With {
                                                       Key .idDocumento_ = y.id_,
                                                       Key .estatus_ = TryCast(y.cuerpoFraccion_.Nodos(5).Nodos(0), Campo).ValorPresentacion,
                                                       Key .fraccionArancelaria_ = TryCast(y.cuerpoFraccion_.Nodos(0).Nodos(0), Campo).Valor,
                                                       Key .nico_ = TryCast(y.cuerpoFraccion_.Nodos(2).Nodos(0), Campo).Valor,
                                                       Key .descripcionFraccion_ = TryCast(y.cuerpoFraccion_.Nodos(1).Nodos(0), Campo).Valor,
                                                       Key .descripcionNico_ = TryCast(y.cuerpoFraccion_.Nodos(3).Nodos(0), Campo).Valor,
                                                       Key .numerosParte_ = y.cuerpoNumeroParte_}).ToList

                    resultado = result_

                End If

                With Estado

                    If resultado.Count > 0 Then

                        _listaNumerosParte = New List(Of Nodo)

                        _listaNumerosParte = TryCast(resultado(0).numerosParte_.Nodos, List(Of Nodo))

                        _auxiliarProducto = New AuxiliarProducto

                        With _auxiliarProducto

                            .id = resultado(0).idDocumento_

                            ._fraccionArancelaria = resultado(0).fraccionArancelaria_

                            ._fraccionArancelariaPresentacion = $"{resultado(0).fraccionArancelaria_} - {resultado(0).descripcionFraccion_}"

                            If Not Recursivo_ Then

                                ._cveunidadmedida = resultado(0).cveunidadmedida_

                                ._unidadmedidapresentacion = $"{resultado(0).cveunidadmedida_} - {resultado(0).unidadmedidacompleta_}"

                            End If

                            ._nico = resultado(0).nico_

                            ._nicoPresentacion = $"{resultado(0).nico_} - {resultado(0).descripcionNico_}"

                            ._status = resultado(0).estatus_

                            ._historicoDescripciones = New List(Of HistoricoDescripciones)

                        End With

                        _historico = New HistoricoDescripciones

                        For Each numerosParte_ In _listaNumerosParte

                            For Each item_ In numerosParte_.Nodos

                                Dim nodo = TryCast(item_.Nodos(0), Campo)

                                Select Case nodo.IDUnico

                                    Case CamposProducto.CP_IDKROM

                                        _historico._idKrom = nodo.Valor

                                    Case CamposProducto.CP_NUMERO_PARTE

                                        _historico._numeroParte = nodo.Valor

                                    Case CamposProducto.CP_ALIAS

                                        _historico._alias = nodo.Valor

                                    Case CamposProducto.CP_DESCRIPCION

                                        _historico._descripcion = nodo.Valor

                                End Select

                            Next

                            _auxiliarProducto._historicoDescripciones.Add(_historico)

                        Next

                        .ObjectReturned = _auxiliarProducto

                        .SetOK()

                    Else

                        .SetOKBut(Me, "No se encontraron registros")

                    End If

                End With

            End Using

        Catch ex As Exception

            ''Volvemos a llamar al método recursivo pero ahora ya sin cve valor para que muestre lo que tenga
            ObtenerProducto(ObjectIdProducto_, True)

        End Try

        Return Estado

    End Function

    Private Function ObtenerProductos(ByVal listaObjectId_ As List(Of ObjectId)) As TagWatcher

        Try

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos(27) With {.EspacioTrabajo = _espacioTrabajo}

                Dim collection_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)((New ConstructorProducto).GetType.Name)

                Dim result_ = collection_.Aggregate().
                                          Match(Function(x) listaObjectId_.Contains(x.Id)).
                                          Project(Function(y) New With {Key .documento_ = y.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente}).
                                          ToList

                If result_.Any() Then
                    result_.AsEnumerable.ToList.ForEach(Sub(x)
                                                            Dim producto As ConstructorProducto = DirectCast(x.documento_, ConstructorProducto)
                                                            If producto IsNot Nothing Then
                                                                ListaProductos.Add(producto)
                                                            End If
                                                        End Sub)
                End If

            End Using

            With Estado

                If ListaProductos.Count > 0 Then

                    .ObjectReturned = ListaProductos

                    .SetOK()

                Else

                    .SetOKBut(Me, "No se encontraron registros")

                End If

            End With

        Catch ex As Exception

            Return Nothing

        End Try

        Return Estado

    End Function

    Private Function GetDebuggerDisplay() As String
        Return ToString()
    End Function


#End Region

#Region "METODOS PUBLICOS"
    Public Function Consultar() As TagWatcher _
        Implements IControladorProductos.Consultar
        Throw New NotImplementedException()
    End Function

    Public Function Consultar(ByVal numeroparte_ As String,
                              ByVal razonsocialcliente_ As String) As TagWatcher _
                              Implements IControladorProductos.Consultar

        With Estado

            If _entorno > 0 Then

                If numeroparte_ <> Nothing And razonsocialcliente_ <> Nothing Then

                    BuscarProductosPorNumeroParteCliente(numeroparte_, razonsocialcliente_)

                Else

                    .SetError(Me, "Consulta no recibida.")

                End If

            Else

                .SetError(Me, "Entorno no puede ser 0")

            End If

        End With

        Return Estado

    End Function

    Public Function Consultar(listaIdProductos_ As List(Of ObjectId)) As TagWatcher _
        Implements IControladorProductos.Consultar

        With Estado

            If _entorno > 0 Then

                If listaIdProductos_.Count > 0 Then

                    ObtenerProductos(listaIdProductos_)

                Else

                    .SetError(Me, "Lista de objectids de productos vacía.")

                End If

            Else

                .SetError(Me, "Entorno no puede ser 0.")

            End If

        End With

        Return Estado

    End Function

    Public Function ConsultarOne(idProducto_ As ObjectId) As TagWatcher _
        Implements IControladorProductos.ConsultarOne

        With Estado

            If _entorno > 0 Then

                If Not idProducto_ = ObjectId.Empty Then

                    ObtenerProducto(idProducto_)

                Else

                    .SetError(Me, "Lista de objectids de productos vacía.")

                End If

            Else

                .SetError(Me, "Entorno no puede ser 0.")

            End If

        End With

        Return Estado

    End Function

    Public Function Archivar(listaIdProductos_ As List(Of ObjectId)) As TagWatcher _
        Implements IControladorProductos.Archivar
        Throw New NotImplementedException()
    End Function

    Public Function ArchivarOne(idProducto_ As ObjectId) As TagWatcher _
        Implements IControladorProductos.ArchivarOne
        Throw New NotImplementedException()
    End Function

    Private Function BuscarProductosPorNumeroParte(ByVal listadoNumerosParte_ As List(Of String),
                                ByVal idCliente_ As String) As TagWatcher _
                                Implements IControladorProductos.BuscarProductosPorNumeroParte

        With Estado

            If listadoNumerosParte_.Count > 0 And idCliente_ IsNot Nothing Then

                ObtenerProductosPorNumeroParte(listadoNumerosParte_, idCliente_)

            Else

                .SetError(Me, "Listado de números de parte y cliente son requeridos")

            End If

        End With

        Return Estado

    End Function

    Public Function BuscarProductosPorDescripcion(listadoDescripciones_ As List(Of String),
                                 idCliente_ As String, Optional razonSocialProveedor_ As String = Nothing) As TagWatcher _
                                 Implements IControladorProductos.BuscarProductosPorDescripcion

        With Estado

            If listadoDescripciones_.Count > 0 And idCliente_ IsNot Nothing Then

                ObtenerProductosPorDescripcion(listadoDescripciones_, idCliente_, razonSocialProveedor_)

            Else

                .SetError(Me, "Listado de números de parte y cliente son requeridos")

            End If

        End With

        Return Estado

    End Function



#End Region
End Class

Public Class AuxiliarProducto
    Property id As ObjectId

    <BsonIgnoreIfNull>
    Property _historicoDescripciones As List(Of HistoricoDescripciones)

    <BsonIgnoreIfNull>
    Property _idCliente As String

    <BsonIgnoreIfNull>
    Property _razonSocialCliente As String

    <BsonIgnoreIfNull>
    Property _razonSocialProveedor As String

    <BsonIgnoreIfNull>
    Property _idKrom As Integer

    <BsonIgnoreIfNull>
    Property _numeroParte As String

    <BsonIgnoreIfNull>
    Property _alias As String

    <BsonIgnoreIfNull>
    Property _descripcion As String

    <BsonIgnoreIfNull>
    Property _fraccionArancelaria As String

    <BsonIgnoreIfNull>
    Property _fraccionArancelariaPresentacion As String

    <BsonIgnoreIfNull>
    Property _cveunidadmedida As String

    <BsonIgnoreIfNull>
    Property _unidadmedidapresentacion As String

    <BsonIgnoreIfNull>
    Property _nico As String

    <BsonIgnoreIfNull>
    Property _nicoPresentacion As String

    <BsonIgnoreIfNull>
    Property _estado As Boolean

    <BsonIgnoreIfNull>
    Property _status As String

End Class

Public Class HistoricoDescripciones

    <BsonIgnoreIfNull>
    Property _idKrom As Integer

    <BsonIgnoreIfNull>
    Property _numeroParte As String

    <BsonIgnoreIfNull>
    Property _alias As String

    <BsonIgnoreIfNull>
    Property _descripcion As String

End Class

