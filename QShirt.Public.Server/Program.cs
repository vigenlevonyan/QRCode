
using Autofac;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QShirt.Application;
using QShirt.Application.Contracts;
using QShirt.Domain;
using QShirt.Persistence;
using QShirt.Public.Server;
using QShirt.Public.Server.Services;
using System.Security.Claims;
using QShirt.Application.AuthOptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Autofac.Extensions.DependencyInjection;
using QShirt.Public.Server.AutofacModules;

ExecutorService.Init();
var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

IWebHostEnvironment environment = builder.Environment;

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.Configure<HubOptions>(options =>
{
    options.MaximumReceiveMessageSize = 1024 * 1024 * 10;
});

#region gRpc
// Add services to the container.
builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<UserInfoSetterInterceptor>();
    options.EnableDetailedErrors = true;
    options.MaxReceiveMessageSize = 50 * 1024 * 1024; // 2 MB
});
#endregion gRpc

builder.Services.AddControllers();

// Swagger
builder.Services.AddSwaggerGen();

#region EF

string connectionString = configuration.GetConnectionString("Default");
builder.Services.AddDbContext<QShirtContext>(options => options.UseSqlServer(connectionString));

#endregion EF

#region Componenets

builder.Services.AddBlazoredLocalStorage();
//builder.Services.AddServiceProxies();

#endregion Components

#region Autorization/Authentication

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = AdminAuthOptions.Issuer,

        ValidateAudience = true,
        ValidAudience = AdminAuthOptions.Audience,

        ValidateLifetime = false,

        IssuerSigningKey = AdminAuthOptions.GetSymmetricSecurityKey(),
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization();

#endregion Autorization/Authentication

#region Autofac
var containerBuilder = new ContainerBuilder();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

containerBuilder.RegisterAssemblyModules(typeof(Program).Assembly);

builder.Host.ConfigureContainer<ContainerBuilder>(b =>
{
    b.RegisterModule<ApplicationModule>();
    b.RegisterModule<PersistenceModule>();
    b.RegisterModule<InfrastructureModule>();
});

#endregion Autofac

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    using (var dbContext = app.Services.GetService<QShirtContext>())
    {
        dbContext?.Database.Migrate();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();

    // Swagger
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseBlazorFrameworkFiles();
app.UseRequestLocalization();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.Use((context, next) =>
{
    //UserInfoProvider userInfoProvider = context.RequestServices.GetService<UserInfoProvider>();
    IUserInfoSetter userInfoProvider =
        context.RequestServices.GetService<IUserInfoSetter>();

    if (userInfoProvider == null)
        throw new InvalidOperationException("UserInfoProvider missing");

    var userName = context.User.Identity?.Name;
    string idFromTokenStr = context.User.Claims
        .SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    if (userName != null)
    {
        userInfoProvider.UserName = userName;
        if (idFromTokenStr != null)
            userInfoProvider.UserTokenId = Guid.Parse(idFromTokenStr);

        userInfoProvider.IsAuthenticated = true;
        userInfoProvider.UserType = UserType.Authenticated;
        if (context.User.IsInRole(UserRole.Admin.ToString()))
            userInfoProvider.UserRole = UserRole.Admin;
    }
    else
        userInfoProvider.IsAuthenticated = false;

    return next();
});

app.UseGrpcWeb();

app.UseEndpoints(endpoints =>
{
    endpoints
        .MapGrpcService<ExecutorService>()
        .EnableGrpcWeb();

    endpoints.MapControllers();
    endpoints.MapFallbackToFile("index.html");
});

app.Run();
