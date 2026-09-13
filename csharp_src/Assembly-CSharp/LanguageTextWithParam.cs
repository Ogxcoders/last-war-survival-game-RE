using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class LanguageTextWithParam : MonoBehaviour
{
	[SerializeField]
	private string key;

	[SerializeField]
	private string[] param;

	private void Awake()
	{
		if (string.IsNullOrEmpty(key))
		{
			return;
		}
		object[] array = null;
		if (param != null && param.Length != 0)
		{
			array = new object[param.Length];
			for (int i = 0; i < param.Length; i++)
			{
				array[i] = param[i];
			}
		}
		TextMeshProUGUI component = GetComponent<TextMeshProUGUI>();
		if (component != null)
		{
			component.text = GameEntry.Localization.GetString(key, array);
			return;
		}
		Text component2 = GetComponent<Text>();
		if (component2 != null)
		{
			component2.text = GameEntry.Localization.GetString(key, array);
			return;
		}
		TextMeshProUGUIEx component3 = GetComponent<TextMeshProUGUIEx>();
		if (component3 != null)
		{
			component3.text = GameEntry.Localization.GetString(key, array);
			return;
		}
		TextMeshProEx component4 = GetComponent<TextMeshProEx>();
		if (component4 != null)
		{
			component4.text = GameEntry.Localization.GetString(key, array);
			return;
		}
		SuperTextMesh component5 = GetComponent<SuperTextMesh>();
		if (component5 != null)
		{
			component5.text = GameEntry.Localization.GetString(key, array);
		}
	}
}
