using System.Collections.Generic;

public static class LODResourceTypeSpec
{
	public static List<LODResourceSpec> kResourceSpecs = new List<LODResourceSpec>
	{
		new LODResourceSpec(LODResourceType.MainCity_Unknown, -1, -1, -1, -1f, -1, -1),
		new LODResourceSpec(LODResourceType.MainCity_Normal, 60, 25, 10, 3f, 4500, 600),
		new LODResourceSpec(LODResourceType.MainCity_VIP18, 100, 40, 20, 3f, 12000, 1000),
		new LODResourceSpec(LODResourceType.MainCity_Festival, 80, 35, 15, 3f, 8000, 800),
		new LODResourceSpec(LODResourceType.Effect_Normal, 20, 10, 5, 3f, 500, 500),
		new LODResourceSpec(LODResourceType.Effect_MainCity, 40, 18, 10, 3f, 500, 500),
		new LODResourceSpec(LODResourceType.Effect_Troop, 20, 10, 5, 3f, 500, 500)
	};

	public static LODResourceSpec GetSpec(LODResourceType type)
	{
		return kResourceSpecs.Find((LODResourceSpec s) => s.Type == type);
	}
}
