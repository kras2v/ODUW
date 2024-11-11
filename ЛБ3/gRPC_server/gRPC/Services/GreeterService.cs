using gRPC_server;
using Grpc.Core;
using System.Text;
using System.Xml.Linq;

namespace gRPC_server.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            Console.WriteLine($"{request.Name} has been sent a request\n");
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + (request.Name)
            });
        }
    }
}
