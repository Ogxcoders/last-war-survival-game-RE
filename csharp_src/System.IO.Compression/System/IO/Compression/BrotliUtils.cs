namespace System.IO.Compression;

internal static class BrotliUtils
{
	public const int WindowBits_Min = 10;

	public const int WindowBits_Default = 22;

	public const int WindowBits_Max = 24;

	public const int Quality_Min = 0;

	public const int Quality_Default = 11;

	public const int Quality_Max = 11;

	public const int MaxInputSize = 2147483132;

	internal static int GetQualityFromCompressionLevel(CompressionLevel level)
	{
		return level switch
		{
			CompressionLevel.Optimal => 11, 
			CompressionLevel.NoCompression => 0, 
			CompressionLevel.Fastest => 1, 
			_ => (int)level, 
		};
	}
}
