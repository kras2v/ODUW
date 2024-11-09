using System;
using System.Threading.Tasks;
using Grpc.Net.Client;

class Program
{
    static async Task Main(string[] args)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:5000");
        var client = new Greeter.GreeterClient(channel);

        var reply = await client.SayHelloAsync(new HelloRequest { Name = "World" });
        Console.WriteLine("Отримано відповідь від сервера: " + reply.Message);
    }
}
