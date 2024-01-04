using Grpc.Core;

namespace MyGrpcTestService.Services
{
    public class ExchangeService : Exchange.ExchangeBase
    {
        public override async Task ProcessRequest(Request request, IServerStreamWriter<Response> responseStream, ServerCallContext context)
        {
            bool pri = true;
            DateTime dt = DateTime.UtcNow;
            while (!context.CancellationToken.IsCancellationRequested && pri)
            {
                // Обработка запроса
                var response = new Response
                {
                    Id = Guid.NewGuid().ToString(),
                    State = "Processed",
                    DateExpiry = DateTime.UtcNow.ToString(),
                    Money = request.Money
                };

                await responseStream.WriteAsync(response);
                await Task.Delay(1000); // Задержка между отправками
                pri = dt.AddSeconds(10) > DateTime.UtcNow;
            }
        }

        public override async Task<Response> SendData(IAsyncStreamReader<Request> requestStream, ServerCallContext context)
        {
            var response = new Response
            {
                //Id = request.Id, // Guid.NewGuid().ToString(),
                State = "Received",
                DateExpiry = DateTime.UtcNow.ToString(),
                //Money = request.Money + 10
            };
            while (await requestStream.MoveNext())
            {
                var request = requestStream.Current;
                Console.WriteLine($"SendData : {request.Id}, {request.Name}, {request.Money}, {request.DateBirth}");
                // Обработка запроса
                response.Id = request.Id;
                response.Money = request.Money + 10;                
            }

            return response; // Возвращаем пустой ответ, если нет данных в потоке
        }
    }

}
