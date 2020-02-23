using GILES.Interface;
using System;
using System.IO;
using UnityEngine;

namespace GILES
{
	public class pb_SaveSceneButton : pb_ToolbarButton
	{
		public pb_FileDialog dialogPrefab;

		private string MapsFolder;

		public override string tooltip => "Save Scene";

		public void OpenSavePanel()
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
			pb_FileDialog.AddOnSaveListener(OnSave);
			pb_ModalWindow.SetContent(pb_FileDialog.gameObject);
			pb_ModalWindow.SetTitle("Save Scene");
			pb_ModalWindow.Show();
		}

		private void OnSave(string path)
		{
			Save(path);
		}

		public void Save(string path)
		{
			if (!path.EndsWith(".json"))
			{
				path += ".json";
			}
			pb_FileUtility.SaveFile(path, pb_Scene.SaveLevel());
		}
	}
}
