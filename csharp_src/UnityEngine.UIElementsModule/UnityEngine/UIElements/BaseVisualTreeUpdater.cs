using System;
using Unity.Profiling;

namespace UnityEngine.UIElements;

internal abstract class BaseVisualTreeUpdater : IVisualTreeUpdater, IDisposable
{
	private BaseVisualElementPanel m_Panel;

	public BaseVisualElementPanel panel
	{
		get
		{
			return m_Panel;
		}
		set
		{
			m_Panel = value;
			if (this.panelChanged != null)
			{
				this.panelChanged(value);
			}
		}
	}

	public VisualElement visualTree => panel.visualTree;

	public abstract ProfilerMarker profilerMarker { get; }

	public event Action<BaseVisualElementPanel> panelChanged;

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public abstract void Update();

	public abstract void OnVersionChanged(VisualElement ve, VersionChangeType versionChangeType);
}
