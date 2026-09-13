using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapDecorateConfig : MonoBehaviour
{
	public enum DecorateType
	{
		Unit,
		Decorate,
		BirthPoint
	}

	public enum DecorateState
	{
		Desert
	}

	[Serializable]
	public class PrefabState
	{
		[Tooltip("其他显示状态")]
		public DecorateState state;

		[Tooltip("该状态下关联的其他预制体，根据不同状态切换表现")]
		public MapDecorateConfig unit;

		[Range(1f, 100f)]
		[Tooltip("随机权重，当有多个相同状态的预制体时，根据权重随机选择")]
		public int weight = 1;

		[Tooltip("概率，根据权重计算，0~1000")]
		public int percent;

		public string unitName
		{
			get
			{
				if (!unit)
				{
					return "空节点";
				}
				return unit.devName;
			}
		}

		private Color GetPercentColor(int value)
		{
			return Color.Lerp(Color.red, Color.green, Mathf.Pow((float)value / 1000f, 0.7f));
		}

		private string GetPercentText()
		{
			return $"{(float)percent / 10f}%";
		}
	}

	public string devName;

	[Tooltip("装饰物类型")]
	public DecorateType decorateType;

	[Tooltip("不可行军，不能造建筑的点为静态点")]
	public bool isStatic;

	[HideInInspector]
	[Tooltip("在整个地图内的生成数量")]
	public int genCount;

	[HideInInspector]
	[Tooltip("按数字顺序从小到大生成")]
	public int priority;

	public bool isBlock;

	[Tooltip("如果是阻挡，阻挡覆盖的范围")]
	public Bounds bounds;

	[Tooltip("其他状态下的表现用的预制体，如被绿化后等状态")]
	public List<PrefabState> states;
}
