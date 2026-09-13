using System;
using UnityEngine.Scripting;

namespace UnityEngine.Tilemaps;

[Serializable]
[RequiredByNativeCode]
public class Tile : TileBase
{
	public enum ColliderType
	{
		None,
		Sprite,
		Grid
	}

	[SerializeField]
	private Sprite m_Sprite;

	[SerializeField]
	private Color m_Color = Color.white;

	[SerializeField]
	private Matrix4x4 m_Transform = Matrix4x4.identity;

	[SerializeField]
	private GameObject m_InstancedGameObject;

	[SerializeField]
	private TileFlags m_Flags = TileFlags.LockColor;

	[SerializeField]
	private ColliderType m_ColliderType = ColliderType.Sprite;

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
			return m_InstancedGameObject;
		}
		set
		{
			m_InstancedGameObject = value;
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

	public ColliderType colliderType
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

	public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
	{
		tileData.sprite = m_Sprite;
		tileData.color = m_Color;
		tileData.transform = m_Transform;
		tileData.gameObject = m_InstancedGameObject;
		tileData.flags = m_Flags;
		tileData.colliderType = m_ColliderType;
	}
}
