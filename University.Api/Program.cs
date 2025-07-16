
using Autofac;
using Autofac.Extensions.DependencyInjection;
using System.ComponentModel;
using University.Api.Modules;
using University.Core.Services;
using University.Data.Contexts;
using University.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(Container =>
{
    Container.RegisterModule<RepositoryModule>();
    Container.RegisterModule<ServiceModule>();
});
// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
