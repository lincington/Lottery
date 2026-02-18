using System.Security.Cryptography;

namespace Common
{

    // byte[] key = new byte[32] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
    //                             0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
    //                             0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
    //                             0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
    //    }; // 256位密钥
    //byte[] iv = new byte[16] {
    //                             0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
    //                             0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ;// 128位IV
    //FileEncryptor.EncryptFile("A.txt", "B.txt", key, iv);
    //FileEncryptor.DecryptFile("B.txt", "C.txt", key, iv);
    //Console.WriteLine(File.ReadAllText("C.txt"));
    //SQLServerHelper sQLServerHelper = new SQLServerHelper();
    // await  sQLServerHelper.MatrixRedBlue();

    public static class FileEncryptor
    {
        public static void EncryptFile(string inputFile, string outputFile, byte[] key, byte[] iv)
        {
            using (FileStream fsInput = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
            using (FileStream fsOutput = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            using (Aes aes = Aes.Create())
            using (CryptoStream cs = new CryptoStream(fsOutput, aes.CreateEncryptor(key, iv), CryptoStreamMode.Write))
            {
                fsInput.CopyTo(cs);
            }
        }

        // 解密文件
        public static void DecryptFile(string inputPath, string outputPath, byte[] key, byte[] iv)
        {
            using (FileStream inputFile = new FileStream(inputPath, FileMode.Open, FileAccess.Read))
            using (FileStream outputFile = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
            using (Aes aes = Aes.Create())
            using (CryptoStream cryptoStream = new CryptoStream(inputFile, aes.CreateDecryptor(key, iv), CryptoStreamMode.Read))
            {
                cryptoStream.CopyTo(outputFile);
            }
        }
    }
}
