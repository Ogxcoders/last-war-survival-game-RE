using UnityEngine;

[DisallowMultipleComponent]
public class BattlefieldBuildingObject : MonoBehaviour
{
	[SerializeField]
	private Material[] skinMaterials;

	[SerializeField]
	public Renderer renderer;

	[SerializeField]
	private int currentIndex = -1;

	public SpriteRenderer iconRenderer;

	public GameObject iconNode;

	public void SetStyleIndex(int index)
	{
		if (currentIndex != index && skinMaterials != null && index < skinMaterials.Length && index >= 0 && !(renderer == null))
		{
			Material material = skinMaterials[index];
			if (!(material == null))
			{
				renderer.sharedMaterial = material;
				currentIndex = index;
			}
		}
	}

	private void EditorChangeStyle()
	{
		if (skinMaterials == null || skinMaterials.Length == 0)
		{
			Debug.LogError("切换失败,skinMaterials为空");
		}
		else if (currentIndex + 1 >= skinMaterials.Length)
		{
			SetStyleIndex(0);
		}
		else if (currentIndex < 0)
		{
			SetStyleIndex(0);
		}
		else
		{
			SetStyleIndex(currentIndex + 1);
		}
	}

	private Color GetColorByIndex(int index)
	{
		if (index == currentIndex)
		{
			return Color.yellow;
		}
		return Color.clear;
	}

	public void SetIcon(string iconPath)
	{
		if (!(iconNode == null) && !(iconRenderer == null))
		{
			if (string.IsNullOrEmpty(iconPath))
			{
				iconNode.SetActive(value: false);
				return;
			}
			iconNode.SetActive(value: true);
			iconRenderer.LoadSprite(iconPath);
		}
	}
}
