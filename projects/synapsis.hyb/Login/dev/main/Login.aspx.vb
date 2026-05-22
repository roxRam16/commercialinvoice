Imports System.Net
Imports System.Security.Claims
Imports System.Threading.Tasks
Imports Amazon.Runtime.Internal
Imports AspNet.Identity.MongoDB
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin
Imports Microsoft.Owin.Security
Imports MongoDB.Driver
Imports sax.authentication
Imports Wma.Exceptions

Public Class Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        ' Verifica si el usuario ya está autenticado

        Dim urlCurrent_ = Request.Url.Host

        If Request.IsAuthenticated Then
            ' Redirige al usuario autenticado a la página principal

            Dim owin_ = New OwinController(HttpContext.Current.GetOwinContext())

            Response.Redirect(owin_.GetUrl())

        End If



        If Session("tbLogin_") IsNot Nothing Then

            IniciaSesion_Click()

        End If

    End Sub

    'Public Sub IniciaSesion_Click()


    '    Dim tagwatcher_ = New TagWatcher With {.Status = TagWatcher.TypeStatus.Ok}

    '    If Request.Form("user") IsNot Nothing And Request.Form("password") IsNot Nothing Then

    '        Dim tagwatcherTask_ As Task(Of TagWatcher) = Login(Request.Form("user").ToString,
    '                                                         Request.Form("password").ToString,
    '                                                          tagwatcher_)

    '        If tagwatcherTask_.Status = TaskStatus.Faulted Then



    '            Session("fallaLogin_") = "Error en el inicio de sesión"

    '        Else

    '            tagwatcherTask_.Wait()

    '            tagwatcher_ = tagwatcherTask_.Result

    '            If tagwatcher_.Status = TagWatcher.TypeStatus.Ok Then

    '                Dim owin_ = New OwinController(HttpContext.Current.GetOwinContext())

    '                Response.Redirect(owin_.GetUrl)

    '            Else

    '                Session("fallaLogin_") = "Usuario Inválido"

    '                'MsgBox("Usuario inválido")

    '            End If

    '        End If

    '    End If

    'End Sub

    'Public Sub IniciaSesion_Click(Optional ByVal user_ As String = Nothing,
    '                              Optional ByVal pwd_ As String = Nothing)

    '    Dim usuario_ As String = Nothing

    '    If user_ IsNot Nothing Then

    '        usuario_ = user_

    '    ElseIf Request.Form("user") IsNot Nothing Then

    '        usuario_ = Request.Form("user").ToString

    '    End If


    '    Dim password_ As String = Request.Form("password")

    '    If pwd_ IsNot Nothing Then

    '        password_ = pwd_

    '    ElseIf Request.Form("password") IsNot Nothing Then

    '        password_ = Request.Form("password").ToString

    '    End If


    '    Dim tagwatcher_ = New TagWatcher With {.Status = TagWatcher.TypeStatus.Ok}

    '    If usuario_ IsNot Nothing And password_ IsNot Nothing Then

    '        Dim tagwatcherTask_ As Task(Of TagWatcher) = Login(usuario_,
    '                                                         password_,
    '                                                          tagwatcher_)

    '        If tagwatcherTask_.Status = TaskStatus.Faulted Then


    '            Session("fallaLogin_") = "Error en el inicio de sesión"

    '        Else

    '            tagwatcherTask_.Wait()

    '            tagwatcher_ = tagwatcherTask_.Result

    '            If tagwatcher_.Status = TagWatcher.TypeStatus.Ok Then

    '                Dim owin_ = New OwinController(HttpContext.Current.GetOwinContext())
    '                'Response.Redirect(owin_.GetUrl)

    '                ''***********************************************************************************************
    '                ' 1. La URL donde vive tu componente SignIn en React
    '                Dim urlReactAuth = "/signin"

    '                ' 2. Preparamos el HTML "transportador"
    '                Dim sb As New StringBuilder()
    '                sb.Append("<html><head><title>Sincronizando...</title>")
    '                sb.Append("<script type='text/javascript'>")
    '                ' Inyectamos los datos en el objeto window
    '                sb.Append($"    window.__AUTH_DATA__ = {{ usr: '{usuario_}', pwd: '{password_}' }};")
    '                ' Redirigimos por JS para mantener el objeto window vivo en la carga
    '                sb.Append($"    window.location.href = '{urlReactAuth}';")
    '                sb.Append("</script>")
    '                sb.Append("</head><body></body></html>")

    '                ' 3. Enviamos la respuesta
    '                Response.Clear()
    '                Response.Write(sb.ToString())
    '                Response.Flush()
    '                Context.ApplicationInstance.CompleteRequest()

    '                '**************************************************************************************************

    '            Else

    '                Session("fallaLogin_") = "Usuario Inválido"

    '                'MsgBox("Usuario inválido")

    '            End If

    '        End If

    '    End If

    'End Sub

    Public Sub IniciaSesion_Click(Optional ByVal user_ As String = Nothing,
                                  Optional ByVal pwd_ As String = Nothing)

        Dim usuario_ As String = Nothing

        If user_ IsNot Nothing Then

            usuario_ = user_

        ElseIf Request.Form("user") IsNot Nothing Then

            usuario_ = Request.Form("user").ToString

        End If


        Dim password_ As String = Request.Form("password")

        If pwd_ IsNot Nothing Then

            password_ = pwd_

        ElseIf Request.Form("password") IsNot Nothing Then

            password_ = Request.Form("password").ToString

        End If


        Dim tagwatcher_ = New TagWatcher With {.Status = TagWatcher.TypeStatus.Ok}

        If usuario_ IsNot Nothing And password_ IsNot Nothing Then

            Dim tagwatcherTask_ As Task(Of TagWatcher) = Login(usuario_,
                                                             password_,
                                                              tagwatcher_)

            If tagwatcherTask_.Status = TaskStatus.Faulted Then


                Session("fallaLogin_") = "Error en el inicio de sesión"

            Else

                tagwatcherTask_.Wait()

                tagwatcher_ = tagwatcherTask_.Result

                If tagwatcher_.Status = TagWatcher.TypeStatus.Ok Then

                    Dim owin_ = New OwinController(HttpContext.Current.GetOwinContext())
                    'Response.Redirect(owin_.GetUrl)

                    '''***********************************************************************************************
                    '' 1. La URL donde vive tu componente SignIn en React
                    'Dim urlReactAuth = "/signin"

                    '' 2. Preparamos el HTML "transportador"
                    'Dim sb As New StringBuilder()
                    'sb.Append("<html><head><title>Sincronizando...</title>")
                    'sb.Append("<script type='text/javascript'>")
                    '' Inyectamos los datos en el objeto window
                    'sb.Append($"    window.__AUTH_DATA__ = {{ usr: '{usuario_}', pwd: '{password_}' }};")
                    '' Redirigimos por JS para mantener el objeto window vivo en la carga
                    'sb.Append($"    window.location.href = '{urlReactAuth}';")
                    'sb.Append("</script>")
                    'sb.Append("</head><body></body></html>")

                    '' 3. Enviamos la respuesta
                    'Response.Clear()
                    'Response.Write(sb.ToString())
                    'Response.Flush()
                    'Context.ApplicationInstance.CompleteRequest()

                    '**************************************************************************************************
                    ' 1. URL de React
                    Dim urlReactAuth = "/signin"

                    ' 2. HTML Transportador Blindado
                    Dim sb As New StringBuilder()
                    sb.Append("<html><head><title>Sincronizando...</title></head><body>")
                    sb.Append("<script type='text/javascript'>")
                    sb.Append("    try {")
                    ' Encapsulamos los datos en una constante
                    sb.Append($"        const data = JSON.stringify({{ usr: '{usuario_}', pwd: '{password_}' }});")
                    ' Escribimos en localStorage
                    sb.Append("        localStorage.setItem('__sync_auth_data__', data);")
                    ' Pequeña pausa de 10ms para asegurar que el motor de almacenamiento termine (opcional pero seguro)
                    sb.Append($"        setTimeout(function() {{ window.location.href = '{urlReactAuth}'; }}, 10);")
                    sb.Append("    } catch (e) {")
                    sb.Append("        console.error('Error guardando en localStorage', e);")
                    ' Si falla el storage, intentamos ir de todas formas o manejar el error
                    sb.Append($"        window.location.href = '{urlReactAuth}';")
                    sb.Append("    }")
                    sb.Append("</script>")
                    sb.Append("</body></html>")

                    ' 3. Envío forzado al cliente
                    Response.Clear()
                    Response.ContentType = "text/html"
                    Response.Write(sb.ToString())
                    Response.Flush()
                    ' Esto es vital para detener el ciclo de vida de la página ASP.NET aquí mismo
                    Response.End()
                    '**************************************************************************************************

                Else

                    Session("fallaLogin_") = "Usuario Inválido"

                    'MsgBox("Usuario inválido")

                End If

            End If

        End If

    End Sub
    Sub Register_Click()

        Response.Redirect("LoginRegister.aspx")

    End Sub

    Public Async Function Login(email_ As String,
                                    password_ As String,
                                    tagwatcher_ As TagWatcher) As Task(Of TagWatcher)

        Dim owin_ = New OwinController(HttpContext.Current.GetOwinContext())

        Dim user_ = Await owin_.GetUserManager.FindAsync(email_, password_).ConfigureAwait(False)

        If user_ IsNot Nothing Then

            tagwatcher_ = Await SignInAsync(owin_, user_, True).ConfigureAwait(False)

            tagwatcher_.ObjectReturned = user_

            Return tagwatcher_

        Else

            tagwatcher_.SetError(Me, "Usuario Inválido")

            Return tagwatcher_

        End If

    End Function

    Private Async Function SignInAsync(owin_ As OwinController,
                                       user_ As ApplicationUser,
                                       isPersistent_ As Boolean) As Task(Of TagWatcher)


        Dim authenticationManager = owin_.GetContext.Authentication


        Dim identity_ = Await owin_.GetUserManager.CreateIdentityAsync(user_,
                                                               DefaultAuthenticationTypes.ApplicationCookie)

        Dim principal_ = New ClaimsPrincipal(identity_)


        ' Custom validation logic
        If principal_.HasClaim(Function(c) c.Type.Contains("SYNAPSIS")) Then

            authenticationManager.SignIn(New AuthenticationProperties() With {
            .IsPersistent = isPersistent_,
            .RedirectUri = owin_.GetUrl
        }, identity_)

            Return New TagWatcher With {.Status = TagWatcher.TypeStatus.Ok}

        Else

            Session("fallaLogin_") = "Insuficientes Privilegios"

            Return New TagWatcher With {.Status = TagWatcher.TypeStatus.OkBut}
            'Exit Sub ' Prevent further execution if not authorized
        End If

        ' Sign-in the user if claim is valid



    End Function


End Class