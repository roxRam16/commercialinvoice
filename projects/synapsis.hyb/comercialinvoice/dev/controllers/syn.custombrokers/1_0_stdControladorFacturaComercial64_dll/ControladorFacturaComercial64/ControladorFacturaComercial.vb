Imports System.IO
Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Wma.Exceptions
Imports Syn.CustomBrokers.Controllers.IControladorFacturaComercial.Modalidades
Imports Rec.Globals.Controllers
'Imports Syn.Utils
Imports gsol
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Nucleo.RecursosComercioExterior.CamposFacturaComercial
Imports Syn.Nucleo.RecursosComercioExterior.SeccionesFacturaComercial
Imports Syn.Nucleo
Imports MongoDB.Bson.Serialization.Attributes
Imports Rec.Globals.Utils
Imports Wma.Exceptions.TagWatcher
Imports System.Xml.Serialization
Imports Syn.CustomBrokers.Controllers.Digitalization
Imports System.Xml
Imports Rec.Globals
Imports System.Runtime.Caching
Imports Syn.Utils


<DebuggerDisplay("{GetDebuggerDisplay(),nq}")>
Public Class ControladorFacturaComercial
    Implements IControladorFacturaComercial, ICloneable, IDisposable

#Region "ATRIBUTOS"

    Private disposedValue As Boolean

    Private _listaFacturas As List(Of ConstructorFacturaComercial)

    Private _listaObjetos As List(Of ObjectId)

    Private _listaFolios As List(Of String)

    Private _listadoCampos As Dictionary(Of ObjectId, Object)

    Private _listadoValoresObject As Dictionary(Of ObjectId, List(Of Nodo))

    Private _listadoValoresString As Dictionary(Of String, List(Of Nodo))

    Private _diccionarioValores As Dictionary(Of ObjectId, Dictionary(Of String, Object))

    Private _diccionarioValoresString As Dictionary(Of String, Dictionary(Of String, List(Of Nodo)))

    Private _listado As List(Of Object)

    Private _listaFactoresMoneda As Dictionary(Of String, Double)

    Private _totalValorIncrementables As Double = 0

    Private _totalValorFletes As Double = 0

    Private _totalValorSeguros As Double = 0

    Private _totalValorEmbalajes As Double = 0

    Private _totalValorOtrosIncrementables As Double = 0

    Private _totalValorDescuentos As Double = 0

    Private _iControladorMonedas As IControladorMonedas

    Private _rOrganismo As Utils.Organismo

    Private _numeroAcuseValor As String

    Private _fechaAcuseValor? As Date

    Private _entorno As Int32

    Private _espacioTrabajo As IEspacioTrabajo

    Private _listaVehiculoFacturasComerciales As List(Of ICommercialInvoiceCustomsDocument)

    Private _controladorSecuencias As IControladorSecuencia

    Private _secuencia As ISecuencia

    Private _documentoElectronicoSubdivisionFactura As ConstructorSubdivisionFacturaComercial

    Private _operacionGenerica As OperacionGenerica

    Private _documentoElectronicoFacturacomercial As ConstructorFacturaComercial

    Private _objectidacusevalor As ObjectId

    Private _estructuraSubdivisionFacturaComercial As SubdivisionFacturaComercial

    Private _facturaComercialcfdi As CommercialInvoiceCFDI

    Private _facturaComercial As ICommercialInvoiceCustomsDocument

    Private _auxiliarAcuseValor As DatosAcuseValor

    Private _documentoElectronico As DocumentoElectronico

    Private _controladorCliente As IControladorClientes

    Private _estructuraCliente As EstructuraCliente

    Private _controladorMonedas As IControladorMonedas

    Private _controladorPais As ControladorPaises

    Private _controladorProveedorOperativo As CtrlProveedoresOperativos

    Private _facturaSubdividible As FacturaSubdividible

    Private _environmentOnline As Int32 = 0

    Private ReadOnly _cache As ObjectCache = MemoryCache.Default

    Private _listaFacturasCustom As List(Of ICommercialInvoiceCustomsDocument)

    Private _listaFacturasGeneric As List(Of ICommercialInvoiceGeneric)


#End Region


#Region "PROPIEDADES PÚBLICAS"
    Public Property FacturasComerciales As List(Of ConstructorFacturaComercial) _
        Implements IControladorFacturaComercial.FacturasComerciales

    Public ReadOnly Property Documento As DocumentoElectronico _
        Implements IControladorFacturaComercial.Documento

    Public ReadOnly Property Documentos As List(Of DocumentoElectronico) _
        Implements IControladorFacturaComercial.Documentos

    Public Property Estado As TagWatcher _
        Implements IControladorFacturaComercial.Estado

    Public Property ModalidadTrabajo As IControladorFacturaComercial.Modalidades _
        Implements IControladorFacturaComercial.ModalidadTrabajo

    Public Property ConservarFacturas As Boolean _
        Implements IControladorFacturaComercial.ConservarFacturas

    Public Property Entorno As Integer _
        Implements IControladorFacturaComercial.Entorno

        Get

            Return _entorno

        End Get

        Set(value As Integer)

            _entorno = value

            ReiniciarControlador(_entorno)

        End Set

    End Property

    Public Property DisponibilidadRecurso As IControladorFacturaComercial.Disponibilidades _
        Implements IControladorFacturaComercial.DisponibilidadRecurso

    Public ReadOnly Property FactorConfiabilidadIA As Double _
        Implements IControladorFacturaComercial.FactorConfiabilidadIA

    Public Property TipoOperacion As IControladorFacturaComercial.TipoOperaciones _
        Implements IControladorFacturaComercial.TipoOperacion

    Public Property EnvironmentOnline As Int32 _
        Implements IControladorFacturaComercial.EnvironmentOnline

        Get

            Return _environmentOnline

        End Get

        Set(value As Int32)

            _environmentOnline = value

        End Set

    End Property




#End Region


#Region "CONSTRUCTORES"
    Sub New(Optional ByVal tipoOperacion_ As IControladorFacturaComercial.TipoOperaciones = IControladorFacturaComercial.TipoOperaciones.Importacion)

        _espacioTrabajo = Nothing

        Inicializa(factura_:=Nothing,
                   modalidadTrabajo_:=IControladorFacturaComercial.Modalidades.Externo,
                   conservarFacturas_:=True,
                   entorno_:=1,
                   disponibilidadRecurso_:=IControladorFacturaComercial.Disponibilidades.SinDefinir,
                   tipoOperacion_:=tipoOperacion_)
    End Sub

    Sub New(ByVal tipoOperacion_ As IControladorFacturaComercial.TipoOperaciones, ByVal environmentOnline_ As Int32)

        '_espacioTrabajo = Nothing

        Inicializa(factura_:=Nothing,
                   modalidadTrabajo_:=IControladorFacturaComercial.Modalidades.Externo,
                   conservarFacturas_:=True,
                   entorno_:=1,
                   disponibilidadRecurso_:=IControladorFacturaComercial.Disponibilidades.SinDefinir,
                   tipoOperacion_:=tipoOperacion_,
                   environmentOnline_:=environmentOnline_)
    End Sub

    Sub New(ByVal entorno_ As Integer,
            ByVal conservarFacturas_ As Boolean,
            Optional ByVal tipoOperacion_ As _
            IControladorFacturaComercial.TipoOperaciones = IControladorFacturaComercial.TipoOperaciones.Importacion)

        _FacturasComerciales = New List(Of ConstructorFacturaComercial)

        Inicializa(Nothing,
               Externo,
               conservarFacturas_,
               entorno_,
               IControladorFacturaComercial.Disponibilidades.SinDefinir,
               tipoOperacion_)

    End Sub

    Sub New(ByVal facturasComerciales_ As ConstructorFacturaComercial,
            ByVal entorno_ As Integer,
            Optional ByVal tipoOperacion_ As _
            IControladorFacturaComercial.TipoOperaciones = IControladorFacturaComercial.TipoOperaciones.Importacion)

        _FacturasComerciales = New List(Of ConstructorFacturaComercial) From {facturasComerciales_}

        Inicializa(Nothing,
               Interno,
               True,
               entorno_,
               IControladorFacturaComercial.Disponibilidades.SinDefinir,
               tipoOperacion_)

    End Sub

    Sub New(ByVal facturasComerciales_ As List(Of ConstructorFacturaComercial),
            ByVal entorno_ As Integer,
            Optional ByVal tipoOperacion_ As _
            IControladorFacturaComercial.TipoOperaciones = IControladorFacturaComercial.TipoOperaciones.Importacion)

        _FacturasComerciales = facturasComerciales_

        Inicializa(Nothing,
               Interno,
               True,
               entorno_,
               IControladorFacturaComercial.Disponibilidades.SinDefinir,
               tipoOperacion_)

    End Sub

    Sub New(ByVal idFactura_ As ObjectId,
            ByVal modalidadTrabajo_ As IControladorFacturaComercial.Modalidades,
            ByVal entorno_ As Integer,
            Optional conservarFacturas_ As Boolean = True,
            Optional ByVal tipoOperacion_ As _
            IControladorFacturaComercial.TipoOperaciones = IControladorFacturaComercial.TipoOperaciones.Importacion)

        Inicializa(idFactura_,
                   modalidadTrabajo_,
                   conservarFacturas_,
                   entorno_,
                   IControladorFacturaComercial.Disponibilidades.SinDefinir,
                   tipoOperacion_)

    End Sub

    Sub New(ByVal folioFactura_ As String,
            ByVal modalidadTrabajo_ As IControladorFacturaComercial.Modalidades,
            ByVal entorno_ As Integer,
            Optional conservarFacturas_ As Boolean = True,
            Optional ByVal tipoOperacion_ As _
            IControladorFacturaComercial.TipoOperaciones = IControladorFacturaComercial.TipoOperaciones.Importacion)

        Inicializa(folioFactura_,
                   modalidadTrabajo_,
                   conservarFacturas_,
                   entorno_,
                   IControladorFacturaComercial.Disponibilidades.SinDefinir,
                   tipoOperacion_)

    End Sub

    Sub New(ByVal idsFacturas_ As List(Of ObjectId),
            ByVal modalidadTrabajo_ As IControladorFacturaComercial.Modalidades,
            ByVal entorno_ As Integer,
            Optional conservarFacturas_ As Boolean = True,
            Optional ByVal tipoOperacion_ As _
            IControladorFacturaComercial.TipoOperaciones = IControladorFacturaComercial.TipoOperaciones.Importacion)

        Inicializa(idsFacturas_,
                   modalidadTrabajo_,
                   conservarFacturas_,
                   entorno_,
                   IControladorFacturaComercial.Disponibilidades.SinDefinir,
                   tipoOperacion_)

    End Sub

    Sub New(ByVal foliosFacturas_ As List(Of String),
            ByVal modalidadTrabajo_ As IControladorFacturaComercial.Modalidades,
            ByVal entorno_ As Integer,
            Optional conservarFacturas_ As Boolean = True,
            Optional ByVal tipoOperacion_ As _
            IControladorFacturaComercial.TipoOperaciones = IControladorFacturaComercial.TipoOperaciones.Importacion)

        Inicializa(foliosFacturas_,
                   modalidadTrabajo_,
                   conservarFacturas_,
                   entorno_,
                   IControladorFacturaComercial.Disponibilidades.SinDefinir,
                   tipoOperacion_)

    End Sub

    Public Sub Inicializa(ByVal factura_ As Object,
                          ByVal modalidadTrabajo_ As IControladorFacturaComercial.Modalidades,
                          ByVal conservarFacturas_ As Boolean,
                          ByVal entorno_ As Integer,
                          ByVal disponibilidadRecurso_ As IControladorFacturaComercial.Disponibilidades,
                          ByVal tipoOperacion_ As IControladorFacturaComercial.TipoOperaciones,
                          Optional ByVal environmentOnline_ As Int32 = 1)

        _Estado = New TagWatcher

        _ModalidadTrabajo = modalidadTrabajo_

        _ConservarFacturas = conservarFacturas_

        _entorno = entorno_

        TipoOperacion = tipoOperacion_

        _DisponibilidadRecurso = disponibilidadRecurso_

        If factura_ IsNot Nothing Then

            _Estado = ListaFacturas(factura_)

            If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                _FacturasComerciales = _Estado.ObjectReturned

            Else

                _FacturasComerciales = Nothing

            End If

        End If


        If _FacturasComerciales Is Nothing Then

            _FacturasComerciales = New List(Of ConstructorFacturaComercial)

        End If

        ' EnvironmentOnline = environmentOnline_

        _environmentOnline = environmentOnline_

    End Sub

#End Region


#Region "LIMPIEZA DE CONTROLADOR"

    Public Sub ReiniciarControlador(Optional ByVal entorno_ As Integer = 1) _
        Implements IControladorFacturaComercial.ReiniciarControlador

        _FacturasComerciales = New List(Of ConstructorFacturaComercial)

        Inicializa(Nothing,
               IControladorFacturaComercial.Modalidades.Externo,
               True,
               entorno_,
               IControladorFacturaComercial.Disponibilidades.SinDefinir,
               IControladorFacturaComercial.TipoOperaciones.Importacion)


        _listaFacturas = New List(Of ConstructorFacturaComercial)

        _listaObjetos = New List(Of ObjectId)

        _listaFolios = New List(Of String)

        _listadoCampos = New Dictionary(Of ObjectId, Object)

        _listadoValoresObject = New Dictionary(Of ObjectId, List(Of Nodo))

        _listadoValoresString = New Dictionary(Of String, List(Of Nodo))

        _diccionarioValores = New Dictionary(Of ObjectId, Dictionary(Of String, Object))

        _diccionarioValoresString = New Dictionary(Of String, Dictionary(Of String, List(Of Nodo)))

        _listado = New List(Of Object)

        _listaFactoresMoneda = New Dictionary(Of String, Double)

        _totalValorIncrementables = 0

        _totalValorFletes = 0

        _totalValorSeguros = 0

        _totalValorEmbalajes = 0

        _totalValorOtrosIncrementables = 0

        _totalValorDescuentos = 0

        _iControladorMonedas = New ControladorMonedas

        _rOrganismo = New Organismo

        _numeroAcuseValor = Nothing

        _fechaAcuseValor = Nothing

        _documentoElectronicoSubdivisionFactura = Nothing

        _documentoElectronicoFacturacomercial = Nothing

    End Sub

    Public Sub CargaFacturas(documentoDigital_ As MemoryStream) _
        Implements IControladorFacturaComercial.CargaFacturas

        _Estado.
            SetError(Me, "Función aún no implementada")

    End Sub

    Public Sub CargaFacturas(documentoDigital_ As List(Of MemoryStream)) _
        Implements IControladorFacturaComercial.CargaFacturas

        _Estado.
            SetError(Me, "Función aún no implementada")

    End Sub

    Protected Overridable Sub Dispose(disposing As Boolean)

        If Not disposedValue Then

            If disposing Then
                ' TODO: eliminar el estado administrado (objetos administrados)
                _listaFacturas = Nothing

                _listaObjetos = Nothing

                _listaFolios = Nothing

                _listadoCampos = Nothing

                _listadoValoresObject = Nothing

                _listadoValoresString = Nothing

                _diccionarioValores = Nothing

                _diccionarioValoresString = Nothing

                _listado = Nothing

                _listaFactoresMoneda = Nothing

                _totalValorIncrementables = Nothing

                _totalValorFletes = Nothing

                _totalValorSeguros = Nothing

                _totalValorEmbalajes = Nothing

                _totalValorOtrosIncrementables = Nothing

                _totalValorDescuentos = Nothing

                _iControladorMonedas = Nothing

                _rOrganismo = Nothing

                _numeroAcuseValor = Nothing

                _fechaAcuseValor = Nothing

                _entorno = Nothing

                _espacioTrabajo = Nothing

                _documentoElectronicoSubdivisionFactura = Nothing

                _documentoElectronicoFacturacomercial = Nothing

                _facturaSubdividible = Nothing

                _environmentOnline = Nothing

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
#End Region


#Region "ACCIONES PRIVADAS"

#Region "ACUSE DE VALOR"

    Private Function ObtenerAcuseValor(idFactura_ As ObjectId) As TagWatcher

        With _Estado

            Try
                Using iEnlace_ As IEnlaceDatos = New EnlaceDatos(21)

                    iEnlace_.EnvironmentOnline = _environmentOnline

                    Dim operationsDB_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)((New ConstructorFacturaComercial).GetType.Name)

                    Dim filtroFactura_ As FilterDefinition(Of OperacionGenerica) =
                  Builders(Of OperacionGenerica).Filter.Eq(Of ObjectId)("_id", idFactura_)

                    Dim filtroFacturaPublicada_ As FilterDefinition(Of OperacionGenerica) =
                          Builders(Of OperacionGenerica).Filter.Eq(Of Boolean)("Publicado", True)

                    Dim filtroFacturaFirmada_ As FilterDefinition(Of OperacionGenerica) =
                          Builders(Of OperacionGenerica).Filter.Ne(Of String)("FirmaElectronica", "")

                    Dim filtroCombinado_ As FilterDefinition(Of OperacionGenerica) =
                    Builders(Of OperacionGenerica).Filter.And(
                        filtroFactura_,
                        filtroFacturaFirmada_,
                        filtroFacturaPublicada_)

                    Dim result_ = operationsDB_.Aggregate().
                    Match(filtroCombinado_).
                    Project(Function(y) New With {
                        Key .id_ = y.Id,
                        Key .encabezado_ = y.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Encabezado")(0)}).ToList()

                    If result_.Count > 0 Then

                        _auxiliarAcuseValor = New DatosAcuseValor

                        Dim tipooperacionresult_ = TipoOperacion
                        Dim tipooperacionText_ = IIf(TipoOperacion = 1, "Importación", "Exportación")

                        result_.ToList().ForEach(Sub(y)

                                                     tipooperacionresult_ = y.encabezado_.Campo(CamposFacturaComercial.CP_TIPO_OPERACION).Valor

                                                     tipooperacionText_ = y.encabezado_.Campo(CamposFacturaComercial.CP_TIPO_OPERACION).ValorPresentacion

                                                     If tipooperacionresult_ = TipoOperacion Then

                                                         If y.encabezado_.Campo(CamposAcuseValor.CA_NUMERO_ACUSEVALOR) IsNot Nothing AndAlso y.encabezado_.Campo(CamposAcuseValor.CA_NUMERO_ACUSEVALOR).Valor <> "" Then

                                                             With _auxiliarAcuseValor
                                                                 ._idFactura = y.id_
                                                                 ._idAcuseValor = y.encabezado_.Campo(CamposAcuseValor.CP_ID_ACUSEVALOR).Valor
                                                                 ._acuseValor = y.encabezado_.Campo(CamposAcuseValor.CA_NUMERO_ACUSEVALOR).Valor
                                                                 ._fechaAcuseValor = y.encabezado_.Campo(CamposAcuseValor.CA_FECHA_ACUSEVALOR).Valor
                                                                 ._facturaSubdividida = y.encabezado_.Campo(CamposFacturaComercial.CA_APLICA_SUBDIVISION).Valor
                                                                 ._numeroFactura = y.encabezado_.Campo(CamposFacturaComercial.CA_NUMERO_FACTURA).Valor
                                                                 ._tipoOperacion = y.encabezado_.Campo(CamposFacturaComercial.CP_TIPO_OPERACION).ValorPresentacion
                                                                 ._facturaenajenada = y.encabezado_.Campo(CamposFacturaComercial.CP_APLICA_ENAJENACION).Valor
                                                             End With

                                                         Else

                                                             _auxiliarAcuseValor = Nothing

                                                         End If

                                                     Else

                                                         _auxiliarAcuseValor = Nothing

                                                     End If

                                                 End Sub)

                        If tipooperacionresult_ = TipoOperacion Then

                            If _auxiliarAcuseValor IsNot Nothing Then

                                .ObjectReturned = _auxiliarAcuseValor

                                .SetOK()

                            Else
                                .ObjectReturned = $"Factura solicitada no contiene datos de acuse de valor"

                                .SetOKBut(Me, $"Factura solicitada no contiene datos de acuse de valor")

                            End If

                        Else

                            .ObjectReturned = $"Factura solicitada no encontrada en operación de {TipoOperacion}"

                            .SetOKBut(Me, $"Factura solicitada no encontrada en en operación de {TipoOperacion}")

                        End If


                    Else

                        .ObjectReturned = $"Factura solicitada no existe en los registros actuales"

                        .SetOKBut(Me, $"Factura solicitada no existe en los registros actuales")

                    End If

                End Using

            Catch ex As Exception

                .ObjectReturned = $"Ha ocurrido una exception -  {ex}"

                .SetError($"Ha ocurrido una exception -  {ex}")

            End Try

        End With

        Return Estado

    End Function

#End Region

#Region "FACTURAS"
    Private Sub FacturasEnMemoria()

        If _FacturasComerciales IsNot Nothing Then

            If _FacturasComerciales.Count > 0 Then

                If ConservarFacturas = False Then

                    _FacturasComerciales = Nothing

                    _FacturasComerciales = New List(Of ConstructorFacturaComercial)

                End If
            Else

                _FacturasComerciales = New List(Of ConstructorFacturaComercial)

            End If

        Else

            _FacturasComerciales = New List(Of ConstructorFacturaComercial)

        End If

    End Sub

    Private Function ConsultaExterior(ByVal listaObjectidFacturas_ As List(Of ObjectId),
                                      Optional ByVal facturasPublicadas_ As Boolean = True) As TagWatcher

        'Con esto reseteamos la variable de Factura o conservamos las que existan en memoria
        FacturasEnMemoria()

        With _Estado

            Dim facturasExistentes_ = False

            If _FacturasComerciales.Count > 0 Then

                facturasExistentes_ = True

            End If

            Try
                Using iEnlace_ As IEnlaceDatos = New EnlaceDatos(21)

                    iEnlace_.EnvironmentOnline = _environmentOnline

                    Dim operationsDB_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)((New ConstructorFacturaComercial).GetType.Name)

                    operationsDB_.Aggregate().Match(Function(x) listaObjectidFacturas_.Contains(x.Id)).
                       ToList().
                       ForEach(Sub(item)

                                   If facturasPublicadas_ Then

                                       If item.Publicado Then

                                           If item.FirmaElectronica IsNot Nothing AndAlso item.FirmaElectronica <> "" Then

                                               item.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Id = item.Id.ToString

                                               _FacturasComerciales.Add(New ConstructorFacturaComercial(True, item.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente))

                                           End If

                                       End If

                                   Else

                                       item.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Id = item.Id.ToString

                                       _FacturasComerciales.Add(New ConstructorFacturaComercial(True, item.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente))

                                   End If

                               End Sub)

                    If facturasExistentes_ Then

                        If _FacturasComerciales.Count > 0 Then

                            .ObjectReturned = _FacturasComerciales

                            .SetOK()
                        Else

                            .ObjectReturned = _FacturasComerciales

                            .SetOKBut(Me, "Facturas no disponibles para agregar a la lista actual de facturas.")

                        End If

                    Else

                        If _FacturasComerciales.Count > 0 Then

                            .ObjectReturned = _FacturasComerciales

                            .SetOK()
                        Else

                            .ObjectReturned = Nothing

                            .SetOKBut(Me, "Facturas no disponibles en los registros con los datos proporcionados")

                        End If

                    End If

                End Using

            Catch ex As Exception

                .ObjectReturned = Nothing

                .SetOKBut(Me, $"Error interno - {ex}")

            End Try

        End With

        Return _Estado

    End Function

    Private Function ConsultaExterior(ByVal listaFacturas_ As List(Of String),
                                      Optional ByVal facturasPublicadas_ As Boolean = True) As TagWatcher

        FacturasEnMemoria()

        With _Estado

            Dim facturasExistentes_ = False

            If _FacturasComerciales.Count > 0 Then

                facturasExistentes_ = True

            End If

            Try
                Using iEnlace_ As IEnlaceDatos = New EnlaceDatos(21)

                    iEnlace_.EnvironmentOnline = _environmentOnline

                    Dim operationsDB_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)((New ConstructorFacturaComercial).GetType.Name)

                    operationsDB_.Aggregate().Match(Function(a) listaFacturas_.
                                                            Contains(a.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.FolioDocumento)).
                                                            ToList().
                                                            ForEach(Sub(item)

                                                                        If facturasPublicadas_ Then

                                                                            If item.Publicado Then

                                                                                If item.FirmaElectronica IsNot Nothing AndAlso item.FirmaElectronica <> "" Then

                                                                                    item.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Id = item.Id.ToString

                                                                                    _FacturasComerciales.Add(New ConstructorFacturaComercial(True, item.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente))

                                                                                End If

                                                                            End If

                                                                        Else

                                                                            item.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Id = item.Id.ToString

                                                                            _FacturasComerciales.Add(New ConstructorFacturaComercial(True, item.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente))

                                                                        End If

                                                                    End Sub)

                    If facturasExistentes_ Then

                        If _FacturasComerciales.Count > 0 Then

                            .ObjectReturned = _FacturasComerciales

                            .SetOK()
                        Else

                            .ObjectReturned = _FacturasComerciales

                            .SetOKBut(Me, "Facturas no disponibles para agregar a la lista actual de facturas.")

                        End If

                    Else

                        If _FacturasComerciales.Count > 0 Then

                            .ObjectReturned = _FacturasComerciales

                            .SetOK()
                        Else

                            .ObjectReturned = Nothing

                            .SetOKBut(Me, "Facturas no disponibles en los registros con los datos proporcionados")

                        End If

                    End If

                End Using

            Catch ex As Exception

                .ObjectReturned = Nothing

                .SetOKBut(Me, $"Error interno - {ex}")

            End Try

        End With

        Return _Estado

    End Function

    Private Function ConsultaInterior(ByVal listaFacturas_ As List(Of ObjectId)) As TagWatcher

        With _Estado

            Try

                If _FacturasComerciales IsNot Nothing AndAlso _FacturasComerciales.Count > 0 Then

                    Dim facturasEspecificas_ = New List(Of ConstructorFacturaComercial)

                    For Each item_ In _FacturasComerciales.Where(Function(f) listaFacturas_.Contains(ObjectId.Parse(f.Id)))

                        facturasEspecificas_.Add(item_)

                    Next

                    If facturasEspecificas_.Count > 0 Then

                        .ObjectReturned = facturasEspecificas_

                        .SetOK()

                    Else

                        .ObjectReturned = Nothing

                        .SetOKBut(Me, "No se encontraron facturas disponibles en el listado actual con los objectdis proporcionados.")

                    End If

                Else

                    .ObjectReturned = Nothing

                    .SetOKBut(Me, "Consulta interna vacía. Haga una consulta externa para obtener facturas desde sus registros principales.")

                End If


            Catch ex As Exception

                .ObjectReturned = Nothing

                .SetOKBut(Me, $"Error interno - {ex}")

            End Try

        End With

        Return _Estado

    End Function

    Private Function ConsultaInterior(ByVal listaFacturas_ As List(Of String)) As TagWatcher

        With _Estado

            Try

                If _FacturasComerciales IsNot Nothing AndAlso _FacturasComerciales.Count > 0 Then

                    Dim facturasEspecificas_ = New List(Of ConstructorFacturaComercial)

                    For Each item_ In _FacturasComerciales.Where(Function(f) listaFacturas_.Equals(f.FolioDocumento))

                        facturasEspecificas_.Add(item_)

                    Next

                    If facturasEspecificas_.Count > 0 Then

                        .ObjectReturned = facturasEspecificas_

                        .SetOK()

                    Else

                        .ObjectReturned = Nothing

                        .SetOKBut(Me, "No se encontraron facturas disponibles en el listado actual con los objectdis proporcionados.")

                    End If

                Else

                    .ObjectReturned = Nothing

                    .SetOKBut(Me, "Consulta interna vacía. Haga una consulta externa para obtener facturas desde sus registros principales.")

                End If

            Catch ex As Exception

                .ObjectReturned = Nothing

                .SetOKBut(Me, $"Error interno - {ex}")

            End Try

        End With

        Return _Estado

    End Function

    Private Function ObtenerFacturas(listaFacturas_ As List(Of ObjectId)) As TagWatcher

        Dim estadoObtenerFacturas_ As New TagWatcher

        Select Case _ModalidadTrabajo

            Case IControladorFacturaComercial.Modalidades.Interno

                estadoObtenerFacturas_ = ConsultaInterior(listaFacturas_)

            Case IControladorFacturaComercial.Modalidades.Externo

                estadoObtenerFacturas_ = ConsultaExterior(listaFacturas_)

        End Select

        Return estadoObtenerFacturas_

    End Function

    Private Function ObtenerFacturas(listaFacturas_ As List(Of String)) As TagWatcher

        Dim estadoObtenerFacturas_ As New TagWatcher

        Select Case _ModalidadTrabajo

            Case IControladorFacturaComercial.Modalidades.Interno

                estadoObtenerFacturas_ = ConsultaInterior(listaFacturas_)

            Case IControladorFacturaComercial.Modalidades.Externo

                estadoObtenerFacturas_ = ConsultaExterior(listaFacturas_)

        End Select

        Return estadoObtenerFacturas_

    End Function

    Private Function DesasociarDocumentoAsociado(ByVal operacionGenericaFacturaComercial_ As OperacionGenerica,
                                             ByVal firmaElectronicaDocumentoAsociado_ As String,
                                             ByVal idDocumentoAsociado_ As ObjectId) As TagWatcher

        With _Estado

            Try

                Dim documentosAsociado_ As New DocumentoAsociado With {
                  .analisisconsistencia = 0,
                  .identificadorrecurso = operacionGenericaFacturaComercial_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.DescripcionTipoDocumentoElectronico,
                  ._iddocumentoasociado = idDocumentoAsociado_,
                  .firmaelectronica = firmaElectronicaDocumentoAsociado_
                }


            Catch ex As Exception

            End Try

        End With

        Return _Estado

    End Function

    Private Function GuardarListaDocumentosAsociados(ByVal operacionGenericaFacturaComercial_ As OperacionGenerica,
                                                ByVal documentosAsociados_ As List(Of DocumentoAsociado)) As TagWatcher

        With _Estado

            Try
                Using iEnlace_ As IEnlaceDatos = New EnlaceDatos(21)

                    iEnlace_.EnvironmentOnline = _environmentOnline

                    Dim operationsDB_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)((New ConstructorFacturaComercial).GetType.Name)

                    operacionGenericaFacturaComercial_.Borrador.Folder.DocumentosAsociados = documentosAsociados_

                    Dim filter_ = Builders(Of OperacionGenerica).Filter.Eq(Function(x) x.Id, operacionGenericaFacturaComercial_.Id)

                    Dim options_ As New ReplaceOptions With {.IsUpsert = True}

                    Dim result_ = operationsDB_.ReplaceOne(filter_, operacionGenericaFacturaComercial_, options_)

                    If result_.ModifiedCount > 0 Then

                        .ObjectReturned = "Documentos Asociados agregados correctamente"
                        .SetOK()

                    ElseIf result_.UpsertedId IsNot Nothing Then

                        .ObjectReturned = "Documento creado correctamente (upsert)"
                        .SetOK()

                    Else

                        .SetOKBut(Me, "No se realizaron cambios")
                        .ObjectReturned = "No se realizaron cambios"

                    End If

                End Using

            Catch ex As Exception

                .ObjectReturned = Nothing

                .SetError(Me, $"Ha ocurrido un error_ {ex}")

            End Try

        End With

        Return Estado

    End Function

#End Region


#Region "FACTURAS X MONEDAS"
    Private Function ObtenerFactorPorMoneda(ByRef monedaIncrementable_ As String,
                                            ByRef fechaMoneda_ As Date) As Dictionary(Of String, Double)

        _listaFactoresMoneda = New Dictionary(Of String, Double)

        _iControladorMonedas = New ControladorMonedas

        Dim factorTipoCambio_ = DirectCast(_iControladorMonedas.ObtenerFactorTipodeCambio(monedaIncrementable_, fechaMoneda_).ObjectReturned, Object)

        If factorTipoCambio_.Count > 0 Then

            If factorTipoCambio_(0) IsNot Nothing AndAlso factorTipoCambio_(1) IsNot Nothing Then

                _listaFactoresMoneda.Add("tipoCambioMoneda_", factorTipoCambio_(1).tipocambio)

                _listaFactoresMoneda.Add("factorMoneda_", factorTipoCambio_(0).factor)

            End If

        End If

        Return _listaFactoresMoneda

    End Function

#End Region


#Region "FACTURAS X INCREMENTABLES"
    Private Function CalcularValorIncrementable(ByRef incrementable_ As Double,
                                                ByRef monedaIncrementable_ As String,
                                                ByRef fechaMoneda_ As Date) As Double

        Dim valoresMoneda_ = ObtenerFactorPorMoneda(monedaIncrementable_, fechaMoneda_)

        If valoresMoneda_.Count > 0 Then

            incrementable_ *= (valoresMoneda_("tipoCambioMoneda_") *
                               valoresMoneda_("factorMoneda_"))

            incrementable_ = IIf(incrementable_ > 0.0, incrementable_, 0.0)

        Else

            incrementable_ = 0.0

        End If

        Return incrementable_

    End Function

    Private Function ObtenerValorIncrementable(ByRef valorSeccion_ As Double,
                                               ByRef monedaSeccion_ As String,
                                               ByRef fechaMoneda_ As Date) As Double

        Dim valorIncrementable_ As Double

        If valorSeccion_ > 0 AndAlso monedaSeccion_ IsNot Nothing Then

            valorIncrementable_ = CalcularValorIncrementable(valorSeccion_, monedaSeccion_, fechaMoneda_)
        Else

            valorIncrementable_ = 0.00

        End If

        Return valorIncrementable_

    End Function

#End Region

#Region "FACTURAS X PARTIDAS"
    Private Function ObtenerSeccionPartidas(ByRef listaFacturas_ _
                                            As List(Of ConstructorFacturaComercial)) _
                                            As Dictionary(Of ObjectId, Object)

        _listadoCampos = New Dictionary(Of ObjectId, Object)

        For Each factura_ In _listaFacturas

            With factura_.Seccion(SFAC4)

                Dim partida_ As New List(Of Object)

                For Each item_ In .Nodos

                    Dim listaCampos_ As New Dictionary(Of String, Object)

                    For Each i_ In item_.Nodos

                        Dim items_ = DirectCast(i_.Nodos(0), Campo)

                        Dim aux_ As New Dictionary(Of String, Object) _
                                                    From {
                                                        {"Valor", items_.Valor},
                                                        {"ValorPresentacion", items_.ValorPresentacion}
                                                    }

                        listaCampos_.Add(items_.Nombre, aux_)

                    Next

                    partida_.Add(listaCampos_)

                Next

                _listadoCampos.Add(ObjectId.Parse(factura_.Id), partida_)

            End With

        Next

        Return _listadoCampos

    End Function

    Private Function ObtenerPartidas(ByVal _facturasComerciales As TagWatcher) As TagWatcher

        With _Estado

            _listadoCampos = New Dictionary(Of ObjectId, Object)

            _listadoCampos = SeccionesCampos(_facturasComerciales, SFAC4)

            If _listadoCampos.Count > 0 Then

                .ObjectReturned = _listadoCampos

                .SetOK()
            Else

                .ObjectReturned = Nothing

                .SetOKBut(Me, "No se llenó la lista")

            End If

        End With

        Return _Estado

    End Function

#End Region


#Region "FACTURAS X SECCIONES"
    Private Function ObtenerSeccionFactura(ByRef listaFacturas_ _
                                           As List(Of ConstructorFacturaComercial),
                                           ByRef seccion_ As Integer) _
                                           As Dictionary(Of ObjectId, Object)

        _listadoCampos = New Dictionary(Of ObjectId, Object)

        For Each factura_ In _listaFacturas

            With factura_.Seccion(seccion_)

                Dim listaCampos_ As New Dictionary(Of String, Object)

                For Each item_ In .Nodos

                    Dim items_ = DirectCast(item_.Nodos(0), Campo)

                    Dim aux_ As New Dictionary(Of String, Object) _
                                              From {
                                                {"Valor", items_.Valor},
                                                {"ValorPresentacion", items_.ValorPresentacion}
                                              }

                    listaCampos_.Add(items_.Nombre, aux_)

                Next

                _listadoCampos.Add(ObjectId.Parse(factura_.Id), listaCampos_)

            End With

        Next

        Return _listadoCampos

    End Function

    Private Function ObtenerSeccionFactura(ByRef listaFacturas_ As List(Of ConstructorFacturaComercial),
                                           ByRef seccion_ As Integer,
                                           ByRef listaNodos_ As List(Of Integer)) _
                                           As Dictionary(Of ObjectId, Object)

        _listadoCampos = New Dictionary(Of ObjectId, Object)

        For Each factura_ In _listaFacturas

            With factura_.Seccion(seccion_)

                Dim listaCampos_ As New Dictionary(Of String, Object)

                For Each item_ In listaNodos_

                    Dim aux_ As New Dictionary(Of String, Object) _
                                              From {
                                                {"Valor", .Attribute(item_).Valor},
                                                {"ValorPresentacion", .Attribute(item_).ValorPresentacion}
                                              }

                    listaCampos_.Add(.Attribute(item_).Nombre, aux_)

                Next

                _listadoCampos.Add(ObjectId.Parse(factura_.Id), listaCampos_)

            End With

        Next

        Return _listadoCampos

    End Function

    Private Function SeccionesCampos(ByRef facturaComercial_ As TagWatcher,
                                     ByRef seccion_ As Integer,
                                     Optional listaNodos_ As List(Of Integer) = Nothing) _
                                     As Dictionary(Of ObjectId, Object)

        _listaFacturas = DirectCast(facturaComercial_.ObjectReturned, List(Of ConstructorFacturaComercial))

        _listadoCampos = New Dictionary(Of ObjectId, Object)

        If seccion_ = 4 Then

            Return ObtenerSeccionPartidas(_listaFacturas)

        ElseIf listaNodos_ IsNot Nothing Then

            Return ObtenerSeccionFactura(_listaFacturas, seccion_, listaNodos_)

        Else

            Return ObtenerSeccionFactura(_listaFacturas, seccion_)

        End If


        Return _listadoCampos

    End Function

#End Region


#Region "FACTURAS X ICOTERMS"

    Private Function ObtenerListaIconterms(ByRef _facturasComerciales As TagWatcher) As TagWatcher

        With _Estado

            _listadoCampos = New Dictionary(Of ObjectId, Object)

            _listadoCampos = SeccionesCampos(_facturasComerciales,
                                           SFAC1, New List(Of Integer) From {RecursosComercioExterior.CamposFacturaComercial.CA_CVE_INCOTERM})

            If _listadoCampos.Count > 0 Then

                .ObjectReturned = _listadoCampos

                .SetOK()

            Else

                .ObjectReturned = Nothing

                .SetOKBut(Me, "No se llenó la lista")

            End If

        End With

        Return _Estado

    End Function

#End Region


#Region "FACTURAS X INCREMENTABLES"
    Private Function ObtenerListaIncrementables(ByVal _facturasComerciales As TagWatcher) As TagWatcher

        With _Estado

            _listadoCampos = New Dictionary(Of ObjectId, Object)

            _listadoCampos = SeccionesCampos(_facturasComerciales, SFAC5)

            If _listadoCampos.Count > 0 Then

                .ObjectReturned = _listadoCampos

                .SetOK()
            Else

                .ObjectReturned = Nothing

                .SetOKBut(Me, "No se llenó la lista")

            End If

        End With

        Return _Estado

    End Function

    Private Function ObtenerTotalIncrementables(ByRef facturasDisponibles_ As TagWatcher,
                                                ByRef fechaMoneda_ As Date) As TagWatcher

        With _Estado

            _listaFacturas = DirectCast(facturasDisponibles_.ObjectReturned, List(Of ConstructorFacturaComercial))

            _listadoCampos = New Dictionary(Of ObjectId, Object)

            For Each facturaComercial_ In _listaFacturas

                Dim totalValorSeguros_

                Dim totalValorFletes_

                Dim totalValorEmbalajes_

                Dim totalValorOtrosIncrementables_

                With facturaComercial_.Seccion(SFAC5)

                    _totalValorSeguros += ObtenerValorIncrementable(.Attribute(CA_SEGURO).Valor,
                                                                    .Attribute(CA_MONEDA_SEGUROS).ValorPresentacion,
                                                                    fechaMoneda_)

                    _totalValorFletes += ObtenerValorIncrementable(.Attribute(CA_FLETES).Valor,
                                                                   .Attribute(CA_MONEDA_FLETES).ValorPresentacion,
                                                                   fechaMoneda_)

                    _totalValorEmbalajes += ObtenerValorIncrementable(.Attribute(CA_EMBALAJES).Valor,
                                                                  .Attribute(CA_MONEDA_EMBALAJES).ValorPresentacion,
                                                                  fechaMoneda_)

                    _totalValorOtrosIncrementables += ObtenerValorIncrementable(.Attribute(CA_OTROS_INCREMENTABLES).Valor,
                                                                           .Attribute(CA_MONEDA_OTROS_INCREMENTABLES).ValorPresentacion,
                                                                           fechaMoneda_)

                    _totalValorDescuentos += ObtenerValorIncrementable(.Attribute(CA_DESCUENTOS).Valor,
                                                                  .Attribute(CA_MONEDA_DESCUENTOS).ValorPresentacion,
                                                                  fechaMoneda_)

                    totalValorSeguros_ = ObtenerValorIncrementable(.Attribute(CA_SEGURO).Valor,
                                                                    .Attribute(CA_MONEDA_SEGUROS).ValorPresentacion,
                                                                    fechaMoneda_)

                    totalValorFletes_ = ObtenerValorIncrementable(.Attribute(CA_FLETES).Valor,
                                                                   .Attribute(CA_MONEDA_FLETES).ValorPresentacion,
                                                                   fechaMoneda_)

                    totalValorEmbalajes_ = ObtenerValorIncrementable(.Attribute(CA_EMBALAJES).Valor,
                                                                  .Attribute(CA_MONEDA_EMBALAJES).ValorPresentacion,
                                                                  fechaMoneda_)

                    totalValorOtrosIncrementables_ = ObtenerValorIncrementable(.Attribute(CA_OTROS_INCREMENTABLES).Valor,
                                                                           .Attribute(CA_MONEDA_OTROS_INCREMENTABLES).ValorPresentacion,
                                                                           fechaMoneda_)
                End With

                _totalValorIncrementables = (_totalValorFletes +
                                        _totalValorSeguros +
                                        _totalValorEmbalajes +
                                        _totalValorOtrosIncrementables)

                Dim auxTotalIncrementable_ = (totalValorFletes_ +
                                        totalValorSeguros_ +
                                        totalValorEmbalajes_ +
                                        totalValorOtrosIncrementables_)


                _listadoCampos.Add(ObjectId.Parse(facturaComercial_.Id), auxTotalIncrementable_)

            Next

            If _listadoCampos.Count > 0 Then

                .ObjectReturned = _listadoCampos

                .SetOK()

            Else

                .ObjectReturned = Nothing

                .SetOKBut(Me, "No se llenó la lista")

            End If

        End With

        Return _Estado

    End Function

    Private Function ObtenerIncrementables(ByRef facturasDisponibles_ As List(Of ConstructorFacturaComercial),
                                           ByRef fechaMoneda_ As Date) As TagWatcher

        Dim estadoIncrementables_ As New TagWatcher

        With estadoIncrementables_

            Dim listaIncrementables_ As New List(Of Incrementables)

            For Each facturaComercial_ In facturasDisponibles_

                Dim incrementables_ As New Incrementables

                With facturaComercial_.Seccion(SFAC5)

                    incrementables_.idFactura = ObjectId.Parse(facturaComercial_.Id)

                    incrementables_.seguros = ObtenerValorIncrementable(.Attribute(CA_SEGURO).Valor,
                                                                    .Attribute(CA_MONEDA_SEGUROS).ValorPresentacion,
                                                                    fechaMoneda_)

                    incrementables_.fletes = ObtenerValorIncrementable(.Attribute(CA_FLETES).Valor,
                                                                   .Attribute(CA_MONEDA_FLETES).ValorPresentacion,
                                                                   fechaMoneda_)

                    incrementables_.embalajes = ObtenerValorIncrementable(.Attribute(CA_EMBALAJES).Valor,
                                                                  .Attribute(CA_MONEDA_EMBALAJES).ValorPresentacion,
                                                                  fechaMoneda_)

                    incrementables_.otros = ObtenerValorIncrementable(.Attribute(CA_OTROS_INCREMENTABLES).Valor,
                                                                           .Attribute(CA_MONEDA_OTROS_INCREMENTABLES).ValorPresentacion,
                                                                           fechaMoneda_)
                End With

                listaIncrementables_.Add(incrementables_)

            Next

            If listaIncrementables_.Count > 0 Then

                .SetOK()

                .ObjectReturned = listaIncrementables_
            Else

                .ObjectReturned = Nothing

                .SetOKBut(Me, "No se encontraron incrementables para esta factura")

            End If

        End With

        Return estadoIncrementables_

    End Function


#End Region

#Region "FACTURAS X VALORES"
    Private Function ObtenerListaValores(idsFacturas_ As List(Of ObjectId),
                                        seccionesCampos_ As Dictionary(Of [Enum], List(Of [Enum]))) As TagWatcher

        With _Estado

            Try

                _listadoValoresObject = New Dictionary(Of ObjectId, List(Of Nodo))

                Dim diccionarioValoresObjectId As New Dictionary(Of ObjectId, List(Of Nodo))

                _rOrganismo = New Organismo With {
                    .EnvironmentOnline = _environmentOnline
                }

                _listadoValoresObject = _rOrganismo.ObtenerCamposSeccionExterior(idsFacturas_,
                                                                                New ConstructorFacturaComercial,
                                                                                seccionesCampos_)

                If _listadoValoresObject.Count > 0 Then

                    For Each listaValor_ In _listadoValoresObject

                        diccionarioValoresObjectId.Add(listaValor_.Key, listaValor_.Value)

                    Next

                    If diccionarioValoresObjectId.Count > 0 Then

                        .ObjectReturned = diccionarioValoresObjectId

                        .SetOK()

                    Else

                        .ObjectReturned = Nothing

                        .SetOKBut(Me, "No se llenó la lista de valores")

                    End If

                Else

                    .ObjectReturned = Nothing

                    .SetError(Me, "No se encontraron campos en el listado de facturas")

                End If

            Catch ex As Exception

                .ObjectReturned = Nothing

                .SetError(Me, $"Ha ocurrido un error_ {ex}")

            End Try

        End With

        Return _Estado

    End Function

    Private Function ObtenerListaValores(foliosFacturas_ As List(Of String),
                                         seccionesCampos_ As Dictionary(Of [Enum], List(Of [Enum]))) _
                                         As TagWatcher

        With _Estado

            Try
                _listadoValoresString = New Dictionary(Of String, List(Of Nodo))

                Dim diccionarioValoresString As New Dictionary(Of String, List(Of Nodo))

                _rOrganismo = New Organismo

                _listadoValoresString = _rOrganismo.ObtenerCamposSeccionExterior(foliosFacturas_,
                                                                  New ConstructorFacturaComercial,
                                                                  seccionesCampos_)

                If _listadoValoresString.Count > 0 Then

                    For Each listaValor_ In _listadoValoresString

                        diccionarioValoresString.Add(listaValor_.Key, listaValor_.Value)

                    Next

                Else

                    .SetError(Me, "No se encontraron campos en el listado de facturas")

                End If

                If diccionarioValoresString.Count > 0 Then : .SetOK() : Else .SetOKBut(Me, "No se llenó la lista de valores") : End If

                .ObjectReturned = diccionarioValoresString


            Catch ex As Exception

                .SetError(Me, ex.Message)

            End Try


        End With

        Return Estado

    End Function

#End Region


#Region "FACTURAS X VALOR DOLARES"

    Private Function ObtenerValorDolaresFactura(ByRef facturasDisponibles_ As TagWatcher,
                                                ByRef fechaMoneda_ As Date, Optional ByVal usoPedimento_ As Boolean = True) _
                                                As TagWatcher
        With _Estado

            _listaFacturas = DirectCast(facturasDisponibles_.ObjectReturned, List(Of ConstructorFacturaComercial))

            _diccionarioValores = New Dictionary(Of ObjectId, Dictionary(Of String, Object))

            For Each facturaComercial_ In _listaFacturas

                With facturaComercial_.Seccion(SFAC1)

                    If .Attribute(CA_MONEDA_FACTURACION).ValorPresentacion IsNot Nothing _
                        And .Attribute(CP_VALOR_FACTURA).Valor IsNot Nothing Then

                        Dim factorMonedaFactura_ = ObtenerFactorPorMoneda(.Attribute(CA_MONEDA_FACTURACION).ValorPresentacion,
                                                                          fechaMoneda_)

                        If factorMonedaFactura_.Count > 0 Then

                            Dim valorDolares_ = .Attribute(CP_VALOR_FACTURA).Valor *
                                               factorMonedaFactura_("factorMoneda_")

                            Dim monedaFactura_ As String = Nothing

                            If usoPedimento_ Then

                                monedaFactura_ = ObtenerMonedaPorObjectid(ObjectId.Parse(facturaComercial_.Seccion(SFAC1).Attribute(CA_MONEDA_FACTURACION).Valor)) _
                                                        .aliasmoneda.FirstOrDefault(Function(a) a.Clave = "cvemonedaA05")?.Valor

                            Else

                                monedaFactura_ = .Attribute(CA_MONEDA_FACTURACION).ValorPresentacion

                            End If


                            _diccionarioValores.Add(ObjectId.Parse(facturaComercial_.Id),
                                                        New Dictionary(Of String, Object) _
                                                        From {
                                                            {"valorFactura_", .Attribute(CP_VALOR_FACTURA).Valor},
                                                            {"monedaFactura_", monedaFactura_},
                                                            {"factorMonedaFactura_", factorMonedaFactura_("factorMoneda_")},
                                                            {"totalValorDolaresFactura_", valorDolares_}
                                                        })

                        Else

                            _Estado.SetError(Me, "No se encontró el factor moneda")

                            Return _Estado

                        End If

                    Else

                        _Estado.SetError(Me, "Valores no encontrados en la sección actual")

                        Return _Estado

                    End If

                End With

            Next

            If _diccionarioValores.Count >= 1 Then : .SetOK() : Else .SetOKBut(Me, "No se llenó la lista de valor dólares") : End If

            .ObjectReturned = _diccionarioValores

            Return _Estado

        End With

    End Function

#End Region


#Region "FACTURAS X PROVEEDOR"

    Private Function ObtenerFacturasProveedor(ByVal idProveedor_ As ObjectId, ByVal idCliente_ As ObjectId,
                                              Optional ByVal facturapublicada_ As Boolean = True) As TagWatcher
        With Estado

            Try

                Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos(21)

                    enlaceDatos_.EnvironmentOnline = _environmentOnline

                    Dim collection_ = enlaceDatos_.GetMongoCollection(Of OperacionGenerica)((New ConstructorFacturaComercial).GetType.Name)

                    Dim filtroPublicado_ As FilterDefinition(Of OperacionGenerica) =
                    Builders(Of OperacionGenerica).Filter.Eq(Function(x) x.Publicado, True)

                    Dim filtroFirmaelectronica_ As FilterDefinition(Of OperacionGenerica) =
                    Builders(Of OperacionGenerica).Filter.Ne(Function(x) x.FirmaElectronica, Nothing)

                    Dim filtroCliente_ As FilterDefinition(Of OperacionGenerica) =
                    Builders(Of OperacionGenerica).Filter.Eq(Function(x) x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.ObjectIdPropietario, idCliente_)

                    Dim filtroTipoOperacion_ As FilterDefinition(Of OperacionGenerica) =
                    Builders(Of OperacionGenerica).Filter.Eq(Function(x) DirectCast(x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts("Encabezado")(0).Nodos(0).Nodos(0).Nodos(0), Campo).Valor, TipoOperacion)

                    Dim filtroNoAsociada_ As FilterDefinition(Of OperacionGenerica) =
                    Builders(Of OperacionGenerica).Filter.Eq(Function(x) DirectCast(x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts("Encabezado")(0).Nodos(0).Nodos(45).Nodos(0), Campo).Valor, False)

                    Dim filtroProveedor_ As FilterDefinition(Of OperacionGenerica) =
                    Builders(Of OperacionGenerica).Filter.Eq(Function(x) DirectCast(x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts("Encabezado")(1).Nodos(0).Nodos(3).Nodos(0), Campo).Valor, idProveedor_.ToString)

                    Dim filtroCombinado_ As FilterDefinition(Of OperacionGenerica) =
                    Builders(Of OperacionGenerica).Filter.And(
                        filtroPublicado_,
                        filtroFirmaelectronica_,
                        filtroCliente_,
                        filtroTipoOperacion_,
                        filtroNoAsociada_,
                        filtroProveedor_)

                    Dim result_ = collection_.Aggregate().
                    Match(filtroCombinado_).
                    Project(Function(y) New With {
                        Key .id_ = y.Id,
                        Key .encabezado_ = y.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Encabezado")(0),
                        Key .encabezadoProveedor_ = y.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Encabezado")(1)
                    }).ToList()

                    If result_.Count > 0 Then

                        _listaVehiculoFacturasComerciales = New List(Of ICommercialInvoiceCustomsDocument)

                        Dim i_ = 0

                        result_.ToList().ForEach(Sub(y)

                                                     Dim auxFactura_ = New CommercialInvoiceCustomsDocument

                                                     With y.encabezado_

                                                         auxFactura_._id = y.id_

                                                         auxFactura_.invoicenumber = .Campo(CamposFacturaComercial.CA_NUMERO_FACTURA).Valor

                                                         auxFactura_.invoicedate = .Campo(CamposFacturaComercial.CA_FECHA_FACTURA).ValorPresentacion

                                                         auxFactura_.valuecertificate = .Campo(CamposAcuseValor.CA_NUMERO_ACUSEVALOR).Valor

                                                         auxFactura_.valuecertificatedate = .Campo(CamposAcuseValor.CA_FECHA_ACUSEVALOR).Valor

                                                         If .Campo(CamposAcuseValor.CP_ID_ACUSEVALOR).Valor IsNot Nothing Then

                                                             auxFactura_.idvaluecertificate = .Campo(CamposAcuseValor.CP_ID_ACUSEVALOR).Valor

                                                         End If

                                                         auxFactura_.customerid = idCliente_

                                                         auxFactura_.supplierid = idProveedor_

                                                         Dim auxdatosincoterm_ = ExtraerClavesyDescripcionesStrings(.Campo(CamposFacturaComercial.CA_CVE_INCOTERM).ValorPresentacion, "-")

                                                         auxFactura_.incotermnumerickey = .Campo(CamposFacturaComercial.CA_CVE_INCOTERM).Valor

                                                         auxFactura_.incotermkey = auxdatosincoterm_.clave_

                                                         auxFactura_.incoterm = auxdatosincoterm_.description_

                                                         auxFactura_.invoicecountry = .Campo(CamposFacturaComercial.CA_PAIS_FACTURACION).ValorPresentacion

                                                         auxFactura_.invoiceseries = .Campo(CamposFacturaComercial.CP_SERIE_FOLIO_FACTURA).Valor

                                                         auxFactura_.totalinvoice = .Campo(CamposFacturaComercial.CP_VALOR_FACTURA).Valor

                                                         auxFactura_.invoicecurrency = .Campo(CamposFacturaComercial.CA_MONEDA_FACTURACION).ValorPresentacion

                                                         auxFactura_.hasalienation = .Campo(CamposFacturaComercial.CP_APLICA_ENAJENACION).Valor

                                                         auxFactura_.hassubdivision = .Campo(CamposFacturaComercial.CA_APLICA_SUBDIVISION).Valor

                                                         auxFactura_.hasadditions = .Campo(CamposFacturaComercial.CP_APLICA_INCREMENTABLES).Valor

                                                         auxFactura_.operationtype = TipoOperacion

                                                         _listaVehiculoFacturasComerciales.Add(auxFactura_)

                                                     End With

                                                     With y.encabezadoProveedor_

                                                         auxFactura_.supplier = New Supplier

                                                         auxFactura_.supplier.appliescertificate = .Campo(CamposFacturaComercial.CA_APLICA_CERTIFICADO).Valor

                                                         auxFactura_.supplier.linkagekey = .Campo(CamposFacturaComercial.CA_CVE_VINCULACION).Valor

                                                         auxFactura_.supplier.linkagedescription = .Campo(CamposFacturaComercial.CA_CVE_VINCULACION).ValorPresentacion

                                                         auxFactura_.supplier.valuationmethodkey = .Campo(CamposFacturaComercial.CP_CVE_METODO_VALORACION).Valor

                                                         auxFactura_.supplier.valuationmethoddescription = .Campo(CamposFacturaComercial.CP_CVE_METODO_VALORACION).ValorPresentacion

                                                         auxFactura_.supplier.certifiername = .Campo(CamposFacturaComercial.CA_APLICA_CERTIFICADO).Valor

                                                         If .Campo(CamposProveedorOperativo.CP_DESTINATARIO_PROVEEDOR).Valor IsNot Nothing Then

                                                             auxFactura_.supplier.isconsignee = .Campo(CamposProveedorOperativo.CP_DESTINATARIO_PROVEEDOR).Valor

                                                         End If

                                                     End With

                                                 End Sub)

                        If _listaVehiculoFacturasComerciales.Count > 0 Then

                            .ObjectReturned = _listaVehiculoFacturasComerciales

                            .SetOK()

                        Else

                            .ObjectReturned = Nothing

                            .SetOKBut(Me, "Facturas solicitadas no encontradas con estos datos")

                        End If

                    Else

                        .ObjectReturned = Nothing

                        .SetOKBut(Me, "Facturas solicitadas no encontradas con estos datos")

                    End If

                End Using

            Catch ex As Exception

                .ObjectReturned = Nothing

                .SetOKBut(Me, $"Ha ocurrido un error {ex}")

            End Try

        End With

        Return Estado

    End Function

    ''ESTOS METODOS DEBEN UNIFIRCARSE, DEJALO ASI DE MOMENTO, PERO HAY QUE COMPONERLO

    Private Function ObtenerFacturasProveedorParaPedimento(ByVal idProveedor_ As ObjectId, ByVal idCliente_ As ObjectId) As TagWatcher
        With Estado

            Try

                Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos(21)

                    enlaceDatos_.EnvironmentOnline = _environmentOnline

                    Dim collection_ = enlaceDatos_.GetMongoCollection(Of OperacionGenerica)((New ConstructorFacturaComercial).GetType.Name)

                    Dim filtroPublicado_ As FilterDefinition(Of OperacionGenerica) =
                    Builders(Of OperacionGenerica).Filter.Eq(Function(x) x.Publicado, True)

                    Dim filtroFirmaelectronica_ As FilterDefinition(Of OperacionGenerica) =
                    Builders(Of OperacionGenerica).Filter.Ne(Function(x) x.FirmaElectronica, Nothing)

                    Dim filtroFacturaActiva_ As FilterDefinition(Of OperacionGenerica) =
                          Builders(Of OperacionGenerica).Filter.Eq(Of Int32)("estado", 1)

                    Dim filtroCliente_ As FilterDefinition(Of OperacionGenerica) =
                    Builders(Of OperacionGenerica).Filter.Eq(Function(x) x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.ObjectIdPropietario, idCliente_)

                    Dim filtroTipoOperacion_ As FilterDefinition(Of OperacionGenerica) =
                    Builders(Of OperacionGenerica).Filter.Eq(Function(x) DirectCast(x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts("Encabezado")(0).Nodos(0).Nodos(0).Nodos(0), Campo).Valor, TipoOperacion)

                    Dim filtroSinSubdivision_ As FilterDefinition(Of OperacionGenerica) =
                    Builders(Of OperacionGenerica).Filter.Eq(Function(x) DirectCast(x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts("Encabezado")(0).Nodos(0).Nodos(42).Nodos(0), Campo).Valor, False)

                    Dim filtroNoAsociada_ As FilterDefinition(Of OperacionGenerica) =
                    Builders(Of OperacionGenerica).Filter.Eq(Function(x) DirectCast(x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts("Encabezado")(0).Nodos(0).Nodos(45).Nodos(0), Campo).Valor, False)

                    Dim filtroProveedor_ As FilterDefinition(Of OperacionGenerica) =
                    Builders(Of OperacionGenerica).Filter.Eq(Function(x) DirectCast(x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts("Encabezado")(1).Nodos(0).Nodos(3).Nodos(0), Campo).Valor, idProveedor_.ToString)

                    Dim filtroCombinado_ As FilterDefinition(Of OperacionGenerica) =
                    Builders(Of OperacionGenerica).Filter.And(
                        filtroPublicado_,
                        filtroFirmaelectronica_,
                        filtroFacturaActiva_,
                        filtroCliente_,
                        filtroTipoOperacion_,
                        filtroSinSubdivision_,
                        filtroNoAsociada_,
                        filtroProveedor_)

                    Dim result_ = collection_.Aggregate().
                    Match(filtroCombinado_).
                    Project(Function(y) New With {
                        Key .id_ = y.Id,
                        Key .encabezado_ = y.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Encabezado")(0),
                        Key .encabezadoProveedor_ = y.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Encabezado")(1)
                    }).ToList()

                    If result_.Count > 0 Then

                        _listaVehiculoFacturasComerciales = New List(Of ICommercialInvoiceCustomsDocument)

                        Dim i_ = 0

                        result_.ToList().ForEach(Sub(y)

                                                     Dim auxFactura_ = New CommercialInvoiceCustomsDocument

                                                     With y.encabezado_

                                                         auxFactura_._id = y.id_

                                                         auxFactura_.invoicenumber = .Campo(CamposFacturaComercial.CA_NUMERO_FACTURA).Valor

                                                         auxFactura_.invoicedate = .Campo(CamposFacturaComercial.CA_FECHA_FACTURA).ValorPresentacion

                                                         auxFactura_.valuecertificate = .Campo(CamposAcuseValor.CA_NUMERO_ACUSEVALOR).Valor

                                                         auxFactura_.valuecertificatedate = .Campo(CamposAcuseValor.CA_FECHA_ACUSEVALOR).Valor

                                                         If .Campo(CamposAcuseValor.CP_ID_ACUSEVALOR).Valor IsNot Nothing Then

                                                             auxFactura_.idvaluecertificate = .Campo(CamposAcuseValor.CP_ID_ACUSEVALOR).Valor

                                                         End If

                                                         auxFactura_.customerid = idCliente_

                                                         auxFactura_.supplierid = idProveedor_

                                                         Dim auxdatosincoterm_ = ExtraerClavesyDescripcionesStrings(.Campo(CamposFacturaComercial.CA_CVE_INCOTERM).ValorPresentacion, "-")

                                                         auxFactura_.incotermnumerickey = .Campo(CamposFacturaComercial.CA_CVE_INCOTERM).Valor

                                                         auxFactura_.incotermkey = auxdatosincoterm_.clave_

                                                         auxFactura_.incoterm = auxdatosincoterm_.description_

                                                         auxFactura_.invoicecountry = .Campo(CamposFacturaComercial.CA_PAIS_FACTURACION).ValorPresentacion

                                                         auxFactura_.invoiceseries = .Campo(CamposFacturaComercial.CP_SERIE_FOLIO_FACTURA).Valor

                                                         auxFactura_.totalinvoice = .Campo(CamposFacturaComercial.CP_VALOR_FACTURA).Valor

                                                         ' System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: eL OBJECTID DE LA MONEDA FACTURA-PROVEEDOR ES: { .Campo(CamposFacturaComercial.CA_MONEDA_FACTURACION).Valor}")


                                                         'Dim cveMonedaApendice_invoicecurrency_ As String = ObtenerMonedaPorObjectid(ObjectId.Parse(.Campo(CamposFacturaComercial.CA_MONEDA_FACTURACION).Valor)) _
                                                         '.aliasmoneda.FirstOrDefault(Function(a) a.Clave = "cvemonedaA05")?.Valor

                                                         Dim objectIdMoneda As ObjectId = ObjectId.Parse(.Campo(CamposFacturaComercial.CA_MONEDA_FACTURACION).Valor)

                                                         Dim obtener_invoicecurrency_ As MonedaGlobal = ObtenerMonedaPorObjectid(objectIdMoneda)

                                                         Dim cveMonedaApendice_invoicecurrency_ As String = obtener_invoicecurrency_?.aliasmoneda.FirstOrDefault(Function(a) a.Clave = "cvemonedaA05")?.Valor

                                                         '  System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: MONEDA PASADA POR LINQY {cveMonedaApendice_invoicecurrency_}")

                                                         'auxFactura_.invoicecurrency = .Campo(CamposFacturaComercial.CA_MONEDA_FACTURACION).ValorPresentacion


                                                         auxFactura_.invoicecurrency = cveMonedaApendice_invoicecurrency_

                                                         'System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: MONEDA QUE SE PUSO CHIDA: {auxFactura_.invoicecurrency}")

                                                         'auxFactura_.invoicecurrency = .Campo(CamposFacturaComercial.CA_MONEDA_FACTURACION).ValorPresentacion

                                                         auxFactura_.hasalienation = .Campo(CamposFacturaComercial.CP_APLICA_ENAJENACION).Valor

                                                         auxFactura_.hassubdivision = .Campo(CamposFacturaComercial.CA_APLICA_SUBDIVISION).Valor

                                                         auxFactura_.hasadditions = .Campo(CamposFacturaComercial.CP_APLICA_INCREMENTABLES).Valor

                                                         auxFactura_.numfactura_subdivision = .Campo(CamposFacturaComercial.CP_NUMERO_FACTURA_SUBDIVISION).Valor

                                                         auxFactura_.operationtype = TipoOperacion

                                                         _listaVehiculoFacturasComerciales.Add(auxFactura_)

                                                     End With

                                                     With y.encabezadoProveedor_

                                                         auxFactura_.supplier = New Supplier

                                                         auxFactura_.supplier.appliescertificate = .Campo(CamposFacturaComercial.CA_APLICA_CERTIFICADO).Valor

                                                         auxFactura_.supplier.linkagekey = .Campo(CamposFacturaComercial.CA_CVE_VINCULACION).Valor

                                                         auxFactura_.supplier.linkagedescription = .Campo(CamposFacturaComercial.CA_CVE_VINCULACION).ValorPresentacion

                                                         auxFactura_.supplier.valuationmethodkey = .Campo(CamposFacturaComercial.CP_CVE_METODO_VALORACION).Valor

                                                         auxFactura_.supplier.valuationmethoddescription = .Campo(CamposFacturaComercial.CP_CVE_METODO_VALORACION).ValorPresentacion

                                                         auxFactura_.supplier.certifiername = .Campo(CamposFacturaComercial.CA_APLICA_CERTIFICADO).Valor

                                                         If .Campo(CamposProveedorOperativo.CP_DESTINATARIO_PROVEEDOR).Valor IsNot Nothing Then

                                                             auxFactura_.supplier.isconsignee = .Campo(CamposProveedorOperativo.CP_DESTINATARIO_PROVEEDOR).Valor

                                                         End If

                                                     End With

                                                 End Sub)

                        If _listaVehiculoFacturasComerciales.Count > 0 Then

                            .ObjectReturned = _listaVehiculoFacturasComerciales

                            .SetOK()

                        Else

                            .ObjectReturned = Nothing

                            .SetOKBut(Me, "Facturas solicitadas no encontradas con estos datos")

                        End If

                    Else

                        .ObjectReturned = Nothing

                        .SetOKBut(Me, "Facturas solicitadas no encontradas con estos datos")

                    End If

                End Using

            Catch ex As Exception

                .ObjectReturned = Nothing

                .SetOKBut(Me, $"Ha ocurrido un error {ex}")

            End Try

        End With

        Return Estado

    End Function

    Private Function ObtenerEstructuraCommercialInvoice(ByVal numeroFactura_ As String)

        With Estado

            Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos(21)

                Try
                    enlaceDatos_.EnvironmentOnline = _environmentOnline

                    Dim operationsDB_ = enlaceDatos_.GetMongoCollection(Of CommercialInvoiceAnalysis)("Reg012CommercialInvoicesAnalysis")

                    Dim filter_ = Builders(Of CommercialInvoiceAnalysis).Filter.Eq(Function(x) x.invoicenumber, numeroFactura_)

                    Dim result_ = operationsDB_.Find(filter_).Limit(1).ToList()

                    If result_.Count > 0 Then

                        .ObjectReturned = result_

                        .SetOK()
                    Else
                        .ObjectReturned = Nothing

                        .SetOKBut(Me, "No se encontraron facturas solicitadas")

                    End If

                Catch ex As Exception

                    .ObjectReturned = Nothing

                    .SetError(Me, $"Ha ocurrido un error_ {ex}")

                End Try

            End Using

        End With

        Return Estado

    End Function

    Private Function ConsultarExistenciaFactura_(ByVal numeroFactura_ As String,
                                                       ByVal idProveedor_ As ObjectId, ByVal fechaFactura_ As String) As TagWatcher

        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos(21)

            iEnlace_.EnvironmentOnline = _environmentOnline

            Dim collection_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)((New ConstructorFacturaComercial).GetType.Name)

            Dim numeroFolioFactura_ As FilterDefinition(Of OperacionGenerica) = Builders(Of OperacionGenerica).Filter.
                            Eq(Function(x) DirectCast(x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Encabezado")(0).Nodos(0).Nodos(2).Nodos(0), Campo).Valor, numeroFactura_)

            Dim fechaFacturaComercial_ As FilterDefinition(Of OperacionGenerica) = Builders(Of OperacionGenerica).Filter.
                            Eq(Function(x) DirectCast(x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Encabezado")(0).Nodos(0).Nodos(4).Nodos(0), Campo).ValorPresentacion, fechaFactura_)

            Dim idProveedorOperativo_ As FilterDefinition(Of OperacionGenerica) = Builders(Of OperacionGenerica).Filter.
                            Eq(Function(x) DirectCast(x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Encabezado")(1).Nodos(0).Nodos(0).Nodos(0), Campo).Valor, idProveedor_.ToString)

            Dim filtroCombinado_ As FilterDefinition(Of OperacionGenerica) = Builders(Of OperacionGenerica).Filter.And(numeroFolioFactura_, idProveedorOperativo_, fechaFacturaComercial_)

            Dim result_ = collection_.Aggregate().
                                      Match(Builders(Of OperacionGenerica).Filter.Or(filtroCombinado_)).
                                      Project(Function(x) New With {Key .id_ = x.Id,
                                      Key .publicado_ = x.Publicado,
                                      Key .firmado_ = x.FirmaElectronica}).ToList()

            With Estado

                If result_.Count > 0 Then

                    .ObjectReturned = result_(0)

                    .SetOKBut(Me, "Factura comercial ya registrada")
                Else

                    .SetOK()

                End If

            End With

        End Using

        Return Estado

    End Function

    Private Function ConsultarExistenciaFactura_(ByVal numeroFactura_ As String,
                                                 ByVal idCliente_ As ObjectId,
                                                 ByVal idProveedor_ As ObjectId, ByVal fechaFactura_ As String) As TagWatcher
        With Estado

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos(21)

                iEnlace_.EnvironmentOnline = _environmentOnline

                Dim collection_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)((New ConstructorFacturaComercial).GetType.Name)

                Dim numeroFolioFactura_ As FilterDefinition(Of OperacionGenerica) = Builders(Of OperacionGenerica).Filter.
                            Eq(Function(x) DirectCast(x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Encabezado")(0).Nodos(0).Nodos(2).Nodos(0), Campo).Valor, numeroFactura_)

                Dim fechaFacturaComercial_ As FilterDefinition(Of OperacionGenerica) = Builders(Of OperacionGenerica).Filter.
                            Eq(Function(x) DirectCast(x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Encabezado")(0).Nodos(0).Nodos(4).Nodos(0), Campo).ValorPresentacion, fechaFactura_)

                Dim idcliente As FilterDefinition(Of OperacionGenerica) = Builders(Of OperacionGenerica).Filter.
                            Eq(Function(x) DirectCast(x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Encabezado")(0).Nodos(0).Nodos(8).Nodos(0), Campo).Valor, idCliente_.ToString)

                Dim idProveedorOperativo_ As FilterDefinition(Of OperacionGenerica) = Builders(Of OperacionGenerica).Filter.
                            Eq(Function(x) DirectCast(x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Encabezado")(1).Nodos(0).Nodos(0).Nodos(0), Campo).Valor, idProveedor_.ToString)

                Dim filtroCombinado_ As FilterDefinition(Of OperacionGenerica) = Builders(Of OperacionGenerica).Filter.And(numeroFolioFactura_, idcliente, idProveedorOperativo_, fechaFacturaComercial_)

                Dim result_ = collection_.Aggregate().
                                      Match(Builders(Of OperacionGenerica).Filter.Or(filtroCombinado_)).
                                      Project(Function(x) New With {Key .id_ = x.Id,
                                      Key .publicado_ = x.Publicado,
                                      Key .firmado_ = x.FirmaElectronica}).ToList()

                If result_.Count > 0 Then

                    .SetOKBut(Me, "Factura comercial ya registrada")
                Else

                    .SetOK()

                End If

            End Using

        End With

        Return Estado

    End Function

    Protected Function GenerarSecuenciaDocumentoElectronico(ByRef tipoSecuencia_ As String) As Secuencia

        _controladorSecuencias = Nothing
        _secuencia = Nothing
        _controladorSecuencias = New ControladorSecuencia
        _secuencia = New Secuencia

        Dim estadoSecuencia_ As TagWatcher = _controladorSecuencias.Generar(nombre_:=tipoSecuencia_,
                                                          tipoSecuencia_:=1,
                                                          compania_:=1, area_:=1,
                                                          subtipoSecuencia_:=1,
                                                          enviroment_:=_environmentOnline)

        If estadoSecuencia_.Status = TypeStatus.Ok Then

            _secuencia = estadoSecuencia_.ObjectReturned

        End If

        Return _secuencia

    End Function

    Private Function ObtenerObjectId(campo As Object) As Object
        ' 1. Verificación básica de nulidad
        If campo Is Nothing OrElse campo.Valor Is Nothing Then Return Nothing

        Dim valorRaw As Object = campo.Valor

        ' 2. Si ya es un ObjectId (a veces el driver ya lo trae convertido), lo regresamos tal cual
        If TypeOf valorRaw Is ObjectId Then
            If DirectCast(valorRaw, ObjectId) = ObjectId.Empty Then Return Nothing
            Return valorRaw
        End If

        ' 3. Si es un String, lo procesamos
        Dim valorStr As String = valorRaw.ToString().Trim()

        ' 4. Validamos que no sea la cadena de ceros o esté vacía
        If String.IsNullOrEmpty(valorStr) OrElse valorStr.Replace("0", "").Length = 0 Then
            Return Nothing
        End If

        ' 5. Validamos que tenga el formato hexadecimal de 24 caracteres de MongoDB
        If valorStr.Length = 24 AndAlso System.Text.RegularExpressions.Regex.IsMatch(valorStr, "^[0-9a-fA-F]+$") Then
            Try
                Return ObjectId.Parse(valorStr)
            Catch
                Return Nothing
            End Try
        End If

        Return Nothing
    End Function

    Private Function ObtenerValor(campo As Object, Optional esPresentacion As Boolean = False) As Object
        If campo IsNot Nothing Then
            Return If(esPresentacion, campo.ValorPresentacion, campo.Valor)
        End If
        Return Nothing
    End Function

    Private Function GenerarEstructuraFacturaSubdividible(ByVal constructorFacturaComercial_ As ConstructorFacturaComercial,
                                                          ByVal _idPreasignacionFacturaOriginal As ObjectId) As FacturaSubdividible

        Dim secuenciaSubdivision_ = GenerarSecuenciaDocumentoElectronico(SecuenciasComercioExterior.SubdivisionFacturaComercial.ToString)

        Dim listaUnidadesParcializables_ As List(Of String) = New List(Of String) _
            From {"1", "2", "3", "4", "5", "8", "10", "13", "14", "16", "22"}

        _facturaSubdividible = New FacturaSubdividible

        Try

            With _facturaSubdividible

                Dim constructor_ = DirectCast(constructorFacturaComercial_, ConstructorFacturaComercial)

                Dim seccionFacturaEncabezado_ = constructor_.EstructuraDocumento.Parts.Item("Encabezado")(0)

                Dim seccionFacturaProveedor_ = constructor_.EstructuraDocumento.Parts.Item("Encabezado")(1)

                .idfacturaoriginal = _idPreasignacionFacturaOriginal

                .numerofactura = constructor_.FolioDocumento

                .cliente = constructor_.NombrePropietario

                .proveedor = seccionFacturaProveedor_.Campo(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR).ValorPresentacion

                .valorfactura_general = seccionFacturaEncabezado_.Campo(CamposFacturaComercial.CP_VALOR_FACTURA).Valor
                .cve_moneda_valorfactura = ObtenerValor(seccionFacturaEncabezado_.Campo(CamposFacturaComercial.CA_MONEDA_FACTURACION))
                .moneda_valorfactura = ObtenerValor(seccionFacturaEncabezado_.Campo(CamposFacturaComercial.CA_MONEDA_FACTURACION), True)

                .valormercancia_general = seccionFacturaEncabezado_.Campo(CamposFacturaComercial.CP_VALOR_MERCANCIA).Valor

                .cve_moneda_mercancia = ObtenerValor(seccionFacturaEncabezado_.Campo(CamposFacturaComercial.CP_MONEDA_VALOR_MERCANCIA))

                .moneda_mercancia_general = ObtenerValor(seccionFacturaEncabezado_.Campo(CamposFacturaComercial.CP_MONEDA_VALOR_MERCANCIA), True)

                .actualizacion = Date.Today()

                .status_id = FacturaSubdividible.EstadoSubdivision.Abierta

                .tipo_cierre = FacturaSubdividible.TipoCierre.SinCierre

                .cierre_manual = Nothing

                .mas_informacion = New DetalleMasInformacion _
                    With {
                    .fecha_factura_original = ObtenerValor(seccionFacturaEncabezado_.Campo(CamposFacturaComercial.CA_FECHA_FACTURA)),
                    .numero_acuse_valor = seccionFacturaEncabezado_.Campo(CamposAcuseValor.CA_NUMERO_ACUSEVALOR).Valor,
                    .FechaCreacion = constructor_.FechaCreacion,
                    .UsuarioGenerador = constructor_.UsuarioGenerador}

                .estado = 1

                .control_saldos = New List(Of DetalleControlSaldo)

                Dim indice_ As Integer = 0

                Dim seccionItemsFactura_ = constructor_.EstructuraDocumento.Parts.Item("Cuerpo")(0)

                For Each item_ In seccionItemsFactura_.Nodos(0).Nodos

                    Dim controlSaldo_ As New DetalleControlSaldo

                    Dim auxnumpartecompleto_ = ObtenerValor(item_.Campo(CamposFacturaComercial.CA_NUMERO_PARTE_PARTIDA), True)

                    Dim auxnumparte_ = ExtraerClavesyDescripcionesStrings(auxnumpartecompleto_, "|")

                    Dim numeroparte_ = ExtraerClavesyDescripcionesStrings(auxnumparte_.description_, "|")

                    With controlSaldo_

                        .sec = indice_ + 1
                        .idProducto = ObtenerObjectId(item_.Campo(CamposFacturaComercial.CP_OBJECTID_PRODUCTOS))
                        .numeropartida = ObtenerValor(item_.Campo(CamposFacturaComercial.CP_NUMERO_PARTIDA))
                        .numeroparte = numeroparte_.clave_
                        .numeropartecompleto = auxnumpartecompleto_
                        .descripcion = ObtenerValor(item_.Campo(CamposFacturaComercial.CA_DESCRIPCION_PARTE_PARTIDA))
                        .cantidad_comercial_original = ObtenerValor(item_.Campo(CamposFacturaComercial.CA_CANTIDAD_COMERCIAL_PARTIDA))
                        .cantidad_comercial_disponible = ObtenerValor(item_.Campo(CamposFacturaComercial.CA_CANTIDAD_COMERCIAL_PARTIDA))
                        .cve_unidad_medida_comercial = ObtenerValor(item_.Campo(CamposFacturaComercial.CA_UNIDAD_MEDIDA_COMERCIAL_PARTIDA))
                        .unidad_medida_comercial = ObtenerValor(item_.Campo(CamposFacturaComercial.CA_UNIDAD_MEDIDA_COMERCIAL_PARTIDA), True)
                        .valor_mercancia_original = ObtenerValor(item_.Campo(CamposFacturaComercial.CA_VALOR_MERCANCIA_PARTIDA))
                        .idmoneda_valor_mercancia_original = ObtenerValor(item_.Campo(CamposFacturaComercial.CA_MONEDA_MERCANCIA_PARTIDA))
                        .moneda_valor_mercancia_original = ObtenerValor(item_.Campo(CamposFacturaComercial.CA_MONEDA_MERCANCIA_PARTIDA), True)
                        .precio_unitario_original = ObtenerValor(item_.Campo(CamposFacturaComercial.CA_PRECIO_UNITARIO_PARTIDA))
                        .idmoneda_precio_unitario_original = ObtenerValor(item_.Campo(CamposFacturaComercial.CP_MONEDA_PRECIO_UNITARIO))
                        .moneda_precio_unitario_original = ObtenerValor(item_.Campo(CamposFacturaComercial.CP_MONEDA_PRECIO_UNITARIO), True)
                        .disponible = True
                        .unidad_parcializable = listaUnidadesParcializables_.Contains(ObtenerValor(item_.Campo(CamposFacturaComercial.CA_UNIDAD_MEDIDA_COMERCIAL_PARTIDA), False))
                        .cantidad_tarifa_original = ObtenerValor(item_.Campo(CamposFacturaComercial.CA_CANTIDAD_TARIFA_PARTIDA))
                        .cve_unidad_medida_tarifa = ObtenerValor(item_.Campo(CamposFacturaComercial.CA_UNIDAD_MEDIDA_TARIFA_PARTIDA))
                        .unidad_medida_tarifa = ObtenerValor(item_.Campo(CamposFacturaComercial.CA_UNIDAD_MEDIDA_TARIFA_PARTIDA), True)
                        .descripcion_merca_original = ObtenerValor(item_.Campo(CamposFacturaComercial.CA_DESCRIPCION_PARTE_PARTIDA_ORIGINAL))
                        .descripcion_merca_cove = ObtenerValor(item_.Campo(CamposFacturaComercial.CA_DESCRIPCION_COVE_PARTIDA))
                        .val_fact_partida = ObtenerValor(item_.Campo(CamposFacturaComercial.CA_VALOR_FACTURA_PARTIDA))
                        .idmoneda_val_fact_partida = ObtenerValor(item_.Campo(CamposFacturaComercial.CP_MONEDA_FACTURA_PARTIDA))
                        .moneda_val_fact_partida = ObtenerValor(item_.Campo(CamposFacturaComercial.CP_MONEDA_FACTURA_PARTIDA), True)
                        .peso_neto_partida = ObtenerValor(item_.Campo(CamposFacturaComercial.CA_PESO_NETO_PARTIDA))
                        .pais_origen = ObtenerValor(item_.Campo(CamposFacturaComercial.CA_PAIS_ORIGEN_PARTIDA), True)
                        .id_pais_origen = ObtenerValor(item_.Campo(CamposFacturaComercial.CA_PAIS_ORIGEN_PARTIDA))
                        .cve_metodo_val_partida = ObtenerValor(item_.Campo(CamposFacturaComercial.CA_CVE_METODO_VALORACION_PARTIDA))
                        .metodo_val_partida = ObtenerValor(item_.Campo(CamposFacturaComercial.CA_CVE_METODO_VALORACION_PARTIDA), True)
                        .orden_compra_partida = ObtenerValor(item_.Campo(CamposFacturaComercial.CP_ORDEN_COMPRA_PARTIDA))
                        .fraccion = ObtenerValor(item_.Campo(CamposFacturaComercial.CA_FRACCION_ARANCELARIA_PARTIDA))
                        .fraccion_descripcion = ObtenerValor(item_.Campo(CamposFacturaComercial.CA_FRACCION_ARANCELARIA_PARTIDA), True)
                        .nico = ObtenerValor(item_.Campo(CamposFacturaComercial.CA_FRACCION_NICO_PARTIDA))
                        .nico_descripcion = ObtenerValor(item_.Campo(CamposFacturaComercial.CA_FRACCION_NICO_PARTIDA), True)
                        .lote_part = ObtenerValor(item_.Campo(CamposFacturaComercial.CA_LOTE_PARTIDA))
                        .numero_serie_part = ObtenerValor(item_.Campo(CamposFacturaComercial.CA_NUMERO_SERIE_PARTIDA))
                        .marca_part = ObtenerValor(item_.Campo(CamposFacturaComercial.CA_MARCA_PARTIDA))
                        .modelo_part = ObtenerValor(item_.Campo(CamposFacturaComercial.CA_MODELO_PARTIDA))
                        .submodelo_part = ObtenerValor(item_.Campo(CamposFacturaComercial.CA_SUBMODELO_PARTIDA))
                        .kilometraje_part = ObtenerValor(item_.Campo(CamposFacturaComercial.CA_KILOMETRAJE_PARTIDA))
                        .timestamp_part = ObtenerValor(item_.Campo(CamposProducto.CP_TIMESTAMP))
                        .identity = ObtenerValor(item_.Campo(CamposGlobales.CP_IDENTITY))

                    End With

                    indice_ += 1

                    _facturaSubdividible.control_saldos.Add(controlSaldo_)

                Next

            End With

        Catch ex As Exception

            Dim error_ = $"Error: Factura subdividible no generada_{ex}"

        End Try

        Return _facturaSubdividible

    End Function


    Private Function GenerarFacturaSubdividible(ByVal constructorFacturaComercial_ As ConstructorFacturaComercial,
                                                ByVal objectidPreasignacionFacturaOriginal_ As ObjectId) As TagWatcher

        With Estado

            Dim estructuraFacturaSubdividible_ As FacturaSubdividible = GenerarEstructuraFacturaSubdividible(constructorFacturaComercial_, objectidPreasignacionFacturaOriginal_)

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos(21)

                iEnlace_.EnvironmentOnline = _environmentOnline

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of FacturaSubdividible)("Reg003FacturasSubdividibles")

                Try
                    Dim result_ = operationsDB_.InsertOneAsync(estructuraFacturaSubdividible_)

                    If result_.Id <> Nothing Then

                        .SetOK()

                    Else

                        .SetError(Me, "Factura subdividible no insertada")

                    End If

                Catch ex As Exception

                    .SetError(Me, $"Error: GenerarFacturaSubdividible_ {ex}")

                End Try

            End Using

        End With

        Return Estado

    End Function

    Protected Function ExtraerClavesyDescripcionesStrings(ByVal textoaseparar_ As String, separador_ As String) As TextoSeparado

        Dim partes As New TextoSeparado()

        If String.IsNullOrEmpty(textoaseparar_) OrElse String.IsNullOrEmpty(separador_) Then
            partes.clave_ = ""
            partes.description_ = ""
            Return partes
        End If

        Dim indiceSeparador As Integer = textoaseparar_.IndexOf(separador_)

        If indiceSeparador <> -1 Then
            partes.clave_ = textoaseparar_.Substring(0, indiceSeparador).Trim()
            partes.description_ = textoaseparar_.Substring(indiceSeparador + separador_.Length).Trim()
        Else
            partes.clave_ = textoaseparar_
            partes.description_ = ""
        End If

        Return partes

    End Function

    Protected Function LimpiarTexto(texto As String, caracteresAEliminar As String) As String
        Dim resultado As String = texto

        For Each c As Char In caracteresAEliminar
            resultado = resultado.Replace(c.ToString(), "")
        Next

        Return resultado.Trim()
    End Function

    Protected Function ParsearValor(texto As String, Optional tipoForzado As String = "") As Object

        texto = texto.Trim()

        ' Si se especifica tipo, forzar conversión
        Select Case tipoForzado.ToLower()
            Case "integer"
                Return Integer.Parse(texto)
            Case "double"
                Return Double.Parse(texto)
            Case "decimal"
                Return Decimal.Parse(texto)
            Case "boolean"
                Return Boolean.Parse(texto)
            Case "datetime"
                Return DateTime.Parse(texto)
        End Select

        ' Si no hay tipo forzado, autodetectar
        Dim numeroEntero As Integer

        If Integer.TryParse(texto, numeroEntero) Then
            Return numeroEntero
        End If

        Dim numeroDecimal As Decimal
        If Decimal.TryParse(texto, numeroDecimal) Then
            Return numeroDecimal
        End If

        Dim numeroDouble As Double
        If Double.TryParse(texto, numeroDouble) Then
            Return numeroDouble
        End If

        Dim valorBooleano As Boolean
        If Boolean.TryParse(texto, valorBooleano) Then
            Return valorBooleano
        End If

        Dim fecha As DateTime
        If DateTime.TryParse(texto, fecha) Then
            Return fecha
        End If

        ' Si no encaja, devolver string
        Return texto
    End Function

    Private Function ValidarEntero(valor_ As Object, Optional defecto_ As Integer = 0) As Integer
        Try
            ' Si el valor es nulo o es un string vacío o solo espacios
            If valor_ Is Nothing OrElse String.IsNullOrWhiteSpace(valor_.ToString()) Then
                Return defecto_
            End If

            ' Intentamos la conversión
            Dim resultado_ As Integer
            If Integer.TryParse(valor_.ToString(), resultado_) Then
                Return resultado_
            Else
                Return defecto_
            End If
        Catch
            Return defecto_
        End Try
    End Function

    Private Function ObtenerFacturasComercialesSinVincularAPedimentos(ByVal listaObjectsIdFacturas_ As List(Of ObjectId)) As TagWatcher

        With Estado

            Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos(21)

                Try

                    enlaceDatos_.EnvironmentOnline = _environmentOnline

                    Dim collection_ = enlaceDatos_.GetMongoCollection(Of OperacionGenerica)((New ConstructorFacturaComercial).GetType.Name)

                    Dim filtroFacturas_ As FilterDefinition(Of OperacionGenerica) =
                        Builders(Of OperacionGenerica).Filter.In(Of ObjectId)("_id", listaObjectsIdFacturas_)

                    Dim filtroFacturaPublicada_ As FilterDefinition(Of OperacionGenerica) =
                          Builders(Of OperacionGenerica).Filter.Eq(Of Boolean)("Publicado", True)

                    Dim filtroFacturaFirmada_ As FilterDefinition(Of OperacionGenerica) =
                          Builders(Of OperacionGenerica).Filter.Ne(Of String)("FirmaElectronica", "")

                    Dim filtroFacturaActiva_ As FilterDefinition(Of OperacionGenerica) =
                          Builders(Of OperacionGenerica).Filter.Eq(Of Int32)("estado", 1)

                    Dim filtroTipoOperacion_ As FilterDefinition(Of OperacionGenerica) =
                        Builders(Of OperacionGenerica).Filter.Eq(Function(x) DirectCast(x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts("Encabezado")(0).Nodos(0).Nodos(0).Nodos(0), Campo).Valor, TipoOperacion)

                    Dim filtroNoAsociadoAPedimento_ As FilterDefinition(Of OperacionGenerica) =
                        Builders(Of OperacionGenerica).Filter.Eq(Function(x) DirectCast(x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts("Encabezado")(0).Nodos(0).Nodos(45).Nodos(0), Campo).Valor, False)

                    Dim filtroCombinado_ As FilterDefinition(Of OperacionGenerica) =
                    Builders(Of OperacionGenerica).Filter.And(
                        filtroFacturas_,
                        filtroFacturaPublicada_,
                        filtroFacturaFirmada_,
                        filtroFacturaActiva_,
                        filtroTipoOperacion_,
                        filtroNoAsociadoAPedimento_)

                    Dim result_ = collection_.Aggregate().
                    Match(filtroCombinado_).
                    Project(Function(y) New With {
                        Key .id_ = y.Id,
                        Key .objectCliente_ = y.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.ObjectIdPropietario,
                        Key .fuente_ = y.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente}).ToList()

                    If result_.Count > 0 Then

                        _listaVehiculoFacturasComerciales = New List(Of ICommercialInvoiceCustomsDocument)

                        result_.ToList().ForEach(Sub(y)

                                                     _documentoElectronicoFacturacomercial = DirectCast(y.fuente_, ConstructorFacturaComercial)

                                                     _listaVehiculoFacturasComerciales.Add(MontarDatosEnCommercialInvoicePedimento(_documentoElectronicoFacturacomercial, y.id_, y.objectCliente_))

                                                 End Sub)

                        If _listaVehiculoFacturasComerciales.Count > 0 Then

                            .ObjectReturned = _listaVehiculoFacturasComerciales

                            .SetOK()

                        Else

                            .SetOKBut(Me, "Facturas solicitadas no encontradas con estos datos")

                        End If

                    Else

                        .SetOKBut(Me, "Facturas solicitadas no encontradas con estos datos")

                    End If

                Catch ex As Exception

                    .SetOKBut(Me, $"Ha ocurrido un Error {ex}")

                End Try

            End Using

        End With

        Return Estado

    End Function

    'Private Function ObtenerMonedaPorObjectid(ByVal idMoneda_ As ObjectId) As MonedaGlobal

    '    System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: entrando a obtener moneda por objectid.")

    '    Dim claveCache_ As String = $"cacheClaveMoneda_{idMoneda_}"

    '    Dim _cacheClaveMoneda = CType(HttpRuntime.Cache(claveCache_), MonedaGlobal)

    '    Try
    '        If _cacheClaveMoneda Is Nothing Then

    '            System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: La clave era nothing vamos a buscarla")

    '            _controladorMonedas = New ControladorMonedas

    '            System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: el id de la moneda es: {idMoneda_}")

    '            Dim listaMonedas_ As List(Of MonedaGlobal) = _controladorMonedas.BuscarMonedas("", idMoneda_)

    '            System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: Lo que devuelve el controlador de monedas es: {listaMonedas_.Count}")

    '            _cacheClaveMoneda = listaMonedas_.LastOrDefault()

    '            System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: la moneda chida es: { _cacheClaveMoneda._id.ToString}")

    '            System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: la moneda chida es: { _cacheClaveMoneda.nombremonedaing}")

    '            HttpRuntime.Cache.Insert(claveCache_, _cacheClaveMoneda, Nothing, DateTime.Now.AddMinutes(5), Caching.Cache.NoSlidingExpiration)

    '        End If

    '    Catch ex As Exception

    '        System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: hubo una excepcion {ex}")

    '        Return Nothing

    '    End Try

    '    System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice: Retonando la moneda : { _cacheClaveMoneda.nombremonedaesp}")
    '    Return _cacheClaveMoneda

    'End Function

    Private Function ObtenerMonedaPorObjectid(ByVal idMoneda_ As ObjectId) As MonedaGlobal
        Dim claveCache_ As String = $"cacheClaveMoneda_{idMoneda_.ToString()}"
        Dim _cacheClaveMoneda As MonedaGlobal = TryCast(_cache(claveCache_), MonedaGlobal)

        Try
            ' Validar que lo que está en caché realmente corresponde al ObjectId pedido
            If _cacheClaveMoneda IsNot Nothing AndAlso _cacheClaveMoneda._id <> idMoneda_ Then
                _cache.Remove(claveCache_)
                _cacheClaveMoneda = Nothing
            End If

            If _cacheClaveMoneda Is Nothing Then
                _controladorMonedas = New ControladorMonedas
                Dim listaMonedas_ As List(Of MonedaGlobal) = _controladorMonedas.BuscarMonedas("", idMoneda_)

                ' Filtrar por ObjectId exacto
                _cacheClaveMoneda = listaMonedas_.FirstOrDefault(Function(m) m._id = idMoneda_)

                If _cacheClaveMoneda Is Nothing Then
                    Return Nothing
                End If

                ' Configurar expiración absoluta de 5 minutos, igual que antes
                Dim policy_ As New CacheItemPolicy() With {
                .AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5)
            }

                _cache.Set(claveCache_, _cacheClaveMoneda, policy_)
            End If

        Catch ex As Exception
            Return Nothing
        End Try

        Return _cacheClaveMoneda

    End Function

    Private Function SeccionItems(ByRef auxFactura_ As Object,
                                  ByRef constructorFact_ As ConstructorFacturaComercial, ByRef esPedimento_ As Boolean) As Object

        auxFactura_.items = New List(Of IItem)

        Dim secuencia_ = 0

        Dim seccionItemsFactura_ = constructorFact_.EstructuraDocumento.Parts.Item("Cuerpo")(0)

        Dim i_ = 0

        For Each item_ In seccionItemsFactura_.Nodos(0).Nodos

            Dim itemAux_ As New Item

            Dim auxnumpartecompleto_ = ExtraerClavesyDescripcionesStrings(item_.Campo(CamposFacturaComercial.CA_NUMERO_PARTE_PARTIDA).ValorPresentacion, "-")

            Dim auxnumparte_ = ExtraerClavesyDescripcionesStrings(auxnumpartecompleto_.clave_, "|")

            Dim auxUMC_ = ExtraerClavesyDescripcionesStrings(item_.Campo(CamposFacturaComercial.CA_UNIDAD_MEDIDA_COMERCIAL_PARTIDA).ValorPresentacion, "-")

            Dim auxUMT_ = ExtraerClavesyDescripcionesStrings(item_.Campo(CamposFacturaComercial.CA_UNIDAD_MEDIDA_TARIFA_PARTIDA).ValorPresentacion, "-")

            Dim auxNico_ = ExtraerClavesyDescripcionesStrings(item_.Campo(CamposFacturaComercial.CA_FRACCION_NICO_PARTIDA).Valor, "-") ''DESCRPIPCION EN VALOR PRESENTACION

            Dim auxFraccion_ = ExtraerClavesyDescripcionesStrings(item_.Campo(CamposFacturaComercial.CA_FRACCION_ARANCELARIA_PARTIDA).Valor, "-") ''DESCRIPCON EN VALOR RPESENTARION

            Dim auxmetodoVal_ = ExtraerClavesyDescripcionesStrings(item_.Campo(CamposFacturaComercial.CA_CVE_METODO_VALORACION_PARTIDA).ValorPresentacion, "-")

            Dim auxpaisdestino_ = ExtraerClavesyDescripcionesStrings(item_.Campo(CamposFacturaComercial.CA_PAIS_DESTINO_PARTIDA).ValorPresentacion, "-")

            Dim auxpaisorigen_ = ExtraerClavesyDescripcionesStrings(item_.Campo(CamposFacturaComercial.CA_PAIS_ORIGEN_PARTIDA).ValorPresentacion, "-")

            itemAux_.productid = item_.Campo(CamposFacturaComercial.CP_OBJECTID_PRODUCTOS).Valor

            itemAux_.sequence = item_.Campo(CamposFacturaComercial.CP_NUMERO_PARTIDA).Valor

            Dim numeroparte_ = ExtraerClavesyDescripcionesStrings(auxnumparte_.description_, "|")

            itemAux_.partnumberkey = auxnumparte_.clave_

            itemAux_.partnumber = numeroparte_.clave_ ''volverla a pasar

            itemAux_.partnumberdescription = numeroparte_.description_ ''descripcion factura original

            itemAux_.commercialquantity = item_.Campo(CamposFacturaComercial.CA_CANTIDAD_COMERCIAL_PARTIDA).Valor

            itemAux_.unitquantitycommercial = ValidarEntero(auxUMC_.clave_)

            itemAux_.commercialunitdescription = auxUMC_.description_

            itemAux_.description = item_.Campo(CamposFacturaComercial.CA_DESCRIPCION_PARTE_PARTIDA).Valor

            itemAux_.value = item_.Campo(CamposFacturaComercial.CA_VALOR_FACTURA_PARTIDA).Valor

            If item_.Campo(CamposFacturaComercial.CP_MONEDA_FACTURA_PARTIDA).Valor IsNot Nothing Then

                If esPedimento_ Then

                    Dim cveMonedaApendice_currency_ As String = ObtenerMonedaPorObjectid(ObjectId.Parse(item_.Campo(CamposFacturaComercial.CP_MONEDA_FACTURA_PARTIDA).Valor)) _
           .aliasmoneda.FirstOrDefault(Function(a) a.Clave = "cvemonedaA05")?.Valor

                    itemAux_.currency = cveMonedaApendice_currency_

                Else

                    itemAux_.currency = item_.Campo(CamposFacturaComercial.CP_MONEDA_FACTURA_PARTIDA).ValorPresentacion

                End If

            End If

            itemAux_.usdvalue = item_.Campo(CamposFacturaComercial.CA_VALOR_DOLARES_PARTIDA).Valor

            If item_.Campo(CamposFacturaComercial.CP_MONEDA_VALOR_DOLARES_PARTIDA).Valor IsNot Nothing Then

                If esPedimento_ Then

                    Dim cveMonedaApendice_usdcurrency As String = ObtenerMonedaPorObjectid(ObjectId.Parse(item_.Campo(CamposFacturaComercial.CP_MONEDA_VALOR_DOLARES_PARTIDA).Valor)) _
          .aliasmoneda.FirstOrDefault(Function(a) a.Clave = "cvemonedaA05")?.Valor

                    itemAux_.usdcurrency = cveMonedaApendice_usdcurrency

                Else

                    itemAux_.usdcurrency = item_.Campo(CamposFacturaComercial.CP_MONEDA_VALOR_DOLARES_PARTIDA).ValorPresentacion

                End If

            End If

            itemAux_.valueunitprice = item_.Campo(CamposFacturaComercial.CA_VALOR_UNITARIO_PARTIDA).Valor

            If item_.Campo(CamposFacturaComercial.CA_MONEDA_VALOR_UNITARIO_PARTIDA).Valor IsNot Nothing Then

                If esPedimento_ Then

                    Dim cveMonedaApendice_currencyvalueunit As String = ObtenerMonedaPorObjectid(ObjectId.Parse(item_.Campo(CamposFacturaComercial.CA_MONEDA_VALOR_UNITARIO_PARTIDA).Valor)) _
             .aliasmoneda.FirstOrDefault(Function(a) a.Clave = "cvemonedaA05")?.Valor

                    itemAux_.currencyvalueunitprice = cveMonedaApendice_currencyvalueunit
                Else

                    itemAux_.currencyunitprice = item_.Campo(CamposFacturaComercial.CA_MONEDA_VALOR_UNITARIO_PARTIDA).ValorPresentacion

                End If

            End If

            itemAux_.merchandisevalue = item_.Campo(CamposFacturaComercial.CA_VALOR_MERCANCIA_PARTIDA).Valor

            If item_.Campo(CamposFacturaComercial.CA_MONEDA_MERCANCIA_PARTIDA).Valor IsNot Nothing Then

                If esPedimento_ Then

                    Dim cveMonedaApendice_merchandisecurrency As String = ObtenerMonedaPorObjectid(ObjectId.Parse(item_.Campo(CamposFacturaComercial.CA_MONEDA_MERCANCIA_PARTIDA).Valor)) _
      .aliasmoneda.FirstOrDefault(Function(a) a.Clave = "cvemonedaA05")?.Valor

                    itemAux_.merchandisecurrency = cveMonedaApendice_merchandisecurrency
                Else

                    itemAux_.merchandisecurrency = item_.Campo(CamposFacturaComercial.CA_MONEDA_MERCANCIA_PARTIDA).ValorPresentacion

                End If

            End If

            itemAux_.unitprice = item_.Campo(CamposFacturaComercial.CA_PRECIO_UNITARIO_PARTIDA).Valor

            If item_.Campo(CamposFacturaComercial.CP_MONEDA_PRECIO_UNITARIO).Valor IsNot Nothing Then

                If esPedimento_ Then

                    Dim cveMonedaApendice_currencyunitprice As String = ObtenerMonedaPorObjectid(ObjectId.Parse(item_.Campo(CamposFacturaComercial.CP_MONEDA_PRECIO_UNITARIO).Valor)) _
                   .aliasmoneda.FirstOrDefault(Function(a) a.Clave = "cvemonedaA05")?.Valor

                    itemAux_.currencyunitprice = cveMonedaApendice_currencyunitprice

                Else

                    itemAux_.currencyunitprice = item_.Campo(CamposFacturaComercial.CP_MONEDA_PRECIO_UNITARIO).ValorPresentacion

                End If

            End If

            itemAux_.valorAgregado = item_.Campo(CamposFacturaComercial.CP_VALOR_AGREGADO_ITEM).Valor

            If item_.Campo(CamposFacturaComercial.CP_MONEDA_VALOR_AGREGADO_ITEM).Valor IsNot Nothing Then

                If esPedimento_ Then

                    Dim cveMonedaApendice_monedaValorAgregado As String = ObtenerMonedaPorObjectid(ObjectId.Parse(item_.Campo(CamposFacturaComercial.CP_MONEDA_VALOR_AGREGADO_ITEM).Valor)) _
              .aliasmoneda.FirstOrDefault(Function(a) a.Clave = "cvemonedaA05")?.Valor

                    itemAux_.monedaValorAgregado = cveMonedaApendice_monedaValorAgregado

                Else

                    itemAux_.monedaValorAgregado = item_.Campo(CamposFacturaComercial.CP_MONEDA_VALOR_AGREGADO_ITEM).ValorPresentacion

                End If

            End If

            itemAux_.netweight = item_.Campo(CamposFacturaComercial.CA_PESO_NETO_PARTIDA).Valor

            If auxpaisdestino_ IsNot Nothing Then

                If auxpaisdestino_.clave_ <> "" AndAlso auxpaisdestino_.description_ <> "" Then

                    itemAux_.destinationcountrykey = auxpaisdestino_.clave_

                    itemAux_.destinationcountry = auxpaisdestino_.description_
                End If

            End If

            If auxpaisorigen_ IsNot Nothing Then

                itemAux_.origincountrykey = auxpaisorigen_.clave_

                itemAux_.origincountry = auxpaisorigen_.description_

            End If

            If auxmetodoVal_ IsNot Nothing Then

                itemAux_.valuationmethodkey = auxmetodoVal_.clave_

                itemAux_.valuationmethoddescription = auxmetodoVal_.description_

            End If

            itemAux_.purchaseorder = item_.Campo(CamposFacturaComercial.CP_ORDEN_COMPRA_PARTIDA).Valor

            itemAux_.quantity = item_.Campo(CamposFacturaComercial.CP_CANTIDAD_FACTURA_PARTIDA).Valor

            If auxFraccion_ IsNot Nothing Then

                itemAux_.tarifffraction = LimpiarTexto(auxFraccion_.clave_, ".")

                Dim auxfracciondescripcion_ = ExtraerClavesyDescripcionesStrings(item_.Campo(CamposFacturaComercial.CA_FRACCION_ARANCELARIA_PARTIDA).ValorPresentacion, "-")

                If auxfracciondescripcion_ IsNot Nothing Then

                    itemAux_.tarifffractiondescription = auxfracciondescripcion_.description_

                End If

            End If

            itemAux_.tariffquantity = item_.Campo(CamposFacturaComercial.CA_CANTIDAD_TARIFA_PARTIDA).Valor

            If auxUMT_ IsNot Nothing Then

                itemAux_.tariffunitdescription = auxUMT_.description_

                itemAux_.tariffunitkey = auxUMT_.clave_

            End If

            If auxNico_ IsNot Nothing Then

                Dim auxNicoDescripcion_ = ExtraerClavesyDescripcionesStrings(item_.Campo(CamposFacturaComercial.CA_FRACCION_NICO_PARTIDA).ValorPresentacion, "-") ''DESCRPIPCION EN VALOR PRESENTACION

                If auxNicoDescripcion_ IsNot Nothing Then

                    itemAux_.nicodescription = auxNicoDescripcion_.description_

                End If

                itemAux_.nico = auxNico_.clave_

            End If

            itemAux_.valorAgregado = item_.Campo(CamposFacturaComercial.CP_VALOR_AGREGADO_ITEM).Valor

            itemAux_.monedaValorAgregado = item_.Campo(CamposFacturaComercial.CP_MONEDA_VALOR_AGREGADO_ITEM).ValorPresentacion

            itemAux_.lot = item_.Campo(CamposFacturaComercial.CA_LOTE_PARTIDA).Valor

            itemAux_.serial = item_.Campo(CamposFacturaComercial.CA_NUMERO_SERIE_PARTIDA).Valor

            itemAux_.brand = item_.Campo(CamposFacturaComercial.CA_MARCA_PARTIDA).Valor

            itemAux_.model = item_.Campo(CamposFacturaComercial.CA_MODELO_PARTIDA).Valor

            itemAux_.submodel = item_.Campo(CamposFacturaComercial.CA_SUBMODELO_PARTIDA).Valor

            itemAux_.mileage = item_.Campo(CamposFacturaComercial.CA_KILOMETRAJE_PARTIDA).Valor

            itemAux_.status = seccionItemsFactura_.Nodos(0).Nodos(i_).estado

            itemAux_.archived = seccionItemsFactura_.Nodos(0).Nodos(i_).archivado

            secuencia_ += 1

            i_ += 1

            auxFactura_.items.Add(itemAux_)

        Next

        Return auxFactura_

    End Function

    Private Function SeccionIncrementables(ByRef auxFactura_ As Object,
                                           ByRef constructorFact_ As ConstructorFacturaComercial, ByRef esPedimento_ As Boolean) As Object

        If TipoOperacion = IControladorFacturaComercial.TipoOperaciones.Importacion Then

            auxFactura_.additions = New List(Of IIncrementable)

            Dim seccionIncrementablesFactura_ = constructorFact_.EstructuraDocumento.Parts.Item("Cuerpo")(1)

            Dim incrementableFletes_ As New Incrementable

            With incrementableFletes_

                .name = "Fletes"

                .amount = seccionIncrementablesFactura_.Campo(CamposFacturaComercial.CA_FLETES).Valor

                .currencyid = ObjectId.Parse(seccionIncrementablesFactura_.Campo(CamposFacturaComercial.CA_MONEDA_FLETES).Valor)

                If seccionIncrementablesFactura_.Campo(CamposFacturaComercial.CA_MONEDA_FLETES).Valor IsNot Nothing Then

                    If esPedimento_ Then

                        Dim cveMonedaApendice_fletes As String = ObtenerMonedaPorObjectid(ObjectId.Parse(seccionIncrementablesFactura_.Campo(CamposFacturaComercial.CA_MONEDA_FLETES).Valor)) _
             .aliasmoneda.FirstOrDefault(Function(a) a.Clave = "cvemonedaA05")?.Valor

                        .currencykey = cveMonedaApendice_fletes

                    Else

                        .currencykey = seccionIncrementablesFactura_.Campo(CamposFacturaComercial.CA_MONEDA_FLETES).ValorPresentacion

                    End If

                End If

            End With

            auxFactura_.additions.Add(incrementableFletes_)

            Dim incrementableSeguros_ As New Incrementable

            With incrementableSeguros_

                .name = "Seguros"

                .amount = seccionIncrementablesFactura_.Campo(CamposFacturaComercial.CA_SEGURO).Valor

                .currencyid = ObjectId.Parse(seccionIncrementablesFactura_.Campo(CamposFacturaComercial.CA_MONEDA_SEGUROS).Valor)

                If seccionIncrementablesFactura_.Campo(CamposFacturaComercial.CA_MONEDA_SEGUROS).Valor IsNot Nothing Then

                    If esPedimento_ Then

                        Dim cveMonedaApendice_seguros As String = ObtenerMonedaPorObjectid(ObjectId.Parse(seccionIncrementablesFactura_.Campo(CamposFacturaComercial.CA_MONEDA_SEGUROS).Valor)) _
                  .aliasmoneda.FirstOrDefault(Function(a) a.Clave = "cvemonedaA05")?.Valor

                        .currencykey = cveMonedaApendice_seguros

                    Else

                        .currencykey = seccionIncrementablesFactura_.Campo(CamposFacturaComercial.CA_MONEDA_SEGUROS).ValorPresentacion

                    End If

                End If

            End With

            auxFactura_.additions.Add(incrementableSeguros_)

            Dim incrementableEmbalajes_ As New Incrementable

            With incrementableEmbalajes_

                .name = "Embalajes"

                .amount = seccionIncrementablesFactura_.Campo(CamposFacturaComercial.CA_EMBALAJES).Valor

                .currencyid = ObjectId.Parse(seccionIncrementablesFactura_.Campo(CamposFacturaComercial.CA_MONEDA_EMBALAJES).Valor)

                If seccionIncrementablesFactura_.Campo(CamposFacturaComercial.CA_MONEDA_EMBALAJES).Valor IsNot Nothing Then

                    If esPedimento_ Then

                        Dim cveMonedaApendice_embalajes As String = ObtenerMonedaPorObjectid(ObjectId.Parse(seccionIncrementablesFactura_.Campo(CamposFacturaComercial.CA_MONEDA_EMBALAJES).Valor)) _
                   .aliasmoneda.FirstOrDefault(Function(a) a.Clave = "cvemonedaA05")?.Valor

                        .currencykey = cveMonedaApendice_embalajes

                    Else

                        .currencykey = seccionIncrementablesFactura_.Campo(CamposFacturaComercial.CA_MONEDA_EMBALAJES).ValorPresentacion

                    End If

                End If

            End With

            auxFactura_.additions.Add(incrementableEmbalajes_)

            Dim otrosincrementables_ As New Incrementable

            With otrosincrementables_

                .name = "Otros incrementables"

                .amount = seccionIncrementablesFactura_.Campo(CamposFacturaComercial.CA_OTROS_INCREMENTABLES).Valor

                .currencyid = ObjectId.Parse(seccionIncrementablesFactura_.Campo(CamposFacturaComercial.CA_MONEDA_OTROS_INCREMENTABLES).Valor)

                If seccionIncrementablesFactura_.Campo(CamposFacturaComercial.CA_MONEDA_OTROS_INCREMENTABLES).Valor IsNot Nothing Then

                    If esPedimento_ Then

                        Dim cveMonedaApendice_otrosincrementables As String = ObtenerMonedaPorObjectid(ObjectId.Parse(seccionIncrementablesFactura_.Campo(CamposFacturaComercial.CA_MONEDA_OTROS_INCREMENTABLES).Valor)) _
                       .aliasmoneda.FirstOrDefault(Function(a) a.Clave = "cvemonedaA05")?.Valor

                        .currencykey = cveMonedaApendice_otrosincrementables

                    Else

                        .currencykey = seccionIncrementablesFactura_.Campo(CamposFacturaComercial.CA_MONEDA_OTROS_INCREMENTABLES).ValorPresentacion

                    End If

                End If

            End With

            auxFactura_.additions.Add(otrosincrementables_)

        End If

        Return auxFactura_

    End Function

    Private Function SeccionCliente(ByRef auxFactura_ As Object,
                                    ByRef constructorFact_ As ConstructorFacturaComercial, ByRef idCliente_ As ObjectId) As Object

        With constructorFact_

            Dim seccionEncabezado_ = .EstructuraDocumento.Parts.Item("Encabezado")(0)

            Dim auxcliente_ = ExtraerClavesyDescripcionesStrings(.Campo(CamposClientes.CA_RAZON_SOCIAL).ValorPresentacion, "|")

            auxFactura_.customernamekey = auxcliente_.description_

            auxFactura_.customername = auxcliente_.clave_

            auxFactura_.customerid = idCliente_

            auxFactura_.customer = New Customer _
                                    With {
                                                .id = idCliente_,
                                                .address = seccionEncabezado_.Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor,
                                                .city = seccionEncabezado_.Campo(CamposDomicilio.CA_CIUDAD).Valor,
                                                .country = seccionEncabezado_.Campo(CamposDomicilio.CA_PAIS).Valor,
                                                .externalnumber = seccionEncabezado_.Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor,
                                                .internalnumber = seccionEncabezado_.Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor,
                                                .locality = seccionEncabezado_.Campo(CamposDomicilio.CA_LOCALIDAD).Valor,
                                                .state = seccionEncabezado_.Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor,
                                                .street = seccionEncabezado_.Campo(CamposDomicilio.CA_CALLE).Valor,
                                                .name = auxcliente_.clave_,
                                                .rfc = seccionEncabezado_.Campo(CamposClientes.CA_RFC_CLIENTE).Valor,
                                                .zipcode = seccionEncabezado_.Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor,
                                                .taxid = seccionEncabezado_.Campo(CamposClientes.CA_TAX_ID).Valor,
                                                .curp = seccionEncabezado_.Campo(CamposClientes.CA_CURP_CLIENTE).Valor
                                        }

        End With

        Return auxFactura_

    End Function

    Private Function SeccionProveedor(ByRef auxFactura_ As Object,
                                      ByRef constructorFact_ As ConstructorFacturaComercial) As Object

        With constructorFact_

            Dim seccionProveedor_ = .EstructuraDocumento.Parts.Item("Encabezado")(1)

            Dim idproveedorAux_ = Nothing
            If seccionProveedor_.Campo(CamposProveedorOperativo.CP_ID_PROVEEDOR) IsNot Nothing Then

                idproveedorAux_ = ObtenerObjectId(seccionProveedor_.Campo(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR))

                auxFactura_.supplierid = idproveedorAux_

            End If

            auxFactura_.suppliernamekey = seccionProveedor_.Campo(CamposProveedorOperativo.CP_CVE_PROVEEDOR).Valor

            auxFactura_.invoicecountrykey = seccionProveedor_.Campo(CamposDomicilio.CA_CVE_PAIS).Valor

            auxFactura_.invoicecountryid = seccionProveedor_.Campo(CamposDomicilio.CA_PAIS).Valor

            auxFactura_.invoicecountry = seccionProveedor_.Campo(CamposDomicilio.CA_PAIS).ValorPresentacion

            auxFactura_.idsuppliertaxid = idproveedorAux_

            Dim razonsocialproveedoraux_ = ExtraerClavesyDescripcionesStrings(seccionProveedor_.Campo(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR).ValorPresentacion, "|")

            auxFactura_.suppliername = razonsocialproveedoraux_.clave_

            Dim auxvinculacion_ = ExtraerClavesyDescripcionesStrings(seccionProveedor_.Campo(CamposFacturaComercial.CA_CVE_VINCULACION).ValorPresentacion, "-")

            Dim auxmetodovaloracion_ = ExtraerClavesyDescripcionesStrings(seccionProveedor_.Campo(CamposFacturaComercial.CP_CVE_METODO_VALORACION).ValorPresentacion, "-")

            auxFactura_.supplier = New Supplier _
                                With {
                                        .id = idproveedorAux_,
                                        .address = seccionProveedor_.Campo(CamposProveedorOperativo.CA_DOMICILIO_FISCAL).Valor,
                                        .city = seccionProveedor_.Campo(CamposDomicilio.CA_CIUDAD).Valor,
                                        .country = seccionProveedor_.Campo(CamposDomicilio.CA_PAIS).Valor,
                                        .externalnumber = seccionProveedor_.Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor,
                                        .internalnumber = seccionProveedor_.Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor,
                                        .locality = seccionProveedor_.Campo(CamposDomicilio.CA_LOCALIDAD).Valor,
                                        .state = seccionProveedor_.Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor,
                                        .street = seccionProveedor_.Campo(CamposDomicilio.CA_CALLE).Valor,
                                        .name = razonsocialproveedoraux_.clave_,
                                        .taxid = seccionProveedor_.Campo(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR).ValorPresentacion,
                                        .zipcode = seccionProveedor_.Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor,
                                        .appliescertificate = seccionProveedor_.Campo(CamposFacturaComercial.CA_APLICA_CERTIFICADO).Valor,
                                        .linkagekey = auxvinculacion_.clave_,
                                        .linkagedescription = auxvinculacion_.description_,
                                        .curp = seccionProveedor_.Campo(CamposProveedorOperativo.CA_CURP_PROVEEDOR).Valor,
                                        .isconsignee = seccionProveedor_.Campo(CamposProveedorOperativo.CP_DESTINATARIO_PROVEEDOR).Valor,
                                        .valuationmethodkey = auxmetodovaloracion_.clave_,
                                        .valuationmethoddescription = auxmetodovaloracion_.description_,
                                        .certifiername = seccionProveedor_.Campo(CamposFacturaComercial.CP_NOMBRE_CERTIFICADOR).Valor,
                                        .rfc = seccionProveedor_.Campo(CamposProveedorOperativo.CA_RFC_PROVEEDOR).Valor
                                }

        End With

        Return auxFactura_

    End Function

    Private Function SeccionDatosGenerales(ByRef auxFactura_ As Object,
                                    ByRef constructorFact_ As ConstructorFacturaComercial,
                                    ByRef idCommercial_ As ObjectId,
                                    ByRef idCliente_ As ObjectId,
                                    ByRef esPedimento_ As Boolean) As TagWatcher

        With constructorFact_

            auxFactura_._id = idCommercial_

            Dim seccionEncabezado_ = .EstructuraDocumento.Parts.Item("Encabezado")(0)

            Dim seccionProveedor_ = .EstructuraDocumento.Parts.Item("Encabezado")(1)

            With seccionEncabezado_

                auxFactura_.invoicenumber = .Campo(CamposFacturaComercial.CA_NUMERO_FACTURA).Valor

                auxFactura_.invoicedate = .Campo(CamposFacturaComercial.CA_FECHA_FACTURA).ValorPresentacion

                auxFactura_.valuecertificate = .Campo(CamposAcuseValor.CA_NUMERO_ACUSEVALOR).Valor

                auxFactura_.valuecertificatedate = .Campo(CamposAcuseValor.CA_FECHA_ACUSEVALOR).Valor

                Dim idAcuse_ = ObtenerObjectId(.Campo(CamposAcuseValor.CP_ID_ACUSEVALOR))

                If idAcuse_ IsNot Nothing Then

                    auxFactura_.idvaluecertificate = idAcuse_

                Else

                    auxFactura_.idvaluecertificate = Nothing

                End If

                auxFactura_.invoiceseries = .Campo(CamposFacturaComercial.CP_SERIE_FOLIO_FACTURA).Valor

                auxFactura_.totalinvoice = .Campo(CamposFacturaComercial.CP_VALOR_FACTURA).Valor

                If esPedimento_ Then

                    Dim cveMonedaApendice_invoicecurrency_ As String = ObtenerMonedaPorObjectid(ObjectId.Parse(.Campo(CamposFacturaComercial.CA_MONEDA_FACTURACION).Valor)) _
                  .aliasmoneda.FirstOrDefault(Function(a) a.Clave = "cvemonedaA05")?.Valor

                    auxFactura_.invoicecurrency = cveMonedaApendice_invoicecurrency_

                    Dim cveMonedaApendice_merchandisecurrency_ As String = ObtenerMonedaPorObjectid(ObjectId.Parse(.Campo(CamposFacturaComercial.CP_MONEDA_VALOR_MERCANCIA).Valor)) _
                        .aliasmoneda.FirstOrDefault(Function(a) a.Clave = "cvemonedaA05")?.Valor

                    auxFactura_.merchandisecurrency = cveMonedaApendice_merchandisecurrency_

                Else

                    auxFactura_.invoicecurrency = .Campo(CamposFacturaComercial.CA_MONEDA_FACTURACION).ValorPresentacion

                    auxFactura_.merchandisecurrency = .Campo(CamposFacturaComercial.CP_MONEDA_VALOR_MERCANCIA).ValorPresentacion

                End If

                auxFactura_.hasalienation = .Campo(CamposFacturaComercial.CP_APLICA_ENAJENACION).Valor

                auxFactura_.hassubdivision = .Campo(CamposFacturaComercial.CA_APLICA_SUBDIVISION).Valor

                auxFactura_.hasadditions = .Campo(CamposFacturaComercial.CP_APLICA_INCREMENTABLES).Valor

                auxFactura_.operationtype = .Campo(CamposFacturaComercial.CP_TIPO_OPERACION).Valor

                Dim auxdatosincoterm_ = ExtraerClavesyDescripcionesStrings(.Campo(CamposFacturaComercial.CA_CVE_INCOTERM).ValorPresentacion, "-")

                auxFactura_.incotermnumerickey = .Campo(CamposFacturaComercial.CA_CVE_INCOTERM).Valor

                auxFactura_.incotermkey = auxdatosincoterm_.clave_

                auxFactura_.incoterm = auxdatosincoterm_.description_

                auxFactura_.merchandisevalue = .Campo(CamposFacturaComercial.CP_VALOR_MERCANCIA).Valor

                auxFactura_.invoiceweight = .Campo(CamposFacturaComercial.CP_PESO_TOTAL).Valor

                auxFactura_.invoicepackages = .Campo(CamposFacturaComercial.CP_BULTOS).Valor

                auxFactura_.purchaseorderref = .Campo(CamposFacturaComercial.CP_ORDEN_COMPRA).Valor

                auxFactura_.customerreference = .Campo(CamposFacturaComercial.CP_REFERENCIA_CLIENTE).Valor

                auxFactura_.initialdataload = $"{ .Campo(CamposFacturaComercial.CP_TIPO_CARGA_DATOS).Valor} - { .Campo(CamposFacturaComercial.CP_TIPO_CARGA_DATOS).ValorPresentacion}"

                auxFactura_.marcadopedimento = .Campo(CamposFacturaComercial.CP_MARCADO_PEDIMENTO).Valor

                auxFactura_.idpedimentoasociado = ObtenerObjectId(.Campo(CamposFacturaComercial.CP_ID_PEDIMENTO_ASOCIADO))

                auxFactura_.numfactura_subdivision = .Campo(CamposFacturaComercial.CP_NUMERO_FACTURA_SUBDIVISION).Valor

            End With

        End With

        Return auxFactura_

    End Function


    Private Function GenerarFacturaComercial(ByVal constructorFactura_ As ConstructorFacturaComercial,
                                                    ByVal idCommercial_ As ObjectId,
                                                    ByVal idCliente_ As ObjectId, ByVal esPedimento_ As Boolean) As TagWatcher

        Dim auxFactura_ As ICommercialInvoiceCustomsDocument = New CommercialInvoiceCustomsDocument

        auxFactura_ = SeccionDatosGenerales(auxFactura_, constructorFactura_, idCommercial_, idCliente_, esPedimento_)

        auxFactura_ = SeccionCliente(auxFactura_, constructorFactura_, idCliente_)

        auxFactura_ = SeccionProveedor(auxFactura_, constructorFactura_)

        auxFactura_ = SeccionItems(auxFactura_, constructorFactura_, esPedimento_)

        auxFactura_ = SeccionIncrementables(auxFactura_, constructorFactura_, esPedimento_)

        Return auxFactura_

    End Function

    'Private Function MontarDatosEnComercialPedimento(ByVal constructorFactura_ As ConstructorFacturaComercial,
    '                                                ByVal idCommercial_ As ObjectId,
    '                                                ByVal idCliente_ As ObjectId) As ICommercialInvoiceCustomsDocument

    '    Dim auxFactura_ As ICommercialInvoiceCustomsDocument = New CommercialInvoiceCustomsDocument

    '    With constructorFactura_

    '        Dim seccionEncabezado_ = .EstructuraDocumento.Parts.Item("Encabezado")(0)

    '        Dim seccionProveedor_ = .EstructuraDocumento.Parts.Item("Encabezado")(1)

    '        auxFactura_._id = idCommercial_

    '        auxFactura_ = SeccionFactura(auxFactura_, constructorFactura_)

    '        With seccionEncabezado_



    '            ''DATOS CLIENTE


    '            ''DATOS PROVEEDOR

    '        End With


    '        ''DATOS ITEMS FACTURA


    '        ''DATOS INCREMENTABLES


    '    End With

    '    Return auxFactura_

    'End Function

    Private Function ObtenerFacturaComercialesPublicadasyFirmadas(ByVal listaObjectsIdFacturas_ As List(Of ObjectId),
                                                                  Optional ByVal esParaPedimento_ As Boolean = False) As TagWatcher

        With Estado

            Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos(21)

                Try

                    enlaceDatos_.EnvironmentOnline = _environmentOnline

                    Dim collection_ = enlaceDatos_.GetMongoCollection(Of OperacionGenerica)((New ConstructorFacturaComercial).GetType.Name)

                    Dim filtroFacturas_ As FilterDefinition(Of OperacionGenerica) =
                        Builders(Of OperacionGenerica).Filter.In(Of ObjectId)("_id", listaObjectsIdFacturas_)

                    Dim filtroFacturaPublicada_ As FilterDefinition(Of OperacionGenerica) =
                          Builders(Of OperacionGenerica).Filter.Eq(Of Boolean)("Publicado", True)

                    Dim filtroFacturaFirmada_ As FilterDefinition(Of OperacionGenerica) =
                          Builders(Of OperacionGenerica).Filter.Ne(Of String)("FirmaElectronica", "")

                    Dim filtroFacturaActiva_ As FilterDefinition(Of OperacionGenerica) =
                          Builders(Of OperacionGenerica).Filter.Eq(Of Int32)("estado", 1)

                    Dim filtroTipoOperacion_ As FilterDefinition(Of OperacionGenerica) =
                        Builders(Of OperacionGenerica).Filter.Eq(Function(x) DirectCast(x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts("Encabezado")(0).Nodos(0).Nodos(0).Nodos(0), Campo).Valor, TipoOperacion)

                    Dim filtroSinSubdivision_ As FilterDefinition(Of OperacionGenerica) = Nothing

                    If esParaPedimento_ Then

                        filtroSinSubdivision_ =
                            Builders(Of OperacionGenerica).Filter.Eq(Function(x) DirectCast(x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts("Encabezado")(0).Nodos(0).Nodos(42).Nodos(0), Campo).Valor, False)

                    Else

                        filtroSinSubdivision_ =
                            Builders(Of OperacionGenerica).Filter.Eq(Function(x) DirectCast(x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts("Encabezado")(0).Nodos(0).Nodos(48).Nodos(0), Campo).Valor, Nothing)

                    End If

                    Dim filtroCombinado_ As FilterDefinition(Of OperacionGenerica) =
                        Builders(Of OperacionGenerica).Filter.And(
                            filtroFacturas_,
                            filtroFacturaPublicada_,
                            filtroFacturaFirmada_,
                            filtroFacturaActiva_,
                            filtroFacturaActiva_,
                            filtroTipoOperacion_,
                            filtroSinSubdivision_)

                    Dim result_ = collection_.Aggregate().
                    Match(filtroCombinado_).
                    Project(Function(y) New With {
                        Key .id_ = y.Id,
                        Key .objectCliente_ = y.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.ObjectIdPropietario,
                        Key .fuente_ = y.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente}).ToList()

                    If result_.Count > 0 Then

                        If esParaPedimento_ Then

                            _listaFacturasCustom = New List(Of ICommercialInvoiceCustomsDocument)

                            result_.ToList().ForEach(Sub(y)

                                                         _documentoElectronicoFacturacomercial = DirectCast(y.fuente_, ConstructorFacturaComercial)

                                                         _listaFacturasCustom.Add(MontarDatosEnCommercialInvoicePedimento(_documentoElectronicoFacturacomercial, y.id_, y.objectCliente_))

                                                     End Sub)

                            If _listaFacturasCustom.Count > 0 Then

                                .ObjectReturned = _listaFacturasCustom

                                .SetOK()

                            Else

                                .SetOKBut(Me, "No se encontraron las facturas solicitadas")

                            End If

                        Else

                            _listaFacturasGeneric = New List(Of ICommercialInvoiceGeneric)

                            result_.ToList().ForEach(Sub(y)

                                                         _documentoElectronicoFacturacomercial = DirectCast(y.fuente_, ConstructorFacturaComercial)

                                                         _listaFacturasGeneric.Add(MontarDatosEnCommercialInvoice(_documentoElectronicoFacturacomercial, y.id_, y.objectCliente_))

                                                     End Sub)

                            If _listaFacturasGeneric.Count > 0 Then

                                .ObjectReturned = _listaFacturasGeneric

                                .SetOK()

                            Else

                                .SetOKBut(Me, "No se encontraron las facturas solicitadas")

                            End If

                        End If

                    Else

                        .SetOKBut(Me, "No se encontraron las facturas solicitadas")

                    End If

                Catch ex As Exception

                    .SetOKBut(Me, $"Ha ocurrido un Error {ex}")

                End Try

            End Using

        End With

        Return Estado

    End Function

#End Region

#Region "ACCIONES MARCAR FACTURA"

    Private Function MarcaFacturas(idFacturas_ As List(Of ObjectId), marcado_ As Boolean) As TagWatcher

        With Estado

            Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos(21)

                enlaceDatos_.EnvironmentOnline = _environmentOnline

                Dim collection_ = enlaceDatos_.GetMongoCollection(Of OperacionGenerica)((New ConstructorFacturaComercial).GetType.Name)

                Dim rutaActualizacion_ As String =
                "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts.Encabezado.0.Nodos.0.Nodos.45.Nodos.0.Valor"

                Dim filter_ As FilterDefinition(Of OperacionGenerica) = Builders(Of OperacionGenerica).Filter.In(Of ObjectId)("_id", idFacturas_)

                Dim update_ As UpdateDefinition(Of OperacionGenerica) = Builders(Of OperacionGenerica).Update.Set(Of Boolean)(rutaActualizacion_, marcado_)

                Try

                    Dim resultado = collection_.UpdateMany(filter_, update_)

                Catch ex As Exception

                    .SetOKBut(Me, $"Ha ocurrido un Error {ex}")

                End Try

            End Using

        End With

        Return Estado

    End Function

    Private Function BuildResumenList(Of T)(
            lista_ As List(Of T),
            code_ As Response.CodeStatus,
            mensaje_ As String,
            Optional getMensaje_ As Func(Of T, String) = Nothing
        ) As List(Of Dictionary(Of String, Object))

        Dim resultado_ As New List(Of Dictionary(Of String, Object))
        For Each item_ In lista_
            resultado_.Add(New Dictionary(Of String, Object) From {
                    {"Code", code_},
                    {"Lista", item_},
                    {"Mensaje", If(getMensaje_ IsNot Nothing, getMensaje_(item_), mensaje_)}
                })
        Next

        Return resultado_

    End Function

    Private Function BuildFacturaAsociada(facturaAsociada_ As Object) As FacturaAsociada
        Return New FacturaAsociada With {
        ._idFactura = facturaAsociada_.id_,
        ._folioFactura = facturaAsociada_.encabezado_.Campo(CamposFacturaComercial.CA_NUMERO_FACTURA).Valor,
        ._idPedimentoAsociado = facturaAsociada_.encabezado_.Campo(CamposFacturaComercial.CP_ID_PEDIMENTO_ASOCIADO).Valor,
        ._esFacturaAsociada = facturaAsociada_.encabezado_.Campo(CamposFacturaComercial.CP_MARCADO_PEDIMENTO).Valor,
        ._tipoOperacion = facturaAsociada_.encabezado_.Campo(CamposFacturaComercial.CP_TIPO_OPERACION).Valor,
        ._aplicaSubdivision = facturaAsociada_.encabezado_.Campo(CamposFacturaComercial.CA_APLICA_SUBDIVISION).Valor
    }
    End Function

    Private Function ComprobacionFacturasNoAsociadas(listaFacturasLigadas_ As List(Of ObjectId), idPedimento_ As ObjectId) As TagWatcher

        Dim estadolocal_ As New TagWatcher

        With estadolocal_

            Try
                Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos(21)

                    enlaceDatos_.EnvironmentOnline = _environmentOnline

                    Dim collection_ = enlaceDatos_.GetMongoCollection(Of OperacionGenerica)((New ConstructorFacturaComercial).GetType.Name)

                    Dim filtroFacturas_ As FilterDefinition(Of OperacionGenerica) =
                        Builders(Of OperacionGenerica).Filter.In(Of ObjectId)("_id", listaFacturasLigadas_)

                    Dim filtroFacturaPublicada_ As FilterDefinition(Of OperacionGenerica) =
                          Builders(Of OperacionGenerica).Filter.Eq(Of Boolean)("Publicado", True)

                    Dim filtroFacturaFirmada_ As FilterDefinition(Of OperacionGenerica) =
                          Builders(Of OperacionGenerica).Filter.Ne(Of String)("FirmaElectronica", "")

                    Dim filtroTipoOperacion_ As FilterDefinition(Of OperacionGenerica) =
                        Builders(Of OperacionGenerica).Filter.Eq(Function(x) DirectCast(x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts("Encabezado")(0).Nodos(0).Nodos(0).Nodos(0), Campo).Valor, TipoOperacion)

                    Dim filtroCombinado_ As FilterDefinition(Of OperacionGenerica) =
                    Builders(Of OperacionGenerica).Filter.And(
                        filtroFacturas_,
                        filtroFacturaPublicada_,
                        filtroFacturaFirmada_,
                        filtroTipoOperacion_)

                    Dim result_ = collection_.Aggregate().
                    Match(filtroCombinado_).
                    Project(Function(y) New With {
                        Key .id_ = y.Id,
                        Key .encabezado_ = y.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Encabezado")(0)}).ToList()

                    If result_.Count > 0 Then

                        Dim listaFacturasLibres_ As New List(Of FacturaAsociada)

                        Dim listaFacturasAsociadasMismoPedimento_ As New List(Of FacturaAsociada)

                        Dim listaFacturasAsociadasOtroPedimento_ As New List(Of FacturaAsociada)

                        Dim listafalsasAsociaciones_ As New List(Of FacturaAsociada)

                        Dim listaFacturasConSubdivision_ As New List(Of FacturaAsociada)

                        result_.ToList().ForEach(Sub(y)

                                                     If y.encabezado_.Campo(CamposFacturaComercial.CA_APLICA_SUBDIVISION).Valor = False Then

                                                         If y.encabezado_.Campo(CamposFacturaComercial.CP_MARCADO_PEDIMENTO).Valor = True AndAlso
                                                         y.encabezado_.Campo(CamposFacturaComercial.CP_ID_PEDIMENTO_ASOCIADO).Valor <> ObjectId.Empty Then

                                                             If y.encabezado_.Campo(CamposFacturaComercial.CP_ID_PEDIMENTO_ASOCIADO).Valor = idPedimento_ Then
                                                                 listaFacturasAsociadasMismoPedimento_.Add(BuildFacturaAsociada(y))
                                                             Else
                                                                 listaFacturasAsociadasOtroPedimento_.Add(BuildFacturaAsociada(y))
                                                             End If

                                                         ElseIf y.encabezado_.Campo(CamposFacturaComercial.CP_MARCADO_PEDIMENTO).Valor = True AndAlso
                                                         y.encabezado_.Campo(CamposFacturaComercial.CP_ID_PEDIMENTO_ASOCIADO).Valor = ObjectId.Empty Then
                                                             listafalsasAsociaciones_.Add(BuildFacturaAsociada(y))

                                                         Else
                                                             listaFacturasLibres_.Add(BuildFacturaAsociada(y))
                                                         End If

                                                     Else
                                                         listaFacturasConSubdivision_.Add(BuildFacturaAsociada(y))
                                                     End If

                                                 End Sub)


                        Dim resumen_ As New Dictionary(Of String, List(Of Dictionary(Of String, Object)))

                        If listaFacturasLibres_.Count > 0 Then
                            resumen_.Add("ListaFacturasLibres",
                            BuildResumenList(listaFacturasLibres_, Response.CodeStatus.RecursoDisponible,
                            Nothing, Function(item_) $"Factura {item_._folioFactura} disponible para asociar."))
                        End If

                        If listaFacturasAsociadasMismoPedimento_.Count > 0 Then
                            resumen_.Add("ListaFacturasAsociadasMismoPedimento",
                            BuildResumenList(listaFacturasAsociadasMismoPedimento_, Response.CodeStatus.RecursoAsociado,
                            Nothing, Function(item_) $"Factura {item_._folioFactura} asociada al pedimento solicitado."))
                        End If

                        If listaFacturasAsociadasOtroPedimento_.Count > 0 Then
                            resumen_.Add("ListaFacturasAsociadasOtroPedimento",
                            BuildResumenList(listaFacturasAsociadasOtroPedimento_, Response.CodeStatus.RecursoNoDisponible,
                            Nothing, Function(item_) $"Factura {item_._folioFactura} asociada a un pedimento diferente al solicitado"))
                        End If

                        If listafalsasAsociaciones_.Count > 0 Then
                            resumen_.Add("ListaFacturasFalsamenteAsociadas",
                            BuildResumenList(listafalsasAsociaciones_, Response.CodeStatus.ErrorAsociacionRecurso,
                            Nothing, Function(item_) $"Ha ocurrido un error, factura {item_._folioFactura} no asociada al pedimento solicitado."))
                        End If

                        If listaFacturasConSubdivision_.Count > 0 Then
                            resumen_.Add("ListaFacturasConSubdivision",
                            BuildResumenList(listaFacturasConSubdivision_, Response.CodeStatus.SinDefinir,
                             Nothing, Function(item_) $"Factura {item_._folioFactura} tiene subdivision"))
                        End If

                        .ObjectReturned = resumen_
                        .SetOK()

                    Else

                        .ObjectReturned = $"Facturas ingresadas no se encuentran en los registros de de esta oficina y/o tipo de operación {TipoOperacion}."

                        .SetOKBut(Me, $"Facturas ingresadas no se encuentran en los registros de de esta oficina y/o tipo de operación {TipoOperacion}.")

                    End If

                End Using

            Catch ex As Exception

                '
                .ObjectReturned = $"Ha ocurrido un error_: {ex}"

                .SetError(Me, $"Ha ocurrido un error_: {ex}")

            End Try

        End With

        Return estadolocal_

    End Function

    Private Function DesasociaFacturasPedimento(listaObjectidFacturas_ As List(Of ObjectId),
                                                idPedimento_ As ObjectId) As TagWatcher

        With Estado

            Try
                Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos(21)

                    enlaceDatos_.EnvironmentOnline = _environmentOnline

                    Dim collection_ = enlaceDatos_.GetMongoCollection(Of OperacionGenerica)((New ConstructorFacturaComercial).GetType.Name)

                    Dim rutaMarca As String =
                        "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts.Encabezado.0.Nodos.0.Nodos.45.Nodos.0.Valor"

                    Dim rutaIdPedimento As String =
                        "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts.Encabezado.0.Nodos.0.Nodos.46.Nodos.0.Valor"

                    Dim filter_ As FilterDefinition(Of OperacionGenerica) = Builders(Of OperacionGenerica).Filter.In(Of ObjectId)("_id", listaObjectidFacturas_)

                    Dim update_ As UpdateDefinition(Of OperacionGenerica) = Builders(Of OperacionGenerica).Update.
                            Set(Of Boolean)(rutaMarca, False).
                            Set(Of ObjectId)(rutaIdPedimento, Nothing)

                    Dim result_ = collection_.UpdateMany(filter_, update_)

                    If result_.MatchedCount <> 0 AndAlso result_.ModifiedCount <> 0 Then

                        .ObjectReturned = "Facturas desasociadas correctamente del pedimento solicitado"

                        .SetOK()

                    Else

                        .SetOKBut(Me, "Facturas ingresas no asociadas a ningún pedimento. ")

                        '.ObjectReturned = "Facturas ingresas no asociadas a ningún pedimento. "

                    End If

                End Using

            Catch ex As Exception

                .SetError(Me, $"Internal Server Error :  {ex}")

            End Try

        End With

        Return Estado

    End Function

    Private Function AsociarListaFacturasPedimento(listaObjectidFacturas_ As List(Of ObjectId), idPedimento_ As ObjectId) As TagWatcher

        With Estado

            Try

                Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos(21)

                    enlaceDatos_.EnvironmentOnline = _environmentOnline

                    Dim collection_ = enlaceDatos_.GetMongoCollection(Of OperacionGenerica)((New ConstructorFacturaComercial).GetType.Name)

                    Dim rutaMarca As String =
                    "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts.Encabezado.0.Nodos.0.Nodos.45.Nodos.0.Valor"

                    Dim rutaIdPedimento As String =
                    "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts.Encabezado.0.Nodos.0.Nodos.46.Nodos.0.Valor"

                    Dim filter_ As FilterDefinition(Of OperacionGenerica) = Builders(Of OperacionGenerica).Filter.In(Of ObjectId)("_id", listaObjectidFacturas_)

                    Dim update_ As UpdateDefinition(Of OperacionGenerica) = Builders(Of OperacionGenerica).Update.
                            Set(Of Boolean)(rutaMarca, True).
                            Set(Of ObjectId)(rutaIdPedimento, idPedimento_)

                    Dim result_ = collection_.UpdateMany(filter_, update_)

                    If result_.MatchedCount <> 0 AndAlso result_.ModifiedCount <> 0 Then

                        .ObjectReturned = "Facturas asociadas correctamente a pedimento"

                        .SetOK()

                    End If

                End Using

            Catch ex As Exception

                .SetError(Me, $"Internal Server Error :  {ex}")

            End Try

        End With

        Return Estado

    End Function

    Private Function ValidaMarcaFactura(idFacturas_ As List(Of ObjectId), idPedimento_ As ObjectId) As TagWatcher

        ''REVISAR

        Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos(21)

            enlaceDatos_.EnvironmentOnline = _environmentOnline

            Dim collection_ = enlaceDatos_.GetMongoCollection(Of OperacionGenerica)((New ConstructorFacturaComercial).GetType.Name)

            Dim idsBsonArray As New BsonArray()
            For Each id As ObjectId In idFacturas_
                idsBsonArray.Add(id)
            Next

            Dim rutaArrayElemAtBase As String = "$Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts.Encabezado.Nodos.Nodos.Nodos"

            Dim results_ = collection_.Aggregate() _
            .Match(New BsonDocument("_id", New BsonDocument("$In", idsBsonArray))) _
            .AppendStage(Of BsonDocument)(BsonDocument.Parse(
                "{
                    $project: {
                        id: '$_id',
                        marca: {
                            $arrayElemAt: [
                                { $arrayElemAt: [ { $arrayElemAt: [ { $arrayElemAt: [ '" & rutaArrayElemAtBase & "', 0 ] }, 0 ] }, 45 ] }, 0
                            ]
                        },
                        id_pedimento: {
                            $arrayElemAt: [
                                { $arrayElemAt: [ { $arrayElemAt: [ { $arrayElemAt: [ '" & rutaArrayElemAtBase & "', 0 ] }, 0 ] }, 46 ] }, 0
                            ]
                        }
                    }
                }")) _
                .AppendStage(Of ResultadoMarcaje)(BsonDocument.Parse(
                "{
                    $project: {
                        _id: '$id',
                        sePuedeMarcar: {
                            $cond: {
                                if: {
                                    $or: [
                                        { $in: [ { $ifNull: ['$" & "marca.Valor" & "', null] }, [false, null] ] },                            
                                        { $and: [
                                            { $eq: ['$" & "marca.Valor" & "', true] },
                                            { $eq: ['$" & "id_pedimento.Valor" & "', { $oid: '" & idPedimento_.ToString() & "' } ] }
                                ]}
                                    ]
                                },
                                then: true,
                                else: false
                            }
                        }
                    }
                }")
            ).ToList()

            Estado.SetOK()

            Estado.ObjectReturned = results_

        End Using

        Return Estado

    End Function

#End Region

#End Region


#Region "ACCIONES PÚBLICAS"

#Region "ACUSE VALOR"
    Public Function ActualizarDatosAcuseValor(idFactura_ As ObjectId,
                                              ByVal valoresAcuseValor_ As Dictionary(Of [Enum], String)) As TagWatcher _
                                              Implements IControladorFacturaComercial.ActualizarDatosAcuseValor

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If Not idFactura_ = ObjectId.Empty And
                    Not valoresAcuseValor_.Count = 0 Then

                    _numeroAcuseValor = valoresAcuseValor_(CamposAcuseValor.CA_NUMERO_ACUSEVALOR)

                    _fechaAcuseValor = Convert.
                                           ToDateTime(valoresAcuseValor_(CamposAcuseValor.CA_FECHA_ACUSEVALOR)).
                                           Date.
                                           ToString("yyyy-MM-dd")

                    _objectidacusevalor = ObjectId.Parse(valoresAcuseValor_(CamposAcuseValor.CP_ID_ACUSEVALOR))

                    Using iEnlace_ As IEnlaceDatos = New EnlaceDatos(21)

                        iEnlace_.EnvironmentOnline = _environmentOnline

                        Dim operationsDB_ = iEnlace_.
                                                GetMongoCollection(Of OperacionGenerica)((New ConstructorFacturaComercial).GetType.Name)

                        Dim filter_ = Builders(Of OperacionGenerica).Filter.Eq(Function(x) x.Id, idFactura_)

                        Dim setStructureOfSubs_ = Builders(Of OperacionGenerica).Update.
                                    Set(Of String)("Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts.Encabezado.0.Nodos.0.Nodos.3.Nodos.0.Valor", _numeroAcuseValor).
                                    Set(Of Date)("Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts.Encabezado.0.Nodos.0.Nodos.5.Nodos.0.Valor", _fechaAcuseValor).
                                    Set(Of ObjectId)("Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts.Encabezado.0.Nodos.0.Nodos.44.Nodos.0.Valor", _objectidacusevalor)

                        Dim result_ = operationsDB_.UpdateOne(filter_, setStructureOfSubs_)

                        If result_.MatchedCount <> 0 Then

                            .SetOK()

                        ElseIf result_.UpsertedId IsNot Nothing Then

                            .SetOK()

                        Else

                            .SetError(Me, "No se generaron cambios")

                        End If

                    End Using

                Else

                    .SetOKBut(Me, "No se encontraron valores disponibles")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

    Public Function BuscarAcuseValor(idFactura_ As ObjectId) As TagWatcher _
        Implements IControladorFacturaComercial.BuscarAcuseValor

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If Not idFactura_ = ObjectId.Empty Then

                    ObtenerAcuseValor(idFactura_)

                Else

                    .SetOKBut(Me, "ObjectId de factura es requerido")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

#End Region


#Region "LISTA FACTURAS"

    Public Function ListaFacturas(idFactura_ As ObjectId) As TagWatcher _
        Implements IControladorFacturaComercial.ListaFacturas

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If Not idFactura_ = ObjectId.Empty Then

                    _listaObjetos = New List(Of ObjectId) From {idFactura_}

                    _Estado = ObtenerFacturas(_listaObjetos)

                Else

                    .SetError(Me, "Facturas no disponibles")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If


        End With


        Return _Estado

    End Function

    Public Function ListaFacturas(idsFacturas_ As List(Of ObjectId)) As TagWatcher _
        Implements IControladorFacturaComercial.ListaFacturas


        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If idsFacturas_.Count > 0 Then

                    _Estado = ObtenerFacturas(idsFacturas_)

                Else

                    .SetError(Me, "Facturas no disponibles")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If


        End With

        Return _Estado

    End Function

    Public Function ListaFacturas(folio_ As String) As TagWatcher _
        Implements IControladorFacturaComercial.ListaFacturas

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If folio_ IsNot Nothing Then

                    _Estado = ObtenerFacturas(New List(Of String) From {folio_})

                Else

                    .SetError(Me, "Facturas no disponibles")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

    Public Function ListaFacturas(folios_ As List(Of String)) As TagWatcher _
        Implements IControladorFacturaComercial.ListaFacturas

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If folios_.Count <> 0 Then

                    _Estado = ObtenerFacturas(folios_)

                Else

                    .SetError(Me, "Facturas no disponibles")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

#End Region


#Region "CARGA FACTURAS"

    Public Function CargaFacturas(listaFacturas_ As List(Of ConstructorFacturaComercial)) As TagWatcher _
        Implements IControladorFacturaComercial.CargaFacturas

        _Estado = New TagWatcher

        With _Estado

            .SetOKBut(Me, "Función sin implementar")

        End With

        Return _Estado

    End Function

    Public Function FirmaDigital(Of T)(_id As ObjectId) As String _
        Implements IControladorFacturaComercial.FirmaDigital

        Return "Sin firma"

    End Function

    Public Function FacturaDisponible(idFactura_ As ObjectId) As Boolean _
        Implements IControladorFacturaComercial.FacturaDisponible

        Return False

    End Function

    Public Function FacturaDisponible(folioFactura_ As String) As Boolean _
        Implements IControladorFacturaComercial.FacturaDisponible

        Return False

    End Function

    Public Function AsociarDocumentos(operacionGenerica_ As OperacionGenerica,
                                      listadocumentosAsociados_ As List(Of DocumentoAsociado)) As TagWatcher _
        Implements IControladorFacturaComercial.AsociarDocumentos

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If operacionGenerica_ IsNot Nothing AndAlso listadocumentosAsociados_.Count > 0 Then

                    GuardarListaDocumentosAsociados(operacionGenerica_, listadocumentosAsociados_)

                Else

                    .SetOKBut(Me, "Operacion generica de la factura y los documentos asociados son requeridos")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

#End Region


#Region "ACCIONES TOTAL INCREMENTABLES"
    Function TotalIncrementables(fechaMoneda_ As Date) As TagWatcher _
        Implements IControladorFacturaComercial.TotalIncrementables

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If _ModalidadTrabajo <> 1 Then

                    If _FacturasComerciales IsNot Nothing Then

                        If _FacturasComerciales.Count > 0 Then

                            .ObjectReturned = _FacturasComerciales

                            _Estado = ObtenerTotalIncrementables(_Estado, fechaMoneda_)

                        Else

                            .SetError(Me, "No se encontraron facturas")

                        End If

                    Else

                        .SetOKBut(Me, "Facturas no disponibles en batería")

                    End If


                Else

                    .SetOKBut(Me, "Método aplicado solo a modalidad interna")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

    Function TotalIncrementables(ByVal idFactura_ As ObjectId,
                                 ByVal fechaMoneda_ As Date) As TagWatcher _
                                 Implements IControladorFacturaComercial.TotalIncrementables

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If Not idFactura_ = ObjectId.Empty Then

                    _Estado = ListaFacturas(idFactura_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        _Estado = ObtenerTotalIncrementables(_Estado, fechaMoneda_)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "ObjectId de factura sin definir")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

    Function TotalIncrementables(ByVal idsFacturas_ As List(Of ObjectId),
                             ByVal fechaMoneda_ As Date) As TagWatcher _
                             Implements IControladorFacturaComercial.TotalIncrementables

        Estado = Nothing

        Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If idsFacturas_.Count() > 0 Then

                    _Estado = ObtenerFacturas(idsFacturas_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        _Estado = ObtenerTotalIncrementables(_Estado, fechaMoneda_)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Lista de ObjectId sin definir")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

    Function TotalIncrementables(ByVal folioFactura_ As String,
                             ByVal fechaMoneda_ As Date) As TagWatcher _
                             Implements IControladorFacturaComercial.TotalIncrementables

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If folioFactura_ IsNot Nothing Then

                    _Estado = ListaFacturas(folioFactura_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        Return ObtenerTotalIncrementables(_Estado, fechaMoneda_)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Folio de factura no disponible")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

    Function TotalIncrementables(ByVal foliosFacturas_ As List(Of String),
                             ByVal fechaMoneda_ As Date) As TagWatcher _
                             Implements IControladorFacturaComercial.TotalIncrementables

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If foliosFacturas_ IsNot Nothing Then

                    _Estado = ListaFacturas(foliosFacturas_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        Return ObtenerTotalIncrementables(_Estado, fechaMoneda_)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Folio de factura no disponible")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function


#End Region


#Region "ACCIONES LISTA INCREMENTABLES"
    Function ListaIncrementables() As TagWatcher _
        Implements IControladorFacturaComercial.ListaIncrementables

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If _ModalidadTrabajo <> 1 Then

                    If _FacturasComerciales IsNot Nothing Then

                        If _FacturasComerciales.Count > 0 Then

                            .ObjectReturned = _FacturasComerciales

                            Return ObtenerListaIncrementables(_Estado)

                        Else

                            .SetError(Me, "Ha ocurrido un error al obtener la lista de incrementables")

                        End If

                    Else

                        .SetOKBut(Me, "Facturas no disponibles en batería")

                    End If

                Else

                    .SetOKBut(Me, "Método aplicado solo a modalidad interna")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

    Function ListaIncrementables(ByVal idFactura_ As ObjectId) As TagWatcher _
                                 Implements IControladorFacturaComercial.ListaIncrementables

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If Not idFactura_ = ObjectId.Empty Then

                    _Estado = ListaFacturas(idFactura_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        Return ObtenerListaIncrementables(_Estado)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Facturas no disponibles en batería")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If


        End With

        Return _Estado

    End Function

    Function ListaIncrementables(ByVal idsFacturas_ As List(Of ObjectId)) As TagWatcher _
                                Implements IControladorFacturaComercial.ListaIncrementables

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If idsFacturas_.Count() > 0 Then

                    _Estado = ListaFacturas(idsFacturas_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        Return ObtenerListaIncrementables(_Estado)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Facturas no disponibles en batería")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

    Function ListaIncrementables(ByVal folioFactura_ As String) As TagWatcher _
                                 Implements IControladorFacturaComercial.ListaIncrementables

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If folioFactura_ IsNot Nothing Then

                    _Estado = ListaFacturas(folioFactura_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        Return ObtenerListaIncrementables(_Estado)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Facturas no disponibles en batería")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

    Function ListaIncrementables(ByVal foliosFacturas_ As List(Of String)) As TagWatcher _
        Implements IControladorFacturaComercial.ListaIncrementables

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If foliosFacturas_ IsNot Nothing Then

                    _Estado = ListaFacturas(foliosFacturas_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        Return ObtenerListaIncrementables(_Estado)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Facturas no disponibles en batería")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

    Function ListarIncrementables(fechaMoneda_ As Date) As TagWatcher _
        Implements IControladorFacturaComercial.ListarIncrementables

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If _ModalidadTrabajo <> 1 Then

                    If _FacturasComerciales IsNot Nothing Then

                        If _FacturasComerciales.Count > 0 Then

                            .ObjectReturned = _FacturasComerciales

                            Return ObtenerIncrementables(DirectCast(_Estado.ObjectReturned, List(Of ConstructorFacturaComercial)), fechaMoneda_)

                        Else

                            .SetError(Me, "No se encontraron facturas")

                        End If

                    Else

                        .SetOKBut(Me, "Facturas no disponibles en batería")

                    End If


                Else

                    .SetOKBut(Me, "Método aplicado solo a modalidad interna")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If


        End With

        Return _Estado

    End Function

    Function ListarIncrementables(ByVal idFactura_ As ObjectId,
                                 ByVal fechaMoneda_ As Date) As TagWatcher _
                                 Implements IControladorFacturaComercial.ListarIncrementables

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If Not idFactura_ = ObjectId.Empty Then

                    _Estado = ListaFacturas(idFactura_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        Return ObtenerIncrementables(DirectCast(_Estado.ObjectReturned, List(Of ConstructorFacturaComercial)), fechaMoneda_)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "ObjectId de factura sin definir")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado


    End Function

    Function ListarIncrementables(ByVal idsFacturas_ As List(Of ObjectId),
                             ByVal fechaMoneda_ As Date) As TagWatcher _
                             Implements IControladorFacturaComercial.ListarIncrementables

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If idsFacturas_.Count() > 0 Then

                    Dim estadoIncrementables_ As New TagWatcher

                    estadoIncrementables_ = ObtenerFacturas(idsFacturas_)

                    If estadoIncrementables_.Status = TagWatcher.TypeStatus.Ok Then

                        Return ObtenerIncrementables(DirectCast(estadoIncrementables_.ObjectReturned, List(Of ConstructorFacturaComercial)), fechaMoneda_)

                    Else

                        .SetOKBut(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Lista de ObjectId sin definir")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

    Function ListarIncrementables(ByVal folioFactura_ As String,
                             ByVal fechaMoneda_ As Date) As TagWatcher _
                             Implements IControladorFacturaComercial.ListarIncrementables

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If folioFactura_ IsNot Nothing Then

                    Dim estadoFacturas_ As New TagWatcher

                    estadoFacturas_ = ListaFacturas(folioFactura_)

                    If estadoFacturas_.Status = TagWatcher.TypeStatus.Ok Then

                        Return ObtenerIncrementables(DirectCast(estadoFacturas_.ObjectReturned, List(Of ConstructorFacturaComercial)), fechaMoneda_)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Folio de factura no disponible")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

    Function ListarIncrementables(ByVal foliosFacturas_ As List(Of String),
                             ByVal fechaMoneda_ As Date) As TagWatcher _
                             Implements IControladorFacturaComercial.ListarIncrementables

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If foliosFacturas_ IsNot Nothing Then

                    _Estado = ListaFacturas(foliosFacturas_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        Return ObtenerIncrementables(DirectCast(_Estado.ObjectReturned, List(Of ConstructorFacturaComercial)), fechaMoneda_)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Folio de factura no disponible")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

#End Region


#Region "ACCIONES LISTA INCOTERMS"
    Function ListaIncoterms() As TagWatcher _
        Implements IControladorFacturaComercial.ListaIncoterms

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If _ModalidadTrabajo <> 1 Then

                    If _FacturasComerciales IsNot Nothing Then

                        If _FacturasComerciales.Count > 0 Then

                            .ObjectReturned = _FacturasComerciales

                            Return ObtenerListaIconterms(_Estado)

                        Else

                            .SetError(Me, "Ha ocurrido un error al obtener la lista de incoterms")

                        End If

                    Else

                        .SetOKBut(Me, "Facturas no disponibles en batería")

                    End If

                Else

                    .SetOKBut(Me, "Función disponible para modalidad interna")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

    Function ListaIncoterms(ByVal idFactura_ As ObjectId) As TagWatcher _
        Implements IControladorFacturaComercial.ListaIncoterms

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If Not idFactura_ = ObjectId.Empty Then

                    _Estado = ListaFacturas(idFactura_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        _Estado = ObtenerListaIconterms(_Estado)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "ObjectId de factura vacío")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Function ListaIncoterms(ByVal idsFacturas_ As List(Of ObjectId)) As TagWatcher _
        Implements IControladorFacturaComercial.ListaIncoterms

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If idsFacturas_.Count() > 0 Then

                    _Estado = ObtenerFacturas(idsFacturas_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        _Estado = ObtenerListaIconterms(_Estado)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Lista de Objectsids de factura vacío")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Function ListaIncoterms(ByVal folioFactura_ As String) As TagWatcher _
        Implements IControladorFacturaComercial.ListaIncoterms

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If folioFactura_ IsNot Nothing Then

                    _Estado = ListaFacturas(folioFactura_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        _Estado = ObtenerListaIconterms(_Estado)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Folio de factura vacío")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Function ListaIncoterms(ByVal foliosFacturas_ As List(Of String)) As TagWatcher _
        Implements IControladorFacturaComercial.ListaIncoterms

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If foliosFacturas_.Count() > 0 Then

                    _Estado = ListaFacturas(foliosFacturas_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        _Estado = ObtenerListaIconterms(_Estado)

                    Else

                        .SetError(Me, "Lista de facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Folios de facturas vacíos")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

#End Region


#Region "ACCIONES LISTA PARTIDAS"
    Function ListaPartidas() As TagWatcher _
        Implements IControladorFacturaComercial.ListaPartidas

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If _ModalidadTrabajo <> 1 Then

                    If _FacturasComerciales IsNot Nothing Then

                        If _FacturasComerciales.Count > 0 Then

                            .ObjectReturned = _FacturasComerciales

                            _Estado = ObtenerPartidas(_Estado)

                        Else

                            .SetError(Me, "Ha ocurrido un error al obtener la lista de partidas")

                        End If

                    Else

                        .SetOKBut(Me, "Facturas no disponibles en batería")

                    End If

                Else

                    .SetOKBut(Me, "Función disponible para modalidad interna")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Function ListaPartidas(ByVal idFactura_ As ObjectId) As TagWatcher _
        Implements IControladorFacturaComercial.ListaPartidas

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If Not idFactura_ = ObjectId.Empty Then

                    _Estado = ListaFacturas(idFactura_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        _Estado = ObtenerPartidas(_Estado)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "ObjectId de factura vacío")

                End If


            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If


        End With

        Return _Estado



    End Function

    Function ListaPartidas(ByVal idsFacturas_ As List(Of ObjectId)) As TagWatcher _
        Implements IControladorFacturaComercial.ListaPartidas

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If idsFacturas_.Count() > 0 Then

                    _Estado = ListaFacturas(idsFacturas_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        _Estado = ObtenerPartidas(_Estado)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Lista de objectsids de factura vacío")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Function ListaPartidas(ByVal folioFactura_ As String) As TagWatcher _
        Implements IControladorFacturaComercial.ListaPartidas

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If folioFactura_ IsNot Nothing Then

                    _Estado = ListaFacturas(folioFactura_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        _Estado = ObtenerPartidas(_Estado)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Folio de factura vacío")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

    Function ListaPartidas(ByVal foliosFacturas_ As List(Of String)) As TagWatcher _
        Implements IControladorFacturaComercial.ListaPartidas

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If foliosFacturas_.Count() > 0 Then

                    _Estado = ListaFacturas(foliosFacturas_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        _Estado = ObtenerPartidas(_Estado)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Lista de folios de facturas vacíos")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

#End Region


#Region "ACCIONES CAMPOS X FACTURA"
    Function ListaCamposFacturaComercial(ByVal idFactura_ As ObjectId,
                                         seccionesCampos_ As Dictionary(Of [Enum], List(Of [Enum]))) _
                                         As TagWatcher _
                                         Implements IControladorFacturaComercial.ListaCamposFacturaComercial

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If Not idFactura_ = ObjectId.Empty Then

                    _Estado = ObtenerListaValores(New List(Of ObjectId) _
                                                  From {idFactura_},
                                                  seccionesCampos_)

                Else

                    .SetOKBut(Me, "Facturas no disponibles en batería")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Function ListaCamposFacturaComercial(ByVal idsFacturas_ As List(Of ObjectId),
                                         ByVal seccionesCampos_ As Dictionary(Of [Enum], List(Of [Enum]))) _
                                         As TagWatcher _
                                         Implements IControladorFacturaComercial.ListaCamposFacturaComercial



        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If idsFacturas_.Count() > 0 Then

                    _Estado = ObtenerListaValores(idsFacturas_, seccionesCampos_)

                Else

                    .SetOKBut(Me, "Facturas no disponibles en batería")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Public Function ListaCamposFacturaComercial(ByVal folioFactura_ As String,
                                                ByVal seccionesCampos_ As Dictionary(Of [Enum], List(Of [Enum]))) _
                                                As TagWatcher _
                                                Implements IControladorFacturaComercial.ListaCamposFacturaComercial




        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If folioFactura_ IsNot Nothing Then

                    _Estado = ObtenerListaValores(New List(Of String) _
                                                  From {folioFactura_},
                                                  seccionesCampos_)

                Else

                    .SetOKBut(Me, "Facturas no disponibles en batería")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Public Function ListaCamposFacturaComercial(ByVal foliosFacturas_ As List(Of String),
                                                ByVal seccionesCampos_ As Dictionary(Of [Enum], List(Of [Enum]))) _
                                                As TagWatcher _
                                                Implements IControladorFacturaComercial.ListaCamposFacturaComercial





        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If foliosFacturas_.Count() > 0 Then

                    _Estado = ObtenerListaValores(foliosFacturas_, seccionesCampos_)

                Else

                    .SetOKBut(Me, "Facturas no disponibles en batería")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

#End Region


#Region "ACCIONES VALOR DOLARES X FACTURA"
    Public Function ConsultaValorDolaresFactura(fechaMoneda_ As Date) As TagWatcher _
        Implements IControladorFacturaComercial.ConsultaValorDolaresFactura

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If _ModalidadTrabajo <> 0 Then

                    If _FacturasComerciales IsNot Nothing Then

                        If _FacturasComerciales.Count > 0 Then

                            .ObjectReturned = _FacturasComerciales

                            _Estado = ObtenerValorDolaresFactura(_Estado, fechaMoneda_)

                        Else

                            .SetOKBut(Me, "Facturas no disponibles en batería")

                        End If

                    Else

                        .SetOKBut(Me, "Facturas no disponibles en batería")

                    End If

                Else

                    .SetOKBut(Me, "Función aplicada a modalidad interna")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

    Public Function ConsultaValorDolaresFactura(ByVal idFactura_ As ObjectId,
                                                fechaMoneda_ As Date) As TagWatcher _
                                                Implements IControladorFacturaComercial.ConsultaValorDolaresFactura

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If Not idFactura_ = ObjectId.Empty Then

                    _Estado = ListaFacturas(idFactura_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        _Estado = ObtenerValorDolaresFactura(_Estado, fechaMoneda_)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "ObjectId de factura vacío")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Public Function ConsultaValorDolaresFactura(ByVal idsFacturas_ As List(Of ObjectId),
                                                fechaMoneda_ As Date) As TagWatcher _
                                                Implements IControladorFacturaComercial.ConsultaValorDolaresFactura

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If idsFacturas_.Count() > 0 Then

                    _Estado = ListaFacturas(idsFacturas_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        _Estado = ObtenerValorDolaresFactura(_Estado, fechaMoneda_)

                    Else

                        .SetOKBut(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Lista de ObjectsId de factura vacía")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Public Function ConsultaValorDolaresFactura(ByVal folioFactura_ As String,
                                                fechaMoneda_ As Date) As TagWatcher _
                                                Implements IControladorFacturaComercial.ConsultaValorDolaresFactura

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If folioFactura_ IsNot Nothing Then

                    Dim estadoListasFacturas_ As New TagWatcher

                    estadoListasFacturas_ = ListaFacturas(folioFactura_)

                    If estadoListasFacturas_.Status = TagWatcher.TypeStatus.Ok Then

                        Return ObtenerValorDolaresFactura(estadoListasFacturas_, fechaMoneda_)

                    Else

                        .SetOKBut(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Folio de factura vacío")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

    Public Function ConsultaValorDolaresFactura(ByVal foliosFacturas_ As List(Of String),
                                                fechaMoneda_ As Date) As TagWatcher _
                                                Implements IControladorFacturaComercial.ConsultaValorDolaresFactura

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If foliosFacturas_.Count() > 0 Then

                    _Estado = ListaFacturas(foliosFacturas_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        Return ObtenerValorDolaresFactura(_Estado, fechaMoneda_)

                    Else

                        .SetOKBut(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Lista de folios de factura vacía")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If


        End With

        Return _Estado

    End Function

#End Region


#Region "FACTURAS X PROVEEDOR"

    Public Function ListaFacturasProveedor(ByVal idProveedor_ As ObjectId,
                                           ByVal idCliente_ As ObjectId,
                                           Optional ByVal facturapublicada_ As Boolean = True) As TagWatcher _
                                           Implements IControladorFacturaComercial.ListaFacturasProveedor

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If Not idProveedor_ = ObjectId.Empty And Not idCliente_ = ObjectId.Empty Then

                    ''TIENES QUE VER LA FORMA 

                    Return ObtenerFacturasProveedor(idProveedor_, idCliente_, facturapublicada_)

                Else

                    .SetOKBut(Me, "Parámetros no recibidos")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

    Public Function ListaFacturasProveedorParaPedimento(ByVal idProveedor_ As ObjectId,
                                           ByVal idCliente_ As ObjectId) As TagWatcher _
                                           Implements IControladorFacturaComercial.ListaFacturasProveedorParaPedimento

        'System.Diagnostics.Debug.WriteLine($"Haber dame tantita dice:ENTRANDO A LISTAFACTURASPROVEEDORPARAPEDIMENTO")

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If Not idProveedor_ = ObjectId.Empty And Not idCliente_ = ObjectId.Empty Then

                    ''TIENES QUE VER LA FORMA 

                    Return ObtenerFacturasProveedorParaPedimento(idProveedor_, idCliente_)

                Else

                    .SetOKBut(Me, "Parámetros no recibidos")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

#End Region


#Region "CONSULTA PLN FACTURAS"

    Public Function ConsultaPLNFactura(consulta_ As String) As BsonDocument _
    Implements IControladorFacturaComercial.ConsultaPLNFactura
        Throw New NotImplementedException()
    End Function

    Public Function Clone() As Object _
    Implements ICloneable.Clone
        Throw New NotImplementedException()
    End Function

    Private Function GetDebuggerDisplay() As String
        Return ToString()
    End Function

    Public Function ObtenerEstructura(numeroFactura_ As String) As TagWatcher _
       Implements IControladorFacturaComercial.ObtenerEstructura


        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If numeroFactura_ IsNot Nothing Then

                    Return ObtenerEstructuraCommercialInvoice(numeroFactura_)

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

    ''' <summary>
    ''' Obtiene el valor presentación de un incoterm 
    ''' </summary>
    ''' <param name="claveIncoterm_"></param>
    ''' <returns></returns>
    Public Shared Function ObtenerIncoterm(ByVal claveIncoterm_ As String) As TagWatcher

        Dim estado_ As New TagWatcher

        With estado_

            If claveIncoterm_ IsNot Nothing And claveIncoterm_ <> "" Then

                Using iEnlace_ As IEnlaceDatos = New EnlaceDatos(26)

                    Dim operationsDB_ = iEnlace_.GetMongoCollection(Of BsonDocument)("[SynapsisN].[dbo].[Vt022TerminosFacturacionA14]")

                    Dim resultado_ = operationsDB_.Aggregate.
                                                   Match("{'t_Cve_TerminoFacturacion':'" & claveIncoterm_ & "'}").
                                                   Project(BsonDocument.Parse("{" & "t_ValorPresentacion:1}")).ToList
                    If resultado_.Count > 0 Then

                        .ObjectReturned = resultado_(0).Elements(1).Value.ToString

                        .SetOK()

                    Else

                        .SetError()

                    End If

                End Using

            End If

        End With

        Return estado_

    End Function

    ''' <summary>
    ''' Obtiene una lista de metodos de valoracion
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function ObtenerListaMetodoValoracion(Optional ByVal tipoOperacion_ As Integer = 1) As TagWatcher

        Dim estado_ As New TagWatcher

        With estado_

            Try

                Using iEnlace_ As IEnlaceDatos = New EnlaceDatos(26)

                    Dim operationsDB_ = iEnlace_.GetMongoCollection(Of BsonDocument)("[SynapsisN].[dbo].[Vt022MetodosValoracionA11]")

                    Dim filter_ As FilterDefinition(Of BsonDocument)

                    If tipoOperacion_ = 1 Then
                        ' Caso: Diferente de 0
                        filter_ = Builders(Of BsonDocument).Filter.Ne(Of Integer)("i_ClaveMetodoValoracion", 0)
                    Else
                        ' Caso: Traer todos (Filtro vacío) o define tu otra condición aquí
                        filter_ = Builders(Of BsonDocument).Filter.Empty
                    End If

                    Dim resultado_ = operationsDB_.Aggregate.
                                                   Match(filter_).
                                                   Project(BsonDocument.Parse("{" & "_id:0, i_ClaveMetodoValoracion:1, t_ClaveDescripcion:1}")).ToList
                    If resultado_.Count > 0 Then


                        .ObjectReturned = resultado_


                        .SetOK()

                    Else

                        .SetError()

                    End If

                End Using

            Catch ex As Exception

            End Try


        End With

        Return estado_

    End Function

    Public Function ConsultarExistenciaFacturaComercial(numeroFactura_ As String,
                                                        idProveedor_ As ObjectId,
                                                        fechaFactura_ As String) As TagWatcher _
                                                        Implements IControladorFacturaComercial.ConsultarExistenciaFacturaComercial

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If numeroFactura_ IsNot Nothing And
                    Not idProveedor_ = ObjectId.Empty And
                    fechaFactura_ IsNot Nothing Then

                    If numeroFactura_ <> "" And fechaFactura_ <> "" Then

                        Return ConsultarExistenciaFactura_(numeroFactura_, idProveedor_, fechaFactura_)

                    Else

                        .SetOKBut(Me, "Todos los elementos son requeridos")

                    End If

                Else

                    .SetOKBut(Me, "Sin elementos suficientes para realizar la búsqueda")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

    Public Function GenerarSubdivisionFacturaComercial(constructorFacturaComercial_ As ConstructorFacturaComercial,
                                                       ByVal objectidPreasignacionFacturaOriginal_ As ObjectId) As TagWatcher _
                                                       Implements IControladorFacturaComercial.GenerarSubdivisionFacturaComercial

        Throw New NotImplementedException()

    End Function



#End Region

#Region "ACCIONES MARCAR FACTURA"

    Public Function MarcarFacturas(idFacturas_ As List(Of ObjectId), marcado_ As Boolean) As TagWatcher

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If idFacturas_ IsNot Nothing Then

                    MarcaFacturas(idFacturas_, marcado_)

                Else

                    .SetOKBut(Me, "Parámetros no recibidos")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

    Public Function AsociarFacturasPedimento(listaObjectIdFacturas_ As List(Of ObjectId), idPedimento As ObjectId) As TagWatcher _
                    Implements IControladorFacturaComercial.AsociarFacturasPedimento

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If listaObjectIdFacturas_ IsNot Nothing Then

                    If listaObjectIdFacturas_.Count <> 0 Then

                        If Not idPedimento = ObjectId.Empty Then

                            ''comprobamos que La FAcTura este asociada al mismo pedimento O ESTe LIBRe

                            Dim comprobarNoAsociacionFacturas_ As TagWatcher = ComprobacionFacturasNoAsociadas(listaObjectIdFacturas_, idPedimento)

                            If comprobarNoAsociacionFacturas_.Status = TypeStatus.Ok AndAlso comprobarNoAsociacionFacturas_.ObjectReturned IsNot Nothing Then

                                Dim informeResultante_ = comprobarNoAsociacionFacturas_.ObjectReturned
                                Dim hayLibres_ As Boolean = informeResultante_.ContainsKey("ListaFacturasLibres")
                                Dim hayMismoPedimento_ As Boolean = informeResultante_.ContainsKey("ListaFacturasAsociadasMismoPedimento")
                                Dim hayOtroPedimento_ As Boolean = informeResultante_.ContainsKey("ListaFacturasAsociadasOtroPedimento")
                                Dim hayFalsasAsociaciones_ As Boolean = informeResultante_.ContainsKey("ListaFacturasFalsamenteAsociadas")
                                ' Dim haySubdivision_ As Boolean = informeResultante_.ContainsKey("ListaFacturasConSubdivision")
                                'Dim hayIncidencias_ As Boolean = hayOtroPedimento_ OrElse hayFalsasAsociaciones_ OrElse haySubdivision_
                                Dim hayIncidencias_ As Boolean = hayOtroPedimento_ OrElse hayFalsasAsociaciones_

                                ' Hay problemas reales, avisar
                                Dim mensajes_ As New List(Of Response)

                                If hayIncidencias_ Then
                                    For Each info_ In informeResultante_
                                        For Each item_ In info_.Value  ' info_.Value es List(Of Dictionary(Of String, Object))
                                            Dim code_ = CType(item_("Code"), Response.CodeStatus)
                                            Dim mensaje_ = item_("Mensaje").ToString()
                                            Dim lista_ = CType(item_("Lista"), FacturaAsociada)

                                            Select Case code_
                                                Case Response.CodeStatus.SinDefinir
                                                        ' mensajes_.Add(RecursoResponse.SinDefinir())
                                                Case Response.CodeStatus.RecursoNoDisponible
                                                    mensajes_.Add(RecursoResponse.NoDisponible(mensaje_, lista_))
                                                Case Response.CodeStatus.ErrorAsociacionRecurso
                                                    ' mensajes_.Add(RecursoResponse.ErrorAsociacion(mensaje_, lista_))
                                                Case Response.CodeStatus.RecursoDisponible
                                                    ' mensajes_.Add(RecursoResponse.Ok(mensaje_, lista_))
                                                Case Response.CodeStatus.RecursoAsociado
                                                    ' mensajes_.Add(RecursoResponse.Ok(mensaje_, lista_))
                                            End Select
                                        Next
                                    Next

                                    .SetOKInfo(Me, "Ver detalles de respuesta")

                                    .ObjectReturned = mensajes_

                                ElseIf hayLibres_ Then
                                    ' Hay facturas libres (con o sin ya asociadas al mismo pedimento), ejecutar directo
                                    Return AsociarListaFacturasPedimento(listaObjectIdFacturas_, idPedimento)

                                ElseIf hayMismoPedimento_ Then
                                    ' Todas ya asociadas al mismo pedimento, no hay nada que hacer
                                    .SetOK()

                                End If



                            Else
                                .ObjectReturned = comprobarNoAsociacionFacturas_.ObjectReturned
                                .SetOKBut(Me, comprobarNoAsociacionFacturas_.ObjectReturned)

                            End If

                        Else
                            .ObjectReturned = "Objectid de pedimento no válido "
                            .SetOKBut(Me, "Objectid de pedimento no válido ")

                        End If

                    Else
                        .ObjectReturned = "Es requerida minimo una factura para trabajar "
                        .SetOKBut(Me, "Es requerida minimo una factura para trabajar ")

                    End If

                Else
                    .ObjectReturned = "Lista de facturas no recibidos"
                    .SetOKBut(Me, "Lista de facturas no recibidos")

                End If

            Else
                .ObjectReturned = "Entorno no puede ser 0"
                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Public Function ValidarMarcaFactura(idFacturas_ As List(Of ObjectId), idPedimento As ObjectId) As TagWatcher _
                    Implements IControladorFacturaComercial.ValidarMarcaFactura

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If idFacturas_ IsNot Nothing Then

                    Return ValidaMarcaFactura(idFacturas_, idPedimento)

                Else

                    .SetOKBut(Me, "Parámetros no recibidos")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Public Function ConsultarExistenciaFacturaComercial(numeroFactura_ As String,
                                                        idCliente_ As ObjectId,
                                                        idProveedor_ As ObjectId,
                                                        fechaFactura_ As String) As TagWatcher _
                                                        Implements IControladorFacturaComercial.ConsultarExistenciaFacturaComercial
        With Estado
            If numeroFactura_ IsNot Nothing Or numeroFactura_ <> "" Then
                If Not idCliente_ = ObjectId.Empty Then
                    If Not idProveedor_ = ObjectId.Empty Then
                        If fechaFactura_ IsNot Nothing Or fechaFactura_ <> "" Then

                            Return ConsultarExistenciaFactura_(numeroFactura_, idCliente_, idProveedor_, fechaFactura_)

                        Else
                            .SetOKBut(Me, "Fecha de factura requerido")
                        End If
                    Else
                        .SetOKBut(Me, "ID proveedor requerido")
                    End If
                Else
                    .SetOKBut(Me, "ID cliente requerido")
                End If
            Else
                .SetOKBut(Me, "Número de factura requerido")
            End If
        End With

        Return Estado
    End Function

    Public Function DesasociarFacturasPedimento(listaObjectidFacturas_ As List(Of ObjectId), idPedimento As ObjectId) As TagWatcher _
        Implements IControladorFacturaComercial.DesasociarFacturasPedimento

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If listaObjectidFacturas_ IsNot Nothing Then

                    If listaObjectidFacturas_.Count <> 0 Then

                        If Not idPedimento = ObjectId.Empty Then

                            ''comprobamos que La FAcTura este asociada al mismo pedimento O ESTe LIBRe

                            Dim comprobarNoAsociacionFacturas_ As TagWatcher = ComprobacionFacturasNoAsociadas(listaObjectidFacturas_, idPedimento)

                            If comprobarNoAsociacionFacturas_.Status = TypeStatus.Ok AndAlso comprobarNoAsociacionFacturas_.ObjectReturned IsNot Nothing Then
                                Dim informeResultante_ = comprobarNoAsociacionFacturas_.ObjectReturned
                                Dim hayLibres_ As Boolean = informeResultante_.ContainsKey("ListaFacturasLibres")
                                Dim hayMismoPedimento_ As Boolean = informeResultante_.ContainsKey("ListaFacturasAsociadasMismoPedimento")
                                Dim hayOtroPedimento_ As Boolean = informeResultante_.ContainsKey("ListaFacturasAsociadasOtroPedimento")
                                Dim hayFalsasAsociaciones_ As Boolean = informeResultante_.ContainsKey("ListaFacturasFalsamenteAsociadas")
                                Dim hayIncidencias_ As Boolean = hayOtroPedimento_ OrElse hayFalsasAsociaciones_ OrElse hayLibres_

                                Dim mensajes_ As New List(Of Response)
                                If hayIncidencias_ Then
                                    For Each info_ In informeResultante_
                                        For Each item_ In info_.Value
                                            Dim code_ = CType(item_("Code"), Response.CodeStatus)
                                            Dim mensaje_ = item_("Mensaje").ToString()
                                            Dim lista_ = CType(item_("Lista"), FacturaAsociada)

                                            Select Case code_
                                                Case Response.CodeStatus.SinDefinir
                                                ' mensajes_.Add(RecursoResponse.SinDefinir())
                                                Case Response.CodeStatus.RecursoNoDisponible
                                                    mensajes_.Add(RecursoResponse.NoDisponible(mensaje_, lista_))
                                                Case Response.CodeStatus.ErrorAsociacionRecurso
                                                    mensajes_.Add(RecursoResponse.ErrorAsociacion(mensaje_, lista_))
                                                Case Response.CodeStatus.RecursoDisponible
                                                    mensajes_.Add(RecursoResponse.Ok(mensaje_, lista_))
                                                Case Response.CodeStatus.RecursoAsociado
                                                    mensajes_.Add(RecursoResponse.Ok(mensaje_, lista_))
                                            End Select
                                        Next
                                    Next
                                    .SetOKInfo(Me, "Ver detalles de respuesta")
                                    .ObjectReturned = mensajes_

                                ElseIf hayMismoPedimento_ Then
                                    Return DesasociaFacturasPedimento(listaObjectidFacturas_, idPedimento)
                                End If
                            Else
                                .ObjectReturned = comprobarNoAsociacionFacturas_.ObjectReturned
                                .SetOKBut(Me, comprobarNoAsociacionFacturas_.ObjectReturned)
                            End If

                        Else
                            .ObjectReturned = "Objectid de pedimento no válido "
                            .SetOKBut(Me, "Objectid de pedimento no válido ")

                        End If

                    Else
                        .ObjectReturned = "Lista de objectids de facturas requeridas"
                        .SetOKBut(Me, "Lista de objectids de facturas requeridas")

                    End If

                Else
                    .ObjectReturned = "Parámetros no recibidos"
                    .SetOKBut(Me, "Parámetros no recibidos")

                End If

            Else
                .ObjectReturned = "Entorno no puede ser 0"
                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

#End Region

#Region "CFDI"
    Protected Function ObtenerInfoCliente(ByVal razonSocialCliente_ As String, ByVal rfc_ As String) As TagWatcher

        _controladorCliente = New ControladorClientes

        _estructuraCliente = New EstructuraCliente

        Dim clientesObtenidos_ As TagWatcher = _controladorCliente.Consultar(razonSocialCliente_)

        Dim tagwatcher_ As New TagWatcher

        If clientesObtenidos_.Status = TypeStatus.Ok Then

            Dim listaClientes_ = DirectCast(clientesObtenidos_.ObjectReturned, List(Of OperacionGenerica))

            If listaClientes_.Count > 0 Then

                Dim rfccoincide_ = True

                For Each documento_ In listaClientes_

                    Dim constructorCliente_ = DirectCast(documento_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente, ConstructorCliente)

                    With constructorCliente_

                        If .Seccion(SeccionesClientes.SCS1).Campo(CamposClientes.CA_RFC_CLIENTE).Valor = rfc_ Then

                            With constructorCliente_.Seccion(SeccionesClientes.SCS1)
                                _estructuraCliente.id = documento_.Id
                                _estructuraCliente.name = .Campo(CamposClientes.CA_RAZON_SOCIAL).Valor
                                _estructuraCliente.cve_cliente = constructorCliente_.FolioDocumento
                                _estructuraCliente.rfc = .Campo(CamposClientes.CA_RFC_CLIENTE).Valor
                                _estructuraCliente.taxid = .Campo(CamposClientes.CA_RFC_CLIENTE).Valor
                                _estructuraCliente.id_domicilio = .Campo(CamposDomicilio.CP_ID_DOMICILIO).Valor
                                _estructuraCliente.sec_domicilio = .Campo(CamposDomicilio.CP_SEC_DOMICILIO).Valor
                                _estructuraCliente.street = .Campo(CamposDomicilio.CA_CALLE).Valor
                                _estructuraCliente.externalnumber = .Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor
                                _estructuraCliente.internalnumber = .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor
                                _estructuraCliente.colonia = .Campo(CamposDomicilio.CA_COLONIA).Valor
                                _estructuraCliente.locality = .Campo(CamposDomicilio.CA_LOCALIDAD).Valor
                                _estructuraCliente.city = .Campo(CamposDomicilio.CA_CIUDAD).Valor
                                _estructuraCliente.municipio = .Campo(CamposDomicilio.CA_MUNICIPIO).Valor
                                _estructuraCliente.cveEntidadFederativa = .Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor
                                _estructuraCliente.entidadFederativa = .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor
                                _estructuraCliente.cvePais = .Campo(CamposDomicilio.CA_CVE_PAIS).Valor
                                _estructuraCliente.country = .Campo(CamposDomicilio.CA_PAIS).Valor
                                _estructuraCliente.zipcode = .Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor
                                _estructuraCliente.address = .Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor
                                _estructuraCliente.firmaElectronicaCliente = documento_.FirmaElectronica

                            End With

                        Else

                            rfccoincide_ = False

                        End If

                    End With

                Next

                If rfccoincide_ Then

                    tagwatcher_.SetOK()

                Else

                    tagwatcher_.SetOKBut(Me, "RFC del cliente es diferente al solicitado")

                End If




            Else

                tagwatcher_.SetOKBut(Me, "Cliente no se encuentra en los registros")

            End If


        Else

            tagwatcher_.SetOKBut(Me, "Cliente no se encuentra en los registros")

        End If

        tagwatcher_.ObjectReturned = _estructuraCliente

        Return tagwatcher_

    End Function

    Protected Function GenerarOperacionGenericaFacturaComercial(ByVal estructura_ As Object,
                                                                ByVal user_ As String,
                                                                Optional ByVal esFacturaGenerica_ As Boolean = True) As TagWatcher

        With Estado

            Dim secuencia_ As ISecuencia = GenerarSecuenciaDocumentoElectronico(SecuenciasComercioExterior.FacturasComerciales.ToString)

            Dim objectidZero_ = "000000000000000000000000"

            If esFacturaGenerica_ Then

                estructura_ = DirectCast(estructura_, CommercialInvoiceGeneric)

            Else

                estructura_ = DirectCast(estructura_, CommercialInvoiceAnalysis)

            End If

            Dim razonSocialCliente_ As String = estructura_.customername

            Dim razonSocialProveedor_ As String = estructura_.suppliername

            _documentoElectronico = New ConstructorFacturaComercial

            Dim existeCliente_ = False

            Dim datoscliente_ = ObtenerInfoCliente(estructura_.customername, estructura_.customer.rfc)

            Dim idmoneda_ = Nothing

            Dim cvemoneda_ = Nothing

            Dim objectIdCliente_ = Nothing

            Dim firmaelectronicaCliente_ As String = Nothing

            With _documentoElectronico

                With .Seccion(SeccionesFacturaComercial.SFAC1)
                    Try
                        If TipoOperacion = IControladorFacturaComercial.TipoOperaciones.Exportacion Then

                            .Campo(CamposFacturaComercial.CP_TIPO_OPERACION).Valor = 2
                            .Campo(CamposFacturaComercial.CP_TIPO_OPERACION).ValorPresentacion = "Exportacion"

                            .Campo(CamposFacturaComercial.CP_TIPO_CARGA_DATOS).Valor = 1 '2 es carga manual
                            .Campo(CamposFacturaComercial.CP_TIPO_CARGA_DATOS).ValorPresentacion = "Carga CFDI"

                        Else

                            .Campo(CamposFacturaComercial.CP_TIPO_OPERACION).Valor = 1
                            .Campo(CamposFacturaComercial.CP_TIPO_OPERACION).ValorPresentacion = "Importacion"

                            .Campo(CamposFacturaComercial.CP_TIPO_CARGA_DATOS).Valor = 1 '2 es carga manual
                            .Campo(CamposFacturaComercial.CP_TIPO_CARGA_DATOS).ValorPresentacion = "Carga IA"

                        End If

                        .Campo(CamposFacturaComercial.CA_NUMERO_FACTURA).Valor = estructura_.invoicenumber
                        .Campo(CamposAcuseValor.CA_NUMERO_ACUSEVALOR).Valor = Nothing
                        .Campo(CamposFacturaComercial.CA_FECHA_FACTURA).Valor = Date.Parse(estructura_.invoicedate)
                        .Campo(CamposFacturaComercial.CA_FECHA_FACTURA).ValorPresentacion = estructura_.invoicedate
                        .Campo(CamposAcuseValor.CA_FECHA_ACUSEVALOR).Valor = Nothing
                        .Campo(CamposFacturaComercial.CP_SERIE_FOLIO_FACTURA).Valor = estructura_.invoiceseries
                        .Campo(CamposFacturaComercial.CP_SERIE_FOLIO_FACTURA).ValorPresentacion = estructura_.invoiceseries

                        ''CLIENTE----------------------------------------------------

                        If datoscliente_.Status = TypeStatus.Ok Then

                            Dim cliente_ = DirectCast(datoscliente_.ObjectReturned, EstructuraCliente)

                            _documentoElectronico.NombrePropietario = $"{cliente_.name} | {cliente_.cve_cliente}"
                            _documentoElectronico.IdPropietario = cliente_.cve_cliente
                            _documentoElectronico.ObjectIdPropietario = cliente_.id

                            objectIdCliente_ = cliente_.id
                            firmaelectronicaCliente_ = cliente_.firmaElectronicaCliente

                            .Campo(CamposClientes.CP_OBJECTID_CLIENTE).Valor = cliente_.id
                            .Campo(CamposClientes.CA_RAZON_SOCIAL).Valor = cliente_.id.ToString()
                            .Campo(CamposClientes.CA_RAZON_SOCIAL).ValorPresentacion = $"{cliente_.name} | {cliente_.cve_cliente}"
                            .Campo(CamposClientes.CP_CVE_CLIENTE).Valor = cliente_.cve_cliente
                            .Campo(CamposClientes.CA_TAX_ID).Valor = cliente_.rfc
                            .Campo(CamposClientes.CA_RFC_CLIENTE).Valor = cliente_.rfc
                            .Campo(CamposClientes.CA_CURP_CLIENTE).Valor = cliente_.curp
                            .Campo(CamposDomicilio.CP_ID_DOMICILIO).Valor = cliente_.id_domicilio
                            .Campo(CamposDomicilio.CP_SEC_DOMICILIO).Valor = cliente_.sec_domicilio
                            .Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor = cliente_.address
                            .Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).ValorPresentacion = cliente_.address
                            .Campo(CamposDomicilio.CA_CALLE).Valor = cliente_.street
                            .Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor = cliente_.externalnumber
                            .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor = cliente_.internalnumber
                            .Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor = $"{cliente_.externalnumber} {cliente_.internalnumber}"
                            .Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor = cliente_.zipcode
                            .Campo(CamposDomicilio.CA_LOCALIDAD).Valor = cliente_.locality
                            .Campo(CamposDomicilio.CA_CIUDAD).Valor = cliente_.city
                            .Campo(CamposDomicilio.CA_MUNICIPIO).Valor = cliente_.municipio
                            .Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor = cliente_.cveEntidadFederativa
                            .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor = cliente_.entidadFederativa
                            .Campo(CamposDomicilio.CA_CVE_PAIS).Valor = cliente_.cvePais
                            .Campo(CamposDomicilio.CA_PAIS).Valor = cliente_.country

                            existeCliente_ = True

                        Else

                            _documentoElectronico.NombrePropietario = estructura_.customername
                            _documentoElectronico.IdPropietario = Nothing
                            _documentoElectronico.ObjectIdPropietario = Nothing

                            .Campo(CamposClientes.CP_OBJECTID_CLIENTE).Valor = Nothing
                            .Campo(CamposClientes.CA_RAZON_SOCIAL).Valor = Nothing
                            .Campo(CamposClientes.CA_RAZON_SOCIAL).ValorPresentacion = estructura_.customername
                            .Campo(CamposClientes.CP_CVE_CLIENTE).Valor = Nothing
                            .Campo(CamposClientes.CA_TAX_ID).Valor = Nothing
                            .Campo(CamposClientes.CA_RFC_CLIENTE).Valor = estructura_.customer.rfc
                            .Campo(CamposClientes.CA_CURP_CLIENTE).Valor = Nothing
                            .Campo(CamposDomicilio.CP_ID_DOMICILIO).Valor = Nothing
                            .Campo(CamposDomicilio.CP_SEC_DOMICILIO).Valor = Nothing
                            .Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor = estructura_.customer.address
                            .Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).ValorPresentacion = estructura_.customer.address
                            .Campo(CamposDomicilio.CA_CALLE).Valor = estructura_.customer.street
                            .Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor = estructura_.customer.externalnumber
                            .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor = estructura_.customer.internalnumber
                            .Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor = $"{estructura_.customer.externalnumber} {estructura_.customer.internalnumber}"
                            .Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor = estructura_.customer.zipcode
                            .Campo(CamposDomicilio.CA_LOCALIDAD).Valor = estructura_.customer.locality
                            .Campo(CamposDomicilio.CA_CIUDAD).Valor = estructura_.customer.city
                            .Campo(CamposDomicilio.CA_MUNICIPIO).Valor = estructura_.customer.locality
                            .Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor = estructura_.customer.state
                            .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor = estructura_.customer.state
                            .Campo(CamposDomicilio.CA_CVE_PAIS).Valor = estructura_.customer.country
                            .Campo(CamposDomicilio.CA_PAIS).Valor = estructura_.customer.country

                        End If

                        ''INCOTERM-----------------------------------------------------------------------------

                        If estructura_.additionaldetails IsNot Nothing Then

                            Dim incoterm_ = ObtenerIncoterm(estructura_.additionaldetails.incoterm)

                            If incoterm_.Status = TypeStatus.Ok Then

                                .Campo(CamposFacturaComercial.CA_CVE_INCOTERM).Valor = estructura_.additionaldetails.incoterm
                                .Campo(CamposFacturaComercial.CA_CVE_INCOTERM).ValorPresentacion = incoterm_.ObjectReturned

                            Else

                                .Campo(CamposFacturaComercial.CA_CVE_INCOTERM).Valor = estructura_.additionaldetails.incoterm
                                .Campo(CamposFacturaComercial.CA_CVE_INCOTERM).ValorPresentacion = estructura_.additionaldetails.incoterm

                            End If

                        Else

                            .Campo(CamposFacturaComercial.CA_CVE_INCOTERM).Valor = Nothing
                            .Campo(CamposFacturaComercial.CA_CVE_INCOTERM).ValorPresentacion = Nothing

                        End If

                        .Campo(CamposFacturaComercial.CP_VALOR_FACTURA).Valor = estructura_.totalinvoice
                        .Campo(CamposFacturaComercial.CP_VALOR_MERCANCIA).Valor = estructura_.totalinvoice

                        ''MONEDAS-----------------------------------------------------------------------

                        If estructura_.invoicecurrency IsNot Nothing Then

                            _controladorMonedas = New ControladorMonedas()

                            Dim listamonedas_ = _controladorMonedas.BuscarMonedas(New List(Of String) From {estructura_.invoicecurrency})

                            If listamonedas_ IsNot Nothing AndAlso listamonedas_.Count > 0 Then

                                idmoneda_ = listamonedas_.FirstOrDefault()._id.ToString()

                                cvemoneda_ = listamonedas_.FirstOrDefault().aliasmoneda.First().Valor

                                .Campo(CamposFacturaComercial.CA_MONEDA_FACTURACION).Valor = idmoneda_
                                .Campo(CamposFacturaComercial.CA_MONEDA_FACTURACION).ValorPresentacion = cvemoneda_
                                .Campo(CamposFacturaComercial.CP_MONEDA_VALOR_MERCANCIA).Valor = idmoneda_
                                .Campo(CamposFacturaComercial.CP_MONEDA_VALOR_MERCANCIA).ValorPresentacion = cvemoneda_

                            Else

                                .Campo(CamposFacturaComercial.CA_MONEDA_FACTURACION).Valor = objectidZero_
                                .Campo(CamposFacturaComercial.CA_MONEDA_FACTURACION).ValorPresentacion = estructura_.invoicecurrency
                                .Campo(CamposFacturaComercial.CP_MONEDA_VALOR_MERCANCIA).Valor = objectidZero_
                                .Campo(CamposFacturaComercial.CP_MONEDA_VALOR_MERCANCIA).ValorPresentacion = estructura_.invoicecurrency

                            End If
                        Else

                            .Campo(CamposFacturaComercial.CA_MONEDA_FACTURACION).Valor = objectidZero_
                            .Campo(CamposFacturaComercial.CA_MONEDA_FACTURACION).ValorPresentacion = estructura_.invoicecurrency
                            .Campo(CamposFacturaComercial.CP_MONEDA_VALOR_MERCANCIA).Valor = objectidZero_
                            .Campo(CamposFacturaComercial.CP_MONEDA_VALOR_MERCANCIA).ValorPresentacion = estructura_.invoicecurrency

                        End If

                        If estructura_.additionaldetails IsNot Nothing Then

                            .Campo(CamposFacturaComercial.CP_BULTOS).Valor = estructura_.additionaldetails.packages
                            .Campo(CamposFacturaComercial.CP_PESO_TOTAL).Valor = estructura_.additionaldetails.totalweight
                            .Campo(CamposFacturaComercial.CP_ORDEN_COMPRA).Valor = estructura_.additionaldetails.purchaseorder
                            .Campo(CamposFacturaComercial.CP_REFERENCIA_CLIENTE).Valor = estructura_.additionaldetails.customerreference

                        End If

                        If TipoOperacion = IControladorFacturaComercial.TipoOperaciones.Exportacion Then

                            .Campo(CamposFacturaComercial.CP_APLICA_ENAJENACION).Valor = True

                        Else

                            .Campo(CamposFacturaComercial.CP_APLICA_ENAJENACION).Valor = False

                        End If

                        .Campo(CamposFacturaComercial.CA_APLICA_SUBDIVISION).Valor = False
                        .Campo(CamposFacturaComercial.CP_APLICA_INCREMENTABLES).Valor = False
                        .Campo(CamposAcuseValor.CP_ID_ACUSEVALOR).Valor = Nothing
                        .Campo(CamposFacturaComercial.CP_MARCADO_PEDIMENTO).Valor = False
                        .Campo(CamposFacturaComercial.CP_ID_PEDIMENTO_ASOCIADO).Valor = Nothing
                        .Campo(CamposFacturaComercial.CP_ID_FACTURA_ORIGINAL).Valor = Nothing
                        .Campo(CamposFacturaComercial.CP_NUMERO_FACTURA_SUBDIVISION).Valor = Nothing

                    Catch ex As Exception

                        Estado.SetError(Me, $"Ha ocurrido un error_ {ex}")

                    End Try

                End With

                With .Seccion(SeccionesFacturaComercial.SFAC2)
                    Try

                        .Campo(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR).ValorPresentacion = estructura_.suppliername

                        .Campo(CamposProveedorOperativo.CP_ID_PROVEEDOR).Valor = objectidZero_

                        .Campo(CamposProveedorOperativo.CP_CVE_PROVEEDOR).Valor = Nothing

                        .Campo(CamposDomicilio.CA_PAIS).Valor = estructura_.supplier.country
                        .Campo(CamposDomicilio.CA_PAIS).ValorPresentacion = estructura_.supplier.country
                        .Campo(CamposProveedorOperativo.CA_RFC_PROVEEDOR).Valor = estructura_.supplier.taxid

                        .Campo(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR).Valor = objectidZero_
                        .Campo(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR).ValorPresentacion = estructura_.supplier.taxid

                        .Campo(CamposProveedorOperativo.CP_ID_DOMICILIO_PROVEEDOR).Valor = objectidZero_

                        .Campo(CamposProveedorOperativo.CA_CURP_PROVEEDOR).Valor = Nothing
                        .Campo(CamposProveedorOperativo.CP_SEC_DOMICILIO_PROVEEDOR).Valor = Nothing
                        .Campo(CamposProveedorOperativo.CA_DOMICILIO_FISCAL).ValorPresentacion = estructura_.supplier.address
                        .Campo(CamposProveedorOperativo.CA_DOMICILIO_FISCAL).Valor = estructura_.supplier.address
                        .Campo(CamposDomicilio.CA_CALLE).Valor = estructura_.supplier.street
                        .Campo(CamposDomicilio.CA_CIUDAD).Valor = estructura_.supplier.city
                        .Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor = estructura_.supplier.zipcode
                        .Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor = estructura_.supplier.externalnumber
                        .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor = estructura_.supplier.internalnumber
                        .Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor = $"{estructura_.supplier.externalnumber} {estructura_.supplier.internalnumber}"
                        .Campo(CamposDomicilio.CA_LOCALIDAD).Valor = estructura_.supplier.locality
                        .Campo(CamposDomicilio.CA_CIUDAD).Valor = estructura_.supplier.city
                        .Campo(CamposDomicilio.CA_MUNICIPIO).Valor = Nothing
                        .Campo(CamposDomicilio.CA_ENTIDAD_MUNICIPIO).Valor = Nothing
                        .Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor = Nothing
                        .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor = estructura_.supplier.state
                        .Campo(CamposDomicilio.CA_CVE_PAIS).Valor = estructura_.supplier.country
                        .Campo(CamposDomicilio.CA_PAIS).Valor = estructura_.supplier.country

                        If estructura_.consigneedetails.consigneedetailsname IsNot Nothing Then

                            If estructura_.suppliername = estructura_.consigneedetails.consigneedetailsname Then

                                .Campo(CamposProveedorOperativo.CP_DESTINATARIO_PROVEEDOR).Valor = True

                            Else

                                If estructura_.consigneedetails.consigneedetailsname <> "" Then

                                    .Campo(CamposProveedorOperativo.CP_DESTINATARIO_PROVEEDOR).Valor = False

                                Else

                                    .Campo(CamposProveedorOperativo.CP_DESTINATARIO_PROVEEDOR).Valor = True

                                End If

                            End If

                        Else

                            .Campo(CamposProveedorOperativo.CP_DESTINATARIO_PROVEEDOR).Valor = True

                        End If


                        ''BUSCAR SI HAY VINCULACION CON EL CLIENTE
                        .Campo(CamposFacturaComercial.CA_CVE_VINCULACION).Valor = Nothing

                        ''BUSCAR SI HAY MÉTODO DE VALORACIÓN
                        .Campo(CamposFacturaComercial.CP_CVE_METODO_VALORACION).Valor = Nothing

                        .Campo(CamposFacturaComercial.CA_APLICA_CERTIFICADO).Valor = False
                        .Campo(CamposFacturaComercial.CP_NOMBRE_CERTIFICADOR).Valor = Nothing
                        '.Campo(CamposProveedorOperativo.CP_DESTINATARIO_PROVEEDOR).Valor = False

                    Catch ex As Exception

                        Estado.SetError(Me, $"Ha ocurrido un error_ {ex}")

                    End Try

                End With

                Dim i_ = 0

                If TipoOperacion = IControladorFacturaComercial.TipoOperaciones.Importacion Then

                    For Each item_ In estructura_.items
                        i_ += 1
                        Dim partida_ = .Seccion(SeccionesFacturaComercial.SFAC4).Partida(_documentoElectronico)
                        With partida_
                            .Campo(CamposFacturaComercial.CP_OBJECTID_PRODUCTOS).Valor = Nothing
                            .Campo(CamposFacturaComercial.CP_NUMERO_PARTIDA).Valor = Nothing

                            ''AQUI HAY QUE HACER BUSQUEDA???

                            .Campo(CamposFacturaComercial.CA_NUMERO_PARTE_PARTIDA).Valor = objectidZero_
                            .Campo(CamposFacturaComercial.CA_NUMERO_PARTE_PARTIDA).ValorPresentacion = item_.partnumber
                            .Campo(CamposFacturaComercial.CA_CANTIDAD_COMERCIAL_PARTIDA).Valor = item_.quantity
                            ''AQUI SE GUARDA O NO, HAY ALGO EXTRAÑO
                            .Campo(CamposFacturaComercial.CA_UNIDAD_MEDIDA_COMERCIAL_PARTIDA).Valor = objectidZero_
                            .Campo(CamposFacturaComercial.CA_UNIDAD_MEDIDA_COMERCIAL_PARTIDA).ValorPresentacion = item_.unit

                            .Campo(CamposFacturaComercial.CA_DESCRIPCION_PARTE_PARTIDA).Valor = item_.description
                            .Campo(CamposFacturaComercial.CA_DESCRIPCION_PARTE_PARTIDA_ORIGINAL).Valor = item_.description

                            ''VALORES IMPO
                            .Campo(CamposFacturaComercial.CA_VALOR_FACTURA_PARTIDA).Valor = item_.total
                            .Campo(CamposFacturaComercial.CA_VALOR_DOLARES_PARTIDA).Valor = item_.value
                            .Campo(CamposFacturaComercial.CA_VALOR_UNITARIO_PARTIDA).Valor = item_.unitprice

                            ''VALORES EXPO
                            .Campo(CamposFacturaComercial.CA_VALOR_MERCANCIA_PARTIDA).Valor = item_.total
                            .Campo(CamposFacturaComercial.CA_PRECIO_UNITARIO_PARTIDA).Valor = item_.unitprice
                            '.Campo(CamposFacturaComercial.CP_VALOR_AGREGADO_ITEM).Valor = 0

                            If idmoneda_ IsNot Nothing AndAlso cvemoneda_ IsNot Nothing Then

                                ''VALORES IMPO
                                .Campo(CamposFacturaComercial.CP_MONEDA_FACTURA_PARTIDA).Valor = idmoneda_
                                .Campo(CamposFacturaComercial.CP_MONEDA_FACTURA_PARTIDA).ValorPresentacion = cvemoneda_

                                .Campo(CamposFacturaComercial.CA_MONEDA_VALOR_UNITARIO_PARTIDA).Valor = idmoneda_
                                .Campo(CamposFacturaComercial.CA_MONEDA_VALOR_UNITARIO_PARTIDA).ValorPresentacion = cvemoneda_

                                .Campo(CamposFacturaComercial.CP_MONEDA_VALOR_DOLARES_PARTIDA).Valor = idmoneda_
                                .Campo(CamposFacturaComercial.CP_MONEDA_VALOR_DOLARES_PARTIDA).ValorPresentacion = cvemoneda_

                                ''VALORES EXPO
                                .Campo(CamposFacturaComercial.CA_MONEDA_MERCANCIA_PARTIDA).Valor = idmoneda_
                                .Campo(CamposFacturaComercial.CA_MONEDA_MERCANCIA_PARTIDA).ValorPresentacion = cvemoneda_
                                .Campo(CamposFacturaComercial.CP_MONEDA_PRECIO_UNITARIO).Valor = idmoneda_
                                .Campo(CamposFacturaComercial.CP_MONEDA_PRECIO_UNITARIO).ValorPresentacion = cvemoneda_
                                '.Campo(CamposFacturaComercial.CP_MONEDA_VALOR_AGREGADO_ITEM).Valor = idmoneda_
                                '.Campo(CamposFacturaComercial.CP_MONEDA_VALOR_AGREGADO_ITEM).ValorPresentacion = cvemoneda_
                            Else


                                .Campo(CamposFacturaComercial.CA_MONEDA_VALOR_UNITARIO_PARTIDA).Valor = objectidZero_
                                .Campo(CamposFacturaComercial.CA_MONEDA_VALOR_UNITARIO_PARTIDA).ValorPresentacion = item_.currency
                                .Campo(CamposFacturaComercial.CP_MONEDA_VALOR_DOLARES_PARTIDA).Valor = objectidZero_
                                .Campo(CamposFacturaComercial.CP_MONEDA_VALOR_DOLARES_PARTIDA).ValorPresentacion = item_.currency

                                ''VALORES EXPO
                                .Campo(CamposFacturaComercial.CA_MONEDA_MERCANCIA_PARTIDA).Valor = objectidZero_
                                .Campo(CamposFacturaComercial.CA_MONEDA_MERCANCIA_PARTIDA).ValorPresentacion = item_.currency
                                .Campo(CamposFacturaComercial.CP_MONEDA_PRECIO_UNITARIO).Valor = objectidZero_
                                .Campo(CamposFacturaComercial.CP_MONEDA_PRECIO_UNITARIO).ValorPresentacion = item_.currency
                                '.Campo(CamposFacturaComercial.CP_MONEDA_VALOR_AGREGADO_ITEM).Valor = objectidZero_
                                '.Campo(CamposFacturaComercial.CP_MONEDA_VALOR_AGREGADO_ITEM).ValorPresentacion = item_.currency

                            End If

                            .Campo(CamposFacturaComercial.CA_PESO_NETO_PARTIDA).Valor = item_.netweight
                            .Campo(CamposFacturaComercial.CA_DESCRIPCION_COVE_PARTIDA).Valor = item_.description
                            .Campo(CamposFacturaComercial.CA_DESCRIPCION_PARTE_PARTIDA).Valor = item_.description
                            .Campo(CamposFacturaComercial.CA_DESCRIPCION_PARTE_PARTIDA_ORIGINAL).Valor = item_.description

                            If item_.destinationcountry IsNot Nothing Then

                                Dim paisDestino_ = ControladorPaises.ConsultarListaPaisesPorClaveISO(item_.destinationcountry)

                                If paisDestino_.Status = TypeStatus.Ok Then

                                    .Campo(CamposFacturaComercial.CA_PAIS_DESTINO_PARTIDA).Valor = paisDestino_.ObjectReturned(0)._id.ToString()
                                    .Campo(CamposFacturaComercial.CA_PAIS_DESTINO_PARTIDA).ValorPresentacion = paisDestino_.ObjectReturned(0).paisPresentacion
                                Else
                                    .Campo(CamposFacturaComercial.CA_PAIS_DESTINO_PARTIDA).Valor = objectidZero_
                                    .Campo(CamposFacturaComercial.CA_PAIS_DESTINO_PARTIDA).ValorPresentacion = item_.destinationcountry

                                End If

                            End If

                            If item_.origincountry IsNot Nothing Then

                                Dim paisOrigin_ = ControladorPaises.ConsultarListaPaisesPorClaveISO(item_.origincountry)

                                If paisOrigin_.Status = TypeStatus.Ok Then

                                    .Campo(CamposFacturaComercial.CA_PAIS_ORIGEN_PARTIDA).Valor = paisOrigin_.ObjectReturned(0)._id.ToString()
                                    .Campo(CamposFacturaComercial.CA_PAIS_ORIGEN_PARTIDA).ValorPresentacion = paisOrigin_.ObjectReturned(0).paisPresentacion
                                Else

                                    .Campo(CamposFacturaComercial.CA_PAIS_ORIGEN_PARTIDA).Valor = objectidZero_
                                    .Campo(CamposFacturaComercial.CA_PAIS_ORIGEN_PARTIDA).ValorPresentacion = item_.origincountry

                                End If

                            End If

                            .Campo(CamposFacturaComercial.CA_CVE_METODO_VALORACION_PARTIDA).Valor = Nothing

                            .Campo(CamposFacturaComercial.CP_ORDEN_COMPRA_PARTIDA).ValorPresentacion = item_.purchaseorder

                            .Campo(CamposFacturaComercial.CP_CANTIDAD_FACTURA_PARTIDA).Valor = item_.quantity
                            ''SE ANDA PELEANDO CON EL DE ARRIBA
                            .Campo(CamposFacturaComercial.CP_UNIDAD_MEDIDA_FACTURA_PARTIDA).Valor = objectidZero_
                            .Campo(CamposFacturaComercial.CP_UNIDAD_MEDIDA_FACTURA_PARTIDA).ValorPresentacion = item_.unit
                            .Campo(CamposFacturaComercial.CA_FRACCION_ARANCELARIA_PARTIDA).Valor = Nothing
                            .Campo(CamposFacturaComercial.CA_CANTIDAD_TARIFA_PARTIDA).Valor = objectidZero_
                            .Campo(CamposFacturaComercial.CA_UNIDAD_MEDIDA_TARIFA_PARTIDA).Valor = Nothing
                            .Campo(CamposFacturaComercial.CA_FRACCION_NICO_PARTIDA).Valor = Nothing
                            .Campo(CamposFacturaComercial.CA_LOTE_PARTIDA).Valor = Nothing
                            .Campo(CamposFacturaComercial.CA_NUMERO_SERIE_PARTIDA).Valor = Nothing
                            .Campo(CamposFacturaComercial.CA_MARCA_PARTIDA).Valor = Nothing
                            .Campo(CamposFacturaComercial.CA_MODELO_PARTIDA).Valor = Nothing
                            .Campo(CamposFacturaComercial.CA_SUBMODELO_PARTIDA).Valor = Nothing
                            .Campo(CamposFacturaComercial.CA_KILOMETRAJE_PARTIDA).Valor = Nothing
                            .Campo(CamposProducto.CP_TIMESTAMP).Valor = Nothing

                            .Campo(CamposGlobales.CP_IDENTITY).Valor = i_

                        End With

                    Next

                Else

                    For Each item_ In estructura_.itemscfdi_
                        i_ += 1
                        Dim partida_ = .Seccion(SeccionesFacturaComercial.SFAC4).Partida(_documentoElectronico)
                        With partida_
                            .Campo(CamposFacturaComercial.CP_OBJECTID_PRODUCTOS).Valor = Nothing
                            .Campo(CamposFacturaComercial.CP_NUMERO_PARTIDA).Valor = Nothing

                            ''AQUI HAY QUE HACER BUSQUEDA???

                            .Campo(CamposFacturaComercial.CA_NUMERO_PARTE_PARTIDA).Valor = objectidZero_
                            .Campo(CamposFacturaComercial.CA_NUMERO_PARTE_PARTIDA).ValorPresentacion = item_.partnumber
                            .Campo(CamposFacturaComercial.CA_CANTIDAD_COMERCIAL_PARTIDA).Valor = item_.Cantidad
                            ''AQUI SE GUARDA O NO, HAY ALGO EXTRAÑO
                            .Campo(CamposFacturaComercial.CA_UNIDAD_MEDIDA_COMERCIAL_PARTIDA).Valor = objectidZero_
                            .Campo(CamposFacturaComercial.CA_UNIDAD_MEDIDA_COMERCIAL_PARTIDA).ValorPresentacion = item_.ClaveUnidad

                            .Campo(CamposFacturaComercial.CA_DESCRIPCION_PARTE_PARTIDA).Valor = item_.description
                            .Campo(CamposFacturaComercial.CA_DESCRIPCION_PARTE_PARTIDA_ORIGINAL).Valor = item_.description

                            ''VALORES IMPO
                            .Campo(CamposFacturaComercial.CA_VALOR_FACTURA_PARTIDA).Valor = item_.total
                            .Campo(CamposFacturaComercial.CA_VALOR_DOLARES_PARTIDA).Valor = item_.ValorDolares
                            .Campo(CamposFacturaComercial.CA_VALOR_UNITARIO_PARTIDA).Valor = item_.ValorUnitario

                            ''VALORES EXPO
                            .Campo(CamposFacturaComercial.CA_VALOR_MERCANCIA_PARTIDA).Valor = item_.total
                            .Campo(CamposFacturaComercial.CA_PRECIO_UNITARIO_PARTIDA).Valor = item_.ValorUnitarioAduana
                            .Campo(CamposFacturaComercial.CP_VALOR_AGREGADO_ITEM).Valor = 0

                            If idmoneda_ IsNot Nothing AndAlso cvemoneda_ IsNot Nothing Then

                                ''VALORES IMPO
                                .Campo(CamposFacturaComercial.CP_MONEDA_FACTURA_PARTIDA).Valor = idmoneda_
                                .Campo(CamposFacturaComercial.CP_MONEDA_FACTURA_PARTIDA).ValorPresentacion = cvemoneda_

                                .Campo(CamposFacturaComercial.CA_MONEDA_VALOR_UNITARIO_PARTIDA).Valor = idmoneda_
                                .Campo(CamposFacturaComercial.CA_MONEDA_VALOR_UNITARIO_PARTIDA).ValorPresentacion = cvemoneda_

                                .Campo(CamposFacturaComercial.CP_MONEDA_VALOR_DOLARES_PARTIDA).Valor = idmoneda_
                                .Campo(CamposFacturaComercial.CP_MONEDA_VALOR_DOLARES_PARTIDA).ValorPresentacion = cvemoneda_

                                ''VALORES EXPO
                                .Campo(CamposFacturaComercial.CA_MONEDA_MERCANCIA_PARTIDA).Valor = idmoneda_
                                .Campo(CamposFacturaComercial.CA_MONEDA_MERCANCIA_PARTIDA).ValorPresentacion = cvemoneda_
                                .Campo(CamposFacturaComercial.CP_MONEDA_PRECIO_UNITARIO).Valor = idmoneda_
                                .Campo(CamposFacturaComercial.CP_MONEDA_PRECIO_UNITARIO).ValorPresentacion = cvemoneda_
                                .Campo(CamposFacturaComercial.CP_MONEDA_VALOR_AGREGADO_ITEM).Valor = idmoneda_
                                .Campo(CamposFacturaComercial.CP_MONEDA_VALOR_AGREGADO_ITEM).ValorPresentacion = cvemoneda_
                            Else


                                .Campo(CamposFacturaComercial.CA_MONEDA_VALOR_UNITARIO_PARTIDA).Valor = objectidZero_
                                .Campo(CamposFacturaComercial.CA_MONEDA_VALOR_UNITARIO_PARTIDA).ValorPresentacion = item_.currency
                                .Campo(CamposFacturaComercial.CP_MONEDA_VALOR_DOLARES_PARTIDA).Valor = objectidZero_
                                .Campo(CamposFacturaComercial.CP_MONEDA_VALOR_DOLARES_PARTIDA).ValorPresentacion = item_.currency

                                ''VALORES EXPO
                                .Campo(CamposFacturaComercial.CA_MONEDA_MERCANCIA_PARTIDA).Valor = objectidZero_
                                .Campo(CamposFacturaComercial.CA_MONEDA_MERCANCIA_PARTIDA).ValorPresentacion = item_.currency
                                .Campo(CamposFacturaComercial.CP_MONEDA_PRECIO_UNITARIO).Valor = objectidZero_
                                .Campo(CamposFacturaComercial.CP_MONEDA_PRECIO_UNITARIO).ValorPresentacion = item_.currency
                                .Campo(CamposFacturaComercial.CP_MONEDA_VALOR_AGREGADO_ITEM).Valor = objectidZero_
                                .Campo(CamposFacturaComercial.CP_MONEDA_VALOR_AGREGADO_ITEM).ValorPresentacion = item_.currency

                            End If


                            .Campo(CamposFacturaComercial.CA_PESO_NETO_PARTIDA).Valor = item_.netweight
                            .Campo(CamposFacturaComercial.CA_DESCRIPCION_COVE_PARTIDA).Valor = item_.description
                            .Campo(CamposFacturaComercial.CA_DESCRIPCION_PARTE_PARTIDA).Valor = item_.description
                            .Campo(CamposFacturaComercial.CA_DESCRIPCION_PARTE_PARTIDA_ORIGINAL).Valor = item_.description

                            If item_.destinationcountry IsNot Nothing Then

                                Dim paisDestino_ = ControladorPaises.ConsultarListaPaisesPorClaveISO(item_.destinationcountry)

                                If paisDestino_.Status = TypeStatus.Ok Then

                                    .Campo(CamposFacturaComercial.CA_PAIS_DESTINO_PARTIDA).Valor = paisDestino_.ObjectReturned(0)._id.ToString()
                                    .Campo(CamposFacturaComercial.CA_PAIS_DESTINO_PARTIDA).ValorPresentacion = paisDestino_.ObjectReturned(0).paisPresentacion
                                Else
                                    .Campo(CamposFacturaComercial.CA_PAIS_DESTINO_PARTIDA).Valor = objectidZero_
                                    .Campo(CamposFacturaComercial.CA_PAIS_DESTINO_PARTIDA).ValorPresentacion = item_.destinationcountry

                                End If

                            End If

                            If item_.origincountry IsNot Nothing Then

                                Dim paisOrigin_ = ControladorPaises.ConsultarListaPaisesPorClaveISO(item_.origincountry)

                                If paisOrigin_.Status = TypeStatus.Ok Then

                                    .Campo(CamposFacturaComercial.CA_PAIS_ORIGEN_PARTIDA).Valor = paisOrigin_.ObjectReturned(0)._id.ToString()
                                    .Campo(CamposFacturaComercial.CA_PAIS_ORIGEN_PARTIDA).ValorPresentacion = paisOrigin_.ObjectReturned(0).paisPresentacion
                                Else

                                    .Campo(CamposFacturaComercial.CA_PAIS_ORIGEN_PARTIDA).Valor = objectidZero_
                                    .Campo(CamposFacturaComercial.CA_PAIS_ORIGEN_PARTIDA).ValorPresentacion = item_.origincountry

                                End If

                            End If

                            .Campo(CamposFacturaComercial.CA_CVE_METODO_VALORACION_PARTIDA).Valor = Nothing

                            .Campo(CamposFacturaComercial.CP_ORDEN_COMPRA_PARTIDA).ValorPresentacion = item_.purchaseorder

                            .Campo(CamposFacturaComercial.CP_CANTIDAD_FACTURA_PARTIDA).Valor = item_.quantity
                            ''SE ANDA PELEANDO CON EL DE ARRIBA
                            .Campo(CamposFacturaComercial.CP_UNIDAD_MEDIDA_FACTURA_PARTIDA).Valor = objectidZero_
                            .Campo(CamposFacturaComercial.CP_UNIDAD_MEDIDA_FACTURA_PARTIDA).ValorPresentacion = item_.unit

                            Dim fraccionnico_ = item_.FraccionArancelaria

                            If fraccionnico_ IsNot Nothing Then

                                .Campo(CamposFacturaComercial.CA_FRACCION_ARANCELARIA_PARTIDA).Valor = fraccionnico_.Substring(0, 8)
                                .Campo(CamposFacturaComercial.CA_CANTIDAD_TARIFA_PARTIDA).Valor = objectidZero_
                                .Campo(CamposFacturaComercial.CA_UNIDAD_MEDIDA_TARIFA_PARTIDA).Valor = item_.ClaveUnidad

                                ''AQUII
                                If item_.Nico Is Nothing Then

                                    If fraccionnico_ IsNot Nothing Then

                                        'Dim AQUI = fraccionnico_.Count

                                        If fraccionnico_.Length > 8 Then

                                            .Campo(CamposFacturaComercial.CA_FRACCION_NICO_PARTIDA).Valor = fraccionnico_.Substring(8, 2)
                                        Else

                                            .Campo(CamposFacturaComercial.CA_FRACCION_NICO_PARTIDA).Valor = Nothing

                                        End If

                                    End If

                                Else

                                    .Campo(CamposFacturaComercial.CA_FRACCION_NICO_PARTIDA).Valor = Nothing

                                End If

                            Else

                                .Campo(CamposFacturaComercial.CA_FRACCION_ARANCELARIA_PARTIDA).Valor = Nothing
                                .Campo(CamposFacturaComercial.CA_CANTIDAD_TARIFA_PARTIDA).Valor = objectidZero_
                                .Campo(CamposFacturaComercial.CA_UNIDAD_MEDIDA_TARIFA_PARTIDA).Valor = Nothing

                                If item_.Nico Is Nothing Then

                                    If fraccionnico_ IsNot Nothing Then

                                        If fraccionnico_.Count > 8 Then

                                            .Campo(CamposFacturaComercial.CA_FRACCION_NICO_PARTIDA).Valor = Nothing

                                        Else

                                            .Campo(CamposFacturaComercial.CA_FRACCION_NICO_PARTIDA).Valor = Nothing

                                        End If

                                    End If

                                Else

                                    .Campo(CamposFacturaComercial.CA_FRACCION_NICO_PARTIDA).Valor = Nothing

                                End If

                            End If

                            .Campo(CamposFacturaComercial.CA_LOTE_PARTIDA).Valor = Nothing
                            .Campo(CamposFacturaComercial.CA_NUMERO_SERIE_PARTIDA).Valor = Nothing
                            .Campo(CamposFacturaComercial.CA_MARCA_PARTIDA).Valor = Nothing
                            .Campo(CamposFacturaComercial.CA_MODELO_PARTIDA).Valor = Nothing
                            .Campo(CamposFacturaComercial.CA_SUBMODELO_PARTIDA).Valor = Nothing
                            .Campo(CamposFacturaComercial.CA_KILOMETRAJE_PARTIDA).Valor = Nothing
                            .Campo(CamposProducto.CP_TIMESTAMP).Valor = Nothing

                            .Campo(CamposGlobales.CP_IDENTITY).Valor = i_

                        End With

                    Next

                End If

                If TipoOperacion = IControladorFacturaComercial.TipoOperaciones.Importacion Then

                    With .Seccion(SeccionesFacturaComercial.SFAC5) ''QUIZAS HAYA QUE DETALLAR MEJOR

                        If estructura_.additionaldetails IsNot Nothing Then

                            If estructura_.additionaldetails.incrementalvalues IsNot Nothing Then

                                If estructura_.additionaldetails.incrementalvalues.Count <> 0 Then

                                    For Each item_ In estructura_.additionaldetails.incrementalvalues
                                        .Campo(CamposFacturaComercial.CA_FLETES).Valor = item_.incremental
                                        .Campo(CamposFacturaComercial.CA_SEGURO).Valor = item_.incremental
                                        .Campo(CamposFacturaComercial.CA_EMBALAJES).Valor = item_.incremental
                                        .Campo(CamposFacturaComercial.CA_OTROS_INCREMENTABLES).Valor = item_.incremental
                                        '.Campo(CamposFacturaComercial.CA_DESCUENTOS).Valor = item_.incremental
                                    Next

                                    If idmoneda_ IsNot Nothing AndAlso cvemoneda_ IsNot Nothing Then

                                        .Campo(CamposFacturaComercial.CA_MONEDA_FLETES).Valor = idmoneda_
                                        .Campo(CamposFacturaComercial.CA_MONEDA_FLETES).ValorPresentacion = cvemoneda_

                                        .Campo(CamposFacturaComercial.CA_MONEDA_SEGUROS).Valor = idmoneda_
                                        .Campo(CamposFacturaComercial.CA_MONEDA_SEGUROS).ValorPresentacion = cvemoneda_

                                        .Campo(CamposFacturaComercial.CA_MONEDA_EMBALAJES).Valor = idmoneda_
                                        .Campo(CamposFacturaComercial.CA_MONEDA_EMBALAJES).ValorPresentacion = cvemoneda_

                                        .Campo(CamposFacturaComercial.CA_MONEDA_OTROS_INCREMENTABLES).Valor = idmoneda_
                                        .Campo(CamposFacturaComercial.CA_MONEDA_OTROS_INCREMENTABLES).ValorPresentacion = cvemoneda_

                                        '.Campo(CamposFacturaComercial.CA_MONEDA_DESCUENTOS).Valor = idmoneda_
                                        '.Campo(CamposFacturaComercial.CA_MONEDA_DESCUENTOS).ValorPresentacion = cvemoneda_

                                    Else

                                        .Campo(CamposFacturaComercial.CA_MONEDA_FLETES).Valor = objectidZero_
                                        .Campo(CamposFacturaComercial.CA_MONEDA_FLETES).ValorPresentacion = estructura_.invoicecurrency

                                        .Campo(CamposFacturaComercial.CA_MONEDA_SEGUROS).Valor = objectidZero_
                                        .Campo(CamposFacturaComercial.CA_MONEDA_SEGUROS).ValorPresentacion = estructura_.invoicecurrency

                                        .Campo(CamposFacturaComercial.CA_MONEDA_EMBALAJES).Valor = objectidZero_
                                        .Campo(CamposFacturaComercial.CA_MONEDA_EMBALAJES).ValorPresentacion = estructura_.invoicecurrency

                                        .Campo(CamposFacturaComercial.CA_MONEDA_OTROS_INCREMENTABLES).Valor = objectidZero_
                                        .Campo(CamposFacturaComercial.CA_MONEDA_OTROS_INCREMENTABLES).ValorPresentacion = estructura_.invoicecurrency

                                        '.Campo(CamposFacturaComercial.CA_MONEDA_DESCUENTOS).Valor = estructura_.invoicecurrency
                                        '.Campo(CamposFacturaComercial.CA_MONEDA_DESCUENTOS).ValorPresentacion = estructura_.invoicecurrency
                                    End If

                                End If

                            End If

                        End If

                    End With

                End If

                .UsuarioGenerador = user_
                .IdDocumento = secuencia_.sec
                .FolioDocumento = estructura_.invoicenumber
                .FolioOperacion = secuencia_.sec
                .TipoPropietario = SecuenciasComercioExterior.FacturasComerciales.ToString
                .TipoDocumentoElectronico = TiposDocumentoElectronico.FacturaComercial

                .Metadatos = New List(Of CampoGenerico) From {
                    .Campo(CamposFacturaComercial.CP_TIPO_OPERACION)
                }

                If TipoOperacion = IControladorFacturaComercial.TipoOperaciones.Exportacion Then

                    .Metadatos.LastOrDefault.Valor = 2
                    .Metadatos.LastOrDefault.ValorPresentacion = "Exportacion"
                Else

                    .Metadatos.LastOrDefault.Valor = 1
                    .Metadatos.LastOrDefault.ValorPresentacion = "Importacion"

                End If

            End With

            Dim operacionGenerica_ As New OperacionGenerica(_documentoElectronico)

            With operacionGenerica_

                .FolioOperacion = _secuencia.sec

                If Not objectIdCliente_ = ObjectId.Empty Then

                    .Borrador.Folder.DocumentosAsociados = New List(Of DocumentoAsociado) From {(New DocumentoAsociado With {
                        .analisisconsistencia = 1,
                        .identificadorrecurso = "ConstructorCliente",
                        ._iddocumentoasociado = objectIdCliente_,
                        .firmaelectronica = firmaelectronicaCliente_})}

                End If

            End With

            .ObjectReturned = New ResponseOperacion With {
                    .OperacionGenerica = operacionGenerica_,
                    .CommercialInvoice = estructura_
                    }

            .SetOK()

        End With

        Return Estado

    End Function

    Protected Function ObtenerOperGenericaFacturaPublicada(ByVal objectIdFactura_ As ObjectId) As TagWatcher

        Dim estadoPreasingacion_ As New TagWatcher

        With estadoPreasingacion_

            Try
                Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos(21)

                    enlaceDatos_.EnvironmentOnline = _environmentOnline

                    Dim collection_ = enlaceDatos_.GetMongoCollection(Of OperacionGenerica)((New ConstructorFacturaComercial).GetType.Name)

                    Dim filtroEstado_ As FilterDefinition(Of OperacionGenerica) =
                          Builders(Of OperacionGenerica).Filter.Eq(Function(x) x.estado, 1)

                    Dim filtroPublicado_ As FilterDefinition(Of OperacionGenerica) =
                    Builders(Of OperacionGenerica).Filter.Eq(Function(x) x.Publicado, True)

                    Dim filtroFirmaelectronica_ As FilterDefinition(Of OperacionGenerica) =
                    Builders(Of OperacionGenerica).Filter.Ne(Function(x) x.FirmaElectronica, Nothing)

                    Dim filtroTipoOperacion_ As FilterDefinition(Of OperacionGenerica) =
                    Builders(Of OperacionGenerica).Filter.Eq(Function(x) DirectCast(x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts("Encabezado")(0).Nodos(0).Nodos(0).Nodos(0), Campo).Valor, IControladorFacturaComercial.TipoOperaciones.Importacion)

                    Dim filtroObjectId_ As FilterDefinition(Of OperacionGenerica) =
                        Builders(Of OperacionGenerica).Filter.Eq(Function(x) x.Id, objectIdFactura_)

                    Dim filtroCombinado_ As FilterDefinition(Of OperacionGenerica) =
                    Builders(Of OperacionGenerica).Filter.And(
                        filtroEstado_,
                        filtroPublicado_,
                        filtroFirmaelectronica_,
                        filtroTipoOperacion_,
                        filtroObjectId_)

                    'Dim result_ = collection_.Aggregate().Match(filtroCombinado_).ToList()

                    Dim result_ = collection_.Find(filtroCombinado_).Limit(1).ToList()

                    If result_.Count > 0 Then

                        .SetOK()

                        .ObjectReturned = result_(0)

                    Else

                        .SetError(Me, $"Factura comercial no encontrada con id: {objectIdFactura_}")

                    End If


                End Using

            Catch ex As Exception

                .SetError(Me, $"Ha ocurrido un error_: {ex}")

            End Try

        End With

        Return estadoPreasingacion_

    End Function

    Protected Function GenerarOperacionGenericaDeFactSubdividida(ByVal comercialinvoiceSubdividida_ As SubdivisionFacturaComercial,
                                                              ByVal userGenero_ As String, ByVal idCustomOperGenerica_ As ObjectId,
                                                              ByVal idOperGenericaOriginal_ As ObjectId) As TagWatcher

        Dim estadoOperacionGenerica_ As New TagWatcher

        With estadoOperacionGenerica_

            Try

                Dim estadoFactOriginal_ As TagWatcher = ObtenerOperGenericaFacturaPublicada(idOperGenericaOriginal_)

                If estadoFactOriginal_.Status = TypeStatus.Ok Then

                    ''OBTENERMOS LA OPERACION GEN ORIGINAL

                    Dim operGenericaFactOriginal_ As OperacionGenerica = DirectCast(estadoFactOriginal_.ObjectReturned, OperacionGenerica)

                    Dim documentosAsociadosOriginal_ As List(Of DocumentoAsociado) = operGenericaFactOriginal_.Borrador.Folder.DocumentosAsociados

                    Dim doctoAsociadoFactOriginal_ As New DocumentoAsociado With {
                              .analisisconsistencia = 1,
                              .identificadorrecurso = "ConstructorFacturaComercial",
                              ._iddocumentoasociado = operGenericaFactOriginal_.Id,
                              .firmaelectronica = operGenericaFactOriginal_.FirmaElectronica
                        }

                    documentosAsociadosOriginal_.Add(doctoAsociadoFactOriginal_)

                    ''LE SACAMOS COPIA // NO TIENE EL CLONE :V
                    ''ESTO NO TIENE CLONE ASI QUE VAMOS DIRECTO AL CONSTRUCTOR_ :V

                    _documentoElectronico = New DocumentoElectronico

                    _documentoElectronico = operGenericaFactOriginal_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Clone()

                    Dim operGenericaFactCopia_ As New OperacionGenerica(_documentoElectronico)

                    Dim secuencia_ As ISecuencia = GenerarSecuenciaDocumentoElectronico(SecuenciasComercioExterior.FacturasComerciales.ToString)


                    ''AHORA REEMPLAZAMOS POR LOS CAMPOS CHIDOS, A ESTE NIVEL, YO CREO QUE WOA A GORDAR TODO EL ITEM COMPLETO
                    ''DE LAS SUBDIVISIONES, PARA NAMAS INSERTAR :V SI NO TENDRIA QUE HACER MAS ROLLO
                    ''Y LA NETFLIX NO HAY TIEMPO PARA PURISMOS :V QUE SE PREOCUPE EL DESARROLLADOR DEL FUTURO
                    ''QUIZÁ SEA YO JAJAJA

                    With operGenericaFactCopia_

                        .Id = idCustomOperGenerica_
                        .FolioOperacion = secuencia_.sec ''LLAMAR A LA SECUENCIA

                        .Borrador.Folder.DocumentosAsociados = documentosAsociadosOriginal_

                        With _documentoElectronico

                            ''AMOS A LLENAR LOS DATOS DE LA JUENTE
                            .Id = idCustomOperGenerica_.ToString
                            .IdDocumento = secuencia_.sec

                            .FechaCreacion = DateTime.Now() ''AAHORITA CHECOSI ASI
                            .UsuarioGenerador = userGenero_
                            .FolioOperacion = secuencia_.sec ''LA SECUENCIA
                            .FolioDocumento = comercialinvoiceSubdividida_.numerofactura_subdivision

                            With _documentoElectronico.Seccion(SeccionesFacturaComercial.SFAC1)

                                .Campo(CamposFacturaComercial.CA_NUMERO_FACTURA).Valor = comercialinvoiceSubdividida_.numerofactura_original ''CHECAR SI ES EL MISMO QUE EL ORIGINAL

                                .Campo(CamposFacturaComercial.CP_VALOR_FACTURA).Valor = comercialinvoiceSubdividida_.valorfactura_general ''VIENE DE LA SUBDIVIDIDA
                                .Campo(CamposFacturaComercial.CA_MONEDA_FACTURACION).Valor = comercialinvoiceSubdividida_.cve_moneda_valorfactura
                                .Campo(CamposFacturaComercial.CA_MONEDA_FACTURACION).ValorPresentacion = comercialinvoiceSubdividida_.moneda_valorfactura

                                .Campo(CamposFacturaComercial.CP_VALOR_MERCANCIA).Valor = comercialinvoiceSubdividida_.valor_mercancia_total ''VIENE DE LA SUBDIVIDIDA
                                .Campo(CamposFacturaComercial.CP_MONEDA_VALOR_MERCANCIA).Valor = comercialinvoiceSubdividida_.cve_moneda_mercancia
                                .Campo(CamposFacturaComercial.CP_MONEDA_VALOR_MERCANCIA).ValorPresentacion = comercialinvoiceSubdividida_.moneda_valor_mercancia_total

                                .Campo(CamposFacturaComercial.CA_APLICA_SUBDIVISION).Valor = False
                                .Campo(CamposFacturaComercial.CP_ID_FACTURA_ORIGINAL).Valor = idOperGenericaOriginal_

                                .Campo(CamposFacturaComercial.CP_NUMERO_FACTURA_SUBDIVISION).Valor = comercialinvoiceSubdividida_.numerofactura_subdivision ''VIENE DE LA SUBDIVIDIDA


                                ''AQUI VIENE LO CHIDO :V
                                Dim i_ = 0

                                _documentoElectronico.Seccion(SeccionesFacturaComercial.SFAC4).Nodos = New List(Of Nodo)

                                If comercialinvoiceSubdividida_.items IsNot Nothing Then

                                    For Each item_ In comercialinvoiceSubdividida_.items

                                        i_ += 1

                                        Dim partida_ = _documentoElectronico.Seccion(SeccionesFacturaComercial.SFAC4).Partida(_documentoElectronico)

                                        With partida_

                                            .Campo(CamposFacturaComercial.CP_OBJECTID_PRODUCTOS).Valor = item_.id_producto

                                            .Campo(CamposFacturaComercial.CP_NUMERO_PARTIDA).Valor = item_.numero_partida

                                            .Campo(CamposFacturaComercial.CA_NUMERO_PARTE_PARTIDA).Valor = item_.id_producto.ToString ''VA EL OBJECTDI
                                            .Campo(CamposFacturaComercial.CA_NUMERO_PARTE_PARTIDA).ValorPresentacion = item_.numeropartecompleto ''FALTA LA PARTE COMPLETA

                                            .Campo(CamposFacturaComercial.CA_CANTIDAD_COMERCIAL_PARTIDA).Valor = item_.cantidad_comercial_requerida
                                            ''AQUI SE GUARDA O NO, HAY ALGO EXTRAÑO
                                            .Campo(CamposFacturaComercial.CA_UNIDAD_MEDIDA_COMERCIAL_PARTIDA).Valor = item_.cve_unidad_medida_comercial
                                            .Campo(CamposFacturaComercial.CA_UNIDAD_MEDIDA_COMERCIAL_PARTIDA).ValorPresentacion = item_.unidad_comercial

                                            .Campo(CamposFacturaComercial.CA_DESCRIPCION_PARTE_PARTIDA).Valor = item_.descripcion_partida

                                            .Campo(CamposFacturaComercial.CA_DESCRIPCION_PARTE_PARTIDA_ORIGINAL).Valor = item_.descripcion_merca_original

                                            ''VALORES IMPO
                                            .Campo(CamposFacturaComercial.CA_VALOR_FACTURA_PARTIDA).Valor = item_.val_fact_partida
                                            '.Campo(CamposFacturaComercial.CA_VALOR_DOLARES_PARTIDA).Valor = item_.va
                                            '.Campo(CamposFacturaComercial.CA_VALOR_UNITARIO_PARTIDA).Valor = item_

                                            ''VALORES EXPO
                                            .Campo(CamposFacturaComercial.CA_VALOR_MERCANCIA_PARTIDA).Valor = item_.valor_mercancia
                                            .Campo(CamposFacturaComercial.CA_PRECIO_UNITARIO_PARTIDA).Valor = item_.precio_unitario

                                            ''VALORES IMPO
                                            .Campo(CamposFacturaComercial.CP_MONEDA_FACTURA_PARTIDA).Valor = item_.idmoneda_val_fact_partida
                                            .Campo(CamposFacturaComercial.CP_MONEDA_FACTURA_PARTIDA).ValorPresentacion = item_.moneda_val_fact_partida

                                            .Campo(CamposFacturaComercial.CA_MONEDA_VALOR_UNITARIO_PARTIDA).Valor = item_.idmoneda_precio_unitario_original
                                            .Campo(CamposFacturaComercial.CA_MONEDA_VALOR_UNITARIO_PARTIDA).ValorPresentacion = item_.moneda_precio_unitario

                                            '.Campo(CamposFacturaComercial.CP_MONEDA_VALOR_DOLARES_PARTIDA).Valor = item_.val
                                            '.Campo(CamposFacturaComercial.CP_MONEDA_VALOR_DOLARES_PARTIDA).ValorPresentacion = item_

                                            ''VALORES EXPO
                                            .Campo(CamposFacturaComercial.CA_MONEDA_MERCANCIA_PARTIDA).Valor = item_.idmoneda_valor_mercancia_original
                                            .Campo(CamposFacturaComercial.CA_MONEDA_MERCANCIA_PARTIDA).ValorPresentacion = item_.moneda_mercancia

                                            .Campo(CamposFacturaComercial.CP_MONEDA_PRECIO_UNITARIO).Valor = item_.idmoneda_precio_unitario_original
                                            .Campo(CamposFacturaComercial.CP_MONEDA_PRECIO_UNITARIO).ValorPresentacion = item_.moneda_precio_unitario

                                            .Campo(CamposFacturaComercial.CA_PESO_NETO_PARTIDA).Valor = item_.peso_neto_partida
                                            .Campo(CamposFacturaComercial.CA_DESCRIPCION_COVE_PARTIDA).Valor = item_.descripcion_merca_cove

                                            .Campo(CamposFacturaComercial.CA_PAIS_ORIGEN_PARTIDA).Valor = item_.id_pais_origen
                                            .Campo(CamposFacturaComercial.CA_PAIS_ORIGEN_PARTIDA).ValorPresentacion = item_.pais_origen

                                            .Campo(CamposFacturaComercial.CA_CVE_METODO_VALORACION_PARTIDA).Valor = item_.cve_metodo_val_partida
                                            .Campo(CamposFacturaComercial.CA_CVE_METODO_VALORACION_PARTIDA).ValorPresentacion = item_.metodo_val_partida

                                            .Campo(CamposFacturaComercial.CP_ORDEN_COMPRA_PARTIDA).Valor = item_.orden_compra_partida

                                            .Campo(CamposFacturaComercial.CP_CANTIDAD_FACTURA_PARTIDA).Valor = item_.cantidad_comercial_requerida
                                            ''SE ANDA PELEANDO CON EL DE ARRIBA
                                            .Campo(CamposFacturaComercial.CP_UNIDAD_MEDIDA_FACTURA_PARTIDA).Valor = item_.cve_unidad_medida_comercial
                                            .Campo(CamposFacturaComercial.CP_UNIDAD_MEDIDA_FACTURA_PARTIDA).ValorPresentacion = item_.unidad_comercial

                                            .Campo(CamposFacturaComercial.CA_FRACCION_ARANCELARIA_PARTIDA).Valor = item_.fraccion
                                            .Campo(CamposFacturaComercial.CA_FRACCION_ARANCELARIA_PARTIDA).ValorPresentacion = item_.fraccion_descripcion


                                            .Campo(CamposFacturaComercial.CA_CANTIDAD_TARIFA_PARTIDA).Valor = item_.cantidad_tarifa_requerida
                                            .Campo(CamposFacturaComercial.CA_UNIDAD_MEDIDA_TARIFA_PARTIDA).Valor = item_.cve_unidad_medida_tarifa
                                            .Campo(CamposFacturaComercial.CA_UNIDAD_MEDIDA_TARIFA_PARTIDA).ValorPresentacion = item_.unidad_medida_tarifa
                                            .Campo(CamposFacturaComercial.CA_FRACCION_NICO_PARTIDA).Valor = item_.nico
                                            .Campo(CamposFacturaComercial.CA_FRACCION_NICO_PARTIDA).ValorPresentacion = item_.nico_descripcion

                                            .Campo(CamposFacturaComercial.CA_LOTE_PARTIDA).Valor = item_.lote_part
                                            .Campo(CamposFacturaComercial.CA_NUMERO_SERIE_PARTIDA).Valor = item_.numero_serie_part
                                            .Campo(CamposFacturaComercial.CA_MARCA_PARTIDA).Valor = item_.marca_part
                                            .Campo(CamposFacturaComercial.CA_MODELO_PARTIDA).Valor = item_.modelo_part
                                            .Campo(CamposFacturaComercial.CA_SUBMODELO_PARTIDA).Valor = item_.submodelo_part
                                            .Campo(CamposFacturaComercial.CA_KILOMETRAJE_PARTIDA).Valor = item_.kilometraje_part
                                            .Campo(CamposProducto.CP_TIMESTAMP).Valor = item_.timestamp_part

                                            .Campo(CamposGlobales.CP_IDENTITY).Valor = i_

                                        End With

                                    Next

                                End If

                            End With

                        End With

                    End With

                    .SetOK()

                    ''PRIMERA ETAPA QUE ESTE LLENA LA OPER COPIA

                    .ObjectReturned = operGenericaFactCopia_

                Else

                    .SetError(Me, $"Factura solicitada no encontrada con id : {idOperGenericaOriginal_}")

                End If

            Catch ex As Exception

                .SetError(Me, $"Ha ocurrido un error_ {ex}")

            End Try

        End With

        Return estadoOperacionGenerica_

    End Function

    Protected Function GuardarPreasignacionFacturaSubdividida(ByVal comercialinvoiceSubdividida_ As SubdivisionFacturaComercial,
                                                              ByVal idOperGenericaOriginal_ As ObjectId,
                                                              ByVal idCustomOperGenerica_ As ObjectId,
                                                              ByVal userGenero_ As String) As TagWatcher

        With Estado

            Try

                Dim estadoOperGen_ As TagWatcher = GenerarOperacionGenericaDeFactSubdividida(comercialinvoiceSubdividida_, userGenero_, idCustomOperGenerica_, idOperGenericaOriginal_)

                If estadoOperGen_.Status = TypeStatus.Ok Then

                    Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos(21)

                        enlaceDatos_.EnvironmentOnline = _environmentOnline

                        With Estado

                            Try

                                Dim client = enlaceDatos_.GetMongoClient

                                Using session = client.StartSession()

                                    session.StartTransaction()

                                    Dim operacionGenFactura_ As OperacionGenerica = estadoOperGen_.ObjectReturned

                                    Dim operacionesCollection =
                                    enlaceDatos_.GetMongoCollection(Of OperacionGenerica)((New ConstructorFacturaComercial).GetType.Name)

                                    operacionesCollection.InsertOne(operacionGenFactura_)

                                    Dim estadoFirmarDocumentos_ = enlaceDatos_.FirmarDocumento("ConstructorFacturaComercial", idCustomOperGenerica_, 0)

                                    session.CommitTransaction()

                                    .SetOK()

                                End Using

                            Catch ex As Exception

                                .SetError(Me, $"Ha ocurrido un error_ {ex}")

                            End Try

                        End With

                    End Using

                Else

                    .SetError(Me, $"Operacion generica no generada")

                End If

            Catch ex As Exception

                .SetError(Me, $"Ha ocurrido un error_ {ex}")

            End Try

        End With

        Return Estado

    End Function


    Protected Function GuardarPreasignacionFacturaComercial(ByRef estructura_ As CommercialInvoiceGeneric,
                                                             ByVal user_ As String, Optional idCustomOperGenerica_ As ObjectId = Nothing) As TagWatcher

        ''GENERAMOS EL DOCUMENTO ELECTRONICO U OPERACION GENERICA 'MANUAL'
        Try

            Dim estadoOperacionGenerica_ As TagWatcher = GenerarOperacionGenericaFacturaComercial(estructura_, user_)

            If estadoOperacionGenerica_.Status = TypeStatus.Ok Then

                Dim resultOperation_ As ResponseOperacion = DirectCast(estadoOperacionGenerica_.ObjectReturned, ResponseOperacion)

                Dim operacionGenerica_ As OperacionGenerica = resultOperation_.OperacionGenerica

                If idCustomOperGenerica_ = ObjectId.Empty Then

                    idCustomOperGenerica_ = ObjectId.GenerateNewId()

                End If

                operacionGenerica_.Id = idCustomOperGenerica_

                operacionGenerica_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Id = idCustomOperGenerica_.ToString

                Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos(21)

                    enlaceDatos_.EnvironmentOnline = _environmentOnline

                    With Estado

                        Dim operacionesCollection =
                            enlaceDatos_.GetMongoCollection(Of OperacionGenerica)((New ConstructorFacturaComercial).GetType.Name)

                        operacionesCollection.InsertOne(operacionGenerica_)

                        Dim response_ As New ResponseOperacion _
                                    With {
                                            .id = operacionGenerica_.Id,
                                            .OperacionGenerica = operacionGenerica_,
                                            .CommercialInvoice = Nothing
                                    }

                        .ObjectReturned = response_

                        .SetOK()

                    End With

                End Using
            Else

                Estado.SetError(Me, "Ha ocurrido un error preasignacion_")

            End If

        Catch ex As Exception

            Estado.SetError(Me, "Factura no insertada")

        End Try

        Return Estado

    End Function

    Protected Function GuardarPreasignacionFacturaCommercialIA(ByRef estructura_ As CommercialInvoiceAnalysis,
                                                               ByVal user_ As String, ByVal idCustomOperGenerica_ As ObjectId) As TagWatcher

        ''GENERAMOS EL DOCUMENTO ELECTRONICO U OPERACION GENERICA 'MANUAL'

        With Estado

            Try
                Dim estadoOperacionGenerica_ As TagWatcher = GenerarOperacionGenericaFacturaComercial(estructura_, user_, False)

                If estadoOperacionGenerica_.Status = TypeStatus.Ok Then

                    Dim resultOperation_ As ResponseOperacion = DirectCast(Estado.ObjectReturned, ResponseOperacion)

                    Dim operacionGenerica_ As OperacionGenerica = resultOperation_.OperacionGenerica

                    operacionGenerica_.Id = idCustomOperGenerica_

                    operacionGenerica_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Id = idCustomOperGenerica_.ToString

                    Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos(21)

                        enlaceDatos_.EnvironmentOnline = _environmentOnline

                        Dim client = enlaceDatos_.GetMongoClient

                        Using session = client.StartSession()

                            session.StartTransaction()

                            Try
                                Dim operacionesCollection =
                            enlaceDatos_.GetMongoCollection(Of OperacionGenerica)(
                                (New ConstructorFacturaComercial).GetType.Name)

                                operacionesCollection.InsertOne(session, operacionGenerica_)

                                Dim analysisCollection =
                            enlaceDatos_.GetMongoCollection(Of CommercialInvoiceAnalysis)(
                                "Reg012CommercialInvoicesAnalysis")

                                Dim estructuraGenerica_ =
                            DirectCast(CType(estructura_, Object), CommercialInvoiceAnalysis)

                                analysisCollection.InsertOne(session, estructuraGenerica_)

                                session.CommitTransaction()

                                .SetOK()

                            Catch ex As Exception

                                session.AbortTransaction()

                                .SetError(Me, "Factura comercial no insertada")

                            End Try

                        End Using

                    End Using

                Else

                    .SetError(Me, "Ha ocurrido un error en la preasingación_")

                End If

            Catch ex As Exception

                .SetError(Me, "Factura comercial no insertada")

            End Try

        End With

        Return Estado

    End Function

    Private Function ComprobarCFDIenRegistros(ByVal folioFacturaComercial_ As String) As TagWatcher

        Dim estadoFactura_ As New TagWatcher

        With estadoFactura_

            Try
                Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos(21)

                    enlaceDatos_.EnvironmentOnline = _environmentOnline

                    Dim collection_ = enlaceDatos_.GetMongoCollection(Of OperacionGenerica)((New ConstructorFacturaComercial).GetType.Name)

                    Dim filter_ = Builders(Of OperacionGenerica).Filter.And(
                    Builders(Of OperacionGenerica).Filter.Eq(Function(x) x.estado, 1),
                    Builders(Of OperacionGenerica).Filter.Eq(Function(x) x.abierto, True),
                    Builders(Of OperacionGenerica).Filter.Eq(Function(x) x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.FolioDocumento, folioFacturaComercial_))

                    Dim result_ = collection_.Find(filter_).ToList()

                    If result_.Count > 0 Then

                        .ObjectReturned = result_

                        .SetOK()
                    Else
                        .ObjectReturned = Nothing

                        .SetOKBut(Me, "Factura comercial no disponible")

                    End If

                End Using

            Catch ex As Exception

                .ObjectReturned = $"Ha ocurrido un error_ {ex}"

                .SetError(Me, $"Ha ocurrido un error_ {ex}")

            End Try

        End With

        Return estadoFactura_

    End Function


    Private Function GenerarCommercialInvoiceDesdeCFDI(cfdi As CFDIFacturaComercial) As TagWatcher
        ''igual y generamos directamente la preasingnacion, a no, pero si necesitpo el comeericla invoice, creo que ya no, osea pa que?????
        Dim estadoCFDI_ As New TagWatcher

        With estadoCFDI_

            Try

                _facturaComercialcfdi = New CommercialInvoiceCFDI

                If cfdi.Version <> "3.3" Then

                    ''PENDIENTE

                    'Dim existeCFDIenRegistros_ As TagWatcher = ComprobarCFDIenRegistros(cfdi.UUID)

                    ' If existeCFDIenRegistros_.Status = TypeStatus.OkBut Then

                    ''HAY QUE IR A BUSCAR UN PAIS DE LA MONEDA :V
                    '_controladorPais = New ControladorPaises
                    '_controladorPais.

                    'cfdi.Moneda


                    With _facturaComercialcfdi
                        .invoicenumber = cfdi.UUID
                        .invoicedate = cfdi.Fecha.ToString("yyyy-MM-dd")
                        .invoicecurrency = cfdi.Moneda
                        .totalinvoice = cfdi.Total
                        .invoicecountry = "USA" ''cambiar aqui esto no debe ir asi :V
                        .invoiceseries = cfdi.Folio
                        .customername = cfdi.Emisor.Nombre
                        .suppliername = cfdi.Receptor.Nombre

                        If cfdi.EmisorDomicilio IsNot Nothing Then
                            .customer = New Digitalization.Customer With {
                           .customername = cfdi.Emisor.Nombre,
                           .rfc = cfdi.Emisor.Rfc,
                           .country = cfdi.EmisorDomicilio.Pais,
                           .address = $"{cfdi.EmisorDomicilio.Calle} {cfdi.EmisorDomicilio.NumeroExterior} {cfdi.EmisorDomicilio.NumeroInterior} {cfdi.EmisorDomicilio.Ciudad} {cfdi.EmisorDomicilio.Localidad} {cfdi.EmisorDomicilio.Estado} {cfdi.EmisorDomicilio.CodigoPostal} {cfdi.EmisorDomicilio.Pais}",
                           .city = cfdi.EmisorDomicilio.Ciudad,
                           .externalnumber = cfdi.EmisorDomicilio.NumeroExterior,
                           .internalnumber = cfdi.EmisorDomicilio.NumeroInterior,
                           .locality = cfdi.EmisorDomicilio.Localidad,
                           .state = cfdi.EmisorDomicilio.Estado,
                           .street = cfdi.EmisorDomicilio.Calle,
                           .zipcode = cfdi.EmisorDomicilio.CodigoPostal}
                        Else
                            .customer = New Digitalization.Customer With {
                          .customername = cfdi.Emisor.Nombre,
                          .rfc = cfdi.Emisor.Rfc}
                        End If



                        If cfdi.ReceptorDomicilio IsNot Nothing Then

                            .supplier = New Digitalization.Supplier With {
                            .supliername = cfdi.Receptor.Nombre,
                            .taxid = cfdi.Receptor.Rfc,
                            .country = cfdi.ReceptorDomicilio.Pais,
                            .address = $"{cfdi.ReceptorDomicilio.Calle} {cfdi.ReceptorDomicilio.NumeroExterior} {cfdi.ReceptorDomicilio.NumeroInterior} {cfdi.ReceptorDomicilio.Ciudad} {cfdi.ReceptorDomicilio.Localidad} {cfdi.ReceptorDomicilio.Estado} {cfdi.ReceptorDomicilio.CodigoPostal} {cfdi.ReceptorDomicilio.Pais}",
                            .city = cfdi.ReceptorDomicilio.Ciudad,
                            .externalnumber = cfdi.ReceptorDomicilio.NumeroExterior,
                            .internalnumber = cfdi.ReceptorDomicilio.NumeroInterior,
                            .locality = cfdi.ReceptorDomicilio.Localidad,
                            .state = cfdi.ReceptorDomicilio.Estado,
                            .street = cfdi.ReceptorDomicilio.Calle,
                            .zipcode = cfdi.ReceptorDomicilio.CodigoPostal
                        }
                        Else

                            .supplier = New Digitalization.Supplier With {
                            .supliername = cfdi.Receptor.Nombre,
                            .taxid = cfdi.Receptor.Rfc}

                        End If

                        .itemscfdi_ = New List(Of ItemCfdi)
                        Dim secuencia As Integer = 1
                        Dim i As Integer = 0

                        If cfdi.ReceptorDomicilio IsNot Nothing Then

                            For Each c In cfdi.Conceptos
                                Dim nuevoItem As New ItemCfdi With {
                                    .sec = secuencia,
                                    .partnumber = c.NoIdentificacion,
                                    .description = c.Descripcion,
                                    .quantity = c.Cantidad,
                                    .unitprice = c.ValorUnitario,
                                    .total = c.Importe,
                                    .currency = cfdi.Moneda,
                                    .usdvalue = c.Importe,
                                    .productid = Nothing,
                                    .discount = Nothing,
                                    .netweight = Nothing,
                                    .purchaseorder = Nothing,
                                    .destinationcountry = IIf(cfdi.ReceptorDomicilio.Pais IsNot Nothing, cfdi.ReceptorDomicilio.Pais, Nothing),
                                    .origincountry = IIf(cfdi.ReceptorDomicilio.Pais IsNot Nothing, cfdi.ReceptorDomicilio.Pais, Nothing),
                                    .Cantidad = c.Cantidad,
                                    .ClaveUnidad = c.ClaveUnidad,
                                    .Unidad = c.Unidad,
                                    .ValorUnitario = c.ValorUnitario
                                }

                                With nuevoItem

                                    If cfdi.Complemento?.ComercioExterior IsNot Nothing Then

                                        ' VALIDACIÓN CRÍTICA: Solo intentamos leer Mercancías si existe ComercioExterior
                                        If cfdi.Complemento?.ComercioExterior?.Mercancias IsNot Nothing AndAlso
                                    cfdi.Complemento.ComercioExterior.Mercancias.Count > i Then

                                            Dim mercancia = cfdi.Complemento.ComercioExterior.Mercancias(i)

                                            .sku = mercancia.NoIdentificacion
                                            .unit = mercancia.UnidadAduana
                                            .value = mercancia.ValorUnitarioAduana
                                            .FraccionArancelaria = mercancia.FraccionArancelaria
                                            .CantidadAduana = mercancia.CantidadAduana
                                            .UnidadAduana = mercancia.UnidadAduana
                                            .ValorDolares = mercancia.ValorDolares
                                            .ValorUnitarioAduana = mercancia.ValorUnitarioAduana

                                        Else
                                            ' Si no hay complemento, ponemos valores por default o lo que gustes
                                            .sku = c.NoIdentificacion
                                            .unit = Nothing
                                            .value = Nothing
                                        End If

                                    End If

                                End With

                                .itemscfdi_.Add(nuevoItem)

                                secuencia += 1
                                i += 1
                            Next
                        Else

                            For Each c In cfdi.Conceptos
                                Dim nuevoItem As New ItemCfdi With {
                                    .sec = secuencia,
                                    .partnumber = c.NoIdentificacion,
                                    .description = c.Descripcion,
                                    .quantity = c.Cantidad,
                                    .unitprice = c.ValorUnitario,
                                    .total = c.Importe,
                                    .currency = cfdi.Moneda,
                                    .usdvalue = c.Importe,
                                    .productid = Nothing,
                                    .discount = Nothing,
                                    .netweight = Nothing,
                                    .purchaseorder = Nothing,
                                    .destinationcountry = Nothing,
                                    .origincountry = Nothing,
                                    .Cantidad = c.Cantidad,
                                    .ClaveUnidad = c.ClaveUnidad,
                                    .Unidad = c.Unidad,
                                    .ValorUnitario = c.ValorUnitario
                                }

                                With nuevoItem

                                    If cfdi.Complemento?.ComercioExterior IsNot Nothing Then



                                        ' VALIDACIÓN CRÍTICA: Solo intentamos leer Mercancías si existe ComercioExterior
                                        If cfdi.Complemento?.ComercioExterior?.Mercancias IsNot Nothing AndAlso
                                    cfdi.Complemento.ComercioExterior.Mercancias.Count > i Then

                                            Dim mercancia = cfdi.Complemento.ComercioExterior.Mercancias(i)

                                            .sku = mercancia.NoIdentificacion
                                            .unit = mercancia.UnidadAduana
                                            .value = mercancia.ValorUnitarioAduana
                                            .FraccionArancelaria = mercancia.FraccionArancelaria
                                            .CantidadAduana = mercancia.CantidadAduana
                                            .UnidadAduana = mercancia.UnidadAduana
                                            .ValorDolares = mercancia.ValorDolares
                                            .ValorUnitarioAduana = mercancia.ValorUnitarioAduana

                                        Else
                                            ' Si no hay complemento, ponemos valores por default o lo que gustes
                                            .sku = c.NoIdentificacion
                                            .unit = Nothing
                                            .value = Nothing
                                        End If

                                    End If

                                End With

                                .itemscfdi_.Add(nuevoItem)

                                secuencia += 1
                                i += 1
                            Next

                        End If

                        If cfdi.DestinatarioDomicilio IsNot Nothing Then

                            If cfdi.Complemento.ComercioExterior.Destinatario.Nombre IsNot Nothing Then

                                .consigneedetails = New Digitalization.ConsigneeDetails With {
                                    .consigneedetailsname = cfdi.Complemento.ComercioExterior.Destinatario.Nombre,
                                    .taxid = cfdi.Complemento.ComercioExterior.Destinatario.NumRegIdTrib,
                                    .country = cfdi.DestinatarioDomicilio.Pais,
                                    .address = $"{cfdi.DestinatarioDomicilio.Calle} {cfdi.DestinatarioDomicilio.NumeroExterior}
                                            {cfdi.DestinatarioDomicilio.NumeroInterior} {cfdi.DestinatarioDomicilio.Ciudad} 
                                            {cfdi.DestinatarioDomicilio.Localidad} {cfdi.DestinatarioDomicilio.Estado} {cfdi.DestinatarioDomicilio.CodigoPostal} {cfdi.DestinatarioDomicilio.Pais}",
                                    .city = cfdi.DestinatarioDomicilio.Ciudad,
                                    .externalnumber = cfdi.DestinatarioDomicilio.NumeroExterior,
                                    .internalnumber = cfdi.DestinatarioDomicilio.NumeroInterior,
                                    .locality = cfdi.DestinatarioDomicilio.Localidad,
                                    .state = cfdi.DestinatarioDomicilio.Estado,
                                    .street = cfdi.DestinatarioDomicilio.Calle,
                                    .zipcode = cfdi.DestinatarioDomicilio.CodigoPostal
                                 }

                            Else

                                .consigneedetails = New Digitalization.ConsigneeDetails With {
                                    .consigneedetailsname = cfdi.Receptor.Nombre,
                                    .taxid = cfdi.Receptor.Rfc
                                }

                            End If

                        Else

                            If cfdi.DestinatarioDomicilio IsNot Nothing Then

                                .consigneedetails = New Digitalization.ConsigneeDetails With {
                                 .consigneedetailsname = cfdi.Complemento.ComercioExterior.Destinatario.Nombre,
                                 .taxid = cfdi.Complemento.ComercioExterior.Destinatario.NumRegIdTrib,
                                 .country = cfdi.DestinatarioDomicilio.Pais,
                                 .address = $"{cfdi.DestinatarioDomicilio.Calle} {cfdi.DestinatarioDomicilio.NumeroExterior} {cfdi.DestinatarioDomicilio.NumeroInterior} 
                         {cfdi.DestinatarioDomicilio.Ciudad} {cfdi.DestinatarioDomicilio.Localidad} 
                         {cfdi.DestinatarioDomicilio.Estado} {cfdi.DestinatarioDomicilio.CodigoPostal} {cfdi.DestinatarioDomicilio.Pais}",
                                 .city = cfdi.DestinatarioDomicilio.Ciudad,
                                 .externalnumber = cfdi.DestinatarioDomicilio.NumeroExterior,
                                 .internalnumber = cfdi.DestinatarioDomicilio.NumeroInterior,
                                 .locality = cfdi.DestinatarioDomicilio.Localidad,
                                 .state = cfdi.DestinatarioDomicilio.Estado,
                                 .street = cfdi.DestinatarioDomicilio.Calle,
                                 .zipcode = cfdi.DestinatarioDomicilio.CodigoPostal
                              }

                            Else

                                If cfdi.ReceptorDomicilio IsNot Nothing Then

                                    .consigneedetails = New Digitalization.ConsigneeDetails With {
                                    .consigneedetailsname = cfdi.Receptor.Nombre,
                                     .taxid = cfdi.Receptor.Rfc,
                                    .country = cfdi.ReceptorDomicilio.Pais,
                                    .address = $"{cfdi.ReceptorDomicilio.Calle} {cfdi.ReceptorDomicilio.NumeroExterior} {cfdi.ReceptorDomicilio.NumeroInterior} {cfdi.ReceptorDomicilio.Ciudad} {cfdi.ReceptorDomicilio.Localidad} {cfdi.ReceptorDomicilio.Estado} {cfdi.ReceptorDomicilio.CodigoPostal} {cfdi.ReceptorDomicilio.Pais}",
                                    .city = cfdi.ReceptorDomicilio.Ciudad,
                                    .externalnumber = cfdi.ReceptorDomicilio.NumeroExterior,
                                    .internalnumber = cfdi.ReceptorDomicilio.NumeroInterior,
                                    .locality = cfdi.ReceptorDomicilio.Localidad,
                                    .state = cfdi.ReceptorDomicilio.Estado,
                                    .street = cfdi.ReceptorDomicilio.Calle,
                                    .zipcode = cfdi.ReceptorDomicilio.CodigoPostal
                                 }

                                Else

                                    .consigneedetails = New Digitalization.ConsigneeDetails With {
                                    .consigneedetailsname = cfdi.Receptor.Nombre,
                                    .taxid = cfdi.Receptor.Rfc
                                }

                                End If

                            End If

                        End If


                        If cfdi.Complemento?.ComercioExterior IsNot Nothing Then

                            .additionaldetails = New Digitalization.AdditionalDetails With {
                            .incoterm = If(cfdi.Complemento?.ComercioExterior?.Incoterm, String.Empty)
                        }

                        End If

                    End With

                    .ObjectReturned = _facturaComercialcfdi

                    .SetOK()

                    'Else

                    '    .ObjectReturned = "UUID de factura comercial ya se encuentra registrado"

                    '    .SetOKBut(Me, "UUID de factura comercial ya se encuentra registrado")

                    ' End If

                Else

                    .ObjectReturned = "Solo se permiten cfdi version 4.1"

                    .SetOKBut(Me, "Solo se permiten cfdi version 4.1")

                End If

            Catch ex As Exception

                .ObjectReturned = $"Ha ocurrido un errir_ {ex}"

                .SetError(Me, $"Ha ocurrido un errir_ {ex}")

            End Try

        End With

        Return estadoCFDI_

    End Function

    ''' <summary>
    ''' Deserializa un XML de CFDI 3.3 o 4.0 automáticamente
    ''' </summary>
    Private Function DeserializarXML(xml As String) As CFDIFacturaComercial
        ''HAY QUE PONERLE TAGWATCGER HIJE
        Try
            ' 1. Limpieza de caracteres iniciales (BOM/basura)
            Dim inicioXml = xml.IndexOf("<")
            If inicioXml < 0 Then Throw New Exception("No es un XML válido")
            xml = xml.Substring(inicioXml).Trim()

            ' 2. CARGA Y LIMPIEZA DE NAMESPACES (El truco maestro)
            Dim doc As New XmlDocument()
            doc.LoadXml(xml)

            ' Usamos Regex para quitar xmlns y prefijos, dejando etiquetas limpias como <Comprobante>
            Dim xmlLimpio As String = System.Text.RegularExpressions.Regex.Replace(doc.OuterXml, "(xmlns:?[^=]*=[""][^""]*[""]|xsi:?[^=]*=[""][^""]*[""]|(?<=[<\s/])[a-zA-Z0-9]+:)", "")

            ' 3. Serializador relajado (sin esperar namespaces específicos)
            ' Forzamos que el Root sea "Comprobante" y el Namespace sea vacío
            Dim xRoot As New XmlRootAttribute("Comprobante")
            xRoot.Namespace = ""

            Dim serializer As New XmlSerializer(GetType(CFDIFacturaComercial), xRoot)

            Using reader As New StringReader(xmlLimpio)
                ' Usamos configuraciones que ignoran errores de caracteres
                Dim settings As New XmlReaderSettings() With {.CheckCharacters = False}
                Using xmlReader As XmlReader = XmlReader.Create(reader, settings)
                    Return CType(serializer.Deserialize(xmlReader), CFDIFacturaComercial)
                End Using
            End Using

        Catch ex As Exception
            Throw New Exception("Error al deserealizar cfdi: " & ex.Message, ex)
        End Try
    End Function

    Function DeserializarCFDI(xml_ As String, user_ As String) As TagWatcher _
        Implements IControladorFacturaComercial.DeserializarCFDI

        _Estado = New TagWatcher

        With _Estado

            Try

                Dim cfdiDeserealizado_ = DeserializarXML(xml_)

                If cfdiDeserealizado_.Version Is Nothing OrElse cfdiDeserealizado_.Version = "3.3" Then

                    .ObjectReturned = $"CFDI (.xml) no válido para esta operación"

                    .SetError(Me, $"Error_: CFDI (.xml) no válido para esta operación")

                Else

                    Dim estadoFacturaGeneradadesdecfdi_ As New TagWatcher

                    estadoFacturaGeneradadesdecfdi_ = GenerarCommercialInvoiceDesdeCFDI(cfdiDeserealizado_)

                    If estadoFacturaGeneradadesdecfdi_.Status = TypeStatus.Ok Then

                        Return GuardarPreasignacionFacturaComercial(estadoFacturaGeneradadesdecfdi_.ObjectReturned,
                                                                     user_)

                    Else
                        .ObjectReturned = $"{estadoFacturaGeneradadesdecfdi_.ObjectReturned}"

                        .SetOKBut(Me, $"{estadoFacturaGeneradadesdecfdi_.ObjectReturned}")

                    End If

                End If

            Catch ex As Exception

                .ObjectReturned = $"Error al deserealizar cfdi {ex}"

                .SetError(Me, $"Error al deserealizar cfdi {ex}")

            End Try

        End With

        Return _Estado

    End Function

    '''''''NO DEBERÍA IR AQUÍ PERO LO VAMOS HACER :V´´´´´´´´´´
    Public Function ActualizarIdFuenteFacturaComercial(ByVal idDocumentoElectronico_ As ObjectId) As TagWatcher

        Dim estado_ As New TagWatcher

        With estado_

            Try

                Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos(21)

                    enlaceDatos_.EnvironmentOnline = _environmentOnline

                    Dim collection_ = enlaceDatos_.GetMongoCollection(Of OperacionGenerica)((New ConstructorFacturaComercial).GetType.Name)

                    Dim filter_ = Builders(Of OperacionGenerica).Filter.Eq(Function(x) x.Id, idDocumentoElectronico_)

                    Dim setData_ = Builders(Of OperacionGenerica).Update.
                                Set(Of String)("Borrador.Folder.ArchivoPrincipal.Dupla.Fuente._id", idDocumentoElectronico_.ToString)

                    Dim result_ = collection_.UpdateOne(filter_, setData_)

                    If result_.MatchedCount <> 0 Then

                        .SetOK()

                    ElseIf result_.UpsertedId IsNot Nothing Then

                        .SetOK()

                    Else

                        .SetError(Me, "No se generaron cambios")

                    End If

                End Using

            Catch ex As Exception

                .SetOKBut(Me, $"Ha ocurrido un Error al actualizar documento electrónico {ex}")

            End Try

        End With

        Return estado_

    End Function

    Public Function ActualizarFuenteDocumentoElectronicoFacturaComercial(ByVal datosFuente_ As DatosFuente,
                                                                                ByVal idDocumentoElectronico_ As ObjectId) As TagWatcher

        Dim estado_ As New TagWatcher

        With estado_

            Try

                Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos(21)

                    enlaceDatos_.EnvironmentOnline = _environmentOnline

                    Dim collection_ = enlaceDatos_.GetMongoCollection(Of OperacionGenerica)((New ConstructorFacturaComercial).GetType.Name)

                    Dim filter_ = Builders(Of OperacionGenerica).Filter.Eq(Function(x) x.Id, idDocumentoElectronico_)

                    Dim setData_ = Builders(Of OperacionGenerica).Update.
                                Set(Of String)("Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.NombrePropietario", datosFuente_.NombrePropietario).
                                Set(Of Integer)("Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.IdPropietario", datosFuente_.IdPropietario).
                                Set(Of ObjectId)("Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.ObjectIdPropietario", datosFuente_.ObjectdIdPropietario)

                    Dim result_ = collection_.UpdateOne(filter_, setData_)

                    If result_.MatchedCount <> 0 Then

                        .SetOK()

                    ElseIf result_.UpsertedId IsNot Nothing Then

                        .SetOK()

                    Else

                        .SetError(Me, "No se generaron cambios")

                    End If

                End Using

            Catch ex As Exception

                .SetOKBut(Me, $"Ha ocurrido un Error al actualizar documento electrónico {ex}")

            End Try

        End With

        Return estado_

    End Function


    ''ACTULIZAREMOS DOCUMENTOS ASOCIADOS DADO QUE EL CFDI NO LO HACE EN AUTOMATICO PORQUE NO NACE ASI
    ''HAY QUE PEDIRLE A CONTROLADOR DE CLIENTE QUE NOS MANDE LA FIRMA DEL CLIENTE :V
    Public Function ActualizarDocumentosAsociadosFacturaComercial(ByVal documentoAsociado_ As DocumentoAsociado,
                                                                  ByVal idDocumentoElectronico_ As ObjectId) As TagWatcher

        Estado = New TagWatcher

        With Estado

            Try

                Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos(21)

                    enlaceDatos_.EnvironmentOnline = _environmentOnline

                    Dim collection_ = enlaceDatos_.GetMongoCollection(Of OperacionGenerica)((New ConstructorFacturaComercial).GetType.Name)

                    ' 1. Definimos el filtro principal
                    Dim filter_ = Builders(Of OperacionGenerica).Filter.Eq(Function(x) x.Id, idDocumentoElectronico_)

                    ' 2. Paso A: Eliminamos cualquier versión anterior (Pull)
                    Dim pullFilter = Builders(Of OperacionGenerica).Update.PullFilter(
                            Function(x) x.Borrador.Folder.DocumentosAsociados,
                            Builders(Of DocumentoAsociado).Filter.Eq(Function(y) y.identificadorrecurso, documentoAsociado_.identificadorrecurso)
                        )

                    collection_.UpdateOne(filter_, pullFilter)

                    ' 3. Paso B: Agregamos el nuevo (Push)
                    Dim pushUpdate = Builders(Of OperacionGenerica).Update.Push(
                            Function(x) x.Borrador.Folder.DocumentosAsociados,
                            documentoAsociado_
                        )

                    ' 4. Ejecutamos
                    Dim result_ = collection_.UpdateOne(filter_, pushUpdate)

                    If result_.MatchedCount <> 0 Then

                        .SetOK()

                    ElseIf result_.UpsertedId IsNot Nothing Then

                        .SetOK()

                    Else

                        .SetError(Me, "No se generaron cambios")

                    End If

                End Using

            Catch ex As Exception

                .SetOKBut(Me, $"Ha ocurrido un Error al actualizar documento electrónico {ex}")

            End Try

        End With

        Return Estado

    End Function


    Public Function ObtenerFacturasComercialesSinVincularPedimento(listaObjectsIdFacturas_ As List(Of ObjectId)) As TagWatcher _
        Implements IControladorFacturaComercial.ObtenerFacturasComercialesSinVincularPedimento


        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If listaObjectsIdFacturas_ IsNot Nothing Then

                    If listaObjectsIdFacturas_.Count > 0 Then

                        Return ObtenerFacturasComercialesSinVincularAPedimentos(listaObjectsIdFacturas_)

                    Else

                        .SetOKBut(Me, "Lista de objectids de factura comercial vacía")

                    End If

                Else

                    .SetOKBut(Me, "Lista de objectids de factura comercial no recibida")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

    Public Function ObtenerFacturasComercialesPublicadasParaPedimento(listaObjectsIdFacturas_ As List(Of ObjectId)) As TagWatcher _
        Implements IControladorFacturaComercial.ObtenerFacturasComercialesPublicadasParaPedimento

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If listaObjectsIdFacturas_ IsNot Nothing Then

                    If listaObjectsIdFacturas_.Count > 0 Then

                        Return ObtenerFacturaComercialesPublicadasyFirmadas(listaObjectsIdFacturas_,
                                                                            esParaPedimento_:=True)

                    Else

                        .SetOKBut(Me, "Lista de objectids de factura comercial vacía")

                    End If

                Else

                    .SetOKBut(Me, "Lista de objectids de factura comercial no recibida")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

    Public Function ObtenerFacturasComercialesPublicadas(listaObjectsIdFacturas_ As List(Of ObjectId)) As TagWatcher _
        Implements IControladorFacturaComercial.ObtenerFacturasComercialesPublicadas

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If listaObjectsIdFacturas_ IsNot Nothing Then

                    If listaObjectsIdFacturas_.Count > 0 Then
                        ''NO ES PARA ESTE MÉTODO

                        Return ObtenerFacturaComercialesPublicadasyFirmadas(listaObjectsIdFacturas_)

                    Else

                        .SetOKBut(Me, "Lista de objectids de factura comercial vacía")

                    End If

                Else

                    .SetOKBut(Me, "Lista de objectids de factura comercial no recibida")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

    Public Function GenerarFacturaComercialDesdeComercialInvoiceAnalizer(comercialinvoiceAnalizer_ As CommercialInvoiceAnalysis,
                                                                         userGenero_ As String) As TagWatcher _
                                                                         Implements IControladorFacturaComercial.GenerarFacturaComercialDesdeComercialInvoiceAnalizer
        Throw New NotImplementedException()

    End Function

    Public Function GenerarFacturaComercialDesdeComercialInvoiceAnalizer(comercialinvoiceAnalizer_ As CommercialInvoiceAnalysis,
                                                                         userGenero_ As String, idCustomOperGenerica_ As ObjectId) As TagWatcher _
                                                                         Implements IControladorFacturaComercial.GenerarFacturaComercialDesdeComercialInvoiceAnalizer
        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If comercialinvoiceAnalizer_ IsNot Nothing Then

                    If Not String.IsNullOrWhiteSpace(userGenero_) Then

                        If Not idCustomOperGenerica_ = ObjectId.Empty Then

                            Return GuardarPreasignacionFacturaCommercialIA(comercialinvoiceAnalizer_, userGenero_, idCustomOperGenerica_)

                        Else
                            .SetError(Me, "idCustomOperGenerica es requerido")

                        End If

                    Else

                        .SetError(Me, "userGenero_ es requerido")

                    End If

                Else

                    .SetError(Me, "comercialinvoiceAnalizer_ es requerida")

                End If

            Else

                .SetError(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

    Public Function GenerarFacturaComercialSubdividible(constructorFacturaComercial_ As ConstructorFacturaComercial,
                                                        objectidPreasignacionFacturaOriginal_ As ObjectId) As TagWatcher _
                                                        Implements IControladorFacturaComercial.GenerarFacturaComercialSubdividible
        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If TipoOperacion = IControladorFacturaComercial.TipoOperaciones.Importacion Then

                    If constructorFacturaComercial_ IsNot Nothing AndAlso Not objectidPreasignacionFacturaOriginal_ = ObjectId.Empty Then

                        Return GenerarFacturaSubdividible(constructorFacturaComercial_, objectidPreasignacionFacturaOriginal_)

                    Else

                        .SetError(Me, "Todos los argumentos son requeridos")

                    End If

                Else

                    .SetError(Me, "Tipo de operacion debe ser importación")

                End If

            Else

                .SetError(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

    Public Function GenerarFacturaComercialDesdeSubdivision(comercialinvoiceSubdividida_ As SubdivisionFacturaComercial,
                                                            idOperGenericaOriginal_ As ObjectId, idCustomOperGenerica_ As ObjectId, userGenero_ As String) As TagWatcher _
                                                            Implements IControladorFacturaComercial.GenerarFacturaComercialDesdeSubdivision

        _Estado = New TagWatcher

        With _Estado

            If _entorno <> 0 Then

                If comercialinvoiceSubdividida_ IsNot Nothing Then

                    If Not String.IsNullOrWhiteSpace(userGenero_) Then

                        If Not idCustomOperGenerica_ = ObjectId.Empty Then

                            If Not idOperGenericaOriginal_ = ObjectId.Empty Then

                                Return GuardarPreasignacionFacturaSubdividida(comercialinvoiceSubdividida_,
                                                                              idOperGenericaOriginal_, idCustomOperGenerica_, userGenero_)
                            Else

                                .SetError(Me, "idOperGenericaOriginal_ es requerido")

                            End If

                        Else
                            .SetError(Me, "idCustomOperGenerica es requerido")

                        End If

                    Else

                        .SetError(Me, "userGenero_ es requerido")

                    End If

                Else

                    .SetError(Me, "comercialinvoiceSubdividida_ es requerida")

                End If

            Else

                .SetError(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function


#End Region

#End Region
End Class


Public Class DatosFuente

    Property NombrePropietario As String

    Property IdPropietario As Integer

    Property ObjectdIdPropietario As ObjectId

End Class

Public Class TextoSeparado
    <BsonIgnoreIfNull>
    Property id_ As String
    <BsonIgnoreIfNull>
    Property clave_ As String
    <BsonIgnoreIfNull>
    Property description_ As String
End Class
Public Class Incrementables
    Property idFactura As ObjectId
    Property fletes As Double
    Property seguros As Double
    Property embalajes As Double
    Property otros As Double
End Class
Public Class ResultadoMarcaje
    ' Definición de la clase de salida esperada (solo _id y SePuedeMarcar)
    Public Property _id As ObjectId
    Public Property sePuedeMarcar As Boolean
End Class
Public Class DatosAcuseValor
    <BsonIgnoreIfNull>
    Public Property _idFactura As ObjectId

    <BsonIgnoreIfNull>
    Public Property _numeroFactura As String

    <BsonIgnoreIfNull>
    Public Property _idAcuseValor As ObjectId

    <BsonIgnoreIfNull>
    Public Property _acuseValor As String

    <BsonIgnoreIfNull>
    Public Property _fechaAcuseValor As Date

    <BsonIgnoreIfNull>
    Public Property _tipoOperacion As String

    <BsonIgnoreIfNull>
    Public Property _facturaSubdividida As Boolean

    <BsonIgnoreIfNull>
    Public Property _facturaenajenada As Boolean

End Class

Public Class FacturaAsociada
    <BsonIgnoreIfNull>
    Public Property _idFactura As ObjectId
    <BsonIgnoreIfNull>
    Public Property _folioFactura As String
    <BsonIgnoreIfNull>
    Public Property _idPedimentoAsociado As ObjectId
    <BsonIgnoreIfNull>
    Public Property _esFacturaAsociada As Boolean
    <BsonIgnoreIfNull>
    Public Property _tipoOperacion As String
    <BsonIgnoreIfNull>
    Public Property _aplicaSubdivision As Boolean
End Class


Public Class ResponseOperacion
    Public Property id As ObjectId
    Public Property OperacionGenerica As OperacionGenerica
    Public Property CommercialInvoice As ICommercialInvoiceGeneric
End Class

Public Class EstructuraCliente
    Inherits Customer
    Public Property cve_cliente As String
    Public Property id_domicilio As ObjectId
    Public Property sec_domicilio As String
    Public Property colonia As String
    Public Property municipio As String
    Public Property cveEntidadFederativa As String
    Public Property entidadFederativa As String
    Public Property cvePais As String
    Public Property firmaElectronicaCliente As String
End Class

Public Class CommercialInvoiceCFDI
    Inherits CommercialInvoiceGeneric

    <BsonIgnoreIfNull>
    Public Property itemscfdi_ As List(Of ItemCfdi)


End Class

Public Class ItemCfdi
    Inherits Digitalization.Item

    <BsonIgnoreIfNull>
    Public Property Cantidad As Double

    <BsonIgnoreIfNull>
    Public Property ClaveUnidad As String

    <BsonIgnoreIfNull>
    Public Property Descripcion As String

    <BsonIgnoreIfNull>
    Public Property Importe As Double

    <BsonIgnoreIfNull>
    Public Property NoIdentificacion As String

    <BsonIgnoreIfNull>
    Public Property Unidad As String

    <BsonIgnoreIfNull>
    Public Property ValorUnitario As Double

    <BsonIgnoreIfNull>
    Public Property CantidadAduana As Double

    <BsonIgnoreIfNull>
    Public Property FraccionArancelaria As String

    <BsonIgnoreIfNull>
    Public Property Nico As String

    <BsonIgnoreIfNull>
    Public Property UnidadAduana As String

    <BsonIgnoreIfNull>
    Public Property ValorDolares As Double

    <BsonIgnoreIfNull>
    Public Property ValorUnitarioAduana As Double

End Class

Public Class Response
    Implements IDisposable

#Region "Enum"

    Enum CodeStatus

        SinDefinir = 0

        RecursoDisponible = 1

        RecursoAsociado = 2

        RecursoNoDisponible = 3

        ErrorAsociacionRecurso = 4

        ObjectIdNoValidoRecurso = 5

        ErrorInterno = 6

    End Enum

#End Region

    Private disposedValue As Boolean

    Public Property Clave As CodeStatus

    Public Property Situacion As String

    Public Property Mensaje As String

    Public Property RecursoAdicional As FacturaAsociada

    ' Constructores
    Public Sub New(clave As Integer,
                   situacion As String,
                   mensaje As String)

        Me.Clave = clave

        Me.Situacion = situacion

        Me.Mensaje = mensaje

    End Sub

    Public Sub New(clave As Integer,
                   situacion As String,
                   mensaje As String,
                   recursoAdicional As FacturaAsociada)

        Me.Clave = clave

        Me.Situacion = situacion

        Me.Mensaje = mensaje

        Me.RecursoAdicional = recursoAdicional

    End Sub

    Protected Overridable Sub Dispose(disposing As Boolean)

        If Not disposedValue Then

            If disposing Then

                Clave = Nothing

                Situacion = Nothing

                Mensaje = Nothing

                RecursoAdicional = Nothing

            End If

            ' TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
            ' TODO: establecer los campos grandes como NULL
            disposedValue = True

        End If

    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' No cambie este código. Coloque el código de limpieza en el método "Dispose(disposing As Boolean)".
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub

End Class


Public Class RecursoResponse

    Public Shared Function SinDefinir() As Response
        Return New Response(Response.CodeStatus.SinDefinir, "SIN_DEFINICION", Nothing)
    End Function

    ' ─── 2xx Éxito ───────────────────────────────
    Public Shared Function Ok(Optional mensaje As String = Nothing) As Response
        Return New Response(Response.CodeStatus.RecursoDisponible, "RECURSO_DISPONIBLE", mensaje)
    End Function

    Public Shared Function Ok(mensaje As String, recursoAdicional As Object) As Response
        Return New Response(Response.CodeStatus.RecursoDisponible, "RECURSO_DISPONIBLE", mensaje, recursoAdicional)
    End Function

    '' ─── 4xx Error del cliente ───────────────────
    Public Shared Function ObjectidNoValido(mensaje As String, Optional recursoAdicional As Object = Nothing) As Response
        Return New Response(Response.CodeStatus.ObjectIdNoValidoRecurso, "OBJECTID_NO_VALIDO", mensaje, recursoAdicional)
    End Function

    Public Shared Function NoDisponible(mensaje As String, Optional recursoAdicional As Object = Nothing) As Response
        Return New Response(Response.CodeStatus.RecursoNoDisponible, "RECURSO_NO_DISPONIBLE", mensaje, recursoAdicional)
    End Function

    Public Shared Function ErrorAsociacion(mensaje As String, Optional recursoAdicional As Object = Nothing) As Response
        Return New Response(Response.CodeStatus.ErrorAsociacionRecurso, "ERROR_ASOCIACION_RECURSO", mensaje, recursoAdicional)
    End Function

    '' ─── 5xx Error del servidor ──────────────────
    Public Shared Function ErrorInterno(mensaje As String) As Response
        Return New Response(Response.CodeStatus.ErrorInterno, "ERROR_INTERNO", mensaje)
    End Function

End Class

