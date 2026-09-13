using System.Numerics;
using MapLineSegmentToIsometricArc;
using TMPro;
using UnityEngine;

namespace Scripts.Utilities;

[ExecuteInEditMode]
public class TextProOnACircleCurve : MonoBehaviour
{
	[SerializeField]
	private float radius = 1000f;

	private TextMeshProUGUIEx m_TextComponent;

	private string _lastText;

	private void Awake()
	{
		m_TextComponent = base.gameObject.GetComponent<TextMeshProUGUIEx>();
	}

	private void OnEnable()
	{
		TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
	}

	private void OnDisable()
	{
		TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
		m_TextComponent?.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
		_lastText = null;
	}

	private void OnTextChanged(Object obj)
	{
		if (obj == m_TextComponent)
		{
			UpdateCircle();
		}
	}

	protected void UpdateCircle()
	{
		string text = m_TextComponent.text;
		if (text == _lastText)
		{
			return;
		}
		_lastText = text;
		m_TextComponent.ForceMeshUpdate();
		TMP_TextInfo textInfo = m_TextComponent.textInfo;
		int characterCount = textInfo.characterCount;
		if (characterCount == 0)
		{
			return;
		}
		float x = m_TextComponent.bounds.min.x;
		float x2 = m_TextComponent.bounds.max.x;
		System.Numerics.Vector2 vector = new System.Numerics.Vector2(0f, 0f - radius);
		Line2CirArcTransformator line2CirArcTransformator = new Line2CirArcTransformator(new System.Numerics.Vector2(x, 0f), new System.Numerics.Vector2(x2, 0f), vector, new System.Numerics.Vector2(0f, 0f));
		for (int i = 0; i < characterCount; i++)
		{
			if (textInfo.characterInfo[i].isVisible)
			{
				int vertexIndex = textInfo.characterInfo[i].vertexIndex;
				int materialReferenceIndex = textInfo.characterInfo[i].materialReferenceIndex;
				UnityEngine.Vector3[] vertices = textInfo.meshInfo[materialReferenceIndex].vertices;
				UnityEngine.Vector3 vector2 = new UnityEngine.Vector2((vertices[vertexIndex].x + vertices[vertexIndex + 2].x) / 2f, textInfo.characterInfo[i].baseLine);
				vertices[vertexIndex] += -vector2;
				vertices[vertexIndex + 1] += -vector2;
				vertices[vertexIndex + 2] += -vector2;
				vertices[vertexIndex + 3] += -vector2;
				System.Numerics.Vector2 vector3 = line2CirArcTransformator.MapLinePoint(new System.Numerics.Vector2(vector2.x, vector2.y));
				System.Numerics.Vector2 vector4 = vector3 - vector;
				float num = Mathf.Atan2(vector4.Y, vector4.X);
				UnityEngine.Matrix4x4 matrix4x = UnityEngine.Matrix4x4.TRS(new UnityEngine.Vector3(vector3.X, vector3.Y, 0f), UnityEngine.Quaternion.AngleAxis(num * 57.29578f - 90f, UnityEngine.Vector3.forward), UnityEngine.Vector3.one);
				vertices[vertexIndex] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex]);
				vertices[vertexIndex + 1] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 1]);
				vertices[vertexIndex + 2] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 2]);
				vertices[vertexIndex + 3] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 3]);
			}
		}
		m_TextComponent.UpdateVertexData();
	}
}
