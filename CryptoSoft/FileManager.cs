﻿using System.Diagnostics;
using System.Text;

namespace CryptoSoft;

public class Cipher(string path, string key)
{
    private static readonly object obj = new object();
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



    public float TransformFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return 0f;
        }

        Stopwatch stopwatch = Stopwatch.StartNew();
        lock (obj) 
        {
            byte[] fileBytes;

          
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fileBytes = new byte[fs.Length];
                fs.Read(fileBytes, 0, fileBytes.Length);
            }

            
            byte[] keyBytes = ConvertToByte(Key);
            fileBytes = XorMethod(fileBytes, keyBytes);

           
            string tempFilePath = filePath + ".temp";
            using (FileStream fs = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                fs.Write(fileBytes, 0, fileBytes.Length);
            }

          
            File.Delete(filePath); 
            File.Move(tempFilePath, filePath); 
        }

        stopwatch.Stop();
        return stopwatch.ElapsedMilliseconds;
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