using System;
using System.Collections.Generic;
using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements;

public class Image : VisualElement
{
	public new class UxmlFactory : UxmlFactory<Image, UxmlTraits>
	{
	}

	public new class UxmlTraits : VisualElement.UxmlTraits
	{
		public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
		{
			get
			{
				yield break;
			}
		}
	}

	private ScaleMode m_ScaleMode;

	private Texture m_Image;

	private VectorImage m_VectorImage;

	private Rect m_UV;

	private Color m_TintColor;

	private bool m_ImageIsInline;

	private bool m_ScaleModeIsInline;

	private bool m_TintColorIsInline;

	public static readonly string ussClassName = "unity-image";

	private static CustomStyleProperty<Texture2D> s_ImageProperty = new CustomStyleProperty<Texture2D>("--unity-image");

	private static CustomStyleProperty<VectorImage> s_VectorImageProperty = new CustomStyleProperty<VectorImage>("--unity-image");

	private static CustomStyleProperty<string> s_ScaleModeProperty = new CustomStyleProperty<string>("--unity-image-size");

	private static CustomStyleProperty<Color> s_TintColorProperty = new CustomStyleProperty<Color>("--unity-image-tint-color");

	public Texture image
	{
		get
		{
			return m_Image;
		}
		set
		{
			if (value != null && vectorImage != null)
			{
				Debug.LogError("Both image and vectorImage are set on Image object");
				m_VectorImage = null;
			}
			m_ImageIsInline = value != null;
			if (m_Image != value)
			{
				m_Image = value;
				IncrementVersion(VersionChangeType.Layout | VersionChangeType.Repaint);
				if (m_Image == null)
				{
					m_UV = new Rect(0f, 0f, 1f, 1f);
				}
			}
		}
	}

	public VectorImage vectorImage
	{
		get
		{
			return m_VectorImage;
		}
		set
		{
			if (value != null && image != null)
			{
				Debug.LogError("Both image and vectorImage are set on Image object");
				m_Image = null;
			}
			m_ImageIsInline = value != null;
			if (m_VectorImage != value)
			{
				m_VectorImage = value;
				IncrementVersion(VersionChangeType.Layout | VersionChangeType.Repaint);
				if (m_VectorImage == null)
				{
					m_UV = new Rect(0f, 0f, 1f, 1f);
				}
			}
		}
	}

	public Rect sourceRect
	{
		get
		{
			return GetSourceRect();
		}
		set
		{
			CalculateUV(value);
		}
	}

	public Rect uv
	{
		get
		{
			return m_UV;
		}
		set
		{
			m_UV = value;
		}
	}

	public ScaleMode scaleMode
	{
		get
		{
			return m_ScaleMode;
		}
		set
		{
			m_ScaleModeIsInline = true;
			if (m_ScaleMode != value)
			{
				m_ScaleMode = value;
				IncrementVersion(VersionChangeType.Layout);
			}
		}
	}

	public Color tintColor
	{
		get
		{
			return m_TintColor;
		}
		set
		{
			m_TintColorIsInline = true;
			if (m_TintColor != value)
			{
				m_TintColor = value;
				IncrementVersion(VersionChangeType.Repaint);
			}
		}
	}

	public Image()
	{
		AddToClassList(ussClassName);
		m_ScaleMode = ScaleMode.ScaleAndCrop;
		m_TintColor = Color.white;
		m_UV = new Rect(0f, 0f, 1f, 1f);
		base.requireMeasureFunction = true;
		RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyleResolved);
		base.generateVisualContent = (Action<MeshGenerationContext>)Delegate.Combine(base.generateVisualContent, new Action<MeshGenerationContext>(OnGenerateVisualContent));
	}

	private Vector2 GetTextureDisplaySize(Texture texture)
	{
		Vector2 result = Vector2.zero;
		if (texture != null)
		{
			result = new Vector2(texture.width, texture.height);
		}
		return result;
	}

	protected internal override Vector2 DoMeasure(float desiredWidth, MeasureMode widthMode, float desiredHeight, MeasureMode heightMode)
	{
		float x = float.NaN;
		float y = float.NaN;
		if (image == null && vectorImage == null)
		{
			return new Vector2(x, y);
		}
		Vector2 zero = Vector2.zero;
		zero = ((!(image != null)) ? vectorImage.size : GetTextureDisplaySize(image));
		Rect rect = sourceRect;
		bool flag = rect != Rect.zero;
		x = (flag ? rect.width : zero.x);
		y = (flag ? rect.height : zero.y);
		if (widthMode == MeasureMode.AtMost)
		{
			x = Mathf.Min(x, desiredWidth);
		}
		if (heightMode == MeasureMode.AtMost)
		{
			y = Mathf.Min(y, desiredHeight);
		}
		return new Vector2(x, y);
	}

	private void OnGenerateVisualContent(MeshGenerationContext mgc)
	{
		if (!(image == null) || !(vectorImage == null))
		{
			MeshGenerationContextUtils.RectangleParams rectParams = default(MeshGenerationContextUtils.RectangleParams);
			if (image != null)
			{
				rectParams = MeshGenerationContextUtils.RectangleParams.MakeTextured(base.contentRect, uv, image, scaleMode, base.panel.contextType);
			}
			else if (vectorImage != null)
			{
				rectParams = MeshGenerationContextUtils.RectangleParams.MakeVectorTextured(base.contentRect, uv, vectorImage, scaleMode, base.panel.contextType);
			}
			rectParams.color = tintColor;
			mgc.Rectangle(rectParams);
		}
	}

	private void OnCustomStyleResolved(CustomStyleResolvedEvent e)
	{
		Texture2D value = null;
		VectorImage value2 = null;
		Color value3 = Color.white;
		ICustomStyle customStyle = e.customStyle;
		if (!m_ImageIsInline && customStyle.TryGetValue(s_ImageProperty, out value))
		{
			m_Image = value;
			if (m_Image != null)
			{
				m_VectorImage = null;
			}
		}
		if (!m_ImageIsInline && customStyle.TryGetValue(s_VectorImageProperty, out value2))
		{
			m_VectorImage = value2;
			if (m_VectorImage != null)
			{
				m_Image = null;
			}
		}
		if (!m_ScaleModeIsInline && customStyle.TryGetValue(s_ScaleModeProperty, out var value4) && StyleSheetCache.TryParseEnum<ScaleMode>(value4, out var intValue))
		{
			m_ScaleMode = (ScaleMode)intValue;
		}
		if (!m_TintColorIsInline && customStyle.TryGetValue(s_TintColorProperty, out value3))
		{
			m_TintColor = value3;
		}
	}

	private void CalculateUV(Rect srcRect)
	{
		m_UV = new Rect(0f, 0f, 1f, 1f);
		Vector2 vector = Vector2.zero;
		Texture texture = image;
		if (texture != null)
		{
			vector = GetTextureDisplaySize(texture);
		}
		VectorImage vectorImage = this.vectorImage;
		if (vectorImage != null)
		{
			vector = vectorImage.size;
		}
		if (vector != Vector2.zero)
		{
			m_UV.x = srcRect.x / vector.x;
			m_UV.width = srcRect.width / vector.x;
			m_UV.height = srcRect.height / vector.y;
			m_UV.y = 1f - m_UV.height - srcRect.y / vector.y;
		}
	}

	private Rect GetSourceRect()
	{
		Rect zero = Rect.zero;
		Vector2 vector = Vector2.zero;
		Texture texture = image;
		if (texture != null)
		{
			vector = GetTextureDisplaySize(texture);
		}
		VectorImage vectorImage = this.vectorImage;
		if (vectorImage != null)
		{
			vector = vectorImage.size;
		}
		if (vector != Vector2.zero)
		{
			zero.x = uv.x * vector.x;
			zero.width = uv.width * vector.x;
			zero.y = (1f - uv.y - uv.height) * vector.y;
			zero.height = uv.height * vector.y;
		}
		return zero;
	}
}
