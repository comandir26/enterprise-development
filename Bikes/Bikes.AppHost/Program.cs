var builder = DistributedApplication.CreateBuilder(args);

var kafka = builder.AddKafka("kafka")
    .WithKafkaUI()
    .WithDataVolume();

var mongodb = builder.AddMongoDB("mongodb")
    .WithDataVolume();

var _ = builder.AddProject<Projects.Bikes_Api_Host>("bikes-api")
    .WithReference(mongodb)
    .WaitFor(mongodb)
    .WithReference(kafka)
    .WaitFor(kafka);

var _2 = builder.AddProject<Projects.Bikes_Generator>("bikes-generator")
    .WithReference(kafka)
    .WaitFor(kafka);

builder.Build().Run();