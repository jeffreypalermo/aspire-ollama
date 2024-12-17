var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.AspireApp1_ApiService>("apiservice");


var ollama = builder.AddOllama("ollama")
    .WithDataVolume()
    .WithOpenWebUI()
    .WithContainerRuntimeArgs("--gpus=all");

var phi3 = ollama.AddModel("phi3", "phi3");

builder.AddProject<Projects.AspireApp1_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WithReference(phi3)
    .WaitFor(phi3)
    .WaitFor(apiService);

builder.Build().Run();
