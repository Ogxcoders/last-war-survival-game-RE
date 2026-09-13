using System.Globalization;

namespace System.Xml;

internal struct BinXmlSqlMoney
{
	private long data;

	public BinXmlSqlMoney(int v)
	{
		data = v;
	}

	public BinXmlSqlMoney(long v)
	{
		data = v;
	}

	public decimal ToDecimal()
	{
		bool isNegative;
		ulong num;
		if (data < 0)
		{
			isNegative = true;
			num = (ulong)(-data);
		}
		else
		{
			isNegative = false;
			num = (ulong)data;
		}
		return new decimal((int)num, (int)(num >> 32), 0, isNegative, 4);
	}

	public override string ToString()
	{
		return ToDecimal().ToString("#0.00##", CultureInfo.InvariantCulture);
	}
}
