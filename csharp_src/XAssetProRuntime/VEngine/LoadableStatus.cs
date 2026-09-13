namespace VEngine;

public enum LoadableStatus
{
	Wait,
	Loading,
	DependentLoading,
	SuccessToLoad,
	FailedToLoad,
	Unloaded,
	CheckVersion,
	Downloading
}
