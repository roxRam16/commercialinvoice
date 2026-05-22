using Microsoft.Extensions.DependencyInjection;
using IdentityMongodb.Models;
using IdentityMongodb.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;


var builder = WebApplication.CreateBuilder(args);

//HABILITAMOS CORS
builder.Services.AddCors();

//AREA DE INYECCIÓN DE DEPENDENCIA
// Vincula la configuración de MongoDbConfig con el archivo appsettings.json
builder.Services.Configure<MongoDbConfig>(
    builder.Configuration.GetSection(nameof(MongoDbConfig))); //nameof eqiuvale a escribir es string el valor de la propiedad, sirve para evitar hardcodeos

//Cualquier contructor del proyecto recibirá la configuración de arriba
builder.Services.AddSingleton<IMongoDbConfig>(d => d.GetRequiredService<IOptions<MongoDbConfig>>().Value);


// 3. Uso en Identity
//INYECTAMOS EL CODIGO
var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbConfig)).Get<MongoDbConfig>();

/*Con esto hemos configurado con éxito la Identidad con la base de datos MongoDB.*/
builder.Services.AddIdentity<ApplicationUser, ApplicationRoles>(options =>
{
    // options.User.AllowedUserNameCharacters = null; //Esto permite cualquier cadena
    // Configurar opciones de Identity si es necesario
    options.User.RequireUniqueEmail = true; // Requerir email único
    options.User.AllowedUserNameCharacters = null; // Permitir todos los caracteres Unicode
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = true; // Se requiere al menos un símbolo especial
    options.Password.RequireUppercase = true; // Se requiere al menos una letra mayúscula


})      /*Globally Unique Identifier) es un identificador único de 128 bits que se utiliza para representar de forma global un valor único*/
        .AddMongoDbStores<ApplicationUser, ApplicationRoles, Guid> /*Usamos el método AddMongoDbStores() (NUTGET) para agregar la implementación de MongoDB de Identity: SIMILAR EN SQL SERVER .AddEntityFrameworkStores<ApplicationDbContext>()*/
        (
            mongoDbSettings.Host, mongoDbSettings.DataBase
        )
        .AddDefaultTokenProviders();
/*El tercer parámetro se proporciona como Guid porque las claves principales de las colecciones “Usuarios” y “Roles” están definidas como Guid.*/


//AREA DE SERVICIOS
//Habilitamos los servicios de controladores
// Add services to the container.
builder.Services.AddControllersWithViews();

//Habilitar los servicios de controladores
//Controllers en caso de apis
//builder.Services.AddControllers();

//AREA DE SWAGGER - SI FUERA APIS REST
//Agregamos swagger
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();



var app = builder.Build();


//AREA DE CONFIGURACION DE PIPELIME
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI(c =>
    //{
    //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Companies API V1");
    //    c.RoutePrefix = string.Empty; // Esto hace que Swagger cargue en la raíz: https://localhost:7010/
    //});

    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


//AREA DE MIDDLEWARES
// Add services to the container.
builder.Services.AddControllersWithViews();

// Configuración de las cookies de autenticación
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true; // Previene acceso al cliente mediante JavaScript
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Tiempo de expiración
    options.SlidingExpiration = true; // Renueva la expiración si el usuario sigue activo
    options.LoginPath = "/Account/Login"; // Página de login
    options.AccessDeniedPath = "/Account/AccessDenied"; // Página de acceso denegado
});

/* TODA LA CONFIFURACIÓN QUE DEBE LLEVAR BUILDER ANTES DE APP */

//var app = builder.Build();


/*** A PARTIR DE AQUI VAN LOS MIDDLEWARE ***********/
/*MIDDLEWARE = COMPONENTE*/
/*Un middleware es un componente que forma parte del pipeline
 * (flujo) de procesamiento de solicitudes HTTP en una aplicación ASP.NET Core. */

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");  // Middleware para manejar errores en DESARROLLO.
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Middleware para servir archivos estáticos como CSS o JavaScript.
app.UseRouting(); // Middleware para manejar rutas.

// Shows UseCors with CorsPolicyBuilder.
app.UseCors(builder =>
{
    builder.AllowAnyOrigin() //Para permitir todos los métodos HTTP.
           .AllowAnyMethod() //Para permitir todos los encabezados de solicitud.
           .AllowAnyHeader(); //el servidor debe permitir las credenciales.

    //MAS DE UN ORIGEN
    //app.UseCors(builder =>
    //{
    //    builder
    //    .WithOrigins(new string[] { "https://www.yogihosting.com", "https://example1.com", "https://example2.com" })
    //    .AllowAnyMethod()
    //    .AllowAnyHeader()
    //    .AllowCredentials();
    //});
    //.WithOrigins("https://www.yogihosting.com") Si desea habilitar CORS para las solicitudes realizadas desde un solo dominio
});


/*AUTENTICACION DE USUARIOS*/
app.UseAuthentication(); // Middleware para autenticar usuarios.
app.UseAuthorization();  // Middleware para autorizar usuarios.



/********************¨RUTAS DE INICIO DE MI APP **************************/
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"); // Middleware de enrutamiento.
/*pattern: "{controller=Home}/{action=Index}/{id?}");*/




//EJECUCIÓN DE NUESTRA APP
app.Run();