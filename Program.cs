using Login_and_Signup.middleware;
using DotNetEnv;
using Login_and_Signup.DB;
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
//CORDS
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        // Add Policy configuration 
        policy.WithOrigins("http://localhost:5050") //--> Your Frontend or localhost
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
// Authentification and Authorization
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();
// 1 Logs
app.UseMiddleware<ConsoleLogs>();

// Swagger if is Development not Production
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Cords - Authentication - Authorization - MapControllers
app.UseHttpsRedirection();
app.UseCors("FrontendPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

