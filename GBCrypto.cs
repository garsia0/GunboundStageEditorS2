using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEditor
{
    public class GBCrypto
    {
        public static byte[] Decompress(byte[] Data, int KnowLength)
        {
            List<byte> Source = new List<byte>();
            Source.AddRange(BitConverter.GetBytes((int)KnowLength));
            Source.AddRange(Data);
            LzHuf Temp = new LzHuf();
            Temp.instream = new MemoryStream(Source.ToArray());
            Temp.outstream = new MemoryStream();
            Temp.Decode();
            Temp.instream.Close();
            byte[] Ouput = Temp.outstream.ToArray();
            Temp.outstream.Close();
            return Ouput;
        }

        public static byte[] Compress(byte[] Data)
        {

            LzHuf Temp = new LzHuf();
            Temp.instream = new MemoryStream(Data);
            Temp.outstream = new MemoryStream();
            Temp.Encode();
            Temp.instream.Close();
            Temp.outstream.Seek(4, SeekOrigin.Begin);
            byte[] Ouput = new byte[Temp.outstream.Length - 4];
            Temp.outstream.Read(Ouput, 0, (int)Temp.outstream.Length - 4);
            Temp.outstream.Close();
            return Ouput;
        }

    }
}
