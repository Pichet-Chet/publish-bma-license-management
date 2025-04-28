
using bma_license_repository;
using bma_license_repository.CustomModel;
using bma_license_repository.CustomModel.Middleware;
using bma_license_repository.Dto;
using bma_license_service;
using bma_license_service.Helper.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using WatchDog;
using WatchDog.src.Enums;
using static bma_license_repository.CustomModel.AppSetting.AppSettingConfig;

IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();

var connectionString = configuration.GetConnectionString("ConnectionStr");
var connectionStrWatchdog = configuration.GetConnectionString("ConnectionStrWatchdog");

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<DevBmaContext>();

builder.Services.AddDbContext<DevBmaContext>(options =>
    options.UseNpgsql(connectionString));

var endpoint = configuration["APP:END_POINT"];

//var coreUrl = configuration["APP:CORE"];

var title = configuration["APP:TITLE"];
var version = configuration["APP:VERSION"];
var versionDescription = configuration["APP:VERSION_DESCRIPTION"];


#region | Logging Watchdog Zone |

//builder.Logging.AddWatchDogLogger();

//builder.Services.AddWatchDogServices(opt =>
//{
//    opt.IsAutoClear = true;
//    opt.ClearTimeSchedule = WatchDogAutoClearScheduleEnum.Every6Hours;
//    opt.SetExternalDbConnString = connectionStrWatchdog;
//    opt.DbDriverOption = WatchDogDbDriverEnum.PostgreSql;
//});

#endregion


#region | Set Config |
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JWTConfig>();
builder.Services.AddSingleton(jwtSettings);

var connecttionStringSetting = builder.Configuration.GetSection("ConnectionStrings").Get<ConnectionStringsConfig>();
builder.Services.AddSingleton(connecttionStringSetting);


#endregion

#region | Add Scope Zone |

builder.Services.AddScoped<IMigrateDataRepository, MigrateDataRepository>();
builder.Services.AddScoped<IMigrateDataService, MigrateDataService>();

builder.Services.AddScoped<ISysConfigurationRepository, SysConfigurationRepository>();
builder.Services.AddScoped<ISysConfigurationService, SysConfigurationService>();

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<ISysUserRepository, SysUserRepository>();
builder.Services.AddScoped<ISysUserService, SysUserService>();

builder.Services.AddScoped<ISysUserGroupRepository, SysUserGroupRepository>();
builder.Services.AddScoped<ISysUserGroupService, SysUserGroupService>();

builder.Services.AddScoped<ISysMessageConfigurationService, SysMessageConfigurationService>();
builder.Services.AddScoped<ISysMessageConfigurationRepository, SysMessageConfigurationRepository>();

builder.Services.AddScoped<ISysUserPermissionRepository, SysUserPermissionRepository>();
builder.Services.AddScoped<ISysUserPermissionService, SysUserPermissionService>();

builder.Services.AddScoped<ISysUserTypeRepository, SysUserTypeRepository>();
builder.Services.AddScoped<ISysUserTypeService, SysUserTypeService>();

builder.Services.AddScoped<ISysOrgranizeRepository, SysOrgranizeRepository>();
builder.Services.AddScoped<ISysOrgranizeService, SysOrgranizeService>();

builder.Services.AddScoped<ISysKeyTypeService, SysKeyTypeService>();
builder.Services.AddScoped<ISysKeyTypeRepository, SysKeyTypeRepository>();

builder.Services.AddScoped<ITransKeyRepository, TransKeyRepository>();
builder.Services.AddScoped<ITransKeyService, TransKeyService>();

builder.Services.AddScoped<ITransJobRepairRepository, TransJobRepairRepository>();
builder.Services.AddScoped<ITransJobRepirService, TransJobRepirService>();

builder.Services.AddScoped<ISysJobRepairStatusRepository, SysJobRepairStatusRepository>();

builder.Services.AddScoped<ISysRepairCategoryRepository, SysRepairCategoryRepository>();

builder.Services.AddScoped<ITransKeyHistoryResitory, TransKeyHistoryResitory>();
builder.Services.AddScoped<ITransKeyHistoryService, TransKeyHistoryService>();

builder.Services.AddScoped<IMasterRepository, MasterRepository>();
builder.Services.AddScoped<IMasterService, MasterService>();

builder.Services.AddScoped<ITransEquipmentRepository, TransEquipmentRepository>();
builder.Services.AddScoped<ITransEquipmentService, TransEquipmentService>();

#endregion


// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
});

builder.Services.AddMemoryCache();


//Configure Authorization
builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

// Add JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            ValidAudience = configuration["JWT:Audience"],
            ValidIssuer = configuration["JWT:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("user", policy => policy.RequireRole("user"));
    options.AddPolicy("it", policy => policy.RequireRole("it"));
    options.AddPolicy("admin", policy => policy.RequireRole("admin"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

DateTime setFileLastModified = DateTime.Now;


// Add Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc($"{version}", new OpenApiInfo
    {
        Title = $"{title}",
        Version = $"{version}",
        Description = $"{versionDescription} build at {setFileLastModified.ToString("dd/MM/yyyy HH:mm")}",
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter the JWT token in the field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
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
                    Array.Empty<string>()
                }
            });
});


builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = Convert.ToInt32(configuration["RATE_LIMIT:PERMIT_LIMIT"]),
                QueueLimit = Convert.ToInt32(configuration["RATE_LIMIT:QUEUE_LIMIT"]),
                Window = TimeSpan.FromSeconds(Convert.ToInt32(configuration["RATE_LIMIT:SECONDS"]))
            }));

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;

        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
        {
            await context.HttpContext.Response.WriteAsync(
                $"Too many requests. Please try again after {retryAfter.TotalMinutes} minute(s). " +
                $"Read more about our rate limits at.", cancellationToken: token);
        }
        else
        {
            await context.HttpContext.Response.WriteAsync(
                "Too many requests. Please try again later. " +
                "Read more about our rate limits at.", cancellationToken: token);
        }
    };
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<DevBmaContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

//app.UsePathBase("/backend");
app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = "backend/swagger"; // ตั้งค่า Route ให้ตรงกับ URL ที่ต้องการ
});



app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseRateLimiter();
//app.UseWatchDog(opt =>
//{
//    opt.WatchPageUsername = configuration["WatchDogLogging:username"];
//    opt.WatchPagePassword = configuration["WatchDogLogging:password"];
//});

app.UseMiddleware<ExceptionHandlingMiddleware>();


app.Run();



public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var resp = new ResponseModel
        {
            Status = ConstantsResponse.StatusError,
            Code = ConstantsResponse.HttpCode500,
            Message = ConstantsResponse.HttpCode500Message,
            InnerException = ex.InnerException == null ? ex.Message : ex.InnerException.ToString()
        };

        var controllerName = context.GetRouteValue("controller")?.ToString();
        var actionName = context.GetRouteValue("action")?.ToString();

        WatchLogger.LogError(resp.InnerException, ex.StackTrace, controllerName + "|" + actionName);

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        return context.Response.WriteAsJsonAsync(resp);
    }

    public DateTime GetLinkerTimestampUtc(string filePath)
    {
        DateTime setFileCreated = DateTime.Now;
        DateTime setFileLastModified = DateTime.Now;

        var bytes = new byte[2048];

        using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            file.Read(bytes, 0, bytes.Length);

            FileInfo fi = new FileInfo(filePath);
            setFileCreated = fi.CreationTime;
            setFileLastModified = fi.LastWriteTime;
        }

        return setFileLastModified;
    }
}