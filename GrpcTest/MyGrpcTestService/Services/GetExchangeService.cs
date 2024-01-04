using Grpc.Core;
using MyGrpcTestService.Data;

namespace MyGrpcTestService.Services
{
    public class GetExchangeService : GetExchange.GetExchangeBase
    {
        private static IDataProcessing? DataProcessing { get; set; }
        public GetExchangeService(IDataProcessing dataProcessing)
        {
            DataProcessing = dataProcessing;
        }
        public override async Task ProcessGetRequest(GetRequest request, IServerStreamWriter<GetResponse> responseStream, ServerCallContext context)
        {
            //bool pri = true;
            //DateTime dt = DateTime.UtcNow;
            //  TODO: Get data
            var list = GetResponseList();
            foreach (var response in list)
            {
                // Обработка запроса
                //var response = new GetResponse
                //{
                //    Id = Guid.NewGuid().ToString(),
                //    //State = "Processed",
                //    //DateExpiry = DateTime.UtcNow.ToString(),
                //    //Money = request.Money
                //};

                await responseStream.WriteAsync(response);
                await Task.Delay(1000); // Задержка между отправками
                //pri = dt.AddSeconds(10) > DateTime.UtcNow;
            } 
        }
        public override async Task ProcessNewRequest(GetNewRequest request, IServerStreamWriter<GetResponse> responseStream, ServerCallContext context)
        {
            //bool pri = true;
            //DateTime dt = DateTime.UtcNow;
            //  TODO: Get data
            var list = GetResponseList();
            foreach (var response in list)
            {
                // Обработка запроса
                //var response = new GetResponse
                //{
                //    Id = Guid.NewGuid().ToString(),
                //    //State = "Processed",
                //    //DateExpiry = DateTime.UtcNow.ToString(),
                //    //Money = request.Money
                //};

                await responseStream.WriteAsync(response);
                await Task.Delay(1000); // Задержка между отправками
                //pri = dt.AddSeconds(10) > DateTime.UtcNow;
            }
            MarkResponseList(list);
        }
        private List<GetResponse> GetResponseList() 
        {
            var list = new List<GetResponse>();
            var getl = DataProcessing?.GetNewUsers();
            if(getl != null) 
            foreach(var gl in getl) 
            {
                list.Add(new GetResponse
                { Id = gl.Id, Name = gl.Name, Money = gl.Money});
            }
            //for (int i = 0; i < 5; ++i) 
            //{ 
            //    list.Add(new GetResponse 
            //    {  Id = Guid.NewGuid().ToString(), Name = $"idf {i}", Money = 33 + i });
            //}
            return list;
        }
        private void MarkResponseList(List<GetResponse> list)
        {
            var fix = list.Select(l => new User() { Id = l.Id, Money = l.Money, Name = l.Name }).ToList();
            DataProcessing?.FixUsers(fix);
        }
    }
}
