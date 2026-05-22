Imports Microsoft.AspNet.Identity

Public Class SignOut
    Inherits Page

    Private _statement As Sax.SaxStatements
    Sub New()

        _statement = Sax.SaxStatements.GetInstance()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        CerrarSesion()

    End Sub


    Sub CerrarSesion()


        Dim cookies_ = Request.Cookies

        Dim cookie_ As HttpCookie = Request.Cookies(".AspNet.ApplicationCookie")

        If cookie_ IsNot Nothing Then
            ' 2. Establecer la fecha de expiración en el pasado.
            ' Esto instruye al navegador a eliminar la cookie.
            cookie_.Expires = DateTime.Now.AddDays(-1)

            Dim sax_ = Sax.SaxStatements.GetInstance

            Dim cookieDomain_ = "." & sax_.GetTargetEnviroment

            If cookieDomain_ = "." OrElse cookieDomain_ = ".localhost" Then

                cookieDomain_ = "localhost"

            End If

            cookie_.Domain = cookieDomain_

            ' 3. Agregar la cookie modificada al Response.
            ' Esto envía la instrucción de eliminación al navegador.
            Response.Cookies.Add(cookie_)
        End If

        Response.Redirect("/FrontEnd/Modulos/TraficoAA/Clientes/Ges022-001-Clientes.aspx")

    End Sub
End Class