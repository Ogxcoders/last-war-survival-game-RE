namespace AIHelp;

public class AIHelpDefine
{
	public delegate void OnAIHelpInitializedCallback(bool isSuccess, string message);

	public delegate void OnNetworkCheckResultCallback(string netLog);

	public delegate void OnMessageCountArrivedCallback(int msgCount);

	public delegate void OnSpecificFormSubmittedCallback();

	public delegate void OnAIHelpSessionOpenCallback();

	public delegate void OnAIHelpSessionCloseCallback();

	public delegate void OnSpecificUrlClickedCallback(string url);
}
