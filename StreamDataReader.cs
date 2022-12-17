using System;
using System.Collections.Generic;
using System.Text;

public class StreamDataReader
{
    private byte[] Buffer;
    private int Offset = 0;

    public StreamDataReader(byte[] Data)
    {
        Buffer = Data;
    }

    protected internal int ReadInt32()
    {
        int res = BitConverter.ToInt32(Buffer, Offset);
        Offset += 4;
        return res;
    }

    protected internal void SetOffset(Int32 Data)
    {
        Offset = Data;
    }
    protected internal UInt32 ReadUInt32()
    {
        UInt32 res = BitConverter.ToUInt32(Buffer, Offset);
        Offset += 4;
        return res;
    }

    protected internal byte ReadByte()
    {
        byte res = Buffer[Offset];
        Offset += 1;
        return res;
    }

    protected internal byte[] ReadBytes(int Length)
    {
        byte[] result = new byte[Length];
        Array.Copy(Buffer, Offset, result, 0, Length);
        Offset += Length;
        return result;
    }

    protected internal short ReadInt16()
    {
        short res = BitConverter.ToInt16(Buffer, Offset);
        Offset += 2;
        return res;
    }

    protected internal double ReadDouble()
    {
        double res = BitConverter.ToDouble(Buffer, Offset);
        Offset += 8;
        return res;
    }

    protected internal long ReadInt64()
    {
        long res = BitConverter.ToInt64(Buffer, Offset);
        Offset += 8;
        return res;
    }

    protected internal string ReadPStringFixed(int Length)
    {
        String Temp = System.Text.Encoding.ASCII.GetString(Buffer, Offset, Length);
        Offset += Length;

        if (String.IsNullOrEmpty(Temp))
        {
            return "";
        }
        else
        {
            int Index = Temp.IndexOf('\0');
            if (Index > -1)
            {
                return Temp.Substring(0, Index);
            }
            else
            {
                return Temp;
            }
        }
    }


    protected internal void Ignore(int inOffset)
    {
        Offset = Offset + inOffset;
    }

    protected internal long CurrentOffset
    {
        get
        {
            return Offset;
        }
    }

    protected internal int Length
    {
        get
        {
            return Buffer.Length;
        }

    }
}

