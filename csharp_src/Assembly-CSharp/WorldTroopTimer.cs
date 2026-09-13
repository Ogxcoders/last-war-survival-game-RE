using UnityEngine;

public class WorldTroopTimer : MonoBehaviour
{
	public TextMeshProEx countDown;

	private bool _isRunning;

	private long _startTime;

	private long _endTime;

	public void SetData(long endTime, Vector3 dstPos)
	{
		_startTime = GameEntry.Timer.GetServerTime();
		_endTime = endTime;
		_isRunning = true;
		base.transform.position = dstPos;
		base.gameObject.SetActive(value: true);
	}

	private void Update()
	{
		if (_isRunning && !(countDown == null))
		{
			long num = _endTime - GameEntry.Timer.GetServerTime();
			if (num <= 0)
			{
				countDown.text = "";
				_isRunning = false;
				base.gameObject.SetActive(value: false);
			}
			else
			{
				string text = GameEntry.Timer.MillisecondToSecondString(num, ":");
				string text2 = GameEntry.Localization.GetString("newbies_march_time_tips1", text);
				countDown.text = text2;
			}
		}
	}
}
