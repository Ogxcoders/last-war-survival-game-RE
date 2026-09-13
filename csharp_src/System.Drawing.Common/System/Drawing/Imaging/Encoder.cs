namespace System.Drawing.Imaging;

public sealed class Encoder
{
	public static readonly Encoder ChrominanceTable;

	public static readonly Encoder ColorDepth;

	public static readonly Encoder Compression;

	public static readonly Encoder LuminanceTable;

	public static readonly Encoder Quality;

	public static readonly Encoder RenderMethod;

	public static readonly Encoder SaveFlag;

	public static readonly Encoder ScanMethod;

	public static readonly Encoder Transformation;

	public static readonly Encoder Version;

	public Guid Guid
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public Encoder(Guid guid)
	{
		throw new PlatformNotSupportedException();
	}
}
