using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace GILES.Interface
{
	public class pb_SaveDialogButton : Button
	{
		public string path;

		public Callback<string> OnClick;

		public void SetDelegateAndPath(Callback<string> del, string path)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			if (directoryInfo == null)
			{
				Debug.Log("Invalid Directory: " + path);
				return;
			}
			this.path = path;
			OnClick = del;
			base.onClick.AddListener(delegate
			{
				OnClick(path);
			});
			GetComponentInChildren<Text>().text = directoryInfo.Name;
		}
	}
}
