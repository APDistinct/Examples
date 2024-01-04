using Grpc.Core;
using MyGrpcTestService.Data;

namespace MyGrpcTestService.Services
{
    public class SetExchangeService : SetExchange.SetExchangeBase
    {
        private static IDataProcessing? DataProcessing { get; set; }
        public SetExchangeService(IDataProcessing dataProcessing)
        {
            DataProcessing = dataProcessing;
        }
        public override async Task<SetResponse> ProcessSetRequest(IAsyncStreamReader<SetRequest> requestStream, ServerCallContext context)
        {
            var list = new List<User>();
            var response = new SetResponse
            {
            };
            while (await requestStream.MoveNext())
            {
                var request = requestStream.Current;
                Console.WriteLine($"SendData : {request.Id}, {request.Name}, {request.Money}, {request.DateBirth}");
                // Обработка запроса
                //var response = new SetResponse
                //{
                //};
                response.Id = request.Id;
                //  TODO: Need to save data
                list.Add(new User() { /*DateBirth = request.DateBirth,*/ Id = request.Id, Money = request.Money, Name =request.Name });
                //list.Add(response);
            }
            DataProcessing?.SetNewUsers(list);

            return response; // list; // Возвращаем пустой ответ, если нет данных в потоке

        }
        private void GetResponseList(List<SetResponse>  responses)
        {
            //var list = responses.Select(r => new User { Name = r.Id, Id = r.Id, /*Age = r.*/})
            var list = new List<GetResponse>();
            for (int i = 0; i < 5; ++i)
            {
                list.Add(new GetResponse
                { Id = Guid.NewGuid().ToString(), Name = $"idf {i}", Money = 33 + i });
            }
            //return list;
        }
    }
}
