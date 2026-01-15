using IntegrationHub.Api.Endpoints;
using IntegrationHub.Api.Middlewares;
using IntegrationHub.Application.FinTech.Handlers;
using IntegrationHub.Application.FinTech.Services;
using IntegrationHub.Domain.FinTech.Events;
using IntegrationHub.Domain.FinTech.OutboundPorts;
using IntegrationHub.Infrastructure.FinTech.Email;
using IntegrationHub.Infrastructure.FinTech.Events;
using IntegrationHub.Infrastructure.FinTech.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IAccountRepository, InMemoryAccountRepository>();
builder.Services.AddSingleton<IEventBus, InMemoryEventBus>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<IEmailService, ConsoleEmailService>();
builder.Services.AddTransient<IEventHandler<AccountCreated>, SendWelcomeEmailHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1");
    });
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapFintechEndpoints();

app.Run();