using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace GILES.Interface
{
	public class pb_FileDialog : MonoBehaviour
	{
		private Stack<string> back = new Stack<string>();

		private Stack<string> forward = new Stack<string>();

		public List<string> Files;

		public GameObject scrollContent;

		public Button saveButton;

		public Button cancelButton;

		public InputField directoryCrumbsField;

		public InputField fileInputField;

		public string currentDirectory;

		public Button backButton;

		public Button forwardButton;

		public Button upButton;

		public pb_SaveDialogButton rowButtonPrefab;

		public pb_GUIStyle oddRowStyle;

		public pb_GUIStyle evenRowStyle;

		private bool _isFileBrowser;

		private string _filePattern = "";

		public Callback<string> OnSave;

		public Callback OnCancel;

		private bool mInitialized;

		public bool isFileBrowser
		{
			get
			{
				return _isFileBrowser;
			}
			set
			{
				_isFileBrowser = value;
				UpdateDirectoryContents();
			}
		}

		public string filePattern
		{
			get
			{
				return _filePattern;
			}
			set
			{
				_filePattern = value;
				UpdateDirectoryContents();
			}
		}

		public void AddOnSaveListener(Callback<string> listener)
		{
			OnSave = listener;
		}

		public void AddOnCancelListener(Callback listener)
		{
			if (OnCancel != null)
			{
				OnCancel = (Callback)Delegate.Combine(OnCancel, listener);
			}
			else
			{
				OnCancel = listener;
			}
		}

		private void Awake()
		{
			if (!mInitialized)
			{
				Initialize();
			}
		}

		private void Initialize()
		{
			mInitialized = true;
			backButton.onClick.RemoveAllListeners();
			forwardButton.onClick.RemoveAllListeners();
			upButton.onClick.RemoveAllListeners();
			cancelButton.onClick.RemoveAllListeners();
			saveButton.onClick.RemoveAllListeners();
			backButton.onClick.AddListener(Back);
			forwardButton.onClick.AddListener(Forward);
			upButton.onClick.AddListener(OpenParentDirectory);
			cancelButton.onClick.AddListener(Cancel);
			saveButton.onClick.AddListener(Save);
			UpdateNavButtonInteractibility();
		}

		public void SetDirectory(string directory)
		{
			if (!mInitialized)
			{
				Initialize();
			}
			if (ValidDir(directory))
			{
				forward.Clear();
				if (ValidDir(currentDirectory))
				{
					back.Push(currentDirectory);
				}
				currentDirectory = directory;
			}
			UpdateDirectoryContents();
		}

		public void UpdateDirectoryContents()
		{
			Files.Clear();
			string[] files = Directory.GetFiles(currentDirectory);
			foreach (string text in files)
			{
				if (text.Contains(".json") || text.Contains(".txt"))
				{
					Files.Add(text);
				}
			}
			files = Directory.GetDirectories(currentDirectory);
			for (int i = 0; i < files.Length; i++)
			{
				string[] files2 = Directory.GetFiles(files[i]);
				foreach (string text2 in files2)
				{
					if (text2.Contains(".json") || text2.Contains(".txt"))
					{
						Files.Add(text2);
					}
				}
			}
			UpdateNavButtonInteractibility();
			scrollContent.SetActive(value: false);
			ClearScrollRect();
			int num = 0;
			for (int k = 0; k < Files.Count; k++)
			{
				pb_SaveDialogButton pb_SaveDialogButton = UnityEngine.Object.Instantiate(rowButtonPrefab);
				pb_SaveDialogButton.SetDelegateAndPath(SetFile, Files[k]);
				pb_GUIStyleApplier component = pb_SaveDialogButton.GetComponent<pb_GUIStyleApplier>();
				component.style = ((num++ % 2 == 0) ? evenRowStyle : oddRowStyle);
				component.ApplyStyle();
				pb_SaveDialogButton.transform.SetParent(scrollContent.transform);
			}
			scrollContent.SetActive(value: true);
		}

		private void ClearScrollRect()
		{
			foreach (Transform item in scrollContent.transform)
			{
				pb_ObjectUtility.Destroy(item.gameObject);
			}
		}

		private bool ValidDir(string dir)
		{
			if (dir != null)
			{
				return Directory.Exists(dir);
			}
			return false;
		}

		private void UpdateNavButtonInteractibility()
		{
			backButton.interactable = (back.Count > 0);
			forwardButton.interactable = (forward.Count > 0);
			upButton.interactable = (ValidDir(currentDirectory) && Directory.GetParent(currentDirectory) != null);
		}

		public void OpenParentDirectory()
		{
			DirectoryInfo parent = Directory.GetParent(currentDirectory);
			if (parent != null)
			{
				SetDirectory(parent.FullName);
			}
		}

		public void SetFile(string path)
		{
			fileInputField.text = path;
		}

		public void Back()
		{
			if (back.Count > 0)
			{
				forward.Push(currentDirectory);
				currentDirectory = back.Pop();
				UpdateDirectoryContents();
			}
		}

		public void Forward()
		{
			if (forward.Count > 0)
			{
				back.Push(currentDirectory);
				currentDirectory = forward.Pop();
				UpdateDirectoryContents();
			}
		}

		public void Cancel()
		{
			if (OnCancel != null)
			{
				OnCancel();
			}
			pb_ModalWindow.Hide();
			if (GameManager.GM.GetComponent<GTTODManager>() != null)
			{
				GameManager.GM.GetComponent<GTTODManager>().Back();
			}
		}

		public void Save()
		{
			OnSave(fileInputField.text);
			pb_ModalWindow.Hide();
		}

		public void HardSave()
		{
			OnSave(currentDirectory + "/" + GetFilePath());
			pb_ModalWindow.Hide();
		}

		private string GetFilePath()
		{
			return fileInputField.text;
		}
	}
}
