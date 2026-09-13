namespace GameKit.Base;

public class DebuggerService : SingletonBehaviour<DebuggerService>
{
	public Debugger Debugger { get; private set; }

	public static void Initialize()
	{
		SingletonBehaviour<DebuggerService>.Instance.gameObject.name = "Debugger";
		SingletonBehaviour<DebuggerService>.Instance.Debugger = SingletonBehaviour<DebuggerService>.Instance.gameObject.AddComponent<Debugger>();
		SingletonBehaviour<DebuggerService>.Instance.Debugger.Startup();
	}
}
