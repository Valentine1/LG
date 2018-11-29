using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LG.XmlData
{
    public partial class DataLoader
    {
        public async Task<XDocument> LoadAsync(string filePath)
        {
            using (FileStream sourceStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true))
            {
                StringBuilder sb = new StringBuilder();

                byte[] buffer = new byte[0x1000];
                int numRead;
                while ((numRead =  sourceStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string text = Encoding.UTF8.GetString(buffer, 0, numRead);
                    sb.Append(text);
                }

                StringReader sreader = new StringReader(sb.ToString());
                while (sreader.Peek() == 65279)
                {
                    sreader.Read();
                }
                XDocument xdoc = XDocument.Load(sreader, LoadOptions.None);

                return xdoc;
            }
        }

        private async Task Save(XDocument xdoc, string filePath)
        {
            StringWriter swriter = new StringWriter();
            xdoc.Save(swriter, SaveOptions.None);
            byte[] encodedText = Encoding.UTF8.GetBytes(swriter.GetStringBuilder().ToString());

            using (FileStream sourceStream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
            {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            };
        }
    }
}
