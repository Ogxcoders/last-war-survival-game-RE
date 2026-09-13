namespace System.Drawing;

[AttributeUsage(AttributeTargets.Assembly)]
public class BitmapSuffixInSameAssemblyAttribute : Attribute
{
	public BitmapSuffixInSameAssemblyAttribute()
	{
		throw new PlatformNotSupportedException();
	}
}
