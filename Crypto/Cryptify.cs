using System.Security.Cryptography;
using System.Text;

namespace OnaxTools
{
    public class Cryptify
    {
        /// <summary>
        /// For user defined encryption of clear text.
        /// </summary>
        /// <param name="clearText">Clear text input for encryption</param>
        /// <param name="EncryptionKey">[Optional] Specify your own encryption key here. Note that this key being used for encryption should be the same, that would be used for decryption.
        /// This parameter is optional and defaults to the library internal encryption key.</param>
        /// <returns></returns>
        public static string Encrypt(string clearText, string EncryptionKey = null)
        {
            EncryptionKey = String.IsNullOrWhiteSpace(EncryptionKey) ? "MAKV2SPBNI99212" : EncryptionKey;
            string encipherText = string.Empty;
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            try
            {
                using Aes encryptor = Aes.Create();
                Rfc2898DeriveBytes pdb = new(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using MemoryStream ms = new ();
                using (CryptoStream cs = new(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                encipherText = Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError($"[OnaxTools][OnaxEncrypt][Encrypt] :: {Logger.OutputException(ex)}");
            }

            return encipherText;
        }

        /// <summary>
        /// For user defined decryption of user defined encrypted string. 
        /// </summary>
        /// <param name="cipherText">Clear text input for decryption</param>
        /// <param name="DecryptionKey">[Optional] Specify your decryption key here. Note that this key used for decryption should be the same that was used for encryption.
        /// This parameter is optional and defaults to the library internal decryption key.</param>
        /// <returns></returns>
        public static string Decrypt(string cipherText, string DecryptionKey = null)
        {
            DecryptionKey = String.IsNullOrWhiteSpace(DecryptionKey) ? "MAKV2SPBNI99212" : DecryptionKey;
            string decipherText = string.Empty;
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            try
            {
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new(DecryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);

                    using MemoryStream ms = new();
                    using (CryptoStream cs = new(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    decipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError($"[OnaxTools][OnaxDecrypt] :: {Logger.OutputException(ex)}");
            }

            return decipherText;
        }

        /// <summary>
        /// Encryption using the SHA512 Encryptionn algorithm
        /// </summary>
        /// <param name="clearText"></param>
        /// <returns></returns>
        public static string EncryptSHA512(string clearText)
        {
            string encPwd = String.Empty;
            try
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(clearText);
                using var hash = System.Security.Cryptography.SHA512.Create();
                var hashedInputBytes = hash.ComputeHash(bytes);
                StringBuilder hashedInputStringBuilder = new(128);
                encPwd = BitConverter.ToString(hashedInputBytes).Replace("-", "");
            }
            catch (Exception ex)
            {
                Logger.OutputException(ex);
                return null;
            }

            return encPwd;
        }

    }
}
