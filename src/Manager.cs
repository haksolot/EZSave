using EZSave.Jobs;

namespace EZSave.Managers {
    public class Manager
    {
        private List<Job> jobs { get; set; } = new List<Job>();
        private int limit { get; set; } = 5;
        private string logDestination { get; set; }
        private string confDestination { get; set; }

        private static Manager instance;
        private static readonly object lockObj = new object();

        private Manager(int limitNew, string destination, string configDestination)
        {
            limit = limitNew;
            logDestination = destination;
            confDestination = configDestination;
        }

        public static Manager GetInstance(int limitNew, string Destination, string ConfigDestination)
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new Manager(limitNew, Destination, ConfigDestination);
                    }
                }
            }
            return instance;
        }

        public void add(Job job) {
            if (jobs.Count >= limit)
            {
                Console.WriteLine("You have exceeded the maximum number of allowed backups (max 5 jobs) !");
            }
            else
            {
                jobs.Add(job);
                Console.WriteLine("The job " + job + " has been successfully added to the list !");
            }
        }

        public void remove(Job job)
        {
            if (jobs.Contains(job))
            {
                jobs.Remove(job);
                Console.WriteLine("The job " + job + " has been deleted from the list !");
            }
            else
            {
                Console.WriteLine("The job " + job + " was not found in the list !");
            }
        }

        public void execute()
        {
            if (jobs.Count > 0)
            {
                foreach (Job job in jobs)
                {
                    job.start();
                    Console.WriteLine("The job " + job + " has been started !");
                }
            }
            else
            {
                Console.WriteLine("The list is empty !");

            }
        }

        public void show()
        {
            foreach (Job job in jobs)
            {
                Console.WriteLine(job);
            }
        }

        //function that create a job and add it to the list
        public void create(string name, string source, string destination, string type)
        {
            Job nouveauJob = new Job(name, source, destination, type);
            add(nouveauJob);
        }

        //function that delete a job config
        public void delete(Job job)
        {
            job.delete(confDestination);
        }

        public void edit(Job job)
        {
            job.save(confDestination);
        }
    }
}