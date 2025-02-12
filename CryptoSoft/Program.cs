namespace CryptoSoft;

public static class Program
{
  public static void Main(string[] args)
  {
    try
    {
      if (args.Length < 2)
      {
        Console.WriteLine("Usage: CryptoSoft.exe <file_or_directory> <key>");
        Environment.Exit(-1);
      }

      var cryptoSoft = new Cipher(args[0], args[1]);
      cryptoSoft.Process();
      Environment.Exit(0);
    }
    catch (Exception e)
    {
      Console.WriteLine($"Error: {e.Message}");
      Environment.Exit(-99);
    }
  }
}
