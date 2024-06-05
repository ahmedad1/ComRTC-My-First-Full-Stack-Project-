using DataProject.Data;
using DataProject.Data.Dto;
using DataProject.SignalR;
using DataProject.Tokens;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project1.Middlewares;
using SmallProject.EmailService;
using SmallProject.Middlewares;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options=>options.Filters.Add<PreventBlackListTokensActionFilter>());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddCors();
builder.Services.AddMediatR(typeof(AppDbContext).Assembly);
builder.Services.AddAutoMapper(typeof(AppDbContext).Assembly);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<ITokens,RefreshAllTokens>();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("constr")).UseLazyLoadingProxies());
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddTransient<ISendService,SendEmailService>();
builder.Services.AddAuthentication(opts =>
{
    opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opts.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(o =>
{
    o.SaveToken = false;
    o.RequireHttpsMetadata = true;
    o.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]!)),
       
    };
    o.Events = new JwtBearerEvents()
    {
        OnMessageReceived = e =>
        {
            if(e.Request.Cookies.TryGetValue("jwt",out string? val))
            {
                e.Token = val;
                
            }
            
            return Task.CompletedTask;
        }
    };

});
builder.Services.AddSignalR();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseCors(x => x.WithOrigins("https://127.0.0.1:5501").AllowCredentials().AllowAnyHeader().AllowAnyMethod());
app.UseStaticFiles();
app.UseMiddleware<Redirection>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapHub<MainHub>("/connection");

app.MapControllers();
app.Run();
