using System;

namespace Leopotam.EcsLite;

public class EcsEntitySnapshot
{
	public int EntityName;

	public int EntityID;

	public short EntityGen;

	public (short PoolID, Type ComponentType, object Data)[] Components;

	public (short PoolID, Type ComponentType, object Data) GetComponent(Type componentType)
	{
		for (int i = 0; i < Components.Length; i++)
		{
			if (Components[i].ComponentType == componentType)
			{
				return Components[i];
			}
		}
		return default((short, Type, object));
	}
}
