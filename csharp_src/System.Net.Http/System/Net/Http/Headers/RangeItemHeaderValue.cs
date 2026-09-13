using System.Collections.Generic;
using System.Globalization;

namespace System.Net.Http.Headers;

public class RangeItemHeaderValue : ICloneable
{
	private long? _from;

	private long? _to;

	public long? From => _from;

	public long? To => _to;

	public RangeItemHeaderValue(long? from, long? to)
	{
		if (!from.HasValue && !to.HasValue)
		{
			throw new ArgumentException("Invalid range. At least one of the two parameters must not be null.");
		}
		if (from.HasValue && from.Value < 0)
		{
			throw new ArgumentOutOfRangeException("from");
		}
		if (to.HasValue && to.Value < 0)
		{
			throw new ArgumentOutOfRangeException("to");
		}
		if (from.HasValue && to.HasValue && from.Value > to.Value)
		{
			throw new ArgumentOutOfRangeException("from");
		}
		_from = from;
		_to = to;
	}

	private RangeItemHeaderValue(RangeItemHeaderValue source)
	{
		_from = source._from;
		_to = source._to;
	}

	public override string ToString()
	{
		if (!_from.HasValue)
		{
			return "-" + _to.Value.ToString(NumberFormatInfo.InvariantInfo);
		}
		if (!_to.HasValue)
		{
			return _from.Value.ToString(NumberFormatInfo.InvariantInfo) + "-";
		}
		return _from.Value.ToString(NumberFormatInfo.InvariantInfo) + "-" + _to.Value.ToString(NumberFormatInfo.InvariantInfo);
	}

	public override bool Equals(object obj)
	{
		if (!(obj is RangeItemHeaderValue rangeItemHeaderValue))
		{
			return false;
		}
		if (_from == rangeItemHeaderValue._from)
		{
			return _to == rangeItemHeaderValue._to;
		}
		return false;
	}

	public override int GetHashCode()
	{
		if (!_from.HasValue)
		{
			return _to.GetHashCode();
		}
		if (!_to.HasValue)
		{
			return _from.GetHashCode();
		}
		return _from.GetHashCode() ^ _to.GetHashCode();
	}

	internal static int GetRangeItemListLength(string input, int startIndex, ICollection<RangeItemHeaderValue> rangeCollection)
	{
		if (string.IsNullOrEmpty(input) || startIndex >= input.Length)
		{
			return 0;
		}
		bool separatorFound = false;
		int nextNonEmptyOrWhitespaceIndex = HeaderUtilities.GetNextNonEmptyOrWhitespaceIndex(input, startIndex, skipEmptyValues: true, out separatorFound);
		if (nextNonEmptyOrWhitespaceIndex == input.Length)
		{
			return 0;
		}
		RangeItemHeaderValue parsedValue = null;
		do
		{
			int rangeItemLength = GetRangeItemLength(input, nextNonEmptyOrWhitespaceIndex, out parsedValue);
			if (rangeItemLength == 0)
			{
				return 0;
			}
			rangeCollection.Add(parsedValue);
			nextNonEmptyOrWhitespaceIndex += rangeItemLength;
			nextNonEmptyOrWhitespaceIndex = HeaderUtilities.GetNextNonEmptyOrWhitespaceIndex(input, nextNonEmptyOrWhitespaceIndex, skipEmptyValues: true, out separatorFound);
			if (nextNonEmptyOrWhitespaceIndex < input.Length && !separatorFound)
			{
				return 0;
			}
		}
		while (nextNonEmptyOrWhitespaceIndex != input.Length);
		return nextNonEmptyOrWhitespaceIndex - startIndex;
	}

	internal static int GetRangeItemLength(string input, int startIndex, out RangeItemHeaderValue parsedValue)
	{
		parsedValue = null;
		if (string.IsNullOrEmpty(input) || startIndex >= input.Length)
		{
			return 0;
		}
		int num = startIndex;
		int offset = num;
		int numberLength = HttpRuleParser.GetNumberLength(input, num, allowDecimal: false);
		if (numberLength > 19)
		{
			return 0;
		}
		num += numberLength;
		num += HttpRuleParser.GetWhitespaceLength(input, num);
		if (num == input.Length || input[num] != '-')
		{
			return 0;
		}
		num++;
		num += HttpRuleParser.GetWhitespaceLength(input, num);
		int offset2 = num;
		int num2 = 0;
		if (num < input.Length)
		{
			num2 = HttpRuleParser.GetNumberLength(input, num, allowDecimal: false);
			if (num2 > 19)
			{
				return 0;
			}
			num += num2;
			num += HttpRuleParser.GetWhitespaceLength(input, num);
		}
		if (numberLength == 0 && num2 == 0)
		{
			return 0;
		}
		long result = 0L;
		if (numberLength > 0 && !HeaderUtilities.TryParseInt64(input, offset, numberLength, out result))
		{
			return 0;
		}
		long result2 = 0L;
		if (num2 > 0 && !HeaderUtilities.TryParseInt64(input, offset2, num2, out result2))
		{
			return 0;
		}
		if (numberLength > 0 && num2 > 0 && result > result2)
		{
			return 0;
		}
		parsedValue = new RangeItemHeaderValue((numberLength == 0) ? ((long?)null) : new long?(result), (num2 == 0) ? ((long?)null) : new long?(result2));
		return num - startIndex;
	}

	object ICloneable.Clone()
	{
		return new RangeItemHeaderValue(this);
	}
}
