using System;
using System.IO;
using GameFramework;

public static class StringExtensions
{
	public ref struct LineSplitEnumerator
	{
		private ReadOnlySpan<char> _str;

		public LineSplitEntry Current { get; private set; }

		public LineSplitEnumerator(ReadOnlySpan<char> str)
		{
			_str = str;
			Current = default(LineSplitEntry);
		}

		public LineSplitEnumerator GetEnumerator()
		{
			return this;
		}

		public bool MoveNext()
		{
			ReadOnlySpan<char> str = _str;
			if (str.Length == 0)
			{
				return false;
			}
			int num = str.IndexOfAny('\r', '\n');
			if (num == -1)
			{
				_str = ReadOnlySpan<char>.Empty;
				Current = new LineSplitEntry(str, ReadOnlySpan<char>.Empty);
				return true;
			}
			if (num < str.Length - 1 && str[num] == '\r' && str[num + 1] == '\n')
			{
				Current = new LineSplitEntry(str.Slice(0, num), str.Slice(num, 2));
				_str = str.Slice(num + 2);
				return true;
			}
			Current = new LineSplitEntry(str.Slice(0, num), str.Slice(num, 1));
			_str = str.Slice(num + 1);
			return true;
		}
	}

	public readonly ref struct LineSplitEntry
	{
		public ReadOnlySpan<char> Line { get; }

		public ReadOnlySpan<char> Separator { get; }

		public LineSplitEntry(ReadOnlySpan<char> line, ReadOnlySpan<char> separator)
		{
			Line = line;
			Separator = separator;
		}

		public void Deconstruct(out ReadOnlySpan<char> line, out ReadOnlySpan<char> separator)
		{
			line = Line;
			separator = Separator;
		}

		public static implicit operator ReadOnlySpan<char>(LineSplitEntry entry)
		{
			return entry.Line;
		}
	}

	public ref struct SegmentSplitEnumerator
	{
		private ReadOnlySpan<char> _str;

		private char _seg;

		private bool ShouldRemoveEmptyEntries;

		public LineSplitEntry Current { get; private set; }

		public SegmentSplitEnumerator(ReadOnlySpan<char> str, char segment, bool removeEmpty = false)
		{
			_str = str;
			_seg = segment;
			ShouldRemoveEmptyEntries = removeEmpty;
			Current = default(LineSplitEntry);
		}

		public SegmentSplitEnumerator GetEnumerator()
		{
			return this;
		}

		public bool MoveNext()
		{
			ReadOnlySpan<char> str = _str;
			if (str.Length == 0)
			{
				return false;
			}
			LineSplitEntry current;
			do
			{
				int num = str.IndexOf(_seg);
				if (num < 0)
				{
					_str = ReadOnlySpan<char>.Empty;
					Current = new LineSplitEntry(str, ReadOnlySpan<char>.Empty);
					if (ShouldRemoveEmptyEntries)
					{
						current = Current;
						return !current.Line.IsEmpty;
					}
					return true;
				}
				Current = new LineSplitEntry(str.Slice(0, num), str.Slice(num, 1));
				_str = str.Slice(num + 1);
				current = Current;
			}
			while (current.Line.IsEmpty && ShouldRemoveEmptyEntries);
			return true;
		}
	}

	public ref struct StreamSplitEnumerator
	{
		private StreamReader _sr;

		private char[] _buffer;

		private ReadOnlySpan<char> _str;

		public LineSplitEntry Current { get; private set; }

		public StreamSplitEnumerator(StreamReader sr, char[] contentBuffer)
		{
			_sr = sr;
			_buffer = contentBuffer;
			_str = new ReadOnlySpan<char>(_buffer, 0, 0);
			Current = default(LineSplitEntry);
		}

		public StreamSplitEnumerator GetEnumerator()
		{
			return this;
		}

		public bool MoveNext()
		{
			ReadOnlySpan<char> str = _str;
			int num = str.IndexOfAny('\r', '\n');
			if (num == -1)
			{
				if (!prepareBuffer())
				{
					return false;
				}
				str = _str;
				num = str.IndexOfAny('\r', '\n');
				if (num == -1)
				{
					_str = ReadOnlySpan<char>.Empty;
					Current = new LineSplitEntry(str, ReadOnlySpan<char>.Empty);
					return true;
				}
			}
			if (num < str.Length - 1 && str[num] == '\r' && str[num + 1] == '\n')
			{
				Current = new LineSplitEntry(str.Slice(0, num), str.Slice(num, 2));
				_str = str.Slice(num + 2);
				return true;
			}
			Current = new LineSplitEntry(str.Slice(0, num), str.Slice(num, 1));
			_str = str.Slice(num + 1);
			return true;
		}

		private bool prepareBuffer()
		{
			if (_sr.EndOfStream)
			{
				return false;
			}
			int num = 0;
			if (!_str.IsEmpty)
			{
				num = _str.Length;
				int num2 = _buffer.Length - num;
				if (num2 < 0)
				{
					Log.Error("Line to long or buffer to small!!!!");
					return false;
				}
				Array.Copy(_buffer, num2, _buffer, 0, num);
			}
			int num3 = _sr.ReadBlock(_buffer, num, _buffer.Length - num);
			int num4 = num3 + num;
			if (num4 < _buffer.Length)
			{
				Array.Clear(_buffer, num4, _buffer.Length - num4);
			}
			_str = new ReadOnlySpan<char>(_buffer, 0, num4);
			if (num3 + num > _buffer.Length)
			{
				Log.Error("out of buffer !!!");
			}
			return true;
		}
	}

	private static int[] s_IntBuffer;

	public static LineSplitEnumerator SplitLines(this string str)
	{
		return new LineSplitEnumerator(str.AsSpan());
	}

	public static LineSplitEnumerator SplitLines(this ReadOnlySpan<char> str)
	{
		return new LineSplitEnumerator(str);
	}

	public static SegmentSplitEnumerator SplitSegments(this string str, char segment)
	{
		return new SegmentSplitEnumerator(str.AsSpan(), segment);
	}

	public static SegmentSplitEnumerator SplitSegments(this ReadOnlySpan<char> strSpan, char segment, bool removeEmpty = false)
	{
		return new SegmentSplitEnumerator(strSpan, segment);
	}

	public static StreamSplitEnumerator SplitLines(this StreamReader sr)
	{
		char[] contentBuffer = new char[4096];
		return new StreamSplitEnumerator(sr, contentBuffer);
	}

	public static StreamSplitEnumerator SplitLines(this StreamReader sr, char[] contentBuffer)
	{
		return new StreamSplitEnumerator(sr, contentBuffer);
	}

	public static bool Split_to_spanspan(this string str, char ch, out ReadOnlySpan<char> span1, out ReadOnlySpan<char> span2)
	{
		return str.AsSpan().Split_to_spanspan(ch, out span1, out span2);
	}

	public static bool Split_to_spanspan(this ReadOnlySpan<char> span, char ch, out ReadOnlySpan<char> span1, out ReadOnlySpan<char> span2)
	{
		if (span.IsEmpty)
		{
			span1 = ReadOnlySpan<char>.Empty;
			span2 = ReadOnlySpan<char>.Empty;
			return false;
		}
		int num = span.IndexOf(ch);
		if (num < 0)
		{
			span1 = span;
			span2 = ReadOnlySpan<char>.Empty;
			return false;
		}
		span1 = span.Slice(0, num);
		span2 = span.Slice(num + 1);
		return true;
	}

	public static bool Split_to_spans(this ReadOnlySpan<char> span, char ch, out ReadOnlySpan<char> span1, out ReadOnlySpan<char> span2, out ReadOnlySpan<char> span3)
	{
		span.Split_to_spanspan(ch, out span1, out var span4);
		return span4.Split_to_spanspan(ch, out span2, out span3);
	}

	public static bool Split_to_spans(this ReadOnlySpan<char> span, char ch, out ReadOnlySpan<char> span1, out ReadOnlySpan<char> span2, out ReadOnlySpan<char> span3, out ReadOnlySpan<char> span4)
	{
		span.Split_to_spanspan(ch, out span1, out var span5);
		span5.Split_to_spanspan(ch, out span2, out span5);
		return span5.Split_to_spanspan(ch, out span3, out span4);
	}

	public static bool Split_to_spans(this ReadOnlySpan<char> span, char ch, out ReadOnlySpan<char> span1, out ReadOnlySpan<char> span2, out ReadOnlySpan<char> span3, out ReadOnlySpan<char> span4, out ReadOnlySpan<char> span5)
	{
		span.Split_to_spanspan(ch, out span1, out var span6);
		span6.Split_to_spanspan(ch, out span2, out span6);
		span6.Split_to_spanspan(ch, out span3, out span6);
		return span6.Split_to_spanspan(ch, out span4, out span5);
	}

	public static bool Split_to_spans(this ReadOnlySpan<char> span, char ch, out ReadOnlySpan<char> span1, out ReadOnlySpan<char> span2, out ReadOnlySpan<char> span3, out ReadOnlySpan<char> span4, out ReadOnlySpan<char> span5, out ReadOnlySpan<char> span6)
	{
		span.Split_to_spanspan(ch, out span1, out var span7);
		span7.Split_to_spanspan(ch, out span2, out span7);
		span7.Split_to_spanspan(ch, out span3, out span7);
		span7.Split_to_spanspan(ch, out span4, out span7);
		return span7.Split_to_spanspan(ch, out span5, out span6);
	}

	public static bool Split_to_ii(this string str, char ch, out int k, out int v)
	{
		return str.AsSpan().Split_to_ii(ch, out k, out v);
	}

	public static bool Split_to_ii(this ReadOnlySpan<char> span, char ch, out int k, out int v)
	{
		int num = span.IndexOf(ch);
		if (num < 0)
		{
			k = span.ToInt();
			v = 0;
			return false;
		}
		k = span.Slice(0, num).ToInt();
		v = span.Slice(num + 1).ToInt();
		return true;
	}

	public static int IndexOf(this ReadOnlySpan<char> span, char value, int startIndex)
	{
		if (startIndex <= 0)
		{
			return span.IndexOf(value);
		}
		int num = span.Slice(startIndex).IndexOf(value);
		if (num < 0)
		{
			return num;
		}
		return startIndex + num;
	}

	public static int[] Split_to_IntArray(this ReadOnlySpan<char> str, char ch, bool removeEmpty = true, int defInt = 0)
	{
		try
		{
			if (s_IntBuffer == null)
			{
				s_IntBuffer = new int[8192];
			}
			Span<int> span = s_IntBuffer;
			int num = 0;
			int num2 = 0;
			do
			{
				int num3 = str.IndexOf(ch, num2);
				if (num3 < 0)
				{
					num3 = str.Length;
				}
				ReadOnlySpan<char> readOnlySpan = str.Slice(num2, num3 - num2);
				readOnlySpan.Trim();
				if (!readOnlySpan.IsEmpty)
				{
					span[num++] = readOnlySpan.ToInt();
				}
				else if (!removeEmpty)
				{
					span[num++] = defInt;
				}
				if (num >= 8192)
				{
					break;
				}
				num2 = num3 + 1;
			}
			while (num2 < str.Length);
			int[] array = new int[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = span[i];
			}
			return array;
		}
		catch (Exception)
		{
			string[] array2 = str.ToString().Split(new char[1] { ch }, StringSplitOptions.RemoveEmptyEntries);
			if (array2.Length != 0)
			{
				return Array.ConvertAll(array2, int.Parse);
			}
		}
		return new int[0];
	}
}
