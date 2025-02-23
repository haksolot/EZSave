using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EZSave.Core.Services
{
    public class SocketClientService
    {
        private TcpClient _client;
        private NetworkStream _stream;

        public async Task ConnectAsync(string ip, int port)
        {
            _client = new TcpClient();
            await _client.ConnectAsync(ip, port);
            Console.WriteLine("✅ Connecté au serveur !");
            _stream = _client.GetStream();
        }

        public async Task SendMessageAsync(object message)
        {
            if (_stream == null) return;

            string json = JsonSerializer.Serialize(message);
            byte[] buffer = Encoding.UTF8.GetBytes(json);

            await _stream.WriteAsync(buffer, 0, buffer.Length);
            Console.WriteLine($"📤 Envoyé : {json}");
        }

        public async Task ListenForMessagesAsync()
        {
            byte[] buffer = new byte[1024];

            try
            {
                while (_client.Connected)
                {
                    int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string jsonMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"📥 Reçu du serveur : {jsonMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Erreur réception : {ex.Message}");
            }
            finally
            {
                Console.WriteLine("🔴 Déconnecté du serveur.");
            }
        }
    }
}