using System;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Providers.MongoDB.Test.GrainInterfaces;
using Orleans.Providers.MongoDB.Test.Grains;

namespace Orleans.Providers.MongoDB.Test.Host
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var silo = new SiloHostBuilder()
                .ConfigureApplicationParts(options =>
                {
                    options.AddApplicationPart(typeof(EmployeeGrain).Assembly).WithReferences();
                })
                .UseMongoDBClustering(options =>
                {
                    options.ConnectionString = "mongodb://localhost/OrleansTestApp";
                })
                .AddStartupTask(async (s, ct) =>
                {
                    var grainFactory = s.GetRequiredService<IGrainFactory>();

                    await grainFactory.GetGrain<IHelloWorldGrain>((int)DateTime.UtcNow.TimeOfDay.Ticks).SayHello("HI");
                })
                .UseMongoDBReminders(options =>
                {
                    options.ConnectionString = "mongodb://localhost/OrleansTestApp";
                })
                .AddMongoDBGrainStorage("MongoDBStore", options =>
                {
                    options.ConnectionString = "mongodb://localhost/OrleansTestApp";
                })
                .Configure<ClusterOptions>(options => options.ClusterId = "helloworldcluster")
                .ConfigureEndpoints(IPAddress.Loopback, 11111, 30000)
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();

            silo.StartAsync().Wait();

            Console.WriteLine("Silo running. Press key to exit...");
            Console.ReadKey();

            silo.StopAsync().Wait();
        }
    }
}