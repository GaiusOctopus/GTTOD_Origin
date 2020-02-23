using UnityEngine;
using UnityEngine.UI;

public class WorkshopButton : MonoBehaviour
{
	private string MyFileName;

	private void Start()
	{
		GetComponent<Button>().onClick.AddListener(SelectButton);
		MyFileName = base.transform.GetChild(0).GetComponent<Text>().text;
	}

	private void SelectButton()
	{
	}
}
