using Aspose.Imaging.MemoryManagement;
using AutoMapper;
using ITI_Libraly_Api.Contexts;
using ITI_Libraly_Api.Service.Class;
using ITI_Libraly_Api.Service.Interface;
using ITI_Libraly_Api.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MobileHrApi.API.Configurations.Authorization;
using Nssf_Exam_Register_Online_Api.Util;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigurationManager configuration = builder.Configuration;

var adminApiConfiguration = configuration.GetSection(nameof(LibraryApiConfiguration)).Get<LibraryApiConfiguration>();
builder.Services.AddSingleton(adminApiConfiguration);
builder.Services.AddControllers();
string conection = configuration.GetConnectionString("DbConnection");
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDbContext<ITI_LibraySystemContext>(opt => opt.UseSqlServer(conection));
IMapper mapper = MappingConfig.RegisterMap().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IPositionService, PositionService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserLoginTimerService, UserLoginTimeService>();
builder.Services.AddScoped<IBookedService, BookService>();
builder.Services.AddScoped<ISupplyerService, SupplyerService>();
builder.Services.AddScoped<IImportService, ImportService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IReadService, ReadService>();
builder.Services.AddScoped<IBorrowService, BorrowService>();
builder.Services.AddScoped<IReturnBorrowService, ReturnBorrowService>();
builder.Services.AddScoped<IReturnReadService, ReturnReadService>();
builder.Services.AddScoped<IGeneralDataService, GeneralDataService>();
builder.Services.AddScoped<INationalService,NationalService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IDasbordService,DasbordService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = adminApiConfiguration.IdentityServerBaseUrl;
    options.Audience = adminApiConfiguration.ApiName;
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ITI LIBRALY API", Version = "v1" });
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"{adminApiConfiguration.IdentityServerBaseUrl}/connect/authorize"),
                TokenUrl = new Uri($"{adminApiConfiguration.IdentityServerBaseUrl}/connect/token"),
                Scopes = new Dictionary<string, string> {
                                { adminApiConfiguration.OidcApiName, adminApiConfiguration.ApiName }
                            }
            }
        }
    });
    c.OperationFilter<AuthorizeCheckOperationFilter>();
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
