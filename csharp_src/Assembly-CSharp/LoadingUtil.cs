public static class LoadingUtil
{
	public static string GetLoadingStateText(int stateNum)
	{
		return (LoadingState)stateNum switch
		{
			LoadingState.Permission => "121015", 
			LoadingState.CheckResVersion => "121016", 
			LoadingState.DownloadManifest => "121017", 
			LoadingState.DownloadUpdate => "121018", 
			LoadingState.LoadDataTable => "121019", 
			LoadingState.GetServerList => "121020", 
			LoadingState.GetServerStatus => "121021", 
			LoadingState.ConnectGame => "121022", 
			LoadingState.Login => "121023", 
			LoadingState.PushInit => "121024", 
			LoadingState.AuthPin => "121025", 
			LoadingState.CNIdentify => "121025", 
			LoadingState.LoadScene => "121026", 
			LoadingState.KRAuth => "korea_verify1", 
			_ => "", 
		};
	}

	public static bool IsLoadingErrorState(int stateNum)
	{
		return stateNum == 18;
	}

	public static bool IsLoadigMaintenanceState(int stateNum)
	{
		return stateNum == 19;
	}
}
