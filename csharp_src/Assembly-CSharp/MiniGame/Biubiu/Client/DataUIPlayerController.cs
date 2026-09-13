using System.Collections;
using DG.Tweening;
using RootMotion.FinalIK;
using UnityEngine;

namespace MiniGame.Biubiu.Client;

public class DataUIPlayerController : DataUIEntityController
{
	public Transform _target;

	public Transform _bulletSpawnPoint;

	public Transform _preIvisTran;

	public GameObject AimGo;

	public Transform UIPos;

	public GameObject GoLine_Yellow;

	public GameObject GoLine_Red;

	private DataResourceLoaderEnv _env;

	public AimIK _aimIK;

	public LimbIK _limbIK;

	public float TargetChangeDt;

	public bool CanToCD;

	public bool Inited;

	private float IKWeight;

	public int PlayerID { get; private set; }

	public Camera Camera => _env.Camera;

	public Vector3 GunAimPosition => _target.position - base.transform.position;

	public Vector3 GunAimPositionSyn { get; set; } = Vector3.zero;

	public bool Downing { get; set; }

	public float DowningTime { get; set; }

	public bool Drag
	{
		get
		{
			if (Downing)
			{
				return Time.time - DowningTime > 0.2f;
			}
			return false;
		}
	}

	public float ClientFireCD { get; set; }

	public float ClientFireTime { get; set; }

	private bool IKEnable
	{
		set
		{
			SetIKWeight(value ? 1f : 0f);
		}
	}

	public void BindLoaderEnv(DataResourceLoaderEnv env)
	{
		_env = env;
	}

	public void Init(bool controller, EPlayerID playerID)
	{
		Inited = true;
		PlayerID = (int)playerID;
		AimGo.SetActive(controller);
	}

	private void Update()
	{
		UpdateFireCd();
	}

	private bool HasCD()
	{
		return ClientFireCD > 0f;
	}

	private void UpdateFireCd()
	{
		if (CanToCD)
		{
			ClientFireCD -= Time.unscaledDeltaTime;
		}
		bool num = HasCD();
		bool flag = !num;
		if (GoLine_Yellow != null && GoLine_Yellow.activeSelf != flag)
		{
			GoLine_Yellow.SetActive(flag);
		}
		bool flag2 = num;
		if (GoLine_Red != null && GoLine_Red.activeSelf != flag2)
		{
			GoLine_Red.SetActive(flag2);
		}
	}

	public void GetInputPos(out Vector3 chestPos, out Vector3 inputPos)
	{
		chestPos = Camera.ScreenToWorldPoint(Input.mousePosition);
		inputPos = new Vector3(chestPos.x, chestPos.y, _preIvisTran.position.z);
		if (Vector3.Distance(inputPos, _preIvisTran.position) < 1f)
		{
			inputPos += 2f * (inputPos - _preIvisTran.position).normalized;
		}
	}

	public override void Fire(DataUIRenderMessage.UIEntityAction entityAction, DataUIRenderLogic.RenderContext renderContext)
	{
		PlayAnimation("attack", 2, 0.5f, delegate
		{
			if (renderContext.World != null && !renderContext.World.GetShared<SharedRuntime>().GameOver && entityAction.IsMe && AimGo != null)
			{
				AimGo.SetActive(value: true);
			}
		});
		if (entityAction.IsMe)
		{
			if (renderContext.go_effec_z_x != null)
			{
				renderContext.go_effec_z_x.SetActive(value: false);
			}
			if (AimGo != null)
			{
				AimGo.SetActive(value: false);
			}
			CanToCD = true;
			ClientFireCD = entityAction.FireCdTime;
		}
		base.Fire(entityAction, renderContext);
	}

	public override void GameEnd(DataUIRenderMessage.UIEntityAction entityAction, DataUIRenderLogic.RenderContext renderContext)
	{
		if (entityAction.Result == 0)
		{
			PlayAnimation("win", 3);
		}
		if (entityAction.IsMe)
		{
			if (renderContext.go_effec_z_x != null)
			{
				renderContext.go_effec_z_x.SetActive(value: false);
			}
			if (AimGo != null)
			{
				AimGo.SetActive(value: false);
			}
		}
		IKEnable = false;
		base.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
	}

	public override void Die()
	{
		PlayAnimation("dead01", 3);
		base.Die();
	}

	public override void Reload(DataUIRenderLogic.RenderContext renderContext)
	{
		StartCoroutine(WaitToReload(renderContext));
	}

	private void SetIKWeight(float weight)
	{
		if (_aimIK != null)
		{
			_aimIK.solver.SetIKPositionWeight(weight);
			_limbIK.solver.SetIKPositionWeight(weight);
			_limbIK.solver.SetIKRotationWeight(weight);
		}
	}

	private IEnumerator WaitToReload(DataUIRenderLogic.RenderContext renderContext)
	{
		yield return new WaitForSeconds(0.1f);
		IKEnable = false;
		IKWeight = 0f;
		AimGo.transform.localScale = Vector3.zero;
		PlayAnimation("reload", 2, 0.5f, delegate
		{
			DOTween.To(() => IKWeight, delegate(float x)
			{
				IKWeight = x;
			}, 1f, 0.5f).OnUpdate(delegate
			{
				SetIKWeight(IKWeight);
			}).OnComplete(delegate
			{
				if ((bool)AimGo)
				{
					AimGo.transform.localScale = Vector3.one;
				}
			});
			if (renderContext.World != null)
			{
				SharedRuntime shared = renderContext.World.GetShared<SharedRuntime>();
				if (FuncUI.HasClientUI(shared, renderContext.World))
				{
					FuncUI.GetClientUI(shared, renderContext.World).GetUIAdapt()?.RefreshUIShow();
				}
			}
		});
	}
}
