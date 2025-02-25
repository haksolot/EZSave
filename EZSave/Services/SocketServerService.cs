using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
        private Dictionary<string, (Thread thread, CancellationTokenSource Cts, ManualResetEvent PauseEvent, string Status)> jobStates;
        private Action _refresh;
        public SocketServerService(Action refresh, int port = 6969, ManagerModel managerModel = null, ConfigFileModel configFileModel = null, Dictionary<string, (Thread thread, CancellationTokenSource Cts, ManualResetEvent PauseEvent, string Status)> state = null)
        {
            _port = port;
            _managerModel = managerModel;
            _configFileModel = configFileModel;
            jobStates = state;
            _refresh = refresh;
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
                    try
                    {
                        JobModel? job = JsonSerializer.Deserialize<JobModel>(command.Data);
                        _managerService.Add(job, _managerModel);
                        _configService.SaveJob(job, _configFileModel);
                        client.Send(Encoding.UTF8.GetBytes("Success"));
                        _refresh();
                        break;
                    }
                    catch
                    {
                        client.Send(Encoding.UTF8.GetBytes("Error"));
                        break;
                    }

                case "deljob":
                    try
                    {
                        JobModel? job = JsonSerializer.Deserialize<JobModel>(command.Data);
                        _managerService.RemoveJob(job, _managerModel);
                        _configService.SaveConfigFile(_configFileModel);
                        client.Send(Encoding.UTF8.GetBytes("Success"));
                        _refresh();
                        break;
                    }
                    catch
                    {
                        client.Send(Encoding.UTF8.GetBytes("Error"));
                        break;
                    }

                case "editjob":
                    try
                    {
                        JobModel? job = JsonSerializer.Deserialize<JobModel>(command.Data);
                        _configFileModel.Jobs[job.Name] = job;
                        _configService.SaveConfigFile(_configFileModel);
                        client.Send(Encoding.UTF8.GetBytes("Success"));
                        _refresh();
                        break;
                    }
                    catch
                    {
                        client.Send(Encoding.UTF8.GetBytes("Error"));
                        break;
                    }

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
                    catch { client.Send(Encoding.UTF8.GetBytes("Error")); break; }

                case "playjob":
                    try
                    {
                        var playJobData = JsonSerializer.Deserialize<PlayJobModel>(command.Data);
                        var listeSelected = playJobData.selected;
                        var job2playName = playJobData.Name;
                        _managerService.ExecuteSelected(jobStates, listeSelected, _managerModel, _configFileModel, job2playName);
                        client.Send(Encoding.UTF8.GetBytes("Success"));
                        break;
                    }
                    catch { client.Send(Encoding.UTF8.GetBytes("Error")); break; }

                case "pausejob":
                    try
                    {
                        var job2pauseData = JsonSerializer.Deserialize<JobControlModel>(command.Data);
                        string job2pauseName = job2pauseData.Name;
                        _managerService.Pause(job2pauseName, jobStates);
                        client.Send(Encoding.UTF8.GetBytes("Success"));
                        break;
                    }
                    catch { client.Send(Encoding.UTF8.GetBytes("Error")); break; }

                case "stopjob":
                    try
                    {
                        var job2StopData = JsonSerializer.Deserialize<JobControlModel>(command.Data);
                        string job2StopName = job2StopData.Name;
                        _managerService.Pause(job2StopName, jobStates);
                        client.Send(Encoding.UTF8.GetBytes("Success"));
                        break;
                    }
                    catch { client.Send(Encoding.UTF8.GetBytes("Error")); break; }

                default:
                    client.Send(Encoding.UTF8.GetBytes("Unknown"));
                    client.Dispose();
                    break;
            }
        }
    }
}