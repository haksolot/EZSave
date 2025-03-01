﻿using System.Diagnostics;
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
        private Dictionary<string, int> _lastProgress = new();
        private Thread? _updateThread;
        private bool _isRunning;

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
                Debug.WriteLine("Client not connected");
                return "Error";
                //return $"Error: {ex.Message}";
            }
        }
        public void StartProgressUpdate(int intervalMs = 5000)
        {
            if (_isRunning) return;
            _isRunning = true;

            _updateThread = new Thread(() =>
            {
                while (_isRunning)
                {
                    var result = SendCommand("getprogress");
                    var jsonResult = JsonSerializer.Deserialize<Dictionary<string, int>>(result);
                    _lastProgress = jsonResult;
                    Thread.Sleep(intervalMs);
                }
            })
            {
                IsBackground = true
            };

            _updateThread.Start();
        }

        public void StopProgressUpdate()
        {
            _isRunning = false;
            _updateThread?.Join();
        }

        public Dictionary<string, int> GetLastProgress() => _lastProgress;
    }
}