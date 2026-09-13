using System.Runtime.CompilerServices;

namespace UnityMeshSimplifier.Internal;

internal class UVChannels<TVec>
{
	private static readonly int UVChannelCount = MeshUtils.UVChannelCount;

	private ResizableArray<TVec>[] channels;

	private TVec[][] channelsData;

	public TVec[][] Data
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			for (int i = 0; i < UVChannelCount; i++)
			{
				if (channels[i] != null)
				{
					channelsData[i] = channels[i].Data;
				}
				else
				{
					channelsData[i] = null;
				}
			}
			return channelsData;
		}
	}

	public ResizableArray<TVec> this[int index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return channels[index];
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			channels[index] = value;
		}
	}

	public UVChannels()
	{
		channels = new ResizableArray<TVec>[UVChannelCount];
		channelsData = new TVec[UVChannelCount][];
	}

	public void Resize(int capacity, bool trimExess = false)
	{
		for (int i = 0; i < UVChannelCount; i++)
		{
			if (channels[i] != null)
			{
				channels[i].Resize(capacity, trimExess);
			}
		}
	}
}
