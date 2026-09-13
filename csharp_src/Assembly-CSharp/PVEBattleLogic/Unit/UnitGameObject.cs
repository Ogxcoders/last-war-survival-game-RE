using System;
using System.Collections.Generic;
using DG.Tweening;
using GameFramework;
using RiverGame.Rendering.MaterialPropertyBlockUtilities;
using TMPro;
using UnityEngine;

namespace PVEBattleLogic.Unit;

public class UnitGameObject
{
	private string _path;

	internal bool _isLoaded;

	public int Handle;

	public int ObjId;

	public int Type;

	internal bool _visible;

	private InstanceRequest _request;

	internal GameObject _gameObject;

	internal Transform _transform;

	private Tweener _posTween;

	private bool _valid;

	private Vector3 _position;

	private Vector3 _rotation;

	private float _scale;

	private int _layer;

	private Transform _parent;

	private CitySpaceManTrigger _citySpaceManTrigger;

	private bool _citySpaceManTriggeredValid;

	private SimpleAnimation _simpleAnimation;

	private bool _simpleAnimationValid;

	private string _curAnimName;

	private Transform _canonTransform;

	private int _appearanceMetaId;

	private Transform[] _firePoints;

	private int _firePointCount;

	private int _firePointIndex;

	private Transform _firePoint;

	private Transform[] _buffPoints;

	private int _buffPointIndex;

	private Transform _uiPoint;

	private Collider _collider;

	private bool _colliderValid;

	private bool _colliderEnabled;

	private List<Renderer> _renderers;

	private int _rendererCount;

	private List<Material> _renderMaterials;

	private List<GPUSkinnedMeshRenderer> _gsRenderers;

	private MaterialPropertyGroup _mpbEffect;

	private bool _replaceEffect;

	private GameObject _numberHpTextGo;

	private TextMeshProEx _numberHpTextMeshProEx;

	private bool _numberHpTransformValid;

	private SuperTextMesh _numberHpSuperTextMesh;

	private bool _numberHpSuperTransformValid;

	private AnimatorCullingMode _defaultCullingMode;

	private Tweener _hpScaleTween;

	private Renderer _glassRenderer;

	private Renderer _waterRenderer;

	private int _cmpAttachedApperanceId;

	private Transform[] _cmpAttachedTransforms;

	private Transform _rotateRoot;

	private static readonly string ShadowString = "shadow";

	public string Path => _path;

	public bool IsLoaded => _isLoaded;

	public UnitGameObject(string path)
	{
		_path = path;
		_visible = false;
		_valid = true;
		_isLoaded = false;
		_colliderEnabled = false;
	}

	public void Show(Vector3 position, Vector3 rotation, Transform parent, float scale = 1f, int layer = -1)
	{
		_position = position;
		_rotation = rotation;
		_scale = scale;
		_layer = layer;
		_parent = parent;
		_visible = true;
		_colliderEnabled = true;
		Load();
		if (_isLoaded)
		{
			OnShow();
		}
	}

	internal void Load()
	{
		if (!string.IsNullOrEmpty(_path) && _request == null)
		{
			_isLoaded = false;
			int property = ((Handle == -1) ? 1 : 2);
			_request = GameEntry.Resource.InstantiateAsync(_path, ObjectPoolTag.Normal, property);
			_request.completed += RequestOnCompleted;
		}
	}

	private void RequestOnCompleted(InstanceRequest req)
	{
		if (req.isError)
		{
			req.Destroy();
		}
		else if (_valid)
		{
			_gameObject = req.gameObject;
			_transform = _gameObject.transform;
			_isLoaded = true;
			OnLoaded();
			if (_visible)
			{
				OnShow();
			}
			_gameObject.SetActive(value: true);
			SetVisibleImp(_visible);
			if (Handle != -1)
			{
				UnitViewFacade.AppendLoaded(ObjId, Handle);
			}
		}
	}

	internal virtual void OnLoaded()
	{
		_simpleAnimation = _gameObject.GetComponentInChildren<SimpleAnimation>(includeInactive: true);
		_simpleAnimationValid = _simpleAnimation != null;
		if (_simpleAnimationValid)
		{
			_defaultCullingMode = _simpleAnimation.cullingMode;
			_simpleAnimation.cullingMode = AnimatorCullingMode.AlwaysAnimate;
		}
		_citySpaceManTrigger = _gameObject.GetComponentInChildren<CitySpaceManTrigger>();
		_citySpaceManTriggeredValid = _citySpaceManTrigger != null;
		if (!_citySpaceManTriggeredValid)
		{
			Log.Error("该单位根节点没挂CitySpaceManTrigger脚本: " + _gameObject.name);
		}
		_collider = _gameObject.GetComponentInChildren<Collider>();
		_colliderValid = _collider != null;
		if (_colliderValid)
		{
			if (BattleColliderUtils.IsCollider2D())
			{
				_collider.enabled = false;
			}
			else
			{
				_collider.enabled = _colliderEnabled;
			}
		}
		InitRender();
		_rotateRoot = _transform.Find("RotateRoot");
	}

	public void SetColliderLayer(int layer)
	{
		_layer = layer;
		if (_colliderValid)
		{
			_collider.gameObject.layer = layer;
		}
	}

	public bool InitFirePoints(int appearanceId, int count)
	{
		if (_appearanceMetaId == appearanceId)
		{
			return false;
		}
		bool flag = _firePointCount == count;
		_appearanceMetaId = appearanceId;
		_firePointCount = count;
		_firePointIndex = 0;
		if (count < 2)
		{
			_firePoints = null;
			_firePointIndex = -1;
		}
		else if (!flag)
		{
			_firePoints = new Transform[count];
		}
		_firePoint = null;
		return true;
	}

	public void AppendFirePoint(string firePointPath)
	{
		bool flag = _firePointIndex >= 0;
		if (!string.IsNullOrEmpty(firePointPath))
		{
			Transform transform = AppearenceUtils.FindAttachmentPoint(_transform, firePointPath);
			if (flag)
			{
				_firePoints[_firePointIndex] = transform;
			}
			else
			{
				_firePoint = transform;
			}
		}
		if (flag)
		{
			if (_firePointIndex == 0)
			{
				_firePoint = _firePoints[0];
			}
			_firePointIndex++;
		}
	}

	public bool InitCompPoints(int appearanceId, int count)
	{
		if (_cmpAttachedApperanceId == appearanceId)
		{
			return false;
		}
		if (count == 0)
		{
			return false;
		}
		_cmpAttachedApperanceId = appearanceId;
		if (_cmpAttachedTransforms == null || _cmpAttachedTransforms.Length != count)
		{
			_cmpAttachedTransforms = new Transform[count];
		}
		else
		{
			int num = _cmpAttachedTransforms.Length;
			for (int i = 0; i < num; i++)
			{
				_cmpAttachedTransforms[i] = null;
			}
		}
		return true;
	}

	public bool AddCompPoint(int index, string cmpAttachedPath)
	{
		if (string.IsNullOrEmpty(cmpAttachedPath))
		{
			return false;
		}
		if (index >= _cmpAttachedTransforms.Length)
		{
			return false;
		}
		Transform transform = AppearenceUtils.FindAttachmentPoint(_transform, cmpAttachedPath);
		_cmpAttachedTransforms[index] = transform;
		return true;
	}

	public Transform GetCmpPointByIndex(int index)
	{
		if (index < 0)
		{
			return null;
		}
		if (_cmpAttachedTransforms == null || _cmpAttachedTransforms.Length <= index)
		{
			return null;
		}
		return _cmpAttachedTransforms[index];
	}

	public void InitBuffPoints(int count)
	{
		_buffPoints = new Transform[count];
		_buffPointIndex = 0;
	}

	public void AppendBuffPoint(string buffPointPath)
	{
		if (!string.IsNullOrEmpty(buffPointPath))
		{
			Transform transform = AppearenceUtils.FindAttachmentPoint(_transform, buffPointPath);
			_buffPoints[_buffPointIndex] = transform;
		}
		_buffPointIndex++;
	}

	public void InitUIPoint(string uiPointPath)
	{
		if (!string.IsNullOrEmpty(uiPointPath))
		{
			_uiPoint = AppearenceUtils.FindAttachmentPoint(_transform, uiPointPath);
		}
		else
		{
			_uiPoint = null;
		}
	}

	public Transform InitCannon(string cononPath)
	{
		if (!string.IsNullOrEmpty(cononPath))
		{
			_canonTransform = AppearenceUtils.FindAttachmentPoint(_transform, cononPath);
			if (_canonTransform != null)
			{
				_canonTransform.localEulerAngles = Vector3.zero;
			}
			return _canonTransform;
		}
		return null;
	}

	internal virtual void OnShow()
	{
		_transform.SetParent(_parent);
		_transform.localPosition = _position;
		_transform.localEulerAngles = _rotation;
		_transform.localScale = Vector3.one * _scale;
		if (_simpleAnimationValid)
		{
			_simpleAnimation.transform.localPosition = Vector3.zero;
		}
		if (_citySpaceManTriggeredValid)
		{
			_citySpaceManTrigger.ObjectId = ObjId;
		}
		if (!_colliderValid)
		{
			return;
		}
		if (_layer >= 0)
		{
			_collider.gameObject.layer = _layer;
		}
		if (BattleColliderUtils.IsCollider2D())
		{
			_collider.enabled = false;
			if (_colliderEnabled && ObjId > 0 && Handle != -1)
			{
				BattleColliderUtils.AddCollider2DAgent(Handle, ObjId, _collider);
			}
		}
	}

	public void InPool()
	{
		SetVisible(visible: false);
		_colliderEnabled = false;
		if (_colliderValid)
		{
			_collider.enabled = false;
			if (BattleColliderUtils.IsCollider2D() && Handle != -1)
			{
				BattleColliderUtils.RemoveCollider2DAgent(Handle);
			}
		}
		MPBReset();
		NumberHpTextReset();
		if (_isLoaded)
		{
			_transform.position = new Vector3(-1000f, -1000f, -1000f);
		}
		ObjId = 0;
	}

	public void OutPool()
	{
		SetVisible(visible: true);
		_colliderEnabled = true;
		if (_colliderValid && !BattleColliderUtils.IsCollider2D())
		{
			_collider.enabled = true;
		}
	}

	public void SetVisible(bool visible)
	{
		if (_visible != visible)
		{
			_visible = visible;
			SetVisibleImp(visible);
		}
	}

	private void SetVisibleImp(bool visible)
	{
		if (!_isLoaded)
		{
			return;
		}
		if (visible)
		{
			for (int i = 0; i < _rendererCount; i++)
			{
				_renderers[i].enabled = true;
			}
			if (_simpleAnimationValid)
			{
				_simpleAnimation.enabled = true;
			}
			return;
		}
		for (int j = 0; j < _rendererCount; j++)
		{
			_renderers[j].enabled = false;
		}
		if (_simpleAnimationValid)
		{
			_simpleAnimation.enabled = false;
		}
		if (_posTween != null)
		{
			_posTween.Kill();
			_posTween = null;
		}
		StopHpTweenScale();
	}

	public Collider GetCollider()
	{
		return _collider;
	}

	public void EnableCollider(bool enable)
	{
		_colliderEnabled = enable;
		if (!_colliderValid)
		{
			return;
		}
		if (BattleColliderUtils.IsCollider2D())
		{
			_collider.enabled = false;
			if (ObjId > 0 && Handle != -1)
			{
				if (_colliderEnabled)
				{
					BattleColliderUtils.AddCollider2DAgent(Handle, ObjId, _collider);
				}
				else
				{
					BattleColliderUtils.RemoveCollider2DAgent(Handle);
				}
			}
		}
		else
		{
			_collider.enabled = enable;
		}
	}

	private void InitRender()
	{
		if (_renderers != null || !_isLoaded)
		{
			return;
		}
		_renderers = new List<Renderer>();
		_renderMaterials = new List<Material>();
		SkinnedMeshRenderer[] componentsInChildren = _gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			SkinnedMeshRenderer skinnedMeshRenderer = componentsInChildren[i];
			ReadOnlySpan<char> nameSpan = skinnedMeshRenderer.gameObject.name.AsSpan();
			if (!NeedSkipShadow(nameSpan))
			{
				_renderers.Add(skinnedMeshRenderer);
				_renderMaterials.Add(skinnedMeshRenderer.sharedMaterial);
			}
		}
		MeshRenderer[] componentsInChildren2 = _gameObject.GetComponentsInChildren<MeshRenderer>();
		int j = 0;
		for (int num2 = componentsInChildren2.Length; j < num2; j++)
		{
			MeshRenderer meshRenderer = componentsInChildren2[j];
			if (!meshRenderer.TryGetComponent<TextMeshPro>(out var _))
			{
				ReadOnlySpan<char> nameSpan2 = meshRenderer.gameObject.name.AsSpan();
				if (!NeedSkipShadow(nameSpan2))
				{
					_renderers.Add(meshRenderer);
					_renderMaterials.Add(meshRenderer.sharedMaterial);
				}
			}
		}
		_rendererCount = _renderers.Count;
		_gsRenderers = new List<GPUSkinnedMeshRenderer>(_rendererCount);
		foreach (Renderer renderer in _renderers)
		{
			_gsRenderers.Add(renderer.TryGetComponent<GPUSkinnedMeshRenderer>(out var component2) ? component2 : null);
		}
	}

	public bool NeedSkipShadow(ReadOnlySpan<char> nameSpan)
	{
		if (nameSpan.Contains(ShadowString.AsSpan(), StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		return false;
	}

	public void ReplaceMaterialFlashRed()
	{
		MPBReset();
		InitRender();
		Material redMaterial = UnitViewFacade.GetRedMaterial();
		if (redMaterial == null)
		{
			return;
		}
		_replaceEffect = true;
		foreach (Renderer renderer in _renderers)
		{
			AppearenceUtils.ReplaceMaterial(renderer, replace: true, redMaterial);
		}
	}

	public void ReplaceMaterialFlashWhite()
	{
		MPBReset();
		InitRender();
		Material whiteMaterial = UnitViewFacade.GetWhiteMaterial();
		if (whiteMaterial == null)
		{
			return;
		}
		_replaceEffect = true;
		foreach (Renderer renderer in _renderers)
		{
			AppearenceUtils.ReplaceMaterial(renderer, replace: true, whiteMaterial);
		}
	}

	public void ReplaceMaterialReset()
	{
		if (!_replaceEffect)
		{
			return;
		}
		_replaceEffect = false;
		if (_renderers != null)
		{
			int i = 0;
			for (int count = _renderers.Count; i < count; i++)
			{
				AppearenceUtils.ReplaceMaterial(_renderers[i], replace: false, _renderMaterials[i]);
			}
		}
	}

	public void MPBFlashRed()
	{
		InitRender();
		ReplaceMaterialReset();
		_mpbEffect = AppearenceStyleRim.Red;
		for (int i = 0; i < _rendererCount; i++)
		{
			AppearenceUtils.HitWhiteV3(_gsRenderers[i], _renderers[i], enable: true, AppearenceStyleRim.Red);
		}
	}

	public void MPBResetGray()
	{
		InitRender();
		ReplaceMaterialReset();
		for (int i = 0; i < _rendererCount; i++)
		{
			AppearenceUtils.HitWhiteV3(_gsRenderers[i], _renderers[i], enable: false, AppearenceStyleRim.Gray);
		}
	}

	public void MPBFlashGray()
	{
		InitRender();
		ReplaceMaterialReset();
		_mpbEffect = AppearenceStyleRim.Gray;
		for (int i = 0; i < _rendererCount; i++)
		{
			AppearenceUtils.HitWhiteV3(_gsRenderers[i], _renderers[i], enable: true, AppearenceStyleRim.Gray);
		}
	}

	public void MPBModelScale()
	{
		InitRender();
		ReplaceMaterialReset();
		_mpbEffect = AppearenceStyleRim.ModelScale;
		for (int i = 0; i < _rendererCount; i++)
		{
			AppearenceUtils.HitWhiteV3(_gsRenderers[i], _renderers[i], enable: true, AppearenceStyleRim.ModelScale);
		}
	}

	public void MPBBornEffect()
	{
		InitRender();
		ReplaceMaterialReset();
		_mpbEffect = AppearenceStyleRim.Born;
		for (int i = 0; i < _rendererCount; i++)
		{
			AppearenceUtils.HitWhiteV3(_gsRenderers[i], _renderers[i], enable: true, AppearenceStyleRim.Born);
		}
	}

	public void MPBShieldEffect()
	{
		InitRender();
		ReplaceMaterialReset();
		_mpbEffect = AppearenceStyleRim.Shield;
		for (int i = 0; i < _rendererCount; i++)
		{
			AppearenceUtils.HitWhiteV3(_gsRenderers[i], _renderers[i], enable: true, AppearenceStyleRim.Shield);
		}
	}

	public void MPBReset()
	{
		if (_mpbEffect != null && _renderers != null)
		{
			for (int i = 0; i < _rendererCount; i++)
			{
				AppearenceUtils.HitWhiteV3(_gsRenderers[i], _renderers[i], enable: false, _mpbEffect);
			}
			_mpbEffect = null;
		}
	}

	private void ClearParent()
	{
		if (_isLoaded && _parent != null)
		{
			_transform.SetParent(null);
		}
		_parent = null;
	}

	public void Dispose()
	{
		MPBReset();
		ReplaceMaterialReset();
		NumberHpTextReset();
		if (_simpleAnimationValid)
		{
			_simpleAnimation.cullingMode = _defaultCullingMode;
		}
		_colliderEnabled = true;
		if (_colliderValid)
		{
			_collider.enabled = true;
			if (BattleColliderUtils.IsCollider2D() && Handle != -1)
			{
				BattleColliderUtils.RemoveCollider2DAgent(Handle);
			}
		}
		ClearParent();
		SetVisible(visible: true);
		if (_posTween != null)
		{
			_posTween.Kill();
			_posTween = null;
		}
		_valid = false;
		if (_request != null)
		{
			_request.Destroy();
			_request = null;
		}
		_cmpAttachedApperanceId = 0;
		_gameObject = null;
		_transform = null;
		_firePoints = null;
		_firePoint = null;
		_simpleAnimation = null;
		_canonTransform = null;
		_cmpAttachedTransforms = null;
		_buffPoints = null;
		_uiPoint = null;
		_collider = null;
		_renderers = null;
		_rendererCount = 0;
		_renderMaterials = null;
		_numberHpTextGo = null;
		_numberHpTextMeshProEx = null;
		_numberHpTransformValid = false;
		_numberHpSuperTextMesh = null;
		_numberHpSuperTransformValid = false;
		_citySpaceManTrigger = null;
		_glassRenderer = null;
		_waterRenderer = null;
	}

	public Transform GetUIPoint()
	{
		if (_uiPoint != null)
		{
			return _uiPoint;
		}
		return _transform;
	}

	public Transform GetRotateRoot()
	{
		if (_rotateRoot != null)
		{
			return _rotateRoot;
		}
		return _transform;
	}

	public Transform GetTransform()
	{
		return _transform;
	}

	public float GetLocalScaleX()
	{
		if (_transform != null)
		{
			return _transform.localScale.x;
		}
		return 0f;
	}

	public void SetLocalScaleX(float newScale)
	{
		_scale = newScale;
		if (_transform != null)
		{
			_transform.localScale = new Vector3(newScale, newScale, newScale);
		}
	}

	public Vector3 GetPosition()
	{
		if (_transform != null)
		{
			return _transform.position;
		}
		return _position;
	}

	public void ResetCannon()
	{
		if (_canonTransform != null)
		{
			_canonTransform.localEulerAngles = Vector3.zero;
		}
	}

	public GameObject GetGameObject()
	{
		return _gameObject;
	}

	public void PlaySimpleAnim(string animName, float speed)
	{
		if (!_simpleAnimationValid)
		{
			return;
		}
		string text = UnitViewFacade.CheckAnimName(_simpleAnimation, animName);
		if (!string.IsNullOrEmpty(text))
		{
			_curAnimName = text;
			_simpleAnimation.Play(text);
			if (speed >= 0f)
			{
				_simpleAnimation.SetStateSpeed(text, speed);
			}
		}
	}

	public SimpleAnimation.State GetSimpleAnimState(string animName)
	{
		if (_simpleAnimationValid)
		{
			return _simpleAnimation.GetState(animName);
		}
		return null;
	}

	public void RewindAndPlaySimpleAnim(string animName, float speed)
	{
		if (!_simpleAnimationValid)
		{
			return;
		}
		string text = UnitViewFacade.CheckAnimName(_simpleAnimation, animName);
		if (!string.IsNullOrEmpty(text))
		{
			_curAnimName = text;
			_simpleAnimation.Rewind(text);
			_simpleAnimation.Play(text);
			if (speed >= 0f)
			{
				_simpleAnimation.SetStateSpeed(text, speed);
			}
		}
	}

	public void CrossFadeSimpleAnim(string animName, float speed, float fadeTime)
	{
		if (_simpleAnimationValid)
		{
			_curAnimName = animName;
			_simpleAnimation.CrossFade(animName, fadeTime);
			if (speed >= 0f)
			{
				_simpleAnimation.SetStateSpeed(animName, speed);
			}
		}
	}

	public void RewindSimpleAnim(string animName)
	{
		if (_simpleAnimationValid)
		{
			string text = UnitViewFacade.CheckAnimName(_simpleAnimation, animName);
			if (!string.IsNullOrEmpty(text))
			{
				_simpleAnimation.Rewind(text);
			}
		}
	}

	public string GetCurAniName()
	{
		return _curAnimName;
	}

	public float GetAnimLength(string animName)
	{
		if (_simpleAnimationValid)
		{
			_simpleAnimation.SetStateSpeed(animName, 1f);
			return _simpleAnimation.GetClipLength(animName);
		}
		return 0f;
	}

	public Transform GetFirePointById(int id)
	{
		if (id == 1)
		{
			return _firePoint;
		}
		if (id <= _firePointCount)
		{
			return _firePoints[id - 1];
		}
		return null;
	}

	public Vector3 GetTransformPoint(Vector3 point)
	{
		if (_isLoaded)
		{
			return _transform.TransformPoint(point);
		}
		return Vector3.zero;
	}

	public void SetLocalPosition(Vector3 pos)
	{
		_position = pos;
		if (_isLoaded)
		{
			if (_posTween != null)
			{
				_posTween.Kill();
				_posTween = null;
			}
			_transform.localPosition = pos;
		}
	}

	public void SetPosition(Vector3 pos)
	{
		if (_isLoaded)
		{
			if (_posTween != null)
			{
				_posTween.Kill();
				_posTween = null;
			}
			_transform.position = pos;
		}
	}

	public void MoveToLocalPos(Vector3 pos, float time)
	{
		_position = pos;
		if (_isLoaded)
		{
			if (_posTween != null)
			{
				_posTween.Kill();
				_posTween = null;
			}
			_posTween = _transform.DOLocalMove(pos, time);
		}
	}

	public Transform InitTxtNumberText(int number)
	{
		if (!_isLoaded)
		{
			return null;
		}
		if (_numberHpTransformValid)
		{
			SetNumberText(number);
			return _numberHpTextGo.transform;
		}
		Transform transform = _transform.Find("txt");
		if (transform != null)
		{
			_numberHpTextGo = transform.gameObject;
			_numberHpTextMeshProEx = transform.GetComponent<TextMeshProEx>();
			_numberHpTransformValid = _numberHpTextMeshProEx != null;
			if (!_numberHpTransformValid)
			{
				_numberHpSuperTextMesh = transform.GetComponent<SuperTextMesh>();
				_numberHpSuperTransformValid = _numberHpSuperTextMesh != null;
			}
			SetNumberText(number);
		}
		return transform;
	}

	internal void SetNumberText(int number)
	{
		if (_numberHpTransformValid)
		{
			if (number < 0)
			{
				_numberHpTextMeshProEx.SetRealText(number.ToString());
			}
			else
			{
				_numberHpTextMeshProEx.SetRealText("+" + number);
			}
		}
		else if (_numberHpSuperTransformValid)
		{
			if (number < 0)
			{
				_numberHpSuperTextMesh.text = number.ToString();
			}
			else
			{
				_numberHpSuperTextMesh.text = "+" + number;
			}
		}
	}

	public Transform InitHpText(int hp)
	{
		if (!_isLoaded)
		{
			return null;
		}
		if (_numberHpTransformValid)
		{
			SetNumberHpText(hp);
			return _numberHpTextGo.transform;
		}
		Transform transform = _transform.Find("HpText");
		if (transform != null)
		{
			_numberHpTextGo = transform.gameObject;
			_numberHpTextMeshProEx = transform.GetComponent<TextMeshProEx>();
			_numberHpTransformValid = _numberHpTextMeshProEx != null;
			if (!_numberHpTransformValid)
			{
				_numberHpSuperTextMesh = transform.GetComponent<SuperTextMesh>();
				_numberHpSuperTransformValid = _numberHpSuperTextMesh != null;
			}
			SetNumberHpText(hp);
			transform.localScale = Vector3.one;
		}
		return transform;
	}

	public void ShowHpTweenScale()
	{
		if (_numberHpTransformValid)
		{
			if (_hpScaleTween != null)
			{
				_hpScaleTween.Restart();
			}
			else
			{
				_hpScaleTween = _numberHpTextGo.transform.DOScale(Vector3.one * 1.22f, 0.1f).SetEase(Ease.OutCubic).SetLoops(2, LoopType.Yoyo)
					.SetAutoKill(autoKillOnCompletion: false);
			}
		}
	}

	internal void StopHpTweenScale()
	{
		if (_hpScaleTween != null)
		{
			_hpScaleTween.Rewind();
		}
	}

	internal void ClearHpTweenScale()
	{
		if (_hpScaleTween != null)
		{
			_hpScaleTween.Kill(complete: true);
			_hpScaleTween = null;
		}
	}

	internal void SetNumberHpText(int number)
	{
		if (_numberHpTransformValid)
		{
			_numberHpTextMeshProEx.SetRealText(number.ToString());
		}
		else if (_numberHpSuperTransformValid)
		{
			_numberHpSuperTextMesh.text = number.ToString();
		}
	}

	public void NumberHpTextActive(bool active)
	{
		if (_numberHpTransformValid || _numberHpSuperTransformValid)
		{
			_numberHpTextGo.SetActive(active);
		}
	}

	private void NumberHpTextReset()
	{
		if (_numberHpTransformValid || _numberHpSuperTransformValid)
		{
			_numberHpTextGo.SetActive(value: true);
		}
	}

	public SimpleAnimation GetSimpleAnimation()
	{
		return _simpleAnimation;
	}

	public void InitGlassAndWaterRenderObj(string glassRenderName, string waterRenderName)
	{
		if (!(_gameObject == null))
		{
			if (_glassRenderer == null)
			{
				_glassRenderer = _gameObject.transform.Find(glassRenderName)?.GetComponent<MeshRenderer>();
			}
			if (_waterRenderer == null)
			{
				_waterRenderer = _gameObject.transform.Find(waterRenderName)?.GetComponent<MeshRenderer>();
			}
		}
	}

	public void MPBGlassCrackEffect(float value)
	{
		if (!(_gameObject == null) && _glassRenderer != null)
		{
			AppearenceStyleRim.GlassCrack.UpdateMaterialProperty(value);
			MaterialPropertyBlock materialPropertyBlock = AppearenceStyleRim.GlassCrack.GetMaterialPropertyBlock();
			AppearenceStyleRim.GlassCrack.ApplyToMaterialPropertyBlock(materialPropertyBlock, useInstancingName: false);
			_glassRenderer.SetPropertyBlock(materialPropertyBlock);
		}
	}

	public void MPBWaveIntensityEffect(float value)
	{
		if (!(_gameObject == null) && _waterRenderer != null)
		{
			AppearenceStyleRim.WaveIntensity.UpdateMaterialProperty(value);
			MaterialPropertyBlock materialPropertyBlock = AppearenceStyleRim.WaveIntensity.GetMaterialPropertyBlock();
			AppearenceStyleRim.WaveIntensity.ApplyToMaterialPropertyBlock(materialPropertyBlock, useInstancingName: false);
			_waterRenderer.SetPropertyBlock(materialPropertyBlock);
		}
	}
}
