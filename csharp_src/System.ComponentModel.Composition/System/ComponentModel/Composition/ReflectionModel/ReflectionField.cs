using System.Reflection;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.ReflectionModel;

internal class ReflectionField : ReflectionWritableMember
{
	private readonly FieldInfo _field;

	public FieldInfo UndelyingField => _field;

	public override MemberInfo UnderlyingMember => UndelyingField;

	public override bool CanRead => true;

	public override bool CanWrite => !UndelyingField.IsInitOnly;

	public override bool RequiresInstance => !UndelyingField.IsStatic;

	public override Type ReturnType => UndelyingField.FieldType;

	public override ReflectionItemType ItemType => ReflectionItemType.Field;

	public ReflectionField(FieldInfo field)
	{
		Assumes.NotNull(field);
		_field = field;
	}

	public override object GetValue(object instance)
	{
		return UndelyingField.SafeGetValue(instance);
	}

	public override void SetValue(object instance, object value)
	{
		UndelyingField.SafeSetValue(instance, value);
	}
}
