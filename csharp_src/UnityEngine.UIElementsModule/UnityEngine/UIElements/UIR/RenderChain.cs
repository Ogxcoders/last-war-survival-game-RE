#define UNITY_ASSERTIONS
using System;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine.UIElements.UIR.Implementation;

namespace UnityEngine.UIElements.UIR;

internal class RenderChain : IDisposable
{
	private struct DepthOrderedDirtyTracking
	{
		public List<VisualElement> heads;

		public List<VisualElement> tails;

		public int[] minDepths;

		public int[] maxDepths;

		public uint dirtyID;

		public void EnsureFits(int maxDepth)
		{
			while (heads.Count <= maxDepth)
			{
				heads.Add(null);
				tails.Add(null);
			}
		}

		public void RegisterDirty(VisualElement ve, RenderDataDirtyTypes dirtyTypes, int dirtyTypeClassIndex)
		{
			Debug.Assert(dirtyTypes != RenderDataDirtyTypes.None);
			int hierarchyDepth = ve.renderChainData.hierarchyDepth;
			minDepths[dirtyTypeClassIndex] = ((hierarchyDepth < minDepths[dirtyTypeClassIndex]) ? hierarchyDepth : minDepths[dirtyTypeClassIndex]);
			maxDepths[dirtyTypeClassIndex] = ((hierarchyDepth > maxDepths[dirtyTypeClassIndex]) ? hierarchyDepth : maxDepths[dirtyTypeClassIndex]);
			if (ve.renderChainData.dirtiedValues != RenderDataDirtyTypes.None)
			{
				ve.renderChainData.dirtiedValues |= dirtyTypes;
				return;
			}
			ve.renderChainData.dirtiedValues = dirtyTypes;
			if (tails[hierarchyDepth] != null)
			{
				tails[hierarchyDepth].renderChainData.nextDirty = ve;
				ve.renderChainData.prevDirty = tails[hierarchyDepth];
				tails[hierarchyDepth] = ve;
			}
			else
			{
				List<VisualElement> list = heads;
				VisualElement value = (tails[hierarchyDepth] = ve);
				list[hierarchyDepth] = value;
			}
		}

		public void ClearDirty(VisualElement ve, RenderDataDirtyTypes dirtyTypesInverse)
		{
			Debug.Assert(ve.renderChainData.dirtiedValues != RenderDataDirtyTypes.None);
			ve.renderChainData.dirtiedValues &= dirtyTypesInverse;
			if (ve.renderChainData.dirtiedValues == RenderDataDirtyTypes.None)
			{
				if (ve.renderChainData.prevDirty != null)
				{
					ve.renderChainData.prevDirty.renderChainData.nextDirty = ve.renderChainData.nextDirty;
				}
				if (ve.renderChainData.nextDirty != null)
				{
					ve.renderChainData.nextDirty.renderChainData.prevDirty = ve.renderChainData.prevDirty;
				}
				if (tails[ve.renderChainData.hierarchyDepth] == ve)
				{
					Debug.Assert(ve.renderChainData.nextDirty == null);
					tails[ve.renderChainData.hierarchyDepth] = ve.renderChainData.prevDirty;
				}
				if (heads[ve.renderChainData.hierarchyDepth] == ve)
				{
					Debug.Assert(ve.renderChainData.prevDirty == null);
					heads[ve.renderChainData.hierarchyDepth] = ve.renderChainData.nextDirty;
				}
				ve.renderChainData.prevDirty = (ve.renderChainData.nextDirty = null);
			}
		}

		public void Reset()
		{
			for (int i = 0; i < minDepths.Length; i++)
			{
				minDepths[i] = int.MaxValue;
				maxDepths[i] = int.MinValue;
			}
		}
	}

	private struct RenderDeviceRestoreInfo
	{
		public VisualElement root;

		public Shader standardShader;

		public bool hasAtlasMan;

		public bool hasVectorImageMan;
	}

	private RenderChainCommand m_FirstCommand;

	private DepthOrderedDirtyTracking m_DirtyTracker;

	private Pool<RenderChainCommand> m_CommandPool = new Pool<RenderChainCommand>();

	private bool m_BlockDirtyRegistration;

	private ChainBuilderStats m_Stats;

	private uint m_StatsElementsAdded;

	private uint m_StatsElementsRemoved;

	private VisualElement m_FirstTextElement;

	private UIRTextUpdatePainter m_TextUpdatePainter;

	private int m_TextElementCount;

	private int m_DirtyTextStartIndex;

	private int m_DirtyTextRemaining;

	private bool m_FontWasReset;

	private Dictionary<VisualElement, Vector2> m_LastGroupTransformElementScale = new Dictionary<VisualElement, Vector2>();

	private static ProfilerMarker s_MarkerRender = new ProfilerMarker("RenderChain.Draw");

	private static ProfilerMarker s_MarkerClipProcessing = new ProfilerMarker("RenderChain.UpdateClips");

	private static ProfilerMarker s_MarkerOpacityProcessing = new ProfilerMarker("RenderChain.UpdateOpacity");

	private static ProfilerMarker s_MarkerTransformProcessing = new ProfilerMarker("RenderChain.UpdateTransforms");

	private static ProfilerMarker s_MarkerVisualsProcessing = new ProfilerMarker("RenderChain.UpdateVisuals");

	private static ProfilerMarker s_MarkerTextRegen = new ProfilerMarker("RenderChain.RegenText");

	internal static Action OnPreRender = null;

	internal UIRVEShaderInfoAllocator shaderInfoAllocator;

	private RenderDeviceRestoreInfo m_RenderDeviceRestoreInfo;

	internal RenderChainCommand firstCommand => m_FirstCommand;

	protected bool disposed { get; private set; }

	internal ChainBuilderStats stats => m_Stats;

	internal IPanel panel { get; private set; }

	internal UIRenderDevice device { get; private set; }

	internal UIRAtlasManager atlasManager { get; private set; }

	internal VectorImageManager vectorImageManager { get; private set; }

	internal UIRStylePainter painter { get; private set; }

	internal bool drawStats { get; set; }

	public event Action<UIRenderDevice> BeforeDrawChain;

	public RenderChain(IPanel panel, Shader standardShader)
	{
		UIRAtlasManager atlasMan = new UIRAtlasManager();
		VectorImageManager vectorImageMan = new VectorImageManager(atlasMan);
		Constructor(panel, new UIRenderDevice(RenderEvents.ResolveShader(standardShader)), atlasMan, vectorImageMan);
	}

	protected RenderChain(IPanel panel, UIRenderDevice device, UIRAtlasManager atlasManager, VectorImageManager vectorImageManager)
	{
		Constructor(panel, device, atlasManager, vectorImageManager);
	}

	private void Constructor(IPanel panelObj, UIRenderDevice deviceObj, UIRAtlasManager atlasMan, VectorImageManager vectorImageMan)
	{
		if (disposed)
		{
			DisposeHelper.NotifyDisposedUsed(this);
		}
		m_DirtyTracker.heads = new List<VisualElement>(8);
		m_DirtyTracker.tails = new List<VisualElement>(8);
		m_DirtyTracker.minDepths = new int[4];
		m_DirtyTracker.maxDepths = new int[4];
		m_DirtyTracker.Reset();
		panel = panelObj;
		device = deviceObj;
		atlasManager = atlasMan;
		vectorImageManager = vectorImageMan;
		shaderInfoAllocator.Construct();
		painter = new UIRStylePainter(this);
		Font.textureRebuilt += OnFontReset;
	}

	private void Destructor()
	{
		Font.textureRebuilt -= OnFontReset;
		painter?.Dispose();
		m_TextUpdatePainter?.Dispose();
		atlasManager?.Dispose();
		vectorImageManager?.Dispose();
		shaderInfoAllocator.Dispose();
		device?.Dispose();
		painter = null;
		m_TextUpdatePainter = null;
		atlasManager = null;
		shaderInfoAllocator = default(UIRVEShaderInfoAllocator);
		device = null;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected void Dispose(bool disposing)
	{
		if (!disposed)
		{
			if (disposing)
			{
				Destructor();
			}
			disposed = true;
		}
	}

	public void Render(Rect viewport, Matrix4x4 projection, PanelClearFlags clearFlags)
	{
		m_Stats = default(ChainBuilderStats);
		m_Stats.elementsAdded += m_StatsElementsAdded;
		m_Stats.elementsRemoved += m_StatsElementsRemoved;
		m_StatsElementsAdded = (m_StatsElementsRemoved = 0u);
		if (shaderInfoAllocator.isReleased)
		{
			RecreateDevice();
		}
		if (OnPreRender != null)
		{
			OnPreRender();
		}
		bool flag = false;
		UIRAtlasManager uIRAtlasManager = atlasManager;
		if (uIRAtlasManager != null && uIRAtlasManager.RequiresReset())
		{
			atlasManager.Reset();
			flag = true;
		}
		VectorImageManager obj = vectorImageManager;
		if (obj != null && obj.RequiresReset())
		{
			vectorImageManager.Reset();
			flag = true;
		}
		if (flag)
		{
			RepaintAtlassedElements();
		}
		m_DirtyTracker.dirtyID++;
		int num = 0;
		RenderDataDirtyTypes renderDataDirtyTypes = RenderDataDirtyTypes.Clipping | RenderDataDirtyTypes.ClippingHierarchy;
		RenderDataDirtyTypes dirtyTypesInverse = ~renderDataDirtyTypes;
		for (int i = m_DirtyTracker.minDepths[num]; i <= m_DirtyTracker.maxDepths[num]; i++)
		{
			VisualElement visualElement = m_DirtyTracker.heads[i];
			while (visualElement != null)
			{
				VisualElement nextDirty = visualElement.renderChainData.nextDirty;
				if ((visualElement.renderChainData.dirtiedValues & renderDataDirtyTypes) != RenderDataDirtyTypes.None)
				{
					if (visualElement.renderChainData.isInChain && visualElement.renderChainData.dirtyID != m_DirtyTracker.dirtyID)
					{
						RenderEvents.ProcessOnClippingChanged(this, visualElement, m_DirtyTracker.dirtyID, device, ref m_Stats);
					}
					m_DirtyTracker.ClearDirty(visualElement, dirtyTypesInverse);
				}
				visualElement = nextDirty;
			}
		}
		m_DirtyTracker.dirtyID++;
		num = 1;
		renderDataDirtyTypes = RenderDataDirtyTypes.Opacity | RenderDataDirtyTypes.OpacityHierarchy;
		dirtyTypesInverse = ~renderDataDirtyTypes;
		for (int j = m_DirtyTracker.minDepths[num]; j <= m_DirtyTracker.maxDepths[num]; j++)
		{
			VisualElement visualElement2 = m_DirtyTracker.heads[j];
			while (visualElement2 != null)
			{
				VisualElement nextDirty2 = visualElement2.renderChainData.nextDirty;
				if ((visualElement2.renderChainData.dirtiedValues & renderDataDirtyTypes) != RenderDataDirtyTypes.None)
				{
					if (visualElement2.renderChainData.isInChain && visualElement2.renderChainData.dirtyID != m_DirtyTracker.dirtyID)
					{
						RenderEvents.ProcessOnOpacityChanged(this, visualElement2, m_DirtyTracker.dirtyID, ref m_Stats);
					}
					m_DirtyTracker.ClearDirty(visualElement2, dirtyTypesInverse);
				}
				visualElement2 = nextDirty2;
			}
		}
		m_DirtyTracker.dirtyID++;
		num = 2;
		renderDataDirtyTypes = RenderDataDirtyTypes.Transform | RenderDataDirtyTypes.ClipRectSize;
		dirtyTypesInverse = ~renderDataDirtyTypes;
		for (int k = m_DirtyTracker.minDepths[num]; k <= m_DirtyTracker.maxDepths[num]; k++)
		{
			VisualElement visualElement3 = m_DirtyTracker.heads[k];
			while (visualElement3 != null)
			{
				VisualElement nextDirty3 = visualElement3.renderChainData.nextDirty;
				if ((visualElement3.renderChainData.dirtiedValues & renderDataDirtyTypes) != RenderDataDirtyTypes.None)
				{
					if (visualElement3.renderChainData.isInChain && visualElement3.renderChainData.dirtyID != m_DirtyTracker.dirtyID)
					{
						RenderEvents.ProcessOnTransformOrSizeChanged(this, visualElement3, m_DirtyTracker.dirtyID, device, ref m_Stats);
					}
					m_DirtyTracker.ClearDirty(visualElement3, dirtyTypesInverse);
				}
				visualElement3 = nextDirty3;
			}
		}
		m_BlockDirtyRegistration = true;
		m_DirtyTracker.dirtyID++;
		num = 3;
		renderDataDirtyTypes = RenderDataDirtyTypes.Visuals | RenderDataDirtyTypes.VisualsHierarchy;
		dirtyTypesInverse = ~renderDataDirtyTypes;
		for (int l = m_DirtyTracker.minDepths[num]; l <= m_DirtyTracker.maxDepths[num]; l++)
		{
			VisualElement visualElement4 = m_DirtyTracker.heads[l];
			while (visualElement4 != null)
			{
				VisualElement nextDirty4 = visualElement4.renderChainData.nextDirty;
				if ((visualElement4.renderChainData.dirtiedValues & renderDataDirtyTypes) != RenderDataDirtyTypes.None)
				{
					if (visualElement4.renderChainData.isInChain && visualElement4.renderChainData.dirtyID != m_DirtyTracker.dirtyID)
					{
						RenderEvents.ProcessOnVisualsChanged(this, visualElement4, m_DirtyTracker.dirtyID, ref m_Stats);
					}
					m_DirtyTracker.ClearDirty(visualElement4, dirtyTypesInverse);
				}
				visualElement4 = nextDirty4;
			}
		}
		m_BlockDirtyRegistration = false;
		m_DirtyTracker.Reset();
		ProcessTextRegen(timeSliced: true);
		if (m_FontWasReset)
		{
			for (int m = 0; m < 2; m++)
			{
				if (!m_FontWasReset)
				{
					break;
				}
				m_FontWasReset = false;
				ProcessTextRegen(timeSliced: false);
			}
		}
		atlasManager?.Commit();
		vectorImageManager?.Commit();
		shaderInfoAllocator.IssuePendingStorageChanges();
		if (this.BeforeDrawChain != null)
		{
			this.BeforeDrawChain(device);
		}
		Exception immediateException = null;
		device.DrawChain(m_FirstCommand, viewport, projection, clearFlags, atlasManager?.atlas, vectorImageManager?.atlas, shaderInfoAllocator.atlas, (panel as BaseVisualElementPanel).scaledPixelsPerPoint, shaderInfoAllocator.transformConstants, shaderInfoAllocator.clipRectConstants, ref immediateException);
		if (immediateException != null)
		{
			if (GUIUtility.IsExitGUIException(immediateException))
			{
				throw immediateException;
			}
			throw new ImmediateModeException(immediateException);
		}
		if (drawStats)
		{
			DrawStats();
		}
	}

	private void ProcessTextRegen(bool timeSliced)
	{
		if ((timeSliced && m_DirtyTextRemaining == 0) || m_TextElementCount == 0)
		{
			return;
		}
		if (m_TextUpdatePainter == null)
		{
			m_TextUpdatePainter = new UIRTextUpdatePainter();
		}
		VisualElement visualElement = m_FirstTextElement;
		m_DirtyTextStartIndex = (timeSliced ? (m_DirtyTextStartIndex % m_TextElementCount) : 0);
		for (int i = 0; i < m_DirtyTextStartIndex; i++)
		{
			visualElement = visualElement.renderChainData.nextText;
		}
		if (visualElement == null)
		{
			visualElement = m_FirstTextElement;
		}
		int num = (timeSliced ? Math.Min(50, m_DirtyTextRemaining) : m_TextElementCount);
		for (int j = 0; j < num; j++)
		{
			RenderEvents.ProcessRegenText(this, visualElement, m_TextUpdatePainter, device, ref m_Stats);
			visualElement = visualElement.renderChainData.nextText;
			m_DirtyTextStartIndex++;
			if (visualElement == null)
			{
				visualElement = m_FirstTextElement;
				m_DirtyTextStartIndex = 0;
			}
		}
		m_DirtyTextRemaining = Math.Max(0, m_DirtyTextRemaining - num);
		if (m_DirtyTextRemaining > 0)
		{
			(panel as BaseVisualElementPanel)?.OnVersionChanged(m_FirstTextElement, VersionChangeType.Transform);
		}
	}

	public void UIEOnStandardShaderChanged(Shader standardShader)
	{
		device.standardShader = RenderEvents.ResolveShader(standardShader);
	}

	public void UIEOnChildAdded(VisualElement parent, VisualElement ve, int index)
	{
		if (m_BlockDirtyRegistration)
		{
			throw new InvalidOperationException("VisualElements cannot be added to an active visual tree during generateVisualContent callback execution");
		}
		if (parent == null || parent.renderChainData.isInChain)
		{
			uint num = RenderEvents.DepthFirstOnChildAdded(this, parent, ve, index, resetState: true);
			Debug.Assert(ve.renderChainData.isInChain);
			Debug.Assert(ve.panel == panel);
			UIEOnClippingChanged(ve, hierarchical: true);
			UIEOnOpacityChanged(ve);
			UIEOnVisualsChanged(ve, hierarchical: true);
			m_StatsElementsAdded += num;
		}
	}

	public void UIEOnChildrenReordered(VisualElement ve)
	{
		if (m_BlockDirtyRegistration)
		{
			throw new InvalidOperationException("VisualElements cannot be moved under an active visual tree during generateVisualContent callback execution");
		}
		int childCount = ve.hierarchy.childCount;
		for (int i = 0; i < childCount; i++)
		{
			RenderEvents.DepthFirstOnChildRemoving(this, ve.hierarchy[i]);
		}
		for (int j = 0; j < childCount; j++)
		{
			RenderEvents.DepthFirstOnChildAdded(this, ve, ve.hierarchy[j], j, resetState: false);
		}
		UIEOnClippingChanged(ve, hierarchical: true);
		UIEOnOpacityChanged(ve, hierarchical: true);
		UIEOnVisualsChanged(ve, hierarchical: true);
	}

	public void UIEOnChildRemoving(VisualElement ve)
	{
		if (m_BlockDirtyRegistration)
		{
			throw new InvalidOperationException("VisualElements cannot be removed from an active visual tree during generateVisualContent callback execution");
		}
		m_StatsElementsRemoved += RenderEvents.DepthFirstOnChildRemoving(this, ve);
		Debug.Assert(!ve.renderChainData.isInChain);
	}

	public void StopTrackingGroupTransformElement(VisualElement ve)
	{
		m_LastGroupTransformElementScale.Remove(ve);
	}

	public void UIEOnClippingChanged(VisualElement ve, bool hierarchical)
	{
		if (ve.renderChainData.isInChain)
		{
			if (m_BlockDirtyRegistration)
			{
				throw new InvalidOperationException("VisualElements cannot change clipping state under an active visual tree during generateVisualContent callback execution");
			}
			m_DirtyTracker.RegisterDirty(ve, (RenderDataDirtyTypes)(4 | (hierarchical ? 8 : 0)), 0);
		}
	}

	public void UIEOnOpacityChanged(VisualElement ve, bool hierarchical = false)
	{
		if (ve.renderChainData.isInChain)
		{
			if (m_BlockDirtyRegistration)
			{
				throw new InvalidOperationException("VisualElements cannot change opacity under an active visual tree during generateVisualContent callback execution");
			}
			m_DirtyTracker.RegisterDirty(ve, (RenderDataDirtyTypes)(0x40 | (hierarchical ? 128 : 0)), 1);
		}
	}

	public void UIEOnTransformOrSizeChanged(VisualElement ve, bool transformChanged, bool clipRectSizeChanged)
	{
		if (ve.renderChainData.isInChain)
		{
			if (m_BlockDirtyRegistration)
			{
				throw new InvalidOperationException("VisualElements cannot change size or transform under an active visual tree during generateVisualContent callback execution");
			}
			RenderDataDirtyTypes dirtyTypes = (RenderDataDirtyTypes)((transformChanged ? 1 : 0) | (clipRectSizeChanged ? 2 : 0));
			m_DirtyTracker.RegisterDirty(ve, dirtyTypes, 2);
		}
	}

	public void UIEOnVisualsChanged(VisualElement ve, bool hierarchical)
	{
		if (ve.renderChainData.isInChain)
		{
			if (m_BlockDirtyRegistration)
			{
				throw new InvalidOperationException("VisualElements cannot be marked for dirty repaint under an active visual tree during generateVisualContent callback execution");
			}
			m_DirtyTracker.RegisterDirty(ve, (RenderDataDirtyTypes)(0x10 | (hierarchical ? 32 : 0)), 3);
		}
	}

	internal void EnsureFitsDepth(int depth)
	{
		m_DirtyTracker.EnsureFits(depth);
	}

	internal void ChildWillBeRemoved(VisualElement ve)
	{
		if (ve.renderChainData.dirtiedValues != RenderDataDirtyTypes.None)
		{
			m_DirtyTracker.ClearDirty(ve, ~ve.renderChainData.dirtiedValues);
		}
		Debug.Assert(ve.renderChainData.dirtiedValues == RenderDataDirtyTypes.None);
		Debug.Assert(ve.renderChainData.prevDirty == null);
		Debug.Assert(ve.renderChainData.nextDirty == null);
	}

	internal RenderChainCommand AllocCommand()
	{
		RenderChainCommand renderChainCommand = m_CommandPool.Get();
		renderChainCommand.Reset();
		return renderChainCommand;
	}

	internal void FreeCommand(RenderChainCommand cmd)
	{
		cmd.Reset();
		m_CommandPool.Return(cmd);
	}

	internal void OnRenderCommandAdded(RenderChainCommand firstCommand)
	{
		if (firstCommand.prev == null)
		{
			m_FirstCommand = firstCommand;
		}
	}

	internal void OnRenderCommandRemoved(RenderChainCommand firstCommand, RenderChainCommand lastCommand)
	{
		if (firstCommand.prev == null)
		{
			m_FirstCommand = lastCommand.next;
		}
	}

	internal void AddTextElement(VisualElement ve)
	{
		if (m_FirstTextElement != null)
		{
			m_FirstTextElement.renderChainData.prevText = ve;
			ve.renderChainData.nextText = m_FirstTextElement;
		}
		m_FirstTextElement = ve;
		m_TextElementCount++;
	}

	internal void RemoveTextElement(VisualElement ve)
	{
		if (ve.renderChainData.prevText != null)
		{
			ve.renderChainData.prevText.renderChainData.nextText = ve.renderChainData.nextText;
		}
		if (ve.renderChainData.nextText != null)
		{
			ve.renderChainData.nextText.renderChainData.prevText = ve.renderChainData.prevText;
		}
		if (m_FirstTextElement == ve)
		{
			m_FirstTextElement = ve.renderChainData.nextText;
		}
		ve.renderChainData.prevText = (ve.renderChainData.nextText = null);
		m_TextElementCount--;
	}

	internal void OnGroupTransformElementChangedTransform(VisualElement ve)
	{
		if (!m_LastGroupTransformElementScale.TryGetValue(ve, out var value) || ve.worldTransform.m00 != value.x || ve.worldTransform.m11 != value.y)
		{
			m_DirtyTextRemaining = m_TextElementCount;
			m_LastGroupTransformElementScale[ve] = new Vector2(ve.worldTransform.m00, ve.worldTransform.m11);
		}
	}

	internal void BeforeRenderDeviceRelease()
	{
		Debug.Assert(device != null);
		Debug.Assert(m_RenderDeviceRestoreInfo.root == null);
		m_RenderDeviceRestoreInfo.root = GetFirstElementInPanel(m_FirstCommand?.owner);
		m_RenderDeviceRestoreInfo.standardShader = device.standardShader;
		m_RenderDeviceRestoreInfo.hasAtlasMan = atlasManager != null;
		m_RenderDeviceRestoreInfo.hasVectorImageMan = vectorImageManager != null;
		UIEOnChildRemoving(m_RenderDeviceRestoreInfo.root);
		Destructor();
	}

	internal void AfterRenderDeviceRelease()
	{
		if (disposed)
		{
			DisposeHelper.NotifyDisposedUsed(this);
		}
		Debug.Assert(device == null);
		VisualElement root = m_RenderDeviceRestoreInfo.root;
		IPanel panelObj = root.panel;
		UIRenderDevice deviceObj = new UIRenderDevice(m_RenderDeviceRestoreInfo.standardShader);
		UIRAtlasManager atlasMan = (m_RenderDeviceRestoreInfo.hasAtlasMan ? new UIRAtlasManager() : null);
		VectorImageManager vectorImageMan = (m_RenderDeviceRestoreInfo.hasVectorImageMan ? new VectorImageManager(atlasMan) : null);
		m_RenderDeviceRestoreInfo = default(RenderDeviceRestoreInfo);
		Constructor(panelObj, deviceObj, atlasMan, vectorImageMan);
		UIEOnChildAdded(root.parent, root, (root.hierarchy.parent != null) ? root.hierarchy.parent.IndexOf(panel.visualTree) : 0);
	}

	internal void RecreateDevice()
	{
		BeforeRenderDeviceRelease();
		AfterRenderDeviceRelease();
	}

	private void RepaintAtlassedElements()
	{
		for (VisualElement visualElement = GetFirstElementInPanel(m_FirstCommand?.owner); visualElement != null; visualElement = visualElement.renderChainData.next)
		{
			if (visualElement.renderChainData.usesAtlas)
			{
				UIEOnVisualsChanged(visualElement, hierarchical: false);
			}
		}
		UIEOnOpacityChanged(panel.visualTree);
	}

	private void OnFontReset(Font font)
	{
		m_FontWasReset = true;
	}

	private void DrawStats()
	{
		bool flag = device != null;
		float num = 12f;
		Rect position = new Rect(30f, 60f, 1000f, 100f);
		GUI.Box(new Rect(20f, 40f, 200f, flag ? 380 : 256), "UIElements Draw Stats");
		GUI.Label(position, "Elements added\t: " + m_Stats.elementsAdded);
		position.y += num;
		GUI.Label(position, "Elements removed\t: " + m_Stats.elementsRemoved);
		position.y += num;
		GUI.Label(position, "Mesh allocs allocated\t: " + m_Stats.newMeshAllocations);
		position.y += num;
		GUI.Label(position, "Mesh allocs updated\t: " + m_Stats.updatedMeshAllocations);
		position.y += num;
		GUI.Label(position, "Clip update roots\t: " + m_Stats.recursiveClipUpdates);
		position.y += num;
		GUI.Label(position, "Clip update total\t: " + m_Stats.recursiveClipUpdatesExpanded);
		position.y += num;
		GUI.Label(position, "Opacity update roots\t: " + m_Stats.recursiveOpacityUpdates);
		position.y += num;
		GUI.Label(position, "Opacity update total\t: " + m_Stats.recursiveOpacityUpdatesExpanded);
		position.y += num;
		GUI.Label(position, "Xform update roots\t: " + m_Stats.recursiveTransformUpdates);
		position.y += num;
		GUI.Label(position, "Xform update total\t: " + m_Stats.recursiveTransformUpdatesExpanded);
		position.y += num;
		GUI.Label(position, "Xformed by bone\t: " + m_Stats.boneTransformed);
		position.y += num;
		GUI.Label(position, "Xformed by skipping\t: " + m_Stats.skipTransformed);
		position.y += num;
		GUI.Label(position, "Xformed by nudging\t: " + m_Stats.nudgeTransformed);
		position.y += num;
		GUI.Label(position, "Xformed by repaint\t: " + m_Stats.visualUpdateTransformed);
		position.y += num;
		GUI.Label(position, "Visual update roots\t: " + m_Stats.recursiveVisualUpdates);
		position.y += num;
		GUI.Label(position, "Visual update total\t: " + m_Stats.recursiveVisualUpdatesExpanded);
		position.y += num;
		GUI.Label(position, "Visual update flats\t: " + m_Stats.nonRecursiveVisualUpdates);
		position.y += num;
		GUI.Label(position, "Group-xform updates\t: " + m_Stats.groupTransformElementsChanged);
		position.y += num;
		GUI.Label(position, "Text regens\t: " + m_Stats.textUpdates);
		position.y += num;
		if (flag)
		{
			position.y += num;
			UIRenderDevice.DrawStatistics drawStatistics = device.GatherDrawStatistics();
			GUI.Label(position, "Frame index\t: " + drawStatistics.currentFrameIndex);
			position.y += num;
			GUI.Label(position, "Command count\t: " + drawStatistics.commandCount);
			position.y += num;
			GUI.Label(position, "Draw commands\t: " + drawStatistics.drawCommandCount);
			position.y += num;
			GUI.Label(position, "Draw range start\t: " + drawStatistics.currentDrawRangeStart);
			position.y += num;
			GUI.Label(position, "Draw ranges\t: " + drawStatistics.drawRangeCount);
			position.y += num;
			GUI.Label(position, "Draw range calls\t: " + drawStatistics.drawRangeCallCount);
			position.y += num;
			GUI.Label(position, "Material sets\t: " + drawStatistics.materialSetCount);
			position.y += num;
			GUI.Label(position, "Immediate draws\t: " + drawStatistics.immediateDraws);
			position.y += num;
			GUI.Label(position, "Total triangles\t: " + drawStatistics.totalIndices / 3);
			position.y += num;
		}
	}

	private static VisualElement GetFirstElementInPanel(VisualElement ve)
	{
		while (ve != null)
		{
			VisualElement prev = ve.renderChainData.prev;
			if (prev == null || !prev.renderChainData.isInChain)
			{
				break;
			}
			ve = ve.renderChainData.prev;
		}
		return ve;
	}
}
