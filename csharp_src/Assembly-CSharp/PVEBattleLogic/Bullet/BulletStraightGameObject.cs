using GameFramework;
using UnityEngine;
using XLua;

namespace PVEBattleLogic.Bullet;

public class BulletStraightGameObject : BulletGameObject
{
	private const float DIE_PERCENT = 0.99f;

	private long _objId;

	private float _lifeTime;

	private Vector3 _startPos;

	private float _rotY;

	private float _bulletScale;

	private int _targetLayerMask;

	private float _metaColliderRadius;

	private bool _noCollision;

	private float _duration;

	private float _spiralLoops;

	private float _spiralRadius;

	private float _flySpeed;

	private Vector3 _inertiaVelocity;

	private AnimationCurve _animationCurve;

	private bool _colliderType;

	private float _defaultDotMaxCD;

	private float _scaledTime;

	private float _angleSpeed;

	private Vector3 _worldVelocity;

	private Vector3 _totalDisplacement;

	private float _dimension;

	private Vector3 _curColliderCenterPos;

	private Vector3 _colliderCenterOffset;

	private Vector3 _worldRight;

	private Vector3 _worldUp;

	private float _dotMaxCD;

	private bool _logicDie;

	public long objId => _objId;

	public BulletStraightGameObject(string bulletViewName)
		: base(bulletViewName)
	{
		_visible = true;
	}

	public void InitViewParam(LuaArrAccess createStraightArrAccess)
	{
		_logicDie = false;
		_objId = createStraightArrAccess.GetInt(2);
		_startPos = new Vector3((float)createStraightArrAccess.GetDouble(3), (float)createStraightArrAccess.GetDouble(4), (float)createStraightArrAccess.GetDouble(5));
		_rotY = (float)createStraightArrAccess.GetDouble(6);
		_targetLayerMask = createStraightArrAccess.GetInt(7);
		_inertiaVelocity = new Vector3((float)createStraightArrAccess.GetDouble(8), (float)createStraightArrAccess.GetDouble(9), (float)createStraightArrAccess.GetDouble(10));
		_lifeTime = (float)createStraightArrAccess.GetDouble(11);
		_bulletScale = (float)createStraightArrAccess.GetDouble(12);
		_metaColliderRadius = (float)createStraightArrAccess.GetDouble(13);
		_noCollision = createStraightArrAccess.GetInt(14) == 1;
		_duration = (float)createStraightArrAccess.GetDouble(15);
		_spiralLoops = (float)createStraightArrAccess.GetDouble(16);
		_spiralRadius = (float)createStraightArrAccess.GetDouble(17);
		_flySpeed = (float)createStraightArrAccess.GetDouble(18);
		if (!BulletViewFacade.TryGetAnimationCurve(createStraightArrAccess.GetInt(19), out var animationCurve) || string.IsNullOrEmpty(animationCurve))
		{
			animationCurve = "0,0,2,2|1,1,0,0";
		}
		_animationCurve = BulletViewFacade.GetAnimationCurve(animationCurve);
		_colliderType = createStraightArrAccess.GetInt(20) == 1;
		_defaultDotMaxCD = (float)createStraightArrAccess.GetDouble(21);
		_scaledTime = 0f;
		_dotMaxCD = _defaultDotMaxCD;
	}

	public void InitViewParam(LuaArrAccess createStraightArrAccess, int indexFix)
	{
		_logicDie = false;
		_objId = createStraightArrAccess.GetInt(indexFix + 2);
		_startPos = new Vector3((float)createStraightArrAccess.GetDouble(indexFix + 3), (float)createStraightArrAccess.GetDouble(indexFix + 4), (float)createStraightArrAccess.GetDouble(indexFix + 5));
		_rotY = (float)createStraightArrAccess.GetDouble(indexFix + 6);
		_targetLayerMask = createStraightArrAccess.GetInt(indexFix + 7);
		_inertiaVelocity = new Vector3((float)createStraightArrAccess.GetDouble(indexFix + 8), (float)createStraightArrAccess.GetDouble(indexFix + 9), (float)createStraightArrAccess.GetDouble(indexFix + 10));
		_lifeTime = (float)createStraightArrAccess.GetDouble(indexFix + 11);
		_bulletScale = (float)createStraightArrAccess.GetDouble(indexFix + 12);
		_metaColliderRadius = (float)createStraightArrAccess.GetDouble(indexFix + 13);
		_noCollision = createStraightArrAccess.GetInt(indexFix + 14) == 1;
		_duration = (float)createStraightArrAccess.GetDouble(indexFix + 15);
		_spiralLoops = (float)createStraightArrAccess.GetDouble(indexFix + 16);
		_spiralRadius = (float)createStraightArrAccess.GetDouble(indexFix + 17);
		_flySpeed = (float)createStraightArrAccess.GetDouble(indexFix + 18);
		if (!BulletViewFacade.TryGetAnimationCurve(createStraightArrAccess.GetInt(indexFix + 19), out var animationCurve) || string.IsNullOrEmpty(animationCurve))
		{
			animationCurve = "0,0,2,2|1,1,0,0";
		}
		_animationCurve = BulletViewFacade.GetAnimationCurve(animationCurve);
		_colliderType = createStraightArrAccess.GetInt(indexFix + 20) == 1;
		_defaultDotMaxCD = (float)createStraightArrAccess.GetDouble(indexFix + 21);
		_scaledTime = 0f;
		_dotMaxCD = _defaultDotMaxCD;
	}

	public void InitViewParam(int viewObjId, BulletStraightGatlingViewParam straightGatlingViewParam, Vector3 startPos, float eulerAnglesY)
	{
		_objId = viewObjId;
		_logicDie = false;
		_startPos = startPos;
		_rotY = eulerAnglesY;
		_targetLayerMask = straightGatlingViewParam.targetLayerMask;
		_inertiaVelocity = straightGatlingViewParam.inertiaVelocity;
		_lifeTime = straightGatlingViewParam.lifeTime;
		_bulletScale = straightGatlingViewParam.bulletScale;
		_metaColliderRadius = straightGatlingViewParam.metaColliderRadius;
		_noCollision = straightGatlingViewParam.noCollision;
		_duration = straightGatlingViewParam.duration;
		_spiralLoops = straightGatlingViewParam.spiralLoops;
		_spiralRadius = straightGatlingViewParam.spiralRadius;
		_flySpeed = straightGatlingViewParam.flySpeed;
		_animationCurve = straightGatlingViewParam.animationCurve;
		_colliderType = straightGatlingViewParam.colliderType;
		_defaultDotMaxCD = straightGatlingViewParam.defaultDotMaxCD;
		_scaledTime = 0f;
		_dotMaxCD = _defaultDotMaxCD;
	}

	internal void InitViewParam(long bulletObjId, float startX, float startY, float startZ, float rotY, int targetLayerMask, int metaId, LuaTable paramTable)
	{
		_logicDie = false;
		_objId = bulletObjId;
		_startPos = new Vector3(startX, startY, startZ);
		_rotY = rotY;
		_targetLayerMask = targetLayerMask;
		_inertiaVelocity = Vector3.zero;
		if (BulletViewFacade.TryGetStraightViewParam(metaId, out var bulletStraightViewParam))
		{
			_lifeTime = bulletStraightViewParam.lifeTime;
			_bulletScale = bulletStraightViewParam.bulletScale;
			_metaColliderRadius = bulletStraightViewParam.metaColliderRadius;
			_noCollision = bulletStraightViewParam.noCollision;
			_duration = bulletStraightViewParam.duration;
			_spiralLoops = bulletStraightViewParam.spiralLoops;
			_spiralRadius = bulletStraightViewParam.spiralRadius;
			_flySpeed = bulletStraightViewParam.flySpeed;
			_animationCurve = bulletStraightViewParam.animationCurve;
			_colliderType = bulletStraightViewParam.colliderType;
			_defaultDotMaxCD = bulletStraightViewParam.defaultDotMaxCD;
		}
		else
		{
			_lifeTime = (bulletStraightViewParam.lifeTime = paramTable.Get<float>("lifeTime"));
			_bulletScale = (bulletStraightViewParam.bulletScale = paramTable.Get<float>("bulletScale"));
			_metaColliderRadius = (bulletStraightViewParam.metaColliderRadius = paramTable.Get<float>("metaColliderRadius"));
			_noCollision = (bulletStraightViewParam.noCollision = paramTable.Get<bool>("noCollision"));
			_duration = (bulletStraightViewParam.duration = paramTable.Get<float>("duration"));
			_spiralLoops = (bulletStraightViewParam.spiralLoops = paramTable.Get<float>("spiralLoops"));
			_spiralRadius = (bulletStraightViewParam.spiralRadius = paramTable.Get<float>("spiralRadius"));
			_flySpeed = (bulletStraightViewParam.flySpeed = paramTable.Get<float>("flySpeed"));
			string curve = paramTable.Get<string>("animCurve");
			_animationCurve = (bulletStraightViewParam.animationCurve = BulletViewFacade.GetAnimationCurve(curve));
			_colliderType = (bulletStraightViewParam.colliderType = paramTable.Get<bool>("colliderType"));
			_defaultDotMaxCD = (bulletStraightViewParam.defaultDotMaxCD = paramTable.Get<float>("defaultDotMaxCD"));
		}
		_scaledTime = 0f;
		_dotMaxCD = _defaultDotMaxCD;
	}

	internal void InitViewParamWithInertiaVelocity(long bulletObjId, float startX, float startY, float startZ, float rotY, int targetLayerMask, float inertiaX, float inertiaY, float inertiaZ, int metaId, LuaTable paramTable)
	{
		_logicDie = false;
		_objId = bulletObjId;
		_startPos = new Vector3(startX, startY, startZ);
		_rotY = rotY;
		_targetLayerMask = targetLayerMask;
		_inertiaVelocity = new Vector3(inertiaX, inertiaY, inertiaZ);
		if (BulletViewFacade.TryGetStraightViewParam(metaId, out var bulletStraightViewParam))
		{
			_lifeTime = bulletStraightViewParam.lifeTime;
			_bulletScale = bulletStraightViewParam.bulletScale;
			_metaColliderRadius = bulletStraightViewParam.metaColliderRadius;
			_noCollision = bulletStraightViewParam.noCollision;
			_duration = bulletStraightViewParam.duration;
			_spiralLoops = bulletStraightViewParam.spiralLoops;
			_spiralRadius = bulletStraightViewParam.spiralRadius;
			_flySpeed = bulletStraightViewParam.flySpeed;
			_animationCurve = bulletStraightViewParam.animationCurve;
			_colliderType = bulletStraightViewParam.colliderType;
			_defaultDotMaxCD = bulletStraightViewParam.defaultDotMaxCD;
		}
		else
		{
			_lifeTime = (bulletStraightViewParam.lifeTime = paramTable.Get<float>("lifeTime"));
			_bulletScale = (bulletStraightViewParam.bulletScale = paramTable.Get<float>("bulletScale"));
			_metaColliderRadius = (bulletStraightViewParam.metaColliderRadius = paramTable.Get<float>("metaColliderRadius"));
			_noCollision = (bulletStraightViewParam.noCollision = paramTable.Get<bool>("noCollision"));
			_duration = (bulletStraightViewParam.duration = paramTable.Get<float>("duration"));
			_spiralLoops = (bulletStraightViewParam.spiralLoops = paramTable.Get<float>("spiralLoops"));
			_spiralRadius = (bulletStraightViewParam.spiralRadius = paramTable.Get<float>("spiralRadius"));
			_flySpeed = (bulletStraightViewParam.flySpeed = paramTable.Get<float>("flySpeed"));
			string curve = paramTable.Get<string>("animCurve");
			_animationCurve = (bulletStraightViewParam.animationCurve = BulletViewFacade.GetAnimationCurve(curve));
			_colliderType = (bulletStraightViewParam.colliderType = paramTable.Get<bool>("colliderType"));
			_defaultDotMaxCD = (bulletStraightViewParam.defaultDotMaxCD = paramTable.Get<float>("defaultDotMaxCD"));
		}
		_scaledTime = 0f;
		_dotMaxCD = _defaultDotMaxCD;
	}

	internal void InitParam(LuaTable paramTable)
	{
		_logicDie = false;
		_objId = paramTable.Get<long>("objId");
		float x = paramTable.Get<float>("startPosX");
		float y = paramTable.Get<float>("startPosY");
		float z = paramTable.Get<float>("startPosZ");
		_startPos = new Vector3(x, y, z);
		_rotY = paramTable.Get<float>("rotY");
		_targetLayerMask = paramTable.Get<int>("targetLayerMask");
		_lifeTime = paramTable.Get<float>("lifeTime");
		_bulletScale = paramTable.Get<float>("bulletScale");
		_metaColliderRadius = paramTable.Get<float>("metaColliderRadius");
		_noCollision = paramTable.Get<bool>("noCollision");
		_duration = paramTable.Get<float>("duration");
		_spiralLoops = paramTable.Get<float>("spiralLoops");
		_spiralRadius = paramTable.Get<float>("spiralRadius");
		_flySpeed = paramTable.Get<float>("flySpeed");
		string curve = paramTable.Get<string>("animCurve");
		_animationCurve = BulletViewFacade.GetAnimationCurve(curve);
		_colliderType = paramTable.Get<bool>("colliderType");
		_defaultDotMaxCD = paramTable.Get<float>("defaultDotMaxCD");
		x = paramTable.Get<float>("inertiaVelocityX");
		y = paramTable.Get<float>("inertiaVelocityY");
		z = paramTable.Get<float>("inertiaVelocityZ");
		_inertiaVelocity = new Vector3(x, y, z);
		_scaledTime = 0f;
		_dotMaxCD = _defaultDotMaxCD;
	}

	internal override void Load()
	{
		base.Load();
		CheckLoaded();
	}

	private void CheckLoaded()
	{
		if (base.IsLoaded)
		{
			OnShow();
		}
	}

	internal override void OnLoaded()
	{
		base.OnLoaded();
		OnShow();
		if (Handle != -1)
		{
			BulletViewFacade.AppendStraightLoaded((int)_objId, _dotMaxCD);
		}
	}

	private void OnShow()
	{
		if (!_visible)
		{
			return;
		}
		_scaledTime = 0f;
		SetLocalScale(Vector3.one * _bulletScale);
		SetPosition(_startPos);
		SetEulerAngles(Vector3.up * _rotY);
		InitCollider();
		ClearTrailRenderer();
		Vector3 forward = GetForward();
		if (_noCollision)
		{
			_worldVelocity = forward * _flySpeed;
		}
		else
		{
			if (_spiralLoops > 0f)
			{
				_worldRight = GetRight();
				_worldUp = GetUp();
				_angleSpeed = _flySpeed;
				_flySpeed /= _spiralLoops;
			}
			_worldVelocity = forward * _flySpeed + _inertiaVelocity;
		}
		_totalDisplacement = _worldVelocity * _duration;
		if (_colliderType)
		{
			float num = Mathf.Sqrt(_worldVelocity.x * _worldVelocity.x + _worldVelocity.z * _worldVelocity.z);
			_dotMaxCD = _dimension / num;
		}
		BattleColliderUtils.SetBulletDotCD(_objId, _dotMaxCD);
	}

	private void InitCollider()
	{
		SphereCollider sphereCollider;
		CapsuleCollider capsuleCollider;
		if (_metaColliderRadius > 0f)
		{
			_dimension = _bulletScale * _metaColliderRadius;
			_curColliderCenterPos = _startPos;
			_colliderCenterOffset = Vector3.zero;
			BattleColliderUtils.AddBullet(_transform, _objId, _metaColliderRadius, _targetLayerMask);
		}
		else if (TryGetSphereCollider(out sphereCollider))
		{
			float radius = sphereCollider.radius;
			_dimension = _bulletScale * radius;
			Vector3 transformPoint = GetTransformPoint(sphereCollider.center);
			_curColliderCenterPos = transformPoint;
			_colliderCenterOffset = sphereCollider.center;
			BattleColliderUtils.AddBullet(_transform, _objId, radius * _bulletScale, _targetLayerMask);
		}
		else if (TryGetCapsuleCollider(out capsuleCollider))
		{
			float radius2 = capsuleCollider.radius;
			float height = capsuleCollider.height;
			int direction = capsuleCollider.direction;
			Vector3 vector = Vector3.right;
			float num = 0f;
			switch (direction)
			{
			case 1:
				vector = Vector3.up;
				break;
			case 2:
				vector = Vector3.forward;
				num = height;
				break;
			}
			Vector3 vector2 = vector * (height * 0.5f);
			_dimension = _bulletScale * (radius2 + num);
			Vector3 center = capsuleCollider.center;
			Vector3 transformPoint2 = GetTransformPoint(center);
			_curColliderCenterPos = transformPoint2;
			_colliderCenterOffset = center;
			BattleColliderUtils.AddCapsuleBullet(_transform, _objId, _bulletScale * radius2, _targetLayerMask, center.x - vector2.x, center.y - vector2.y, center.z - vector2.z, center.x + vector2.x, center.y + vector2.y, center.z + vector2.z);
		}
		else
		{
			Log.Error("子弹没有Sphere或Capsule Collider 并且没有配置radius，子弹名：" + base.Path);
		}
	}

	public void DeadDelay(float delayTime)
	{
		_logicDie = true;
		_lifeTime = delayTime;
	}

	internal bool UpdateTransform(float deltaTime, out int lifeEnd)
	{
		lifeEnd = 0;
		if (!base.IsLoaded)
		{
			return false;
		}
		_lifeTime -= deltaTime;
		if (_logicDie)
		{
			lifeEnd = 1;
			return _lifeTime <= 0f;
		}
		if (_lifeTime <= 0f)
		{
			lifeEnd = 1;
			return true;
		}
		if (_spiralLoops > 0f)
		{
			_scaledTime += deltaTime;
			float scaledTime = _scaledTime;
			float f = scaledTime * _angleSpeed;
			Vector3 vector = _worldRight * Mathf.Cos(f) + _worldUp * Mathf.Sin(f);
			vector *= _spiralRadius;
			Vector3 position = _startPos + _worldVelocity * scaledTime + vector;
			SetPosition(position);
		}
		else
		{
			_scaledTime += deltaTime;
			float num = _scaledTime / _duration;
			if (!(num < 0.99f))
			{
				return true;
			}
			float num2 = _animationCurve.Evaluate(num);
			Vector3 position2 = _startPos + _totalDisplacement * num2;
			SetPosition(position2);
		}
		return false;
	}

	public override Vector3 GetColliderCenterWorldPos()
	{
		return _transform.position + _colliderCenterOffset;
	}

	public override float GetDotMaxCD()
	{
		return _dotMaxCD;
	}
}
