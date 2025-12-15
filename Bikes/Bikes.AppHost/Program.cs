var builder = DistributedApplication.CreateBuilder(args);

var mongodb = builder.AddMongoDB("mongodb")
    .WithDataVolume();

var api = builder.AddProject<Projects.Bikes_Api_Host>("bikes-api")
    .WithReference(mongodb);

builder.Build().Run();