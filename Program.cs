using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserManagementSystem.Context;
using UserManagementSystem.GenericRepository;
using UserManagementSystem.Services;
using UserManagementSystem.Services.UserService;


var builder = WebApplication.CreateBuilder(args);
var provider = builder.Services.BuildServiceProvider();
var configuration=provider.GetRequiredService<IConfiguration>();
// Add services to the container.
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;

}).AddEntityFrameworkStores<AppDBContext>().AddDefaultTokenProviders(); ;
builder.Services.AddDbContext<AppDBContext>(options =>  options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

var smtpServer = configuration.GetSection("AppSettings:SmtpServer").Value;
var smtpPort = int.Parse(configuration.GetSection("AppSettings:SmtpPort").Value);
var smtpUsername = configuration.GetSection("AppSettings:SenderEmail").Value;
var smtpPassword = configuration.GetSection("AppSettings:SenderEmailPassword").Value;

builder.Services.AddTransient<IEmailSender>(x => new EmailSender(smtpServer, smtpPort, smtpUsername, smtpPassword));
builder.Services.AddControllers();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped( typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddAutoMapper(typeof(Program));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes(configuration.GetSection("AppSettings:Secret").Value)
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
