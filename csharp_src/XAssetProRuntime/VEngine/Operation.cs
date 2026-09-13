using System;
using System.Collections;
using System.Collections.Generic;

namespace VEngine;

public class Operation : IEnumerator
{
	internal static readonly List<Operation> Processing = new List<Operation>();

	public Action<Operation> completed;

	public OperationStatus status { get; protected set; }

	public float progress { get; protected set; }

	public bool isDone
	{
		get
		{
			if (status != OperationStatus.Failed)
			{
				return status == OperationStatus.Success;
			}
			return true;
		}
	}

	public string error { get; protected set; }

	public bool isError => !string.IsNullOrEmpty(error);

	public object Current { get; }

	public bool MoveNext()
	{
		return !isDone;
	}

	public void Reset()
	{
	}

	internal static void Process(Operation operation)
	{
		operation.status = OperationStatus.Processing;
		Processing.Add(operation);
	}

	protected virtual void Update()
	{
	}

	public virtual void Start()
	{
		status = OperationStatus.Processing;
		Process(this);
	}

	public void Cancel()
	{
		Finish("User Cancel.");
	}

	protected void Finish(string errorCode = null)
	{
		error = errorCode;
		status = (string.IsNullOrEmpty(error) ? OperationStatus.Success : OperationStatus.Failed);
		progress = 1f;
	}

	protected void Complete()
	{
		if (completed != null)
		{
			Action<Operation> value = completed;
			completed(this);
			completed = (Action<Operation>)Delegate.Remove(completed, value);
		}
	}

	public static void UpdateOperations()
	{
		for (int i = 0; i < Processing.Count; i++)
		{
			Operation operation = Processing[i];
			if (Updater.busy)
			{
				return;
			}
			operation.Update();
			if (operation.isDone)
			{
				Processing.RemoveAt(i);
				i--;
				if (operation.status == OperationStatus.Failed)
				{
					Logger.E("Unable to complete {0} with error: {1}", operation.GetType().Name, operation.error);
				}
				operation.Complete();
			}
		}
		InstantiateObject.UpdateObjects();
	}
}
