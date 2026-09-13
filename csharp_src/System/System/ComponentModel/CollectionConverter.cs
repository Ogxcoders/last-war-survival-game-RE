using System.Collections;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel;

[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
public class CollectionConverter : TypeConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == null)
		{
			throw new ArgumentNullException("destinationType");
		}
		if (destinationType == typeof(string) && value is ICollection)
		{
			return global::SR.GetString("(Collection)");
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

	public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
	{
		return null;
	}

	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return false;
	}
}
