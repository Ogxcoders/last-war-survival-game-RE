namespace System.Diagnostics.Tracing;

[Flags]
public enum EventManifestOptions
{
	AllCultures = 2,
	AllowEventSourceOverride = 8,
	None = 0,
	OnlyIfNeededForRegistration = 4,
	Strict = 1
}
