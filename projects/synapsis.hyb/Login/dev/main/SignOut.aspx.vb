Imports Microsoft.AspNet.Identity

Public Class SignOut
    Inherits Page

#Region "Enums"

    Enum POODS

        LOGIN = 2

        MASTERPAGE = 3

    End Enum

#End Region
    Sub New()



    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        CerrarSesion()

    End Sub


    Sub CerrarSesion()


        Dim context_ = HttpContext.Current.GetOwinContext()

        Dim authenticationManager = context_.Authentication

        authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie)

        HttpContext.Current.Session.Clear()

        HttpContext.Current.Session.Abandon()

        Response.Redirect("Login.aspx")

    End Sub

End Class