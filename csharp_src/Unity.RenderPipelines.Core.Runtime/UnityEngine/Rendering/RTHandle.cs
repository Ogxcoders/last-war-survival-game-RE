namespace UnityEngine.Rendering;

public class RTHandle
{
	internal RTHandleSystem m_Owner;

	internal RenderTexture m_RT;

	internal Texture m_ExternalTexture;

	internal RenderTargetIdentifier m_NameID;

	internal bool m_EnableMSAA;

	internal bool m_EnableRandomWrite;

	internal bool m_EnableHWDynamicScale;

	internal string m_Name;

	internal ScaleFunc scaleFunc;

	public Vector2 scaleFactor { get; internal set; }

	public bool useScaling { get; internal set; }

	public Vector2Int referenceSize { get; internal set; }

	public RTHandleProperties rtHandleProperties => m_Owner.rtHandleProperties;

	public RenderTexture rt => m_RT;

	public RenderTargetIdentifier nameID => m_NameID;

	public string name => m_Name;

	internal RTHandle(RTHandleSystem owner)
	{
		m_Owner = owner;
	}

	public static implicit operator RenderTexture(RTHandle handle)
	{
		return handle.rt;
	}

	public static implicit operator Texture(RTHandle handle)
	{
		if (!(handle.rt != null))
		{
			return handle.m_ExternalTexture;
		}
		return handle.rt;
	}

	public static implicit operator RenderTargetIdentifier(RTHandle handle)
	{
		return handle.nameID;
	}

	internal void SetRenderTexture(RenderTexture rt)
	{
		m_RT = rt;
		m_ExternalTexture = null;
		m_NameID = new RenderTargetIdentifier(rt);
	}

	internal void SetTexture(Texture tex)
	{
		m_RT = null;
		m_ExternalTexture = tex;
		m_NameID = new RenderTargetIdentifier(tex);
	}

	public void Release()
	{
		m_Owner.Remove(this);
		CoreUtils.Destroy(m_RT);
		m_NameID = BuiltinRenderTextureType.None;
		m_RT = null;
		m_ExternalTexture = null;
	}

	public Vector2Int GetScaledSize(Vector2Int refSize)
	{
		if (scaleFunc != null)
		{
			return scaleFunc(refSize);
		}
		return new Vector2Int(Mathf.RoundToInt(scaleFactor.x * (float)refSize.x), Mathf.RoundToInt(scaleFactor.y * (float)refSize.y));
	}
}
