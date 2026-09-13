using UnityEngine;
using UnityEngine.UI;

public class AutoChangeFont : MonoBehaviour
{
	private Text _text;

	private void Awake()
	{
		_text = GetComponent<Text>();
		if (_text != null)
		{
			Font fontByLanguage = GameEntry.Localization.GetFontByLanguage();
			if (fontByLanguage != null)
			{
				_text.font = fontByLanguage;
			}
		}
	}
}
