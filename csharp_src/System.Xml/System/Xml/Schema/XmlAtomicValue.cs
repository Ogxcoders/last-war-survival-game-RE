using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.XPath;

namespace System.Xml.Schema;

public sealed class XmlAtomicValue : XPathItem, ICloneable
{
	[StructLayout(LayoutKind.Explicit, Size = 8)]
	private struct Union
	{
		[FieldOffset(0)]
		public bool boolVal;

		[FieldOffset(0)]
		public double dblVal;

		[FieldOffset(0)]
		public long i64Val;

		[FieldOffset(0)]
		public int i32Val;

		[FieldOffset(0)]
		public DateTime dtVal;
	}

	private class NamespacePrefixForQName : IXmlNamespaceResolver
	{
		public string prefix;

		public string ns;

		public NamespacePrefixForQName(string prefix, string ns)
		{
			this.ns = ns;
			this.prefix = prefix;
		}

		public string LookupNamespace(string prefix)
		{
			if (prefix == this.prefix)
			{
				return ns;
			}
			return null;
		}

		public string LookupPrefix(string namespaceName)
		{
			if (ns == namespaceName)
			{
				return prefix;
			}
			return null;
		}

		public IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
		{
			return new Dictionary<string, string>(1) { [prefix] = ns };
		}
	}

	private XmlSchemaType xmlType;

	private object objVal;

	private TypeCode clrType;

	private Union unionVal;

	private NamespacePrefixForQName nsPrefix;

	public override bool IsNode => false;

	public override XmlSchemaType XmlType => xmlType;

	public override Type ValueType => xmlType.Datatype.ValueType;

	public override object TypedValue
	{
		get
		{
			XmlValueConverter valueConverter = xmlType.ValueConverter;
			if (objVal == null)
			{
				switch (clrType)
				{
				case TypeCode.Boolean:
					return valueConverter.ChangeType(unionVal.boolVal, ValueType);
				case TypeCode.Int32:
					return valueConverter.ChangeType(unionVal.i32Val, ValueType);
				case TypeCode.Int64:
					return valueConverter.ChangeType(unionVal.i64Val, ValueType);
				case TypeCode.Double:
					return valueConverter.ChangeType(unionVal.dblVal, ValueType);
				case TypeCode.DateTime:
					return valueConverter.ChangeType(unionVal.dtVal, ValueType);
				}
			}
			return valueConverter.ChangeType(objVal, ValueType, nsPrefix);
		}
	}

	public override bool ValueAsBoolean
	{
		get
		{
			XmlValueConverter valueConverter = xmlType.ValueConverter;
			if (objVal == null)
			{
				switch (clrType)
				{
				case TypeCode.Boolean:
					return unionVal.boolVal;
				case TypeCode.Int32:
					return valueConverter.ToBoolean(unionVal.i32Val);
				case TypeCode.Int64:
					return valueConverter.ToBoolean(unionVal.i64Val);
				case TypeCode.Double:
					return valueConverter.ToBoolean(unionVal.dblVal);
				case TypeCode.DateTime:
					return valueConverter.ToBoolean(unionVal.dtVal);
				}
			}
			return valueConverter.ToBoolean(objVal);
		}
	}

	public override DateTime ValueAsDateTime
	{
		get
		{
			XmlValueConverter valueConverter = xmlType.ValueConverter;
			if (objVal == null)
			{
				switch (clrType)
				{
				case TypeCode.Boolean:
					return valueConverter.ToDateTime(unionVal.boolVal);
				case TypeCode.Int32:
					return valueConverter.ToDateTime(unionVal.i32Val);
				case TypeCode.Int64:
					return valueConverter.ToDateTime(unionVal.i64Val);
				case TypeCode.Double:
					return valueConverter.ToDateTime(unionVal.dblVal);
				case TypeCode.DateTime:
					return unionVal.dtVal;
				}
			}
			return valueConverter.ToDateTime(objVal);
		}
	}

	public override double ValueAsDouble
	{
		get
		{
			XmlValueConverter valueConverter = xmlType.ValueConverter;
			if (objVal == null)
			{
				switch (clrType)
				{
				case TypeCode.Boolean:
					return valueConverter.ToDouble(unionVal.boolVal);
				case TypeCode.Int32:
					return valueConverter.ToDouble(unionVal.i32Val);
				case TypeCode.Int64:
					return valueConverter.ToDouble(unionVal.i64Val);
				case TypeCode.Double:
					return unionVal.dblVal;
				case TypeCode.DateTime:
					return valueConverter.ToDouble(unionVal.dtVal);
				}
			}
			return valueConverter.ToDouble(objVal);
		}
	}

	public override int ValueAsInt
	{
		get
		{
			XmlValueConverter valueConverter = xmlType.ValueConverter;
			if (objVal == null)
			{
				switch (clrType)
				{
				case TypeCode.Boolean:
					return valueConverter.ToInt32(unionVal.boolVal);
				case TypeCode.Int32:
					return unionVal.i32Val;
				case TypeCode.Int64:
					return valueConverter.ToInt32(unionVal.i64Val);
				case TypeCode.Double:
					return valueConverter.ToInt32(unionVal.dblVal);
				case TypeCode.DateTime:
					return valueConverter.ToInt32(unionVal.dtVal);
				}
			}
			return valueConverter.ToInt32(objVal);
		}
	}

	public override long ValueAsLong
	{
		get
		{
			XmlValueConverter valueConverter = xmlType.ValueConverter;
			if (objVal == null)
			{
				switch (clrType)
				{
				case TypeCode.Boolean:
					return valueConverter.ToInt64(unionVal.boolVal);
				case TypeCode.Int32:
					return valueConverter.ToInt64(unionVal.i32Val);
				case TypeCode.Int64:
					return unionVal.i64Val;
				case TypeCode.Double:
					return valueConverter.ToInt64(unionVal.dblVal);
				case TypeCode.DateTime:
					return valueConverter.ToInt64(unionVal.dtVal);
				}
			}
			return valueConverter.ToInt64(objVal);
		}
	}

	public override string Value
	{
		get
		{
			XmlValueConverter valueConverter = xmlType.ValueConverter;
			if (objVal == null)
			{
				switch (clrType)
				{
				case TypeCode.Boolean:
					return valueConverter.ToString(unionVal.boolVal);
				case TypeCode.Int32:
					return valueConverter.ToString(unionVal.i32Val);
				case TypeCode.Int64:
					return valueConverter.ToString(unionVal.i64Val);
				case TypeCode.Double:
					return valueConverter.ToString(unionVal.dblVal);
				case TypeCode.DateTime:
					return valueConverter.ToString(unionVal.dtVal);
				}
			}
			return valueConverter.ToString(objVal, nsPrefix);
		}
	}

	internal XmlAtomicValue(XmlSchemaType xmlType, bool value)
	{
		if (xmlType == null)
		{
			throw new ArgumentNullException("xmlType");
		}
		this.xmlType = xmlType;
		clrType = TypeCode.Boolean;
		unionVal.boolVal = value;
	}

	internal XmlAtomicValue(XmlSchemaType xmlType, DateTime value)
	{
		if (xmlType == null)
		{
			throw new ArgumentNullException("xmlType");
		}
		this.xmlType = xmlType;
		clrType = TypeCode.DateTime;
		unionVal.dtVal = value;
	}

	internal XmlAtomicValue(XmlSchemaType xmlType, double value)
	{
		if (xmlType == null)
		{
			throw new ArgumentNullException("xmlType");
		}
		this.xmlType = xmlType;
		clrType = TypeCode.Double;
		unionVal.dblVal = value;
	}

	internal XmlAtomicValue(XmlSchemaType xmlType, int value)
	{
		if (xmlType == null)
		{
			throw new ArgumentNullException("xmlType");
		}
		this.xmlType = xmlType;
		clrType = TypeCode.Int32;
		unionVal.i32Val = value;
	}

	internal XmlAtomicValue(XmlSchemaType xmlType, long value)
	{
		if (xmlType == null)
		{
			throw new ArgumentNullException("xmlType");
		}
		this.xmlType = xmlType;
		clrType = TypeCode.Int64;
		unionVal.i64Val = value;
	}

	internal XmlAtomicValue(XmlSchemaType xmlType, string value)
	{
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		if (xmlType == null)
		{
			throw new ArgumentNullException("xmlType");
		}
		this.xmlType = xmlType;
		objVal = value;
	}

	internal XmlAtomicValue(XmlSchemaType xmlType, string value, IXmlNamespaceResolver nsResolver)
	{
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		if (xmlType == null)
		{
			throw new ArgumentNullException("xmlType");
		}
		this.xmlType = xmlType;
		objVal = value;
		if (nsResolver != null && (this.xmlType.TypeCode == XmlTypeCode.QName || this.xmlType.TypeCode == XmlTypeCode.Notation))
		{
			string prefixFromQName = GetPrefixFromQName(value);
			nsPrefix = new NamespacePrefixForQName(prefixFromQName, nsResolver.LookupNamespace(prefixFromQName));
		}
	}

	internal XmlAtomicValue(XmlSchemaType xmlType, object value)
	{
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		if (xmlType == null)
		{
			throw new ArgumentNullException("xmlType");
		}
		this.xmlType = xmlType;
		objVal = value;
	}

	internal XmlAtomicValue(XmlSchemaType xmlType, object value, IXmlNamespaceResolver nsResolver)
	{
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		if (xmlType == null)
		{
			throw new ArgumentNullException("xmlType");
		}
		this.xmlType = xmlType;
		objVal = value;
		if (nsResolver != null && (this.xmlType.TypeCode == XmlTypeCode.QName || this.xmlType.TypeCode == XmlTypeCode.Notation))
		{
			string text = (objVal as XmlQualifiedName).Namespace;
			nsPrefix = new NamespacePrefixForQName(nsResolver.LookupPrefix(text), text);
		}
	}

	public XmlAtomicValue Clone()
	{
		return this;
	}

	object ICloneable.Clone()
	{
		return this;
	}

	public override object ValueAs(Type type, IXmlNamespaceResolver nsResolver)
	{
		XmlValueConverter valueConverter = xmlType.ValueConverter;
		if (type == typeof(XPathItem) || type == typeof(XmlAtomicValue))
		{
			return this;
		}
		if (objVal == null)
		{
			switch (clrType)
			{
			case TypeCode.Boolean:
				return valueConverter.ChangeType(unionVal.boolVal, type);
			case TypeCode.Int32:
				return valueConverter.ChangeType(unionVal.i32Val, type);
			case TypeCode.Int64:
				return valueConverter.ChangeType(unionVal.i64Val, type);
			case TypeCode.Double:
				return valueConverter.ChangeType(unionVal.dblVal, type);
			case TypeCode.DateTime:
				return valueConverter.ChangeType(unionVal.dtVal, type);
			}
		}
		return valueConverter.ChangeType(objVal, type, nsResolver);
	}

	public override string ToString()
	{
		return Value;
	}

	private string GetPrefixFromQName(string value)
	{
		int colonOffset;
		int num = ValidateNames.ParseQName(value, 0, out colonOffset);
		if (num == 0 || num != value.Length)
		{
			return null;
		}
		if (colonOffset != 0)
		{
			return value.Substring(0, colonOffset);
		}
		return string.Empty;
	}
}
