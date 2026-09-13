using System.Collections.Generic;

public class ResourceUtils
{
	private static Dictionary<ResourceType, List<int>> resourceTargetBuildType = new Dictionary<ResourceType, List<int>>
	{
		{
			ResourceType.Oil,
			new List<int> { 413000 }
		},
		{
			ResourceType.Metal,
			new List<int> { 412000 }
		},
		{
			ResourceType.Water,
			new List<int> { 432000 }
		},
		{
			ResourceType.Electricity,
			new List<int> { 431000, 444000, 447000 }
		}
	};

	public static Dictionary<ResourceType, List<int>> pairStorage = new Dictionary<ResourceType, List<int>>
	{
		{
			ResourceType.Oil,
			new List<int> { 439000 }
		},
		{
			ResourceType.Metal,
			new List<int>()
		},
		{
			ResourceType.Water,
			new List<int> { 438000 }
		},
		{
			ResourceType.Electricity,
			new List<int> { 437000 }
		}
	};

	public static string GetResourceImagePath(ResourceType resType)
	{
		return resType switch
		{
			ResourceType.Oil => "Assets/Main/Sprites/UI/LWCommon/Sprite/Common_icon_oil", 
			ResourceType.Metal => "Assets/Main/Sprites/UI/LWCommon/Sprite/Common_icon_metal", 
			ResourceType.Water => "Assets/Main/Sprites/UI/LWCommon/Sprite/Common_icon_water", 
			ResourceType.Electricity => "Assets/Main/Sprites/UI/LWCommon/Sprite/Common_icon_electricity", 
			ResourceType.Food => "Assets/Main/Sprites/UI/LWCommon/Sprite/Common_icon_money", 
			ResourceType.GOLD => "Assets/Main/Sprites/UI/LWCommon/Sprite/Common_icon_gold", 
			_ => "", 
		};
	}

	public static string GetRewardTypeImagePath(int resType)
	{
		return (RewardType)resType switch
		{
			RewardType.FOOD => "ui_food_max", 
			RewardType.OIL => "ui_oil_max", 
			RewardType.METAL => "ui_metal_max", 
			RewardType.NUCLEAR => "ui_nuclear_max", 
			RewardType.WATER => "ui_water_max", 
			RewardType.TRADE => "ui_gold", 
			RewardType.ELECTRICITY => "ui_electricity_max", 
			RewardType.PEOPLE => "ui_people_max", 
			RewardType.HONOR => "u_alliance_contributeicon02", 
			RewardType.ALLIANCE_POINT => "u_alliance_contributeicon01", 
			_ => "", 
		};
	}

	public static string GetRewardTypeName(int resType)
	{
		return (RewardType)resType switch
		{
			RewardType.OIL => "107511", 
			RewardType.METAL => "107512", 
			RewardType.NUCLEAR => "100088", 
			RewardType.WATER => "100546", 
			RewardType.TRADE => "100183", 
			RewardType.ELECTRICITY => "100002", 
			RewardType.PEOPLE => "100396", 
			RewardType.FOOD => "100000", 
			RewardType.HONOR => "390261", 
			RewardType.ALLIANCE_POINT => "390266", 
			_ => "", 
		};
	}

	public static List<int> GetResourceTypeCityBuildingByType(int type)
	{
		if (resourceTargetBuildType.ContainsKey((ResourceType)type))
		{
			return resourceTargetBuildType[(ResourceType)type];
		}
		return new List<int>();
	}

	public static int GetResourcesTypeByCityBuildingType(int type)
	{
		return type switch
		{
			413000 => 0, 
			431000 => 12, 
			444000 => 12, 
			447000 => 12, 
			432000 => 11, 
			412000 => 1, 
			_ => 0, 
		};
	}

	public static ResourceType RewardType2ResourceType(RewardType rewardType)
	{
		return rewardType switch
		{
			RewardType.OIL => ResourceType.Oil, 
			RewardType.WATER => ResourceType.Water, 
			RewardType.ELECTRICITY => ResourceType.Electricity, 
			RewardType.METAL => ResourceType.Metal, 
			RewardType.FOOD => ResourceType.Food, 
			_ => ResourceType.Oil, 
		};
	}

	public static UIMainBottomBuildType GetBuildTabTypeByResourceType(ResourceType type)
	{
		return type switch
		{
			ResourceType.Oil => UIMainBottomBuildType.Oil, 
			ResourceType.Metal => UIMainBottomBuildType.Metal, 
			ResourceType.Water => UIMainBottomBuildType.Water, 
			ResourceType.Electricity => UIMainBottomBuildType.Electricity, 
			ResourceType.Food => UIMainBottomBuildType.People, 
			_ => UIMainBottomBuildType.Build, 
		};
	}

	public static ResourceType GetResourceTypeByBuildTabType(UIMainBottomBuildType type)
	{
		return type switch
		{
			UIMainBottomBuildType.Oil => ResourceType.Oil, 
			UIMainBottomBuildType.Metal => ResourceType.Metal, 
			UIMainBottomBuildType.Water => ResourceType.Water, 
			UIMainBottomBuildType.Electricity => ResourceType.Electricity, 
			_ => ResourceType.Food, 
		};
	}

	public static float ArmyConsume(int resourcesType)
	{
		return 0f;
	}

	public static long GetResourceItemCount(int type)
	{
		return 0L;
	}

	public static int GetOutBuildByResType(ResourceType resourceType)
	{
		if (resourceType == ResourceType.Electricity)
		{
			return 431000;
		}
		if (resourceTargetBuildType.ContainsKey(resourceType))
		{
			List<int> list = resourceTargetBuildType[resourceType];
			if (list != null && list.Count > 0)
			{
				return list[0];
			}
		}
		return 0;
	}
}
