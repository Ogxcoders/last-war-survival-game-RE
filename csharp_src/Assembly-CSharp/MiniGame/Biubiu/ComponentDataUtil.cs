using System;
using Box2DSharp.Common;

namespace MiniGame.Biubiu;

public static class ComponentDataUtil
{
	public static void SetPropertyValue(this ref ComponentData componentData, PropertyID propertyID, FP value)
	{
		short num = (short)propertyID;
		if (num > componentData.KeyValues.Length)
		{
			Array.Resize(ref componentData.KeyValues, num + 1);
		}
		componentData.KeyValues[num].Value = value;
		componentData.KeyValues[num].PropertyID = num;
	}

	public static FP GetPropertyValue(this ref ComponentData componentData, PropertyID propertyID)
	{
		return componentData.KeyValues[(int)propertyID].Value;
	}

	public static FP GetPropertyValue(this ref ComponentData componentData, PropertyID propertyID, FP dv)
	{
		short num = (short)propertyID;
		if (componentData.KeyValues.Length > num)
		{
			return componentData.KeyValues[(int)propertyID].Value;
		}
		return dv;
	}

	public static bool TryGetPropertyValue(this ref ComponentData componentData, PropertyID propertyID, out FP value)
	{
		short num = (short)propertyID;
		if (componentData.KeyValues.Length > num)
		{
			value = componentData.KeyValues[(int)propertyID].Value;
			return true;
		}
		value = default(FP);
		return false;
	}
}
