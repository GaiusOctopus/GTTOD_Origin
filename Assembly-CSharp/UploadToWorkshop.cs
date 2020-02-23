using GILES.Example;
using LapinerTools.Steam;
using LapinerTools.Steam.Data;
using LapinerTools.Steam.UI;
using LapinerTools.uMyGUI;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UploadToWorkshop : MonoBehaviour
{
	[Header("Set-Up")]
	public List<string> Files;

	public List<Transform> CameraAngles;

	public Transform WorkshopContent;

	public GameObject ButtonObject;

	public Text FileDisplay;

	public Transform Camera;

	public GameObject UploadButton;

	public List<GameObject> DisableObjects;

	[Header("Private Variables")]
	private string MapsFolder;

	private string MapFileName;

	private string MapPath;

	private int Angle;

	private bool hasSpawned;

	private Transform LastButton;

	public void Start()
	{
		if (Application.isEditor)
		{
			MapsFolder = "D:\\Steam\\steamapps\\workshop\\content\\541200";
		}
		else
		{
			MapsFolder = Path.Combine(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..\\..\\")), "workshop\\content\\541200");
		}
		Directory.CreateDirectory(MapsFolder);
		Directory.CreateDirectory(MapsFolder);
		new DirectoryInfo(MapsFolder);
		string[] files = Directory.GetFiles(MapsFolder);
		foreach (string item in files)
		{
			Files.Add(item);
		}
		files = Directory.GetDirectories(MapsFolder);
		for (int i = 0; i < files.Length; i++)
		{
			string[] files2 = Directory.GetFiles(files[i]);
			foreach (string item2 in files2)
			{
				Files.Add(item2);
			}
		}
		foreach (string file in Files)
		{
			AddButton(Path.GetFileName(file.ToString()).ToString(), file);
		}
	}

	public void AddButton(string FileName, string Path)
	{
		LastButton = UnityEngine.Object.Instantiate(ButtonObject).transform;
		LastButton.parent = WorkshopContent;
		LastButton.localScale = WorkshopContent.localScale;
		LastButton.position = WorkshopContent.position;
		LastButton.rotation = WorkshopContent.rotation;
		LastButton.GetChild(0).GetComponent<Text>().text = FileName;
		LastButton.GetComponent<Button>().onClick.AddListener(delegate
		{
			SelectFile(FileName, Path);
		});
	}

	public void SelectFile(string FileName, string Path)
	{
		MapFileName = FileName;
		FileDisplay.text = FileName;
		MapPath = Path;
	}

	public void LoadScene(int sceneIndex)
	{
		SceneManager.LoadScene(sceneIndex);
	}

	public void SwitchCameraAngles()
	{
		if (Angle < 4)
		{
			Angle++;
			Camera.position = CameraAngles[Angle].position;
			Camera.rotation = CameraAngles[Angle].rotation;
		}
		else
		{
			Angle = 0;
			Camera.position = CameraAngles[Angle].position;
			Camera.rotation = CameraAngles[Angle].rotation;
		}
	}

	public void LoadInLevel()
	{
		if (!hasSpawned)
		{
			pb_SceneLoader.LoadScene(MapPath);
			UploadButton.SetActive(value: true);
			hasSpawned = true;
			foreach (GameObject disableObject in DisableObjects)
			{
				disableObject.SetActive(value: false);
			}
		}
	}

	public void OpenUploadScreen()
	{
		SteamMainBase<SteamWorkshopMain>.Instance.IsDebugLogEnabled = true;
		string text = Path.Combine(Application.persistentDataPath, "DummyItemContentFolder" + DateTime.Now.Ticks);
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		File.WriteAllText(Path.Combine(text, MapFileName), File.ReadAllText(MapPath));
		WorkshopItemUpdate workshopItemUpdate = new WorkshopItemUpdate();
		workshopItemUpdate.ContentPath = text;
		((SteamWorkshopPopupUpload)uMyGUI_PopupManager.Instance.ShowPopup("steam_ugc_upload")).UploadUI.SetItemData(workshopItemUpdate);
	}
}
