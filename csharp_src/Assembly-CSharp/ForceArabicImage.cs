using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ForceArabicImage : MonoBehaviour
{
	public bool IsReverseImage = true;

	private void Start()
	{
		_ = GameEntry.Localization.Language;
		_ = 3;
	}

	private void ProcessText(RectTransform rect, int depth)
	{
		if (depth > 1 && rect.TryGetComponent<ForceArabicImage>(out var _))
		{
			return;
		}
		if ((rect.TryGetComponent<Text>(out var _) || rect.TryGetComponent<TextMeshProUGUI>(out var _)) && (depth & 1) == 1)
		{
			Vector3 localScale = rect.localScale;
			rect.localScale = new Vector3(0f - localScale.x, localScale.y, localScale.z);
			depth++;
		}
		foreach (RectTransform item in rect)
		{
			ProcessText(item, depth);
		}
	}

	private void Update()
	{
	}
}
