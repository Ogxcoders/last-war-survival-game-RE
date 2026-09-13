using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class UIFormBlurEffect : MonoBehaviour
{
	private RawImage blurImage;

	private void Awake()
	{
		blurImage = GetComponent<RawImage>();
	}

	private void OnEnable()
	{
		blurImage.color = new Color(0f, 0f, 0f, 0.75f);
	}
}
