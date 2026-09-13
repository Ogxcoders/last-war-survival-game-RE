using System.Collections.Generic;
using Sfs2XLw.Entities.Data;
using Sfs2XLw.Util;

namespace Sfs2XLw.Protocol.Serialization;

public interface ISFSDataSerializer
{
	ByteArray Object2Binary(ISFSObject obj);

	ByteArray Array2Binary(ISFSArray array);

	ISFSObject Binary2Object(ByteArray data, int offset = 0);

	ISFSArray Binary2Array(ByteArray data, int offset = 0);

	string Object2Json(Dictionary<string, object> map);

	string Array2Json(List<object> list);

	ISFSObject Json2Object(string jsonStr);

	ISFSArray Json2Array(string jsonStr);
}
