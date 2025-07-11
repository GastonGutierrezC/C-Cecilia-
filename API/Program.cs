
using API.Profiles;
using AutoMapper;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<BreadContext>(optionsBuilder =>
{
    var connectionString = builder.Configuration.GetConnectionString("CeciConnectionString");
    optionsBuilder.UseSqlServer(connectionString);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddAuthentication();
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
app.MapControllers();

app.Run();
