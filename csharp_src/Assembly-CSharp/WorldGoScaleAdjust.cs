using System.Collections.Generic;
using UnityEngine;

public class WorldGoScaleAdjust : MonoBehaviour
{
	[SerializeField]
	private List<Transform> _transGoScaleList;

	[SerializeField]
	private List<Transform> _transGoPosList;

	[SerializeField]
	private AnimationCurve _scaleCurve;

	[SerializeField]
	private AnimationCurve _posCurve;

	[Header("Data")]
	[SerializeField]
	private int _maxDistance;

	[SerializeField]
	private Vector3 _maxScale;

	[SerializeField]
	private float _maxLocalPosY;

	private float _cacheDistance;

	private void Update()
	{
		float lodDistance = SceneManager.World.GetLodDistance();
		if (Mathf.Approximately(_cacheDistance, lodDistance))
		{
			return;
		}
		_cacheDistance = lodDistance;
		if (_scaleCurve != null && _transGoScaleList != null && _transGoScaleList.Count > 0)
		{
			Vector3 localScale = _scaleCurve.Evaluate(lodDistance / (float)_maxDistance) * _maxScale;
			foreach (Transform transGoScale in _transGoScaleList)
			{
				transGoScale.transform.localScale = localScale;
			}
		}
		if (_posCurve == null || _transGoPosList == null || _transGoPosList.Count <= 0)
		{
			return;
		}
		float y = _posCurve.Evaluate(lodDistance / (float)_maxDistance) * _maxLocalPosY;
		foreach (Transform transGoPos in _transGoPosList)
		{
			transGoPos.transform.localPosition = new Vector3(transGoPos.transform.localPosition.x, y, transGoPos.transform.localPosition.z);
		}
	}
}
