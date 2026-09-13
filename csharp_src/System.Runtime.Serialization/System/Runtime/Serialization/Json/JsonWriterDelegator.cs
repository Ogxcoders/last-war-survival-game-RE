using System.Globalization;
using System.Xml;

namespace System.Runtime.Serialization.Json;

internal class JsonWriterDelegator : XmlWriterDelegator
{
	private DateTimeFormat dateTimeFormat;

	public JsonWriterDelegator(XmlWriter writer)
		: base(writer)
	{
	}

	public JsonWriterDelegator(XmlWriter writer, DateTimeFormat dateTimeFormat)
		: this(writer)
	{
		this.dateTimeFormat = dateTimeFormat;
	}

	internal override void WriteChar(char value)
	{
		WriteString(XmlConvert.ToString(value));
	}

	internal override void WriteBase64(byte[] bytes)
	{
		if (bytes != null)
		{
			ByteArrayHelperWithString.Instance.WriteArray(base.Writer, bytes, 0, bytes.Length);
		}
	}

	internal override void WriteQName(XmlQualifiedName value)
	{
		if (value != XmlQualifiedName.Empty)
		{
			writer.WriteString(value.Name);
			writer.WriteString(":");
			writer.WriteString(value.Namespace);
		}
	}

	internal override void WriteUnsignedLong(ulong value)
	{
		WriteDecimal(value);
	}

	internal override void WriteDecimal(decimal value)
	{
		writer.WriteAttributeString("type", "number");
		base.WriteDecimal(value);
	}

	internal override void WriteDouble(double value)
	{
		writer.WriteAttributeString("type", "number");
		base.WriteDouble(value);
	}

	internal override void WriteFloat(float value)
	{
		writer.WriteAttributeString("type", "number");
		base.WriteFloat(value);
	}

	internal override void WriteLong(long value)
	{
		writer.WriteAttributeString("type", "number");
		base.WriteLong(value);
	}

	internal override void WriteSignedByte(sbyte value)
	{
		writer.WriteAttributeString("type", "number");
		base.WriteSignedByte(value);
	}

	internal override void WriteUnsignedInt(uint value)
	{
		writer.WriteAttributeString("type", "number");
		base.WriteUnsignedInt(value);
	}

	internal override void WriteUnsignedShort(ushort value)
	{
		writer.WriteAttributeString("type", "number");
		base.WriteUnsignedShort(value);
	}

	internal override void WriteUnsignedByte(byte value)
	{
		writer.WriteAttributeString("type", "number");
		base.WriteUnsignedByte(value);
	}

	internal override void WriteShort(short value)
	{
		writer.WriteAttributeString("type", "number");
		base.WriteShort(value);
	}

	internal override void WriteBoolean(bool value)
	{
		writer.WriteAttributeString("type", "boolean");
		base.WriteBoolean(value);
	}

	internal override void WriteInt(int value)
	{
		writer.WriteAttributeString("type", "number");
		base.WriteInt(value);
	}

	internal void WriteJsonBooleanArray(bool[] value, XmlDictionaryString itemName, XmlDictionaryString itemNamespace)
	{
		for (int i = 0; i < value.Length; i++)
		{
			WriteBoolean(value[i], itemName, itemNamespace);
		}
	}

	internal void WriteJsonDateTimeArray(DateTime[] value, XmlDictionaryString itemName, XmlDictionaryString itemNamespace)
	{
		for (int i = 0; i < value.Length; i++)
		{
			WriteDateTime(value[i], itemName, itemNamespace);
		}
	}

	internal void WriteJsonDecimalArray(decimal[] value, XmlDictionaryString itemName, XmlDictionaryString itemNamespace)
	{
		for (int i = 0; i < value.Length; i++)
		{
			WriteDecimal(value[i], itemName, itemNamespace);
		}
	}

	internal void WriteJsonInt32Array(int[] value, XmlDictionaryString itemName, XmlDictionaryString itemNamespace)
	{
		for (int i = 0; i < value.Length; i++)
		{
			WriteInt(value[i], itemName, itemNamespace);
		}
	}

	internal void WriteJsonInt64Array(long[] value, XmlDictionaryString itemName, XmlDictionaryString itemNamespace)
	{
		for (int i = 0; i < value.Length; i++)
		{
			WriteLong(value[i], itemName, itemNamespace);
		}
	}

	internal override void WriteDateTime(DateTime value)
	{
		if (dateTimeFormat == null)
		{
			WriteDateTimeInDefaultFormat(value);
		}
		else
		{
			writer.WriteString(value.ToString(dateTimeFormat.FormatString, dateTimeFormat.FormatProvider));
		}
	}

	private void WriteDateTimeInDefaultFormat(DateTime value)
	{
		if (value.Kind != DateTimeKind.Utc)
		{
			long num = (System.LocalAppContextSwitches.DoNotUseTimeZoneInfo ? (value.Ticks - TimeZone.CurrentTimeZone.GetUtcOffset(value).Ticks) : (value.Ticks - TimeZoneInfo.Local.GetUtcOffset(value).Ticks));
			if (num > DateTime.MaxValue.Ticks || num < DateTime.MinValue.Ticks)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("JSON DateTime is out of range."), new ArgumentOutOfRangeException("value")));
			}
		}
		writer.WriteString("/Date(");
		writer.WriteValue((value.ToUniversalTime().Ticks - JsonGlobals.unixEpochTicks) / 10000);
		switch (value.Kind)
		{
		case DateTimeKind.Unspecified:
		case DateTimeKind.Local:
		{
			TimeSpan timeSpan = (System.LocalAppContextSwitches.DoNotUseTimeZoneInfo ? TimeZone.CurrentTimeZone.GetUtcOffset(value.ToLocalTime()) : TimeZoneInfo.Local.GetUtcOffset(value.ToLocalTime()));
			if (timeSpan.Ticks < 0)
			{
				writer.WriteString("-");
			}
			else
			{
				writer.WriteString("+");
			}
			int num2 = Math.Abs(timeSpan.Hours);
			writer.WriteString((num2 < 10) ? ("0" + num2) : num2.ToString(CultureInfo.InvariantCulture));
			int num3 = Math.Abs(timeSpan.Minutes);
			writer.WriteString((num3 < 10) ? ("0" + num3) : num3.ToString(CultureInfo.InvariantCulture));
			break;
		}
		}
		writer.WriteString(")/");
	}

	internal void WriteJsonSingleArray(float[] value, XmlDictionaryString itemName, XmlDictionaryString itemNamespace)
	{
		for (int i = 0; i < value.Length; i++)
		{
			WriteFloat(value[i], itemName, itemNamespace);
		}
	}

	internal void WriteJsonDoubleArray(double[] value, XmlDictionaryString itemName, XmlDictionaryString itemNamespace)
	{
		for (int i = 0; i < value.Length; i++)
		{
			WriteDouble(value[i], itemName, itemNamespace);
		}
	}

	internal override void WriteStartElement(string prefix, string localName, string ns)
	{
		if (localName != null && localName.Length == 0)
		{
			WriteStartElement("item", "item");
			WriteAttributeString(null, "item", null, localName);
		}
		else
		{
			base.WriteStartElement(prefix, localName, ns);
		}
	}
}
