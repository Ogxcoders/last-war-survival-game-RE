namespace System.Runtime.Serialization;

internal class DataContractPairKey
{
	private object object1;

	private object object2;

	public DataContractPairKey(object object1, object object2)
	{
		this.object1 = object1;
		this.object2 = object2;
	}

	public override bool Equals(object other)
	{
		if (!(other is DataContractPairKey dataContractPairKey))
		{
			return false;
		}
		if (dataContractPairKey.object1 != object1 || dataContractPairKey.object2 != object2)
		{
			if (dataContractPairKey.object1 == object2)
			{
				return dataContractPairKey.object2 == object1;
			}
			return false;
		}
		return true;
	}

	public override int GetHashCode()
	{
		return object1.GetHashCode() ^ object2.GetHashCode();
	}
}
