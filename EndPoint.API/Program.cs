using DataSample.Application.Interfaces.Contexts;
using DataSample.Common.Roles;
using Microsoft.AspNetCore.Authentication.Cookies;
using DataSample.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using MediatR;
using DataSample.Application.Services.Users.Commands.EditUser;
using DataSample.Application.Services.Users.Commands.RgegisterUser;
using DataSample.Application.Services.Users.Commands.RemoveUser;
using DataSample.Application.Services.Users.Commands.UserLogin;
using DataSample.Application.Services.Users.Commands.UserSatusChange;
using DataSample.Application.Services.Users.Queries.GetRoles;
using DataSample.Application.Services.Users.Queries.GetUsers;
using System.Reflection;
using DataSample.Application.Services.Fainances.Commands;
using DataSample.Domain.Entities.Fainances;
using EndPoint.API.Helper;
using DataSample.Application.Services.Fainances.Commands.RemoveCheque;
using DataSample.Application.Services.Fainances.Queries.GetAllCheque;
using DataSample.Application.Services.Fainances.Queries.GetChegueById;
using FluentValidation;
using System.Globalization;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(UserRoles.Admin, policy => policy.RequireRole(UserRoles.Admin));
    options.AddPolicy(UserRoles.Customer, policy => policy.RequireRole(UserRoles.Customer));
    options.AddPolicy(UserRoles.Operator, policy => policy.RequireRole(UserRoles.Operator));
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = new PathString("/Authentication/Signin");
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5.0);
    options.AccessDeniedPath = new PathString("/Authentication/Signin");
});


builder.Services.AddScoped<IDataBaseContext, DataBaseContext>();

string contectionString = "Server=./;Database=dataSampledb;Trusted_Connection=False;MultipleActiveResultSets=true;TrustServerCertificate=True";
builder.Services.AddEntityFrameworkSqlServer().AddDbContext<DataBaseContext>(option => option.UseSqlServer(contectionString));

builder.Services.AddScoped<IUser, CurrentUser>();
builder.Services.AddHttpContextAccessor(); 

builder.Services.AddScoped<IDataBaseContext, DataBaseContext>();
builder.Services.AddScoped<IEditUserService, EditUserService>();
builder.Services.AddScoped<IRegisterUserService, RegisterUserService>();
builder.Services.AddScoped<IRemoveUserService, RemoveUserService>();
builder.Services.AddScoped<IUserLoginService, UserLoginService>();
builder.Services.AddScoped<IUserSatusChangeService, UserSatusChangeService>();
builder.Services.AddScoped<IGetRolesService, GetRolesService>();
builder.Services.AddScoped<IGetUsersService, GetUsersService>();

builder.Services.AddMediatR(typeof(Program));
builder.Services.AddMediatR(typeof(ValidatorDto));

builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ValidatorDto>());

var app = builder.Build();
init.Configure(app);

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


public static class init
{
    public static void Configure(IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<DataBaseContext>();
            context.Database.Migrate();
        }
    }
}
