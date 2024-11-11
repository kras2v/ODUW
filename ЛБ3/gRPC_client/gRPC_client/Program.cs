using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using gRPC_server;

Console.OutputEncoding = System.Text.Encoding.UTF8;

using var channel = GrpcChannel.ForAddress("http://localhost:5070");
var client = new Greeter.GreeterClient(channel);

Console.WriteLine("Enter your name:");
var client_name = Console.ReadLine();
var reply = await client.SayHelloAsync(new HelloRequest { Name = client_name });
Console.WriteLine("You've got an answer from server: " + reply.Message);
