using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Proyecto_Web_Q2.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Servicios

// AddSingleton: una sola instancia para TODA la vida de la app con respecto al FireBaseService
builder.Services.AddSingleton<FireBaseService>();

// AddScoped: instancia nueva POR CADA petición HTTP
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CaseService>();
builder.Services.AddScoped<MediatorService>();
builder.Services.AddScoped<AgreementService>();
builder.Services.AddScoped<SessionService>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<LabNoteService>();

builder.Services.AddControllers();

// Este plugin es para que podamos usar Scalar para que esa wea nos lea los endpoints
builder.Services.AddOpenApi();

/*
Autenticación con JWT (JSON WEB TOKEN) Es para que no tengamos que guardar la sesión al servidor

Notas de clase:
- Le decimos a al app que el esquema de auth es JWT Bearer
- Bearer significa que el token ciaja en el header: Authorization: Bearer <token>
*/
builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Verificar que el token lo emitimos nosotros (app)
            ValidateAudience = true, // Verificar que el token est para la misma app
            ValidateLifetime = true, // Verificar que el token no ha expirado
            ValidateIssuerSigningKey = true, // Verificar que la firma es valida

            // Verificar que estos valores coincidan con los que usamos para generar el token
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            ),
        };
    });

// AddAuthorization, esto habilita el uso del Authorize en los controladores
builder.Services.AddAuthorization();

/*
CORS -> Para permitir que el Frontend pueda llamar a nuestra bendita api

Notas de Clase:
- CORS controla que origenes externos pueden llamar el API
- En desarrollo lo dejamos abierto para que Angular lo pueda consumir
- En produccion esto se restringe a dominios especificos
 */
builder.Services.AddCors(opt =>
    opt.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader())
);

var app = builder.Build();

// Middleware

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(opt =>
    {
        // Usando AddHttpAuthentication porque WithHttpBearerAuthentication va quedar obsoleto segun el LSP xd
        opt.WithTitle("Proyecto de Clase 847")
            .AddHttpAuthentication("Bearer", bearer => bearer.Token = "");
    });
}

// CORS debe ir antes de Authentication y Authorization
// Las peticiones del frotend se rechazan antes de llegar al auth
app.UseCors("AllowAll");

// lee el token del header y valida quien es el usuario
app.UseAuthentication();

// verifica que el usuario este autenticado y con permisos para el endpoint
app.UseAuthorization();

// conecta las rutas HTTP con los metodos de nuestros controllers
app.MapControllers();

app.Run();

  