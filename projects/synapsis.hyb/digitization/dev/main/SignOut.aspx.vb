Imports Microsoft.AspNet.Identity
Imports Sax

Public Class SignOut
    Inherits Page

    Private _statement As Sax.SaxStatements
    Sub New()

        _statement = Sax.SaxStatements.GetInstance(25)

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        CerrarSesion()

    End Sub


    Sub CerrarSesion()

        Dim context_ = HttpContext.Current.GetOwinContext()

        Dim authenticationManager = context_.Authentication

        authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie)

        'Dim accessControl_ = New ControladorAccesoKBW()

        'accessControl_.Desconectar()

        HttpContext.Current.Session.Clear()

        HttpContext.Current.Session.Abandon()

        'HttpContext.Current.Request.Cookies.Clear()

        Dim endpointPods_ As List(Of Sax.endpointpod) = _statement.GetEndPointPods

        Dim paginaLogin_ = "http://" &
                              endpointPods_(0).ip &
                              ":" &
                              endpointPods_(0).protocols(0).port &
                              "/SignOut.aspx"
        Response.Redirect(paginaLogin_)

        '   Response.Redirect("http://localhost:14326/Login.aspx")

    End Sub
End Class