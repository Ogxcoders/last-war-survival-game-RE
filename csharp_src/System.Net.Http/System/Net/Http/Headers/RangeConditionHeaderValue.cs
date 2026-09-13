namespace System.Net.Http.Headers;

public class RangeConditionHeaderValue : ICloneable
{
	private DateTimeOffset? _date;

	private EntityTagHeaderValue _entityTag;

	public DateTimeOffset? Date => _date;

	public EntityTagHeaderValue EntityTag => _entityTag;

	public RangeConditionHeaderValue(DateTimeOffset date)
	{
		_date = date;
	}

	public RangeConditionHeaderValue(EntityTagHeaderValue entityTag)
	{
		if (entityTag == null)
		{
			throw new ArgumentNullException("entityTag");
		}
		_entityTag = entityTag;
	}

	public RangeConditionHeaderValue(string entityTag)
		: this(new EntityTagHeaderValue(entityTag))
	{
	}

	private RangeConditionHeaderValue(RangeConditionHeaderValue source)
	{
		_entityTag = source._entityTag;
		_date = source._date;
	}

	private RangeConditionHeaderValue()
	{
	}

	public override string ToString()
	{
		if (_entityTag == null)
		{
			return HttpRuleParser.DateToString(_date.Value);
		}
		return _entityTag.ToString();
	}

	public override bool Equals(object obj)
	{
		if (!(obj is RangeConditionHeaderValue rangeConditionHeaderValue))
		{
			return false;
		}
		if (_entityTag == null)
		{
			if (rangeConditionHeaderValue._date.HasValue)
			{
				return _date.Value == rangeConditionHeaderValue._date.Value;
			}
			return false;
		}
		return _entityTag.Equals(rangeConditionHeaderValue._entityTag);
	}

	public override int GetHashCode()
	{
		if (_entityTag == null)
		{
			return _date.Value.GetHashCode();
		}
		return _entityTag.GetHashCode();
	}

	public static RangeConditionHeaderValue Parse(string input)
	{
		int index = 0;
		return (RangeConditionHeaderValue)GenericHeaderParser.RangeConditionParser.ParseValue(input, null, ref index);
	}

	public static bool TryParse(string input, out RangeConditionHeaderValue parsedValue)
	{
		int index = 0;
		parsedValue = null;
		if (GenericHeaderParser.RangeConditionParser.TryParseValue(input, null, ref index, out var parsedValue2))
		{
			parsedValue = (RangeConditionHeaderValue)parsedValue2;
			return true;
		}
		return false;
	}

	internal static int GetRangeConditionLength(string input, int startIndex, out object parsedValue)
	{
		parsedValue = null;
		if (string.IsNullOrEmpty(input) || startIndex + 1 >= input.Length)
		{
			return 0;
		}
		int num = startIndex;
		DateTimeOffset result = DateTimeOffset.MinValue;
		EntityTagHeaderValue parsedValue2 = null;
		char c = input[num];
		char c2 = input[num + 1];
		if (c == '"' || ((c == 'w' || c == 'W') && c2 == '/'))
		{
			int entityTagLength = EntityTagHeaderValue.GetEntityTagLength(input, num, out parsedValue2);
			if (entityTagLength == 0)
			{
				return 0;
			}
			num += entityTagLength;
			if (num != input.Length)
			{
				return 0;
			}
		}
		else
		{
			if (!HttpRuleParser.TryStringToDate(input.Substring(num), out result))
			{
				return 0;
			}
			num = input.Length;
		}
		RangeConditionHeaderValue rangeConditionHeaderValue = new RangeConditionHeaderValue();
		if (parsedValue2 == null)
		{
			rangeConditionHeaderValue._date = result;
		}
		else
		{
			rangeConditionHeaderValue._entityTag = parsedValue2;
		}
		parsedValue = rangeConditionHeaderValue;
		return num - startIndex;
	}

	object ICloneable.Clone()
	{
		return new RangeConditionHeaderValue(this);
	}
}
