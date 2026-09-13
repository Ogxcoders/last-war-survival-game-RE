using UnityEngine;
using UnityEngine.UI;

namespace RuntimeInspectorNamespace;

public class ObjectReferencePickerItem : RecycledListItem
{
	[SerializeField]
	private Image background;

	[SerializeField]
	private RawImage texturePreview;

	private LayoutElement texturePreviewLayoutElement;

	[SerializeField]
	private Text referenceNameText;

	private int m_skinVersion;

	private UISkin m_skin;

	private bool m_isSelected;

	public object Reference { get; private set; }

	public UISkin Skin
	{
		get
		{
			return m_skin;
		}
		set
		{
			if (m_skin != value || m_skinVersion != m_skin.Version)
			{
				m_skin = value;
				((RectTransform)base.transform).sizeDelta = new Vector2(0f, Skin.LineHeight);
				int num = Mathf.Max(5, Skin.LineHeight - 7);
				texturePreviewLayoutElement.SetWidth(num);
				texturePreviewLayoutElement.SetHeight(num);
				referenceNameText.SetSkinText(m_skin);
				IsSelected = m_isSelected;
			}
		}
	}

	public bool IsSelected
	{
		get
		{
			return m_isSelected;
		}
		set
		{
			m_isSelected = value;
			if (m_isSelected)
			{
				background.color = Skin.SelectedItemBackgroundColor;
				referenceNameText.color = Skin.SelectedItemTextColor;
			}
			else
			{
				background.color = Color.clear;
				referenceNameText.color = Skin.TextColor;
			}
		}
	}

	private void Awake()
	{
		texturePreviewLayoutElement = texturePreview.GetComponent<LayoutElement>();
		GetComponent<PointerEventListener>().PointerClick += delegate
		{
			OnClick();
		};
	}

	public void SetContent(object reference, string displayName)
	{
		Reference = reference;
		referenceNameText.text = displayName;
		Texture texture = (reference as Object).GetTexture();
		if (texture != null)
		{
			texturePreview.gameObject.SetActive(value: true);
			texturePreview.texture = texture;
		}
		else
		{
			texturePreview.gameObject.SetActive(value: false);
		}
	}
}
