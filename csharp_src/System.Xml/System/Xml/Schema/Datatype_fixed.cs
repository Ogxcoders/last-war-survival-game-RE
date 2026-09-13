namespace System.Xml.Schema;

internal class Datatype_fixed : Datatype_decimal
{
	public override object ParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
	{
		Exception ex;
		try
		{
			Numeric10FacetsChecker obj = FacetsChecker as Numeric10FacetsChecker;
			decimal num = XmlConvert.ToDecimal(s);
			ex = obj.CheckTotalAndFractionDigits(num, 18, 4, checkTotal: true, checkFraction: true);
			if (ex == null)
			{
				return num;
			}
		}
		catch (XmlSchemaException ex2)
		{
			throw ex2;
		}
		catch (Exception innerException)
		{
			throw new XmlSchemaException(Res.GetString("The value '{0}' is invalid according to its data type.", s), innerException);
		}
		throw ex;
	}

	internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
	{
		typedValue = null;
		decimal result;
		Exception ex = XmlConvert.TryToDecimal(s, out result);
		if (ex == null)
		{
			ex = (FacetsChecker as Numeric10FacetsChecker).CheckTotalAndFractionDigits(result, 18, 4, checkTotal: true, checkFraction: true);
			if (ex == null)
			{
				typedValue = result;
				return null;
			}
		}
		return ex;
	}
}
