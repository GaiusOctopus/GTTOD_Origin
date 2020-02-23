using System;
using System.IO;
using UnityEngine;

namespace GILES
{
	public static class pb_FileUtility
	{
		public static string ReadFile(string path)
		{
			if (!File.Exists(path))
			{
				Debug.LogError("File path does not exist!\n" + path);
				return "";
			}
			return File.ReadAllText(path);
		}

		public static bool SaveFile(string path, string contents)
		{
			try
			{
				new FileInfo(path);
				File.WriteAllText(path, contents);
			}
			catch (Exception ex)
			{
				Debug.LogError("Failed writing to path: " + path + "\n" + ex.ToString());
				return false;
			}
			return true;
		}

		public static bool IsValidPath(string path, string extension)
		{
			if (!string.IsNullOrEmpty(path) && Uri.IsWellFormedUriString(path, UriKind.RelativeOrAbsolute))
			{
				return path.EndsWith(extension);
			}
			return false;
		}

		public static string GetFullPath(string path)
		{
			return Path.GetFullPath(path);
		}

		public static PathType GetPathType(string path)
		{
			if (!File.Exists(path))
			{
				if (!Directory.Exists(path))
				{
					return PathType.Null;
				}
				return PathType.Directory;
			}
			return PathType.File;
		}

		public static string SanitizePath(string path, string extension = null)
		{
			string text = GetFullPath(path);
			if (extension != null && !text.EndsWith(extension))
			{
				if (!extension.StartsWith("."))
				{
					extension = "." + extension;
				}
				text += extension;
			}
			return text;
		}
	}
}
