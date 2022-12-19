# Sample Benchmark for helping CosmosDb document design
A small utility for bench queries and models on cosmosdb

for having this working you need to setup a cosmosdb account and a database with a collection
then you need to setup the following environment variables ```COSMOS_ENDPOINT``` and ```COSMOS_KEY```

you can doing this executing the following commands

```ps
 $env:COSMOS_ENDPOINT = "your endpoint"
 $env:COSMOS_KEY = "your key"
```

then you can run the benchmark with ```dotnet run -c release```. 

