using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using EZSave.Core.Models;

namespace EZSave.Core.Services
{
    public class SocketServerService
    {
        private readonly int _port;
        private ManagerModel _managerModel;
        private ManagerService _managerService = new ManagerService();
        private ConfigFileModel _configFileModel;
        private ConfigService _configService = new ConfigService();
        public SocketServerService(int port = 6969, ManagerModel managerModel = null, ConfigFileModel configFileModel = null)
        {
            _port = port;
            _managerModel = managerModel;
            _configFileModel = configFileModel;
        }

        public void Start()
        {
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            EndPoint endPoint = new IPEndPoint(IPAddress.Any, _port);
            listener.Bind(endPoint);
            listener.Listen(16);
            while (true)
            {
                try
                {
                    Socket client = listener.Accept();
                    new Thread(_ => ListenToClient(client)).Start();
                }
                catch { break; }
            }
        }

        public void ListenToClient(Socket client)
        {
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[1024];
                    int count = client.Receive(buffer);

                    if (count == 0)
                        break;

                    string request = Encoding.UTF8.GetString(buffer, 0, count);
                    var command = JsonSerializer.Deserialize<CommandModel>(request);

                    if (command != null)
                        ProcessCommand(client, command);
                }
            }
            catch
            {

            }
            finally
            {
                //client.Dispose();
            }
        }

        private void ProcessCommand(Socket client, CommandModel command)
        {
            switch (command.Command.ToLower())
            {
                case "getjoblist":
                    string jsonResponse = JsonSerializer.Serialize(_managerModel.Jobs);
                    client.Send(Encoding.UTF8.GetBytes(jsonResponse));
                    break;

                case "addjob":
                    JobModel? job = JsonSerializer.Deserialize<JobModel>(command.Data);
                    var state = addJob(job.Name, job.Source, job.Destination, job.Type);
                    if (state)
                        client.Send(Encoding.UTF8.GetBytes("Success"));
                    else
                        client.Send(Encoding.UTF8.GetBytes("Error"));
                    break;

                case "getconf":
                    string jsonConf = JsonSerializer.Serialize(_configFileModel);
                    client.Send(Encoding.UTF8.GetBytes(jsonConf));
                    break;

                case "saveconf":
                    try
                    {
                        ConfigFileModel? newConfig = JsonSerializer.Deserialize<ConfigFileModel>(command.Data);
                        if (newConfig != null)
                        {
                            _configFileModel = newConfig;
                            client.Send(Encoding.UTF8.GetBytes("Success"));
                            break;
                        }
                        else
                        {
                            client.Send(Encoding.UTF8.GetBytes("Error"));
                            break;
                        }
                    }
                    catch
                    {
                        client.Send(Encoding.UTF8.GetBytes("Error"));
                        break;
                    }

                case "playjob":

                case "pausejob":

                default:
                    client.Send(Encoding.UTF8.GetBytes("Unknown"));
                    client.Dispose();
                    break;
            }
        }

        private bool addJob(string name, string source, string destination, string type)
        {
            try
            {
                var job = new JobModel();
                job.Name = name;
                job.Source = source;
                job.Destination = destination;
                job.Type = type;
                _managerService.Add(job, _managerModel);
                _configService.SaveJob(job, _configFileModel);

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}