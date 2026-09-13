using System.Reflection;

namespace System.Web.Services.Protocols;

public sealed class SoapHeaderMapping
{
	private MemberInfo member;

	private Type header_type;

	private bool is_unknown_header;

	private SoapHeaderDirection direction;

	public SoapHeaderDirection Direction => direction;

	public MemberInfo MemberInfo => member;

	public Type HeaderType => header_type;

	public bool Custom => is_unknown_header;

	[System.MonoTODO]
	public bool Repeats
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	internal SoapHeaderMapping(MemberInfo member, SoapHeaderAttribute attributeInfo)
	{
		this.member = member;
		direction = attributeInfo.Direction;
		if (member is PropertyInfo)
		{
			header_type = ((PropertyInfo)member).PropertyType;
		}
		else
		{
			header_type = ((FieldInfo)member).FieldType;
		}
		if (HeaderType == typeof(SoapHeader) || HeaderType == typeof(SoapUnknownHeader) || HeaderType == typeof(SoapHeader[]) || HeaderType == typeof(SoapUnknownHeader[]))
		{
			is_unknown_header = true;
		}
		else if (!typeof(SoapHeader).IsAssignableFrom(HeaderType))
		{
			throw new InvalidOperationException($"Header members type must be a SoapHeader subclass");
		}
	}

	internal object GetHeaderValue(object ob)
	{
		if (member is PropertyInfo)
		{
			return ((PropertyInfo)member).GetValue(ob, null);
		}
		return ((FieldInfo)member).GetValue(ob);
	}

	internal void SetHeaderValue(object ob, SoapHeader header)
	{
		object value = header;
		if (Custom && HeaderType.IsArray)
		{
			SoapUnknownHeader soapUnknownHeader = header as SoapUnknownHeader;
			SoapUnknownHeader[] array = (SoapUnknownHeader[])GetHeaderValue(ob);
			if (array == null || array.Length == 0)
			{
				value = new SoapUnknownHeader[1] { soapUnknownHeader };
			}
			else
			{
				SoapUnknownHeader[] array2 = new SoapUnknownHeader[array.Length + 1];
				Array.Copy(array, array2, array.Length);
				array2[array.Length] = soapUnknownHeader;
				value = array2;
			}
		}
		if (member is PropertyInfo)
		{
			((PropertyInfo)member).SetValue(ob, value, null);
		}
		else
		{
			((FieldInfo)member).SetValue(ob, value);
		}
	}
}
