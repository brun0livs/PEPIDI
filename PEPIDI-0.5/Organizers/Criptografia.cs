using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class Criptografia
{
    private static readonly string chave = "PE*br2025!IDnruo*5Pi!2D0!eBUNr!2"; // 32 caracteres no total

    public static void EncriptarParaFicheiro(string texto, string caminho)
    {
        byte[] bytesChave = Encoding.UTF8.GetBytes(chave.PadRight(32));
        using (Aes aes = Aes.Create())
        {
            aes.Key = bytesChave;
            aes.GenerateIV();

            using (FileStream fs = new FileStream(caminho, FileMode.Create))
            {
                fs.Write(aes.IV, 0, aes.IV.Length); // guardar IV

                using (CryptoStream cs = new CryptoStream(fs, aes.CreateEncryptor(), CryptoStreamMode.Write))
                using (StreamWriter sw = new StreamWriter(cs))
                {
                    sw.Write(texto);
                }
            }
        }
    }

    public static string DesencriptarDeFicheiro(string caminho)
    {
        byte[] bytesChave = Encoding.UTF8.GetBytes(chave.PadRight(32));
        using (FileStream fs = new FileStream(caminho, FileMode.Open))
        {
            byte[] iv = new byte[16];
            fs.Read(iv, 0, iv.Length);

            using (Aes aes = Aes.Create())
            {
                aes.Key = bytesChave;
                aes.IV = iv;

                using (CryptoStream cs = new CryptoStream(fs, aes.CreateDecryptor(), CryptoStreamMode.Read))
                using (StreamReader sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}

