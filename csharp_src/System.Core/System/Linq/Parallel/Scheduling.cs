namespace System.Linq.Parallel;

internal static class Scheduling
{
	internal const bool DefaultPreserveOrder = false;

	internal static int DefaultDegreeOfParallelism = Math.Min(Environment.ProcessorCount, 16);

	internal const int DEFAULT_BOUNDED_BUFFER_CAPACITY = 512;

	internal const int DEFAULT_BYTES_PER_CHUNK = 512;

	internal const int ZOMBIED_PRODUCER_TIMEOUT = -1;

	internal const int MAX_SUPPORTED_DOP = 512;

	internal static int GetDefaultDegreeOfParallelism()
	{
		return DefaultDegreeOfParallelism;
	}

	internal static int GetDefaultChunkSize<T>()
	{
		if (default(T) != null || Nullable.GetUnderlyingType(typeof(T)) != null)
		{
			return 128;
		}
		return 512 / IntPtr.Size;
	}
}
