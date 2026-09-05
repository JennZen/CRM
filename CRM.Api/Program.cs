using CRM.Application.Interfaces.Repositories;
using CRM.Application.Interfaces.Services;
using CRM.Application.Services;
using CRM.DataAccess.Mapping;
using CRM.DataAccess.Repositories;
using CRM.Services.Services;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IDataLayer>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    var dataStore = XpoDefault.GetConnectionProvider(connectionString, AutoCreateOption.DatabaseAndSchema);
    return new ThreadSafeDataLayer(dataStore);
});

builder.Services.AddScoped<UnitOfWork>(provider =>
    new UnitOfWork(provider.GetRequiredService<IDataLayer>()));

builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();  
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddSingleton<CRM.DataAccess.Mapping.CustomerMapper>();
builder.Services.AddSingleton<CRM.DataAccess.Mapping.RequestMapper>();
builder.Services.AddSingleton<CRM.DataAccess.Mapping.UserMapper>();
builder.Services.AddSingleton<CRM.Application.Mapping.RequestMapper>();
builder.Services.AddSingleton<CRM.Application.Mapping.CustomerMapper>();
builder.Services.AddSingleton<CRM.Application.Mapping.UserMapper>();

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
