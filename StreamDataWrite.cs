using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;


public class StreamDataWrite
{
    private List<byte> mstream = new List<byte>();

    protected internal void WriteBytes(byte[] value)
    {
        mstream.AddRange(value);
    }


    protected internal void WriteStringFixed(string value, int Size)
    {
        if (value != null)
        {
            WriteBytes(Encoding.ASCII.GetBytes(value));
            if (value.Length > Size)
            {
                Console.WriteLine("Error Writing String Data");
            }
            WriteBytes(new byte[Size - value.Length]);
        }
        else
        {
            WriteBytes(new byte[Size]);
        }
    }

    protected internal void WriteInt32(int value)
    {
        WriteBytes(BitConverter.GetBytes(value));
    }

    protected internal void WriteInt16(short val)
    {
        WriteBytes(BitConverter.GetBytes(val));
    }

    protected internal void WriteUInt16(UInt16 val)
    {
        WriteBytes(BitConverter.GetBytes(val));
    }
    protected internal void WriteUInt32(UInt32 val)
    {
        WriteBytes(BitConverter.GetBytes(val));
    }

    protected internal void WriteByte(byte value)
    {
        mstream.Add(value);
    }

    protected internal void WriteDouble(double value)
    {
        WriteBytes(BitConverter.GetBytes(value));
    }

    protected internal void WriteInt64(long value)
    {
        WriteBytes(BitConverter.GetBytes(value));
    }

    public byte[] ToByteArray()
    {
        byte[] R = mstream.ToArray();
        mstream.Clear();
        return R;
    }
    public long Length
    {
        get { return mstream.Count; }
    }
}

