using System.Collections.Generic;
using UnityEngine;

public class WorldGoTransAdj : MonoBehaviour
{
	[SerializeField]
	private List<WorldGoTransAdjCurve> _curveList;

	[SerializeField]
	private List<WorldGoTransAdjParam> _worldTransList;

	[SerializeField]
	private float _maxCameraDistance;

	private float _cacheDistance;

	private Dictionary<string, AnimationCurve> _curveDic = new Dictionary<string, AnimationCurve>();

	private void Start()
	{
		foreach (WorldGoTransAdjCurve curve in _curveList)
		{
			_curveDic[curve.CurveName] = curve.Curve;
		}
		if (_maxCameraDistance <= 0f)
		{
			_maxCameraDistance = 2.1474836E+09f;
		}
	}

	private void Update()
	{
		if (SceneManager.World == null)
		{
			return;
		}
		float lodDistance = SceneManager.World.GetLodDistance();
		if (Mathf.Approximately(_cacheDistance, lodDistance))
		{
			return;
		}
		_cacheDistance = lodDistance;
		foreach (WorldGoTransAdjParam worldTrans in _worldTransList)
		{
			if (worldTrans.AdjType != WorldGoTransAdjParam.AdjustType.None && _curveDic.ContainsKey(worldTrans.RefCurveName))
			{
				AnimationCurve animationCurve = _curveDic[worldTrans.RefCurveName];
				if (worldTrans.AdjType == WorldGoTransAdjParam.AdjustType.LocalScale)
				{
					Vector3 localScale = animationCurve.Evaluate(lodDistance / _maxCameraDistance) * worldTrans.V3Param;
					worldTrans.TransNode.localScale = localScale;
				}
				else if (worldTrans.AdjType == WorldGoTransAdjParam.AdjustType.LocalPos)
				{
					Vector3 localPosition = animationCurve.Evaluate(lodDistance / _maxCameraDistance) * worldTrans.V3Param;
					worldTrans.TransNode.localPosition = localPosition;
				}
				else if (worldTrans.AdjType == WorldGoTransAdjParam.AdjustType.LocalPosZ)
				{
					float z = animationCurve.Evaluate(lodDistance / _maxCameraDistance) * worldTrans.ParamNumF;
					worldTrans.TransNode.localPosition = new Vector3(worldTrans.TransNode.localPosition.x, worldTrans.TransNode.localPosition.y, z);
				}
			}
		}
	}
}
