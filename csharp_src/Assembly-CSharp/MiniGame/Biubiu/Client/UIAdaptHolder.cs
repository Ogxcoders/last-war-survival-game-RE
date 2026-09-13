using System;
using MiniGame.Core;

namespace MiniGame.Biubiu.Client;

public class UIAdaptHolder : IResourceHolder
{
	public object Data { get; set; }

	public bool IsDone { get; }

	public bool IsError { get; }

	public bool IsValid { get; }

	public float Progress { get; }

	public string ErrorMessage { get; private set; }

	public Action<IResourceHolder> OnDone { get; set; }

	public T As<T>()
	{
		return (T)Data;
	}

	public UIAdaptHolder(IBindUI bindUI)
	{
		bindUI.BindAdapt((DataUIAdapt)(Data = new DataUIAdapt()));
		IsDone = true;
		IsError = false;
		IsValid = true;
		Progress = 1f;
	}

	public void Dispose()
	{
		(Data as DataUIAdapt)?.Dispose();
		Data = null;
	}
}
