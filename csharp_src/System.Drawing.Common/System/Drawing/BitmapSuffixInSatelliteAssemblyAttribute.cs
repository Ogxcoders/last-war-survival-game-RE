namespace System.Drawing;

[AttributeUsage(AttributeTargets.Assembly)]
public class BitmapSuffixInSatelliteAssemblyAttribute : Attribute
{
	public BitmapSuffixInSatelliteAssemblyAttribute()
	{
		throw new PlatformNotSupportedException();
	}
}
