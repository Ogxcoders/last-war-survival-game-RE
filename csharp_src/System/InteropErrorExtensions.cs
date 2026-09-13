internal static class InteropErrorExtensions
{
	public static global::Interop.ErrorInfo Info(this global::Interop.Error error)
	{
		return new global::Interop.ErrorInfo(error);
	}
}
