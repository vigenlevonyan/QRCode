using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QShirt.Application.AuthOptions;
using QShirt.Persistence;
using QShirt.WebApi.AutofacModules;

var builder = WebApplication.CreateBuilder(args);

//Configurations
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHealthChecks();
builder.Services.AddSwaggerGen(opts => opts.SupportNonNullableReferenceTypes());

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ApiVersionReader = new HeaderApiVersionReader("api-version");
    options.ReportApiVersions = true;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // indicates whether the issuer will be validated during token validation
            ValidIssuer = WebApiAuthOptions.Issuer, // string representing the issuer

            ValidateAudience = true, // whether the token audience will be validated
            ValidAudience = WebApiAuthOptions.Audience, // setting the token audience

            ValidateLifetime = false, // whether the lifetime will be validated

            IssuerSigningKey = WebApiAuthOptions.GetSymmetricSecurityKey(), // setting the security key
            ValidateIssuerSigningKey = true // security key validation
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


#region EF

string connectionString = configuration.GetConnectionString("Default");
builder.Services.AddDbContext<QShirtContext>(options => options.UseSqlServer(connectionString));

#endregion EF


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "External systemsApi", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
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
            new string[] { }
        }
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Autofac
var containerBuilder = new ContainerBuilder();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(b => b.RegisterAssemblyModules(typeof(Program).Assembly));
builder.Host.ConfigureContainer<ContainerBuilder>(b =>
{
    b.RegisterModule<ApplicationModule>();
    b.RegisterModule<PersistenceModule>();
    b.RegisterModule<InfrastructureModule>();
});

#endregion Autofac

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
    endpoints.MapControllers();
    endpoints.MapHealthChecks("/ready", new HealthCheckOptions
    {
        Predicate = reg => reg.Tags.Contains("readiness")
    });
    endpoints.MapHealthChecks("/lively", new HealthCheckOptions
    {
        Predicate = reg => reg.Tags.Contains("liveness")
    });
});

app.Run();
