namespace System.ComponentModel.Design.Serialization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
public sealed class DesignerSerializerAttribute : Attribute
{
	private string _typeId;

	public string SerializerTypeName { get; }

	public string SerializerBaseTypeName { get; }

	public override object TypeId
	{
		get
		{
			if (_typeId == null)
			{
				string text = SerializerBaseTypeName;
				int num = text.IndexOf(',');
				if (num != -1)
				{
					text = text.Substring(0, num);
				}
				_typeId = GetType().FullName + text;
			}
			return _typeId;
		}
	}

	public DesignerSerializerAttribute(Type serializerType, Type baseSerializerType)
	{
		SerializerTypeName = serializerType.AssemblyQualifiedName;
		SerializerBaseTypeName = baseSerializerType.AssemblyQualifiedName;
	}

	public DesignerSerializerAttribute(string serializerTypeName, Type baseSerializerType)
	{
		SerializerTypeName = serializerTypeName;
		SerializerBaseTypeName = baseSerializerType.AssemblyQualifiedName;
	}

	public DesignerSerializerAttribute(string serializerTypeName, string baseSerializerTypeName)
	{
		SerializerTypeName = serializerTypeName;
		SerializerBaseTypeName = baseSerializerTypeName;
	}
}
