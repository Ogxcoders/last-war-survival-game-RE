using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UISweepLight : MonoBehaviour
{
	public Material Material;

	public float _SweepXM = 1f;

	private RectTransform rectTransform;

	private bool isInPrefabMode;

	private Vector2 anchoredPos;

	private void Start()
	{
		AutoGetMaterial();
	}

	private void Update()
	{
		CheckPrefabMode();
		if (Material == null)
		{
			return;
		}
		rectTransform = GetComponent<RectTransform>();
		Material.SetFloat("_SweepXM", _SweepXM);
		Canvas rootCanvas = GetRootCanvas();
		if (!(rootCanvas == null))
		{
			if (rootCanvas != null && rootCanvas.renderMode == RenderMode.ScreenSpaceCamera)
			{
				anchoredPos = rectTransform.anchoredPosition;
			}
			else
			{
				anchoredPos = rectTransform.position - rootCanvas.GetComponent<RectTransform>().position;
			}
			if (isInPrefabMode)
			{
				anchoredPos = rectTransform.anchoredPosition;
			}
			Material.SetFloat("_UIPosX", anchoredPos.x);
			Material.SetFloat("_UIPosY", anchoredPos.y);
		}
	}

	private void AutoGetMaterial()
	{
		Image component = GetComponent<Image>();
		if (component != null && component.material != null)
		{
			Material = component.material;
			return;
		}
		RawImage component2 = GetComponent<RawImage>();
		if (component2 != null && component2.material != null)
		{
			Material = component2.material;
			return;
		}
		Renderer component3 = GetComponent<Renderer>();
		if (component3 != null && component3.sharedMaterial != null)
		{
			Material = component3.sharedMaterial;
			return;
		}
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in componentsInChildren)
		{
			if (renderer.sharedMaterial != null)
			{
				Material = renderer.sharedMaterial;
				return;
			}
		}
		Graphic[] componentsInChildren2 = GetComponentsInChildren<Graphic>();
		foreach (Graphic graphic in componentsInChildren2)
		{
			if (graphic.material != null)
			{
				Material = graphic.material;
				return;
			}
		}
		Debug.LogWarning("在 " + base.gameObject.name + " 上找不到材质球！");
	}

	private void CheckPrefabMode()
	{
		isInPrefabMode = false;
	}

	private Vector2 GetRuntimePosition()
	{
		if (GetComponentInParent<Canvas>() == null)
		{
			return Vector2.zero;
		}
		return rectTransform.anchoredPosition;
	}

	private Vector2 GetPrefabModePosition()
	{
		return rectTransform.anchoredPosition;
	}

	private Canvas GetRootCanvas()
	{
		Canvas componentInParent = GetComponentInParent<Canvas>();
		if (componentInParent == null)
		{
			return null;
		}
		return componentInParent.rootCanvas;
	}
}
