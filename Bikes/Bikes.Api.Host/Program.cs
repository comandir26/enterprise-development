using Bikes.Api.Host.Kafka;
using Bikes.Application.Extensions;
using Bikes.Infrastructure.MongoDb;
using Bikes.Infrastructure.MongoDb.Extensions;
using Bikes.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMongoDbInfrastructure(builder.Configuration);
builder.Services.AddBikeRentalServices();
builder.Services.AddKafkaConsumer();

var app = builder.Build();

app.Lifetime.ApplicationStarted.Register(() =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Waiting 5 seconds for services to stabilize...");
    Thread.Sleep(5000);
});

app.MapDefaultEndpoints();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var seeder = scope.ServiceProvider.GetRequiredService<MongoDbSeeder>();
        await seeder.SeedAsync();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Database seeded successfully!");
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Lifetime.ApplicationStarted.Register(async () =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Application fully started. Waiting 3 seconds before accepting requests...");
    await Task.Delay(3000);
    logger.LogInformation("Application ready to accept requests.");
});

app.Run();