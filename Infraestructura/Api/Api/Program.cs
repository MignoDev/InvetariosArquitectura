using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// =============================================
// CONFIGURACIÓN DE BASE DE DATOS
// =============================================

// La conexión a la base de datos se maneja directamente en los controladores
// usando procedimientos almacenados con SqlConnection

// =============================================
// CONFIGURACIÓN DE SERVICIOS DE INFRAESTRUCTURA
// =============================================

// HttpClient para servicios externos
builder.Services.AddHttpClient();

// =============================================
// CONFIGURACIÓN DE CACHING
// =============================================

// Agregar memoria cache
builder.Services.AddMemoryCache();

// Swagger configuration
builder.Services.AddSwaggerGen(
    options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1.0.0",
            Title = "Sistema de Inventario API",
            Description = "API simplificada para la gestión de inventario con procedimientos almacenados",
            TermsOfService = new Uri("https://example.com/terms"),
            Contact = new OpenApiContact
            {
                Name = "Sistema de Inventario",
                Url = new Uri("https://example.com/contact")
            },
            License = new OpenApiLicense
            {
                Name = "MIT License",
                Url = new Uri("https://example.com/license")
            }
        });

        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });       
    }
);

// Configuraci�n de JWT
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(options => {
//    //options.Authority = "https://localhost:7035";
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateAudience = false,
//        ValidateIssuerSigningKey = true,
//        ValidateIssuer = false,
//        IssuerSigningKey = new SymmetricSecurityKey(
//            Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AuthOptions:TokenKey").Value!)
//        )
//    };
//});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

