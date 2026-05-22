
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

Imports Microsoft.AspNet.Identity
Imports Microsoft.Owin
Imports Microsoft.Owin.Security.Cookies
Imports Microsoft.Owin.Security.DataHandler
Imports Microsoft.Owin.Security.DataProtection
Imports MongoDB.Bson.Serialization.Conventions
Imports Owin
Imports Sax.authentication

<Assembly: OwinStartup(GetType(Startup))>
Public Class Startup
    Public Sub Configuration(app As IAppBuilder)

        Dim sax_ = sax.SaxStatements.GetInstance

        Dim cookieDomain_ = "." & sax_.GetTargetEnviroment

        If cookieDomain_ = "." OrElse cookieDomain_ = ".localhost" Then

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

        '        .CookieSecure = CookieSecureOption.Never,

        '.CookieSameSite = SameSiteMode.None

        app.CreatePerOwinContext(Of ApplicationDbContext)(Function(options, context)
                                                              Return New ApplicationDbContext()
                                                          End Function)

        app.CreatePerOwinContext(Of ApplicationUserManager)(Function(options, context)
                                                                Return ApplicationUserManager.Create(options, context)
                                                            End Function)

        ' Esto le dice al driver: "Si ves algo en el BSON que no está en mi clase, simplemente ignóralo"
        Dim pack = New ConventionPack From {New IgnoreExtraElementsConvention(True)}
        ConventionRegistry.Register("IgnorarAtributosViejos", pack, Function(t) True)
    End Sub

    'Public Sub Configuration(app As IAppBuilder)
    '    ' Configurar la protección de datos compartida
    '    'Dim dataProtectionProvider_ = DataProtectionProvider.Create(New DirectoryInfo("C:\shared-keys"))

    '    Dim dataProtectionProvider_ = New DpapiDataProtectionProvider("SYNAPSIS")
    '    '   Dim dataProtector_ As Microsoft.AspNetCore.DataProtection.IDataProtector = dataProtectionProvider_.CreateProtector("Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationMiddleware", "Cookies", "v2")

    '    ' Configurar la autenticación de cookies
    '    app.UseCookieAuthentication(New CookieAuthenticationOptions() With {
    '            .AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
    '            .LoginPath = New PathString("/Login.aspx"),
    '            .CookieName = ".AspNet.SharedCookie",
    '            .CookieDomain = "localhost", ' Reemplaza con la IP de tu servidor
    '            .TicketDataFormat = New TicketDataFormat(dataProtectionProvider_.Create("Cookies"))
    '        })

    '    app.CreatePerOwinContext(Of ApplicationDbContext)(Function(options, context)
    '                                                              Return New ApplicationDbContext()
    '                                                          End Function)

    '        app.CreatePerOwinContext(Of ApplicationUserManager)(Function(options, context)
    '                                                                Return ApplicationUserManager.Create(options, context)
    '                                                            End Function)

    '    app.CreatePerOwinContext(Of ApplicationSignInManager)(Function(options, context)
    '                                                              Return ApplicationSignInManager.Create(options, context)
    '                                                          End Function)

    'End Sub

    'Public void ConfigureServices(IServiceCollection services)
    '{
    '    // ... otros servicios

    '    services.AddAuthorization(options =>
    '    {
    '        options.AddPolicy("RequireHomePhone", policy =>
    '        {
    '            policy.RequireClaim(ClaimTypes.HomePhone, "2299575809")
    '        })
    '    })
    '}

End Class