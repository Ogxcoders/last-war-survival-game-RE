using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering;

public static class TextureXR
{
	private static int m_MaxViews = 1;

	private static Texture m_BlackUIntTexture2DArray;

	private static Texture m_BlackUIntTexture;

	private static RTHandle m_BlackUIntTexture2DArrayRTH;

	private static RTHandle m_BlackUIntTextureRTH;

	private static Texture2DArray m_ClearTexture2DArray;

	private static Texture2D m_ClearTexture;

	private static RTHandle m_ClearTexture2DArrayRTH;

	private static RTHandle m_ClearTextureRTH;

	private static Texture2DArray m_MagentaTexture2DArray;

	private static Texture2D m_MagentaTexture;

	private static RTHandle m_MagentaTexture2DArrayRTH;

	private static RTHandle m_MagentaTextureRTH;

	private static Texture2DArray m_BlackTexture2DArray;

	private static RTHandle m_BlackTexture2DArrayRTH;

	private static RTHandle m_BlackTextureRTH;

	private static Texture2DArray m_WhiteTexture2DArray;

	private static RTHandle m_WhiteTexture2DArrayRTH;

	private static RTHandle m_WhiteTextureRTH;

	public static int maxViews
	{
		set
		{
			m_MaxViews = value;
		}
	}

	public static int slices => m_MaxViews;

	public static bool useTexArray
	{
		get
		{
			switch (SystemInfo.graphicsDeviceType)
			{
			case GraphicsDeviceType.Direct3D11:
			case GraphicsDeviceType.PlayStation4:
			case GraphicsDeviceType.Direct3D12:
			case GraphicsDeviceType.Vulkan:
				return true;
			default:
				return false;
			}
		}
	}

	public static TextureDimension dimension
	{
		get
		{
			if (!useTexArray)
			{
				return TextureDimension.Tex2D;
			}
			return TextureDimension.Tex2DArray;
		}
	}

	public static RTHandle GetBlackUIntTexture()
	{
		if (!useTexArray)
		{
			return m_BlackUIntTextureRTH;
		}
		return m_BlackUIntTexture2DArrayRTH;
	}

	public static RTHandle GetClearTexture()
	{
		if (!useTexArray)
		{
			return m_ClearTextureRTH;
		}
		return m_ClearTexture2DArrayRTH;
	}

	public static RTHandle GetMagentaTexture()
	{
		if (!useTexArray)
		{
			return m_MagentaTextureRTH;
		}
		return m_MagentaTexture2DArrayRTH;
	}

	public static RTHandle GetBlackTexture()
	{
		if (!useTexArray)
		{
			return m_BlackTextureRTH;
		}
		return m_BlackTexture2DArrayRTH;
	}

	public static RTHandle GetBlackTextureArray()
	{
		return m_BlackTexture2DArrayRTH;
	}

	public static RTHandle GetWhiteTexture()
	{
		if (!useTexArray)
		{
			return m_WhiteTextureRTH;
		}
		return m_WhiteTexture2DArrayRTH;
	}

	public static void Initialize(CommandBuffer cmd, ComputeShader clearR32_UIntShader)
	{
		if (m_BlackUIntTexture2DArray == null)
		{
			RTHandles.Release(m_BlackUIntTexture2DArrayRTH);
			m_BlackUIntTexture2DArray = CreateBlackUIntTextureArray(cmd, clearR32_UIntShader);
			m_BlackUIntTexture2DArrayRTH = RTHandles.Alloc(m_BlackUIntTexture2DArray);
			RTHandles.Release(m_BlackUIntTextureRTH);
			m_BlackUIntTexture = CreateBlackUintTexture(cmd, clearR32_UIntShader);
			m_BlackUIntTextureRTH = RTHandles.Alloc(m_BlackUIntTexture);
			RTHandles.Release(m_ClearTextureRTH);
			m_ClearTexture = new Texture2D(1, 1, TextureFormat.ARGB32, mipChain: false)
			{
				name = "Clear Texture"
			};
			m_ClearTexture.SetPixel(0, 0, Color.clear);
			m_ClearTexture.Apply();
			m_ClearTextureRTH = RTHandles.Alloc(m_ClearTexture);
			RTHandles.Release(m_ClearTexture2DArrayRTH);
			m_ClearTexture2DArray = CreateTexture2DArrayFromTexture2D(m_ClearTexture, "Clear Texture2DArray");
			m_ClearTexture2DArrayRTH = RTHandles.Alloc(m_ClearTexture2DArray);
			RTHandles.Release(m_MagentaTextureRTH);
			m_MagentaTexture = new Texture2D(1, 1, TextureFormat.ARGB32, mipChain: false)
			{
				name = "Magenta Texture"
			};
			m_MagentaTexture.SetPixel(0, 0, Color.magenta);
			m_MagentaTexture.Apply();
			m_MagentaTextureRTH = RTHandles.Alloc(m_MagentaTexture);
			RTHandles.Release(m_MagentaTexture2DArrayRTH);
			m_MagentaTexture2DArray = CreateTexture2DArrayFromTexture2D(m_MagentaTexture, "Magenta Texture2DArray");
			m_MagentaTexture2DArrayRTH = RTHandles.Alloc(m_MagentaTexture2DArray);
			RTHandles.Release(m_BlackTextureRTH);
			m_BlackTextureRTH = RTHandles.Alloc(Texture2D.blackTexture);
			RTHandles.Release(m_BlackTexture2DArrayRTH);
			m_BlackTexture2DArray = CreateTexture2DArrayFromTexture2D(Texture2D.blackTexture, "Black Texture2DArray");
			m_BlackTexture2DArrayRTH = RTHandles.Alloc(m_BlackTexture2DArray);
			RTHandles.Release(m_WhiteTextureRTH);
			m_WhiteTextureRTH = RTHandles.Alloc(Texture2D.whiteTexture);
			RTHandles.Release(m_WhiteTexture2DArrayRTH);
			m_WhiteTexture2DArray = CreateTexture2DArrayFromTexture2D(Texture2D.whiteTexture, "White Texture2DArray");
			m_WhiteTexture2DArrayRTH = RTHandles.Alloc(m_WhiteTexture2DArray);
		}
	}

	private static Texture2DArray CreateTexture2DArrayFromTexture2D(Texture2D source, string name)
	{
		Texture2DArray texture2DArray = new Texture2DArray(source.width, source.height, slices, source.format, mipChain: false)
		{
			name = name
		};
		for (int i = 0; i < slices; i++)
		{
			Graphics.CopyTexture(source, 0, 0, texture2DArray, i, 0);
		}
		return texture2DArray;
	}

	private static Texture CreateBlackUIntTextureArray(CommandBuffer cmd, ComputeShader clearR32_UIntShader)
	{
		RenderTexture renderTexture = new RenderTexture(1, 1, 0, GraphicsFormat.R32_UInt)
		{
			dimension = TextureDimension.Tex2DArray,
			volumeDepth = slices,
			useMipMap = false,
			autoGenerateMips = false,
			enableRandomWrite = true,
			name = "Black UInt Texture Array"
		};
		renderTexture.Create();
		int kernelIndex = clearR32_UIntShader.FindKernel("ClearUIntTextureArray");
		cmd.SetComputeTextureParam(clearR32_UIntShader, kernelIndex, "_TargetArray", renderTexture);
		cmd.DispatchCompute(clearR32_UIntShader, kernelIndex, 1, 1, slices);
		return renderTexture;
	}

	private static Texture CreateBlackUintTexture(CommandBuffer cmd, ComputeShader clearR32_UIntShader)
	{
		RenderTexture renderTexture = new RenderTexture(1, 1, 0, GraphicsFormat.R32_UInt)
		{
			dimension = TextureDimension.Tex2D,
			volumeDepth = slices,
			useMipMap = false,
			autoGenerateMips = false,
			enableRandomWrite = true,
			name = "Black UInt Texture Array"
		};
		renderTexture.Create();
		int kernelIndex = clearR32_UIntShader.FindKernel("ClearUIntTexture");
		cmd.SetComputeTextureParam(clearR32_UIntShader, kernelIndex, "_Target", renderTexture);
		cmd.DispatchCompute(clearR32_UIntShader, kernelIndex, 1, 1, slices);
		return renderTexture;
	}
}
