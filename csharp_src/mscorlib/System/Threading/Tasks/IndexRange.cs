using System.Runtime.InteropServices;

namespace System.Threading.Tasks;

[StructLayout(LayoutKind.Auto)]
internal struct IndexRange
{
	internal long _nFromInclusive;

	internal long _nToExclusive;

	internal volatile Box<long> _nSharedCurrentIndexOffset;

	internal int _bRangeFinished;
}
