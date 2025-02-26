using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using EZSave.Core.Models;

namespace EZSave.Core.Services
{
    public class SocketClientService
    {
        private readonly string _serverIp;
        private readonly int _port;

        public SocketClientService(string serverIp = "127.0.0.1", int port = 6969)
        {
            _serverIp = serverIp;
            _port = port;
        }

        public string SendCommand(string command, string data = "")
        {
            try
            {
                using (Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    client.Connect(new IPEndPoint(IPAddress.Parse(_serverIp), _port));

                    // Création de l'objet JSON à envoyer
                    var commandObject = new CommandModel { Command = command, Data = data };
                    string jsonCommand = JsonSerializer.Serialize(commandObject);

                    // Envoi de la requête
                    byte[] requestBytes = Encoding.UTF8.GetBytes(jsonCommand);
                    client.Send(requestBytes);

                    // Réception de la réponse
                    byte[] buffer = new byte[4096]; // Taille du buffer ajustée
                    int receivedBytes = client.Receive(buffer);

                    // Décodage de la réponse
                    string response = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
                    client.Dispose();
                    return response;
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}