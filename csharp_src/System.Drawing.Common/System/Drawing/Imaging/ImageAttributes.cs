using System.Drawing.Drawing2D;

namespace System.Drawing.Imaging;

public sealed class ImageAttributes : ICloneable, IDisposable
{
	public ImageAttributes()
	{
		throw new PlatformNotSupportedException();
	}

	public void ClearBrushRemapTable()
	{
		throw new PlatformNotSupportedException();
	}

	public void ClearColorKey()
	{
		throw new PlatformNotSupportedException();
	}

	public void ClearColorKey(ColorAdjustType type)
	{
		throw new PlatformNotSupportedException();
	}

	public void ClearColorMatrix()
	{
		throw new PlatformNotSupportedException();
	}

	public void ClearColorMatrix(ColorAdjustType type)
	{
		throw new PlatformNotSupportedException();
	}

	public void ClearGamma()
	{
		throw new PlatformNotSupportedException();
	}

	public void ClearGamma(ColorAdjustType type)
	{
		throw new PlatformNotSupportedException();
	}

	public void ClearNoOp()
	{
		throw new PlatformNotSupportedException();
	}

	public void ClearNoOp(ColorAdjustType type)
	{
		throw new PlatformNotSupportedException();
	}

	public void ClearOutputChannel()
	{
		throw new PlatformNotSupportedException();
	}

	public void ClearOutputChannel(ColorAdjustType type)
	{
		throw new PlatformNotSupportedException();
	}

	public void ClearOutputChannelColorProfile()
	{
		throw new PlatformNotSupportedException();
	}

	public void ClearOutputChannelColorProfile(ColorAdjustType type)
	{
		throw new PlatformNotSupportedException();
	}

	public void ClearRemapTable()
	{
		throw new PlatformNotSupportedException();
	}

	public void ClearRemapTable(ColorAdjustType type)
	{
		throw new PlatformNotSupportedException();
	}

	public void ClearThreshold()
	{
		throw new PlatformNotSupportedException();
	}

	public void ClearThreshold(ColorAdjustType type)
	{
		throw new PlatformNotSupportedException();
	}

	public object Clone()
	{
		throw new PlatformNotSupportedException();
	}

	public void Dispose()
	{
		throw new PlatformNotSupportedException();
	}

	~ImageAttributes()
	{
		throw new PlatformNotSupportedException();
	}

	public void GetAdjustedPalette(ColorPalette palette, ColorAdjustType type)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetBrushRemapTable(ColorMap[] map)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetColorKey(Color colorLow, Color colorHigh)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetColorKey(Color colorLow, Color colorHigh, ColorAdjustType type)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetColorMatrices(ColorMatrix newColorMatrix, ColorMatrix grayMatrix)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetColorMatrices(ColorMatrix newColorMatrix, ColorMatrix grayMatrix, ColorMatrixFlag flags)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetColorMatrices(ColorMatrix newColorMatrix, ColorMatrix grayMatrix, ColorMatrixFlag mode, ColorAdjustType type)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetColorMatrix(ColorMatrix newColorMatrix)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetColorMatrix(ColorMatrix newColorMatrix, ColorMatrixFlag flags)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetColorMatrix(ColorMatrix newColorMatrix, ColorMatrixFlag mode, ColorAdjustType type)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetGamma(float gamma)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetGamma(float gamma, ColorAdjustType type)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetNoOp()
	{
		throw new PlatformNotSupportedException();
	}

	public void SetNoOp(ColorAdjustType type)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetOutputChannel(ColorChannelFlag flags)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetOutputChannel(ColorChannelFlag flags, ColorAdjustType type)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetOutputChannelColorProfile(string colorProfileFilename)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetOutputChannelColorProfile(string colorProfileFilename, ColorAdjustType type)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetRemapTable(ColorMap[] map)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetRemapTable(ColorMap[] map, ColorAdjustType type)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetThreshold(float threshold)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetThreshold(float threshold, ColorAdjustType type)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetWrapMode(WrapMode mode)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetWrapMode(WrapMode mode, Color color)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetWrapMode(WrapMode mode, Color color, bool clamp)
	{
		throw new PlatformNotSupportedException();
	}
}
