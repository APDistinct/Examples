using Grpc.Core;
using Grpc.Net.Client;
using MyGrpcClientSet;
using System.Threading.Channels;
using System.Xml.Linq;

class Program
{
    static async Task Main(string[] args)
    {
        var channelRead = GrpcChannel.ForAddress("https://localhost:5001");
        //new Channel("localhost:5000", ChannelCredentials.Insecure);
        var clientRead = new SetExchange.SetExchangeClient(channelRead);

        var requests = InitData();
        var list = requests.ToList();

        bool nextstep = true;
        while (nextstep)
        {
            // Send to server
            using (var call = clientRead.ProcessSetRequest())
            {
                var requestStream = Task.Run(async () =>
                {
                    //var request1 = new SetRequest
                    //{
                    //    Id = Guid.NewGuid().ToString(),
                    //    Name = "John Doe",
                    //    DateBirth = DateTime.UtcNow.ToString(),
                    //    Money = 100.50f
                    //};
                    foreach(var request1 in list) 
                    //for (int i = 0; i < 5; ++i) // while (true)
                    {
                        try
                        {
                            await call.RequestStream.WriteAsync(request1);
                            Console.WriteLine($"SendData : {request1.Id}, {request1.Name}");
                            //request1.Money += 12;
                            //request1.Name = "John Doe " + i.ToString();
                            //request1.Id = Guid.NewGuid().ToString();
                            await Task.Delay(1000); // Wait a little
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                });
                await requestStream;
                await call.RequestStream.CompleteAsync();
                //var response = await call.ResponseAsync;
                //if (response != null)
                //    Console.WriteLine($"SendData Response: {response.Id}, {response.State}, {response.DateExpiry}, {response.Money}");
                //else
                //    Console.WriteLine($"SendData Response: Error");


            }

            channelRead.ShutdownAsync().Wait();
            Console.WriteLine("Press 'q' to exit...");
            var key = Console.ReadKey().KeyChar;
            nextstep = !(key.Equals('q') || key.Equals('Q'));
            if (nextstep)
            {
                Console.WriteLine("Number of user to change(0-4)");
                key = Console.ReadKey().KeyChar;
                int num = 0;
                list.Clear();
                if (int.TryParse(key.ToString(), out num))
                {
                    if(num >= 0 && num <5) 
                    {
                        var req = requests[num];
                        req.Money += 11;
                        list.Add(req);
                    }
                }
                else
                {
                    Console.WriteLine($"Bad number {num}");
                }
            }
        }
    }
    private static SetRequest[] InitData()
    {
        List<SetRequest> list = new List<SetRequest>();

        for (int i = 0; i < 5; ++i)
        {
            var request1 = new SetRequest
            {
                Id = Guid.NewGuid().ToString(),
                Name = "John Doe " + i.ToString(),
                DateBirth = DateTime.UtcNow.ToString(),
                Money = 100.50f + 12 * i,
            };
            list.Add(request1);
        }
        return list.ToArray();
    }
}