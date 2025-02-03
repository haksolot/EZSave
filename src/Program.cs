using System;
// See https://aka.ms/new-console-template for more information

public partial class Program
{
    public static void Main(string[] args)
    {
        var slog = new Log
        {
            name = "test",
            datetime = DateTime.Now,
            source = "C:/Users/Utilisateur/EZSave",
            destination = "C:/Users/Utilisateur/EZSave/save",
            size = 100,
            tt = 2000
        };
        slog.write();
    }
}