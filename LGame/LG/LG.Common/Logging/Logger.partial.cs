using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Security.Cryptography;

namespace LG.Logging
{
    public partial class Logger
    {
        async public static void Log(string s)
        {
            using (var releaser = await myLock.LockAsync())
            {
                StorageFile sfile = await ApplicationData.Current.LocalFolder.CreateFileAsync("errorlog.txt", CreationCollisionOption.OpenIfExists);
                using (IRandomAccessStream rasw = await sfile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    IBuffer ibuf = null;
                    using (IInputStream inputStream = rasw.GetInputStreamAt(0))
                    {
                        ulong size = rasw.Size;
                        DataReader dataReader = new DataReader(inputStream);
                        await dataReader.LoadAsync((uint)size);
                        ibuf = dataReader.ReadBuffer((uint)size);
                        inputStream.Dispose();
                    }

                    if(ibuf.Length < 64000)
                    {
                        await rasw.WriteAsync(ibuf);
                    }

                    await rasw.WriteAsync(CryptographicBuffer.ConvertStringToBinary("\r\n"+ DateTime.Now.ToUniversalTime().ToString() + " : " + s, BinaryStringEncoding.Utf8));
                    rasw.Seek(0);
                    await rasw.FlushAsync();
                    rasw.Dispose();
                }
            }
        }
    }
}
