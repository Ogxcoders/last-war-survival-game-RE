using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace FM_Mono;

[Preserve]
public class Runtime
{
	public class UpdateDllContext : IDisposable
	{
		private string _assemblyPath;

		public void Initialize()
		{
			_assemblyPath = Path.Combine(persistentDataPath, "Assemblies");
			if (!Directory.Exists(_assemblyPath))
			{
				Directory.CreateDirectory(_assemblyPath);
			}
			LockDllLocalDir();
		}

		public void CopyDllTo(string srcPath)
		{
			string fileName = Path.GetFileName(srcPath);
			File.Copy(srcPath, Path.Combine(_assemblyPath, fileName), overwrite: true);
		}

		public void CopyDllTo(byte[] dllBytes, string dllFileName)
		{
			File.WriteAllBytes(Path.Combine(_assemblyPath, dllFileName), dllBytes);
		}

		public void Dispose()
		{
			UnlockDllLocalDir();
		}
	}

	[Preserve]
	public static string persistentDataPath => _fm_mono_get_persistent_data_path();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern string _fm_mono_get_persistent_data_path();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern bool IsDllLocalDirLocked();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void LockDllLocalDir();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void UnlockDllLocalDir();

	public static UpdateDllContext CreateDllUpdate()
	{
		UpdateDllContext updateDllContext = new UpdateDllContext();
		updateDllContext.Initialize();
		return updateDllContext;
	}
}
