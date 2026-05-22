using CompaniesApi.Models;
using CompaniesApi.Settings;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using CompaniesApi.Services;


var builder = WebApplication.CreateBuilder(args);


//INYECCION DE DEPENDENCIA
// Vincula la configuración de MongoDbConfig con el archivo appsettings.json
builder.Services.Configure<MongoDbConfig>(
    builder.Configuration.GetSection(nameof(MongoDbConfig))); //nameof eqiuvale a escribir es string el valor de la propiedad, sirve para evitar hardcodeos


//Cualquier contructor del proyecto recibirá la configuración de arriba
builder.Services.AddSingleton<IMongoDbConfig>(d => d.GetRequiredService<IOptions<MongoDbConfig>>().Value);

builder.Services.AddSingleton<CompanyService>();

//area de servicios
//Habilitar los servicios de controladores
builder.Services.AddControllers();

//Agregamos swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Companies API V1");
        c.RoutePrefix = string.Empty; // Esto hace que Swagger cargue en la raíz: https://localhost:7010/
    });
}


//area de Middlewares
//Con esto enviamos nuestra peticion a los controladores
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
