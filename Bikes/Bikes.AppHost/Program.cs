var builder = DistributedApplication.CreateBuilder(args);

var kafka = builder.AddKafka("kafka")
    .WithKafkaUI() 
    .WithDataVolume(); 

var mongodb = builder.AddMongoDB("mongodb")
    .WithDataVolume();

var api = builder.AddProject<Projects.Bikes_Api_Host>("bikes-api")
    .WithReference(mongodb)
    .WithReference(kafka); 

builder.Build().Run();