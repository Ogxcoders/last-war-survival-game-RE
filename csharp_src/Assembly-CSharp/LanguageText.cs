using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LanguageText : MonoBehaviour
{
	[SerializeField]
	private string key;

	private void Awake()
	{
		if (string.IsNullOrEmpty(key))
		{
			return;
		}
		TextMeshProUGUI component = GetComponent<TextMeshProUGUI>();
		if (component != null)
		{
			component.text = GameEntry.Localization.GetString(key);
			return;
		}
		Text component2 = GetComponent<Text>();
		TextMeshProUGUIEx component3 = GetComponent<TextMeshProUGUIEx>();
		TextMeshProEx component4 = GetComponent<TextMeshProEx>();
		if (component2 != null)
		{
			component2.text = GameEntry.Localization.GetString(key);
			return;
		}
		if (component3 != null)
		{
			component3.text = GameEntry.Localization.GetString(key);
			return;
		}
		if (component4 != null)
		{
			component4.text = GameEntry.Localization.GetString(key);
			return;
		}
		SuperTextMesh component5 = GetComponent<SuperTextMesh>();
		if (component5 != null)
		{
			component5.text = GameEntry.Localization.GetString(key);
		}
	}
}
