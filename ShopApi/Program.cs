using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopApi.DataAccess;
using ShopApi.Entities;
using ShopApi.Helpers;
using ShopApi.Repositories;
using ShopApi.Repositories.IRepositories;
using ShopApi.Services;
using System.Configuration;
using System.Text;



var builder = WebApplication.CreateBuilder(args);
var CORSallowAny = "allowAny";
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
/////////////////////////

builder.Services.AddIdentityCore<AppUser>(opt =>
{
    opt.Password.RequireNonAlphanumeric = false;
})
.AddRoles<AppRole>()
.AddRoleManager<RoleManager<AppRole>>()
.AddSignInManager<SignInManager<AppUser>>()
.AddRoleValidator<RoleValidator<AppRole>>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };

                    //options.Events = new JwtBearerEvents
                    //{
                    //    OnMessageReceived = context =>
                    //    {
                    //        var accessToken = context.Request.Query["access_token"];

                    //        var path = context.HttpContext.Request.Path;
                    //        if (!string.IsNullOrEmpty(accessToken) &&
                    //            path.StartsWithSegments("/hubs"))
                    //        {
                    //            context.Token = accessToken;
                    //        }

                    //        return Task.CompletedTask;
                    //    }
                    //};
                    ///SIRNALR
                });

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: CORSallowAny,
              policy =>
              {
                  policy.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
;
              });
});
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DbContextConnString"));
});
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddScoped<IPhotoService, PhotoService>();

//////////////////////
var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(CORSallowAny);
app.UseAuthentication();//new
app.UseAuthorization();
app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<ApplicationDbContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    await context.Database.MigrateAsync();//akualizuje db w azure jak znajdzie now¹ migracje https://www.morrowsolutions.dev/post/automate-database-deployments-with-entity-framework-core
    //await context.Database.GetPendingMigrations//napisne jak znajdue t¹ migracje
    await Seed.SeedUsers(userManager, roleManager);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}
await app.RunAsync();
