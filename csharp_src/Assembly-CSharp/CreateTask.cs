using System;
using System.IO;
using System.Text;

internal class CreateTask : FileActionTask
{
	private string content;

	private Action callback;

	public CreateTask(string id, string content, Action callback)
		: base(id)
	{
		this.content = content;
		this.callback = callback;
	}

	public override void Process()
	{
		if (!Directory.Exists(DirectoryPath))
		{
			Directory.CreateDirectory(DirectoryPath);
		}
		using (FileStream fileStream = new FileStream(FilePath, FileMode.Create))
		{
			if (FileContentHelper.useBinary)
			{
				using BinaryWriter binaryWriter = new BinaryWriter(fileStream);
				binaryWriter.Write(content);
			}
			else
			{
				byte[] bytes = Encoding.UTF8.GetBytes(content);
				fileStream.Write(bytes, 0, bytes.Length);
			}
		}
		base.Process();
	}

	protected internal override void CallBack()
	{
		callback?.Invoke();
	}
}
