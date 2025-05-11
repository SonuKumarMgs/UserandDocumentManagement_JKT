using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserandDocumentManagement_JKT.CustomMiddileWare;
using UserandDocumentManagement_JKT.Data;
using UserandDocumentManagement_JKT.Sevices.Implementations;
using UserandDocumentManagement_JKT.Sevices.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbCotext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ConDb")));

builder.Services.AddTransient<ISignupService, SignupServiceImpl>();
builder.Services.AddTransient<IUserService, UserServiceImpl>();
builder.Services.AddTransient<IDocumentService, DocumentServiceImpl>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();
var app = builder.Build();
app.UseRouting();
app.UseMiddleware<GlobalResponseMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
