using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.Storage.Streams;
using Windows.ApplicationModel;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Security.Cryptography.Certificates;
using Windows.Security.Cryptography.DataProtection;
using LG.Logging;

namespace LG.Data
{
    public static partial class EncryptionProvider
    {
        /// <summary> 
        /// Encrypt a string 
        /// </summary> 
        /// <param name="input">String to encrypt</param> 
        /// <param name="password">Password to use for encryption</param> 
        /// <returns>Encrypted string</returns> 
        public static IBuffer Encrypt(string input)
        {
            // get IV, key and encrypt 
            var iv = CryptographicBuffer.CreateFromByteArray(UTF8Encoding.UTF8.GetBytes(EncryptionProvider.PublicKey));
            SymmetricKeyAlgorithmProvider provider = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbc);
            IBuffer buffer = CryptographicBuffer.ConvertStringToBinary(EncryptionProvider.PublicKey, BinaryStringEncoding.Utf8);
            CryptographicKey key = provider.CreateSymmetricKey(buffer);
            IBuffer binput = CryptographicBuffer.ConvertStringToBinary(input, BinaryStringEncoding.Utf8);

            // Append the padding (before encrypting)
            long remainder = binput.Length % provider.BlockLength;
            long newSize = binput.Length + provider.BlockLength - remainder;
            byte[] b;
            CryptographicBuffer.CopyToByteArray(binput, out b);

            Array.Resize(ref b, (int)newSize);
            IBuffer binput2 = CryptographicBuffer.CreateFromByteArray(b);
            IBuffer encryptedBuffer = CryptographicEngine.Encrypt(key, binput2, iv);

            return encryptedBuffer;
        }

        /// <summary> 
        /// Encrypt ibuffer
        /// </summary> 
        /// <param name="input">ibuffer to encrypt</param> 
        /// <param name="password">Password to use for encryption</param> 
        /// <returns>Encrypted IBuffer</returns> 
        public static IBuffer Encrypt(IBuffer ibuf)
        {
            // get IV, key and encrypt 
            var iv = CryptographicBuffer.CreateFromByteArray(UTF8Encoding.UTF8.GetBytes(EncryptionProvider.PublicKey));
            SymmetricKeyAlgorithmProvider provider = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbc);
            IBuffer buffer = CryptographicBuffer.ConvertStringToBinary(EncryptionProvider.PublicKey, BinaryStringEncoding.Utf8);
            CryptographicKey key = provider.CreateSymmetricKey(buffer);

            // Append the padding (before encrypting)
            long remainder = ibuf.Length % provider.BlockLength;
            long newSize = ibuf.Length + provider.BlockLength - remainder;
            byte[] b;
            CryptographicBuffer.CopyToByteArray(ibuf, out b);
            if (newSize > b.Length)
            {
                if (b[b.Length - 1] == 0)
                {
                    b[b.Length - 1] = 1;
                }
                Array.Resize(ref b, (int)newSize);
            }
            IBuffer binput2 = CryptographicBuffer.CreateFromByteArray(b);
            IBuffer encryptedBuffer = CryptographicEngine.Encrypt(key, binput2, iv);
            CryptographicBuffer.CopyToByteArray(encryptedBuffer, out b);
            return encryptedBuffer;
        }

        /// <summary> 
        /// Decrypt a string previously ecnrypted with Encrypt method and the same password 
        /// </summary> 
        /// <param name="input">String to decrypt</param> 
        /// <param name="password">Password to use for decryption</param> 
        /// <returns>Decrypted string</returns> 
        public static string DecryptToString(IBuffer ibuf)
        {
            // get IV, key and decrypt 
            var iv = CryptographicBuffer.CreateFromByteArray(UTF8Encoding.UTF8.GetBytes(EncryptionProvider.PublicKey));
            SymmetricKeyAlgorithmProvider provider = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbc);
            var buffer = CryptographicBuffer.ConvertStringToBinary(EncryptionProvider.PublicKey, BinaryStringEncoding.Utf8);
            var key = provider.CreateSymmetricKey(buffer);
            //IBuffer binput = CryptographicBuffer.DecodeFromBase64String(input);
            IBuffer decryptedBuffer = CryptographicEngine.Decrypt(key, ibuf, iv);
            byte[] b;
            CryptographicBuffer.CopyToByteArray(decryptedBuffer, out b);
            char[] ch = System.Text.Encoding.UTF8.GetChars(b);

            StringBuilder sb = new StringBuilder();
            foreach (char c in ch)
            {
                if (c == '\0')
                {
                    break;
                }
                sb.Append(c);
            }
            string s = sb.ToString();
            try
            {
                return (CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, decryptedBuffer)).TrimEnd(new char[] { '\0' });
            }
            catch (Exception ex)
            {
                return s;
            }
        }
        /// <summary> 
        /// Decrypt IBuffer previously ecnrypted with Encrypt method and the same password 
        /// </summary> 
        /// <param name="input">IBuffer to decrypt</param> 
        /// <param name="password">Password to use for decryption</param> 
        /// <returns>Decrypted IBuffer</returns> 
        public static IBuffer Decrypt(IBuffer ibuf)
        {
            // get IV, key and decrypt 
            var iv = CryptographicBuffer.CreateFromByteArray(UTF8Encoding.UTF8.GetBytes(EncryptionProvider.PublicKey));
            SymmetricKeyAlgorithmProvider provider = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbc);
            var buffer = CryptographicBuffer.ConvertStringToBinary(EncryptionProvider.PublicKey, BinaryStringEncoding.Utf8);
            var key = provider.CreateSymmetricKey(buffer);
            byte[] b;
            CryptographicBuffer.CopyToByteArray(ibuf, out b);
            IBuffer decryptedBuffer = CryptographicEngine.Decrypt(key, ibuf, iv);

            CryptographicBuffer.CopyToByteArray(decryptedBuffer, out b);
            int newSize = b.Length;
            for (int i = b.Length - 1; i >= 0; i--)
            {
                if (b[i] == 0)
                {
                    newSize--;
                }
                else
                {
                    break;
                }
            }
            Array.Resize(ref b, (int)newSize);
            return CryptographicBuffer.CreateFromByteArray(b); 
        }

        async public static Task<IRandomAccessStream> DecryptMedia(StorageFile file)
        {
            IRandomAccessStream ras = await file.OpenAsync(FileAccessMode.Read);
            IBuffer ibuf = new Windows.Storage.Streams.Buffer((uint)ras.Size);
            ibuf = await ras.ReadAsync(ibuf, (uint)ras.Size, InputStreamOptions.None);
            IBuffer ibuf2 = EncryptionProvider.Decrypt(ibuf);
            InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream();
            await ms.WriteAsync(ibuf2);
            ms.Seek(0);
            return ms;
        }
    }
}
