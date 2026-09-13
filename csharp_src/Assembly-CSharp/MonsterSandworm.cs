using UnityEngine;

public class MonsterSandworm : MonoBehaviour
{
	public static class AnimName
	{
		public const string Show = "shachong_zuanchu";

		public const string Breathe = "shachong_huxi";

		public const string Idle = "shachong_daiji";

		public const string Exit = "shachong_zuanjin";
	}

	[SerializeField]
	private GPUSkinningAnimator _monsterAnim;

	[SerializeField]
	private SimpleAnimation _effectAnim;

	public static string[] TestAnimList = new string[12]
	{
		"shachong_zuanchu", "shachong_daiji", "shachong_huxi", "shachong_huxi", "shachong_huxi", "shachong_huxi", "shachong_huxi", "shachong_daiji", "shachong_huxi", "shachong_huxi",
		"shachong_huxi", "shachong_zuanjin"
	};

	public static float UnderWaitTime = 4f;

	private int _curIndex;

	private bool _isTimerDuring;

	private bool _isActive;

	private void Awake()
	{
		_curIndex = 0;
		float delaySec = (float)Random.Range(10, 50) / 10f;
		_isTimerDuring = true;
		_isActive = true;
		GameEntry.Timer.RegisterTimer(delaySec, delegate
		{
			_isTimerDuring = false;
			CheckPlayAnim();
		});
	}

	private float Play(string animName)
	{
		string text = animName + "_effect";
		if (_effectAnim.GetState(text) != null)
		{
			UIUtils.PlayAnimationReturnTime(_effectAnim, text);
		}
		_monsterAnim.Play(animName);
		return _monsterAnim.GetClipLength(animName);
	}

	private void CheckPlayAnim()
	{
		if (!_isActive)
		{
			return;
		}
		if (_curIndex >= TestAnimList.Length)
		{
			_isTimerDuring = true;
			GameEntry.Timer.RegisterTimer(UnderWaitTime, delegate
			{
				_isTimerDuring = false;
				_curIndex = 0;
				CheckPlayAnim();
			});
			return;
		}
		float num = Play(TestAnimList[_curIndex]);
		if (num > 0f)
		{
			_isTimerDuring = true;
			GameEntry.Timer.RegisterTimer(num, delegate
			{
				_isTimerDuring = false;
				_curIndex++;
				CheckPlayAnim();
			});
		}
	}

	private void OnDisable()
	{
		_isActive = false;
	}

	private void OnEnable()
	{
		_isActive = true;
		if (!_isTimerDuring)
		{
			CheckPlayAnim();
		}
	}
}
