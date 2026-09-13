using System;
using System.IO;
using BestHTTP.Logger;

namespace BestHTTP.PlatformSupport.FileSystem;

public sealed class DefaultIOService : IIOService
{
	public Stream CreateFileStream(string path, FileStreamModes mode)
	{
		if (HTTPManager.Logger.Level == Loglevels.All)
		{
			HTTPManager.Logger.Verbose("DefaultIOService", $"CreateFileStream path: '{path}' mode: {mode}");
		}
		return mode switch
		{
			FileStreamModes.Create => new FileStream(path, FileMode.Create), 
			FileStreamModes.Open => new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read), 
			FileStreamModes.Append => new FileStream(path, FileMode.Append), 
			_ => throw new NotImplementedException("DefaultIOService.CreateFileStream - mode not implemented: " + mode), 
		};
	}

	public void DirectoryCreate(string path)
	{
		if (HTTPManager.Logger.Level == Loglevels.All)
		{
			HTTPManager.Logger.Verbose("DefaultIOService", $"DirectoryCreate path: '{path}'");
		}
		Directory.CreateDirectory(path);
	}

	public bool DirectoryExists(string path)
	{
		bool flag = Directory.Exists(path);
		if (HTTPManager.Logger.Level == Loglevels.All)
		{
			HTTPManager.Logger.Verbose("DefaultIOService", $"DirectoryExists path: '{path}' exists: {flag}");
		}
		return flag;
	}

	public string[] GetFiles(string path)
	{
		string[] files = Directory.GetFiles(path);
		if (HTTPManager.Logger.Level == Loglevels.All)
		{
			HTTPManager.Logger.Verbose("DefaultIOService", $"GetFiles path: '{path}' files count: {files.Length}");
		}
		return files;
	}

	public void FileDelete(string path)
	{
		if (HTTPManager.Logger.Level == Loglevels.All)
		{
			HTTPManager.Logger.Verbose("DefaultIOService", $"FileDelete path: '{path}'");
		}
		File.Delete(path);
	}

	public bool FileExists(string path)
	{
		bool flag = File.Exists(path);
		if (HTTPManager.Logger.Level == Loglevels.All)
		{
			HTTPManager.Logger.Verbose("DefaultIOService", $"FileExists path: '{path}' exists: {flag}");
		}
		return flag;
	}
}
