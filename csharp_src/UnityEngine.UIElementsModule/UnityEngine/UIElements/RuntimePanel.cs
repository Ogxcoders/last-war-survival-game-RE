namespace UnityEngine.UIElements;

internal class RuntimePanel : Panel
{
	internal RenderTexture targetTexture = null;

	public RuntimePanel(ScriptableObject ownerObject, EventDispatcher dispatcher = null)
		: base(ownerObject, ContextType.Player, dispatcher)
	{
	}

	public override void Repaint(Event e)
	{
		if (targetTexture == null)
		{
			base.clearFlags = PanelClearFlags.Depth;
			base.Repaint(e);
			return;
		}
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = targetTexture;
		base.clearFlags = PanelClearFlags.All;
		base.Repaint(e);
		RenderTexture.active = active;
	}
}
