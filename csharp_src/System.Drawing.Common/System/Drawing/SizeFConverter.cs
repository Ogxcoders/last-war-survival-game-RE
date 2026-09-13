using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Drawing;

public class SizeFConverter : TypeConverter
{
	private static readonly string[] s_propertySort = new string[2] { "Width", "Height" };

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
			char separator = culture.TextInfo.ListSeparator[0];
			string[] array = text2.Split(separator);
			float[] array2 = new float[array.Length];
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(float));
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = (float)converter.ConvertFromString(context, culture, array[i]);
			}
			if (array2.Length == 2)
			{
				return new SizeF(array2[0], array2[1]);
			}
			throw new ArgumentException(global::SR.Format("Text \"{0}\" cannot be parsed. The expected text format is \"{1}\".", text2, "Width,Height"));
		}
		return base.ConvertFrom(context, culture, value);
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == null)
		{
			throw new ArgumentNullException("destinationType");
		}
		if (destinationType == typeof(string) && value is SizeF sizeF)
		{
			if (culture == null)
			{
				culture = CultureInfo.CurrentCulture;
			}
			string separator = culture.TextInfo.ListSeparator + " ";
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(float));
			string[] array = new string[2];
			int num = 0;
			array[num++] = converter.ConvertToString(context, culture, sizeF.Width);
			array[num++] = converter.ConvertToString(context, culture, sizeF.Height);
			return string.Join(separator, array);
		}
		if (destinationType == typeof(InstanceDescriptor) && value is SizeF sizeF2)
		{
			ConstructorInfo constructor = typeof(SizeF).GetConstructor(new Type[2]
			{
				typeof(float),
				typeof(float)
			});
			if (constructor != null)
			{
				return new InstanceDescriptor(constructor, new object[2] { sizeF2.Width, sizeF2.Height });
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
		object obj = propertyValues["Width"];
		object obj2 = propertyValues["Height"];
		if (obj == null || obj2 == null || !(obj is float) || !(obj2 is float))
		{
			throw new ArgumentException(global::SR.Format("IDictionary parameter contains at least one entry that is not valid. Ensure all values are consistent with the object's properties."));
		}
		return new SizeF((float)obj, (float)obj2);
	}

	public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
	{
		return TypeDescriptor.GetProperties(typeof(SizeF), attributes).Sort(s_propertySort);
	}

	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return true;
	}
}
