using MiniGame.Core;

namespace MiniGame.Biubiu;

public class GameBiubiuJsonTypeBinder : GameSerializerBinder
{
	protected override void SerializeTypeAssembly(ref string assemblyName, ref string typeName)
	{
		if (assemblyName != null)
		{
			if (typeName.Contains("MiniGame.Core.Server"))
			{
				assemblyName = "MiniGame.Core.Server";
			}
			else if (typeName.Contains("MiniGame.Core"))
			{
				assemblyName = "MiniGame.Core.Share";
			}
			else if (typeName.Contains("MiniGame.Biubiu.Server"))
			{
				assemblyName = "MiniGame.biubiu.Server";
			}
			else if (typeName.Contains("MiniGame.Biubiu"))
			{
				assemblyName = "MiniGame.biubiu.Share";
			}
			else if (typeName.Contains("Box2DSharp") || typeName.Contains("Leopotam.EcsLite"))
			{
				assemblyName = "MiniGame.Core.Share";
			}
		}
		if (typeName.Contains(", Assembly-CSharp"))
		{
			if (typeName.Contains("MiniGame.Core.Server"))
			{
				typeName = typeName.Replace(", Assembly-CSharp", ", MiniGame.Core.Server");
			}
			else if (typeName.Contains("MiniGame.Core"))
			{
				typeName = typeName.Replace(", Assembly-CSharp", ", MiniGame.Core.Share");
			}
			else if (typeName.Contains("MiniGame.Biubiu.Server"))
			{
				typeName = typeName.Replace(", Assembly-CSharp", ", MiniGame.biubiu.Server");
			}
			else if (typeName.Contains("MiniGame.Biubiu"))
			{
				typeName = typeName.Replace(", Assembly-CSharp", ", MiniGame.biubiu.Share");
			}
			else if (typeName.Contains("Box2DSharp") || typeName.Contains("Leopotam.EcsLite"))
			{
				typeName = typeName.Replace(", Assembly-CSharp", ", MiniGame.Core.Share");
			}
		}
	}

	protected override void DeserializeTypeAssembly(ref string assemblyName, ref string typeName)
	{
		if (assemblyName != null)
		{
			assemblyName = assemblyName.Replace("MiniGame.biubiu.Server", "Assembly-CSharp");
			assemblyName = assemblyName.Replace("MiniGame.biubiu.Share", "Assembly-CSharp");
			assemblyName = assemblyName.Replace("MiniGame.Core.Server", "Assembly-CSharp");
			assemblyName = assemblyName.Replace("MiniGame.Core.Share", "Assembly-CSharp");
		}
		typeName = typeName.Replace("MiniGame.biubiu.Server", "Assembly-CSharp");
		typeName = typeName.Replace("MiniGame.biubiu.Share", "Assembly-CSharp");
		typeName = typeName.Replace("MiniGame.Core.Server", "Assembly-CSharp");
		typeName = typeName.Replace("MiniGame.Core.Share", "Assembly-CSharp");
	}
}
