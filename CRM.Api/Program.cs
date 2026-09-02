using CRM.Application.Interfaces.Repositories;
using CRM.Application.Mapping;
using CRM.DataAccess.Models;
using CRM.DataAccess.Repositories;
using CRM.Domain.Entities;
using DevExpress.Xpo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

XpoDefault.DataLayer = XpoDefault.GetDataLayer(
    connectionString,
    DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema
);

/*using (var uow = new UnitOfWork(XpoDefault.DataLayer))
{
    uow.UpdateSchema(
        typeof(CustomerDb),
        typeof(RequestDb),
        typeof(UserDb)
    );
    uow.CreateObjectTypeRecords(
        typeof(CustomerDb),
        typeof(RequestDb),
        typeof(UserDb)
    );
}*/

builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddScoped<CustomerMapper>();
builder.Services.AddScoped<RequestMapper>();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();

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
