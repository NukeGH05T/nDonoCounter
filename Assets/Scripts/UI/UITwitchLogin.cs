using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITwitchLogin : MonoBehaviour
{
	public void DisableObject(GameObject gameObject) {
		gameObject.SetActive(false);
	}
	
	public void EnableObject(GameObject gameObject) {
		gameObject.SetActive(true);
	}
	
	public void EnableTwitchWindow(GameObject mainPanel, TMP_InputField inputField) {
		mainPanel.SetActive(true);
		inputField.text = "data";
	}

	public void CopyInputFieldText(TMP_InputField inputField)
	{
		GUIUtility.systemCopyBuffer = inputField.text;
	}
}
