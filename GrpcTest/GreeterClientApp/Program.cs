// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using Grpc.Net.Client;
using GreeterClientApp;

// создаем канал для обмена сообщениями с сервером
// параметр - адрес сервера gRPC
using var channel = GrpcChannel.ForAddress("https://localhost:5001");
// создаем клиент
var client = new Greeter.GreeterClient(channel);
Console.Write("Введите имя: ");
var name = Console.ReadLine();
// обмениваемся сообщениями с сервером
var reply = await client.SayHelloAsync(new HelloRequest { Name = name });
Console.WriteLine($"Ответ сервера: {reply.Message}");
Console.ReadKey();
