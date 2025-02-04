using System;

class Program
{
    static void Main()
    {
        // Création d'un Progress pour un transfert de 10 fichiers
        Progress progress = new Progress(10, 500.0f, "C:\\Source", "D:\\Destination");

        // Création du Status du job
        Status jobStatus = new Status("Backup Job", progress);

        // Lancer le job
        jobStatus.Start();
        jobStatus.ShowState();

        // Simulation du transfert en deux étapes
        jobStatus.JobProgress.UpdateProgress(4);
        jobStatus.ShowProgress();

        jobStatus.JobProgress.UpdateProgress(6);
        jobStatus.ShowProgress();

        // Vérification si terminé
        if (jobStatus.JobProgress.IsComplete())
        {
            jobStatus.Stop();
        }

        jobStatus.ShowState();
    }
}
