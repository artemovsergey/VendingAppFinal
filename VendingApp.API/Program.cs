using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using VendingApp.API.Data;
using VendingApp.API.Hubs;
using VendingApp.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["tokenKey"]!));

builder.Services.AddControllers();

builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = key,
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddDbContext<VendingAppContext>();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo());
    o.EnableAnnotations();

    o.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme { Type = SecuritySchemeType.Http, Scheme = "bearer" }
    );

    o.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = [],
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapGet(
    "/token",
    () =>
    {
        // утверждения
        var claims = new[] { new Claim(ClaimTypes.Name, "test") };

        // токен
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return Results.Ok(new JwtSecurityTokenHandler().WriteToken(token));
    }
);

app.MapHub<VendingHub>("/vendingHub");

app.Run();
