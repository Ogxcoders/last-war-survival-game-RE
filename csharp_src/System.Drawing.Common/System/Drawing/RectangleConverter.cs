using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Drawing;

public class RectangleConverter : TypeConverter
{
	public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
	{
		if (sourceType == typeof(string))
		{
			return true;
		}
		return base.CanConvertFrom(context, sourceType);
	}

	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		if (value is string text)
		{
			string text2 = text.Trim();
			if (text2.Length == 0)
			{
				return null;
			}
			if (culture == null)
			{
				culture = CultureInfo.CurrentCulture;
			}
			char c = culture.TextInfo.ListSeparator[0];
			string[] array = text2.Split(new char[1] { c });
			int[] array2 = new int[array.Length];
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = (int)converter.ConvertFromString(context, culture, array[i]);
			}
			if (array2.Length == 4)
			{
				return new Rectangle(array2[0], array2[1], array2[2], array2[3]);
			}
			throw new ArgumentException(global::SR.Format("Text \"{0}\" cannot be parsed. The expected text format is \"{1}\".", "text", text2, "x, y, width, height"));
		}
		return base.ConvertFrom(context, culture, value);
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == null)
		{
			throw new ArgumentNullException("destinationType");
		}
		if (value is Rectangle)
		{
			if (destinationType == typeof(string))
			{
				Rectangle rectangle = (Rectangle)value;
				if (culture == null)
				{
					culture = CultureInfo.CurrentCulture;
				}
				string separator = culture.TextInfo.ListSeparator + " ";
				TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
				string[] array = new string[4];
				int num = 0;
				array[num++] = converter.ConvertToString(context, culture, rectangle.X);
				array[num++] = converter.ConvertToString(context, culture, rectangle.Y);
				array[num++] = converter.ConvertToString(context, culture, rectangle.Width);
				array[num++] = converter.ConvertToString(context, culture, rectangle.Height);
				return string.Join(separator, array);
			}
			if (destinationType == typeof(InstanceDescriptor))
			{
				Rectangle rectangle2 = (Rectangle)value;
				ConstructorInfo constructor = typeof(Rectangle).GetConstructor(new Type[4]
				{
					typeof(int),
					typeof(int),
					typeof(int),
					typeof(int)
				});
				if (constructor != null)
				{
					return new InstanceDescriptor(constructor, new object[4] { rectangle2.X, rectangle2.Y, rectangle2.Width, rectangle2.Height });
				}
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

	public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
	{
		if (propertyValues == null)
		{
			throw new ArgumentNullException("propertyValues");
		}
		object obj = propertyValues["X"];
		object obj2 = propertyValues["Y"];
		object obj3 = propertyValues["Width"];
		object obj4 = propertyValues["Height"];
		if (obj == null || obj2 == null || obj3 == null || obj4 == null || !(obj is int) || !(obj2 is int) || !(obj3 is int) || !(obj4 is int))
		{
			throw new ArgumentException(global::SR.Format("IDictionary parameter contains at least one entry that is not valid. Ensure all values are consistent with the object's properties."));
		}
		return new Rectangle((int)obj, (int)obj2, (int)obj3, (int)obj4);
	}

	public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
	{
		return TypeDescriptor.GetProperties(typeof(Rectangle), attributes).Sort(new string[4] { "X", "Y", "Width", "Height" });
	}

	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return true;
	}
}
