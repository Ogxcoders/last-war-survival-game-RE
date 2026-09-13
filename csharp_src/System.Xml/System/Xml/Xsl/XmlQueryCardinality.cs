using System.IO;

namespace System.Xml.Xsl;

internal struct XmlQueryCardinality
{
	private int value;

	private static readonly XmlQueryCardinality[,] cardinalityProduct = new XmlQueryCardinality[8, 8]
	{
		{ None, Zero, None, Zero, None, Zero, None, Zero },
		{ Zero, Zero, Zero, Zero, Zero, Zero, Zero, Zero },
		{ None, Zero, One, ZeroOrOne, More, NotOne, OneOrMore, ZeroOrMore },
		{ Zero, Zero, ZeroOrOne, ZeroOrOne, NotOne, NotOne, ZeroOrMore, ZeroOrMore },
		{ None, Zero, More, NotOne, More, NotOne, More, NotOne },
		{ Zero, Zero, NotOne, NotOne, NotOne, NotOne, NotOne, NotOne },
		{ None, Zero, OneOrMore, ZeroOrMore, More, NotOne, OneOrMore, ZeroOrMore },
		{ Zero, Zero, ZeroOrMore, ZeroOrMore, NotOne, NotOne, ZeroOrMore, ZeroOrMore }
	};

	private static readonly XmlQueryCardinality[,] cardinalitySum = new XmlQueryCardinality[8, 8]
	{
		{ None, Zero, One, ZeroOrOne, More, NotOne, OneOrMore, ZeroOrMore },
		{ Zero, Zero, One, ZeroOrOne, More, NotOne, OneOrMore, ZeroOrMore },
		{ One, One, More, OneOrMore, More, OneOrMore, More, OneOrMore },
		{ ZeroOrOne, ZeroOrOne, OneOrMore, ZeroOrMore, More, ZeroOrMore, OneOrMore, ZeroOrMore },
		{ More, More, More, More, More, More, More, More },
		{ NotOne, NotOne, OneOrMore, ZeroOrMore, More, NotOne, OneOrMore, ZeroOrMore },
		{ OneOrMore, OneOrMore, More, OneOrMore, More, OneOrMore, More, OneOrMore },
		{ ZeroOrMore, ZeroOrMore, OneOrMore, ZeroOrMore, More, ZeroOrMore, OneOrMore, ZeroOrMore }
	};

	private static readonly string[] toString = new string[8] { "", "?", "", "?", "+", "*", "+", "*" };

	private static readonly string[] serialized = new string[8] { "None", "Zero", "One", "ZeroOrOne", "More", "NotOne", "OneOrMore", "ZeroOrMore" };

	public static XmlQueryCardinality None => new XmlQueryCardinality(0);

	public static XmlQueryCardinality Zero => new XmlQueryCardinality(1);

	public static XmlQueryCardinality One => new XmlQueryCardinality(2);

	public static XmlQueryCardinality ZeroOrOne => new XmlQueryCardinality(3);

	public static XmlQueryCardinality More => new XmlQueryCardinality(4);

	public static XmlQueryCardinality NotOne => new XmlQueryCardinality(5);

	public static XmlQueryCardinality OneOrMore => new XmlQueryCardinality(6);

	public static XmlQueryCardinality ZeroOrMore => new XmlQueryCardinality(7);

	private XmlQueryCardinality(int value)
	{
		this.value = value;
	}

	public bool Equals(XmlQueryCardinality other)
	{
		return value == other.value;
	}

	public static bool operator ==(XmlQueryCardinality left, XmlQueryCardinality right)
	{
		return left.value == right.value;
	}

	public static bool operator !=(XmlQueryCardinality left, XmlQueryCardinality right)
	{
		return left.value != right.value;
	}

	public override bool Equals(object other)
	{
		if (other is XmlQueryCardinality)
		{
			return Equals((XmlQueryCardinality)other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return value;
	}

	public static XmlQueryCardinality operator |(XmlQueryCardinality left, XmlQueryCardinality right)
	{
		return new XmlQueryCardinality(left.value | right.value);
	}

	public static XmlQueryCardinality operator &(XmlQueryCardinality left, XmlQueryCardinality right)
	{
		return new XmlQueryCardinality(left.value & right.value);
	}

	public static XmlQueryCardinality operator *(XmlQueryCardinality left, XmlQueryCardinality right)
	{
		return cardinalityProduct[left.value, right.value];
	}

	public static XmlQueryCardinality operator +(XmlQueryCardinality left, XmlQueryCardinality right)
	{
		return cardinalitySum[left.value, right.value];
	}

	public static bool operator <=(XmlQueryCardinality left, XmlQueryCardinality right)
	{
		return (left.value & ~right.value) == 0;
	}

	public static bool operator >=(XmlQueryCardinality left, XmlQueryCardinality right)
	{
		return (right.value & ~left.value) == 0;
	}

	public XmlQueryCardinality AtMost()
	{
		return new XmlQueryCardinality(value | (value >> 1) | (value >> 2));
	}

	public bool NeverSubset(XmlQueryCardinality other)
	{
		if (value != 0)
		{
			return (value & other.value) == 0;
		}
		return false;
	}

	public string ToString(string format)
	{
		if (format == "S")
		{
			return serialized[value];
		}
		return ToString();
	}

	public override string ToString()
	{
		return toString[value];
	}

	public XmlQueryCardinality(string s)
	{
		value = 0;
		for (int i = 0; i < serialized.Length; i++)
		{
			if (s == serialized[i])
			{
				value = i;
				break;
			}
		}
	}

	public void GetObjectData(BinaryWriter writer)
	{
		writer.Write((byte)value);
	}

	public XmlQueryCardinality(BinaryReader reader)
		: this(reader.ReadByte())
	{
	}
}
