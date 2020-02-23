using GILES.Interface;
using System;
using System.IO;
using UnityEngine;

namespace GILES.Example
{
	public class LoadAndPlayScene : MonoBehaviour
	{
		public pb_FileDialog dialogPrefab;

		private string MapsFolder;

		public void OpenLoadPanel()
		{
			pb_FileDialog pb_FileDialog = UnityEngine.Object.Instantiate(dialogPrefab);
			if (Application.isEditor)
			{
				MapsFolder = "D:\\Steam\\steamapps\\workshop\\content\\541200";
			}
			else
			{
				MapsFolder = Path.Combine(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..\\..\\")), "workshop\\content\\541200");
			}
			Directory.CreateDirectory(MapsFolder);
			pb_FileDialog.SetDirectory(MapsFolder);
			pb_FileDialog.isFileBrowser = true;
			pb_FileDialog.filePattern = "*.json";
			pb_FileDialog.AddOnSaveListener(OnOpen);
			pb_ModalWindow.SetContent(pb_FileDialog.gameObject);
			pb_ModalWindow.SetTitle("Open Scene");
			pb_ModalWindow.Show();
		}

		private void OnOpen(string path)
		{
			pb_SceneLoader.LoadScene(path);
		}
	}
}
