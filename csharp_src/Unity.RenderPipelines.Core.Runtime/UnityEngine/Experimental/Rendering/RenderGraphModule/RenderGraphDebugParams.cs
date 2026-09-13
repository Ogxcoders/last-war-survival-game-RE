using System.Collections.Generic;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule;

internal class RenderGraphDebugParams
{
	public bool enableRenderGraph;

	public bool tagResourceNamesWithRG;

	public bool clearRenderTargetsAtCreation;

	public bool clearRenderTargetsAtRelease;

	public bool unbindGlobalTextures;

	public bool logFrameInformation;

	public bool logResources;

	public void RegisterDebug()
	{
		List<DebugUI.Widget> list = new List<DebugUI.Widget>();
		list.Add(new DebugUI.BoolField
		{
			displayName = "Enable Render Graph",
			getter = () => enableRenderGraph,
			setter = delegate(bool value)
			{
				enableRenderGraph = value;
			}
		});
		list.Add(new DebugUI.BoolField
		{
			displayName = "Tag Resources with RG",
			getter = () => tagResourceNamesWithRG,
			setter = delegate(bool value)
			{
				tagResourceNamesWithRG = value;
			}
		});
		list.Add(new DebugUI.BoolField
		{
			displayName = "Clear Render Targets at creation",
			getter = () => clearRenderTargetsAtCreation,
			setter = delegate(bool value)
			{
				clearRenderTargetsAtCreation = value;
			}
		});
		list.Add(new DebugUI.BoolField
		{
			displayName = "Clear Render Targets at release",
			getter = () => clearRenderTargetsAtRelease,
			setter = delegate(bool value)
			{
				clearRenderTargetsAtRelease = value;
			}
		});
		list.Add(new DebugUI.BoolField
		{
			displayName = "Unbind Global Textures",
			getter = () => unbindGlobalTextures,
			setter = delegate(bool value)
			{
				unbindGlobalTextures = value;
			}
		});
		list.Add(new DebugUI.Button
		{
			displayName = "Log Frame Information",
			action = delegate
			{
				logFrameInformation = true;
			}
		});
		list.Add(new DebugUI.Button
		{
			displayName = "Log Resources",
			action = delegate
			{
				logResources = true;
			}
		});
		DebugManager.instance.GetPanel("Render Graph", createIfNull: true).children.Add(list.ToArray());
	}

	public void UnRegisterDebug()
	{
		DebugManager.instance.RemovePanel("Render Graph");
	}
}
