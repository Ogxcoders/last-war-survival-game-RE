namespace System.Drawing;

[AttributeUsage(AttributeTargets.Class)]
public class ToolboxBitmapAttribute : Attribute
{
	public static readonly ToolboxBitmapAttribute Default;

	public ToolboxBitmapAttribute(string imageFile)
	{
		throw new PlatformNotSupportedException();
	}

	public ToolboxBitmapAttribute(Type t)
	{
		throw new PlatformNotSupportedException();
	}

	public ToolboxBitmapAttribute(Type t, string name)
	{
		throw new PlatformNotSupportedException();
	}

	public override bool Equals(object value)
	{
		throw new PlatformNotSupportedException();
	}

	public override int GetHashCode()
	{
		throw new PlatformNotSupportedException();
	}

	public Image GetImage(object component)
	{
		throw new PlatformNotSupportedException();
	}

	public Image GetImage(object component, bool large)
	{
		throw new PlatformNotSupportedException();
	}

	public Image GetImage(Type type)
	{
		throw new PlatformNotSupportedException();
	}

	public Image GetImage(Type type, bool large)
	{
		throw new PlatformNotSupportedException();
	}

	public Image GetImage(Type type, string imgName, bool large)
	{
		throw new PlatformNotSupportedException();
	}

	public static Image GetImageFromResource(Type t, string imageName, bool large)
	{
		throw new PlatformNotSupportedException();
	}
}
