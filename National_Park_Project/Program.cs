using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using National_Park_Project;
using National_Park_Project.Data;
using National_Park_Project.DTOMapping;
using National_Park_Project.Repository;
using National_Park_Project.Repository.IRepository;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();


var cs = builder.Configuration.GetConnectionString("constr"); 
builder.Services.AddDbContext<ApplicationDbContext>(options =>options.UseSqlServer(cs));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var appsettingsection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appsettingsection);
var appsetting = appsettingsection.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appsetting.Secret);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
x.RequireHttpsMetadata = false;
x.SaveToken = false;
x.TokenValidationParameters = new()
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(key),
    ValidateIssuer = false,
    ValidateAudience = false
};
   
});


builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<INationalParkRepository,NationalParkRepository>();
builder.Services.AddScoped<ITrailRepository,TrailRepository>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
