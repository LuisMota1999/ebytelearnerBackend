using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ebyteLearner.DTOs.Auth;

namespace ebyteLearner.Services
{
    public interface IWebSocketService
    {
        Task InitConnectionSocket(HttpContext context);
        void Sender(string username, string message);
        void SenderAll(string username, string message);
    }

    public class WebSocketService: IWebSocketService
    {
        private static readonly Dictionary<string, WebSocket> Clients = new Dictionary<string, WebSocket>();
        private readonly ILogger<WebSocketService> _logger;

        public WebSocketService(ILogger<WebSocketService> logger)
        {
            _logger = logger;
        }

        public async Task InitConnectionSocket(HttpContext context)
        {
            var username = context.Request.RouteValues["UsernameID"] as string;
            if (string.IsNullOrEmpty(username))
            {
                context.Response.StatusCode = 400;
                return;
            }
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            _logger.LogInformation("[WEBSOCKET] NEW CLIENT: " + username);
            Clients[username] = webSocket;

            await Reader(webSocket);
        }

        private async Task Reader(WebSocket webSocket)
        {
            var buffer = new byte[1024];
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                    return;
                }

                var messageBytes = new byte[result.Count];
                Array.Copy(buffer, messageBytes, result.Count);
                var message = Encoding.UTF8.GetString(messageBytes);

                _logger.LogInformation(message);
            }
        }

        public void Sender(string username, string message)
        {
            if (!Clients.ContainsKey(username))
            {
                _logger.LogInformation("[WEBSOCKET] THE CLIENT '" + username + "' DOESN'T EXIST");
                return;
            }

            _logger.LogInformation("[WEBSOCKET] SENDING MESSAGE '" + message + "' TO USER '" + username + "'");
            var bytes = Encoding.UTF8.GetBytes(message);
            var buffer = new ArraySegment<byte>(bytes);
            var webSocket = Clients[username];
            webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None).GetAwaiter().GetResult();
        }

        public void SenderAll(string username, string message)
        {
            _logger.LogInformation("[WEBSOCKET] SENDING MESSAGE '" + message + "' TO ALL USERS EXCEPT USER '" + username + "'");
            foreach (var index in Clients.Keys)
            {
                if (index != username)
                {
                    Sender(index, message);
                }
            }
        }
    }
}
