using System;
using System.IO;

internal class DeleteTask : FileActionTask
{
	private Action callback;

	public DeleteTask(string id, Action callback)
		: base(id)
	{
		this.callback = callback;
	}

	public override void Process()
	{
		if (File.Exists(FilePath))
		{
			File.Delete(FilePath);
		}
		base.Process();
	}

	protected internal override void CallBack()
	{
		callback?.Invoke();
	}
}
