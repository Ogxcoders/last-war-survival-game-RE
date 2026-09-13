using System.Globalization;
using System.Threading;

namespace System.Runtime;

internal class NameGenerator
{
	private static NameGenerator nameGenerator = new NameGenerator();

	private long id;

	private string prefix;

	private NameGenerator()
	{
		prefix = "_" + Guid.NewGuid().ToString().Replace('-', '_') + "_";
	}

	public static string Next()
	{
		long num = Interlocked.Increment(ref nameGenerator.id);
		return nameGenerator.prefix + num.ToString(CultureInfo.InvariantCulture);
	}
}
