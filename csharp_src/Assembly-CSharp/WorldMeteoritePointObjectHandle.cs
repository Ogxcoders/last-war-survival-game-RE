using TMPro;
using UnityEngine;

public class WorldMeteoritePointObjectHandle : MonoBehaviour
{
	public SpriteRenderer srLodIcon;

	public SpriteRenderer srIconState;

	public SpriteRenderer srModelState;

	public SpriteRenderer srScoreIcon;

	public GameObject goIconTroopNode;

	public CircleMeshInstanced srIconHead;

	public GameObject goResCountNode;

	public GameObject goModelRoot;

	public GameObject goModelNode;

	public GameObject goCountdownNode;

	public TextMeshPro textRemain;

	public SuperTextMesh textCountdown;

	public SpriteRenderer srProgress;

	public SpriteRenderer srMask;

	public UIPlayerHead srPlayer;

	public SpriteRenderer srFrame;

	public UIPlayerHead srPlayer2;

	public SpriteRenderer srFrame2;

	public Animation anim;

	public SimpleAnimation disappearAnimation;

	public GameObject iconNode;

	public SkinnedMeshRenderer modelMeshRenderer;

	public Material defaultMaterial;

	public void Dispose()
	{
		srIconHead?.Release();
	}

	public void ResetMaterial()
	{
		if (modelMeshRenderer != null && defaultMaterial != null)
		{
			modelMeshRenderer.sharedMaterial = defaultMaterial;
		}
	}
}
