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

                BsonClassMap.RegisterClassMap(Of ConstructorCliente)()

                BsonClassMap.RegisterClassMap(Of ConstructorProducto)()

                BsonClassMap.RegisterClassMap(Of ConstructorTIGIE)()

                BsonClassMap.RegisterClassMap(Of ConstructorProveedoresOperativos)()

                BsonClassMap.RegisterClassMap(Of ConstructorDestinatario)()

            End If

            'If Not BsonClassMap.IsClassMapRegistered(GetType(IMetadatos)) Then
            '    BsonClassMap.RegisterClassMap(Of Metadatos)()
            'End If

        End Sub

    End Class

End Namespace



'Imports Microsoft.AspNet.Identity
'Imports Microsoft.AspNet.Identity.MongoDB
'Imports MongoDB.Driver
'Imports Owin
'Imports Rec.Globals.Controllers

'Public Class Startup
'    Public Sub Configuration(app As IAppBuilder)
'        ' ... otras configuraciones

'        ' Configurar el contexto de MongoDB
'        Dim connectionString As String = "mongodb://localhost:27017"
'        Dim databaseName As String = "YourDatabaseName"
'        Dim client = New MongoClient(connectionString)
'        Dim Database = client.GetDatabase(databaseName)

'        app.CreatePerOwinContext(Function() New ApplicationDbContext())
'        app.CreatePerOwinContext(Function() New UserManager(New UserStore < ApplicationUser > (New ApplicationDbContext(Database))))
'        app.CreatePerOwinContext(Function() New RoleManager(New RoleStore < IdentityRole > (New ApplicationDbContext(Database))))

'        ' ...
'    End Sub
'End Class