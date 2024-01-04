using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using MyGrpcTestClient;
using System.Threading.Channels;

class Program
{
    static async Task Main(string[] args)
    {
        var  channelRead = GrpcChannel.ForAddress("https://localhost:5001");
        //new Channel("localhost:5000", ChannelCredentials.Insecure);
        var clientRead = new Exchange.ExchangeClient(channelRead);

        var request = new Request
        {
            Id = Guid.NewGuid().ToString(),
            Name = "John Doe",
            DateBirth = Timestamp.FromDateTime(DateTime.UtcNow), //           DateTime.UtcNow/*.ToString()*/,
            Money = 100.50f
        };

        // Бесконечный опрос сервера
        using (var call = clientRead.ProcessRequest(request))
        {
            var responseReader = Task.Run(async () =>
            {
                while (await call.ResponseStream.MoveNext())
                {
                    var response = call.ResponseStream.Current;
                    Console.WriteLine($"ProcessRequest Response: {response.Id}, {response.State}, {response.DateExpiry}, {response.Money}");
                }
            });

            //Console.WriteLine("Press Enter to stop...");
            //Console.ReadLine();

            //await call.ResponseStream.CompleteAsync();
            //await call.RequestStream.CompleteAsync();
            await responseReader;
        }
        //channelRead.ShutdownAsync().Wait();

        //var channelWrite = GrpcChannel.ForAddress("http://localhost:5000");
        ////new Channel("localhost:5000", ChannelCredentials.Insecure);
        //var clientWrite = new Exchange.ExchangeClient(channelWrite);

        // Бесконечный опрос клиента
        using (var call = clientRead.SendData())
        {
            var requestStream = Task.Run(async () =>
            {
                var request1 = new Request
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "John Doe",
                    DateBirth = Timestamp.FromDateTime(DateTime.UtcNow)/*.ToString()*/,
                    Money = 100.50f
                };
                for (int i = 0; i < 5; ++i) // while (true)
                {
                    try
                    {
                        await call.RequestStream.WriteAsync(request1);
                        Console.WriteLine($"SendData : {request1.Id}, {request1.Name}");
                        request1.Money += 12;
                        await Task.Delay(1000); // Задержка между отправками
                    }
                    catch (Exception ex) 
                    { 
                        Console.WriteLine(ex.Message); 
                    } 
                }
            } );
            await requestStream;
            await call.RequestStream.CompleteAsync();
            var response = await call.ResponseAsync;
            if (response != null)
                Console.WriteLine($"SendData Response: {response.Id}, {response.State}, {response.DateExpiry}, {response.Money}");
            else
                Console.WriteLine($"SendData Response: Error");

        }

        channelRead.ShutdownAsync().Wait();
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}