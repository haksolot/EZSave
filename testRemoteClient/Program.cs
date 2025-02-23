using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using EZSave.Core.Models;

namespace SocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // Créer un objet JobModel
            JobModel job = new JobModel
            {
                Name = "BackupJob1",
                Source = "C:\\source\\folder",
                Destination = "D:\\destination\\folder",
                Type = "FullBackup"
            };

            // Sérialiser l'objet JobModel en JSON
            string jsonJob = JsonSerializer.Serialize(job);

            // Créer la commande avec le job sérialisé dans le champ Data
            CommandModel command = new CommandModel
            {
                Command = "addJob",
                Data = jsonJob // Le champ Data contient ici le JSON du job
            };

            // Sérialiser la commande en JSON
            string jsonCommand = JsonSerializer.Serialize(command);

            // Se connecter au serveur (par exemple, localhost et port 6969)
            using (TcpClient client = new TcpClient("127.0.0.1", 6969))
            {
                NetworkStream stream = client.GetStream();
                byte[] data = Encoding.UTF8.GetBytes(jsonCommand);

                // Envoyer la commande au serveur
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Commande envoyée :");
                Console.WriteLine(jsonCommand);

                // Lire la réponse du serveur
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);

                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Réponse du serveur : " + response);
            }

            Console.WriteLine("Appuyez sur une touche pour fermer.");
            Console.ReadKey();
        }
    }
}


// ###### GETING JOB LIST ! ######

//using System.Collections.ObjectModel;
//using System.Net.Sockets;
//using System.Text;
//using System.Text.Json;
//using EZSave.Core.Models;

//Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//client.Connect("127.0.0.1", 6969);

//// Envoie la commande
//string request = JsonSerializer.Serialize(new CommandModel { Command = "getJobList", Data = "" });
//client.Send(Encoding.UTF8.GetBytes(request));

//// Réception de la réponse
//byte[] buffer = new byte[4096]; // Augmenter la taille si nécessaire
//int count = client.Receive(buffer);
//string response = Encoding.UTF8.GetString(buffer, 0, count);

//// Désérialisation en ObservableCollection<JobModel>
//var jobs = JsonSerializer.Deserialize<ObservableCollection<JobModel>>(response);

//// Affichage des jobs reçus
//foreach (var job in jobs)
//{
//    Console.WriteLine($"Nom: {job.Name}, Source: {job.Source}, Destination: {job.Destination}, Type: {job.Type}");
//}

//client.Dispose();
