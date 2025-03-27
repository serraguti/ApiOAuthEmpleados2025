using ApiOAuthEmpleados.Data;
using ApiOAuthEmpleados.Helpers;
using ApiOAuthEmpleados.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//CREAMOS UNA INSTANCIA DE NUESTRO HELPER
HelperActionServicesOAuth helper =
    new HelperActionServicesOAuth(builder.Configuration);
//ESTA INSTANCIA SOLAMENTE DEBEMOS CREARLA UNA VEZ
//PARA QUE NUESTRA APLICACION PUEDA VALIDAR CON TODO LO QUE HA CREADO
builder.Services.AddSingleton<HelperActionServicesOAuth>(helper);
//HABILITAMOS LA SEGURIDAD UTILIZANDO LA CLASE HELPER
builder.Services.AddAuthentication(helper.GetAuthenticateSchema())
    .AddJwtBearer(helper.GetJwtBearerOptions());

// Add services to the container.
string connectionString =
    builder.Configuration.GetConnectionString("SqlAzure");
builder.Services.AddTransient<RepositoryHospital>();
builder.Services.AddDbContext<HospitalContext>
    (options => options.UseSqlServer(connectionString));
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}
app.MapOpenApi();
app.UseHttpsRedirection();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "Api Seguridad Empleados");
    options.RoutePrefix = "";
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
