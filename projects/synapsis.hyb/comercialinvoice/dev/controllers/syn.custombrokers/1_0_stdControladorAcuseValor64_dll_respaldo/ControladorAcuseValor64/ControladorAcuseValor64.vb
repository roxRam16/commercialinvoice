
Imports System.IO
Imports System.Net
Imports System.Security.AccessControl
Imports System.Security.Cryptography
Imports System.Security.Cryptography.Pkcs
Imports System.Security.Cryptography.X509Certificates
Imports System.Security.Cryptography.Xml
Imports System.Security.Policy
Imports System.ServiceModel
Imports System.ServiceModel.Channels
Imports System.ServiceModel.Configuration
Imports System.ServiceModel.Description
Imports System.ServiceModel.Dispatcher
Imports System.ServiceModel.Security.Tokens
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Xml.Xsl
Imports gsol.krom
Imports iText.IO.Image
Imports iText.Kernel.Colors
Imports iText.Kernel.Font
Imports iText.Kernel.Geom
Imports iText.Kernel.Pdf
Imports iText.Kernel.Pdf.Canvas
Imports iText.Layout
Imports iText.Layout.Element
Imports iText.Layout.Font
Imports iText.Layout.Properties
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports MS.Internal.Xml
Imports Org.BouncyCastle.Asn1.Nist
Imports Org.BouncyCastle.Asn1.Pkcs
Imports Org.BouncyCastle.Asn1.X509 ' Para DefaultDigestAlgorithmIdentifierFinder
Imports Org.BouncyCastle.Bcpg
Imports Org.BouncyCastle.Cms
Imports Org.BouncyCastle.Crypto
Imports Org.BouncyCastle.Crypto.Digests
Imports Org.BouncyCastle.Crypto.Parameters
Imports Org.BouncyCastle.Crypto.Signers
Imports Org.BouncyCastle.OpenSsl ' Para lectura de Pkcs8 EncryptedPrivateKeyInfo
Imports Org.BouncyCastle.Operators
Imports Org.BouncyCastle.Pkcs
Imports Org.BouncyCastle.Security
Imports Org.BouncyCastle.Utilities
Imports Org.BouncyCastle.Utilities.IO.Pem
Imports Org.BouncyCastle.X509
Imports Org.BouncyCastle.X509.Store
Imports Rpt.Global
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Utils
Imports VUCEM
Imports Wma.Exceptions



Public Class ControladorAcuseValor
    Implements IControladorAcuseValor, ICloneable, IDisposable

#Region "Enums"

    Public Enum TipoOperacion

        Importacion = 1

        Exportacion = 2

    End Enum



#End Region

#Region "Atributos"

    Private _organismo As New Organismo

    Private _acusesValorGenerados As List(Of ConstructorAcuseValor)

    Private _documentos As List(Of DocumentoElectronico)

    Private _tipoOperacion As TipoOperacion

    Private _bulkCamposPedidos As Dictionary(Of ObjectId, List(Of Nodo))

    Private _estado As TagWatcher

    Private _ivucemActions As IVUCEMActions

    Private _itextHandler As ItextHandler

#End Region

#Region "Propiedades"

    Public Property AcusesValorGenerados As List(Of ConstructorAcuseValor) _
                        Implements IControladorAcuseValor.AcusesValorGenerados
        Get

            Return _acusesValorGenerados

        End Get

        Set(value As List(Of ConstructorAcuseValor))

            _acusesValorGenerados = value

        End Set

    End Property


    Public Property BulkCamposPedidos As Dictionary(Of ObjectId, List(Of Nodo)) _
        Implements IControladorAcuseValor.BulkCamposPedidos
        Get

            Return _bulkCamposPedidos

        End Get

        Set(value As Dictionary(Of ObjectId, List(Of Nodo)))

            _bulkCamposPedidos = value

        End Set

    End Property

    Public Property Estado As TagWatcher _
        Implements IControladorAcuseValor.Estado
        Get

            Return _estado

        End Get

        Set(value As TagWatcher)

            _estado = value

        End Set

    End Property



#End Region

#Region "Constructores"

    Sub New()

        _acusesValorGenerados = New List(Of ConstructorAcuseValor)

        _estado = New TagWatcher

    End Sub

#End Region


#Region "Métodos"

    Public Sub ActulizarCertificado(certPath_ As String,
                                    keyPath_ As String,
                                    userName_ As String,
                                    certPassword_ As String,
                                    passwordWSSE_ As String)

        _ivucemActions = New VUCEMActions(userName_,
                                               certPassword_,
                                               passwordWSSE_,
                                               certPath_,
                                               keyPath_)

    End Sub

    Public Sub ActulizarCertificado(certBytes_ As Byte(),
                                    keyBytes_ As Byte(),
                                    userName_ As String,
                                    certPassword_ As String,
                                    passwordWSSE_ As String)

        _ivucemActions = New VUCEMActions(userName_,
                                               certPassword_,
                                               passwordWSSE_,
                                               certBytes_,
                                               keyBytes_)

    End Sub

    Public Function ConsultaAcusesValor(idCoves_ As List(Of ObjectId),
                    Optional campos_ As Dictionary(Of [Enum], List(Of [Enum])) = Nothing) As TagWatcher _
                    Implements IControladorAcuseValor.ConsultaAcusesValor


        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos(21) With
            {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

            If campos_ Is Nothing Then

                _acusesValorGenerados = New List(Of ConstructorAcuseValor)

                Dim operationsDB_ As IMongoCollection(Of OperacionGenerica) =
                    _enlaceDatos.GetMongoCollection(Of OperacionGenerica)(GetType(ConstructorAcuseValor).Name)

                operationsDB_.Aggregate().Project(Function(r) New With {
                                                        Key .ids = r.Id,
                                                        Key .documento = r.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente
                                                      }).
                                                      Match(Function(chi) idCoves_.Contains(chi.ids)).
                                                      ToList().ForEach(Sub(items)
                                                                           items.documento.Id = items.ids.ToString
                                                                           _acusesValorGenerados.Add(New ConstructorAcuseValor(True, items.documento))
                                                                       End Sub)

                _estado.ObjectReturned = _acusesValorGenerados

            Else

                _bulkCamposPedidos = _organismo.ObtenerCamposSeccionExterior(idCoves_, New ConstructorAcuseValor, campos_)

                _estado.ObjectReturned = _bulkCamposPedidos

            End If

        End Using

        _estado.SetOK()

        Return _estado

    End Function

    Public Function ConsultaAcuseValor(idAcuseValor_ As ObjectId,
                                 Optional campos_ As Dictionary(Of [Enum], List(Of [Enum])) = Nothing) As TagWatcher _
                                 Implements IControladorAcuseValor.ConsultaAcuseValor

        _estado = New TagWatcher

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos(21)

            If campos_ Is Nothing Then

                Dim consulta_ As String = ""

                _acusesValorGenerados = New List(Of ConstructorAcuseValor)

                Dim operationsDB_ As IMongoCollection(Of OperacionGenerica) = _enlaceDatos.GetMongoCollection(Of OperacionGenerica)(GetType(ConstructorAcuseValor).Name)

                operationsDB_.Aggregate().Project(Function(r) New With {
                                                        Key .ids = r.Id,
                                                        Key .documento = r.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente
                                                      }).
                                                      Match(Function(ch) ch.ids = idAcuseValor_).
                                                      ToList().ForEach(Sub(items)

                                                                           items.documento.Id = items.ids.ToString

                                                                           _acusesValorGenerados.Add(items.documento)

                                                                       End Sub)

                _estado.ObjectReturned = _acusesValorGenerados

            Else

                _bulkCamposPedidos = _organismo.ObtenerCamposSeccionExterior(New List(Of ObjectId) From {idAcuseValor_}, New ConstructorAcuseValor, campos_)

                _estado.ObjectReturned = _bulkCamposPedidos

            End If

        End Using

        _estado.SetOK()

        Return _estado

    End Function

    Public Function filtracomparacion(sTaxId_ As String, sRFC As String, stoken_ As String) As Boolean

        If sTaxId_ = "" Then

            If sRFC = stoken_ Then

                Return True

            Else

                Return False

            End If

        Else

            If sTaxId_ = stoken_ Then

                Return True

            Else

                Return False

            End If

        End If

    End Function

    Public Function GenerarAcuseValor(constructorAcuseValor_ As ConstructorAcuseValor,
                                      certPath_ As String,
                                      keyPath_ As String,
                                      userName_ As String,
                                      certPassword_ As String,
                                      webServicePassoword_ As String,
                                      Optional adendar_ As Boolean = False) As TagWatcher _
                                      Implements IControladorAcuseValor.GenerarAcuseValor

        Dim acuseValor_ As String = EnvioCOVE(constructorAcuseValor_,
                                              certPath_,
                                              keyPath_,
                                              userName_,
                                              certPassword_,
                                              webServicePassoword_)



        _estado = New TagWatcher

        If acuseValor_.Contains("COVE") Then

            With _estado

                .SetOK()

                .ObjectReturned = acuseValor_

            End With

            Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos(21) With
            {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

                Dim operationsDB_ As IMongoCollection(Of OperacionGenerica) = _enlaceDatos.
                                                                              GetMongoCollection(Of OperacionGenerica)(constructorAcuseValor_.
                                                                                                                       GetType.
                                                                                                                       Name)

                Dim ruta_ = _organismo.ObtenerRutaCampo(constructorAcuseValor_,
                                                        SeccionesAcuseValor.SAcuseValor1,
                                                        CamposAcuseValor.CA_NUMERO_ACUSEVALOR)

                ruta_ = ruta_.Substring(0, ruta_.Length - 2)

                Dim puntosNumeroCove_ = "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts." &
                                         ruta_.
                                         Replace("(", ".").
                                         Replace(")", "") &
                                         ".Valor"

                Dim puntosNumeroCove2_ = "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts." &
                                          ruta_.
                                          Replace("(", ".").
                                          Replace(")", "") &
                                          ".ValorPresentacion"

                ruta_ = _organismo.ObtenerRutaCampo(constructorAcuseValor_,
                                                    SeccionesAcuseValor.SAcuseValor1,
                                                    CamposAcuseValor.CA_FECHA_ACUSEVALOR)

                ruta_ = ruta_.Substring(0,
                                        ruta_.Length - 2)

                Dim puntosFechaCove_ = "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts." &
                                        ruta_.
                                        Replace("(", ".").
                                        Replace(")", "") &
                                        ".Valor"

                '' Crear el objeto de actualización
                Dim fechaaux_ = DateTime.Now

                Dim update_ As BsonDocument = IIf(Not adendar_,
                                             BsonDocument.Parse("{$set:{'" & puntosNumeroCove_ & "':'" & acuseValor_ &
                                                                    "', '" & puntosFechaCove_ & "':ISODate('" &
                                                                    fechaaux_.ToString("yyyy-MM-ddTHH:mm:ss.00Z") & "')}}"),
                                             BsonDocument.Parse("{$set:{'" & puntosNumeroCove_ & "':'" & acuseValor_ &
                                                                "','" & puntosNumeroCove2_ & "':'" &
                                                                constructorAcuseValor_.Seccion(SeccionesAcuseValor.SAcuseValor1).
                                                                Attribute(CamposAcuseValor.CA_NUMERO_ACUSEVALOR).Valor &
                                                                "', '" & puntosFechaCove_ & "':ISODate('" &
                                                                (New DateTime).ToString("yyyy-MM-ddTHH:mm:ss.00Z") & "')}}"))


                '' Realizar la actualización
                Dim acuseValorId_ = New ObjectId(constructorAcuseValor_.Id)

                constructorAcuseValor_.Seccion(SeccionesAcuseValor.SAcuseValor1).Campo(CamposAcuseValor.CA_FECHA_ACUSEVALOR).Valor = fechaaux_

                operationsDB_.UpdateOne(Function(e) e.Id = acuseValorId_, update_)

                ' Dim algo_ = Await operationsDB_.UpdateOneAsync(Function(e) e.Id = acuseValorId_, update_).Result.(False).

            End Using

            Dim controladorFacturaComercial_ As New ControladorFacturaComercial(1, True)

            Dim facturasDisponible_ As TagWatcher = controladorFacturaComercial_.
                                                    ListaFacturas(constructorAcuseValor_.
                                                                  Seccion(SeccionesAcuseValor.SAcuseValor1).
                                                                  Attribute(CamposAcuseValor.CP_ID_FACTURA_ACUSEVALOR).
                                                                  Valor)

            _acusesValorGenerados = New List(Of ConstructorAcuseValor) From {constructorAcuseValor_}

            If facturasDisponible_.ObjectReturned.COunt > 0 Then

                controladorFacturaComercial_.
                ActualizarDatosAcuseValor(constructorAcuseValor_.
                                          Seccion(SeccionesAcuseValor.SAcuseValor1).
                                          Attribute(CamposAcuseValor.CP_ID_FACTURA_ACUSEVALOR).
                                          Valor,
                                          New Dictionary(Of [Enum], String) From {{CamposAcuseValor.CP_ID_ACUSEVALOR,
                                                                                   acuseValor_},
                                                                                   {CamposAcuseValor.CA_FECHA_ACUSEVALOR,
                                                                                   constructorAcuseValor_.
                                                                                   Seccion(SeccionesAcuseValor.SAcuseValor1).
                                                                                   Attribute(CamposAcuseValor.CA_FECHA_ACUSEVALOR).
                                                                                   Valor}})

            End If

        Else

            With _estado

                .SetError(acuseValor_)

                .ObjectReturned = acuseValor_

            End With

        End If

        Return _estado

    End Function

    Public Function GenerarAcuseValor(constructorAcuseValor_ As ConstructorAcuseValor,
                                      certBytes_ As Byte(),
                                      keyBytes_ As Byte(),
                                      userName_ As String,
                                      certPassword_ As String,
                                      webServicePassoword_ As String,
                                      Optional adendar_ As Boolean = False) As TagWatcher _
                                      Implements IControladorAcuseValor.GenerarAcuseValor

        Dim acuseValor_ As String = EnvioCOVE(constructorAcuseValor_,
                                              certBytes_,
                                              keyBytes_,
                                              userName_,
                                              certPassword_,
                                              webServicePassoword_)

        'Dim acuseValor_ As String = "COVE257RTQWB2"

        _estado = New TagWatcher

        If acuseValor_.Contains("COVE") Then

            With _estado

                .SetOK()

                .ObjectReturned = acuseValor_

            End With

            Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos(21) With
            {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

                Dim operationsDB_ As IMongoCollection(Of OperacionGenerica) = _enlaceDatos.
                                                                              GetMongoCollection(Of OperacionGenerica)(constructorAcuseValor_.
                                                                                                                       GetType.Name)

                Dim ruta_ = _organismo.ObtenerRutaCampo(constructorAcuseValor_,
                                                    SeccionesAcuseValor.SAcuseValor1,
                                                    CamposAcuseValor.CA_NUMERO_ACUSEVALOR)

                ruta_ = ruta_.Substring(0, ruta_.Length - 2)

                Dim puntosNumeroCove_ = "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts." &
                                        ruta_.Replace("(", ".").Replace(")", "") &
                                        ".Valor"

                Dim puntosNumeroCove2_ = "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts." &
                                         ruta_.Replace("(", ".").Replace(")", "") &
                                         ".ValorPresentacion"

                ruta_ = _organismo.ObtenerRutaCampo(constructorAcuseValor_,
                                                    SeccionesAcuseValor.SAcuseValor1,
                                                    CamposAcuseValor.CA_FECHA_ACUSEVALOR)

                ruta_ = ruta_.Substring(0,
                                        ruta_.Length - 2)

                Dim puntosFechaCove_ = "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts." &
                                       ruta_.Replace("(", ".").Replace(")", "") &
                                       ".Valor"

                '' Crear el objeto de actualización
                Dim fechaaux_ = DateTime.Now

                Dim update_ As BsonDocument = IIf(Not adendar_,
                                                  BsonDocument.Parse("{$set:{'" &
                                                                     puntosNumeroCove_ &
                                                                     "':'" &
                                                                     acuseValor_ &
                                                                     "', '" &
                                                                     puntosFechaCove_ &
                                                                     "':ISODate('" &
                                                                    fechaaux_.ToString("yyyy-MM-ddTHH:mm:ss.00Z") &
                                                                    "')}}"),
                                                  BsonDocument.Parse("{$set:{'" &
                                                                     puntosNumeroCove_ &
                                                                     "':'" &
                                                                     acuseValor_ &
                                                                     "','" &
                                                                     puntosNumeroCove2_ &
                                                                     "':'" &
                                                                     constructorAcuseValor_.Seccion(SeccionesAcuseValor.SAcuseValor1).
                                                                     Attribute(CamposAcuseValor.CA_NUMERO_ACUSEVALOR).Valor &
                                                                     "', '" &
                                                                     puntosFechaCove_ &
                                                                     "':ISODate('" &
                                                                     (New DateTime).ToString("yyyy-MM-ddTHH:mm:ss.00Z") &
                                                                     "')}}"))


                '' Realizar la actualización
                Dim acuseValorId_ = New ObjectId(constructorAcuseValor_.Id)

                constructorAcuseValor_.Seccion(SeccionesAcuseValor.SAcuseValor1).Campo(CamposAcuseValor.CA_FECHA_ACUSEVALOR).Valor = fechaaux_

                operationsDB_.UpdateOne(Function(e) e.Id = acuseValorId_, update_)

            End Using

            Dim controladorFacturaComercial_ As New ControladorFacturaComercial(1, True)

            Dim idFactura_ As ObjectId = constructorAcuseValor_.
                                             Seccion(SeccionesAcuseValor.SAcuseValor1).
                                             Attribute(CamposAcuseValor.CP_ID_FACTURA_ACUSEVALOR).Valor

            Dim facturasDisponible_ As TagWatcher = controladorFacturaComercial_.ListaFacturas(ObjectId.Parse("68c47768663de50dfa669fcb"))

            _acusesValorGenerados = New List(Of ConstructorAcuseValor) From {constructorAcuseValor_}

            If facturasDisponible_.ObjectReturned.COunt > 0 Then



                Dim idAcuseValor_ As String = constructorAcuseValor_.Id

                Dim fechaAcuseValor_ As String = Date.Parse(constructorAcuseValor_.Seccion(SeccionesAcuseValor.SAcuseValor1).Attribute(CamposAcuseValor.CA_FECHA_ACUSEVALOR).Valor).ToString("yyyy-MM-dd")

                controladorFacturaComercial_.ActualizarDatosAcuseValor(idFactura_,
                                                                       New Dictionary(Of [Enum], String) From {{CamposAcuseValor.CP_ID_ACUSEVALOR, idAcuseValor_},
                                                                                                               {CamposAcuseValor.CA_NUMERO_ACUSEVALOR, acuseValor_},
                                                                                                               {CamposAcuseValor.CA_FECHA_ACUSEVALOR, fechaAcuseValor_}})

            End If

        Else

            With _estado

                .SetError(acuseValor_)

                .ObjectReturned = acuseValor_

            End With

        End If

        Return _estado

    End Function

    Public Sub Dispose() Implements IDisposable.Dispose

        Throw New NotImplementedException()

    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone

        Throw New NotImplementedException()

    End Function



    'Public Function ChecaFormatos(ByVal format_ As InputControl.InputFormat, ByVal valorAsignado_ As String)

    '    Select Case format_

    '        Case InputControl.InputFormat.Calendar

    '            If IsDate(valorAsignado_) Then

    '                Return Convert.ToDateTime(valorAsignado_).Date.ToString("yyyy-MM-dd")

    '            End If

    '        Case InputControl.InputFormat.Money

    '            Return FormatCurrency(valorAsignado_)

    '        Case Else

    '            Return valorAsignado_

    '    End Select

    '    Return Nothing

    'End Function

    Public Function ObtenerAcuseValor(idAcuseValor_ As ObjectId) As TagWatcher _
                                 Implements IControladorAcuseValor.ObtenerAcuseValor

        Dim ConstructorAcuseValor_ = _acusesValorGenerados.Find(Function(e) e.Id = idAcuseValor_.ToString)

        If ConstructorAcuseValor_ Is Nothing Then

            Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos(21) With
            {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

                _bulkCamposPedidos = _organismo.ObtenerCamposSeccionExterior(New List(Of ObjectId) From {idAcuseValor_},
                                                                             New ConstructorAcuseValor, New Dictionary(Of [Enum],
                                                                             List(Of [Enum])) From {{SeccionesAcuseValor.SAcuseValor1,
                                                                             New List(Of [Enum]) From {CamposAcuseValor.CA_NUMERO_ACUSEVALOR}}})

                If _bulkCamposPedidos Is Nothing Then

                    _estado.ObjectReturned = ""

                Else

                    _estado.ObjectReturned = DirectCast(_bulkCamposPedidos(idAcuseValor_).Item(0), Campo).Valor

                End If

            End Using

        Else

            _estado.ObjectReturned = ConstructorAcuseValor_.Seccion(SeccionesAcuseValor.SAcuseValor1).Attribute(CamposAcuseValor.CA_NUMERO_ACUSEVALOR).Valor

        End If

        _estado.SetOK()

        Return _estado

    End Function

    Public Function DescargarXML(idCOVEs As List(Of ObjectId)) As TagWatcher Implements IControladorAcuseValor.DescargarXML

        Throw New NotImplementedException()

    End Function

    Public Function DescargarPDF(idCOVEs As List(Of ObjectId)) As TagWatcher Implements IControladorAcuseValor.DescargarPDF

        Throw New NotImplementedException()

    End Function

    Function ActualizarIDS(idCOVE As ObjectId) As TagWatcher Implements IControladorAcuseValor.ActualizarIDS

        Dim status_ = New TagWatcher

        Using iEnlaceDatos_ As IEnlaceDatos = New EnlaceDatos(21)

            Dim operationDB_ = iEnlaceDatos_.GetMongoCollection(Of OperacionGenerica)("ConstructorAcuseValor")


            Dim update_ = Builders(Of OperacionGenerica).Update.
                                    Set(Of ObjectId)("Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts.Encabezado.0.Nodos.0.Nodos.0.Nodos.0.Valor", idCOVE).
                                    Set(Of ObjectId)("Borrador.Folder.ArchivoPrincipal.Dupla.Fuente._id", idCOVE)


            status_.ObjectReturned = operationDB_.UpdateOne(Function(e) e.Id = idCOVE, update_)

            If status_.ObjectReturned.MatchedCount > 0 Then

                status_.SetOK()



            Else

                status_.SetError("No se actualizó ningún elemento")

            End If

            Return status_

        End Using

    End Function

    Function EnvioCOVE(documentoElectronico_ As ConstructorAcuseValor,
                             certPath_ As String,
                             keyPath_ As String,
                             userName_ As String,
                             certPassword_ As String,
                             webServicePassoword_ As String) As String Implements IControladorAcuseValor.EnvioCOVE

        ActulizarCertificado(certPath_,
                             keyPath_,
                             userName_,
                             certPassword_,
                             webServicePassoword_)

        Return _ivucemActions.SubmitCoveRequest(documentoElectronico_,
                                                True)

    End Function

    Function EnvioCOVE(documentoElectronico_ As ConstructorAcuseValor,
                             certBytes As Byte(),
                             keyBytes As Byte(),
                             userName_ As String,
                             certPassword_ As String,
                             webServicePassoword_ As String) As String Implements IControladorAcuseValor.EnvioCOVE

        ActulizarCertificado(certBytes,
                             keyBytes,
                             userName_,
                             certPassword_,
                             webServicePassoword_)

        Return _ivucemActions.SubmitCoveRequest(documentoElectronico_,
                                                True)

    End Function

    Public Function ObtenerPDF(resultCove_ As String) As Byte() Implements IControladorAcuseValor.ObtenerPDF

        Return _ivucemActions.GetCoveAcknowledgmentPdf(resultCove_, True)

    End Function

    Public Function ObtenerPDF(idAcuseValor_ As ObjectId) As Byte() Implements IControladorAcuseValor.ObtenerPDF

        Using controladorDocumento_ = New ControladorDocumento

            Dim tagWatcher_ = ConsultaAcuseValor(idAcuseValor_,
                                                 New Dictionary(Of [Enum],
                                                                List(Of [Enum])) From {{SeccionesAcuseValor.SAcuseValor1,
                                                                                        New List(Of [Enum]) From {CamposAcuseValor.CA_NUMERO_ACUSEVALOR}},
                                                                                       {SeccionesAcuseValor.SAcuseValor5,
                                                                                       New List(Of [Enum]) From {CamposAcuseValor.CP_ID_CLIENTE_SELLO}}})


            Dim resultCove_ As String = DirectCast(tagWatcher_.ObjectReturned(idAcuseValor_)(0), Campo).Valor

            Dim idCLiente_ As ObjectId = DirectCast(tagWatcher_.ObjectReturned(idAcuseValor_)(1), Campo).Valor

            Dim bulkCamposPedidos = _organismo.ObtenerCamposSeccionExterior(New List(Of ObjectId) From {idCLiente_},
                                                                             New ConstructorCliente, New Dictionary(Of [Enum],
                                                                             List(Of [Enum])) From {{SeccionesClientes.SCS5,
                                                                             New List(Of [Enum]) From {CamposClientes.CP_CONTRASENIA_SELLOS,
                                                                                                       CamposClientes.CP_CVE_WEB_SERVICES_SELLOS,
                                                                                                       CamposClientes.CP_RUTA_ARCHIVO_SER_SELLOS,
                                                                                                       CamposClientes.CP_RUTA_ARCHIVO_KEY_SELLOS}},
                                                                                                       {SeccionesClientes.SCS1,
                                                                             New List(Of [Enum]) From {CamposClientes.CA_RFC_CLIENTE}}})



            Dim certPassword_ = DirectCast(bulkCamposPedidos(idCLiente_).Item(0), Campo).Valor

            Dim webServicePassoword_ = DirectCast(bulkCamposPedidos(idCLiente_).Item(1), Campo).Valor

            Dim certBytes As Byte() = controladorDocumento_.
                                      GetDocument(DirectCast(bulkCamposPedidos(idCLiente_).Item(2),
                                                  Campo).Valor).
                                      ObjectReturned

            Dim keyBytes As Byte() = controladorDocumento_.
                                     GetDocument(DirectCast(bulkCamposPedidos(idCLiente_).Item(3),
                                                 Campo).Valor).
                                     ObjectReturned

            Dim userName_ = DirectCast(bulkCamposPedidos(idCLiente_).Item(4),
                                       Campo).Valor

            ActulizarCertificado(certBytes,
                                 keyBytes,
                                 userName_,
                                 certPassword_,
                                 webServicePassoword_)


            Return _ivucemActions.GetCoveAcknowledgmentPdf(resultCove_, True)

        End Using

    End Function

    Public Function ObtenerXML(idAcuseValor_ As ObjectId) As String Implements IControladorAcuseValor.ObtenerXML

        Using controladorDocumento_ = New ControladorDocumento

            Dim tagWatcher_ = ConsultaAcuseValor(idAcuseValor_,
                                             Nothing)

            Dim documentoElectronico_ As DocumentoElectronico = tagWatcher_.ObjectReturned(0)

            Dim resultCove_ As String = documentoElectronico_.
                                        Attribute(CamposAcuseValor.CA_NUMERO_ACUSEVALOR).
                                        Valor

            Dim idCLiente_ As ObjectId = documentoElectronico_.
                                         Attribute(CamposAcuseValor.CP_ID_CLIENTE_SELLO).
                                         Valor

            Dim bulkCamposPedidos = _organismo.ObtenerCamposSeccionExterior(New List(Of ObjectId) From {idCLiente_},
                                                                             New ConstructorCliente, New Dictionary(Of [Enum],
                                                                             List(Of [Enum])) From {{SeccionesClientes.SCS5,
                                                                             New List(Of [Enum]) From {CamposClientes.CP_CONTRASENIA_SELLOS,
                                                                                                       CamposClientes.CP_CVE_WEB_SERVICES_SELLOS,
                                                                                                       CamposClientes.CP_RUTA_ARCHIVO_SER_SELLOS,
                                                                                                       CamposClientes.CP_RUTA_ARCHIVO_KEY_SELLOS}},
                                                                                                       {SeccionesClientes.SCS1,
                                                                             New List(Of [Enum]) From {CamposClientes.CA_RFC_CLIENTE}}})

            Dim certPassword_ = DirectCast(bulkCamposPedidos(idCLiente_).Item(0), Campo).Valor

            Dim webServicePassoword_ = DirectCast(bulkCamposPedidos(idCLiente_).Item(1), Campo).Valor

            Dim certBytes As Byte() = controladorDocumento_.GetDocument(DirectCast(bulkCamposPedidos(idCLiente_).Item(2), Campo).Valor).ObjectReturned

            Dim keyBytes As Byte() = controladorDocumento_.GetDocument(DirectCast(bulkCamposPedidos(idCLiente_).Item(3), Campo).Valor).ObjectReturned

            Dim userName_ = DirectCast(bulkCamposPedidos(idCLiente_).Item(4), Campo).Valor

            ActulizarCertificado(certBytes, keyBytes, userName_, certPassword_, webServicePassoword_)

            Return _ivucemActions.SubmitCoveRequest(documentoElectronico_,
                                                    customerStamp_:=True,
                                                    onlyXML_:=True)

        End Using

    End Function

    Public Function ObtenerPDFEdocument(resultEdocument_ As String) As Byte() Implements IControladorAcuseValor.ObtenerPDFEdocument

        Return _ivucemActions.GetAcknowledgmentEdocumentPDF(resultEdocument_)

    End Function

    Public Function RegistrarEdocument(fileName_ As String,
                                 edocument_ As Byte(),
                                 email_ As String,
                                 idDocumentType_ As Integer,
                                 rfcConsulta_ As String,
                                 Optional idCustomer_ As ObjectId = Nothing,
                                 Optional reference_ As String = Nothing,
                                 Optional force_ As Boolean = True) As String Implements IControladorAcuseValor.RegistrarEdocument



        Return _ivucemActions.SubmitEdocument(fileName_,
                                              edocument_,
                                              email_,
                                              idDocumentType_,
                                              rfcConsulta_,
                                              idCustomer_,
                                              reference_,
                                              force_)

    End Function

    Function PruebaEnvioMV(documentoElectronico_ As ConstructorAcuseValor) As String Implements IControladorAcuseValor.PruebaEnvioMV

        Return _ivucemActions.SubmitMVRequest(documentoElectronico_, False)

    End Function

    Public Function ImprimirCove(document_ As ConstructorAcuseValor) As Byte() _
        Implements IControladorAcuseValor.ImprimirCove

        If _ivucemActions Is Nothing Then

            _ivucemActions = New VUCEMActions

        End If

        Return _ivucemActions.GetCovePrintRepresentation(document_)

    End Function

    Public Function ImprimirCove(objedtId_ As ObjectId) As Byte() _
        Implements IControladorAcuseValor.ImprimirCove

        Dim indexAcuseValor_

        If _acusesValorGenerados Is Nothing Then

            ConsultaAcuseValor(objedtId_)

            indexAcuseValor_ = 0

        Else

            indexAcuseValor_ = _acusesValorGenerados.FindIndex(Function(e) Equals(e.Id, objedtId_.ToString))

            If indexAcuseValor_ = -1 Then

                ConsultaAcuseValor(objedtId_)

                indexAcuseValor_ = 0

            End If

        End If

        Return ImprimirCove(_acusesValorGenerados(indexAcuseValor_))

    End Function

#End Region

End Class


