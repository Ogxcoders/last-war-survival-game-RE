using System.Globalization;

namespace System.Xml.Schema;

internal class TypedObject
{
	private class DecimalStruct
	{
		private bool isDecimal;

		private decimal[] dvalue;

		public bool IsDecimal
		{
			get
			{
				return isDecimal;
			}
			set
			{
				isDecimal = value;
			}
		}

		public decimal[] Dvalue => dvalue;

		public DecimalStruct()
		{
			dvalue = new decimal[1];
		}

		public DecimalStruct(int dim)
		{
			dvalue = new decimal[dim];
		}
	}

	private DecimalStruct dstruct;

	private object ovalue;

	private string svalue;

	private XmlSchemaDatatype xsdtype;

	private int dim = 1;

	private bool isList;

	public int Dim => dim;

	public bool IsList => isList;

	public bool IsDecimal => dstruct.IsDecimal;

	public decimal[] Dvalue => dstruct.Dvalue;

	public object Value
	{
		get
		{
			return ovalue;
		}
		set
		{
			ovalue = value;
		}
	}

	public XmlSchemaDatatype Type
	{
		get
		{
			return xsdtype;
		}
		set
		{
			xsdtype = value;
		}
	}

	public TypedObject(object obj, string svalue, XmlSchemaDatatype xsdtype)
	{
		ovalue = obj;
		this.svalue = svalue;
		this.xsdtype = xsdtype;
		if (xsdtype.Variety == XmlSchemaDatatypeVariety.List || xsdtype is Datatype_base64Binary || xsdtype is Datatype_hexBinary)
		{
			isList = true;
			dim = ((Array)obj).Length;
		}
	}

	public override string ToString()
	{
		return svalue;
	}

	public void SetDecimal()
	{
		if (dstruct != null)
		{
			return;
		}
		XmlTypeCode typeCode = xsdtype.TypeCode;
		if (typeCode == XmlTypeCode.Decimal || (uint)(typeCode - 40) <= 12u)
		{
			if (isList)
			{
				dstruct = new DecimalStruct(dim);
				for (int i = 0; i < dim; i++)
				{
					dstruct.Dvalue[i] = Convert.ToDecimal(((Array)ovalue).GetValue(i), NumberFormatInfo.InvariantInfo);
				}
			}
			else
			{
				dstruct = new DecimalStruct();
				dstruct.Dvalue[0] = Convert.ToDecimal(ovalue, NumberFormatInfo.InvariantInfo);
			}
			dstruct.IsDecimal = true;
		}
		else if (isList)
		{
			dstruct = new DecimalStruct(dim);
		}
		else
		{
			dstruct = new DecimalStruct();
		}
	}

	private bool ListDValueEquals(TypedObject other)
	{
		for (int i = 0; i < Dim; i++)
		{
			if (Dvalue[i] != other.Dvalue[i])
			{
				return false;
			}
		}
		return true;
	}

	public bool Equals(TypedObject other)
	{
		if (Dim != other.Dim)
		{
			return false;
		}
		if (Type != other.Type)
		{
			if (!Type.IsComparable(other.Type))
			{
				return false;
			}
			other.SetDecimal();
			SetDecimal();
			if (IsDecimal && other.IsDecimal)
			{
				return ListDValueEquals(other);
			}
		}
		if (IsList)
		{
			if (other.IsList)
			{
				return Type.Compare(Value, other.Value) == 0;
			}
			Array array = Value as Array;
			if (array is XmlAtomicValue[] array2)
			{
				if (array2.Length == 1)
				{
					return array2.GetValue(0).Equals(other.Value);
				}
				return false;
			}
			if (array.Length == 1)
			{
				return array.GetValue(0).Equals(other.Value);
			}
			return false;
		}
		if (other.IsList)
		{
			Array array3 = other.Value as Array;
			if (array3 is XmlAtomicValue[] array4)
			{
				if (array4.Length == 1)
				{
					return array4.GetValue(0).Equals(Value);
				}
				return false;
			}
			if (array3.Length == 1)
			{
				return array3.GetValue(0).Equals(Value);
			}
			return false;
		}
		return Value.Equals(other.Value);
	}
}
