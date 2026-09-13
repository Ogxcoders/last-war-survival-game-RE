using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Google.Protobuf.Reflection;

public sealed class DescriptorDeclaration
{
	public IDescriptor Descriptor { get; }

	public int StartLine { get; }

	public int StartColumn { get; }

	public int EndLine { get; }

	public int EndColumn { get; }

	public string LeadingComments { get; }

	public string TrailingComments { get; }

	public IReadOnlyList<string> LeadingDetachedComments { get; }

	private DescriptorDeclaration(IDescriptor descriptor, SourceCodeInfo.Types.Location location)
	{
		Descriptor = descriptor;
		bool flag = location.Span.Count == 4;
		StartLine = location.Span[0] + 1;
		StartColumn = location.Span[1] + 1;
		EndLine = (flag ? (location.Span[2] + 1) : StartLine);
		EndColumn = location.Span[flag ? 3 : 2] + 1;
		LeadingComments = location.LeadingComments;
		TrailingComments = location.TrailingComments;
		LeadingDetachedComments = new ReadOnlyCollection<string>(location.LeadingDetachedComments.ToList());
	}

	internal static DescriptorDeclaration FromProto(IDescriptor descriptor, SourceCodeInfo.Types.Location location)
	{
		return new DescriptorDeclaration(descriptor, location);
	}
}
