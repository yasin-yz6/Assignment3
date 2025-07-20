using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using University.Api.Configurations;
using University.Api.Helpers;
using University.Core.Services;
using University.Data.Contexts;
using University.Data.Entities.Identity;
using University.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x => //SWAGGER
{
    x.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme// to insert the token
    {
        //the informations...
        In = ParameterLocation.Header,//put in the header
        Description = "Please insert Jwt with beare info field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    x.AddSecurityRequirement(
        new OpenApiSecurityRequirement {
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
//PUTTING THE AUTHORIZATION IN SWAGGER

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();//Logger creating

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<UniversityDbContext>(Options => 
    Options.UseSqlServer(connectionString));//making the sql connection her // the connection link in appsetting


builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<UniversityDbContext>()
    .AddDefaultTokenProviders();//about Identity table


builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(Container =>
{
    Container.RegisterType<UniversityDbContext>().AsSelf().InstancePerLifetimeScope();
    Container.RegisterType<StudentRepository>().As<IStudentRepository>().InstancePerLifetimeScope();
    Container.RegisterType<CourseRepository>().As<ICourseRepository>().InstancePerLifetimeScope();
    Container.RegisterType<StudentService>().As<IStudentService>().InstancePerLifetimeScope();
    Container.RegisterType<CourseService>().As<ICourseService>().InstancePerLifetimeScope();
    Container.RegisterType<AuthService>().As<IAuthService>().InstancePerLifetimeScope();
    Container.RegisterType<JwtTokenHelper>().As<IJwtTokenHelper>().InstancePerLifetimeScope();
    //---

});//autofac

builder.Host.UseSerilog();


var jwtSettings = builder.Configuration.GetSection("JwtSettings"); //adding the JwrSetting by GetSection!
builder.Services.Configure<JwtSettings>(jwtSettings);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;//these are types of authentication
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;//...
})
    .AddJwtBearer(options =>
    {
        var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"], //we call (get) these properties from settings 
            ValidAudience = jwtSettings["Audience"],//...
            IssuerSigningKey = new SymmetricSecurityKey(secretKey)
        };
    });


// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}//using swagger in development

app.UseHttpsRedirection();
app.UseAuthentication();//use authentication...
app.UseAuthorization();
app.MapControllers();
app.Run();
