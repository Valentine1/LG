using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.Security.Cryptography;
using LG.Data;

namespace LG.XmlData
{
    delegate XDocument GetXmlDocumnent(object s);
    public partial class DataLoader
    {
        //private async Task<XDocument> LoadAsync(string path)
        //{
        //    StorageFolder sf = await ApplicationData.Current.LocalFolder.GetFolderAsync(@"data\");
        //    StorageFile file = await sf.GetFileAsync(Path.GetFileName(path));
        //    IBuffer ibuf = null;
        //    byte[] b;
        //    bool excCaught = false;
        //    try
        //    {
        //        using (IRandomAccessStream ras = await file.OpenReadAsync())
        //        {
        //            b = new byte[ras.Size];
        //            ibuf = await ras.ReadAsync(CryptographicBuffer.CreateFromByteArray(b), (uint)ras.Size, InputStreamOptions.None);
        //            ras.Dispose();
        //        }
        //    }
        //    catch (System.UnauthorizedAccessException ex)
        //    {
        //        excCaught = true;
        //    }
        //    if (excCaught)
        //    {
        //        using (IRandomAccessStream ras = await file.OpenReadAsync())
        //        {
        //            b = new byte[ras.Size];
        //            ibuf = await ras.ReadAsync(CryptographicBuffer.CreateFromByteArray(b), (uint)ras.Size, InputStreamOptions.None);
        //            ras.Dispose();
        //        }
        //    }
        //    CryptographicBuffer.CopyToByteArray(ibuf, out b);
        //    char[] ch = System.Text.Encoding.UTF8.GetChars(b);
        //    string s = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, ibuf);
        //    s = EncryptionProvider.Decrypt(s, EncryptionProvider.PublicKey);
        //    StringReader sreader = new StringReader(s);
        //    while (sreader.Peek() == 65279)
        //    {
        //        sreader.Read();
        //    }
        //    XDocument xdoc = XDocument.Load(sreader, LoadOptions.None);

        //    return xdoc;

        //}

        private async Task<XDocument> LoadAsync(string path)
        {
            StorageFolder sf = await ApplicationData.Current.LocalFolder.GetFolderAsync(@"data\");
            StorageFile file = await sf.GetFileAsync(Path.GetFileName(path));
            IBuffer ibuf = null;
            byte[] b;
            using (IRandomAccessStream ras = await file.OpenReadAsync())
            {
                using (IInputStream inputStream = ras.GetInputStreamAt(0))
                {
                    ulong size = ras.Size;
                    DataReader dataReader = new DataReader(inputStream);
                    await dataReader.LoadAsync((uint)size);
                    ibuf = dataReader.ReadBuffer((uint)size);
                    inputStream.Dispose();
                }
                ras.Dispose();
            }
            CryptographicBuffer.CopyToByteArray(ibuf, out b);
            // char[] ch = System.Text.Encoding.UTF8.GetChars(b);
            // string s = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, ibuf);
            string s = EncryptionProvider.DecryptToString(ibuf);
            StringReader sreader = new StringReader(s);
            while (sreader.Peek() == 65279)
            {
                sreader.Read();
            }
            XDocument xdoc = XDocument.Load(sreader, LoadOptions.None);
            return xdoc;

        }
        async private Task Save(XDocument xdoc, string path)
        {
            StringWriter swriter = new StringWriter();
            xdoc.Save(swriter, SaveOptions.None);
            string xmlString = swriter.GetStringBuilder().ToString();
            IBuffer xmlEncryptedBuffer = EncryptionProvider.Encrypt(xmlString);

            StorageFolder sf = await ApplicationData.Current.LocalFolder.GetFolderAsync(@"data\");
            var file = await sf.GetFileAsync(Path.GetFileName(path));
            using (IRandomAccessStream ras = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (IOutputStream outs = ras.GetOutputStreamAt(0))
                {
                    await outs.WriteAsync(xmlEncryptedBuffer);
                    bool suc = await outs.FlushAsync();
                    outs.Dispose();
                }
                ras.Dispose();
            }
        }

        //private XDocument Load(string path)
        //{
        //    return XDocument.Load(path);
        //}
        //private void Save(XDocument xdoc, string path)
        //{
        //    xdoc.Save(path);
        //}
    }
}
