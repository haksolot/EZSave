using System.Diagnostics;
using System.Text;

namespace CryptoSoft;

public class Cipher(string path, string key)
{
  private string Path { get; } = path;
  private string Key { get; } = key;

  public void Process()
  {
    if (File.Exists(Path))
    {
      TransformFile(Path);
    }
    else if (Directory.Exists(Path))
    {
      TransformDirectory();
    }
    else
    {
    }
  }

  private void TransformDirectory()
  {
    var files = Directory.GetFiles(Path, "*", SearchOption.AllDirectories)
                         .Where(f => f.EndsWith(".crypto", StringComparison.OrdinalIgnoreCase));

    foreach (var file in files)
    {
      TransformFile(file);
    }
  }

  public Stopwatch TransformFile(string filePath)
  {
    if (!File.Exists(filePath))
    {
      /*Console.WriteLine($"File not found: {filePath}");*/
      return null;
    }
    Stopwatch stopwatch = Stopwatch.StartNew();
    var fileBytes = File.ReadAllBytes(filePath);
    var keyBytes = ConvertToByte(Key);
    fileBytes = XorMethod(fileBytes, keyBytes);
    File.WriteAllBytes(filePath, fileBytes);
    stopwatch.Stop();
    return stopwatch;
    /*Console.WriteLine($"Processed: {filePath} in {stopwatch.ElapsedMilliseconds} ms");*/
  }

  private static byte[] ConvertToByte(string text)
  {
    return Encoding.UTF8.GetBytes(text);
  }

  private static byte[] XorMethod(IReadOnlyList<byte> fileBytes, IReadOnlyList<byte> keyBytes)
  {
    var result = new byte[fileBytes.Count];
    for (var i = 0; i < fileBytes.Count; i++)
    {
      result[i] = (byte)(fileBytes[i] ^ keyBytes[i % keyBytes.Count]);
    }
    return result;
  }
}
