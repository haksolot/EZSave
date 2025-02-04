using System;
// See https://aka.ms/new-console-template for more information

public partial class Program
{
    public static void Main(string[] args)
    {
        var slog1 = new Log
        {
            name = "test",
            datetime = DateTime.Now,
            source = "C:/Users/Utilisateur/EZSave",
            destination = "C:/Users/Utilisateur/EZSave/save",
            size = 100,
            tt = 2000
        };
       
        var slog2 = new Log
        {
            name = "test2",
            datetime = DateTime.Now,
            source = "C:/Users/Utilisateur/EZSave",
            destination = "C:/Users/Utilisateur/EZSave/save",
            size = 100,
            tt = 2000
        };
        slog1.write();
        slog1.size=80;
        slog1.tt=1500;
        slog1.write();
        slog1.show();
        slog2.write();
        slog2.show();
    }
}