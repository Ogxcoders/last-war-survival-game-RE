using UnityEngine;
using UnityEngine.SceneManagement;

public class iTweenEditor : MonoBehaviour
{
	public new string name = "";

	public bool autoPlay = true;

	public float waitTime = 0.25f;

	public float tweenTime = 2f;

	public iTween.LoopType loopType;

	public iTween.EaseType easeType = iTween.EaseType.linear;

	public bool ignoreTimescale = true;

	public virtual void iTweenPlay()
	{
	}

	public void LoadLevel(string screenName)
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(screenName);
	}

	public void LoadLevelAdditive(string screenName)
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(screenName, LoadSceneMode.Additive);
	}

	public void EnableGameObject(GameObject go)
	{
		go.SetActive(value: true);
	}

	public void DisableGameObject(GameObject go)
	{
		go.SetActive(value: false);
	}

	public void DestroyGameObject(GameObject go)
	{
		Object.Destroy(go);
	}

	public void EnableMonoBehaviour(MonoBehaviour mb)
	{
		mb.enabled = true;
	}

	public void DisableMonoBehaviour(MonoBehaviour mb)
	{
		mb.enabled = false;
	}

	public new void DestroyObject(Object obj)
	{
		Object.Destroy(obj);
	}

	public void PlayTween(iTweenEditor tween)
	{
		tween.iTweenPlay();
	}
}
