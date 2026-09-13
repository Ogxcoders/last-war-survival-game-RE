using System.Linq;
using System.Text;
using BestHTTP.JSON;

namespace BestHTTP.Forms;

public sealed class RawJsonForm : HTTPFormBase
{
	private byte[] CachedData;

	public override void PrepareRequest(HTTPRequest request)
	{
		request.SetHeader("Content-Type", "application/json");
	}

	public override byte[] GetData()
	{
		if (CachedData != null && !base.IsChanged)
		{
			return CachedData;
		}
		string s = Json.Encode(base.Fields.ToDictionary((HTTPFieldData x) => x.Name, (HTTPFieldData x) => x.Text));
		base.IsChanged = false;
		return CachedData = Encoding.UTF8.GetBytes(s);
	}
}
