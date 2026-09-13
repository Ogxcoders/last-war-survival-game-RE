using DG.Tweening;
using UnityEngine;

public static class ObjectMover
{
	private const float moveSpeed = 5f;

	private const string idleAnimation = "idle";

	public static void MoveFromSpawn(GameObject obj, int eventId, int pointIndex)
	{
		if (obj == null)
		{
			return;
		}
		string text = eventId.ToString() + pointIndex;
		bool flag = GameEntry.Setting.PlayerPrefsGetBool("AttackCityS0Radar" + text, defaultValue: false);
		SimpleAnimation simpleAnim = obj.GetComponentInChildren<SimpleAnimation>(includeInactive: true);
		if (simpleAnim == null)
		{
			Debug.LogError(obj.name + "未找到 SimpleAnimation 组件！");
			return;
		}
		if (flag)
		{
			obj.transform.eulerAngles = new Vector3(0f, 180f, 0f);
			simpleAnim.Play("idle");
			return;
		}
		int num = 1;
		string stateName = "born";
		string templateData = GameEntry.ConfigCache.GetTemplateData("detect_event", eventId.ToInt(), "para3");
		if (!string.IsNullOrEmpty(templateData) && templateData.Contains("|"))
		{
			string[] array = templateData.Split(new char[1] { '|' });
			if (array.Length == 2)
			{
				num = array[0].ToInt();
				stateName = array[1];
			}
		}
		int index = GameEntry.Lua.CallWithReturn<int, int>("CSharpCallLuaInterface.GetAttackCityS0CityPointId", eventId);
		Vector2Int tilePos = SceneManager.World.IndexToTilePos(index);
		Vector3 vector = SceneManager.World.TileToWorld(tilePos);
		if (num == 1)
		{
			simpleAnim.Play(stateName);
			Transform transform = obj.transform;
			Vector3 position = transform.position;
			transform.position = vector;
			Vector3 normalized = (position - vector).normalized;
			if (normalized != Vector3.zero)
			{
				transform.forward = normalized;
			}
			float duration = Vector3.Distance(vector, position) / 5f;
			transform.DOMove(position, duration).SetEase(Ease.InOutSine).OnComplete(delegate
			{
				simpleAnim.Play("idle");
			})
				.SetLink(obj);
			GameEntry.Setting.PlayerPrefsSetBool("AttackCityS0Radar" + text, value: true);
			return;
		}
		SimpleAnimation.State state = simpleAnim.GetState(stateName);
		if (state != null && state.clip != null)
		{
			simpleAnim.Play(stateName);
			float length = state.clip.length;
			DOTween.Sequence().AppendInterval(length).AppendCallback(delegate
			{
				simpleAnim.Play("idle");
			})
				.SetLink(obj);
			GameEntry.Setting.PlayerPrefsSetBool("AttackCityS0Radar" + text, value: true);
		}
		else
		{
			simpleAnim.Play("idle");
		}
	}
}
