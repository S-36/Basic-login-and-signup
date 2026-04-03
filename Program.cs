using Login_and_Signup.middleware;
using DotNetEnv;
using Login_and_Signup.DB;
using Login_and_Signup.JWT;
using Login_and_Signup.User.Interface;
using Login_and_Signup.User.repository;
using Login_and_Signup.User.services;
using Login_and_Signup.Bycript;
using Microsoft.AspNetCore.Authentication.JwtBearer;
var builder = WebApplication.CreateBuilder(args);
// Load ENV FILES 
Env.Load();

// Load Database Configuration 
builder.Services.Configure<MongoSettings>(options =>
{
   options.ConectionString = Environment.GetEnvironmentVariable("DB_CONECTION") ?? throw new InvalidOperationException("DB_CONECTION is not set"); 
   options.DatabaseName = Environment.GetEnvironmentVariable("DB_NAME") ?? throw new InvalidOperationException("DB_NAME is not set"); 
});

// Configure builder services here 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Login and Signup API", Version = "v1" });
});

// JWT Configuration
builder.Services.Configure<JwtSettings>(options =>
{
   options.SecretKey = Environment.GetEnvironmentVariable("SECRET_KEY") ?? throw new InvalidOperationException("SECRET_KEY_JWT is not set");
   options.Audience = Environment.GetEnvironmentVariable("AUDIENCE") ?? throw new InvalidOperationException("AUDIENCE_JWT is not set");
   options.Issuer = Environment.GetEnvironmentVariable("ISSUER") ?? throw new InvalidOperationException("ISSUER_JWT is not set");
   // If Hours is not set use 1 hour 
   options.ExpirationHours = int.TryParse( Environment.GetEnvironmentVariable("JWT_EXPIRATION_HOURS"), out var hours) ? hours : 1;
    
});

//JWT Authentification Configuration
builder.Services.AddAuthentication(options =>
{   // Set the default authentication scheme to JWT Bearer
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true, // Valida el issuer
        ValidateAudience = true, // Valida la audiencia 
        ValidateLifetime = true, // Valida la expiracion
        ValidateIssuerSigningKey = true, // Valida la firma del token

        // Los valores con los que se valida el token
        ValidIssuer = Environment.GetEnvironmentVariable("ISSUER") ?? throw new InvalidOperationException("ISSUER_JWT is not set"),
        ValidAudience = Environment.GetEnvironmentVariable("AUDIENCE") ?? throw new InvalidOperationException("AUDIENCE_JWT is not set"),
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET_KEY") ?? throw new InvalidOperationException("SECRET_KEY_JWT is not set"))),
    
        ClockSkew = TimeSpan.Zero // Elimina el tiempo de tolerancia para la expiracion del token
    };
});

// Custom Services 
builder.Services.AddSingleton<IMongoContext, MongoDBContext>();
builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.AddSingleton<IBycriptService, BycriptService>();
// Services and Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();


//CORDS
builder.Services.AddCors(options =>
{
    // Politica para Desarollo Permite todo
    options.AddPolicy("DevelomentPolicy", policy =>
    {
        // Add Policy configuration 
        policy.WithOrigins("http://localhost:5050") //--> Your Frontend or localhost
        .AllowAnyHeader()
        .AllowAnyMethod();
    });

    options.AddPolicy("ProductionPolicy", policy =>
    {
        // Add Policy configuration 
        policy.WithOrigins("https://yourproductionfrontend.com") //--> El frontend de produccion
        .WithHeaders("Authorization", "Content-Type") //--> Permite Solo los headers de proteccion y contenido en produccion
        .WithMethods("GET", "POST", "PUT", "DELETE"); //--> Permite los metodos que necesites en produccion
    });
});
// Authentification and Authorization
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

// Inicio del pipeline de la app
app.UseMiddleware<ConsoleLogs>();

// Swagger if is Development not Production
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Cords - Authentication - Authorization - MapControllers
app.UseHttpsRedirection();
// Usa los CORDS dependiendo del entorno
if(app.Environment.IsDevelopment())
{
    app.UseCors("DevelomentPolicy");
}
else
{
    app.UseCors("ProductionPolicy");
}
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

