
'Imports Microsoft.Owin

'Imports Microsoft.Owin.Security.Cookies
'Imports Microsoft.Owin.Security.Google
'Imports Owin
'Imports System.Configuration

'<Assembly: OwinStartup(GetType(Startup))>

'Public Class Startup
'    Public Sub Configuration(app As IAppBuilder)
'        ' Configuramos la autenticación basada en cookies primero
'        app.UseCookieAuthentication(New CookieAuthenticationOptions With {
'            .AuthenticationType = "ApplicationCookie",
'            .LoginPath = New PathString("/Login.aspx")
'        })

'        ' Luego, configura la autenticación con Google
'        Dim googleOAuth2AuthenticationOptions = New GoogleOAuth2AuthenticationOptions With {
'            .ClientId = ConfigurationManager.AppSettings("GoogleClientId"),
'            .ClientSecret = ConfigurationManager.AppSettings("GoogleClientSecret"),
'            .CallbackPath = New PathString("/signin-google"),
'            .SignInAsAuthenticationType = "ApplicationCookie", ' Asegúrate de especificar el tipo de autenticación
'            .Provider = New GoogleOAuth2AuthenticationProvider With {
'                .OnAuthenticated = Function(context)
'                                       ' Opcional: accede a información del usuario autenticado si es necesario
'                                       Return Threading.Tasks.Task.FromResult(0)
'                                   End Function
'            }
'        }

'        app.UseGoogleAuthentication(googleOAuth2AuthenticationOptions)

'    End Sub
'End Class

Imports ConstructorExpedienteElectronico64.Syn.Documento
Imports Microsoft.AspNet.Identity
Imports Microsoft.Owin
Imports Microsoft.Owin.Security.Cookies
Imports MongoDB.Bson.Serialization
Imports Owin
Imports Syn.Documento

<Assembly: OwinStartup(GetType(Principalidad.Startup))>

Namespace Principalidad
    Public Class Startup
        Public Sub Configuration(app As IAppBuilder)
            ' Configuración de autenticación basada en cookies
            Dim sax_ = Sax.SaxStatements.GetInstance

            Dim cookieDomain_ = "." & sax_.GetTargetEnviroment

            If cookieDomain_ = "." Then

                cookieDomain_ = "localhost"

            End If

            app.UseCookieAuthentication(New CookieAuthenticationOptions() With {
                .AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                .LoginPath = New PathString("/Login.aspx"),
                .CookieName = ".AspNet.ApplicationCookie",
                .CookieSecure = CookieSecureOption.Never,
                .CookieDomain = cookieDomain_,
                .CookieSameSite = SameSiteMode.Lax
            })

            If Not BsonClassMap.IsClassMapRegistered(GetType(DocumentoElectronico)) Then
                BsonClassMap.RegisterClassMap(Of DocumentoElectronico)()
                BsonClassMap.RegisterClassMap(Of ConstructorProcesamientoElectDocumentos)()
                BsonClassMap.RegisterClassMap(Of ConstructorExpededienteElectronico)()
                BsonClassMap.RegisterClassMap(Of ConstructorCliente)()
                BsonClassMap.RegisterClassMap(Of ConstructorFacturaComercial)()
                BsonClassMap.RegisterClassMap(Of ConstructorProveedoresOperativos)()
                BsonClassMap.RegisterClassMap(Of ConstructorProducto)()
            End If

        End Sub
    End Class

End Namespace
