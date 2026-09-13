using System;
using System.Text;
using UnityEngine;

public class BulletMotionEditor : MonoBehaviour
{
	[SerializeField]
	private int skillId = 200100;

	[SerializeField]
	private float attackRange;

	[SerializeField]
	private float cooldown = 2f;

	[SerializeField]
	private GameObject bulletEffect;

	[SerializeField]
	private float heightWidthRatio = 0.5f;

	[SerializeField]
	private float height = 1f;

	[SerializeField]
	private float flySpeed = 20f;

	[SerializeField]
	private AnimationCurve motionCurve;

	[SerializeField]
	private string curveString = "";

	[SerializeField]
	private bool curveToString;

	[SerializeField]
	private bool stringToCurve;

	[SerializeField]
	public RectInt Rectangle = new RectInt(31, 15, 10, 10);

	[SerializeField]
	private bool isUltimate;

	public int SkillId
	{
		get
		{
			return skillId;
		}
		set
		{
			skillId = value;
		}
	}

	public float AttackRange
	{
		get
		{
			return attackRange;
		}
		set
		{
			attackRange = value;
		}
	}

	public float Cooldown
	{
		get
		{
			return cooldown;
		}
		set
		{
			cooldown = value;
		}
	}

	public GameObject BulletEffect
	{
		get
		{
			return bulletEffect;
		}
		set
		{
			bulletEffect = value;
		}
	}

	public float HeightWidthRatio
	{
		get
		{
			return heightWidthRatio;
		}
		set
		{
			heightWidthRatio = value;
		}
	}

	public float Height
	{
		get
		{
			return height;
		}
		set
		{
			height = value;
		}
	}

	public float FlySpeed
	{
		get
		{
			return flySpeed;
		}
		set
		{
			flySpeed = value;
		}
	}

	public AnimationCurve MotionCurve
	{
		get
		{
			return motionCurve;
		}
		set
		{
			motionCurve = value;
		}
	}

	public string CurveString
	{
		get
		{
			return curveString;
		}
		set
		{
			curveString = value;
		}
	}

	public bool IsUltimate
	{
		get
		{
			return isUltimate;
		}
		set
		{
			isUltimate = value;
		}
	}

	private void Update()
	{
		if (curveToString)
		{
			curveToString = false;
			curveString = CurveToString(motionCurve);
		}
		if (stringToCurve)
		{
			stringToCurve = false;
			motionCurve = StringToCurve(curveString);
		}
	}

	public static string CurveToString(AnimationCurve curve)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < curve.length; i++)
		{
			if (i > 0)
			{
				stringBuilder.Append("|");
			}
			stringBuilder.Append($"{curve[i].time:F3},{curve[i].value:F3},{curve[i].inTangent:F3},{curve[i].outTangent:F3}");
		}
		return stringBuilder.ToString();
	}

	public static AnimationCurve StringToCurve(string str)
	{
		try
		{
			string[] array = str.Split(new char[1] { '|' });
			AnimationCurve animationCurve = new AnimationCurve();
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(new char[1] { ',' });
				float[] array3 = new float[4];
				for (int j = 0; j < 4; j++)
				{
					array3[j] = float.Parse(array2[j]);
				}
				Keyframe keyframe = new Keyframe(array3[0], array3[1], array3[2], array3[3]);
				keyframe.weightedMode = WeightedMode.None;
				Keyframe key = keyframe;
				animationCurve.AddKey(key);
			}
			return animationCurve;
		}
		catch (Exception value)
		{
			Console.WriteLine(value);
			throw;
		}
	}
}
