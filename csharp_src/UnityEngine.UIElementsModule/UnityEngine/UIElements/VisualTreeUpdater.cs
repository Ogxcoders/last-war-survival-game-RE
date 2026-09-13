using System;

namespace UnityEngine.UIElements;

internal sealed class VisualTreeUpdater : IDisposable
{
	private class UpdaterArray
	{
		private IVisualTreeUpdater[] m_VisualTreeUpdaters;

		public IVisualTreeUpdater this[VisualTreeUpdatePhase phase]
		{
			get
			{
				return m_VisualTreeUpdaters[(int)phase];
			}
			set
			{
				m_VisualTreeUpdaters[(int)phase] = value;
			}
		}

		public IVisualTreeUpdater this[int index]
		{
			get
			{
				return m_VisualTreeUpdaters[index];
			}
			set
			{
				m_VisualTreeUpdaters[index] = value;
			}
		}

		public UpdaterArray()
		{
			m_VisualTreeUpdaters = new IVisualTreeUpdater[7];
		}
	}

	private BaseVisualElementPanel m_Panel;

	private UpdaterArray m_UpdaterArray;

	public VisualTreeUpdater(BaseVisualElementPanel panel)
	{
		m_Panel = panel;
		m_UpdaterArray = new UpdaterArray();
		SetDefaultUpdaters();
	}

	public void Dispose()
	{
		for (int i = 0; i < 7; i++)
		{
			IVisualTreeUpdater visualTreeUpdater = m_UpdaterArray[i];
			visualTreeUpdater.Dispose();
		}
	}

	public void UpdateVisualTree()
	{
		for (int i = 0; i < 7; i++)
		{
			IVisualTreeUpdater visualTreeUpdater = m_UpdaterArray[i];
			using (visualTreeUpdater.profilerMarker.Auto())
			{
				visualTreeUpdater.Update();
			}
		}
	}

	public void UpdateVisualTreePhase(VisualTreeUpdatePhase phase)
	{
		IVisualTreeUpdater visualTreeUpdater = m_UpdaterArray[phase];
		using (visualTreeUpdater.profilerMarker.Auto())
		{
			visualTreeUpdater.Update();
		}
	}

	public void OnVersionChanged(VisualElement ve, VersionChangeType versionChangeType)
	{
		for (int i = 0; i < 7; i++)
		{
			IVisualTreeUpdater visualTreeUpdater = m_UpdaterArray[i];
			visualTreeUpdater.OnVersionChanged(ve, versionChangeType);
		}
	}

	public void SetUpdater(IVisualTreeUpdater updater, VisualTreeUpdatePhase phase)
	{
		m_UpdaterArray[phase]?.Dispose();
		updater.panel = m_Panel;
		m_UpdaterArray[phase] = updater;
	}

	public void SetUpdater<T>(VisualTreeUpdatePhase phase) where T : IVisualTreeUpdater, new()
	{
		m_UpdaterArray[phase]?.Dispose();
		T val = new T
		{
			panel = m_Panel
		};
		m_UpdaterArray[phase] = val;
	}

	public IVisualTreeUpdater GetUpdater(VisualTreeUpdatePhase phase)
	{
		return m_UpdaterArray[phase];
	}

	private void SetDefaultUpdaters()
	{
		SetUpdater<VisualTreeViewDataUpdater>(VisualTreeUpdatePhase.ViewData);
		SetUpdater<VisualTreeBindingsUpdater>(VisualTreeUpdatePhase.Bindings);
		SetUpdater<VisualElementAnimationSystem>(VisualTreeUpdatePhase.Animation);
		SetUpdater<VisualTreeStyleUpdater>(VisualTreeUpdatePhase.Styles);
		SetUpdater<UIRLayoutUpdater>(VisualTreeUpdatePhase.Layout);
		SetUpdater<VisualTreeTransformClipUpdater>(VisualTreeUpdatePhase.TransformClip);
		SetUpdater<UIRRepaintUpdater>(VisualTreeUpdatePhase.Repaint);
	}
}
