<%@ WebHandler Language="VB" Class="DescargarAcuseHandler" %>

Imports System
Imports System.Web
Imports Syn.CustomBrokers.Controllers
Imports MongoDB.Bson

Public Class DescargarAcuseHandler : Implements IHttpHandler

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Public Sub ProcessRequest(ByVal context_ As HttpContext) Implements IHttpHandler.ProcessRequest
        ' Llama a tu función para obtener el array de bytes del PDF

        Dim idacuseValor_ As String = context_.Request.QueryString("idacuseValor")

        Dim acuseValor_ As String = context_.Request.QueryString("acuseValor")

        Dim onlyXML_ As String = context_.Request.QueryString("onlyXML")

        Dim representacionImpresa_ As String = context_.Request.QueryString("representacionImpresa")

        If onlyXML_ = "SI" Then

            Dim xmlString_ As String = Me.GenerarXML(idacuseValor_)

            Try

                Dim xmlBytes_ As Byte() = System.Text.Encoding.UTF8.GetBytes(xmlString_)

                context_.Response.Clear()

                context_.Response.ContentType = "text/xml"

                context_.Response.AddHeader("Content-Disposition", "attachment; filename=" & acuseValor_ & ".xml")

                context_.Response.BinaryWrite(xmlBytes_)

                context_.Response.End()

            Catch ex As Exception

            End Try



        Else

            Dim pdfBytes_ As Byte()

            Dim fileName_ = ""

            If representacionImpresa_ = "SI" Then


                pdfBytes_ = Me.RepresencionImpresa(idacuseValor_)

            Else

                pdfBytes_ = Me.GenerarPdfBytes(idacuseValor_)

                fileName_ = "ACUSE"

            End If

            context_.Response.Clear()

            context_.Response.ContentType = "application/pdf"

            context_.Response.AddHeader("Content-Disposition", "attachment; filename=" &
                                        fileName_ &
                                        acuseValor_ &
                                        ".pdf")

            context_.Response.BinaryWrite(pdfBytes_)

            context_.Response.End()

        End If

        ' Aquí Response.End() es seguro porque estamos en un handler

    End Sub


    ' Tu función que genera el array de bytes
    Private Function GenerarPdfBytes(acuseValor_ As String) As Byte()
        ' ... tu código para generar el PDF ...
        Dim icontroladorAcuse_ As IControladorAcuseValor = New ControladorAcuseValor

        Dim idCove_ As New ObjectId

        If ObjectId.TryParse(acuseValor_, idCove_) Then

            Return icontroladorAcuse_.ObtenerPDF(idCove_)

        Else

            If acuseValor_.Contains("REPRESENTACIONIMPRESA") Then

                Return Nothing 'icontroladorAcuse_.GenerarCOVEHtml

            Else

                Return icontroladorAcuse_.ObtenerPDFEdocument(acuseValor_)

            End If

        End If

    End Function

    Private Function GenerarXML(acuseValor_ As String) As String
        ' ... tu código para generar el PDF ...
        Dim icontroladorAcuse_ As IControladorAcuseValor = New ControladorAcuseValor

        Dim idCove_ As New ObjectId

        If ObjectId.TryParse(acuseValor_, idCove_) Then

            Return icontroladorAcuse_.ObtenerXML(idCove_)

        Else

            Return "ObjectId inválido"

        End If

    End Function

    Private Function RepresencionImpresa(acuseValor_ As String) As Byte()
        ' ... tu código para generar el PDF ...
        Dim icontroladorAcuse_ As IControladorAcuseValor = New ControladorAcuseValor

        Dim idCove_ As New ObjectId

        If ObjectId.TryParse(acuseValor_, idCove_) Then

            Return icontroladorAcuse_.ImprimirCove(idCove_)

        Else

            Return Nothing

        End If

    End Function

End Class