using API.Middleware;
using Application.Activities.Queries;
using Application.Activities.Validators;
using Application.Core;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddCors();

builder.Services.AddMediatR(x =>
{

    x.RegisterServicesFromAssemblyContaining<GetActivityList.Handler>();

    x.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);

builder.Services.AddValidatorsFromAssemblyContaining<CreateActivityValidator>();

builder.Services.AddTransient<ExceptionMiddleware>();

var app = builder.Build();


app.UseMiddleware<ExceptionMiddleware>();


app.UseCors(opts => opts.AllowAnyHeader()
.AllowAnyMethod()
.WithOrigins("http://localhost:3000", "https://localhost:3000"));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var servcices = scope.ServiceProvider;

// Db setup and seed
try
{
    var context = servcices.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync();

    await DbInitializer.SeedData(context);
}
catch (Exception ex)
{
    var logger = servcices.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Error occured during migration");
}

app.Run();