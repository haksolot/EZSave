public class Manager
{
    private List<Jobs> jobs { get; set; }
    private int limit { get; set; } = 5
    private string logDestination { get; set; }

    public void add(Jobs job) { 
        if(jobs.Count >= limit)
        {
            Console.WriteLine("You have exceeded the maximum number of allowed backups (max 5 jobs) !" );
        }
        else
        {
            jobs.Add(job);
            Console.WriteLine("The job " + job + " has been successfully added to the list !");
        }
    }

    public void delete(Jobs job)
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

    public execute()
    {
        foreach (Jobs job in jobs)
        {
            start(job);
            Console.WriteLine("The job " + job + " has been started !");
        }
    }

    public void show()
    {
        foreach (Jobs job in jobs)
        {
            Console.WriteLine(job);
        }
    }
}
