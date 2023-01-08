using FLChat.DAL;
using MihaZupan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace FLChat.TelegramBot
{
    public class TelegramBotClientHandler
    {
        private readonly TelegramBotClient _client;
        private readonly TelegramLogWritter _log;

        public TelegramBotClient Client => _client;
        public TelegramLogWritter Log => _log;

        public TelegramBotClientHandler(string token, IWebProxy proxy) {
            _log = new TelegramLogWritter(true, TransportKind.Telegram);
            _client = new TelegramBotClient(token, proxy);
            System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls12;

            _client.MakingApiRequest += _log.Request;
            _client.ApiResponseReceived += _log.Response;
            _client.ApiRequestException += _log.Exception;
        }

        public TelegramBotClientHandler(string token, string proxyAddr, int proxyPort, string proxyUser, string proxyPsw)
            : this(token, new HttpToSocks5Proxy(proxyAddr, proxyPort, proxyUser, proxyPsw)) {
        }
    }
}
