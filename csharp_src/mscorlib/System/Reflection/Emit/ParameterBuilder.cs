using System.Runtime.InteropServices;

namespace System.Reflection.Emit;

[StructLayout(LayoutKind.Sequential)]
public class ParameterBuilder
{
	private MethodBase methodb;

	private string name;

	private CustomAttributeBuilder[] cattrs;

	private UnmanagedMarshal marshal_info;

	private ParameterAttributes attrs;

	private int position;

	private int table_idx;

	private object def_value;

	public virtual int Attributes => (int)attrs;

	public bool IsIn => (attrs & ParameterAttributes.In) != 0;

	public bool IsOut => (attrs & ParameterAttributes.Out) != 0;

	public bool IsOptional => (attrs & ParameterAttributes.Optional) != 0;

	public virtual string Name => name;

	public virtual int Position => position;

	internal ParameterBuilder(MethodBase mb, int pos, ParameterAttributes attributes, string strParamName)
	{
		name = strParamName;
		position = pos;
		attrs = attributes;
		methodb = mb;
		if (mb is DynamicMethod)
		{
			table_idx = 0;
		}
		else
		{
			table_idx = mb.get_next_table_index(this, 8, 1);
		}
	}

	public virtual ParameterToken GetToken()
	{
		return new ParameterToken(8 | table_idx);
	}

	public virtual void SetConstant(object defaultValue)
	{
		if (position > 0)
		{
			TypeBuilder.SetConstantValue(methodb.GetParameterType(position - 1), defaultValue, ref defaultValue);
		}
		def_value = defaultValue;
		attrs |= ParameterAttributes.HasDefault;
	}

	public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
	{
		switch (customBuilder.Ctor.ReflectedType.FullName)
		{
		case "System.Runtime.InteropServices.InAttribute":
			attrs |= ParameterAttributes.In;
			return;
		case "System.Runtime.InteropServices.OutAttribute":
			attrs |= ParameterAttributes.Out;
			return;
		case "System.Runtime.InteropServices.OptionalAttribute":
			attrs |= ParameterAttributes.Optional;
			return;
		case "System.Runtime.InteropServices.MarshalAsAttribute":
			attrs |= ParameterAttributes.HasFieldMarshal;
			marshal_info = CustomAttributeBuilder.get_umarshal(customBuilder, is_field: false);
			return;
		case "System.Runtime.InteropServices.DefaultParameterValueAttribute":
			SetConstant(CustomAttributeBuilder.decode_cattr(customBuilder).ctorArgs[0]);
			return;
		}
		if (cattrs != null)
		{
			CustomAttributeBuilder[] array = new CustomAttributeBuilder[cattrs.Length + 1];
			cattrs.CopyTo(array, 0);
			array[cattrs.Length] = customBuilder;
			cattrs = array;
		}
		else
		{
			cattrs = new CustomAttributeBuilder[1];
			cattrs[0] = customBuilder;
		}
	}

	[ComVisible(true)]
	public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
	{
		SetCustomAttribute(new CustomAttributeBuilder(con, binaryAttribute));
	}

	[Obsolete("An alternate API is available: Emit the MarshalAs custom attribute instead.")]
	public virtual void SetMarshal(UnmanagedMarshal unmanagedMarshal)
	{
		marshal_info = unmanagedMarshal;
		attrs |= ParameterAttributes.HasFieldMarshal;
	}
}
