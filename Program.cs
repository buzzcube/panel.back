global using panel.back.Models;
global using panel.back.Services;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using panel.back;


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{

    var builder = WebApplication.CreateBuilder(args);

    var seqHost = Environment.GetEnvironmentVariable("seqHost");
    var seqAPI = Environment.GetEnvironmentVariable("seqAPI");

    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console()
        .WriteTo.Seq(seqHost, apiKey: seqAPI)
        .ReadFrom.Configuration(ctx.Configuration));

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
     .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, c =>
     {
         c.Authority = Environment.GetEnvironmentVariable("validIssuer");
         c.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
         {
             ValidAudience = Environment.GetEnvironmentVariable("validAudience"),
             ValidIssuer = Environment.GetEnvironmentVariable("validIssuer")
         };
     });
    builder.Services.AddAuthorization(o =>
    {
        o.AddPolicy("SuperAdmin", p => p.
            RequireAuthenticatedUser().
            RequireClaim("permissions", "SuperAdmin"));
    });

    builder.Services.AddDbContext<DataContext>(options =>
    {
        var connectionString = Environment.GetEnvironmentVariable("mysql");
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    });

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(option =>
    {
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "Panel API", Version = "v1" });
        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
    });

    builder.Services.AddScoped<IUserService, UserService>();

    var app = builder.Build();
    app.UseSerilogRequestLogging();

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
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}
