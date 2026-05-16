using Fanpa.API.Common;
using Fanpa.API.Endpoints;
using Fanpa.API.ServiceCollections;
using Fanpa.Application.DI;
using Fanpa.Infrastructure.DI;
using Fanpa.Persistence.DI;

var builder = WebApplication.CreateBuilder(args);

// default service collections
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Fanpa API",
        Version = "v1"
    });
});
builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();

// optioned service collections
builder.Services.AddCustomAuthentication(builder.Configuration);
builder.Services.AddCustomRateLimiter();

// DI from another layer
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration); // docker compose up --build
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// pipeline (middlewares)
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapAuthEndpoint();
app.MapUsersEndpoint();

app.Run();
