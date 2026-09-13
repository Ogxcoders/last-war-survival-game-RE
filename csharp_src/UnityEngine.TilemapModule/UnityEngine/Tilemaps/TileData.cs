using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Tilemaps;

[RequiredByNativeCode]
[NativeType(Header = "Modules/Tilemap/TilemapScripting.h")]
public struct TileData
{
	private Sprite m_Sprite;

	private Color m_Color;

	private Matrix4x4 m_Transform;

	private GameObject m_GameObject;

	private TileFlags m_Flags;

	private Tile.ColliderType m_ColliderType;

	public Sprite sprite
	{
		get
		{
			return m_Sprite;
		}
		set
		{
			m_Sprite = value;
		}
	}

	public Color color
	{
		get
		{
			return m_Color;
		}
		set
		{
			m_Color = value;
		}
	}

	public Matrix4x4 transform
	{
		get
		{
			return m_Transform;
		}
		set
		{
			m_Transform = value;
		}
	}

	public GameObject gameObject
	{
		get
		{
			return m_GameObject;
		}
		set
		{
			m_GameObject = value;
		}
	}

	public TileFlags flags
	{
		get
		{
			return m_Flags;
		}
		set
		{
			m_Flags = value;
		}
	}

	public Tile.ColliderType colliderType
	{
		get
		{
			return m_ColliderType;
		}
		set
		{
			m_ColliderType = value;
		}
	}
}
