using UnityEngine;

public class WorldTroopDetectRescueStart : WorldTroopStateBase
{
	private float startTime;

	private const float totalTime = 4.5f;

	private const float qipaoShowTime = 2f;

	private const float qipaoHideTime = 3.33f;

	private bool isShowAnim;

	private GameObject qipao1;

	private GameObject qipao2;

	public WorldTroopDetectRescueStart(WorldTroop worldTroop, WorldTroopStateMachine stateMachine)
		: base(worldTroop, stateMachine)
	{
	}

	public override void OnStateEnter()
	{
		base.OnStateEnter();
		if (worldTroop != null && !isShowAnim)
		{
			worldTroop.SetRotationRoot();
			worldTroop.SetRotation(Quaternion.Euler(new Vector3(0f, 45f, 0f)));
			GameEntry.Event.Fire(EventId.ShowMarchTrans, worldTroop.GetMarchUUID());
			if (worldTroop.IsCanPlayAni())
			{
				worldTroop.PlayAnim("huangseyunshuji_01_show");
			}
			startTime = Time.realtimeSinceStartup;
			isShowAnim = true;
			worldTroop.HideJunkMan();
			qipao1 = worldTroop.GetModel().transform.Find("A_build_huangseyunshuji_01/A_build@huangseyunshuji_01_LOD1_skin/To_unity/Root/biaoqing_01/Qipao").gameObject;
			qipao2 = worldTroop.GetModel().transform.Find("A_build_huangseyunshuji_01/A_build@huangseyunshuji_01_LOD1_skin/To_unity/Root/biaoqing_02/Qipao").gameObject;
			qipao1.SetActive(value: false);
			qipao2.SetActive(value: false);
		}
	}

	public override void OnStateLeave()
	{
		base.OnStateLeave();
		if (worldTroop != null)
		{
			worldTroop.RemoveAttack();
			GameEntry.Event.Fire(EventId.HideMarchTrans, worldTroop.GetMarchUUID());
			base.OnStateLeave();
		}
	}

	public override void OnStateUpdate(float deltaTime)
	{
		base.OnStateUpdate(deltaTime);
		if (worldTroop == null)
		{
			return;
		}
		if (Time.realtimeSinceStartup - startTime > 3.33f)
		{
			qipao1.SetActive(value: false);
			qipao2.SetActive(value: false);
		}
		else if (Time.realtimeSinceStartup - startTime > 2f)
		{
			qipao1.SetActive(value: true);
			qipao2.SetActive(value: true);
		}
		if (Time.realtimeSinceStartup - startTime > 4.5f)
		{
			if (worldTroop.IsCanPlayAni())
			{
				worldTroop.PlayAnim("idle");
			}
			worldTroop.ShowJunkMan();
		}
		if (worldTroop.GetMarchTargetType() == MarchTargetType.BACK_HOME)
		{
			ChangeState(WorldTroopState.Move);
		}
	}
}
