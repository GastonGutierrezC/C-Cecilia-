using API.Profiles;
using API.Services;
using AutoMapper;
using Core.Interfaces;
using Core.Interfaces.Services;
using Infrastructure.Data;
using Microsoft.OpenApi.Models;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<BreadContext>(optionsBuilder =>
{
    var connectionString = builder.Configuration.GetConnectionString("CeciConnectionString");
    optionsBuilder.UseSqlServer(connectionString);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API C-Cecilia", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ingrese el token JWT con el prefijo 'Bearer ' (ejemplo: 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference 
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IInputProductService, InputProductService>();
builder.Services.AddScoped<IHomemadeProductService, HomemadeProductService>();
builder.Services.AddScoped<ISalesMetricsService, SalesMetricsService>();
builder.Services.AddScoped<ISingleItemSalesMetricsService, SingleItemSalesMetricsService>();
builder.Services.AddScoped<IOutputProductService, OutputProductService>();
builder.Services.AddScoped<IOutputIngredientService, OutputIngredientService>();
builder.Services.AddScoped<IOutputService, OutputService>();
builder.Services.AddScoped<IInputService, InputService>();
builder.Services.AddScoped<IExternalProductService, ExternalProductService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IProviderMetricsService, ProviderMetricsService>();




builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtBearer";
    options.DefaultChallengeScheme = "JwtBearer";
})
.AddJwtBearer("JwtBearer", options =>
{
    var key = System.Text.Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"]!);
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key)
    };
});

builder.Services.AddControllers();
builder.Services.AddAutoMapper(expression =>
{
    expression.AddProfile<ProductProfile>();
    expression.AddProfile<IngredientProfile>();
    expression.AddProfile<UserProfile>();
    expression.AddProfile<ProductIngredientProfile>();
    expression.AddProfile<InputProductProfile>();
    expression.AddProfile<InputProfile>();
    expression.AddProfile<InputIngredientProfile>();
    expression.AddProfile<OutputProfile>();
    expression.AddProfile<OutputProductProfile>();
    expression.AddProfile<OutputIngredientProfile>();
    expression.AddProfile<InputUserProfile>();
    expression.AddProfile<OutputUserProfile>();
    expression.AddProfile<ProviderProfile>();




});

var app = builder.Build();

app.UseCors(policyBuilder =>
{
    policyBuilder.WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});
app.UseSwagger();
app.UseSwaggerUI();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<BreadContext>();

    await SeedData.SeedAsync(context);
}


app.Run();
