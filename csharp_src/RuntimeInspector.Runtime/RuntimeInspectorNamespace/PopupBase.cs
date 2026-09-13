using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RuntimeInspectorNamespace;

public abstract class PopupBase : MonoBehaviour
{
	private const float POINTER_VALIDATE_INTERVAL = 5f;

	[SerializeField]
	private LayoutElement borderLayoutElement;

	[SerializeField]
	private Image background;

	[SerializeField]
	protected Text label;

	private RectTransform rectTransform;

	private RectTransform canvasTransform;

	private Camera worldCamera;

	protected PointerEventData pointer;

	private float nextPointerValidation;

	private int m_skinVersion;

	private UISkin m_skin;

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
				m_skinVersion = m_skin.Version;
				borderLayoutElement.SetHeight((float)m_skin.LineHeight * 2.5f);
				background.GetComponent<LayoutElement>().minHeight = m_skin.LineHeight;
				float a = background.color.a;
				Color color = m_skin.InputFieldNormalBackgroundColor.Tint(0.05f);
				color.a = a;
				background.color = color;
				label.SetSkinInputFieldText(m_skin);
			}
		}
	}

	public void Initialize(Canvas canvas)
	{
		rectTransform = (RectTransform)base.transform;
		canvasTransform = (RectTransform)canvas.transform;
		if (canvas.renderMode == RenderMode.ScreenSpaceOverlay || (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera == null))
		{
			worldCamera = null;
		}
		else
		{
			worldCamera = (canvas.worldCamera ? canvas.worldCamera : Camera.main);
		}
	}

	protected void SetPointer(PointerEventData pointer)
	{
		this.pointer = pointer;
		nextPointerValidation = 5f;
		RepositionSelf();
	}

	protected void RepositionSelf()
	{
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, pointer.position, worldCamera, out var localPoint))
		{
			rectTransform.anchoredPosition = localPoint;
		}
	}

	protected abstract void DestroySelf();

	private void Update()
	{
		nextPointerValidation -= Time.unscaledDeltaTime;
		if (nextPointerValidation <= 0f)
		{
			nextPointerValidation = 5f;
			if (!pointer.IsPointerValid())
			{
				DestroySelf();
			}
		}
	}
}
