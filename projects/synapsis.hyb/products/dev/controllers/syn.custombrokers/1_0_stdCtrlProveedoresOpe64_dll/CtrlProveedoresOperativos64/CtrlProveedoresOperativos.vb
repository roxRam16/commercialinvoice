Imports gsol
Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization
Imports MongoDB.Bson.Serialization.Attributes
Imports MongoDB.Driver
Imports Rec.Globals.Empresas
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Utils
Imports Wma.Exceptions

Public Class CtrlProveedoresOperativos
    Implements IDisposable, ICloneable

#Region "Enums"
    Public Enum TiposProveedores

        SinDefinir = 0

        Nacionales = 1

        Extranjeros = 2

    End Enum

    Public Enum TipoOperacion

        SinDefinir = 0

        Importacion = 1

        Exportacion = 2

    End Enum

    ''ESTO NO, UTILICEMOS SOBRECARGAS
    Public Enum TipoSelectOption

        IdRazonsocial = 1

        IdIdentificador = 2

        CveRazonsocial = 3

        CveIdentificador = 4

    End Enum

    Private Enum TipoUso

        Proveedor = 1

        Destinatario = 2

    End Enum

#End Region

#Region "Propiedades privadas"

    Private _espacioTrabajo As IEspacioTrabajo

    Private disposedValue As Boolean

    Private _listaProveedores As List(Of AuxiliarProveedor)

    Private _listaDestinatarios As List(Of AuxiliarDestinatario)

#End Region

#Region "Propiedades"

    Property _TipoOperacion As TipoOperacion

    Property _TipoProveedor As TiposProveedores

    Property _Proveedor As ConstructorProveedoresOperativos

    Property _ProveedorAuxiliar As AuxiliarProveedor

    Property _DestinatarioAuxiliar As AuxiliarDestinatario

    Property _Destinatario As ConstructorDestinatario

    Property _Estado As TagWatcher

#End Region

#Region "Constructores"
    Sub New()

        _espacioTrabajo = New EspacioTrabajo

        _Estado = New TagWatcher


    End Sub

    Sub New(ByVal tipoProveedor_ As TiposProveedores,
            Optional ByVal tipoOperacion_ As TipoOperacion = TipoOperacion.Importacion,
            Optional ByVal espacioTrabajo_ As IEspacioTrabajo = Nothing)

        Inicializa(tipoProveedor_, tipoOperacion_, espacioTrabajo_)

    End Sub

    Private Sub Inicializa(tipoProveedor_ As TiposProveedores,
                           tipoOperacion_ As TipoOperacion, espacioTrabajo_ As IEspacioTrabajo)

        _espacioTrabajo = espacioTrabajo_

        _TipoOperacion = tipoOperacion_

        _TipoProveedor = tipoProveedor_

        _Estado = New TagWatcher


    End Sub

#End Region

#Region "Funciones obsoletas"
    <Obsolete("Esta función está obsoleta.", False)>
    Public Function ListarProveedores() As List(Of ConstructorProveedoresOperativos)
        'No funcional hasta que se utilice
        Dim provedororOperativo_ As New ConstructorProveedoresOperativos()

        Dim listaProveedores_ As New List(Of ConstructorProveedoresOperativos)

        Select Case _TipoOperacion

            Case TipoOperacion.Exportacion

                Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos(27) With {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

                    Using _entidadDatos As IEntidadDatos = provedororOperativo_

                        Dim documentoElectronico_ As DocumentoElectronico = _entidadDatos

                        Return listaProveedores_

                    End Using

                End Using

            Case TipoOperacion.Importacion

        End Select

        Return listaProveedores_

    End Function

    <Obsolete("Esta función está obsoleta.", False)>
    Public Function BuscarProveedores(ByVal razonSocial_ As String,
                                      Optional ByVal esDestinatario_ As Boolean = True) As Object
        'Solo esta funcional en tipo exportación
        Dim provedororOperativo_ As New ConstructorProveedoresOperativos()

        Dim tagwacher_ As New TagWatcher

        Select Case _TipoOperacion

            Case TipoOperacion.Exportacion

                Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos(27) With
                    {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

                    Using _entidadDatos As IEntidadDatos = provedororOperativo_

                        Dim documentoElectronico_ As DocumentoElectronico = _entidadDatos

                        tagwacher_ = _enlaceDatos.BusquedaGeneralDocumento(documentoElectronico_,
                                                                            1,
                                                                            5003,
                                                                            razonSocial_)

                        Return tagwacher_.ObjectReturned

                    End Using

                End Using

            Case TipoOperacion.Importacion

        End Select

        Return tagwacher_

    End Function

    <Obsolete("Esta función está obsoleta.", False)>
    Public Function BuscarProveedor(Optional ByVal objectId_ As ObjectId = Nothing,
                                    Optional ByVal listaObjectId_ As List(Of ObjectId) = Nothing) As Object

        'Solo funciona en exportación y cuando traes un Object Id
        Dim provedororOperativo_ As New ConstructorProveedoresOperativos()

        Dim tipo_ As String = GetType(ConstructorProveedoresOperativos).Name

        Select Case _TipoOperacion

            Case TipoOperacion.Exportacion

                If objectId_ <> Nothing Then

                    Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos(27) With
                        {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

                        Dim operacionesDB_ = enlaceDatos_.GetMongoCollection(Of OperacionGenerica)(tipo_)

                        Dim resultadoDocumentos_ As New List(Of OperacionGenerica)

                        Dim filtro_ = Builders(Of OperacionGenerica).Filter.Eq(Function(x) x.Id, objectId_)

                        resultadoDocumentos_ = operacionesDB_.Find(filtro_).Limit(1).ToList

                        If resultadoDocumentos_.Count Then

                            Dim operacionGenerica_ As OperacionGenerica = resultadoDocumentos_(0)

                            Return New TagWatcher(1) With {.ObjectReturned = operacionGenerica_}

                        Else

                            Return New TagWatcher(0, Me, "No se encontró ningún valor para esta consulta")

                        End If

                    End Using

                Else


                End If


            Case TipoOperacion.Importacion

        End Select

        Return New TagWatcher(0, Me, "Sin resultados")

    End Function

    <Obsolete("Esta función está obsoleta.", False)>
    Public Function BuscarDomicilios(ByVal proveedores_ As DocumentoElectronico) As Object

        'Regresa una lista de select option se debe ajustar a que se devuelva una lista de domicilios
        'usando la clase de empresa-domicilios y que se use linq para la lectura.
        Dim listaDomicilios_ As New List(Of SelectOption)

        If proveedores_ IsNot Nothing Then

            If proveedores_.Seccion(SeccionesProvedorOperativo.SPRO2).CantidadPartidas > 0 Then

                For indice_ As Int32 = 1 To proveedores_.Seccion(SeccionesProvedorOperativo.SPRO2).CantidadPartidas

                    With proveedores_.Seccion(SeccionesProvedorOperativo.SPRO2).Partida(indice_)

                        If .estado = 1 Then

                            Dim domicilio_ = .Attribute(CamposProveedorOperativo.CA_RFC_PROVEEDOR).Valor & " | " &
                                .Attribute(CamposDomicilio.CA_CALLE).Valor & " #" &
                                .Attribute(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor & " C.P. " &
                                .Attribute(CamposDomicilio.CA_CALLE).Valor & " COL." &
                                .Attribute(CamposDomicilio.CA_COLONIA).Valor & ", " &
                                .Attribute(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor & ", " &
                                .Attribute(CamposDomicilio.CA_PAIS).Valor

                            Dim id_ = .Attribute(CamposProveedorOperativo.CA_RFC_PROVEEDOR).Valor

                            Dim dataSourceItem_ = New SelectOption With {
                                .Text = domicilio_,
                                .Value = id_
                            }

                            listaDomicilios_.Add(dataSourceItem_)

                        End If

                    End With

                Next

                Return listaDomicilios_

            End If

        End If

        Return listaDomicilios_

    End Function

    <Obsolete("Esta función está obsoleta.", False)>
    Public Function BuscarVinculaciones(ByVal proveedores_ As DocumentoElectronico,
                                        ByVal idProveedor_ As String, ByVal idCliente_ As ObjectId) As Object

        'Solo esta para exportacion, se debe ajustar a que mande una lista de vinculaciones o un objeto diferente
        'al select option
        Dim listaVinculacion_ As New List(Of SelectOption)

        Select Case _TipoOperacion

            Case TipoOperacion.Exportacion

                If proveedores_ IsNot Nothing Then

                    If proveedores_.Seccion(SeccionesProvedorOperativo.SPRO4).CantidadPartidas > 0 Then

                        For indice_ As Int32 = 1 To proveedores_.Seccion(SeccionesProvedorOperativo.SPRO4).CantidadPartidas

                            With proveedores_.Seccion(SeccionesProvedorOperativo.SPRO4).Partida(indice_)

                                If .estado = 1 And .Attribute(CamposProveedorOperativo.CP_ID_CLIENTE_VINCULACION).Valor =
                                    idCliente_ And .Attribute(CamposProveedorOperativo.CP_RFC_PROVEEDOR_VINCULACION).ValorPresentacion =
                                    idProveedor_ Then

                                    Dim vinculacion_ = .Attribute(CamposProveedorOperativo.CA_CVE_VINCULACION).ValorPresentacion

                                    Dim cve_ = .Attribute(CamposProveedorOperativo.CA_CVE_VINCULACION).Valor

                                    Dim dataSourceItem_ = New SelectOption With {
                                        .Text = vinculacion_,
                                        .Value = cve_
                                    }

                                    listaVinculacion_.Add(dataSourceItem_)

                                End If

                            End With

                        Next

                        Return listaVinculacion_

                    End If

                End If

            Case TipoOperacion.Importacion

        End Select

        Return listaVinculacion_

    End Function

    <Obsolete("Esta función está obsoleta.", False)>
    Public Function BuscarConfiguraciones(ByVal proveedores_ As DocumentoElectronico,
                                          ByVal idProveedor_ As String, ByVal idCliente_ As ObjectId) As Object
        'Solo esta para exportacion, se debe ajustar a que mande una lista de configuraciones o un objeto
        'diferente al select option
        Dim listaConfiguracion_ As New List(Of SelectOption)

        Select Case _TipoOperacion

            Case TipoOperacion.Exportacion

                If proveedores_ IsNot Nothing Then

                    If proveedores_.Seccion(SeccionesProvedorOperativo.SPRO5).CantidadPartidas > 0 Then

                        For indice_ As Int32 = 1 To proveedores_.Seccion(SeccionesProvedorOperativo.SPRO5).CantidadPartidas

                            With proveedores_.Seccion(SeccionesProvedorOperativo.SPRO5).Partida(indice_)

                                If .estado = 1 And .Attribute(CamposProveedorOperativo.CP_ID_CLIENTE_CONFIGURACION).Valor =
                                    idCliente_ And .Attribute(CamposProveedorOperativo.CP_RFC_PROVEEDOR_CONFIGURACION).ValorPresentacion =
                                    idProveedor_ Then

                                    Dim metodoValoracion_ = .Attribute(CamposProveedorOperativo.CA_CVE_METODO_VALORACION).ValorPresentacion

                                    Dim cve_ = .Attribute(CamposProveedorOperativo.CA_CVE_METODO_VALORACION).Valor

                                    Dim dataSourceItem_ = New SelectOption With {
                                        .Text = metodoValoracion_,
                                        .Value = cve_
                                    }

                                    listaConfiguracion_.Add(dataSourceItem_)

                                End If

                            End With

                        Next

                        Return listaConfiguracion_

                    End If

                End If

            Case TipoOperacion.Importacion

        End Select

        Return listaConfiguracion_

    End Function

    <Obsolete("Esta función está obsoleta.", False)>
    Public Function ToSelectOption(Optional ByVal listaProveedores_ As Object = Nothing,
                                   Optional ByVal proveedorDocumento_ As DocumentoElectronico = Nothing,
                                   Optional ByVal tipoSelect_ As TipoSelectOption = TipoSelectOption.IdRazonsocial) As List(Of SelectOption)
        'Completar las funciones y considerar que puede ser adaptado en el controlador backend
        Dim listaProveedoresComponente_ As New List(Of SelectOption)

        If listaProveedores_ IsNot Nothing Then

            If listaProveedores_.Count() > 0 Then

                For Each item_ As Dictionary(Of Object, Object) In listaProveedores_

                    Select Case tipoSelect_

                        Case TipoSelectOption.IdRazonsocial

                            listaProveedoresComponente_.Add(New SelectOption With {.Value = item_.Item("ID").ToString, .Text = item_.Item("valorOperacion") & " - " & item_.Item("folioOperacion")})

                        Case TipoSelectOption.IdIdentificador

                        Case TipoSelectOption.CveRazonsocial

                        Case TipoSelectOption.CveIdentificador

                    End Select

                Next

            End If

        ElseIf proveedorDocumento_ IsNot Nothing Then


        End If

        Return listaProveedoresComponente_

    End Function

    <Obsolete("Esta función está obsoleta.", False)>
    Public Function BuscarProveedores1(ByVal token_ As String, btipooperacion_ As Boolean) As List(Of SelectOption)
        Dim listaProveedores_ As New List(Of SelectOption)
        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos(27) With
                   {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

            Dim CtrlRecursosGenerales_ As New Organismo
            Dim operationsDB_ As IMongoCollection(Of OperacionGenerica) = _enlaceDatos.GetMongoCollection(Of OperacionGenerica)(GetType(ConstructorProveedoresOperativos).Name)

            Dim itipooperacion_ As Integer
            If btipooperacion_ Then
                itipooperacion_ = 1
            Else
                itipooperacion_ = 2
            End If

            operationsDB_.Aggregate().Project(Function(ch) New With {
                                          Key .IDS = ch.Id,
                                          Key .razonsocial = ch.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.NombreCliente,
                                          Key .foliodocumentos = ch.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.FolioDocumento,
                                          Key .tipouso = DirectCast(ch.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Encabezado")(0).Nodos(0).Nodos(4).Nodos(0), Campo).Valor
              }).Match(BsonDocument.Parse(CtrlRecursosGenerales_.SeparacionPalabras(token_, "razonsocial", "tipouso", itipooperacion_.ToString, ""))).
                ToList().AsEnumerable.ToList().ForEach(Sub(estatus_)
                                                           listaProveedores_.Add(New SelectOption With {
                                                                          .Value = estatus_.IDS.ToString,
                                                                          .Text = estatus_.razonsocial & " | " & estatus_.foliodocumentos
                                                           })
                                                       End Sub)

        End Using
        Return listaProveedores_


    End Function

    <Obsolete("Esta función está obsoleta.", False)>
    Public Function BuscarProveedor(ByVal token_ As String, ByRef stipoidentificador_ As String) As ConstructorProveedoresOperativos
        Dim ConstructorProveedoresOperativos_ As New ConstructorProveedoresOperativos

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos(27) With
               {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}
            Dim operationsDB_ As IMongoCollection(Of OperacionGenerica)
            operationsDB_ = _enlaceDatos.GetMongoCollection(Of OperacionGenerica)(GetType(ConstructorProveedoresOperativos).Name)
            If stipoidentificador_ = "ID" Then
                operationsDB_.Aggregate().Project(Function(ch) New With {
                                      Key .IDS = ch.Id,
                                      Key .DocumentoProveedor = ch.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente
                                }).Match(Function(e) e.IDS = New ObjectId(token_)).
                                 ToList().ForEach(Sub(estatus_)
                                                      ConstructorProveedoresOperativos_ = New ConstructorProveedoresOperativos(True, estatus_.DocumentoProveedor) _
                                                                                              With {.Id = estatus_.IDS.ToString}
                                                  End Sub)
            Else

                If stipoidentificador_ = "TAXID" Then
                    operationsDB_.Aggregate().Project(Function(ch) New With {
                                      Key .IDS = ch.Id,
                                      Key .DocumentoProveedor = ch.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente,
                                      Key .TaxId = DirectCast(ch.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Cuerpo")(0).Nodos(0).Nodos(0).Nodos(2).Nodos(0), Campo).Valor
          }).Match(Function(e) e.TaxId.Equals(token_)).
            ToList().ForEach(Sub(estatus_)
                                 ConstructorProveedoresOperativos_ = New ConstructorProveedoresOperativos(True, estatus_.DocumentoProveedor) _
                                                                                              With {.Id = estatus_.IDS.ToString}
                             End Sub)
                Else
                    If stipoidentificador_ = "RFC" Then

                        operationsDB_.Aggregate().Project(Function(ch) New With {
                                          Key .IDS = ch.Id,
                                          Key .DocumentoProveedor = ch.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente,
                                          Key .RFC = DirectCast(ch.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Cuerpo")(0).Nodos(0).Nodos(0).Nodos(1).Nodos(0), Campo).Valor
                                          }).Match(Function(e) e.RFC.Equals(token_)).
                ToList().ForEach(Sub(estatus_)
                                     ConstructorProveedoresOperativos_ = New ConstructorProveedoresOperativos(True, estatus_.DocumentoProveedor) _
                                                                                              With {.Id = estatus_.IDS.ToString}
                                 End Sub)
                    Else

                        operationsDB_.Aggregate().Project(Function(ch) New With {
                                          Key .IDS = ch.Id,
                                          Key .DocumentoProveedor = ch.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente,
                                          Key .RazonSocial = ch.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.NombreCliente
              }).Match(Function(e) e.RazonSocial.Equals(token_)).
                ToList().ForEach(Sub(estatus_)
                                     ConstructorProveedoresOperativos_ = New ConstructorProveedoresOperativos(True, estatus_.DocumentoProveedor) _
                                                                                              With {.Id = estatus_.IDS.ToString}
                                 End Sub)
                    End If

                End If

            End If

        End Using
        Return ConstructorProveedoresOperativos_
    End Function

    <Obsolete("Esta función está obsoleta.", False)>
    Private Function BuscarProveedorConDomicilio(ByVal razonsocial_ As String,
                                                 ByVal pais_ As String,
                                                 Optional ByVal calle_ As String = Nothing,
                                                 Optional ByVal codigopostal_ As String = Nothing) As TagWatcher
        With _Estado

            Try

                Using iEnlace_ As IEnlaceDatos = New EnlaceDatos(27) With {.EspacioTrabajo = _espacioTrabajo}

                    Dim collection_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)((New ConstructorProveedoresOperativos).GetType.Name)

                    Dim tipoOperacionString_ = IIf(_TipoOperacion = TipoOperacion.Importacion, "Importación", "Exportación")

                    Dim tipoProveedor_ = IIf(_TipoProveedor = TiposProveedores.Extranjeros, "2", "1")

                    Dim pipeline_ = collection_.Aggregate().
                                                Project(Function(x) _
                                                    New With {
                                                    Key .fuente_ = x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente,
                                                    Key .seccionEncabezado_ = x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Encabezado")(0).Nodos(0)}).
                                                    Project(Function(y) _
                                                    New With {
                                                    Key .documento_ = y.fuente_,
                                                    Key .tipoproveedor_ = DirectCast(y.seccionEncabezado_.Nodos(3).Nodos(0), Campo).Valor,
                                                    Key .razonsocial_ = DirectCast(y.seccionEncabezado_.Nodos(4).Nodos(0), Campo).Valor,
                                                    Key .tipouso_ = DirectCast(y.seccionEncabezado_.Nodos(5).Nodos(0), Campo).ValorPresentacion}).
                                                Match(Function(z) z.razonsocial_.Equals(razonsocial_) And
                                                z.tipoproveedor_.Equals(tipoProveedor_) And
                                                z.tipouso_.Equals(tipoOperacionString_)).Limit(1).ToList

                    _ProveedorAuxiliar = New AuxiliarProveedor

                    _Proveedor = New ConstructorProveedoresOperativos

                    If pipeline_.Any() Then

                        If pipeline_.Count > 0 Then

                            pipeline_.AsEnumerable.ToList.ForEach(Sub(x)
                                                                      _Proveedor = New ConstructorProveedoresOperativos(True, x.documento_)
                                                                  End Sub)

                            If _Proveedor.Seccion(SeccionesProvedorOperativo.SPRO2).CantidadPartidas > 0 Then

                                For indice_ As Int32 = 1 To _Proveedor.Seccion(SeccionesProvedorOperativo.SPRO2).CantidadPartidas

                                    With _Proveedor.Seccion(SeccionesProvedorOperativo.SPRO2).Partida(indice_)

                                        If pais_ = .Campo(CamposDomicilio.CA_CVE_PAIS).Valor Then

                                            If calle_ = .Campo(CamposDomicilio.CA_CALLE).Valor And
                                            codigopostal_ = .Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor Then

                                                Dim domicilioAux_ As New Rec.Globals.Empresas.Domicilio

                                                domicilioAux_._iddomicilio = New ObjectId(.Campo(CamposProveedorOperativo.CP_ID_DOMICILIO_PROVEEDOR).Valor.ToString)

                                                domicilioAux_.domicilioPresentacion = .Campo(CamposProveedorOperativo.CA_DOMICILIO_FISCAL).Valor

                                                domicilioAux_.calle = .Campo(CamposDomicilio.CA_CALLE).Valor

                                                domicilioAux_.numeroexterior = .Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor

                                                domicilioAux_.numerointerior = .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor

                                                domicilioAux_.codigopostal = .Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor

                                                domicilioAux_.colonia = .Campo(CamposDomicilio.CA_COLONIA).Valor

                                                domicilioAux_.localidad = .Campo(CamposDomicilio.CA_LOCALIDAD).Valor

                                                domicilioAux_.ciudad = .Campo(CamposDomicilio.CA_CIUDAD).Valor

                                                domicilioAux_.municipio = .Campo(CamposDomicilio.CA_MUNICIPIO).Valor

                                                domicilioAux_.cveMunicipio = .Campo(CamposDomicilio.CA_ENTIDAD_MUNICIPIO).Valor

                                                domicilioAux_.entidadfederativa = .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor

                                                _ProveedorAuxiliar.id = _Proveedor.Seccion(SeccionesProvedorOperativo.SPRO1).Campo(CamposProveedorOperativo.CP_ID_EMPRESA).Valor

                                                _ProveedorAuxiliar._clave = _Proveedor.Seccion(SeccionesProvedorOperativo.SPRO1).Campo(CamposProveedorOperativo.CP_CVE_PROVEEDOR).Valor

                                                _ProveedorAuxiliar._razonsocial = _Proveedor.Seccion(SeccionesProvedorOperativo.SPRO1).Campo(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR).Valor

                                                _ProveedorAuxiliar._rfc = .Campo(CamposProveedorOperativo.CA_RFC_PROVEEDOR).Valor

                                                _ProveedorAuxiliar._taxid = .Campo(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR).Valor

                                                _ProveedorAuxiliar._curp = .Campo(CamposProveedorOperativo.CA_CURP_PROVEEDOR).Valor

                                                _ProveedorAuxiliar._domicilio = domicilioAux_

                                                _ProveedorAuxiliar._cvepais = .Campo(CamposDomicilio.CA_CVE_PAIS).Valor

                                                _ProveedorAuxiliar._pais = .Campo(CamposDomicilio.CA_PAIS).Valor

                                                _ProveedorAuxiliar._esdestinatario = .Campo(CamposProveedorOperativo.CP_DESTINATARIO_PROVEEDOR).Valor

                                                '_ProveedorAuxiliar._vinculacion = .Attribute(CamposProveedorOperativo.CP_VINCULACION).Valor
                                                '_ProveedorAuxiliar._cvemetodovaloracion = .Attribute(CamposProveedorOperativo.CP_CVE_EMPRESA).Valor
                                                '_ProveedorAuxiliar._aplicacertificado = .Attribute(CamposProveedorOperativo.CP_CVE_EMPRESA).Valor
                                                '_ProveedorAuxiliar._razonsocialcertificado = .Attribute(CamposProveedorOperativo.CP_CVE_EMPRESA).Valor

                                            Else

                                                _ProveedorAuxiliar = Nothing

                                            End If

                                        End If

                                    End With

                                Next

                            End If

                            If _ProveedorAuxiliar IsNot Nothing Then

                                If _ProveedorAuxiliar._domicilio.domicilioPresentacion IsNot Nothing Then

                                    .ObjectReturned = _ProveedorAuxiliar

                                    .SetOK()

                                Else

                                    .SetOKBut(Me, "No se encontró proveedor")

                                End If

                            Else

                                .SetOKBut(Me, "No se encontró proveedor")

                            End If

                        Else

                            .SetOKBut(Me, "No se encontró proveedor")

                        End If

                    End If

                End Using

            Catch ex As Exception

                .SetOKBut(Me, "Ha ocurrido un error")

            End Try

        End With

        Return _Estado

    End Function

#End Region

#Region "Métodos privados"
    Private Function ObtenerProveedoresRazonSocial(ByVal razonsocial_ As String,
                                                   ByVal pagenumber_ As Int16, ByVal limitedatos_ As Int16) As TagWatcher

        With _Estado

            Try
                Using iEnlace_ As IEnlaceDatos = New EnlaceDatos(27) With {.EspacioTrabajo = _espacioTrabajo}

                    Dim collection_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)((New ConstructorProveedoresOperativos).GetType.Name)

                    Dim filter_ = Builders(Of OperacionGenerica).Filter.Text(razonsocial_)

                    Dim pipeline_ = collection_.Aggregate().
                                                Match(filter_).
                                                Project(Function(x) _
                                                        New With {
                                                        Key .id_ = x.Id,
                                                        Key .publicado_ = x.Publicado,
                                                        Key .razonsocial_ = x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.NombrePropietario,
                                                        Key .activo = x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Encabezado")(0).Nodos(0).Nodos(12)}).
                                                        Project(Function(y) _
                                                               New With {
                                                               Key y.id_,
                                                               Key y.publicado_,
                                                               Key y.razonsocial_,
                                                               Key .activo_ = DirectCast(y.activo.Nodos(0), Campo).Valor,
                                                               Key .activoPresentacion = DirectCast(y.activo.Nodos(0), Campo).ValorPresentacion}).
                                                 Skip((pagenumber_ - 1) * limitedatos_).
                                                 Limit(10).ToList

                    If pipeline_.Any() Then

                        _listaProveedores = New List(Of AuxiliarProveedor)

                        If pipeline_.Count > 0 Then

                            pipeline_.AsEnumerable.ToList.ForEach(Sub(x)
                                                                      Dim auxProveedor As New AuxiliarProveedor

                                                                      auxProveedor.id = x.id_.ToString

                                                                      auxProveedor._razonsocial = x.razonsocial_

                                                                      _listaProveedores.Add(auxProveedor)

                                                                  End Sub)
                        End If

                        If _listaProveedores.Count > 0 Then

                            .ObjectReturned = _listaProveedores

                            .SetOK()

                        Else

                            .SetOKBut(Me, "No se encontró proveedor")

                        End If

                    Else

                        .SetOKBut(Me, "No se encontró proveedor")

                    End If

                End Using

            Catch ex As Exception

                .SetError($"Ha ocurrido un error {ex}")

            End Try

        End With

        Return _Estado

    End Function

    Private Function ObtenerProveedoresRazonSocialPais(ByVal razonsocialPais_ As String,
                                                   ByVal pagenumber_ As Int16, ByVal limitedatos_ As Int16) As TagWatcher

        With _Estado

            Try
                Using iEnlace_ As IEnlaceDatos = New EnlaceDatos(27) With {.EspacioTrabajo = _espacioTrabajo}

                    Dim collection_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)((New ConstructorProveedoresOperativos).GetType.Name)

                    Dim razon_ = (If(razonsocialPais_, "")).Trim()

                    Dim paisFiltros_ = SeparaPais(razon_.Split({" "c}, StringSplitOptions.RemoveEmptyEntries))

                    Dim filter_ = Builders(Of OperacionGenerica).Filter.Text(razonsocialPais_)

                    Dim docs_ = collection_.Find(filter_).ToList()

                    Dim auxiliarProveedores_ As List(Of AuxiliarProveedor) = PreparaAuxiliar(TipoUso.Proveedor, paisFiltros_, docs_).ObjectReturned

                    If auxiliarProveedores_.Count > 0 Then

                        .ObjectReturned = auxiliarProveedores_

                        .SetOK()

                    Else

                        .SetOKBut(Me, "No se encontró proveedor")

                    End If

                End Using

            Catch ex As Exception

                .SetError($"Ha ocurrido un error {ex}")

            End Try

        End With

        Return _Estado

    End Function

    Private Function SeparaPais(partes_ As String()) As String()

        Dim paisFiltros_ As String() = Nothing

        If partes_.Length > 0 Then

            paisFiltros_ = partes_.Where(Function(x) x.Length >= 2 AndAlso x.Length <= 3 AndAlso x.All(AddressOf Char.IsLetter)) _
                .Select(Function(x) x.ToUpperInvariant()).Distinct().ToArray()

        End If

        Return paisFiltros_

    End Function

    Private Function PreparaAuxiliar(tipoUso_ As TipoUso, paisFiltros_ As String(), docs_ As List(Of OperacionGenerica)) As TagWatcher

        Dim auxiliares_ = Nothing

        Dim seccion_ = Nothing

        Dim tagwatcher_ = New TagWatcher

        If tipoUso_ = TipoUso.Proveedor Then

            auxiliares_ = New List(Of AuxiliarProveedor)

            seccion_ = SeccionesProvedorOperativo.SPRO2

        Else

            auxiliares_ = New List(Of AuxiliarDestinatario)

            seccion_ = SeccionesDestinatarios.SDES2

        End If

        If paisFiltros_ IsNot Nothing Then

            Dim auxiliar_ = Nothing

            For Each paisFiltro_ In paisFiltros_

                For Each documento_ In docs_

                    For Each partida_ As Partida In documento_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Seccion(seccion_).Nodos.Where(Function(x) x.Attribute(CamposDomicilio.CA_CVE_PAIS).Valor.Equals(paisFiltro_))

                        If tipoUso_ = TipoUso.Proveedor Then

                            auxiliar_ = CargaAuxiliarProveedor(documento_, partida_)

                            auxiliares_.Add(auxiliar_)

                        Else

                            auxiliar_ = CargaAuxiliarDestinatario(documento_, partida_)

                            auxiliares_.Add(auxiliar_)

                        End If

                    Next

                Next

            Next

            If auxiliar_ Is Nothing Then

                For Each documento_ In docs_

                    For Each partida_ As Partida In documento_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Seccion(seccion_).Nodos

                        If tipoUso_ = TipoUso.Proveedor Then

                            auxiliar_ = CargaAuxiliarProveedor(documento_, partida_)

                            auxiliares_.Add(auxiliar_)

                        Else

                            auxiliar_ = CargaAuxiliarDestinatario(documento_, partida_)

                            auxiliares_.Add(auxiliar_)

                        End If

                    Next

                Next

            End If

        Else

            For Each documento_ In docs_

                For Each partida_ As Partida In documento_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Seccion(seccion_).Nodos

                    Dim auxiliar_ = Nothing

                    If tipoUso_ = TipoUso.Proveedor Then

                        auxiliar_ = CargaAuxiliarProveedor(documento_, partida_)

                        auxiliares_.Add(auxiliar_)

                    Else

                        auxiliar_ = CargaAuxiliarDestinatario(documento_, partida_)

                        auxiliares_.Add(auxiliar_)

                    End If

                Next

            Next

        End If

        tagwatcher_.ObjectReturned = auxiliares_

        Return tagwatcher_

    End Function

    Private Function CargaAuxiliarProveedor(documento_ As OperacionGenerica, partida_ As Partida) As AuxiliarProveedor

        Dim auxiliarProveedor_ As New AuxiliarProveedor

        With auxiliarProveedor_

            .id = documento_.Id.ToString

            If partida_.Attribute(CamposProveedorOperativo.CP_ID_PROVEEDOR) IsNot Nothing Then

                .idtaxid = partida_.Attribute(CamposProveedorOperativo.CP_ID_PROVEEDOR).Valor.ToString

            End If

            ._cvepais = partida_.Attribute(CamposDomicilio.CA_CVE_PAIS).Valor

            ._pais = partida_.Attribute(CamposDomicilio.CA_PAIS).Valor

            ._taxid = partida_.Attribute(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR).Valor

            ._razonsocial = documento_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.NombrePropietario

            If partida_.Attribute(CamposProveedorOperativo.CP_FIRMA_ELECTRONICA) IsNot Nothing Then

                ._firmaElectronica = partida_.Attribute(CamposProveedorOperativo.CP_FIRMA_ELECTRONICA).Valor

            End If

            ._domicilio = New Domicilio

            ._domicilio._iddomicilio = New ObjectId(partida_.Attribute(CamposProveedorOperativo.CP_ID_DOMICILIO_PROVEEDOR).Valor.ToString)

            ._domicilio.calle = partida_.Attribute(CamposDomicilio.CA_CALLE).Valor

            ._domicilio.numeroexterior = partida_.Attribute(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor

            ._domicilio.numerointerior = partida_.Attribute(CamposDomicilio.CA_NUMERO_INTERIOR).Valor

            ._domicilio.codigopostal = partida_.Attribute(CamposDomicilio.CA_CODIGO_POSTAL).Valor

            ._domicilio.ciudad = partida_.Attribute(CamposDomicilio.CA_CIUDAD).Valor

            ._domicilio.colonia = partida_.Attribute(CamposDomicilio.CA_COLONIA).Valor

            ._domicilio.localidad = partida_.Attribute(CamposDomicilio.CA_LOCALIDAD).Valor

            ._domicilio.cveEntidadfederativa = partida_.Attribute(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor

            ._domicilio.municipio = partida_.Attribute(CamposDomicilio.CA_MUNICIPIO).Valor

            ._domicilio.cveMunicipio = partida_.Attribute(CamposDomicilio.CA_ENTIDAD_MUNICIPIO).Valor

            ._domicilio.entidadfederativa = partida_.Attribute(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor

            ._domicilio.domicilioPresentacion = partida_.Attribute(CamposProveedorOperativo.CA_DOMICILIO_FISCAL).Valor

            ._domicilio.archivado = partida_.Attribute(CamposProveedorOperativo.CA_ESTADO_DOMICILIO_PROVEEDOR).Valor

            ._domicilio.sec = partida_.Attribute(CamposProveedorOperativo.CP_SEC_DOMICILIO_PROVEEDOR).Valor
        End With

        Dim vinculaciones As Seccion = documento_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Seccion(SeccionesProvedorOperativo.SPRO4) '.Nodos.Where(Function(x) x.Attribute(CamposProveedorOperativo.CP_TAX_ID_VINCULACION).Valor.Equals(partida_.Attribute(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR).Valor))

        If vinculaciones.Nodos IsNot Nothing Then

            If vinculaciones.CantidadPartidas > 0 Then

                auxiliarProveedor_._listavinculaciones = New List(Of Vinculaciones)

                'For indice_ As Int32 = 1 To _Proveedor.Seccion(SeccionesProvedorOperativo.SPRO4).CantidadPartidas
                For Each partidaVinculacion_ As Partida In vinculaciones.Nodos.Where(Function(x) x.Attribute(CamposProveedorOperativo.CP_TAX_ID_VINCULACION).ValorPresentacion.Equals(partida_.Attribute(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR).Valor))

                    Dim vinculacion As New Vinculaciones

                    With partidaVinculacion_

                        If partida_.Attribute(CamposProveedorOperativo.CP_ID_PROVEEDOR) IsNot Nothing AndAlso partida_.Attribute(CamposProveedorOperativo.CP_ID_PROVEEDOR).Valor IsNot Nothing Then

                            vinculacion.idProveedor = partida_.Attribute(CamposProveedorOperativo.CP_ID_PROVEEDOR).Valor.ToString

                        End If
                        vinculacion.idClienteVinculado = .Campo(CamposProveedorOperativo.CP_ID_CLIENTE_VINCULACION).Valor

                        vinculacion.clienteVinculado = .Campo(CamposProveedorOperativo.CP_ID_CLIENTE_VINCULACION).ValorPresentacion

                        vinculacion.taxidVinculado = .Campo(CamposProveedorOperativo.CP_TAX_ID_VINCULACION).Valor

                        vinculacion.rfcVinculado = .Campo(CamposProveedorOperativo.CP_RFC_PROVEEDOR_VINCULACION).Valor

                        vinculacion.cveVinculacion = .Campo(CamposProveedorOperativo.CA_CVE_VINCULACION).Valor

                        vinculacion.vinculacion = .Campo(CamposProveedorOperativo.CA_CVE_VINCULACION).ValorPresentacion

                        vinculacion.porcentaje = .Campo(CamposProveedorOperativo.CP_PORCENTAJE_VINCULACION).Valor

                    End With

                    auxiliarProveedor_._listavinculaciones.Add(vinculacion)

                Next

            End If

        End If


        Return auxiliarProveedor_

    End Function

    Private Function ObtenerProveedorPorRazonSocial(ByVal razonSocial_ As String) As TagWatcher

        With _Estado

            Try

                Using iEnlace_ As IEnlaceDatos = New EnlaceDatos(27) With {.EspacioTrabajo = _espacioTrabajo}

                    Dim collection_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)((New ConstructorProveedoresOperativos).GetType.Name)

                    Dim pipeline_ = collection_.Aggregate().
                                                Project(Function(x) _
                                                 New With {
                                                 Key .documentoElectronico_ = x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente,
                                                 Key .id_ = x.Id}).
                                                 Match(Function(z) z.documentoElectronico_.NombrePropietario.Equals(razonSocial_)).
                                                 Limit(1).ToList
                    If pipeline_.Any() Then

                        If pipeline_.Count > 0 Then

                            _Proveedor = New ConstructorProveedoresOperativos

                            Dim idProveedorOperacionGenerica_ = Nothing

                            pipeline_.AsEnumerable.ToList.ForEach(Sub(x)

                                                                      idProveedorOperacionGenerica_ = x.id_.ToString

                                                                      _Proveedor = New ConstructorProveedoresOperativos(True, x.documentoElectronico_)

                                                                  End Sub)

                            GenerarEstructuraProveedorAuxiliar(_Proveedor, idProveedorOperacionGenerica_)

                        Else
                            .SetOKBut(Me, "Sin resultados")

                        End If

                    Else

                        .SetOKBut(Me, "Sin resultados")

                    End If

                End Using

            Catch ex As Exception

                .SetError(Me, $"Ha ocurrido un error: {ex}")

            End Try

        End With

        Return _Estado

    End Function

    Private Function ObtenerProveedorPorObjectId(ByVal objectidProveedor_ As ObjectId) As TagWatcher
        With _Estado
            Try
                Using iEnlace_ As IEnlaceDatos = New EnlaceDatos(27) With {.EspacioTrabajo = _espacioTrabajo}

                    Dim collection_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)((New ConstructorProveedoresOperativos).GetType.Name)

                    Dim pipeline_ = collection_.Aggregate().
                                                Project(Function(x) _
                                                 New With {
                                                 Key .documentoElectronico_ = x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente,
                                                 Key .id_ = x.Id}).
                                                 Match(Function(z) z.id_.Equals(objectidProveedor_)).
                                                 Limit(1).ToList

                    If pipeline_.Any() Then

                        If pipeline_.Count > 0 Then

                            _Proveedor = New ConstructorProveedoresOperativos

                            Dim idProveedorOperacionGenerica_ = Nothing

                            pipeline_.AsEnumerable.ToList.ForEach(Sub(x)

                                                                      idProveedorOperacionGenerica_ = x.id_.ToString

                                                                      _Proveedor = New ConstructorProveedoresOperativos(True, x.documentoElectronico_)

                                                                  End Sub)

                            GenerarEstructuraProveedorAuxiliar(_Proveedor, idProveedorOperacionGenerica_)

                        Else
                            .SetOKBut(Me, "Sin resultados")

                        End If

                    Else

                        .SetOKBut(Me, "Sin resultados")

                    End If

                End Using

            Catch ex As Exception

                .SetError(Me, $"Ha ocurrido un error: {ex}")

            End Try
        End With

        Return _Estado
    End Function

    Private Function GenerarEstructuraProveedorAuxiliar(ByRef Proveedor_ As ConstructorProveedoresOperativos,
                                                        ByRef idProveedorOperacionGenerica_ As String) As TagWatcher

        With _Estado

            Try
                _ProveedorAuxiliar = New AuxiliarProveedor

                If Proveedor_.Seccion(SeccionesProvedorOperativo.SPRO1) IsNot Nothing Then

                    With Proveedor_.Seccion(SeccionesProvedorOperativo.SPRO1)

                        _ProveedorAuxiliar.id = idProveedorOperacionGenerica_

                        _ProveedorAuxiliar._razonsocial = .Campo(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR).Valor

                        _ProveedorAuxiliar._clave = .Campo(CamposProveedorOperativo.CP_CVE_PROVEEDOR).Valor

                        If .Campo(CamposProveedorOperativo.CP_TIPO_PROVEEDOR).Valor Then

                            _ProveedorAuxiliar._rfc = .Campo(CamposProveedorOperativo.CA_RFC_PROVEEDOR).Valor

                            If .Campo(CamposProveedorOperativo.CP_TIPO_PERSONA_PROVEEDOR).Valor = False Then

                                If .Campo(CamposProveedorOperativo.CA_CURP_PROVEEDOR).Valor IsNot Nothing Then

                                    _ProveedorAuxiliar._curp = .Campo(CamposProveedorOperativo.CA_CURP_PROVEEDOR).Valor
                                End If

                            End If

                        End If

                        _ProveedorAuxiliar._espersonamoral = .Campo(CamposProveedorOperativo.CP_TIPO_PERSONA_PROVEEDOR).Valor

                        _ProveedorAuxiliar._esextranjero = .Campo(CamposProveedorOperativo.CP_TIPO_PROVEEDOR).Valor

                        _ProveedorAuxiliar._activo = .Campo(CamposProveedorOperativo.CP_PROVEEDOR_HABILITADO).Valor

                    End With

                End If

                If Proveedor_.Seccion(SeccionesProvedorOperativo.SPRO2).CantidadPartidas > 0 Then

                    _ProveedorAuxiliar._listadomiciliosconTaxid = New List(Of DomiciliosTaxid)

                    For indice_ As Int32 = 1 To Proveedor_.Seccion(SeccionesProvedorOperativo.SPRO2).CantidadPartidas

                        Dim domicilioAux_ As New DomiciliosTaxid

                        With Proveedor_.Seccion(SeccionesProvedorOperativo.SPRO2).Partida(indice_)

                            domicilioAux_.id = idProveedorOperacionGenerica_

                            domicilioAux_._iddomicilio = New ObjectId(.Campo(CamposProveedorOperativo.CP_ID_DOMICILIO_PROVEEDOR).Valor.ToString)

                            domicilioAux_.calle = .Campo(CamposDomicilio.CA_CALLE).Valor

                            domicilioAux_.numeroexterior = .Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor

                            domicilioAux_.numerointerior = .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor

                            domicilioAux_.codigopostal = .Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor

                            domicilioAux_.ciudad = .Campo(CamposDomicilio.CA_CIUDAD).Valor

                            domicilioAux_.colonia = .Campo(CamposDomicilio.CA_COLONIA).Valor

                            domicilioAux_.localidad = .Campo(CamposDomicilio.CA_LOCALIDAD).Valor

                            domicilioAux_.cveEntidadfederativa = .Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor

                            domicilioAux_.municipio = .Campo(CamposDomicilio.CA_MUNICIPIO).Valor

                            domicilioAux_.cveMunicipio = .Campo(CamposDomicilio.CA_ENTIDAD_MUNICIPIO).Valor

                            domicilioAux_.entidadfederativa = .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor

                            If .Campo(CamposProveedorOperativo.CP_TIPO_PROVEEDOR) IsNot Nothing Then

                                If .Campo(CamposProveedorOperativo.CP_TIPO_PROVEEDOR).Valor = False Then

                                    domicilioAux_.clavetaxid = .Campo(CamposProveedorOperativo.CA_CVE_TAX_ID_PROVEEDOR).Valor

                                    domicilioAux_.taxid = .Campo(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR).Valor

                                End If

                            End If

                            domicilioAux_.domicilioPresentacion = .Campo(CamposProveedorOperativo.CA_DOMICILIO_FISCAL).Valor

                            domicilioAux_.archivado = .Campo(CamposProveedorOperativo.CA_ESTADO_DOMICILIO_PROVEEDOR).Valor

                            domicilioAux_.sec = .Campo(CamposProveedorOperativo.CP_SEC_DOMICILIO_PROVEEDOR).Valor

                            domicilioAux_.idpais = .Campo(CamposDomicilio.CA_ID_PAIS).Valor

                            domicilioAux_.cvePais = .Campo(CamposDomicilio.CA_CVE_PAIS).Valor

                            domicilioAux_.pais = .Campo(CamposDomicilio.CA_PAIS).Valor

                            domicilioAux_._firmaElectronica = .Campo(CamposProveedorOperativo.CP_FIRMA_ELECTRONICA).Valor

                        End With

                        _ProveedorAuxiliar._listadomiciliosconTaxid.Add(domicilioAux_)

                    Next

                End If

                'VINCULACIONES
                If Proveedor_.Seccion(SeccionesProvedorOperativo.SPRO4) IsNot Nothing Then

                    If Proveedor_.Seccion(SeccionesProvedorOperativo.SPRO4).CantidadPartidas > 0 Then

                        _ProveedorAuxiliar._listavinculaciones = New List(Of Vinculaciones)

                        For indice_ As Int32 = 1 To Proveedor_.Seccion(SeccionesProvedorOperativo.SPRO4).CantidadPartidas

                            Dim vinculacion As New Vinculaciones

                            With Proveedor_.Seccion(SeccionesProvedorOperativo.SPRO4).Partida(indice_)

                                vinculacion.idProveedor = idProveedorOperacionGenerica_

                                vinculacion.idClienteVinculado = .Campo(CamposProveedorOperativo.CP_ID_CLIENTE_VINCULACION).Valor

                                vinculacion.clienteVinculado = .Campo(CamposProveedorOperativo.CP_ID_CLIENTE_VINCULACION).ValorPresentacion

                                vinculacion.taxidVinculado = .Campo(CamposProveedorOperativo.CP_TAX_ID_VINCULACION).Valor

                                vinculacion.rfcVinculado = .Campo(CamposProveedorOperativo.CP_RFC_PROVEEDOR_VINCULACION).Valor

                                vinculacion.cveVinculacion = .Campo(CamposProveedorOperativo.CA_CVE_VINCULACION).Valor

                                vinculacion.vinculacion = .Campo(CamposProveedorOperativo.CA_CVE_VINCULACION).ValorPresentacion

                                vinculacion.porcentaje = .Campo(CamposProveedorOperativo.CP_PORCENTAJE_VINCULACION).Valor

                            End With

                            _ProveedorAuxiliar._listavinculaciones.Add(vinculacion)

                        Next

                    End If

                End If

                'METODO VALORACION
                If Proveedor_.Seccion(SeccionesProvedorOperativo.SPRO5) IsNot Nothing Then

                    If Proveedor_.Seccion(SeccionesProvedorOperativo.SPRO5).CantidadPartidas > 0 Then

                        _ProveedorAuxiliar._listaconfiguracionesadicionales = New List(Of ConfiguracionAdicional)

                        For indice_ As Int32 = 1 To Proveedor_.Seccion(SeccionesProvedorOperativo.SPRO5).CantidadPartidas

                            Dim configuracionadicional_ As New ConfiguracionAdicional

                            With Proveedor_.Seccion(SeccionesProvedorOperativo.SPRO5).Partida(indice_)

                                configuracionadicional_.idProveedor = idProveedorOperacionGenerica_

                                configuracionadicional_.idclienteConfiguracion = .Campo(CamposProveedorOperativo.CP_ID_CLIENTE_CONFIGURACION).Valor

                                configuracionadicional_.clienteConfiguracion = .Campo(CamposProveedorOperativo.CP_ID_CLIENTE_CONFIGURACION).ValorPresentacion

                                configuracionadicional_.taxidConfiguracion = .Campo(CamposProveedorOperativo.CP_TAX_ID_CONFIGURACION).Valor

                                configuracionadicional_.rfcConfiguracion = .Campo(CamposProveedorOperativo.CP_RFC_PROVEEDOR_CONFIGURACION).Valor

                                configuracionadicional_.idmetodovaloracion = .Campo(CamposProveedorOperativo.CA_CVE_METODO_VALORACION).Valor

                                configuracionadicional_.metodovaloracion = .Campo(CamposProveedorOperativo.CA_CVE_METODO_VALORACION).ValorPresentacion

                                configuracionadicional_.cveincoterm = .Campo(CamposProveedorOperativo.CA_CVE_INCOTERM).Valor

                                configuracionadicional_.incoterm = .Campo(CamposProveedorOperativo.CP_INCOTERM).Valor

                            End With

                            _ProveedorAuxiliar._listaconfiguracionesadicionales.Add(configuracionadicional_)

                        Next

                    End If

                End If

                If _ProveedorAuxiliar IsNot Nothing Then

                    .ObjectReturned = _ProveedorAuxiliar

                    .SetOK()

                Else

                    .SetOKBut(Me, "No se encontró proveedor")

                End If

            Catch ex As Exception

                .SetError($"Ha ocurrido un error {ex}")

            End Try

        End With

        Return _Estado

    End Function


    Private Function ObtenerProveedorPorObjectIdTarjeta(ByVal objectidProveedor_ As ObjectId) As TagWatcher

        With _Estado

            Try

                Using iEnlace_ As IEnlaceDatos = New EnlaceDatos(27) With {.EspacioTrabajo = _espacioTrabajo}

                    Dim collection_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)((New ConstructorProveedoresOperativos).GetType.Name)

                    Dim pipeline = New List(Of BsonDocument) From {
                        New BsonDocument("$set",
                            New BsonDocument("tarjeta",
                                New BsonDocument("$first",
                                    New BsonDocument("$first",
                                        "$Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts.Cuerpo.Nodos"
                                    )
                                )
                            )
                        ),
                        New BsonDocument("$unwind", "$tarjeta.Nodos"),
                        New BsonDocument("$set",
                            New BsonDocument("idtarjeta",
                                New BsonDocument("$let",
                                    New BsonDocument From {
                                        {
                                            "vars",
                                            New BsonDocument("obj",
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray From {
                                                        New BsonDocument("$arrayElemAt",
                                                            New BsonArray From {
                                                                "$tarjeta.Nodos.Nodos.Nodos",
                                                                0
                                                            }
                                                        ),
                                                        0
                                                    }
                                                )
                                            )
                                        },
                                        {"in", "$$obj.Valor"}
                                    }
                                )
                            )
                        ),
                        New BsonDocument("$match",
                            New BsonDocument("idtarjeta",
                                objectidProveedor_
                            )
                        ),
                        New BsonDocument("$project",
                            New BsonDocument From {
                                {"Documento", "$Borrador.Folder.ArchivoPrincipal.Dupla.Fuente"},
                                {"tarjetas", "$tarjeta.Nodos"},
                                {"idtarjeta", 1}
                            }
                        )
                    }

                    ' Ejecutar el pipeline
                    Dim resultados = collection_.Aggregate(Of BsonDocument)(pipeline).ToList()

                    _ProveedorAuxiliar = New AuxiliarProveedor

                    _Proveedor = New ConstructorProveedoresOperativos

                    Dim Documento_ As ConstructorProveedoresOperativos = Nothing

                    Dim seccionTarjetas_ As Partida = Nothing

                    Dim id_ As ObjectId = ObjectId.Empty

                    Dim idTarjeta_ As ObjectId = ObjectId.Empty

                    If resultados.Any() Then

                        If resultados.Count > 0 Then

                            Dim doc = resultados.First()

                            id_ = doc("_id").AsObjectId

                            idTarjeta_ = doc("idtarjeta").AsObjectId

                            Documento_ = BsonSerializer.Deserialize(Of ConstructorProveedoresOperativos)(doc("Documento").AsBsonDocument)

                            seccionTarjetas_ = BsonSerializer.Deserialize(Of Partida)(doc("tarjetas").AsBsonDocument)

                            Dim operacionGenerica_ As New OperacionGenerica(Documento_)

                            operacionGenerica_.Id = id_

                            _ProveedorAuxiliar = CargaAuxiliarProveedor(operacionGenerica_, seccionTarjetas_)

                        End If

                        If _ProveedorAuxiliar IsNot Nothing Then

                            .ObjectReturned = _ProveedorAuxiliar

                            .SetOK()

                        Else

                            .SetOKBut(Me, "No se encontró proveedor")

                        End If

                    Else

                        .SetOKBut(Me, "No se encontró proveedor")

                    End If

                End Using

            Catch ex As Exception

                .SetError($"Ha ocurrido un error {ex}")

            End Try

        End With

        Return _Estado

    End Function

    Private Function TraerVinculaciones(ByVal idProveedor_ As ObjectId, ByVal taxIdProveedor_ As String, ByVal idCliente_ As ObjectId) As TagWatcher

        Dim seccionVinculacion_ As Seccion = Nothing

        Dim partida_ As Partida = Nothing

        With _Estado

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos(27) With {.EspacioTrabajo = _espacioTrabajo}

                Dim collection_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)((New ConstructorProveedoresOperativos).GetType.Name)

                Dim resultado = collection_.Find(Function(x) x.Id.Equals(idProveedor_)).First

                seccionVinculacion_ = resultado.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Seccion(SeccionesProvedorOperativo.SPRO4)

            End Using

            If seccionVinculacion_ IsNot Nothing Then

                Dim contador = 0

                For Each partida_ In seccionVinculacion_.Nodos.Where(Function(x) x.Attribute(CamposProveedorOperativo.
                    CP_TAX_ID_VINCULACION).ValorPresentacion.Equals(taxIdProveedor_) And x.Attribute(CamposProveedorOperativo.
                    CP_ID_CLIENTE_VINCULACION).Valor.Equals(idCliente_))

                    Dim vinculacion_ As New Vinculaciones

                    With vinculacion_

                        .clienteVinculado = idCliente_.ToString

                        .taxidVinculado = taxIdProveedor_

                        .idProveedor = idProveedor_.ToString

                        .cveVinculacion = partida_.Attribute(CamposProveedorOperativo.CA_CVE_VINCULACION).Valor

                        .vinculacion = partida_.Attribute(CamposProveedorOperativo.CA_CVE_VINCULACION).ValorPresentacion

                        .porcentaje = partida_.Attribute(CamposProveedorOperativo.CP_PORCENTAJE_VINCULACION).Valor

                    End With

                    .ObjectReturned = vinculacion_

                    contador += 1

                Next

                If contador = 0 Then

                    .SetOKBut(Me, "No se encontró vinculación")

                    .ObjectReturned = Nothing

                End If

            End If



        End With

        Return _Estado

    End Function

#End Region

#Region "Datos destinatarios"
    Private Function ObtenerDestinatariosRazonSocial(ByVal razonsocial_ As String,
                                                     ByVal pagenumber_ As Int16, ByVal limitedatos_ As Int16) As TagWatcher
        With _Estado

            Try
                Using iEnlace_ As IEnlaceDatos = New EnlaceDatos(27) With {.EspacioTrabajo = _espacioTrabajo}

                    Dim collection_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)((New ConstructorDestinatario).GetType.Name)

                    Dim filter_ = Builders(Of OperacionGenerica).Filter.Text(razonsocial_)

                    Dim pipeline_ = collection_.Aggregate().
                                                Match(filter_).
                                                Project(Function(x) _
                                                        New With {
                                                        Key .id_ = x.Id,
                                                        Key .publicado_ = x.Publicado,
                                                        Key .razonsocial_ = x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.NombrePropietario,
                                                        Key .documento_ = x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts}).
                                                        Project(Function(y) _
                                                                    New With {
                                                                        Key y.id_,
                                                                        Key y.publicado_,
                                                                        Key y.razonsocial_,
                                                                        Key .activo_ = DirectCast(y.documento_.Item("Encabezado")(0).Nodos(0).Nodos(5).Nodos(0), Campo).Valor,
                                                                        Key .activoPresentacion_ = DirectCast(y.documento_.Item("Encabezado")(0).Nodos(0).Nodos(5).Nodos(0), Campo).Valor,
                                                                        Key .taxid_ = DirectCast(y.documento_.Item("Cuerpo")(0).Nodos(0).Nodos(0).Nodos(1).Nodos(0), Campo).Valor}).ToList


                    'Dim pipeline_ = collection_.Aggregate().
                    '                            Match(filter_).
                    '                            Project(Function(x) _
                    '                                    New With {
                    '                                    Key .id_ = x.Id,
                    '                                    Key .publicado_ = x.Publicado,
                    '                                    Key .razonsocial_ = x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.NombrePropietario,
                    '                                    Key .activo = x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Encabezado")(0).Nodos(0).Nodos(5)}).
                    '                                    Project(Function(y) _
                    '                                           New With {
                    '                                           Key y.id_,
                    '                                           Key y.publicado_,
                    '                                           Key y.razonsocial_,
                    '                                           Key .activo_ = DirectCast(y.activo.Nodos(0), Campo).Valor,
                    '                                           Key .activoPresentacion = DirectCast(y.activo.Nodos(0), Campo).ValorPresentacion}).
                    '                             Skip((pagenumber_ - 1) * limitedatos_).
                    '                             Limit(10).ToList

                    If pipeline_.Any() Then

                        _listaDestinatarios = New List(Of AuxiliarDestinatario)

                        If pipeline_.Count > 0 Then

                            pipeline_.AsEnumerable.ToList.ForEach(Sub(x)
                                                                      Dim auxDestinatario As New AuxiliarDestinatario

                                                                      auxDestinatario.id = x.id_.ToString

                                                                      auxDestinatario._razonsocial = x.razonsocial_

                                                                      auxDestinatario._taxid = x.taxid_

                                                                      _listaDestinatarios.Add(auxDestinatario)

                                                                  End Sub)
                        End If

                        If _listaDestinatarios.Count > 0 Then

                            .ObjectReturned = _listaDestinatarios

                            .SetOK()

                        Else

                            .SetOKBut(Me, "No se encontró destinatario")

                        End If

                    Else

                        .SetOKBut(Me, "No se encontró destinatario")

                    End If

                End Using

            Catch ex As Exception

                .SetError($"Ha ocurrido un error {ex}")

            End Try

        End With

        Return _Estado

    End Function

    Private Function ObtenerDestinatariosRazonSocialPais(ByVal razonsocialPais_ As String,
                                                     ByVal pagenumber_ As Int16, ByVal limitedatos_ As Int16) As TagWatcher
        With _Estado

            Try
                Using iEnlace_ As IEnlaceDatos = New EnlaceDatos(27) With {.EspacioTrabajo = _espacioTrabajo}

                    Dim collection_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)((New ConstructorDestinatario).GetType.Name)

                    Dim razon_ = (If(razonsocialPais_, "")).Trim()

                    Dim paisFiltros_ = SeparaPais(razon_.Split({" "c}, StringSplitOptions.RemoveEmptyEntries))

                    Dim filter_ = Builders(Of OperacionGenerica).Filter.Text(razonsocialPais_)

                    Dim docs_ = collection_.Find(filter_).ToList()

                    Dim auxiliarDestinatarios_ As List(Of AuxiliarDestinatario) = PreparaAuxiliar(TipoUso.Destinatario, paisFiltros_, docs_).ObjectReturned

                    If auxiliarDestinatarios_.Count > 0 Then

                        .ObjectReturned = auxiliarDestinatarios_

                        .SetOK()

                    Else

                        .SetOKBut(Me, "No se encontró destinatario")

                    End If

                End Using

            Catch ex As Exception

                .SetError($"Ha ocurrido un error {ex}")

            End Try

        End With

        Return _Estado

    End Function

    Private Function CargaAuxiliarDestinatario(documento_ As OperacionGenerica, partida_ As Partida) As AuxiliarDestinatario

        Dim auxiliarDestinatario_ As New AuxiliarDestinatario

        auxiliarDestinatario_._listadomiciliosconTaxid = New List(Of DomiciliosTaxid)

        Dim domicilioAux_ As New DomiciliosTaxid

        With partida_

            auxiliarDestinatario_.id = documento_.Id.ToString

            auxiliarDestinatario_._cvepais = .Campo(CamposDomicilio.CA_CVE_PAIS).Valor

            auxiliarDestinatario_._razonsocial = documento_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.NombrePropietario

            auxiliarDestinatario_._taxid = .Campo(CamposDestinatario.CA_TAX_ID).Valor

            auxiliarDestinatario_._pais = .Campo(CamposDomicilio.CA_PAIS).Valor

            domicilioAux_.id = .Campo(CamposDestinatario.CP_ID_DESTINATARIO).Valor.ToString

            auxiliarDestinatario_.idtaxid = .Campo(CamposDestinatario.CP_ID_DESTINATARIO).Valor.ToString

            auxiliarDestinatario_._firmaElectronica = .Campo(CamposProveedorOperativo.CP_FIRMA_ELECTRONICA).Valor.ToString

            domicilioAux_._iddomicilio = New ObjectId(.Campo(CamposDestinatario.CP_ID_DOMICILIO_DESTINATARIO).Valor.ToString)

            domicilioAux_.calle = .Campo(CamposDomicilio.CA_CALLE).Valor

            domicilioAux_.numeroexterior = .Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor

            domicilioAux_.numerointerior = .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor

            domicilioAux_.codigopostal = .Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor

            domicilioAux_.ciudad = .Campo(CamposDomicilio.CA_CIUDAD).Valor

            domicilioAux_.colonia = .Campo(CamposDomicilio.CA_COLONIA).Valor

            domicilioAux_.localidad = .Campo(CamposDomicilio.CA_LOCALIDAD).Valor

            domicilioAux_.cveEntidadfederativa = .Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor

            domicilioAux_.municipio = .Campo(CamposDomicilio.CA_MUNICIPIO).Valor

            domicilioAux_.cveMunicipio = .Campo(CamposDomicilio.CA_ENTIDAD_MUNICIPIO).Valor

            domicilioAux_.entidadfederativa = .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor

            domicilioAux_.clavetaxid = .Campo(CamposDestinatario.CA_CVE_TAX_ID_DESTINATARIO).Valor

            domicilioAux_.taxid = .Campo(CamposDestinatario.CA_TAX_ID).Valor

            domicilioAux_.domicilioPresentacion = .Campo(CamposDestinatario.CA_DOMICILIO_FISCAL_DESTINATARIO).Valor

            domicilioAux_.archivado = .Campo(CamposDestinatario.CA_ESTADO_DOMICILIO_DESTINATARIO).Valor

            domicilioAux_.sec = .Campo(CamposDestinatario.CP_SEC_DOMICILIO_DESTINATARIO).Valor

            domicilioAux_.idpais = .Campo(CamposDomicilio.CA_ID_PAIS).Valor

            domicilioAux_.cvePais = .Campo(CamposDomicilio.CA_CVE_PAIS).Valor

            domicilioAux_.pais = .Campo(CamposDomicilio.CA_PAIS).Valor

        End With

        auxiliarDestinatario_._listadomiciliosconTaxid.Add(domicilioAux_)

        Return auxiliarDestinatario_

    End Function

    Private Function ObtenerDestinatarioPorObjectId(ByVal objectidDestinatario_ As ObjectId) As TagWatcher

        With _Estado

            Try

                Using iEnlace_ As IEnlaceDatos = New EnlaceDatos(27) With {.EspacioTrabajo = _espacioTrabajo}

                    Dim collection_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)((New ConstructorDestinatario).GetType.Name)

                    Dim pipeline_ = collection_.Aggregate().
                                                Project(Function(x) _
                                                 New With {
                                                 Key .documentoElectronico_ = x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente,
                                                 Key .id_ = x.Id}).
                                                 Match(Function(z) z.id_.Equals(objectidDestinatario_)).
                                                 Limit(1).ToList

                    _DestinatarioAuxiliar = New AuxiliarDestinatario

                    _Destinatario = New ConstructorDestinatario

                    If pipeline_.Any() Then

                        If pipeline_.Count > 0 Then

                            pipeline_.AsEnumerable.ToList.ForEach(Sub(x)
                                                                      _Destinatario = New ConstructorDestinatario(True, x.documentoElectronico_)
                                                                  End Sub)

                            If _Destinatario.Seccion(SeccionesDestinatarios.SDES1) IsNot Nothing Then

                                With _Destinatario.Seccion(SeccionesDestinatarios.SDES1)

                                    _DestinatarioAuxiliar.id = .Campo(CamposDestinatario.CP_ID_DESTINATARIO).Valor.ToString

                                    _DestinatarioAuxiliar._razonsocial = .Campo(CamposDestinatario.CA_RAZON_SOCIAL).Valor

                                    _DestinatarioAuxiliar._clave = .Campo(CamposDestinatario.CP_CVE_DESTINATARIO).Valor

                                    If .Campo(CamposDestinatario.CA_RFC_DESTINATARIO) IsNot Nothing Then

                                        _DestinatarioAuxiliar._rfc = .Campo(CamposDestinatario.CA_RFC_DESTINATARIO).Valor

                                    End If

                                    If .Campo(CamposDestinatario.CA_TAX_ID) IsNot Nothing Then

                                        _DestinatarioAuxiliar._taxid = .Campo(CamposDestinatario.CA_TAX_ID).Valor

                                    End If

                                    _DestinatarioAuxiliar._activo = .Campo(CamposDestinatario.CA_DESTINATARIO_HABILITADO).Valor

                                End With

                            End If

                            If _Destinatario.Seccion(SeccionesDestinatarios.SDES2).CantidadPartidas > 0 Then

                                _DestinatarioAuxiliar._listadomiciliosconTaxid = New List(Of DomiciliosTaxid)

                                For indice_ As Int32 = 1 To _Destinatario.Seccion(SeccionesDestinatarios.SDES2).CantidadPartidas

                                    Dim domicilioAux_ As New DomiciliosTaxid

                                    With _Destinatario.Seccion(SeccionesDestinatarios.SDES2).Partida(indice_)

                                        domicilioAux_.id = _Destinatario.Seccion(SeccionesDestinatarios.SDES1).Campo(CamposDestinatario.CP_ID_DESTINATARIO).Valor.ToString

                                        domicilioAux_._iddomicilio = New ObjectId(.Campo(CamposDestinatario.CP_ID_DOMICILIO_DESTINATARIO).Valor.ToString)

                                        domicilioAux_.calle = .Campo(CamposDomicilio.CA_CALLE).Valor

                                        domicilioAux_.numeroexterior = .Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor

                                        domicilioAux_.numerointerior = .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor

                                        domicilioAux_.codigopostal = .Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor

                                        domicilioAux_.ciudad = .Campo(CamposDomicilio.CA_CIUDAD).Valor

                                        domicilioAux_.colonia = .Campo(CamposDomicilio.CA_COLONIA).Valor

                                        domicilioAux_.localidad = .Campo(CamposDomicilio.CA_LOCALIDAD).Valor

                                        domicilioAux_.cveEntidadfederativa = .Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor

                                        domicilioAux_.municipio = .Campo(CamposDomicilio.CA_MUNICIPIO).Valor

                                        domicilioAux_.cveMunicipio = .Campo(CamposDomicilio.CA_ENTIDAD_MUNICIPIO).Valor

                                        domicilioAux_.entidadfederativa = .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor

                                        domicilioAux_.clavetaxid = .Campo(CamposDestinatario.CA_CVE_TAX_ID_DESTINATARIO).Valor

                                        domicilioAux_.taxid = .Campo(CamposDestinatario.CA_TAX_ID).Valor

                                        domicilioAux_.domicilioPresentacion = .Campo(CamposDestinatario.CA_DOMICILIO_FISCAL_DESTINATARIO).Valor

                                        domicilioAux_.archivado = .Campo(CamposDestinatario.CA_ESTADO_DOMICILIO_DESTINATARIO).Valor

                                        domicilioAux_.sec = .Campo(CamposDestinatario.CP_SEC_DOMICILIO_DESTINATARIO).Valor

                                        domicilioAux_.idpais = .Campo(CamposDomicilio.CA_ID_PAIS).Valor

                                        domicilioAux_.cvePais = .Campo(CamposDomicilio.CA_CVE_PAIS).Valor

                                        domicilioAux_.pais = .Campo(CamposDomicilio.CA_PAIS).Valor

                                    End With

                                    _DestinatarioAuxiliar._listadomiciliosconTaxid.Add(domicilioAux_)

                                Next

                            End If

                            If _DestinatarioAuxiliar IsNot Nothing Then

                                .ObjectReturned = _DestinatarioAuxiliar

                                .SetOK()

                            Else

                                .SetOKBut(Me, "No se encontró destinatario")

                            End If

                        Else

                            .SetOKBut(Me, "No se encontró destinatario")

                        End If

                    End If

                End Using

            Catch ex As Exception

                .SetError($"Ha ocurrido un error {ex}")

            End Try

        End With

        Return _Estado

    End Function

#End Region

#Region "Métodos públicos"
    <Obsolete("Esta función está obsoleta.", False)>
    Public Function ConsultarOne(ByVal razonsocial_ As String,
                                 ByVal pais_ As String,
                                 Optional ByVal calle_ As String = Nothing,
                                 Optional ByVal codigopostal_ As String = Nothing) As TagWatcher
        With _Estado

            If _TipoProveedor <> TiposProveedores.SinDefinir Then

                If razonsocial_ IsNot Nothing And pais_ IsNot Nothing Then

                    BuscarProveedorConDomicilio(razonsocial_, pais_, calle_, codigopostal_)

                Else

                    .SetError(Me, "Todos los campos son requeridos")

                End If

            Else

                .SetError(Me, "Tipo de proveedor no definido")

            End If

        End With

        Return _Estado

    End Function

    Public Function ObtenerProveedoresPorRazonSocial(ByVal razonsocial_ As String,
                                                     Optional ByVal pagenumber_ As Int16 = 1,
                                                     Optional ByVal limitedatos_ As Int16 = 10) As TagWatcher
        With _Estado

            If razonsocial_ <> "" Then

                ObtenerProveedoresRazonSocial(razonsocial_, pagenumber_, limitedatos_)

            Else

                .SetOKBut(Me, "Razón social de proveedor no recibido")

            End If

        End With

        Return _Estado

    End Function
    Public Function ObtenerProveedoresPorRazonSocialPais(ByVal razonsocialPais_ As String,
                                                     Optional ByVal pagenumber_ As Int16 = 1,
                                                     Optional ByVal limitedatos_ As Int16 = 10) As TagWatcher
        With _Estado

            If razonsocialPais_ <> "" Then

                ObtenerProveedoresRazonSocialPais(razonsocialPais_, pagenumber_, limitedatos_)

            Else

                .SetOKBut(Me, "Razón social de proveedor no recibido")

            End If

        End With

        Return _Estado

    End Function

    Public Function ObtenerDatosTarjetaPorObjectId(ByVal objectidProveedor As ObjectId) As TagWatcher

        With _Estado

            If Not objectidProveedor = ObjectId.Empty Then

                ObtenerProveedorPorObjectIdTarjeta(objectidProveedor)

            Else

                .SetOKBut(Me, "Proveedor no recibido")

            End If

        End With

        Return _Estado

    End Function

    Public Function ObtenerDatosProveedorPorRazonSocial(ByVal razonSocial_ As String) As TagWatcher

        With _Estado

            If razonSocial_ IsNot Nothing Or razonSocial_ <> "" Then

                ObtenerProveedorPorRazonSocial(razonSocial_)

            Else

                .SetOKBut(Me, "Proveedor no recibido")

            End If

        End With

        Return _Estado

    End Function

    Public Function ObtenerDatosProveedorPorObjectId(ByVal objectidProveedor As ObjectId) As TagWatcher

        With _Estado

            If Not objectidProveedor = ObjectId.Empty Then

                ObtenerProveedorPorObjectId(objectidProveedor)

            Else

                .SetOKBut(Me, "Proveedor no recibido")

            End If

        End With

        Return _Estado

    End Function

    Public Function TraerVinculacion(ByVal idProveedor_ As ObjectId, ByVal taxIdProveedor_ As String, ByVal idCliente_ As ObjectId) As TagWatcher

        With _Estado

            If Not idProveedor_ = ObjectId.Empty Then

                TraerVinculaciones(idProveedor_, taxIdProveedor_, idCliente_)

            Else

                .SetOKBut(Me, "Proveedor no recibido")

            End If

            If .ObjectReturned IsNot Nothing Then

                .SetOK()

            Else

                .SetOKBut(Me, "No se encontró vinculación")

            End If

        End With

        Return _Estado

    End Function

#Region "Destinatario"
    Public Function ObtenerDestinatariosPorRazonSocial(ByVal razonsocial_ As String,
                                                       Optional ByVal pagenumber_ As Int16 = 1,
                                                       Optional ByVal limitedatos_ As Int16 = 10) As TagWatcher
        With _Estado

            If razonsocial_ <> "" Then

                ObtenerDestinatariosRazonSocial(razonsocial_, pagenumber_, limitedatos_)

            Else

                .SetOKBut(Me, "Razón social de proveedor no recibido")

            End If

        End With

        Return _Estado

    End Function
    Public Function ObtenerDestinatariosPorRazonSocialPais(ByVal razonsocialPais_ As String,
                                                       Optional ByVal pagenumber_ As Int16 = 1,
                                                       Optional ByVal limitedatos_ As Int16 = 10) As TagWatcher
        With _Estado

            If razonsocialPais_ <> "" Then

                ObtenerDestinatariosRazonSocialPais(razonsocialPais_, pagenumber_, limitedatos_)

            Else

                .SetOKBut(Me, "Razón social de proveedor no recibido")

            End If

        End With

        Return _Estado

    End Function

    Public Function ObtenerDatosDestinatarioPorObjectId(ByVal objectidDestinatario_ As ObjectId) As TagWatcher
        _Estado = Nothing
        _Estado = New TagWatcher
        With _Estado

            If Not objectidDestinatario_ = ObjectId.Empty Then

                ObtenerDestinatarioPorObjectId(objectidDestinatario_)

            Else

                .SetOKBut(Me, "Destinatario no recibido")

            End If

        End With

        Return _Estado

    End Function

#End Region

    Protected Overridable Sub Dispose(disposing As Boolean)

        If Not disposedValue Then

            If disposing Then
                ' TODO: eliminar el estado administrado (objetos administrados)
                _espacioTrabajo = Nothing

                disposedValue = Nothing

                _TipoOperacion = Nothing

                _TipoProveedor = Nothing

                _Proveedor = Nothing

                _ProveedorAuxiliar = Nothing

                _Estado = Nothing

            End If

            ' TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
            ' TODO: establecer los campos grandes como NULL
            disposedValue = True

        End If

    End Sub

    ' TODO: reemplazar el finalizador solo si "Dispose(disposing As Boolean)" tiene código para liberar los recursos no administrados
    Protected Overrides Sub Finalize()
        ' No cambie este código. Coloque el código de limpieza en el método "Dispose(disposing As Boolean)".
        Dispose(disposing:=False)

        MyBase.Finalize()

    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' No cambie este código. Coloque el código de limpieza en el método "Dispose(disposing As Boolean)".
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)

    End Sub

    Public Sub ReiniciarControlador()

        Inicializa(TiposProveedores.SinDefinir, TipoOperacion.SinDefinir, Nothing)

    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Throw New NotImplementedException()
    End Function

#End Region

End Class

Public Class AuxiliarProveedor
    <BsonIgnoreIfNull>
    Property id As String

    <BsonIgnoreIfNull>
    Property idtaxid As String

    Property _razonsocial As String

    <BsonIgnoreIfNull>
    Property _clave As String

    <BsonIgnoreIfNull>
    Property _taxid As String

    <BsonIgnoreIfNull>
    Property _rfc As String

    <BsonIgnoreIfNull>
    Property _curp As String

    <BsonIgnoreIfNull>
    Property _domicilio As Rec.Globals.Empresas.Domicilio

    <BsonIgnoreIfNull>
    Property _cvepais As String

    <BsonIgnoreIfNull>
    Property _pais As String

    <BsonIgnoreIfNull>
    Property _vinculacion As Integer

    <BsonIgnoreIfNull>
    Property _cvemetodovaloracion As Integer

    <BsonIgnoreIfNull>
    Property _aplicacertificado As Boolean

    <BsonIgnoreIfNull>
    Property _razonsocialcertificado As String

    <BsonIgnoreIfNull>
    Property _esdestinatario As Boolean

    <BsonIgnoreIfNull>
    Property _listadomicilios As List(Of Rec.Globals.Empresas.Domicilio)

    <BsonIgnoreIfNull>
    Property _listataxids As List(Of Rec.Globals.Empresas.TaxId)

    <BsonIgnoreIfNull>
    Property _listadomiciliosconTaxid As List(Of DomiciliosTaxid)

    <BsonIgnoreIfNull>
    Property _espersonamoral As Boolean

    <BsonIgnoreIfNull>
    Property _esextranjero As Boolean

    <BsonIgnoreIfNull>
    Property _activo As Boolean

    <BsonIgnoreIfNull>
    Property _listavinculaciones As List(Of Vinculaciones)

    <BsonIgnoreIfNull>
    Property _listaconfiguracionesadicionales As List(Of ConfiguracionAdicional)

    <BsonIgnoreIfNull>
    Property _firmaElectronica As String

End Class

Public Class DomiciliosTaxid
    Inherits Rec.Globals.Empresas.Domicilio

    <BsonIgnoreIfNull>
    Property id As String

    <BsonIgnoreIfNull>
    Property clavetaxid As String

    <BsonIgnoreIfNull>
    Property taxid As String

    <BsonIgnoreIfNull>
    Property pais As String

    <BsonIgnoreIfNull>
    Property cvePais As String

    <BsonIgnoreIfNull>
    Property idpais As String

    <BsonIgnoreIfNull>
    Property _firmaElectronica As String

End Class

Public Class Vinculaciones

    <BsonIgnoreIfNull>
    Property idProveedor As String

    <BsonIgnoreIfNull>
    Property idClienteVinculado As ObjectId

    <BsonIgnoreIfNull>
    Property clienteVinculado As String

    <BsonIgnoreIfNull>
    Property taxidVinculado As String

    <BsonIgnoreIfNull>
    Property rfcVinculado As String

    <BsonIgnoreIfNull>
    Property cveVinculacion As String

    <BsonIgnoreIfNull>
    Property vinculacion As String

    <BsonIgnoreIfNull>
    Property porcentaje As Double

End Class

Public Class ConfiguracionAdicional

    <BsonIgnoreIfNull>
    Property idProveedor As String

    <BsonIgnoreIfNull>
    Property idclienteConfiguracion As ObjectId

    <BsonIgnoreIfNull>
    Property clienteConfiguracion As String

    <BsonIgnoreIfNull>
    Property taxidConfiguracion As String

    <BsonIgnoreIfNull>
    Property rfcConfiguracion As String

    <BsonIgnoreIfNull>
    Property idmetodovaloracion As String

    <BsonIgnoreIfNull>
    Property metodovaloracion As String

    <BsonIgnoreIfNull>
    Property cveincoterm As String

    <BsonIgnoreIfNull>
    Property incoterm As String

End Class

Public Class HistorialDomicilios

    <BsonIgnoreIfNull>
    Property _idProveedorDestinatario As String
    <BsonIgnoreIfNull>
    Property _razonsocial As String
    <BsonIgnoreIfNull>
    Property _iddomicilio As String
    <BsonIgnoreIfNull>
    Property _secdomicilio As String
    <BsonIgnoreIfNull>
    Property _taxidrfcdomicilio As String
    <BsonIgnoreIfNull>
    Property _domiciliofiscal As String
    <BsonIgnoreIfNull>
    Property _loginuser As String
    <BsonIgnoreIfNull>
    Property _entorno As String
    <BsonIgnoreIfNull>
    Property _estadodomicilio As Boolean
    <BsonIgnoreIfNull>
    Property _archivadodomicilio As String
    <BsonIgnoreIfNull>
    Property motivoarchivadodomicilio As String
    <BsonIgnoreIfNull>
    Property fechaarchivadodomicilio As String

End Class

Public Class AuxiliarDestinatario

    <BsonIgnoreIfNull>
    Property id As String

    <BsonIgnoreIfNull>
    Property idtaxid As String

    Property _razonsocial As String

    <BsonIgnoreIfNull>
    Property _clave As String

    <BsonIgnoreIfNull>
    Property _clavetaxid As String

    <BsonIgnoreIfNull>
    Property _taxid As String

    <BsonIgnoreIfNull>
    Property _rfc As String

    <BsonIgnoreIfNull>
    Property _curp As String

    <BsonIgnoreIfNull>
    Property _cvepais As String

    <BsonIgnoreIfNull>
    Property _pais As String

    <BsonIgnoreIfNull>
    Property _listataxids As List(Of Rec.Globals.Empresas.TaxId)

    <BsonIgnoreIfNull>
    Property _listadomiciliosconTaxid As List(Of DomiciliosTaxid)

    <BsonIgnoreIfNull>
    Property _activo As Boolean

    <BsonIgnoreIfNull>
    Property _firmaElectronica As String

End Class