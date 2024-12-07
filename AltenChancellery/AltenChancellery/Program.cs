using DBLayer.DBContext;
using DBLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using AltenChancellery.Configuration;
using System.Reflection;
using ServiceLayer.Auth;
using ServiceLayer.Services.Interfaces;
using ServiceLayer.Services.Implementations;
using Microsoft.AspNetCore.Hosting.Builder;
using DBLayer.Repositories.Interfaces;
using DBLayer.Repositories.Implementations;
using DBLayer.UnitOfWork;
using FluentValidation;
using ServiceLayer.FluentValidators;

var builder = WebApplication.CreateBuilder(args);

//GET Current Enviroment
var enviroment = builder.Environment;

//Configuration for the enviroment appsettings file
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
          .AddJsonFile($"appsettings.{enviroment.EnvironmentName}.json", optional: true, reloadOnChange: true);

//GET DB connection string
var connectionString = builder.Configuration.GetConnectionString("DBConnection");

//GET configuration from appsetting file
ConfigurationManager configuration = builder.Configuration;

//Add DB Connection
builder.Services.AddDbContext<ApplicationDBContext>(
    options =>
    options.UseSqlServer(connectionString)
    );

// Add swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add Automapper config
builder.Services.AddAutoMapper(typeof(MappingProfile));
var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new MappingProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);


//Add Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOfficeService, OfficeService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOfficeRepository, OfficeRepository>();

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();


// Add Blazor and API services
builder.Services.AddValidatorsFromAssembly(typeof(UserValidator).Assembly);
builder.Services.AddControllers().AddFluentValidation();
builder.Services.AddServerSideBlazor();
builder.Services.AddRazorPages();
builder.Services.AddSwaggerGen(options =>
{

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.ApiKey,
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});
//Add Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDBContext>()
    .AddDefaultTokenProviders();


//Add Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
//Add JWT Bearer
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});

var app = builder.Build();

var scope = app.Services.CreateScope();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

//Create role and seed
await SeedRolesAndAdminUser(roleManager, userManager);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Alten-ChancelleryAPI-v1");
        c.RoutePrefix = "swagger"; // URL: /swagger
    });
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.MapControllers();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

async Task SeedRolesAndAdminUser(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
{
    // Defination of the roules
    var roleNames = typeof(UserRoles)
                    .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                    .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
                    .Select(fi => fi.GetValue(null).ToString())
                    .ToList();

    // Creation of the roules
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }


}
