using Hartalega.FloorSystem.Framework.DbExceptionLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Hartalega.FloorSystem.Framework.Common
{
   public static class EncryptDecrypt
    {

       public static string GetDecryptedString(string Message, string Passphrase)
       {
           byte[] Results;
           System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

           MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
           byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

           TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

           TDESAlgorithm.Key = TDESKey;
           TDESAlgorithm.Mode = CipherMode.ECB;
           TDESAlgorithm.Padding = PaddingMode.PKCS7;

           byte[] DataToDecrypt = Convert.FromBase64String(Message);

           try
           {
               ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
               Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
           }
           catch (Exception ex)
           {
               throw new FloorSystemException(Messages.GETDECRYPTEDSTRINGMETHODEXCEPTION, Constants.ENCRYPTDECRYPT, ex);
           }

           finally
           {
               TDESAlgorithm.Clear();
               HashProvider.Clear();
           }

           return UTF8.GetString(Results);
       }

       public static string GetEncryptedString(string Message, string Passphrase)
       {
           byte[] Results;
           System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
           MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
           byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));
           TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
           TDESAlgorithm.Key = TDESKey;
           TDESAlgorithm.Mode = CipherMode.ECB;
           TDESAlgorithm.Padding = PaddingMode.PKCS7;
           byte[] DataToEncrypt = UTF8.GetBytes(Message);
           try
           {
               ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
               Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
           }

           catch (Exception ex)
           {
               throw new FloorSystemException(Messages.GETENCRYPTEDSTRINGMETHODEXCEPTION, Constants.ENCRYPTDECRYPT, ex);
           }

           finally
           {
               TDESAlgorithm.Clear();
               HashProvider.Clear();
           }
           return Convert.ToBase64String(Results);
       }
    }
}
