using Grpc.Core;
using Grpc.Net.Client;
using MyGrpcClientGet;
using System.Threading.Channels;

class Program
{
    static async Task Main(string[] args)
    {
        var channelRead = GrpcChannel.ForAddress("https://localhost:5001");
        //new Channel("localhost:5000", ChannelCredentials.Insecure);
        var clientRead = new GetExchange.GetExchangeClient(channelRead);

        var request = new GetNewRequest();

        //var request = new GetRequest
        //{
        //    Id = Guid.NewGuid().ToString(),
        //    //Name = "John Doe",
        //    //DateBirth = DateTime.UtcNow.ToString(),
        //    //Money = 100.50f
        //};

        // Бесконечный опрос сервера
        bool nextstep = true;
        while (nextstep)
        {
            using (var call = clientRead.ProcessNewRequest(request))
            {
                var responseReader = Task.Run(async () =>
                {
                    while (await call.ResponseStream.MoveNext())
                    {
                        var response = call.ResponseStream.Current;
                        Console.WriteLine($"ProcessRequest Response: {response.Id}, {response.DateBirth}, {response.Name}, {response.Money}");
                    }
                });

                //Console.WriteLine("Press Enter to stop...");
                //Console.ReadLine();

                //await call.ResponseStream.CompleteAsync();
                //await call.RequestStream.CompleteAsync();
                await responseReader;
            }
            //channelRead.ShutdownAsync().Wait();


            channelRead.ShutdownAsync().Wait();
            Console.WriteLine("Press 'q' to exit...");
            var key = Console.ReadKey().KeyChar;
            nextstep = !(key.Equals('q') || key.Equals('Q'));
        }
    }
}