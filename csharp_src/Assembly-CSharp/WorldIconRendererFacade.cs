using System;
using System.Collections.Generic;
using System.Text;
using FibMatrix;
using GameFramework;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using VEngine;
using XLua;

public class WorldIconRendererFacade : WorldManagerBase
{
	public class WorldHappyIconConfig
	{
		public string renderName;

		public string materialPath;

		public RenderPassEvent renderPassEvent;

		public int sortOrder;

		public int minLod;

		public int maxLod;

		public Color gizmosColor = Color.green;
	}

	public class HappyIcon : WorldGpuInstancingRenderer.IHandle, IDisposable
	{
		private static ObjectPool<HappyIcon> pool;

		private WorldGpuInstancingRenderer instancingRenderer;

		public Vector3 worldPosition;

		public Vector4 color;

		public int iconIndex;

		private bool destroyed;

		public float yRadian;

		public int DataIndex { get; set; } = -1;

		public static HappyIcon Create(WorldGpuInstancingRenderer instancingRenderer)
		{
			if (pool == null)
			{
				pool = new ObjectPool<HappyIcon>();
			}
			HappyIcon happyIcon = pool.Allocate();
			happyIcon.worldPosition = Vector3.zero;
			happyIcon.iconIndex = 0;
			happyIcon.yRadian = 0f;
			happyIcon.color = Vector4.one;
			happyIcon.instancingRenderer = instancingRenderer;
			happyIcon.destroyed = false;
			instancingRenderer.CreateIndex(happyIcon);
			return happyIcon;
		}

		public void Refresh()
		{
			instancingRenderer?.UpdateData(this);
		}

		public void Refresh(Vector3 worldPosition, int iconIndex)
		{
			if (instancingRenderer != null)
			{
				this.iconIndex = iconIndex;
				this.worldPosition = worldPosition;
				instancingRenderer.UpdateData(this);
			}
		}

		public void SetColor(Color color)
		{
			this.color = color;
			instancingRenderer?.UpdateData(this);
		}

		public void SetEulerAngleY(float yAngle)
		{
			yRadian = yAngle * (MathF.PI / 180f);
		}

		public void SetColorNoRefresh(Color color)
		{
			this.color = color;
		}

		public void Destroy()
		{
			destroyed = true;
			if (instancingRenderer == null)
			{
				pool?.Recycle(this);
				return;
			}
			instancingRenderer?.ReleaseIndex(this);
			instancingRenderer = null;
			pool?.Recycle(this);
		}

		public virtual void Dispose()
		{
			if (!destroyed)
			{
				Destroy();
			}
		}
	}

	public class HappyIconGroup : WorldGpuInstancingRenderer.RenderGroup
	{
		private Vector4[] uvInfoArray = new Vector4[1023];

		private Vector4[] colorArray = new Vector4[1023];

		public Color gizmosColor = Color.green;

		protected override Color GizmosColor => gizmosColor;

		protected override void OnBeforeDraw()
		{
			base.MPB.SetVectorArray("_UVInfo", uvInfoArray);
			base.MPB.SetVectorArray("_BlendColor", colorArray);
		}

		protected override void OnDataUpdate(int arrayIndex, object data)
		{
			if (data is HappyIcon happyIcon)
			{
				UpdatePosition(arrayIndex, happyIcon.worldPosition);
				uvInfoArray[arrayIndex].Set(happyIcon.iconIndex, happyIcon.yRadian, 0f, 0f);
				colorArray[arrayIndex] = happyIcon.color;
			}
		}
	}

	public class HappyIconRendererProxy
	{
		private WorldGpuInstancingRenderer renderer;

		private Asset materialRequest;

		private Color gizmosColor = Color.green;

		protected virtual Mesh Mesh => WorldGpuInstancingUtils.CreateNormalMesh();

		public HappyIconRendererProxy(WorldHappyIconConfig config)
		{
			materialRequest = GameEntry.Resource.LoadAssetAsync(config.materialPath, typeof(Material));
			renderer = WorldGpuInstancingRenderer.Create(config.renderName, Mesh, null, CreateGroup, config.renderPassEvent, config.sortOrder, config.minLod, config.maxLod);
			gizmosColor = config.gizmosColor;
			Asset asset = materialRequest;
			asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
			{
				if (materialRequest != null && materialRequest.status == LoadableStatus.SuccessToLoad)
				{
					if (renderer == null)
					{
						materialRequest.Release();
						materialRequest = null;
					}
					else
					{
						Material material = null;
						material = new Material(materialRequest.asset as Material);
						renderer.material = material;
						OnInit();
					}
				}
			});
		}

		protected virtual HappyIconGroup CreateGroup()
		{
			return new HappyIconGroup
			{
				gizmosColor = gizmosColor
			};
		}

		protected virtual void OnInit()
		{
		}

		protected virtual void OnDispose()
		{
		}

		public HappyIcon CreateRenderer()
		{
			if (renderer == null)
			{
				return null;
			}
			return HappyIcon.Create(renderer);
		}

		public void Dispose()
		{
			if (materialRequest != null)
			{
				materialRequest.Release();
				materialRequest = null;
			}
			if (renderer != null)
			{
				renderer.DisposeRenderer();
				renderer = null;
			}
			OnDispose();
		}

		public string Description()
		{
			return renderer?.Description() ?? "NULL";
		}
	}

	private static Dictionary<string, WorldHappyIconConfig> worldHappyIconConfigs = new Dictionary<string, WorldHappyIconConfig>(StringComparer.OrdinalIgnoreCase);

	private Dictionary<string, HappyIconRendererProxy> rendersByName = new Dictionary<string, HappyIconRendererProxy>();

	public static void InitConfig()
	{
		if (worldHappyIconConfigs.Count > 0)
		{
			return;
		}
		LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetWorldIconRenderConfig");
		if (luaTable == null || luaTable.Length <= 0)
		{
			Log.Error("__GameException__ WorldIconRendererFacade.InitConfig Failed.");
			return;
		}
		int i = 1;
		for (int length = luaTable.Length; i <= length; i++)
		{
			if (!(luaTable[i] is LuaTable luaTable2))
			{
				continue;
			}
			string text = luaTable2.Get<string>("name");
			if (!string.IsNullOrEmpty(text))
			{
				worldHappyIconConfigs[text] = new WorldHappyIconConfig
				{
					renderName = text,
					materialPath = luaTable2.Get<string>("matPath"),
					renderPassEvent = luaTable2.Get<RenderPassEvent>("event"),
					sortOrder = luaTable2.Get<int>("order"),
					minLod = luaTable2.Get<int>("minLod"),
					maxLod = luaTable2.Get<int>("maxLod")
				};
				if (luaTable2.ContainsKey("gizmosColor"))
				{
					worldHappyIconConfigs[text].gizmosColor = luaTable2.Get<Color>("gizmosColor");
				}
			}
		}
	}

	private static void DisposeConfig()
	{
		worldHappyIconConfigs.Clear();
	}

	public WorldIconRendererFacade(WorldScene scene)
		: base(scene)
	{
	}

	public override void UnInit()
	{
		foreach (KeyValuePair<string, HappyIconRendererProxy> item in rendersByName)
		{
			item.Value?.Dispose();
		}
		rendersByName.Clear();
		DisposeConfig();
		base.UnInit();
	}

	public HappyIcon CreateIcon(string name)
	{
		rendersByName.TryGetValue(name, out var value);
		if (value == null)
		{
			if (!worldHappyIconConfigs.TryGetValue(name, out var value2))
			{
				return null;
			}
			value = new HappyIconRendererProxy(value2);
			rendersByName.Add(name, value);
		}
		return value?.CreateRenderer();
	}

	public override string Description()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("---大世界实例化渲染器---");
		foreach (KeyValuePair<string, HappyIconRendererProxy> item in rendersByName)
		{
			stringBuilder.AppendLine(item.Key ?? "");
			stringBuilder.AppendLine(item.Value.Description() ?? "");
		}
		return stringBuilder.ToString();
	}
}
