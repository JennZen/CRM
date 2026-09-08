using CRM.Application.Interfaces.Repositories;
using CRM.Application.Interfaces.Services;
using CRM.Application.Services;
using CRM.DataAccess.Auth;
using CRM.DataAccess.Mapping;
using CRM.DataAccess.Repositories;
using CRM.Services.Services;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your token:"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
    };
});

builder.Services.AddAuthorization();

builder.Services.AddSingleton<IDataLayer>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    var dataStore = XpoDefault.GetConnectionProvider(connectionString, AutoCreateOption.DatabaseAndSchema);
    return new SimpleDataLayer(dataStore);
});

builder.Services.AddScoped<UnitOfWork>(provider =>
    new UnitOfWork(provider.GetRequiredService<IDataLayer>()));

builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();  
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IHasherService, HasherService>();

builder.Services.AddSingleton<CRM.DataAccess.Mapping.CustomerMapper>();
builder.Services.AddSingleton<CRM.DataAccess.Mapping.RequestMapper>();
builder.Services.AddSingleton<CRM.DataAccess.Mapping.UserMapper>();
builder.Services.AddSingleton<CRM.Application.Mapping.RequestMapper>();
builder.Services.AddSingleton<CRM.Application.Mapping.CustomerMapper>();
builder.Services.AddSingleton<CRM.Application.Mapping.UserMapper>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dataLayer = scope.ServiceProvider.GetRequiredService<IDataLayer>();
    using var uow = new UnitOfWork(dataLayer);
    uow.UpdateSchema(typeof(CRM.DataAccess.Models.CustomerDb),
                      typeof(CRM.DataAccess.Models.RequestDb),
                      typeof(CRM.DataAccess.Models.UserDb));
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
