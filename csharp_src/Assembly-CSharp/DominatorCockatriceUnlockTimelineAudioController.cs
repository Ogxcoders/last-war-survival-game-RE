using UnityEngine;

public class DominatorCockatriceUnlockTimelineAudioController : MonoBehaviour
{
	[SerializeField]
	private string _audioClipPath;

	public void Awake()
	{
		if (!_audioClipPath.IsNullOrEmpty())
		{
			GameEntry.Sound.PlayTimelineSound(_audioClipPath);
		}
	}
}
