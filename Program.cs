using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserManagementSystem.Data;
using UserManagementSystem.Services.RoleService;
using UserManagementSystem.Services.UserService;


var builder = WebApplication.CreateBuilder(args);
var provider = builder.Services.BuildServiceProvider();
var configuration=provider.GetRequiredService<IConfiguration>();
// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>  options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddAutoMapper(typeof(Program));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes(configuration.GetSection("JWT:Secret").Value)
                        ),
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                });

var app = builder.Build();

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
