using System;
using System.Collections.Generic;
using UnityEngine;
using VEngine;

public class WorldBoardObject : WorldPointObject
{
	private MeshRenderer[] renderers;

	private ShapeFlag shape;

	private BoardState state;

	private string _prefabName;

	private string _lightPrefabName;

	private InstanceRequest _lightInstance;

	private Material _material;

	private string _materialPath;

	private Asset _materialAsset;

	private List<int> printId;

	public static readonly string[] shapeString = new string[16]
	{
		"0000", "0001", "0010", "0011", "0100", "0101", "0110", "0111", "1000", "1001",
		"1010", "1011", "1100", "1101", "1110", "1111"
	};

	public WorldBoardObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
		if (world.GetPointInfo(pointIndex) is BoardPointInfo boardPointInfo)
		{
			serverId = boardPointInfo.serverId;
			state = boardPointInfo.state;
		}
		_prefabName = "";
		printId = new List<int>();
		_materialAsset = null;
		_material = null;
	}

	public override void CreateGameObject()
	{
		base.CreateGameObject();
		CreateBoard();
	}

	private void CreateBoard()
	{
		shape = GetShape();
		string text = null;
		string text2 = null;
		bool flag = false;
		bool flag2 = false;
		Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
		PointInfo pointInfo = world.GetPointInfo(pointIndex);
		if (pointInfo != null && pointInfo is BoardPointInfo boardPointInfo)
		{
			if (boardPointInfo.inside == 1)
			{
				flag = true;
			}
			if (boardPointInfo.IsMine())
			{
				flag2 = true;
			}
		}
		bool flag3 = false;
		if (state == BoardState.NORMAL)
		{
			if (!dictionary.ContainsKey(pointIndex))
			{
				ShapeFlag shapeFlag = (ShapeFlag)0;
				shapeFlag |= ShapeFlag.NX;
				shapeFlag |= ShapeFlag.PX;
				ShapeFlag shapeFlag2 = (ShapeFlag)0;
				shapeFlag2 |= ShapeFlag.PZ;
				shapeFlag2 |= ShapeFlag.NZ;
				bool num = shape == shapeFlag && !HasAdjacentBuilding(GameDefines.DirectionType.Top) && !HasAdjacentBuilding(GameDefines.DirectionType.Down);
				bool flag4 = shape == shapeFlag2 && !HasAdjacentBuilding(GameDefines.DirectionType.Left) && !HasAdjacentBuilding(GameDefines.DirectionType.Right);
				if (num || flag4)
				{
					Vector2Int vector2Int = world.IndexToTilePos(pointIndex);
					if ((vector2Int.x + vector2Int.y) % 2 == 0)
					{
						flag3 = true;
					}
				}
			}
			if (flag2)
			{
				text = $"Assets/Main/Prefabs/Road/Road_Self_{shapeString[(int)shape]}.prefab";
				if (flag3)
				{
					text2 = $"Assets/Main/Prefabs/Road/RoadLightSelf{shapeString[(int)shape]}.prefab";
				}
			}
			else if (flag)
			{
				text = $"Assets/Main/Prefabs/Road/Road_In_City_{shapeString[(int)shape]}.prefab";
				if (flag3)
				{
					text2 = $"Assets/Main/Prefabs/Road/RoadLightInCity{shapeString[(int)shape]}.prefab";
				}
			}
			else
			{
				text = $"Assets/Main/Prefabs/Road/Road_Out_City_{shapeString[(int)shape]}.prefab";
				if (flag3)
				{
					text2 = $"Assets/Main/Prefabs/Road/RoadLightOutCity{shapeString[(int)shape]}.prefab";
				}
			}
		}
		else if (state == BoardState.Updating)
		{
			text = (flag2 ? $"Assets/Main/Prefabs/Road/Road_Self_Updating_{shapeString[(int)shape]}.prefab" : ((!flag) ? $"Assets/Main/Prefabs/Road/Road_Out_City_Updating_{shapeString[(int)shape]}.prefab" : $"Assets/Main/Prefabs/Road/Road_In_City_Updating_{shapeString[(int)shape]}.prefab"));
		}
		string materialPath = GetMaterialPath();
		if (_prefabName != text)
		{
			_prefabName = text;
			if (!string.IsNullOrEmpty(text))
			{
				AddOldObject();
				RefreshShowColor();
				instance = GameEntry.Resource.InstantiateAsync(text);
				instance.completed += delegate
				{
					ClearOldObject();
					gameObject = instance.gameObject;
					if (gameObject != null)
					{
						gameObject.SetActive(value: false);
						CheckLoadComplete();
					}
				};
			}
		}
		else if (_materialPath != materialPath)
		{
			RefreshShowColor();
		}
		if (!(_lightPrefabName != text2))
		{
			return;
		}
		_lightPrefabName = text2;
		if (!string.IsNullOrEmpty(text2))
		{
			if (_lightInstance != null)
			{
				_lightInstance.Destroy();
				_lightInstance = null;
			}
			_lightInstance = GameEntry.Resource.InstantiateAsync(text2);
			_lightInstance.completed += delegate
			{
				_lightInstance.gameObject.transform.SetParent(world.DynamicObjNode);
				_lightInstance.gameObject.SetActive(isVisible);
				_lightInstance.gameObject.transform.position = base.WorldPosition;
			};
		}
		else if (_lightInstance != null)
		{
			_lightInstance.Destroy();
			_lightInstance = null;
		}
	}

	public void StartPrint(int id)
	{
		printId.Add(id);
		state = BoardState.Updating;
		UpdatePrintProgress(0f, new Vector4(1f, 0f, 1f, 0f));
	}

	public override void UpdateGameObject()
	{
		base.UpdateGameObject();
		if (!(gameObject == null))
		{
			CreateBoard();
		}
	}

	public override void Destroy()
	{
		DeleteMaterial();
		for (int i = 0; i < printId.Count; i++)
		{
			world.FakeModelManager.FinishPrintRoad(printId[i]);
		}
		if (_lightInstance != null)
		{
			_lightInstance.Destroy();
			_lightInstance = null;
		}
		base.Destroy();
	}

	public void UpdatePrintProgress(float progress, Vector4 dir)
	{
		if (renderers != null)
		{
			float num = 0.245f;
			float num2 = 0.2625f;
			float value = num + progress * (1f - num - num2);
			MeshRenderer[] array = renderers;
			foreach (MeshRenderer obj in array)
			{
				obj.material.SetFloat("_Progress", value);
				obj.material.SetVector("_Direction", dir);
				obj.material.SetVector("_WorldPivot", gameObject.transform.position);
			}
		}
	}

	public void FinishPrint()
	{
		state = BoardState.NORMAL;
		UpdateGameObject();
	}

	private ShapeFlag GetShape()
	{
		ShapeFlag shapeFlag = (ShapeFlag)0;
		PointInfo pointInfo = SceneManager.World.GetPointInfo(pointIndex);
		if (pointInfo != null)
		{
			if (SceneManager.World.IsRoadPoint(pointIndex, pointInfo.ownerUid, 4))
			{
				shapeFlag |= ShapeFlag.NZ;
			}
			if (SceneManager.World.IsRoadPoint(pointIndex, pointInfo.ownerUid, 3))
			{
				shapeFlag |= ShapeFlag.NX;
			}
			if (SceneManager.World.IsRoadPoint(pointIndex, pointInfo.ownerUid, 2))
			{
				shapeFlag |= ShapeFlag.PX;
			}
			if (SceneManager.World.IsRoadPoint(pointIndex, pointInfo.ownerUid, 1))
			{
				shapeFlag |= ShapeFlag.PZ;
			}
		}
		return shapeFlag;
	}

	public bool HasAdjacentBuilding(GameDefines.DirectionType dir)
	{
		Vector2Int tilePos = SceneManager.World.IndexToTilePos(pointIndex);
		switch (dir)
		{
		case GameDefines.DirectionType.Top:
			tilePos += Vector2Int.up;
			break;
		case GameDefines.DirectionType.Right:
			tilePos += Vector2Int.right;
			break;
		case GameDefines.DirectionType.Left:
			tilePos += Vector2Int.left;
			break;
		case GameDefines.DirectionType.Down:
			tilePos += Vector2Int.down;
			break;
		}
		int num = SceneManager.World.TilePosToIndex(tilePos);
		PointInfo pointInfo = SceneManager.World.GetPointInfo(num);
		if (pointInfo != null)
		{
			return pointInfo.pointType == WorldPointType.PlayerBuilding;
		}
		return false;
	}

	private void RefreshShowColor()
	{
		DeleteMaterial();
		_materialPath = GetMaterialPath();
		if (!string.IsNullOrEmpty(_materialPath))
		{
			_materialAsset = GameEntry.Resource.LoadAssetAsync(_materialPath, typeof(Material));
			Asset materialAsset = _materialAsset;
			materialAsset.completed = (Action<Asset>)Delegate.Combine(materialAsset.completed, (Action<Asset>)delegate(Asset request)
			{
				_material = request.asset as Material;
				CheckLoadComplete();
			});
		}
	}

	private void CheckLoadComplete()
	{
		if (gameObject != null && _material != null)
		{
			gameObject.transform.SetParent(world.DynamicObjNode);
			gameObject.SetActive(isVisible);
			gameObject.transform.position = base.WorldPosition;
			renderers = gameObject.GetComponentsInChildren<MeshRenderer>(includeInactive: true);
			if (renderers != null)
			{
				MeshRenderer[] array = renderers;
				foreach (MeshRenderer meshRenderer in array)
				{
					if (!meshRenderer.material.name.Contains("O_build_"))
					{
						continue;
					}
					meshRenderer.material = _material;
					float value = 1f;
					float value2 = 1f;
					PointInfo pointInfo = world.GetPointInfo(pointIndex);
					if (pointInfo != null && pointInfo is BoardPointInfo boardPointInfo)
					{
						switch (boardPointInfo.GetPlayerType())
						{
						case PlayerType.PlayerSelf:
							value = 1f;
							value2 = 1f;
							break;
						case PlayerType.PlayerAlliance:
						case PlayerType.PlayerAllianceLeader:
							value = 0f;
							value2 = 1f;
							break;
						case PlayerType.PlayerOther:
							value = 0f;
							value2 = 0f;
							break;
						}
					}
					meshRenderer.material.SetFloat("_Fresnel_switch", value);
					meshRenderer.material.SetFloat("_Fresnel_Color_switch", value2);
				}
			}
			if (state == BoardState.Updating)
			{
				UpdatePrintProgress(0f, new Vector4(1f, 0f, 1f, 0f));
			}
		}
		CheckShowTroopDestination();
	}

	private string GetMaterialPath()
	{
		string result = "";
		PointInfo pointInfo = world.GetPointInfo(pointIndex);
		if (pointInfo != null && pointInfo is BoardPointInfo boardPointInfo)
		{
			switch (boardPointInfo.GetPlayerType())
			{
			case PlayerType.PlayerSelf:
				result = ((state != BoardState.Updating) ? "Assets/_Art/Models/Environment/Build/ChengWaiLuMian/material/O_build_lm_new_low.mat" : "Assets/_Art/Models/Environment/Build/ChengWaiLuMian/material/O_build_cw_lm_printing.mat");
				break;
			case PlayerType.PlayerAlliance:
			case PlayerType.PlayerAllianceLeader:
				result = ((state != BoardState.Updating) ? "Assets/_Art/Models/Environment/Build/ChengWaiLuMian/material/O_build_lm_blue.mat" : "Assets/_Art/Models/Environment/Build/ChengWaiLuMian/material/O_build_cw_lm_printing.mat");
				break;
			case PlayerType.PlayerOther:
				result = ((state != BoardState.Updating) ? "Assets/_Art/Models/Environment/Build/ChengWaiLuMian/material/O_build_lm_red.mat" : "Assets/_Art/Models/Environment/Build/ChengWaiLuMian/material/O_build_cw_lm_printing.mat");
				break;
			}
		}
		return result;
	}

	private void DeleteMaterial()
	{
		if (_materialAsset != null)
		{
			_materialAsset.Release();
			_materialAsset = null;
		}
		if (_material != null)
		{
			_material = null;
		}
	}

	public BoardState GetState()
	{
		return state;
	}

	public override void CheckShowTroopDestination()
	{
		bool flag = isSHowDestination;
		foreach (WorldMarch ownerMarch in world.GetOwnerMarches(GameEntry.Data.Player.Uid))
		{
			if (ownerMarch.IsVisibleMarch())
			{
				PointInfo pointInfo = world.GetPointInfo(pointIndex);
				if (pointInfo != null && ownerMarch.targetUuid == pointInfo.uuid && ownerMarch.targetUuid != 0L)
				{
					flag = false;
					Vector3 realPos = base.WorldPosition;
					int num = 1;
					EnumDestinationSignalType destinationType = world.GetDestinationType(ownerMarch.uuid, ownerMarch.targetUuid, pointIndex, ownerMarch.target, isFormation: false, ref realPos, ref num);
					ShowTroopDestinationSignal(realPos, destinationType, num);
					break;
				}
			}
		}
		if (flag)
		{
			HideTroopDestinationSignal();
		}
	}
}
