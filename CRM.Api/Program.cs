using CRM.Application.Interfaces.Repositories;
using CRM.DataAccess.Mapping;
using CRM.DataAccess.Repositories;
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
builder.Services.AddSingleton<RequestMapper>();
builder.Services.AddSingleton<CustomerMapper>();

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
