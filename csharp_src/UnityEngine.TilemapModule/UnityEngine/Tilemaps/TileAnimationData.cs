using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Tilemaps;

[RequiredByNativeCode]
[NativeType(Header = "Modules/Tilemap/TilemapScripting.h")]
public struct TileAnimationData
{
	private Sprite[] m_AnimatedSprites;

	private float m_AnimationSpeed;

	private float m_AnimationStartTime;

	public Sprite[] animatedSprites
	{
		get
		{
			return m_AnimatedSprites;
		}
		set
		{
			m_AnimatedSprites = value;
		}
	}

	public float animationSpeed
	{
		get
		{
			return m_AnimationSpeed;
		}
		set
		{
			m_AnimationSpeed = value;
		}
	}

	public float animationStartTime
	{
		get
		{
			return m_AnimationStartTime;
		}
		set
		{
			m_AnimationStartTime = value;
		}
	}
}
