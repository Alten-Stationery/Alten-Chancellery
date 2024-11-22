using DBLayer.DBContext;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//GET Current Enviroment
var enviroment = builder.Environment;

//Configuration for the enviroment appsettings file
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
          .AddJsonFile($"appsettings.{enviroment.EnvironmentName}.json", optional: true, reloadOnChange: true);

//GET DB connection string
var connectionString = builder.Configuration.GetConnectionString("DBConnection");


//Add DB Connection
builder.Services.AddDbContext<ApplicationDBContext>(
    options =>
    options.UseSqlServer(connectionString)
    );


// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
