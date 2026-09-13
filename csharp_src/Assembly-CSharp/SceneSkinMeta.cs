using System.Collections.Generic;
using UnityEngine;

public class SceneSkinMeta
{
	public int id;

	public int mapType;

	public string loading_bg;

	public string loading_logo;

	public string world_deco_byte;

	public string world_deco_asset;

	public string world_block;

	public string world_terrain;

	public string world_terrain_low;

	public string world_terrain_control;

	public string world_terrain_black;

	public string world_map;

	public string world_city_color;

	public int splash_fill_mode;

	public string world_zone_line;

	public string city_terrain;

	public string city_deco;

	public string city_fog;

	public string radar_bg;

	public string world_city_table_name;

	public int world_terrain_mode;

	public string world_terrain_mode_mat;

	public string troop_line_color;

	public float camera_scaling = 1f;

	public int seasonType;

	public int seasonNext;

	public string world_fog;

	public string world_fog_bloody;

	public string light_monster;

	public int edge_performance;

	public int loading_bgm;

	public int home_bgm;

	public int world_bgm;

	public int city_sound;

	public int world_sound;

	public string edge_world_fog;

	public string cityPostProcessVolume;

	private List<Vector4> world_terrain_control_list;

	public SeasonType GetMapType()
	{
		return (SeasonType)mapType;
	}

	public SeasonType GetPreviewType()
	{
		return (SeasonType)seasonNext;
	}

	public bool IsCityStrongholdMode()
	{
		return mapType == 2;
	}

	public bool IsSnowMode()
	{
		return mapType == 3;
	}

	public bool IsDesertMode()
	{
		return mapType == 1;
	}

	public bool IsMummyMode()
	{
		return mapType == 4;
	}

	public bool IsDarknessMode()
	{
		return mapType == 5;
	}

	public bool IsNineNationMode()
	{
		return mapType == 6;
	}

	public bool IsNotSeason()
	{
		return mapType == 0;
	}

	public List<Vector4> GetWorldTerrainOffset()
	{
		if (string.IsNullOrEmpty(world_terrain_control))
		{
			return null;
		}
		if (world_terrain_control_list == null)
		{
			world_terrain_control_list = new List<Vector4>();
			string[] array = world_terrain_control.Split(new char[1] { '|' });
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(new char[1] { ',' });
				if (array2.Length == 2)
				{
					world_terrain_control_list.Add(new Vector4(float.Parse(array2[0]), float.Parse(array2[1]), 0f, 0f));
				}
				else if (array2.Length == 4)
				{
					world_terrain_control_list.Add(new Vector4(float.Parse(array2[0]), float.Parse(array2[1]), float.Parse(array2[2]), float.Parse(array2[3])));
				}
			}
		}
		return world_terrain_control_list;
	}
}
