using System.Runtime.Serialization;

namespace System.Transactions;

[Serializable]
[System.MonoTODO("Not supported yet")]
public sealed class DependentTransaction : Transaction, ISerializable
{
	private bool completed;

	internal bool Completed => completed;

	internal DependentTransaction(Transaction parent, DependentCloneOption option)
		: base(parent.IsolationLevel)
	{
	}

	[System.MonoTODO]
	public void Complete()
	{
		throw new NotImplementedException();
	}

	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
		completed = info.GetBoolean("completed");
	}
}
