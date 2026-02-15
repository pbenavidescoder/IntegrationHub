using IntegrationHub.Api.Endpoints;
using IntegrationHub.Api.Middlewares;
using IntegrationHub.Application.FinTech.Handlers;
using IntegrationHub.Application.FinTech.InboundPorts;
using IntegrationHub.Application.FinTech.OutboundPorts;
using IntegrationHub.Application.FinTech.Services;
using IntegrationHub.Domain.FinTech.Events;
using IntegrationHub.Domain.FinTech.OutboundPorts;
using IntegrationHub.Infrastructure.FinTech.Email;
using IntegrationHub.Infrastructure.FinTech.Events;
using IntegrationHub.Infrastructure.FinTech.Gateways;
using IntegrationHub.Infrastructure.FinTech.Persistence;
using IntegrationHub.Infrastructure.FinTech.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

//Application
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<IEmailService, ConsoleEmailService>();
builder.Services.AddTransient<IEventHandler<AccountCreated>, SendWelcomeEmailHandler>();
//Infrastrructure
builder.Services.AddSingleton<IAccountRepository, InMemoryAccountRepository>();
builder.Services.AddSingleton<IEventBus, InMemoryEventBus>();
builder.Services.AddScoped<IPaymentQueryService, StripePaymentQueryService>();
builder.Services.AddScoped<IPaymentGateway, StripePaymentGateway>();
builder.Services.AddTransient<IPaymentWebHookHandler, StripeWebhookHandler>();

builder.Services.Configure<StripeSettings>(
    builder.Configuration.GetSection("Stripe"));

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