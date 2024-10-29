using Lorien.Api.Filters;
using Lorien.Api.Jobs;
using Lorien.IoC;

const string ClientApplicationPermissionPolicyName = nameof(ClientApplicationPermissionPolicyName);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: ClientApplicationPermissionPolicyName,
    policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.RegisterApplicationDependencies(builder.Configuration);

builder.Services.AddScoped<ApiExceptionFilterAttribute>();

builder.Services.AddMemoryCache();

builder.Services.AddHostedService<CachingJob>();

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
