using UnityEngine;

public class ForceArabicText : MonoBehaviour
{
	public bool isControlNonAutoMirrorAlign;

	public TextAnchor ArabicLangForceAlign = TextAnchor.MiddleRight;

	public TextAnchor NonArabicLangForceAlign = TextAnchor.MiddleLeft;

	public bool isControlAutoMirrorAlign;

	public TextAnchor ArabicLangForceAlign_AutoMirror = TextAnchor.MiddleRight;

	public TextAnchor NonArabicLangForceAlign_AutoMirror = TextAnchor.MiddleLeft;

	public bool isControlAlignByTextInAutoMirror;

	public bool isProcessArabicInNonArabicLang;

	public Material ArabicTMProFontMaterial;

	public bool IsArabicDisableBoldFont;

	public bool IsReverseImage;

	private void Start()
	{
	}

	private void Update()
	{
	}
}
