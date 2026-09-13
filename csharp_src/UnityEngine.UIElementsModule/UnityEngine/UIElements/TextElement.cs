using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements;

public class TextElement : BindableElement, ITextElement, INotifyValueChanged<string>
{
	public new class UxmlFactory : UxmlFactory<TextElement, UxmlTraits>
	{
	}

	public new class UxmlTraits : BindableElement.UxmlTraits
	{
		private UxmlStringAttributeDescription m_Text = new UxmlStringAttributeDescription
		{
			name = "text"
		};

		public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
		{
			get
			{
				yield break;
			}
		}

		public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
		{
			base.Init(ve, bag, cc);
			((ITextElement)ve).text = m_Text.GetValueFromBag(bag, cc);
		}
	}

	public static readonly string ussClassName = "unity-text-element";

	private TextHandle m_TextHandle = TextHandle.New();

	[SerializeField]
	private string m_Text;

	internal TextHandle textHandle => m_TextHandle;

	public virtual string text
	{
		get
		{
			return ((INotifyValueChanged<string>)this).value;
		}
		set
		{
			((INotifyValueChanged<string>)this).value = value;
		}
	}

	string INotifyValueChanged<string>.value
	{
		get
		{
			return m_Text ?? string.Empty;
		}
		set
		{
			if (!(m_Text != value))
			{
				return;
			}
			if (base.panel != null)
			{
				using (ChangeEvent<string> changeEvent = ChangeEvent<string>.GetPooled(text, value))
				{
					changeEvent.target = this;
					((INotifyValueChanged<string>)this).SetValueWithoutNotify(value);
					SendEvent(changeEvent);
					return;
				}
			}
			((INotifyValueChanged<string>)this).SetValueWithoutNotify(value);
		}
	}

	public TextElement()
	{
		base.requireMeasureFunction = true;
		AddToClassList(ussClassName);
		base.generateVisualContent = (Action<MeshGenerationContext>)Delegate.Combine(base.generateVisualContent, new Action<MeshGenerationContext>(OnGenerateVisualContent));
		RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
	}

	private void OnAttachToPanel(AttachToPanelEvent e)
	{
		m_TextHandle.useLegacy = e.destinationPanel.contextType == ContextType.Editor;
	}

	private void OnGenerateVisualContent(MeshGenerationContext mgc)
	{
		mgc.Text(MeshGenerationContextUtils.TextParams.MakeStyleBased(this, text), m_TextHandle, base.scaledPixelsPerPoint);
	}

	public Vector2 MeasureTextSize(string textToMeasure, float width, MeasureMode widthMode, float height, MeasureMode heightMode)
	{
		return MeasureVisualElementTextSize(this, textToMeasure, width, widthMode, height, heightMode, m_TextHandle);
	}

	internal static Vector2 MeasureVisualElementTextSize(VisualElement ve, string textToMeasure, float width, MeasureMode widthMode, float height, MeasureMode heightMode, TextHandle textHandle)
	{
		float x = float.NaN;
		float y = float.NaN;
		Font value = ve.computedStyle.unityFont.value;
		if (textToMeasure == null || value == null)
		{
			return new Vector2(x, y);
		}
		Vector3 vector = ve.ComputeGlobalScale();
		float num = (vector.x + vector.y) * 0.5f * ve.scaledPixelsPerPoint;
		if (num <= 0f)
		{
			return Vector2.zero;
		}
		if (widthMode == MeasureMode.Exactly)
		{
			x = width;
		}
		else
		{
			MeshGenerationContextUtils.TextParams textSettings = GetTextSettings(ve, textToMeasure);
			textSettings.wordWrap = false;
			textSettings.richText = false;
			x = Mathf.Ceil(textHandle.ComputeTextWidth(textSettings, num));
			if (widthMode == MeasureMode.AtMost)
			{
				x = Mathf.Min(x, width);
			}
		}
		if (heightMode == MeasureMode.Exactly)
		{
			y = height;
		}
		else
		{
			MeshGenerationContextUtils.TextParams textSettings2 = GetTextSettings(ve, textToMeasure);
			textSettings2.wordWrapWidth = x;
			textSettings2.richText = false;
			y = Mathf.Ceil(textHandle.ComputeTextHeight(textSettings2, num));
			if (heightMode == MeasureMode.AtMost)
			{
				y = Mathf.Min(y, height);
			}
		}
		return new Vector2(x, y);
	}

	protected internal override Vector2 DoMeasure(float desiredWidth, MeasureMode widthMode, float desiredHeight, MeasureMode heightMode)
	{
		return MeasureTextSize(text, desiredWidth, widthMode, desiredHeight, heightMode);
	}

	private static MeshGenerationContextUtils.TextParams GetTextSettings(VisualElement ve, string text)
	{
		ComputedStyle computedStyle = ve.computedStyle;
		return new MeshGenerationContextUtils.TextParams
		{
			rect = ve.contentRect,
			text = text,
			font = computedStyle.unityFont.value,
			fontSize = (int)computedStyle.fontSize.value.value,
			fontStyle = computedStyle.unityFontStyleAndWeight.value,
			fontColor = computedStyle.color.value,
			anchor = computedStyle.unityTextAlign.value,
			wordWrap = (computedStyle.whiteSpace.value == WhiteSpace.Normal),
			wordWrapWidth = ((computedStyle.whiteSpace.value == WhiteSpace.Normal) ? ve.contentRect.width : 0f),
			richText = true
		};
	}

	void INotifyValueChanged<string>.SetValueWithoutNotify(string newValue)
	{
		if (m_Text != newValue)
		{
			m_Text = newValue;
			IncrementVersion(VersionChangeType.Layout | VersionChangeType.Repaint);
			if (!string.IsNullOrEmpty(base.viewDataKey))
			{
				SaveViewData();
			}
		}
	}
}
