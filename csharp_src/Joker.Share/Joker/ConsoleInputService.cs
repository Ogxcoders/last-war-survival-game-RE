using System;

namespace Joker;

public class ConsoleInputService : IService
{
	public void Awake()
	{
	}

	public void Startup()
	{
		Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e)
		{
			App.Exit();
			e.Cancel = true;
		};
	}

	public void Shutdown()
	{
	}

	public void Destroy()
	{
	}
}
