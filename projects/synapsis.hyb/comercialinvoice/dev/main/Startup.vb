Imports Microsoft.AspNet.Identity
Imports Microsoft.Owin
Imports Microsoft.Owin.Security.Cookies
Imports MongoDB.Bson.Serialization
Imports MongoDB.Bson.Serialization.Conventions
Imports Owin
Imports Syn.Documento

<Assembly: OwinStartup(GetType(Principal.Startup))>

Namespace Principal
    Public Class Startup
        Public Sub Configuration(app As IAppBuilder)

            Dim sax_ = Sax.SaxStatements.GetInstance

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
                .CookieSameSite = SameSiteMode.Lax,
                .CookiePath = "/"
    })

            If Not BsonClassMap.IsClassMapRegistered(GetType(DocumentoElectronico)) Then

                BsonClassMap.RegisterClassMap(Of DocumentoElectronico)()

                BsonClassMap.RegisterClassMap(Of ConstructorFacturaComercial)()

                BsonClassMap.RegisterClassMap(Of ConstructorAcuseValor)()

                BsonClassMap.RegisterClassMap(Of ConstructorCliente)()

                BsonClassMap.RegisterClassMap(Of ConstructorProveedoresOperativos)()

                BsonClassMap.RegisterClassMap(Of ConstructorSubdivisionFacturaComercial)()

                ' BsonClassMap.RegisterClassMap(Of ConstructorProcesamientoElectDocumentos)()

                BsonClassMap.RegisterClassMap(Of ConstructorProducto)()

                BsonClassMap.RegisterClassMap(Of ConstructorTIGIE)()

                BsonClassMap.RegisterClassMap(Of ConstructorDestinatario)()

            End If

            Dim pack = New ConventionPack From {New IgnoreExtraElementsConvention(True)}
            ConventionRegistry.Register("IgnorarAtributosViejos", pack, Function(t) True)

        End Sub
    End Class
End Namespace
