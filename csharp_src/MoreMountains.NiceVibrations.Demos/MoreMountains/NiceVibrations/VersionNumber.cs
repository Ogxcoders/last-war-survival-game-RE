using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.NiceVibrations;

[RequireComponent(typeof(Text))]
public class VersionNumber : MonoBehaviour
{
	public string Version = "v3.3";

	protected Text _text;

	protected virtual void Awake()
	{
		_text = base.gameObject.GetComponent<Text>();
	}

	protected virtual void Start()
	{
		_text.text = Version;
		if (MMVibrationManager.iOS())
		{
			Text text = _text;
			text.text = text.text + " iOS " + MMVibrationManager.iOSVersion;
		}
		if (MMVibrationManager.Android())
		{
			Text text2 = _text;
			text2.text = text2.text + " Android " + MMNVAndroid.AndroidSDKVersion();
		}
	}
}
