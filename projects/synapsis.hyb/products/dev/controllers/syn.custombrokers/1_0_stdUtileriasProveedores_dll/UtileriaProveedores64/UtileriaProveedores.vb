Imports gsol
Imports gsol.Web.Components
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports System.Web.Caching
Imports Rec.Globals.Controllers.Empresas
Imports Rec.Globals.Empresas
Imports Rec.Globals.Utils
Imports Syn.CustomBrokers.Controllers
Imports Syn.Utils
Imports Wma.Exceptions
Imports Wma.Exceptions.TagWatcher
Imports Syn.Nucleo.RecursosComercioExterior
Imports Rec.Globals.Controllers
Imports System.Web
Imports System.Net


Public Class UtileriaProveedores

#Region "Propiedades privadas"

    Private _estado As TagWatcher

    Private _empresaInternacional As IEmpresaInternacional

    Private _empresaNacional As IEmpresaNacional

    Private _listaDomicilios As List(Of Rec.Globals.Empresas.Domicilio)

    Private _controladorProveedores As CtrlProveedoresOperativos

    Private _auxiliarProveedorComprador As AuxiliarProveedor

    Private _auxiliarDestinatario As AuxiliarDestinatario

    Private _lista As List(Of SelectOption)

    Private _controladorEmpresas As IControladorEmpresas

    Private _listaempresas As IEmpresa

    Private _listaHistorialDomicilios As List(Of HistorialDomicilios)

    Private _estructuraempresaInternacional As IEmpresaInternacional

    Private _estructuraempresaNacional As IEmpresaNacional

    Private _domicilio As Domicilio

    Private _secuencia As ISecuencia

    Private _controladorSecuencias As IControladorSecuencia

    Private _paisdomicilio As PaisDomicilio

    Private _cachePaisMexicano As PaisDomicilio



#End Region

#Region "Constructor"

    Sub New()

        _estado = New TagWatcher

        _empresaInternacional = New EmpresaInternacional

        _empresaNacional = New EmpresaNacional

        _listaDomicilios = New List(Of Rec.Globals.Empresas.Domicilio)

        _controladorProveedores = New CtrlProveedoresOperativos

        _auxiliarProveedorComprador = New AuxiliarProveedor

        _auxiliarDestinatario = New AuxiliarDestinatario

        _domicilio = New Domicilio
    End Sub


#End Region
#Region "Buscar empresas"
    Public Function ListarEmpresasPorRazonSocial(ByVal razonsocial_ As String, Optional ByVal tipoempresa_ _
                                                 As IControladorEmpresas.TiposEmpresas = IControladorEmpresas.TiposEmpresas.Internacional) As List(Of SelectOption)

        _lista = New List(Of SelectOption)

        _controladorEmpresas = New Rec.Globals.Controllers.Empresas.ControladorEmpresas(New EspacioTrabajo(),
                                                                                        tipoempresa_) With {.ListaEmpresas = New List(Of IEmpresa)}

        With _controladorEmpresas

            _estado = .Consultar(razonsocial_)

            If _estado.Status = TypeStatus.Ok Then

                If _estado.ObjectReturned.count > 0 Then

                    For Each item_ In _estado.ObjectReturned
                        _lista.Add(New SelectOption _
                               With {
                                    .Value = item_._id.ToString,
                                    .Text = item_.razonsocial})
                    Next

                End If

            End If

        End With

        Return _lista

    End Function

    Public Function BuscarEmpresaPorObjectId(ByVal objectidEmpresa_ As String,
                                                Optional ByVal tipoempresa_ As IControladorEmpresas.TiposEmpresas = IControladorEmpresas.TiposEmpresas.Internacional) As TagWatcher
        _estado = New TagWatcher

        _controladorEmpresas = New Rec.Globals.Controllers.Empresas.ControladorEmpresas(New EspacioTrabajo(), tipoempresa_) _
            With {.ListaEmpresas = New List(Of IEmpresa)}

        _estado = _controladorEmpresas.ConsultarUna(ObjectId.Parse(objectidEmpresa_))

        If _estado.Status = TypeStatus.Ok Then

            Return _estado

        End If

        _estado.Status = TypeStatus.OkBut

        Return _estado

    End Function

#End Region

#Region "Guardar empresas"
    Public Function GuardarEmpresa(ByVal estructuraempresa_ As EmpresaNacional,
                                   ByVal session_ As IClientSessionHandle,
                                   Optional ByVal esempresanueva_ As Boolean = True) As TagWatcher

        _controladorEmpresas = New Rec.Globals.Controllers.Empresas.ControladorEmpresas(New EspacioTrabajo()) With {.ListaEmpresas = New List(Of IEmpresa)}

        If esempresanueva_ Then

            _estado = _controladorEmpresas.Agregar(estructuraempresa_, True, session_)

        Else

            _estado = _controladorEmpresas.Modificar(estructuraempresa_, session_)

        End If

        Return _estado

    End Function

    Public Function GuardarEmpresa(ByVal estructuraempresa_ As EmpresaInternacional,
                                   ByVal session_ As IClientSessionHandle,
                                   ByVal cvePais_ As String,
                                   Optional ByVal esempresanueva_ As Boolean = True) As TagWatcher

        _controladorEmpresas = New Rec.Globals.Controllers.Empresas.ControladorEmpresas(New EspacioTrabajo(),
                                                                                             IControladorEmpresas.TiposEmpresas.Internacional,
                                                                                             paisEmpresa_:=cvePais_) With {.ListaEmpresas = New List(Of IEmpresa)}

        If esempresanueva_ Then

            _estado = _controladorEmpresas.Agregar(estructuraempresa_, True, session_)

        Else

            _estado = _controladorEmpresas.Modificar(estructuraempresa_, session_)

        End If

        Return _estado

    End Function

#Region "Obtener datos para empresa nacional"
    Public Function ObtenerDatosPaisMexicano() As PaisDomicilio

        ''DEJAR PENDIENTE PARA AUTOMATIZARLO PORQUE NO JALA
        _estado = ControladorPaises.ConsultarListaPaisesPorClaveISO("MEX")

        Dim pais_ = _estado.ObjectReturned

        _cachePaisMexicano = New PaisDomicilio

        _cachePaisMexicano = CType(HttpRuntime.Cache("cachePaisMexicano"), PaisDomicilio)

        If _cachePaisMexicano Is Nothing Then
            ' Si no existe en caché, lo creamos
            _paisdomicilio = New PaisDomicilio

            With _paisdomicilio
                .idpais = pais_(0)._id
                .pais = pais_(0).cveISO3
                .paisPresentacion = pais_(0).paisPresentacion
            End With

            _cachePaisMexicano = _paisdomicilio

            HttpRuntime.Cache.Insert("cachePaisMexicano", _cachePaisMexicano, Nothing, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration)
        End If

        Return _cachePaisMexicano

    End Function
#End Region

#Region "Obtener datos para procesar"
    Private Function ObtenerDomicilio(ByVal domicilioTaxid_ As DomiciliosTaxid,
                                           Optional ByVal esdomicilionuevo_ As Boolean = False) As Domicilio

        With domicilioTaxid_

            _domicilio = New Rec.Globals.Empresas.Domicilio(calle_:= .calle,
                                                            sec_:=IIf(esdomicilionuevo_, 1, .sec),
                                                            numeroexterior_:= .numeroexterior,
                                                            numerointerior_:= .numerointerior,
                                                            colonia_:= .colonia,
                                                            codigopostal_:= .codigopostal,
                                                            ciudad_:= .ciudad,
                                                            localidad_:= .localidad,
                                                            municipio_:= .municipio,
                                                            entidadfederativa_:= .entidadfederativa,
                                                            pais_:= .pais)
        End With

        If esdomicilionuevo_ = False Then

            _domicilio._iddomicilio = domicilioTaxid_._iddomicilio

        End If

        Return _domicilio

    End Function

    Public Function GenerarEstructuraEmpresa(ByVal razonsocial_ As String,
                                             ByVal taxid_ As String,
                                             ByVal domicilio_ As DomiciliosTaxid) As EmpresaInternacional

        _controladorEmpresas = New ControladorEmpresas(New EspacioTrabajo(),
                                                       IControladorEmpresas.TiposEmpresas.Internacional, paisEmpresa_:=domicilio_.cvePais)

        _estructuraempresaInternacional = New EmpresaInternacional

        _domicilio = New Domicilio

        _domicilio = ObtenerDomicilio(domicilio_, esdomicilionuevo_:=True)

        _estructuraempresaInternacional = _controladorEmpresas.EstructuraEmpresaInternacional(razonsocial_, _domicilio, taxid_)

        Return _estructuraempresaInternacional

    End Function

    Public Function GenerarEstructuraEmpresa(ByVal razonsocial_ As String,
                                             ByVal rfc_ As String,
                                             ByVal domicilio_ As DomiciliosTaxid,
                                             Optional ByVal curp_ As String = Nothing,
                                             Optional ByVal tipopersona_ As IEmpresaNacional.TiposPersona = IEmpresaNacional.TiposPersona.Moral) As EmpresaNacional

        _controladorEmpresas = New ControladorEmpresas(New EspacioTrabajo(), IControladorEmpresas.TiposEmpresas.Nacional)

        _estructuraempresaNacional = New EmpresaNacional

        _domicilio = New Domicilio

        _domicilio = ObtenerDomicilio(domicilio_, esdomicilionuevo_:=True)

        If tipopersona_ = IEmpresaNacional.TiposPersona.Fisica Then

            _estructuraempresaNacional = _controladorEmpresas.EstructuraEmpresaNacional(razonsocial_,
                                                                                        rfc_,
                                                                                         _domicilio,
                                                                                        IEmpresaNacional.TiposPersona.Fisica,
                                                                                        curp_)
        Else

            _estructuraempresaNacional = _controladorEmpresas.EstructuraEmpresaNacional(razonsocial_,
                                                                              rfc_,
                                                                               _domicilio,
                                                                              IEmpresaNacional.TiposPersona.Moral)

        End If

        Return _estructuraempresaNacional

    End Function

    Public Function ObtenerDatosDesdePillbox(ByVal fcRazonSocial_ As String,
                                             ByVal datasourcePillbox_ As PillboxControl) As AuxiliarDestinatario

        _auxiliarDestinatario = New AuxiliarDestinatario

        With _auxiliarDestinatario

            ._razonsocial = fcRazonSocial_

            ._listadomiciliosconTaxid = New List(Of DomiciliosTaxid)

        End With

        _listaHistorialDomicilios = New List(Of HistorialDomicilios)

        For Each item_ In datasourcePillbox_.DataSource

            Dim domicilioAux_ As New DomiciliosTaxid

            With domicilioAux_

                _auxiliarDestinatario._taxid = item_("icTaxid")

                .clavetaxid = item_("icCveTaxid")

                .taxid = item_("icTaxid")

                If item_("icIdDomicilio") <> "" Then

                    ._iddomicilio = New MongoDB.Bson.ObjectId(item_("icIdDomicilio").ToString)

                End If

                If item_("icSecDomicilio") <> "" Then

                    .sec = Integer.Parse(item_("icSecDomicilio"))

                End If

                .calle = item_("icCalle")

                .numeroexterior = item_("icNumeroExterior")

                .numerointerior = item_("icNumeroInterior")

                .colonia = item_("icColonia")

                .codigopostal = item_("icCodigoPostal")

                .ciudad = item_("icCiudad")

                .localidad = item_("icLocalidad")

                .municipio = item_("icMunicipio")

                .entidadfederativa = item_("icEntidadFederativa")

                .pais = item_("icPais")

                .domicilioPresentacion = item_("scDomicilio")

                .cveEntidadfederativa = item_("icCveEntidadFederativa")

                .cveMunicipio = item_("icCveMunicipio")

                .cvePais = item_("icCvePais")

                .idpais = item_("icIdPais")

                If item_("icestadoproveedor") <> "" Then

                    .estado = Integer.Parse(item_("icestadoproveedor"))

                End If

                .archivado = item_("archivado")

            End With

            _auxiliarDestinatario._listadomiciliosconTaxid.Add(domicilioAux_)

        Next

        Return _auxiliarDestinatario

    End Function

    Public Function ObtenerDatosProveedorDesdePillbox(ByVal fcRazonSocial_ As String,
                                             ByVal datasourcePillbox_ As PillboxControl) As AuxiliarProveedor

        _auxiliarProveedorComprador = New AuxiliarProveedor

        With _auxiliarProveedorComprador

            ._razonsocial = fcRazonSocial_

            ._listadomiciliosconTaxid = New List(Of DomiciliosTaxid)

        End With

        _listaHistorialDomicilios = New List(Of HistorialDomicilios)

        For Each item_ In datasourcePillbox_.DataSource

            Dim domicilioAux_ As New DomiciliosTaxid

            With domicilioAux_

                If item_.ContainsKey("icTaxid") AndAlso item_("icTaxid") IsNot Nothing Then

                    _auxiliarProveedorComprador._taxid = item_("icTaxid")

                    .clavetaxid = item_("icCveTaxid")

                    .taxid = item_("icTaxid")

                End If

                If item_("icIdDomicilio") <> "" Then

                    ._iddomicilio = New MongoDB.Bson.ObjectId(item_("icIdDomicilio").ToString)

                End If

                If item_("icSecDomicilio") <> "" Then

                    .sec = Integer.Parse(item_("icSecDomicilio"))

                End If

                .calle = item_("icCalle")

                .numeroexterior = item_("icNumeroExterior")

                .numerointerior = item_("icNumeroInterior")

                .colonia = item_("icColonia")

                .codigopostal = item_("icCodigoPostal")

                .ciudad = item_("icCiudad")

                .localidad = item_("icLocalidad")

                .municipio = item_("icMunicipio")

                .entidadfederativa = item_("icEntidadFederativa")

                .pais = item_("icPais")

                .domicilioPresentacion = item_("scDomicilio")

                .cveEntidadfederativa = item_("icCveEntidadFederativa")

                .cveMunicipio = item_("icCveMunicipio")

                .cvePais = item_("icCvePais")

                .idpais = item_("icIdPais")

                If item_("icestadoproveedor") <> "" Then

                    .estado = Integer.Parse(item_("icestadoproveedor"))

                End If

                .archivado = item_("archivado")

            End With

            _auxiliarProveedorComprador._listadomiciliosconTaxid.Add(domicilioAux_)

        Next

        Return _auxiliarProveedorComprador

    End Function

#End Region
#Region "Generador de secuencias"
    Public Function GenerarSecuencia(Optional ByVal tiposecuencia_ _
                                     As SecuenciasComercioExterior = SecuenciasComercioExterior.ProveedoresOperativos) _
                                     As ISecuencia

        _estado = New TagWatcher

        _secuencia = New Secuencia

        _controladorSecuencias = New ControladorSecuencia

        _estado = _controladorSecuencias.Generar(tiposecuencia_.ToString, 1, 1, 1, 1, 1)

        If _estado.Status = TypeStatus.Ok Then

            _secuencia = DirectCast(_estado.ObjectReturned, Secuencia)

            _secuencia.nombre = tiposecuencia_.ToString
        End If

        Return _secuencia

    End Function

#End Region
#Region "EXTRAER DATOS DE COMPARACION"
    ''' <summary>
    ''' Extrae todos los TaxIDs activos de una empresa con su información completa
    ''' </summary>
    ''' <param name="empresa">Objeto EmpresaInternacional</param>
    ''' <returns>Lista de objetos TaxIdInfo con IdTaxId y TaxIdValue</returns>
    Public Function ExtraerTaxIds(empresa As EmpresaInternacional) As List(Of TaxId)

        If empresa Is Nothing OrElse empresa.taxids Is Nothing Then
            Return New List(Of TaxId)()
        End If

        ' Usar LINQ para extraer TaxIDs activos con su información completa
        Dim taxIds As List(Of TaxId) = empresa.taxids _
            .Where(Function(t) t.estado = 1 AndAlso Not t.archivado) _
            .Select(Function(t) New TaxId(
                t.idtaxid.ToString,
t.taxid
            )) _
            .ToList()

        Return taxIds

    End Function

    ''' <summary>
    ''' Extrae solo los valores de TaxID como strings (función original)
    ''' </summary>
    ''' <param name="empresa">Objeto EmpresaInternacional</param>
    ''' <returns>Lista de strings con los TaxIDs</returns>
    Public Function ExtraerTaxIdsSimple(empresa As EmpresaInternacional) As List(Of TaxId)
        If empresa Is Nothing OrElse empresa.taxids Is Nothing Then
            Return New List(Of TaxId)()
        End If

        ' Usar LINQ para extraer solo los TaxIDs activos y no archivados
        Dim taxIds As List(Of TaxId) = empresa.taxids.
                                        Where(Function(t) t.estado = 1 AndAlso Not t.archivado).
                                        Select(Function(t) t.taxid).
                                        AsEnumerable

        Return taxIds

    End Function

    ''' <summary>
    ''' Extrae TaxIDs como tuplas (IdTaxId, TaxIdValue)
    ''' </summary>
    ''' <param name="empresa">Objeto EmpresaInternacional</param>
    ''' <returns>Lista de tuplas con (IdTaxId, TaxIdValue)</returns>
    Public Function ExtraerTaxIdsTupla(empresa As EmpresaInternacional) As List(Of TaxId)
        If empresa Is Nothing OrElse empresa.taxids Is Nothing Then
            Return New List(Of TaxId)()
        End If

        ' Usar LINQ para extraer TaxIDs como tuplas
        Dim taxIds As List(Of TaxId) = empresa.taxids _
.Where(Function(t) t.estado = 1 AndAlso Not t.archivado) _
            .Select(Function(t) (
                IdTaxId:=t.idtaxid,
                TaxIdValue:=t.taxid
            )).AsEnumerable

        Return taxIds

    End Function

    ''' <summary>
    ''' Verifica si un TaxID existe en la empresa usando el ObjectId del TaxID
    ''' </summary>
    ''' <param name="empresa">Objeto EmpresaInternacional</param>
    ''' <param name="idTaxId">ObjectId del TaxID a buscar</param>
    ''' <returns>True si existe, False si no existe</returns>
    Public Function ExisteTaxId(empresa As EmpresaInternacional,
                                idTaxId_ As ObjectId) As Boolean

        If empresa Is Nothing Then
            Return False
        End If

        ' Usar LINQ para verificar si existe el TaxID con el ObjectId especificado
        Return empresa.taxids.Any(Function(t) t.idtaxid.Equals(idTaxId_))

    End Function

    ''' <summary>
    ''' Verifica si un TaxID existe en la empresa usando el ObjectId del TaxID
    ''' </summary>
    ''' <param name="empresa">Objeto EmpresaInternacional</param>
    ''' <param name="idTaxId">ObjectId del TaxID a buscar</param>
    ''' <returns>True si existe, False si no existe</returns>
    Public Function ExisteTaxEnActuales(empresa As EmpresaInternacional,
                                        taxId_ As String) As Boolean

        If empresa Is Nothing OrElse empresa.taxids Is Nothing OrElse taxId_ Is Nothing Then
            Return False
        End If

        ' Usar LINQ para verificar si existe el TaxID con el ObjectId especificado
        Return empresa.taxids.Any(Function(t) t.taxid.Equals(taxId_))

    End Function

    ''' <summary>
    ''' Extrae solo las direcciones como string de un país específico
    ''' </summary>
    ''' <param name="empresa_">Objeto EmpresaInternacional</param>
    ''' <param name="codigoPais_">Código del país</param>
    ''' <returns>Lista de strings con las direcciones completas</returns>
    Public Function ExtraerDomiciliosPorPais(empresa_ As IEmpresa,
                                              codigoPais_ As ObjectId) As List(Of Domicilio)

        If empresa_ Is Nothing OrElse empresa_.paisesdomicilios Is Nothing Then

            Return New List(Of Domicilio)()

        End If

        ' Usar LINQ para extraer solo las presentaciones de domicilio
        Dim domicilios_ As List(Of Domicilio) = empresa_.paisesdomicilios _
.Where(Function(p) p.idpais.Equals(codigoPais_)) _
.SelectMany(Function(p) p.domicilios) _
            .Where(Function(d) d.estado = 1 AndAlso Not d.archivado).ToList

        Return domicilios_

    End Function

    Public Function ObtenerDomicilioEnPais(empresa_ As IEmpresa,
                                           idPais_ As ObjectId, idDomicilio_ As ObjectId) As Domicilio

        Dim domicilio_ As New Domicilio

        If empresa_ Is Nothing OrElse empresa_.paisesdomicilios Is Nothing Then

            Return domicilio_

        End If

        Dim domiciliosPorPais_ As List(Of Domicilio) = ExtraerDomiciliosPorPais(empresa_, idPais_)

        For Each domicilioItem_ In domiciliosPorPais_

            If domicilioItem_._iddomicilio.ToString = idDomicilio_.ToString Then

                With domicilio_
                    ._iddomicilio = domicilioItem_._iddomicilio
                    .calle = domicilioItem_.calle
                    .ciudad = domicilioItem_.ciudad
                    .codigopostal = domicilioItem_.codigopostal
                    .colonia = domicilioItem_.colonia
                    .cveEntidadfederativa = domicilioItem_.cveEntidadfederativa
                    .cveMunicipio = domicilioItem_.cveMunicipio
                    .domicilioPresentacion = domicilioItem_.domicilioPresentacion
                    .entidadfederativa = domicilioItem_.entidadfederativa
                    .estado = domicilioItem_.estado
                    .localidad = domicilioItem_.localidad
                    .municipio = domicilioItem_.municipio
                    .numeroexterior = domicilioItem_.numeroexterior
                    .numerointerior = domicilioItem_.numerointerior
                    .sec = domicilioItem_.sec

                End With

            End If

        Next

        Return domicilio_

    End Function

    ''' <summary>
    ''' Verifica si un domicilio existe en un país específico usando ObjectIds
    ''' </summary>
    ''' <param name="empresa">Objeto EmpresaInternacional</param>
    ''' <param name="idPais_">ObjectId del país</param>
    ''' <param name="idDomicilio">ObjectId del domicilio</param>
    ''' <returns>True si existe, False si no existe</returns>
    Public Function ExistePaisEmpresa(empresa As EmpresaInternacional,
                                      idPais_ As ObjectId) As Boolean

        'If empresa Is Nothing OrElse empresa.paisesdomicilios Is Nothing OrElse
        '   Not idPais_ = ObjectId.Empty Then
        '    Return False
        'End If

        ' Usar LINQ para verificar si existe el domicilio en el país especificado
        Return empresa.paisesdomicilios.Any(Function(t) t.idpais.Equals(idPais_))

    End Function

    ''' <summary>
    ''' Verifica si un domicilio existe en un país específico usando ObjectIds
    ''' </summary>
    ''' <param name="empresa_">Objeto EmpresaInternacional</param>
    ''' <param name="idPais_">ObjectId del país</param>
    ''' <returns>True si existe, False si no existe</returns>
    Public Function ExisteDomicilioEnPais(empresa_ As EmpresaInternacional,
                                          idPais_ As ObjectId,
                                          idDomicilio_ As ObjectId) As Boolean

        ' Usar LINQ para verificar si existe el domicilio en el país especificado
        Return empresa_.paisesdomicilios _
.Where(Function(p) p.idpais.Equals(idPais_)) _
.SelectMany(Function(p) p.domicilios) _
            .Any(Function(d) d._iddomicilio.Equals(idDomicilio_))

    End Function

    Public Function ExisteDomicilioEnPaisNacional(empresa_ As EmpresaNacional,
                                                  idDomicilio_ As ObjectId) As Boolean

        ' Usar LINQ para verificar si existe el domicilio en el país especificado
        Return empresa_.paisesdomicilios _
            .SelectMany(Function(p) p.domicilios) _
.Any(Function(d) d._iddomicilio.Equals(idDomicilio_))

    End Function


    ''' <summary>
    ''' Verifica si un domicilio existe en un país específico usando ObjectIds
    ''' </summary>
    ''' <param name="empresa">Objeto EmpresaInternacional</param>
    ''' <param name="idPais_">ObjectId del país</param>
    ''' <returns>True si existe, False si no existe</returns>
    Public Function ExisteDomicilioPresentacionEnDomicilios(empresa As EmpresaInternacional,
                                                            idPais_ As ObjectId,
                                                            idDomicilio_ As ObjectId,
                                                            domicilioPresentacion_ As String) As Boolean

        ' Usar LINQ para verificar si existe el domicilio en el país especificado
        Return empresa.paisesdomicilios _
            .Where(Function(p) p.idpais.Equals(idPais_)) _
.SelectMany(Function(p) p.domicilios) _
            .Any(Function(d) d._iddomicilio.Equals(idDomicilio_) AndAlso d.domicilioPresentacion.Equals(domicilioPresentacion_))

    End Function

    Public Function ExisteDomicilioPresentacionEnDomiciliosNacionales(empresa As EmpresaNacional,
                                                            idDomicilio_ As ObjectId,
                                                            domicilioPresentacion_ As String) As Boolean

        ' Usar LINQ para verificar si existe el domicilio en el país especificado
        Return empresa.paisesdomicilios _
            .SelectMany(Function(p) p.domicilios) _
            .Any(Function(d) d._iddomicilio.Equals(idDomicilio_) AndAlso d.domicilioPresentacion.Equals(domicilioPresentacion_))

    End Function

    Public Function ListaDomiciliosPorPais(ByVal objectIdRazonSocial_ As String,
                                           ByVal objectidPais_ As String,
                                           Optional ByVal tipoempresa_ As IControladorEmpresas.TiposEmpresas = IControladorEmpresas.TiposEmpresas.Internacional) As List(Of SelectOption)

        _lista = New List(Of SelectOption)

        Dim listaDomiciliosPorPais_ As New List(Of Domicilio)

        If tipoempresa_ = IControladorEmpresas.TiposEmpresas.Internacional Then

            _estado = BuscarEmpresaPorObjectId(objectIdRazonSocial_)

        Else

            _estado = BuscarEmpresaPorObjectId(objectIdRazonSocial_, IControladorEmpresas.TiposEmpresas.Nacional)

        End If

        listaDomiciliosPorPais_ = ExtraerDomiciliosPorPais(_estado.ObjectReturned, ObjectId.Parse(objectidPais_))

        For Each item_ In listaDomiciliosPorPais_

            _lista.Add(New SelectOption With
                       {.Value = item_._iddomicilio.ToString,
                        .Text = item_.domicilioPresentacion})
        Next

        Return _lista

    End Function

#End Region
#Region "RFC"
    ''' <summary>
    ''' Verifica si un TaxID existe en la empresa usando el ObjectId del TaxID
    ''' </summary>
    ''' <param name="empresa_">Objeto EmpresaNacional</param>
    ''' <param name="idrfc_">ObjectId del rfc a buscar</param>
    ''' <returns>True si existe, False si no existe</returns>
    Public Function ExisteidRFCEmpresa(empresa_ As EmpresaNacional,
                             idrfc_ As ObjectId) As Boolean

        'If empresa_ Is Nothing OrElse Not idrfc_ = ObjectId.Empty Then
        '    Return False
        'End If

        Return empresa_.rfcs.Any(Function(t) t.idrfc.Equals(idrfc_))

    End Function

    Public Function CoincideRFCEmpresa(empresa_ As EmpresaNacional, rfc_ As String) As Boolean

        'If empresa_ Is Nothing OrElse Not idrfc_ = ObjectId.Empty Then
        '    Return False
        'End If

        Return empresa_.rfcs.Any(Function(t) t.rfc.Equals(rfc_))

    End Function
#End Region

#Region "Generemos estructura empresa nacional con RFC nuevo"
    Public Function GenerarEmpresaNacionalConEstructuraRFCNuevo(ByVal empresaNacional_ As EmpresaNacional,
                                                                ByVal rfc_ As String) As EmpresaNacional

        _controladorEmpresas = New ControladorEmpresas(New EspacioTrabajo(), IControladorEmpresas.TiposEmpresas.Nacional)

        If empresaNacional_.rfcs IsNot Nothing Then

            Dim secrfc_ = 1

            If empresaNacional_.rfcs.Any() Then

                secrfc_ = empresaNacional_.rfcs.Count + 1

            End If

            Dim rfcAux_ = New Rec.Globals.Empresas.Rfc(rfc_, sec_:=secrfc_)

            empresaNacional_._idrfc = rfcAux_.idrfc

            empresaNacional_.rfc = rfcAux_.rfc

            empresaNacional_.rfcs = New List(Of Rec.Globals.Empresas.Rfc) From {rfcAux_}

        End If
        ''POR ULTIMO MODIFICAMOS LA EMPRESA
        ''INICIALIZA EL CONTROLADOR
        _estado = _controladorEmpresas.Modificar(empresaNacional_)

        If _estado.Status = TypeStatus.Ok Then
            ''IGUAL Y LA VOY A BUSCAR PARA MANDARLA COMO ESTA EN MONGO
            'empresaInternacional_.taxids.Add(empresaInternacionalAux_.taxids.Last)
            _empresaNacional = New EmpresaNacional

            _estado = New TagWatcher

            _estado = _controladorEmpresas.ConsultarUna(empresaNacional_._id)

            If _estado.Status = TypeStatus.Ok Then

                _empresaNacional = _estado.ObjectReturned

                Return _empresaNacional

            End If

        End If

        Return _empresaNacional

    End Function
#End Region

#Region "Generemos estructura empresa nacional con CURP nueva"
    Public Function GenerarEmpresaNacionalConEstructuraCURPNueva(ByVal empresaNacional_ As EmpresaNacional,
                                                                ByVal curp_ As String) As EmpresaNacional

        _controladorEmpresas = New ControladorEmpresas(New EspacioTrabajo(), IControladorEmpresas.TiposEmpresas.Nacional)

        If empresaNacional_.curps IsNot Nothing Then

            Dim seccurp_ = 1

            If empresaNacional_.curps.Any() Then

                seccurp_ = empresaNacional_.curps.Count + 1

            End If

            Dim curpAux_ = New Rec.Globals.Empresas.Curp(curp_, sec_:=seccurp_)

            empresaNacional_._idcurp = curpAux_.idcurp

            empresaNacional_.curp = curpAux_.curp

            empresaNacional_.curps = New List(Of Rec.Globals.Empresas.Curp) From {curpAux_}

        End If
        ''POR ULTIMO MODIFICAMOS LA EMPRESA
        ''INICIALIZA EL CONTROLADOR
        _estado = _controladorEmpresas.Modificar(empresaNacional_)

        If _estado.Status = TypeStatus.Ok Then
            ''IGUAL Y LA VOY A BUSCAR PARA MANDARLA COMO ESTA EN MONGO
            'empresaInternacional_.taxids.Add(empresaInternacionalAux_.taxids.Last)
            _empresaNacional = New EmpresaNacional

            _estado = New TagWatcher

            _estado = _controladorEmpresas.ConsultarUna(empresaNacional_._id)

            If _estado.Status = TypeStatus.Ok Then

                _empresaNacional = _estado.ObjectReturned

                Return _empresaNacional

            End If

        End If

        Return _empresaNacional

    End Function
#End Region


#Region "Generar estructura empresa internacional con TAXID NUEVO"
    Public Function GenerarEmpresaInternacionalConEstructuraTaxidNuevo(ByVal empresaInternacional_ As EmpresaInternacional,
                                                                       ByVal cvepais_ As String,
                                                                       ByVal taxid_ As String) As EmpresaInternacional

        _controladorEmpresas = New ControladorEmpresas(New EspacioTrabajo(),
                                                       IControladorEmpresas.TiposEmpresas.Internacional, paisEmpresa_:=cvepais_)
        If empresaInternacional_ IsNot Nothing Then

            If empresaInternacional_.taxids IsNot Nothing Then

                Dim sectaxid_ = 1

                If empresaInternacional_.taxids.Any() Then

                    sectaxid_ = empresaInternacional_.taxids.Count + 1

                End If

                empresaInternacional_.taxids = New List(Of Rec.Globals.Empresas.TaxId) From {New Rec.Globals.Empresas.TaxId(taxid_, sec_:=sectaxid_)}

                ' empresaInternacional_.taxids.Add(New Rec.Globals.Empresas.TaxId(taxid_, sec_:=sectaxid_))

            End If

        End If
        ''POR ULTIMO MODIFICAMOS LA EMPRESA
        ''INICIALIZA EL CONTROLADOR
        _estado = _controladorEmpresas.Modificar(empresaInternacional_)

        If _estado.Status = TypeStatus.Ok Then
            ''IGUAL Y LA VOY A BUSCAR PARA MANDARLA COMO ESTA EN MONGO
            'empresaInternacional_.taxids.Add(empresaInternacionalAux_.taxids.Last)
            _empresaInternacional = New EmpresaInternacional

            _estado = New TagWatcher

            _estado = _controladorEmpresas.ConsultarUna(empresaInternacional_._id)

            If _estado.Status = TypeStatus.Ok Then

                _empresaInternacional = _estado.ObjectReturned

                Return _empresaInternacional

            End If

        End If

        Return empresaInternacional_

    End Function
#End Region

#Region "Generar estructura empresa internacional con PAIS Y DOMICILIO NUEVO"
    Public Function GenerarEmpresaInternacionalConDomicilioNuevo(ByVal empresaInternacional_ As EmpresaInternacional,
                                                                 ByVal idpais_ As ObjectId,
                                                                 ByVal cvepais_ As String,
                                                                 ByVal pais_ As String,
                                                                 ByVal domicilio_ As Domicilio, Optional ByVal existepais_ As Boolean = True) As EmpresaInternacional

        _controladorEmpresas = New ControladorEmpresas(New EspacioTrabajo(),
                                                       IControladorEmpresas.TiposEmpresas.Internacional, paisEmpresa_:=cvepais_)

        ''VERIFICAR SI LA EMPRESA YA TIENE EL PAIS
        Dim domicilioPresentacion_ = Nothing

        With domicilio_

            domicilioPresentacion_ = $"{ .calle} { .numeroexterior} - INT { .numerointerior} CP { .codigopostal} { .colonia} { .localidad} { .municipio} { .ciudad} { .entidadfederativa} {pais_}"

        End With

        domicilio_.domicilioPresentacion = domicilioPresentacion_

        If existepais_ Then
            ''INSERTAMOS EL DOMICILIO DIRECTAMENTE AL NODO DE LOS PAISES
            empresaInternacional_.paisesdomicilios.Where(Function(x) x.idpais.
                                                    Equals(idpais_)).
                                                    ToList.
                                                    ForEach(Sub(item_) item_.domicilios.
                                                    Add(domicilio_))

        Else
            '    ''CREAMOS EL NODO DE LOS PAISES

            empresaInternacional_.paisesdomicilios = New List(Of PaisDomicilio)()

            empresaInternacional_.paisesdomicilios.Add(New PaisDomicilio With {.idpais = idpais_,
                                                       .pais = cvepais_,
                                                       .paisPresentacion = pais_,
                                                       .domicilios = New List(Of Domicilio)()})

            '''LUEGO INSERTAMOS ESE DOMICILIO EN ESE NODO
            empresaInternacional_.paisesdomicilios.Where(Function(x) x.idpais.
                                                    Equals(idpais_)).
                                                    ToList.
                                                    ForEach(Sub(item_) item_.domicilios.
                                                    Add(domicilio_))

        End If

        ''POR ULTIMO MODIFICAMOS LA EMPRESA
        ''INICIALIZA EL CONTROLADOR
        _estado = _controladorEmpresas.Modificar(empresaInternacional_)

        If _estado.Status = TypeStatus.Ok Then
            ''IGUAL Y LA VOY A BUSCAR PARA MANDARLA COMO ESTA EN MONGO
            _empresaInternacional = New EmpresaInternacional

            _estado = New TagWatcher

            _estado = _controladorEmpresas.ConsultarUna(empresaInternacional_._id)

            If _estado.Status = TypeStatus.Ok Then

                _empresaInternacional = _estado.ObjectReturned

                Return _empresaInternacional

            End If

        End If

        Return empresaInternacional_

    End Function

#End Region

#Region "Genera estructura empresa nacional con DOMICILIO NUEVO"
    Public Function GenerarDomicilioPresentacion(ByVal domicilio_ As Domicilio,
                                                 ByVal pais_ As String) As String
        Dim domicilioPresentacion_ = Nothing

        With domicilio_

            domicilioPresentacion_ = $"{ .calle} { .numeroexterior} - INT { .numerointerior} CP { .codigopostal} { .colonia} { .localidad} { .municipio} { .ciudad} { .entidadfederativa} {pais_}"

        End With

        Return domicilioPresentacion_

    End Function

    Public Function GenerarEmpresaNacionalConDomicilioNuevo(ByVal empresaNacional_ As EmpresaNacional,
                                                            ByVal domicilio_ As Domicilio, ByVal pais_ As String) As EmpresaNacional

        _controladorEmpresas = New ControladorEmpresas(New EspacioTrabajo(), IControladorEmpresas.TiposEmpresas.Nacional)

        ''VERIFICAR SI LA EMPRESA YA TIENE EL PAIS
        Dim domicilioPresentacion_ = Nothing

        With domicilio_

            domicilioPresentacion_ = $"{ .calle} { .numeroexterior} - INT { .numerointerior} CP { .codigopostal} { .colonia} { .localidad} { .municipio} { .ciudad} { .entidadfederativa} {pais_}"

        End With

        Dim secdomicilioactual_ = empresaNacional_.paisesdomicilios.LastOrDefault.domicilios.Count + 1

        domicilio_.sec = secdomicilioactual_

        domicilio_.domicilioPresentacion = domicilioPresentacion_

        empresaNacional_.paisesdomicilios.LastOrDefault.domicilios.Add(domicilio_)

        ''INICIALIZA EL CONTROLADOR
        _estado = _controladorEmpresas.Modificar(empresaNacional_)

        If _estado.Status = TypeStatus.Ok Then
            ''IGUAL Y LA VOY A BUSCAR PARA MANDARLA COMO ESTA EN MONGO
            _empresaNacional = New EmpresaNacional

            _estado = New TagWatcher

            _estado = _controladorEmpresas.ConsultarUna(empresaNacional_._id)

            If _estado.Status = TypeStatus.Ok Then

                _empresaNacional = _estado.ObjectReturned

                Return _empresaNacional

            End If

        End If

        Return empresaNacional_

    End Function
#End Region


#Region "Obtener Datos del destinatario por el controlador"
    Public Function ObtenerDatosDestinatarioDesdeControlador(ByVal objectiddestinatario_ As ObjectId) As AuxiliarDestinatario

        _auxiliarDestinatario = New AuxiliarDestinatario

        _estado = New TagWatcher

        _controladorProveedores = New CtrlProveedoresOperativos()

        _estado = _controladorProveedores.ObtenerDatosDestinatarioPorObjectId(objectiddestinatario_)

        If _estado.Status = TypeStatus.Ok Then

            _auxiliarDestinatario = DirectCast(_estado.ObjectReturned, AuxiliarDestinatario)

        End If

        Return _auxiliarDestinatario

    End Function

#End Region

#Region "Obtener Datos del proveedor por el controlador"
    Public Function ObtenerDatosProveedorDesdeControlador(ByVal objectidproveedor_ As ObjectId) As AuxiliarProveedor

        _auxiliarProveedorComprador = New AuxiliarProveedor

        _estado = New TagWatcher

        _controladorProveedores = New CtrlProveedoresOperativos()

        _estado = _controladorProveedores.ObtenerDatosProveedorPorObjectId(objectidproveedor_)

        If _estado.Status = TypeStatus.Ok Then

            _auxiliarProveedorComprador = DirectCast(_estado.ObjectReturned, AuxiliarProveedor)

        End If

        Return _auxiliarProveedorComprador

    End Function

#End Region

#Region "EntidadFederativa"
    Public Function ObtenerListaEntidadesFederativas(ByVal ObjectIdPais_ As ObjectId,
                                                     Optional ByVal entidadFederativa_ As String = Nothing) As List(Of SelectOption)

        _lista = New List(Of SelectOption)

        _estado = New TagWatcher

        If entidadFederativa_ Is Nothing Then

            _estado = ControladorPaises.ObtenerEntidadesFederativasPorPais(ObjectIdPais_)

        Else

            _estado = ControladorPaises.ObtenerListaEntidadesFederativas(ObjectIdPais_, entidadFederativa_)

        End If

        If _estado.Status = TypeStatus.Ok Then

            If _estado.ObjectReturned.count > 0 Then

                Dim listaEntidadesFederativas_ = DirectCast(_estado.ObjectReturned, List(Of EntidadFederativa))

                For Each item_ In listaEntidadesFederativas_

                    _lista.Add(New SelectOption _
                               With {
                                    .Value = item_.abreviatura,
                                    .Text = $"{item_.abreviatura} - {item_.entidadfederativa}"})
                Next

            End If

        End If

        Return _lista

    End Function
#End Region

#End Region
End Class