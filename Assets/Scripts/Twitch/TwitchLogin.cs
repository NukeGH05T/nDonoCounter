using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class TwitchLogin : MonoBehaviour
{
    public TwitchConnect TwitchConnect;
    public Button BTNRequestToken;
    public Button BTNConnect;
    
    private string url = "https://twitchtokengenerator.com/api/create/Q2hhdHRsZQ==/chat:edit+chat:read";
    private string url2 = "https://twitchtokengenerator.com/api/status/";

    private string id = "";
    private bool success;
    private string rd = "";
    private TMP_InputField inputField;
    public void RequestToken()
    {
        StartCoroutine(GetRequest(url));
    }

    public void Connect()
    {
        if (id == null)
        {
            Console.WriteLine("Missing ID. Please make sure to request token before connecting.");
            return;
        }
        StartCoroutine(GetTokenStatus(url2 + id));
    }

    public void UpdateTwitchWindow(TMP_InputField _inputField)
    {
        inputField = _inputField;
        Invoke("SetInputFieldLink", 2);
    }

    public void OpenRequestInBrowser()
    {
        print("RD: " + rd);
        OpenBrowser(rd);
    }

    public void OpenBrowser(string _url)
    {
        if (url == null)
        {
            return;
        }
        Application.OpenURL(_url);
    }

    private void SetInputFieldLink()
    {
        inputField.text = rd;
    }

    private IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || 
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                // Deserialize JSON response
                ResponseData1 responseData1 = JsonUtility.FromJson<ResponseData1>(webRequest.downloadHandler.text);

                // Send user to TokenGeneratorSite
                id = responseData1.id;
                Debug.Log("Received Message: " + responseData1.message);
                //Application.OpenURL(responseData1.message);
                rd = responseData1.message;
            }
        }
    }
    
    private IEnumerator GetTokenStatus(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                ResponseData2 responseData = JsonUtility.FromJson<ResponseData2>(webRequest.downloadHandler.text);

                if (responseData != null && responseData.success)
                {
                    // Handle the response data here
                    Debug.Log("Token Status Response: " + webRequest.downloadHandler.text);
                    TwitchConnect.tokenUser = responseData.username;
                    TwitchConnect.AttemptTwitchConnection(responseData.token);
                }
                else
                {
                    Debug.LogError("Failed to get token status. Response: " + webRequest.downloadHandler.text);
                }
            }
        }
    }

    // Class to represent the JSON structure
    [System.Serializable]
    public class ResponseData1
    {
        public bool success;
        public string id;
        public string message;
    }
    
    [System.Serializable]
    public class ResponseData2
    {
        public bool success;
        public string id;
        public string[] scopes;
        public string token;
        public string refresh;
        public string username;
        public string user_id;
        public string client_id;
    }
}
