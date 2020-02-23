using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SteamWorkshop : MonoBehaviour
{
	public static SteamWorkshop singleton;

	public bool fetchedContent;

	private string itemContent;

	private string lastFileName;

	private string thisUploadsIamgeLoc = "";

	public List<PublishedFileId_t> subscribedItemList;

	public Text FileResult;

	public Text UploadResult;

	public Text ThumbnailResult;

	private CallResult<SubmitItemUpdateResult_t> ItemUpdateResult;

	private CallResult<RemoteStoragePublishFileResult_t> RemoteStoragePublishFileResult;

	private CallResult<RemoteStorageEnumerateUserSubscribedFilesResult_t> RemoteStorageEnumerateUserSubscribedFilesResult;

	private CallResult<RemoteStorageGetPublishedFileDetailsResult_t> RemoteStorageGetPublishedFileDetailsResult;

	private CallResult<RemoteStorageDownloadUGCResult_t> RemoteStorageDownloadUGCResult;

	private CallResult<RemoteStorageUnsubscribePublishedFileResult_t> RemoteStorageUnsubscribePublishedFileResult;

	private PublishedFileId_t publishedFileID;

	private UGCHandle_t UGCHandle;

	private void Awake()
	{
		singleton = this;
		subscribedItemList = new List<PublishedFileId_t>();
	}

	private void OnEnable()
	{
		ItemUpdateResult = CallResult<SubmitItemUpdateResult_t>.Create(OnItemUpdateResult);
		RemoteStoragePublishFileResult = CallResult<RemoteStoragePublishFileResult_t>.Create(OnRemoteStoragePublishFileResult);
		RemoteStorageEnumerateUserSubscribedFilesResult = CallResult<RemoteStorageEnumerateUserSubscribedFilesResult_t>.Create(OnRemoteStorageEnumerateUserSubscribedFilesResult);
		RemoteStorageGetPublishedFileDetailsResult = CallResult<RemoteStorageGetPublishedFileDetailsResult_t>.Create(OnRemoteStorageGetPublishedFileDetailsResult);
		RemoteStorageDownloadUGCResult = CallResult<RemoteStorageDownloadUGCResult_t>.Create(OnRemoteStorageDownloadUGCResult);
		RemoteStorageUnsubscribePublishedFileResult = CallResult<RemoteStorageUnsubscribePublishedFileResult_t>.Create(OnRemoteStorageUnsubscribePublishedFileResult);
	}

	private void OnItemUpdateResult(SubmitItemUpdateResult_t pCallback, bool bIOFailure)
	{
		if (pCallback.m_eResult == EResult.k_EResultOK)
		{
			Debug.Log("The item is now uploaded with a thumbnail");
		}
		else
		{
			Debug.Log("The item is now uploaded with out a thumbnail");
		}
	}

	private IEnumerator downloadFiles()
	{
		for (int dlItem = 0; dlItem < subscribedItemList.Count; dlItem++)
		{
			fetchedContent = false;
			GetItemContent(dlItem);
			while (!fetchedContent)
			{
				yield return new WaitForEndOfFrame();
			}
		}
	}

	public void GetSubscribedItems()
	{
		SteamAPICall_t hAPICall = SteamRemoteStorage.EnumerateUserSubscribedFiles(0u);
		RemoteStorageEnumerateUserSubscribedFilesResult.Set(hAPICall);
	}

	public void GetItemContent(int ItemID)
	{
		publishedFileID = subscribedItemList[ItemID];
		SteamAPICall_t publishedFileDetails = SteamRemoteStorage.GetPublishedFileDetails(publishedFileID, 0u);
		RemoteStorageGetPublishedFileDetailsResult.Set(publishedFileDetails);
	}

	public void DeleteFile(string filename)
	{
		SteamRemoteStorage.FileDelete(filename);
	}

	public void SaveToWorkshop(string fileName, string fileData, string workshopTitle, string workshopDescription, string[] tags, string imageLoc)
	{
		lastFileName = fileName;
		if (SteamRemoteStorage.FileExists(fileName))
		{
			Debug.Log("Item with that filename already exists");
			FileResult.text = "Item with that filename already exists";
		}
		else if (!UploadFile(fileName, fileData))
		{
			Debug.Log("Upload cannot be completed");
			FileResult.text = "Upload cannot be completed";
		}
		else
		{
			FileResult.text = "File found, passing to uploader";
			thisUploadsIamgeLoc = imageLoc;
			UploadToWorkshop(fileName, workshopTitle, workshopDescription, tags);
		}
	}

	private bool UploadFile(string fileName, string fileData)
	{
		byte[] array = new byte[Encoding.UTF8.GetByteCount(fileData)];
		Encoding.UTF8.GetBytes(fileData, 0, fileData.Length, array, 0);
		SteamRemoteStorage.FileWrite(fileName, array, array.Length);
		return true;
	}

	private void UploadToWorkshop(string fileName, string workshopTitle, string workshopDescription, string[] tags)
	{
		SteamAPICall_t hAPICall = SteamRemoteStorage.PublishWorkshopFile(fileName, null, SteamUtils.GetAppID(), workshopTitle, workshopDescription, ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic, tags, EWorkshopFileType.k_EWorkshopFileTypeFirst);
		RemoteStoragePublishFileResult.Set(hAPICall);
	}

	public void Unsubscribe(PublishedFileId_t file)
	{
		SteamAPICall_t hAPICall = SteamRemoteStorage.UnsubscribePublishedFile(file);
		RemoteStorageUnsubscribePublishedFileResult.Set(hAPICall);
	}

	private IEnumerator startPictureUpdate(RemoteStoragePublishFileResult_t pCallback)
	{
		yield return new WaitForSeconds(1f);
		UGCUpdateHandle_t handle = SteamUGC.StartItemUpdate(SteamUtils.GetAppID(), pCallback.m_nPublishedFileId);
		if (SteamUGC.SetItemPreview(handle, thisUploadsIamgeLoc))
		{
			Debug.Log("Thumbnail upload intialization success");
			ThumbnailResult.text = "Thumbnail upload intialization success";
			SteamAPICall_t hAPICall = SteamUGC.SubmitItemUpdate(handle, "Add Screenshot");
			ItemUpdateResult.Set(hAPICall);
		}
		else
		{
			Debug.Log("Thumbnail upload intialization failed, but file upload succeded");
			ThumbnailResult.text = "Thumbnail upload intialization failed, but file upload succeded";
		}
	}

	private void OnRemoteStorageUnsubscribePublishedFileResult(RemoteStorageUnsubscribePublishedFileResult_t pCallback, bool bIOFailure)
	{
	}

	private void OnRemoteStoragePublishFileResult(RemoteStoragePublishFileResult_t pCallback, bool bIOFailure)
	{
		if (pCallback.m_eResult == EResult.k_EResultOK)
		{
			Debug.Log("File upload success, starting thumbnail upload");
			UploadResult.text = "File upload success, starting thumbnail upload";
			StartCoroutine(startPictureUpdate(pCallback));
			publishedFileID = pCallback.m_nPublishedFileId;
			DeleteFile(lastFileName);
		}
		else
		{
			Debug.Log("File upload failed");
			UploadResult.text = "File upload failed";
		}
	}

	private void OnRemoteStorageEnumerateUserSubscribedFilesResult(RemoteStorageEnumerateUserSubscribedFilesResult_t pCallback, bool bIOFailure)
	{
		subscribedItemList = new List<PublishedFileId_t>();
		for (int i = 0; i < pCallback.m_nTotalResultCount; i++)
		{
			PublishedFileId_t item = pCallback.m_rgPublishedFileId[i];
			subscribedItemList.Add(item);
		}
		StartCoroutine(downloadFiles());
	}

	private void OnRemoteStorageGetPublishedFileDetailsResult(RemoteStorageGetPublishedFileDetailsResult_t pCallback, bool bIOFailure)
	{
		if (pCallback.m_eResult == EResult.k_EResultOK)
		{
			bool flag = false;
			if (File.Exists(pCallback.m_pchFileName))
			{
				Debug.Log("File exists so now we check if it's outdated");
				uint num = (uint)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
				if (pCallback.m_rtimeUpdated > num)
				{
					flag = true;
				}
			}
			else
			{
				Debug.Log("File doesn't exist we need to download it");
				flag = true;
			}
			if (flag)
			{
				UGCHandle = pCallback.m_hFile;
				SteamAPICall_t hAPICall = SteamRemoteStorage.UGCDownload(UGCHandle, 0u);
				RemoteStorageDownloadUGCResult.Set(hAPICall);
			}
			else
			{
				fetchedContent = true;
				Debug.Log("File is up to date and we can now continue to the next one");
			}
		}
		else
		{
			fetchedContent = true;
		}
	}

	private void OnRemoteStorageDownloadUGCResult(RemoteStorageDownloadUGCResult_t pCallback, bool bIOFailure)
	{
		byte[] array = new byte[pCallback.m_nSizeInBytes];
		int count = SteamRemoteStorage.UGCRead(UGCHandle, array, pCallback.m_nSizeInBytes, 0u, EUGCReadAction.k_EUGCRead_Close);
		itemContent = Encoding.UTF8.GetString(array, 0, count);
		File.WriteAllText(pCallback.m_pchFileName, itemContent);
		fetchedContent = true;
	}
}
