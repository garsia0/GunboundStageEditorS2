/**************************************************************
    lzhuf.c
    written by Haruyasu Yoshizaki 1988/11/20
    some minor changes 1989/04/06
    comments translated by Haruhiko Okumura 1989/04/07
    getbit and getbyte modified 1990/03/23 by Paul Edwards
      so that they would work on machines where integers are
      not necessarily 16 bits (although ANSI guarantees a
      minimum of 16).  This program has compiled and run with
      no errors under Turbo C 2.0, Power C, and SAS/C 4.5
      (running on an IBM mainframe under MVS/XA 2.2).  Could
      people please use YYYY/MM/DD date format so that everyone
      in the world can know what format the date is in?
    external storage of filesize changed 1990/04/18 by Paul Edwards to
      Intel's "little endian" rather than a machine-dependant style so
      that files produced on one machine with lzhuf can be decoded on
      any other.  "little endian" style was chosen since lzhuf
      originated on PC's, and therefore they should dictate the
      standard.
    initialization of something predicting spaces changed 1990/04/22 by
      Paul Edwards so that when the compressed file is taken somewhere
      else, it will decode properly, without changing ascii spaces to
      ebcdic spaces.  This was done by changing the ' ' (space literal)
      to 0x20 (which is the far most likely character to occur, if you
      don't know what environment it will be running on.
**************************************************************/

using System;
using System.IO;

public class LzHuf
{
	public MemoryStream instream, outstream;
	private uint textsize = 0, codesize = 0, printcount = 0;
	private string wterr = "Can't write.";
	private void Error(string message)
	{
		Console.Write("\n{0}\n", message);
	}
	public const int N = 4096; // buffer size
	public const int F = 60; // lookahead buffer size
	public const int THRESHOLD = 2;
	public const int NIL = N;   /* leaf of tree */

	private byte[] Data_Buf = new byte[N + F - 1];
	private int match_position, match_length;

	private int[] lson = new int[N + 1];
	private int[] rson = new int[N + 257];
	private int[] dad = new int[N + 1];

	private void InitTree() // initialize trees
	{
		int i;

		for (i = N + 1; i <= N + 256; i++)
		{
			rson[i] = NIL; // root
		}
		for (i = 0; i < N; i++)
		{
			dad[i] = NIL; // node
		}
	}

	private void InsertNode(int r) // insert to tree
	{
		int i;
		int p;
		int cmp;
		byte[] key;
		uint c;

		cmp = 1;
		//key = &text_buf[r];
		key = new byte[Data_Buf.Length - r];
		Array.Copy(Data_Buf, r, key, 0, Data_Buf.Length - r);
		p = N + 1 + key[0];
		rson[r] = lson[r] = NIL;
		match_length = 0;
		for (; ; )
		{
			if (cmp >= 0)
			{
				if (rson[p] != NIL)
				{
					p = rson[p];
				}
				else
				{
					rson[p] = r;
					dad[r] = p;
					return;
				}
			}
			else
			{
				if (lson[p] != NIL)
				{
					p = lson[p];
				}
				else
				{
					lson[p] = r;
					dad[r] = p;
					return;
				}
			}
			for (i = 1; i < F; i++)
			{
				if ((cmp = key[i] - Data_Buf[p + i]) != 0)
				{
					break;
				}
			}
			if (i > THRESHOLD)
			{
				if (i > match_length)
				{
					match_position = ((r - p) & (N - 1)) - 1;
					if ((match_length = i) >= F)
					{
						break;
					}
				}
				if (i == match_length)
				{
					if ((c = (uint)(((r - p) & (N - 1)) - 1)) < (uint)match_position)
					{
						match_position = (int)c;
					}
				}
			}
		}
		dad[r] = dad[p];
		lson[r] = lson[p];
		rson[r] = rson[p];
		dad[lson[p]] = r;
		dad[rson[p]] = r;
		if (rson[dad[p]] == p)
		{
			rson[dad[p]] = r;
		}
		else
		{
			lson[dad[p]] = r;
		}
		dad[p] = NIL; // remove p
	}
	private void DeleteNode(int p) // remove from tree
	{
		int q;

		if (dad[p] == NIL)
		{
			return; // not registered
		}
		if (rson[p] == NIL)
		{
			q = lson[p];
		}
		else
		{
			if (lson[p] == NIL)
			{
				q = rson[p];
			}
			else
			{
				q = lson[p];
				if (rson[q] != NIL)
				{
					do
					{
						q = rson[q];
					} while (rson[q] != NIL);
					rson[dad[q]] = lson[q];
					dad[lson[q]] = dad[q];
					lson[q] = lson[p];
					dad[lson[p]] = q;
				}
				rson[q] = rson[p];
				dad[rson[p]] = q;
			}
		}
		dad[q] = dad[p];
		if (rson[dad[p]] == p)
		{
			rson[dad[p]] = q;
		}
		else
		{
			lson[dad[p]] = q;
		}
		dad[p] = NIL;
	}

	public const int N_CHAR = 256 - THRESHOLD + F;
	public const int T = N_CHAR * 2 - 1;
	public const int R = T - 1;
	public const int MAX_FREQ = 0x8000;

	/* for encoding */
	static byte[] p_len = { 0x03, 0x04, 0x04, 0x04, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08 };
	static byte[] p_code = { 0x00, 0x20, 0x30, 0x40, 0x50, 0x58, 0x60, 0x68, 0x70, 0x78, 0x80, 0x88, 0x90, 0x94, 0x98, 0x9C, 0xA0, 0xA4, 0xA8, 0xAC, 0xB0, 0xB4, 0xB8, 0xBC, 0xC0, 0xC2, 0xC4, 0xC6, 0xC8, 0xCA, 0xCC, 0xCE, 0xD0, 0xD2, 0xD4, 0xD6, 0xD8, 0xDA, 0xDC, 0xDE, 0xE0, 0xE2, 0xE4, 0xE6, 0xE8, 0xEA, 0xEC, 0xEE, 0xF0, 0xF1, 0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF };
	/* for decoding */
	static byte[] d_code = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x09, 0x09, 0x09, 0x09, 0x09, 0x09, 0x09, 0x09, 0x0A, 0x0A, 0x0A, 0x0A, 0x0A, 0x0A, 0x0A, 0x0A, 0x0B, 0x0B, 0x0B, 0x0B, 0x0B, 0x0B, 0x0B, 0x0B, 0x0C, 0x0C, 0x0C, 0x0C, 0x0D, 0x0D, 0x0D, 0x0D, 0x0E, 0x0E, 0x0E, 0x0E, 0x0F, 0x0F, 0x0F, 0x0F, 0x10, 0x10, 0x10, 0x10, 0x11, 0x11, 0x11, 0x11, 0x12, 0x12, 0x12, 0x12, 0x13, 0x13, 0x13, 0x13, 0x14, 0x14, 0x14, 0x14, 0x15, 0x15, 0x15, 0x15, 0x16, 0x16, 0x16, 0x16, 0x17, 0x17, 0x17, 0x17, 0x18, 0x18, 0x19, 0x19, 0x1A, 0x1A, 0x1B, 0x1B, 0x1C, 0x1C, 0x1D, 0x1D, 0x1E, 0x1E, 0x1F, 0x1F, 0x20, 0x20, 0x21, 0x21, 0x22, 0x22, 0x23, 0x23, 0x24, 0x24, 0x25, 0x25, 0x26, 0x26, 0x27, 0x27, 0x28, 0x28, 0x29, 0x29, 0x2A, 0x2A, 0x2B, 0x2B, 0x2C, 0x2C, 0x2D, 0x2D, 0x2E, 0x2E, 0x2F, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F };
	static byte[] d_len = { 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08 };

	uint[] freq = new uint[T + 1];
	int[] prnt = new int[T + N_CHAR];
	int[] son = new int[T];
	private uint getbuf = 0;
	private byte getlen = 0;

	private int GetBit() // get one bit
	{
		uint i;

		while (getlen <= 8)
		{
			if ((int)(i = (uint)getc(instream)) < 0)
			{
				i = 0;
			}
			getbuf |= i << (8 - getlen);
			getlen += 8;
		}
		i = getbuf;
		getbuf <<= 1;
		getlen--;
		return (int)((i & 0x8000) >> 15);
	}
	private int GetByte() // get one byte
	{
		uint i;

		while (getlen <= 8)
		{
			if ((int)(i = (uint)getc(instream)) < 0)
			{
				i = 0;
			}
			getbuf |= i << (8 - getlen);
			getlen += 8;
		}
		i = getbuf;
		getbuf <<= 8;
		getlen -= 8;
		return (int)((i & 0xff00) >> 8);
	}

	private uint putbuf = 0;
	private byte putlen = 0;

	private void Putcode(int l, uint c) // output c bits of code
	{
		putbuf |= c >> putlen;
		if ((putlen += Convert.ToByte(l)) >= 8)
		{
			if (putc((int)putbuf >> 8, outstream) == -1)
			{
				Error(wterr);
			}
			if ((putlen -= 8) >= 8)
			{
				if (putc((int)putbuf, outstream) == -1)
				{
					Error(wterr);
				}
				codesize += 2;
				putlen -= 8;
				putbuf = (uint)(c << (l - putlen));
			}
			else
			{
				putbuf <<= 8;
				codesize++;
			}
		}
	}
	private void StartHuff()
	{
		int i;
		int j;

		for (i = 0; i < N_CHAR; i++)
		{
			freq[i] = 1;
			son[i] = i + T;
			prnt[i + T] = i;
		}
		i = 0;
		j = N_CHAR;
		while (j <= R)
		{
			freq[j] = freq[i] + freq[i + 1];
			son[j] = i;
			prnt[i] = prnt[i + 1] = j;
			i += 2;
			j++;
		}
		freq[T] = 0xffff;
		prnt[R] = 0;
	}

	public int fgetc(MemoryStream Data)
	{
		try
		{
			return Data.ReadByte();
		}
		catch
		{
			return -1;
		}
	}
	public int getc(MemoryStream Data)
	{
		try
		{
			return Data.ReadByte();
		}
		catch
		{
			return -1;
		}
	}
	public int putc(int value, MemoryStream Data)
	{
		try
		{
			Data.WriteByte((byte)value);
			return value;
		}
		catch
		{
			return -1;
		}
	}
	public int fputc(int value, MemoryStream Data)
	{
		try
		{
			Data.WriteByte((byte)value);
			return value;
		}
		catch
		{
			return -1;
		}
	}

	/* reconstruction of tree */

	private void reconst()
	{
		int i;
		int j;
		int k;
		uint f;
		uint l;

		/* collect leaf nodes in the first half of the table */
		/* and replace the freq by (freq + 1) / 2. */
		j = 0;
		for (i = 0; i < T; i++)
		{
			if (son[i] >= T)
			{
				freq[j] = (freq[i] + 1) / 2;
				son[j] = son[i];
				j++;
			}
		}
		/* begin constructing tree by connecting sons */
		for (i = 0, j = N_CHAR; j < T; i += 2, j++)
		{
			k = i + 1;
			f = freq[j] = freq[i] + freq[k];
			for (k = j - 1; f < freq[k]; k--)
			{
				;
			}
			k++;
			l = (uint)((j - k) * 2);
			Array.Copy(freq, k, freq, k + 1, l);
			freq[k] = f;
			Array.Copy(son, k, son, k + 1, l);
			son[k] = i;
		}
		/* connect prnt */
		for (i = 0; i < T; i++)
		{
			if ((k = son[i]) >= T)
			{
				prnt[k] = i;
			}
			else
			{
				prnt[k] = prnt[k + 1] = i;
			}
		}
	}

	/* increment frequency of given code by one, and update tree */

	private void update(int c)
	{
		int i;
		int j;
		int k;
		int l;

		if (freq[R] == MAX_FREQ)
		{
			reconst();
		}
		c = prnt[c + T];
		do
		{
			k = (int)++freq[c];

			/* if the order is disturbed, exchange nodes */
			if ((uint)k > freq[l = c + 1])
			{
				while ((uint)k > freq[++l])
				{
					;
				}
				l--;
				freq[c] = freq[l];
				freq[l] = (uint)k;

				i = son[c];
				prnt[i] = l;
				if (i < T)
				{
					prnt[i + 1] = l;
				}

				j = son[l];
				son[l] = i;

				prnt[j] = c;
				if (j < T)
				{
					prnt[j + 1] = c;
				}
				son[c] = j;

				c = l;
			}
		} while ((c = prnt[c]) != 0); // repeat up to root
	}

	private uint code;
	private uint len;

	private void EncodeChar(uint c)
	{
		uint i;
		int j;
		int k;

		i = 0;
		j = 0;
		k = prnt[c + T];

		/* travel from leaf to root */
		do
		{
			i >>= 1;

			/* if node's address is odd-numbered, choose bigger brother node */
			if ((k & 1) != 0)
			{
				i += 0x8000;
			}

			j++;
		} while ((k = prnt[k]) != R);
		Putcode(j, i);
		code = i;
		len = (uint)j;
		update((int)c);
	}
	private void EncodePosition(uint c)
	{
		uint i;

		/* output upper 6 bits by table lookup */
		i = (c >> 6);
		Putcode(p_len[i], (uint)p_code[i] << 8);

		/* output lower 6 bits verbatim */
		Putcode(6, (c & 0x3f) << 10);
	}
	private void EncodeEnd()
	{
		if (putlen > 0)
		{
			if (putc((int)putbuf >> 8, outstream) == -1)
			{
				Error(wterr);
			}
			codesize++;
		}
	}
	private int DecodeChar()
	{
		uint c;

		c = (uint)son[R];

		/* travel from root to leaf, */
		/* choosing the smaller child node (son[]) if the read bit is 0, */
		/* the bigger (son[]+1} if 1 */
		while (c < T)
		{
			c += (uint)GetBit();
			c = (uint)son[c];
		}
		c -= T;
		update((int)c);
		return (int)c;
	}
	private int DecodePosition()
	{
		uint i;
		uint j;
		uint c;

		/* recover upper 6 bits from table */
		i = (uint)GetByte();
		c = (uint)((uint)d_code[i] << 6);
		j = d_len[i];

		/* read lower 6 bits verbatim */
		j -= 2;
		while ((j--) != 0)
		{
			i = (i << 1) + (uint)GetBit();
		}
		return (int)(c | (i & 0x3f));
	}
	public void Encode() // compression
	{
		int i;
		int c;
		int len;
		int r;
		int s;
		int last_match_length;

		textsize = (uint)instream.Length;
		fputc((int)((textsize & 0xff)), outstream);
		fputc((int)((textsize & 0xff00) >> 8), outstream);
		fputc((int)((textsize & 0xff0000) >> 16), outstream);
		fputc((int)((textsize & 0xff000000) >> 24), outstream);
		/*
		if (ferror(outfile))
		{
			Error(wterr); // output size of text
		}*/

		if (textsize == 0)
		{
			return;
		}

		textsize = 0; // rewind and re-read
		StartHuff();
		InitTree();
		s = 0;
		r = N - F;
		for (i = s; i < r; i++)
		{
			Data_Buf[i] = 0x20;
		}
		for (len = 0; len < F && (c = getc(instream)) != -1; len++)
		{
			Data_Buf[r + len] = (byte)c;
		}
		textsize = (uint)len;
		for (i = 1; i <= F; i++)
		{
			InsertNode(r - i);
		}
		InsertNode(r);
		do
		{
			if (match_length > len)
			{
				match_length = len;
			}
			if (match_length <= THRESHOLD)
			{
				match_length = 1;
				EncodeChar(Data_Buf[r]);
			}
			else
			{
				EncodeChar((uint)(255 - THRESHOLD + match_length));
				EncodePosition((uint)match_position);
			}
			last_match_length = match_length;
			for (i = 0; i < last_match_length && (c = getc(instream)) != -1; i++)
			{
				DeleteNode(s);
				Data_Buf[s] = (byte)c;
				if (s < F - 1)
				{
					Data_Buf[s + N] = (byte)c;
				}
				s = (s + 1) & (N - 1);
				r = (r + 1) & (N - 1);
				InsertNode(r);
			}
			if ((textsize += (uint)i) > printcount)
			{
				Console.Write("{0,12:D}\r", textsize);
				printcount += 1024;
			}
			while (i++ < last_match_length)
			{
				DeleteNode(s);
				s = (s + 1) & (N - 1);
				r = (r + 1) & (N - 1);
				if ((--len) != 0)
				{
					InsertNode(r);
				}
			}
		} while (len > 0);
		EncodeEnd();
		Console.Write("In : {0:D} bytes\n", textsize);
		Console.Write("Out: {0:D} bytes\n", codesize);
		Console.Write("Out/In: {0:f3}\n", 1.0 * codesize / textsize);
	}
	public void Decode() // recover
	{
		int i;
		int j;
		int k;
		int r;
		int c;
		uint count;

		textsize = ((uint)fgetc(instream));
		textsize |= ((uint)fgetc(instream) << 8);
		textsize |= ((uint)fgetc(instream) << 16);
		textsize |= ((uint)fgetc(instream) << 24);

		/*
	if (ferror(infile))
	{
		// Error("Can't read");  // read size of text
		if (textsize == 0)
		{
			return;
		}
	}*/

		StartHuff();
		for (i = 0; i < N - F; i++)
		{
			Data_Buf[i] = 0x20;
		}
		r = N - F;
		for (count = 0; count < textsize;)
		{
			c = DecodeChar();
			if (c < 256)
			{
				if (putc(c, outstream) == -1)
				{
					Error(wterr);
				}
				Data_Buf[r++] = (byte)c;
				r &= (N - 1);
				count++;
			}
			else
			{
				i = (r - DecodePosition() - 1) & (N - 1);
				j = c - 255 + THRESHOLD;
				for (k = 0; k < j; k++)
				{
					c = Data_Buf[(i + k) & (N - 1)];
					if (putc(c, outstream) == -1)
					{
						Error(wterr);
					}
					Data_Buf[r++] = (byte)c;
					r &= (N - 1);
					count++;
				}
			}
			if (count > printcount)
			{
				Console.Write("{0,12:D}\r", count);
				printcount += 1024;
			}
		}
		Console.Write("{0,12:D}\n", count);
	}
}
