public class EditorStartupLoading : AppStartupLoading
{
	public EditorStartupLoading()
	{
		for (int i = 0; i < _stateList.Length; i++)
		{
			_stateList[i] = null;
		}
		_stateList[1] = new LogoState(this);
		_stateList[2] = new PermissionState(this);
		_stateList[4] = new EditorCheckResVersionState(this);
		_stateList[5] = new DownloadManifestState(this);
		_stateList[6] = new DownloadUpdateState(this);
		_stateList[7] = new LoadDataTableState(this);
		_stateList[8] = new EditorSceneSelectorState(this);
	}
}
