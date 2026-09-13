namespace System.Net.Http.Headers;

public class EntityTagHeaderValue : ICloneable
{
	private static EntityTagHeaderValue s_any;

	private string _tag;

	private bool _isWeak;

	public string Tag => _tag;

	public bool IsWeak => _isWeak;

	public static EntityTagHeaderValue Any
	{
		get
		{
			if (s_any == null)
			{
				s_any = new EntityTagHeaderValue();
				s_any._tag = "*";
				s_any._isWeak = false;
			}
			return s_any;
		}
	}

	public EntityTagHeaderValue(string tag)
		: this(tag, isWeak: false)
	{
	}

	public EntityTagHeaderValue(string tag, bool isWeak)
	{
		if (string.IsNullOrEmpty(tag))
		{
			throw new ArgumentException("The value cannot be null or empty.", "tag");
		}
		int length = 0;
		if (HttpRuleParser.GetQuotedStringLength(tag, 0, out length) != HttpParseResult.Parsed || length != tag.Length)
		{
			throw new FormatException("The specified value is not a valid quoted string.");
		}
		_tag = tag;
		_isWeak = isWeak;
	}

	private EntityTagHeaderValue(EntityTagHeaderValue source)
	{
		_tag = source._tag;
		_isWeak = source._isWeak;
	}

	private EntityTagHeaderValue()
	{
	}

	public override string ToString()
	{
		if (_isWeak)
		{
			return "W/" + _tag;
		}
		return _tag;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is EntityTagHeaderValue entityTagHeaderValue))
		{
			return false;
		}
		if (_isWeak == entityTagHeaderValue._isWeak)
		{
			return string.Equals(_tag, entityTagHeaderValue._tag, StringComparison.Ordinal);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return _tag.GetHashCode() ^ _isWeak.GetHashCode();
	}

	public static EntityTagHeaderValue Parse(string input)
	{
		int index = 0;
		return (EntityTagHeaderValue)GenericHeaderParser.SingleValueEntityTagParser.ParseValue(input, null, ref index);
	}

	public static bool TryParse(string input, out EntityTagHeaderValue parsedValue)
	{
		int index = 0;
		parsedValue = null;
		if (GenericHeaderParser.SingleValueEntityTagParser.TryParseValue(input, null, ref index, out var parsedValue2))
		{
			parsedValue = (EntityTagHeaderValue)parsedValue2;
			return true;
		}
		return false;
	}

	internal static int GetEntityTagLength(string input, int startIndex, out EntityTagHeaderValue parsedValue)
	{
		parsedValue = null;
		if (string.IsNullOrEmpty(input) || startIndex >= input.Length)
		{
			return 0;
		}
		bool isWeak = false;
		int num = startIndex;
		char c = input[startIndex];
		if (c == '*')
		{
			parsedValue = Any;
			num++;
		}
		else
		{
			if (c == 'W' || c == 'w')
			{
				num++;
				if (num + 2 >= input.Length || input[num] != '/')
				{
					return 0;
				}
				isWeak = true;
				num++;
				num += HttpRuleParser.GetWhitespaceLength(input, num);
			}
			int startIndex2 = num;
			int length = 0;
			if (HttpRuleParser.GetQuotedStringLength(input, num, out length) != HttpParseResult.Parsed)
			{
				return 0;
			}
			parsedValue = new EntityTagHeaderValue();
			if (length == input.Length)
			{
				parsedValue._tag = input;
				parsedValue._isWeak = false;
			}
			else
			{
				parsedValue._tag = input.Substring(startIndex2, length);
				parsedValue._isWeak = isWeak;
			}
			num += length;
		}
		num += HttpRuleParser.GetWhitespaceLength(input, num);
		return num - startIndex;
	}

	object ICloneable.Clone()
	{
		if (this == s_any)
		{
			return s_any;
		}
		return new EntityTagHeaderValue(this);
	}
}
